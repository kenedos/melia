using System;
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

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Zealot
{
	[Package("laima")]
	[SkillHandler(SkillId.Zealot_Fanaticism)]
	public class Zealot_Fanaticism : IGroundSkillHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private static readonly TimeSpan BuffDuration = TimeSpan.FromMinutes(15);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead)
			{
				StopSkill(caster);
				return;
			}

			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				StopSkill(character);
				return;
			}

			try
			{
				var skillLevel = Math.Clamp(skill.Level, MinimumSkillLevel, MaximumSkillLevel);
				var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

				skill.IncreaseOverheat();
				character.SetAttackState(true);

				Send.ZC_SKILL_READY(character, skill, skillHandle, originPos, character.Position);
				Send.ZC_SKILL_MELEE_GROUND(character, skill, character.Position);

				character.StopBuff(BuffId.Fanaticism_Buff);
				character.StartBuff(BuffId.Fanaticism_Buff, skillLevel, 0, BuffDuration, character, skill.Id);
			}
			finally
			{
				StopSkill(character);
			}
		}

		private static void StopSkill(ICombatEntity caster)
		{
			if (caster == null)
				return;

			caster.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(caster);
		}
	}
}
