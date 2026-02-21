using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the FluFlu_Debuff, which applies the Fear debuff to the target.
	/// </summary>
	/// <remarks>
	/// NumArg1: The skill level, used to calculate the duration of the Fear debuff.
	/// </remarks>
	[BuffHandler(BuffId.FluFlu_Debuff)]
	public class FluFlu_Debuff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var caster = buff.Caster;
			if (caster == null) return;

			var skillLevel = buff.NumArg1;

			// Calculate the duration for the Fear debuff.
			var duration = TimeSpan.FromMilliseconds(8000 + skillLevel * 200);

			// Apply the Fear debuff to the target.
			target.StartBuff(BuffId.Fear, skillLevel, 0, duration, caster);

			// Since this buff's only job is to apply another buff,
			// it can be removed immediately.
			target.StopBuff(buff.Id);
		}
	}
}
