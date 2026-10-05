using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Scouts.Rogue
{
	/// <summary>
	/// Handler for the Rogue skill Sneak Hit.
	/// Passive skill that grants a damage buff when a Cloaking buff ends.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Rogue_SneakHit)]
	public class Rogue_SneakHitOverride : ISelfSkillHandler, ISkillOnBuffEndHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			caster.ServerMessage(Localization.Get("Sneak Hit is a passive skill."));
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);
		}

		/// <summary>
		/// Called when any buff ends on the character.
		/// If the buff has the "Cloaking" tag, starts SneakHit_Buff.
		/// </summary>
		public void OnBuffEnd(Skill skill, ICombatEntity target, Buff buff)
		{
			if (!buff.Data.Tags.Contains(BuffTag.Cloaking))
				return;

			target.StartBuff(BuffId.SneakHit_Buff, skill.Level, 0, TimeSpan.FromSeconds(3), target);
		}
	}
}
