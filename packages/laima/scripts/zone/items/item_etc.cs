using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Yggdrasil.Util;
using static Melia.Shared.Util.TaskHelper;
using static Melia.Zone.Scripting.Shortcuts;

public class ItemEtcScripts : GeneralScript
{
	/// <summary>
	/// Unlocks an achievement by adding 1 achievement point specified by the item.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="strArg"></param>
	/// <param name="numArg1"></param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ACHIEVE_BOX(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var achievementPointName = strArg;

		character.Achievements.AddAchievementPoints(achievementPointName, 1);

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_ADD_CHAT_EMOTICON(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (!ZoneServer.Instance.Data.ChatEmoticonDb.TryFind(strArg, out var data))
			return ItemUseResult.Fail;

		character.SetAccountProperty($"HaveEmoticon_{data.Id}", 1);
		character.AddonMessage(AddonMessage.ADD_CHAT_EMOTICON);

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_ADD_CHAT_EMOTICON_PACK(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var startIndex = (int)numArg1;
		var endIndex = (int)numArg2;
		var account = character.Connection.Account;

		// Check if first emoticon in pack is already owned
		var firstEmoticonName = $"{strArg}{startIndex}";
		if (ZoneServer.Instance.Data.ChatEmoticonDb.TryFind(firstEmoticonName, out var firstEmoticonData))
		{
			if (account.Properties.GetFloat($"HaveEmoticon_{firstEmoticonData.Id}") >= 1)
			{
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, ScpArgMsg("Premium_Emoticon_Desc1"), 3);
				return ItemUseResult.Fail;
			}
		}

		var unlocked = 0;

		// Iterate through emoticon pack range
		for (var i = startIndex; i <= endIndex; i++)
		{
			var emoticonName = $"{strArg}{i}";
			if (ZoneServer.Instance.Data.ChatEmoticonDb.TryFind(emoticonName, out var emoticonData))
			{
				character.SetAccountProperty($"HaveEmoticon_{emoticonData.Id}", 1);
				unlocked++;
			}
		}

		if (unlocked > 0)
		{
			character.AddonMessage(AddonMessage.ADD_CHAT_EMOTICON);
		}

		return ItemUseResult.Okay;
	}

	/// <summary>
	/// Increase an entry count for Challenge Mode: Division Singularity?
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="strArg"></param>
	/// <param name="numArg1"></param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ChallengeExpertModeCountUp(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.ModifyAccountProperty(PropertyName.ChallengeMode_HardMode_EnableEntryCount, 1);
		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, ScpArgMsg("ChallengeExpertModeReset_MSG2"));

		return ItemUseResult.Okay;
	}

	/// <summary>
	/// Summons a friendly monster
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="strArg"></param>
	/// <param name="numArg1"></param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_SUMMONORB_FRIEND(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (!ZoneServer.Instance.Data.MonsterDb.TryFind(strArg, out var monsterData))
			return ItemUseResult.Fail;
		var monster = new Mob(monsterData.Id, RelationType.Friendly);

		monster.Faction = FactionType.Summon;
		monster.Position = character.Position;

		monster.OwnerHandle = character.Handle;
		monster.Components.Add(new LifeTimeComponent(monster, TimeSpan.FromSeconds(180)));
		monster.Components.Add(new MovementComponent(monster));
		monster.Components.Add(new AiComponent(monster, "AlcheSummon"));

		character.Map.AddMonster(monster);

		return ItemUseResult.Okay;
	}

	/// <summary>
	/// Summons an unfriendly monster
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="strArg"></param>
	/// <param name="numArg1"></param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_SUMMONORB_ENEMY(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (!ZoneServer.Instance.Data.MonsterDb.TryFind(strArg, out var monsterData))
			return ItemUseResult.Fail;
		var monster = new Mob(monsterData.Id, RelationType.Enemy);

		monster.Faction = FactionType.Summon;
		monster.Position = character.Position;

		monster.Components.Add(new LifeTimeComponent(monster, TimeSpan.FromSeconds(180)));
		monster.Components.Add(new MovementComponent(monster));
		monster.Components.Add(new AiComponent(monster, "AlcheSummon"));

		character.Map.AddMonster(monster);

		return ItemUseResult.Okay;
	}

	/// <summary>
	/// Use hair color spray
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="strArg"></param>
	/// <param name="numArg1"></param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_HAIRCOLOR(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var etcColor = $"HairColor_{strArg}";

		if (character.Etc.Properties[etcColor] == 1)
		{
			character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, ScpArgMsg("HairColorExist"), 2);
			return ItemUseResult.Fail;
		}

		SetAllowHairColor(character, etcColor);

		character.AddonMessage(AddonMessage.HAIR_COLOR_CHANGE, item.Data.ClassName, 100);

		return ItemUseResult.Okay;
	}

	public static bool HaveHairColor(Character character, string color)
	{
		var etc = character.Etc.Properties;
		var nowAllowedColor = etc.GetString("AllowedHairColor");

		if (nowAllowedColor.Contains(color) || etc[$"HairColor_{color}"] == 1)
			return true;
		return false;
	}

	public static void SetAllowHairColor(Character character, string newColor)
	{
		string[] colorTable = { "default", "black", "blue", "white", "pink", "blond", "red", "green", "gray", "lightsalmon", "purple", "orange", "midnightblue" };
		var index = -1;
		for (var i = 0; i < colorTable.Length; i++)
		{
			if (newColor == colorTable[i])
			{
				index = i;
				break;
			}
		}

		if (index == -1)
			return;

		var etcColor = $"HairColor_{colorTable[index]}";
		var etc = character.Etc.Properties;

		if (etc[etcColor] != 1)
			character.SetEtcProperty(etcColor, 1);
	}

	public static void SetAllowHairColor(Character character, string color, string achieveName)
	{
		var etc = character.Etc.Properties;
		var etcPropName = "AchieveReward_" + achieveName;

		if (etc == null || etc[etcPropName] == 1)
		{
			return;
		}

		if (HaveHairColor(character, color))
		{
			character.SetEtcProperty(etcPropName, 1);
			character.SystemMessage("GainAchieveHairBefore");
			character.AddonMessage("ACHIEVE_REWARD");
			return;
		}

		SetAllowHairColor(character, color);

		character.SystemMessage($"GainAchieveHair{color.ToUpperInvariant()[0] + color.Substring(1)}");
		character.AddonMessage("ACHIEVE_REWARD", "", 0);
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_STRING_GIVE_ITEM_NUMBER_SPLIT(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var items = ParseItems(strArg);

		foreach (var parsedItem in items)
			character.AddItem(parsedItem.Key, parsedItem.Value);

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_STRING_GIVE_ITEM_NUMBER_SPLIT_COLLECTION(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var items = ParseItems(strArg);

		foreach (var parsedItem in items)
			character.AddItem(parsedItem.Key, parsedItem.Value);

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_DRESS_ROOM_COLLECTION_GET(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		// Collections not implemented.
		return ItemUseResult.Fail;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_AddHP1(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var hpHeal = (int)numArg1;
		if (numArg2 != 0)
			hpHeal = RandomProvider.Get().Next((int)numArg1, (int)numArg2);

		if (character.Properties.TryGetFloat(PropertyName.HPPotion_BM, out var hpPotionBM) && hpPotionBM > 0)
			hpHeal = (int)MathF.Floor(hpHeal * (1 + hpPotionBM / 100));

		character.Heal(hpHeal, 0);

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_AddSP1(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var spHeal = (int)numArg1;
		if (numArg2 != 0)
			spHeal = RandomProvider.Get().Next((int)numArg1, (int)numArg2);

		if (character.Properties.TryGetFloat(PropertyName.SPPotion_BM, out var spPotionBM) && spPotionBM > 0)
			spHeal = (int)MathF.Floor(spHeal * (1 + spPotionBM / 100));

		if (character.IsBuffActiveByKeyword(BuffTag.Curse))
			spHeal = 0;

		character.Heal(0, spHeal);

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_AddSTA1(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var stamina = (int)numArg1 * 1000;
		var staminaMax = (int)numArg2 * 1000;
		if (staminaMax != 0)
			stamina = RandomProvider.Get().Next(stamina, staminaMax);

		if (character.Properties.TryGetFloat(PropertyName.STAPotion_BM, out var staminaPotionBM) && staminaPotionBM > 0)
			stamina = (int)MathF.Floor(stamina * (1 + staminaPotionBM / 100));

		character.ModifyStamina(stamina);

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_AddHPSP1(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var hpPoint = RandomProvider.Get().Next((int)numArg1, (int)numArg2);
		var spPoint = RandomProvider.Get().Next((int)numArg1, (int)numArg2) / 2;

		if (character.Properties.TryGetFloat(PropertyName.SPPotion_BM, out var spPotionBM) && spPotionBM > 0)
			spPoint = (int)MathF.Floor(spPoint * (1 + spPotionBM / 100));

		if (character.Properties.TryGetFloat(PropertyName.HPPotion_BM, out var hpPotionBM) && hpPotionBM > 0)
			hpPoint = (int)MathF.Floor(hpPoint * (1 + hpPotionBM / 100));

		if (character.IsBuffActiveByKeyword(BuffTag.Curse))
			spPoint = 0;

		character.Heal(hpPoint, spPoint);

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_AddTeamBuff(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (!ZoneServer.Instance.Data.BuffDb.TryFind(strArg, out var buff))
			return ItemUseResult.Fail;

		character.StartBuff(buff.Id, numArg1, 0, TimeSpan.FromMilliseconds(numArg2), character);
		var partyMembers = character.Map.GetPartyMembersInRange(character, 300);

		foreach (var partyMember in partyMembers)
		{
			if (partyMember == character)
				continue;
			partyMember.StartBuff(buff.Id, numArg1, 0, TimeSpan.FromMilliseconds(numArg2), character);
		}

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult DLC_BOX1(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("steam_Premium_tpBox_650", 1, "DLC_BOX1");
		character.AddItem("steam_PremiumToken_60d", 1, "DLC_BOX1");
		character.AddItem("steam_JOB_HOGLAN_COUPON", 1, "DLC_BOX1");
		character.AddItem("steam_Hat_629003", 1, "DLC_BOX1");
		character.AddItem("steam_Hat_629004", 1, "DLC_BOX1");
		character.AddItem("Premium_SkillReset", 1, "DLC_BOX1");
		character.AddItem("steam_Premium_StatReset", 1, "DLC_BOX1");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult DLC_BOX2(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("steam_Premium_tpBox_380", 1, "DLC_BOX2");
		character.AddItem("steam_PremiumToken_30day", 1, "DLC_BOX2");
		character.AddItem("steam_Hat_629004", 1, "DLC_BOX2");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult DLC_BOX3(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("steam_Premium_tpBox_160", 1, "DLC_BOX3");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult GIVE_MIC_10(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("Mic", 10, "TPSHOP_MIC_50");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult GIVE_ENCHANTSCROLL_10(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("Premium_Enchantchip", 10, "TPSHOP_ENCHANTSCROLL_20");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult DLC_BOX4(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("steam_Premium_tpBox_650", 1, "DLC_BOX4");
		character.AddItem("steam_PremiumToken_60d", 1, "DLC_BOX4");
		character.AddItem("steam_JOB_HOGLAN_COUPON", 1, "DLC_BOX4");
		character.AddItem("steam_Hat_629003", 1, "DLC_BOX4");
		character.AddItem("steam_Hat_629004", 1, "DLC_BOX4");
		character.AddItem("Premium_SkillReset", 1, "DLC_BOX4");
		character.AddItem("steam_Premium_StatReset", 1, "DLC_BOX4");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult DLC_BOX5(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("steam_Premium_tpBox_380", 1, "DLC_BOX5");
		character.AddItem("steam_PremiumToken_30day", 1, "DLC_BOX5");
		character.AddItem("steam_Hat_629004", 1, "DLC_BOX5");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult DLC_BOX6(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("steam_Premium_tpBox_190", 1, "DLC_BOX6");
		character.AddItem("PremiumToken_15d", 1, "DLC_BOX6");
		character.AddItem("RestartCristal", 15, "DLC_BOX6");
		character.AddItem("Premium_boostToken", 5, "DLC_BOX6");
		character.AddItem("Mic", 15, "DLC_BOX6");
		character.AddItem("Premium_WarpScroll", 15, "DLC_BOX6");
		character.AddItem("Drug_Premium_HP1", 20, "DLC_BOX6");
		character.AddItem("Drug_Premium_SP1", 20, "DLC_BOX6");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult DLC_BOX7(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("steam_PremiumToken_30day", 1, "DLC_BOX7");
		character.AddItem("Premium_Enchantchip_10", 4, "DLC_BOX7");
		character.AddItem("Premium_indunReset", 5, "DLC_BOX7");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_BUYPOINT(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		// TODO: Figure out what exactly is World PVP Property?
		//character.AddWorldPvpProperty("ShopPoint", (int)numArg1);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_PENGUINPACK_2016(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("egg_006", 1, "PENGUINPACK_2016");
		character.AddItem("food_penguin", 50, "PENGUINPACK_2016");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ADVENTURERPACK_2016(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("Premium_indunReset", 1, "ADVENTURERPACK_2016");
		character.AddItem("Premium_boostToken", 3, "ADVENTURERPACK_2016");
		character.AddItem("Event_drug_steam_1h", 2, "ADVENTURERPACK_2016");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult DLC_BOX8(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("PremiumToken_15d", 1, "DLC_BOX8");
		character.AddItem("Premium_eventTpBox_65", 1, "DLC_BOX8");
		character.AddItem("Drug_Premium_HP1", 30, "DLC_BOX8");
		character.AddItem("Drug_Premium_SP1", 30, "DLC_BOX8");
		character.AddItem("RestartCristal", 10, "DLC_BOX8");
		character.AddItem("Mic", 10, "DLC_BOX8");
		character.AddItem("Premium_WarpScroll", 10, "DLC_BOX8");
		character.AddItem("Drug_STA1", 30, "DLC_BOX8");
		character.AddItem("Drug_Haste2_DLC", 30, "DLC_BOX8");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_SET01_COMPANION_STEAM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("egg_009", 1, "EGG009_PACK");
		character.AddItem("food_cereal", 50, "EGG009_PACK");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ALICEPACK_2016(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var ctrlType = character.Job.Id.ToClass();
		var jobList = new List<JobClass>
		{
			JobClass.Swordsman,
			JobClass.Wizard,
			JobClass.Archer,
			JobClass.Cleric
		};
		var jobNum = jobList.IndexOf(ctrlType);

		if (jobNum == -1)
			return ItemUseResult.Fail;

		var costumes = new string[,]
		{
				{ "costume_war_m_011", "costume_war_f_011" },
				{ "costume_wiz_m_011", "costume_wiz_f_011" },
				{ "costume_arc_m_012", "costume_arc_f_012" },
				{ "costume_clr_m_012", "costume_clr_f_012" }
		};

		var costumeName = costumes[jobNum, (int)character.Gender - 1];

		character.AddItem(costumeName, 1, "ALICEPACK_2016");
		character.AddItem("AliceHairBox_2016", 1, "ALICEPACK_2016");
		character.AddItem("Premium_Enchantchip", 10, "ALICEPACK_2016");

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ALICEHAIRBOX_2016(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		// All the boilerplate is now inside the helper function!
		character.StartDialog(item, async (dialog) =>
		{
			var selection = await dialog.Select(ScpArgMsg("SUMMERHAIR_2016_SEL"),
				ScpArgMsg("ALICEHAIRACC01"), ScpArgMsg("ALICEHAIRACC02"), ScpArgMsg("ALICEHAIRACC03"),
				ScpArgMsg("ALICEHAIRACC04"), ScpArgMsg("ALICEHAIRACC05"), ScpArgMsg("ALICEHAIRACC06"),
				ScpArgMsg("ALICEHAIRACC07"), ScpArgMsg("ALICEHAIRACC08"), ScpArgMsg("Cancel"));

			var summerHairs = new List<string>
		{
			"Hat_628152", "Hat_628153", "Hat_628154", "Hat_628155",
			"Hat_628156", "Hat_628157", "Hat_628158", "Hat_628159"
		};

			if (selection >= 1 && selection <= 8)
			{
				character.AddItem(summerHairs[selection - 1], 1, "ALICEHAIRBOX_2016");
				character.ConsumeItem(item.ObjectId, 1);
			}
		});

		return ItemUseResult.OkayNotConsumed;
	}

	[ScriptableFunction]
	public static ItemUseResult ACHIEVE_HAUNTEDARTIST(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddAchievementPoint("HauntedArtist", 1);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult DLC_BOX9(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("Drug_Premium_HP1", 100, "DLC_BOX9");
		character.AddItem("Drug_Premium_SP1", 100, "DLC_BOX9");
		character.AddItem("Event_160908_7", 10, "DLC_BOX9");
		character.AddItem("Event_drug_160218", 10, "DLC_BOX9");
		character.AddItem("Premium_Clear_dungeon_01", 5, "DLC_BOX9");
		character.AddItem("steam_Premium_SkillReset_1", 1, "DLC_BOX9");
		character.AddItem("steam_Premium_StatReset_1", 1, "DLC_BOX9");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult DLC_BOX10(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("PremiumToken_7d_Steam", 1, "DLC_BOX10");
		character.AddItem("Premium_Enchantchip14", 10, "DLC_BOX10");
		character.AddItem("steam_emoticonItem_24_46", 1, "DLC_BOX10");
		character.AddItem("Event_ArborDay_Costume_Box", 1, "DLC_BOX10");
		character.AddItem("E_Artefact_630005", 1, "DLC_BOX10");
		character.AddItem("Hat_629501", 1, "DLC_BOX10");
		character.AddItem("Premium_hairColor_05", 1, "DLC_BOX10");
		character.AddItem("LENS01_003", 1, "DLC_BOX10");
		character.AddItem("Event_drug_steam_1h", 10, "DLC_BOX10");
		character.AddItem("GIMMICK_Drug_HPSP2", 20, "DLC_BOX10");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_AddBuff_Item(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		BuffId buffNameToAdd;
		if (character.IsBuffActive(BuffId.Premium_Fortunecookie_5))
			buffNameToAdd = BuffId.Premium_Fortunecookie_5;
		else if (character.IsBuffActive(BuffId.Premium_Fortunecookie_4))
			buffNameToAdd = BuffId.Premium_Fortunecookie_5;
		else if (character.IsBuffActive(BuffId.Premium_Fortunecookie_3))
			buffNameToAdd = BuffId.Premium_Fortunecookie_4;
		else if (character.IsBuffActive(BuffId.Premium_Fortunecookie_2))
			buffNameToAdd = BuffId.Premium_Fortunecookie_3;
		else if (character.IsBuffActive(BuffId.Premium_Fortunecookie_1))
			buffNameToAdd = BuffId.Premium_Fortunecookie_2;
		else
			buffNameToAdd = BuffId.Premium_Fortunecookie_1;

		character.StartBuff(buffNameToAdd, (int)numArg1, 0, TimeSpan.FromMilliseconds((int)numArg2), character);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_LETICIA_MONSTERGEM_BOX03(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var items = new List<(string, int)>
			{
				("Gem_Hoplite_Stabbing", 30), ("Gem_Hoplite_Pierce", 30), ("Gem_Hoplite_Finestra", 40),
				("Gem_Hoplite_SynchroThrusting", 30), ("Gem_Hoplite_LongStride", 30), ("Gem_Hoplite_SpearLunge", 30),
				("Gem_Hoplite_ThrouwingSpear", 30), ("Gem_Barbarian_Embowel", 20), ("Gem_Barbarian_StompingKick", 20),
				("Gem_Barbarian_Cleave", 20), ("Gem_Barbarian_HelmChopper", 30), ("Gem_Barbarian_Warcry", 40),
				("Gem_Barbarian_Frenzy", 40), ("Gem_Barbarian_Seism", 40), ("Gem_Barbarian_GiantSwing", 40),
				("Gem_Barbarian_Pouncing", 30), ("Gem_Bokor_Hexing", 30), ("Gem_Bokor_Effigy", 30),
				("Gem_Bokor_Zombify", 30), ("Gem_Bokor_Mackangdal", 30), ("Gem_Bokor_BwaKayiman", 30),
				("Gem_Bokor_Samdiveve", 30), ("Gem_Bokor_Ogouveve", 30), ("Gem_Bokor_Damballa", 40),
				("Gem_Dievdirbys_CarveVakarine", 40), ("Gem_Dievdirbys_CarveZemina", 40), ("Gem_Dievdirbys_CarveLaima", 40),
				("Gem_Dievdirbys_Carve", 40), ("Gem_Dievdirbys_CarveOwl", 40), ("Gem_Dievdirbys_CarveAustrasKoks", 25),
				("Gem_Dievdirbys_CarveAusirine", 25),
			};
		GiveWeightedRandomItem(character, items, "SCR_USE_LETICIA_MONSTERGEM_BOX03");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_LETICIA_MONSTERGEM_BOX04(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var items = new List<(string, int)>
			{
				("Gem_Psychokino_PsychicPressure", 40), ("Gem_Psychokino_Telekinesis", 40), ("Gem_Psychokino_Swap", 40),
				("Gem_Psychokino_Teleportation", 40), ("Gem_Psychokino_MagneticForce", 40), ("Gem_Psychokino_Raise", 40),
				("Gem_Psychokino_GravityPole", 40), ("Gem_Linker_Physicallink", 40), ("Gem_Linker_JointPenalty", 30),
				("Gem_Linker_HangmansKnot", 30), ("Gem_Linker_SpiritualChain", 40), ("Gem_Linker_UmbilicalCord", 40),
				("Gem_Sapper_StakeStockades", 30), ("Gem_Sapper_Cover", 30), ("Gem_Sapper_Claymore", 40),
				("Gem_Sapper_PunjiStake", 30), ("Gem_Sapper_DetonateTraps", 30), ("Gem_Sapper_BroomTrap", 50),
				("Gem_Sapper_CollarBomb", 20), ("Gem_Sapper_SpikeShooter", 20), ("Gem_Hunter_Coursing", 30),
				("Gem_Hunter_Snatching", 30), ("Gem_Hunter_Pointing", 30), ("Gem_Hunter_RushDog", 40),
				("Gem_Hunter_Retrieve", 40), ("Gem_Hunter_Hounding", 40), ("Gem_Hunter_Growling", 40)
			};
		GiveWeightedRandomItem(character, items, "SCR_USE_LETICIA_MONSTERGEM_BOX04");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_EVENT_NRU_BOX_1(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddItem("Event_Nru_Always_Box_2", 1, "EV170516_NRU");
		character.AddItem("BRC99_103", 1, "EV170516_NRU");
		character.AddItem("BRC99_104", 1, "EV170516_NRU");
		character.AddItem("NECK99_107", 1, "EV170516_NRU");
		character.AddItem("Event_drug_steam_1h", 10, "EV170516_NRU");
		character.AddItem("Drug_Fortunecookie", 5, "EV170516_NRU");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_RETURNQUEST_1705(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var account = character.Connection.Account;
		var todayDayOfYear = DateTime.Now.DayOfYear;

		if (account.Properties.GetFloat(PropertyName.EVENT_RETURN_COUNT) >= 10)
		{
			character.AddonMessage("NOTICE_Dm_!", ScpArgMsg("BLACK_HOLE_CLEAR_BOX_MSG2"));
			return ItemUseResult.Okay;
		}
		if (account.Properties.GetFloat(PropertyName.EVENT_RETURN_DAY) == todayDayOfYear)
		{
			character.AddonMessage("NOTICE_Dm_!", ScpArgMsg("Event_Returner_Desc01"));
			return ItemUseResult.Okay;
		}

		character.StartDialog(item, async (dialog) =>
		{
			var rewards = new (string itemName, int count)[]
			{
			("Premium_boostToken03_event01", 1), ("Premium_indunReset_14d", 2),
			("Premium_dungeoncount_Event", 3), ("CS_IndunReset_GTower_14d", 1),
			("CS_IndunReset_Nunnery_14d", 1)
			};

			var selection = await dialog.Select(ScpArgMsg("NPC_EVENT_RETURNER_DLG1"),
				ScpArgMsg("Auto_DaeHwa_JongLyo"), ScpArgMsg("Premium_boostToken03_event01"),
				ScpArgMsg("Premium_indunReset_14d"), ScpArgMsg("Dungeoncount_14d_Name"),
				ScpArgMsg("CS_IndunReset_GTower_14d"), ScpArgMsg("CS_IndunReset_Nunnery_14d"));

			if (selection > 1 && selection <= rewards.Length + 1)
			{
				var (itemName, count) = rewards[selection - 2];
				character.AddItem(itemName, count, "RETURNER_1705");
				character.AddItem("Event_drug_steam_1h", 3, "RETURNER_1705");

				character.SetAccountProperty(PropertyName.EVENT_RETURN_DAY, DateTime.Now.DayOfYear);
				character.ModifyAccountProperty(PropertyName.EVENT_RETURN_COUNT, 1);

				var newReturnCount = character.Connection.Account.Properties.GetFloat(PropertyName.EVENT_RETURN_COUNT);
				character.AddonMessage("NOTICE_Dm_Clear", ScpArgMsg("LevelUp_Event_Desc01", "REWARD", newReturnCount));

				character.ConsumeItem(item.ObjectId, 1);
			}
		});

		return ItemUseResult.OkayNotConsumed;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_LETICIA_MONSTERGEM_1RANK(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var items = new List<string> {
				"Gem_Swordman_Thrust", "Gem_Swordman_Bash", "Gem_Swordman_GungHo", "Gem_Swordman_Concentrate",
				"Gem_Swordman_PainBarrier", "Gem_Swordman_Restrain", "Gem_Swordman_PommelBeat", "Gem_Swordman_DoubleSlash",
				"Gem_Wizard_EnergyBolt", "Gem_Wizard_Lethargy", "Gem_Wizard_Sleep", "Gem_Wizard_ReflectShield",
				"Gem_Wizard_EarthQuake", "Gem_Wizard_Surespell", "Gem_Wizard_MagicMissile", "Gem_Archer_SwiftStep",
				"Gem_Archer_Multishot", "Gem_Archer_Fulldraw", "Gem_Archer_ObliqueShot", "Gem_Archer_Kneelingshot",
				"Gem_Archer_HeavyShot", "Gem_Archer_TwinArrows", "Gem_Cleric_Heal", "Gem_Cleric_Cure",
				"Gem_Cleric_SafetyZone", "Gem_Cleric_DeprotectedZone", "Gem_Cleric_DivineMight", "Gem_Cleric_Fade", "Gem_Cleric_PatronSaint"
			};
		GiveUniformRandomItem(character, items, "LETICIA_MONSTERGEM_1RANK");
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_ITEM_LVUPCARD(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var maxLevel = (int)numArg1;
		var currentLevel = character.Level;
		var currentXpInLevel = character.Exp;

		if (currentLevel > maxLevel)
		{
			character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, ScpArgMsg("PILGRIM_48_SQ_030_MSG03"), 3);
			return ItemUseResult.Okay;
		}

		var expForNextLevel = character.MaxExp;
		var expToGive = expForNextLevel - currentXpInLevel;
		var jobExpToGive = (long)Math.Floor(expToGive * 0.77);

		character.GiveExp(expToGive, jobExpToGive, null);

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult ACHIEVE_Steam_Magazine_Num1(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddAchievementPoint("EVENT_Steam_Magazine_Num1", 1);

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult Achieve_Event_Grand_Contributor(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddAchievementPoint("Achieve_Event_Grand_Contributor", 1);

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult SCR_USE_DIAMOND_MOUR_BOX_GIVE_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		// Lua: (pc, string1, arg1, arg2) -> TxGiveItem(tx, arg1, arg2, '...'.arg2)
		// C#: (character, item, strArg, numArg1, numArg2)
		// Mapping: strArg -> arg1 (item name), numArg1 -> arg2 (count)
		var count = (int)numArg1;
		character.AddItem(strArg, count, $"DIAMOND_MOUR_BOX_S_GIVE_COUNT{count}");

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public static ItemUseResult Achieve_Event_Weapon_Designer(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddAchievementPoint("Achieve_Event_Weapon_Designer", 1);

		return ItemUseResult.Okay;
	}
}
