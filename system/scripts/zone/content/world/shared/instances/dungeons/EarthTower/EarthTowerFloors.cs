//--- Melia Script ----------------------------------------------------------
// Earth Tower Floors
//--- Description -----------------------------------------------------------
// Generated from the client mgame data of the Earth Tower floors.
// Do not edit by hand.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using Melia.Shared.World;

public static partial class EarthTowerFloors
{
	public static readonly Dictionary<int, Position> Arrivals = new()
	{
		{ 1, new Position(2756.82f, 267.83f, -4102.92f) },
		{ 2, new Position(2959.50f, 267.83f, -1799.01f) },
		{ 3, new Position(2853.93f, 269.10f, 446.96f) },
		{ 4, new Position(2869.98f, 268.00f, 2481.83f) },
		{ 5, new Position(2786.19f, 268.00f, 4357.52f) },
		{ 6, new Position(140.25f, 271.55f, -4165.26f) },
		{ 7, new Position(107.01f, 268.00f, -1877.96f) },
		{ 8, new Position(54.60f, 268.00f, 371.87f) },
		{ 9, new Position(171.17f, 268.00f, 2397.61f) },
		{ 10, new Position(171.99f, 268.00f, 4374.35f) },
		{ 11, new Position(-2644.18f, 268.91f, -4330.42f) },
		{ 12, new Position(-2643.15f, 268.91f, -1840.84f) },
		{ 13, new Position(-2765.16f, 268.91f, 395.24f) },
		{ 14, new Position(-2539.37f, 268.91f, 2384.62f) },
		{ 15, new Position(-2445.35f, 268.91f, 4305.00f) },
		{ 16, new Position(-5233.89f, 268.91f, -4147.83f) },
		{ 17, new Position(-5254.39f, 268.91f, -1806.24f) },
		{ 18, new Position(-4843.60f, 268.91f, 383.79f) },
		{ 19, new Position(-4953.96f, 268.91f, 2389.53f) },
		{ 20, new Position(-4871.21f, 268.91f, 4325.05f) },
		{ 21, new Position(5320.45f, 268.16f, -3759.12f) },
		{ 22, new Position(5181.98f, 267.36f, -1692.24f) },
		{ 23, new Position(5194.23f, 267.32f, 466.36f) },
		{ 24, new Position(5106.69f, 267.42f, 2746.00f) },
		{ 25, new Position(4831.74f, 267.12f, 5011.93f) },
		{ 26, new Position(2706.68f, 266.93f, -3972.39f) },
		{ 27, new Position(2464.81f, 267.13f, -1700.32f) },
		{ 28, new Position(2451.24f, 266.86f, 381.78f) },
		{ 29, new Position(2345.59f, 266.95f, 2542.80f) },
		{ 30, new Position(2223.15f, 266.75f, 4889.25f) },
		{ 31, new Position(29.35f, 267.52f, -4261.76f) },
		{ 32, new Position(-39.10f, 267.24f, -1997.60f) },
		{ 33, new Position(-164.88f, 266.85f, 330.76f) },
		{ 34, new Position(-179.43f, 267.02f, 2664.82f) },
		{ 35, new Position(-167.26f, 267.25f, 4828.30f) },
		{ 36, new Position(-2772.27f, 267.37f, -4279.30f) },
		{ 37, new Position(-2858.71f, 267.23f, -2002.95f) },
		{ 38, new Position(-2959.53f, 266.82f, 44.26f) },
		{ 39, new Position(-3008.71f, 267.38f, 2373.31f) },
		{ 40, new Position(-3033.74f, 266.69f, 4667.39f) },
	};

	public static void Load(Dictionary<string, MGameData> games)
	{
		games["M_GTOWER_STAGE_1"] = Floor1();
		games["M_GTOWER_STAGE_2"] = Floor2();
		games["M_GTOWER_STAGE_3"] = Floor3();
		games["M_GTOWER_STAGE_4"] = Floor4();
		games["M_GTOWER_STAGE_5"] = Floor5();
		games["M_GTOWER_STAGE_6"] = Floor6();
		games["M_GTOWER_STAGE_7"] = Floor7();
		games["M_GTOWER_STAGE_8"] = Floor8();
		games["M_GTOWER_STAGE_9"] = Floor9();
		games["M_GTOWER_STAGE_10"] = Floor10();
		games["M_GTOWER_STAGE_11"] = Floor11();
		games["M_GTOWER_STAGE_12"] = Floor12();
		games["M_GTOWER_STAGE_13"] = Floor13();
		games["M_GTOWER_STAGE_14"] = Floor14();
		games["M_GTOWER_STAGE_15"] = Floor15();
		games["M_GTOWER_STAGE_16"] = Floor16();
		games["M_GTOWER_STAGE_17"] = Floor17();
		games["M_GTOWER_STAGE_18"] = Floor18();
		games["M_GTOWER_STAGE_19"] = Floor19();
		games["M_GTOWER_STAGE_20"] = Floor20();
		games["M_GTOWER2_STAGE_21"] = Floor21();
		games["M_GTOWER2_STAGE_22"] = Floor22();
		games["M_GTOWER2_STAGE_23"] = Floor23();
		games["M_GTOWER2_STAGE_24"] = Floor24();
		games["M_GTOWER2_STAGE_25"] = Floor25();
		games["M_GTOWER2_STAGE_26"] = Floor26();
		games["M_GTOWER2_STAGE_27"] = Floor27();
		games["M_GTOWER2_STAGE_28"] = Floor28();
		games["M_GTOWER2_STAGE_29"] = Floor29();
		games["M_GTOWER2_STAGE_30"] = Floor30();
		games["M_GTOWER2_STAGE_31"] = Floor31();
		games["M_GTOWER2_STAGE_32"] = Floor32();
		games["M_GTOWER2_STAGE_33"] = Floor33();
		games["M_GTOWER2_STAGE_34"] = Floor34();
		games["M_GTOWER2_STAGE_35"] = Floor35();
		games["M_GTOWER2_STAGE_36"] = Floor36();
		games["M_GTOWER2_STAGE_37"] = Floor37();
		games["M_GTOWER2_STAGE_38"] = Floor38();
		games["M_GTOWER2_STAGE_39"] = Floor39();
		games["M_GTOWER2_STAGE_40"] = Floor40();
	}

	private static MGameData Floor1()
	{
		var g = new MGameData("M_GTOWER_STAGE_1", 1);
		var d0 = new[] { new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER_STAGE_1", "MGTSTAGE1", "1"), new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER_STAGE_1", "MGTSTAGE1", "150", "@dicID_^*$ETC_20160224_020777$*^", "scroll", "3") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020772$*^", "stage_ready", "10");
		s0.Obj(1, 40001, 2708.62f, 436.31f, -6130.79f, 102).Named("Earth Tower 1F").Enter("G_TOWER_WARP_TO_1", 50).Neutral();
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		s0.Event("Setting", 1, 0)
			.Do("GAME_ST_EVT_EXEC_VALUE", "MGT_STAGE_1", "0");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020773$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, 2762.59f, 267.83f, -4136.97f, 91).Named("Earth Tower 2F").Enter("G_TOWER_WARP_TO_2", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_2");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020776$*^", "stage_start", "20");
		s3.Obj(0, 100050, 2502.55f, 233.98f, -4558.30f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(1, 100050, 2553.38f, 233.98f, -4546.85f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(2, 100050, 2571.35f, 233.98f, -4670.34f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(3, 100050, 2511.27f, 233.98f, -4601.68f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(4, 100050, 2580.54f, 233.98f, -4567.05f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(5, 100050, 2586.22f, 233.98f, -4505.44f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(6, 100050, 2578.15f, 233.98f, -4618.35f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(7, 100050, 2528.28f, 233.98f, -4636.74f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(8, 100050, 2615.46f, 233.98f, -4650.41f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(9, 100050, 2635.52f, 233.98f, -4592.65f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(10, 100050, 2792.59f, 233.98f, -4383.73f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(11, 100050, 2830.89f, 233.98f, -4351.30f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(12, 100050, 2852.97f, 233.98f, -4389.77f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(13, 100056, 2843.01f, 233.98f, -4425.27f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(14, 100056, 2815.93f, 233.98f, -4434.92f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(15, 100056, 2757.92f, 233.98f, -4391.68f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(16, 100056, 2732.90f, 233.98f, -4341.79f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(17, 100056, 2771.89f, 233.98f, -4296.33f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(18, 100056, 2845.24f, 233.98f, -4296.90f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(19, 100056, 2900.15f, 233.98f, -4368.18f, 0).Gen(1, 200).Dead(d0);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "MGTSTAGE1", "OVER", "150")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s3.Event("Act", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "1", "0");
		var s4 = g.Stage("CNT", false);
		s4.Start("MGAME_SET_TIMEOUT", "270");
		s4.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s4.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG");
		var s5 = g.Stage("RankResetPoint", true);
		return g;
	}

	private static MGameData Floor2()
	{
		var g = new MGameData("M_GTOWER_STAGE_2", 2);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020843$*^", "stage_ready", "10");
		s0.Obj(0, 151043, 2724.67f, 239.54f, -2225.33f, -60);
		s0.Obj(1, 151043, 3234.53f, 239.54f, -2222.10f, -122);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020844$*^", "move_to_point", "20");
		s1.Obj(0, 40001, 2971.16f, 267.83f, -1834.35f, 91).Named("Earth Tower 3F").Enter("G_TOWER_WARP_TO_3", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_3");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20200710_049101$*^", "stage_start", "20");
		s3.Obj(0, 100065, 2995.93f, 239.54f, -2219.50f, 0).Gen(1, 400);
		s3.Obj(1, 100065, 2979.69f, 239.54f, -2243.65f, 0).Gen(1, 400);
		s3.Obj(2, 100065, 2941.04f, 239.54f, -2183.92f, 0).Gen(1, 400);
		s3.Obj(3, 100065, 2968.26f, 239.54f, -2154.99f, 0).Gen(1, 400);
		s3.Obj(4, 100065, 3013.78f, 239.54f, -2174.38f, 0).Gen(1, 400);
		s3.Obj(5, 100065, 3054.55f, 239.54f, -2236.19f, 0).Gen(1, 400);
		s3.Obj(6, 100065, 3031.67f, 239.54f, -2267.97f, 0).Gen(1, 400);
		s3.Obj(7, 100065, 2991.69f, 239.54f, -2278.02f, 0).Gen(1, 400);
		s3.Obj(8, 100065, 2954.59f, 239.54f, -2264.01f, 0).Gen(1, 400);
		s3.Obj(9, 100065, 2911.84f, 239.54f, -2209.70f, 0).Gen(1, 400);
		s3.Obj(10, 100065, 2963.74f, 239.54f, -2199.85f, 0).Gen(1, 400);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s3.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10", "3")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10", "1", "0");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020847$*^", "stage_start", "20");
		s4.Obj(0, 100067, 2973.09f, 239.54f, -2208.88f, 0).Gen(1, 400);
		s4.Obj(1, 100067, 2972.83f, 239.54f, -2210.61f, 0).Gen(1, 400);
		s4.Obj(2, 100067, 2972.83f, 239.54f, -2210.61f, 0).Gen(1, 400);
		s4.Obj(3, 100067, 2972.83f, 239.54f, -2210.61f, 0).Gen(1, 400);
		s4.Obj(4, 100067, 2972.83f, 239.54f, -2210.61f, 0).Gen(1, 400);
		s4.Obj(5, 100067, 2972.83f, 239.54f, -2210.61f, 0).Gen(1, 400);
		s4.Obj(6, 100067, 2972.83f, 239.54f, -2210.61f, 0).Gen(1, 400);
		s4.Obj(7, 100067, 2972.83f, 239.54f, -2210.61f, 0).Gen(1, 400);
		s4.Obj(8, 100067, 2972.83f, 239.54f, -2210.61f, 0).Gen(1, 400);
		s4.Obj(9, 100067, 2972.83f, 239.54f, -2210.61f, 0).Gen(1, 400);
		s4.Obj(10, 100067, 2972.83f, 239.54f, -2210.61f, 0).Gen(1, 400);
		s4.Event("ACT1", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10", "5")
			.If("GAME_ST_EVT_COND_TIMECHECK", "15")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10", "1", "0");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "30");
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s5.Event("Succl_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s5.Event("Fail_npcCNT", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/0/SETTING/1", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		return g;
	}

	private static MGameData Floor3()
	{
		var g = new MGameData("M_GTOWER_STAGE_3", 3);
		var d0 = new[] { new MCall("S_AI_DEAD_ADD_BUFF", "GT_STAGE_3_ATK_BUFF", "1", "0", "30000", "1", "100") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020854$*^", "stage_ready", "10");
		s0.Obj(0, 100076, 2863.57f, 240.80f, 38.63f, -78);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020855$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, 2862.44f, 268.23f, 408.35f, 91).Named("Earth Tower 4F").Enter("G_TOWER_WARP_TO_4", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_4");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020857$*^", "stage_start", "20");
		s3.Obj(1, 100068, 2815.00f, 239.70f, 44.00f, 0).Gen(1, 400);
		s3.Obj(2, 100068, 2815.00f, 239.70f, 50.00f, 0).Gen(1, 400);
		s3.Obj(4, 100068, 2815.00f, 239.70f, 49.00f, 0).Gen(1, 400);
		s3.Obj(5, 100068, 2815.00f, 239.70f, 49.00f, 0).Gen(1, 400);
		s3.Obj(6, 100068, 2815.00f, 239.70f, 49.00f, 0).Gen(1, 400);
		s3.Obj(7, 100068, 2815.00f, 239.70f, 49.00f, 0).Gen(1, 400);
		s3.Obj(8, 100068, 2815.00f, 239.70f, 49.00f, 0).Gen(1, 400);
		s3.Obj(9, 100068, 2815.00f, 239.70f, 49.00f, 0).Gen(1, 400);
		s3.Obj(10, 100068, 2815.00f, 239.70f, 48.00f, 0).Gen(1, 400);
		s3.Obj(11, 100068, 2815.00f, 239.70f, 48.00f, 0).Gen(1, 400);
		s3.Obj(0, 100016, 2720.73f, 240.80f, -70.23f, 0).Gen(1, 300).Dead(d0);
		s3.Event("END", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/0", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s3.Event("KEY", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "15")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0", "0")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0", "1", "0");
		s3.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11", "3")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11", "1", "0");
		s3.Event("cnt", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020860$*^", "stage_start", "20");
		s4.Obj(0, 100053, 2815.00f, 239.70f, 40.00f, 0).Gen(1, 400);
		s4.Obj(1, 100053, 2815.00f, 239.70f, 40.00f, 0).Gen(1, 400);
		s4.Obj(2, 100053, 2815.00f, 239.70f, 40.00f, 0).Gen(1, 400);
		s4.Obj(3, 100053, 2815.00f, 239.70f, 40.00f, 0).Gen(1, 400);
		s4.Obj(4, 100053, 2815.00f, 239.70f, 40.00f, 0).Gen(1, 400);
		s4.Obj(5, 100053, 2815.00f, 239.70f, 40.00f, 0).Gen(1, 400);
		s4.Obj(6, 100053, 2815.00f, 239.70f, 40.00f, 0).Gen(1, 400);
		s4.Obj(7, 100053, 2815.00f, 239.70f, 40.00f, 0).Gen(1, 400);
		s4.Obj(8, 100053, 2815.00f, 239.70f, 40.00f, 0).Gen(1, 400);
		s4.Obj(9, 100053, 2815.00f, 239.70f, 40.00f, 0).Gen(1, 400);
		s4.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9", "3")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9", "1", "0");
		s4.Event("cnt", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_3_PROG");
		var s5 = g.Stage("STAGE_3_PROG", false);
		s5.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020861$*^", "stage_start", "20");
		s5.Obj(0, 100053, 2807.82f, 240.80f, 0.55f, 0).Gen(1, 400);
		s5.Obj(1, 100053, 2807.82f, 240.80f, 0.55f, 0).Gen(1, 400);
		s5.Obj(2, 100053, 2807.82f, 240.80f, 0.55f, 0).Gen(1, 400);
		s5.Obj(3, 100053, 2807.82f, 240.80f, 0.55f, 0).Gen(1, 400);
		s5.Obj(4, 100053, 2807.82f, 240.80f, 0.55f, 0).Gen(1, 400);
		s5.Obj(5, 100053, 2807.82f, 240.80f, 0.55f, 0).Gen(1, 400);
		s5.Obj(6, 100053, 2807.47f, 240.80f, 0.19f, 0).Gen(1, 400);
		s5.Obj(7, 100068, 2889.37f, 240.80f, 82.46f, 0).Gen(1, 400);
		s5.Obj(8, 100068, 2889.37f, 240.80f, 82.46f, 0).Gen(1, 400);
		s5.Obj(9, 100068, 2889.37f, 240.80f, 82.46f, 0).Gen(1, 400);
		s5.Obj(10, 100068, 2889.03f, 240.80f, 82.10f, 0).Gen(1, 400);
		s5.Obj(11, 100068, 2889.03f, 240.80f, 82.10f, 0).Gen(1, 400);
		s5.Obj(12, 100068, 2888.11f, 240.80f, 82.35f, 0).Gen(1, 400);
		s5.Obj(13, 100068, 2888.11f, 240.80f, 82.35f, 0).Gen(1, 400);
		s5.Obj(14, 100068, 2888.11f, 240.80f, 82.35f, 0).Gen(1, 400);
		s5.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_3_PROG/0/STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8/STAGE_3_PROG/9/STAGE_3_PROG/10/STAGE_3_PROG/11/STAGE_3_PROG/12/STAGE_3_PROG/13/STAGE_3_PROG/14", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_3_PROG/0/STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8/STAGE_3_PROG/9/STAGE_3_PROG/10/STAGE_3_PROG/11/STAGE_3_PROG/12/STAGE_3_PROG/13/STAGE_3_PROG/14", "1", "0");
		var s6 = g.Stage("CNT", false);
		s6.Start("MGAME_SET_TIMEOUT", "270");
		s6.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL");
		s6.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL");
		return g;
	}

	private static MGameData Floor4()
	{
		var g = new MGameData("M_GTOWER_STAGE_4", 4);
		var d0 = new[] { new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER_STAGE_4", "MGTSTAGE4", "1"), new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER_STAGE_4", "MGTSTAGE4", "50", "@dicID_^*$ETC_20160224_020777$*^", "scroll", "3") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020863$*^", "stage_ready", "10");
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		s0.Event("Setting", 1, 0)
			.Do("GAME_ST_EVT_EXEC_VALUE", "MGTSTAGE4", "0");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020864$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, 2875.86f, 266.33f, 2442.23f, 91).Named("Earth Tower 5F").Enter("G_TOWER_WARP_TO_5", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_5");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20180320_031924$*^", "stage_start", "20");
		s3.Obj(0, 100051, 2999.71f, 239.70f, 2092.32f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(1, 100069, 2757.06f, 239.70f, 2090.83f, 0).Gen(1, 200).Dead(d0);
		s3.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "1")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/0", "8")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1", "1", "0");
		s3.Event("CNT", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "MGTSTAGE4", "OVER", "50")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020867$*^", "stage_start", "20");
		s4.Obj(0, 100006, 2870.19f, 239.70f, 2093.57f, 0).Gen(1, 300).Dead(d0);
		s4.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "1")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0/STAGE_2_PROG/1", "0")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0", "1", "0");
		s4.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "MGTSTAGE4", "OVER", "50")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "270");
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "CNT");
		s5.Event("Fail_CNT", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG");
		return g;
	}

	private static MGameData Floor5()
	{
		var g = new MGameData("M_GTOWER_STAGE_5", 5);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020869$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("FAIL", false);
		s1.Start("MGAME_SET_TIMEOUT", "30");
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s1.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s1.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s2 = g.Stage("STAGE_1_PROG", false);
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020870$*^", "stage_start", "20");
		s2.Obj(0, 100080, 2792.88f, 239.89f, 3956.60f, -93);
		s2.Obj(1, 100089, 2801.78f, 239.89f, 4091.32f, 0).Gen(3, 200);
		s2.Obj(2, 100089, 2801.78f, 239.89f, 4091.32f, 0).Gen(1, 200);
		s2.Obj(3, 100089, 2801.78f, 239.89f, 4091.32f, 0).Gen(1, 200);
		s2.Obj(4, 100089, 2801.78f, 239.89f, 4091.32f, 0).Gen(1, 200);
		s2.Obj(5, 100089, 2801.78f, 239.89f, 4091.32f, 0).Gen(1, 200);
		s2.Obj(6, 100089, 2801.41f, 239.89f, 4090.94f, 0).Gen(1, 200);
		s2.Obj(7, 100089, 2764.15f, 239.89f, 4078.53f, 0).Gen(1, 200);
		s2.Event("END", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		var s3 = g.Stage("CNT", false);
		s3.Start("MGAME_SET_TIMEOUT", "270");
		s3.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG");
		s3.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG");
		var s4 = g.Stage("CHECK", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020783$*^", "reward_box", "15");
		s4.Start("MGAME_SET_TIMEOUT", "120");
		s4.Event("KEEP", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_SELECT_STAGE_5", "==", "300")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		s4.Event("OUT", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_SELECT_STAGE_5", "==", "100")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		s4.Event("TIMEOUT", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "120")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		var s5 = g.Stage("SUCCESS", false);
		s5.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020871$*^", "move_to_point", "20");
		s5.Start("MGAME_SET_TIMEOUT", "20");
		s5.Obj(0, 40001, 2791.59f, 268.00f, 4320.96f, 91).Manual().Named("Earth Tower 6F").Enter("G_TOWER_WARP_TO_6", 50).Neutral();
		s5.Event("RunMgame", 1, 0)
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_6");
		var s6 = g.Stage("OUT", false);
		s6.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20190419_041813$*^", "raid_clear", "60");
		s6.Start("MGAME_SET_TIMEOUT", "60");
		s6.Obj(0, 156162, 2795.77f, 239.89f, 3957.99f, 0).Neutral();
		s6.Obj(1, 160065, 2716.22f, 239.89f, 3964.09f, 0);
		s6.Event("LEAVE", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("MGAME_RETURN")
			.Do("MGAME_END", "1");
		s6.Event("Reward", 1, 0)
			.Do("MGAME_EXEC_ACTORSCP_MAIN", "TX_GT_REWARD_R1_1");
		return g;
	}

	private static MGameData Floor6()
	{
		var g = new MGameData("M_GTOWER_STAGE_6", 6);
		var d0 = new[] { new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER_STAGE_6", "MGTSTAGE6", "1"), new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER_STAGE_6", "MGTSTAGE6", "165", "@dicID_^*$ETC_20160224_020777$*^", "scroll", "3") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020873$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(1, 40001, 2709.61f, 433.85f, -6136.05f, 98).Named("Earth Tower 6F").Enter("G_TOWER_WARP_TO_6", 50).Neutral();
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020874$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, 141.20f, 262.12f, -4209.10f, 91).Named("Earth Tower 7F").Enter("G_TOWER_WARP_TO_7", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_7");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020876$*^", "stage_start", "20");
		s3.Obj(0, 100064, -108.21f, 243.26f, -4597.04f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(1, 100064, -97.72f, 243.26f, -4649.19f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(2, 100064, -146.43f, 243.26f, -4592.96f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(3, 100064, -109.92f, 243.26f, -4550.37f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(4, 100064, -55.00f, 243.26f, -4593.66f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(5, 100064, 40.25f, 243.26f, -4622.82f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(6, 100064, -52.16f, 243.26f, -4666.21f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(7, 100064, -34.00f, 243.26f, -4696.05f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(8, 100064, 164.82f, 243.26f, -4403.30f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(9, 100064, -11.55f, 243.26f, -4573.36f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(10, 100064, -42.07f, 243.26f, -4536.28f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(11, 100064, 190.35f, 243.26f, -4417.54f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(12, 100064, 213.19f, 243.26f, -4372.24f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(13, 100064, 260.80f, 243.26f, -4370.37f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(14, 100066, 288.60f, 243.26f, -4419.59f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(15, 100066, 247.04f, 243.26f, -4453.55f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(16, 100066, 252.09f, 243.26f, -4504.71f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(17, 100066, 321.36f, 243.26f, -4454.18f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(18, 100066, 362.62f, 243.26f, -4437.12f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(19, 100066, 353.67f, 243.26f, -4498.52f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(20, 100066, 559.44f, 150.30f, -4834.02f, 0).Gen(1, 50).Dead(d0);
		s3.Obj(21, 100066, 521.85f, 150.30f, -4825.91f, 0).Gen(1, 50).Dead(d0);
		s3.Obj(22, 100066, 517.40f, 150.30f, -4821.53f, 0).Gen(1, 50).Dead(d0);
		s3.Obj(23, 100066, 517.40f, 150.30f, -4821.53f, 0).Gen(1, 50).Dead(d0);
		s3.Obj(24, 100066, 517.40f, 150.30f, -4821.53f, 0).Gen(1, 50).Dead(d0);
		s3.Obj(25, 100066, 517.40f, 150.30f, -4821.53f, 0).Gen(1, 50).Dead(d0);
		s3.Obj(26, 100066, 517.40f, 150.30f, -4821.53f, 0).Gen(1, 50).Dead(d0);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "MGTSTAGE6", "OVER", "165")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s3.Event("Act", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "8")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "1", "0");
		var s4 = g.Stage("CNT", false);
		s4.Start("MGAME_SET_TIMEOUT", "270");
		s4.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s4.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG");
		var s5 = g.Stage("RankResetPoint", true);
		return g;
	}

	private static MGameData Floor7()
	{
		var g = new MGameData("M_GTOWER_STAGE_7", 7);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020878$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 154071, 106.54f, 239.70f, -2292.95f, 90);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		s0.Event("SET", 1, 0)
			.Do("GAME_ST_EVT_EXEC_VALUE", "GT_STAGE_7_PAD", "1");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020880$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, 111.64f, 268.00f, -1913.82f, 91).Named("Earth Tower 8F").Enter("G_TOWER_WARP_TO_8", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_8");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020882$*^", "stage_start", "20");
		s3.Obj(0, 100018, 109.88f, 239.70f, -2096.59f, 0).Gen(1, 200);
		s3.Obj(2, 100018, 91.62f, 239.70f, -2135.16f, 0).Gen(1, 200);
		s3.Obj(3, 100018, 94.56f, 239.70f, -2136.18f, 0).Gen(1, 200);
		s3.Obj(4, 100018, 212.66f, 239.70f, -2185.94f, 0).Gen(1, 200);
		s3.Obj(5, 100018, 203.67f, 239.70f, -2178.16f, 0).Gen(1, 200);
		s3.Obj(6, 100018, 124.65f, 239.70f, -2124.50f, 0).Gen(1, 200);
		s3.Obj(7, 100018, 157.62f, 239.70f, -2115.00f, 0).Gen(1, 200);
		s3.Obj(8, 100018, 128.48f, 239.70f, -2149.81f, 0).Gen(1, 200);
		s3.Obj(9, 100018, 197.22f, 239.70f, -2204.46f, 0).Gen(1, 200);
		s3.Obj(10, 100018, 60.73f, 239.70f, -2127.23f, 0).Gen(1, 200);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s3.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "5")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10", "1", "0");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020883$*^", "stage_start", "20");
		s4.Obj(0, 100039, 66.95f, 239.70f, -2410.16f, 0).Gen(1, 200);
		s4.Obj(1, 100039, 71.57f, 239.70f, -2423.11f, 0).Gen(1, 200);
		s4.Obj(2, 100039, 57.11f, 239.70f, -2425.10f, 0).Gen(1, 200);
		s4.Obj(3, 100039, 23.60f, 239.70f, -2406.05f, 0).Gen(1, 200);
		s4.Obj(4, 100039, 49.88f, 239.70f, -2397.74f, 0).Gen(1, 200);
		s4.Obj(5, 100039, 53.12f, 239.70f, -2407.68f, 0).Gen(1, 200);
		s4.Obj(6, 100039, 50.38f, 239.70f, -2413.04f, 0).Gen(1, 200);
		s4.Obj(7, 100039, 38.77f, 239.70f, -2410.65f, 0).Gen(1, 200);
		s4.Obj(8, 100039, 51.08f, 239.70f, -2401.85f, 0).Gen(1, 200);
		s4.Obj(9, 100039, 49.66f, 239.70f, -2415.49f, 0).Gen(1, 200);
		s4.Obj(10, 100039, 49.19f, 239.70f, -2415.10f, 0).Gen(1, 200);
		s4.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_3_PROG");
		s4.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "5")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/11/STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/11/STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10", "1", "0");
		var s5 = g.Stage("STAGE_3_PROG", false);
		s5.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020884$*^", "stage_start", "20");
		s5.Obj(0, 100018, 269.15f, 239.70f, -2300.77f, 0).Gen(1, 200);
		s5.Obj(1, 100039, -87.36f, 239.70f, -2259.13f, 0).Gen(1, 200);
		s5.Obj(2, 100018, 251.34f, 239.70f, -2343.10f, 0).Gen(1, 200);
		s5.Obj(3, 100018, 234.33f, 239.70f, -2189.20f, 0).Gen(1, 200);
		s5.Obj(4, 100018, 271.08f, 239.70f, -2243.67f, 0).Gen(1, 200);
		s5.Obj(5, 100018, 313.69f, 239.70f, -2296.74f, 0).Gen(1, 200);
		s5.Obj(6, 100039, -19.12f, 239.70f, -2269.46f, 0).Gen(1, 200);
		s5.Obj(7, 100039, -19.25f, 239.70f, -2237.82f, 0).Gen(1, 200);
		s5.Obj(8, 100039, -87.44f, 239.70f, -2290.10f, 0).Gen(1, 200);
		s5.Obj(9, 100039, -38.62f, 239.70f, -2329.47f, 0).Gen(1, 200);
		s5.Obj(10, 100039, -24.56f, 239.70f, -2310.42f, 0).Gen(1, 200);
		s5.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "5")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_3_PROG/0/STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8/STAGE_3_PROG/9/STAGE_3_PROG/10", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_3_PROG/0/STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8/STAGE_3_PROG/9/STAGE_3_PROG/10", "1", "0");
		var s6 = g.Stage("CNT", false);
		s6.Start("MGAME_SET_TIMEOUT", "180");
		s6.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		s6.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "180")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s6.Event("SUCC", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_STAGE_7_PAD", "OVER", "30")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "SETTING");
		s6.Event("fail", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/0", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG");
		return g;
	}

	private static MGameData Floor8()
	{
		var g = new MGameData("M_GTOWER_STAGE_8", 8);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020886$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 100075, 51.54f, 239.70f, -34.54f, 0);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020887$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, 59.90f, 268.00f, 334.80f, 91).Named("Earth Tower 9F").Enter("G_TOWER_WARP_TO_9", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_9");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020889$*^", "stage_start", "20");
		s3.Obj(0, 100052, 236.89f, 239.70f, -4.37f, 0).Gen(1, 200);
		s3.Obj(1, 100040, -85.01f, 239.70f, -41.77f, 0).Gen(1, 200);
		s3.Obj(2, 100040, -85.34f, 239.70f, -56.61f, 0).Gen(1, 200);
		s3.Obj(3, 100040, -84.17f, 239.70f, -56.47f, 0).Gen(1, 200);
		s3.Obj(4, 100040, -73.13f, 239.70f, -49.51f, 0).Gen(1, 200);
		s3.Obj(5, 100040, -74.99f, 239.70f, -40.07f, 0).Gen(1, 200);
		s3.Obj(6, 100040, -82.30f, 239.70f, -26.40f, 0).Gen(1, 200);
		s3.Obj(7, 100040, -83.78f, 239.70f, -23.62f, 0).Gen(1, 200);
		s3.Obj(8, 100040, -91.48f, 239.70f, -17.28f, 0).Gen(1, 200);
		s3.Obj(9, 100040, -114.95f, 239.70f, -21.99f, 0).Gen(1, 200);
		s3.Obj(10, 100040, -128.02f, 239.70f, -39.52f, 0).Gen(1, 200);
		s3.Obj(11, 100040, -113.87f, 239.70f, -67.49f, 0).Gen(1, 200);
		s3.Obj(12, 100052, 226.56f, 239.70f, -18.67f, 0).Gen(1, 200);
		s3.Obj(13, 100052, 228.85f, 239.70f, -24.20f, 0).Gen(1, 200);
		s3.Obj(14, 100052, 252.42f, 239.70f, -24.41f, 0).Gen(1, 200);
		s3.Obj(15, 100052, 257.45f, 239.70f, -6.75f, 0).Gen(1, 200);
		s3.Obj(16, 100052, 250.84f, 239.70f, 2.13f, 0).Gen(1, 200);
		s3.Obj(17, 100052, 233.63f, 239.70f, 0.76f, 0).Gen(1, 200);
		s3.Obj(18, 100052, 224.17f, 239.70f, -9.60f, 0).Gen(1, 200);
		s3.Obj(19, 100052, 223.99f, 239.70f, -22.11f, 0).Gen(1, 200);
		s3.Obj(20, 100052, 226.97f, 239.70f, -27.81f, 0).Gen(1, 200);
		s3.Obj(21, 100052, 238.57f, 239.70f, -47.38f, 0).Gen(1, 200);
		s3.Obj(22, 100052, 247.57f, 239.70f, -42.48f, 0).Gen(1, 200);
		s3.Event("ACT", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s3.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11", "3")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11", "1", "0");
		s3.Event("ACT2", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19/STAGE_1_PROG/20/STAGE_1_PROG/21/STAGE_1_PROG/22", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19/STAGE_1_PROG/20/STAGE_1_PROG/21/STAGE_1_PROG/22", "1", "0");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020890$*^", "stage_start", "20");
		s4.Obj(0, 20024, 57.18f, 239.70f, -31.58f, 0).Gen(1, 300);
		s4.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "15")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0", "1", "0");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "270");
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		s5.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		s5.Event("END_CNT", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/0", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		return g;
	}

	private static MGameData Floor9()
	{
		var g = new MGameData("M_GTOWER_STAGE_9", 9);
		var d0 = new[] { new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER_STAGE_9", "GT_STAGE_POINT_9", "1"), new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER_STAGE_9", "GT_STAGE_POINT_9", "60", "@dicID_^*$ETC_20160224_020777$*^", "scroll", "3") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020893$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 20026, 167.23f, 239.70f, 2001.09f, 0).Hidden();
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020894$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, 177.19f, 268.00f, 2362.80f, 91).Named("Earth Tower 10F").Enter("G_TOWER_WARP_TO_10", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_10");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20180320_031926$*^", "stage_start", "20");
		s3.Obj(0, 100062, 83.13f, 239.70f, 1938.47f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(1, 100059, 289.79f, 239.70f, 2008.00f, 0).Gen(1, 300).Dead(d0);
		s3.Obj(3, 100011, 162.71f, 239.70f, 2100.25f, 0).Gen(1, 300).Manual().Dead(d0);
		s3.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "1")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1", "8")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1", "1", "0");
		s3.Event("ACT2", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "1")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/3", "0")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/3", "1", "0");
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_STAGE_POINT_9", "OVER", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		var s4 = g.Stage("CNT", false);
		s4.Start("MGAME_SET_TIMEOUT", "270");
		s4.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "ADD_POINT");
		s4.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG");
		return g;
	}

	private static MGameData Floor10()
	{
		var g = new MGameData("M_GTOWER_STAGE_10", 10);
		var d0 = new[] { new MCall("S_AI_DEAD_ADD_BUFF", "GT_STAGE_10_ROOT", "1", "0", "15000", "1", "100") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020779$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("FAIL", false);
		s1.Start("MGAME_SET_TIMEOUT", "30");
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s1.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s1.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s2 = g.Stage("STAGE_1_PROG", false);
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020780$*^", "stage_start", "20");
		s2.Obj(0, 100081, 177.95f, 239.70f, 3978.95f, -88);
		s2.Obj(1, 100085, 48.44f, 239.70f, 4202.85f, 0).Gen(1, 1).Passive().Dead(d0);
		s2.Obj(2, 100085, -97.05f, 239.70f, 3990.39f, 0).Gen(1, 1).Passive().Dead(d0);
		s2.Obj(3, 100085, -40.30f, 239.70f, 3945.35f, 0).Gen(1, 1).Passive().Dead(d0);
		s2.Obj(4, 100085, -109.52f, 239.70f, 3927.40f, 0).Gen(1, 1).Passive().Dead(d0);
		s2.Obj(5, 100085, 13.72f, 239.70f, 4303.42f, 0).Gen(1, 1).Passive().Dead(d0);
		s2.Obj(6, 100085, -14.15f, 239.70f, 4257.02f, 0).Gen(1, 1).Passive().Dead(d0);
		s2.Obj(7, 100085, -67.87f, 239.70f, 4049.37f, 0).Gen(1, 1).Passive().Dead(d0);
		s2.Obj(8, 100085, 4.55f, 239.70f, 4149.14f, 0).Gen(1, 1).Passive().Dead(d0);
		s2.Obj(9, 100085, 82.93f, 239.70f, 4257.34f, 0).Gen(1, 1).Passive().Dead(d0);
		s2.Obj(10, 100085, -83.15f, 239.70f, 3881.45f, 0).Gen(1, 1).Passive().Dead(d0);
		s2.Event("END", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT");
		s2.Event("PROG", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "90")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		var s3 = g.Stage("STAGE_2_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020781$*^", "stage_start", "20");
		s3.Obj(0, 100007, -33.18f, 239.70f, 4037.93f, -42);
		s3.Obj(1, 100007, 352.07f, 239.70f, 4059.53f, -119);
		var s4 = g.Stage("CNT", false);
		s4.Start("MGAME_SET_TIMEOUT", "270");
		s4.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL");
		s4.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL");
		var s5 = g.Stage("CHECK", false);
		s5.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020783$*^", "reward_box", "15");
		s5.Start("MGAME_SET_TIMEOUT", "120");
		s5.Event("KEEP", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_SELECT_STAGE_10", "==", "300")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		s5.Event("OUT", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_SELECT_STAGE_10", "==", "100")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		s5.Event("TIMEOUT", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "120")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		var s6 = g.Stage("SUCCESS", false);
		s6.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020785$*^", "move_to_point", "20");
		s6.Start("MGAME_SET_TIMEOUT", "20");
		s6.Obj(0, 40001, 179.48f, 265.07f, 4337.93f, 91).Manual().Named("Earth Tower 11F").Enter("G_TOWER_WARP_TO_11", 50).Neutral();
		s6.Event("RunMGame", 1, 0)
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_11");
		var s7 = g.Stage("OUT", false);
		s7.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20190419_041810$*^", "raid_clear", "60");
		s7.Start("MGAME_SET_TIMEOUT", "60");
		s7.Obj(0, 156162, 171.75f, 239.70f, 3989.99f, 0).Neutral();
		s7.Obj(1, 160065, 119.42f, 239.70f, 3978.66f, 0);
		s7.Event("LEAVE", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("MGAME_RETURN")
			.Do("MGAME_END", "1");
		s7.Event("Reward", 1, 0)
			.Do("MGAME_EXEC_ACTORSCP_MAIN", "TX_GT_REWARD_R1_2");
		return g;
	}

	private static MGameData Floor11()
	{
		var g = new MGameData("M_GTOWER_STAGE_11", 11);
		var d0 = new[] { new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER_STAGE_11", "MGTSTAGE11", "1"), new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER_STAGE_11", "MGTSTAGE11", "180", "@dicID_^*$ETC_20160224_020777$*^", "scroll", "3") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020789$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 40001, 2707.00f, 436.49f, -6131.00f, 99).Named("Earth Tower 11F").Enter("G_TOWER_WARP_TO_11", 50).Neutral();
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020790$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, -2639.40f, 268.91f, -4366.48f, 91).Named("Earth Tower 12F").Enter("G_TOWER_WARP_TO_12", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_12");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020792$*^", "stage_start", "20");
		s3.Obj(0, 100026, -2793.34f, 240.61f, -4730.87f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(1, 100026, -2787.95f, 240.61f, -4743.43f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(2, 100026, -2804.21f, 240.61f, -4754.14f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(3, 100026, -2830.42f, 240.61f, -4721.82f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(4, 100026, -2825.15f, 240.61f, -4710.51f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(5, 100026, -2807.54f, 240.61f, -4707.27f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(6, 100026, -2791.57f, 240.61f, -4726.70f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(7, 100026, -2785.64f, 240.61f, -4752.06f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(8, 100026, -2800.51f, 240.61f, -4768.72f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(9, 100026, -2819.48f, 240.61f, -4761.20f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(10, 100026, -2569.90f, 240.61f, -4596.16f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(11, 100026, -2561.93f, 240.61f, -4633.49f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(12, 100026, -2557.47f, 240.61f, -4655.89f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(13, 100026, -2531.65f, 240.61f, -4672.08f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(14, 100088, -2532.18f, 240.61f, -4648.06f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(15, 100088, -2544.15f, 240.61f, -4621.97f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(16, 100088, -2547.13f, 240.61f, -4594.45f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(17, 100088, -2515.15f, 240.61f, -4632.03f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(18, 100088, -2512.49f, 240.61f, -4651.67f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(19, 100088, -2522.01f, 240.61f, -4659.80f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(20, 100088, -2271.12f, 147.66f, -4990.37f, 0).Gen(5, 10).Dead(d0);
		s3.Obj(21, 100088, -2321.13f, 147.66f, -5021.26f, 0).Gen(5, 10).Dead(d0);
		s3.Obj(22, 100088, -2283.81f, 147.66f, -5026.20f, 0).Gen(5, 10).Dead(d0);
		s3.Obj(23, 100088, -2250.47f, 147.66f, -4997.07f, 0).Gen(5, 10).Dead(d0);
		s3.Obj(24, 100088, -2250.47f, 147.66f, -4997.07f, 0).Gen(5, 10).Dead(d0);
		s3.Obj(25, 100088, -2250.47f, 147.66f, -4997.07f, 0).Gen(5, 10).Dead(d0);
		s3.Obj(26, 100088, -2333.17f, 147.66f, -5029.27f, 0).Gen(5, 10).Dead(d0);
		s3.Obj(27, 100088, -2371.42f, 147.66f, -5097.68f, 0).Gen(5, 10).Dead(d0);
		s3.Obj(28, 100088, -2363.65f, 147.66f, -5106.43f, 0).Gen(5, 10).Dead(d0);
		s3.Obj(29, 100088, -2334.36f, 147.66f, -5059.14f, 0).Gen(5, 10).Dead(d0);
		s3.Obj(30, 100088, -2298.00f, 147.66f, -5043.58f, 0).Gen(5, 10).Dead(d0);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "MGTSTAGE11", "OVER", "180")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s3.Event("Act", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "6")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "1", "0");
		var s4 = g.Stage("CNT", false);
		s4.Start("MGAME_SET_TIMEOUT", "270");
		s4.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s4.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG");
		var s5 = g.Stage("RankResetPoint", true);
		return g;
	}

	private static MGameData Floor12()
	{
		var g = new MGameData("M_GTOWER_STAGE_12", 12);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020794$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 151043, -2644.57f, 241.48f, -2255.35f, 0);
		s0.Obj(1, 100075, -2838.68f, 241.48f, -2055.56f, 0);
		s0.Obj(2, 100075, -2911.25f, 241.48f, -2247.12f, 0);
		s0.Obj(3, 100075, -2824.88f, 241.48f, -2436.14f, 0);
		s0.Obj(4, 100075, -2455.46f, 241.48f, -2047.16f, 0);
		s0.Obj(5, 100075, -2398.62f, 241.48f, -2246.03f, 0);
		s0.Obj(6, 100075, -2472.97f, 241.48f, -2426.44f, 0);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		s0.Event("Set_value", 1, 0)
			.Do("GAME_ST_EVT_EXEC_VALUE", "CLEAR_12", "0");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020796$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, -2638.94f, 268.91f, -1864.01f, 91).Named("Earth Tower 13F").Enter("G_TOWER_WARP_TO_13", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_13");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "SETTING")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020798$*^", "stage_start", "20");
		s3.Obj(0, 100055, -2592.68f, 241.48f, -2173.17f, 0).Gen(1, 200);
		s3.Obj(1, 100046, -2726.99f, 241.48f, -2301.45f, 0).Gen(1, 200);
		s3.Obj(2, 100046, -2830.67f, 241.48f, -2060.33f, 0).Gen(1, 10);
		s3.Obj(3, 100046, -2909.01f, 241.48f, -2246.62f, 0).Gen(1, 10);
		s3.Obj(4, 100046, -2824.67f, 241.48f, -2436.36f, 0).Gen(1, 10);
		s3.Obj(5, 100055, -2471.73f, 241.48f, -2422.32f, 0).Gen(1, 10);
		s3.Obj(6, 100055, -2395.91f, 241.48f, -2249.48f, 0).Gen(1, 10);
		s3.Obj(7, 100055, -2453.41f, 241.48f, -2045.28f, 0).Gen(1, 10);
		s3.Obj(8, 100046, -2695.82f, 241.48f, -2323.83f, 0).Gen(1, 200);
		s3.Obj(9, 100046, -2692.11f, 241.48f, -2317.94f, 0).Gen(1, 200);
		s3.Obj(10, 100093, -2691.78f, 241.48f, -2317.17f, 0).Gen(1, 200);
		s3.Obj(11, 100093, -2688.04f, 241.48f, -2311.23f, 0).Gen(1, 200);
		s3.Obj(12, 100093, -2683.37f, 241.48f, -2313.55f, 0).Gen(1, 200);
		s3.Obj(13, 100093, -2685.81f, 241.48f, -2324.95f, 0).Gen(1, 200);
		s3.Obj(14, 100093, -2581.57f, 241.48f, -2202.10f, 0).Gen(1, 200);
		s3.Obj(15, 100093, -2566.83f, 241.48f, -2161.49f, 0).Gen(1, 200);
		s3.Obj(16, 100093, -2591.09f, 241.48f, -2235.31f, 0).Gen(1, 200);
		s3.Obj(17, 100093, -2550.51f, 241.48f, -2240.98f, 0).Gen(1, 200);
		s3.Event("ACT1", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/1", "0")
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/2", "5", "0");
		s3.Event("ACT2", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/2", "0")
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/3", "5", "0");
		s3.Event("ACT3", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/3", "0")
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/4", "5", "0");
		s3.Event("ACT4", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/1/SETTING/4", "0")
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/7", "5", "0");
		s3.Event("ACT5", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/5", "0")
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/6", "5", "0");
		s3.Event("ACT6", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/6", "0")
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/5", "5", "0");
		s3.Event("ACT0", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17", "1", "0");
		s3.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17", "1", "0");
		var s4 = g.Stage("CNT", false);
		s4.Start("MGAME_SET_TIMEOUT", "30");
		s4.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		s4.Event("SUCC_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "SETTING")
			.Do("GAME_ST_EVT_EXEC_VALUE", "CLEAR_12", "1");
		s4.Event("Fail_npcCNT", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/0", "0")
			.If("GAME_ST_EVT_COND_VALUE", "CLEAR_12", "==", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		return g;
	}

	private static MGameData Floor13()
	{
		var g = new MGameData("M_GTOWER_STAGE_13", 13);
		var d0 = new[] { new MCall("S_AI_DEAD_ADD_BUFF", "GT_STAGE_13_DAMAGE_BUFF", "1", "0", "15000", "1", "100") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020800$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 100073, -2765.67f, 240.61f, -12.39f, 0);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020802$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, -2762.79f, 268.58f, 362.89f, 91).Named("Earth Tower 14F").Enter("G_TOWER_WARP_TO_14", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_14");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020804$*^", "stage_start", "20");
		s3.Obj(0, 100024, -2604.38f, 240.61f, 125.99f, 0).Gen(1, 300);
		s3.Obj(1, 100025, -2965.21f, 240.61f, 12.07f, 0).Gen(1, 300);
		s3.Obj(2, 100025, -2773.11f, 240.61f, -7.49f, 0).Gen(1, 300).Dead(d0);
		s3.Obj(3, 100025, -2964.63f, 240.61f, -11.72f, 0).Gen(1, 300);
		s3.Obj(4, 100025, -2963.47f, 240.61f, -11.90f, 0).Gen(1, 300);
		s3.Obj(5, 100025, -2959.14f, 240.61f, -27.36f, 0).Gen(1, 300);
		s3.Obj(6, 100025, -2959.75f, 240.61f, -40.95f, 0).Gen(1, 300);
		s3.Obj(7, 100025, -2950.27f, 240.61f, -48.06f, 0).Gen(1, 300);
		s3.Obj(8, 100025, -2945.47f, 240.61f, -63.28f, 0).Gen(1, 300);
		s3.Obj(9, 100025, -2984.72f, 240.61f, -50.33f, 0).Gen(1, 300);
		s3.Obj(10, 100025, -2999.88f, 240.61f, -11.56f, 0).Gen(1, 300);
		s3.Obj(11, 100024, -2588.66f, 240.61f, 100.24f, 0).Gen(1, 300);
		s3.Obj(12, 100024, -2588.59f, 240.61f, 99.33f, 0).Gen(1, 300);
		s3.Obj(13, 100024, -2567.47f, 240.61f, 50.73f, 0).Gen(1, 300);
		s3.Obj(14, 100024, -2553.49f, 240.61f, 12.48f, 0).Gen(1, 300);
		s3.Obj(15, 100024, -2542.48f, 240.61f, -7.00f, 0).Gen(1, 300);
		s3.Obj(16, 100024, -2523.64f, 240.61f, -5.69f, 0).Gen(1, 300);
		s3.Obj(17, 100024, -2529.64f, 240.61f, 35.48f, 0).Gen(1, 300);
		s3.Obj(18, 100024, -2534.98f, 240.61f, 52.31f, 0).Gen(1, 300);
		s3.Obj(19, 100024, -2543.53f, 240.61f, 83.74f, 0).Gen(1, 300);
		s3.Event("END", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/0", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s3.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/1/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/1/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10", "1", "0");
		s3.Event("ACT2", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "1", "0");
		s3.Event("KEY", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "18")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/2", "0")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/2", "1", "0");
		var s4 = g.Stage("CNT", false);
		s4.Start("MGAME_SET_TIMEOUT", "270");
		s4.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL");
		s4.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL");
		return g;
	}

	private static MGameData Floor14()
	{
		var g = new MGameData("M_GTOWER_STAGE_14", 14);
		var d0 = new[] { new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER_STAGE_14", "GT_STAGE_POINT_14", "1"), new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER_STAGE_14", "GT_STAGE_POINT_14", "70", "@dicID_^*$ETC_20160224_020777$*^", "scroll", "3") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020807$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 20026, -2544.95f, 240.61f, 1961.63f, 0);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_3_PROG");
		s0.Event("Setting", 1, 0)
			.Do("GAME_ST_EVT_EXEC_VALUE", "GT_STAGE_POINT_14", "0");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020808$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, -2540.26f, 268.91f, 2360.33f, 91).Named("Earth Tower 15F").Enter("G_TOWER_WARP_TO_15", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_15");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20180320_031928$*^", "stage_start", "20");
		s3.Obj(0, 100043, -2595.13f, 240.61f, 1979.59f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(1, 100044, -2459.15f, 240.61f, 1974.32f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(2, 20026, -2554.29f, 250.55f, 1669.36f, 90).Manual().Hidden();
		s3.Obj(3, 20026, -2308.52f, 240.61f, 1761.99f, 0).Manual().Hidden();
		s3.Obj(4, 20026, -2235.70f, 240.61f, 1979.11f, 0).Manual().Hidden();
		s3.Obj(5, 20026, -2314.23f, 240.61f, 2212.78f, 0).Manual().Hidden();
		s3.Obj(6, 20026, -2550.20f, 240.61f, 2275.07f, 0).Manual().Hidden();
		s3.Obj(7, 20026, -2751.05f, 240.61f, 2213.16f, 0).Manual().Hidden();
		s3.Obj(8, 20026, -2850.07f, 240.61f, 1975.46f, 0).Manual().Hidden();
		s3.Obj(9, 20026, -2769.44f, 240.61f, 1770.31f, 90).Manual().Hidden();
		s3.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "1")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1", "8")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1", "1", "0");
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_STAGE_POINT_14", "OVER", "70")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s3.Event("ACT2", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Obj(0, 100004, -2551.88f, 240.61f, 1987.74f, 0).Gen(1, 300).Dead(d0);
		s4.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "1")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0", "0")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0", "1", "0");
		var s5 = g.Stage("STAGE_3_PROG", false);
		s5.Obj(0, 100061, -2726.71f, 240.61f, 2265.22f, -126);
		s5.Obj(1, 100061, -2861.11f, 240.61f, 2136.17f, -66);
		s5.Obj(2, 100061, -2847.23f, 240.61f, 1977.85f, -55);
		s5.Obj(3, 100061, -2187.34f, 240.61f, 1996.86f, 34);
		s5.Obj(4, 100061, -2184.73f, 240.61f, 2124.51f, 34);
		s5.Obj(5, 100061, -2289.94f, 240.61f, 2227.27f, 34);
		s5.Obj(6, 100061, -2805.79f, 240.61f, 1750.52f, 34);
		s5.Obj(7, 100061, -2300.88f, 240.61f, 1734.76f, 34);
		s5.Obj(8, 100061, -2620.89f, 240.61f, 1737.59f, 34);
		s5.Obj(9, 100061, -2465.54f, 240.61f, 1738.47f, 34);
		s5.Obj(10, 100061, -2607.56f, 240.61f, 2264.68f, 34);
		s5.Obj(11, 100061, -2471.85f, 240.61f, 2269.65f, 34);
		s5.Obj(12, 100061, -2875.96f, 240.61f, 1818.52f, 1);
		s5.Obj(13, 100061, -2188.31f, 240.61f, 1839.00f, 160);
		s5.Obj(14, 100061, -2420.15f, 240.61f, 1620.33f, 34);
		s5.Obj(15, 100061, -2668.50f, 240.61f, 1641.91f, 3);
		var s6 = g.Stage("CNT", false);
		s6.Start("MGAME_SET_TIMEOUT", "270");
		s6.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "ADD_POINT");
		s6.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_3_PROG");
		return g;
	}

	private static MGameData Floor15()
	{
		var g = new MGameData("M_GTOWER_STAGE_15", 15);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020812$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("FAIL", false);
		s1.Start("MGAME_SET_TIMEOUT", "30");
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s1.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s1.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s2 = g.Stage("STAGE_1_PROG", false);
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020813$*^", "stage_start", "20");
		s2.Obj(0, 100082, -2441.28f, 233.79f, 3900.59f, -86);
		s2.Obj(1, 100005, -2707.66f, 233.79f, 3915.52f, -3);
		s2.Obj(2, 100091, -2607.77f, 233.79f, 3759.62f, 0).Gen(1, 200);
		s2.Obj(3, 100091, -2608.06f, 233.79f, 3759.32f, 0).Gen(1, 200);
		s2.Obj(4, 100091, -2608.06f, 233.79f, 3759.32f, 0).Gen(1, 200);
		s2.Obj(5, 100091, -2608.06f, 233.79f, 3759.32f, 0).Gen(1, 200);
		s2.Obj(6, 100091, -2608.06f, 233.79f, 3759.32f, 0).Gen(1, 200);
		s2.Obj(7, 100091, -2608.06f, 233.79f, 3759.32f, 0).Gen(1, 200);
		s2.Obj(8, 100091, -2608.06f, 233.79f, 3759.32f, 0).Gen(1, 200);
		s2.Obj(9, 100091, -2608.06f, 233.79f, 3759.32f, 0).Gen(1, 200);
		s2.Obj(10, 100091, -2608.06f, 233.79f, 3759.32f, 0).Gen(1, 200);
		s2.Obj(11, 100091, -2608.06f, 233.79f, 3759.32f, 0).Gen(1, 200);
		s2.Obj(12, 100091, -2608.06f, 233.79f, 3759.32f, 0).Gen(1, 200);
		s2.Event("END", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s2.Event("STAGE2", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "90")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s2.Event("STAGE3", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "180")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_3_PROG");
		s2.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "15")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12", "1", "0");
		var s3 = g.Stage("STAGE_2_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020814$*^", "stage_start", "10");
		s3.Obj(0, 100005, -2472.40f, 233.79f, 4118.07f, -3);
		s3.Obj(1, 100045, -2398.17f, 233.79f, 3728.69f, 0).Gen(1, 200);
		s3.Obj(2, 100045, -2398.17f, 233.79f, 3728.69f, 0).Gen(1, 200);
		s3.Obj(3, 100045, -2398.17f, 233.79f, 3728.69f, 0).Gen(1, 200);
		s3.Obj(4, 100045, -2398.17f, 233.79f, 3728.69f, 0).Gen(1, 200);
		s3.Obj(5, 100045, -2398.17f, 233.79f, 3728.69f, 0).Gen(1, 200);
		s3.Obj(6, 100045, -2394.36f, 233.79f, 3728.66f, 0).Gen(1, 200);
		s3.Obj(7, 100045, -2394.36f, 233.79f, 3728.66f, 0).Gen(1, 200);
		s3.Obj(8, 100045, -2394.36f, 233.79f, 3728.66f, 0).Gen(1, 200);
		s3.Obj(9, 100045, -2394.36f, 233.79f, 3728.66f, 0).Gen(1, 200);
		s3.Obj(10, 100045, -2394.36f, 233.79f, 3728.66f, 0).Gen(1, 200);
		s3.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "15")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10", "1", "0");
		var s4 = g.Stage("STAGE_3_PROG", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020814$*^", "stage_start", "10");
		s4.Obj(0, 100005, -2217.16f, 233.79f, 3906.42f, -3);
		s4.Obj(1, 100022, -2321.51f, 233.79f, 4072.76f, 0).Gen(1, 200);
		s4.Obj(2, 100022, -2321.51f, 233.79f, 4072.76f, 0).Gen(1, 200);
		s4.Obj(3, 100022, -2321.51f, 233.79f, 4072.76f, 0).Gen(1, 200);
		s4.Obj(4, 100022, -2321.51f, 233.79f, 4072.76f, 0).Gen(1, 200);
		s4.Obj(5, 100022, -2321.51f, 233.79f, 4072.76f, 0).Gen(1, 200);
		s4.Obj(6, 100022, -2321.51f, 233.79f, 4072.76f, 0).Gen(1, 200);
		s4.Obj(7, 100022, -2321.51f, 233.79f, 4072.76f, 0).Gen(1, 200);
		s4.Obj(8, 100022, -2321.51f, 233.79f, 4072.76f, 0).Gen(1, 200);
		s4.Obj(9, 100022, -2321.51f, 233.79f, 4072.76f, 0).Gen(1, 200);
		s4.Obj(10, 100022, -2321.51f, 233.79f, 4072.76f, 0).Gen(1, 200);
		s4.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "15")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8/STAGE_3_PROG/9/STAGE_3_PROG/10", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8/STAGE_3_PROG/9/STAGE_3_PROG/10", "1", "0");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "270");
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s5.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		var s6 = g.Stage("CHECK", false);
		s6.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020783$*^", "reward_box", "15");
		s6.Start("MGAME_SET_TIMEOUT", "120");
		s6.Event("KEEP", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_SELECT_STAGE_15", "==", "300")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		s6.Event("OUT", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_SELECT_STAGE_15", "==", "100")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		s6.Event("TIMEOUT", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "120")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		var s7 = g.Stage("SUCCESS", false);
		s7.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020816$*^", "move_to_point", "20");
		s7.Start("MGAME_SET_TIMEOUT", "20");
		s7.Obj(0, 40001, -2444.40f, 268.91f, 4283.24f, 91).Named("Earth Tower 16F").Enter("G_TOWER_WARP_TO_16", 50).Neutral();
		s7.Event("RunMgame", 1, 0)
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_16");
		var s8 = g.Stage("OUT", false);
		s8.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20190419_041811$*^", "raid_clear", "60");
		s8.Start("MGAME_SET_TIMEOUT", "60");
		s8.Obj(0, 156162, -2453.76f, 233.79f, 3920.48f, 0).Neutral();
		s8.Obj(1, 160065, -2502.94f, 233.79f, 3907.85f, 0);
		s8.Event("LEAVE", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("MGAME_RETURN")
			.Do("MGAME_END", "1");
		s8.Event("Reward", 1, 0)
			.Do("MGAME_EXEC_ACTORSCP_MAIN", "TX_GT_REWARD_R1_3");
		return g;
	}

	private static MGameData Floor16()
	{
		var g = new MGameData("M_GTOWER_STAGE_16", 16);
		var d0 = new[] { new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER_STAGE_16", "MGTSTAGE16", "195", "@dicID_^*$ETC_20160224_020777$*^", "scroll", "3"), new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER_STAGE_16", "MGTSTAGE16", "1") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020818$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(1, 40001, 2711.24f, 435.42f, -6131.84f, 98).Named("Earth Tower 16F").Enter("G_TOWER_WARP_TO_16", 50).Neutral();
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020819$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, -5229.69f, 268.91f, -4181.67f, 91).Named("Earth Tower 17F").Enter("G_TOWER_WARP_TO_17", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_17");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020821$*^", "stage_start", "20");
		s3.Obj(0, 100035, -5391.21f, 239.93f, -4560.42f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(1, 100035, -5368.65f, 239.93f, -4580.78f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(2, 100035, -5367.65f, 239.93f, -4582.27f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(3, 100035, -5357.00f, 239.93f, -4570.11f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(4, 100035, -5364.77f, 239.93f, -4547.60f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(5, 100035, -5374.08f, 239.93f, -4542.39f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(6, 100035, -5383.64f, 239.93f, -4547.78f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(7, 100035, -5377.97f, 239.93f, -4579.88f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(8, 100036, -5378.26f, 239.93f, -4594.38f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(9, 100036, -5378.26f, 239.93f, -4594.38f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(10, 100036, -5181.60f, 239.93f, -4458.88f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(11, 100036, -5160.23f, 239.93f, -4374.72f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(12, 100036, -5156.20f, 239.93f, -4481.40f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(13, 100036, -5153.75f, 239.93f, -4488.10f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(14, 100070, -5160.95f, 239.93f, -4516.17f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(15, 100070, -5135.86f, 239.93f, -4521.79f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(16, 100070, -5119.31f, 239.93f, -4500.63f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(17, 100070, -5123.93f, 239.93f, -4475.31f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(18, 100070, -5144.88f, 239.93f, -4452.60f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(20, 100001, -4839.00f, 147.66f, -4811.00f, 169).Gen(1, 10).Dead(d0);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "MGTSTAGE16", "OVER", "195")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s3.Event("Act", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "4")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "1", "0");
		var s4 = g.Stage("CNT", false);
		s4.Start("MGAME_SET_TIMEOUT", "270");
		s4.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s4.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG");
		var s5 = g.Stage("RankResetPoint", true);
		return g;
	}

	private static MGameData Floor17()
	{
		var g = new MGameData("M_GTOWER_STAGE_17", 17);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020823$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 154009, -5349.01f, 240.61f, -2110.17f, 0);
		s0.Obj(1, 154009, -5352.88f, 240.61f, -2301.83f, 0);
		s0.Obj(2, 154009, -5159.16f, 240.61f, -2112.30f, 0);
		s0.Obj(3, 154009, -5162.43f, 240.61f, -2307.83f, 0);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020825$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, -5249.42f, 268.91f, -1829.69f, 91).Named("Earth Tower 18F").Enter("G_TOWER_WARP_TO_18", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_18");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020827$*^", "stage_start", "20");
		s3.Obj(0, 100033, -5127.95f, 240.61f, -2135.86f, 0).Gen(1, 300);
		s3.Obj(1, 100034, -5367.83f, 240.61f, -2315.64f, 0).Gen(1, 300);
		s3.Obj(2, 100071, -5380.04f, 240.61f, -2098.24f, 0).Gen(1, 300);
		s3.Obj(3, 20024, -4979.13f, 240.61f, -2218.63f, 0);
		s3.Obj(4, 20024, -5246.32f, 240.61f, -1988.98f, 0);
		s3.Obj(5, 20024, -5541.98f, 240.61f, -2207.10f, 0);
		s3.Obj(6, 20024, -5260.16f, 240.61f, -2468.44f, 0);
		s3.Obj(7, 100071, -5386.48f, 240.61f, -2088.16f, 0).Gen(1, 200);
		s3.Obj(8, 100071, -5386.48f, 240.61f, -2088.16f, 0).Gen(1, 200);
		s3.Obj(9, 100071, -5386.48f, 240.61f, -2088.16f, 0).Gen(1, 200);
		s3.Obj(10, 100071, -5386.48f, 240.61f, -2088.16f, 0).Gen(1, 200);
		s3.Obj(11, 100071, -5386.48f, 240.61f, -2088.16f, 0).Gen(1, 200);
		s3.Obj(12, 100071, -5386.48f, 240.61f, -2088.16f, 0).Gen(1, 200);
		s3.Obj(13, 100071, -5386.48f, 240.61f, -2088.16f, 0).Gen(1, 200);
		s3.Obj(14, 100071, -5386.48f, 240.61f, -2088.16f, 0).Gen(1, 200);
		s3.Obj(15, 100071, -5386.48f, 240.61f, -2088.16f, 0).Gen(1, 200);
		s3.Obj(16, 100071, -5386.48f, 240.61f, -2088.16f, 0).Gen(1, 200);
		s3.Obj(17, 100071, -5386.48f, 240.61f, -2088.16f, 0).Gen(1, 200);
		s3.Obj(18, 100034, -5371.04f, 240.61f, -2311.79f, 0).Gen(1, 200);
		s3.Obj(19, 100034, -5371.04f, 240.61f, -2311.79f, 0).Gen(1, 200);
		s3.Obj(20, 100034, -5371.04f, 240.61f, -2311.79f, 0).Gen(1, 200);
		s3.Obj(21, 100034, -5371.79f, 240.61f, -2311.12f, 0).Gen(1, 200);
		s3.Obj(22, 100034, -5371.79f, 240.61f, -2311.12f, 0).Gen(1, 200);
		s3.Obj(23, 100034, -5371.79f, 240.61f, -2311.12f, 0).Gen(1, 200);
		s3.Obj(24, 100034, -5373.08f, 240.61f, -2310.96f, 0).Gen(1, 200);
		s3.Obj(25, 100034, -5373.08f, 240.61f, -2310.96f, 0).Gen(1, 200);
		s3.Obj(26, 100034, -5373.08f, 240.61f, -2310.96f, 0).Gen(1, 200);
		s3.Obj(27, 100034, -5373.08f, 240.61f, -2310.96f, 0).Gen(1, 200);
		s3.Obj(28, 100033, -5125.13f, 240.61f, -2135.84f, 0).Gen(1, 200);
		s3.Obj(29, 100033, -5125.13f, 240.61f, -2135.84f, 0).Gen(1, 200);
		s3.Obj(30, 100033, -5125.13f, 240.61f, -2135.84f, 0).Gen(1, 200);
		s3.Obj(31, 100033, -5125.13f, 240.61f, -2135.84f, 0).Gen(1, 200);
		s3.Obj(32, 100033, -5125.13f, 240.61f, -2135.84f, 0).Gen(1, 200);
		s3.Obj(33, 100033, -5125.13f, 240.61f, -2135.84f, 0).Gen(1, 200);
		s3.Obj(34, 100033, -5125.13f, 240.61f, -2135.84f, 0).Gen(1, 200);
		s3.Obj(35, 100033, -5125.13f, 240.61f, -2135.84f, 0).Gen(1, 200);
		s3.Obj(36, 100033, -5125.13f, 240.61f, -2135.84f, 0).Gen(1, 200);
		s3.Obj(37, 100033, -5125.13f, 240.61f, -2135.84f, 0).Gen(1, 200);
		s3.Event("Killer", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6", "1", "0");
		s3.Event("ACT1", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/2/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17", "3")
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/2/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17", "1", "0");
		s3.Event("ACT2", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/1/STAGE_1_PROG/18/STAGE_1_PROG/19/STAGE_1_PROG/20/STAGE_1_PROG/21/STAGE_1_PROG/22/STAGE_1_PROG/23/STAGE_1_PROG/24/STAGE_1_PROG/25/STAGE_1_PROG/26/STAGE_1_PROG/27", "6")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/1/STAGE_1_PROG/18/STAGE_1_PROG/19/STAGE_1_PROG/20/STAGE_1_PROG/21/STAGE_1_PROG/22/STAGE_1_PROG/23/STAGE_1_PROG/24/STAGE_1_PROG/25/STAGE_1_PROG/26/STAGE_1_PROG/27", "1", "0");
		s3.Event("ACT3", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/28/STAGE_1_PROG/29/STAGE_1_PROG/30/STAGE_1_PROG/31/STAGE_1_PROG/32/STAGE_1_PROG/33/STAGE_1_PROG/34/STAGE_1_PROG/35/STAGE_1_PROG/36/STAGE_1_PROG/37", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/28/STAGE_1_PROG/29/STAGE_1_PROG/30/STAGE_1_PROG/31/STAGE_1_PROG/32/STAGE_1_PROG/33/STAGE_1_PROG/34/STAGE_1_PROG/35/STAGE_1_PROG/36/STAGE_1_PROG/37", "1", "0");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Obj(0, 100002, -5253.02f, 240.61f, -2213.83f, 0);
		s4.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "15")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0", "0")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0", "1", "0");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "30");
		s5.Obj(0, 100004, -5569.69f, 240.61f, -2353.45f, 0);
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s5.Event("SUCC_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s5.Event("Fail_npcCNT", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/0/SETTING/1/SETTING/2/SETTING/3", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		return g;
	}

	private static MGameData Floor18()
	{
		var g = new MGameData("M_GTOWER_STAGE_18", 18);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020830$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 100078, -4723.51f, 240.61f, -67.19f, 0);
		s0.Obj(1, 100078, -4794.90f, 240.61f, -145.35f, 0);
		s0.Obj(2, 100078, -4908.20f, 240.61f, -142.76f, 0);
		s0.Obj(3, 100078, -4987.19f, 240.61f, -67.67f, 0);
		s0.Obj(4, 100078, -4982.83f, 240.61f, 31.16f, 0);
		s0.Obj(5, 100078, -4906.61f, 240.61f, 122.12f, 0);
		s0.Obj(6, 100078, -4791.33f, 240.61f, 119.40f, 0);
		s0.Obj(7, 100078, -4722.76f, 240.61f, 37.39f, 0);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020832$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, -4840.19f, 265.27f, 344.71f, 91).Named("Earth Tower 19F").Enter("G_TOWER_WARP_TO_19", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_19");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020834$*^", "stage_start", "20");
		s3.Obj(0, 100041, -4906.32f, 240.61f, -33.42f, 0).Gen(1, 300);
		s3.Obj(1, 100037, -4801.78f, 240.61f, -31.00f, 0).Gen(1, 300);
		s3.Obj(2, 100038, -4858.60f, 240.61f, 49.86f, 0).Gen(1, 300);
		s3.Obj(3, 100041, -4902.19f, 240.61f, -32.34f, 0).Gen(1, 300);
		s3.Obj(4, 100041, -4902.19f, 240.61f, -32.34f, 0).Gen(1, 300);
		s3.Obj(5, 100041, -4902.19f, 240.61f, -32.34f, 0).Gen(1, 300);
		s3.Obj(6, 100041, -4902.19f, 240.61f, -32.34f, 0).Gen(1, 300);
		s3.Obj(7, 100041, -4902.19f, 240.61f, -32.34f, 0).Gen(1, 300);
		s3.Obj(8, 100041, -4902.19f, 240.61f, -32.34f, 0).Gen(1, 300);
		s3.Obj(9, 100041, -4902.19f, 240.61f, -32.34f, 0).Gen(1, 300);
		s3.Obj(10, 100038, -4857.84f, 240.61f, 49.05f, 0).Gen(1, 300);
		s3.Obj(11, 100038, -4857.84f, 240.61f, 49.05f, 0).Gen(1, 300);
		s3.Obj(12, 100038, -4857.84f, 240.61f, 49.05f, 0).Gen(1, 300);
		s3.Obj(13, 100037, -4805.31f, 240.61f, -30.33f, 0).Gen(1, 300);
		s3.Obj(14, 100037, -4805.31f, 240.61f, -30.33f, 0).Gen(1, 300);
		s3.Obj(15, 100037, -4805.31f, 240.61f, -30.33f, 0).Gen(1, 300);
		s3.Obj(16, 100037, -4805.31f, 240.61f, -30.33f, 0).Gen(1, 300);
		s3.Obj(17, 100037, -4805.80f, 240.61f, -30.83f, 0).Gen(1, 300);
		s3.Obj(18, 100037, -4806.30f, 240.61f, -31.32f, 0).Gen(1, 300);
		s3.Obj(19, 100037, -4806.30f, 240.61f, -31.32f, 0).Gen(1, 300);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s3.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "10")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "1", "0");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020835$*^", "stage_start", "20");
		s4.Obj(0, 100003, -4848.34f, 240.61f, -22.31f, 0);
		s4.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0", "0")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0", "1", "0");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "270");
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		s5.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		s5.Event("SUCC_npcCNT", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/0/SETTING/1/SETTING/2/SETTING/3/SETTING/4/SETTING/5/SETTING/6/SETTING/7", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		return g;
	}

	private static MGameData Floor19()
	{
		var g = new MGameData("M_GTOWER_STAGE_19", 19);
		var d0 = new[] { new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER_STAGE_19", "GT_STAGE_POINT_19", "1"), new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER_STAGE_19", "GT_STAGE_POINT_19", "80", "@dicID_^*$ETC_20160224_020777$*^", "scroll", "3") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020837$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		s0.Event("Setting", 1, 0)
			.Do("GAME_ST_EVT_EXEC_VALUE", "GT_STAGE_POINT_19", "0");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020838$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, -4947.44f, 268.91f, 2357.74f, 91).Named("Earth Tower 20F").Enter("G_TOWER_WARP_TO_20", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER_STAGE_20");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20180320_031930$*^", "stage_start", "20");
		s3.Obj(0, 100031, -4964.40f, 240.61f, 2077.12f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(1, 100030, -4854.32f, 240.61f, 1920.35f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(2, 100047, -5052.49f, 240.61f, 1906.34f, 0).Gen(1, 200).Dead(d0);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_STAGE_POINT_19", "OVER", "80")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s3.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "1")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2", "7")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2", "1", "0");
		s3.Event("ACT2", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020841$*^", "stage_start", "20");
		s4.Obj(0, 100008, -4943.03f, 240.61f, 1984.30f, 0).Gen(1, 100).Dead(d0);
		s4.Event("ACT", 2, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0", "0")
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0", "1", "0");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "270");
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "ADD_POINT");
		s5.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG");
		return g;
	}

	private static MGameData Floor20()
	{
		var g = new MGameData("M_GTOWER_STAGE_20", 20);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020848$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("FAIL", false);
		s1.Start("MGAME_SET_TIMEOUT", "30");
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s1.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s1.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s2 = g.Stage("STAGE_1_PROG", false);
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020849$*^", "stage_start", "20");
		s2.Obj(0, 100079, -4864.90f, 231.83f, 3910.09f, -95);
		s2.Obj(2, 100009, -4975.02f, 231.83f, 3923.18f, -85);
		s2.Obj(3, 100009, -4770.50f, 231.83f, 3926.96f, -95);
		s2.Event("PROG", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s2.Event("END", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		var s3 = g.Stage("STAGE_2_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020850$*^", "stage_start", "20");
		s3.Obj(0, 100010, -5011.19f, 231.83f, 3723.75f, 53);
		s3.Obj(1, 100029, -5061.61f, 231.83f, 3858.83f, 0).Gen(1, 200);
		s3.Obj(2, 100029, -5061.61f, 231.83f, 3858.83f, 0).Gen(1, 200);
		s3.Obj(3, 100029, -5061.61f, 231.83f, 3858.83f, 0).Gen(1, 200);
		s3.Obj(4, 100029, -5061.61f, 231.83f, 3858.83f, 0).Gen(1, 200);
		s3.Obj(5, 100029, -5061.61f, 231.83f, 3858.83f, 0).Gen(1, 200);
		s3.Obj(6, 100029, -5061.61f, 231.83f, 3858.83f, 0).Gen(1, 200);
		s3.Obj(7, 100029, -5061.61f, 231.83f, 3858.83f, 0).Gen(1, 200);
		s3.Obj(8, 100029, -5061.61f, 231.83f, 3858.83f, 0).Gen(1, 200);
		s3.Obj(9, 100029, -5061.61f, 231.83f, 3858.83f, 0).Gen(1, 200);
		s3.Obj(10, 100029, -5061.61f, 231.83f, 3858.83f, 0).Gen(1, 200);
		s3.Obj(11, 100029, -5061.61f, 231.83f, 3858.83f, 0).Gen(1, 200);
		s3.Obj(12, 100048, -4863.51f, 231.83f, 4121.66f, 0).Gen(1, 200);
		s3.Obj(13, 100048, -4863.51f, 231.83f, 4121.66f, 0).Gen(1, 200);
		s3.Obj(14, 100048, -4863.51f, 231.83f, 4121.66f, 0).Gen(1, 200);
		s3.Obj(15, 100048, -4863.51f, 231.83f, 4121.66f, 0).Gen(1, 200);
		s3.Obj(16, 100048, -4863.51f, 231.83f, 4121.66f, 0).Gen(1, 200);
		s3.Obj(17, 100048, -4863.51f, 231.83f, 4121.66f, 0).Gen(1, 200);
		s3.Obj(18, 100048, -4863.51f, 231.83f, 4121.66f, 0).Gen(1, 200);
		s3.Obj(19, 100048, -4863.51f, 231.83f, 4121.66f, 0).Gen(1, 200);
		s3.Event("PROG", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_3_PROG");
		s3.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10/STAGE_2_PROG/11", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10/STAGE_2_PROG/11", "1", "0");
		s3.Event("ACT2", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/12/STAGE_2_PROG/13/STAGE_2_PROG/14/STAGE_2_PROG/15/STAGE_2_PROG/16/STAGE_2_PROG/17/STAGE_2_PROG/18/STAGE_2_PROG/19", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/12/STAGE_2_PROG/13/STAGE_2_PROG/14/STAGE_2_PROG/15/STAGE_2_PROG/16/STAGE_2_PROG/17/STAGE_2_PROG/18/STAGE_2_PROG/19", "1", "0");
		var s4 = g.Stage("STAGE_3_PROG", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020851$*^", "stage_start", "10");
		s4.Obj(0, 100010, -4725.02f, 231.83f, 3759.74f, 129);
		s4.Obj(1, 100049, -4659.43f, 231.83f, 3982.89f, 0).Gen(1, 200);
		s4.Obj(2, 100049, -4659.43f, 231.83f, 3982.89f, 0).Gen(1, 200);
		s4.Obj(3, 100049, -4659.43f, 231.83f, 3982.89f, 0).Gen(1, 200);
		s4.Obj(4, 100049, -4659.43f, 231.83f, 3982.89f, 0).Gen(1, 200);
		s4.Obj(5, 100049, -4659.43f, 231.83f, 3982.89f, 0).Gen(1, 200);
		s4.Obj(6, 100049, -4659.43f, 231.83f, 3982.89f, 0).Gen(1, 200);
		s4.Obj(7, 100049, -4659.43f, 231.83f, 3982.89f, 0).Gen(1, 200);
		s4.Obj(8, 100049, -4659.43f, 231.83f, 3982.89f, 0).Gen(1, 200);
		s4.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8", "1", "0");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "270");
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s5.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		var s6 = g.Stage("CHECK", false);
		s6.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020783$*^", "reward_box", "15");
		s6.Start("MGAME_SET_TIMEOUT", "120");
		s6.Event("KEEP", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_SELECT_STAGE_20", "==", "300")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		s6.Event("OUT", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_SELECT_STAGE_20", "==", "100")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		s6.Event("TIMEOUT", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "120")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		var s7 = g.Stage("SUCCESS", false);
		s7.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025655$*^", "move_to_point", "20");
		s7.Obj(0, 40001, -4871.91f, 268.91f, 4303.40f, 91).Named("@dicID_^*$ETC_20161005_024891$*^").Enter("G_TOWER_WARP_TO_SECOND", 50).Neutral();
		var s8 = g.Stage("OUT", false);
		s8.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20190419_041812$*^", "raid_clear", "60");
		s8.Start("MGAME_SET_TIMEOUT", "60");
		s8.Obj(0, 156162, 5224.00f, 240.41f, -3844.00f, 0).Neutral();
		s8.Obj(1, 160065, 5199.00f, 212.03f, -2536.00f, 0);
		s8.Event("LEAVE", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("MGAME_RETURN")
			.Do("MGAME_END", "1");
		s8.Event("Reward", 1, 0)
			.Do("MGAME_EXEC_ACTORSCP_MAIN", "TX_GT_REWARD_R1_4");
		var s9 = g.Stage("InCount_Compare", false);
		return g;
	}

	private static MGameData Floor21()
	{
		var g = new MGameData("M_GTOWER2_STAGE_21", 21);
		var d0 = new[] { new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER2_STAGE_21", "MGTSTAGE21", "150", "@dicID_^*$ETC_20160224_020777$*^", "scroll", "3"), new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER2_STAGE_21", "MGTSTAGE21", "1") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025558$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(1, 40001, 5460.21f, 368.15f, -5762.44f, 90).Named("Earth Tower 21F").Enter("G_TOWER_WARP_TO_21", 50).Neutral();
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		s0.Event("Setting", 1, 0)
			.Do("GAME_ST_EVT_EXEC_VALUE", "MGT_STAGE_1", "0");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025560$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, 5319.80f, 247.93f, -3817.45f, 91).Named("Earth Tower 22F").Enter("G_TOWER_WARP_TO_22", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_22");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020776$*^", "stage_start", "20");
		s3.Obj(0, 100116, 5181.39f, 240.41f, -4289.89f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(1, 100116, 5138.78f, 240.41f, -4281.73f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(2, 100116, 5120.82f, 240.41f, -4255.08f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(3, 100116, 5134.15f, 240.41f, -4193.20f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(4, 100116, 5187.62f, 240.41f, -4218.75f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(5, 100116, 5210.11f, 240.41f, -4269.03f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(6, 100116, 5211.45f, 240.41f, -4308.65f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(7, 100116, 5178.56f, 240.41f, -4329.21f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(8, 100116, 5159.89f, 240.41f, -4244.51f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(9, 100116, 5110.87f, 240.41f, -4205.68f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(10, 100104, 5167.70f, 240.41f, -4005.71f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(11, 100104, 5173.07f, 240.41f, -4059.20f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(12, 100104, 5227.58f, 240.41f, -4032.08f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(13, 100104, 5209.00f, 240.41f, -3971.87f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(14, 100104, 5149.18f, 240.41f, -3935.27f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(15, 100103, 5423.71f, 240.41f, -4125.23f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(16, 100103, 5427.77f, 240.41f, -4184.35f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(17, 100103, 5492.39f, 240.41f, -4161.99f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(18, 100103, 5481.55f, 240.41f, -4069.32f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(19, 100103, 5441.64f, 240.41f, -4024.51f, 0).Gen(1, 200).Dead(d0);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "MGTSTAGE21", "OVER", "150")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s3.Event("Act", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "10")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "1", "0");
		var s4 = g.Stage("CNT", false);
		s4.Start("MGAME_SET_TIMEOUT", "270");
		s4.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s4.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG");
		var s5 = g.Stage("RankResetPoint", true);
		return g;
	}

	private static MGameData Floor22()
	{
		var g = new MGameData("M_GTOWER2_STAGE_22", 22);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025562$*^", "stage_ready", "10");
		s0.Obj(0, 154078, 5187.60f, 240.61f, -2099.84f, -89);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025563$*^", "move_to_point", "20");
		s1.Obj(0, 40001, 5187.66f, 267.36f, -1712.13f, 91).Named("Earth Tower 23F").Enter("G_TOWER_WARP_TO_23", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_23");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20200710_049099$*^", "stage_start", "20");
		s3.Obj(0, 100107, 5017.37f, 240.61f, -2193.01f, 0).Gen(1, 400);
		s3.Obj(1, 100107, 4990.46f, 240.61f, -2128.92f, 0).Gen(1, 400);
		s3.Obj(2, 100107, 4977.58f, 240.61f, -2054.99f, 0).Gen(1, 400);
		s3.Obj(3, 100107, 4929.68f, 240.61f, -2071.31f, 0).Gen(1, 400);
		s3.Obj(4, 100107, 4926.34f, 240.61f, -2125.47f, 0).Gen(1, 400);
		s3.Obj(5, 100149, 4954.39f, 240.61f, -2181.14f, 0).Gen(1, 400);
		s3.Obj(6, 100149, 4997.95f, 240.61f, -2257.70f, 0).Gen(1, 400);
		s3.Obj(7, 100149, 5060.74f, 240.61f, -2253.83f, 0).Gen(1, 400);
		s3.Obj(8, 100149, 4997.01f, 240.61f, -2021.68f, 0).Gen(1, 400);
		s3.Obj(9, 100149, 4927.12f, 240.61f, -1997.40f, 0).Gen(1, 400);
		s3.Obj(10, 100107, 5328.10f, 240.61f, -2143.08f, 0).Gen(1, 400);
		s3.Obj(11, 100107, 5291.02f, 240.61f, -2006.85f, 0).Gen(1, 400);
		s3.Obj(12, 100107, 5257.87f, 240.61f, -1941.71f, 0).Gen(1, 400);
		s3.Obj(13, 100149, 5367.64f, 240.61f, -2243.36f, 0).Gen(1, 400);
		s3.Obj(14, 100149, 5420.35f, 240.61f, -2135.44f, 0).Gen(1, 400);
		s3.Obj(15, 100149, 5377.50f, 240.61f, -1994.36f, 0).Gen(1, 400);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s3.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15", "8")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15", "1", "0");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020847$*^", "stage_start", "20");
		s4.Obj(0, 100156, 5057.87f, 240.61f, -2170.10f, 0).Gen(1, 400);
		s4.Obj(1, 100156, 4975.06f, 240.61f, -2038.14f, 0).Gen(1, 400);
		s4.Obj(2, 100156, 5024.27f, 240.61f, -1939.35f, 0).Gen(1, 400);
		s4.Obj(3, 100156, 5182.58f, 240.61f, -1940.30f, 0).Gen(1, 400);
		s4.Obj(4, 100156, 5299.20f, 240.61f, -2001.95f, 0).Gen(1, 400);
		s4.Obj(5, 100156, 5225.17f, 240.61f, -2229.34f, 0).Gen(1, 400);
		s4.Obj(6, 100156, 5302.21f, 240.61f, -2161.71f, 0).Gen(1, 400);
		s4.Obj(7, 100156, 5160.23f, 240.61f, -2141.72f, 0).Gen(1, 400);
		s4.Obj(8, 100156, 5187.07f, 240.61f, -2024.52f, 0).Gen(1, 400);
		s4.Obj(9, 100156, 5260.01f, 240.61f, -2068.86f, 0).Gen(1, 400);
		s4.Obj(10, 100156, 5112.53f, 240.61f, -2067.56f, 0).Gen(1, 400);
		s4.Obj(11, 100156, 5219.19f, 240.61f, -2128.09f, 0).Gen(1, 400);
		s4.Event("ACT1", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10/STAGE_2_PROG/11", "10")
			.If("GAME_ST_EVT_COND_TIMECHECK", "15")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10/STAGE_2_PROG/11", "1", "0");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "30");
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s5.Event("Succl_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s5.Event("Fail_npcCNT", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/0", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		return g;
	}

	private static MGameData Floor23()
	{
		var g = new MGameData("M_GTOWER2_STAGE_23", 23);
		var d0 = new[] { new MCall("S_AI_DEAD_ADD_BUFF", "GT23_SoulDuel_ATK", "1", "0", "10000", "1", "100") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025567$*^", "stage_ready", "10");
		s0.Obj(0, 100181, 5200.78f, 240.80f, 55.20f, -78);
		s0.Obj(1, 100182, 5225.03f, 240.80f, -7.79f, 0).Gen(1, 50);
		s0.Obj(2, 100182, 5270.43f, 240.80f, 36.17f, 0).Gen(1, 50);
		s0.Obj(3, 100182, 5122.83f, 240.80f, 95.20f, 0).Gen(1, 50);
		s0.Obj(4, 100182, 5238.17f, 240.80f, 115.10f, 0).Gen(1, 50);
		s0.Obj(5, 100182, 5249.83f, 240.80f, 1.26f, 0).Gen(1, 50);
		s0.Obj(6, 100182, 5185.58f, 240.80f, -24.49f, 0).Gen(1, 50);
		s0.Obj(7, 100182, 5152.49f, 240.80f, 7.89f, 0).Gen(1, 50);
		s0.Obj(8, 100182, 5122.18f, 240.80f, 47.48f, 0).Gen(1, 50);
		s0.Obj(9, 100182, 5168.65f, 240.80f, 124.97f, 0).Gen(1, 50);
		s0.Obj(10, 100182, 5270.96f, 240.80f, 66.05f, 0).Gen(1, 50);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025570$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, 5196.65f, 262.11f, 429.07f, 91).Named("Earth Tower 24F").Enter("G_TOWER_WARP_TO_24", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_24");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025571$*^", "stage_start", "20");
		s3.Obj(1, 100113, 4984.84f, 240.80f, 34.70f, 0).Gen(1, 400);
		s3.Obj(0, 100113, 5005.18f, 240.80f, 106.14f, 0).Gen(1, 400);
		s3.Obj(2, 100113, 5037.28f, 240.80f, 64.29f, 0).Gen(1, 400);
		s3.Obj(3, 100113, 5027.67f, 240.80f, 28.36f, 0).Gen(1, 400);
		s3.Obj(4, 100113, 5018.82f, 240.80f, -6.31f, 0).Gen(1, 400);
		s3.Obj(5, 100113, 4968.29f, 240.80f, 14.27f, 0).Gen(1, 400);
		s3.Obj(6, 100113, 4959.84f, 240.80f, 86.97f, 0).Gen(1, 400);
		s3.Obj(7, 100113, 4984.08f, 240.80f, 113.29f, 0).Gen(1, 400);
		s3.Obj(8, 100142, 5182.94f, 240.80f, -131.89f, 0).Gen(1, 400).Dead(d0);
		s3.Obj(9, 100142, 5238.03f, 240.80f, -123.75f, 0).Gen(1, 400).Dead(d0);
		s3.Obj(10, 100142, 5286.69f, 240.80f, -97.16f, 0).Gen(1, 400).Dead(d0);
		s3.Obj(11, 100142, 5347.91f, 240.80f, -41.31f, 0).Gen(1, 400).Dead(d0);
		s3.Obj(12, 100142, 5370.01f, 240.80f, 5.67f, 0).Gen(1, 400).Dead(d0);
		s3.Obj(13, 100142, 5372.28f, 240.80f, 41.89f, 0).Gen(1, 400).Dead(d0);
		s3.Obj(14, 100142, 5369.27f, 240.80f, 81.23f, 0).Gen(1, 400).Dead(d0);
		s3.Event("END", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/0", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s3.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/1/STAGE_1_PROG/0/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14", "3")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/1/STAGE_1_PROG/0/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14", "1", "0");
		s3.Event("cnt", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020860$*^", "stage_start", "20");
		s4.Obj(0, 100142, 5359.26f, 240.80f, 65.27f, 0).Gen(1, 400).Dead(d0);
		s4.Obj(1, 100142, 5378.06f, 240.80f, 15.12f, 0).Gen(1, 400).Dead(d0);
		s4.Obj(2, 100142, 5348.33f, 240.80f, -13.26f, 0).Gen(1, 400).Dead(d0);
		s4.Obj(3, 100142, 5314.98f, 240.80f, -45.27f, 0).Gen(1, 400).Dead(d0);
		s4.Obj(4, 100142, 5320.09f, 240.80f, -105.46f, 0).Gen(1, 400).Dead(d0);
		s4.Obj(5, 100142, 5392.96f, 240.80f, -43.01f, 0).Gen(1, 400).Dead(d0);
		s4.Obj(6, 100142, 5427.05f, 240.80f, 55.86f, 0).Gen(1, 400).Dead(d0);
		s4.Obj(7, 100113, 5095.83f, 240.80f, -93.71f, 0).Gen(1, 400);
		s4.Obj(8, 100113, 5045.99f, 240.80f, 28.23f, 0).Gen(1, 400);
		s4.Obj(9, 100113, 5046.08f, 240.80f, 76.90f, 0).Gen(1, 400);
		s4.Obj(10, 100113, 5055.14f, 240.80f, 140.28f, 0).Gen(1, 400);
		s4.Obj(11, 100113, 5066.92f, 240.80f, 178.03f, 0).Gen(1, 400);
		s4.Obj(12, 100113, 5042.37f, 240.80f, -4.44f, 0).Gen(1, 400);
		s4.Obj(13, 100113, 5022.48f, 240.80f, -26.16f, 0).Gen(1, 400);
		s4.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10/STAGE_2_PROG/11/STAGE_2_PROG/12/STAGE_2_PROG/13", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10/STAGE_2_PROG/11/STAGE_2_PROG/12/STAGE_2_PROG/13", "1", "0");
		s4.Event("cnt", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_3_PROG");
		var s5 = g.Stage("STAGE_3_PROG", false);
		s5.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020861$*^", "stage_start", "20");
		s5.Obj(0, 100129, 5101.97f, 240.80f, 205.10f, 0).Gen(1, 400);
		s5.Obj(1, 100129, 5185.38f, 240.80f, 286.65f, 0).Gen(1, 400);
		s5.Obj(2, 100129, 5253.19f, 240.80f, 230.30f, 0).Gen(1, 400);
		s5.Obj(3, 100129, 5267.78f, 240.80f, 184.16f, 0).Gen(1, 400);
		s5.Obj(4, 100129, 5196.72f, 240.80f, 210.79f, 0).Gen(1, 400);
		s5.Obj(5, 100129, 5165.97f, 240.80f, 216.15f, 0).Gen(1, 400);
		s5.Obj(6, 100129, 5146.45f, 240.80f, 183.16f, 0).Gen(1, 400);
		s5.Obj(7, 100113, 5088.65f, 240.80f, -97.69f, 0).Gen(1, 400);
		s5.Obj(8, 100113, 5172.72f, 240.80f, -127.50f, 0).Gen(1, 400);
		s5.Obj(9, 100113, 5208.35f, 240.80f, -119.45f, 0).Gen(1, 400);
		s5.Obj(10, 100113, 5284.67f, 240.80f, -82.61f, 0).Gen(1, 400);
		s5.Obj(11, 100113, 5309.78f, 240.80f, -58.84f, 0).Gen(1, 400);
		s5.Obj(12, 100113, 5299.68f, 240.80f, -108.58f, 0).Gen(1, 400);
		s5.Obj(13, 100113, 5193.83f, 240.80f, -169.43f, 0).Gen(1, 400);
		s5.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_3_PROG/0/STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8/STAGE_3_PROG/9/STAGE_3_PROG/10/STAGE_3_PROG/11/STAGE_3_PROG/12/STAGE_3_PROG/13/STAGE_3_PROG/14", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_3_PROG/0/STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8/STAGE_3_PROG/9/STAGE_3_PROG/10/STAGE_3_PROG/11/STAGE_3_PROG/12/STAGE_3_PROG/13/STAGE_3_PROG/14", "1", "0");
		var s6 = g.Stage("CNT", false);
		s6.Start("MGAME_SET_TIMEOUT", "270");
		s6.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL");
		s6.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL");
		return g;
	}

	private static MGameData Floor24()
	{
		var g = new MGameData("M_GTOWER2_STAGE_24", 24);
		var d0 = new[] { new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER2_STAGE_24", "GT_STAGE_POINT_24", "1"), new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER2_STAGE_24", "GT_STAGE_POINT_24", "50", "@dicID_^*$ETC_20160224_020777$*^", "scroll", "3") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025573$*^", "stage_ready", "10");
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		s0.Event("Setting", 1, 0)
			.Do("GAME_ST_EVT_EXEC_VALUE", "GT_STAGE_POINT_24", "0");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025574$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, 5115.42f, 267.42f, 2716.28f, 91).Named("Earth Tower 25F").Enter("G_TOWER_WARP_TO_25", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_25");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20180320_031924$*^", "stage_start", "20");
		s3.Obj(0, 100141, 5018.51f, 240.36f, 2335.98f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(1, 100145, 5187.56f, 240.36f, 2331.76f, 0).Gen(1, 200).Dead(d0);
		s3.Event("ACT1", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/1/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/0", "8")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1", "1", "0");
		s3.Event("ACT2", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_STAGE_POINT_24", "OVER", "50")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025576$*^", "stage_start", "20");
		s4.Obj(0, 100146, 5106.57f, 240.36f, 2331.95f, 0).Gen(1, 200).Dead(d0);
		s4.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0", "0")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0", "1", "0");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "270");
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "CNT");
		s5.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG");
		return g;
	}

	private static MGameData Floor25()
	{
		var g = new MGameData("M_GTOWER2_STAGE_25", 25);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025578$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("FAIL", false);
		s1.Start("MGAME_SET_TIMEOUT", "30");
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s1.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s1.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s2 = g.Stage("STAGE_1_PROG", false);
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025579$*^", "stage_start", "20");
		s2.Obj(0, 100167, 4834.46f, 239.89f, 4614.31f, -93);
		s2.Obj(1, 100159, 4616.48f, 239.89f, 4657.34f, 0).Gen(3, 200);
		s2.Obj(2, 100159, 4617.17f, 239.89f, 4624.27f, 0).Gen(3, 200);
		s2.Obj(3, 100159, 4621.74f, 239.89f, 4584.96f, 0).Gen(3, 200);
		s2.Obj(4, 100159, 4630.71f, 239.89f, 4532.50f, 0).Gen(3, 200);
		s2.Obj(5, 100159, 4647.86f, 239.89f, 4488.70f, 0).Gen(3, 200);
		s2.Obj(6, 100161, 5023.63f, 239.89f, 4844.06f, 0).Gen(3, 200);
		s2.Obj(7, 100161, 5037.17f, 239.89f, 4794.32f, 0).Gen(3, 200);
		s2.Obj(8, 100161, 5054.95f, 239.89f, 4739.06f, 0).Gen(3, 200);
		s2.Obj(9, 100161, 5056.89f, 239.89f, 4688.71f, 0).Gen(3, 200);
		s2.Obj(10, 100161, 5057.24f, 239.89f, 4637.12f, 0).Gen(3, 200);
		s2.Event("END", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		var s3 = g.Stage("CNT", false);
		s3.Start("MGAME_SET_TIMEOUT", "270");
		s3.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG");
		s3.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG");
		var s4 = g.Stage("CHECK", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020783$*^", "reward_box", "15");
		s4.Start("MGAME_SET_TIMEOUT", "120");
		s4.Event("KEEP", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_SELECT_STAGE_25", "==", "300")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		s4.Event("OUT", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_SELECT_STAGE_25", "==", "100")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		s4.Event("TIMEOUT", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "120")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		var s5 = g.Stage("SUCCESS", false);
		s5.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025581$*^", "move_to_point", "20");
		s5.Start("MGAME_SET_TIMEOUT", "20");
		s5.Obj(0, 40001, 4832.84f, 241.20f, 4952.13f, 91).Named("Earth Tower 26F").Enter("G_TOWER_WARP_TO_26", 50).Neutral();
		s5.Event("RunMgame", 1, 0)
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_26");
		var s6 = g.Stage("OUT", false);
		s6.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20190419_041805$*^", "raid_clear", "60");
		s6.Start("MGAME_SET_TIMEOUT", "60");
		s6.Obj(0, 156162, 4838.61f, 239.89f, 4620.06f, 0).Neutral();
		s6.Obj(1, 160065, 4778.38f, 239.89f, 4621.39f, 0);
		s6.Event("LEAVE", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("MGAME_RETURN")
			.Do("MGAME_END", "1");
		s6.Event("Reward", 1, 0)
			.Do("MGAME_EXEC_ACTORSCP_MAIN", "TX_GT_REWARD_R2_1");
		return g;
	}

	private static MGameData Floor26()
	{
		var g = new MGameData("M_GTOWER2_STAGE_26", 26);
		var d0 = new[] { new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER2_STAGE_26", "MGTSTAGE26", "1"), new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER2_STAGE_26", "MGTSTAGE26", "165", "@dicID_^*$ETC_20160224_020777$*^", "scroll", "3") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20190419_041806$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(1, 40001, 5455.90f, 373.45f, -5751.13f, 88).Named("Earth Tower 26F").Enter("G_TOWER_WARP_TO_26", 50).Neutral();
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025584$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, 2707.19f, 265.99f, -3999.75f, 91).Named("Earth Tower 27F").Enter("G_TOWER_WARP_TO_27", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_27");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020876$*^", "stage_start", "20");
		s3.Obj(0, 100112, 2527.38f, 239.76f, -4421.18f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(1, 100112, 2515.02f, 239.76f, -4361.34f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(2, 100112, 2528.71f, 239.76f, -4294.57f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(3, 100112, 2579.37f, 239.76f, -4287.57f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(4, 100112, 2592.97f, 239.76f, -4339.68f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(5, 100112, 2593.91f, 239.76f, -4395.33f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(6, 100112, 2589.90f, 239.76f, -4434.59f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(7, 100112, 2537.38f, 239.76f, -4451.76f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(8, 100112, 2567.22f, 239.76f, -4468.10f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(9, 100112, 2580.53f, 239.76f, -4499.69f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(10, 100111, 2829.20f, 239.76f, -4284.84f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(11, 100111, 2823.68f, 239.76f, -4323.08f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(12, 100111, 2830.43f, 239.76f, -4364.21f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(13, 100111, 2834.33f, 239.76f, -4395.29f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(14, 100111, 2850.03f, 239.76f, -4417.59f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(15, 100110, 2747.16f, 239.76f, -4540.78f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(16, 100110, 2780.68f, 239.76f, -4533.01f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(17, 100110, 2804.62f, 239.76f, -4525.41f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(18, 100110, 2843.06f, 239.76f, -4517.35f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(19, 100110, 2870.58f, 239.76f, -4509.17f, 0).Gen(1, 200).Dead(d0);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "MGTSTAGE26", "OVER", "165")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s3.Event("Act", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "10")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "1", "0");
		var s4 = g.Stage("CNT", false);
		s4.Start("MGAME_SET_TIMEOUT", "270");
		s4.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s4.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG");
		var s5 = g.Stage("RankResetPoint", true);
		return g;
	}

	private static MGameData Floor27()
	{
		var g = new MGameData("M_GTOWER2_STAGE_27", 27);
		var d0 = new[] { new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER2_STAGE_27", "MGTSTAGE27", "1"), new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER2_STAGE_27", "MGTSTAGE27", "6", "@dicID_^*$ETC_20161005_025588$*^", "!", "5") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025586$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 154077, 2425.18f, 232.97f, -2124.87f, 0).Dead(d0);
		s0.Obj(1, 154077, 2401.46f, 232.97f, -2001.59f, 0).Dead(d0);
		s0.Obj(2, 154077, 2535.10f, 232.97f, -2017.30f, 0).Dead(d0);
		s0.Obj(3, 154077, 2621.22f, 232.97f, -2116.77f, 0).Dead(d0);
		s0.Obj(4, 154077, 2549.02f, 232.97f, -2237.88f, 0).Dead(d0);
		s0.Obj(5, 154077, 2437.22f, 232.97f, -2237.31f, 0).Dead(d0);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		s0.Event("SET", 1, 0)
			.Do("GAME_ST_EVT_EXEC_VALUE", "MGTSTAGE27", "0");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025589$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, 2464.14f, 267.13f, -1710.78f, 91).Named("Earth Tower 28F").Enter("G_TOWER_WARP_TO_28", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_28");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025591$*^", "stage_start", "20");
		s3.Obj(0, 100135, 2284.32f, 232.97f, -1979.15f, 0).Gen(1, 200);
		s3.Obj(1, 100135, 2308.30f, 232.97f, -1934.17f, 0).Gen(1, 200);
		s3.Obj(2, 100135, 2335.57f, 232.97f, -1945.29f, 0).Gen(1, 200);
		s3.Obj(3, 100135, 2340.73f, 232.97f, -2002.06f, 0).Gen(1, 200);
		s3.Obj(4, 100135, 2305.48f, 232.97f, -2023.73f, 0).Gen(1, 200);
		s3.Obj(5, 100135, 2620.14f, 232.97f, -1897.82f, 0).Gen(1, 200);
		s3.Obj(6, 100139, 2638.01f, 232.97f, -1877.33f, 0).Gen(1, 200);
		s3.Obj(7, 100139, 2675.75f, 232.97f, -1919.70f, 0).Gen(1, 200);
		s3.Obj(8, 100139, 2689.09f, 232.97f, -1962.00f, 0).Gen(1, 200);
		s3.Obj(9, 100139, 2670.05f, 232.97f, -1970.55f, 0).Gen(1, 200);
		s3.Obj(10, 100139, 2283.34f, 232.97f, -2265.01f, 0).Gen(1, 200);
		s3.Obj(11, 100139, 2655.73f, 232.97f, -2214.34f, 0).Gen(1, 200);
		s3.Obj(12, 100139, 2319.07f, 232.97f, -2242.00f, 0).Gen(1, 200);
		s3.Obj(13, 100139, 2349.10f, 232.97f, -2283.71f, 0).Gen(1, 200);
		s3.Obj(14, 100135, 2329.92f, 232.97f, -2295.58f, 0).Gen(1, 200);
		s3.Obj(15, 100135, 2613.90f, 232.97f, -2305.16f, 0).Gen(1, 200);
		s3.Obj(16, 100135, 2609.44f, 232.97f, -2276.95f, 0).Gen(1, 200);
		s3.Obj(17, 100135, 2617.81f, 232.97f, -2260.61f, 0).Gen(1, 200);
		s3.Obj(18, 100135, 2640.10f, 232.97f, -2274.75f, 0).Gen(1, 200);
		s3.Obj(19, 100135, 2649.77f, 232.97f, -2296.15f, 0).Gen(1, 200);
		s3.Obj(20, 100135, 2252.61f, 232.97f, -2237.64f, 0).Gen(1, 200);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s3.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "15")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19/STAGE_1_PROG/20", "12")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19/STAGE_1_PROG/20", "1", "0");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025592$*^", "stage_start", "20");
		s4.Obj(0, 100138, 2432.43f, 232.97f, -2135.16f, 0).Gen(1, 200);
		s4.Obj(1, 100138, 2418.65f, 232.97f, -2090.19f, 0).Gen(1, 200);
		s4.Obj(2, 100138, 2441.55f, 232.97f, -2068.38f, 0).Gen(1, 200);
		s4.Obj(3, 100138, 2508.35f, 232.97f, -2073.11f, 0).Gen(1, 200);
		s4.Obj(4, 100138, 2540.47f, 232.97f, -2126.53f, 0).Gen(1, 200);
		s4.Obj(5, 100138, 2525.31f, 232.97f, -2157.88f, 0).Gen(1, 200);
		s4.Obj(6, 100138, 2472.36f, 232.97f, -2198.83f, 0).Gen(1, 200);
		s4.Obj(7, 100138, 2428.89f, 232.97f, -2174.38f, 0).Gen(1, 200);
		s4.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_3_PROG");
		s4.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7", "3")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7", "1", "0");
		var s5 = g.Stage("STAGE_3_PROG", false);
		s5.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020884$*^", "stage_start", "20");
		s5.Obj(0, 100139, 2425.33f, 232.97f, -2114.06f, 0).Gen(1, 200);
		s5.Obj(1, 100139, 2462.37f, 232.97f, -2031.70f, 0).Gen(1, 200);
		s5.Obj(2, 100139, 2510.15f, 232.97f, -2104.18f, 0).Gen(1, 200);
		s5.Obj(3, 100139, 2495.72f, 232.97f, -2156.98f, 0).Gen(1, 200);
		s5.Obj(4, 100139, 2420.54f, 232.97f, -2180.66f, 0).Gen(1, 200);
		s5.Obj(5, 100139, 2350.79f, 232.97f, -2151.17f, 0).Gen(1, 200);
		s5.Obj(6, 100139, 2340.73f, 232.97f, -2009.07f, 0).Gen(1, 200);
		s5.Obj(7, 100139, 2413.20f, 232.97f, -1947.90f, 0).Gen(1, 200);
		s5.Obj(8, 100139, 2520.50f, 232.97f, -1996.17f, 0).Gen(1, 200);
		s5.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "5")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_3_PROG/0/STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_3_PROG/0/STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8", "1", "0");
		var s6 = g.Stage("CNT", false);
		s6.Start("MGAME_SET_TIMEOUT", "30");
		s6.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		s6.Event("SUCC", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s6.Event("fail", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/0/SETTING/1/SETTING/2/SETTING/3/SETTING/4/SETTING/5", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG");
		return g;
	}

	private static MGameData Floor28()
	{
		var g = new MGameData("M_GTOWER2_STAGE_28", 28);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025594$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 100184, 2678.90f, 239.70f, -12.17f, 0);
		s0.Obj(1, 100185, 2457.27f, 239.70f, 220.62f, 0);
		s0.Obj(2, 100186, 2453.00f, 239.70f, -229.14f, 0);
		s0.Obj(3, 100187, 2209.96f, 239.70f, 5.54f, 0);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025595$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, 2454.16f, 266.86f, 354.87f, 91).Named("Earth Tower 29F").Enter("G_TOWER_WARP_TO_29", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_29");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025597$*^", "stage_start", "20");
		s3.Obj(0, 100125, 2264.76f, 239.70f, -37.12f, 0);
		s3.Obj(1, 100125, 2316.57f, 239.70f, -31.48f, 0);
		s3.Obj(2, 100125, 2315.33f, 239.70f, -82.39f, 0);
		s3.Obj(3, 100125, 2303.80f, 239.70f, 13.19f, 0);
		s3.Obj(4, 100125, 2299.76f, 239.70f, 73.33f, 0);
		s3.Obj(5, 100125, 2248.20f, 239.70f, 35.70f, 0);
		s3.Obj(6, 100125, 2253.42f, 239.70f, -88.44f, 0);
		s3.Obj(7, 100125, 2285.62f, 239.70f, -128.32f, 0);
		s3.Obj(8, 100125, 2295.05f, 239.70f, -145.67f, 0);
		s3.Obj(9, 100126, 2609.16f, 239.70f, -158.05f, 0);
		s3.Obj(10, 100126, 2602.80f, 239.70f, -86.40f, 0);
		s3.Obj(11, 100126, 2600.62f, 239.70f, -19.86f, 0);
		s3.Obj(12, 100126, 2602.64f, 239.70f, 58.17f, 0);
		s3.Obj(13, 100126, 2610.83f, 239.70f, 115.62f, 0);
		s3.Obj(14, 100126, 2671.59f, 239.70f, 42.65f, 0);
		s3.Obj(15, 100126, 2676.34f, 239.70f, -75.55f, 0);
		s3.Obj(16, 100126, 2663.68f, 239.70f, -173.98f, 0);
		s3.Obj(17, 100126, 2645.40f, 239.70f, -133.90f, 0);
		s3.Obj(18, 100126, 2659.31f, 239.70f, -32.50f, 0);
		s3.Event("ACT", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s3.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8", "1", "0");
		s3.Event("ACT2", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18", "1", "0");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Obj(0, 100152, 2327.83f, 239.70f, 23.65f, 0);
		s4.Obj(1, 100152, 2286.03f, 239.70f, -32.60f, 0);
		s4.Obj(2, 100152, 2318.89f, 239.70f, -151.85f, 0);
		s4.Obj(3, 100152, 2445.70f, 239.70f, -189.84f, 0);
		s4.Obj(4, 100152, 2675.91f, 239.70f, -39.55f, 0);
		s4.Obj(5, 100152, 2576.48f, 239.70f, 82.46f, 0);
		s4.Obj(6, 100152, 2460.59f, 239.70f, 60.36f, 0);
		s4.Obj(7, 100152, 2484.04f, 239.70f, -61.81f, 0);
		s4.Obj(8, 100152, 2429.52f, 239.70f, -68.47f, 0);
		s4.Obj(9, 100152, 2439.22f, 239.70f, 2.94f, 0);
		s4.Obj(10, 100152, 2544.44f, 239.70f, -53.13f, 0);
		s4.Obj(11, 100152, 2477.04f, 239.70f, -115.82f, 0);
		s4.Obj(12, 100152, 2330.51f, 239.70f, -89.82f, 0);
		s4.Obj(13, 100152, 2399.73f, 239.70f, 46.03f, 0);
		s4.Obj(14, 100152, 2420.51f, 239.70f, 162.65f, 0);
		s4.Obj(15, 100152, 2561.62f, 239.70f, 33.97f, 0);
		s4.Obj(16, 100152, 2569.23f, 239.70f, -13.93f, 0);
		s4.Obj(17, 100152, 2559.91f, 239.70f, -110.77f, 0);
		s4.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "15")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10/STAGE_2_PROG/11/STAGE_2_PROG/12/STAGE_2_PROG/13/STAGE_2_PROG/14/STAGE_2_PROG/15/STAGE_2_PROG/16/STAGE_2_PROG/17", "8")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10/STAGE_2_PROG/11/STAGE_2_PROG/12/STAGE_2_PROG/13/STAGE_2_PROG/14/STAGE_2_PROG/15/STAGE_2_PROG/16/STAGE_2_PROG/17", "1", "0");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "270");
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		s5.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		s5.Event("END_CNT", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/0/SETTING/1/SETTING/2/SETTING/3", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		return g;
	}

	private static MGameData Floor29()
	{
		var g = new MGameData("M_GTOWER2_STAGE_29", 29);
		var d0 = new[] { new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER2_STAGE_29", "GT_STAGE_POINT_29", "1"), new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER2_STAGE_29", "GT_STAGE_POINT_29", "60", "@dicID_^*$ETC_20160224_020777$*^", "scroll", "3") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025599$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s0.Event("Setting", 1, 0)
			.Do("GAME_ST_EVT_EXEC_VALUE", "GT_STAGE_POINT_29", "0");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025600$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, 2351.93f, 263.98f, 2505.12f, 91).Named("Earth Tower 30F").Enter("G_TOWER_WARP_TO_30", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_30");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20180320_031926$*^", "stage_start", "20");
		s3.Obj(0, 100114, 2289.52f, 239.70f, 2149.08f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(1, 100127, 2390.88f, 239.70f, 2088.70f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(2, 100128, 2398.85f, 239.70f, 2205.72f, 0).Gen(1, 200).Dead(d0);
		s3.Event("ACT1", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0", "3")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0", "1", "0");
		s3.Event("ACT2", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/2", "3")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/2", "1", "0");
		s3.Event("ACT3", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/1", "3")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/1", "1", "0");
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_STAGE_POINT_29", "OVER", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		var s4 = g.Stage("CNT", false);
		s4.Start("MGAME_SET_TIMEOUT", "270");
		s4.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "ADD_POINT");
		s4.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG");
		return g;
	}

	private static MGameData Floor30()
	{
		var g = new MGameData("M_GTOWER2_STAGE_30", 30);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025604$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("FAIL", false);
		s1.Start("MGAME_SET_TIMEOUT", "30");
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s1.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s1.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s2 = g.Stage("STAGE_1_PROG", false);
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025605$*^", "stage_start", "20");
		s2.Obj(0, 100168, 2230.00f, 239.70f, 4491.36f, -88);
		s2.Obj(1, 100147, 1974.37f, 239.70f, 4461.86f, 0).Gen(1, 1);
		s2.Obj(2, 100147, 1975.52f, 239.70f, 4597.20f, 0).Gen(1, 1);
		s2.Obj(3, 100147, 2011.00f, 239.70f, 4686.27f, 0).Gen(1, 1);
		s2.Obj(4, 100147, 1984.50f, 239.70f, 4531.25f, 0).Gen(1, 1);
		s2.Obj(5, 100147, 1970.11f, 239.70f, 4383.49f, 0).Gen(1, 1);
		s2.Event("END", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT");
		s2.Event("PROG", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "90")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		var s3 = g.Stage("STAGE_2_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161214_026370$*^", "stage_start", "20");
		s3.Obj(0, 100148, 2043.63f, 239.70f, 4508.65f, 0).Gen(1, 1);
		s3.Obj(1, 100148, 2043.30f, 239.70f, 4422.33f, 0).Gen(1, 1);
		s3.Obj(2, 100148, 2041.02f, 239.70f, 4468.67f, 0).Gen(1, 1);
		s3.Obj(3, 100148, 2040.06f, 239.70f, 4616.37f, 0).Gen(1, 1);
		s3.Obj(4, 100148, 2038.91f, 239.70f, 4564.44f, 0).Gen(1, 1);
		s3.Obj(5, 100155, 1985.58f, 239.70f, 4503.26f, 0).Gen(1, 1);
		s3.Obj(6, 100155, 2350.55f, 239.70f, 4378.46f, 0).Gen(1, 1);
		s3.Obj(7, 100155, 2358.84f, 239.70f, 4471.51f, 0).Gen(1, 1);
		s3.Obj(8, 100155, 2350.56f, 239.70f, 4538.14f, 0).Gen(1, 1);
		s3.Obj(9, 100155, 2343.59f, 239.70f, 4612.90f, 0).Gen(1, 1);
		s3.Obj(10, 100155, 2364.56f, 239.70f, 4416.38f, 0).Gen(1, 1);
		s3.Obj(11, 100155, 2406.41f, 239.70f, 4489.54f, 0).Gen(1, 1);
		var s4 = g.Stage("CNT", false);
		s4.Start("MGAME_SET_TIMEOUT", "270");
		s4.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL");
		s4.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL");
		var s5 = g.Stage("CHECK", false);
		s5.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020783$*^", "reward_box", "15");
		s5.Start("MGAME_SET_TIMEOUT", "120");
		s5.Event("KEEP", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_SELECT_STAGE_30", "==", "300")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		s5.Event("OUT", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_SELECT_STAGE_30", "==", "100")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		s5.Event("TIMEOUT", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "120")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		var s6 = g.Stage("SUCCESS", false);
		s6.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020785$*^", "move_to_point", "20");
		s6.Start("MGAME_SET_TIMEOUT", "20");
		s6.Obj(0, 40001, 2224.79f, 264.22f, 4850.37f, 91).Named("Earth Tower 31F").Enter("G_TOWER_WARP_TO_31", 50).Neutral();
		s6.Event("RunMGame", 1, 0)
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_31");
		var s7 = g.Stage("OUT", false);
		s7.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20190419_041807$*^", "raid_clear", "60");
		s7.Start("MGAME_SET_TIMEOUT", "60");
		s7.Obj(0, 156162, 2227.51f, 239.70f, 4488.12f, 0).Neutral();
		s7.Obj(1, 160065, 2177.56f, 239.70f, 4486.53f, 0);
		s7.Event("LEAVE", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("MGAME_RETURN")
			.Do("MGAME_END", "1");
		s7.Event("Reward", 1, 0)
			.Do("MGAME_EXEC_ACTORSCP_MAIN", "TX_GT_REWARD_R2_2");
		return g;
	}

	private static MGameData Floor31()
	{
		var g = new MGameData("M_GTOWER2_STAGE_31", 31);
		var d0 = new[] { new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER2_STAGE_31", "MGTSTAGE31", "1"), new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER2_STAGE_31", "MGTSTAGE31", "180", "@dicID_^*$ETC_20160224_020777$*^", "scroll", "3") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025609$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 40001, 5454.20f, 373.75f, -5744.57f, 81).Named("Earth Tower 31F").Enter("G_TOWER_WARP_TO_31", 50).Neutral();
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025610$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, 34.20f, 263.98f, -4296.79f, 91).Named("Earth Tower 32F").Enter("G_TOWER_WARP_TO_32", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_32");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020792$*^", "stage_start", "20");
		s3.Obj(0, 100119, -100.12f, 240.61f, -4667.11f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(1, 100119, -94.76f, 240.61f, -4703.77f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(2, 100119, -88.75f, 240.61f, -4724.32f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(3, 100119, -92.23f, 240.61f, -4743.58f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(4, 100119, -85.12f, 240.61f, -4760.70f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(5, 100118, -81.94f, 240.61f, -4788.44f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(6, 100118, -113.39f, 240.61f, -4776.90f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(7, 100118, -116.88f, 240.61f, -4753.54f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(8, 100118, -123.50f, 240.61f, -4723.04f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(9, 100118, -126.84f, 240.61f, -4693.79f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(10, 100109, 47.72f, 240.61f, -4565.79f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(11, 100109, 73.98f, 240.61f, -4531.63f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(12, 100109, 103.29f, 240.61f, -4508.04f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(13, 100109, 120.28f, 240.61f, -4530.78f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(14, 100109, 97.35f, 240.61f, -4551.53f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(15, 100099, 80.18f, 240.61f, -4738.21f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(16, 100099, 104.60f, 240.61f, -4712.97f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(17, 100099, 130.07f, 240.61f, -4684.95f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(18, 100099, 107.26f, 240.61f, -4737.35f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(19, 100099, 129.97f, 240.61f, -4717.98f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(20, 100179, 33.34f, 240.61f, -4652.49f, 0).Dead(d0);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "MGTSTAGE31", "OVER", "180")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s3.Event("Act", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "6")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "10")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "1", "0");
		s3.Event("ACT2", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/20", "0")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/20", "1", "0");
		var s4 = g.Stage("CNT", false);
		s4.Start("MGAME_SET_TIMEOUT", "270");
		s4.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s4.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG");
		var s5 = g.Stage("RankResetPoint", true);
		return g;
	}

	private static MGameData Floor32()
	{
		var g = new MGameData("M_GTOWER2_STAGE_32", 32);
		var d0 = new[] { new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER2_STAGE_32", "MGTSTAGE32", "2", "@dicID_^*$ETC_20161005_025614$*^", "!", "10"), new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER2_STAGE_32", "MGTSTAGE32", "1") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025612$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 154076, -163.21f, 241.04f, -2403.64f, 0).Dead(d0);
		s0.Obj(1, 154076, 77.22f, 241.04f, -2403.49f, 0).Dead(d0);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025615$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, -34.06f, 267.24f, -2015.42f, 91).Named("Earth Tower 33F").Enter("G_TOWER_WARP_TO_33", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_33");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025617$*^", "stage_start", "20");
		s3.Obj(0, 100136, -207.50f, 241.04f, -2327.10f, 0).Gen(1, 200);
		s3.Obj(1, 100136, -211.57f, 241.04f, -2412.16f, 0).Gen(1, 200);
		s3.Obj(2, 100136, -207.03f, 241.04f, -2447.31f, 0).Gen(1, 200);
		s3.Obj(3, 100136, -204.01f, 241.04f, -2470.77f, 0).Gen(1, 200);
		s3.Obj(4, 100136, -241.43f, 241.04f, -2390.36f, 0).Gen(1, 200);
		s3.Obj(5, 100137, -229.98f, 241.04f, -2323.30f, 0).Gen(1, 200);
		s3.Obj(6, 100137, -216.46f, 241.04f, -2279.48f, 0).Gen(1, 200);
		s3.Obj(7, 100137, -190.73f, 241.04f, -2375.82f, 0).Gen(1, 200);
		s3.Obj(8, 100137, -230.78f, 241.04f, -2456.11f, 0).Gen(1, 200);
		s3.Obj(9, 100137, -234.71f, 241.04f, -2420.13f, 0).Gen(1, 200);
		s3.Obj(10, 100140, 186.01f, 241.04f, -2312.25f, 0).Gen(1, 200);
		s3.Obj(11, 100140, 183.95f, 241.04f, -2354.47f, 0).Gen(1, 200);
		s3.Obj(12, 100140, 181.46f, 241.04f, -2397.53f, 0).Gen(1, 200);
		s3.Obj(13, 100140, 181.88f, 241.04f, -2447.21f, 0).Gen(1, 200);
		s3.Obj(14, 100140, 183.91f, 241.04f, -2475.78f, 0).Gen(1, 200);
		s3.Obj(15, 100165, 223.70f, 241.04f, -2448.72f, 0).Gen(1, 200);
		s3.Obj(16, 100165, 222.11f, 241.04f, -2413.58f, 0).Gen(1, 200);
		s3.Obj(17, 100165, 221.79f, 241.04f, -2380.32f, 0).Gen(1, 200);
		s3.Obj(18, 100165, 220.66f, 241.04f, -2355.48f, 0).Gen(1, 200);
		s3.Obj(19, 100165, 219.02f, 241.04f, -2329.10f, 0).Gen(1, 200);
		s3.Obj(20, 100174, -296.47f, 241.04f, -2417.79f, 0);
		s3.Obj(21, 100174, 310.09f, 241.04f, -2406.91f, 0);
		s3.Event("ACT1", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "10")
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "1", "0");
		s3.Event("", 0, 0)
			.If("MGAME_EVT_COND_PCCNT_OVER", "1")
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/20/STAGE_1_PROG/21", "1", "0");
		var s4 = g.Stage("CNT", false);
		s4.Start("MGAME_SET_TIMEOUT", "30");
		s4.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		s4.Event("SUCC_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		s4.Event("Fail_npcCNT", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/0/SETTING/1", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		return g;
	}

	private static MGameData Floor33()
	{
		var g = new MGameData("M_GTOWER2_STAGE_33", 33);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025619$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 100183, -182.75f, 240.61f, 24.43f, 0);
		s0.Obj(1, 100183, -172.25f, 240.61f, -158.31f, 0);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		s0.Event("Setting", 1, 0)
			.Do("GAME_ST_EVT_EXEC_VALUE", "GT33_HIT_CNT", "0");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025620$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, -166.04f, 266.85f, 306.79f, 91).Named("Earth Tower 34F").Enter("G_TOWER_WARP_TO_34", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_34");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025622$*^", "stage_start", "20");
		s3.Obj(0, 100105, -304.01f, 240.61f, -236.95f, 0).Gen(1, 300);
		s3.Obj(1, 100105, -335.07f, 240.61f, -202.23f, 0).Gen(1, 300);
		s3.Obj(2, 100105, -340.58f, 240.61f, -170.44f, 0).Gen(1, 300);
		s3.Obj(3, 100130, -288.09f, 240.61f, -186.39f, 0).Gen(1, 300);
		s3.Obj(4, 100130, -253.14f, 240.61f, -217.22f, 0).Gen(1, 300);
		s3.Obj(5, 100130, -251.92f, 240.61f, -258.63f, 0).Gen(1, 300);
		s3.Obj(6, 100130, -268.52f, 240.61f, -271.07f, 0).Gen(1, 300);
		s3.Obj(7, 100144, -315.69f, 240.61f, -250.89f, 0).Gen(1, 300);
		s3.Obj(8, 100144, -357.85f, 240.61f, -217.67f, 0).Gen(1, 300);
		s3.Obj(9, 100144, -368.18f, 240.61f, -159.18f, 0).Gen(1, 300);
		s3.Obj(10, 100144, -311.38f, 240.61f, -139.97f, 0).Gen(1, 300);
		s3.Obj(11, 100144, -263.10f, 240.61f, 73.30f, 0).Gen(1, 300);
		s3.Obj(12, 100144, -273.50f, 240.61f, 115.76f, 0).Gen(1, 300);
		s3.Obj(13, 100144, -228.14f, 240.61f, 146.45f, 0).Gen(1, 300);
		s3.Obj(14, 100105, -192.59f, 240.61f, 140.70f, 0).Gen(1, 300);
		s3.Obj(15, 100105, -179.18f, 240.61f, 99.52f, 0).Gen(1, 300);
		s3.Obj(16, 100105, -188.84f, 240.61f, 75.21f, 0).Gen(1, 300);
		s3.Obj(17, 100105, -223.70f, 240.61f, 100.27f, 0).Gen(1, 300);
		s3.Obj(18, 100105, -224.48f, 240.61f, 101.04f, 0).Gen(1, 300);
		s3.Obj(19, 100105, -215.09f, 240.61f, 66.56f, 0).Gen(1, 300);
		s3.Obj(20, 100105, 49.98f, 240.61f, 37.39f, 0).Gen(1, 300);
		s3.Obj(21, 100105, 19.28f, 240.61f, 23.86f, 0).Gen(1, 300);
		s3.Obj(22, 100105, 29.77f, 240.61f, -17.54f, 0).Gen(1, 300);
		s3.Obj(23, 100105, 66.63f, 240.61f, -47.58f, 0).Gen(1, 300);
		s3.Obj(24, 100153, 103.16f, 240.61f, -35.35f, 0).Gen(1, 300);
		s3.Obj(25, 100153, 93.65f, 240.61f, 4.41f, 0).Gen(1, 300);
		s3.Obj(26, 100153, 102.00f, 240.61f, 15.18f, 0).Gen(1, 300);
		s3.Obj(27, 100153, 80.90f, 240.61f, -51.88f, 0).Gen(1, 300);
		s3.Obj(28, 100153, 46.46f, 240.61f, -35.52f, 0).Gen(1, 300);
		s3.Obj(29, 100153, 9.97f, 240.61f, -202.23f, 0).Gen(1, 300);
		s3.Obj(30, 100153, -7.67f, 240.61f, -204.56f, 0).Gen(1, 300);
		s3.Obj(31, 100153, 36.40f, 240.61f, -221.34f, 0).Gen(1, 300);
		s3.Obj(32, 100153, 60.91f, 240.61f, -184.54f, 0).Gen(1, 300);
		s3.Obj(33, 100105, 84.81f, 240.61f, -226.69f, 0).Gen(1, 300);
		s3.Obj(34, 100105, 41.49f, 240.61f, -230.56f, 0).Gen(1, 300);
		s3.Obj(35, 100105, 14.28f, 240.61f, -187.35f, 0).Gen(1, 300);
		s3.Obj(36, 100105, -21.06f, 240.61f, -185.29f, 0).Gen(1, 300);
		s3.Obj(37, 100180, -383.90f, 240.61f, -28.88f, -9);
		s3.Obj(39, 100180, -41.95f, 240.61f, 132.01f, -88);
		s3.Event("END", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/0/SETTING/1", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s3.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/1/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/0/STAGE_1_PROG/2/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "10")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "1", "0");
		s3.Event("ACT2", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/20/STAGE_1_PROG/21/STAGE_1_PROG/22/STAGE_1_PROG/23/STAGE_1_PROG/24/STAGE_1_PROG/25/STAGE_1_PROG/26/STAGE_1_PROG/27/STAGE_1_PROG/28/STAGE_1_PROG/29/STAGE_1_PROG/30/STAGE_1_PROG/31/STAGE_1_PROG/32/STAGE_1_PROG/33/STAGE_1_PROG/34/STAGE_1_PROG/35/STAGE_1_PROG/36", "10")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/20/STAGE_1_PROG/21/STAGE_1_PROG/22/STAGE_1_PROG/23/STAGE_1_PROG/24/STAGE_1_PROG/25/STAGE_1_PROG/26/STAGE_1_PROG/27/STAGE_1_PROG/28/STAGE_1_PROG/29/STAGE_1_PROG/30/STAGE_1_PROG/31/STAGE_1_PROG/32/STAGE_1_PROG/33/STAGE_1_PROG/34/STAGE_1_PROG/35/STAGE_1_PROG/36", "1", "0");
		s3.Event("KEY", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "18")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/37/STAGE_1_PROG/39", "0")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/37/STAGE_1_PROG/39", "1", "0");
		var s4 = g.Stage("CNT", false);
		s4.Start("MGAME_SET_TIMEOUT", "270");
		s4.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL");
		s4.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL");
		return g;
	}

	private static MGameData Floor34()
	{
		var g = new MGameData("M_GTOWER2_STAGE_34", 34);
		var d0 = new[] { new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER2_STAGE_34", "GT_STAGE_POINT_34", "1"), new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER2_STAGE_34", "GT_STAGE_POINT_34", "70", "@dicID_^*$ETC_20160224_020777$*^", "scroll", "3") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025623$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 20026, -180.11f, 240.61f, 2253.26f, 0);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_3_PROG");
		s0.Event("Setting", 1, 0)
			.Do("GAME_ST_EVT_EXEC_VALUE", "GT_STAGE_POINT_34", "0");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025624$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, -175.22f, 266.74f, 2632.96f, 91).Named("Earth Tower 15F").Enter("G_TOWER_WARP_TO_35", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_35");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20180320_031928$*^", "stage_start", "20");
		s3.Obj(0, 100108, -252.29f, 240.61f, 2249.90f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(1, 100122, -179.61f, 240.61f, 2249.67f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(2, 100132, -152.49f, 240.61f, 2330.11f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(3, 100154, -149.71f, 240.61f, 2184.42f, 0).Gen(1, 200).Dead(d0);
		s3.Event("ACT1", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/2/STAGE_1_PROG/3", "6")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/2/STAGE_1_PROG/3", "1", "0");
		s3.Event("Stage2", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s3.Event("ACT2", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/1", "1")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/1", "1", "0");
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_STAGE_POINT_34", "OVER", "70")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Obj(0, 100176, -178.57f, 240.61f, 2249.78f, 0).Gen(1, 200).Dead(d0);
		s4.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0", "0")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0", "1", "0");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "270");
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "ADD_POINT");
		s5.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG");
		return g;
	}

	private static MGameData Floor35()
	{
		var g = new MGameData("M_GTOWER2_STAGE_35", 35);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025626$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("FAIL", false);
		s1.Start("MGAME_SET_TIMEOUT", "30");
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s1.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s1.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s2 = g.Stage("STAGE_1_PROG", false);
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025627$*^", "stage_start", "20");
		s2.Obj(0, 100169, -164.25f, 239.91f, 4442.11f, -86);
		s2.Obj(1, 100160, -350.49f, 239.91f, 4496.59f, 0).Gen(1, 200);
		s2.Obj(2, 100160, -353.75f, 239.91f, 4459.89f, 0).Gen(1, 200);
		s2.Obj(3, 100160, -353.59f, 239.91f, 4423.08f, 0).Gen(1, 200);
		s2.Obj(4, 100160, -360.24f, 239.91f, 4376.38f, 0).Gen(1, 200);
		s2.Obj(5, 100160, -307.73f, 239.91f, 4372.34f, 0).Gen(1, 200);
		s2.Obj(6, 100160, -303.98f, 239.91f, 4421.07f, 0).Gen(1, 200);
		s2.Obj(7, 100160, -309.31f, 239.91f, 4458.33f, 0).Gen(1, 200);
		s2.Obj(8, 100160, -309.37f, 239.91f, 4491.80f, 0).Gen(1, 200);
		s2.Obj(9, 100160, -34.53f, 239.91f, 4383.26f, 0).Gen(1, 200);
		s2.Obj(10, 100160, -39.18f, 239.91f, 4413.37f, 0).Gen(1, 200);
		s2.Obj(11, 100160, -50.69f, 239.91f, 4475.61f, 0).Gen(1, 200);
		s2.Obj(12, 100160, -55.58f, 239.91f, 4524.66f, 0).Gen(1, 200);
		s2.Obj(13, 100160, -8.43f, 239.91f, 4530.75f, 0).Gen(1, 200);
		s2.Obj(14, 100160, -5.14f, 239.91f, 4497.17f, 0).Gen(1, 200);
		s2.Obj(15, 100160, -4.54f, 239.91f, 4448.01f, 0).Gen(1, 200);
		s2.Obj(16, 100160, -0.36f, 239.91f, 4417.48f, 0).Gen(1, 200);
		s2.Obj(17, 100178, -193.86f, 239.91f, 4236.35f, 0);
		s2.Event("END", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s2.Event("STAGE2", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "90")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s2.Event("STAGE3", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "180")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_3_PROG");
		s2.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "15")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16", "1", "0");
		s2.Event("ACT2", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/17", "0")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/17", "1", "0");
		var s3 = g.Stage("STAGE_2_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025628$*^", "stage_start", "10");
		s3.Obj(0, 100163, -297.15f, 239.91f, 4402.06f, 0).Gen(1, 200);
		s3.Obj(1, 100163, -289.32f, 239.91f, 4467.02f, 0).Gen(1, 200);
		s3.Obj(2, 100163, -280.38f, 239.91f, 4514.98f, 0).Gen(1, 200);
		s3.Obj(3, 100163, -276.34f, 239.91f, 4553.75f, 0).Gen(1, 200);
		s3.Obj(4, 100163, -186.17f, 239.91f, 4385.78f, 0).Gen(1, 200);
		s3.Obj(5, 100163, -174.97f, 239.91f, 4437.69f, 0).Gen(1, 200);
		s3.Obj(6, 100163, -162.25f, 239.91f, 4484.51f, 0).Gen(1, 200);
		s3.Obj(7, 100163, -161.67f, 239.91f, 4541.22f, 0).Gen(1, 200);
		s3.Obj(8, 100163, -114.57f, 239.91f, 4377.90f, 0).Gen(1, 200);
		s3.Obj(9, 100163, -96.35f, 239.91f, 4425.31f, 0).Gen(1, 200);
		s3.Obj(10, 100163, -93.31f, 239.91f, 4472.30f, 0).Gen(1, 200);
		s3.Obj(11, 100163, -77.08f, 239.91f, 4516.02f, 0).Gen(1, 200);
		s3.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "15")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10/STAGE_2_PROG/11", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10/STAGE_2_PROG/11", "1", "0");
		var s4 = g.Stage("STAGE_3_PROG", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025628$*^", "stage_start", "10");
		s4.Obj(0, 100166, -218.49f, 239.91f, 4468.94f, 0).Gen(1, 200);
		s4.Obj(1, 100166, -264.31f, 239.91f, 4422.65f, 0).Gen(1, 200);
		s4.Obj(2, 100166, -247.74f, 239.91f, 4531.68f, 0).Gen(1, 200);
		s4.Obj(3, 100166, -148.62f, 239.91f, 4580.36f, 0).Gen(1, 200);
		s4.Obj(4, 100166, -89.54f, 239.91f, 4453.53f, 0).Gen(1, 200);
		s4.Obj(5, 100166, -124.49f, 239.91f, 4364.57f, 0).Gen(1, 200);
		s4.Obj(6, 100166, -207.64f, 239.91f, 4324.20f, 0).Gen(1, 200);
		s4.Obj(7, 100166, -166.07f, 239.91f, 4407.71f, 0).Gen(1, 200);
		s4.Obj(8, 100166, -156.78f, 239.91f, 4481.01f, 0).Gen(1, 200);
		s4.Obj(9, 100166, -89.91f, 239.91f, 4517.30f, 0).Gen(1, 200);
		s4.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "15")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_3_PROG/0/STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8/STAGE_3_PROG/9", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_3_PROG/0/STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8/STAGE_3_PROG/9", "1", "0");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "270");
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s5.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		var s6 = g.Stage("CHECK", false);
		s6.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020783$*^", "reward_box", "15");
		s6.Start("MGAME_SET_TIMEOUT", "120");
		s6.Event("KEEP", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_SELECT_STAGE_35", "==", "300")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		s6.Event("OUT", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_SELECT_STAGE_35", "==", "100")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		s6.Event("TIMEOUT", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "120")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CHECK");
		var s7 = g.Stage("SUCCESS", false);
		s7.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020816$*^", "move_to_point", "20");
		s7.Start("MGAME_SET_TIMEOUT", "20");
		s7.Obj(0, 40001, -160.11f, 267.25f, 4795.23f, 91).Named("Earth Tower 36F").Enter("G_TOWER_WARP_TO_36", 50).Neutral();
		s7.Event("RunMgame", 1, 0)
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_36");
		var s8 = g.Stage("OUT", false);
		s8.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20190419_041808$*^", "raid_clear", "60");
		s8.Start("MGAME_SET_TIMEOUT", "60");
		s8.Obj(0, 156162, -172.37f, 239.91f, 4437.59f, 0).Neutral();
		s8.Obj(1, 160065, -232.12f, 239.91f, 4438.16f, 0);
		s8.Event("LEAVE", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("MGAME_RETURN")
			.Do("MGAME_END", "1");
		s8.Event("Reward", 1, 0)
			.Do("MGAME_EXEC_ACTORSCP_MAIN", "TX_GT_REWARD_R2_3");
		return g;
	}

	private static MGameData Floor36()
	{
		var g = new MGameData("M_GTOWER2_STAGE_36", 36);
		var d0 = new[] { new MCall("SAI_DEAD_ADD_VALUE_NOTICE", "M_GTOWER2_STAGE_36", "MGTSTAGE36", "1", "195", "@dicID_^*$ETC_20160224_020777$*^", "scroll", "3") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025631$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 40001, 5453.81f, 373.47f, -5751.11f, 83).Named("Earth Tower 36F").Enter("G_TOWER_WARP_TO_36", 50).Neutral();
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025632$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, -2771.54f, 257.49f, -4317.93f, 91).Named("Earth Tower 37F").Enter("G_TOWER_WARP_TO_37", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_37");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020821$*^", "stage_start", "20");
		s3.Obj(0, 100098, -2910.45f, 239.93f, -4639.46f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(1, 100098, -2916.62f, 239.93f, -4680.18f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(2, 100098, -2916.09f, 239.93f, -4707.55f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(3, 100098, -2911.92f, 239.93f, -4750.34f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(4, 100098, -2913.31f, 239.93f, -4782.63f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(5, 100117, -2935.63f, 239.93f, -4484.01f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(6, 100117, -2890.48f, 239.93f, -4524.30f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(7, 100117, -2868.29f, 239.93f, -4552.37f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(8, 100117, -2832.43f, 239.93f, -4548.22f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(9, 100117, -2807.00f, 239.93f, -4539.02f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(10, 100120, -2648.43f, 239.93f, -4555.21f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(11, 100120, -2661.65f, 239.93f, -4569.63f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(12, 100120, -2648.94f, 239.93f, -4598.47f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(13, 100120, -2632.00f, 239.93f, -4623.44f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(14, 100120, -2609.20f, 239.93f, -4659.38f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(15, 100164, -2775.52f, 239.93f, -4824.53f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(16, 100164, -2761.87f, 239.93f, -4801.55f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(17, 100164, -2743.02f, 239.93f, -4800.28f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(18, 100164, -2724.81f, 239.93f, -4807.27f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(19, 100164, -2675.48f, 239.93f, -4785.30f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(20, 100175, -2774.69f, 239.93f, -4669.91f, -106).Dead(d0);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "MGTSTAGE36", "OVER", "195")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s3.Event("Act", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "4")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "10")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19", "1", "0");
		s3.Event("ACT2", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "45")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/20", "0")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/20", "1", "0");
		var s4 = g.Stage("CNT", false);
		s4.Start("MGAME_SET_TIMEOUT", "270");
		s4.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s4.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG");
		var s5 = g.Stage("RankResetPoint", true);
		return g;
	}

	private static MGameData Floor37()
	{
		var g = new MGameData("M_GTOWER2_STAGE_37", 37);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025633$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 57580, -2857.76f, 240.61f, -2193.23f, 84).Named("@dicID_^*$QUEST_20160224_003701$*^");
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s0.Event("SET", 1, 0)
			.Do("GAME_ST_EVT_EXEC_VALUE", "GTOWER32", "0");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025634$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, -2855.26f, 267.23f, -2034.81f, 91).Named("Earth Tower 38F").Enter("G_TOWER_WARP_TO_38", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_38");
		s1.Event("SET", 1, 0)
			.Do("GAME_ST_EVT_EXEC_VALUE", "GTOWER32", "2")
			.Do("MGAME_EVT_EXEC_DELMON", "direction/0");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20200710_049100$*^", "stage_start", "20");
		s3.Obj(0, 100177, -2912.27f, 240.61f, -2200.70f, 0).Gen(1, 150);
		s3.Obj(1, 100106, -3114.03f, 240.61f, -2340.13f, 0).Gen(1, 100);
		s3.Obj(2, 100106, -3094.49f, 240.61f, -2463.33f, 0).Gen(1, 100);
		s3.Obj(3, 100106, -3056.63f, 240.61f, -2541.41f, 0).Gen(1, 100);
		s3.Obj(4, 100106, -2968.30f, 240.61f, -2549.41f, 0).Gen(1, 100);
		s3.Obj(5, 100106, -3032.50f, 240.61f, -2433.64f, 0).Gen(1, 100);
		s3.Obj(6, 100106, -3049.25f, 240.61f, -2351.69f, 0).Gen(1, 100);
		s3.Obj(7, 100131, -2591.06f, 240.61f, -2207.08f, 0).Gen(1, 100);
		s3.Obj(8, 100131, -2538.95f, 240.61f, -2287.98f, 0).Gen(1, 100);
		s3.Obj(9, 100131, -2520.64f, 240.61f, -2392.92f, 0).Gen(1, 100);
		s3.Obj(10, 100131, -2536.72f, 240.61f, -2465.53f, 0).Gen(1, 100);
		s3.Obj(11, 100131, -2577.38f, 240.61f, -2511.36f, 0).Gen(1, 100);
		s3.Obj(12, 100131, -2611.57f, 240.61f, -2456.48f, 0).Gen(1, 100);
		s3.Obj(13, 100131, -2593.98f, 240.61f, -2366.87f, 0).Gen(1, 100);
		s3.Obj(14, 100131, -2624.69f, 240.61f, -2261.99f, 0).Gen(1, 100);
		s3.Obj(15, 100102, -2849.71f, 240.61f, -2440.62f, 0).Gen(1, 300);
		s3.Obj(16, 100102, -2861.32f, 240.61f, -2401.17f, 0).Gen(1, 300);
		s3.Obj(17, 100102, -2833.70f, 240.61f, -2415.52f, 0).Gen(1, 300);
		s3.Obj(18, 100102, -2824.23f, 240.61f, -2438.61f, 0).Gen(1, 300);
		s3.Obj(19, 100102, -2841.20f, 240.61f, -2449.04f, 0).Gen(1, 300);
		s3.Obj(20, 100102, -2875.63f, 240.61f, -2427.68f, 0).Gen(1, 300);
		s3.Obj(21, 100102, -2875.87f, 240.61f, -2401.77f, 0).Gen(1, 300);
		s3.Obj(22, 100102, -2858.71f, 240.61f, -2386.13f, 0).Gen(1, 300);
		s3.Obj(23, 100102, -2818.10f, 240.61f, -2405.30f, 0).Gen(1, 300);
		s3.Obj(24, 100102, -2802.79f, 240.61f, -2440.08f, 0).Gen(1, 300);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s3.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6", "1")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6", "1", "0");
		s3.Event("ACT2", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14", "1")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14", "1", "0");
		s3.Event("ACT3", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "15")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19/STAGE_1_PROG/20/STAGE_1_PROG/21/STAGE_1_PROG/22/STAGE_1_PROG/23/STAGE_1_PROG/24", "7")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_1_PROG/18/STAGE_1_PROG/19/STAGE_1_PROG/20/STAGE_1_PROG/21/STAGE_1_PROG/22/STAGE_1_PROG/23/STAGE_1_PROG/24", "1", "0");
		s3.Event("ACT_ELT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "35")
			.If("MGAME_EVT_COND_MONCNT_OVER", "STAGE_1_PROG/0", "1")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0", "1", "0");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Obj(0, 100123, -2960.30f, 240.61f, -2199.11f, 0).Gen(1, 200);
		s4.Obj(1, 100123, -2845.82f, 240.61f, -2146.76f, 0).Gen(1, 200);
		s4.Obj(2, 100123, -2758.72f, 240.61f, -2155.26f, 0).Gen(1, 200);
		s4.Obj(3, 100123, -2687.91f, 240.61f, -2213.74f, 0).Gen(1, 200);
		s4.Obj(4, 100123, -2791.54f, 240.61f, -2254.90f, 0).Gen(1, 200);
		s4.Obj(5, 100123, -2923.21f, 240.61f, -2265.52f, 0).Gen(1, 200);
		s4.Obj(6, 100123, -3048.49f, 240.61f, -2323.85f, 0).Gen(1, 200);
		s4.Obj(7, 100131, -3032.40f, 240.61f, -2564.15f, 0).Gen(1, 200);
		s4.Obj(8, 100131, -3030.98f, 240.61f, -2602.13f, 0).Gen(1, 200);
		s4.Obj(9, 100131, -2890.33f, 240.61f, -2639.12f, 0).Gen(1, 200);
		s4.Obj(10, 100131, -2936.48f, 240.61f, -2569.08f, 0).Gen(1, 200);
		s4.Obj(11, 100131, -2844.20f, 240.61f, -2593.32f, 0).Gen(1, 200);
		s4.Obj(12, 100131, -2711.94f, 240.61f, -2551.11f, 0).Gen(1, 200);
		s4.Obj(13, 100131, -2673.62f, 240.61f, -2582.99f, 0).Gen(1, 200);
		s4.Obj(14, 100131, -2814.22f, 240.61f, -2560.20f, 0).Gen(1, 200);
		s4.Obj(15, 100131, -2871.22f, 240.61f, -2572.18f, 0).Gen(1, 200);
		s4.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10/STAGE_2_PROG/11/STAGE_2_PROG/12/STAGE_2_PROG/13/STAGE_2_PROG/14/STAGE_2_PROG/15", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10/STAGE_2_PROG/11/STAGE_2_PROG/12/STAGE_2_PROG/13/STAGE_2_PROG/14/STAGE_2_PROG/15", "1", "0");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "30");
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s5.Event("SUCC_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "SETTING");
		s5.Event("Fail_npcCNT", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/0/SETTING/1/SETTING/2/SETTING/3", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		var s6 = g.Stage("direction", false);
		s6.Obj(0, 20025, -2858.18f, 240.61f, -2071.53f, 90).Hidden();
		s6.Event("ACT", 1, 0)
			.Do("GAME_ST_EVT_EXEC_VALUE", "GTOWER32", "1");
		return g;
	}

	private static MGameData Floor38()
	{
		var g = new MGameData("M_GTOWER2_STAGE_38", 38);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025638$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 100173, -3143.04f, 240.61f, -121.22f, 0);
		s0.Obj(1, 100173, -2761.61f, 240.61f, -98.79f, 0);
		s0.Obj(2, 100173, -2898.04f, 240.61f, -373.76f, 0);
		s0.Obj(3, 100173, -2792.93f, 240.61f, -577.09f, 0);
		s0.Obj(4, 100173, -3207.61f, 240.61f, -454.33f, 0);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025639$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, -2957.87f, 266.82f, 20.54f, 91).Named("Earth Tower 39F").Enter("G_TOWER_WARP_TO_39", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_39");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025641$*^", "stage_start", "20");
		s3.Obj(0, 100143, -3118.04f, 240.61f, -152.71f, 0).Gen(1, 300);
		s3.Obj(1, 100143, -3087.36f, 240.61f, -202.98f, 0).Gen(1, 300);
		s3.Obj(2, 100143, -3154.28f, 240.61f, -228.05f, 0).Gen(1, 300);
		s3.Obj(3, 100143, -3078.91f, 240.61f, -100.91f, 0).Gen(1, 300);
		s3.Obj(4, 100143, -3049.09f, 240.61f, -151.39f, 0).Gen(1, 300);
		s3.Obj(5, 100143, -3182.62f, 240.61f, -184.48f, 0).Gen(1, 300);
		s3.Obj(6, 100134, -2786.55f, 240.61f, -497.49f, 0).Gen(1, 300);
		s3.Obj(7, 100134, -2765.11f, 240.61f, -443.93f, 0).Gen(1, 300);
		s3.Obj(8, 100134, -2735.50f, 240.61f, -415.55f, 0).Gen(1, 300);
		s3.Obj(9, 100134, -2702.89f, 240.61f, -464.26f, 0).Gen(1, 300);
		s3.Obj(10, 100134, -2733.68f, 240.61f, -497.26f, 0).Gen(1, 300);
		s3.Obj(11, 100134, -2763.64f, 240.61f, -520.49f, 0).Gen(1, 300);
		s3.Obj(12, 100157, -3023.17f, 240.61f, -430.18f, 0).Gen(1, 300);
		s3.Obj(13, 100157, -2976.96f, 240.61f, -498.47f, 0).Gen(1, 300);
		s3.Obj(14, 100157, -2926.38f, 240.61f, -409.36f, 0).Gen(1, 300);
		s3.Obj(15, 100157, -2839.84f, 240.61f, -372.39f, 0).Gen(1, 300);
		s3.Obj(16, 100157, -2865.61f, 240.61f, -302.30f, 0).Gen(1, 300);
		s3.Obj(17, 100157, -2971.40f, 240.61f, -241.38f, 0).Gen(1, 300);
		s3.Obj(18, 100157, -3016.08f, 240.61f, -281.14f, 0).Gen(1, 300);
		s3.Obj(19, 100157, -3075.92f, 240.61f, -313.56f, 0).Gen(1, 300);
		s3.Obj(20, 100157, -2982.13f, 240.61f, -353.05f, 0).Gen(1, 300);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s3.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11", "10")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11", "1", "0");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020835$*^", "stage_start", "20");
		s4.Obj(0, 100150, -3306.09f, 240.61f, -378.55f, 0).Gen(1, 300);
		s4.Obj(1, 100150, -3218.70f, 240.61f, -468.57f, 0).Gen(1, 300);
		s4.Obj(2, 100150, -3230.30f, 240.61f, -400.97f, 0).Gen(1, 300);
		s4.Obj(3, 100150, -3236.00f, 240.61f, -363.30f, 0).Gen(1, 300);
		s4.Obj(4, 100150, -3223.26f, 240.61f, -297.78f, 0).Gen(1, 300);
		s4.Obj(5, 100150, -3165.46f, 240.61f, -381.78f, 0).Gen(1, 300);
		s4.Obj(6, 100157, -3048.59f, 240.61f, -331.13f, 0).Gen(1, 300);
		s4.Obj(7, 100157, -2891.35f, 240.61f, -402.70f, 0).Gen(1, 300);
		s4.Obj(8, 100157, -2965.68f, 240.61f, -388.81f, 0).Gen(1, 300);
		s4.Obj(9, 100157, -3026.10f, 240.61f, -506.48f, 0).Gen(1, 300);
		s4.Obj(10, 100157, -2871.79f, 240.61f, -498.37f, 0).Gen(1, 300);
		s4.Obj(11, 100157, -2956.87f, 240.61f, -272.12f, 0).Gen(1, 300);
		s4.Event("ACT", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "15")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5", "2")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/4/STAGE_2_PROG/5", "1", "0");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "270");
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		s5.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		s5.Event("SUCC_npcCNT", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "SETTING/0/SETTING/1/SETTING/2/SETTING/3/SETTING/4", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		return g;
	}

	private static MGameData Floor39()
	{
		var g = new MGameData("M_GTOWER2_STAGE_39", 39);
		var d0 = new[] { new MCall("SAI_DEAD_ADD_MGAME_V_NAME", "M_GTOWER2_STAGE_39", "GT_STAGE_POINT_39", "1"), new MCall("S_AI_DEAD_GTOWER_DM_NAME", "M_GTOWER2_STAGE_39", "GT_STAGE_POINT_39", "80", "@dicID_^*$ETC_20160224_020777$*^", "scroll", "3") };
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025643$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Obj(0, 20026, -3009.47f, 240.61f, 1986.57f, 0);
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		s0.Event("Setting", 1, 0)
			.Do("GAME_ST_EVT_EXEC_VALUE", "GT_STAGE_POINT_39", "0");
		var s1 = g.Stage("SUCCESS", false);
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025644$*^", "move_to_point", "20");
		s1.Start("MGAME_SET_TIMEOUT", "20");
		s1.Obj(0, 40001, -3003.74f, 265.22f, 2339.74f, 91).Named("Earth Tower 40F").Enter("G_TOWER_WARP_TO_40", 50).Neutral();
		s1.Event("RunMgame", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "20")
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_STAGE_40");
		var s2 = g.Stage("FAIL", false);
		s2.Start("MGAME_SET_TIMEOUT", "30");
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s2.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s2.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT");
		var s3 = g.Stage("STAGE_1_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20180320_031930$*^", "stage_start", "20");
		s3.Obj(0, 100115, -3102.47f, 240.61f, 1997.55f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(1, 100124, -3006.07f, 240.61f, 1894.98f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(2, 100133, -2927.24f, 240.61f, 1988.19f, 0).Gen(1, 200).Dead(d0);
		s3.Obj(3, 100162, -3020.18f, 240.61f, 2083.76f, 0).Gen(1, 200).Dead(d0);
		s3.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_VALUE", "GT_STAGE_POINT_39", "OVER", "80")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "SUCCESS")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s3.Event("ACT1", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/1/STAGE_1_PROG/3", "6")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/1/STAGE_1_PROG/3", "1", "0");
		s3.Event("ACT2", 0, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0/STAGE_1_PROG/2", "0")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/0/STAGE_1_PROG/2", "1", "0");
		s3.Event("Stage2", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		var s4 = g.Stage("STAGE_2_PROG", false);
		s4.Obj(0, 100172, -2623.91f, 147.66f, 1718.76f, -142).Gen(1, 1).Dead(d0);
		s4.Obj(1, 100158, -3005.29f, 240.61f, 1981.58f, 0).Gen(1, 300);
		s4.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0", "0")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0", "2", "0");
		s4.Event("ACT2", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/1", "4")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/1", "1", "0");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "270");
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "ADD_POINT");
		s5.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG");
		return g;
	}

	private static MGameData Floor40()
	{
		var g = new MGameData("M_GTOWER2_STAGE_40", 40);
		var s0 = g.Stage("SETTING", true);
		s0.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025647$*^", "stage_ready", "10");
		s0.Start("MGAME_SET_TIMEOUT", "10");
		s0.Event("END", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "CNT")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_1_PROG");
		var s1 = g.Stage("FAIL", false);
		s1.Start("MGAME_SET_TIMEOUT", "30");
		s1.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20160224_020775$*^", "raid_fail", "30");
		s1.Event("Fail", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "30")
			.Do("MGAME_RETURN");
		s1.Event("EndStage", 1, 0)
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DISABLE", "CNT")
			.Do("MGAME_EVT_EXEC_DELMON", "STAGE_1_PROG/0/STAGE_1_PROG/1/STAGE_1_PROG/2/STAGE_1_PROG/3/STAGE_1_PROG/4/STAGE_1_PROG/5/STAGE_1_PROG/6/STAGE_1_PROG/7/STAGE_1_PROG/8/STAGE_1_PROG/9/STAGE_1_PROG/10/STAGE_1_PROG/11/STAGE_1_PROG/12/STAGE_1_PROG/13/STAGE_1_PROG/14/STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17/STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10/STAGE_2_PROG/11/STAGE_2_PROG/12/STAGE_3_PROG/0/STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8/STAGE_3_PROG/9/STAGE_3_PROG/10/STAGE_3_PROG/11/STAGE_3_PROG/12/STAGE_3_PROG/13/STAGE_3_PROG/14/STAGE_3_PROG/15/STAGE_3_PROG/16/STAGE_3_PROG/17/STAGE_3_PROG/18/STAGE_3_PROG/19");
		var s2 = g.Stage("STAGE_1_PROG", false);
		s2.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025648$*^", "stage_start", "20");
		s2.Obj(0, 100170, -3032.04f, 239.64f, 4270.04f, -95);
		s2.Obj(1, 100151, -3152.85f, 239.64f, 4149.03f, 0).Gen(1, 200);
		s2.Obj(2, 100151, -3077.09f, 239.64f, 4094.17f, 0).Gen(1, 200);
		s2.Obj(3, 100151, -3025.05f, 239.64f, 4061.29f, 0).Gen(1, 200);
		s2.Obj(4, 100151, -2921.45f, 239.64f, 4081.45f, 0).Gen(1, 200);
		s2.Obj(5, 100151, -2855.37f, 239.64f, 4127.44f, 0).Gen(1, 200);
		s2.Obj(6, 100151, -2801.03f, 239.64f, 4252.30f, 0).Gen(1, 200);
		s2.Obj(7, 100151, -2849.86f, 239.64f, 4407.80f, 0).Gen(1, 200);
		s2.Obj(8, 100151, -2950.40f, 239.64f, 4491.69f, 0).Gen(1, 200);
		s2.Obj(9, 100151, -3077.17f, 239.65f, 4557.27f, 0).Gen(1, 200);
		s2.Obj(10, 100151, -3204.32f, 239.64f, 4456.32f, 0).Gen(1, 200);
		s2.Obj(11, 100151, -3249.41f, 239.64f, 4381.38f, 0).Gen(1, 200);
		s2.Obj(12, 100151, -3281.98f, 239.64f, 4281.22f, 0).Gen(1, 200);
		s2.Obj(13, 100151, -3241.60f, 239.64f, 4237.61f, 0).Gen(1, 200);
		s2.Obj(14, 100151, -3218.93f, 239.64f, 4200.69f, 0).Gen(1, 200);
		s2.Obj(15, 100171, -3171.38f, 239.64f, 4067.91f, 0).Gen(1, 200);
		s2.Obj(16, 100171, -3306.20f, 239.64f, 4143.41f, 0).Gen(1, 200);
		s2.Obj(17, 100171, -3217.35f, 239.64f, 4051.33f, 0).Gen(1, 200);
		s2.Event("PROG", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_2_PROG");
		s2.Event("END", 1, 0)
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/0", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_DESTROY", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_1_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_2_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "STAGE_3_PROG")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "OUT")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT")
			.Do("MGAME_EXEC_ACTORSCP_MAIN", "TX_GT_REWARD_R2_4");
		s2.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "15")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17", "0")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_1_PROG/15/STAGE_1_PROG/16/STAGE_1_PROG/17", "1", "0");
		var s3 = g.Stage("STAGE_2_PROG", false);
		s3.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025649$*^", "stage_start", "20");
		s3.Obj(0, 100121, -3184.52f, 239.64f, 4479.91f, 0).Gen(1, 200);
		s3.Obj(1, 100121, -3126.86f, 239.64f, 4491.13f, 0).Gen(1, 200);
		s3.Obj(2, 100121, -3073.21f, 239.64f, 4495.16f, 0).Gen(1, 200);
		s3.Obj(3, 100121, -3014.30f, 239.64f, 4498.62f, 0).Gen(1, 200);
		s3.Obj(5, 100121, -2969.13f, 239.64f, 4495.99f, 0).Gen(1, 200);
		s3.Obj(6, 100121, -2930.97f, 239.64f, 4491.62f, 0).Gen(1, 200);
		s3.Obj(7, 100121, -3156.01f, 239.64f, 4440.42f, 0).Gen(1, 200);
		s3.Obj(8, 100121, -3109.51f, 239.64f, 4436.64f, 0).Gen(1, 200);
		s3.Obj(9, 100121, -3059.21f, 239.64f, 4443.96f, 0).Gen(1, 200);
		s3.Obj(10, 100121, -3020.82f, 239.64f, 4447.41f, 0).Gen(1, 200);
		s3.Obj(11, 100121, -2978.65f, 239.64f, 4442.45f, 0).Gen(1, 200);
		s3.Obj(12, 100121, -2935.90f, 239.64f, 4446.03f, 0).Gen(1, 200);
		s3.Event("PROG", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "STAGE_3_PROG");
		s3.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "5")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10/STAGE_2_PROG/11/STAGE_2_PROG/12", "5")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_2_PROG/0/STAGE_2_PROG/1/STAGE_2_PROG/2/STAGE_2_PROG/3/STAGE_2_PROG/5/STAGE_2_PROG/6/STAGE_2_PROG/7/STAGE_2_PROG/8/STAGE_2_PROG/9/STAGE_2_PROG/10/STAGE_2_PROG/11/STAGE_2_PROG/12", "1", "0");
		var s4 = g.Stage("STAGE_3_PROG", false);
		s4.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025650$*^", "stage_start", "10");
		s4.Obj(0, 100101, -3179.11f, 239.64f, 4158.51f, 0).Gen(1, 200);
		s4.Obj(1, 100101, -3146.21f, 239.64f, 4138.97f, 0).Gen(1, 200);
		s4.Obj(2, 100101, -3118.13f, 239.64f, 4116.47f, 0).Gen(1, 200);
		s4.Obj(3, 100101, -3071.51f, 239.64f, 4094.23f, 0).Gen(1, 200);
		s4.Obj(4, 100101, -3049.90f, 239.64f, 4099.51f, 0).Gen(1, 200);
		s4.Obj(5, 100101, -3032.20f, 239.64f, 4108.64f, 0).Gen(1, 200);
		s4.Obj(6, 100101, -3058.77f, 239.64f, 4136.09f, 0).Gen(1, 200);
		s4.Obj(7, 100101, -3101.12f, 239.64f, 4149.19f, 0).Gen(1, 200);
		s4.Obj(8, 100101, -3136.74f, 239.64f, 4170.34f, 0).Gen(1, 200);
		s4.Obj(9, 100101, -3184.82f, 239.64f, 4198.53f, 0).Gen(1, 200);
		s4.Obj(10, 100100, -2872.30f, 239.64f, 4208.10f, 0).Gen(1, 200);
		s4.Obj(11, 100100, -2871.95f, 239.64f, 4267.82f, 0).Gen(1, 200);
		s4.Obj(12, 100100, -2893.37f, 239.64f, 4337.64f, 0).Gen(1, 200);
		s4.Obj(13, 100100, -2921.69f, 239.64f, 4397.21f, 0).Gen(1, 200);
		s4.Obj(14, 100100, -2884.05f, 239.64f, 4401.13f, 0).Gen(1, 200);
		s4.Obj(15, 100100, -2856.72f, 239.64f, 4315.65f, 0).Gen(1, 200);
		s4.Obj(16, 100100, -2859.90f, 239.64f, 4275.63f, 0).Gen(1, 200);
		s4.Obj(17, 100100, -2868.06f, 239.64f, 4238.39f, 0).Gen(1, 200);
		s4.Obj(18, 100100, -2821.40f, 239.64f, 4219.69f, 0).Gen(1, 200);
		s4.Obj(19, 100100, -2836.17f, 239.64f, 4191.04f, 0).Gen(1, 200);
		s4.Event("ACT1", 0, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "10")
			.If("MGAME_EVT_COND_MONCNT", "STAGE_3_PROG/0/STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8/STAGE_3_PROG/9/STAGE_3_PROG/10/STAGE_3_PROG/11/STAGE_3_PROG/12/STAGE_3_PROG/13/STAGE_3_PROG/14/STAGE_3_PROG/15/STAGE_3_PROG/16/STAGE_3_PROG/17/STAGE_3_PROG/18/STAGE_3_PROG/19", "10")
			.Do("MGAME_EVT_EXEC_CREMON", "STAGE_3_PROG/0/STAGE_3_PROG/1/STAGE_3_PROG/2/STAGE_3_PROG/3/STAGE_3_PROG/4/STAGE_3_PROG/5/STAGE_3_PROG/6/STAGE_3_PROG/7/STAGE_3_PROG/8/STAGE_3_PROG/9/STAGE_3_PROG/10/STAGE_3_PROG/11/STAGE_3_PROG/12/STAGE_3_PROG/13/STAGE_3_PROG/14/STAGE_3_PROG/15/STAGE_3_PROG/16/STAGE_3_PROG/17/STAGE_3_PROG/18/STAGE_3_PROG/19", "1", "0");
		var s5 = g.Stage("CNT", false);
		s5.Start("MGAME_SET_TIMEOUT", "270");
		s5.Event("Fail_PCcnt", 1, 0)
			.If("MGAME_EVT_COND_PCCNT", "0")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		s5.Event("Fail_Timmer", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "270")
			.If("MGAME_EVT_COND_MONCNT_OVER", "STAGE_1_PROG/0", "1")
			.Do("GAME_ST_EVT_EXEC_STAGE_START", "FAIL")
			.Do("GAME_ST_EVT_EXEC_STAGE_CLEAR", "CNT");
		var s6 = g.Stage("OUT", false);
		s6.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20190419_041809$*^", "raid_clear", "60");
		s6.Start("MGAME_SET_TIMEOUT", "60");
		s6.Obj(0, 156162, -3047.16f, 239.64f, 4270.38f, 0).Neutral();
		s6.Obj(1, 160065, -3111.12f, 239.64f, 4276.87f, 0);
		s6.Event("LEAVE", 1, 0)
			.If("GAME_ST_EVT_COND_TIMECHECK", "60")
			.Do("MGAME_RETURN")
			.Do("MGAME_END", "1");
		var s7 = g.Stage("FINAL", false);
		s7.Start("MGAME_SET_RAID_ICON", "@dicID_^*$ETC_20161005_025653$*^", "move_to_point", "10");
		s7.Obj(0, 40001, -3029.44f, 266.69f, 4649.64f, 90).Named("@dicID_^*$ETC_20160811_023835$*^").Enter("G_TOWER_WARP_TO_41", 50).Neutral();
		s7.Event("zemina", 1, 0)
			.Do("MGAME_EXEC_RUNMGAME", "M_GTOWER2_ZEMINA_ROOM");
		return g;
	}
}
