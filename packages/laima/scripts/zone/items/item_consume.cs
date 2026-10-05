using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using static Melia.Zone.Scripting.Shortcuts;

public class StatBoostItemScripts : GeneralScript
{
	// Cap constants for dungeon drop potions
	private const int MaxHpPotionUses = 40;			// +50 HP per use = 2000 HP max
	private const int MaxSpPotionUses = 40;			// +20 SP per use = +800 SP max
	private const int MaxStatPotionPoints = 100;    // +1 stat point per use = 100 max
	private const int MaxSkillPointPotionUses = 15; // +1 skill point per use = 15 max

	private const int MaxWeightIncreasePotionUses = 20; // +1000 weight increase per use = 20000 max

	// Variable names for tracking usage
	private const string HpPotionUsesVar = "Melia.HpPotionUses";
	private const string SpPotionUsesVar = "Melia.SpPotionUses";
	private const string StatPotionPointsVar = "Melia.StatPotionPoints";
	private const string SkillPointPotionUsesVar = "Melia.SkillPointPotionUses";
	private const string MaxWeightIncreasePotionUsesVar = "Melia.MaxWeightIncreasePotionUses";

	private ItemUseResult IncreaseStatBonus(Character character, string propertyName, float amount, string effect = null)
	{
		character.Properties.Modify(propertyName, amount);
		if (!string.IsNullOrEmpty(effect))
		{
			character.PlayEffect(effect, 6, 1, EffectLocation.Bottom, 1);
		}
		character.InvalidateProperties();
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_MaxSTAUP(Character character, Item item, string strArg, float numArg1, float numArg2)
		=> IncreaseStatBonus(character, "MAXSTA_Bonus", numArg1);

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_MaxHPUP(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var currentUses = character.Variables.Perm.GetInt(HpPotionUsesVar);
		if (currentUses >= MaxHpPotionUses)
		{
			character.AddonMessage("NOTICE_Dm_Clear", $"You have reached the maximum HP bonus from potions ({MaxHpPotionUses}/{MaxHpPotionUses}).", 3);
			return ItemUseResult.Fail;
		}

		var newUses = currentUses + 1;
		character.Variables.Perm.SetInt(HpPotionUsesVar, newUses);
		character.PlayEffect("F_pc_status_con_up", 6, 1, EffectLocation.Bottom, 1);
		character.AddonMessage("NOTICE_Dm_Clear", $"HP increased! ({newUses}/{MaxHpPotionUses})", 3);
		return IncreaseStatBonus(character, "MHP_Bonus", numArg1);
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_MaxSPUP(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var currentUses = character.Variables.Perm.GetInt(SpPotionUsesVar);
		if (currentUses >= MaxSpPotionUses)
		{
			character.AddonMessage("NOTICE_Dm_Clear", $"You have reached the maximum SP bonus from potions ({MaxSpPotionUses}/{MaxSpPotionUses}).", 3);
			return ItemUseResult.Fail;
		}

		var newUses = currentUses + 1;
		character.Variables.Perm.SetInt(SpPotionUsesVar, newUses);
		character.PlayEffect("F_pc_status_mna_up", 6, 1, EffectLocation.Bottom, 1);
		character.AddonMessage("NOTICE_Dm_Clear", $"SP increased! ({newUses}/{MaxSpPotionUses})", 3);
		return IncreaseStatBonus(character, "MSP_Bonus", numArg1);
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_MaxATKUP(Character character, Item item, string strArg, float numArg1, float numArg2)
		=> IncreaseStatBonus(character, "MATK_Bonus", numArg1);

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_MaxDEFUP(Character character, Item item, string strArg, float numArg1, float numArg2)
		=> IncreaseStatBonus(character, "MAXDEF_Bonus", numArg1);

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_MaxWeightUP(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var currentUses = character.Variables.Perm.GetInt(MaxWeightIncreasePotionUsesVar);
		if (currentUses >= MaxWeightIncreasePotionUses)
		{
			character.AddonMessage("NOTICE_Dm_Clear", $"You have reached the maximum Weight Increase bonus from potions ({MaxWeightIncreasePotionUses}/{MaxWeightIncreasePotionUses}).", 3);
			return ItemUseResult.Fail;
		}

		var newUses = currentUses + 1;
		character.Variables.Perm.SetInt(MaxWeightIncreasePotionUsesVar, newUses);
		character.AddonMessage("NOTICE_Dm_Clear", $"Max Weight increased! ({newUses}/{MaxWeightIncreasePotionUses})", 3);
		return IncreaseStatBonus(character, "MaxWeight_Bonus", numArg1);
	}

	// Primary Stats
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_STRUP(Character character, Item item, string strArg, float numArg1, float numArg2)
		=> IncreaseStatBonus(character, "STR_Bonus", numArg1, "F_pc_status_str_up");

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_DEXUP(Character character, Item item, string strArg, float numArg1, float numArg2)
		=> IncreaseStatBonus(character, "DEX_Bonus", numArg1, "F_pc_status_dex_up");

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_CONUP(Character character, Item item, string strArg, float numArg1, float numArg2)
		=> IncreaseStatBonus(character, "CON_Bonus", numArg1, "F_pc_status_con_up");

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_INTUP(Character character, Item item, string strArg, float numArg1, float numArg2)
		=> IncreaseStatBonus(character, "INT_Bonus", numArg1, "F_pc_status_int_up");

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_MNAUP(Character character, Item item, string strArg, float numArg1, float numArg2)
		=> IncreaseStatBonus(character, "MNA_Bonus", numArg1, "F_pc_status_mna_up");

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_STATUP(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var currentPoints = character.Variables.Perm.GetInt(StatPotionPointsVar);
		if (currentPoints >= MaxStatPotionPoints)
		{
			character.AddonMessage("NOTICE_Dm_Clear", $"You have reached the maximum stat points from potions ({MaxStatPotionPoints}/{MaxStatPotionPoints}).", 3);
			return ItemUseResult.Fail;
		}

		var newPoints = currentPoints + 1;
		character.Variables.Perm.SetInt(StatPotionPointsVar, newPoints);
		character.AddStatPoints(1);
		character.PlayEffect("F_pc_StatPoint_up", 4, 1, EffectLocation.Bottom, 1);
		character.AddonMessage("NOTICE_Dm_Clear", $"Stat point gained! ({newPoints}/{MaxStatPotionPoints})", 3);
		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemTxResult SCR_USE_ITEM_INDUN_COUNT_RESET(Character character,Item item,int[] numArgs)
	{
		if (item == null)
			return ItemTxResult.Fail;

		if (!character.Inventory.TryGetItem(item.ObjectId, out _))
			return ItemTxResult.Fail;

		if (item.Id != ItemId.Premium_IndunReset)
			return ItemTxResult.Fail;

		if (item.IsExpired || item.IsLocked)
			return ItemTxResult.Fail;

		if (!ZoneServer.Instance.DungeonReset.TryRestoreOneDailyEntry(character))
		{
			character.AddonMessage(
				"NOTICE_Dm_Clear",
				"You don't have any consumed Instanced Dungeon entries.",
				3);

			return ItemTxResult.Fail;
		}

		character.AddonMessage(
			"NOTICE_Dm_Clear",
			"One Instanced Dungeon entry has been restored.",
			3);

		return ItemTxResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_161215EVENT_SEED(Character character,Item item,string strArg,float numArg1,float numArg2)
	{
		character.StartBuff(BuffId.Event_CharExpRate,50,0,TimeSpan.FromMinutes(30),character,0);

		character.PlayEffect("F_pc_status_con_up",6,1,EffectLocation.Bottom,1);

		character.AddonMessage("NOTICE_Dm_Clear","Miracle Seed activated for 30 minutes.",3);

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_ADD_SKILL_POINT(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var currentUses = character.Variables.Perm.GetInt(SkillPointPotionUsesVar);

		if (currentUses >= MaxSkillPointPotionUses)
		{
			character.AddonMessage("NOTICE_Dm_Clear", $"You have reached the Skill Point Potion usage limit ({MaxSkillPointPotionUses}/{MaxSkillPointPotionUses}).", 3);
			return ItemUseResult.Fail;
		}

		var jobs = character.Jobs.GetList().ToArray();

		if (jobs.Length == 0)
		{
			character.AddonMessage("NOTICE_Dm_Clear", "No jobs were found for this character.", 3);
			return ItemUseResult.Fail;
		}

		foreach (var job in jobs)
		{
			if (!character.Jobs.ModifySkillPoints(job.Id, 1))
			{
				character.AddonMessage("NOTICE_Dm_Clear", $"Unable to add a skill point to {job.Id}.", 3);
				return ItemUseResult.Fail;
			}
		}

		var newUses = currentUses + 1;

		character.Variables.Perm.SetInt(SkillPointPotionUsesVar, newUses);
		character.PlayEffect("F_pc_StatPoint_up", 4, 1, EffectLocation.Bottom, 1);
		character.AddonMessage("NOTICE_Dm_Clear", $"Skill point gained for all unlocked jobs! ({newUses}/{MaxSkillPointPotionUses})", 3);

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemTxResult SCR_USE_ITEM_PREMIUM_TOKEN(Character character, Item item, int[] numArgs)
	{
		const int normalExpTomeItemId = 490015;
		const int x4ExpTomeItemId = 490094;
		const int x8ExpTomeItemId = 490095;

		if (character == null || item == null)
			return ItemTxResult.Fail;

		if (!character.Inventory.TryGetItem(item.ObjectId, out _))
			return ItemTxResult.Fail;

		if (item.IsExpired || item.IsLocked)
			return ItemTxResult.Fail;

		var itemId = (int)item.Id;
		BuffId buffId;
		float expRate;
		TimeSpan duration;
		string tomeName;

		switch (itemId)
		{
			case normalExpTomeItemId:
				buffId = BuffId.Premium_boostToken;
				expRate = 30f;
				duration = TimeSpan.FromHours(1);
				tomeName = "EXP Tome";
				break;
			case x4ExpTomeItemId:
				buffId = BuffId.Premium_boostToken02;
				expRate = 150f;
				duration = TimeSpan.FromHours(1);
				tomeName = "x4 EXP Tome";
				break;
			case x8ExpTomeItemId:
				buffId = BuffId.Premium_boostToken03;
				expRate = 300f;
				duration = TimeSpan.FromHours(1);
				tomeName = "x8 EXP Tome";
				break;
			default:
				return ItemTxResult.Fail;
		}

		character.StopBuff(BuffId.Premium_boostToken);
		character.StopBuff(BuffId.Premium_boostToken02);
		character.StopBuff(BuffId.Premium_boostToken03);

		character.StartBuff(buffId, expRate, 0f, duration, character, 0);
		character.PlayEffect("F_pc_status_con_up", 6, 1, EffectLocation.Bottom, 1);
		character.AddonMessage("NOTICE_Dm_Clear", $"{tomeName} activated: +{expRate:0}% EXP for {duration.TotalMinutes:0} minutes.", 3);

		return ItemTxResult.Okay;
	}
}
