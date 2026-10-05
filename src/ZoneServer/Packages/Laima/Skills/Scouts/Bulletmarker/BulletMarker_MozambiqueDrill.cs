using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Zone.Skills.SplashAreas;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for Bullet Marker skill Mozambique Drill.
	/// SkillId: 51110
	/// Factor: 89 + 21 per level
	/// Multi Hit: 4
	/// Overheat: 3
	/// Requires Double Gun Stance.
	/// Mozambique Drill: Ricochet allows the skill to hit 1 additional target.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Bulletmarker_MozambiqueDrill)]
	public class BulletMarker_MozambiqueDrill : IForceSkillHandler
	{
		private const int HitCount = 4;
		private const int BaseMaxTargets = 1;
		private const int RicochetAdditionalTargets = 1;
		private const int RicochetSearchRadius = 120;
		private static readonly TimeSpan FirstHitDelay = TimeSpan.FromMilliseconds(10);
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(100);
		private static readonly TimeSpan HitAnimationTime = TimeSpan.FromMilliseconds(100);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (!caster.TryGetBuff(BuffId.DoubleGunStance_Buff, out _))
			{
				character.ServerMessage(Localization.Get("Double Gun Stance must be active."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (target == null || target.IsDead)
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.InSkillUseRange(skill, target))
			{
				character.ServerMessage(Localization.Get("Too far away."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var consumedOutrageStack = BulletMarkerOverheatingHelper.TryConsumeOutrageStack(character);
			BulletMarkerOverheatingHelper.AddSkillStacks(character, skill);

			skill.Run(this.Attack(skill, caster, target, consumedOutrageStack));
		}

		private async Task Attack(Skill skill, ICombatEntity caster, ICombatEntity primaryTarget, bool consumedOutrageStack)
		{
			var forceId = ForceId.GetNew();

			Send.ZC_SKILL_FORCE_TARGET(caster, primaryTarget, skill, forceId, null);

			await skill.Wait(FirstHitDelay);

			var hitCount = consumedOutrageStack ? HitCount * 2 : HitCount;

			for (var i = 0; i < hitCount; i++)
			{
				if (primaryTarget == null || primaryTarget.IsDead)
					break;

				var targets = this.GetTargets(caster, primaryTarget);

				foreach (var target in targets)
				{
					if (target == null || target.IsDead)
						continue;

					var modifier = this.CreateSkillModifier(caster);
					var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

					this.ApplyEnhanceAbility(caster, skillHitResult);

					if (consumedOutrageStack)
						skillHitResult.Damage *= 0.85f;

					target.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, HitAnimationTime, TimeSpan.Zero);
					skillHit.ForceId = forceId;

					Send.ZC_SKILL_HIT_INFO(caster, skillHit);
				}

				if (i < hitCount - 1)
					await skill.Wait(DelayBetweenHits);
			}

			caster.SetAttackState(false);
		}

		private ICombatEntity[] GetTargets(ICombatEntity caster, ICombatEntity primaryTarget)
		{
			var maxTargets = this.GetMaxTargets(caster);

			if (maxTargets <= BaseMaxTargets)
				return new[] { primaryTarget };

			var area = new Circle(primaryTarget.Position, RicochetSearchRadius);

			var additionalTargets = caster.Map
				.GetAttackableEnemiesIn(caster, area)
				.Where(target => target != null && !target.IsDead && target != primaryTarget)
				.Take(RicochetAdditionalTargets)
				.ToList();

			var targets = new ICombatEntity[1 + additionalTargets.Count];
			targets[0] = primaryTarget;

			for (var i = 0; i < additionalTargets.Count; i++)
				targets[i + 1] = additionalTargets[i];

			return targets;
		}

		private void ApplyEnhanceAbility(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			if (caster is not Character character)
				return;

			if (!character.Abilities.TryGet(AbilityId.Bulletmarker5, out var ability) || !ability.Active)
				return;

			var enhanceRate = ability.Level * 0.005f;

			if (ability.Level >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}

		private int GetMaxTargets(ICombatEntity caster)
		{
			if (caster is not Character character)
				return BaseMaxTargets;

			if (!character.Abilities.TryGet(AbilityId.Bulletmarker10, out var ability) || !ability.Active)
				return BaseMaxTargets;

			return BaseMaxTargets + RicochetAdditionalTargets;
		}

		private SkillModifier CreateSkillModifier(ICombatEntity caster)
		{
			var modifier = new SkillModifier();

			if (caster is not Character character)
				return modifier;

			if (!character.Abilities.TryGet(AbilityId.Bulletmarker9, out var ability) || !ability.Active)
				return modifier;

			var abilityLevel = Math.Clamp(ability.Level, 1, 5);
			modifier.DefensePenetrationRate = abilityLevel * 0.02f;

			return modifier;
		}
	}
}
