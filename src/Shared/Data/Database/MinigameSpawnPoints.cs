using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.World;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class MinigameSpawnPointData
	{
		public string MapClassName { get; set; }
		public Position Position { get; set; }
		public Direction Direction { get; set; }
	}

	/// <summary>
	/// Minigame spawn point database.
	/// </summary>
	public class MinigameSpawnPointDb : DatabaseJson<MinigameSpawnPointData>
	{
		/// <summary>
		/// Returns all entries for given map.
		/// </summary>
		/// <param name="mapClassName"></param>
		/// <returns></returns>
		public MinigameSpawnPointData[] Find(string mapClassName)
			=> this.Entries.Where(x => x.MapClassName == mapClassName).ToArray();

		/// <summary>
		/// Returns all minigame spawn locations for the given map.
		/// </summary>
		/// <param name="mapClassName"></param>
		/// <returns></returns>
		public List<Position> FindPositions(string mapClassName)
			=> this.Entries.Where(x => x.MapClassName == mapClassName).Select(x => x.Position).ToList();

		/// <summary>
		/// Reads given entry and adds it to the database.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("map", "x", "y", "z", "direction");

			var data = new MinigameSpawnPointData();

			data.MapClassName = entry.ReadString("map");

			var x = entry.ReadFloat("x");
			var y = entry.ReadFloat("y");
			var z = entry.ReadFloat("z");
			data.Position = new Position(x, y, z);
			data.Direction = new Direction(entry.ReadFloat("direction"));

			this.Add(data);
		}
	}
}
