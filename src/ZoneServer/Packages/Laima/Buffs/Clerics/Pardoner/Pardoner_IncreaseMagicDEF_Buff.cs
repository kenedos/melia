using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Increase Magic Defense buff.
	/// Raises the target's magic defense by a rate and by a flat amount.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.IncreaseMagicDEF_Buff)]
	public class Pardoner_IncreaseMagicDEF_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			AddPropertyModifier(buff, target, PropertyName.MDEF_RATE_BM, GetCaptionRatio(buff, 1) / 100f);
			AddPropertyModifier(buff, target, PropertyName.MDEF_BM, GetCaptionRatio(buff, 2));
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.MDEF_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.MDEF_BM);
		}
	}
}
