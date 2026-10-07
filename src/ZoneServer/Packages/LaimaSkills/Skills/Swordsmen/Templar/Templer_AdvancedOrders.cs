using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Templar
{
	/// <summary>
	/// Handler for the Templar skill Advanced Orders, which toggles the orders on
	/// and off.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Templer_AdvancedOrders)]
	public class Templer_AdvancedOrdersOverride : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster.IsBuffActive(BuffId.AdvancedOrders_On_Buff))
			{
				caster.StopBuff(BuffId.AdvancedOrders_On_Buff);
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			caster.StartBuff(BuffId.AdvancedOrders_On_Buff, skill.Level, 0, TimeSpan.Zero, caster, skill.Id);
		}
	}
}
