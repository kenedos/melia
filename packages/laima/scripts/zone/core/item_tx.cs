//--- Melia Script ----------------------------------------------------------
// Dialog Transaction Scripts
//--- Description -----------------------------------------------------------
// Handles "Dialog TX" requests from the client.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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

public class ItemTxFunctionsScript : GeneralScript
{
	// Class names must match exactly as in items.txt (e.g., Gem_Swordman_Bash, GEM_OutLaw_Bully)
	private static readonly HashSet<string> AllowedSkillGemClasses = new(StringComparer.OrdinalIgnoreCase)
	{
		"Swordman",
		"Highlander",
		"Peltasta",
		"Barbarian",
		"Hoplite",
		"Archer",
		"Ranger",
		"Sapper",
		"QuarrelShooter",
		"Hunter",
		"Wizard",
		"Pyromancer",
		"Cryomancer",
		"Psychokino",
		"Bokor",
		"Cleric",
		"Priest",
		"Kriwi",
		"Paladin",
		"Dievdirbys",
		"Scout",
		"Linker",
		"Assassin",
		"OutLaw",
		"Corsair",
	};

	[ScriptableFunction]
	public ItemTxResult SCR_TRADE_SELECT_SKILL_GEM(Character character, Item item, int[] numArgs)
	{
		if (!character.Inventory.TryGetItem(item.ObjectId, out _))
			return ItemTxResult.Fail;

		if (numArgs.Length < 1)
		{
			Log.Warning("SCR_TRADE_SELECT_SKILL_GEM: No item selected.");
			return ItemTxResult.Fail;
		}

		var selectedItemId = numArgs[0];

		if (!ZoneServer.Instance.Data.ItemDb.TryFind(selectedItemId, out var selectedItemData))
		{
			Log.Warning("SCR_TRADE_SELECT_SKILL_GEM: Selected item '{0}' not found in database.", selectedItemId);
			return ItemTxResult.Fail;
		}

		var className = selectedItemData.ClassName;
		if (!className.StartsWith("Gem_", StringComparison.OrdinalIgnoreCase))
		{
			Log.Warning("SCR_TRADE_SELECT_SKILL_GEM: Selected item '{0}' is not a skill gem.", className);
			return ItemTxResult.Fail;
		}

		var parts = className.Split('_');
		if (parts.Length < 3)
		{
			Log.Warning("SCR_TRADE_SELECT_SKILL_GEM: Invalid skill gem className format: {0}", className);
			return ItemTxResult.Fail;
		}

		var jobClass = parts[1];
		if (!AllowedSkillGemClasses.Contains(jobClass))
		{
			character.ServerMessage($"Skill gems for {jobClass} are not available yet.");
			return ItemTxResult.Fail;
		}

		character.Inventory.Add(selectedItemId, 1, InventoryAddType.PickUp);

		return ItemTxResult.Okay;
	}

	[ScriptableFunction]
	public ItemTxResult TX_SCR_USE_SKILL_STAT_RESET(Character character, Item item, int[] numArgs)
	{
		if (!character.Inventory.TryGetItem(item.ObjectId, out _))
			return ItemTxResult.Fail;

		if (item.Id != ItemId.Premium_SkillReset
			&& item.Id != ItemId.Steam_Premium_SkillReset
			&& item.Id != ItemId.Steam_Premium_SkillReset_1
			&& item.Id != ItemId.Premium_SkillReset_14d
			&& item.Id != ItemId.Premium_SkillReset_1d
			&& item.Id != ItemId.Premium_SkillReset_60d
			&& item.Id != ItemId.Premium_SkillReset_Trade
			&& item.Id != ItemId.PC_Premium_SkillReset)
			return ItemTxResult.Fail;

		if (item.IsExpired)
			return ItemTxResult.Fail;

		if (item.IsLocked)
			return ItemTxResult.Fail;

		character.ResetSkills();
		character.AddonMessage(AddonMessage.RESET_SKL_UP);

		return ItemTxResult.Okay;
	}

	[ScriptableFunction]
	public ItemTxResult TX_SCR_USE_STAT_RESET(Character character, Item item, int[] numArgs)
	{
		if (!character.Inventory.TryGetItem(item.ObjectId, out _))
			return ItemTxResult.Fail;

		if (item.Id != ItemId.Premium_StatReset
			&& item.Id != ItemId.Steam_Premium_StatReset
			&& item.Id != ItemId.Premium_StatReset14
			&& item.Id != ItemId.Steam_Premium_StatReset_1
			&& item.Id != ItemId.Premium_StatReset_TA
			&& item.Id != ItemId.Premium_StatReset_1d
			&& item.Id != ItemId.Premium_StatReset_60d
			&& item.Id != ItemId.Premium_StatReset_Trade
			&& item.Id != ItemId.Premium_StatReset_30d
			&& item.Id != ItemId.PC_Premium_StatReset)
			return ItemTxResult.Fail;

		if (item.IsExpired)
			return ItemTxResult.Fail;

		if (item.IsLocked)
			return ItemTxResult.Fail;

		character.ResetStats();

		return ItemTxResult.Okay;
	}
}
