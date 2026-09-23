//--- Melia Script ----------------------------------------------------------
// Track Minigames
//--- Description -----------------------------------------------------------
// Staged monster waves played inside a track's private layer.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Tracks;
using Yggdrasil.Logging;

/// <summary>
/// A minigame played inside a track's private layer.
/// </summary>
public class TrackMinigame
{
	private static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(1);

	private readonly Dictionary<string, MinigameStage> _stages = new Dictionary<string, MinigameStage>();
	private bool _running;

	/// <summary>
	/// Returns the character whose track the minigame runs in.
	/// </summary>
	public Character Character { get; }

	/// <summary>
	/// Returns the track the minigame runs in.
	/// </summary>
	public Track Track { get; }

	/// <summary>
	/// Creates a new minigame for the given track.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="track"></param>
	public TrackMinigame(Character character, Track track)
	{
		this.Character = character;
		this.Track = track;
	}

	/// <summary>
	/// Returns the stage with the given name, creating it if necessary.
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public MinigameStage Stage(string name)
	{
		if (!_stages.TryGetValue(name, out var stage))
			_stages[name] = stage = new MinigameStage(this, name);

		return stage;
	}

	/// <summary>
	/// Starts the given stages and begins evaluating their events.
	/// </summary>
	/// <param name="stageNames"></param>
	public void Start(params string[] stageNames)
	{
		if (_running)
			return;

		// A shared track spawns its monsters once, from the owner.
		if (this.Track.Group != null && this.Track.Owner != null && this.Track.Owner != this.Character)
			return;

		_running = true;

		foreach (var name in stageNames)
			this.StartStage(name);

		_ = this.Run();
	}

	/// <summary>
	/// Starts the stage with the given name, unless it is already active.
	/// </summary>
	/// <param name="name"></param>
	public void StartStage(string name)
	{
		if (_stages.TryGetValue(name, out var stage))
			stage.Begin();
	}

	/// <summary>
	/// Ends the stage with the given name, removing its monsters.
	/// </summary>
	/// <param name="name"></param>
	public void ClearStage(string name)
	{
		if (_stages.TryGetValue(name, out var stage))
			stage.Clear();
	}

	/// <summary>
	/// Completes the given quest objective for every player in the track.
	/// </summary>
	/// <param name="questId"></param>
	/// <param name="objectiveIdent"></param>
	public void CompleteObjective(long questId, string objectiveIdent)
		=> this.CompleteObjective(new QuestId(questId), objectiveIdent);

	/// <summary>
	/// Completes the given quest objective for every player in the track.
	/// </summary>
	/// <param name="questId"></param>
	/// <param name="objectiveIdent"></param>
	public void CompleteObjective(QuestId questId, string objectiveIdent)
	{
		foreach (var member in this.Members.ToList())
		{
			if (member.Quests.IsActive(questId))
				member.Quests.CompleteObjective(questId, objectiveIdent);
		}
	}

	/// <summary>
	/// Returns true while the minigame's track is still being played.
	/// </summary>
	public bool IsActive
		=> _running && (this.Track.Owner ?? this.Character).Tracks?.ActiveTrack == this.Track;

	/// <summary>
	/// Returns the characters the minigame's monsters go after.
	/// </summary>
	internal IEnumerable<Character> Members
		=> this.Track.Group != null ? this.Track.Group.Members : new[] { this.Character };

	private async Task Run()
	{
		try
		{
			while (this.IsActive)
			{
				await Task.Delay(TickInterval);

				if (!this.IsActive)
					break;

				foreach (var stage in _stages.Values.Where(a => a.IsActive).ToList())
					stage.Tick();
			}
		}
		catch (Exception ex)
		{
			Log.Error("TrackMinigame: Error while running the minigame for '{0}': {1}", this.Track.Id, ex);
		}
		finally
		{
			_running = false;
		}
	}
}

/// <summary>
/// One stage of a track minigame.
/// </summary>
public class MinigameStage
{
	private readonly List<MinigameSpawn> _spawns = new List<MinigameSpawn>();
	private readonly List<MinigameEvent> _events = new List<MinigameEvent>();
	private DateTime _startTime;

	/// <summary>
	/// Returns the minigame the stage belongs to.
	/// </summary>
	public TrackMinigame Game { get; }

	/// <summary>
	/// Returns the stage's name.
	/// </summary>
	public string Name { get; }

	/// <summary>
	/// Returns true while the stage is running.
	/// </summary>
	public bool IsActive { get; private set; }

	/// <summary>
	/// Returns the seconds since the stage started.
	/// </summary>
	public double Elapsed => (DateTime.Now - _startTime).TotalSeconds;

	internal MinigameStage(TrackMinigame game, string name)
	{
		this.Game = game;
		this.Name = name;
	}

	/// <summary>
	/// Adds a spawn point whose monsters appear when the stage starts.
	/// </summary>
	/// <param name="monsterId">The monster to spawn.</param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	/// <param name="direction"></param>
	/// <param name="count">How many monsters the point keeps standing.</param>
	/// <param name="respawnSeconds">Seconds before a killed monster comes back, or 0 for never.</param>
	/// <param name="aggressive">Whether the monsters go straight for the players.</param>
	/// <param name="level">Level override, or 0 for the monster's own.</param>
	/// <param name="maxHp">Max HP override, or 0 for the monster's own.</param>
	/// <returns></returns>
	public MinigameStage Monster(int monsterId, double x, double y, double z, double direction = 0, int count = 1, int respawnSeconds = 0, bool aggressive = true, int level = 0, int maxHp = 0)
	{
		_spawns.Add(new MinigameSpawn(this, monsterId, new Position((float)x, (float)y, (float)z), direction, count, respawnSeconds, aggressive, level, maxHp));
		return this;
	}

	/// <summary>
	/// Adds an event that runs its action whenever its condition holds.
	/// </summary>
	/// <param name="condition"></param>
	/// <param name="action"></param>
	/// <param name="execCount">How often the event may run, or 0 for no limit.</param>
	/// <returns></returns>
	public MinigameStage On(Func<MinigameStage, bool> condition, Action<MinigameStage> action, int execCount = 0)
	{
		_events.Add(new MinigameEvent(condition, action, execCount));
		return this;
	}

	/// <summary>
	/// Returns how many monsters of the given spawn points, or of all of them, are alive.
	/// </summary>
	/// <param name="spawnIndices"></param>
	/// <returns></returns>
	public int Alive(params int[] spawnIndices)
	{
		var spawns = spawnIndices.Length == 0 ? (IEnumerable<MinigameSpawn>)_spawns : spawnIndices.Where(i => i >= 0 && i < _spawns.Count).Select(i => _spawns[i]);
		return spawns.Sum(a => a.AliveCount);
	}

	/// <summary>
	/// Returns the HP percentage of the spawn point's first living monster.
	/// </summary>
	/// <param name="spawnIndex"></param>
	/// <returns></returns>
	public float HpRate(int spawnIndex)
	{
		if (spawnIndex < 0 || spawnIndex >= _spawns.Count)
			return 0;

		var mob = _spawns[spawnIndex].Living.FirstOrDefault();
		if (mob == null || mob.MaxHp <= 0)
			return 0;

		return mob.Hp * 100f / mob.MaxHp;
	}

	/// <summary>
	/// Spawns additional monsters at the given spawn point.
	/// </summary>
	/// <param name="spawnIndex"></param>
	/// <param name="count"></param>
	public void Spawn(int spawnIndex, int count = 1)
	{
		if (spawnIndex < 0 || spawnIndex >= _spawns.Count)
			return;

		for (var i = 0; i < count; i++)
			_spawns[spawnIndex].SpawnOne();
	}

	internal void Begin()
	{
		if (this.IsActive)
			return;

		this.IsActive = true;
		_startTime = DateTime.Now;

		foreach (var spawn in _spawns)
			spawn.SpawnInitial();
	}

	internal void Clear()
	{
		if (!this.IsActive)
			return;

		this.IsActive = false;

		foreach (var spawn in _spawns)
			spawn.RemoveAll();
	}

	internal void Tick()
	{
		foreach (var ev in _events)
		{
			if (!this.IsActive)
				return;

			if (ev.ExecCount > 0 && ev.Executed >= ev.ExecCount)
				continue;

			if (!ev.Condition(this))
				continue;

			ev.Executed++;
			ev.Action(this);
		}
	}

	private class MinigameEvent
	{
		public Func<MinigameStage, bool> Condition { get; }
		public Action<MinigameStage> Action { get; }
		public int ExecCount { get; }
		public int Executed { get; set; }

		public MinigameEvent(Func<MinigameStage, bool> condition, Action<MinigameStage> action, int execCount)
		{
			this.Condition = condition;
			this.Action = action;
			this.ExecCount = execCount;
		}
	}
}

/// <summary>
/// A spawn point of a minigame stage.
/// </summary>
internal class MinigameSpawn
{
	private readonly MinigameStage _stage;
	private readonly int _monsterId;
	private readonly Position _position;
	private readonly double _direction;
	private readonly int _count;
	private readonly int _respawnSeconds;
	private readonly bool _aggressive;
	private readonly int _level;
	private readonly int _maxHp;
	private readonly List<Mob> _mobs = new List<Mob>();

	public IEnumerable<Mob> Living => _mobs.Where(a => !a.IsDead && a.Map != null);

	public int AliveCount => this.Living.Count();

	public MinigameSpawn(MinigameStage stage, int monsterId, Position position, double direction, int count, int respawnSeconds, bool aggressive, int level, int maxHp)
	{
		_stage = stage;
		_monsterId = monsterId;
		_position = position;
		_direction = direction;
		_count = Math.Max(1, count);
		_respawnSeconds = respawnSeconds;
		_aggressive = aggressive;
		_level = level;
		_maxHp = maxHp;
	}

	public void SpawnInitial()
	{
		for (var i = 0; i < _count; i++)
			this.SpawnOne();
	}

	public void SpawnOne()
	{
		var game = _stage.Game;
		var character = game.Character;

		if (!game.IsActive || character.Map == null)
			return;

		if (!ZoneServer.Instance.Data.MonsterDb.TryFind(_monsterId, out var monsterData))
		{
			Log.Warning("TrackMinigame: Monster '{0}' not found.", _monsterId);
			return;
		}

		var mob = new Mob(monsterData.Id, RelationType.Enemy);

		var pos = _position;
		if (character.Map.Ground.TryGetHeightAt(pos, out var height))
			pos = new Position(pos.X, height, pos.Z);

		mob.Position = pos;
		mob.SpawnPosition = pos;
		mob.Direction = new Direction(_direction);
		mob.Layer = character.Layer;
		mob.Faction = FactionType.Monster;
		mob.Visibility = ActorVisibility.Always;
		mob.AddEffect(new ScriptInvisibleEffect());

		mob.Components.Add(new MovementComponent(mob));
		mob.Components.Add(new AiComponent(mob, "BasicMonster"));

		if (_aggressive)
			mob.Tendency = TendencyType.Aggressive;

		mob.Died += this.OnDied;

		character.Map.AddMonster(mob, immediate: true);

		var overrides = new PropertyOverrides();
		if (_level > 0)
			overrides["Lv"] = _level;
		if (_maxHp > 0)
			overrides["MHP"] = _maxHp;
		if (overrides.Count > 0)
			mob.ApplyOverrides(overrides);

		if (_aggressive)
		{
			foreach (var member in game.Members)
			{
				if (member.CanTarget(mob))
					mob.InsertHate(member);
			}
		}

		_mobs.RemoveAll(a => a.IsDead || a.Map == null);
		_mobs.Add(mob);
	}

	public void RemoveAll()
	{
		foreach (var mob in this.Living.ToList())
			mob.Map?.RemoveMonster(mob);

		_mobs.Clear();
	}

	private void OnDied(Mob mob, ICombatEntity killer)
	{
		if (_respawnSeconds <= 0)
			return;

		_ = this.Respawn();
	}

	private async Task Respawn()
	{
		await Task.Delay(TimeSpan.FromSeconds(_respawnSeconds));

		if (!_stage.IsActive || !_stage.Game.IsActive)
			return;

		if (this.AliveCount < _count)
			this.SpawnOne();
	}
}
