using System.Collections.Generic;
using Melia.Shared.World;
using Newtonsoft.Json;

namespace Melia.Zone.Data.Spawning
{
	// Represents the top-level structure (a list of map definitions)
	// The JSON file itself will be deserialized into List<MapSpawnDefinition>

	public class MapSpawnDefinition
	{
		[JsonProperty("mapClassName")]
		public string MapClassName { get; set; }

		[JsonProperty("spawnPoints")]
		public List<NpcSpawnPoint> SpawnPoints { get; set; } = new List<NpcSpawnPoint>();
	}

	public class NpcSpawnPoint
	{
		[JsonProperty("pointName")]
		public string PointName { get; set; } // Optional identifier

		[JsonProperty("position")]
		public Position Position { get; set; } // Use a simple struct/class for position

		[JsonProperty("direction")]
		public double Direction { get; set; }

		// List of potential NPCs that can spawn at this point
		[JsonProperty("possibleNPCs")]
		public List<PossibleNpcData> PossibleNPCs { get; set; } = new List<PossibleNpcData>();
	}

	// Using a simple struct/class for position data in JSON
	public class PositionData
	{
		[JsonProperty("x")]
		public float X { get; set; }

		[JsonProperty("y")]
		public float Y { get; set; } // Can be 0 initially, calculated later

		[JsonProperty("z")]
		public float Z { get; set; }

		public Position ToPosition => new Position(X, Y, Z);
	}

	public class PossibleNpcData
	{
		[JsonProperty("modelId")]
		public int ModelId { get; set; } // The MonsterId to use for appearance

		[JsonProperty("nameFormat")]
		public string NameFormat { get; set; } // Template for the name, e.g., "[Hunter] %FirstName%"

		[JsonProperty("role")]
		public string Role { get; set; } // Crucial for identifying NPC function

		[JsonProperty("dialogFunction", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string DialogFunction { get; set; } // Name of the dialog function to assign

		[JsonProperty("enterFunction", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string EnterFunction { get; set; } // Name of the dialog function to assign

		[JsonProperty("LeaveFunction", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string LeaveFunction { get; set; } // Name of the dialog function to assign

		// Optional fields with default values
		[JsonProperty("minLevel", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int MinLevel { get; set; } = 1; // Default to 1 if missing

		[JsonProperty("maxLevel", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int MaxLevel { get; set; } = 999; // Default to high value if missing

		[JsonProperty("spawnChance", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public double SpawnChance { get; set; } = 1.0; // Default to 100% if missing

		// Add other optional properties as needed (e.g., scale, initial state, specific vars)
		// [JsonProperty("scale", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		// public double Scale { get; set; } = 1.0;

		[JsonProperty("initialVars", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public Dictionary<string, object> InitialVars { get; set; }
	}
}
