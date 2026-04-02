using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for card_MDEF buff, which temporarily increases Magical Defense.
	/// Used by Frogola card (Magical Defense +[★ * 3]% for 20s on HP potion use).
	/// </summary>
	[BuffHandler(BuffId.card_MDEF)]
	public class card_MDEF : BuffHandler
	{
		private const float MDEFMultiplierPerStar = 3f / 100f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var cardStarLevel = buff.NumArg1;
			var mdefMultiplier = cardStarLevel * MDEFMultiplierPerStar;

			AddPropertyModifier(buff, target, PropertyName.MDEF_RATE_BM, mdefMultiplier);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.MDEF_RATE_BM);
		}
	}
}
