using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for Blessing, Increases Attack.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Blessing_Buff)]
	public class Blessing_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var atkMultiplier = GetCaptionRatio(buff, 1) / 100f;

			// Apply the attack modifier
			AddPropertyModifier(buff, target, PropertyName.PATK_RATE_BM, atkMultiplier);
			AddPropertyModifier(buff, target, PropertyName.MATK_RATE_BM, atkMultiplier);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			// Remove the attack modifier
			RemovePropertyModifier(buff, target, PropertyName.PATK_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.MATK_RATE_BM);
		}
	}
}
