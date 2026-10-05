using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;

public static class TranscendenceHelper
{
	private const int MaxTranscendStage = 10;
	private const int BlessedGemItemId = 646045;

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

	public static bool CanBeTranscended(Item item)
	{
		return item?.Data != null &&
			item.ObjectId > 0 &&
			item.Data.Type == ItemType.Equip;
	}

	public static bool IsAllowedEquipSlot(EquipSlot slot)
	{
		return AllowedSlots.Contains(slot);
	}

	public static bool TryTranscendItem(
		Character character,
		Item item,
		out string message)
	{
		message = "";

		if (character == null)
		{
			message = "Invalid character.";
			return false;
		}

		if (item == null)
		{
			message = "Invalid item.";
			return false;
		}

		if (!CanBeTranscended(item))
		{
			message = "Only equipment can be transcended.";
			return false;
		}

		var equippedEntry = character.Inventory
			.GetEquip()
			.FirstOrDefault(e =>
				e.Value != null &&
				e.Value.ObjectId == item.ObjectId);

		if (equippedEntry.Value == null)
		{
			message = "Only equipped items can be transcended.";
			return false;
		}

		if (!IsAllowedEquipSlot(equippedEntry.Key))
		{
			message = "This equipment cannot be transcended.";
			return false;
		}

		var currentStage = (int)item.Properties.GetFloat(
			PropertyName.Transcend,
			0);

		if (currentStage >= MaxTranscendStage)
		{
			message = "This item is already at max transcendence.";
			return false;
		}

		var nextStage = currentStage + 1;
		var requiredGems = GetRequiredBlessedGems(nextStage);

		var blessedGem = character.Inventory
			.GetItems()
			.Values
			.FirstOrDefault(inventoryItem =>
				inventoryItem != null &&
				inventoryItem.Id == BlessedGemItemId &&
				inventoryItem.Amount >= requiredGems);

		if (blessedGem == null)
		{
			message = $"Not enough Blessed Gems. Required: {requiredGems}.";
			return false;
		}

		var removeResult = character.Inventory.Remove(
			blessedGem.ObjectId,
			requiredGems,
			InventoryItemRemoveMsg.Used);

		if (removeResult != InventoryResult.Success)
		{
			message = "Failed to consume the required Blessed Gems.";
			return false;
		}

		item.Properties.SetFloat(
			PropertyName.Transcend,
			nextStage);

		Send.ZC_OBJECT_PROPERTY(
			character.Connection,
			item,
			PropertyName.Transcend);

		character.InvalidateProperties();

		Send.ZC_OBJECT_PROPERTY(character);

		Send.ZC_ITEM_EQUIP_LIST(character);

		character.AddonMessage("ITEM_PROP_UPDATE");
		character.AddonMessage("EQUIP_ITEM_LIST_UPDATE");

		message =
			$"Transcendence successful: {item.Data.ClassName} " +
			$"{currentStage} -> {nextStage}. " +
			$"Blessed Gems used: {requiredGems}.";

		return true;
	}

	public static int GetRequiredBlessedGems(int stage)
	{
		return stage switch
		{
			1 => 1,
			2 => 3,
			3 => 6,
			4 => 11,
			5 => 19,
			6 => 32,
			7 => 53,
			8 => 87,
			9 => 142,
			10 => 231,
			_ => 0,
		};
	}
}
