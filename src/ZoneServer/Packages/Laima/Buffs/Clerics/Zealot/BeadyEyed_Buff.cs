using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Zealot
{
	[Package("laima")]
	[BuffHandler(BuffId.BeadyEyed_Buff)]
	public class BeadyEyed_BuffOverride : BuffHandler
	{
		private const float CriticalRateBonus = 20f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.CRTHR_BM, CriticalRateBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTHR_BM);
		}
	}
}
