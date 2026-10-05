using System;
using Melia.Shared.Game.Const;
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
	[SkillHandler(SkillId.Zealot_FanaticIllusion)]
	public class Zealot_FanaticIllusion : IGroundSkillHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private static readonly TimeSpan BuffDuration = TimeSpan.FromSeconds(15);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead)
				return;

			if (!character.TrySpendSp(skill))
			{
				character.SetAttackState(false);
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			character.SetAttackState(true);

			Send.ZC_SKILL_READY(character, skill, 1, originPos, character.Position);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, character.Position);

			var skillLevel = Math.Clamp(skill.Level, MinimumSkillLevel, MaximumSkillLevel);
			character.StartBuff(BuffId.FanaticIllusion_Buff, skillLevel, 0, BuffDuration, character, skill.Id);

			skill.IncreaseOverheat();
			character.SetAttackState(false);
		}
	}
}
