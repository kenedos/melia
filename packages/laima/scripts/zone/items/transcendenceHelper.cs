using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
	public static class TranscendenceHelper
	{
		private const int MaxTranscendStage = 10;
		private const int BlessedGemItemId = 646045;

		public static bool TryTranscendItem(Character character, Item item, out string message)
		{
			message = "";

			if (item == null)
			{
				message = "Invalid item.";
				return false;
			}

			if (item.Data.Type != ItemType.Equip)
			{
				message = "Only equipment can be transcended.";
				return false;
			}

			if (item.ObjectId <= 0)
			{
				message = "Invalid equipment object.";
				return false;
			}

			var currentStage = (int)item.Properties.GetFloat(PropertyName.Transcend, 0);

			if (currentStage >= MaxTranscendStage)
			{
				message = "This item is already at max transcendence.";
				return false;
			}

			var nextStage = currentStage + 1;
			var requiredGems = GetRequiredBlessedGems(nextStage);

			var blessedGem = character.Inventory.GetItems().Values
				.FirstOrDefault(inventoryItem =>
					inventoryItem != null &&
					inventoryItem.Id == BlessedGemItemId &&
					inventoryItem.Amount >= requiredGems);

			if (blessedGem == null)
			{
				message = $"Not enough Blessed Gems. Required: {requiredGems}.";
				return false;
			}

			character.Inventory.Remove(blessedGem.ObjectId, requiredGems, InventoryItemRemoveMsg.Used);

			item.Properties.SetFloat(PropertyName.Transcend, nextStage);
			item.Properties.InvalidateAll();

			Send.ZC_OBJECT_PROPERTY(character, item);

			if (character.Inventory.GetEquip().Values.Any(equip => equip.ObjectId == item.ObjectId))
				character.InvalidateProperties();

			character.AddonMessage("INV_ITEM_LIST_GET");
			character.AddonMessage("EQUIP_ITEM_LIST_UPDATE");

			message = $"Transcendence successful: {item.Data.ClassName} {currentStage} -> {nextStage}. Blessed Gems used: {requiredGems}.";
			return true;
		}

		public static int GetRequiredBlessedGems(int stage)
		{
			return stage switch
			{
				1 => 1,
				2 => 2,
				3 => 4,
				4 => 8,
				5 => 16,
				6 => 32,
				7 => 64,
				8 => 128,
				9 => 256,
				10 => 512,
				_ => 0,
			};
		}
	}
