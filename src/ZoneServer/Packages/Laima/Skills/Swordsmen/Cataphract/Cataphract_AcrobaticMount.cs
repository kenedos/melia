using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Cataphract
{
	/// <summary>
	/// Handler for the Cataphract skill Acrobatic Mount.
	/// Passive skill that grants bonuses after mounted dash.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cataphract_AcrobaticMount)]
	public class Cataphract_AcrobaticMountOverride : ISelfSkillHandler, ISkillOnBuffStartHandler
	{
		/// <summary>
		/// Handles the Acrobatic Mount skill execution.
		/// This is now a passive skill - using it just shows a message.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			caster.ServerMessage(Localization.Get("Acrobatic Mount is a passive skill. Dash while mounted to activate its effects."));
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);
		}

		/// <summary>
		/// Called when any buff starts on the character.
		/// Applies Acrobatic Mount buff when DashRun starts while mounted.
		/// </summary>
		public void OnBuffStart(Skill skill, ICombatEntity target, Buff buff)
		{
			if (buff.Id != BuffId.DashRun)
				return;

			if (!target.IsRiding())
				return;

			target.StartBuff(BuffId.AcrobaticMount_Buff, skill.Level, 0, TimeSpan.FromSeconds(5), target);
		}
	}
}
