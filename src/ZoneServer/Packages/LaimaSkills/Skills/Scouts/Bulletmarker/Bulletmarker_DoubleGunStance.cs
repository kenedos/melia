using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for the Bullet Marker skill Double Gun Stance, which toggles
	/// the double pistol stance and its basic attack.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Bulletmarker_DoubleGunStance)]
	public class Bulletmarker_DoubleGunStanceOverride : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, originPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, ForceId.GetNew(), null);

			if (caster.IsBuffActive(BuffId.DoubleGunStance_Buff))
				caster.StopBuff(BuffId.DoubleGunStance_Buff);
			else
				caster.StartBuff(BuffId.DoubleGunStance_Buff, skill.Level, 0, TimeSpan.Zero, caster, skill.Id);
		}
	}
}
