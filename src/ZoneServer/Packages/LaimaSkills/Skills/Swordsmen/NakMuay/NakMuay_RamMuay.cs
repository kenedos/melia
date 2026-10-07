using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Swordsmen.NakMuay
{
	/// <summary>
	/// Handler for the Nak Muay skill Ram Muay, which toggles the stance
	/// that unlocks the Nak Muay's skills and basic attacks.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.NakMuay_RamMuay)]
	public class NakMuay_RamMuayOverride : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, originPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, ForceId.GetNew(), null);

			if (caster.IsBuffActive(BuffId.RamMuay_Buff))
				caster.StopBuff(BuffId.RamMuay_Buff);
			else
				caster.StartBuff(BuffId.RamMuay_Buff, skill.Level, 0, TimeSpan.Zero, caster, skill.Id);
		}
	}
}
