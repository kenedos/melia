//--- Melia Script -----------------------------------------------------------
// Zachariel Crossroads Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_rokas_31'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas31MobScript : GeneralScript
{
	protected override void Load()
	{
		// Property Overrides -------------------------------


		// Monster Spawners ---------------------------------

		AddSpawner("f_rokas_31.Id1", MonsterId.Warleader_Hogma, min: 4, max: 5, respawn: Seconds(25));
		AddSpawner("f_rokas_31.Id2", MonsterId.Rootcrystal_03, min: 3, max: 4, respawn: Seconds(5));
		AddSpawner("f_rokas_31.Id3", MonsterId.Tontulia, min: 12, max: 15);
		AddSpawner("f_rokas_31.Id4", MonsterId.Warleader_Hogma, amount: 3, respawn: Seconds(25));
		AddSpawner("f_rokas_31.Id5", MonsterId.Repusbunny_Mage, min: 4, max: 5);

		// Monster Spawn Points -----------------------------

		// 'Warleader_Hogma' GenType 11 Spawn Points
		AddSpawnPoint("f_rokas_31.Id1", "f_rokas_31", Rectangle(-661, 6, 9999));

		// 'Rootcrystal_03' GenType 600 Spawn Points
		AddSpawnPoint("f_rokas_31.Id2", "f_rokas_31", Rectangle(-30, -489, 30));
		AddSpawnPoint("f_rokas_31.Id2", "f_rokas_31", Rectangle(-621, -984, 30));
		AddSpawnPoint("f_rokas_31.Id2", "f_rokas_31", Rectangle(869, -1061, 30));
		AddSpawnPoint("f_rokas_31.Id2", "f_rokas_31", Rectangle(513, -1375, 30));
		AddSpawnPoint("f_rokas_31.Id2", "f_rokas_31", Rectangle(507, -37, 30));
		AddSpawnPoint("f_rokas_31.Id2", "f_rokas_31", Rectangle(-565, -343, 30));
		AddSpawnPoint("f_rokas_31.Id2", "f_rokas_31", Rectangle(-695, 150, 30));
		AddSpawnPoint("f_rokas_31.Id2", "f_rokas_31", Rectangle(-917, 387, 30));

		// 'Tontulia' GenType 615 Spawn Points
		AddSpawnPoint("f_rokas_31.Id3", "f_rokas_31", Rectangle(373, -942, 40));
		AddSpawnPoint("f_rokas_31.Id3", "f_rokas_31", Rectangle(-845, 271, 40));
		AddSpawnPoint("f_rokas_31.Id3", "f_rokas_31", Rectangle(-697, -983, 40));
		AddSpawnPoint("f_rokas_31.Id3", "f_rokas_31", Rectangle(-537, -1100, 40));
		AddSpawnPoint("f_rokas_31.Id3", "f_rokas_31", Rectangle(-417, -882, 40));
		AddSpawnPoint("f_rokas_31.Id3", "f_rokas_31", Rectangle(-579, -248, 40));
		AddSpawnPoint("f_rokas_31.Id3", "f_rokas_31", Rectangle(-417, -12, 40));
		AddSpawnPoint("f_rokas_31.Id3", "f_rokas_31", Rectangle(-700, 84, 40));
		AddSpawnPoint("f_rokas_31.Id3", "f_rokas_31", Rectangle(-271, -1040, 40));

		// 'Warleader_Hogma' GenType 620 Spawn Points
		AddSpawnPoint("f_rokas_31.Id4", "f_rokas_31", Rectangle(-73, -581, 400));

		// 'Repusbunny_Mage' GenType 622 Spawn Points
		AddSpawnPoint("f_rokas_31.Id5", "f_rokas_31", Rectangle(-582, -1092, 40));
		AddSpawnPoint("f_rokas_31.Id5", "f_rokas_31", Rectangle(-548, -939, 40));
		AddSpawnPoint("f_rokas_31.Id5", "f_rokas_31", Rectangle(-22, -545, 40));
		AddSpawnPoint("f_rokas_31.Id5", "f_rokas_31", Rectangle(-441, -103, 40));
		AddSpawnPoint("f_rokas_31.Id5", "f_rokas_31", Rectangle(-750, 174, 40));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Cactusvel, "f_rokas_31", 1, Hours(2), Hours(4));
	}
}
