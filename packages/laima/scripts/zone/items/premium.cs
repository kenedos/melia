//--- Melia Script ----------------------------------------------------------
// Premium Items (Repair Kit...)
//--- Description -----------------------------------------------------------
// Item-related scripts that are defined as Premium group items.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Storages;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;

public class PremiumItemScripts : GeneralScript
{
	[ScriptableFunction]
	public ItemUseResult PREMIUM_REPAIR(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		foreach (var equipItem in character.Inventory.GetEquip().Values)
			equipItem.ModifyDurability(character, (int)item.Data.Script.NumArg1);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_GOLDMORU_BOX(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem(ItemId.Moru_Gold, (int)numArg1);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_EXTEND_ACCOUNT_WAREHOUSE(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.ModifyAccountProperty(PropertyName.AccountWareHouseExtend, 1);
		character.AddonMessage(AddonMessage.ACCOUNT_WAREHOUSE_ITEM_LIST);
		character.AddonMessage(AddonMessage.ACCOUNT_UPDATE);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_FREE_EXTEND_ACCOUNT_WAREHOUSE(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		// Prefer the argument declared by this voucher; retain legacy NumArg2 support.
		var requestedAmount = numArg1 > 0 ? numArg1 : numArg2;
		if (float.IsNaN(requestedAmount) || float.IsInfinity(requestedAmount)
			|| requestedAmount >= int.MaxValue)
			return ItemUseResult.Fail;

		var amount = requestedAmount > 0 ? (int)requestedAmount : 1;
		if (amount <= 0 || character?.Connection?.Account?.TeamStorage == null)
			return ItemUseResult.Fail;

		var storage = character.Connection.Account.TeamStorage;
		var result = storage.TryExtendStorageByItem(amount);
		if (result != StorageResult.Success)
		{
			character.ServerMessage("Team Storage could not be expanded. Check the maximum storage capacity.");
			return ItemUseResult.Fail;
		}

		if (amount == 1)
			character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, ScpArgMsg("ACCOUNT_UPDATE1"), 5);
		else if (amount == 2)
			character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, ScpArgMsg("ACCOUNT_UPDATE2"), 5);

		return ItemUseResult.Okay;
	}
}
