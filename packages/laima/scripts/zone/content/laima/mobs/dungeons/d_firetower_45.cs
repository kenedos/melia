//--- Melia Script -----------------------------------------------------------
// Mage Tower 5F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_firetower_45'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower45MobScript : GeneralScript
{
	protected override void Load()
	{
		// Property Overrides -------------------------------


		// Monster Spawners ---------------------------------

		AddSpawner("d_firetower_45.Id1", MonsterId.Rootcrystal_01, min: 9, max: 12, respawn: Seconds(30), tendency: TendencyType.Peaceful);
		AddSpawner("d_firetower_45.Id2", MonsterId.Dimmer, min: 12, max: 18, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_45.Id3", MonsterId.Tower_Of_Firepuppet_Black, min: 18, max: 26, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_45.Id4", MonsterId.Fire_Dragon_Purple, min: 16, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_45.Id5", MonsterId.Fire_Dragon_Purple, min: 16, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_45.Id6", MonsterId.Dimmer, min: 12, max: 14, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_45.Id7", MonsterId.Tower_Of_Firepuppet_Black, min: 6, max: 7, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_45.Id8", MonsterId.Fire_Dragon_Purple, min: 12, max: 18, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_45.Id9", MonsterId.Dimmer, min: 12, max: 18, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_45.Id10", MonsterId.Minivern_Elite, amount: 18, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_45.Id11", MonsterId.Minivern, min: 20, max: 30, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_45.Id12", MonsterId.Fire_Dragon_Purple, min: 10, max: 15, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 1 Spawn Points
		AddSpawnPoint("d_firetower_45.Id1", "d_firetower_45", Rectangle(-1254, -1707, 100));
		AddSpawnPoint("d_firetower_45.Id1", "d_firetower_45", Rectangle(-1617, -1272, 100));
		AddSpawnPoint("d_firetower_45.Id1", "d_firetower_45", Rectangle(-854, -1269, 100));
		AddSpawnPoint("d_firetower_45.Id1", "d_firetower_45", Rectangle(-1251, -754, 100));
		AddSpawnPoint("d_firetower_45.Id1", "d_firetower_45", Rectangle(-576, -645, 100));
		AddSpawnPoint("d_firetower_45.Id1", "d_firetower_45", Rectangle(-1569, -194, 100));
		AddSpawnPoint("d_firetower_45.Id1", "d_firetower_45", Rectangle(45, 120, 100));
		AddSpawnPoint("d_firetower_45.Id1", "d_firetower_45", Rectangle(-96, 1038, 100));
		AddSpawnPoint("d_firetower_45.Id1", "d_firetower_45", Rectangle(483, 641, 100));
		AddSpawnPoint("d_firetower_45.Id1", "d_firetower_45", Rectangle(-28, 1524, 100));
		AddSpawnPoint("d_firetower_45.Id1", "d_firetower_45", Rectangle(773, 1890, 100));
		AddSpawnPoint("d_firetower_45.Id1", "d_firetower_45", Rectangle(985, 1263, 100));

		// 'Dimmer' GenType 2 Spawn Points
		AddSpawnPoint("d_firetower_45.Id2", "d_firetower_45", Rectangle(-98, 41, 9999));

		// 'Tower_Of_Firepuppet_Black' GenType 3 Spawn Points
		AddSpawnPoint("d_firetower_45.Id3", "d_firetower_45", Rectangle(-73, 100, 9999));

		// 'Fire_Dragon_Purple' GenType 206 Spawn Points
		AddSpawnPoint("d_firetower_45.Id4", "d_firetower_45", Rectangle(-1240, -701, 9999));

		// 'Fire_Dragon_Purple' GenType 207 Spawn Points
		AddSpawnPoint("d_firetower_45.Id5", "d_firetower_45", Rectangle(-1243, -727, 25));
		AddSpawnPoint("d_firetower_45.Id5", "d_firetower_45", Rectangle(-957, -1269, 25));
		AddSpawnPoint("d_firetower_45.Id5", "d_firetower_45", Rectangle(-825, -56, 25));
		AddSpawnPoint("d_firetower_45.Id5", "d_firetower_45", Rectangle(-1201, -1284, 25));
		AddSpawnPoint("d_firetower_45.Id5", "d_firetower_45", Rectangle(-1276, -1407, 25));
		AddSpawnPoint("d_firetower_45.Id5", "d_firetower_45", Rectangle(-1347, -1263, 25));
		AddSpawnPoint("d_firetower_45.Id5", "d_firetower_45", Rectangle(-1259, -1204, 25));
		AddSpawnPoint("d_firetower_45.Id5", "d_firetower_45", Rectangle(-800, -1230, 25));
		AddSpawnPoint("d_firetower_45.Id5", "d_firetower_45", Rectangle(-661, -1332, 25));
		AddSpawnPoint("d_firetower_45.Id5", "d_firetower_45", Rectangle(-591, -1162, 25));
		AddSpawnPoint("d_firetower_45.Id5", "d_firetower_45", Rectangle(-1219, -900, 25));
		AddSpawnPoint("d_firetower_45.Id5", "d_firetower_45", Rectangle(-849, -664, 25));
		AddSpawnPoint("d_firetower_45.Id5", "d_firetower_45", Rectangle(-1219, -607, 25));
		AddSpawnPoint("d_firetower_45.Id5", "d_firetower_45", Rectangle(-1070, -623, 25));
		AddSpawnPoint("d_firetower_45.Id5", "d_firetower_45", Rectangle(-1224, -455, 25));
		AddSpawnPoint("d_firetower_45.Id5", "d_firetower_45", Rectangle(-1226, -1028, 25));

		// 'Dimmer' GenType 208 Spawn Points
		AddSpawnPoint("d_firetower_45.Id6", "d_firetower_45", Rectangle(-1228, -266, 100));
		AddSpawnPoint("d_firetower_45.Id6", "d_firetower_45", Rectangle(-1232, -1621, 100));
		AddSpawnPoint("d_firetower_45.Id6", "d_firetower_45", Rectangle(-576, -689, 100));

		// 'Tower_Of_Firepuppet_Black' GenType 209 Spawn Points
		AddSpawnPoint("d_firetower_45.Id7", "d_firetower_45", Rectangle(-927, -78, 200));
		AddSpawnPoint("d_firetower_45.Id7", "d_firetower_45", Rectangle(-1540, -187, 200));
		AddSpawnPoint("d_firetower_45.Id7", "d_firetower_45", Rectangle(-907, -688, 200));

		// 'Fire_Dragon_Purple' GenType 216 Spawn Points
		AddSpawnPoint("d_firetower_45.Id8", "d_firetower_45", Rectangle(-144, 977, 25));
		AddSpawnPoint("d_firetower_45.Id8", "d_firetower_45", Rectangle(-112, 1118, 25));
		AddSpawnPoint("d_firetower_45.Id8", "d_firetower_45", Rectangle(-10, 1016, 25));
		AddSpawnPoint("d_firetower_45.Id8", "d_firetower_45", Rectangle(578, 800, 25));
		AddSpawnPoint("d_firetower_45.Id8", "d_firetower_45", Rectangle(592, 966, 25));
		AddSpawnPoint("d_firetower_45.Id8", "d_firetower_45", Rectangle(477, 886, 25));
		AddSpawnPoint("d_firetower_45.Id8", "d_firetower_45", Rectangle(704, 861, 25));
		AddSpawnPoint("d_firetower_45.Id8", "d_firetower_45", Rectangle(50, 1414, 25));
		AddSpawnPoint("d_firetower_45.Id8", "d_firetower_45", Rectangle(24, 1553, 25));
		AddSpawnPoint("d_firetower_45.Id8", "d_firetower_45", Rectangle(-155, 1531, 25));
		AddSpawnPoint("d_firetower_45.Id8", "d_firetower_45", Rectangle(991, 1233, 25));
		AddSpawnPoint("d_firetower_45.Id8", "d_firetower_45", Rectangle(855, 1318, 25));
		AddSpawnPoint("d_firetower_45.Id8", "d_firetower_45", Rectangle(634, 1271, 25));
		AddSpawnPoint("d_firetower_45.Id8", "d_firetower_45", Rectangle(408, 1459, 25));
		AddSpawnPoint("d_firetower_45.Id8", "d_firetower_45", Rectangle(702, 1455, 25));

		// 'Dimmer' GenType 217 Spawn Points
		AddSpawnPoint("d_firetower_45.Id9", "d_firetower_45", Rectangle(996, 1290, 25));
		AddSpawnPoint("d_firetower_45.Id9", "d_firetower_45", Rectangle(59, 1518, 25));
		AddSpawnPoint("d_firetower_45.Id9", "d_firetower_45", Rectangle(-99, 1037, 25));
		AddSpawnPoint("d_firetower_45.Id9", "d_firetower_45", Rectangle(-219, 1043, 25));
		AddSpawnPoint("d_firetower_45.Id9", "d_firetower_45", Rectangle(960, 815, 25));
		AddSpawnPoint("d_firetower_45.Id9", "d_firetower_45", Rectangle(561, 863, 25));
		AddSpawnPoint("d_firetower_45.Id9", "d_firetower_45", Rectangle(726, 1603, 25));
		AddSpawnPoint("d_firetower_45.Id9", "d_firetower_45", Rectangle(779, 1866, 25));
		AddSpawnPoint("d_firetower_45.Id9", "d_firetower_45", Rectangle(660, 1364, 25));
		AddSpawnPoint("d_firetower_45.Id9", "d_firetower_45", Rectangle(353, 1408, 25));
		AddSpawnPoint("d_firetower_45.Id9", "d_firetower_45", Rectangle(646, 1169, 25));

		// 'Minivern' Spawn Points
		AddSpawnPoint("d_firetower_45.Id10", "d_firetower_45", Rectangle(-15, 37, 9999));

		// 'Minivern_Elite' Spawn Points
		AddSpawnPoint("d_firetower_45.Id11", "d_firetower_45", Rectangle(835, 2316, 9999));

		// 'Fire_Dragon_Purple' Spawn Points
		AddSpawnPoint("d_firetower_45.Id12", "d_firetower_45", Rectangle(-1250, -191, 9999));

		// Boss Spawners ---------------------------------
		// AddBossSpawner(MonsterId.Boss_Helgasercle, "d_firetower_45", 1, Hours(2), Hours(4));
	}
}
