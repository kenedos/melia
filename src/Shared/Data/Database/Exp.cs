using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class BaseExpData
	{
		public int Level { get; set; }
		public long Exp { get; set; }
	}

	[Serializable]
	public class JobExpData
	{
		public int Rank { get; set; }
		public int Level { get; set; }
		public long Exp { get; set; }
	}

	public enum ExpType
	{
		Pet,
		Friend,
		Ancient,
	}

	/// <summary>
	/// Exp database.
	/// </summary>
	public class ExpDb : DatabaseJson<object>
	{
		private readonly List<BaseExpData> _exp = new List<BaseExpData>();
		private readonly List<JobExpData> _jobExp = new List<JobExpData>();
		private readonly List<BaseExpData> _guildExp = new List<BaseExpData>();
		private readonly List<BaseExpData> _petExp = new List<BaseExpData>();
		private readonly List<BaseExpData> _friendExp = new List<BaseExpData>();
		private readonly List<BaseExpData> _ancientExp = new List<BaseExpData>();

		// Precomputed lookup caches for job exp (built in AfterLoad)
		private int _highestJobRank;
		private Dictionary<int, int> _maxJobLevelByRank = new Dictionary<int, int>();
		private Dictionary<(int rank, int level), long> _totalJobExpByRankLevel = new Dictionary<(int, int), long>();

		public override void Clear()
		{
			_exp.Clear();
			_jobExp.Clear();
			_guildExp.Clear();
			_petExp.Clear();
			_ancientExp.Clear();
			_maxJobLevelByRank.Clear();
			_totalJobExpByRankLevel.Clear();
			_highestJobRank = 0;
			base.Clear();
		}

		/// <summary>
		/// Returns exp required to reach the next level after the
		/// given one.
		/// </summary>
		/// <remarks>
		/// Returns 0 if there's no data for the given level,
		/// i.e. if it's the last one or goes beyond.
		/// </remarks>
		/// <param name="level"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Thrown if level is invalid (< 1).</exception>
		public long GetNextExp(int level)
		{
			if (level < 1)
				throw new ArgumentException("Invalid level (too low).");

			if (level > _exp.Count)
				return 0;

			var index = level - 1;
			var exp = _exp[index];

			return exp.Exp;
		}

		/// <summary>
		/// Returns the EXP required to reach the given level.
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		public long GetTotalExp(int level)
		{
			var result = 0L;
			for (var i = 1; i < level; ++i)
				result += this.GetNextExp(i);

			return result;
		}

		/// <summary>
		/// Returns the total exp required to reach the next job level
		/// after the given level, in the given rank.
		/// </summary>
		/// <param name="rank"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		public long GetNextTotalJobExp(int rank, int level)
		{
			var maxLevel = this.GetMaxJobLevel(rank);

			if (level < 1 || level > maxLevel)
				throw new ArgumentException($"Invalid level {level} (expected: 1~{maxLevel}).");

			if (rank > _highestJobRank)
				rank = _highestJobRank;

			if (_totalJobExpByRankLevel.TryGetValue((rank, level), out var totalExp))
				return totalExp;

			throw new KeyNotFoundException("No job exp data found for rank '" + rank + "' and level '" + level + "'.");
		}

		/// <summary>
		/// Returns the max job level for the given rank.
		/// </summary>
		/// <param name="rank"></param>
		/// <returns></returns>
		public int GetMaxJobLevel(int rank)
		{
			if (rank > _highestJobRank)
				rank = _highestJobRank;

			if (_maxJobLevelByRank.TryGetValue(rank, out var maxLevel))
				return maxLevel;

			throw new KeyNotFoundException("No job exp data found for rank '" + rank + "'.");
		}

		/// <summary>
		/// Returns the max level.
		/// </summary>
		/// <returns></returns>
		public int GetMaxLevel()
		{
			return _exp.Where(a => a.Exp > 0).Max(a => a.Level);
		}

		/// <summary>
		/// Returns exp required to reach the next guild level after the
		/// given one.
		/// </summary>
		/// <remarks>
		/// Returns 0 if there's no data for the given guild level,
		/// i.e. if it's the last one or goes beyond.
		/// </remarks>
		/// <param name="level"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Thrown if level is invalid (< 1).</exception>
		public long GetNextGuildExp(int level)
		{
			if (level < 1)
				throw new ArgumentException("Invalid level (too low).");

			if (level > _guildExp.Count)
				return 0;

			var index = level - 1;
			var exp = _guildExp[index];

			return exp.Exp;
		}

		/// <summary>
		/// Returns the EXP required to reach the given guild level.
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		public long GetTotalGuildExp(int level)
		{
			var result = 0L;
			for (var i = 1; i < level; ++i)
				result += this.GetNextGuildExp(i);

			return result;
		}

		/// <summary>
		/// Reads given entry and adds it to the database.
		/// </summary>
		/// <remarks>
		/// Uses the JSON database to be fed its entries, but doesn't
		/// adhere to the Entries format and uses custom lists instead.
		/// </remarks>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			if (entry.ContainsKey("exp"))
				this.ReadExpEntry(entry);
			else if (entry.ContainsKey("jobExp"))
				this.ReadJobExpEntry(entry);
			else if (entry.ContainsKey("guildExp"))
				this.ReadGuildExpEntry(entry);
			else if (entry.ContainsKey("petExp"))
				this.ReadExpEntry(entry, "petExp", _petExp);
			else if (entry.ContainsKey("friendExp"))
				this.ReadExpEntry(entry, "friendExp", _friendExp);
			else if (entry.ContainsKey("ancientExp"))
				this.ReadExpEntry(entry, "ancientExp", _ancientExp);
			else
				throw new DatabaseErrorException("Unknown exp type.");
		}

		/// <summary>
		/// Reads given EXP entry.
		/// </summary>
		/// <param name="expEntry"></param>
		protected void ReadExpEntry(JObject expEntry)
		{
			foreach (JObject entry in expEntry["exp"])
			{
				entry.AssertNotMissing("level", "exp");

				var data = new BaseExpData();

				data.Level = entry.ReadInt("level");
				data.Exp = entry.ReadLong("exp");

				_exp.Add(data);
			}
		}

		/// <summary>
		/// Reads given EXP entry.
		/// </summary>
		/// <param name="expEntry"></param>
		protected void ReadExpEntry(JObject expEntry, string entryName, List<BaseExpData> exp)
		{
			foreach (JObject entry in expEntry[entryName])
			{
				entry.AssertNotMissing("level", "exp");

				var data = new BaseExpData();

				data.Level = entry.ReadInt("level");
				data.Exp = entry.ReadLong("exp");

				exp.Add(data);
			}
		}

		/// <summary>
		/// Reads given job EXP entry.
		/// </summary>
		/// <param name="jobExpEntry"></param>
		protected void ReadJobExpEntry(JObject jobExpEntry)
		{
			foreach (JObject entry in jobExpEntry["jobExp"])
			{
				entry.AssertNotMissing("rank", "level", "exp");

				var data = new JobExpData();

				data.Rank = entry.ReadInt("rank");
				data.Level = entry.ReadInt("level");
				data.Exp = entry.ReadLong("exp");

				_jobExp.Add(data);
			}
		}

		/// <summary>
		/// Reads given Guild EXP entry.
		/// </summary>
		/// <param name="guildExpEntry"></param>
		protected void ReadGuildExpEntry(JObject guildExpEntry)
		{
			foreach (JObject entry in guildExpEntry["guildExp"])
			{
				entry.AssertNotMissing("level", "exp");

				var data = new BaseExpData();

				data.Level = entry.ReadInt("level");
				data.Exp = entry.ReadLong("exp");

				_guildExp.Add(data);
			}
		}

		/// <summary>
		/// Called after loading, adds data to Entries so the servers
		/// can show that something was loaded.
		/// </summary>
		protected override void AfterLoad()
		{
			this.Entries.AddRange(_exp.Cast<object>());
			this.Entries.AddRange(_jobExp);
			this.Entries.AddRange(_guildExp);
			this.Entries.AddRange(_petExp);
			this.Entries.AddRange(_friendExp);
			this.Entries.AddRange(_ancientExp);

			this.BuildJobExpCaches();
		}

		/// <summary>
		/// Builds precomputed lookup caches for job exp data so that
		/// GetMaxJobLevel and GetNextTotalJobExp are O(1) lookups.
		/// </summary>
		private void BuildJobExpCaches()
		{
			if (_jobExp.Count == 0)
				return;

			_highestJobRank = 0;
			foreach (var entry in _jobExp)
			{
				if (entry.Rank > _highestJobRank)
					_highestJobRank = entry.Rank;
			}

			// Group by rank and compute max level + cumulative exp
			var byRank = new Dictionary<int, List<JobExpData>>();
			foreach (var entry in _jobExp)
			{
				if (!byRank.TryGetValue(entry.Rank, out var list))
				{
					list = new List<JobExpData>();
					byRank[entry.Rank] = list;
				}
				list.Add(entry);
			}

			foreach (var kvp in byRank)
			{
				var rank = kvp.Key;
				var entries = kvp.Value;

				// Find max level for this rank
				var maxLevel = 0;
				foreach (var entry in entries)
				{
					if (entry.Level > maxLevel)
						maxLevel = entry.Level;
				}
				_maxJobLevelByRank[rank] = maxLevel;

				// Sort by level and compute cumulative exp
				entries.Sort((a, b) => a.Level.CompareTo(b.Level));
				var cumulativeExp = 0L;
				foreach (var entry in entries)
				{
					cumulativeExp += entry.Exp;
					_totalJobExpByRankLevel[(rank, entry.Level)] = cumulativeExp;
				}
			}
		}

		/// <summary>
		/// Returns level based off exp.
		/// </summary>
		/// <remarks>
		/// Returns 0 if there's no data for the given exp,
		/// i.e. if it's the last one or goes beyond.
		/// </remarks>
		/// <param name="exp"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Thrown if exp is invalid (< 0).</exception>
		public int GetLevel(ExpType type, int exp)
		{
			List<BaseExpData> expList;
			switch (type)
			{
				case ExpType.Friend:
					expList = _friendExp;
					break;
				case ExpType.Ancient:
					expList = _ancientExp;
					break;
				default:
					expList = _petExp;
					break;
			}
			if (exp < 0)
				throw new ArgumentException("Invalid exp (too low).");

			if (exp > expList.Count)
				return 0;

			var expData = expList.Find(a => a.Exp >= exp && exp <= a.Exp);

			return expData?.Level ?? 0;
		}

		/// <summary>
		/// Returns exp required to reach the next level after the
		/// given one.
		/// </summary>
		/// <remarks>
		/// Returns 0 if there's no data for the given level,
		/// i.e. if it's the last one or goes beyond.
		/// </remarks>
		/// <param name="level"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Thrown if level is invalid (< 1).</exception>
		public long GetNextExp(ExpType type, int level)
		{
			List<BaseExpData> expList;
			switch (type)
			{
				case ExpType.Friend:
					expList = _friendExp;
					break;
				case ExpType.Ancient:
					expList = _ancientExp;
					break;
				default:
					expList = _petExp;
					break;
			}
			if (level < 1)
				throw new ArgumentException("Invalid level (too low).");

			if (level > expList.Count)
				return 0;

			var index = level - 1;
			var exp = expList[index];

			return exp.Exp;
		}

		/// <summary>
		/// Returns the EXP required to reach the given level.
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		public long GetTotalExp(ExpType type, int level)
		{
			var result = 0L;
			for (var i = 1; i < level; ++i)
				result += this.GetNextExp(type, i);

			return result;
		}
	}
}
