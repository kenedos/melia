using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.Handlers.Wizards.Onmyoji
{
	[Package("laima")]
	[BuffHandler(BuffId.WhiteTigerHowling_Buff)]
	public class Onmyoji_WhiteTigerHowling_BuffOverride : BuffHandler
	{
		private const string MovementSpeedProperty = PropertyName.MSPD_BM;
		private const float MovementSpeedBonus = 10f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, MovementSpeedProperty, MovementSpeedBonus);
			Send.ZC_MSPD(buff.Target);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, MovementSpeedProperty);
			Send.ZC_MSPD(buff.Target);
		}
	}
}
