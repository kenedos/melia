// ===================================================================
// Character.Exploration.cs - Map exploration and fog of war rewards
// ===================================================================
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Spawning;
using Yggdrasil.Logging;

namespace Melia.Zone.World.Actors.Characters
{
	public partial class Character
	{
		// Reward Tiers Configuration for Map Exploration
		// Format: (MinLevel, MaxLevel, ItemId, Amount)
		private static readonly (int MinLevel, int MaxLevel, int ItemId, int Amount)[] ExplorationRewardTiers =
		{
			(1, 10, 640080, 2),
			(11, 20, 640081, 1),
			(21, 30, 640081, 2),
			(31, 40, 640082, 1),
			(41, 50, 640084, 1),
			(51, 60, 640085, 1),
			(61, 70, 640086, 1),
			(71, 80, 640113, 1),
			(81, 999, 640114, 1),
		};

		private const float ExplorationRewardThreshold = 90f;

		/// <summary>
		/// Attempts to give an exploration reward if the player has explored
		/// enough of the map and hasn't already received the reward.
		/// </summary>
		/// <param name="mapId">The map ID being explored.</param>
		/// <param name="percentage">The current exploration percentage.</param>
		/// <returns>True if a reward was given, false otherwise.</returns>
		public bool TryGiveMapExplorationReward(int mapId, float percentage)
		{
			if (HasReceivedExplorationReward(mapId))
				return false;

			if (percentage < ExplorationRewardThreshold)
				return false;

			var rewardKey = $"Melia.MapExploration.Reward.{mapId}";

			// Check if already rewarded for this map
			if (this.Variables.Perm.GetBool(rewardKey))
				return false;

			// Get average level of mobs from spawners in the map
			var averageLevel = this.GetMapAverageMonsterLevel(mapId);
			if (averageLevel <= 0)
				return false;

			// Find the appropriate reward tier
			if (!this.TryGetExplorationRewardTier(averageLevel, out var rewardItemId, out var rewardAmount))
				return false;

			// Mark as rewarded before giving (prevents double rewards on rapid packets)
			this.Variables.Perm.SetBool(rewardKey, true);

			// Give the reward
			this.Inventory.Add(rewardItemId, rewardAmount, InventoryAddType.PickUp);

			// Notify the player
			var message = percentage >= 100
				? "Map fully explored! Received exploration reward!"
				: $"Map exploration reached {percentage:F0}%! Received exploration reward!";

			this.AddonMessage("NOTICE_Dm_Clear", message, 8);

			return true;
		}

		/// <summary>
		/// Returns whether the character has already received the exploration
		/// reward for the specified map.
		/// </summary>
		/// <param name="mapId">The map ID to check.</param>
		/// <returns>True if reward was already received.</returns>
		public bool HasReceivedExplorationReward(int mapId)
		{
			var rewardKey = $"Melia.MapExploration.Reward.{mapId}";
			return this.Variables.Perm.GetBool(rewardKey);
		}

		/// <summary>
		/// Gets the average level of monsters from spawners in the specified map.
		/// Only includes Normal and Elite monsters, excluding bosses, root crystals, etc.
		/// </summary>
		/// <param name="mapId">The map ID to check.</param>
		/// <returns>The average monster level, or 0 if no spawners found.</returns>
		private int GetMapAverageMonsterLevel(int mapId)
		{
			var spawners = ZoneServer.Instance.World.GetSpawners()
				.OfType<MonsterSpawner>()
				.Where(s => s.Maps.Contains(mapId) && s.MonsterData != null)
				.Where(s => s.MonsterData.Rank == MonsterRank.Normal || s.MonsterData.Rank == MonsterRank.Elite)
				.ToList();

			if (spawners.Count == 0)
				return 0;

			var totalLevel = spawners.Sum(s => s.MonsterData.Level);
			return totalLevel / spawners.Count;
		}

		/// <summary>
		/// Gets the appropriate reward tier for the given average monster level.
		/// </summary>
		/// <param name="averageLevel">The average monster level.</param>
		/// <param name="itemId">The reward item ID.</param>
		/// <param name="amount">The reward amount.</param>
		/// <returns>True if a matching tier was found.</returns>
		private bool TryGetExplorationRewardTier(int averageLevel, out int itemId, out int amount)
		{
			foreach (var tier in ExplorationRewardTiers)
			{
				if (averageLevel >= tier.MinLevel && averageLevel <= tier.MaxLevel)
				{
					itemId = tier.ItemId;
					amount = tier.Amount;
					return true;
				}
			}

			itemId = 0;
			amount = 0;
			return false;
		}
	}
}
