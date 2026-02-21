//--- Melia Script ----------------------------------------------------------
// Pad Functions
//--- Description -----------------------------------------------------------
// Scriptable functions that handle pad specific behaviors.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;

internal class PadFunctionsScript : GeneralScript
{
	[ScriptableFunction]
	public static void SCR_ENGINEER_SKILL_CC_ACTIVATE(ICombatEntity caster, Skill skill, Pad pad)
	{
		var targets = pad.Trigger.GetActors<ICombatEntity>();
		switch (skill.Id)
		{
			case SkillId.Engineer_FrozenTurret:
				foreach (var target in targets)
				{
					if (caster.CanAttack(target))
						target.StartBuff(BuffId.ENGINEER_TURRET_SLOW_DEBUFF, skill.Level, 0, TimeSpan.FromSeconds(4), caster);
				}
				break;
			default:
				break;
		}
	}
}
