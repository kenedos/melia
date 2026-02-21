//--- Melia Script ----------------------------------------------------------
// Treasure Spawns
//--- Description -----------------------------------------------------------
// Spawns Treasure Chests randomly in a list of maps
//---------------------------------------------------------------------------

using System;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Spawning;

public class RandomTreasureChestScript : GeneralScript
{
	protected override void Load()
	{
		var spawnPointDb = ZoneServer.Instance.Data.TreasureSpawnPointDb;
		var dropDb = ZoneServer.Instance.Data.TreasureDropDb;

		var spawnPoints = spawnPointDb.Entries.ToArray();
		var maxPopulation = (int)Math.Ceiling(spawnPoints.Length * 0.05);

		var spawner = new TreasureChestSpawner(
			$"treasure_spawner",
			dropDb,
			maxPopulation,
			respawnDelay: TimeSpan.FromSeconds(60),
			spawnPoints
		);

		ZoneServer.Instance.World.AddSpawner(spawner);
	}
}
