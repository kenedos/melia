//--- Melia Script ----------------------------------------------------------
// Normal Transaction Scripts
//--- Description -----------------------------------------------------------
// Handles "Normal TX" requests from the client.
//---------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.L10N;
using Melia.Shared.Versioning;
using Melia.Zone;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Items.Effects;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;

public class NormalTxFunctionsScript : GeneralScript
{
	[ScriptableFunction]
	public NormalTxResult SCR_TX_PROPERTY_ACTIVE_TOGGLE(Character character, string strArg)
	{
		var className = strArg;

		if (!character.Abilities.Toggle(className))
			Log.Warning("SCR_TX_PROPERTY_ACTIVE_TOGGLE: User '{0}' tried to toggle ability '{1}', which they either don't have or is passive.", character.Connection.Account.Name, className);

		return NormalTxResult.Okay;
	}

	[ScriptableFunction]
	public NormalTxResult GUIDE_QUEST_OPEN_UI(Character character, string strArg)
	{
		switch (strArg)
		{
			case "status":
			{
				// Sent when opening the character info. Presumably a
				// request to open the help panel or show a tooltip or
				// something.
				return NormalTxResult.Okay;
			}
			case "inventory":
			{
				// Sent when opening the inventory. Presumably a request
				// to open the help panel or show a tooltip or something.
				return NormalTxResult.Okay;
			}
		}

		return NormalTxResult.Fail;
	}

	[ScriptableFunction]
	public NormalTxResult SCR_TX_STAT_UP(Character character, int[] numArgs)
	{
		// Check amount of parameters
		if (numArgs.Length != 5)
		{
			Log.Warning("SCR_TX_STAT_UP: User '{0}' sent an unexpected number of stat changes. Got {1}, expected 5.", character.Username, numArgs.Length);
			return NormalTxResult.Fail;
		}

		Send.ZC_ADDON_MSG(character, AddonMessage.RESET_STAT_UP, 0, null, 1);

		for (var i = 0; i < numArgs.Length; ++i)
		{
			var addPoints = numArgs[i];
			if (addPoints <= 0)
				continue;

			if (character.Properties.GetFloat(PropertyName.StatPoint) < addPoints)
			{
				Log.Warning("SCR_TX_STAT_UP: User '{0}' tried to spent more stat points than they have.", character.Username);
				break;
			}

			//characterProperties.UsedStat += stat;
			character.Properties.Modify(PropertyName.UsedStat, addPoints);

			switch (i)
			{
				case 0: character.Properties.Modify(PropertyName.STR_STAT, addPoints); break;
				case 1: character.Properties.Modify(PropertyName.CON_STAT, addPoints); break;
				case 2: character.Properties.Modify(PropertyName.INT_STAT, addPoints); break;
				case 3: character.Properties.Modify(PropertyName.MNA_STAT, addPoints); break;
				case 4: character.Properties.Modify(PropertyName.DEX_STAT, addPoints); break;
			}
		}

		// Official doesn't update UsedStat with this packet,
		// but presumably the PROP_UPDATE below. Why send more
		// packets than necessary though?
		character.InvalidateProperties();

		for (var i = 0; i < numArgs.Length; ++i)
		{
			var addPoints = numArgs[i];
			if (addPoints <= 0)
				continue;

			switch (i)
			{
				case 0: Send.ZC_PC_PROP_UPDATE(character, (short)PropertyTable.GetId("PC", PropertyName.STR_STAT), 0); break;
				case 1: Send.ZC_PC_PROP_UPDATE(character, (short)PropertyTable.GetId("PC", PropertyName.CON_STAT), 0); break;
				case 2: Send.ZC_PC_PROP_UPDATE(character, (short)PropertyTable.GetId("PC", PropertyName.INT_STAT), 0); break;
				case 3: Send.ZC_PC_PROP_UPDATE(character, (short)PropertyTable.GetId("PC", PropertyName.MNA_STAT), 0); break;
				case 4: Send.ZC_PC_PROP_UPDATE(character, (short)PropertyTable.GetId("PC", PropertyName.DEX_STAT), 0); break;
			}
		}
		Send.ZC_PC_PROP_UPDATE(character, (short)PropertyTable.GetId("PC", PropertyName.UsedStat), 0);
		character.AddonMessage("PC_PROPERTY_UPDATE_TO_SYSMENU");

		/*
		{ name: "F_pc_status_atk_up", id: 13913 },
		{ name: "F_pc_status_dex_up", id: 13914 },
		{ name: "F_pc_status_con_up", id: 13915 },
		{ name: "F_pc_status_int_up", id: 13916 },
		{ name: "F_pc_status_mna_up", id: 13917 },
		*/

		var delayCount = 0;
		for (var i = 0; i < numArgs.Length; ++i)
		{
			var addPoints = numArgs[i];
			if (addPoints <= 0)
				continue;
			var delay = delayCount * 500;
			switch (i)
			{
				case 0: character.PlayEffectLocal("F_pc_status_str_up", 6, EffectLocation.Bottom, TimeSpan.FromMilliseconds(delay)); break;
				case 1: character.PlayEffectLocal("F_pc_status_con_up", 6, EffectLocation.Bottom, TimeSpan.FromMilliseconds(delay)); break;
				case 2: character.PlayEffectLocal("F_pc_status_int_up", 6, EffectLocation.Bottom, TimeSpan.FromMilliseconds(delay)); break;
				case 3: character.PlayEffectLocal("F_pc_status_mna_up", 6, EffectLocation.Bottom, TimeSpan.FromMilliseconds(delay)); break;
				case 4: character.PlayEffectLocal("F_pc_status_dex_up", 6, EffectLocation.Bottom, TimeSpan.FromMilliseconds(delay)); break;
			}
			delayCount++;
		}

		return NormalTxResult.Okay;
	}

	[ScriptableFunction]
	public NormalTxResult SCR_TX_SKILL_UP(Character character, int[] numArgs)
	{
		var jobId = (JobId)numArgs[0];
		var amounts = numArgs.Skip(1).ToArray();

		// Check job
		var job = character.Jobs.Get(jobId);
		if (job == null)
		{
			Log.Warning("SCR_TX_SKILL_UP: User '{0}' tried to learn skills of a job they don't have.", character.Username);
			return NormalTxResult.Fail;
		}

		// Check skill data
		// The clients sends the number of points to add to every
		// skill, incl. the skills the player shouldn't be able to
		// put points into yet, so we need to use the job's MaxLevel
		// for getting all available skills.
		SkillTreeData[] skillTreeData;
		if (Versions.Protocol > 500)
			skillTreeData = ZoneServer.Instance.Data.SkillTreeDb.FindSkills(job.Id, job.MaxLevel);
		else
			skillTreeData = ZoneServer.Instance.Data.SkillTreeDb.FindSkills(job.Id, job.Circle);
		if (amounts.Length != skillTreeData.Length)
		{
			Log.Warning("SCR_TX_SKILL_UP: User '{0}' sent an unexpected number of skill level changes. Got {1}, expected {2}.", character.Username, amounts.Length, skillTreeData.Length);
			return NormalTxResult.Fail;
		}

		// Iterate over the amounts and try to apply them to the skills
		for (var i = 0; i < amounts.Length; ++i)
		{
			var addLevels = amounts[i];
			if (addLevels <= 0)
				continue;

			// Check skill points
			if (job.SkillPoints < addLevels)
			{
				Log.Warning("SCR_TX_SKILL_UP: User '{0}' tried to use more skill points than they have.", character.Username);
				break;
			}

			var data = skillTreeData[i];
			var skillId = data.SkillId;

			// Check max level
			var maxLevel = character.Skills.GetMaxLevel(skillId);
			var currentLevel = character.Skills.GetLevel(skillId);
			var newLevel = (currentLevel + addLevels);

			// Safety check.
			if (job.Level < data.UnlockLevel)
				continue;

			if (newLevel > maxLevel)
			{
				// Don't warn about this, since the client doesn't
				// check the max level for skills with unlock levels.
				// The player can try, but nothing should happen.
				//Log.Warning("SCR_TX_SKILL_UP: User '{0}' tried to level '{1}' past the max level ({2} > {3}).", character.Username, skillId, newLevel, maxLevel);
				continue;
			}

			// Create skill or update its level
			var skill = character.Skills.Get(skillId);
			if (skill == null)
			{
				skill = new Skill(character, skillId, newLevel);
				character.Skills.Add(skill);
			}
			else
			{
				skill.LevelByDB = newLevel;
				skill.Properties.InvalidateAll();
				Send.ZC_OBJECT_PROPERTY(character.Connection, skill);
			}

			job.SkillPoints -= addLevels;

			ZoneServer.Instance.ServerEvents.PlayerSkillLevelChanged.Raise(new PlayerSkillLevelChangedEventArgs(character, skill));
		}

		Send.ZC_ADDON_MSG(character, AddonMessage.RESET_SKL_UP);
		Send.ZC_JOB_PTS(character, job);
		//Send.ZC_ADDITIONAL_SKILL_POINT(character, job);

		return NormalTxResult.Okay;
	}

	[ScriptableFunction]
	public NormalTxResult SCR_TX_EQUIP_CARD_SLOT(Character character, string strArg)
	{
		var argList = strArg.Split('#');
		if (argList.Length < 2)
			return NormalTxResult.Fail;
		if (!int.TryParse(argList[0], out var slot))
			return NormalTxResult.Fail;
		slot += 1;
		if (!long.TryParse(argList[1], out var itemWorldId))
			return NormalTxResult.Fail;
		var card = character.Inventory.GetItem(itemWorldId);

		if (card == null || character.Inventory.TryGetCard(slot, out var checkCard))
			return NormalTxResult.Fail;

		if (character.Inventory.EquipCard(slot, itemWorldId) != InventoryResult.Success)
		{
			Log.Warning("SCR_TX_EQUIP_CARD_SLOT: User '{0}' failed equip item {1} into slot {2}.", character.Username, itemWorldId, slot);
			return NormalTxResult.Fail;
		}

		var clientScript = string.Format($"_CARD_SLOT_EQUIP('{slot}', '{card.Id}', '{card.Properties[PropertyName.CardLevel]}', '{card.Properties[PropertyName.ItemExp]}')");
		character.ExecuteClientScript(clientScript);

		return NormalTxResult.Okay;
	}

	[ScriptableFunction]
	public NormalTxResult SCR_TX_UNEQUIP_CARD_SLOT(Character character, int[] numArgs)
	{
		if (numArgs.Length < 2)
			return NormalTxResult.Fail;
		var slot = numArgs[0];
		slot += 1;
		var showNoEffect = numArgs[1] == 1;

		if (!character.Inventory.TryGetCard(slot, out var card))
		{
			Log.Warning("SCR_TX_UNEQUIP_CARD_SLOT: User '{0}' failed to find item from slot {1}.", character.Username, slot);
			return NormalTxResult.Fail;
		}

		if (character.Inventory.UnequipCard(slot) != InventoryResult.Success)
		{
			Log.Warning("SCR_TX_UNEQUIP_CARD_SLOT: User '{0}' failed to unequip item from slot {1}.", character.Username, slot);
			return NormalTxResult.Fail;
		}

		var cardGroupName = card.Data.CardGroup.ToString();
		var clientScript = string.Format($"_CARD_SLOT_REMOVE('{slot}', '{cardGroupName}')");
		character.ExecuteClientScript(clientScript);

		return NormalTxResult.Okay;
	}

	[ScriptableFunction("SCR_TX_TRADE_SELECT_ITEM")]
	[ScriptableFunction("SCR_TX_TRADE_SELECT_ITEM_2")]
	[ScriptableFunction("SCR_TX_TRADE_SELECT_ITEM_3")]
	[ScriptableFunction("SCR_TX_TRADE_SELECT_ITEM_RANOPT")]
	public NormalTxResult SCR_TX_TRADE_SELECT_ITEM(Character character, string strArg)
	{
		var argList = strArg.Split('#');
		if (!long.TryParse(argList[0], out var itemWorldId))
			return NormalTxResult.Fail;
		if (!int.TryParse(argList[1], out var selectionIndex))
			return NormalTxResult.Fail;

		var item = character.Inventory.GetItem(itemWorldId);

		if (item == null)
		{
			Log.Warning("SCR_TX_TRADE_SELECT_ITEM: Failed to find item with world id {0} by {1}", itemWorldId, character.Username);
			return NormalTxResult.Fail;
		}

		// Check if the item's lifetime is over
		if (item.Properties[PropertyName.ItemLifeTimeOver] == 1)
		{
			character.SystemMessage("CannotUseLifeTimeOverItem");
			return NormalTxResult.Fail;
		}

		if (!ZoneServer.Instance.Data.SelectItemDb.TryFind(item.Data.ClassName, out var selectItem))
		{
			Log.Warning("SCR_TX_TRADE_SELECT_ITEM: Failed to find item with world id {0} by {1}", itemWorldId, character.Username);
			return NormalTxResult.Fail;
		}

		// Selection Index is 1 based instead of 0 based, so offset by 1
		selectionIndex--;
		if (selectionIndex >= selectItem.Items.Count || selectionIndex < 0)
		{
			Log.Debug("SCR_TX_TRADE_SELECT_ITEM: Invalid selection index {0} by {1}", selectionIndex, character.Username);
			return NormalTxResult.Fail;
		}
		var requiredItemCount = selectItem.RequiredItemCount[0];

		if (selectItem.RequiredItemCount.Count > 1 && selectionIndex < selectItem.RequiredItemCount.Count)
			requiredItemCount = selectItem.RequiredItemCount[selectionIndex];

		if (item.Amount >= requiredItemCount && selectItem.Items.Count != 0 && selectionIndex < selectItem.Items.Count)
		{
			character.Inventory.Remove(itemWorldId, requiredItemCount, InventoryItemRemoveMsg.Used);
			var items = selectItem.Items[selectionIndex];
			foreach (var itemToAdd in items)
			{
				var itemData = ZoneServer.Instance.Data.ItemDb.FindByClass(itemToAdd.ItemId);
				if (itemData != null)
					character.Inventory.Add(new Item(itemData.Id, itemToAdd.Amount), InventoryAddType.New);
			}
		}

		return NormalTxResult.Okay;
	}

	[ScriptableFunction]
	public NormalTxResult SCR_EVENT_STAMP_TOUR_CHECK(Character character, string strArg)
	{
		// Example String Arg: POPO_EVENT_STAMP 1

		var checkAccountProp = false;

		if (!checkAccountProp)
			return NormalTxResult.Fail;

		character.AddonMessage(AddonMessage.EVENT_STAMP_TOUR_REWARD_GET, null, 1);

		return NormalTxResult.Okay;
	}

}
