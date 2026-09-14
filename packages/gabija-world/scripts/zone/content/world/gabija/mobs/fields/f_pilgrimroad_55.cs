//--- Melia Script -----------------------------------------------------------
// Penitence Route of Great Cathedral Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_pilgrimroad_55'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad55MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_pilgrimroad_55.Id1", MonsterId.Infro_Blud_Red, min: 8, max: 10);
		AddSpawner("f_pilgrimroad_55.Id2", MonsterId.InfroHoglan_Red, min: 8, max: 10);
		AddSpawner("f_pilgrimroad_55.Id3", MonsterId.Rootcrystal_03, min: 9, max: 12, respawn: Minutes(1));
		AddSpawner("f_pilgrimroad_55.Id4", MonsterId.Infro_Blud_Red, min: 6, max: 7);
		AddSpawner("f_pilgrimroad_55.Id5", MonsterId.Burialer, min: 6, max: 7);

		// Monster Spawn Points -----------------------------

		// 'Infro_Blud_Red' GenType 4 Spawn Points
		AddSpawnPoint("f_pilgrimroad_55.Id1", "f_pilgrimroad_55", Rectangle(-816, 153, 9999));

		// 'InfroHoglan_Red' GenType 8 Spawn Points
		AddSpawnPoint("f_pilgrimroad_55.Id2", "f_pilgrimroad_55", Rectangle(1331, -129, 30));
		AddSpawnPoint("f_pilgrimroad_55.Id2", "f_pilgrimroad_55", Rectangle(305, -497, 30));
		AddSpawnPoint("f_pilgrimroad_55.Id2", "f_pilgrimroad_55", Rectangle(1040, -743, 30));
		AddSpawnPoint("f_pilgrimroad_55.Id2", "f_pilgrimroad_55", Rectangle(793, -646, 30));
		AddSpawnPoint("f_pilgrimroad_55.Id2", "f_pilgrimroad_55", Rectangle(1198, -689, 30));
		AddSpawnPoint("f_pilgrimroad_55.Id2", "f_pilgrimroad_55", Rectangle(638, -535, 30));
		AddSpawnPoint("f_pilgrimroad_55.Id2", "f_pilgrimroad_55", Rectangle(1376, -289, 30));
		AddSpawnPoint("f_pilgrimroad_55.Id2", "f_pilgrimroad_55", Rectangle(1495, -187, 30));
		AddSpawnPoint("f_pilgrimroad_55.Id2", "f_pilgrimroad_55", Rectangle(477, -534, 30));
		AddSpawnPoint("f_pilgrimroad_55.Id2", "f_pilgrimroad_55", Rectangle(1466, -432, 30));

		// 'Rootcrystal_03' GenType 28 Spawn Points
		AddSpawnPoint("f_pilgrimroad_55.Id3", "f_pilgrimroad_55", Rectangle(1523, -180, 10));
		AddSpawnPoint("f_pilgrimroad_55.Id3", "f_pilgrimroad_55", Rectangle(1357, -514, 10));
		AddSpawnPoint("f_pilgrimroad_55.Id3", "f_pilgrimroad_55", Rectangle(957, -564, 10));
		AddSpawnPoint("f_pilgrimroad_55.Id3", "f_pilgrimroad_55", Rectangle(1094, -696, 10));
		AddSpawnPoint("f_pilgrimroad_55.Id3", "f_pilgrimroad_55", Rectangle(594, -609, 10));
		AddSpawnPoint("f_pilgrimroad_55.Id3", "f_pilgrimroad_55", Rectangle(237, -357, 10));
		AddSpawnPoint("f_pilgrimroad_55.Id3", "f_pilgrimroad_55", Rectangle(-233, -404, 10));
		AddSpawnPoint("f_pilgrimroad_55.Id3", "f_pilgrimroad_55", Rectangle(7, -158, 10));
		AddSpawnPoint("f_pilgrimroad_55.Id3", "f_pilgrimroad_55", Rectangle(-414, 294, 10));
		AddSpawnPoint("f_pilgrimroad_55.Id3", "f_pilgrimroad_55", Rectangle(-727, 110, 10));
		AddSpawnPoint("f_pilgrimroad_55.Id3", "f_pilgrimroad_55", Rectangle(-649, 363, 10));
		AddSpawnPoint("f_pilgrimroad_55.Id3", "f_pilgrimroad_55", Rectangle(-499, 613, 10));

		// 'Infro_Blud_Red' GenType 30 Spawn Points
		AddSpawnPoint("f_pilgrimroad_55.Id4", "f_pilgrimroad_55", Rectangle(-54, -503, 35));
		AddSpawnPoint("f_pilgrimroad_55.Id4", "f_pilgrimroad_55", Rectangle(-270, -355, 35));
		AddSpawnPoint("f_pilgrimroad_55.Id4", "f_pilgrimroad_55", Rectangle(-13, -307, 35));
		AddSpawnPoint("f_pilgrimroad_55.Id4", "f_pilgrimroad_55", Rectangle(254, -611, 35));
		AddSpawnPoint("f_pilgrimroad_55.Id4", "f_pilgrimroad_55", Rectangle(-258, -679, 35));
		AddSpawnPoint("f_pilgrimroad_55.Id4", "f_pilgrimroad_55", Rectangle(194, -389, 35));

		// 'Burialer' GenType 43 Spawn Points
		AddSpawnPoint("f_pilgrimroad_55.Id5", "f_pilgrimroad_55", Rectangle(-778, 284, 25));
		AddSpawnPoint("f_pilgrimroad_55.Id5", "f_pilgrimroad_55", Rectangle(-985, 246, 25));
		AddSpawnPoint("f_pilgrimroad_55.Id5", "f_pilgrimroad_55", Rectangle(-232, -166, 25));
		AddSpawnPoint("f_pilgrimroad_55.Id5", "f_pilgrimroad_55", Rectangle(-709, 71, 25));
		AddSpawnPoint("f_pilgrimroad_55.Id5", "f_pilgrimroad_55", Rectangle(-654, 434, 25));
		AddSpawnPoint("f_pilgrimroad_55.Id5", "f_pilgrimroad_55", Rectangle(-302, -474, 25));
		AddSpawnPoint("f_pilgrimroad_55.Id5", "f_pilgrimroad_55", Rectangle(73, -260, 25));
	}
}
