using System.Collections.Generic;
using Newtonsoft.Json;

namespace Melia.Zone.Data.Spawning
{
	public class MapPoiData
	{
		[JsonProperty("mapClassName")]
		public string MapClassName { get; set; }

		[JsonProperty("pointsOfInterest")]
		public List<PointOfInterest> PointsOfInterest { get; set; } = new List<PointOfInterest>();
	}

	public class PointOfInterest
	{
		[JsonProperty("poiName")]
		public string PoiName { get; set; } // Internal unique name for this POI

		[JsonProperty("displayName")]
		public string DisplayName { get; set; } // User-facing name

		[JsonProperty("position")]
		public PositionData Position { get; set; } // Center position

		[JsonProperty("radius")]
		public float Radius { get; set; } = 50f; // Default radius
	}
}
