using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Card
{
	/// <summary>
	/// Handler for the CARD_Haste buff from the Velpede card, which raises
	/// movement speed.
	/// </summary>
	[Package("system")]
	[BuffHandler(BuffId.CARD_Haste)]
	public class CARD_HasteOverride : BuffHandler
	{
		private const float MovementSpeedBonus = 4;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			SetPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, MovementSpeedBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
