using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Generic handler for potion-use stat buff cards (CARD_CON, CARD_MNA).
	/// On activation, scans all equipped cards that use the same buff ID,
	/// sums their star levels per property, and applies the bonuses.
	/// Supports multiple different stat cards sharing the same buff ID
	/// (e.g., Stonefroster/CON + Pyroego/STR both use CARD_CON) and
	/// multiple copies of the same card at different star levels.
	/// </summary>
	[BuffHandler(BuffId.CARD_MNA, BuffId.CARD_CON)]
	public class CARD_MNA : BuffHandler
	{
		private const float BonusPerStar = 10f;
		private const string PropertiesKey = "Melia.Card.Properties";
		private const string ScriptFunction = "SCR_CARDEFFECT_ADD_BUFF_PC_STAT";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			// Remove previous modifiers on reapplication
			RemoveAllModifiers(buff, character);

			var allCards = character.Inventory.GetCards();
			var buffClassName = buff.Id.ToString();

			foreach (var cardKvp in allCards)
			{
				var card = cardKvp.Value;
				if (card == null)
					continue;

				if (!ZoneServer.Instance.Data.EquipCardDb.TryFind(card.Data.ClassName, out var cardData))
					continue;

				if (cardData.Script?.Function != ScriptFunction)
					continue;

				// Check if this card uses the same buff ID.
				// Non-existent buff names (CARD_INT, CARD_STR, CARD_DEX)
				// fall back based on potion type: HP -> CARD_CON, SP -> CARD_MNA.
				var cardBuffName = cardData.Script.StrArg;
				if (!Enum.TryParse<BuffId>(cardBuffName, out var cardBuffId))
				{
					var potionType = cardData.ConditionScript?.StrArg ?? "";
					cardBuffId = potionType == "HPPOTION" ? BuffId.CARD_CON : BuffId.CARD_MNA;
				}

				if (cardBuffId != buff.Id)
					continue;

				var propertyName = cardData.Script.StrArg2;
				var starLevel = Math.Max(card.CardLevel, 1);
				var bonus = starLevel * BonusPerStar;

				// Track property in comma-separated list
				var existing = buff.Vars.GetString(PropertiesKey);
				if (string.IsNullOrEmpty(existing))
					buff.Vars.SetString(PropertiesKey, propertyName);
				else if (!existing.Contains(propertyName))
					buff.Vars.SetString(PropertiesKey, existing + "," + propertyName);

				AddPropertyModifier(buff, character, propertyName, bonus);
			}

			// Invalidate all affected stats
			InvalidateStats(buff, character);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			RemoveAllModifiers(buff, character);
		}

		/// <summary>
		/// Removes all tracked property modifiers and invalidates stats.
		/// </summary>
		private static void RemoveAllModifiers(Buff buff, Character character)
		{
			var properties = buff.Vars.GetString(PropertiesKey);
			if (string.IsNullOrEmpty(properties))
				return;

			foreach (var propertyName in properties.Split(','))
			{
				RemovePropertyModifier(buff, character, propertyName);
			}

			InvalidateStats(buff, character);
			buff.Vars.SetString(PropertiesKey, "");
		}

		/// <summary>
		/// Invalidates all main stats that have tracked modifiers.
		/// </summary>
		private static void InvalidateStats(Buff buff, Character character)
		{
			var properties = buff.Vars.GetString(PropertiesKey);
			if (string.IsNullOrEmpty(properties))
				return;

			foreach (var propertyName in properties.Split(','))
			{
				var underscoreIdx = propertyName.IndexOf('_');
				if (underscoreIdx > 0)
					character.InvalidateProperties(propertyName.Substring(0, underscoreIdx));
			}
		}
	}
}
