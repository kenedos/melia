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

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Inquisitor
{
	[Package("laima")]
	[SkillHandler(SkillId.Inquisitor_IronMaiden)]
	public class Inquisitor_IronMaiden : IGroundSkillHandler, IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const float MinimumDurationSeconds = 2.3f;
		private const float MaximumDurationSeconds = 5f;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_atk_long_cast_f", "voice_war_atk_long_cast");
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopSound("voice_atk_long_cast_f", "voice_war_atk_long_cast");
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos, target);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos, targets?.FirstOrDefault());
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			var target = this.FindTarget(character, skill, selectedTarget);

			if (target == null)
			{
				character.ServerMessage(Localization.Get("No valid target was found."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			var skillLevel = Math.Clamp(skill.Level, MinimumSkillLevel, MaximumSkillLevel);
			var durationSeconds = MinimumDurationSeconds + (skillLevel - 1) * (MaximumDurationSeconds - MinimumDurationSeconds) / (MaximumSkillLevel - MinimumSkillLevel);
			var duration = TimeSpan.FromSeconds(durationSeconds);

			character.TurnTowards(target.Position);
			character.SetAttackState(true);

			Send.ZC_SKILL_READY(character, skill, 1, originPos, target.Position);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, target.Position);

			target.StartBuff(BuffId.IronMaiden_Debuff, skillLevel, 0, duration, character, skill.Id);

			skill.IncreaseOverheat();
			character.SetAttackState(false);
		}

		private ICombatEntity FindTarget(Character caster, Skill skill, ICombatEntity selectedTarget)
		{
			if (this.IsValidTarget(caster, selectedTarget))
				return selectedTarget;

			return caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, skill.Data.MaxRange)
				.Where(target => this.IsValidTarget(caster, target))
				.OrderBy(target => caster.Position.Get2DDistance(target.Position))
				.FirstOrDefault();
		}

		private bool IsValidTarget(Character caster, ICombatEntity target)
		{
			if (target == null || target.IsDead || !caster.IsEnemy(target))
				return false;

			var size = target.Properties.GetString(PropertyName.Size);
			var isSmall = string.Equals(size, SizeType.S.ToString(), StringComparison.OrdinalIgnoreCase);
			var isMedium = string.Equals(size, SizeType.M.ToString(), StringComparison.OrdinalIgnoreCase);

			if (!isSmall && !isMedium)
				return false;

			if (Enum.TryParse<MonsterRank>(target.Properties.GetString(PropertyName.MonRank), true, out var rank) && rank == MonsterRank.Boss)
				return false;

			return true;
		}
	}
}
