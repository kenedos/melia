using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.Handlers.Card
{
	/// <summary>
	/// Handler for the CARD_Wound debuff from the Rajapearl card.
	/// Deals periodic bleeding damage based on the attack damage that triggered it.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.CARD_Wound)]
	public class CARD_WoundOverride : DamageOverTimeBuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.NumArg2 *= 2f;

			base.OnActivate(buff, activationType);

			if (activationType == ActivationType.Start)
				Send.ZC_SHOW_EMOTICON(buff.Target, "I_emo_bleed", buff.Duration);
		}

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Bleeding;
		}
	}
}
