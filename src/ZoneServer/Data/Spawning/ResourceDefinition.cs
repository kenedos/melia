using System.Collections.Generic;
using Newtonsoft.Json;

namespace Melia.Zone.Data.Spawning
{

	public class ResourceNodeDefinition
	{
		[JsonProperty("nodeTypeName")]
		public string NodeTypeName { get; set; } // e.g., "Oak Sapling"

		[JsonProperty("modelId")]
		public int ModelId { get; set; } // NPC model to use

		[JsonProperty("yieldItemId")]
		public int YieldItemId { get; set; }

		[JsonProperty("yieldAmountMin")]
		public int YieldAmountMin { get; set; } = 1;

		[JsonProperty("yieldAmountMax")]
		public int YieldAmountMax { get; set; } = 1;

		[JsonProperty("gatherTimeSeconds")]
		public int GatherTimeSeconds { get; set; } = 3;

		[JsonProperty("gatherAnimation")]
		public string GatherAnimation { get; set; } // e.g., "GATHER_PLANT"

		[JsonProperty("respawnTimeSeconds")]
		public int RespawnTimeSeconds { get; set; } = 300; // Default 5 minutes

		// Optional: Maybe add required tool, skill level, etc.
	}

	// --- Data defining NODE SPAWN LOCATIONS (from resource_node_spawns.json) ---

	public class MapResourceNodeSpawns
	{
		[JsonProperty("mapClassName")]
		public string MapClassName { get; set; }

		[JsonProperty("spawnPoints")]
		public List<ResourceNodeSpawnPoint> SpawnPoints { get; set; } = new List<ResourceNodeSpawnPoint>();
	}

	public class ResourceNodeSpawnPoint
	{
		[JsonProperty("pointName")]
		public string PointName { get; set; } // Optional

		[JsonProperty("nodeTypeName")]
		public string NodeTypeName { get; set; } // Which type of node spawns here

		[JsonProperty("position")]
		public PositionData Position { get; set; }

		[JsonProperty("direction")]
		public double Direction { get; set; }

		// Optional: Spawn chance, conditions?
	}
}
