//--- Melia Script ----------------------------------------------------------
// Warp Items
//--- Description -----------------------------------------------------------
// Item scripts that warp the character.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.Scripting.Shared;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using static Melia.Zone.Scripting.Shortcuts;

public class WarpScript : GeneralScript
{
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_WARP_KLAIPE(Character character, Item item, string buffName, float numArg1, float numArg2)
	{
		if (ZoneServer.Instance.World.TryGetMap("c_Klaipe", out var klaipeda))
		{
			var location = new Location(klaipeda.Id, -161, 149, 54);
			character.Warp(location);
		}

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_WARP_ORSHA(Character character, Item item, string buffName, float numArg1, float numArg2)
	{
		if (ZoneServer.Instance.World.TryGetMap("c_orsha", out var orsha))
		{
			var location = new Location(orsha.Id, 157, 176, 268);
			character.Warp(location);
		}

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_WARP_FEDIMIAN(Character character, Item item, string buffName, float numArg1, float numArg2)
	{
		if (ZoneServer.Instance.World.TryGetMap("c_fedimian", out var fedimian))
		{
			var location = new Location(fedimian.Id, -277, 162, -281);
			character.Warp(location);
		}

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv50(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("f_gele_57_4", 1180.28f, 1899.31f, -78.24f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv80(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("f_rokas_31", 465.12f, -104.48f, 107.20f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv100(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("c_request_1", -52.76f, -38.66f, 0.35f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv100_2(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("f_orchard_32_4", -1669.46f, -863.73f, 73.62f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv110(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("f_remains_40", 3185.22f, 2686.16f, 645.84f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv120(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("c_nunnery", 160.35f, -89.13f, -75.75f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv140(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("f_pilgrimroad_51", 1272.88f, 2058.53f, 571.24f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv150(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("f_siauliai_50_1", -39.05f, -1825.53f, 0.31f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv170(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("f_pilgrimroad_51", 1204.79f, 1884.27f, 571.24f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv170_2(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("f_siauliai_46_4", 1188.81f, -294.57f, 188.67f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv190(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("f_flash_61", -749.41f, 1332.78f, 449.08f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv200(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("f_remains_40", 3290.65f, 2923.09f, 645.84f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv210_2(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("f_tableland_28_2", -233.16f, -726.73f, 37.83f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv230(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("c_fedimian", -509.30f, -256.87f, 170.50f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv230_2(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("f_siauliai_15_re", -350.08f, 2138.39f, 1015.05f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv240_2(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("d_zachariel_79_1", 546.24f, 624.44f, -62.25f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv260(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("f_flash_64", -532.73f, 1469.33f, 885.49f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv260_2(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("f_remains_37_3", -2661.98f, 2556.54f, 52.40f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv290(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("f_maple_25_2", 1206.53f, 771.63f, 641.89f);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Dungeon_Lv315(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.Warp("f_whitetrees_56_1", 303.26f, -73.88f, 1.10f);
		return ItemUseResult.Okay;
	}

	// --- Complex Reward Script ---

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_Guide_Cube_1(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var session = character.SessionObjects.Main;
		var account = character.Connection.Account;

		// Structured reward data for readability and safety
		var rewardTiers = new List<LevelReward>
		{
			new(5, "Premium_boostToken_14d", 2, "Premium_WarpScroll", 5, "RestartCristal", 5, "Event_drug_steam_1h", 10),
			new(10, "Premium_boostToken_14d", 2, "Premium_WarpScroll", 5, "RestartCristal", 5, "Mic", 10, "NECK99_107", 1, "Scroll_Warp_Klaipe", 1, "JOB_VELHIDER_COUPON", 1, "E_SWD04_106", 1, "E_TSW04_106", 1, "E_MAC04_108", 1, "E_TSF04_106", 1, "E_STF04_107", 1, "E_SPR04_103", 1, "E_TSP04_107", 1, "E_BOW04_106", 1, "E_TBW04_106", 1, "E_SHD04_102", 1),
			new(15, "Premium_boostToken_14d", 2, "Premium_WarpScroll", 5, "RestartCristal", 5, "Mic", 10, "Hat_628051", 1, "Event_drug_steam_1h", 10),
			new(30, "Premium_boostToken02_event01", 2, "BRC99_103", 1, "BRC99_104", 1),
			new(50, "Premium_boostToken02_event01", 2, "Premium_indunReset_14d", 1, "Mic", 10, "Event_Warp_Dungeon_Lv50", 1, "E_FOOT04_101", 1),
			new(80, "Premium_boostToken02_event01", 4, "Premium_indunReset_14d", 2, "Premium_WarpScroll", 5, "RestartCristal", 5, "GIMMICK_Drug_HPSP1", 20, "Event_Warp_Dungeon_Lv80", 1, "E_costume_Com_4", 1, "E_HAIR_M_116", 1, "E_HAIR_F_117", 1),
			new(100, "Premium_boostToken03_event01", 1, "RestartCristal", 5, "Mic", 10, "Scroll_Warp_Fedimian", 1, "Event_Warp_Dungeon_Lv100_2", 1, "Event_Warp_Dungeon_Lv100", 1, "Hat_628061", 1),
			new(110, "Premium_boostToken03_event01", 2, "Premium_indunReset_14d", 1, "Event_Warp_Dungeon_Lv110", 1),
			new(120, "Premium_boostToken03_event01", 2, "Premium_indunReset_14d", 1, "Event_Warp_Dungeon_Lv120", 1, "Mic", 10, "Premium_Enchantchip14", 2),
			new(140, "Premium_boostToken03_event01", 2, "Event_Warp_Dungeon_Lv140", 1, "GIMMICK_Drug_HPSP2", 20, "ABAND01_118", 1),
			new(150, "Premium_boostToken03_event01", 2, "Premium_WarpScroll", 5, "RestartCristal", 10, "Event_Warp_Dungeon_Lv150", 1, "E_BRC04_101", 1, "E_BRC02_109", 1),
			new(170, "Premium_boostToken03_event01", 2, "Drug_Fortunecookie", 1, "Premium_indunReset_14d", 1, "Event_Warp_Dungeon_Lv170", 1, "Event_Warp_Dungeon_Lv170_2", 1),
			new(190, "Premium_boostToken03_event01", 2, "Drug_Fortunecookie", 2, "Event_Warp_Dungeon_Lv190", 1, "GIMMICK_Drug_HPSP3", 20),
			new(200, "Premium_boostToken03_event01", 2, "Drug_Fortunecookie", 3, "Premium_indunReset_14d", 1, "Event_Warp_Dungeon_Lv200", 1),
			new(210, "Premium_boostToken03_event01", 2, "Drug_Fortunecookie", 4, "Premium_indunReset_14d", 1, "Event_Warp_Dungeon_Lv210_2", 1, "E_BRC03_108", 1, "E_BRC04_103", 1),
			new(230, "Premium_boostToken03", 2, "Drug_Fortunecookie", 5, "Premium_indunReset", 1, "Event_Warp_Dungeon_Lv230", 1, "Event_Warp_Dungeon_Lv230_2", 1, "E_BRC03_120", 1),
			new(240, "Premium_boostToken03", 2, "Drug_Fortunecookie", 5, "Premium_indunReset", 2, "GIMMICK_Drug_HPSP3", 20, "Event_Warp_Dungeon_Lv240_2", 1),
			new(260, "Premium_boostToken03", 2, "Event_Warp_Dungeon_Lv260", 1, "Event_Warp_Dungeon_Lv260_2", 1, "Hat_628133", 1),
			new(290, "Premium_boostToken03", 2, "Premium_Enchantchip14", 4, "Event_Warp_Dungeon_Lv290", 1, "Gacha_G_013", 1)
		};

		// This value indicates which tier the player is currently eligible for.
		// E.g., if it's 0, they are eligible for the first tier (at index 0).
		var currentTierIndex = (int)session.Properties.GetFloat(PropertyName.EVENT_VALUE_SOBJ11);

		if (currentTierIndex >= rewardTiers.Count)
		{
			// Player has claimed all rewards.
			return ItemUseResult.Fail;
		}

		var currentTier = rewardTiers[currentTierIndex];

		if (character.Level >= currentTier.RequiredLevel)
		{
			// Player is eligible, give rewards.
			foreach (var reward in currentTier.Rewards)
			{
				character.AddItem(reward.ItemName, reward.Count, "Retention_Event");
			}

			// Handle special one-off rewards
			if (account.Properties.GetString(PropertyName.DAYCHECK_EVENT_LAST_DATE) != "retention")
			{
				account.Properties.SetString(PropertyName.DAYCHECK_EVENT_LAST_DATE, "retention");
				account.Properties.SetFloat(PropertyName.EVENT_WHITE_R1, 0);
				account.Properties.SetFloat(PropertyName.EVENT_WHITE_R2, 0);
			}
			if (character.Level >= 15 && account.Properties.GetFloat(PropertyName.EVENT_WHITE_R1) == 0 && currentTierIndex == 2)
			{
				character.AddItem("PremiumToken_1d", 1, "Retention_Event");
				character.ModifyAccountProperty(PropertyName.EVENT_WHITE_R1, 1);
			}
			if (character.Level >= 110 && account.Properties.GetFloat(PropertyName.EVENT_WHITE_R2) == 0 && currentTierIndex == 7)
			{
				character.AddItem("PremiumToken_1d", 1, "Retention_Event");
				character.ModifyAccountProperty(PropertyName.EVENT_WHITE_R2, 1);
			}

			// Increment the player's tier for the next time they use the cube.
			character.ModifySessionProperty(PropertyName.EVENT_VALUE_SOBJ11, 1);

			// Notify player of the next tier
			var nextTierLevel = (currentTierIndex + 1 < rewardTiers.Count) ? rewardTiers[currentTierIndex + 1].RequiredLevel : 0;
			if (nextTierLevel > 0)
			{
				character.AddonMessage("NOTICE_Dm_!", ScpArgMsg("Retention_Select2", "LEVELCOUNT", nextTierLevel), 5);
			}

			return ItemUseResult.Okay;
		}
		else
		{
			// Player is not high enough level for the current tier.
			character.AddonMessage("NOTICE_Dm_!", ScpArgMsg("Retention_Select2", "LEVELCOUNT", currentTier.RequiredLevel), 5);
			return ItemUseResult.Fail; // Item is not consumed
		}
	}

	// --- NPC Dialog Script ---
	[DialogFunction]
	public static async Task SCR_NPC_RETENTION_DIALOG(Dialog dialog)
	{
		var character = dialog.Player;
		var session = character.SessionObjects.Main;

		if (ZoneServer.Instance.Data.ServerDb.Entries.Values.FirstOrDefault()?.Name != "GLOBAL")
		{
			return;
		}

		if (session.Properties.GetFloat(PropertyName.EVENT_VALUE_SOBJ12) >= 1)
		{
			return;
		}

		var creationTime = character.Connection.Account.CreationDate;

		if ((creationTime.Month == 3 && creationTime.Day >= 28 && creationTime.Year == 2017) ||
			(creationTime.Month >= 4 && creationTime.Year == 2017))
		{
			await dialog.Msg(ScpArgMsg("NPC_EVENT_RETENTION_1"));
			character.AddItem("Event_Guide_Cube_1", 19, "RETENTION_EVENT");
			character.ModifySessionProperty(PropertyName.EVENT_VALUE_SOBJ12, 1);
		}
		else
		{
			await dialog.Msg(ScpArgMsg("NPC_EVENT_RETENTION_2"));
		}
	}

	public class LevelReward
	{
		public int RequiredLevel { get; }
		public List<(string ItemName, int Count)> Rewards { get; }

		public LevelReward(int requiredLevel, params object[] rewardsData)
		{
			RequiredLevel = requiredLevel;
			Rewards = new List<(string, int)>();

			for (var i = 0; i < rewardsData.Length; i += 2)
			{
				if (i + 1 < rewardsData.Length)
				{
					var itemName = (string)rewardsData[i];
					var count = Convert.ToInt32(rewardsData[i + 1]);
					Rewards.Add((itemName, count));
				}
			}
		}
	}
}
