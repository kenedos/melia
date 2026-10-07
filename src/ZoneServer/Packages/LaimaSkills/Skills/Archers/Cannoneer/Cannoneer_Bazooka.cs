using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Archers.Cannoneer
{
	/// <summary>
	/// Handler for the Cannoneer skill Bazooka, which toggles the Bazooka
	/// stance for Cannon Shot and Cannon Barrage.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Cannoneer_Bazooka)]
	public class Cannoneer_BazookaOverride : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, originPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, ForceId.GetNew(), null);

			if (caster.IsBuffActive(BuffId.Bazooka_Buff))
				caster.StopBuff(BuffId.Bazooka_Buff);
			else
				caster.StartBuff(BuffId.Bazooka_Buff, skill.Level, 0, TimeSpan.Zero, caster, skill.Id);
		}
	}
}
