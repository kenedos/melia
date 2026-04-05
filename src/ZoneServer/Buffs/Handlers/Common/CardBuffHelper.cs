using System;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Shared helper for card buff handlers that need to scan equipped
	/// cards and sum star levels.
	/// </summary>
	public static class CardBuffHelper
	{
		/// <summary>
		/// Sums the star levels of all equipped cards that use the given
		/// script function and resolve to the given buff ID.
		/// </summary>
		public static float GetTotalStarLevel(Character character, BuffId buffId, string scriptFunction)
		{
			var allCards = character.Inventory.GetCards();
			var total = 0f;

			foreach (var cardKvp in allCards)
			{
				var card = cardKvp.Value;
				if (card == null)
					continue;

				if (!ZoneServer.Instance.Data.EquipCardDb.TryFind(card.Data.ClassName, out var cardData))
					continue;

				if (cardData.Script?.Function != scriptFunction)
					continue;

				// Check if this card resolves to the same buff ID.
				// Non-existent buff names fall back based on potion type.
				var cardBuffName = cardData.Script.StrArg;
				if (!Enum.TryParse<BuffId>(cardBuffName, out var cardBuffId))
				{
					var potionType = cardData.ConditionScript?.StrArg ?? "";
					cardBuffId = potionType == "HPPOTION" ? BuffId.CARD_CON : BuffId.CARD_MNA;
				}

				if (cardBuffId != buffId)
					continue;

				total += Math.Max(card.CardLevel, 1);
			}

			return total;
		}
	}
}
