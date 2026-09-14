//--- Melia Script -----------------------------------------------------------
// Drill Ground of Confliction Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_underfortress_66'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress66MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_underfortress_66.Id1", MonsterId.Chafperor_Purple, min: 34, max: 45, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_66.Id2", MonsterId.Chafperor_Mage_Purple, min: 12, max: 16, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_66.Id3", MonsterId.Ticen_Mage_Blue, min: 12, max: 15, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_66.Id4", MonsterId.Rootcrystal_05, min: 12, max: 15, respawn: Seconds(20), tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'Chafperor_Purple' GenType 9 Spawn Points
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-39, 281, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-668, 544, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-1281, 543, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-1206, -1161, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-19, -1208, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-323, -113, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(169, 42, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-1009, -546, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-534, -675, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-601, -1110, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(13, -454, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-68, -7, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-212, 547, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(63, 852, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-289, 201, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-1137, 747, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-1096, -781, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-1151, -618, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-451, -427, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(27, -971, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-872, -1169, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-422, 11, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-154, -478, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-795, 519, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(106, 160, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-753, -451, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(262, -471, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(96, -1246, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-973, 594, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-487, 188, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-121, 229, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-156, 385, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(7, 618, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(405, 165, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(642, 123, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(600, -115, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(673, -317, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(384, -128, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(102, -242, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-93, -214, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-405, -159, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-563, -874, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(52, -786, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-995, -762, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-852, -558, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-1033, -393, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-1102, -479, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-844, -780, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-731, -1158, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-532, -978, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(75, -1148, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(497, -392, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(-227, 8, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(61, -112, 20));
		AddSpawnPoint("d_underfortress_66.Id1", "d_underfortress_66", Rectangle(289, -61, 20));

		// 'Chafperor_Mage_Purple' GenType 38 Spawn Points
		AddSpawnPoint("d_underfortress_66.Id2", "d_underfortress_66", Rectangle(6, 814, 30));
		AddSpawnPoint("d_underfortress_66.Id2", "d_underfortress_66", Rectangle(73, -1241, 30));
		AddSpawnPoint("d_underfortress_66.Id2", "d_underfortress_66", Rectangle(116, -1094, 30));
		AddSpawnPoint("d_underfortress_66.Id2", "d_underfortress_66", Rectangle(-576, -1121, 30));
		AddSpawnPoint("d_underfortress_66.Id2", "d_underfortress_66", Rectangle(-898, -1178, 30));
		AddSpawnPoint("d_underfortress_66.Id2", "d_underfortress_66", Rectangle(-1312, -1192, 30));
		AddSpawnPoint("d_underfortress_66.Id2", "d_underfortress_66", Rectangle(-1081, -668, 30));
		AddSpawnPoint("d_underfortress_66.Id2", "d_underfortress_66", Rectangle(-570, -473, 30));
		AddSpawnPoint("d_underfortress_66.Id2", "d_underfortress_66", Rectangle(-213, 566, 30));
		AddSpawnPoint("d_underfortress_66.Id2", "d_underfortress_66", Rectangle(118, -106, 30));
		AddSpawnPoint("d_underfortress_66.Id2", "d_underfortress_66", Rectangle(-603, 617, 30));
		AddSpawnPoint("d_underfortress_66.Id2", "d_underfortress_66", Rectangle(-1171, 548, 30));
		AddSpawnPoint("d_underfortress_66.Id2", "d_underfortress_66", Rectangle(-162, 196, 30));
		AddSpawnPoint("d_underfortress_66.Id2", "d_underfortress_66", Rectangle(-204, 25, 30));
		AddSpawnPoint("d_underfortress_66.Id2", "d_underfortress_66", Rectangle(-29, -465, 30));
		AddSpawnPoint("d_underfortress_66.Id2", "d_underfortress_66", Rectangle(-943, -519, 30));

		// 'Ticen_Mage_Blue' GenType 39 Spawn Points
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(-128, -245, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(-215, 256, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(13, 736, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(-682, 555, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(-1159, 539, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(-1211, 829, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(-1099, -671, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(-941, -537, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(-963, -1180, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(-577, -1147, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(-555, -726, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(-365, -117, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(42, -1082, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(-7, -1265, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(36, -85, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(18, -724, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(-525, -501, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(258, -415, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(-227, 542, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(200, 203, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(673, 74, 30));
		AddSpawnPoint("d_underfortress_66.Id3", "d_underfortress_66", Rectangle(645, -233, 30));

		// 'Rootcrystal_05' GenType 57 Spawn Points
		AddSpawnPoint("d_underfortress_66.Id4", "d_underfortress_66", Rectangle(78, -1028, 40));
		AddSpawnPoint("d_underfortress_66.Id4", "d_underfortress_66", Rectangle(67, -520, 40));
		AddSpawnPoint("d_underfortress_66.Id4", "d_underfortress_66", Rectangle(-533, -545, 40));
		AddSpawnPoint("d_underfortress_66.Id4", "d_underfortress_66", Rectangle(-788, -1128, 40));
		AddSpawnPoint("d_underfortress_66.Id4", "d_underfortress_66", Rectangle(-1242, -1216, 40));
		AddSpawnPoint("d_underfortress_66.Id4", "d_underfortress_66", Rectangle(-979, -727, 40));
		AddSpawnPoint("d_underfortress_66.Id4", "d_underfortress_66", Rectangle(635, -121, 40));
		AddSpawnPoint("d_underfortress_66.Id4", "d_underfortress_66", Rectangle(919, 268, 40));
		AddSpawnPoint("d_underfortress_66.Id4", "d_underfortress_66", Rectangle(1993, 207, 40));
		AddSpawnPoint("d_underfortress_66.Id4", "d_underfortress_66", Rectangle(-72, 204, 40));
		AddSpawnPoint("d_underfortress_66.Id4", "d_underfortress_66", Rectangle(82, 760, 40));
		AddSpawnPoint("d_underfortress_66.Id4", "d_underfortress_66", Rectangle(-735, 605, 40));
		AddSpawnPoint("d_underfortress_66.Id4", "d_underfortress_66", Rectangle(-1079, 625, 40));
		AddSpawnPoint("d_underfortress_66.Id4", "d_underfortress_66", Rectangle(-210, -103, 40));
		AddSpawnPoint("d_underfortress_66.Id4", "d_underfortress_66", Rectangle(147, -182, 40));
	}
}
