using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for card_MATK buff, which temporarily increases Magical Attack.
	/// Used by cards that provide Magical Attack +[★ * 1.5]% for duration.
	/// </summary>
	[BuffHandler(BuffId.card_MATK)]
	public class card_MATK : BuffHandler
	{
		private const float MATKMultiplierPerStar = 1.5f / 100f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var cardStarLevel = buff.NumArg1;
			var matkMultiplier = cardStarLevel * MATKMultiplierPerStar;

			AddPropertyModifier(buff, target, PropertyName.MATK_RATE_BM, matkMultiplier);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.MATK_RATE_BM);
		}
	}
}
