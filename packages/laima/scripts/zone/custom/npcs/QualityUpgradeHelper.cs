using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;

public static class QualityUpgradeHelper
{
	private const int NuclePowderItemId = 649025;
	private const int SierraPowderItemId = 649026;

	private static readonly EquipSlot[] AllowedSlots =
	{
		EquipSlot.Top,
		EquipSlot.Gloves,
		EquipSlot.Shoes,
		EquipSlot.RightHand,
		EquipSlot.LeftHand,
		EquipSlot.Pants,
		EquipSlot.RightHandSub,
		EquipSlot.LeftHandSub,
		EquipSlot.Necklace,
		EquipSlot.Bracelet1,
		EquipSlot.Bracelet2,
	};

	public static bool CanBeUpgraded(Item item)
	{
		if (item?.Data == null || item.ObjectId <= 0 || item.Data.Type != ItemType.Equip || item.IsLocked)
			return false;

		var grade = GetGrade(item);
		return grade == ItemGrade.Unique || grade == ItemGrade.Legend;
	}

	public static bool IsAllowedEquipSlot(EquipSlot slot)
	{
		return AllowedSlots.Contains(slot);
	}

	public static bool TryGetRequirements(Item item, out ItemGrade targetGrade, out int requiredItems, out int sierraCost, out int nucleCost)
	{
		targetGrade = ItemGrade.None;
		requiredItems = 0;
		sierraCost = 0;
		nucleCost = 0;

		if (item == null)
			return false;

		switch (GetGrade(item))
		{
			case ItemGrade.Unique:
				targetGrade = ItemGrade.Legend;
				requiredItems = 2;
				sierraCost = 50;
				nucleCost = 500;
				return true;
			case ItemGrade.Legend:
				targetGrade = ItemGrade.Goddess;
				requiredItems = 2;
				sierraCost = 200;
				nucleCost = 1000;
				return true;
			default:
				return false;
		}
	}

	public static bool TryUpgradeItem(Character character, Item item, out string message)
	{
		message = "";

		if (character == null)
		{
			message = "Invalid character.";
			return false;
		}

		if (!CanBeUpgraded(item))
		{
			message = "Only unlocked Unique or Legend equipment can be upgraded.";
			return false;
		}

		var equippedEntry = character.Inventory
			.GetEquip()
			.FirstOrDefault(entry => entry.Value != null && entry.Value.ObjectId == item.ObjectId);

		if (equippedEntry.Value == null)
		{
			message = "Only equipped items can have their quality upgraded.";
			return false;
		}

		if (!IsAllowedEquipSlot(equippedEntry.Key))
		{
			message = "This equipment slot cannot have its quality upgraded.";
			return false;
		}

		if (!TryGetRequirements(item, out var targetGrade, out var requiredItems, out var sierraCost, out var nucleCost))
		{
			message = "This item cannot have its quality upgraded.";
			return false;
		}

		var sourceGrade = GetGrade(item);
		var copiesRequired = requiredItems - 1;
		var copies = character.Inventory
			.GetItems(inventoryItem =>
				inventoryItem != null &&
				inventoryItem.ObjectId != item.ObjectId &&
				inventoryItem.Id == item.Id &&
				inventoryItem.Data.Type == ItemType.Equip &&
				!inventoryItem.IsLocked &&
				GetGrade(inventoryItem) == sourceGrade)
			.Values
			.ToList();

		if (copies.Sum(copy => copy.Amount) < copiesRequired)
		{
			message = $"Not enough identical {GetGradeName(sourceGrade)} copies. Required in inventory: {copiesRequired}.";
			return false;
		}

		var sierraPowders = GetUnlockedItems(character, SierraPowderItemId);
		var nuclePowders = GetUnlockedItems(character, NuclePowderItemId);

		if (sierraPowders.Sum(powder => powder.Amount) < sierraCost)
		{
			message = $"Not enough Sierra Powder. Required: {sierraCost}.";
			return false;
		}

		if (nuclePowders.Sum(powder => powder.Amount) < nucleCost)
		{
			message = $"Not enough Nucle Powder. Required: {nucleCost}.";
			return false;
		}

		if (!RemoveItems(character, copies, copiesRequired) ||
			!RemoveItems(character, sierraPowders, sierraCost) ||
			!RemoveItems(character, nuclePowders, nucleCost))
		{
			message = "Failed to consume the required upgrade materials.";
			return false;
		}

		item.Properties.SetFloat(PropertyName.ItemGrade, (int)targetGrade);
		character.InvalidateProperties();

		Send.ZC_OBJECT_PROPERTY(character.Connection, item);
		Send.ZC_OBJECT_PROPERTY(character);
		Send.ZC_ITEM_EQUIP_LIST(character);
		character.AddonMessage("ITEM_PROP_UPDATE");
		character.AddonMessage("EQUIP_ITEM_LIST_UPDATE");

		message = $"Quality upgrade successful: {item.Data.ClassName} {GetGradeName(sourceGrade)} -> {GetGradeName(targetGrade)}.";
		return true;
	}

	public static ItemGrade GetGrade(Item item)
	{
		return (ItemGrade)(int)item.Properties.GetFloat(PropertyName.ItemGrade);
	}

	public static string GetGradeName(ItemGrade grade)
	{
		return grade switch
		{
			ItemGrade.Unique => "Unique",
			ItemGrade.Legend => "Legend",
			ItemGrade.Goddess => "Goddess",
			_ => grade.ToString(),
		};
	}

	private static List<Item> GetUnlockedItems(Character character, int itemId)
	{
		return character.Inventory
			.GetItems(item => item != null && item.Id == itemId && !item.IsLocked)
			.Values
			.ToList();
	}

	private static bool RemoveItems(Character character, IEnumerable<Item> items, int amount)
	{
		var remaining = amount;
		foreach (var inventoryItem in items)
		{
			if (remaining <= 0)
				break;

			var removeAmount = Math.Min(inventoryItem.Amount, remaining);
			var removeResult = character.Inventory.Remove(
				inventoryItem.ObjectId,
				removeAmount,
				InventoryItemRemoveMsg.Used);

			if (removeResult != InventoryResult.Success)
				return false;

			remaining -= removeAmount;
		}

		return remaining == 0;
	}
}
