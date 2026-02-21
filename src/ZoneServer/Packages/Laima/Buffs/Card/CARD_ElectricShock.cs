using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Card
{
	/// <summary>
	/// Handler for the CARD_ElectricShock debuff from monster cards.
	/// Deals periodic lightning damage based on the attack damage that triggered it.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.CARD_ElectricShock)]
	public class CARD_ElectricShockOverride : DamageOverTimeBuffHandler
	{
		/// <summary>
		/// Returns the hit type for lightning damage visuals.
		/// </summary>
		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Lightning;
		}
	}
}
