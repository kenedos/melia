using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using static Melia.Zone.Scripting.Shortcuts;

public class StatBoostItemScripts : GeneralScript
{
	// Cap constants for dungeon drop potions
	private const int MaxHpPotionUses = 20;      // +50 HP per use = +1000 HP max
	private const int MaxSpPotionUses = 20;      // +20 SP per use = +400 SP max
	private const int MaxStatPotionPoints = 100; // +1 stat point per use = 100 max

	// Variable names for tracking usage
	private const string HpPotionUsesVar = "Melia.HpPotionUses";
	private const string SpPotionUsesVar = "Melia.SpPotionUses";
	private const string StatPotionPointsVar = "Melia.StatPotionPoints";

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
		=> IncreaseStatBonus(character, "MaxWeight_Bonus", numArg1);

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
}
