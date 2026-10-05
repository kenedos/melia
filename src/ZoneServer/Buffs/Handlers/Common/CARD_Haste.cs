using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handles the movement speed buff granted by the Velpede card.
	/// The card grants +4 movement speed for 6 seconds.
	/// Repeated activations refresh the buff without stacking the bonus.
	/// </summary>
	[BuffHandler(BuffId.CARD_Haste)]
	public class CARD_Haste : BuffHandler
	{
		private const float MovementSpeedBonus = 4f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			SetPropertyModifier(
				buff,
				buff.Target,
				PropertyName.MSPD_BM,
				MovementSpeedBonus
			);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(
				buff,
				buff.Target,
				PropertyName.MSPD_BM
			);
		}
	}
}
