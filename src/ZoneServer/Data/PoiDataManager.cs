using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Melia.Zone.Data.Spawning;
using Newtonsoft.Json;
using Yggdrasil.Logging;

namespace Melia.Zone.Data
{
	public static class PoiDataManager
	{
		private static readonly string PoiFilePath = Path.Combine("system", "db", "procedural", "map_pois.json");
		private static Dictionary<string, List<PointOfInterest>> _poisByMap = new(StringComparer.OrdinalIgnoreCase);

		public static bool LoadPoiData()
		{
			Log.Info("Loading Point of Interest data...");
			if (!File.Exists(PoiFilePath)) { Log.Error($"POI file not found: {Path.GetFullPath(PoiFilePath)}"); return false; }

			try
			{
				var jsonContent = File.ReadAllText(PoiFilePath);
				var mapPoiList = JsonConvert.DeserializeObject<List<MapPoiData>>(jsonContent);

				if (mapPoiList == null) { Log.Error($"Failed to deserialize POI data from {PoiFilePath}."); return false; }

				_poisByMap.Clear();
				var totalPois = 0;
				foreach (var mapData in mapPoiList)
				{
					if (string.IsNullOrWhiteSpace(mapData.MapClassName) || mapData.PointsOfInterest == null) continue;
					_poisByMap[mapData.MapClassName] = mapData.PointsOfInterest;
					totalPois += mapData.PointsOfInterest.Count;
				}
				Log.Info($"Loaded {totalPois} Points of Interest across {_poisByMap.Count} maps.");
				return true;
			}
			catch (Exception ex)
			{
				Log.Error($"Error loading POI data from {PoiFilePath}");
				Log.Error(ex);
				return false;
			}
		}

		public static List<PointOfInterest> GetPoisForMap(string mapClassName)
		{
			_poisByMap.TryGetValue(mapClassName, out var pois);
			return pois ?? new List<PointOfInterest>(); // Return empty list if not found
		}
	}
}
