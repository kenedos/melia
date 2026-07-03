//--- Melia Script ----------------------------------------------------------
// Transcending Items
//--- Description -----------------------------------------------------------
// Item-related scripts that transcend equipment.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

public class TranscendingItemScripts : GeneralScript
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

	[ScriptableFunction]
	public DialogTxResult SCR_ITEM_TRANSCEND(Character character, DialogTxArgs args)
	{
		if (args.TxItems == null || args.TxItems.Length < 1)
			return DialogTxResult.Fail;

		var item = args.TxItems[0].Item;

		if (item == null)
			return DialogTxResult.Fail;

		TranscendenceHelper.TryTranscendItem(character, item, out var message);
		character.ServerMessage(message);

		return DialogTxResult.Okay;
	}

	[ScriptableFunction("SCR_ITEM_TRANSCEND_TX")]
	public DialogTxResult SCR_ITEM_TRANSCEND_TX(Character character, DialogTxArgs args)
	{
		if (args.TxItems == null || args.TxItems.Length < 1)
			return DialogTxResult.Fail;

		var item = args.TxItems[0].Item;

		if (item == null)
			return DialogTxResult.Fail;

		TranscendenceHelper.TryTranscendItem(character, item, out var message);
		character.ServerMessage(message);

		character.ExecuteClientScript("ui.CloseFrame('itemtranscend');");
		character.ExecuteClientScript("ui.CloseFrame('inventory');");
		character.ExecuteClientScript("control.EnableControl(1);");

		Send.ZC_DIALOG_CLOSE(character.Connection);
		Send.ZC_LEAVE_TRIGGER(character.Connection);

		return DialogTxResult.Okay;
	}

	[ScriptableFunction("SCR_REQ_LEGEND_ITEM_DIALOG")]
	public CustomCommandResult SCR_REQ_LEGEND_ITEM_DIALOG(Character character, int numArg1, int numArg2, int numArg3)
	{
		character.ServerMessage($"SCR_REQ_LEGEND_ITEM_DIALOG args: {numArg1}, {numArg2}, {numArg3}");
		return CustomCommandResult.Okay;
	}
}
