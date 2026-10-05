using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Melia.Shared.World;
using Melia.Zone.Data.Spawning;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Maps;
using Melia.Zone.World.Actors.Monsters;
using Newtonsoft.Json;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone.Spawning
{
	public class NpcSpawnManager
	{
		private static readonly string SpawnFilePath = Path.Combine("system", "db", "procedural", "npc_spawns.json");
		private static List<MapSpawnDefinition> _mapSpawnDefinitions = new();
		private static Dictionary<string, List<NpcSpawnPoint>> _spawnsByMap = new(StringComparer.OrdinalIgnoreCase);

		// Simple name generation (replace with your actual name generator if you have one)
		private static readonly string[] _firstNames = { "Alden", "Brynn", "Cassian", "Delia", "Erion", "Fiona", "Gareth", "Hazel", "Ivar", "Jenna" };
		private static readonly string[] _lastNames = { "Oakwood", "Stonehand", "Riverun", "Brightwood", "Shadowclaw", "Ironhide" };


		/// <summary>
		/// Loads the NPC spawn definitions from the JSON file.
		/// Should be called once during server startup.
		/// </summary>
		public static bool LoadSpawnData()
		{
			Log.Info("Loading procedural NPC spawn data...");
			if (!File.Exists(SpawnFilePath))
			{
				Log.Error($"Procedural NPC spawn file not found at: {Path.GetFullPath(SpawnFilePath)}");
				return false;
			}

			try
			{
				var jsonContent = File.ReadAllText(SpawnFilePath);
				_mapSpawnDefinitions = JsonConvert.DeserializeObject<List<MapSpawnDefinition>>(jsonContent);

				if (_mapSpawnDefinitions == null)
				{
					Log.Error($"Failed to deserialize NPC spawn data from {SpawnFilePath}. Result was null.");
					_mapSpawnDefinitions = new List<MapSpawnDefinition>(); // Ensure not null
					return false;
				}

				// Organize data into a dictionary for faster lookup by map ClassName
				_spawnsByMap.Clear();
				foreach (var mapDef in _mapSpawnDefinitions)
				{
					if (string.IsNullOrWhiteSpace(mapDef.MapClassName)) continue;
					_spawnsByMap[mapDef.MapClassName] = mapDef.SpawnPoints ?? new List<NpcSpawnPoint>();
				}

				foreach (var mapClassName in _spawnsByMap.Keys)
				{
					if (ZoneServer.Instance.World.TryGetMap(mapClassName, out var map))
					{
						InitializeMapSpawns(map);
					}
				}

				Log.Info($"Loaded {_mapSpawnDefinitions.Count} map definitions with a total of {_spawnsByMap.Values.Sum(list => list.Count)} NPC spawn points.");
				return true;
			}
			catch (JsonException jsonEx)
			{
				Log.Error($"JSON Error loading NPC spawn data from {SpawnFilePath}: {jsonEx.Message}");
				Log.Error(jsonEx);
				return false;
			}
			catch (Exception ex)
			{
				Log.Error($"Error loading NPC spawn data from {SpawnFilePath}: {ex.Message}");
				Log.Error(ex);
				return false;
			}
		}

		/// <summary>
		/// Spawns procedural NPCs for a given map instance when it loads or resets.
		/// </summary>
		/// <param name="mapInstance">The map instance to spawn NPCs onto.</param>
		public static void SpawnNpcsForMap(Map mapInstance)
		{
			if (mapInstance == null || mapInstance == Map.Limbo) return;

			if (!_spawnsByMap.TryGetValue(mapInstance.ClassName, out var spawnPoints))
			{
				// No procedural spawns defined for this map
				Log.Debug($"No procedural NPC spawn points found for map: {mapInstance.ClassName}");
				return;
			}

			if (spawnPoints.Count == 0) return;

			Log.Debug($"Spawning procedural NPCs for map: {mapInstance.ClassName} ({spawnPoints.Count} points defined)");
			var spawnedCount = 0;

			foreach (var point in spawnPoints)
			{
				// Select which NPC (if any) to spawn at this point based on chance
				var npcToSpawnData = SelectNpcToSpawn(point.PossibleNPCs);
				var position = point.Position;

				if (npcToSpawnData != null)
				{
					// Generate a name
					var finalName = GenerateNpcName(npcToSpawnData.NameFormat);

					// Calculate Y position based on ground height
					var groundY = position.Y; // Use Y from data as fallback
					if (mapInstance.Ground.TryGetHeightAt(position, out float height))
					{
						groundY = height;
					}
					else
					{
						Log.Warning($"Could not get ground height for NPC spawn '{npcToSpawnData.NameFormat}' at {point.Position.X},{point.Position.Z} on map {mapInstance.ClassName}. Using Y={groundY}.");
						mapInstance.Ground.TryGetNearestValidPosition(point.Position, out position);
					}

					// Use Shortcuts.AddNpc or a dedicated method
					// We need to ensure the Role and DialogFunction are handled
					try
					{
						// Using a hypothetical overload or modification of AddNpc that handles Role/DialogFunc
						SpawnNpcInternal(
							mapInstance: mapInstance,
							modelId: npcToSpawnData.ModelId,
							name: finalName,
							role: npcToSpawnData.Role,
							dialogFuncName: npcToSpawnData.DialogFunction,
							enterFuncName: npcToSpawnData.EnterFunction,
							leaveFuncName: npcToSpawnData.LeaveFunction,
							x: point.Position.X,
							y: groundY, // Use calculated ground height
							z: point.Position.Z,
							direction: point.Direction
						// Pass other optional params like scale, state if defined in data
						);
						spawnedCount++;
					}
					catch (Exception ex)
					{
						Log.Error($"Failed to spawn procedural NPC '{finalName}' (Model: {npcToSpawnData.ModelId}) on map {mapInstance.ClassName}: {ex.Message}");
						Log.Error(ex);
					}
				}
			}
			Log.Info($"Spawned {spawnedCount} procedural NPCs on map {mapInstance.ClassName}.");
		}

		/// <summary>
		/// Selects one NPC template from a list based on SpawnChance.
		/// </summary>
		private static PossibleNpcData SelectNpcToSpawn(List<PossibleNpcData> possibilities)
		{
			if (possibilities == null || possibilities.Count == 0) return null;

			var totalChance = possibilities.Sum(npc => Math.Max(0, npc.SpawnChance)); // Ensure non-negative chance
			if (totalChance <= 0) return null; // No chance for any NPC

			// Normalize chances if they exceed 1.0, or just use them as weights?
			// Simple weighted random selection:
			var roll = RandomProvider.Get().NextDouble() * totalChance;
			double cumulative = 0;

			foreach (var npcData in possibilities)
			{
				cumulative += Math.Max(0, npcData.SpawnChance);
				if (roll < cumulative)
				{
					return npcData; // Select this NPC
				}
			}

			// Fallback (should theoretically not be reached if totalChance > 0)
			return possibilities.LastOrDefault(npc => npc.SpawnChance > 0);
		}

		/// <summary>
		/// Generates a name based on the format string.
		/// </summary>
		private static string GenerateNpcName(string nameFormat)
		{
			if (string.IsNullOrWhiteSpace(nameFormat)) return "Nameless NPC";

			// Replace placeholders - Use a more robust name generator if available
			var finalName = nameFormat;
			if (finalName.Contains("%FirstName%"))
			{
				finalName = finalName.Replace("%FirstName%", _firstNames[RandomProvider.Get().Next(_firstNames.Length)]);
			}
			if (finalName.Contains("%LastName%"))
			{
				finalName = finalName.Replace("%LastName%", _lastNames[RandomProvider.Get().Next(_lastNames.Length)]);
			}

			// Wrap name in localization code if applicable
			if (Dialog.IsLocalizationKey(finalName))
			{
				finalName = Dialog.WrapLocalizationKey(finalName);
			}
			// Insert line breaks in tagged NPC names that don't have one
			else if (finalName.StartsWith('[') && !finalName.Contains("{nl}"))
			{
				var endIndex = finalName.LastIndexOf("] ");
				if (endIndex != -1)
				{
					// Remove space and insert new line instead.
					finalName = finalName.Remove(endIndex + 1, 1);
					finalName = finalName.Insert(endIndex + 1, "{nl}");
				}
			}

			return finalName;
		}

		/// <summary>
		/// Internal helper to spawn the NPC, setting Role and Dialog Function.
		/// This replaces direct calls to Shortcuts.AddNpc from outside this manager.
		/// </summary>
		private static Npc SpawnNpcInternal(Map mapInstance, int modelId, string name, string role, float x, float y, float z, double direction, string dialogFuncName = null, string enterFuncName = null, string leaveFuncName = null)
		{
			// This method encapsulates the actual NPC creation and setup

			// 1. Look up the Dialog Function
			DialogFunc dialogFunc = default;
			if (!string.IsNullOrEmpty(dialogFuncName) && !ZoneServer.Instance.DialogFunctions.TryGet(dialogFuncName, out dialogFunc))
			{
				Log.Warning($"Dialog function '{dialogFuncName}' specified in spawn data for NPC '{name}' (Model: {modelId}) not found.");
				// Decide: Spawn without dialog, or don't spawn? Let's spawn without.
			}

			// 1a. Look up the Enter Function
			TriggerActorFuncAsync enterFunc = default;
			if (!string.IsNullOrEmpty(enterFuncName) && !ZoneServer.Instance.TriggerFunctions.TryGet(enterFuncName, out enterFunc))
			{
				Log.Warning($"Enter Trigger function '{enterFuncName}' specified in spawn data for NPC '{name}' (Model: {modelId}) not found.");
			}

			// 1b. Look up the Leave Function
			TriggerActorFuncAsync leaveFunc = default;
			if (!string.IsNullOrEmpty(leaveFuncName) && !ZoneServer.Instance.TriggerFunctions.TryGet(leaveFuncName, out leaveFunc))
			{
				Log.Warning($"Leave Trigger function '{leaveFuncName}' specified in spawn data for NPC '{name}' (Model: {modelId}) not found.");
			}

			// 2. Create the NPC instance using a basic Shortcuts.AddNpc overload or Npc constructor
			// We might need a simplified AddNpc that *doesn't* try to look up dialog funcs by name itself.
			// Let's assume we use the constructor and AddMonster directly for more control:

			var pos = new Position(x, y, z);
			var loc = new Location(mapInstance.Id, pos);
			var dir = new Direction(direction);

			var npc = new Npc(modelId, name, loc, dir, 0); // Using Npc constructor
			npc.UniqueName = $"PROC_NPC_{npc.Handle}"; // Assign a unique name pattern

			// 3. Set Role (using Properties as decided)
			if (!string.IsNullOrEmpty(role))
			{
				npc.SetRole(role); // Key must match what ContextTrigger expects
			}

			// 4. Set Dialog Trigger *using the looked-up delegate*
			if (dialogFunc != null)
			{
				npc.SetClickTrigger(dialogFuncName, dialogFunc);
			}

			if (enterFunc != null)
			{
				npc.SetEnterTrigger(enterFuncName, enterFunc);
			}

			if (leaveFunc != null)
			{
				npc.SetLeaveTrigger(leaveFuncName, leaveFunc);
			}

			// 5. Set other properties (Scale, State, etc.) if needed from data

			// 6. Add to map
			mapInstance.AddMonster(npc);

			ZoneServer.Instance.World.NPCs.TryAdd(npc.UniqueName, npc);

			Log.Debug($"Spawned NPC {npc.Name} at {npc.Position} on {mapInstance.ClassName}");

			return npc;
		}

		/// <summary>
		/// Hook this method into your server's map loading/initialization process.
		/// </summary>
		public static void InitializeMapSpawns(Map map)
		{
			ClearProceduralNpcsFromMap(map);
			SpawnNpcsForMap(map);
		}

		public static void ClearProceduralNpcsFromMap(Map mapInstance)
		{
			var proceduralNpcs = mapInstance.GetMonsters(m => m is Npc npc && !string.IsNullOrEmpty(npc.UniqueName) && npc.UniqueName.StartsWith("PROC_NPC_")).ToList();
			if (proceduralNpcs.Count != 0)
				Log.Debug($"Removing {proceduralNpcs.Count} procedural NPCs from map {mapInstance.ClassName}.");
			foreach (var npc in proceduralNpcs)
			{
				mapInstance.RemoveMonster(npc);
				ZoneServer.Instance.World.NPCs.TryRemove(npc.UniqueName, out _);
			}
		}
	}
}
