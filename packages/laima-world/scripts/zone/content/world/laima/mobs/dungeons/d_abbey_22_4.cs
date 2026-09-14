//--- Melia Script -----------------------------------------------------------
// Narvas Temple Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_abbey_22_4'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey224MobScript : GeneralScript
{
	protected override void Load()
	{
		// Property Overrides -------------------------------

		// Spawn Buffs -------------------------------------
		AddSpawnBuff("d_abbey_22_4", MonsterId.Hohen_Mage_Black, BuffId.EliteMonsterBuff, chance: 100);

		// Monster Spawners ---------------------------------

		AddSpawner("d_abbey_22_4.Id1", MonsterId.Rootcrystal_05, min: 14, max: 18, respawn: Minutes(1));
		AddSpawner("d_abbey_22_4.Id3", MonsterId.Nook, min: 18, max: 24, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_4.Id4", MonsterId.Boor, min: 20, max: 26, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_4.Id5", MonsterId.Mangosting, min: 40, max: 60, respawn: Minutes(1), tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_4.Id6", MonsterId.Half_Mangosting, min: 60, max: 86, respawn: Minutes(1), tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_4.Id9", MonsterId.Rootcrystal_01, min: 13, max: 17, respawn: Minutes(1), tendency: TendencyType.Aggressive);

		// Hohen_Mage_Black Room Spawners
		AddSpawner("d_abbey_22_4.Id10", MonsterId.Hohen_Mage_Black, min: 1, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_4.Id11", MonsterId.Hohen_Mage_Black, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_4.Id12", MonsterId.Hohen_Mage_Black, min: 1, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_4.Id13", MonsterId.Hohen_Mage_Black, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_4.Id14", MonsterId.Hohen_Mage_Black, min: 1, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_4.Id15", MonsterId.Hohen_Mage_Black, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_4.Id16", MonsterId.Hohen_Mage_Black, min: 1, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_4.Id17", MonsterId.Hohen_Mage_Black, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_4.Id18", MonsterId.Hohen_Mage_Black, min: 1, max: 3, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_05' GenType 43 Spawn Points
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(535, 1267, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(1518, 1284, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(-54, 1281, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(-669, 1252, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(-1475, 1271, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(-27, 666, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(526, 647, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(-658, 667, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(-1484, 610, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(-244, 137, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(541, 76, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(819, 45, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(-669, 48, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(-1534, 55, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(-1591, -606, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(-665, -500, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(-700, -1085, 20));
		AddSpawnPoint("d_abbey_22_4.Id1", "d_abbey_22_4", Rectangle(-176, -1085, 20));

		// 'Hohen_Mage_Black' Room Spawn Points
		AddSpawnPoint("d_abbey_22_4.Id10", "d_abbey_22_4", Rectangle(-1438, 460, 400));
		AddSpawnPoint("d_abbey_22_4.Id11", "d_abbey_22_4", Rectangle(523, 1299, 400));
		AddSpawnPoint("d_abbey_22_4.Id12", "d_abbey_22_4", Rectangle(-695, 1305, 400));
		AddSpawnPoint("d_abbey_22_4.Id13", "d_abbey_22_4", Rectangle(-1567, 1260, 400));
		AddSpawnPoint("d_abbey_22_4.Id14", "d_abbey_22_4", Rectangle(555, 669, 400));
		AddSpawnPoint("d_abbey_22_4.Id15", "d_abbey_22_4", Rectangle(-636, 635, 400));
		AddSpawnPoint("d_abbey_22_4.Id16", "d_abbey_22_4", Rectangle(-661, 53, 400));
		AddSpawnPoint("d_abbey_22_4.Id17", "d_abbey_22_4", Rectangle(-1487, -13, 400));
		AddSpawnPoint("d_abbey_22_4.Id18", "d_abbey_22_4", Rectangle(668, 71, 400));
		// 'Nook' GenType 51 Spawn Points
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(1642, 1093, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(1562, 1392, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(1394, 1117, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(1361, 1399, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(1210, 1229, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(945, 1299, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(675, 1136, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(656, 1423, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(585, 1210, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(405, 1108, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(463, 1430, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(493, 1205, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(200, 1267, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(381, 1446, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(-338, 1302, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(-561, 1259, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(-643, 1326, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(-755, 1250, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(-859, 1320, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(-1677, 1140, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(-1453, 1098, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(-1413, 1345, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(-1664, 1445, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(-1714, 1310, 40));
		AddSpawnPoint("d_abbey_22_4.Id3", "d_abbey_22_4", Rectangle(263, 646, 40));

		// 'Boor' GenType 52 Spawn Points
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(577, 522, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(730, 528, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(739, 747, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(569, 777, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(39, 717, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-74, 597, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(140, 645, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-247, 634, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-854, 483, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-818, 554, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-623, 497, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-520, 466, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-491, 641, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-494, 785, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-586, 764, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-820, 671, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-601, 623, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-983, 669, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-1137, 664, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-1640, 475, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-1663, 633, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-1499, 516, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-1430, 687, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-1312, 490, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-1691, 754, 40));
		AddSpawnPoint("d_abbey_22_4.Id4", "d_abbey_22_4", Rectangle(-1716, 519, 40));

		// 'Mangosting' GenType 53 Spawn Points
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(385, -98, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(432, 26, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(548, -127, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(601, -9, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(757, -120, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(798, -27, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(933, -108, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(936, 16, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(805, 105, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(886, 214, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(781, 207, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(625, 231, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(565, 107, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(327, 128, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(388, 187, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(715, 30, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-108, 143, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(11, 152, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-860, -55, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-672, 69, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-665, 205, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-544, 44, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-677, -79, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-880, 175, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-820, 48, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-1133, 156, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-1006, 149, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-1334, 156, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-1491, 126, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-1393, 46, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-1362, -109, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-1513, -158, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-1627, -179, 40));
		AddSpawnPoint("d_abbey_22_4.Id5", "d_abbey_22_4", Rectangle(-1691, 5, 40));

		// 'Half_Mangosting' GenType 54 Spawn Points
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-1627, -85, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-1617, 71, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-1500, 27, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-1505, -53, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-1435, -44, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-1665, -304, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-1607, -386, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-1708, -486, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-1596, -498, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-1639, -615, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-1664, -676, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-1557, -715, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-1511, -629, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-1473, -696, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-1451, -547, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-1543, -554, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-717, -188, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-667, -294, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-663, -406, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-756, -445, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-711, -531, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-605, -479, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-814, -588, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-661, -620, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-690, -766, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-822, -917, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-810, -1019, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-813, -1116, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-682, -1202, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-651, -975, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-659, -1080, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-524, -1190, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-544, -1045, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-447, -1109, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-535, -947, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-409, -975, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-638, -1181, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-282, -1054, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-258, -934, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-205, -1041, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-57, -1147, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-38, -1228, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-108, -1082, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(14, -1122, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-19, -922, 40));
		AddSpawnPoint("d_abbey_22_4.Id6", "d_abbey_22_4", Rectangle(-138, -958, 40));

		// 'Rootcrystal_01' GenType 59 Spawn Points
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(1609, 1235, 40));
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(453, 1322, 40));
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(-62, 1302, 40));
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(-679, 1289, 40));
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(-1471, 1264, 40));
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(-1488, 611, 40));
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(-562, 634, 40));
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(603, 540, 40));
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(708, -7, 40));
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(-83, 140, 40));
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(-714, 74, 40));
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(-1571, -22, 40));
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(-1550, -591, 40));
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(-667, -489, 40));
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(-585, -1060, 40));
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(-65, -1126, 40));
		AddSpawnPoint("d_abbey_22_4.Id9", "d_abbey_22_4", Rectangle(1245, 1286, 40));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Unicorn, "d_abbey_22_4", 1, Hours(2), Hours(4));
	}
}
