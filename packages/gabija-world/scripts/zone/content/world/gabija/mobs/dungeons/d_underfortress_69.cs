//--- Melia Script -----------------------------------------------------------
// Fortress Battlegrounds Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_underfortress_69'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress69MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_underfortress_69.Id1", MonsterId.Kepari_Green, min: 27, max: 35, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_69.Id2", MonsterId.Templeslave_Blue, min: 12, max: 16, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_69.Id3", MonsterId.Flask_Blue, min: 30, max: 40, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_69.Id4", MonsterId.Kepari_Mage_Green, min: 12, max: 15, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_69.Id5", MonsterId.Rootcrystal_05, min: 20, max: 26, respawn: Seconds(20), tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'Kepari_Green' GenType 28 Spawn Points
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-1218, -1737, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-1288, -2129, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-174, -2232, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-143, -1563, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(370, -2002, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(680, -2358, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(760, -1847, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(1101, -2024, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(1582, -2217, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(1981, -2184, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(1671, -1692, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(1750, -1189, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(1761, -2226, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(1762, 1019, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(1702, 1409, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(1537, 1194, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(2070, 1320, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(1209, 1222, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(939, 1353, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(125, 1404, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(393, 106, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-834, -667, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-591, -60, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-376, 614, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-892, 1632, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-772, 1284, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-1508, 89, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(7, -756, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-991, -2221, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-155, -1928, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-667, 882, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-695, 221, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-153, 1396, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(606, 1235, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(619, 786, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(524, 488, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(1957, 753, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(1843, 722, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(1969, -1621, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-970, -2085, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-1526, -1983, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-1299, -2270, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-874, -2153, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-991, -1914, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-556, -2120, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-242, -2345, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-331, -1931, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(123, -1980, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(59, -1888, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-141, -1382, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-117, -979, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-117, -1166, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(222, -407, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(441, -230, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-230, 236, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(-720, -196, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(96, -245, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(615, 1394, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(710, 1573, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(722, 1408, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(449, 1134, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(363, 1217, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(648, 1018, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(1122, 1372, 30));
		AddSpawnPoint("d_underfortress_69.Id1", "d_underfortress_69", Rectangle(1957, 1213, 30));

		// 'Templeslave_Blue' GenType 29 Spawn Points
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(627, 873, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(-133, 1435, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(-1479, -1624, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(1928, 870, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(1616, -2048, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(1722, -1348, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(1806, -2241, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(874, -1655, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(573, -2285, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(-167, -1469, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(47, -1973, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(-1023, -1904, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(-1096, -1466, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(-827, -499, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(-986, 317, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(-798, 1424, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(361, -177, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(1783, -1711, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(778, -1957, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(1590, -2246, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(-353, -2033, 40));
		AddSpawnPoint("d_underfortress_69.Id2", "d_underfortress_69", Rectangle(-802, -2138, 40));

		// 'Flask_Blue' GenType 30 Spawn Points
		AddSpawnPoint("d_underfortress_69.Id3", "d_underfortress_69", Rectangle(517, 1295, 9999));

		// 'Kepari_Mage_Green' GenType 32 Spawn Points
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-682, 1423, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-715, -238, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-886, -946, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-1250, -1632, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-1095, -2251, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-772, -2249, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-977, -1998, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-1304, 114, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-1179, 165, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-664, 478, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-550, 674, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-927, 1798, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-712, 1049, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-298, 395, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-126, -1315, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(323, -266, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-196, -1922, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(57, -636, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-1154, -1478, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-954, -1339, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-934, -1308, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-910, -819, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-1472, -1704, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-1571, -1970, 30));
		AddSpawnPoint("d_underfortress_69.Id4", "d_underfortress_69", Rectangle(-1366, -2065, 30));

		// 'Rootcrystal_05' GenType 43 Spawn Points
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(287, -141, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(-732, -297, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(-373, 568, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(-1031, 289, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(-703, 1131, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(-976, 1785, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(-995, 2248, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(-77, -1042, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(612, 654, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(601, 1153, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(67, 1418, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(946, 1386, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(1633, 1337, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(1719, 1609, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(2021, 855, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(1894, 20, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(1700, -766, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(1755, -1298, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(1409, -1861, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(1731, -2306, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(811, -1695, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(604, -2367, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(-49, -1910, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(-656, -2147, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(-1304, -2316, 40));
		AddSpawnPoint("d_underfortress_69.Id5", "d_underfortress_69", Rectangle(-1206, -1678, 40));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Mandara, "d_underfortress_69", 1, Hours(2), Hours(4));
	}
}
