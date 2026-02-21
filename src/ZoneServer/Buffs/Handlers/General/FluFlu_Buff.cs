using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the FluFlu_Buff, which applies Fear and Confuse debuffs to the target.
	/// </summary>
	/// <remarks>
	/// NumArg1: The skill level, used to calculate the duration of the debuffs.
	/// </remarks>
	[BuffHandler(BuffId.FluFlu_Buff)]
	public class FluFlu_Buff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var caster = buff.Caster;
			if (caster == null) return;

			// Show the initial buff text effect.
			Send.ZC_NORMAL.PlayTextEffect(target, caster, "SHOW_BUFF_TEXT", (float)buff.Id, null, "Item");

			var skillLevel = buff.NumArg1;

			// Calculate the duration for the debuffs.
			var duration = TimeSpan.FromMilliseconds(8000 + skillLevel * 200);

			// Apply the Fear and Confuse debuffs to the target.
			target.StartBuff(BuffId.Fear, skillLevel, 0, duration, caster);
			target.StartBuff(BuffId.Confuse, skillLevel, 0, duration, caster);

			// Since this buff's only purpose is to apply other buffs, it can be
			// removed immediately after activation.
			target.StopBuff(buff.Id);
		}
	}
}
