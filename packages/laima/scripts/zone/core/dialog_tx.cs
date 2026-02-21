//--- Melia Script ----------------------------------------------------------
// Dialog Transaction Scripts
//--- Description -----------------------------------------------------------
// Handles "Dialog TX" requests from the client.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class DialogTxFunctionsScript : GeneralScript
{
	[ScriptableFunction]
	public DialogTxResult SCR_TX_REPAIR(Character character, DialogTxArgs args)
	{
		foreach (var txItem in args.TxItems)
		{
			var item = txItem.Item;

			var price = 100;
			if (ScriptableFunctions.ItemCalc.TryGet("SCR_Get_Item_RepairPrice", out var repairPriceFunc))
			{
				price = (int)repairPriceFunc(item);
			}

			if (character.HasSilver(price))
			{
				character.RemoveItem(ItemId.Vis, price);
				item.ModifyDurability(character);
			}
		}

		character.AddonMessage(AddonMessage.UPDATE_DLG_REPAIR);

		return DialogTxResult.Okay;
	}

	[ScriptableFunction]
	public DialogTxResult SCR_TX_REPAIR_ALL(Character character, DialogTxArgs args)
	{
		if (args.TxItems.Length < 2)
			return DialogTxResult.Fail;

		var repairItem = args.TxItems[0].Item;

		return DialogTxResult.Okay;
	}

	[ScriptableFunction]
	public DialogTxResult SCR_ITEM_EXP_UP(Character character, DialogTxArgs args)
	{
		if (args.TxItems.Length < 2)
			return DialogTxResult.Fail;

		if (character.IsJoinColonyWarMap)
			return DialogTxResult.Fail;

		if (character.IsIndun || character.IsMissionInst)
		{
			character.SystemMessage("CannotCraftInIndun");
			return DialogTxResult.Fail;
		}

		if (!character.IsSitting)
		{
			character.SystemMessage("AvailableOnlyWhileResting");
			return DialogTxResult.Fail;
		}

		var tgtItem = args.TxItems[0].Item;
		var tgtGroup = tgtItem.Data.Group;
		var tgtEquipXpGroup = tgtItem.Data.EquipExpGroup;

		if (tgtEquipXpGroup == EquipExpGroup.None)
			return DialogTxResult.Fail;

		foreach (var txItem in args.TxItems)
		{
			if (txItem.Item.IsExpired)
			{
				character.SystemMessage("CannotUseLifeTimeOverItem");
				return DialogTxResult.Fail;
			}
		}

		var makingTime = 5;
		character.AddonMessage(AddonMessage.ITEM_EXP_START, "", makingTime);

		var sessionObject = character.AddSessionObject(SessionObjectId.TemporaryStopEvent);
		sessionObject.Properties.SetFloat(PropertyName.Step1, 5500);
		Send.ZC_OBJECT_PROPERTY(character, sessionObject, PropertyName.Step1);
		Send.ZC_CANCEL_MOUSE_MOVE(character);

		character.TimeActions.Start(ScpArgMsg("ItemCraftProcess"), "None", "UPGRADEGEM", TimeSpan.FromSeconds(makingTime), (Character character, TimeAction timeAction) =>
		{
			var timeActionResult = timeAction.Result;
			var stopEvent = character.SessionObjects.Get(SessionObjectId.TemporaryStopEvent);
			stopEvent.Properties.SetFloat(PropertyName.Goal1, timeActionResult == TimeActionResult.Completed ? 1 : 0);
			Send.ZC_OBJECT_PROPERTY(character, stopEvent, PropertyName.Goal1);
			character.RemoveSessionObject(SessionObjectId.TemporaryStopEvent);
			if (timeActionResult != TimeActionResult.Completed || args.TxItems == null || args.TxItems.Length < 2 || tgtItem.IsLocked)
			{
				character.AddonMessage(AddonMessage.ITEM_EXP_STOP);
				return;
			}

			var beforeItemExp = (int)tgtItem.Properties[PropertyName.ItemExp];

			tgtItem.GetItemLevelExp(out var lv, out var curExp, out var maxExp);

			var totalPoint = 0;

			foreach (var txItem in args.TxItems.Skip(1))
			{
				var materialItem = txItem.Item;
				var materialItemCount = txItem.Amount;

				if (tgtItem.ObjectId == materialItem.ObjectId || materialItem.IsLocked)
					return;
				if (tgtItem.IsExpired || materialItem.IsExpired)
					return;

				var exp = materialItem.GetMixMaterialExp();
				var price = materialItemCount * materialItem.GetMaterialPrice();
				totalPoint += materialItemCount * exp;

				character.Inventory.Remove(materialItem.ObjectId, materialItemCount, InventoryItemRemoveMsg.Given, reason: "ItemExp");
			}

			var multiplier = 1;
			var totalExp = (totalPoint * multiplier) + beforeItemExp;

			tgtItem.GetItemLevelExp(totalExp, out var totalLevel, out var cur, out var max);

			if (tgtGroup == ItemGroup.Card)
				tgtItem.Properties[PropertyName.CardLevel] = totalLevel;
			else if (tgtGroup == ItemGroup.Gem)
			{
				tgtItem.Properties[PropertyName.Level] = totalLevel;
				tgtItem.UpdateGemStatOptions();
			}

			var newItemList = new List<string>();
			var newItemExp = new List<int>();

			tgtItem.Properties[PropertyName.ItemExp] = totalExp;

			character.AddonMessage("ITEM_EXPUP_END", multiplier.ToString(), totalPoint);

			// Update gem level on client
			Send.ZC_OBJECT_PROPERTY(character, tgtItem);
			Send.ZC_ITEM_INVENTORY_DIVISION_LIST(character);
			Send.ZC_EQUIP_GEM_INFO(character);

			if (tgtGroup == ItemGroup.Card)
			{
				if (character.AdventureBook.IsNewEntry(AdventureBookType.ItemCrafted, tgtItem.Id))
					character.AddonMessage(AddonMessage.ADVENTURE_BOOK_NEW, tgtItem.Data.Name);
				// Card Level
				character.AdventureBook.AddItemCrafted(tgtItem.Id, totalLevel);
			}
			else if (tgtGroup == ItemGroup.Gem)
			{
				if (character.AdventureBook.IsNewEntry(AdventureBookType.ItemCrafted, tgtItem.Id))
					character.AddonMessage(AddonMessage.ADVENTURE_BOOK_NEW, tgtItem.Data.Name);
				// Gem Level
				character.AdventureBook.AddItemCrafted(tgtItem.Id, totalLevel);
			}
		});

		return DialogTxResult.Okay;
	}

	[ScriptableFunction]
	public DialogTxResult SCR_ITEM_MANUFACTURE_RECIPE(Character character, DialogTxArgs args)
	{
		if (args.NumArgs.Length < 2)
			return DialogTxResult.Fail;

		var recipeId = args.NumArgs[0];
		var craftAmount = args.NumArgs[1];

		if (!ZoneServer.Instance.Data.RecipeDb.TryFind(recipeId, out var recipe)
			|| !ZoneServer.Instance.Data.ItemDb.TryFind(a => a.ClassName == recipe.ProductClassName, out var itemData)
			|| craftAmount <= 0)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "", 0);
			return DialogTxResult.Fail;
		}

		character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_START, recipe.ClassName);

		var sessionObject = character.AddSessionObject(SessionObjectId.TemporaryStopEvent);
		sessionObject.Properties.SetFloat(PropertyName.Step1, 8500);
		Send.ZC_OBJECT_PROPERTY(character, sessionObject, PropertyName.Step1);
		Send.ZC_CANCEL_MOUSE_MOVE(character);
		character.TimeActions.Start(ScpArgMsg("ItemCraftProcess"), "None", "CRAFT", TimeSpan.FromSeconds(8), (Character character, TimeAction timeAction) =>
		{
			if (timeAction.Result == TimeActionResult.Completed)
			{
				character?.RemoveSessionObject(SessionObjectId.TemporaryStopEvent);

				for (var i = 0; i < craftAmount; i++)
				{
					foreach (var txItem in args.TxItems)
					{
						var item = txItem.Item;
						var amount = txItem.Amount;

						if (recipe.Ingredients.Exists(a => a.ClassName == item.Data.ClassName && amount < a.Amount))
						{
							character?.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, recipe.ClassName);
							return;
						}
					}

					var hasAllItems = true;
					foreach (var txItem in args.TxItems)
					{
						if (character == null || !character.Inventory.HasItem(txItem.Item.Id, txItem.Amount))
							hasAllItems = false;
					}

					if (!hasAllItems)
					{
						character?.RemoveSessionObject(SessionObjectId.TemporaryStopEvent);
						character?.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, recipe.ClassName);
						break;
					}

					foreach (var txItem in args.TxItems)
					{
						character?.Inventory.Remove(txItem.Item.ObjectId, txItem.Amount, InventoryItemRemoveMsg.Given);
					}

					var craftedItem = new Item(itemData.Id, recipe.ProductAmount);
					if (args.StrArgs.Length == 2)
					{
						var customName = args.StrArgs[0];
						var memo = args.StrArgs[1];
						craftedItem.Properties.SetString(PropertyName.CustomName, customName);
						craftedItem.Properties.SetString(PropertyName.Memo, memo);
					}
					craftedItem.Properties.SetString(PropertyName.Maker, character?.Name);

					// Improves item grade randomly
					var grade = (int)craftedItem.Data.Grade;
					// Make sure we don't accidentally divide by 0
					grade = (int)Math.Max(1, grade);
					while (grade < (int)ItemGrade.Goddess)
					{
						var chanceToIncreaseGrade = 0.5f / grade;

						if (RandomProvider.Get().NextDouble() < chanceToIncreaseGrade)
							grade++;
						else
							break;
					}

					craftedItem.Properties.SetFloat(PropertyName.ItemGrade, grade);
					craftedItem.Properties.SetFloat(PropertyName.NeedRandomOption, 1);
					craftedItem.GenerateGradeBasedRandomOptions();
					character?.Inventory.Add(craftedItem);
				}

				Send.ZC_ITEM_INVENTORY_DIVISION_LIST(character);

				character?.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_SUCCESS, recipe.ClassName, craftAmount);
			}
			else
			{
				character?.RemoveSessionObject(SessionObjectId.TemporaryStopEvent);
				character?.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, recipe.ClassName);
			}
			character?.PlayAnimation(AnimationName.Rest);
		});
		Task.Delay(1000)
			.ContinueWith(_ =>
			{
				var evStopObject = character?.SessionObjects.GetOrCreate(SessionObjectId.TemporaryStopEvent);
				evStopObject?.Properties.SetFloat(PropertyName.Goal1, 1);
				Send.ZC_OBJECT_PROPERTY(character, evStopObject, PropertyName.Goal1);
			});

		return DialogTxResult.Okay;
	}

	[ScriptableFunction]
	public DialogTxResult SCR_PUT_COLLECTION(Character character, DialogTxArgs args)
	{
		var collectionId = args.NumArgs[0];
		var item = args.TxItems[0].Item;

		if (!character.Collections.RegisterItem(collectionId, item.Id))
			return DialogTxResult.Fail;

		character.Inventory.Remove(item, 1, InventoryItemRemoveMsg.Destroyed);

		// This is necessary for the collection to go through on the front end
		Send.ZC_ITEM_INVENTORY_DIVISION_LIST(character);
		Send.ZC_NORMAL.UpdateCollection(character, collectionId, item.Id);

		return DialogTxResult.Okay;


	}

	[ScriptableFunction]
	public DialogTxResult SCR_MAKE_SOCKET(Character character, DialogTxArgs args)
	{
		var txItems = args.TxItems;
		if (txItems == null)
		{
			Log.Warning("SCR_MAKE_SOCKET: User '{0}' sent no items.", character.Username);
			return DialogTxResult.Fail;
		}

		foreach (var txItem in txItems)
		{
			var targetItem = txItem.Item;
			if (targetItem == null)
			{
				Log.Warning("SCR_MAKE_SOCKET: User '{0}' sent no item.", character.Username);
				return DialogTxResult.Fail;
			}

			var itemData = targetItem.Data;

			if (itemData == null)
			{
				character.AddonMessage("NOTICE_Dm_scroll", "CHATHEDRAL53_MQ03_ITEM02", 3);
				return DialogTxResult.Fail;
			}

			if (targetItem.NeedsAppraisal)
			{
				character.AddonMessage("NOTICE_Dm_scroll", "CHATHEDRAL53_MQ03_ITEM02", 3);
				return DialogTxResult.Fail;
			}

			if (targetItem.IsLocked)
			{
				character.SystemMessage("MaterialItemIsLockEither");
				return DialogTxResult.Fail;
			}

			if (itemData.Type != ItemType.Equip)
			{
				character.SystemMessage("GemNotEquip");
				return DialogTxResult.Fail;
			}

			if (targetItem.Potential <= 0)
			{
				character.SystemMessage("NoMorePotential");
				return DialogTxResult.Fail;
			}

			var maxSockets = targetItem.MaxSockets;
			var socketsUsed = targetItem.GetUsedSockets();
			var nextFreeSocket = targetItem.GetNextFreeSocket();

			if (maxSockets - socketsUsed <= 0)
			{
				character.SystemMessage("ThisItemCannotPlusSocket");
				return DialogTxResult.Fail;
			}

			if (socketsUsed >= maxSockets)
			{
				character.SystemMessage("ALREADY_MAX_SOCKET");
				return DialogTxResult.Fail;
			}

			targetItem.Properties.TryGetFloat(PropertyName.UseLv, out var lv);
			targetItem.Properties.TryGetFloat(PropertyName.ItemGrade, out var grade);
			var price = 100;
			if (ScriptableFunctions.ItemCalc.TryGet("SCR_Get_Item_SocketPrice", out var socketPriceFunc))
			{
				price = (int)socketPriceFunc(targetItem);
			}

			if (!character.HasSilver(price))
				return DialogTxResult.Fail;

			targetItem.Properties.Modify(PropertyName.PR, -1);

			if (price > 0)
				character.RemoveItem(ItemId.Vis, price);

			targetItem.CreateSocket(nextFreeSocket);
			Send.ZC_OBJECT_PROPERTY(character, targetItem);
			Send.ZC_EQUIP_GEM_INFO(character);
		}

		character.AddonMessage(AddonMessage.MSG_MAKE_ITEM_SOCKET);
		character.InvalidateProperties();

		return DialogTxResult.Okay;
	}

	[ScriptableFunction]
	public DialogTxResult SCR_REMOVE_GEM(Character character, DialogTxArgs args)
	{
		var selectedSlot = args.NumArgs[0];

		var txItems = args.TxItems;
		if (txItems == null)
		{
			Log.Warning("SCR_REMOVE_GEM: User '{0}' sent no items.", character.Username);
			return DialogTxResult.Fail;
		}

		foreach (var txItem in txItems)
		{
			var targetItem = txItem.Item;
			if (targetItem == null)
			{
				Log.Warning("SCR_REMOVE_GEM: User '{0}' sent no item.", character.Username);
				return DialogTxResult.Fail;
			}

			if (targetItem.IsLocked)
			{
				character.SystemMessage("MaterialItemIsLockEither");
				return DialogTxResult.Fail;
			}

			if (targetItem.Data.Type != ItemType.Equip)
			{
				character.SystemMessage("GemNotEquip");
				return DialogTxResult.Fail;
			}

			var gemToExtract = targetItem.GetGemAtSocket(selectedSlot);
			if (gemToExtract == null)
			{
				character.SystemMessage("IS_HAVENT_GEM");
				return DialogTxResult.Fail;
			}

			var price = 10000;
			if (ZoneServer.Instance.Data.SocketPriceDb.TryFind(targetItem.UseLevel, out var socketPrice))
				price = socketPrice.RemovePrice;

			if (!character.HasSilver(price))
				return DialogTxResult.Fail;

			if (price > 0)
				character.RemoveItem(ItemId.Vis, price);

			if (targetItem.Properties.TryGetFloat($"Socket_Equip_{selectedSlot}", out _))
				targetItem.Properties.SetFloat($"Socket_Equip_{selectedSlot}", 0);

			if (targetItem.Properties.TryGetFloat($"SocketItemExp_{selectedSlot}", out _))
				targetItem.Properties.SetFloat($"SocketItemExp_{selectedSlot}", 0);

			if (targetItem.Properties.TryGetFloat($"Socket_JamLv_{selectedSlot}", out _))
				targetItem.Properties.SetFloat($"Socket_JamLv_{selectedSlot}", 0);

			if (targetItem.Properties.TryGetFloat($"Socket_GemBelongingCount_{selectedSlot}", out _))
				targetItem.Properties.SetFloat($"Socket_GemBelongingCount_{selectedSlot}", 0);

			targetItem.RemoveGemAtSocket(selectedSlot);
			var prevLv = gemToExtract.GemLevel - 1;
			Math.Max(prevLv, 0);

			if (prevLv >= 1)
			{
				var prevExp = ZoneServer.Instance.Data.ItemExpDb.GetTotalExp(gemToExtract.Data.EquipExpGroup, prevLv);
				gemToExtract.Properties.SetFloat(PropertyName.ItemExp, prevExp);
				gemToExtract.Properties.SetFloat(PropertyName.Level, prevLv);

				if (gemToExtract.Properties.TryGetFloat(PropertyName.GemRoastingLv, out var gemRoastingLv))
					gemToExtract.Properties.SetFloat(PropertyName.GemRoastingLv, gemRoastingLv);

				gemToExtract.UpdateGemStatOptions();

				character.Inventory.Add(gemToExtract);
			}

			character.InvalidateProperties(targetItem);

			Item invItem = null;
			if (invItem != null)
			{
				//GameFunctions.ItemExpMongoLog(character, "ExpDown", invItem, -prevExp, gemExp, gemExp - prevExp);
			}

			if (prevLv != 1)
			{
				//GameFunctions.SocketMongoLog(character, "GemUnequip", newGemItemName, prevLv, gemExp, selectedSlot, newGemItemName, targetItem);
			}
			else
			{
				//GameFunctions.SocketMongoLog(character, "GemDestroy", newGemItemName, prevLv, gemExp, selectedSlot, newGemItemName, targetItem);
			}

		}

		character.AddonMessage(AddonMessage.MSG_REMOVE_GEM);
		character.InvalidateProperties();

		return DialogTxResult.Okay;
	}

	[ScriptableFunction("GODDESS_REINFORCE")]
	public DialogTxResult GODDESS_REINFORCE(Character character, DialogTxArgs args)
	{
		if (args.TxItems.Length < 2)
			return DialogTxResult.Fail;

		var equipItem = args.TxItems[0];
		var materialItem = args.TxItems[1];

		// TODO: Validate items with a goddess equip upgrade db
		character.Inventory.Remove(materialItem.Item.ObjectId, materialItem.Amount, InventoryItemRemoveMsg.Given);
		equipItem.Item.Properties.Modify(PropertyName.Reinforce_2, 1);

		Send.ZC_OBJECT_PROPERTY(character, equipItem.Item);
		character.AddonMessage(AddonMessage.MSG_SUCCESS_GODDESS_REINFORCE_EXEC, "1", 1);

		return DialogTxResult.Okay;
	}

	[ScriptableFunction("SCR_ITEM_APPRAISAL")]
	public DialogTxResult APPRAISAL(Character character, DialogTxArgs args)
	{
		var appraisalPriceFunc = ScriptableFunctions.ItemCalc.Get("SCR_Get_Item_AppraisalPrice");
		foreach (var txItem in args.TxItems)
		{
			var item = txItem.Item;

			if (item == null)
			{
				character.SystemMessage("DataError");
				return DialogTxResult.Fail;
			}

			var price = (int)appraisalPriceFunc(item);

			if (!character.HasSilver(price))
				return DialogTxResult.Fail;

			Send.ZC_ENABLE_CONTROL(character.Connection, "APPRAISER", false);
			Send.ZC_LOCK_KEY(character, "APPRAISER", true);

			Task.Delay(1500).ContinueWith(t =>
			{
				character.RemoveItem(ItemId.Vis, price);
				Send.ZC_ENABLE_CONTROL(character.Connection, "APPRAISER", true);
				Send.ZC_LOCK_KEY(character, "APPRAISER", false);
				item.Appraisal();
				if (Feature.IsEnabled("GradeRandomOptions"))
				{
					item.Properties.SetFloat(PropertyName.NeedRandomOption, 1);
					item.GenerateGradeBasedRandomOptions();
				}
				else
					item.GenerateRandomOptions();
				Send.ZC_OBJECT_PROPERTY(character, item);
				character.AddonMessage(AddonMessage.SUCCESS_APPRALSAL);
				character.AddonMessage(AddonMessage.UPDATE_ITEM_APPRAISAL, "Equip");
				Send.ZC_NORMAL.ShowItemBalloon(character, item);
			});
		}

		return DialogTxResult.Okay;
	}

	[ScriptableFunction]
	public DialogTxResult SCR_ITEM_DECOMPOSE(Character character, DialogTxArgs args)
	{
		var itemString = new StringBuilder();
		var totalSilver = 0;
		foreach (var txItem in args.TxItems)
		{
			var item = txItem.Item;
			if (item == null)
			{
				character.SystemMessage("DataError");
				return DialogTxResult.Fail;
			}
			if (character.Inventory.Remove(item.ObjectId, 1, silently: true) == InventoryResult.Success)
			{
				// TODO: Check with Ken. No clue if transcend is implemented.
				var transcendBonus = 0;
				itemString.Append($"\"{item.Data.ClassName}/{item.Amount}/{transcendBonus}\",");
				totalSilver += item.Data.SellPrice;
			}
		}
		itemString.Length--;
		Send.ZC_ITEM_INVENTORY_DIVISION_LIST(character);
		character.InvalidateProperties(PropertyName.NowWeight, PropertyName.MSPD);
		character.AddItem(ItemId.Vis, totalSilver);
		character.ExecuteClientScript($"ITEM_DECOMPOSE_COMPLETE({itemString})");
		return DialogTxResult.Okay;
	}

	[ScriptableFunction("SCR_REVERT_ITEM_OPTION")]
	public DialogTxResult REVERT_ITEM_OPTION(Character character, DialogTxArgs args)
	{
		foreach (var txItem in args.TxItems)
		{
			var item = txItem.Item;

			item.GenerateRandomOptions();
			Send.ZC_OBJECT_PROPERTY(character, item);
			character.AddonMessage(AddonMessage.MSG_SUCCESS_FREE_RANDOM_OPTION);
			character.AddonMessage(AddonMessage.UPDATE_ITEM_APPRAISAL, "Equip");
		}

		return DialogTxResult.Okay;
	}

	[ScriptableFunction]
	public DialogTxResult MULTIPLE_USE_ABILITY(Character character, DialogTxArgs args)
	{
		var amount = 0;
		foreach (var txItem in args.TxItems)
		{
			var item = txItem.Item;
			if (item == null)
			{
				character.SystemMessage("DataError");
				return DialogTxResult.Fail;
			}

			if (character.Inventory.Remove(item.ObjectId, txItem.Amount) != InventoryResult.Success)
				return DialogTxResult.Fail;

			if (item.Data.Script.StrArg == "AbilityPoint")
				amount += (int)item.Data.Script.NumArg1 * txItem.Amount;
		}

		if (amount > 0)
			character.ModifyAbilityPoints(amount);


		return DialogTxResult.Okay;
	}

	[ScriptableFunction("TX_EVENT_SHOP_1_THREAD1")]
	public DialogTxResult EVENT_SHOP_1_THREAD1(Character character, DialogTxArgs args)
	{
		if (args.NumArgs.Length < 2 && args.StrArgs.Length < 1)
			return DialogTxResult.Fail;

		var shopItemId = args.NumArgs[0];
		var shopItemAmount = args.NumArgs[1];
		var shopType = args.StrArgs[0];

		if (!ZoneServer.Instance.Data.TradeShopDb.TryFind(shopItemId, out var shopItem))
		{
			Log.Warning("EVENT_SHOP_1_THREAD1: User '{0}' tried to trade for an item that doesn't exist with id {1}.", character.Username, shopItemId);
			return DialogTxResult.Fail;
		}

		if (shopItem.ShopType != shopType)
		{
			Log.Debug("EVENT_SHOP_1_THREAD1: User '{0}' tried to trade for an item that didn't match the trade shop type: {1}.", character.Username, shopType);
			return DialogTxResult.Fail;
		}

		foreach (var txItem in args.TxItems)
		{
			var item = txItem.Item;
			var amount = txItem.Amount;

			if (!shopItem.RequiredItems.ContainsKey(item.Data.Id) || amount < (shopItemAmount * shopItem.RequiredItems[item.Data.Id]))
				return DialogTxResult.Fail;
		}

		foreach (var txItem in args.TxItems)
		{
			character.Inventory.Remove(txItem.Item.ObjectId, txItem.Amount * shopItemAmount, InventoryItemRemoveMsg.Given);
		}

		character.AddItem(shopItem.ItemCraftedClassName, shopItem.ItemCraftedCount * shopItemAmount);
		character.AddonMessage(AddonMessage.EARTHTOWERSHOP_BUY_ITEM, shopItem.ItemCraftedClassName);
		character.AddonMessage(AddonMessage.EARTHTOWERSHOP_BUY_ITEM_RESULT, shopItem.ShopType + "/0/0");

		return DialogTxResult.Okay;
	}

	[ScriptableFunction]
	public static DialogTxResult MULTIPLE_USE_STRING_GIVE_ITEM_NUMBER_SPLIT(Character character, DialogTxArgs args)
	{
		if (args.TxItems.Length < 1)
			return DialogTxResult.Fail;

		var hasAllItems = true;
		var itemsToRemove = new Dictionary<string, int>();
		var itemsToAdd = new Dictionary<string, int>();
		foreach (var txItem in args.TxItems)
		{
			var newItems = txItem.Item.Data.Script.StrArg.Split(";");
			foreach (var item in newItems)
			{
				if (string.IsNullOrEmpty(item))
					continue;
				var itemData = item.Split("/");
				var newItemClassName = itemData[0];
				if (!int.TryParse(itemData[1], out var newItemAmount))
				{
					Log.Debug("MULTIPLE_USE_STRING_GIVE_ITEM_NUMBER_SPLIT: User '{0}' unable to parse {1}.", character.Username, itemData[1]);
					return DialogTxResult.Fail;
				}

				var data = ZoneServer.Instance.Data.ItemDb.FindByClass(newItemClassName);
				var requiredItems = data.Script.StrArg.Split(";");

				foreach (var requiredItem in requiredItems)
				{
					if (string.IsNullOrEmpty(requiredItem))
						continue;
					var requiredItemData = requiredItem.Split("/");
					var requiredItemClassName = requiredItemData[0];
					if (!int.TryParse(requiredItemData[1], out var requireItemAmount))
					{
						Log.Debug("MULTIPLE_USE_STRING_GIVE_ITEM_NUMBER_SPLIT: User '{0}' unable to parse {1}.", character.Username, requiredItemData[1]);
						return DialogTxResult.Fail;
					}

					if (!character.HasItem(requiredItemClassName, requireItemAmount))
					{
						hasAllItems = false;
						break;
					}
					itemsToRemove.Add(requiredItemClassName, requireItemAmount);
				}

				if (!hasAllItems)
					break;

				itemsToAdd.Add(newItemClassName, newItemAmount);
			}
		}

		if (!hasAllItems)
			return DialogTxResult.Fail;

		foreach (var itemToRemove in itemsToRemove)
			character.RemoveItem(itemToRemove.Key, itemToRemove.Value);

		foreach (var itemToAdd in itemsToAdd)
			character.AddItem(itemToAdd.Key, itemToAdd.Value);

		return DialogTxResult.Okay;
	}

	[ScriptableFunction("SCR_REGISTER_CABINET_ITEM")]
	public DialogTxResult REGISTER_CABINET_ITEM(Character character, DialogTxArgs args)
	{
		if (args.TxItems.Length == 0 || args.StrArgs.Length < 4)
			return DialogTxResult.Fail;

		foreach (var txItem in args.TxItems)
		{
			if (!character.HasItem(txItem.Item.Id, txItem.Amount))
			{
				Log.Debug("REGISTER_CABINET_ITEM: User '{0}' doesn't have enough of the required item: {1}.", character.Username, txItem.Item.Amount);
				return DialogTxResult.Fail;
			}
		}

		// Weapon
		if (!Enum.TryParse<CabinetType>(args.StrArgs[0], out var type))
			return DialogTxResult.Fail;
		// 1026
		if (!int.TryParse(args.StrArgs[1], out var cabinetId))
			return DialogTxResult.Fail;
		// 2
		if (!int.TryParse(args.StrArgs[2], out var level))
			return DialogTxResult.Fail;

		if (!ZoneServer.Instance.Data.CabinetDb.TryFind(type, cabinetId, out var data))
			return DialogTxResult.Fail;

		if (!string.IsNullOrEmpty(data.UpgradeAccountProperty))
			character.SetAccountProperty(data.UpgradeAccountProperty, level);

		foreach (var txItem in args.TxItems)
			character.Inventory.Remove(txItem.Item.ObjectId, txItem.Amount, InventoryItemRemoveMsg.Given);

		return DialogTxResult.Okay;
	}

	[ScriptableFunction("SCR_ENCHANT_GODDESS_ITEM")]
	public DialogTxResult ENCHANT_GODDESS_ITEM(Character character, DialogTxArgs args)
	{
		if (args.TxItems.Length == 0 || args.StrArgs.Length < 2)
			return DialogTxResult.Fail;

		var item = args.TxItems[0].Item;
		// Weapon
		if (!Enum.TryParse<CabinetType>(args.StrArgs[0], out var type))
			return DialogTxResult.Fail;
		// 1026
		if (!int.TryParse(args.StrArgs[1], out var cabinetId))
			return DialogTxResult.Fail;

		if (!ZoneServer.Instance.Data.CabinetDb.TryFind(type, cabinetId, out var cabinetData))
			return DialogTxResult.Fail;

		if (item == null || item.Properties.Has(PropertyName.InheritanceItemName))
			return DialogTxResult.Fail;

		item.Properties.SetString(PropertyName.InheritanceItemName, cabinetData.ClassName);
		Send.ZC_OBJECT_PROPERTY(character, item);

		character.AddonMessage(AddonMessage.ITEM_CABINET_SUCCESS_ENCHANT);

		return DialogTxResult.Okay;
	}

	[ScriptableFunction("TX_EVENT_TOS_WHOLE_SHOP")]
	public DialogTxResult EVENT_TOS_WHOLE_SHOP(Character character, DialogTxArgs args)
	{
		if (args.NumArgs.Length < 2 && args.TxItems.Length < 1)
			return DialogTxResult.Fail;

		var shopType = "EVENT_TOS_WHOLE_SHOP";
		var shopItemId = args.NumArgs[0];
		var shopItemAmount = args.NumArgs[1];

		if (!ZoneServer.Instance.Data.TradeShopDb.TryFind(shopItemId, out var shopItem))
		{
			Log.Warning("EVENT_TOS_WHOLE_SHOP: User '{0}' tried to trade for an item that doesn't exist with id {1}.", character.Username, shopItemId);
			return DialogTxResult.Fail;
		}

		if (shopItem.ShopType != shopType)
		{
			Log.Debug("EVENT_TOS_WHOLE_SHOP: User '{0}' tried to trade for an item that didn't match the trade shop type: Actual: {0} Expected: {1}.", character.Username, shopItem.ShopType, shopType);
			return DialogTxResult.Fail;
		}

		var totalCoins = character.Connection.Account.Properties[PropertyName.EVENT_TOS_WHOLE_TOTAL_COIN];
		var requiredCoins = shopItem.RequiredItems[ItemId.Tos_Event_Coin] * shopItemAmount;
		if (totalCoins < requiredCoins)
		{
			Log.Debug("EVENT_TOS_WHOLE_SHOP: User '{0}' tried to exchange an item without enough coins: Total Coins {1}, Required Coins {2}.", character.Username, totalCoins, requiredCoins);
			return DialogTxResult.Fail;
		}

		var newCoinAmount = totalCoins - requiredCoins;
		character.ModifyAccountProperty(PropertyName.EVENT_TOS_WHOLE_TOTAL_COIN, -requiredCoins);

		character.ExecuteClientScript(ClientScripts.EARTH_TOWER_SHOP_TRADE_LEAVE);
		character.AddItem(shopItem.ItemCraftedClassName, shopItem.ItemCraftedCount * shopItemAmount);
		character.AddonMessage(AddonMessage.EARTHTOWERSHOP_BUY_ITEM, shopItem.ItemCraftedClassName);
		character.AddonMessage(AddonMessage.EARTHTOWERSHOP_BUY_ITEM_RESULT, $"{shopItem.ShopType}/{totalCoins}/{newCoinAmount}");

		return DialogTxResult.Okay;
	}

	[ScriptableFunction]
	public DialogTxResult SCR_POPOBOOST_REAWRD_LIST_REQUEST(Character character, DialogTxArgs args)
	{
		// TODO: Figure out where the rewards are stored in client,
		// keep track of gear score and check if the character can
		// gain rewards based off it.
		// Check if rewards already received.
		if (character.Connection.Account.Properties[PropertyName.POPOBOOST_2404_PROGRESS6] == 1)
			return DialogTxResult.Okay;

		// Set Respective AccountProperty (POPOBOOST_2404_PROGRESS0..POPOBOOST_2404_PROGRESS6)
		character.SetAccountProperty(PropertyName.POPOBOOST_2404_PROGRESS6, 1);
		// Give Rewards
		//character.AddItem();

		character.AddonMessage(AddonMessage.POPOBOOST_REWARD_RSET);
		return DialogTxResult.Okay;
	}
}
