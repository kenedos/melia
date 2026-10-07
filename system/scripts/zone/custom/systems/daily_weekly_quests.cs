//--- Melia Script ----------------------------------------------------------
// Daily and Weekly Quests
//--- Description -----------------------------------------------------------
// Two quest boards that hand out hunting objectives (monster ranks, races,
// attributes, sizes, regions and dungeon clears) which reset with the
// instanced dungeon reset and pay out Wings of Vaivora Coins.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Zone;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class CustomDailyWeeklyQuestsScript : GeneralScript
{
	private const int RewardItemId = ItemId.Misc_0533;

	private static readonly RaceType[] Races = { RaceType.Klaida, RaceType.Paramune, RaceType.Forester, RaceType.Velnias, RaceType.Widling };
	private static readonly AttributeType[] Attributes = { AttributeType.Fire, AttributeType.Ice, AttributeType.Lightning, AttributeType.Earth, AttributeType.Poison, AttributeType.Holy, AttributeType.Dark, AttributeType.Soul };
	private static readonly SizeType[] Sizes = { SizeType.S, SizeType.M, SizeType.L };

	private static readonly QuestBoard Daily = new("Melia.DailyQuests.", weekly: false);
	private static readonly QuestBoard Weekly = new("Melia.WeeklyQuests.", weekly: true);

	protected override void Load()
	{
		if (!Feature.IsEnabled("CustomNpcs"))
			return;

		AddNpc(57223, L("[Daily Quests] Vitor"), "c_Klaipe", -590, 890, 0, this.DailyDialog);
		AddNpc(57223, L("[Weekly Quests] Joao"), "c_Klaipe", -700, 890, 0, this.WeeklyDialog);
	}

	[On("EntityKilled")]
	private void OnEntityKilled(object sender, CombatEventArgs args)
	{
		if (args.Target is not Mob mob || mob.MonsterType != RelationType.Enemy || mob.OwnerHandle != 0)
			return;

		var killer = mob.GetKillBeneficiary(args.Attacker);
		if (killer == null)
			return;

		foreach (var character in GetSharingCharacters(killer))
		{
			Daily.AddKill(character, mob);
			Weekly.AddKill(character, mob);
		}
	}

	[On("PlayerClearedDungeon")]
	private void OnPlayerClearedDungeon(object sender, PlayerDungeonEventArgs args)
	{
		Daily.AddProgress(args.Character, TaskKind.Dungeon, 0);
		Weekly.AddProgress(args.Character, TaskKind.Dungeon, 0);
	}

	private async Task DailyDialog(Dialog dialog)
	{
		var character = dialog.Player;
		dialog.SetTitle(L("Daily Quests"));

		Daily.ResetIfExpired(character);

		if (!Daily.IsActive(character))
		{
			var difficulty = await dialog.Select(L("Every day I hand out a fresh set of hunting objectives. Pick how hard you want today's to be."),
				Option(L("Easy"), "1"),
				Option(L("Medium"), "2"),
				Option(L("Hard"), "3"),
				Option(L("Not now"), "exit"));

			if (difficulty == "exit")
				return;

			Daily.Generate(character, int.Parse(difficulty));
		}

		while (true)
		{
			var tasks = Daily.GetTasks(character);
			var selection = await dialog.Select(FormatTasks(L("Today's objectives:"), tasks, 0, tasks.Count),
				Option(LF("Claim rewards ({0})", Daily.CountClaimable(character)), "claim"),
				Option(Daily.GetBonusText(character), "bonus"),
				Option(L("When do they reset?"), "reset"),
				Option(L("Leave"), "exit"));

			if (selection == "exit")
				return;

			if (selection == "reset")
				await dialog.Msg(LF("Daily objectives reset every day at {0}.", FormatResetTime()));
			else
				await this.Claim(dialog, Daily, selection == "bonus");
		}
	}

	private async Task WeeklyDialog(Dialog dialog)
	{
		var character = dialog.Player;
		dialog.SetTitle(L("Weekly Quests"));

		Weekly.ResetIfExpired(character);

		if (!Weekly.IsActive(character))
		{
			var accept = await dialog.Select(L("Weekly objectives are a long hunt: thousands of monsters of every kind, regional sweeps and dungeon clears. Will you take them on?"),
				Option(L("Accept the weekly objectives"), "accept"),
				Option(L("Not now"), "exit"));

			if (accept == "exit")
				return;

			Weekly.Generate(character, 0);
		}

		while (true)
		{
			var tasks = Weekly.GetTasks(character);
			var selection = await dialog.Select(LF("Completed objectives: {0}/{1}", tasks.Count(a => a.Done), tasks.Count),
				Option(L("General objectives"), "general"),
				Option(L("Races"), "races"),
				Option(L("Attributes"), "attributes"),
				Option(L("Sizes"), "sizes"),
				Option(L("Regions"), "regions"),
				Option(LF("Claim rewards ({0})", Weekly.CountClaimable(character)), "claim"),
				Option(L("When do they reset?"), "reset"),
				Option(L("Leave"), "exit"));

			switch (selection)
			{
				case "exit":
					return;

				case "reset":
					await dialog.Msg(LF("Weekly objectives reset every {0} at {1}.", ZoneServer.Instance.Conf.World.InstancedDungeonWeeklyResetDay, FormatResetTime()));
					break;

				case "claim":
					await this.Claim(dialog, Weekly, false);
					break;

				default:
					var kinds = selection switch
					{
						"races" => new[] { TaskKind.Race },
						"attributes" => new[] { TaskKind.Attribute },
						"sizes" => new[] { TaskKind.Size },
						"regions" => new[] { TaskKind.Map },
						_ => new[] { TaskKind.Monster, TaskKind.Elite, TaskKind.Boss, TaskKind.Mythic, TaskKind.Dungeon },
					};
					var shown = tasks.Where(a => kinds.Contains(a.Kind)).ToList();
					await dialog.Msg(FormatTasks(L("Objectives:"), shown, 0, shown.Count));
					break;
			}
		}
	}

	private async Task Claim(Dialog dialog, QuestBoard board, bool bonus)
	{
		var character = dialog.Player;

		if (bonus)
		{
			var amount = board.ClaimBonus(character);
			if (amount == 0)
				await dialog.Msg(L("The completion bonus is paid once every objective's reward has been claimed."));
			else
				await dialog.Msg(LF("Well done! Here are {0} more coins for finishing everything.", amount));
			return;
		}

		var coins = board.ClaimRewards(character);
		if (coins == 0)
			await dialog.Msg(L("You have no finished objectives to claim yet."));
		else
			await dialog.Msg(LF("Here's your reward: {0} Wings of Vaivora Coins.", coins));
	}

	private static string FormatTasks(string header, List<QuestTask> tasks, int start, int count)
	{
		var sb = new StringBuilder(header);

		foreach (var task in tasks.Skip(start).Take(count))
		{
			var state = task.Claimed ? L("claimed") : task.Done ? L("done") : $"{task.Progress}/{task.Target}";
			sb.Append("{nl}").Append(GetTaskName(task)).Append(": ").Append(state);
		}

		return sb.ToString();
	}

	private static string GetTaskName(QuestTask task)
	{
		return task.Kind switch
		{
			TaskKind.Monster => L("Defeat monsters"),
			TaskKind.Elite => L("Defeat elite monsters"),
			TaskKind.Boss => L("Defeat field bosses"),
			TaskKind.Mythic => L("Defeat mythic monsters"),
			TaskKind.Dungeon => L("Clear dungeons"),
			TaskKind.Map => LF("Defeat monsters in {0}", ZoneServer.Instance.Data.MapDb.Entries.TryGetValue(task.Param, out var map) ? map.Name : "?"),
			TaskKind.Race => LF("Defeat {0} monsters", (RaceType)task.Param),
			TaskKind.Attribute => LF("Defeat {0} monsters", (AttributeType)task.Param),
			TaskKind.Size => LF("Defeat size {0} monsters", (SizeType)task.Param),
			_ => "?",
		};
	}

	private static string FormatResetTime()
	{
		var conf = ZoneServer.Instance.Conf.World;
		return $"{conf.InstancedDungeonResetHour:00}:{conf.InstancedDungeonResetMinute:00}";
	}

	private static List<Character> GetSharingCharacters(Character killer)
	{
		var result = new List<Character> { killer };
		var conf = ZoneServer.Instance.Conf.World;
		var party = killer.Connection?.Party;

		if (!conf.PartyQuestSharingEnabled || !conf.PartyShareKillObjectives || party == null || party.QuestSharing != PartyQuestSharing.Enabled)
			return result;

		var members = conf.PartyQuestSharingRange <= 0 ? killer.Map.GetPartyMembers(killer) : killer.Map.GetPartyMembersInRange(killer, conf.PartyQuestSharingRange);
		result.AddRange(members.Where(a => a != killer));

		return result;
	}

	private static bool Matches(QuestTask task, Mob mob)
	{
		var mythic = mob.IsMythicMonster();
		var elite = mob.Rank == MonsterRank.Elite || mob.IsBuffActive(BuffId.EliteMonsterBuff);

		return task.Kind switch
		{
			TaskKind.Monster => !mythic && !elite && mob.Rank != MonsterRank.Boss,
			TaskKind.Elite => !mythic && elite,
			TaskKind.Boss => !mythic && mob.Rank == MonsterRank.Boss,
			TaskKind.Mythic => mythic,
			TaskKind.Map => mob.Map.Id == task.Param,
			TaskKind.Race => (int)mob.Race == task.Param,
			TaskKind.Attribute => (int)mob.Attribute == task.Param,
			TaskKind.Size => (int)mob.Data.Size == task.Param,
			_ => false,
		};
	}

	private enum TaskKind { Monster, Elite, Boss, Mythic, Dungeon, Map, Race, Attribute, Size }

	private class QuestTask
	{
		public TaskKind Kind;
		public int Param;
		public int Target;
		public int Progress;
		public bool Claimed;
		public int Reward;

		public bool Done => this.Progress >= this.Target;
	}

	/// <summary>
	/// A set of objectives saved in a character's permanent variables.
	/// </summary>
	private class QuestBoard
	{
		private readonly string _prefix;
		private readonly bool _weekly;

		public QuestBoard(string prefix, bool weekly)
		{
			_prefix = prefix;
			_weekly = weekly;
		}

		public bool IsActive(Character character)
			=> character.Variables.Perm.GetInt(_prefix + "Count", 0) > 0;

		/// <summary>
		/// Clears the board if it was generated before the last reset.
		/// </summary>
		public void ResetIfExpired(Character character)
		{
			var vars = character.Variables.Perm;
			if (vars.GetLong(_prefix + "Period", 0) >= this.GetPeriodStart().Ticks)
				return;

			var count = vars.GetInt(_prefix + "Count", 0);
			for (var i = 0; i < count; i++)
			{
				foreach (var field in new[] { "Kind", "Param", "Target", "Progress", "Claimed", "Reward" })
					vars.Remove($"{_prefix}{i}.{field}");
			}

			vars.Remove(_prefix + "Count");
			vars.Remove(_prefix + "BonusClaimed");
			vars.Remove(_prefix + "Difficulty");
		}

		/// <summary>
		/// Creates a new set of objectives for the character.
		/// </summary>
		public void Generate(Character character, int difficulty)
		{
			var tasks = _weekly ? this.CreateWeeklyTasks(character) : this.CreateDailyTasks(character, difficulty);
			var vars = character.Variables.Perm;

			vars.SetLong(_prefix + "Period", this.GetPeriodStart().Ticks);
			vars.SetInt(_prefix + "Difficulty", difficulty);
			vars.SetInt(_prefix + "Count", tasks.Count);

			for (var i = 0; i < tasks.Count; i++)
				this.Save(character, i, tasks[i]);
		}

		public List<QuestTask> GetTasks(Character character)
		{
			var vars = character.Variables.Perm;
			var count = vars.GetInt(_prefix + "Count", 0);
			var result = new List<QuestTask>(count);

			for (var i = 0; i < count; i++)
			{
				result.Add(new QuestTask
				{
					Kind = (TaskKind)vars.GetInt($"{_prefix}{i}.Kind"),
					Param = vars.GetInt($"{_prefix}{i}.Param"),
					Target = vars.GetInt($"{_prefix}{i}.Target"),
					Progress = vars.GetInt($"{_prefix}{i}.Progress"),
					Claimed = vars.GetBool($"{_prefix}{i}.Claimed"),
					Reward = vars.GetInt($"{_prefix}{i}.Reward"),
				});
			}

			return result;
		}

		public void AddKill(Character character, Mob mob)
		{
			if (!this.IsActive(character))
				return;

			var tasks = this.GetTasks(character);
			for (var i = 0; i < tasks.Count; i++)
			{
				if (tasks[i].Kind != TaskKind.Dungeon && !tasks[i].Done && Matches(tasks[i], mob))
					this.Advance(character, i, tasks[i]);
			}
		}

		public void AddProgress(Character character, TaskKind kind, int param)
		{
			if (!this.IsActive(character))
				return;

			var tasks = this.GetTasks(character);
			for (var i = 0; i < tasks.Count; i++)
			{
				if (tasks[i].Kind == kind && tasks[i].Param == param && !tasks[i].Done)
					this.Advance(character, i, tasks[i]);
			}
		}

		public int CountClaimable(Character character)
			=> this.GetTasks(character).Count(a => a.Done && !a.Claimed);

		/// <summary>
		/// Pays out every finished, unclaimed objective and returns the
		/// number of coins given.
		/// </summary>
		public int ClaimRewards(Character character)
		{
			var tasks = this.GetTasks(character);
			var total = 0;

			for (var i = 0; i < tasks.Count; i++)
			{
				if (!tasks[i].Done || tasks[i].Claimed)
					continue;

				character.Variables.Perm.SetBool($"{_prefix}{i}.Claimed", true);
				total += tasks[i].Reward;
			}

			if (total > 0)
				character.AddItem(RewardItemId, total);

			return total;
		}

		public string GetBonusText(Character character)
		{
			if (character.Variables.Perm.GetBool(_prefix + "BonusClaimed"))
				return L("Completion bonus (claimed)");

			return LF("Completion bonus ({0} coins)", this.GetBonus(character));
		}

		/// <summary>
		/// Pays the completion bonus once every reward was claimed and
		/// returns its amount, or 0.
		/// </summary>
		public int ClaimBonus(Character character)
		{
			var vars = character.Variables.Perm;
			var tasks = this.GetTasks(character);

			if (vars.GetBool(_prefix + "BonusClaimed") || tasks.Count == 0 || tasks.Any(a => !a.Claimed))
				return 0;

			var bonus = this.GetBonus(character);
			vars.SetBool(_prefix + "BonusClaimed", true);
			character.AddItem(RewardItemId, bonus);

			return bonus;
		}

		private int GetBonus(Character character)
			=> 3 * (1 << Math.Max(0, character.Variables.Perm.GetInt(_prefix + "Difficulty", 1) - 1));

		private void Advance(Character character, int index, QuestTask task)
		{
			task.Progress++;
			character.Variables.Perm.SetInt($"{_prefix}{index}.Progress", task.Progress);

			if (task.Done)
				character.ServerMessage(LF("{0} objective complete: {1}", _weekly ? L("Weekly") : L("Daily"), GetTaskName(task)));
		}

		private void Save(Character character, int index, QuestTask task)
		{
			var vars = character.Variables.Perm;
			vars.SetInt($"{_prefix}{index}.Kind", (int)task.Kind);
			vars.SetInt($"{_prefix}{index}.Param", task.Param);
			vars.SetInt($"{_prefix}{index}.Target", task.Target);
			vars.SetInt($"{_prefix}{index}.Progress", 0);
			vars.SetBool($"{_prefix}{index}.Claimed", false);
			vars.SetInt($"{_prefix}{index}.Reward", task.Reward);
		}

		private List<QuestTask> CreateDailyTasks(Character character, int difficulty)
		{
			var scale = 1 << (difficulty - 1);
			var reward = scale;
			var rnd = RandomProvider.Get();

			QuestTask Task(TaskKind kind, int target, int param = 0) => new() { Kind = kind, Target = target * scale, Param = param, Reward = reward };

			var tasks = new List<QuestTask>
			{
				Task(TaskKind.Monster, 250),
				Task(TaskKind.Elite, 20),
				Task(TaskKind.Boss, 2),
				Task(TaskKind.Mythic, 5),
				new() { Kind = TaskKind.Dungeon, Target = difficulty == 1 ? 1 : difficulty == 2 ? 3 : 5, Reward = reward },
				Task(TaskKind.Race, 200, (int)Races[rnd.Next(Races.Length)]),
				Task(TaskKind.Attribute, 150, (int)Attributes[rnd.Next(Attributes.Length)]),
				Task(TaskKind.Size, 150, (int)Sizes[rnd.Next(Sizes.Length)]),
			};

			var maps = GetHuntingMaps(character);
			if (maps.Count > 0)
				tasks.Add(Task(TaskKind.Map, 250, maps[rnd.Next(maps.Count)]));

			return tasks;
		}

		private List<QuestTask> CreateWeeklyTasks(Character character)
		{
			const int Reward = 4;
			QuestTask Task(TaskKind kind, int target, int param = 0) => new() { Kind = kind, Target = target, Param = param, Reward = Reward };

			var tasks = new List<QuestTask>
			{
				Task(TaskKind.Monster, 8000),
				Task(TaskKind.Elite, 640),
				Task(TaskKind.Boss, 64),
				Task(TaskKind.Mythic, 160),
				Task(TaskKind.Dungeon, 40),
			};

			tasks.AddRange(Races.Select(a => Task(TaskKind.Race, 6400, (int)a)));
			tasks.AddRange(Attributes.Select(a => Task(TaskKind.Attribute, 4800, (int)a)));
			tasks.AddRange(Sizes.Select(a => Task(TaskKind.Size, 4800, (int)a)));
			tasks.AddRange(GetHuntingMaps(character).OrderBy(_ => RandomProvider.Get().Next()).Take(8).Select(a => Task(TaskKind.Map, 1500, a)));

			return tasks;
		}

		private DateTime GetPeriodStart()
		{
			var conf = ZoneServer.Instance.Conf.World;
			var now = DateTime.Now;
			var start = now.Date.AddHours(conf.InstancedDungeonResetHour).AddMinutes(conf.InstancedDungeonResetMinute);

			if (start > now)
				start = start.AddDays(-1);

			if (_weekly)
				start = start.AddDays(-(((int)start.DayOfWeek - (int)conf.InstancedDungeonWeeklyResetDay + 7) % 7));

			return start;
		}

		private static List<int> GetHuntingMaps(Character character)
		{
			var maps = ZoneServer.Instance.Data.MapDb.Entries.Values
				.Where(a => (a.Type == MapType.Field || a.Type == MapType.Dungeon) && a.SpawnedMonsterIds.Count > 0)
				.ToList();

			var nearLevel = maps
				.Where(a => a.Level <= character.Level + 5)
				.OrderByDescending(a => a.Level)
				.Take(15)
				.Select(a => a.Id)
				.ToList();

			if (nearLevel.Count > 0)
				return nearLevel;

			return maps.OrderBy(a => a.Level).Take(15).Select(a => a.Id).ToList();
		}
	}
}
