using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	/// <summary>
	/// A map's star rank, which decides its death penalty.
	/// </summary>
	public class MapRankData
	{
		public string MapClassName { get; set; }
		public int Rank { get; set; }
	}

	/// <summary>
	/// Map star rank database, indexed by map class name.
	/// </summary>
	public class MapRankDb : DatabaseJsonIndexed<string, MapRankData>
	{
		/// <summary>
		/// Returns the map's star rank, or 0 if it has none.
		/// </summary>
		/// <param name="mapClassName"></param>
		/// <returns></returns>
		public int GetRank(string mapClassName)
		{
			if (mapClassName == null || !this.Entries.TryGetValue(mapClassName.ToLowerInvariant(), out var data))
				return 0;

			return data.Rank;
		}

		/// <summary>
		/// Reads given entry and adds it to the database.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("map", "rank");

			var data = new MapRankData();

			data.MapClassName = entry.ReadString("map");
			data.Rank = entry.ReadInt("rank");

			this.Entries[data.MapClassName.ToLowerInvariant()] = data;
		}
	}
}
