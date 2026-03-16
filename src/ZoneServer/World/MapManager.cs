using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using Yggdrasil.Logging;

namespace Melia.Zone.World
{
	/// <summary>
	/// Holds and manages a list of maps.
	/// </summary>
	public class MapManager
	{
		private readonly ConcurrentDictionary<string, Map> _mapsName = new(StringComparer.OrdinalIgnoreCase);
		private readonly ConcurrentDictionary<int, Map> _maps = new();

		private readonly ConcurrentDictionary<string, HashSet<string>> _mapConnections = new(StringComparer.OrdinalIgnoreCase);
		private readonly object _connectionLock = new();

		/// <summary>
		/// Start of the dynamic maps id range
		/// </summary>
		public const int DynamicMaps = 35001;

		/// <summary>
		/// Returns the number of maps currently loaded.
		/// </summary>
		public int Count => _maps.Count;

		/// <summary>
		/// Adds map to the world.
		/// </summary>
		/// <param name="map"></param>
		/// <exception cref="ArgumentException">
		/// Thrown if a map with the same id as the given one
		/// already exists.
		/// </exception>
		public void Add(Map map)
		{
			if (!_maps.TryAdd(map.WorldId, map))
			{
				Log.Warning($"Map id {map.Id} ('{map.ClassName}') already exists. It will not be added again.");
				return;
			}

			if (!_mapsName.TryAdd(map.ClassName, map))
			{
				Log.Warning($"Map with ClassName '{map.ClassName}' already exists in name dictionary. WorldId: {map.WorldId}");
			}
		}

		/// <summary>
		/// Removes map with given id from the world.
		/// </summary>
		/// <param name="worldMapId"></param>
		/// <exception cref="ArgumentException">
		/// Thrown if no map with the given id exists.
		/// </exception>
		public void Remove(int worldMapId)
		{
			if (!_maps.TryRemove(worldMapId, out var map))
				throw new ArgumentException($"Map {worldMapId} doesn't exist.");

			if (map != null)
				_mapsName.TryRemove(map.ClassName, out _);
		}

		/// <summary>
		/// Returns true if a map with the given id exists.
		/// </summary>
		/// <param name="worldMapId"></param>
		/// <returns></returns>
		public bool Has(int worldMapId)
		{
			return _maps.ContainsKey(worldMapId);
		}

		/// <summary>
		/// Returns true if a map with the given id exists.
		/// </summary>
		/// <param name="mapName"></param>
		/// <returns></returns>
		public bool Has(string mapName)
		{
			return _mapsName.ContainsKey(mapName);
		}

		/// <summary>
		/// Returns the map with the given id. Returns null if map
		/// was not found.
		/// </summary>
		/// <param name="worldId"></param>
		/// <returns></returns>
		public Map Get(int worldId)
		{
			_maps.TryGetValue(worldId, out var map);
			return map;
		}

		/// <summary>
		/// Returns map by name, or null if it doesn't exist.
		/// </summary>
		/// <param name="mapClassName"></param>
		public Map Get(string mapClassName)
		{
			_mapsName.TryGetValue(mapClassName, out var result);
			return result;
		}

		/// <summary>
		/// Returns map with given id via out, returns true if the
		/// map was found and false if not. If it wasn't found,
		/// the value returned via out will be null.
		/// </summary>
		/// <param name="worldMapId"></param>
		/// <param name="map"></param>
		/// <returns></returns>
		public bool TryGet(int worldMapId, out Map map)
		{
			return _maps.TryGetValue(worldMapId, out map);
		}

		/// <summary>
		/// Returns map with given name via out, returns true if the
		/// map was found and false if not. If it wasn't found,
		/// the value returned via out will be null.
		/// </summary>
		/// <param name="mapName"></param>
		/// <param name="map"></param>
		/// <returns></returns>
		public bool TryGet(string mapName, out Map map)
		{
			return _mapsName.TryGetValue(mapName, out map);
		}

		/// <summary>
		/// Returns a list of all maps.
		/// </summary>
		/// <returns></returns>
		public Map[] GetList()
		{
			return _maps.Values.ToArray();
		}

		/// <summary>
		/// Returns a list of all maps that match the given predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public Map[] GetList(Func<Map, bool> predicate)
		{
			return _maps.Values.Where(predicate).ToArray();
		}

		/// <summary>
		/// Executes the given function an all maps.
		/// </summary>
		/// <param name="func"></param>
		public void Execute(Action<Map> func)
		{
			foreach (var map in _maps.Values)
				func(map);
		}

		/// <summary>
		/// Returns a list of results queried from all maps.
		/// </summary>
		/// <typeparam name="TObj"></typeparam>
		/// <param name="func"></param>
		/// <returns></returns>
		public TObj[] ExecuteQuery<TObj>(Func<Map, IEnumerable<TObj>> func)
		{
			return _maps.Values.SelectMany(func).ToArray();
		}

		/// <summary>
		/// Generates a new id for a dynamic map.
		/// </summary>
		/// <returns></returns>
		public int GenerateDynamicMapId()
		{
			for (var i = DynamicMaps; i < ushort.MaxValue; ++i)
			{
				if (!_maps.ContainsKey(i))
					return i;
			}

			throw new Exception("No dynamic map ids available.");
		}

		/// <summary>
		/// Removes all scripted entities, like NPCs.
		/// </summary>
		public void RemoveScriptedEntities()
		{
			foreach (var map in _maps.Values)
				map.RemoveScriptedEntities();
		}

		/// <summary>
		/// Returns the first character found with the given team name,
		/// or null if none were found.
		/// </summary>
		public Character GetCharacterByTeamName(string teamName)
		{
			foreach (var map in _maps.Values)
			{
				var character = map.GetCharacterByTeamName(teamName);
				if (character != null)
					return character;
			}

			return null;
		}

		/// <summary>
		/// Returns the first monster that matches the given predicate
		/// on any map via out. Returns false if no matching monster was
		/// found.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="monster"></param>
		/// <returns></returns>
		public bool TryGetMonster(Func<IMonster, bool> predicate, out IMonster monster)
		{
			foreach (var map in _maps.Values)
			{
				if (map.TryGetMonster(predicate, out var m))
				{
					monster = m;
					return true;
				}
			}

			monster = null;
			return false;
		}

		/// <summary>
		/// Returns the total number of player characters across all maps.
		/// </summary>
		/// <returns></returns>
		public int GetCharacterCount()
		{
			return _maps.Values.Sum(a => a.CharacterCount);
		}

		/// <summary>
		/// Returns all characters that are currently online.
		/// </summary>
		public Character[] GetCharacters()
		{
			return _maps.Values.SelectMany(a => a.GetCharacters()).ToArray();
		}

		/// <summary>
		/// Returns all online characters that match the given predicate.
		/// </summary>
		public Character[] GetCharacters(Func<Character, bool> predicate)
		{
			return _maps.Values.SelectMany(a => a.GetCharacters(predicate)).ToArray();
		}

		/// <summary>
		/// Broadcasts packet on all maps.
		/// </summary>
		/// <param name="packet"></param>
		public void Broadcast(Packet packet)
		{
			foreach (var map in _maps.Values)
				map.Broadcast(packet);
		}

		/// <summary>
		/// Returns all online characters that match the given predicate.
		/// </summary>
		public Character GetCharacter(Func<Character, bool> predicate)
		{
			foreach (var map in _maps.Values)
			{
				var character = map.GetCharacter(predicate);

				if (character != null)
					return character;
			}
			return null;
		}

		/// <summary>
		/// Records a direct connection between two maps, typically when a warp is added.
		/// Automatically records the reverse connection as well.
		/// </summary>
		/// <param name="sourceMap">The map the warp starts on.</param>
		/// <param name="destinationMap">The map the warp leads to.</param>
		public void RecordMapConnection(Map sourceMap, Map destinationMap)
		{
			if (sourceMap?.ClassName == null || destinationMap?.ClassName == null || sourceMap == destinationMap || sourceMap == Map.Limbo || destinationMap == Map.Limbo)
			{
				return;
			}

			var sourceName = sourceMap.ClassName;
			var destName = destinationMap.ClassName;

			lock (_connectionLock)
			{
				// Add forward connection
				if (!_mapConnections.TryGetValue(sourceName, out var forwardSet))
				{
					forwardSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
					_mapConnections[sourceName] = forwardSet;
				}
				forwardSet.Add(destName);


				// Add reverse connection
				if (!_mapConnections.TryGetValue(destName, out var reverseSet))
				{
					reverseSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
					_mapConnections[destName] = reverseSet;
				}
				reverseSet.Add(sourceName);
			}
		}

		/// <summary>
		/// Gets a list of map ClassNames directly connected to the source map via recorded connections (warps).
		/// </summary>
		/// <param name="sourceMapClassName">The ClassName of the source map.</param>
		/// <returns>A list of connected map ClassNames, or an empty list if none found.</returns>
		public List<string> GetConnectedMaps(string sourceMapClassName)
		{
			lock (_connectionLock)
			{
				if (_mapConnections.TryGetValue(sourceMapClassName, out var connectedSet))
				{
					return connectedSet.ToList(); // Return a copy
				}
			}
			return new List<string>(); // Return empty list
		}

		/// <summary>
		/// Clears all recorded map connections. Call this if warps are reloaded.
		/// </summary>
		public void ClearMapConnections()
		{
			lock (_connectionLock)
			{
				_mapConnections.Clear();
				Log.Info("MapManager: Cleared all recorded map connections.");
			}
		}

		// Optional: Debug method
		public void LogMapConnections()
		{
			lock (_connectionLock)
			{
				Log.Debug("--- Recorded Map Connections (MapManager) ---");
				foreach (var kvp in _mapConnections.OrderBy(k => k.Key))
				{
					Log.Debug($"{kvp.Key} -> [{string.Join(", ", kvp.Value.OrderBy(v => v))}]");
				}
				Log.Debug("---------------------------------------------");
			}
		}
	}
}
