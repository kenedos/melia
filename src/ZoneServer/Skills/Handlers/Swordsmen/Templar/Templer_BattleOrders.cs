using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Swordsmen.Templar
{
	[Package("laima")]
	[SkillHandler(SkillId.Templer_BattleOrders)]
	public class Templer_BattleOrdersOverride : ITargetSkillHandler, IGroundSkillHandler, IMeleeGroundSkillHandler, IDynamicCasted, ICancelSkillHandler
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			StopSkill(caster);
		}

		public void Handle(Skill skill, ICombatEntity caster)
		{
			StopSkill(caster);
		}

		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (caster == null)
				return;

			this.Cast(skill, caster, caster.Position, caster.Position);
		}

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
			if (skill == null || caster is not Character character || character.IsDead)
			{
				StopSkill(caster);
				return;
			}

			if (character.IsBuffActive(BuffId.BattleOrders_On_Buff))
			{
				character.StopBuff(BuffId.BattleOrders_On_Buff);
				StopSkill(character);
				return;
			}

			try
			{
				character.SetAttackState(true);

				var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

				Send.ZC_SKILL_READY(character, skill, skillHandle, originPos, farPos);
				Send.ZC_NORMAL.UpdateSkillEffect(character, 0, originPos, character.Direction, Position.Zero);
				Send.ZC_SKILL_MELEE_GROUND(character, skill, farPos);

				character.StartBuff(BuffId.BattleOrders_On_Buff, skill.Level, 0f, TimeSpan.Zero, character, skill.Id);
			}
			finally
			{
				StopSkill(character);
			}
		}

		private static void StopSkill(ICombatEntity caster)
		{
			if (caster is not Character character)
				return;

			character.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(character);
		}
	}
}
