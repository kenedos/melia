//--- Melia Script ----------------------------------------------------------
// Simony Skill Scroll Crafting
//--- Description -----------------------------------------------------------
// Handles the Pardoner's Simony skill scroll crafting transaction.
// Allows creating scrolls from learned skills that others can use.
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
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;

public class SimonyCraftingScript : GeneralScript
{
	// Skill type ranges for different crafting skills
	private const int PardonerSkillTypeMin = 40000;
	private const int PardonerSkillTypeMax = 50000;
	private const int EnchanterSkillTypeMin = 20000;
	private const int EnchanterSkillTypeMax = 30000;

	// Crafting time in seconds per scroll
	private const float BaseCraftTimePerScroll = 3.0f;

	// Bottle item names for crafting
	private const string SimonyBottle = "misc_parchment";
	private const string EnchanterBottle = "misc_magicscroll";
	private const string RuneBottle = "misc_runeStone";

	/// <summary>
	/// Handles the SCR_SKILLITEM_MAKE transaction for crafting skill scrolls.
	/// Called when a player attempts to craft scrolls via Simony or CraftMagicScrolls.
	/// </summary>
	/// <param name="character">The character crafting the scroll.</param>
	/// <param name="item">The item used to trigger crafting (if any).</param>
	/// <param name="numArgs">Arguments: [0] = skillType, [1] = level, [2] = count</param>
	/// <returns>Transaction result.</returns>
	[ScriptableFunction]
	public ItemTxResult SCR_SKILLITEM_MAKE(Character character, Item item, int[] numArgs)
	{
		if (numArgs == null || numArgs.Length < 2)
		{
			//Log.Debug("SCR_SKILLITEM_MAKE: Failed - Invalid arguments. Expected 3 args (skillType, level, count).");
			Log.Debug("SCR_SKILLITEM_MAKE: Failed - Invalid arguments. Expected 2 args (skillType, count).");
			return ItemTxResult.Fail;
		}

		var skillType = numArgs[0];
		var count = numArgs[1];

		if (count <= 0 || count > 999)
		{
			Log.Debug("SCR_SKILLITEM_MAKE: Failed - Invalid count: {0}", count);
			return ItemTxResult.Fail;
		}

		// Validate skill type and get the crafting skill
		if (!TryGetCraftingSkill(character, skillType, out var craftingSkill, out var craftingSkillName))
		{
			Log.Debug("SCR_SKILLITEM_MAKE: Failed - Could not find crafting skill for skillType: {0}", skillType);
			return ItemTxResult.Fail;
		}

		// Check if the Simony class data allows this skill to be made into a scroll
		if (!CanMakeScroll(skillType))
		{
			Log.Debug("SCR_SKILLITEM_MAKE: Failed - Skill type {0} cannot be made into a scroll.", skillType);
			character.ServerMessage("This skill cannot be made into a scroll.");
			return ItemTxResult.Fail;
		}

		// Get the skill class data
		if (!ZoneServer.Instance.Data.SkillDb.TryFind((SkillId)skillType, out var skillData))
		{
			Log.Debug("SCR_SKILLITEM_MAKE: Failed - Could not find skill data for type: {0}", skillType);
			return ItemTxResult.Fail;
		}

		// Check if character has the skill
		var characterSkill = character.Skills.Get(skillData.Id);
		if (characterSkill == null)
		{
			Log.Debug("SCR_SKILLITEM_MAKE: Failed - Character does not have skill: {0}", skillData.ClassName);
			return ItemTxResult.Fail;
		}

		// Maximum scroll level is min of character's skill level and crafting skill level
		var level = 1;
		var maxLevel = Math.Min(characterSkill.Level, craftingSkill.Level);
		if (level > maxLevel || level <= 0)
		{
			Log.Debug("SCR_SKILLITEM_MAKE: Failed - Requested level {0} exceeds max level {1}", level, maxLevel);
			character.ServerMessage("You can only craft scrolls up to level {0}.", maxLevel);
			return ItemTxResult.Fail;
		}

		// Calculate material costs
		var price = GetSkillMatPrice(characterSkill, level);
		var (bottleName, bottleCount) = GetSkillMatItem(craftingSkillName, characterSkill, level);
		var totalPrice = (int)price * count;
		var totalBottles = bottleCount * count;

		// Check if character has enough silver
		if (!character.Inventory.HasItem(ItemId.Vis, totalPrice))
		{
			Log.Debug("SCR_SKILLITEM_MAKE: Failed - Not enough silver. Required: {0}", totalPrice);
			character.ServerMessage("Not enough silver. Required: {0}", totalPrice);
			return ItemTxResult.Fail;
		}

		// Check if character has enough bottles
		if (!character.Inventory.HasItem(bottleName, totalBottles))
		{
			Log.Debug("SCR_SKILLITEM_MAKE: Failed - Not enough {0}. Required: {1}", bottleName, totalBottles);
			character.ServerMessage("Not enough materials. Required: {0} x{1}", bottleName, totalBottles);
			return ItemTxResult.Fail;
		}

		// Calculate crafting time
		var craftTimeSec = GetSkillItemMakeTime(characterSkill, count);

		// Apply ReduceCraftTime_Buff if active
		if (character.TryGetBuff(BuffId.ReduceCraftTime_Buff, out var reduceBuff))
		{
			var buffLevel = reduceBuff.NumArg1;
			craftTimeSec *= (1f - buffLevel * 0.05f);
		}

		// Start the crafting process
		_ = DoCrafting(character, skillType, level, count, craftingSkillName, bottleName, totalBottles, totalPrice, craftTimeSec);

		return ItemTxResult.Okay;
	}

	/// <summary>
	/// Performs the actual crafting process with animation and timing.
	/// </summary>
	private async Task DoCrafting(Character character, int skillType, int level, int count,
		string craftingSkillName, string bottleName, int totalBottles, int totalPrice, float craftTimeSec)
	{
		try
		{
			// Send crafting start to client
			var script = string.Format("SKILLITEM_MAKE_START({0})", craftTimeSec);
			character.ExecuteClientScript(script);

			// Play crafting sound
			character.PlaySound("system_craft_bargauge");

			// Determine animation based on crafting skill
			var anim = craftingSkillName == "Enchanter_CraftMagicScrolls" ? "MAKING" : "MAKING_SIMONY";

			// Wait for crafting time
			// In a full implementation, this would use DOTIMEACTION_R equivalent
			// For now, we'll just wait the duration
			await Task.Delay(TimeSpan.FromSeconds(craftTimeSec));

			// Check if character is still valid and can complete crafting
			if (character == null || !character.IsOnline)
			{
				Log.Debug("SCR_SKILLITEM_MAKE: Crafting cancelled - Character offline.");
				return;
			}

			// Verify materials are still available after the wait
			if (!character.Inventory.HasItem(ItemId.Vis, totalPrice))
			{
				character.PlaySound("system_craft_potion_fail");
				character.ServerMessage("Crafting failed - Not enough silver.");
				return;
			}

			if (!character.Inventory.HasItem(bottleName, totalBottles))
			{
				character.PlaySound("system_craft_potion_fail");
				character.ServerMessage("Crafting failed - Not enough materials.");
				return;
			}

			// Remove materials
			character.RemoveItem(bottleName, totalBottles);
			character.RemoveItem(ItemId.Vis, totalPrice);

			// Create the scroll item
			var scrollItem = new Item(ItemId.Scroll_SkillItem, count);
			scrollItem.Properties.SetFloat(PropertyName.SkillType, skillType);
			scrollItem.Properties.SetFloat(PropertyName.SkillLevel, level);

			// Add scroll to inventory
			character.Inventory.Add(scrollItem, InventoryAddType.PickUp);

			// Play success sound
			character.PlaySound("system_craft_potion_succes");

			// Get skill name for message
			if (ZoneServer.Instance.Data.SkillDb.TryFind((SkillId)skillType, out var skillData))
			{
				character.ServerMessage("Successfully crafted {0} x{1} Lv.{2} scroll(s).", skillData.Name, count, level);
			}

			Log.Debug("SCR_SKILLITEM_MAKE: Successfully crafted {0} scrolls of skill type {1} level {2} for character {3}",
				count, skillType, level, character.Name);
		}
		catch (Exception ex)
		{
			Log.Error("SCR_SKILLITEM_MAKE: Error during crafting - {0}", ex.Message);
			character?.PlaySound("system_craft_potion_fail");
		}
	}

	/// <summary>
	/// Attempts to get the crafting skill (Simony or CraftMagicScrolls) based on skill type.
	/// </summary>
	private bool TryGetCraftingSkill(Character character, int skillType, out Skill craftingSkill, out string craftingSkillName)
	{
		craftingSkill = null;
		craftingSkillName = null;

		// Pardoner skills (40000-50000) use Simony
		if (skillType > PardonerSkillTypeMin && skillType < PardonerSkillTypeMax)
		{
			craftingSkill = character.Skills.Get(SkillId.Pardoner_Simony);
			craftingSkillName = "Pardoner_Simony";
		}
		// Runecaster skills (20000-30000) use CraftMagicScrolls (Replaces Enchanter and Also doesn't exist)
		else if (skillType > EnchanterSkillTypeMin && skillType < EnchanterSkillTypeMax)
		{
			craftingSkill = character.Skills.Get(SkillId.RuneCaster_CraftMagicScrolls);
			craftingSkillName = "Enchanter_CraftMagicScrolls";
		}
		// Enchanter skills (20000-30000) use CraftMagicScrolls
		else if (skillType > EnchanterSkillTypeMin && skillType < EnchanterSkillTypeMax)
		{
			craftingSkill = character.Skills.Get(SkillId.Enchanter_CraftMagicScrolls);
			craftingSkillName = "Enchanter_CraftMagicScrolls";
		}

		return craftingSkill != null;
	}

	/// <summary>
	/// Checks if a skill can be made into a scroll based on Simony data.
	/// </summary>
	private bool CanMakeScroll(int skillType)
	{
		// Check the Simony database for CanMake = true
		if (ZoneServer.Instance.Data.SimonyDb.TryFind(skillType, out var simonyData))
		{
			return simonyData.CanMake;
		}

		// If no Simony data exists, check if it's in a valid skill range
		// Pardoner scrolls: 40000-50000, Enchanter scrolls: 20000-30000
		if ((skillType > PardonerSkillTypeMin && skillType < PardonerSkillTypeMax) ||
			(skillType > EnchanterSkillTypeMin && skillType < EnchanterSkillTypeMax))
		{
			return true;
		}

		return false;
	}

	/// <summary>
	/// Calculates the silver cost for crafting a skill scroll.
	/// </summary>
	private long GetSkillMatPrice(Skill skill, int level)
	{
		// Check if we have Simony database entry for custom pricing
		var skillType = (int)skill.Id;
		if (ZoneServer.Instance.Data.SimonyDb.TryFind(skillType, out var simonyData))
		{
			// Use database base cost, scaled by level
			return simonyData.BaseCost + (level * 500L);
		}

		// Default formula: Base price scales with skill level and rank
		var basePrice = 1000L;
		var levelMultiplier = level * 500L;
		//var skillRankMultiplier = (int)skill.Data.Rank * 200L;
		var skillRankMultiplier = (int)1 * 200L;

		return basePrice + levelMultiplier + skillRankMultiplier;
	}

	/// <summary>
	/// Gets the bottle item and count required for crafting.
	/// </summary>
	private (string bottleName, int count) GetSkillMatItem(string craftingSkillName, Skill skill, int level)
	{
		var skillType = (int)skill.Id;

		// Check Simony database for custom materials
		if (ZoneServer.Instance.Data.SimonyDb.TryFind(skillType, out var simonyData))
		{
			// Material count scales with level
			var scaledCount = simonyData.MaterialCount + (level / 5);
			return (simonyData.MaterialClassName, scaledCount);
		}

		// Default: Determine bottle type based on crafting skill
		var bottleName = craftingSkillName == "Enchanter_CraftMagicScrolls" ? EnchanterBottle : SimonyBottle;

		// Bottle count scales with level
		var count = 1 + (level / 5);

		return (bottleName, count);
	}

	/// <summary>
	/// Calculates the crafting time in seconds.
	/// </summary>
	private float GetSkillItemMakeTime(Skill skill, int count)
	{
		var skillType = (int)skill.Id;

		// Check Simony database for custom craft time
		if (ZoneServer.Instance.Data.SimonyDb.TryFind(skillType, out var simonyData))
		{
			return simonyData.CraftTime * count;
		}

		// Default: Base time per scroll, increases with count
		return BaseCraftTimePerScroll * count;
	}
}
