//--- Melia Script ----------------------------------------------------------
// Crafting Table Scripts
//--- Description -----------------------------------------------------------
// Handles "Dialog TX" requests from the client.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using Yggdrasil.Util.Commands;
using static Melia.Zone.Scripting.Shortcuts;

public class CraftingTableFunctionsScript : GeneralScript
{
	protected override void Load()
	{
		base.Load();

		if (!Feature.IsEnabled(FeatureId.CraftingTable))
			return;
		AddChatCommand("craftingtable", "", "Opens the crafting table.", 0, 99, HandleCraftingTable);
	}

	private CommandResult HandleCraftingTable(Character sender, Character target, string message, string commandName, Arguments args)
	{
		if (!Feature.IsEnabled(FeatureId.CraftingTable))
			return CommandResult.Okay;

		sender.AddonMessage("OPEN_DLG_CRAFTINGTABLE");
		return CommandResult.Okay;
	}

	public enum CraftingTableRecipe
	{
		RerollGem = 1,
		ImproveQuality = 2,
		RerollMods = 3
	}

	[ScriptableFunction]
	public DialogTxResult SCR_CRAFTINGTABLE_CRAFT(Character character, DialogTxArgs args)
	{
		if (!Feature.IsEnabled(FeatureId.CraftingTable))
			return DialogTxResult.Okay;

		if (args.TxItems.Length < 1 || args.NumArgs.Length < 1)
		{
			Log.Debug("SCR_CRAFTINGTABLE_CRAFT: Invalid args. TxItems={0}, NumArgs={1}", args.TxItems.Length, args.NumArgs.Length);
			return DialogTxResult.Fail;
		}

		// NumArgs[0] = recipeId
		var recipeType = (CraftingTableRecipe)args.NumArgs[0];

		switch (recipeType)
		{
			case CraftingTableRecipe.RerollGem:
				return HandleRerollGem(character, args);
			case CraftingTableRecipe.ImproveQuality:
				return HandleImproveQuality(character, args);
			case CraftingTableRecipe.RerollMods:
				return HandleRerollMods(character, args);
			default:
				Log.Debug("SCR_CRAFTINGTABLE_CRAFT: Unknown recipeType {0}", recipeType);
				character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
				return DialogTxResult.Fail;
		}
	}

	private DialogTxResult HandleRerollGem(Character character, DialogTxArgs args)
	{
		// Validate: all items are gems, same ClassName, total >= 3
		string gemClassName = null;
		var totalGemCount = 0;

		foreach (var txItem in args.TxItems)
		{
			var item = txItem.Item;
			if (item.Data.Group != ItemGroup.Gem)
			{
				Log.Warning("HandleRerollGem: User '{0}' sent non-gem item '{1}'.", character.Username, item.Data.ClassName);
				character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
				return DialogTxResult.Fail;
			}

			if (gemClassName == null)
				gemClassName = item.Data.ClassName;

			if (item.Data.ClassName != gemClassName)
			{
				Log.Warning("HandleRerollGem: User '{0}' sent mixed gem types.", character.Username);
				character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
				return DialogTxResult.Fail;
			}

			totalGemCount += txItem.Amount;
		}

		if (totalGemCount < 3 || gemClassName == null)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Fail;
		}

		var sourceGemData = ZoneServer.Instance.Data.ItemDb.FindByClass(gemClassName);
		if (sourceGemData == null)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Fail;
		}

		// Find all gems of the same gemLevel but different className
		var sameTypeGems = ZoneServer.Instance.Data.ItemDb.Entries.Values
			.Where(i => i.Group == ItemGroup.Gem
				&& i.GemLevel == sourceGemData.GemLevel
				&& i.ClassName != gemClassName)
			.ToList();

		if (sameTypeGems.Count == 0)
		{
			character.ServerMessage("No other gems of this level exist.");
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Fail;
		}

		// Remove 3 gems from inventory
		var gemsToRemove = 3;
		foreach (var txItem in args.TxItems)
		{
			var removeCount = Math.Min(txItem.Amount, gemsToRemove);
			character.Inventory.Remove(txItem.Item.ObjectId, removeCount, InventoryItemRemoveMsg.Given);
			gemsToRemove -= removeCount;
			if (gemsToRemove <= 0)
				break;
		}

		// Pick random gem from candidates
		var rnd = RandomProvider.Get();
		var resultGem = sameTypeGems[rnd.Next(sameTypeGems.Count)];

		character.AddItem(resultGem.ClassName, 1);

		Send.ZC_ITEM_INVENTORY_DIVISION_LIST(character);
		character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_SUCCESS, resultGem.ClassName);

		return DialogTxResult.Okay;
	}

	private DialogTxResult HandleImproveQuality(Character character, DialogTxArgs args)
	{
		// Validate: 1 equipment + 40x Nucle Powder (misc_ore22)
		DialogTxItem equipTxItem = null;
		DialogTxItem powderTxItem = null;

		foreach (var txItem in args.TxItems)
		{
			if (txItem.Item.Data.ClassName == "misc_ore22")
				powderTxItem = txItem;
			else if (txItem.Item.Data.Type == ItemType.Equip)
				equipTxItem = txItem;
		}

		if (equipTxItem == null || powderTxItem == null || powderTxItem.Amount < 40)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Fail;
		}

		var equipItem = equipTxItem.Item;
		var currentGrade = (int)equipItem.Properties.GetFloat(PropertyName.ItemGrade);

		if (currentGrade >= (int)ItemGrade.Goddess)
		{
			character.ServerMessage("Item is already at maximum grade.");
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Fail;
		}

		// Remove 40 Nucle Powder
		character.Inventory.Remove(powderTxItem.Item.ObjectId, 40, InventoryItemRemoveMsg.Given);

		// Increase grade by 1
		var newGrade = currentGrade + 1;
		equipItem.Properties.SetFloat(PropertyName.ItemGrade, newGrade);
		equipItem.Properties.SetFloat(PropertyName.NeedRandomOption, 1);
		equipItem.GenerateGradeBasedRandomOptions();

		Send.ZC_OBJECT_PROPERTY(character, equipItem);
		Send.ZC_ITEM_INVENTORY_DIVISION_LIST(character);
		character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_SUCCESS, equipItem.Data.ClassName);

		return DialogTxResult.Okay;
	}

	private DialogTxResult HandleRerollMods(Character character, DialogTxArgs args)
	{
		// Validate: 1 equipment + 40x Sierra Powder (misc_ore23)
		DialogTxItem equipTxItem = null;
		DialogTxItem powderTxItem = null;

		foreach (var txItem in args.TxItems)
		{
			if (txItem.Item.Data.ClassName == "misc_ore23")
				powderTxItem = txItem;
			else if (txItem.Item.Data.Type == ItemType.Equip)
				equipTxItem = txItem;
		}

		if (equipTxItem == null || powderTxItem == null || powderTxItem.Amount < 40)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Fail;
		}

		var equipItem = equipTxItem.Item;

		// Remove 40 Sierra Powder
		character.Inventory.Remove(powderTxItem.Item.ObjectId, 40, InventoryItemRemoveMsg.Given);

		// Reroll random options
		equipItem.Properties.SetFloat(PropertyName.NeedRandomOption, 1);
		equipItem.GenerateGradeBasedRandomOptions();

		Send.ZC_OBJECT_PROPERTY(character, equipItem);
		Send.ZC_ITEM_INVENTORY_DIVISION_LIST(character);
		character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_SUCCESS, equipItem.Data.ClassName);

		return DialogTxResult.Okay;
	}
}
