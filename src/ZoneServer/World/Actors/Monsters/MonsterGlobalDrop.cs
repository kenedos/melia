using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone.World.Actors.Monsters
{
	public partial class Mob
	{
		/// <summary>
		/// Base chance for an item to drop from the global drop list.
		/// </summary>
		private static readonly Dictionary<ItemType, float> BaseGlobalDropChances = new()
		{
			{ ItemType.Equip, 0.0025f },    // 1 in 400
			{ ItemType.Consume, 0.001f },    // 1 in 1000
			{ ItemType.Recipe, 0.00166f },    // 1 in 600
			{ ItemType.Etc, 0.0025f },    // 1 in 400
		};

		/// <summary>
		/// Calculates the drop chance of an item type given a killer.
		/// baseDropChance will be the drop chance before item bonuses.
		/// adjustedDropChance will be the drop chance after all bonuses.
		/// </summary>
		private void GetGlobalDropChance(Character killer, ItemType type, out double dropChance, out double adjustedDropChance)
		{
			dropChance = BaseGlobalDropChances[type];

			// Other items drop more the higher level mob is.
			var level = Math.Max(0, this.Level - 40);
			var levelMultiplier = 1f + ((float)Math.Pow(level, 1.4f) / 200f);
			dropChance *= levelMultiplier;

			// Equips have additional drop bonus scaling inversely to
			// mob level. This guarantees players at lower levels get
			// the equips they need, even if normal grade.
			if (type == ItemType.Equip)
			{
				level = Math.Min(40, this.Level);
				// Linear interpolation:
				// multiplier = 3x at level 1,
				// multiplier = 1x at level 40 or more
				levelMultiplier = Math.Max(1.0f, 3.0f - (2.0f * (level - 1) / 39.0f));
				dropChance *= levelMultiplier;
			}

			// Each point of looting chance increases drop rate by 0.1%,
			// so 500 looting chance = 1.5x drop rate.
			var lootingChance = killer.Properties.GetFloat(PropertyName.LootingChance);
			var lootingRate = 1f + lootingChance * 0.001f;
			adjustedDropChance = dropChance * lootingRate;
		}

		private List<DropStack> GetGlobalDropStacks(Character killer)
		{
			var drops = new List<DropStack>();

			if (killer == null || killer.Map == null)
				return drops;

			if (killer.Map.Data?.Type == MapType.Dungeon || killer.Map.IsInstance)
				return drops;

			var isSuperMob = this.TryGetSuperMob(out var superMobType);
			var superMobRerolls = 0;
			var superMobItemThreshold = 0f;
			var worldConf = ZoneServer.Instance.Conf.World;

			// Set reroll counts and guaranteed thresholds based on mob type
			if (isSuperMob)
			{
				switch (superMobType)
				{
					case SuperMobType.Silver:
						superMobRerolls = worldConf.SilverJackpotRolls;
						superMobItemThreshold = worldConf.SilverJackpotGuaranteedItemThreshold / 100f;
						break;

					case SuperMobType.Gold:
						superMobRerolls = worldConf.GoldJackpotRolls;
						superMobItemThreshold = worldConf.GoldJackpotGuaranteedItemThreshold / 100f;
						break;

					case SuperMobType.Elite:
						superMobRerolls = worldConf.EliteRolls;
						superMobItemThreshold = worldConf.EliteGuaranteedItemThreshold / 100f;
						break;
				}
			}

			if (this.Rank == MonsterRank.Boss)
			{
				superMobRerolls = worldConf.BossRolls;
				superMobItemThreshold = worldConf.BossGuaranteedItemThreshold / 100f;
			}

			foreach (var type in BaseGlobalDropChances.Keys)
			{
				this.GetGlobalDropChance(killer, type, out var baseDropChance, out _);
				baseDropChance *= worldConf.GlobalDropRate / 100;

				var rerolls = worldConf.GlobalDropRolls;
				if (type == ItemType.Etc)
				{
					rerolls += (this.Level / 20) + 1;
				}
				if (worldConf.GlobalDropSuperMobItemReroll)
					rerolls += superMobRerolls;

				var guaranteed = false;
				if (worldConf.GlobalDropSuperMobItemThreshold)
					guaranteed = baseDropChance > superMobItemThreshold;

				do
				{
					rerolls--;

					var drop = this.TryGlobalDrop(killer, type, guaranteed);
					if (drop != null)
						drops.Add(drop);

					guaranteed = false;
				}
				while (rerolls > 0);
			}

			return drops;
		}

		/// <summary>
		/// Attempts to drop an item from the global drop list.
		/// </summary>
		private DropStack TryGlobalDrop(Character killer, ItemType type, bool guaranteed)
		{
			if (killer == null || killer.Map == null || killer.Map.Data == null)
				return null;

			if (ZoneServer.Instance.Data.GlobalDropDb.Count == 0)
				return null;

			if (killer?.Map?.Data?.Type == MapType.Dungeon || killer?.Map?.Data?.Type == MapType.Instance)
				return null;

			// Get list of items sorted by item level
			var sortedItems = ZoneServer.Instance.Data.GlobalDropDb.Entries.Values
				.Where(drop => drop.Item.Type == type && drop.Item.MinLevel <= this.Level)
				.OrderByDescending(drop => drop.Item.MinLevel)
				.ToList();

			if (sortedItems.Count == 0)
				return null;

			// Get highest level
			var highestLevel = sortedItems[0].Item.MinLevel;

			// We want to get 10 to 50 highest level items randomly, but if
			// items have the same item level, we always add them to our
			// eligible item list.
			var eligibleItems = new List<GlobalDropData>();
			var currentLevel = highestLevel;
			var targetCount = RandomProvider.Get().Next(10, 51);
			foreach (var item in sortedItems)
			{
				if (eligibleItems.Count >= targetCount && item.Item.MinLevel < currentLevel)
					break;

				eligibleItems.Add(item);
				currentLevel = item.Item.MinLevel;
			}

			// Gets the global drop chance for the given type
			this.GetGlobalDropChance(killer, type, out var typeDropChance, out var typeAdjustedDropChance);

			var rng = RandomProvider.Get().NextDouble();
			// Log.Debug($"Global Drop Chance {type} - {typeAdjustedDropChance}");

			// Checks if we dropped an item from this type
			if (!guaranteed && rng > typeAdjustedDropChance)
				return null;

			// Calculate total weight based on probability factors
			var totalWeight = eligibleItems.Sum(drop => drop.ProbabilityFactor);
			var randomNumber = RandomProvider.Get().NextDouble() * totalWeight;

			// Choose item based on weighted probability
			var chosenItem = eligibleItems.First(drop => (randomNumber -= drop.ProbabilityFactor) <= 0);

			// Calculate the specific item's drop chance considering the type drop chance
			// and its probability factor weight
			var itemWeight = chosenItem.ProbabilityFactor / totalWeight;
			var absoluteDropChance = typeDropChance * itemWeight;
			var absoluteAdjustedDropChance = typeAdjustedDropChance * itemWeight;
			var stack = new DropStack(chosenItem.Item.Id, 1, (float)absoluteDropChance, (float)absoluteAdjustedDropChance);

			// Log.Debug($"Global Drop {type} - {chosenItem.Id}");

			return stack;
		}
	}
}
