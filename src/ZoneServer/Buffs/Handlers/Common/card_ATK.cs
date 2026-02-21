using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for card_ATK buff, which temporarily increases Physical Attack.
	/// Used by cards like Glass Mole (Physical Attack +[★ * 1.5]% for duration).
	/// </summary>
	[BuffHandler(BuffId.card_ATK)]
	public class card_ATK : BuffHandler
	{
		private const float ATKMultiplierPerStar = 1.5f / 100f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var cardStarLevel = buff.NumArg1;
			var atkMultiplier = cardStarLevel * ATKMultiplierPerStar;

			AddPropertyModifier(buff, target, PropertyName.PATK_RATE_BM, atkMultiplier);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.PATK_RATE_BM);
		}
	}
}
