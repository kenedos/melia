using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for Aspersion, Increases Defense.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Aspersion_Buff)]
	public class Aspersion_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var defMultiplier = GetCaptionRatio(buff, 1) / 100f;

			// Apply the defense modifier
			AddPropertyModifier(buff, target, PropertyName.DEF_RATE_BM, defMultiplier);
			AddPropertyModifier(buff, target, PropertyName.MDEF_RATE_BM, defMultiplier);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			// Remove the defense modifier
			RemovePropertyModifier(buff, target, PropertyName.DEF_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.MDEF_RATE_BM);
		}
	}
}
