using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Clerics.Sadhu
{
	/// <summary>
	/// Handler for the Sadhu skill Out of Body Cancel.
	/// Returns spirit to body by removing the spirit form buff.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sadhu_OutofBodyCancel)]
	public class Sadhu_OutofBodyCancelOverride : IMeleeGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.IsBuffActive(BuffId.OOBE_Soulmaster_Buff))
				return;

			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			caster.StopBuff(BuffId.OOBE_Soulmaster_Buff);
		}
	}
}
