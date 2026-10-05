using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Yggdrasil.Logging;

namespace Melia.Zone.World.Actors.Characters.Components
{
	/// <summary>
	/// Adventure Book
	/// </summary>
	/// <remarks>
	/// WIP: Salman - Still need to figure out some of the adventure book info structures.
	/// </remarks>
	public class AdventureBookComponent : CharacterComponent
	{
		private readonly SortedList<int, int> _monstersKilled = new();
		private readonly SortedList<int, SortedList<int, int>> _monstersDrops = new();
		private readonly SortedList<int, AdventureBookItemEntry> _items = new();
		private readonly SortedList<int, int> _fishing = new();
		private readonly SortedList<int, int> _jobs = new();
		private readonly SortedList<int, int> _dungeons = new();
		private readonly SortedList<int, int> _personalShops = new();
		public AdventureBookComponent(Character character) : base(character)
		{
		}
		public SortedList<int, int> GetList(AdventureBookType type)
		{
			switch (type)
			{
				case AdventureBookType.MonsterKilled:
					lock (_monstersKilled) return _monstersKilled;
				case AdventureBookType.Fishing:
					lock (_fishing) return _fishing;
				case AdventureBookType.Job:
					lock (_jobs) return _jobs;
				case AdventureBookType.Dungeon:
					lock (_dungeons) return _dungeons;
				case AdventureBookType.PersonalShop:
					lock (_personalShops) return _personalShops;
				default:
					return (SortedList<int, int>)Enumerable.Empty<SortedList<int, int>>();
			}
		}

		/// <summary>
		/// Returns a thread-safe snapshot of the specified list for database operations.
		/// </summary>
		public KeyValuePair<int, int>[] GetListSnapshot(AdventureBookType type)
		{
			switch (type)
			{
				case AdventureBookType.MonsterKilled:
					lock (_monstersKilled) return _monstersKilled.ToArray();
				case AdventureBookType.Fishing:
					lock (_fishing) return _fishing.ToArray();
				case AdventureBookType.Job:
					lock (_jobs) return _jobs.ToArray();
				case AdventureBookType.Dungeon:
					lock (_dungeons) return _dungeons.ToArray();
				case AdventureBookType.PersonalShop:
					lock (_personalShops) return _personalShops.ToArray();
				default:
					return Array.Empty<KeyValuePair<int, int>>();
			}
		}

		/// <summary>
		/// Returns the item entries.
		/// </summary>
		/// <returns></returns>
		public SortedList<int, AdventureBookItemEntry> GetItems()
		{
			lock (_items)
				return _items;
		}

		/// <summary>
		/// Returns a thread-safe snapshot of item entries for database operations.
		/// </summary>
		public KeyValuePair<int, AdventureBookItemEntry>[] GetItemsSnapshot()
		{
			lock (_items)
				return _items.ToArray();
		}

		/// <summary>
		/// Returns monster drops
		/// </summary>
		/// <returns></returns>
		public SortedList<int, SortedList<int, int>> GetMonsterDrops()
		{
			lock (_monstersDrops)
				return _monstersDrops;
		}

		/// <summary>
		/// Returns a thread-safe snapshot of monster drops for database operations.
		/// </summary>
		public Dictionary<int, KeyValuePair<int, int>[]> GetMonsterDropsSnapshot()
		{
			lock (_monstersDrops)
			{
				var result = new Dictionary<int, KeyValuePair<int, int>[]>(_monstersDrops.Count);
				foreach (var kvp in _monstersDrops)
					result[kvp.Key] = kvp.Value.ToArray();
				return result;
			}
		}

		/// <summary>
		/// Checks if an entry doesn't exist already.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool IsNewEntry(AdventureBookType type, int id)
		{
			switch (type)
			{
				case AdventureBookType.MonsterKilled:
					lock (_monstersKilled) return !_monstersKilled.ContainsKey(id);
				case AdventureBookType.Fishing:
					lock (_fishing) return !_fishing.ContainsKey(id);
				case AdventureBookType.Job:
					lock (_jobs) return !_jobs.ContainsKey(id);
				case AdventureBookType.Dungeon:
					lock (_dungeons) return !_dungeons.ContainsKey(id);
				case AdventureBookType.PersonalShop:
					lock (_personalShops) return !_personalShops.ContainsKey(id);
				case AdventureBookType.ItemObtained:
				case AdventureBookType.ItemCrafted:
				case AdventureBookType.ItemUsed:
					lock (_items) return !_items.ContainsKey(id);
			}

			return true;
		}

		public void AddJob(int id, int amount = 1, bool silently = false)
		{
			lock (_jobs)
			{
				if (!_jobs.ContainsKey(id))
					_jobs.Add(id, amount);
				else
					_jobs[id] += amount;
			}

			if (!silently)
				this.UpdateJobCountProperty();
		}

		public void AddDungeon(int id, int amount = 1, bool silently = false)
		{
			lock (_dungeons)
			{
				if (!_dungeons.ContainsKey(id))
					_dungeons.Add(id, amount);
				else
					_dungeons[id] += amount;

				if (!silently)
					Send.ZC_ADVENTURE_BOOK_INFO(this.Character, AdventureBookType.Dungeon, _dungeons);
			}
		}

		public void AddPersonalShop(int id, int amount, bool silently = false)
		{
			lock (_personalShops)
			{
				if (!_personalShops.ContainsKey(id))
					_personalShops.Add(id, amount);
				else
					_personalShops[id] += amount;
			}
		}

		public void UpdateJobCountProperty()
		{
			int count;
			lock (_jobs)
				count = _jobs.Count;
			this.Character.SetAccountProperty("AdventureBookJobCount", count);
		}

		public void SynchronizeJobCountProperty()
		{
			int count;
			lock (_jobs)
				count = _jobs.Count;
			this.Character.SetAccountProperty("AdventureBookJobCount", count);
		}

		public void AddMonsterKill(int id, int amount = 1, bool silently = false)
		{
			lock (_monstersKilled)
			{
				if (!_monstersKilled.ContainsKey(id))
					_monstersKilled.Add(id, amount);
				else
					_monstersKilled[id] += amount;

				if (!silently)
					Send.ZC_ADVENTURE_BOOK_INFO(this.Character, AdventureBookType.MonsterKilled, _monstersKilled);
			}
		}

		public void AddItem(int itemId, int craftCount, int obtainCount, int useCount)
		{
			lock (_items)
			{
				if (!_items.TryGetValue(itemId, out var value))
					_items.Add(itemId, new AdventureBookItemEntry()
					{
						ItemId = itemId,
						CraftedCount = craftCount,
						ObtainedCount = obtainCount,
						UsedCount = useCount
					});
				else
				{
					value.CraftedCount = craftCount;
					value.ObtainedCount = obtainCount;
					value.UsedCount = useCount;
				}
			}
		}

		public void AddItemCrafted(int id, int amount, bool silently = false)
		{
			lock (_items)
			{
				if (!_items.TryGetValue(id, out var value))
					_items.Add(id, new AdventureBookItemEntry()
					{
						ItemId = id,
						CraftedCount = amount
					});
				else
					value.CraftedCount += amount;

				if (!silently)
					Send.ZC_ADVENTURE_BOOK_INFO(this.Character, AdventureBookType.ItemCrafted, _items);
			}
		}

		public void AddItemObtained(int id, int amount, bool silently = false)
		{
			lock (_items)
			{
				if (!_items.TryGetValue(id, out var value))
					_items.Add(id, new AdventureBookItemEntry()
					{
						ItemId = id,
						ObtainedCount = amount
					});
				else
					value.ObtainedCount += amount;

				if (!silently)
					Send.ZC_ADVENTURE_BOOK_INFO(this.Character, AdventureBookType.ItemObtained, _items);
			}
		}

		public void AddItemUsed(int id, int amount, bool silently = false)
		{
			lock (_items)
			{
				if (!_items.TryGetValue(id, out var value))
					_items.Add(id, new AdventureBookItemEntry()
					{
						ItemId = id,
						UsedCount = amount
					});
				else
					value.UsedCount += amount;

				if (!silently)
					Send.ZC_ADVENTURE_BOOK_INFO(this.Character, AdventureBookType.ItemUsed, _items);
			}
		}
		public void AddMonsterDrop(int monsterId, int id, int amount, bool silently = false)
		{
			lock (_monstersDrops)
			{
				if (!_monstersDrops.ContainsKey(monsterId))
					_monstersDrops.Add(monsterId, new SortedList<int, int>());
				if (!_monstersDrops[monsterId].ContainsKey(id))
					_monstersDrops[monsterId].Add(id, amount);
				else
					_monstersDrops[monsterId][id] += amount;

				if (!silently)
					Send.ZC_ADVENTURE_BOOK_INFO(this.Character, AdventureBookType.MonsterDrop, _monstersDrops);
			}
		}

		/// <summary>
		/// Updates the client with all adventure book info.
		/// </summary>
		public void UpdateClient()
		{
			lock (_monstersKilled)
				Send.ZC_ADVENTURE_BOOK_INFO(this.Character, AdventureBookType.MonsterKilled, _monstersKilled);
			lock (_monstersDrops)
				Send.ZC_ADVENTURE_BOOK_INFO(this.Character, AdventureBookType.MonsterDrop, _monstersDrops);
			lock (_items)
				Send.ZC_ADVENTURE_BOOK_INFO(this.Character, AdventureBookType.ItemObtained, _items);
			lock (_dungeons)
				Send.ZC_ADVENTURE_BOOK_INFO(this.Character, AdventureBookType.Dungeon, _dungeons);
		}
	}

	public class AdventureBookItemEntry
	{
		public int ItemId { get; set; }
		public int CraftedCount { get; set; }
		public int UsedCount { get; set; }
		public int ObtainedCount { get; set; }
	}
}
