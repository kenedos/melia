using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Appraiser
{
	/// <summary>
	/// Huge Magnifier.
	/// Damages enemies around the selected target. If the selected
	/// target has Devaluation, spreads its effects to nearby enemies.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Appraiser_LargeMagnifyingGlass)]
	public class Appraiser_LargeMagnifyingGlass : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int MaximumTargets = 10;
		private const int MaximumEnhanceLevel = 100;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			var selectedTargets = new List<ICombatEntity>();

			if (target != null)
				selectedTargets.Add(target);

			this.Handle(skill, caster, originPos, farPos, selectedTargets);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> selectedTargets)
		{
			if (caster is not Character character || caster.IsDead)
				return;

			var primaryTarget = selectedTargets?
				.Where(target => target != null && !target.IsDead)
				.OrderBy(target => farPos.Get2DDistance(target.Position))
				.FirstOrDefault();

			if (primaryTarget == null)
			{
				this.CancelSkill(skill, caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			var targetPosition = primaryTarget.Position;

			caster.TurnTowards(targetPosition);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(
				caster,
				skill,
				1,
				originPos,
				targetPosition
			);

			Send.ZC_NORMAL.UpdateSkillEffect(
				caster,
				caster.Handle,
				originPos,
				caster.Direction,
				targetPosition
			);

			Send.ZC_SKILL_MELEE_GROUND(
				caster,
				skill,
				targetPosition
			);

			skill.IncreaseOverheat();

			var areaTargets = caster.Map
				.GetAttackableEnemiesInPosition(
					caster,
					targetPosition,
					skill.Data.SplashRange
				)
				.Where(target => target != null && !target.IsDead)
				.OrderBy(target =>
					targetPosition.Get2DDistance(target.Position))
				.Take(MaximumTargets)
				.ToList();

			if (!areaTargets.Contains(primaryTarget))
			{
				if (areaTargets.Count >= MaximumTargets)
					areaTargets.RemoveAt(areaTargets.Count - 1);

				areaTargets.Insert(0, primaryTarget);
			}

			var hasDevaluation = primaryTarget.TryGetBuff(
				BuffId.Devaluation_Debuff,
				out var sourceDevaluation
			);

			var devaluationDuration = TimeSpan.Zero;

			if (hasDevaluation)
				devaluationDuration = sourceDevaluation.RemainingDuration;

			var enhanceMultiplier = this.GetEnhanceMultiplier(character);
			var hits = new List<SkillHitInfo>();

			foreach (var enemy in areaTargets)
			{
				if (enemy == null || enemy.IsDead)
					continue;

				var modifier = SkillModifier.Default;
				modifier.DamageMultiplier *= enhanceMultiplier;

				var skillHitResult = SCR_SkillHit(
					caster,
					enemy,
					skill,
					modifier
				);

				if (skillHitResult.Result != HitResultType.Dodge)
				{
					enemy.TakeDamage(
						skillHitResult.Damage,
						caster
					);

					var skillHit = new SkillHitInfo(
						caster,
						enemy,
						skill,
						skillHitResult,
						skill.Data.DefaultHitDelay,
						skill.Data.DefaultHitDelay
					);

					hits.Add(skillHit);
				}

				if (!hasDevaluation ||
					devaluationDuration <= TimeSpan.Zero)
					continue;

				if (enemy != primaryTarget)
					this.SpreadDevaluation(
						enemy,
						character,
						sourceDevaluation,
						devaluationDuration
					);

				this.ApplyDamageReceivedDebuff(
					enemy,
					character,
					sourceDevaluation,
					devaluationDuration
				);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);

			caster.SetAttackState(false);
		}

		private void CancelSkill(Skill skill, ICombatEntity caster)
		{
			caster.SetAttackState(false);
			Send.ZC_SKILL_CAST_CANCEL(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
		}

		private void SpreadDevaluation(
			ICombatEntity target,
			Character caster,
			Melia.Zone.Buffs.Buff sourceDevaluation,
			TimeSpan duration
		)
		{
			target.StartBuff(
				BuffId.Devaluation_Debuff,
				sourceDevaluation.NumArg1,
				sourceDevaluation.NumArg2,
				duration,
				caster,
				SkillId.Appraiser_Devaluation,
				clonedDebuff =>
				{
					clonedDebuff.NumArg3 =
						sourceDevaluation.NumArg3;

					clonedDebuff.NumArg4 =
						sourceDevaluation.NumArg4;

					clonedDebuff.NumArg5 =
						sourceDevaluation.NumArg5;

					foreach (var variable in
						sourceDevaluation.Vars.GetList())
					{
						if (variable.Key.StartsWith(
							"Melia.Modifier."
						))
						{
							continue;
						}

						clonedDebuff.Vars.Set(
							variable.Key,
							variable.Value
						);
					}
				}
			);
		}

		private void ApplyDamageReceivedDebuff(
			ICombatEntity target,
			Character caster,
			Melia.Zone.Buffs.Buff sourceDevaluation,
			TimeSpan duration
		)
		{
			target.StartBuff(
				BuffId.Devaluation_Damage_Debuff,
				10f,
				sourceDevaluation.NumArg2,
				duration,
				caster,
				SkillId.Appraiser_LargeMagnifyingGlass
			);
		}

		private float GetEnhanceMultiplier(Character character)
		{
			var abilityLevel = Math.Min(
				character.Abilities.GetLevel(
					AbilityId.Appraiser10
				),
				MaximumEnhanceLevel
			);

			if (abilityLevel <= 0)
				return 1f;

			var enhancePercent = abilityLevel * 0.5f;

			if (abilityLevel >= MaximumEnhanceLevel)
				enhancePercent += 10f;

			return 1f + enhancePercent / 100f;
		}
	}
}
