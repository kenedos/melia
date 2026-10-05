using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Archers.Appraiser
{
	/// <summary>
	/// Applies Devaluation to the selected target and nearby enemies.
	/// Reduces physical and magic defense by 10% at level 1,
	/// increasing to 15% at level 10.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Appraiser_Devaluation)]
	public class Appraiser_Devaluation : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int MaximumSkillLevel = 10;
		private const int MaximumEnhanceLevel = 100;
		private const float BaseDefenseReductionPercent = 10f;
		private const float MaximumDefenseReductionPercent = 20f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			var selectedTargets = new List<ICombatEntity>();

			if (target != null)
				selectedTargets.Add(target);

			this.Handle(skill, caster, originPos, farPos, selectedTargets);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> selectedTargets)
		{
			if (caster == null || caster.IsDead)
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
				if (caster is Character character)
					character.ServerMessage(Localization.Get("Not enough SP."));

				return;
			}

			var targetPosition = primaryTarget.Position;

			caster.TurnTowards(targetPosition);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, targetPosition);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPosition);

			skill.IncreaseOverheat();

			var skillLevel = Math.Min(
				Math.Max(skill.Level, 1),
				MaximumSkillLevel
			);

			var reductionPercent =
				BaseDefenseReductionPercent +
				(MaximumDefenseReductionPercent - BaseDefenseReductionPercent) *
				(skillLevel - 1) /
				(MaximumSkillLevel - 1);

			reductionPercent *= this.GetEnhanceMultiplier(caster);

			var reductionRate = reductionPercent / 100f;
			var duration = TimeSpan.FromSeconds(skillLevel);

			var areaTargets = caster.Map
				.GetAttackableEnemiesInPosition(
					caster,
					targetPosition,
					skill.Data.SplashRange
				)
				.Where(target => target != null && !target.IsDead)
				.ToList();

			if (!areaTargets.Contains(primaryTarget))
				areaTargets.Insert(0, primaryTarget);

			foreach (var enemy in areaTargets)
			{
				if (enemy == null || enemy.IsDead)
					continue;

				enemy.StartBuff(
					BuffId.Devaluation_Debuff,
					reductionRate,
					skillLevel,
					duration,
					caster,
					skill.Id
				);
			}

			caster.SetAttackState(false);
		}

		private void CancelSkill(Skill skill, ICombatEntity caster)
		{
			caster.SetAttackState(false);
			Send.ZC_SKILL_CAST_CANCEL(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
		}

		private float GetEnhanceMultiplier(ICombatEntity caster)
		{
			if (caster is not Character character)
				return 1f;

			var abilityLevel = Math.Min(
				character.Abilities.GetLevel(AbilityId.Appraiser4),
				MaximumEnhanceLevel
			);

			if (abilityLevel <= 0)
				return 1f;

			var bonusPercent = abilityLevel * 0.5f;

			if (abilityLevel >= MaximumEnhanceLevel)
				bonusPercent += 10f;

			return 1f + bonusPercent / 100f;
		}
	}
}
