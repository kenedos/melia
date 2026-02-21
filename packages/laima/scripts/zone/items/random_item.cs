//--- Melia Script ----------------------------------------------------------
// EXP Card Items
//--- Description -----------------------------------------------------------
// Item-related scripts that grant the user experience points.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;
using Yggdrasil.Util;

public class RandomItemScripts : GeneralScript
{
	[ScriptableFunction]
	public ItemUseResult SCR_USE_STRING_GIVE_RANDOM_ITEM_NUMBER_SPLIT(Character character, Item item, string items, float numArg1, float numArg2)
	{
		var splitItems = items.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
		if (splitItems.Length == 0)
		{
			Log.Warning("SCR_USE_STRING_GIVE_RANDOM_ITEM_NUMBER_SPLIT: No valid items in {0}.", items);
			return ItemUseResult.Fail;
		}

		var randomIndex = RandomProvider.Get().Next(splitItems.Length);
		var itemString = splitItems[randomIndex];
		var itemDataSplit = itemString.Split('/');
		var itemClassName = itemDataSplit[0];
		var itemAmount = 1;

		if (string.IsNullOrWhiteSpace(itemClassName))
		{
			Log.Warning("SCR_USE_STRING_GIVE_RANDOM_ITEM_NUMBER_SPLIT: Unable to find an item in {0}.", items);
			return ItemUseResult.Fail;
		}

		if (itemDataSplit.Length > 1 && !int.TryParse(itemDataSplit[1], out itemAmount))
		{
			Log.Warning("SCR_USE_STRING_GIVE_RANDOM_ITEM_NUMBER_SPLIT: Unable to parse item amount in {0} - {1}.", items, itemDataSplit[1]);
			return ItemUseResult.Fail;
		}

		character.AddItem(itemClassName, itemAmount);

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_STRING_GIVE_RANDOM_ITEM_NUMBER_SPLIT_CNTCOST(Character character, Item item, string items, float requiredAmount, float numArg2)
	{
		if (item.Amount < requiredAmount)
		{
			character.AddonMessage("NOTICE_Dm_Clear", $"You need {(int)requiredAmount}x {item.Data.Name} to use this item. ({item.Amount}/{(int)requiredAmount})", 3);
			return ItemUseResult.Fail;
		}

		var splitItems = items.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
		if (splitItems.Length == 0)
		{
			Log.Warning("SCR_USE_STRING_GIVE_RANDOM_ITEM_NUMBER_SPLIT_CNTCOST: No valid items in {0}.", items);
			return ItemUseResult.Fail;
		}

		var randomIndex = RandomProvider.Get().Next(splitItems.Length);
		var itemString = splitItems[randomIndex];
		var itemDataSplit = itemString.Split('/');
		var itemClassName = itemDataSplit[0];
		var itemAmount = 1;

		if (string.IsNullOrWhiteSpace(itemClassName))
		{
			Log.Warning("SCR_USE_STRING_GIVE_RANDOM_ITEM_NUMBER_SPLIT_CNTCOST: Unable to find an item in {0}.", items);
			return ItemUseResult.Fail;
		}

		if (itemDataSplit.Length > 1 && !int.TryParse(itemDataSplit[1], out itemAmount))
		{
			Log.Warning("SCR_USE_STRING_GIVE_RANDOM_ITEM_NUMBER_SPLIT_CNTCOST: Unable to parse item amount in {0} - {1}.", items, itemDataSplit[1]);
			return ItemUseResult.Fail;
		}

		character.Inventory.Remove(item.ObjectId, (int)requiredAmount, InventoryItemRemoveMsg.Used);
		character.AddItem(itemClassName, itemAmount);

		return ItemUseResult.OkayNotConsumed;
	}
}
