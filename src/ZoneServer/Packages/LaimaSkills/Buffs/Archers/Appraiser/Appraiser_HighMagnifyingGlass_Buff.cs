using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Archers.Appraiser
{
	/// <summary>
	/// Handler for the High Scale Magnifying Glass buff, which raises
	/// accuracy and block penetration.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.HighMagnifyingGlass_Buff)]
	public class Appraiser_HighMagnifyingGlass_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = GetCaptionRatio(buff, 1);

			AddPairedPropertyModifier(buff, buff.Target, PropertyName.HR_BM, PropertyName.HR_RATE_BM, bonus);
			AddPairedPropertyModifier(buff, buff.Target, PropertyName.BLK_BREAK_BM, PropertyName.BLK_BREAK_RATE_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePairedPropertyModifier(buff, buff.Target, PropertyName.HR_BM, PropertyName.HR_RATE_BM);
			RemovePairedPropertyModifier(buff, buff.Target, PropertyName.BLK_BREAK_BM, PropertyName.BLK_BREAK_RATE_BM);
		}
	}
}
