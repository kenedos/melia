using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Dungeons.Stages;
using Melia.Zone.World.Maps;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone.World.Dungeons
{
	/// <summary>
	/// Represents the lifecycle state of a dungeon instance.
	/// </summary>
	public enum InstanceState
	{
		/// <summary>Instance has been created but not yet initialized.</summary>
		Created,

		/// <summary>Instance is being initialized (players joining, setup in progress).</summary>
		Initializing,

		/// <summary>Instance has started and is actively running.</summary>
		Started,

		/// <summary>Instance is in progress with active gameplay.</summary>
		Active,

		/// <summary>Instance is in the process of completing.</summary>
		Completing,

		/// <summary>Instance has been completed successfully.</summary>
		Complete,

		/// <summary>Instance is empty (all players left) and awaiting timeout or rejoin.</summary>
		Empty,

		/// <summary>Instance has been destroyed and cleaned up.</summary>
		Destroyed
	}

	/// <summary>
	/// Represents a single dungeon instance with stage management and player tracking.
	/// </summary>
	public class InstanceDungeon
	{
		#region Constants

		/// <summary>Instance variable name for storing dungeon start time.</summary>
		private const string StartTimeVarName = "Melia.Dungeon.StartTime";

		/// <summary>Instance variable name for tracking if entry count was already incremented.</summary>
		public const string EntryCountIncrementedVarName = "Melia.Dungeon.EntryCountIncremented";

		#endregion

		/// <summary>
		/// Lock object for thread-safe dungeon state transitions.
		/// </summary>
		private readonly object _stateLock = new();

		/// <summary>
		/// Gets or sets the unique dungeon instance's id.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the dungeon's id.
		/// </summary>
		public int DungeonId { get; set; }

		/// <summary>
		/// Gets or sets the dungeon map.
		/// </summary>
		public Map Map { get; set; }

		/// <summary>
		/// Gets the dungeon map's ID.
		/// </summary>
		public int MapId => this.Map.Id;

		/// <summary>
		/// Gets or sets the dungeon owner's character.
		/// </summary>
		public Character Owner { get; set; }

		/// <summary>
		/// Gets or sets the dungeon completion state.
		/// </summary>
		public bool IsComplete { get; set; }

		/// <summary>
		/// Gets whether the dungeon has been started.
		/// </summary>
		public bool IsStarted { get; private set; }

		/// <summary>
		/// Gets the current lifecycle state of the dungeon instance.
		/// </summary>
		public InstanceState State { get; private set; } = InstanceState.Created;

		/// <summary>
		/// Whether to show the reward HUD for this dungeon instance.
		/// Set from DungeonScript.UseRewardHud during StartDungeon.
		/// </summary>
		public bool UseRewardHud { get; private set; } = true;

		public Position StartPosition { get; set; }
		public int Layer { get; set; } = 0;
		public List<Character> Characters { get; set; }
		public long PartyId { get; set; }
		public InstanceDungeonData InstanceDungeonData { get; private set; }
		public DungeonRankCalculator RankCalculator { get; private set; }

		/// <summary>
		/// Gets or sets the dungeon required level.
		/// </summary>
		public int Level => this.InstanceDungeonData.Level;

		public List<DungeonStage> Stages { get; set; }
		public DungeonStage CurrentStage { get; set; }

		/// <summary>
		/// Dictionary mapping stage IDs to stage instances for quick lookup.
		/// </summary>
		private readonly Dictionary<string, DungeonStage> _stageMap = new();

		/// <summary>
		/// Returns the Instance Dungeon's variables.
		/// </summary>
		/// <remarks>
		/// Instance Dungeon variables are temporary and are not saved across server
		/// restarts.
		/// </remarks>
		public Variables Vars { get; } = new Variables();

		#region Monster Tracking

		private readonly List<IMonster> _persistentMonsters = new List<IMonster>();

		/// <summary>
		/// Monsters that persist across stage transitions until dungeon ends.
		/// These are spawned by UnlockStage and are NOT cleaned up when stages complete.
		/// </summary>
		public IReadOnlyList<IMonster> PersistentMonsters => _persistentMonsters;

		private readonly object _monsterTrackingLock = new();

		/// <summary>
		/// Total number of monsters expected across ALL stages in this dungeon instance.
		/// This is calculated once at dungeon start from stage definitions.
		/// </summary>
		public int TotalExpectedMonsters { get; private set; } = 0;

		/// <summary>
		/// Total number of monsters actually spawned so far in this dungeon instance.
		/// </summary>
		public int TotalMonstersSpawned { get; private set; } = 0;

		/// <summary>
		/// Total number of monsters killed across all stages in this dungeon instance.
		/// </summary>
		public int TotalMonstersKilled { get; private set; } = 0;

		/// <summary>
		/// Total base exp from all monsters killed in this dungeon instance.
		/// Used for calculating dungeon completion rewards.
		/// </summary>
		public long TotalMonsterExp { get; private set; } = 0;

		/// <summary>
		/// Total job exp from all monsters killed in this dungeon instance.
		/// Used for calculating dungeon completion rewards.
		/// </summary>
		public long TotalMonsterJobExp { get; private set; } = 0;

		/// <summary>
		/// Gets the percentage of monsters killed across the entire dungeon instance.
		/// Uses TotalExpectedMonsters (calculated from all stages) as the denominator
		/// to prevent the progress bar from advancing too quickly.
		/// </summary>
		public int InstanceKillPercentage
		{
			get
			{
				// Use expected total if available, otherwise fall back to spawned count
				var total = this.TotalExpectedMonsters > 0 ? this.TotalExpectedMonsters : this.TotalMonstersSpawned;
				if (total == 0) return 0;
				return (int)(this.TotalMonstersKilled / (double)total * 100.0);
			}
		}

		/// <summary>
		/// Calculates the total expected monsters across all stages.
		/// Should be called once during dungeon initialization.
		/// </summary>
		public void CalculateTotalExpectedMonsters()
		{
			lock (_monsterTrackingLock)
			{
				this.TotalExpectedMonsters = 0;

				foreach (var stage in this.Stages)
				{
					this.TotalExpectedMonsters += stage.GetExpectedMonsterCount();
				}
			}
		}

		/// <summary>
		/// Allows manually setting the expected monster count.
		/// Useful for dungeons with dynamic/procedural spawning.
		/// </summary>
		public void SetExpectedMonsterCount(int count)
		{
			lock (_monsterTrackingLock)
			{
				this.TotalExpectedMonsters = count;
			}
		}

		/// <summary>
		/// Adds to the expected monster count.
		/// Useful for stages that spawn additional monsters dynamically.
		/// </summary>
		public void AddExpectedMonsters(int count)
		{
			lock (_monsterTrackingLock)
			{
				this.TotalExpectedMonsters += count;
			}
		}

		/// <summary>
		/// Registers a monster spawn for instance-wide tracking.
		/// Called automatically when monsters are added to stages.
		/// </summary>
		public void RegisterMonsterSpawn(int count = 1)
		{
			lock (_monsterTrackingLock)
			{
				this.TotalMonstersSpawned += count;
			}
		}

		/// <summary>
		/// Registers a monster kill for instance-wide tracking.
		/// Called automatically when monsters die.
		/// </summary>
		/// <param name="count">Number of monsters killed</param>
		/// <param name="baseExp">Base exp from the killed monster(s)</param>
		/// <param name="jobExp">Job exp from the killed monster(s)</param>
		public void RegisterMonsterKill(int count = 1, long baseExp = 0, long jobExp = 0)
		{
			lock (_monsterTrackingLock)
			{
				this.TotalMonstersKilled += count;
				this.TotalMonsterExp += baseExp;
				this.TotalMonsterJobExp += jobExp;
			}
			// Update the HUD whenever a monster is killed (if enabled)
			if (this.UseRewardHud)
			{
				this.UpdateDungeonRewardHud();
			}
		}
		#endregion

		#region Background Stages
		/// <summary>
		/// Background stages that run in parallel with the main stage progression.
		/// Examples: dungeon timers, periodic events, global condition checks.
		/// </summary>
		public List<BackgroundStage> BackgroundStages { get; } = new();

		private readonly Dictionary<string, BackgroundStage> _backgroundStageMap = new();
		private CancellationTokenSource _backgroundStageCts;
		private readonly List<Task> _backgroundStageTasks = new();

		private readonly CancellationTokenSource _stageCts = new();

		/// <summary>
		/// Cancellation token that is signaled when the dungeon is cleaned up.
		/// Stages should pass this to Task.Delay calls to avoid pinning
		/// the InstanceDungeon in memory after destruction.
		/// </summary>
		public CancellationToken StageCancellationToken => _stageCts.Token;

		/// <summary>
		/// Adds a background stage to the dungeon.
		/// Background stages run in parallel with the main stage progression.
		/// </summary>
		public void AddBackgroundStage(BackgroundStage stage)
		{
			this.BackgroundStages.Add(stage);
			if (!string.IsNullOrEmpty(stage.StageId))
			{
				_backgroundStageMap[stage.StageId] = stage;
			}
		}

		/// <summary>
		/// Gets a background stage by ID.
		/// </summary>
		public BackgroundStage GetBackgroundStage(string stageId)
		{
			return _backgroundStageMap.GetValueOrDefault(stageId);
		}

		/// <summary>
		/// Starts all background stages.
		/// Called automatically when the dungeon starts.
		/// Tracks running tasks for proper cleanup.
		/// </summary>
		private Task StartBackgroundStages(DungeonScript dungeonScript)
		{
			_backgroundStageCts = new CancellationTokenSource();
			_backgroundStageTasks.Clear();

			foreach (var stage in this.BackgroundStages)
			{
				// Start each background stage and track the task
				var task = stage.Start(this, dungeonScript, _backgroundStageCts.Token);
				_backgroundStageTasks.Add(task);
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Stops all background stages.
		/// Called when the dungeon ends or is completed.
		/// </summary>
		public void StopBackgroundStages()
		{
			// Cancel all background stages
			_backgroundStageCts?.Cancel();

			// Stop each stage
			foreach (var stage in this.BackgroundStages)
			{
				stage.Stop();
			}
		}

		/// <summary>
		/// Stops all background stages and waits for their tasks to complete.
		/// Use this for graceful shutdown.
		/// </summary>
		public async Task StopBackgroundStagesAsync()
		{
			this.StopBackgroundStages();

			// Wait for all tasks to complete (with timeout)
			if (_backgroundStageTasks.Count > 0)
			{
				try
				{
					await Task.WhenAll(_backgroundStageTasks).WaitAsync(TimeSpan.FromSeconds(5));
				}
				catch (TimeoutException)
				{
					Log.Warning("InstanceDungeon: Background stages did not complete within timeout for instance '{0}'.", this.Id);
				}
				catch (Exception)
				{
					// Tasks may throw when cancelled, which is expected
				}
			}

			_backgroundStageTasks.Clear();
		}
		#endregion

		// ===========================
		// FACTORY AND INITIALIZATION
		// ===========================
		public static InstanceDungeon CreateNew(string id, InstanceDungeonData dungeonData, MapData mapData, Character owner, List<Character> characters, List<DungeonStage> stages, Position startPosition)
		{
			var instance = new InstanceDungeon
			{
				Id = id,
				DungeonId = dungeonData.Id,
				Map = ZoneServer.Instance.World.GetMap(mapData.Id),
				Owner = owner,
				Characters = characters,
				PartyId = owner.Connection?.Party?.DbId ?? 0,
				InstanceDungeonData = dungeonData,
				Stages = stages,
				StartPosition = startPosition
			};
			instance.RankCalculator = new DungeonRankCalculator(instance);
			instance.RegisterStages();
			return instance;
		}

		/// <summary>
		/// Gets the stage with the specified ID, or null if not found.
		/// </summary>
		public DungeonStage GetStage(string stageId)
		{
			return _stageMap.GetValueOrDefault(stageId);
		}

		/// <summary>
		/// Registers all stages in the stage map for quick lookup.
		/// Should be called during dungeon creation.
		/// </summary>
		public void RegisterStages()
		{
			_stageMap.Clear();
			foreach (var stage in this.Stages)
			{
				if (!string.IsNullOrEmpty(stage.StageId))
				{
					_stageMap[stage.StageId] = stage;
				}
			}
		}

		public bool IsOwner(Character character)
		{
			return (this.Owner == character);
		}

		/// <summary>
		/// Called when the dungeon instance should start.
		/// Handles state transitions and entry count tracking.
		/// Thread-safe - only the first caller will execute the start logic.
		/// </summary>
		public async Task StartDungeon(DungeonScript dungeonScript)
		{
			// Atomic check-and-set to prevent race conditions
			lock (_stateLock)
			{
				if (this.IsStarted)
					return;

				this.IsStarted = true;
				this.State = InstanceState.Initializing;
			}

			// Record start time
			this.Vars.Set(StartTimeVarName, DateTime.UtcNow);

			// Store reward HUD preference from dungeon script
			this.UseRewardHud = dungeonScript.UseRewardHud;

			// Calculate total expected monsters BEFORE starting stages
			// This ensures the kill percentage is calculated correctly from the start
			this.CalculateTotalExpectedMonsters();

			// Call the script's instance creation hook for one-time setup.
			dungeonScript.OnInstanceCreated(this);

			this.Layer = this.Owner?.StartLayer(silent: true) ?? 1;

			// Check if entry count should be incremented on enter (default) or on complete
			var incrementOnComplete = ZoneServer.Instance.Conf.World.InstancedDungeonIncrementEntryOnComplete;

			// Take a snapshot of characters to avoid collection modification during iteration
			var charactersSnapshot = this.Characters.ToList();
			foreach (var character in charactersSnapshot)
			{
				if (character == null) continue;

				// Increment entry count on enter if not configured to increment on complete
				if (!incrementOnComplete && character.Connection?.Account != null)
				{
					character.Dungeon.IncreaseEntryCount(this.DungeonId, 1);
				}

				character.SetPosition(this.StartPosition);
				Send.ZC_SET_POS(character);
				character.Layer = this.Layer;
				character.LookAround();
				character.Dungeon.InstanceDungeon = this;

				if (character.Connection != null)
				{
					Send.ZC_NORMAL.SetMapMode(character.Connection, "Indun");
					Send.ZC_NORMAL.IndunAddonMsgParam(character.Connection, 2, "START", 1);
				}
			}

			// Track that entry count was incremented (to prevent double-increment on complete)
			if (!incrementOnComplete)
			{
				this.Vars.Set(EntryCountIncrementedVarName, true);
			}

			this.State = InstanceState.Started;

			// Start background stages (including default timer)
			// NOTE: DungeonTimerStage handles all timing - no separate duration monitoring needed
			await this.StartBackgroundStages(dungeonScript);

			// Transition to the first stage.
			await this.MoveToNextStage(dungeonScript);

			this.State = InstanceState.Active;

			// Open and initialize the reward HUD (replaces PercentUI stage)
			// Only if enabled for this dungeon
			if (this.UseRewardHud)
			{
				this.OpenDungeonRewardHud();
			}
		}

		/// <summary>
		/// Moves to the next stage based on the current stage's transitions.
		/// Thread-safe to prevent concurrent stage transitions.
		/// </summary>
		public async Task MoveToNextStage(DungeonScript dungeonScript)
		{
			DungeonStage previousStage;

			lock (_stateLock)
			{
				previousStage = this.CurrentStage;
			}

			previousStage?.Complete();

			string nextStageId = null;

			// Use the transition system
			if (previousStage != null)
			{
				nextStageId = previousStage.GetNextStageId(this);
			}
			else
			{
				// If there's no previous stage, start with the first stage in the list
				if (this.Stages.Count > 0)
				{
					nextStageId = this.Stages[0].StageId;
				}
			}

			// Handle special terminal stage IDs
			if (nextStageId == StageId.Complete)
			{
				dungeonScript.DungeonComplete(this);
				return;
			}
			if (nextStageId == StageId.Fail)
			{
				dungeonScript.DungeonEnded(this, true);
				return;
			}

			// Find the next stage by ID
			DungeonStage nextStage = null;
			if (!string.IsNullOrEmpty(nextStageId))
			{
				nextStage = this.GetStage(nextStageId);
			}

			// Fallback to sequential behavior if no valid transition found
			if (nextStage == null && previousStage != null)
			{
				var currentIndex = this.Stages.IndexOf(previousStage);
				if (currentIndex > -1 && currentIndex + 1 < this.Stages.Count)
				{
					nextStage = this.Stages[currentIndex + 1];
				}
			}

			lock (_stateLock)
			{
				this.CurrentStage = nextStage;
			}

			if (nextStage != null)
			{
				// There's another stage, initialize it.
				await nextStage.Initialize(this);

				// Update reward HUD when stage changes (if enabled)
				if (this.UseRewardHud)
				{
					this.UpdateDungeonRewardHud();
				}
			}
			else
			{
				// No more stages, the dungeon is complete.
				dungeonScript.DungeonComplete(this);
			}
		}

		/// <summary>
		/// Opens and initializes the dungeon reward HUD for all characters.
		/// This is called once when the dungeon starts (replaces PercentUI stage).
		/// </summary>
		public void OpenDungeonRewardHud()
		{
			foreach (var character in this.Characters)
			{
				if (character?.Connection == null) continue;

				// Open the reward HUD addon with the dungeon's MiniGame ID
				character.AddonMessage(AddonMessage.OPEN_INDUN_REWARD_HUD, this.InstanceDungeonData.ClassName);

				// Tell the client to show the percentage UI
				Send.ZC_NORMAL.IndunAddonMsgParam(character.Connection, 2, "PercentUI", 0);
			}

			// Send initial percentage update
			this.UpdateDungeonRewardHud();
		}

		/// <summary>
		/// Updates the dungeon reward HUD for all characters.
		/// Now uses instance-wide kill percentage instead of per-stage.
		/// </summary>
		public void UpdateDungeonRewardHud()
		{
			// Use instance-wide percentage instead of per-stage
			var currentPercentage = this.InstanceKillPercentage;

			// Send HUD update to all characters
			foreach (var character in this.Characters)
			{
				if (character?.Connection == null) continue;
				character.AddonMessage(AddonMessage.REFRESH_INDUN_REWARD_HUD, this.InstanceDungeonData.ClassName, currentPercentage);
			}
		}

		/// <summary>
		/// Opens the dungeon reward HUD for a specific character (used when rejoining).
		/// </summary>
		public void OpenDungeonRewardHudForCharacter(Character character)
		{
			if (!this.UseRewardHud || character?.Connection == null)
				return;

			// Open the reward HUD addon with the dungeon's MiniGame ID
			character.AddonMessage(AddonMessage.OPEN_INDUN_REWARD_HUD, this.InstanceDungeonData.ClassName);

			// Tell the client to show the percentage UI
			Send.ZC_NORMAL.IndunAddonMsgParam(character.Connection, 2, "PercentUI", 0);

			// Send current percentage
			character.AddonMessage(AddonMessage.REFRESH_INDUN_REWARD_HUD, this.InstanceDungeonData.ClassName, this.InstanceKillPercentage);
		}

		/// <summary>
		/// Gets the completion reward multiplier based on instance-wide monster kill percentage.
		/// </summary>
		public int GetCompletionRewardMultiplier()
		{
			var percentage = this.InstanceKillPercentage;
			if (percentage >= 100) return 5;
			const double pointsPerTier = 20.0;
			return (int)Math.Truncate(percentage / pointsPerTier) + 1;
		}

		/// <summary>
		/// Returns true if all stages are marked as completed.
		/// </summary>
		public bool IsDungeonComplete()
		{
			return this.Stages.TrueForAll(s => s.IsCompleted);
		}

		/// <summary>
		/// Gets the elapsed time since the dungeon started.
		/// </summary>
		public TimeSpan GetElapsedTime()
		{
			if (!this.Vars.Has(StartTimeVarName))
				return TimeSpan.Zero;

			var startTime = this.Vars.Get<DateTime>(StartTimeVarName);
			return DateTime.UtcNow - startTime;
		}

		/// <summary>
		/// Returns true if entry count was already incremented for this instance.
		/// Used to prevent double-increment between enter and complete.
		/// </summary>
		public bool WasEntryCountIncremented()
		{
			return this.Vars.GetBool(EntryCountIncrementedVarName);
		}

		/// <summary>
		/// Marks that entry count has been incremented for this instance.
		/// </summary>
		public void MarkEntryCountIncremented()
		{
			this.Vars.Set(EntryCountIncrementedVarName, true);
		}

		/// <summary>
		/// Cleanup when dungeon ends.
		/// Properly stops background stages and disposes resources.
		/// </summary>
		public void Cleanup()
		{
			if (this.State == InstanceState.Destroyed)
				return;

			this.State = InstanceState.Destroyed;
			this.StopBackgroundStages();
			CleanupPersistentMonsters();

			// Dispose the background stage CTS
			_backgroundStageCts?.Dispose();
			_backgroundStageCts = null;

			// Cancel the stage CTS to unblock fire-and-forget tasks
			// in TimedSurvivalStage, WaveSurvivalStage, TimelineStage, etc.
			_stageCts.Cancel();
			_stageCts.Dispose();

			// Clear task references
			_backgroundStageTasks.Clear();

			// Clear stage monster lists to release mob references
			if (this.Stages != null)
			{
				foreach (var stage in this.Stages)
					stage.Monsters.Clear();
				this.Stages.Clear();
			}
			_stageMap.Clear();

			// Clear background stages
			this.BackgroundStages.Clear();
			_backgroundStageMap.Clear();

			// Clear character references to prevent memory leak
			this.Characters?.Clear();
		}

		/// <summary>
		/// Registers a monster as persistent. It will not be removed when stages complete.
		/// Called by UnlockStage.SpawnPersistent* methods.
		/// </summary>
		public void RegisterPersistentMonster(IMonster monster)
		{
			if (!_persistentMonsters.Contains(monster))
			{
				_persistentMonsters.Add(monster);
			}
		}

		/// <summary>
		/// Unregisters a persistent monster. Called when manually removing persistent content.
		/// </summary>
		public void UnregisterPersistentMonster(IMonster monster)
		{
			_persistentMonsters.Remove(monster);
		}

		/// <summary>
		/// Removes all persistent monsters. Called when dungeon ends.
		/// </summary>
		public void CleanupPersistentMonsters()
		{
			foreach (var monster in _persistentMonsters)
			{
				monster.Map?.RemoveMonster(monster);
			}
			_persistentMonsters.Clear();

			// Also cleanup any UnlockStage persistent monsters
			foreach (var stage in Stages)
			{
				if (stage is UnlockStage unlockStage)
				{
					unlockStage.CleanupPersistentMonsters();
				}
			}
		}
	}
}
