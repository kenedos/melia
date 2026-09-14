//--- Melia Script -----------------------------------------------------------
// Ruklys Street Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_flash_61'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash61MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_flash_61.Id1", MonsterId.Denden, min: 12, max: 15);
		AddSpawner("f_flash_61.Id2", MonsterId.Moyabu, min: 12, max: 15);
		AddSpawner("f_flash_61.Id3", MonsterId.Moyabu, min: 15, max: 20);
		AddSpawner("f_flash_61.Id4", MonsterId.Goblin2_Sword, min: 12, max: 15);
		AddSpawner("f_flash_61.Id5", MonsterId.Rootcrystal_01, min: 12, max: 15, respawn: Seconds(20));
		AddSpawner("f_flash_61.Id6", MonsterId.Denden, min: 12, max: 15);

		// Monster Spawn Points -----------------------------

		// 'Denden' GenType 4 Spawn Points
		AddSpawnPoint("f_flash_61.Id1", "f_flash_61", Rectangle(680, 506, 9999));

		// 'Moyabu' GenType 14 Spawn Points
		AddSpawnPoint("f_flash_61.Id2", "f_flash_61", Rectangle(707, 562, 9999));

		// 'Moyabu' GenType 15 Spawn Points
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(-26, 144, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(-873, 521, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(80, 33, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(-708, 686, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(-59, 306, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(-167, 231, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(-277, 59, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(-98, 552, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(758, 286, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(986, 503, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(997, 65, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(910, 346, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(1163, 373, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(1011, 263, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(776, 90, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(728, 520, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(-776, 973, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(-861, 611, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(-626, 530, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(-669, 864, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(-141, 23, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(-180, 373, 30));
		AddSpawnPoint("f_flash_61.Id3", "f_flash_61", Rectangle(232, 92, 30));

		// 'Goblin2_Sword' GenType 28 Spawn Points
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(865, 72, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(740, 498, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(922, 1115, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(-680, 686, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(931, 354, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(1110, 419, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(-749, 944, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(737, 292, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(-866, 527, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(800, 975, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(782, 1295, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(1289, 1275, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(1111, 1080, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(1345, 1078, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(1286, 946, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(1012, 943, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(1145, 1196, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(732, 1126, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(-190, 132, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(65, 94, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(-102, 475, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(-212, 954, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(200, 992, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(-79, 682, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(-456, 1187, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(-550, 1419, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(-630, 1215, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(-552, 1031, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(-38, 1055, 25));
		AddSpawnPoint("f_flash_61.Id4", "f_flash_61", Rectangle(340, 1003, 25));

		// 'Rootcrystal_01' GenType 32 Spawn Points
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(-879, -198, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(-706, -37, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(-227, 51, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(39, 41, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(-86, 343, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(-83, 610, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(-910, 591, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(-696, 559, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(-670, 799, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(-608, 996, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(-452, 1310, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(-85, 1131, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(-34, 991, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(363, 974, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(817, 1040, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(1209, 1124, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(850, 463, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(914, 74, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(1133, 109, 100));
		AddSpawnPoint("f_flash_61.Id5", "f_flash_61", Rectangle(680, 448, 100));

		// 'Denden' GenType 33 Spawn Points
		AddSpawnPoint("f_flash_61.Id6", "f_flash_61", Rectangle(-571, 1250, 25));
		AddSpawnPoint("f_flash_61.Id6", "f_flash_61", Rectangle(-626, 1466, 25));
		AddSpawnPoint("f_flash_61.Id6", "f_flash_61", Rectangle(-575, 993, 25));
		AddSpawnPoint("f_flash_61.Id6", "f_flash_61", Rectangle(-409, 1263, 25));
		AddSpawnPoint("f_flash_61.Id6", "f_flash_61", Rectangle(-480, 1387, 25));
		AddSpawnPoint("f_flash_61.Id6", "f_flash_61", Rectangle(247, 966, 25));
		AddSpawnPoint("f_flash_61.Id6", "f_flash_61", Rectangle(-235, 1301, 25));
		AddSpawnPoint("f_flash_61.Id6", "f_flash_61", Rectangle(-262, 906, 25));
		AddSpawnPoint("f_flash_61.Id6", "f_flash_61", Rectangle(43, 828, 25));
		AddSpawnPoint("f_flash_61.Id6", "f_flash_61", Rectangle(-74, 1135, 25));
		AddSpawnPoint("f_flash_61.Id6", "f_flash_61", Rectangle(-148, 720, 25));
		AddSpawnPoint("f_flash_61.Id6", "f_flash_61", Rectangle(-703, 571, 25));
		AddSpawnPoint("f_flash_61.Id6", "f_flash_61", Rectangle(-782, 957, 25));
		AddSpawnPoint("f_flash_61.Id6", "f_flash_61", Rectangle(-428, 1077, 25));
		AddSpawnPoint("f_flash_61.Id6", "f_flash_61", Rectangle(-663, 773, 25));
	}
}
