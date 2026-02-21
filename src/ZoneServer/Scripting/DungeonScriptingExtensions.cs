using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Dungeons;
using Melia.Zone.World.Dungeons.Stages;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;

namespace Melia.Zone.Scripting
{
	/// <summary>
	/// Contains implementations of monster ranking scripts for dynamic property calculation.
	/// </summary>
	/// <remarks>Probably rework this to use stat calculations in Zone Scripts</remarks>
	public class DungeonRankCalculator
	{
		private readonly Lazy<PartyStats> _stats;
		private readonly InstanceDungeon _instance;

		public DungeonRankCalculator(InstanceDungeon instance)
		{
			_instance = instance;
			_stats = new Lazy<PartyStats>(() => new PartyStats(instance.Characters));
		}

		/// <summary> Caches calculated party statistics for the dungeon instance. </summary>
		private class PartyStats
		{
			public int MinLevel { get; }
			public int MaxLevel { get; }
			public double AverageLevel { get; }
			public int PlayerCount { get; }

			public PartyStats(List<Character> characters)
			{
				var validChars = characters?.Where(c => c != null).ToList() ?? new List<Character>();
				this.PlayerCount = validChars.Count;

				if (PlayerCount == 0) return;

				long totalLevel = 0;
				this.MinLevel = int.MaxValue;
				this.MaxLevel = 0;

				foreach (var character in validChars)
				{
					totalLevel += character.Level;
					if (character.Level > this.MaxLevel) this.MaxLevel = character.Level;
					if (character.Level < this.MinLevel) this.MinLevel = character.Level;
				}
				this.AverageLevel = (double)totalLevel / PlayerCount;
			}
		}

		/// <summary> Calculates the monster rank level for normal difficulty. </summary>
		public int GetMonsterRankLevelNormal1()
		{
			var s = _stats.Value;
			if (s.PlayerCount == 0) return 1;

			var ret = s.AverageLevel;
			if (s.PlayerCount > 1)
			{
				ret = s.AverageLevel - Math.Floor((s.MaxLevel - s.MinLevel) * (1.0 / (s.PlayerCount + 7)));
			}

			if (ret > s.MaxLevel) ret = s.MaxLevel;
			if (s.MaxLevel - 30 > ret) ret = s.MaxLevel - 30;

			return (int)Math.Floor(ret);
		}

		/// <summary> Calculates the uphill level for normal difficulty. </summary>
		public int GetUphillLevelNormal1()
		{
			var s = _stats.Value;
			if (s.PlayerCount == 0) return 1;

			var step = _instance.Vars.GetInt("UphillStep", 1);

			var ret = s.AverageLevel;
			if (s.PlayerCount > 1)
			{
				ret = s.MaxLevel;
			}

			if (ret > s.MaxLevel) ret = s.MaxLevel;

			int addLevel = 0;
			if (step > 0)
			{
				addLevel = (step < 30) ? (int)Math.Floor((step - 1) / 3.0) : 10;
			}

			ret += addLevel;

			return (int)Math.Floor(ret) + addLevel;
		}

		/// <summary> Calculates the uphill MHP defense value. </summary>
		public int GetUphillMhpDeff1()
		{
			var s = _stats.Value;
			var value = 91433;
			if (s.MaxLevel > 120)
			{
				value += (s.MaxLevel - 120) * 919;
			}
			return value;
		}
	}

	/// <summary>
	/// Defines a set of stat multipliers for a specific dungeon difficulty.
	/// </summary>
	public class DifficultyProfile
	{
		public string Name { get; set; }
		public float HpMultiplier { get; set; } = 1.0f;
		public float AttackMultiplier { get; set; } = 1.0f;
		public float DefenseMultiplier { get; set; } = 1.0f;

		public static DifficultyProfile Normal { get; } = new DifficultyProfile { Name = "Normal" };
		public static DifficultyProfile Hard { get; } = new DifficultyProfile { Name = "Hard", HpMultiplier = 1.5f, AttackMultiplier = 1.3f };
		public static DifficultyProfile VeryHard { get; } = new DifficultyProfile { Name = "Very Hard", HpMultiplier = 2.0f, AttackMultiplier = 1.5f, DefenseMultiplier = 1.2f };
	}
}
