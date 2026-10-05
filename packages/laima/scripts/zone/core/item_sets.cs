//--- Melia Script ----------------------------------------------------------
// Item Set Functions
//--- Description -----------------------------------------------------------
// Scriptable functions that handle item set bonus effects.
// Each set has a single script function that receives old and new piece
// counts, allowing it to handle all threshold logic internally.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Data.Database;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;

public class ItemSetFunctionsScript : GeneralScript
{
	#region Helper Functions

	private static bool CrossedUp(int oldCount, int newCount, int threshold)
		=> oldCount < threshold && newCount >= threshold;

	private static bool CrossedDown(int oldCount, int newCount, int threshold)
		=> oldCount >= threshold && newCount < threshold;

	private static void AddAllStats(Character c, float amount)
	{
		c.Properties.Modify("STR_BM", amount);
		c.Properties.Modify("DEX_BM", amount);
		c.Properties.Modify("INT_BM", amount);
		c.Properties.Modify("CON_BM", amount);
	}

	private static void AddAllItemStats(Character c, float amount)
	{
		c.Properties.Modify("STR_ITEM_BM", amount);
		c.Properties.Modify("DEX_ITEM_BM", amount);
		c.Properties.Modify("CON_ITEM_BM", amount);
		c.Properties.Modify("INT_ITEM_BM", amount);
		c.Properties.Modify("MNA_ITEM_BM", amount);
	}

	private static void Modify(Character c, string prop, float amount)
		=> c.Properties.Modify(prop, amount);

	private static void AddBuff(Character c, string buffClassName)
	{
		if (ZoneServer.Instance.Data.BuffDb.TryFind(a => a.ClassName == buffClassName, out var buffData))
			c.Buffs.Start(buffData.Id, 0, 0, TimeSpan.Zero, c);
	}

	private static void RemoveBuff(Character c, string buffClassName)
	{
		if (ZoneServer.Instance.Data.BuffDb.TryFind(a => a.ClassName == buffClassName, out var buffData))
			c.Buffs.Remove(buffData.Id);
	}

	#endregion

	// =========================================================================
	// SET_001 - Light Plate Set
	// =========================================================================
	[ScriptableFunction("SCR_set_001")]
	public static void SCR_set_001(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "SR_BM", 1);
		else if (CrossedDown(o, n, 2)) Modify(c, "SR_BM", -1);
	}

	// =========================================================================
	// SET_002 - Zega Set
	// =========================================================================
	[ScriptableFunction("SCR_set_002")]
	public static void SCR_set_002(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "DEF_BM", 6);
		else if (CrossedDown(o, n, 2)) Modify(c, "DEF_BM", -6);
	}

	// =========================================================================
	// SET_003 - Magi Set
	// =========================================================================
	[ScriptableFunction("SCR_set_003")]
	public static void SCR_set_003(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "Dark_Atk_BM", 30);
		else if (CrossedDown(o, n, 2)) Modify(c, "Dark_Atk_BM", -30);
	}

	// =========================================================================
	// SET_004 - Jude Set
	// =========================================================================
	[ScriptableFunction("SCR_set_004")]
	public static void SCR_set_004(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "MSPD_BM", 5);
		else if (CrossedDown(o, n, 2)) Modify(c, "MSPD_BM", -5);
	}

	// =========================================================================
	// SET_005 - Ismin Set
	// =========================================================================
	[ScriptableFunction("SCR_set_005")]
	public static void SCR_set_005(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "ResFire_BM", 15);
		else if (CrossedDown(o, n, 2)) Modify(c, "ResFire_BM", -15);
	}

	// =========================================================================
	// SET_007 - Cafrisun Set
	// =========================================================================
	[ScriptableFunction("SCR_set_007")]
	public static void SCR_set_007(Character c, ItemSetData s, int o, int n)
	{
		// 2-piece: RHP +16
		if (CrossedUp(o, n, 2)) Modify(c, "RHP_BM", 16);
		else if (CrossedDown(o, n, 2)) Modify(c, "RHP_BM", -16);

		// 3-piece: All item stats +1
		if (CrossedUp(o, n, 3)) AddAllItemStats(c, 1);
		else if (CrossedDown(o, n, 3)) AddAllItemStats(c, -1);

		// 4-piece: Buff
		if (CrossedUp(o, n, 4)) AddBuff(c, "item_set_007_buff");
		else if (CrossedDown(o, n, 4)) RemoveBuff(c, "item_set_007_buff");
	}

	// =========================================================================
	// SET_008/009/010 - Riena Sets
	// =========================================================================
	[ScriptableFunction("SCR_set_008")]
	public static void SCR_set_008(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "ResFire_BM", 16);
		else if (CrossedDown(o, n, 2)) Modify(c, "ResFire_BM", -16);
	}

	[ScriptableFunction("SCR_set_009")]
	public static void SCR_set_009(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "ResFire_BM", 16);
		else if (CrossedDown(o, n, 2)) Modify(c, "ResFire_BM", -16);
	}

	[ScriptableFunction("SCR_set_010")]
	public static void SCR_set_010(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "ResFire_BM", 16);
		else if (CrossedDown(o, n, 2)) Modify(c, "ResFire_BM", -16);
	}

	// =========================================================================
	// SET_011 - Watcher Set
	// =========================================================================
	[ScriptableFunction("SCR_set_011")]
	public static void SCR_set_011(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "Velnias_Atk_BM", 6);
		else if (CrossedDown(o, n, 2)) Modify(c, "Velnias_Atk_BM", -6);

		if (CrossedUp(o, n, 3)) Modify(c, "Velnias_Atk_BM", 8);
		else if (CrossedDown(o, n, 3)) Modify(c, "Velnias_Atk_BM", -8);

		if (CrossedUp(o, n, 4)) AddBuff(c, "item_set_011pre_buff");
		else if (CrossedDown(o, n, 4)) RemoveBuff(c, "item_set_011pre_buff");
	}

	// =========================================================================
	// SET_012 - Abomination Set
	// =========================================================================
	[ScriptableFunction("SCR_set_012")]
	public static void SCR_set_012(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) AddAllItemStats(c, 3);
		else if (CrossedDown(o, n, 2)) AddAllItemStats(c, -3);
	}

	// =========================================================================
	// SET_013 - Earth Armor Sets (Cloth/Leather/Plate share this)
	// =========================================================================
	[ScriptableFunction("SCR_set_013")]
	public static void SCR_set_013(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "CON_ITEM_BM", 5);
		else if (CrossedDown(o, n, 2)) Modify(c, "CON_ITEM_BM", -5);

		if (CrossedUp(o, n, 3)) Modify(c, "MaxSta_BM", 10);
		else if (CrossedDown(o, n, 3)) Modify(c, "MaxSta_BM", -10);

		if (CrossedUp(o, n, 4)) AddBuff(c, "item_set_013pre_buff");
		else if (CrossedDown(o, n, 4)) RemoveBuff(c, "item_set_013pre_buff");
	}

	// =========================================================================
	// SET_016 - Legwyn Family Sets (Cloth/Leather/Plate share this)
	// =========================================================================
	[ScriptableFunction("SCR_set_016")]
	public static void SCR_set_016(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "ResDark_BM", 10);
		else if (CrossedDown(o, n, 2)) Modify(c, "ResDark_BM", -10);

		if (CrossedUp(o, n, 3)) Modify(c, "MNA_ITEM_BM", 6);
		else if (CrossedDown(o, n, 3)) Modify(c, "MNA_ITEM_BM", -6);

		if (CrossedUp(o, n, 4)) AddBuff(c, "item_set_016pre_buff");
		else if (CrossedDown(o, n, 4)) RemoveBuff(c, "item_set_016pre_buff");
	}

	// =========================================================================
	// SET_020 - Velnia Monkey Set
	// =========================================================================
	[ScriptableFunction("SCR_set_020")]
	public static void SCR_set_020(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "DEX_ITEM_BM", 6);
		else if (CrossedDown(o, n, 2)) Modify(c, "DEX_ITEM_BM", -6);
	}

	// =========================================================================
	// SET_021 - Elgos Set
	// =========================================================================
	[ScriptableFunction("SCR_set_021")]
	public static void SCR_set_021(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "MDEF_BM", 5);
		else if (CrossedDown(o, n, 2)) Modify(c, "MDEF_BM", -5);

		if (CrossedUp(o, n, 3)) Modify(c, "MDEF_BM", 10);
		else if (CrossedDown(o, n, 3)) Modify(c, "MDEF_BM", -10);

		if (CrossedUp(o, n, 4)) Modify(c, "INT_ITEM_BM", 15);
		else if (CrossedDown(o, n, 4)) Modify(c, "INT_ITEM_BM", -15);
	}

	// =========================================================================
	// SET_022 - Ibre Set
	// =========================================================================
	[ScriptableFunction("SCR_set_022")]
	public static void SCR_set_022(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "DEF_BM", 5);
		else if (CrossedDown(o, n, 2)) Modify(c, "DEF_BM", -5);

		if (CrossedUp(o, n, 3)) Modify(c, "DEF_BM", 10);
		else if (CrossedDown(o, n, 3)) Modify(c, "DEF_BM", -10);

		if (CrossedUp(o, n, 4)) Modify(c, "DR_BM", 15);
		else if (CrossedDown(o, n, 4)) Modify(c, "DR_BM", -15);
	}

	// =========================================================================
	// SET_023 - Grynas Set
	// =========================================================================
	[ScriptableFunction("SCR_set_023")]
	public static void SCR_set_023(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "ResDark_BM", 10);
		else if (CrossedDown(o, n, 2)) Modify(c, "ResDark_BM", -10);

		if (CrossedUp(o, n, 3)) Modify(c, "ResDark_BM", 20);
		else if (CrossedDown(o, n, 3)) Modify(c, "ResDark_BM", -20);

		if (CrossedUp(o, n, 4)) Modify(c, "MHP_BM", 1020);
		else if (CrossedDown(o, n, 4)) Modify(c, "MHP_BM", -1020);
	}

	// =========================================================================
	// SET_024 - Poratore Set
	// =========================================================================
	[ScriptableFunction("SCR_set_024")]
	public static void SCR_set_024(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) AddBuff(c, "item_set_024");
		else if (CrossedDown(o, n, 2)) RemoveBuff(c, "item_set_024");
	}

	// =========================================================================
	// SET_025 - Virtov Cloth Set
	// =========================================================================
	[ScriptableFunction("SCR_set_025")]
	public static void SCR_set_025(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "Ice_Atk_BM", 12);
		else if (CrossedDown(o, n, 2)) Modify(c, "Ice_Atk_BM", -12);

		if (CrossedUp(o, n, 3)) Modify(c, "Ice_Atk_BM", 26);
		else if (CrossedDown(o, n, 3)) Modify(c, "Ice_Atk_BM", -26);

		if (CrossedUp(o, n, 4)) Modify(c, "DefAries_BM", 25);
		else if (CrossedDown(o, n, 4)) Modify(c, "DefAries_BM", -25);
	}

	// =========================================================================
	// SET_026 - Virtov Leather Set
	// =========================================================================
	[ScriptableFunction("SCR_set_026")]
	public static void SCR_set_026(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "Ice_Atk_BM", 12);
		else if (CrossedDown(o, n, 2)) Modify(c, "Ice_Atk_BM", -12);

		if (CrossedUp(o, n, 3)) Modify(c, "Ice_Atk_BM", 26);
		else if (CrossedDown(o, n, 3)) Modify(c, "Ice_Atk_BM", -26);

		if (CrossedUp(o, n, 4)) Modify(c, "DefStrike_BM", 25);
		else if (CrossedDown(o, n, 4)) Modify(c, "DefStrike_BM", -25);
	}

	// =========================================================================
	// SET_027 - Virtov Plate Set
	// =========================================================================
	[ScriptableFunction("SCR_set_027")]
	public static void SCR_set_027(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "Ice_Atk_BM", 12);
		else if (CrossedDown(o, n, 2)) Modify(c, "Ice_Atk_BM", -12);

		if (CrossedUp(o, n, 3)) Modify(c, "Ice_Atk_BM", 26);
		else if (CrossedDown(o, n, 3)) Modify(c, "Ice_Atk_BM", -26);

		if (CrossedUp(o, n, 4)) Modify(c, "DefSlash_BM", 25);
		else if (CrossedDown(o, n, 4)) Modify(c, "DefSlash_BM", -25);
	}

	// =========================================================================
	// SET_028 - Lolopanther Cloth Set
	// =========================================================================
	[ScriptableFunction("SCR_set_028")]
	public static void SCR_set_028(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "RSP_BM", 100);
		else if (CrossedDown(o, n, 2)) Modify(c, "RSP_BM", -100);

		if (CrossedUp(o, n, 3)) Modify(c, "Earth_Atk_BM", 42);
		else if (CrossedDown(o, n, 3)) Modify(c, "Earth_Atk_BM", -42);

		if (CrossedUp(o, n, 4)) Modify(c, "INT_ITEM_BM", 25);
		else if (CrossedDown(o, n, 4)) Modify(c, "INT_ITEM_BM", -25);
	}

	// =========================================================================
	// SET_029 - Lolopanther Leather Set
	// =========================================================================
	[ScriptableFunction("SCR_set_029")]
	public static void SCR_set_029(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "CRTHR_BM", 46);
		else if (CrossedDown(o, n, 2)) Modify(c, "CRTHR_BM", -46);

		if (CrossedUp(o, n, 3)) Modify(c, "Earth_Atk_BM", 42);
		else if (CrossedDown(o, n, 3)) Modify(c, "Earth_Atk_BM", -42);

		if (CrossedUp(o, n, 4)) Modify(c, "DEX_ITEM_BM", 25);
		else if (CrossedDown(o, n, 4)) Modify(c, "DEX_ITEM_BM", -25);
	}

	// =========================================================================
	// SET_030 - Lolopanther Plate Set
	// =========================================================================
	[ScriptableFunction("SCR_set_030")]
	public static void SCR_set_030(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "RHP_BM", 190);
		else if (CrossedDown(o, n, 2)) Modify(c, "RHP_BM", -190);

		if (CrossedUp(o, n, 3)) Modify(c, "Earth_Atk_BM", 42);
		else if (CrossedDown(o, n, 3)) Modify(c, "Earth_Atk_BM", -42);

		if (CrossedUp(o, n, 4)) Modify(c, "STR_ITEM_BM", 25);
		else if (CrossedDown(o, n, 4)) Modify(c, "STR_ITEM_BM", -25);
	}

	// =========================================================================
	// SET_031-034 - Metal Accessory Sets
	// =========================================================================
	[ScriptableFunction("SCR_set_031")]
	public static void SCR_set_031(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 3)) Modify(c, "MDEF_BM", 10);
		else if (CrossedDown(o, n, 3)) Modify(c, "MDEF_BM", -10);
	}

	[ScriptableFunction("SCR_set_032")]
	public static void SCR_set_032(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 3)) Modify(c, "MDEF_BM", 14);
		else if (CrossedDown(o, n, 3)) Modify(c, "MDEF_BM", -14);
	}

	[ScriptableFunction("SCR_set_033")]
	public static void SCR_set_033(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 3)) Modify(c, "MDEF_BM", 18);
		else if (CrossedDown(o, n, 3)) Modify(c, "MDEF_BM", -18);
	}

	[ScriptableFunction("SCR_set_034")]
	public static void SCR_set_034(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 3)) Modify(c, "MDEF_BM", 22);
		else if (CrossedDown(o, n, 3)) Modify(c, "MDEF_BM", -22);
	}

	// =========================================================================
	// SET_035 - Manahas Set
	// =========================================================================
	[ScriptableFunction("SCR_set_035")]
	public static void SCR_set_035(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "RHP_BM", 240);
		else if (CrossedDown(o, n, 2)) Modify(c, "RHP_BM", -240);

		if (CrossedUp(o, n, 3)) AddAllItemStats(c, 6);
		else if (CrossedDown(o, n, 3)) AddAllItemStats(c, -6);

		if (CrossedUp(o, n, 4))
		{
			AddBuff(c, "item_set_035_buff");
			Modify(c, "STR_ITEM_BM", 12);
			Modify(c, "MNA_ITEM_BM", 12);
		}
		else if (CrossedDown(o, n, 4))
		{
			RemoveBuff(c, "item_set_035_buff");
			Modify(c, "STR_ITEM_BM", -12);
			Modify(c, "MNA_ITEM_BM", -12);
		}
	}

	// =========================================================================
	// SET_036 - Solmiki Cloth Set (shared by 036/037/038)
	// =========================================================================
	[ScriptableFunction("SCR_set_036")]
	public static void SCR_set_036(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 3)) AddAllItemStats(c, 25);
		else if (CrossedDown(o, n, 3)) AddAllItemStats(c, -25);

		if (CrossedUp(o, n, 4)) Modify(c, "MSP_BM", 1437);
		else if (CrossedDown(o, n, 4)) Modify(c, "MSP_BM", -1437);

		if (CrossedUp(o, n, 6)) Modify(c, "Earth_Atk_BM", 162);
		else if (CrossedDown(o, n, 6)) Modify(c, "Earth_Atk_BM", -162);

		if (CrossedUp(o, n, 7))
		{
			Modify(c, "MATK_BM", 571);
			Modify(c, "MHR_BM", 69);
		}
		else if (CrossedDown(o, n, 7))
		{
			Modify(c, "MATK_BM", -571);
			Modify(c, "MHR_BM", -69);
		}
	}

	// =========================================================================
	// SET_039 - Varpas Set
	// =========================================================================
	[ScriptableFunction("SCR_set_039")]
	public static void SCR_set_039(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "CRTHR_BM", 30);
		else if (CrossedDown(o, n, 2)) Modify(c, "CRTHR_BM", -30);

		if (CrossedUp(o, n, 3))
		{
			Modify(c, "PATK_BM", 120);
			Modify(c, "SR_BM", 1);
			AddBuff(c, "item_set_039pre_buff");
		}
		else if (CrossedDown(o, n, 3))
		{
			Modify(c, "PATK_BM", -120);
			Modify(c, "SR_BM", -1);
			RemoveBuff(c, "item_set_039pre_buff");
		}
	}

	// =========================================================================
	// SET_040 - Verijo Set
	// =========================================================================
	[ScriptableFunction("SCR_set_040")]
	public static void SCR_set_040(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "MHR_BM", 60);
		else if (CrossedDown(o, n, 2)) Modify(c, "MHR_BM", -60);

		if (CrossedUp(o, n, 3))
		{
			Modify(c, "MATK_BM", 120);
			Modify(c, "SR_BM", 1);
			AddBuff(c, "item_set_039pre_buff");
		}
		else if (CrossedDown(o, n, 3))
		{
			Modify(c, "MATK_BM", -120);
			Modify(c, "SR_BM", -1);
			RemoveBuff(c, "item_set_039pre_buff");
		}
	}

	// =========================================================================
	// SET_041-043 - Guild Battle Sets
	// =========================================================================
	[ScriptableFunction("SCR_set_041")]
	public static void SCR_set_041(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 1)) AddBuff(c, "item_set_041_buff");
		else if (CrossedDown(o, n, 1)) RemoveBuff(c, "item_set_041_buff");
	}

	[ScriptableFunction("SCR_set_042")]
	public static void SCR_set_042(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 1)) AddBuff(c, "item_set_042_buff");
		else if (CrossedDown(o, n, 1)) RemoveBuff(c, "item_set_042_buff");
	}

	[ScriptableFunction("SCR_set_043")]
	public static void SCR_set_043(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 1)) AddBuff(c, "item_set_043_buff");
		else if (CrossedDown(o, n, 1)) RemoveBuff(c, "item_set_043_buff");
	}

	// =========================================================================
	// SET_044/052 - Sugar/White Day Sets (placeholder)
	// =========================================================================
	[ScriptableFunction("SCR_set_044_WHITE2")]
	public static void SCR_set_044_WHITE2(Character c, ItemSetData s, int o, int n) { }

	[ScriptableFunction("SCR_set_052_WHITE2")]
	public static void SCR_set_052_WHITE2(Character c, ItemSetData s, int o, int n) { }

	// =========================================================================
	// SET_045 - Kite Moor Set
	// =========================================================================
	[ScriptableFunction("SCR_set_045")]
	public static void SCR_set_045(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "CON_ITEM_BM", 25);
		else if (CrossedDown(o, n, 2)) Modify(c, "CON_ITEM_BM", -25);

		if (CrossedUp(o, n, 3)) AddBuff(c, "Res_TS_ATK");
		else if (CrossedDown(o, n, 3)) RemoveBuff(c, "Res_TS_ATK");
	}

	// =========================================================================
	// SET_046 - Frieno Set
	// =========================================================================
	[ScriptableFunction("SCR_set_046")]
	public static void SCR_set_046(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2))
		{
			Modify(c, "SR_BM", 1);
			Modify(c, "CRTATK_BM", 231);
		}
		else if (CrossedDown(o, n, 2))
		{
			Modify(c, "SR_BM", -1);
			Modify(c, "CRTATK_BM", -231);
		}

		if (CrossedUp(o, n, 3)) AddBuff(c, "SHOCK_BOOM");
		else if (CrossedDown(o, n, 3)) RemoveBuff(c, "SHOCK_BOOM");
	}

	// =========================================================================
	// SET_047 - Pasiutes Set
	// =========================================================================
	[ScriptableFunction("SCR_set_047")]
	public static void SCR_set_047(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2))
		{
			Modify(c, "MHP_BM", 1023);
			Modify(c, "DEX_ITEM_BM", 10);
		}
		else if (CrossedDown(o, n, 2))
		{
			Modify(c, "MHP_BM", -1023);
			Modify(c, "DEX_ITEM_BM", -10);
		}

		if (CrossedUp(o, n, 3)) AddBuff(c, "DFFENCE_SHIELD_PRE");
		else if (CrossedDown(o, n, 3)) RemoveBuff(c, "DFFENCE_SHIELD_PRE");
	}

	// =========================================================================
	// SET_048 - Lynnki Sit Set
	// =========================================================================
	[ScriptableFunction("SCR_set_048")]
	public static void SCR_set_048(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) Modify(c, "MATK_BM", 120);
		else if (CrossedDown(o, n, 2)) Modify(c, "MATK_BM", -120);

		if (CrossedUp(o, n, 3)) AddBuff(c, "STACK_MATK_PRE");
		else if (CrossedDown(o, n, 3)) RemoveBuff(c, "STACK_MATK_PRE");
	}

	// =========================================================================
	// SET_053 - Velcoffer Costume Set
	// =========================================================================
	[ScriptableFunction("SCR_VELCOFFER_COSTUME_SET01")]
	public static void SCR_VELCOFFER_COSTUME_SET01(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 2)) AddBuff(c, "COSTUME_VELCOFFER_SET");
		else if (CrossedDown(o, n, 2)) RemoveBuff(c, "COSTUME_VELCOFFER_SET");
	}

	// =========================================================================
	// SET_054/055 - Territory Wars Beta Sets
	// =========================================================================
	[ScriptableFunction("SCR_set_054_COLONY")]
	public static void SCR_set_054_COLONY(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 1)) AddBuff(c, "item_set_041_buff");
		else if (CrossedDown(o, n, 1)) RemoveBuff(c, "item_set_041_buff");
	}

	[ScriptableFunction("SCR_set_055_COLONY")]
	public static void SCR_set_055_COLONY(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 1)) AddBuff(c, "item_set_041_buff");
		else if (CrossedDown(o, n, 1)) RemoveBuff(c, "item_set_041_buff");
	}

	// =========================================================================
	// SET_056-058 - Nepagristas/Nematomas/Rangovas Sets
	// =========================================================================
	[ScriptableFunction("SCR_set_056_NEPAGRITAS")]
	public static void SCR_set_056_NEPAGRITAS(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 3)) AddBuff(c, "ACC_REIN_TRANSCEND");
		else if (CrossedDown(o, n, 3)) RemoveBuff(c, "ACC_REIN_TRANSCEND");
	}

	[ScriptableFunction("SCR_set_057_NEMATOMAS")]
	public static void SCR_set_057_NEMATOMAS(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 3)) AddBuff(c, "ACC_BACKATTACK_NOBLOCK");
		else if (CrossedDown(o, n, 3)) RemoveBuff(c, "ACC_BACKATTACK_NOBLOCK");
	}

	[ScriptableFunction("SCR_set_058_RANGOVAS")]
	public static void SCR_set_058_RANGOVAS(Character c, ItemSetData s, int o, int n)
	{
		if (CrossedUp(o, n, 3)) AddBuff(c, "SUMMON_TRANSE_ADD_STATUS");
		else if (CrossedDown(o, n, 3)) RemoveBuff(c, "SUMMON_TRANSE_ADD_STATUS");
	}

	// =========================================================================
	// SET_059-062 - Drakonas Sets (placeholders)
	// =========================================================================
	[ScriptableFunction("SCR_set_059_piece2")]
	public static void SCR_set_059_piece2(Character c, ItemSetData s, int o, int n) { }

	[ScriptableFunction("SCR_set_060_piece2")]
	public static void SCR_set_060_piece2(Character c, ItemSetData s, int o, int n) { }

	[ScriptableFunction("SCR_set_061_piece2")]
	public static void SCR_set_061_piece2(Character c, ItemSetData s, int o, int n) { }

	[ScriptableFunction("SCR_set_062_piece2")]
	public static void SCR_set_062_piece2(Character c, ItemSetData s, int o, int n) { }

	// =========================================================================
	// SET_063/064 - Irredian Sets (placeholders)
	// =========================================================================
	[ScriptableFunction("SCR_set_063_piece3")]
	public static void SCR_set_063_piece3(Character c, ItemSetData s, int o, int n) { }

	[ScriptableFunction("SCR_set_064_piece3")]
	public static void SCR_set_064_piece3(Character c, ItemSetData s, int o, int n) { }

	// =========================================================================
	// SET_067 - Skiaclips Costume Set
	// =========================================================================
	[ScriptableFunction("SCR_SKIACLIPS_COSTUME_SET01")]
	public static void SCR_SKIACLIPS_COSTUME_SET01(Character c, ItemSetData s, int o, int n) { }

	// =========================================================================
	// SET_068-073 - Carnas/Pyktis/Kantribe/Juoda Sets (placeholders)
	// =========================================================================
	[ScriptableFunction("SCR_set_068_piece3")]
	public static void SCR_set_068_piece3(Character c, ItemSetData s, int o, int n) { }

	[ScriptableFunction("SCR_set_069_piece3")]
	public static void SCR_set_069_piece3(Character c, ItemSetData s, int o, int n) { }

	[ScriptableFunction("SCR_set_070_piece3")]
	public static void SCR_set_070_piece3(Character c, ItemSetData s, int o, int n) { }

	[ScriptableFunction("SCR_set_071_piece3")]
	public static void SCR_set_071_piece3(Character c, ItemSetData s, int o, int n) { }

	[ScriptableFunction("SCR_set_072_piece3")]
	public static void SCR_set_072_piece3(Character c, ItemSetData s, int o, int n) { }

	[ScriptableFunction("SCR_set_073_piece3")]
	public static void SCR_set_073_piece3(Character c, ItemSetData s, int o, int n) { }

	// =========================================================================
	// SET_074-077 - Tantalizer/Moringponia Costume Sets (placeholders)
	// =========================================================================
	[ScriptableFunction("SCR_set_074_piece2")]
	public static void SCR_set_074_piece2(Character c, ItemSetData s, int o, int n) { }

	[ScriptableFunction("SCR_set_075_piece2")]
	public static void SCR_set_075_piece2(Character c, ItemSetData s, int o, int n) { }

	[ScriptableFunction("SCR_set_076_piece2")]
	public static void SCR_set_076_piece2(Character c, ItemSetData s, int o, int n) { }

	[ScriptableFunction("SCR_set_077_piece2")]
	public static void SCR_set_077_piece2(Character c, ItemSetData s, int o, int n) { }

	// =========================================================================
	// SET_078-080 - Isbikimas/Triukas/Isgarinti Sets (placeholders)
	// =========================================================================
	[ScriptableFunction("SCR_set_078_piece3")]
	public static void SCR_set_078_piece3(Character c, ItemSetData s, int o, int n) { }

	[ScriptableFunction("SCR_set_079_piece3")]
	public static void SCR_set_079_piece3(Character c, ItemSetData s, int o, int n) { }

	[ScriptableFunction("SCR_set_080_piece3")]
	public static void SCR_set_080_piece3(Character c, ItemSetData s, int o, int n) { }

	// =========================================================================
	// SET_090/092 - Botanic Sets (placeholders)
	// =========================================================================
	[ScriptableFunction("SCR_set_090_piece2")]
	public static void SCR_set_090_piece2(Character c, ItemSetData s, int o, int n) { }

	[ScriptableFunction("SCR_set_092_piece2")]
	public static void SCR_set_092_piece2(Character c, ItemSetData s, int o, int n) { }
}
