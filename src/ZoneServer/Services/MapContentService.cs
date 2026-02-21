using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Spawning;
using Melia.Zone.World;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;
using Melia.Zone.Data.Spawning;
using Melia.Zone.Data;
using Melia.Zone.Spawning;

namespace Melia.Zone.Services
{
	/// <summary>
	/// Provides methods to query map-specific content like spawnable monsters
	/// and available items.Assumes spawn/drop data is loaded and accessible
	/// via ZoneServer.Instance.
	/// </summary>
	/// <remarks>
	/// Why does this exist, you might ask?
	/// Because our spawning system overly complex or no damn reason, to tie
	/// maps and spawners together we need this.
	/// </remarks>
	public static class MapContentService
	{
		// --- Monster Related ---

		/// <summary>
		/// Gets monster IDs spawned on the specified map suitable for the given player level.
		/// Filters out NPCs and Material rank monsters.
		/// </summary>
		/// <param name="mapClassName">The ClassName of the map.</param>
		/// <param name="playerLevel">The player's level.</param>
		/// <param name="levelTolerance">How many levels +/- the player's level are acceptable.</param>
		/// <returns>A list of suitable monster IDs.</returns>
		public static List<int> GetSpawnableMonsterIds(string mapClassName, int playerLevel, int levelTolerance = 5)
		{
			var suitableIds = new HashSet<int>();
			var spawners = ZoneServer.Instance.World.GetSpawnersForMap(mapClassName); // Use your new World method

			foreach (var spawner in spawners)
			{
				// Get MonsterId from the specific spawner type
				var monsterId = GetSpawnerMonsterId(spawner);
				if (monsterId == 0) continue;

				if (suitableIds.Contains(monsterId))
					continue;

				// Get Monster Static Data
				if (ZoneServer.Instance.Data.MonsterDb.TryFind(monsterId, out var monsterData))
				{
					// Level Check
					var levelDifference = Math.Abs(monsterData.Level - playerLevel);
					if (levelDifference <= levelTolerance && monsterData.Rank != MonsterRank.NPC &&
							monsterData.Rank != MonsterRank.Material &&
							monsterData.Rank != MonsterRank.Neutral &&
							monsterData.Rank != MonsterRank.MISC)
					{
						suitableIds.Add(monsterId);
					}
				}
			}
			return suitableIds.ToList();
		}

		/// <summary>
		/// Helper to get MonsterId from different spawner types.
		/// </summary>
		/// <param name="spawner"></param>
		/// <returns></returns>
		private static int GetSpawnerMonsterId(ISpawner spawner)
		{
			if (spawner is MonsterSpawner ms) return ms.MonsterData.Id;
			if (spawner is EventMonsterSpawner ems) return ems.MonsterData.Id;
			// Add other spawner types if necessary
			return 0;
		}


		// --- Item Related ---

		/// <summary>
		/// Gets potential item drops from a list of monster IDs, suitable for gathering quests.
		/// </summary>
		/// <param name="monsterIds">List of monster IDs to check drops for.</param>
		/// <param name="minDropChance">Minimum base drop chance (0-100) to consider.</param>
		/// <returns>List of tuples containing (ItemId, SourceHint).</returns>
		public static List<(int ItemId, string SourceHint)> GetGatherableDropsFromMobs(IEnumerable<int> monsterIds, float minDropChance = 10.0f) // Default min chance 10%
		{
			var possibleItems = new List<(int ItemId, string SourceHint)>();
			if (monsterIds == null) return possibleItems;

			foreach (var mobId in monsterIds)
			{
				if (ZoneServer.Instance.Data.MonsterDb.TryFind(mobId, out var monsterData) && monsterData.Drops != null)
				{
					foreach (var dropData in monsterData.Drops)
					{
						if (dropData.ItemId == ItemId.Silver || dropData.ItemId == ItemId.Gold) continue;
						if (dropData.DropChance < minDropChance) continue;

						if (!ZoneServer.Instance.Data.ItemDb.Contains(dropData.ItemId))
						{
							Log.Warning($"GetGatherableDropsFromMobs: Monster {mobId} lists drop for non-existent item {dropData.ItemId}.");
							continue;
						}

						possibleItems.Add((dropData.ItemId, L($"from {L(monsterData.Name)}")));
					}
				}
			}

			return possibleItems.DistinctBy(item => item.ItemId).ToList();
		}

		/// <summary>
		/// Gets resource items available from nodes defined to spawn on a specific map.
		/// </summary>
		/// <param name="mapClassName">The ClassName of the map.</param>
		/// <returns>List of tuples containing (ItemId, SourceHint).</returns>
		public static List<(int ItemId, string SourceHint)> GetGatherableResourcesFromNodes(string mapClassName)
		{
			var possibleItems = new List<(int ItemId, string SourceHint)>();

			// --- Need access to the loaded resource node *spawn points* for this map ---
			// This data is currently loaded by ResourceNodeSpawnManager._spawnsByMap
			// Option A: Expose _spawnsByMap (or a getter) from ResourceNodeSpawnManager (simplest)
			// Option B: Pass the loaded spawn data to this service during init
			// Option C: Duplicate loading logic (less ideal)

			// Let's assume Option A for now: ResourceNodeSpawnManager has a static getter
			if (!ResourceNodeSpawnManager.TryGetSpawnPointsForMap(mapClassName, out var nodeSpawnsOnMap) || nodeSpawnsOnMap.Count == 0)
			{
				return possibleItems; // No nodes defined to spawn on this map
			}

			// Get unique node *types* that can spawn on this map
			var nodeTypesOnMap = nodeSpawnsOnMap.Select(sp => sp.NodeTypeName).Distinct(StringComparer.OrdinalIgnoreCase);

			foreach (var nodeTypeName in nodeTypesOnMap)
			{
				// Look up the definition for this node type
				if (ResourceNodeDataManager.TryGetData(nodeTypeName, out var nodeDef))
				{
					// Get the item yielded by this node type
					var yieldItemId = nodeDef.YieldItemId;
					var sourceHint = $"from {nodeDef.NodeTypeName} nodes"; // Simpler hint


					// Add to possible items list (ensure uniqueness by ItemId later if needed)
					possibleItems.Add((yieldItemId, sourceHint));
				}
				else
				{
					Log.Warning($"Resource node spawn point on map '{mapClassName}' references undefined node type '{nodeTypeName}'.");
				}
			}

			// Return unique items (in case multiple node types yield the same item)
			return possibleItems.GroupBy(item => item.ItemId)
							   .Select(group => group.First())
							   .ToList();
		}

		public static string GetBasicSourceHintForItem(string mapClassName, int itemId)
		{
			// 1. Check if item drops from mobs on map
			var spawnableMonsterIds = GetSpawnableMonsterIds(mapClassName, 1, 999); // Check all levels for any source
			foreach (var monsterId in spawnableMonsterIds)
			{
				if (ZoneServer.Instance.Data.MonsterDb.TryFind(monsterId, out var mobData) && mobData.Drops != null && mobData.Drops.Exists(d => d.ItemId == itemId))
				{
					return L($"from {L(mobData.Name)}"); // Found a mob that drops it
				}
			}

			// 2. Check if item comes from nodes on map
			if (ResourceNodeSpawnManager.TryGetSpawnPointsForMap(mapClassName, out var nodeSpawnsOnMap))
			{
				var nodeTypesOnMap = nodeSpawnsOnMap.Select(sp => sp.NodeTypeName).Distinct(StringComparer.OrdinalIgnoreCase);
				foreach (var nodeTypeName in nodeTypesOnMap)
				{
					if (ResourceNodeDataManager.TryGetData(nodeTypeName, out var nodeDef) && nodeDef.YieldItemId == itemId)
					{
						return $"from {nodeDef.NodeTypeName} nodes";
					}
				}
			}

			// 3. Fallback
			return L("somewhere in the area");
		}

	}
}
