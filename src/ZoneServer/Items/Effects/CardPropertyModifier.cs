using System.Linq;
using Melia.Shared.Game.Properties;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Logging;

namespace Melia.Zone.Items.Effects
{
	/// <summary>
	/// Manages property modifications applied by equipped cards.
	/// Tracks modifications by card slot number using character variables for persistence.
	///
	/// Uses a hybrid system:
	/// - EQUIP_* properties: Calculated on-demand (like equipment stats)
	/// - Other properties (_BM): Applied as buff modifiers to character.Properties
	/// </summary>
	public class CardPropertyModifier
	{
		public static CardPropertyModifier Instance { get; } = new CardPropertyModifier();

		/// <summary>
		/// Prefix used for storing card modifiers in character Variables.Perm.
		/// </summary>
		public const string ModifierVarPrefix = "Melia.CardModifier.";

		/// <summary>
		/// Returns true if the property should be calculated on-demand (like equipment)
		/// rather than applied as a buff modifier.
		/// </summary>
		/// <param name="propertyName">The property name to check.</param>
		/// <returns>True if property should use on-demand calculation.</returns>
		public static bool IsOnDemandProperty(string propertyName)
			=> propertyName.StartsWith("EQUIP_");

		/// <summary>
		/// Returns the name of the variable used to store modifiers for
		/// the given card slot and property.
		/// </summary>
		private static string GetModifierVarName(int cardSlot, string propertyName)
			=> $"{ModifierVarPrefix}Slot{cardSlot}.{propertyName}";

		/// <summary>
		/// Adds or updates a property modifier for a card.
		/// Looks up the card's slot automatically from the character's inventory.
		/// Stores tracking data in character.Variables.Perm for persistence.
		/// </summary>
		public void AddPropertyModifier(Character character, long cardObjectId, string propertyName, float value)
		{
			// Look up the card's slot from inventory
			var cardSlot = this.GetCardSlotByObjectId(character, cardObjectId);
			if (cardSlot == 0)
			{
				Log.Warning($"AddPropertyModifier: Could not find card slot for ObjectId {cardObjectId}");
				return;
			}

			this.AddPropertyModifierBySlot(character, cardSlot, propertyName, value);
		}

		/// <summary>
		/// Internal method to add property modifier by slot.
		/// For EQUIP_* properties: Only stores in Variables.Perm (calculated on-demand).
		/// For other properties: Stores in Variables.Perm AND applies to character.Properties.
		/// </summary>
		private void AddPropertyModifierBySlot(Character character, int cardSlot, string propertyName, float value)
		{
			if (!PropertyTable.Exists(character.Properties.Namespace, propertyName))
			{
				Log.Warning($"AddPropertyModifier: card slot {cardSlot} tried to add to property {propertyName} but doesn't exist in id namespace: {character.Properties.Namespace}.");
				return;
			}

			var varName = GetModifierVarName(cardSlot, propertyName);

			// Get old value from persistent storage
			var oldValue = character.Variables.Perm.GetFloat(varName, 0f);

			// Store new value in persistent storage
			character.Variables.Perm.SetFloat(varName, value);

			// For non-EQUIP_ properties, apply as buff modifier
			if (!IsOnDemandProperty(propertyName))
			{
				character.Properties.Modify(propertyName, value - oldValue);
			}
		}

		/// <summary>
		/// Gets the card slot for a given ObjectId.
		/// </summary>
		private int GetCardSlotByObjectId(Character character, long cardObjectId)
		{
			var cards = character.Inventory.GetCards();
			foreach (var kvp in cards)
			{
				if (kvp.Value.ObjectId == cardObjectId)
					return kvp.Key;
			}
			return 0;
		}

		/// <summary>
		/// Removes a specific property modifier for a card.
		/// </summary>
		public void RemovePropertyModifier(Character character, long cardObjectId, string propertyName)
		{
			var cardSlot = this.GetCardSlotByObjectId(character, cardObjectId);
			if (cardSlot == 0)
				return;

			this.RemovePropertyModifierBySlot(character, cardSlot, propertyName);
		}

		/// <summary>
		/// Internal method to remove property modifier by slot.
		/// For EQUIP_* properties: Only removes from Variables.Perm.
		/// For other properties: Removes from Variables.Perm AND reverses character.Properties change.
		/// </summary>
		private void RemovePropertyModifierBySlot(Character character, int cardSlot, string propertyName)
		{
			if (!PropertyTable.Exists(character.Properties.Namespace, propertyName))
			{
				Log.Warning($"RemovePropertyModifier: card slot {cardSlot} tried to remove to property {propertyName} but doesn't exist in id namespace: {character.Properties.Namespace}.");
				return;
			}

			var varName = GetModifierVarName(cardSlot, propertyName);

			if (character.Variables.Perm.TryGetFloat(varName, out var value))
			{
				// For non-EQUIP_ properties, reverse the buff modifier
				if (!IsOnDemandProperty(propertyName))
				{
					character.Properties.Modify(propertyName, -value);
				}

				character.Variables.Perm.Remove(varName);
			}
		}

		/// <summary>
		/// Removes all property modifiers for a card.
		/// Looks up the card's slot automatically from the character's inventory.
		/// If card is not found and clearAllIfNotFound is true, clears all slots as fallback.
		/// </summary>
		public void RemoveAllModifiers(Character character, long cardObjectId, bool clearAllIfNotFound = false)
		{
			var cardSlot = this.GetCardSlotByObjectId(character, cardObjectId);
			if (cardSlot == 0)
			{
				if (clearAllIfNotFound)
				{
					Log.Debug($"RemoveAllModifiers: Card ObjectId {cardObjectId} not found, clearing all slots as fallback.");
					for (var slot = 1; slot <= 15; slot++)
					{
						this.RemoveAllModifiersBySlot(character, slot);
					}
				}
				else
				{
					Log.Warning($"RemoveAllModifiers: Could not find card slot for ObjectId {cardObjectId}. Card may have been removed from inventory before cleanup.");
				}
				return;
			}

			this.RemoveAllModifiersBySlot(character, cardSlot);
		}

		/// <summary>
		/// Internal method to remove all modifiers for a specific slot.
		/// </summary>
		private void RemoveAllModifiersBySlot(Character character, int cardSlot)
		{
			var prefix = $"{ModifierVarPrefix}Slot{cardSlot}.";
			var varsToRemove = character.Variables.Perm.GetList()
				.Where(kvp => kvp.Key.StartsWith(prefix))
				.ToList();

			foreach (var kvp in varsToRemove)
			{
				var propertyName = kvp.Key.Substring(prefix.Length);

				if (character.Variables.Perm.TryGetFloat(kvp.Key, out var value))
				{
					// For non-EQUIP_ properties, reverse the buff modifier
					if (!IsOnDemandProperty(propertyName))
					{
						character.Properties.Modify(propertyName, -value);
					}

					character.Variables.Perm.Remove(kvp.Key);
				}
			}
		}

		/// <summary>
		/// Restores buff-style card property modifiers from character's permanent variables.
		/// Called after loading a character to reapply card effects to properties.
		/// Only restores non-EQUIP_ properties (EQUIP_* are calculated on-demand).
		/// </summary>
		public void RestoreAllModifiers(Character character)
		{
			var cardVars = character.Variables.Perm.GetList()
				.Where(kvp => kvp.Key.StartsWith(ModifierVarPrefix) && kvp.Value is float)
				.ToList();

			foreach (var kvp in cardVars)
			{
				// Parse property name from var key: "Melia.CardModifier.Slot{X}.{propertyName}"
				var afterPrefix = kvp.Key.Substring(ModifierVarPrefix.Length);
				var dotIndex = afterPrefix.IndexOf('.');
				if (dotIndex < 0)
					continue;

				var propertyName = afterPrefix.Substring(dotIndex + 1);

				// Skip EQUIP_* properties - they're calculated on-demand
				if (IsOnDemandProperty(propertyName))
					continue;

				var value = (float)kvp.Value;

				if (PropertyTable.Exists(character.Properties.Namespace, propertyName))
				{
					character.Properties.Modify(propertyName, value);
				}
			}
		}

		/// <summary>
		/// Gets the total card bonus for a specific property.
		/// Calculates on-demand by summing all card slot contributions from Variables.Perm.
		/// Used for EQUIP_* properties that aren't stored in character.Properties.
		/// </summary>
		/// <param name="character">The character to get bonuses for.</param>
		/// <param name="propertyName">The property name to sum bonuses for.</param>
		/// <returns>The total bonus from all equipped cards for this property.</returns>
		public float GetCardBonus(Character character, string propertyName)
		{
			var suffix = "." + propertyName;
			return character.Variables.Perm.GetList()
				.Where(kvp => kvp.Key.StartsWith(ModifierVarPrefix) &&
							  kvp.Key.EndsWith(suffix) &&
							  kvp.Value is float)
				.Sum(kvp => (float)kvp.Value);
		}
	}
}
