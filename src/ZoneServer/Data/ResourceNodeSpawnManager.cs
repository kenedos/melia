using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Data.Spawning;
using Melia.Zone.Scripting.Dialogues; // For DialogFunc definition
using Melia.Zone.World.Maps;    // For Map class
using Melia.Zone.World.Actors.Monsters; // For Npc class
using Newtonsoft.Json;
using Yggdrasil.Logging;
// using Yggdrasil.Util; // Not needed for Random if nodes always spawn

using static Melia.Zone.Scripting.Shortcuts;
using Melia.Zone.Data;

namespace Melia.Zone.Spawning
{
	public static class ResourceNodeSpawnManager
	{
		// --- File Paths ---
		private static readonly string NodeSpawnsPath = Path.Combine("system", "db", "procedural", "resource_node_spawns.json");

		// --- Loaded Data ---
		private static readonly Dictionary<string, List<ResourceNodeSpawnPoint>> _spawnsByMap = new(StringComparer.OrdinalIgnoreCase);

		// Name of the shared dialog function for all nodes
		private const string NodeDialogFunctionName = "GenericResourceNodeDialog";

		public static bool TryGetSpawnPointsForMap(string map, out List<ResourceNodeSpawnPoint> resourceNodeSpawnPoints)
		{
			lock (_spawnsByMap)
				return _spawnsByMap.TryGetValue(map, out resourceNodeSpawnPoints);
		}

		/// <summary>
		/// Loads resource node definitions and spawn points from JSON files.
		/// Should be called once during server startup.
		/// </summary>
		public static bool LoadAllNodeData()
		{
			var defSuccess = ResourceNodeDataManager.LoadNodeDefinitions();
			var spawnSuccess = LoadNodeSpawnPoints();
			return defSuccess && spawnSuccess;
		}

		/// <summary>
		/// Loads the specific spawn locations for nodes on maps.
		/// </summary>
		private static bool LoadNodeSpawnPoints()
		{
			Log.Info("Loading resource node spawn points...");
			if (!File.Exists(NodeSpawnsPath)) { Log.Error($"Node spawn points file not found: {Path.GetFullPath(NodeSpawnsPath)}"); return false; }

			try
			{
				var jsonContent = File.ReadAllText(NodeSpawnsPath);
				var mapSpawnList = JsonConvert.DeserializeObject<List<MapResourceNodeSpawns>>(jsonContent);

				if (mapSpawnList == null) { Log.Error($"Failed to deserialize node spawn points from {NodeSpawnsPath}. Result was null."); return false; }

				_spawnsByMap.Clear();
				var totalPoints = 0;
				foreach (var mapDef in mapSpawnList)
				{
					if (string.IsNullOrWhiteSpace(mapDef.MapClassName)) continue;
					var validPoints = mapDef.SpawnPoints?
							.Where(sp => !string.IsNullOrWhiteSpace(sp.NodeTypeName)
									  && ResourceNodeDataManager.GetData(sp.NodeTypeName) != null)
							.ToList() ?? new List<ResourceNodeSpawnPoint>();
					_spawnsByMap[mapDef.MapClassName] = validPoints;
					totalPoints += validPoints.Count;
				}

				foreach (var mapClassName in _spawnsByMap.Keys)
				{
					if (ZoneServer.Instance.World.TryGetMap(mapClassName, out var map))
					{
						InitializeMapNodes(map);
					}
				}

				Log.Info($"Loaded {mapSpawnList.Count} map definitions with a total of {totalPoints} valid resource node spawn points.");
				return true;
			}
			catch (Exception ex)
			{
				Log.Error($"Error loading node spawn points from {NodeSpawnsPath}");
				Log.Error(ex);
				return false;
			}
		}


		/// <summary>
		/// Spawns resource nodes for a given map instance.
		/// </summary>
		/// <param name="mapInstance">The map instance.</param>
		public static void SpawnNodesForMap(Map mapInstance)
		{
			if (mapInstance == null || mapInstance == Map.Limbo) return;

			if (!_spawnsByMap.TryGetValue(mapInstance.ClassName, out var spawnPoints) || spawnPoints.Count == 0)
			{
				Log.Debug($"No resource node spawn points found for map: {mapInstance.ClassName}");
				return;
			}

			Log.Debug($"Spawning resource nodes for map: {mapInstance.ClassName} ({spawnPoints.Count} points defined)");
			var spawnedCount = 0;

			// Look up the shared dialog function ONCE
			if (!ZoneServer.Instance.DialogFunctions.TryGet(NodeDialogFunctionName, out var nodeDialogFunc))
			{
				Log.Error($"Shared node dialog function '{NodeDialogFunctionName}' not found! Nodes will not be interactive.");
				// Proceed spawning non-interactive nodes? Or stop? Let's proceed but log error.
			}

			foreach (var point in spawnPoints)
			{
				// Get the definition for this node type
				if (!ResourceNodeDataManager.TryGetData(point.NodeTypeName, out var nodeDef))
				{
					Log.Warning($"Node definition '{point.NodeTypeName}' specified at spawn point '{point.PointName}' on map {mapInstance.ClassName} not found. Skipping.");
					continue;
				}

				// Calculate Y position
				var position = new Position(point.Position.X, point.Position.Y, point.Position.Z);
				if (mapInstance.Ground.TryGetHeightAt(position, out var height))
				{
					position.Y = height;
				}
				else if (!mapInstance.Ground.TryGetNearestValidPosition(position, out position)) // Try finding nearest valid spot
				{
					Log.Warning($"Could not get valid ground position for node '{point.NodeTypeName}' near {point.Position.X},{point.Position.Z} on map {mapInstance.ClassName}. Skipping.");
					continue;
				}

				try
				{
					SpawnNodeInternal(mapInstance, nodeDef, point, position, nodeDialogFunc);
					spawnedCount++;
				}
				catch (Exception ex)
				{
					Log.Error($"Failed to spawn resource node '{nodeDef.NodeTypeName}' on map {mapInstance.ClassName}");
					Log.Error(ex);
				}
			}
			Log.Info($"Spawned {spawnedCount} resource nodes on map {mapInstance.ClassName}.");
		}


		/// <summary>
		/// Internal helper to create and configure the node NPC.
		/// </summary>
		private static Npc SpawnNodeInternal(Map mapInstance, ResourceNodeDefinition nodeDef, ResourceNodeSpawnPoint spawnPoint, Position finalPosition, DialogFunc dialogFunc)
		{
			var loc = new Location(mapInstance.Id, finalPosition);
			var dir = new Direction(spawnPoint.Direction);

			// Create NPC using the node's Model ID and Type Name
			// Use L() if NodeTypeName can be a localization key
			var nodeNpc = new Npc(nodeDef.ModelId, L(nodeDef.NodeTypeName), loc, dir, 0);
			nodeNpc.UniqueName = $"PROC_NODE_{nodeDef.NodeTypeName}_{nodeNpc.Handle}"; // Unique name

			// --- Configure Node Properties ---
			nodeNpc.SetRole("ResourceNode"); // Set specific role
			nodeNpc.Vars.SetString("NodeType", nodeDef.NodeTypeName); // Store type name for dialog
			nodeNpc.Vars.SetBool("IsDepleted", false); // Initial state
			nodeNpc.Vars.Set("RespawnTime", DateTime.MinValue); // Initial state

			// Make it non-aggressive, non-moving, potentially non-targetable?
			nodeNpc.Faction = FactionType.Neutral;
			nodeNpc.Properties.SetFloat(PropertyName.MHP, 999999);
			nodeNpc.Properties.SetFloat(PropertyName.HP, 999999);

			// Assign the shared dialog function
			if (dialogFunc != null)
			{
				nodeNpc.SetClickTrigger(NodeDialogFunctionName, dialogFunc);
			}

			// Add to map
			mapInstance.AddMonster(nodeNpc);
			// No need to add to World.NPCs by UniqueName unless needed for global lookup

			Log.Debug($"Spawned Node {nodeNpc.Name} ({nodeDef.NodeTypeName}) at {finalPosition} on {mapInstance.ClassName}");
			return nodeNpc;
		}


		/// <summary>
		/// Hook this method into your server's map loading/initialization process, AFTER NpcSpawnManager.
		/// </summary>
		public static void InitializeMapNodes(Map map)
		{
			ClearResourceNodesFromMap(map);
			SpawnNodesForMap(map);
		}

		public static void ClearResourceNodesFromMap(Map mapInstance)
		{
			var nodeNpcs = mapInstance.GetMonsters(m => m is Npc npc && npc.GetRole() == "ResourceNode").ToList();
			Log.Debug($"Removing {nodeNpcs.Count} resource nodes from map {mapInstance.ClassName}.");
			foreach (var npc in nodeNpcs) { mapInstance.RemoveMonster(npc); }
		}
	}
}
