using System;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using System.Collections.Generic;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.PlagueDoctor
{
	/// <summary>
	/// Incineration.
	/// Applies a burning damage-over-time effect to enemies affected by debuffs.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.PlagueDoctor_Incineration)]
	public class PlagueDoctor_Incineration : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int MaximumTargets = 8;
		private const float EffectRadius = 100f;
		private static readonly TimeSpan BaseDuration = TimeSpan.FromSeconds(10);
		private static readonly TimeSpan AdditionalDurationPerDebuff = TimeSpan.FromSeconds(1);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			if (caster is not Character character || character.IsDead)
				return;

			if (caster.Map == null)
				return;

			var area = new Circle(farPos, EffectRadius);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, area).Where(enemy => enemy != null && !enemy.IsDead && this.CountDebuffs(enemy) > 0).OrderBy(enemy => farPos.Get2DDistance(enemy.Position)).Take(MaximumTargets).ToList();

			if (targets.Count == 0)
			{
				character.ServerMessage(Localization.Get("No enemy affected by a debuff was found."));
				this.CancelSkill(skill, caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				this.CancelSkill(skill, caster);
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var spreadChainId = PlagueDoctorSpreadTracker.CreateChain();

			foreach (var enemy in targets)
			{
				var debuffCount = this.CountDebuffs(enemy);
				var duration = BaseDuration + TimeSpan.FromTicks(AdditionalDurationPerDebuff.Ticks * debuffCount);

				enemy.StartBuff(BuffId.Incineration_Debuff, skill.Level, debuffCount, duration, caster, skill.Id, newBuff => newBuff.Vars.SetInt("Melia.PlagueDoctor.IncinerationSpreadChainId", spreadChainId));
			}

			caster.SetAttackState(false);
		}

		private int CountDebuffs(ICombatEntity target)
		{
			var buffComponent = target.Components.Get<BuffComponent>();

			if (buffComponent == null)
				return 0;

			return buffComponent.GetList().Count(buff => buff.Data.Type == BuffType.Debuff && buff.Id != BuffId.Incineration_Debuff);
		}

		private void CancelSkill(Skill skill, ICombatEntity caster)
		{
			caster.SetAttackState(false);
			Send.ZC_SKILL_CAST_CANCEL(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
		}
	}
}
