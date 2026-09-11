using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Grace: Additional Damage buff sold by a Spell Shop.
	/// Raises the target's attack by a rate.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.SpellShop_Blessing_Buff)]
	public class Pardoner_SpellShop_Blessing_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var atkMultiplier = GetCaptionRatio(buff, 1) / 100f;

			AddPropertyModifier(buff, target, PropertyName.PATK_RATE_BM, atkMultiplier);
			AddPropertyModifier(buff, target, PropertyName.MATK_RATE_BM, atkMultiplier);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.PATK_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.MATK_RATE_BM);
		}
	}
}
