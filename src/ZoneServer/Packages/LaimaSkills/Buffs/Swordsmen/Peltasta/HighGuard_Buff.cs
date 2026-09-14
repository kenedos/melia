using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.HandlersOverrides.Swordsman.Peltasta
{
	/// <summary>
	/// Handle for the High Guard Buff, which increases the target's block rate
	/// and critical defense, but prevents evasion.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.HighGuard_Buff)]
	public class HighGuard_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			AddPairedPropertyModifier(buff, target, PropertyName.BLK_BM, PropertyName.BLK_RATE_BM, GetCaptionRatio(buff, 1));
			AddPropertyModifier(buff, target, PropertyName.CRTDR_RATE_BM, GetCaptionRatio(buff, 2) / 100f);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePairedPropertyModifier(buff, target, PropertyName.BLK_BM, PropertyName.BLK_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.CRTDR_RATE_BM);
		}
	}
}
