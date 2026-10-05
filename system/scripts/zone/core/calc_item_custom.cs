//--- Melia Script ----------------------------------------------------------
// Combat Calculation Script
//--- Description -----------------------------------------------------------
// Functions that calculate item-related values, such as generate options.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Network;
using Melia.Zone;
using Melia.Zone.Buffs.Handlers;
using Melia.Zone.Buffs.Handlers.Swordsman.Highlander;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Yggdrasil.Extensions;
using Yggdrasil.Logging;
using Yggdrasil.Util;

public class ItemCalculationsScript : GeneralScript
{
	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	//[ScriptableFunction]
	public float SCR_Get_ItemAttack(Item item)
	{
		return 0;
		//return value;
	}

	[ScriptableFunction]
	public float SCR_Get_Item_MAXATK(Item item)
	{
		var lv = item.UseLevel;
		if (lv == 0)
			return 0;

		var hiddenLv = item.HiddenLevel;
		if (hiddenLv > 0)
			lv = hiddenLv;

		if (!item.TryGetProp(PropertyName.ItemGrade, out float grade))
			return 0;

		//lv = SCR_PVP_ITEM_LV_GRADE_REINFORCE_SET(item, lv, grade);

		var gradeRatio = 0f;
		if (ZoneServer.Instance.Data.ItemGradeDb.TryFindByGrade((int)grade, out var itemGradeData))
			gradeRatio = itemGradeData.BasicRatio / 100f;
		var itemATK = (100 + lv * 1.5f) * gradeRatio;
		if (lv == 0)
			itemATK = 0;

		var slot = item.Data.EquipSlot;
		if (string.IsNullOrEmpty(slot))
			return 0;

		var classType = item.Data.EquipType1;

		if (classType == EquipType.Pistol || classType == EquipType.Dagger)
			itemATK = itemATK * 0.5f;

		var weaponClass = ZoneServer.Instance.Data.ItemGradeDb.FindValue("WeaponClassTypeRatio", classType);
		if (weaponClass == 0)
			return 0;

		var damageRange = ZoneServer.Instance.Data.ItemGradeDb.FindValue("WeaponDamageRange", classType);
		if (damageRange == 0)
			return 0;

		if (itemGradeData != null && weaponClass > 0)
			itemATK *= weaponClass;

		var upper440BonusRatio = ZoneServer.Instance.Data.ItemGradeDb.FindValue("Upper440LevelClassTypeRatioIncrease", classType);
		if (upper440BonusRatio != 0 && lv >= 440)
			itemATK *= upper440BonusRatio;

		var changeBasicProp = item.Properties.GetFloat(PropertyName.ChangeBasicPropValue);
		if (changeBasicProp > 0)
			itemATK = changeBasicProp;

		var maxAtk = itemATK * damageRange;
		//Log.Debug("Calculated Max ATK: {0}", maxAtk);
		return MathF.Round(maxAtk + GetReinforceAddValueAtk(item, PropertyName.ATK), MidpointRounding.AwayFromZero);
	}



	[ScriptableFunction]
	public float SCR_Get_Item_MINATK(Item item)
	{
		var lv = item.UseLevel;
		if (lv == 0)
			return 0;

		var hiddenLv = item.HiddenLevel;
		if (hiddenLv > 0)
			lv = hiddenLv;

		if (!item.TryGetProp(PropertyName.ItemGrade, out float grade))
			return 0;

		//lv = SCR_PVP_ITEM_LV_GRADE_REINFORCE_SET(item, lv, grade);

		var gradeRatio = 0f;
		if (ZoneServer.Instance.Data.ItemGradeDb.TryFindByGrade((int)grade, out var itemGradeData))
			gradeRatio = itemGradeData.BasicRatio / 100f;
		var itemATK = (100 + lv * 1.5f) * gradeRatio;
		if (lv == 0)
			itemATK = 0;

		var slot = item.Data.EquipSlot;
		if (string.IsNullOrEmpty(slot))
			return 0;

		var classType = item.Data.EquipType1;

		if (classType == EquipType.Pistol || classType == EquipType.Dagger)
			itemATK = itemATK * 0.5f;

		var weaponClass = ZoneServer.Instance.Data.ItemGradeDb.FindValue("WeaponClassTypeRatio", classType);
		if (weaponClass == 0)
			return 0;

		var damageRange = ZoneServer.Instance.Data.ItemGradeDb.FindValue("WeaponDamageRange", classType);
		if (damageRange == 0)
			return 0;

		if (itemGradeData != null && weaponClass > 0)
			itemATK *= weaponClass;

		var upper440BonusRatio = ZoneServer.Instance.Data.ItemGradeDb.FindValue("Upper440LevelClassTypeRatioIncrease", classType);
		if (upper440BonusRatio != 0 && lv >= 440)
			itemATK *= upper440BonusRatio;

		var changeBasicProp = item.Properties.GetFloat(PropertyName.ChangeBasicPropValue);
		if (changeBasicProp > 0)
			itemATK = changeBasicProp;

		var minAtk = itemATK * (2 - damageRange);
		//Log.Debug("Calculated Min ATK: {0} + {1}", minAtk, GetReinforceAddValueAtk(item, PropertyName.ATK));
		return MathF.Round(minAtk + GetReinforceAddValueAtk(item, PropertyName.ATK), MidpointRounding.AwayFromZero);
	}

	[ScriptableFunction]
	public float SCR_Get_Item_MATK(Item item)
	{
		var lv = item.UseLevel;
		if (lv == 0)
			return 0;

		var hiddenLv = item.HiddenLevel;
		if (hiddenLv > 0)
			lv = hiddenLv;

		if (!item.TryGetProp(PropertyName.ItemGrade, out float grade))
			return 0;

		//lv = SCR_PVP_ITEM_LV_GRADE_REINFORCE_SET(item, lv, grade);

		var gradeRatio = 0f;
		if (ZoneServer.Instance.Data.ItemGradeDb.TryFindByGrade((int)grade, out var itemGradeData))
			gradeRatio = itemGradeData.BasicRatio / 100f;
		var itemATK = (100 + lv * 1.5f) * gradeRatio;
		if (lv == 0)
			itemATK = 0;

		var slot = item.Data.EquipSlot;
		if (string.IsNullOrEmpty(slot))
			return 0;

		var classType = item.Data.EquipType1;

		var weaponClass = ZoneServer.Instance.Data.ItemGradeDb.FindValue("WeaponClassTypeRatio", classType);
		if (weaponClass == 0)
			return 0;

		var damageRange = ZoneServer.Instance.Data.ItemGradeDb.FindValue("WeaponDamageRange", classType);
		if (damageRange == 0)
			return 0;

		if (itemGradeData != null && weaponClass > 0)
			itemATK *= weaponClass;

		var upper440BonusRatio = ZoneServer.Instance.Data.ItemGradeDb.FindValue("Upper440LevelClassTypeRatioIncrease", classType);
		if (upper440BonusRatio != 0 && lv >= 440)
			itemATK *= upper440BonusRatio;

		var changeBasicProp = item.Properties.GetFloat(PropertyName.ChangeBasicPropValue);
		if (changeBasicProp > 0)
			itemATK = changeBasicProp;

		//Log.Debug("Calculated MATK: {0}", itemATK);
		return MathF.Round(itemATK + GetReinforceAddValueAtk(item, PropertyName.MATK), MidpointRounding.AwayFromZero);
	}

	/// <summary>
	/// Calculate an item's DEF
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_Item_DEF(Item item)
	{
		var lv = item.UseLevel;
		if (lv == 0)
			return 0;

		var hiddenLv = item.HiddenLevel;
		if (hiddenLv > 0)
			lv = hiddenLv;

		if (!item.TryGetProp(PropertyName.ItemGrade, out float grade))
			return 0;

		//lv = SCR_PVP_ITEM_LV_GRADE_REINFORCE_SET(item, lv, grade);

		var gradeRatio = 0f;
		if (ZoneServer.Instance.Data.ItemGradeDb.TryFindByGrade((int)grade, out var itemGradeData))
			gradeRatio = itemGradeData.BasicRatio / 100f;

		var equipMaterial = item.Data.Material;
		var classType = item.Data.EquipType1;

		if (classType == EquipType.BELT || classType == EquipType.SHOULDER)
			equipMaterial = ArmorMaterialType.Leather;

		if (equipMaterial == ArmorMaterialType.None)
			return 0;

		var slot = item.Data.EquipSlot;
		if (string.IsNullOrEmpty(slot))
			return 0;

		var armorClass = ZoneServer.Instance.Data.ItemGradeDb.FindValue("ArmorClassTypeRatio", classType);
		if (armorClass == 0)
			return 0;

		var basicDef = ((100f + lv * 1.5f) * armorClass) * gradeRatio;

		if (lv >= 440)
		{
			var upper440BonusRatio = ZoneServer.Instance.Data.ItemGradeDb.FindValue("Upper440LevelClassTypeRatioIncrease", classType);
			if (upper440BonusRatio != 0)
				basicDef *= upper440BonusRatio;
		}

		var changeBasicProp = item.Properties.GetFloat(PropertyName.ChangeBasicPropValue);
		if (changeBasicProp > 0)
			basicDef = changeBasicProp;

		var armorMaterialRatio = ZoneServer.Instance.Data.ItemGradeDb.FindValue("armorMaterial_DEF", equipMaterial);
		if (armorMaterialRatio == 0)
			return 0;

		basicDef *= armorMaterialRatio;

		basicDef = MathF.Floor(basicDef);
		//Log.Debug("Calculated DEF: {0}", basicDef);
		return MathF.Floor(basicDef + GetReinforceAddValueAtk(item, PropertyName.DEF));
	}

	/// <summary>
	/// Calculate an item's MDEF
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_Item_MDEF(Item item)
	{
		var lv = item.UseLevel;
		if (lv == 0)
			return 0;

		var hiddenLv = item.HiddenLevel;
		if (hiddenLv > 0)
			lv = hiddenLv;

		if (!item.TryGetProp(PropertyName.ItemGrade, out float grade))
			return 0;

		//lv = SCR_PVP_ITEM_LV_GRADE_REINFORCE_SET(item, lv, grade);

		var gradeRatio = 0f;
		if (ZoneServer.Instance.Data.ItemGradeDb.TryFindByGrade((int)grade, out var itemGradeData))
			gradeRatio = itemGradeData.BasicRatio / 100f;

		var slot = item.Data.EquipSlot;
		if (string.IsNullOrEmpty(slot))
			return 0;

		var equipMaterial = item.Data.Material;
		var classType = item.Data.EquipType1;

		if (classType == EquipType.BELT || classType == EquipType.SHOULDER)
			equipMaterial = ArmorMaterialType.Leather;

		if (equipMaterial == ArmorMaterialType.None)
			return 0;

		var armorClass = ZoneServer.Instance.Data.ItemGradeDb.FindValue("ArmorClassTypeRatio", classType);
		if (armorClass == 0)
			return 0;

		var basicMDef = ((100f + lv * 1.5f) * armorClass) * gradeRatio;

		var upper440BonusRatio = ZoneServer.Instance.Data.ItemGradeDb.FindValue("Upper440LevelClassTypeRatioIncrease", classType);
		if (upper440BonusRatio != 0 && lv >= 440)
			basicMDef *= upper440BonusRatio;

		var changeBasicProp = item.Properties.GetFloat(PropertyName.ChangeBasicPropValue);
		if (changeBasicProp > 0)
			basicMDef = changeBasicProp;

		var armorMaterialRatio = ZoneServer.Instance.Data.ItemGradeDb.FindValue("armorMaterial_MDEF", equipMaterial);
		if (armorMaterialRatio == 0)
			return 0;

		basicMDef *= armorMaterialRatio;

		basicMDef = MathF.Floor(basicMDef);
		//Log.Debug("Calculated MDEF: {0}", basicMDef);
		return MathF.Floor(basicMDef + GetReinforceAddValueAtk(item, PropertyName.MDEF));
	}

	/// <summary>
	/// Calculate an item's repair cost based on
	/// equipment type, level, and rank.
	/// </summary>
	[ScriptableFunction]
	public float SCR_Get_Item_RepairPrice(Item item)
	{
		// Base price
		var basePrice = 100f;

		// Level multiplier calculation (smoothed exponential-to-linear curve)
		var level = item.UseLevel;
		var baseMultiplier = 1f;
		var exponentialFactor = 0.04f;
		var linearFactor = 25f;
		var transitionLevel = 100f;
		var transitionSmoothness = 50f;

		var exponentialPart = Math.Pow(1 + exponentialFactor, level);
		var linearPart = 1 + (level / linearFactor);
		var transitionFactor = 1 / (1 + Math.Exp(-(level - transitionLevel) / transitionSmoothness));
		var levelMultiplier = baseMultiplier * ((float)exponentialPart * (1 - (float)transitionFactor) + (float)linearPart * (float)transitionFactor);

		// Grade multiplier (20% per grade linear increase)
		var itemGrade = (int)item.Properties.GetFloat(PropertyName.ItemGrade);
		var gradeMultiplier = 1 + (itemGrade - 1) * 0.2f;

		// Type multiplier
		var typeMultiplier = 1.0f;
		var equipType = item.Data.Group;
		switch (equipType)
		{
			case ItemGroup.Weapon:
				typeMultiplier = 1.5f;
				break;
			case ItemGroup.SubWeapon:
				typeMultiplier = 1.4f;
				break;
			case ItemGroup.Armor:
				typeMultiplier = 1.2f;
				break;
		}

		// Calculate final repair cost
		var repairCost = basePrice * levelMultiplier * gradeMultiplier * typeMultiplier;

		// Round down to nearest integer
		return (float)Math.Floor(repairCost);
	}

	[ScriptableFunction]
	public float SCR_Get_Item_SocketPrice(Item item)
	{
		// Get the item level and grade
		var itemLevel = item.UseLevel;
		if (itemLevel <= 0)
			return 0;

		if (!item.Properties.TryGetFloat(PropertyName.ItemGrade, out var grade))
			return 0;

		// Get current socket count
		var currentSockets = item.GetUsedSockets();

		// Define grade ratios as per Lua implementation
		float[] gradeRatios = { 1.2f, 1f, 0.5f, 0.4f, 0.3f, 0.1f };

		// Calculate grade multiplier
		var itemGradeRatio = 1f;
		var priceMultiplier = 2f;
		var basePrice = 10000;

		if (currentSockets >= 1)
		{
			// Adjust grade index to 0-based for C# array
			var gradeIndex = (int)grade - 1;
			if (gradeIndex >= 0 && gradeIndex < gradeRatios.Length)
				itemGradeRatio = gradeRatios[gradeIndex];
		}

		// Find the base socket price from the socket price database
		var socketPriceDb = ZoneServer.Instance.Data.SocketPriceDb;
		if (socketPriceDb == null)
			return 0;

		foreach (var priceData in socketPriceDb.Entries.Values)
		{
			if (priceData.ItemLevel == itemLevel)
			{
				var priceRatio = currentSockets + 1;
				// Calculate final price using the same formula as Lua
				// In Lua: cls.NewSocketPrice * secretNumber * (priceRatio ^ (1 / itemGradeRatio))
				var finalPrice = basePrice + priceData.AddPrice *
							 priceMultiplier *
							 ((float)Math.Pow(priceRatio, 2) / itemGradeRatio);

				return (float)Math.Floor(finalPrice);
			}
		}

		return 0;
	}

	public float GetReinforceAddValueAtk(Item item, string prop)
	{
		var basicTooltipProp = item.Data.MainProperties;

		if (string.IsNullOrEmpty(basicTooltipProp) || !basicTooltipProp.Contains(prop))
			return 0;

		var lv = item.Level;
		if (lv <= 0) return 0;

		if (!item.Properties.TryGetFloat(PropertyName.ItemGrade, out var grade))
			return 0;
		if (!item.Properties.TryGetFloat(PropertyName.Reinforce_2, out var reinforceValue))
			return 0;

		var reinforceRatio = item.Properties.GetFloat(PropertyName.ReinforceRatio, 100);

		//(lv, grade, reinforceValue, reinforceRatio) = ScrPvpItemLvGradeReinforceSet(item, lv, grade, reinforceValue, reinforceRatio);
		//var gradeRatio = ScrGetItemGradeRatio(grade, "ReinforceRatio");
		var gradeRatio = 0f;
		if (ZoneServer.Instance.Data.ItemGradeDb.TryFindByGrade((int)grade, out var itemGradeData))
			gradeRatio = itemGradeData.ReinforceRatio;

		//reinforceValue += reinfBonusValue;
		var value = (float)Math.Floor(reinforceValue + (lv * (reinforceValue * (0.08f + (float)Math.Floor((Math.Min(21, reinforceValue) - 1) / 5) * 0.015f))));
		value *= (reinforceRatio / 100) * (gradeRatio / 100);

		var classType = item.Data.EquipType1;
		if (classType == EquipType.Trinket) value *= 0.5f;
		if (classType == EquipType.Neck || classType == EquipType.Ring) value *= 0.25f;

		return (int)Math.Round(value, MidpointRounding.AwayFromZero);
	}


	[ScriptableFunction]
	public float SCR_Get_Item_AppraisalPrice(Item item)
	{
		var sellPrice = item.Data.SellPrice;
		// Get the item level and grade
		var level = item.UseLevel;
		if (level <= 0)
			return 0;

		if (!item.Properties.TryGetFloat(PropertyName.ItemGrade, out var grade))
			return 0;

		// Default price ratio 
		var priceRatio = 10;

		// Calculate the base sell price if not provided
		if (sellPrice == 0)
		{
			if (grade <= 2)
			{
				sellPrice = level * priceRatio;
			}
			else if (grade > 2)
			{
				sellPrice = (int)Math.Floor(level * priceRatio * 1.5);
			}
			else
			{
				return 0;
			}
		}

		return sellPrice;
	}
}
