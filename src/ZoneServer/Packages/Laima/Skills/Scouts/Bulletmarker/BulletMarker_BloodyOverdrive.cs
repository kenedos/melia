using System;
using System.Collections.Generic;
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
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for Bullet Marker skill Bloody Overdrive.
	/// SkillId: 51107
	/// Factor: 123 + 28 per level
	/// Multi Hit: 30
	/// Hits all enemies inside a circular area centered on the caster.
	/// Requires Double Gun Stance.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Bulletmarker_BloodyOverdrive)]
	public class BulletMarker_BloodyOverdrive : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int HitCount = 20;
		private const int AreaRadius = 90;
		private const int MaxTargets = 20;
		private const int RicochetAdditionalTargets = 1;
		private const float RicochetChancePerLevel = 0.05f;
		private const int RicochetMaxLevel = 10;
		private static readonly TimeSpan FirstHitDelay = TimeSpan.FromMilliseconds(10);
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(100);
		private static readonly TimeSpan HitAnimationTime = TimeSpan.FromMilliseconds(10);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos)
		{
			if (caster is not Character character)
				return;

			if (!caster.TryGetBuff(BuffId.DoubleGunStance_Buff, out _))
			{
				character.ServerMessage(Localization.Get("Double Gun Stance must be active."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var consumedOutrageStack = BulletMarkerOverheatingHelper.TryConsumeOutrageStack(character);
			BulletMarkerOverheatingHelper.AddSkillStacks(character, skill);

			this.ApplyInvincibilityAbility(character, skill);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, originPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos);

			skill.Run(this.Attack(skill, caster, consumedOutrageStack));
		}

		private async Task Attack(Skill skill, ICombatEntity caster, bool consumedOutrageStack)
		{
			await skill.Wait(FirstHitDelay);

			for (var hitIndex = 0; hitIndex < HitCount; hitIndex++)
			{
				var area = new Circle(caster.Position, AreaRadius);

				var maxTargets = this.GetMaxTargets(caster);

				var targets = caster.Map
					.GetAttackableEnemiesIn(caster, area)
					.Where(target => target != null && !target.IsDead)
					.Take(maxTargets)
					.ToList();

				foreach (var target in targets)
				{
					var skillHitResult = SCR_SkillHit(caster, target, skill);

					this.ApplyEnhanceAbility(caster, skillHitResult);

					if (consumedOutrageStack)
						skillHitResult.Damage *= 1.25f;

					target.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, HitAnimationTime, TimeSpan.Zero);

					Send.ZC_SKILL_HIT_INFO(caster, skillHit);
				}

				if (hitIndex < HitCount - 1)
					await skill.Wait(DelayBetweenHits);
			}

			caster.SetAttackState(false);
		}

		private void ApplyEnhanceAbility(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			if (caster is not Character character)
				return;

			if (!character.Abilities.TryGet(AbilityId.Bulletmarker4, out var ability) || !ability.Active)
				return;

			var enhanceRate = ability.Level * 0.005f;

			if (ability.Level >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}

		private int GetMaxTargets(ICombatEntity caster)
		{
			if (caster is not Character character)
				return MaxTargets;

			if (!character.Abilities.TryGet(AbilityId.Bulletmarker8, out var ability) || !ability.Active)
				return MaxTargets;

			var abilityLevel = Math.Min(ability.Level, RicochetMaxLevel);

			if (abilityLevel <= 0)
				return MaxTargets;

			var ricochetChance = abilityLevel * RicochetChancePerLevel;

			if (Random.Shared.NextDouble() >= ricochetChance)
				return MaxTargets;

			return MaxTargets + RicochetAdditionalTargets;
		}

		private void ApplyInvincibilityAbility(Character character, Skill skill)
		{
			if (!character.Abilities.TryGet(AbilityId.Bulletmarker12, out var ability) || !ability.Active)
				return;

			var duration = FirstHitDelay + TimeSpan.FromMilliseconds(DelayBetweenHits.TotalMilliseconds * (HitCount - 1)) + HitAnimationTime;

			character.StartBuff(BuffId.Skill_NoDamage_Buff, skill.Level, 0f, duration, character, skill.Id);
		}
	}
}
