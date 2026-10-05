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
	/// Expose Weakness.
	/// Damages the selected target and enemies around it, applying
	/// Blindside_Debuff and increasing their chance of receiving
	/// critical hits.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Appraiser_Blindside)]
	public class Appraiser_Blindside : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const float BaseCriticalChanceBonus = 6f;
		private const float CriticalChanceBonusPerLevel = 0.5f;
		private const int MaximumSkillLevel = 10;
		private static readonly TimeSpan DebuffDuration = TimeSpan.FromSeconds(10);

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

			var skillLevel = Math.Min(
				Math.Max(skill.Level, 1),
				MaximumSkillLevel
			);

			var criticalChanceBonus =
				BaseCriticalChanceBonus +
				(skillLevel - 1) * CriticalChanceBonusPerLevel;

			
			foreach (var enemy in areaTargets)
			{
				if (enemy == null || enemy.IsDead)
					continue;

				enemy.StartBuff(
					BuffId.Blindside_Debuff,
					criticalChanceBonus,
					skillLevel,
					DebuffDuration,
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
	}
}
