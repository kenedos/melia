//--- Melia Script ----------------------------------------------------------
// Character Calculations, 2016
//--- Description -----------------------------------------------------------
// The character formulas of the 2016 game, ported from its calc_property_pc
// script. The functions in calc_character.cs switch to them whenever the
// LaimaFormulas feature is disabled.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

public static class CharacterFormulas2016
{
	/// <summary>
	/// Returns the diminishing bonus the game adds on top of a base stat.
	/// </summary>
	/// <param name="stat"></param>
	/// <returns></returns>
	private static float AddStat(float stat)
	{
		float value;

		if (stat < 51)
			value = stat / 5f;
		else if (stat < 151)
			value = 50 / 5f + (stat - 50) / 4f;
		else if (stat < 301)
			value = 50 / 5f + 100 / 4f + (stat - 150) / 3f;
		else if (stat < 501)
			value = 50 / 5f + 100 / 4f + 150 / 3f + (stat - 300) / 2f;
		else
			value = 50 / 5f + 100 / 4f + 150 / 3f + 200 / 2f + (stat - 500);

		return (float)Math.Floor(value);
	}

	/// <summary>
	/// Returns the stat before the diminishing bonus and equipment.
	/// </summary>
	private static float BaseStat(Character character, string job, string stat, string bonus, string temp, string name)
	{
		var properties = character.Properties;

		var byJob = properties.GetFloat(job);
		var byStat = properties.GetFloat(stat);
		var byBonus = properties.GetFloat(bonus);
		var byTemp = character.Variables.Temp.GetFloat(temp);
		var byReward = character.Variables.Perm.GetFloat(name) + character.Quests.GetRewardProperty(name);

		return byJob + byStat + byBonus + byTemp + byReward;
	}

	private static float FinalStat(float value)
		=> (float)Math.Floor(Math.Max(1, value));

	private static float JobScale(Character character, float value)
		=> value + value * (character.Jobs.Count - 1) * 0.1f;

	private static float LeftHand(Character character, string propertyName)
		=> character.Inventory.GetItem(EquipSlot.LeftHand)?.Properties.GetFloat(propertyName, 0) ?? 0;

	private static float RightHand(Character character, string propertyName)
		=> character.Inventory.GetItem(EquipSlot.RightHand)?.Properties.GetFloat(propertyName, 0) ?? 0;

	private static bool Is(Character character, JobClass jobClass)
		=> character.JobClass == jobClass;

	public static float STR_JOB(Character character)
		=> character.Job?.Data.Str ?? 1;

	public static float CON_JOB(Character character)
		=> character.Job?.Data.Con ?? 1;

	public static float INT_JOB(Character character)
		=> character.Job?.Data.Int ?? 1;

	public static float MNA_JOB(Character character)
		=> character.Job?.Data.Spr ?? 1;

	public static float DEX_JOB(Character character)
		=> character.Job?.Data.Dex ?? 1;

	public static float STR(Character character)
	{
		var baseStat = BaseStat(character, PropertyName.STR_JOB, PropertyName.STR_STAT, PropertyName.STR_Bonus, PropertyName.STR_TEMP, PropertyName.STR);
		var value = JobScale(character, baseStat + AddStat(baseStat));

		return FinalStat((float)Math.Floor(value + character.Properties.GetFloat(PropertyName.STR_ADD)));
	}

	public static float INT(Character character)
	{
		var baseStat = BaseStat(character, PropertyName.INT_JOB, PropertyName.INT_STAT, PropertyName.INT_Bonus, PropertyName.INT_TEMP, PropertyName.INT);
		var value = JobScale(character, baseStat + AddStat(baseStat));

		return FinalStat((float)Math.Floor(value + character.Properties.GetFloat(PropertyName.INT_ADD)));
	}

	public static float DEX(Character character)
	{
		var baseStat = BaseStat(character, PropertyName.DEX_JOB, PropertyName.DEX_STAT, PropertyName.DEX_Bonus, PropertyName.DEX_TEMP, PropertyName.DEX);

		return FinalStat((float)Math.Floor(baseStat + character.Properties.GetFloat(PropertyName.DEX_ADD) + AddStat(baseStat)));
	}

	public static float CON(Character character)
	{
		var baseStat = BaseStat(character, PropertyName.CON_JOB, PropertyName.CON_STAT, PropertyName.CON_Bonus, PropertyName.CON_TEMP, PropertyName.CON);

		return FinalStat((float)Math.Floor(baseStat + character.Properties.GetFloat(PropertyName.CON_ADD) + AddStat(baseStat)));
	}

	public static float MNA(Character character)
	{
		var baseStat = BaseStat(character, PropertyName.MNA_JOB, PropertyName.MNA_STAT, PropertyName.MNA_Bonus, PropertyName.MNA_TEMP, PropertyName.MNA);

		return FinalStat((float)Math.Floor(baseStat + character.Properties.GetFloat(PropertyName.MNA_ADD) + AddStat(baseStat)));
	}

	public static float MHP(Character character)
	{
		var properties = character.Properties;

		var level = properties.GetFloat(PropertyName.Lv, 1);
		var con = properties.GetFloat(PropertyName.CON, 1);

		var byItemRatio = 1f + character.Inventory.GetEquipProperties(PropertyName.MHPRatio) * 0.01f;
		var byItem = character.Inventory.GetEquipProperties(PropertyName.MHP);
		var byBuff = properties.GetFloat(PropertyName.MHP_BM);
		var byLevel = (float)Math.Floor((level - 1) * 8.5f * 2);
		var byStat = (float)Math.Floor(con * 85);
		var byReward = character.Quests.GetRewardProperty(PropertyName.MHP);

		var jobRate = character.Job?.Data.HpRate ?? 1;
		var value = ((jobRate * byLevel) + byStat) * byItemRatio + byItem + properties.GetFloat(PropertyName.MHP_Bonus) + byBuff + byReward;

		return (int)Math.Max(1, value);
	}

	public static float MSP(Character character)
	{
		var properties = character.Properties;

		var level = properties.GetFloat(PropertyName.Lv, 1);
		var mna = properties.GetFloat(PropertyName.MNA, 1);

		var byItem = character.Inventory.GetEquipProperties(PropertyName.MSP);
		var byBuff = properties.GetFloat(PropertyName.MSP_BM);
		var byLevel = (float)Math.Floor((level - 1) * 6.7f);
		var byStat = (float)Math.Floor(mna * 13);
		var byReward = character.Quests.GetRewardProperty(PropertyName.MSP);

		var addSp = Is(character, JobClass.Cleric) ? level * 1.675f : 0;

		var jobRate = character.Job?.Data.SpRate ?? 1;
		var value = (jobRate * byLevel) + byItem + byStat + properties.GetFloat(PropertyName.MSP_Bonus) + byBuff + addSp + byReward;

		if (value < 1)
			value = 0;

		return (int)value;
	}

	public static float MaxSta(Character character)
	{
		var properties = character.Properties;

		var byItem = character.Inventory.GetEquipProperties(PropertyName.MSTA);
		var byBonus = properties.GetFloat(PropertyName.MAXSTA_Bonus, 0);
		var byBuff = properties.GetFloat(PropertyName.MaxSta_BM, 0);
		var byReward = character.Quests.GetRewardProperty(PropertyName.MSTA);

		return (int)((25 + byItem + byBonus + byBuff + byReward) * 1000);
	}

	public static float Sta_Run(Character character)
	{
		var value = 50f;

		if (character.Buffs.Has(BuffId.RootCrystalMoveSpeed))
			return 0;

		var dashRun = character.Properties.GetFloat(PropertyName.DashRun, 0);
		if (dashRun > 0)
		{
			var dashAmount = 500f;
			if (dashRun == 2)
				dashAmount *= 0.9f;

			value += dashAmount;
		}

		return (int)(250f * value / 100f);
	}

	public static float Sta_Recover(Character character)
	{
		if (character.IsBuffActiveByKeyword(BuffTag.Curse))
			return 0;

		var properties = character.Properties;

		var value = 400 + properties.GetFloat(PropertyName.REST_BM, 0) + properties.GetFloat(PropertyName.RSta_BM, 0);

		if (character.Buffs.Has(BuffId.SitRest))
			value *= 2;

		return (int)value;
	}

	public static float Sta_Jump(Character character)
		=> 0;

	public static float RHP(Character character)
	{
		if (character.IsBuffActiveByKeyword(BuffTag.Curse))
			return 0;

		var properties = character.Properties;

		var level = properties.GetFloat(PropertyName.Lv, 1);
		var con = properties.GetFloat(PropertyName.CON, 1);
		var jobRate = character.Job?.Data.HpRate ?? 1;

		var byItem = character.Inventory.GetEquipProperties(PropertyName.RHP);
		var value = jobRate * level * 0.5f + con + properties.GetFloat(PropertyName.RHP_BM) + byItem;

		if (value < 1)
			value = 0;

		return (int)value;
	}

	public static float RHPTIME(Character character)
	{
		var value = 20000 - character.Properties.GetFloat(PropertyName.RHPTIME_BM);

		if (character.Buffs.Has(BuffId.SitRest))
			value /= 2;

		return (int)Math.Max(1000, value);
	}

	public static float RSP(Character character)
	{
		if (character.IsBuffActiveByKeyword(BuffTag.Curse, BuffTag.Formation, BuffTag.SpDrain))
			return 0;

		var properties = character.Properties;

		var level = properties.GetFloat(PropertyName.Lv, 1);
		var mna = properties.GetFloat(PropertyName.MNA, 1);
		var jobRate = character.Job?.Data.SpRate ?? 1;

		var addRsp = Is(character, JobClass.Cleric) ? level / 4f : 0;
		var byItem = character.Inventory.GetEquipProperties(PropertyName.RSP);

		var value = jobRate * level * 0.5f + mna + addRsp + properties.GetFloat(PropertyName.RSP_BM) + byItem;

		if (value < 1)
			value = 1;

		if (character.Buffs.Has(BuffId.Summoning_Buff))
			value = 0;

		return (int)value;
	}

	public static float RSPTIME(Character character)
	{
		var value = 20000 - character.Properties.GetFloat(PropertyName.RSPTIME_BM);

		if (character.Buffs.Has(BuffId.SitRest))
			value /= 2;

		if (value < 0)
			value = 1000;

		return (int)value;
	}

	public static float MaxWeight(Character character)
	{
		var properties = character.Properties;

		var con = properties.GetFloat(PropertyName.CON);
		var str = properties.GetFloat(PropertyName.STR);

		var value = 5000 + properties.GetFloat(PropertyName.MaxWeight_BM) + properties.GetFloat(PropertyName.MaxWeight_Bonus) + (con * 5) + (str * 5) + character.Quests.GetRewardProperty(PropertyName.MaxWeight);

		return value;
	}

	public static float MINPATK(Character character)
	{
		var properties = character.Properties;

		var str = properties.GetFloat(PropertyName.STR, 1);
		if (Is(character, JobClass.Swordsman))
			str *= 1.3f;

		var inventory = character.Inventory;
		var byItem = inventory.GetEquipProperties(PropertyName.MINATK) + inventory.GetEquipProperties(PropertyName.PATK) + inventory.GetEquipProperties(PropertyName.ADD_MINATK);
		var level = properties.GetFloat(PropertyName.Lv, 1);

		var throwItem = character.Buffs.Has(BuffId.Warrior_RH_VisibleObject) ? RightHand(character, PropertyName.MINATK) : 0;

		var value = level + str + byItem + properties.GetFloat(PropertyName.PATK_BM) - LeftHand(character, PropertyName.MINATK) - throwItem;

		return (int)Math.Max(1, value);
	}

	public static float MAXPATK(Character character)
	{
		var properties = character.Properties;

		var str = properties.GetFloat(PropertyName.STR, 1);
		if (Is(character, JobClass.Swordsman))
			str *= 1.3f;

		var inventory = character.Inventory;
		var byItem = inventory.GetEquipProperties(PropertyName.MAXATK) + inventory.GetEquipProperties(PropertyName.PATK) + inventory.GetEquipProperties(PropertyName.ADD_MAXATK);
		var level = properties.GetFloat(PropertyName.Lv, 1);

		var throwItem = character.Buffs.Has(BuffId.Warrior_RH_VisibleObject) ? RightHand(character, PropertyName.MAXATK) : 0;

		var value = level + str + byItem + properties.GetFloat(PropertyName.PATK_BM) + properties.GetFloat(PropertyName.MAXPATK_BM) - LeftHand(character, PropertyName.MAXATK) - throwItem;

		return (int)Math.Max(1, value);
	}

	public static float MINPATK_SUB(Character character)
	{
		var properties = character.Properties;

		var str = properties.GetFloat(PropertyName.STR, 1);
		if (Is(character, JobClass.Swordsman))
			str *= 1.3f;

		var inventory = character.Inventory;
		var byItem = inventory.GetEquipProperties(PropertyName.MINATK) + inventory.GetEquipProperties(PropertyName.PATK) + inventory.GetEquipProperties(PropertyName.ADD_MINATK);
		var level = properties.GetFloat(PropertyName.Lv, 1);

		var value = level + str + byItem + properties.GetFloat(PropertyName.PATK_BM) - RightHand(character, PropertyName.MINATK);

		return (int)Math.Max(1, value);
	}

	public static float MAXPATK_SUB(Character character)
	{
		var properties = character.Properties;

		var str = properties.GetFloat(PropertyName.STR, 1);
		if (Is(character, JobClass.Swordsman))
			str *= 1.3f;

		var inventory = character.Inventory;
		var byItem = inventory.GetEquipProperties(PropertyName.MAXATK) + inventory.GetEquipProperties(PropertyName.PATK) + inventory.GetEquipProperties(PropertyName.ADD_MAXATK);
		var level = properties.GetFloat(PropertyName.Lv, 1);

		var value = level + str + byItem + properties.GetFloat(PropertyName.PATK_BM) + properties.GetFloat(PropertyName.MAXPATK_SUB_BM) - RightHand(character, PropertyName.MAXATK);

		return (int)Math.Max(1, value);
	}

	public static float MINMATK(Character character)
	{
		var properties = character.Properties;

		var inventory = character.Inventory;
		var byItem = inventory.GetEquipProperties(PropertyName.MATK) + inventory.GetEquipProperties(PropertyName.ADD_MATK) + inventory.GetEquipProperties(PropertyName.ADD_MINATK);
		var level = properties.GetFloat(PropertyName.Lv, 1);

		var throwItem = character.Buffs.Has(BuffId.Warrior_RH_VisibleObject) ? RightHand(character, PropertyName.MATK) : 0;

		var value = level + properties.GetFloat(PropertyName.INT, 1) + byItem + properties.GetFloat(PropertyName.MATK_BM) - throwItem;

		return (int)Math.Max(1, value);
	}

	public static float MAXMATK(Character character)
	{
		var properties = character.Properties;

		var inventory = character.Inventory;
		var byItem = inventory.GetEquipProperties(PropertyName.MATK) + inventory.GetEquipProperties(PropertyName.ADD_MATK) + inventory.GetEquipProperties(PropertyName.ADD_MAXATK);
		var level = properties.GetFloat(PropertyName.Lv, 1);

		var throwItem = character.Buffs.Has(BuffId.Warrior_RH_VisibleObject) ? RightHand(character, PropertyName.MATK) : 0;

		var value = level + properties.GetFloat(PropertyName.INT, 1) + byItem + properties.GetFloat(PropertyName.MATK_BM) - throwItem;

		return (int)Math.Max(1, value);
	}

	public static float DEF(Character character)
	{
		var properties = character.Properties;

		var level = properties.GetFloat(PropertyName.Lv, 1);
		var byItem = character.Inventory.GetEquipProperties(PropertyName.DEF) + character.Inventory.GetEquipProperties(PropertyName.ADD_DEF);
		var byBuff = properties.GetFloat(PropertyName.DEF_BM);
		var byLevel = (float)Math.Floor(level / 2);
		var addDef = Is(character, JobClass.Swordsman) ? level / 4f : 0;

		var normal = byItem + byLevel + addDef;
		var rankBonus = (float)Math.Floor(normal * ((character.Jobs.Count - 1) * 0.1f));
		var value = normal + rankBonus + byBuff + properties.GetFloat(PropertyName.MAXDEF_Bonus);

		if (value < 1)
			value = 0;

		return (int)value;
	}

	public static float MDEF(Character character)
	{
		var properties = character.Properties;

		var level = properties.GetFloat(PropertyName.Lv, 1);
		var byItem = character.Inventory.GetEquipProperties(PropertyName.MDEF) + character.Inventory.GetEquipProperties(PropertyName.ADD_MDEF);
		var byLevel = level * 0.5f;
		var byStat = properties.GetFloat(PropertyName.MNA, 1) / 5f;
		var addDef = Is(character, JobClass.Wizard) ? level / 4f : 0;

		var normal = byLevel + byItem + addDef + byStat;
		var rankBonus = (float)Math.Floor(normal * ((character.Jobs.Count - 1) * 0.1f));
		var value = normal + rankBonus + properties.GetFloat(PropertyName.MDEF_BM);

		return (int)value;
	}

	public static float BLK(Character character)
	{
		var properties = character.Properties;

		var blockRate = character.Inventory.GetEquipProperties(PropertyName.BlockRate);
		var crossGuard = character.IsBuffActive(BuffId.CrossGuard_Buff) || character.IsBuffActive(BuffId.StoneSkin_Buff);

		var isShield = blockRate > 0;
		if (!isShield && !crossGuard)
			return 0;

		var level = properties.GetFloat(PropertyName.Lv, 1);
		var shield = Math.Min(blockRate, 1f);
		if (Is(character, JobClass.Swordsman))
			shield *= 2;

		var value = 0f;
		if (isShield)
			value = level * 0.5f + properties.GetFloat(PropertyName.CON, 1) + character.Inventory.GetEquipProperties(PropertyName.BLK) + shield * level * 0.03f;

		value = (float)Math.Floor(value + properties.GetFloat(PropertyName.BLK_BM));

		if (character.IsGuarding())
			value += (float)Math.Floor(level * 5.5f);

		return (int)Math.Max(0, value);
	}

	public static float BLK_BREAK(Character character)
	{
		var properties = character.Properties;

		var level = properties.GetFloat(PropertyName.Lv, 1);
		var str = properties.GetFloat(PropertyName.STR, 1);
		var mna = properties.GetFloat(PropertyName.MNA, 1);

		var value = level * 0.5f + mna + character.Inventory.GetEquipProperties(PropertyName.BLK_BREAK) + properties.GetFloat(PropertyName.BLK_BREAK_BM) + str;
		if (Is(character, JobClass.Swordsman))
			value += str * 0.2f;

		return (int)value;
	}

	public static float HR(Character character)
	{
		var properties = character.Properties;

		var level = properties.GetFloat(PropertyName.Lv, 1);
		var byItem = character.Inventory.GetEquipProperties(PropertyName.HR) + character.Inventory.GetEquipProperties(PropertyName.ADD_HR);
		var addHr = Is(character, JobClass.Archer) ? (level + 4) / 4f : 0;

		var value = properties.GetFloat(PropertyName.DEX, 1) + level + addHr + byItem + properties.GetFloat(PropertyName.HR_BM);

		return (int)value;
	}

	public static float DR(Character character)
	{
		var properties = character.Properties;

		var level = properties.GetFloat(PropertyName.Lv, 1);
		var byItem = character.Inventory.GetEquipProperties(PropertyName.DR) + character.Inventory.GetEquipProperties(PropertyName.ADD_DR);
		var addDr = Is(character, JobClass.Archer) ? level / 8f : 0;

		var value = properties.GetFloat(PropertyName.DEX, 1) + level + addDr + byItem + properties.GetFloat(PropertyName.DR_BM);

		return (int)value;
	}

	public static float CRTHR(Character character)
	{
		var properties = character.Properties;

		var level = properties.GetFloat(PropertyName.Lv, 1);
		var addCrthr = Is(character, JobClass.Archer) ? level / 5f : 0;

		var value = properties.GetFloat(PropertyName.DEX, 1) + character.Inventory.GetEquipProperties(PropertyName.CRTHR) + addCrthr + properties.GetFloat(PropertyName.CRTHR_BM);

		return (int)value;
	}

	public static float CRTDR(Character character)
	{
		var properties = character.Properties;

		var value = properties.GetFloat(PropertyName.CON, 1) + character.Inventory.GetEquipProperties(PropertyName.CRTDR) + properties.GetFloat(PropertyName.CRTDR_BM);

		return (int)value;
	}

	public static float CRTATK(Character character)
	{
		var properties = character.Properties;

		var value = properties.GetFloat(PropertyName.STR, 1) + character.Inventory.GetEquipProperties(PropertyName.CRTATK) + properties.GetFloat(PropertyName.CRTATK_BM);

		return (int)value;
	}

	public static float SR(Character character)
	{
		var baseValue = 3;
		if (Is(character, JobClass.Swordsman))
			baseValue = 4;
		else if (Is(character, JobClass.Archer))
			baseValue = 0;

		var byItem = character.Inventory.GetEquipProperties(PropertyName.SR);
		var byBuffs = character.Properties.GetFloat(PropertyName.SR_BM);

		return (int)(baseValue + byItem + byBuffs);
	}

	public static float MSPD(Character character)
	{
		var properties = character.Properties;

		var value = 30f;
		var byItem = character.Inventory.GetEquipProperties(PropertyName.MSPD);

		value += byItem + properties.GetFloat(PropertyName.MSPD_BM);
		value *= (100 + properties.GetFloat(PropertyName.SPD_BM)) / 100f;

		var nowWeight = properties.GetFloat(PropertyName.NowWeight);
		var maxWeight = properties.GetFloat(PropertyName.MaxWeight);
		if (nowWeight >= maxWeight)
			value /= 3;

		if (value > 60)
			value = 60;

		value += properties.GetFloat(PropertyName.MSPD_Bonus);

		return (int)Math.Max(1, value);
	}
}
