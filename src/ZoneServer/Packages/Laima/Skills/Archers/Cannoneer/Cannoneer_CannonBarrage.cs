using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Abilities.Handlers.Archers.Cannoneer;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Cannoneer
{
	/// <summary>

	/// Cannon Barrage

	/// Aplica seis hits no alvo principal e nos inimigos próximos.

	/// Bazooka aumenta alcance, quantidade de alvos, dano e cooldown.

	/// </summary>

	[Package("laima")]
	[SkillHandler(SkillId.Cannoneer_CannonBarrage)]
	public class Cannoneer_CannonBarrageOverride : IForceSkillHandler
	{
		private const int HitCount = 6;
		private const int BaseMaximumTargets = 8;
		private const float NormalMaximumRange = 160f;
		private static readonly TimeSpan FirstHitDelay = TimeSpan.Zero;
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(100);
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity designatedTarget)
		{
			if (caster is not Character character || character.IsDead)
				return;

			if (designatedTarget == null || designatedTarget.IsDead)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!CannoneerBazookaHelper.IsInsideAllowedDistance(character, originPos, designatedTarget.Position, NormalMaximumRange))
			{
				character.ServerMessage(Localization.Get("The target is outside the valid attack distance."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			CannoneerBazookaHelper.ApplyCooldownMultiplier(character, skill);

			caster.TurnTowards(designatedTarget);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, designatedTarget.Position, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, designatedTarget.Handle, caster.Position, designatedTarget.Direction, Position.Zero);
			Send.ZC_SKILL_FORCE_TARGET(caster, designatedTarget, skill);

			var maximumTargets = BaseMaximumTargets + CannoneerBazookaHelper.GetAdditionalAoeAttackRatio(character);
			var splashRange = Math.Max(1f, skill.Properties.GetFloat(PropertyName.SplRange));
			var splashArea = new Circle(designatedTarget.Position, splashRange);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea)
				.Where(target => target != null && !target.IsDead)
				.Distinct()
				.OrderBy(target => target == designatedTarget ? 0 : 1)
				.ThenBy(target => target.Position.Get2DDistance(designatedTarget.Position))
				.Take(maximumTargets)
				.ToList();

			if (!targets.Contains(designatedTarget))
			{
				if (targets.Count >= maximumTargets)
					targets.RemoveAt(targets.Count - 1);

				targets.Insert(0, designatedTarget);
			}

			var enhanceMultiplier = Cannoneer_CannonBarrageEnhanceAbility.GetDamageMultiplier(character);
			var bazookaMultiplier = CannoneerBazookaHelper.GetFinalDamageMultiplier(character);
			var damageMultiplier = enhanceMultiplier * bazookaMultiplier;

			foreach (var target in targets)
				Cannoneer_CannonBarrageStunAbility.TryApplyStun(character, target);

			skill.Run(this.Attack(skill, character, targets, damageMultiplier));
		}

		private async Task Attack(Skill skill, Character character, IList<ICombatEntity> targets, float damageMultiplier)
		{
			try
			{
				await skill.Wait(FirstHitDelay);

				for (var hitIndex = 0; hitIndex < HitCount; hitIndex++)
				{
					if (character.IsDead)
						break;

					var hits = new List<SkillHitInfo>();

					foreach (var target in targets)
					{
						if (target == null || target.IsDead)
							continue;

						var skillHitResult = SCR_SkillHit(character, target, skill);
						skillHitResult.Damage = Math.Max(1, (int)(skillHitResult.Damage * damageMultiplier));
						target.TakeDamage(skillHitResult.Damage, character);

						var skillHit = new SkillHitInfo(character, target, skill, skillHitResult, HitAnimationTime, TimeSpan.Zero);
						skillHit.ForceId = ForceId.GetNew();
						hits.Add(skillHit);
					}

					if (hits.Count > 0)
						Send.ZC_SKILL_HIT_INFO(character, hits);

					if (hitIndex + 1 < HitCount)
						await skill.Wait(DelayBetweenHits);
				}
			}
			finally
			{
				character.SetAttackState(false);
				Send.ZC_NORMAL.Skill_45(character);
				Send.ZC_NORMAL.SkillCancel(character, skill.Id);
				Send.ZC_NORMAL.SkillCancelCancel(character, skill.Id);
			}
		}
	}
}
