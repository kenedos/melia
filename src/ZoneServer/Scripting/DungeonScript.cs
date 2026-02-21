using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.World;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Dungeons;
using Melia.Zone.World.Dungeons.Stages;
using Melia.Zone.World.Quests.Objectives;
using Yggdrasil.Logging;
using static Melia.Shared.Util.TaskHelper;

namespace Melia.Zone.Scripting
{
	/// <summary>
	/// Script for managing instanced dungeons with stages.
	/// Includes default background timer behavior that can be customized.
	/// </summary>
	public abstract partial class DungeonScript : GeneralScript
	{
		#region Constants

		/// <summary>Variable name for storing the active instance dungeon script ID.</summary>
		public const string ActiveInstanceVarName = "Melia.Dungeon.ActiveInstanceDungeonId";

		/// <summary>Instance variable name for storing dungeon completion time.</summary>
		private const string CompletionTimeVarName = "Melia.Dungeon.CompletionTime";

		/// <summary>Instance variable name for storing when the instance became empty.</summary>
		private const string EmptyStartTimeVarName = "Melia.Dungeon.EmptyStartTime";

		/// <summary>Interval in milliseconds for checking instance timeout.</summary>
		private const int InstanceTimeoutCheckIntervalMs = 5000;

		/// <summary>Maximum time to wait for instance creation in milliseconds.</summary>
		private const int InstanceCreationWaitTimeoutMs = 10000;

		/// <summary>Maximum time to wait for party members to warp in.</summary>
		private static readonly TimeSpan MaxWarpWaitTime = TimeSpan.FromMinutes(1);

		/// <summary>Time after completion before auto-warping players out.</summary>
		private static readonly TimeSpan AutoWarpDelay = TimeSpan.FromSeconds(15);

		#endregion

		#region Static Fields

		private static readonly ConcurrentDictionary<string, DungeonScript> Scripts = new();

		#endregion

		#region Instance Fields

		/// <summary>
		/// Thread-safe dictionary mapping owner character DbId to their dungeon instance.
		/// </summary>
		private readonly ConcurrentDictionary<long, InstanceDungeon> _instancesByOwner = new();

		/// <summary>
		/// Thread-safe dictionary mapping character DbId to their dungeon instance.
		/// </summary>
		private readonly ConcurrentDictionary<long, InstanceDungeon> _instancesByCharacter = new();

		/// <summary>
		/// Thread-safe dictionary mapping characters to their warp cancellation tokens.
		/// </summary>
		private readonly ConcurrentDictionary<Character, CancellationTokenSource> _warpCancellationTokens = new();

		/// <summary>
		/// Thread-safe dictionary mapping characters to their activity monitoring cancellation tokens.
		/// </summary>
		private readonly ConcurrentDictionary<Character, CancellationTokenSource> _activityCheckTokens = new();

		/// <summary>
		/// Thread-safe dictionary mapping instances to their timeout cancellation tokens.
		/// </summary>
		private readonly ConcurrentDictionary<InstanceDungeon, CancellationTokenSource> _instanceTimeoutTokens = new();

		/// <summary>
		/// Synchronization objects for instance creation per owner.
		/// Used to signal when an instance is ready for party members.
		/// </summary>
		private readonly ConcurrentDictionary<long, TaskCompletionSource<InstanceDungeon>> _instanceCreationTasks = new();

		#endregion

		/// <summary>
		/// Gets the configured empty dungeon timeout in seconds.
		/// </summary>
		private static int InstanceTimeoutSeconds => ZoneServer.Instance.Conf.World.InstancedDungeonEmptyTimeoutSeconds;

		/// <summary>
		/// The dungeon's unique id.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// The dungeon's name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// The dungeon's character(s) start position.
		/// </summary>
		public Position StartPosition { get; private set; }

		/// <summary>
		/// The dungeon's character(s) start direction.
		/// </summary>
		public Direction StartDirection { get; private set; } = Direction.South;

		/// <summary>
		/// The main map associated with this dungeon.
		/// </summary>
		public string MapName { get; private set; }

		#region Default Configuration

		/// <summary>
		/// Whether to use the default dungeon timer.
		/// Override this to false in derived classes to disable the automatic timer.
		/// </summary>
		protected virtual bool UseDefaultTimer => true;

		/// <summary>
		/// Default dungeon duration.
		/// Override this in derived classes to customize.
		/// </summary>
		protected virtual TimeSpan DefaultDungeonDuration => TimeSpan.FromMinutes(ZoneServer.Instance.Conf.World.InstancedDungeonDurationMinutes);

		/// <summary>
		/// Default warning messages for the dungeon timer.
		/// Override this in derived classes to customize.
		/// </summary>
		protected virtual Dictionary<TimeSpan, string> DefaultTimerWarnings => new()
		{
			{ TimeSpan.FromMinutes(10), "The dungeon will close in 10 minutes." },
			{ TimeSpan.FromMinutes(5), "The dungeon will close in 5 minutes." },
			{ TimeSpan.FromMinutes(1), "The dungeon will close in 1 minute!" }
		};

		/// <summary>
		/// Creates the default timer stage for this dungeon.
		/// Override to provide custom timer behavior or return null to disable.
		/// </summary>
		protected virtual DungeonTimerStage CreateDefaultTimer()
		{
			if (!UseDefaultTimer)
				return null;

			return new DungeonTimerStage(DefaultDungeonDuration, DefaultTimerWarnings, "default_timer");
		}

		/// <summary>
		/// Whether to show the reward HUD (kill percentage UI) for this dungeon.
		/// Override this to false in derived classes to disable the reward HUD.
		/// </summary>
		public virtual bool UseRewardHud => true;

		#endregion

		/// <summary>
		/// Creates custom background stages for this dungeon.
		/// Override to add dungeon-specific background behaviors.
		/// </summary>
		protected virtual List<BackgroundStage> CreateBackgroundStages()
		{
			return new List<BackgroundStage>();
		}

		#region Script Registry

		/// <summary>
		/// Attempts to get a dungeon script by its ID.
		/// </summary>
		/// <param name="dungeonId">The dungeon script ID.</param>
		/// <param name="dungeonScript">The dungeon script if found.</param>
		/// <returns>True if the script was found.</returns>
		public static bool TryGet(string dungeonId, out DungeonScript dungeonScript)
		{
			return Scripts.TryGetValue(dungeonId, out dungeonScript);
		}

		#endregion

		/// <summary>
		/// Returns the current player instance based on their character dbId.
		/// Thread-safe lookup across all registered dungeon scripts.
		/// </summary>
		public static InstanceDungeon GetInstance(long characterDbId)
		{
			foreach (var script in Scripts.Values)
			{
				if (script._instancesByCharacter.TryGetValue(characterDbId, out var instance))
					return instance;
			}
			return null;
		}

		/// <summary>
		/// Warps a list of characters to a dungeon. This is the central entry point
		/// for dungeon initialization, used by both manual entry and auto-match.
		/// The instance is created upfront before anyone warps, so all party members
		/// join the same instance regardless of warp order.
		/// </summary>
		/// <param name="characters">The characters to warp into the dungeon.</param>
		/// <param name="dungeonId">The instance dungeon ID.</param>
		/// <returns>True if the warp was initiated successfully.</returns>
		public static bool WarpPartyToDungeon(IList<Character> characters, int dungeonId)
		{
			if (characters == null || characters.Count == 0)
				return false;

			// Get dungeon data
			if (!ZoneServer.Instance.Data.InstanceDungeonDb.Entries.TryGetValue(dungeonId, out var dungeonData))
			{
				Log.Warning("DungeonScript.WarpPartyToDungeon: Unknown dungeon id ({0}).", dungeonId);
				return false;
			}

			// Try to get the dungeon script for start position
			DungeonScript dungeonScript = null;
			if (!string.IsNullOrEmpty(dungeonData.MiniGame))
				TryGet(dungeonData.MiniGame, out dungeonScript);

			// Ensure map data exists
			if (dungeonData.Map == null && dungeonScript != null && !string.IsNullOrEmpty(dungeonScript.MapName))
			{
				if (ZoneServer.Instance.Data.MapDb.TryFind(dungeonScript.MapName, out var dungeonMap))
				{
					dungeonData.MapName = dungeonScript.MapName;
					dungeonData.Map = dungeonMap;
				}
			}

			if (dungeonData.Map == null)
			{
				Log.Warning("DungeonScript.WarpPartyToDungeon: Dungeon '{0}' has no map data.", dungeonData.ClassName);
				return false;
			}

			// Determine start position
			var startPosition = dungeonScript?.StartPosition ?? new Position(0, 0, 0);

			// Find the party leader (or use first character for solo)
			Character leader = null;
			foreach (var character in characters)
			{
				if (character == null)
					continue;

				var party = character.Connection?.Party;
				if (party != null && party.IsLeader(character))
				{
					leader = character;
					break;
				}
			}

			// If no leader found, use first character
			if (leader == null)
				leader = characters.FirstOrDefault(c => c != null);

			if (leader == null)
				return false;

			// Pre-create the instance and register all party members BEFORE warping
			if (dungeonScript != null)
			{
				dungeonScript.CreateInstanceForParty(leader, characters.Where(c => c != null).ToList(), dungeonData);
			}

			// Warp all characters
			foreach (var character in characters)
			{
				if (character == null)
					continue;

				// Set dungeon variables
				character.Variables.Perm.SetInt(AutoMatchZoneManager.DungeonIdVarName, dungeonId);

				// Warp to dungeon
				character.Warp(dungeonData.Map.Id, startPosition);
			}

			Log.Info("DungeonScript.WarpPartyToDungeon: Warping {0} players to dungeon '{1}'.", characters.Count, dungeonData.ClassName);
			return true;
		}

		/// <summary>
		/// Creates a dungeon instance for a party and registers all members.
		/// Called before warping to ensure the instance exists when players enter.
		/// Signals waiting party members via TaskCompletionSource when the instance is ready.
		/// </summary>
		private void CreateInstanceForParty(Character leader, IList<Character> partyMembers, InstanceDungeonData dungeonData)
		{
			// Check if instance already exists for this leader
			if (_instancesByOwner.TryGetValue(leader.DbId, out var existingInstance))
			{
				// If the existing instance is complete, clean it up and create a new one
				if (existingInstance.IsComplete)
				{
					this.DestroyInstance(existingInstance);
				}
				else
				{
					// Active instance exists - register any new party members
					// that aren't already mapped (e.g. members who joined the
					// party after the first attempt, or whose mapping was lost)
					foreach (var member in partyMembers)
					{
						if (member != null && !_instancesByCharacter.ContainsKey(member.DbId))
						{
							_instancesByCharacter[member.DbId] = existingInstance;
							member.Variables.Perm.SetString(ActiveInstanceVarName, this.Id);

							if (!existingInstance.Characters.Contains(member))
								existingInstance.Characters.Add(member);
						}
					}
					return;
				}
			}

			// Create the instance
			var instance = InstanceDungeon.CreateNew(this.Id, dungeonData, dungeonData.Map, leader, partyMembers.ToList(), this.GetDungeonStages(), this.StartPosition);

			// Register owner and all party members
			_instancesByOwner[leader.DbId] = instance;
			foreach (var member in partyMembers)
			{
				if (member != null)
				{
					_instancesByCharacter[member.DbId] = instance;
					member.Variables.Perm.SetString(ActiveInstanceVarName, this.Id);
				}
			}

			// Signal any waiting party members that the instance is ready
			if (_instanceCreationTasks.TryRemove(leader.DbId, out var tcs))
			{
				tcs.TrySetResult(instance);
			}

			Log.Info("DungeonScript.CreateInstanceForParty: Pre-created instance '{0}' for {1} players.", instance.Id, partyMembers.Count);
		}

		/// <summary>
		/// Initializes the dungeon script.
		/// </summary>
		/// <returns>True if initialization succeeded.</returns>
		public override bool Init()
		{
			this.Load();

			// Dispose any existing script with the same ID to prevent duplicate event subscriptions.
			// This can happen when scripts are reloaded or when multiple script files define
			// the same dungeon ID.
			if (Scripts.TryGetValue(this.Id, out var existingScript) && existingScript != this)
			{
				Log.Warning("DungeonScript.Init: Replacing existing script for dungeon '{0}'. Disposing old script.", this.Id);
				existingScript.Dispose();
			}

			ZoneServer.Instance.ServerEvents.PlayerEnteredMap.Subscribe(this.OnMapEntryInternal);
			ZoneServer.Instance.ServerEvents.PlayerLeftMap.Subscribe(this.OnMapLeaveInternal);
			ZoneServer.Instance.ServerEvents.PlayerLeftParty.Subscribe(this.OnPlayerLeftParty);

			// Set map data if script defines it but database doesn't have it
			if (ZoneServer.Instance.Data.InstanceDungeonDb.TryGet(this.Id, out var dungeonData)
				&& !string.IsNullOrEmpty(this.MapName)
				&& (string.IsNullOrEmpty(dungeonData.MapName) || dungeonData.Map == null)
				&& ZoneServer.Instance.Data.MapDb.TryFind(this.MapName, out var map))
			{
				dungeonData.MapName = this.MapName;
				dungeonData.Map = map;
			}

			// Register this script (thread-safe via ConcurrentDictionary)
			Scripts[this.Id] = this;

			return base.Init();
		}

		protected void SetId(string id) => this.Id = id;
		protected void SetName(string name) => this.Name = name;
		protected void SetStartPosition(Position position) => this.StartPosition = position;
		protected void SetStartDirection(Direction direction) => this.StartDirection = direction;
		protected void SetMapName(string mapName) => this.MapName = mapName;

		/// <summary>
		/// Called once when a new dungeon instance is created, before any stages begin.
		/// Ideal for spawning persistent NPCs, objectives, add custom background stages
		/// and setting up initial state.
		/// </summary>
		public virtual void OnInstanceCreated(InstanceDungeon instance)
		{
			// Add default timer if enabled
			var defaultTimer = this.CreateDefaultTimer();
			if (defaultTimer != null)
			{
				instance.AddBackgroundStage(defaultTimer);
			}

			// Add any custom background stages
			var customStages = this.CreateBackgroundStages();
			foreach (var stage in customStages)
			{
				instance.AddBackgroundStage(stage);
			}
		}

		/// <summary>
		/// Called when a character enters the dungeon map.
		/// </summary>
		/// <param name="character"></param>
		protected virtual void OnMapEntry(Character character)
		{
			CallSafe(this.SetupPlayer(character));
		}

		/// <summary>
		/// Initializes the dungeon for a character entering the map.
		/// Uses TaskCompletionSource for proper synchronization between party members.
		/// </summary>
		/// <param name="character">The character entering the dungeon.</param>
		protected async Task SetupPlayer(Character character)
		{
			if (!ZoneServer.Instance.Data.InstanceDungeonDb.TryGetByMapClassName(character.Map.ClassName, out var dungeonData))
			{
				return;
			}

			if (dungeonData.Map == null)
			{
				var accountName = character.Connection?.Account?.Name ?? "Unknown";
				Log.Error("DungeonScript: User '{0}' joined a dungeon map '{1}' that has no map data.", accountName, character.Map.ClassName);
				return;
			}

			// Check if character has a stored active instance from permanent variables
			var storedInstanceId = character.Variables.Perm.GetString(ActiveInstanceVarName);
			if (!string.IsNullOrEmpty(storedInstanceId) && storedInstanceId == this.Id)
			{
				// Look for an active instance this character was part of
				var existingInstance = this.FindActiveInstanceForCharacter(character.DbId);
				if (existingInstance != null)
				{
					// Instance is still active - join it
					await this.JoinInstance(character, existingInstance);
					return;
				}
				else
				{
					// Instance no longer exists - clear the stale variable and
					// fall through to the normal join/creation paths below
					character.Variables.Perm.SetString(ActiveInstanceVarName, null);
					Log.Info("DungeonScript: Stored instance reference expired for character '{0}'. Trying normal join path.", character.Name);
				}
			}

			// Check if character is already mapped to an instance (e.g., party member warping in)
			if (_instancesByCharacter.TryGetValue(character.DbId, out var mappedInstance))
			{
				await this.JoinInstance(character, mappedInstance);
				return;
			}

			// Get party - players must be in a party to enter dungeons
			var party = character.Connection?.Party;
			if (party == null)
			{
				Log.Info("DungeonScript: Character '{0}' entered dungeon map without a party. Warping out.", character.Name);
				while (character.IsWarping) await Task.Delay(50);
				character.Warp(character.GetCityReturnLocation());
				return;
			}

			var leaderCharacter = party.GetLeader();
			if (leaderCharacter == null)
			{
				Log.Info("DungeonScript: Party has no leader for character '{0}'. Warping out.", character.Name);
				while (character.IsWarping) await Task.Delay(50);
				character.Warp(character.GetCityReturnLocation());
				return;
			}

			var partyMembers = party.GetPartyMembers();

			// If we're not the leader, wait for the instance to be created by the leader
			if (leaderCharacter.DbId != character.DbId)
			{
				// Use TaskCompletionSource for proper synchronization instead of polling
				var tcs = _instanceCreationTasks.GetOrAdd(leaderCharacter.DbId, _ => new TaskCompletionSource<InstanceDungeon>());

				try
				{
					// Wait with timeout for instance creation
					using var cts = new CancellationTokenSource(InstanceCreationWaitTimeoutMs);
					cts.Token.Register(() => tcs.TrySetCanceled());

					// First check if instance was already created before we started waiting
					if (_instancesByCharacter.TryGetValue(character.DbId, out var existingInstance))
					{
						await this.JoinInstance(character, existingInstance);
						return;
					}

					// Wait for the leader to signal instance creation
					var instance = await tcs.Task;

					// Verify we're mapped to this instance
					if (_instancesByCharacter.TryGetValue(character.DbId, out var newInstance))
					{
						await this.JoinInstance(character, newInstance);
					}
					else
					{
						Log.Warning("DungeonScript: Non-leader '{0}' instance was created but character not mapped. Warping out.", character.Name);
						while (character.IsWarping) await Task.Delay(50);
						character.Warp(character.GetCityReturnLocation());
					}
				}
				catch (TaskCanceledException)
				{
					Log.Warning("DungeonScript: Non-leader '{0}' timed out waiting for instance creation ({1}ms). Warping out.", character.Name, InstanceCreationWaitTimeoutMs);
					while (character.IsWarping) await Task.Delay(50);
					character.Warp(character.GetCityReturnLocation());
				}
				finally
				{
					// Clean up the TCS if we were the last one waiting
					_instanceCreationTasks.TryRemove(leaderCharacter.DbId, out _);
				}
				return;
			}

			// Leader logic: check if instance already exists
			if (_instancesByOwner.TryGetValue(leaderCharacter.DbId, out var existingLeaderInstance))
			{
				// If the existing instance is complete, clean it up and create a new one
				if (existingLeaderInstance.IsComplete)
				{
					this.DestroyInstance(existingLeaderInstance);
				}
				else
				{
					// Active instance exists, don't create a new one
					return;
				}
			}

			// Create a TCS for party members to wait on
			var creationTcs = _instanceCreationTasks.GetOrAdd(leaderCharacter.DbId, _ => new TaskCompletionSource<InstanceDungeon>());

			// Create instance immediately and register all party members
			// This ensures non-leaders can find their instance mapping right away
			var newDungeonInstance = InstanceDungeon.CreateNew(this.Id, dungeonData, dungeonData.Map, leaderCharacter, partyMembers, this.GetDungeonStages(), this.StartPosition);

			_instancesByOwner[leaderCharacter.DbId] = newDungeonInstance;
			foreach (var member in partyMembers)
			{
				if (member != null)
				{
					_instancesByCharacter[member.DbId] = newDungeonInstance;
					// Store instance reference in permanent variables for reconnection
					member.Variables.Perm.SetString(ActiveInstanceVarName, this.Id);
				}
			}

			// Signal waiting party members that the instance is ready
			creationTcs.TrySetResult(newDungeonInstance);
			_instanceCreationTasks.TryRemove(leaderCharacter.DbId, out _);

			// Wait for all members to finish warping before starting the dungeon
			var warpWaitStart = DateTime.UtcNow;
			foreach (var memberCharacter in partyMembers)
			{
				if (memberCharacter == null) continue;
				while (memberCharacter.IsWarping && (DateTime.UtcNow - warpWaitStart) < MaxWarpWaitTime)
					await Task.Delay(50);
			}

			await newDungeonInstance.StartDungeon(this);
		}

		/// <summary>
		/// Finds an active instance that the character was previously part of.
		/// </summary>
		private InstanceDungeon FindActiveInstanceForCharacter(long characterDbId)
		{
			// First check direct mapping
			if (_instancesByCharacter.TryGetValue(characterDbId, out var instance))
				return instance;

			// Check all instances to see if character was an original member
			foreach (var inst in _instancesByOwner.Values)
			{
				if (inst.Characters.Any(c => c?.DbId == characterDbId))
					return inst;
			}

			return null;
		}

		/// <summary>
		/// Handles joining an existing instance - either first-time entry for a pre-created
		/// instance or rejoining an already-running dungeon.
		/// </summary>
		private async Task JoinInstance(Character character, InstanceDungeon instance)
		{
			while (character.IsWarping) await Task.Delay(50);

			// If dungeon hasn't started yet (pre-created instance), start it
			if (!instance.IsStarted)
			{
				await this.StartPreCreatedInstance(character, instance);
				return;
			}

			// Dungeon already running - this is a rejoin
			await this.RejoinInstance(character, instance);
		}

		/// <summary>
		/// Starts a pre-created instance when the first player enters.
		/// Waits for all party members to enter before fully starting.
		/// </summary>
		private async Task StartPreCreatedInstance(Character character, InstanceDungeon instance)
		{
			// Cancel any pending timeout since a player is entering the pre-created instance.
			// This handles the race condition where a timeout was started when the player
			// left their previous map (before entering the dungeon map).
			this.CancelInstanceTimeout(instance);

			// Add character to instance's character list if not already present
			if (!instance.Characters.Contains(character))
			{
				instance.Characters.Add(character);
			}

			// Wait for all registered party members to finish warping
			var partyMembers = _instancesByCharacter
				.Where(kvp => kvp.Value == instance)
				.Select(kvp => ZoneServer.Instance.World.GetCharacter(c => c.DbId == kvp.Key))
				.Where(c => c != null)
				.ToList();

			var warpWaitStart = DateTime.UtcNow;
			foreach (var member in partyMembers)
			{
				while (member.IsWarping && (DateTime.UtcNow - warpWaitStart) < MaxWarpWaitTime)
					await Task.Delay(50);
			}

			// Start the dungeon (only the first caller will actually start it due to IsStarted check)
			await instance.StartDungeon(this);

			// Set up this character for the dungeon
			character.SetPosition(instance.StartPosition);
			character.Layer = instance.Layer;
			character.LookAround();
			character.Dungeon.InstanceDungeon = instance;

			// Null-safe connection access for sending packets
			if (character.Connection != null)
			{
				Send.ZC_NORMAL.SetMapMode(character.Connection, "Indun");
				Send.ZC_NORMAL.IndunAddonMsgParam(character.Connection, 2, "START", 1);
			}

			instance.OpenDungeonRewardHudForCharacter(character);
			this.StartActivityMonitoring(character, instance);

			Log.Info("DungeonScript: Character '{0}' entered pre-created instance '{1}'.", character.Name, instance.Id);
		}

		/// <summary>
		/// Handles rejoining an already-running instance.
		/// </summary>
		private async Task RejoinInstance(Character character, InstanceDungeon instance)
		{
			// Check if rejoining is allowed by config
			if (!ZoneServer.Instance.Conf.World.InstancedDungeonAllowRejoin)
			{
				// Clear all dungeon variables and warp player out
				character.Variables.Perm.SetString(ActiveInstanceVarName, null);
				character.Variables.Perm.SetInt(AutoMatchZoneManager.DungeonIdVarName, 0);
				character.Variables.Perm.SetLong(AutoMatchZoneManager.SessionIdVarName, 0);
				_instancesByCharacter.TryRemove(character.DbId, out _);
				Log.Info("DungeonScript: Character '{0}' attempted to rejoin but rejoining is disabled. Warping out.", character.Name);
				character.Warp(character.GetCityReturnLocation());
				return;
			}

			// Cancel any pending timeout since a player is rejoining
			this.CancelInstanceTimeout(instance);

			// Restore character mapping if it was cleared
			_instancesByCharacter.TryAdd(character.DbId, instance);

			// Add character back to instance's character list if not already present
			if (!instance.Characters.Contains(character))
			{
				instance.Characters.Add(character);
			}

			// Restore permanent variable reference
			character.Variables.Perm.SetString(ActiveInstanceVarName, this.Id);

			character.SetPosition(instance.StartPosition);
			character.Layer = instance.Layer;
			character.LookAround();
			character.Dungeon.InstanceDungeon = instance;

			// Null-safe connection access for sending packets
			if (character.Connection != null)
			{
				Send.ZC_NORMAL.SetMapMode(character.Connection, "Indun");
				Send.ZC_NORMAL.IndunAddonMsgParam(character.Connection, 2, "START", 1);
			}

			// Open the reward HUD for the rejoining player
			instance.OpenDungeonRewardHudForCharacter(character);

			// Start activity monitoring for rejoining player
			this.StartActivityMonitoring(character, instance);

			Log.Info("DungeonScript: Character '{0}' rejoined instance '{1}'.", character.Name, instance.Id);
		}

		/// <summary>
		/// Starts periodic activity monitoring for a character in the dungeon.
		/// Checks if the dungeon is still active and warps them out if not.
		/// </summary>
		private void StartActivityMonitoring(Character character, InstanceDungeon instance)
		{
			// Cancel and dispose any existing monitoring
			if (_activityCheckTokens.TryRemove(character, out var existingCts))
			{
				existingCts.Cancel();
				existingCts.Dispose();
			}

			var cts = new CancellationTokenSource();
			_activityCheckTokens[character] = cts;

			CallSafe(this.MonitorDungeonActivity(character, instance, cts));
		}

		/// <summary>
		/// Monitors if a dungeon is still active for a character.
		/// Properly disposes CTS and handles all exit paths.
		/// </summary>
		private async Task MonitorDungeonActivity(Character character, InstanceDungeon instance, CancellationTokenSource cts)
		{
			var token = cts.Token;
			try
			{
				while (!token.IsCancellationRequested)
				{
					await Task.Delay(TimeSpan.FromSeconds(5), token);

					// Check if character is still in the dungeon map
					if (character.MapId != instance.MapId)
						break;

					// Check if the instance is still valid
					if (!_instancesByCharacter.ContainsKey(character.DbId))
					{
						Log.Warning("DungeonScript: Character {0} is in dungeon map but has no active instance. Warping out.", character.Name);
						character.Variables.Perm.SetString(ActiveInstanceVarName, null);
						character.Variables.Perm.SetInt(AutoMatchZoneManager.DungeonIdVarName, 0);
						character.Variables.Perm.SetLong(AutoMatchZoneManager.SessionIdVarName, 0);
						instance.Characters.Remove(character);

						if (instance.Owner?.DbId == character.DbId)
							_instancesByOwner.TryRemove(character.DbId, out _);

						character.Warp(character.GetCityReturnLocation());
						break;
					}

					// Check if the character is still in the dungeon party (null-safe)
					var partyId = character.Connection?.Party?.DbId ?? 0;
					if (instance.PartyId != 0 && partyId != instance.PartyId)
					{
						Log.Warning("DungeonScript: Character {0} is no longer in the dungeon party. Warping out.", character.Name);
						_instancesByCharacter.TryRemove(character.DbId, out _);
						character.Variables.Perm.SetString(ActiveInstanceVarName, null);
						character.Variables.Perm.SetInt(AutoMatchZoneManager.DungeonIdVarName, 0);
						character.Variables.Perm.SetLong(AutoMatchZoneManager.SessionIdVarName, 0);
						instance.Characters.Remove(character);

						// If this character owns the instance, remove the owner
						// mapping so a future re-entry creates a fresh instance
						if (instance.Owner?.DbId == character.DbId)
							_instancesByOwner.TryRemove(character.DbId, out _);

						character.Warp(character.GetCityReturnLocation());
						break;
					}

					// Check if the dungeon is complete
					if (instance.IsComplete)
					{
						// Dungeon completed - stop monitoring
						// (DungeonComplete already handles rewards, portal, and auto-warp)
						break;
					}
				}
			}
			catch (TaskCanceledException) { }
			finally
			{
				// Always clean up and dispose the CTS
				_activityCheckTokens.TryRemove(character, out _);
				cts.Dispose();
			}
		}

		/// <summary>
		/// Starts the instance timeout countdown. If no player joins within
		/// InstanceTimeoutSeconds, the instance is destroyed.
		/// </summary>
		private void StartInstanceTimeout(InstanceDungeon instance)
		{
			// Cancel any existing timeout for this instance
			this.CancelInstanceTimeout(instance);

			var cts = new CancellationTokenSource();
			_instanceTimeoutTokens[instance] = cts;

			instance.Vars.Set(EmptyStartTimeVarName, DateTime.UtcNow);
			Log.Debug("DungeonScript: Instance '{0}' is now empty. Starting {1} second timeout.", instance.Id, InstanceTimeoutSeconds);

			CallSafe(this.MonitorInstanceTimeout(instance, cts));
		}

		/// <summary>
		/// Cancels the instance timeout countdown (called when a player rejoins).
		/// Does NOT dispose the CTS - that's handled by MonitorInstanceTimeout's finally block.
		/// </summary>
		private void CancelInstanceTimeout(InstanceDungeon instance)
		{
			if (_instanceTimeoutTokens.TryGetValue(instance, out var existingCts))
			{
				existingCts.Cancel();
				instance.Vars.Remove(EmptyStartTimeVarName);
				Log.Debug("DungeonScript: Instance '{0}' timeout cancelled - player rejoined.", instance.Id);
			}
		}

		/// <summary>
		/// Monitors an empty instance and destroys it after timeout.
		/// Properly disposes the CancellationTokenSource on exit.
		/// </summary>
		private async Task MonitorInstanceTimeout(InstanceDungeon instance, CancellationTokenSource cts)
		{
			var token = cts.Token;
			try
			{
				while (!token.IsCancellationRequested)
				{
					await Task.Delay(InstanceTimeoutCheckIntervalMs, token);

					// Check cancellation again after delay (race condition protection)
					if (token.IsCancellationRequested)
						break;

					if (!instance.Vars.Has(EmptyStartTimeVarName))
						break;

					var emptyStartTime = instance.Vars.Get<DateTime>(EmptyStartTimeVarName);
					var elapsedSeconds = (DateTime.UtcNow - emptyStartTime).TotalSeconds;

					if (elapsedSeconds >= InstanceTimeoutSeconds)
					{
						// Re-check cancellation to close the race window between
						// the while-loop check and reaching this point. Without
						// this, CancelInstanceTimeout can fire after the loop
						// check passes but before DungeonEnded runs.
						if (token.IsCancellationRequested)
							break;

						// Final safety check: verify no players have rejoined before destroying
						// This prevents a race where a player rejoins after the elapsed check
						// but before DungeonEnded is called
						var anyMemberInside = instance.Characters.Any(c => c != null && c.MapId == instance.MapId);
						if (anyMemberInside)
						{
							Log.Debug("DungeonScript: Instance '{0}' timeout aborted - player detected inside.", instance.Id);
							break;
						}

						Log.Info("DungeonScript: Instance '{0}' timed out after {1} seconds with no players. Destroying.", instance.Id, InstanceTimeoutSeconds);
						// Use DungeonEnded to properly clean up character variables and auto-match session
						this.DungeonEnded(instance, false);
						break;
					}
				}
			}
			catch (TaskCanceledException)
			{
				// Expected when token is cancelled (player rejoined)
			}
			finally
			{
				// Always clean up and dispose the CTS
				_instanceTimeoutTokens.TryRemove(instance, out _);
				cts.Dispose();
			}
		}

		/// <summary>
		/// Checks if an instance is still active (exists and not timed out).
		/// </summary>
		private bool IsInstanceActive(InstanceDungeon instance)
		{
			return instance != null && _instancesByOwner.Values.Contains(instance);
		}

		/// <summary>
		/// Forces the instance to end due to duration expiration.
		/// Warps all players out before destroying the instance.
		/// </summary>
		public void ForceInstanceExpiration(InstanceDungeon instance)
		{

			foreach (var character in instance.Characters)
			{
				if (character?.Connection == null)
					continue;

				character.AddonMessage("NOTICE_Dm_!", "Dungeon time has expired!", 5);

				if (character.MapId == instance.MapId)
				{
					character.Warp(character.GetCityReturnLocation());
				}
			}

			this.DungeonEnded(instance, false);
		}

		/// <summary>
		/// Destroys an instance completely, clearing all character associations.
		/// </summary>
		private void DestroyInstance(InstanceDungeon instance)
		{
			// Cancel any pending timeout
			this.CancelInstanceTimeout(instance);

			// Stop all background stages and clean up
			instance.Cleanup();

			// Clear character mappings for all characters that were associated
			foreach (var kvp in _instancesByCharacter)
			{
				if (kvp.Value == instance)
				{
					_instancesByCharacter.TryRemove(kvp.Key, out _);
				}
			}

			// Remove owner association only if this instance is still the one mapped
			if (instance.Owner != null)
			{
				if (_instancesByOwner.TryGetValue(instance.Owner.DbId, out var mappedInstance) && mappedInstance == instance)
				{
					_instancesByOwner.TryRemove(instance.Owner.DbId, out _);
				}
			}

			// Clean up any remaining stage state
			instance.CurrentStage?.Complete();

			Log.Info("DungeonScript: Instance '{0}' has been destroyed.", instance.Id);
		}

		public virtual Mob SpawnMonster(InstanceDungeon instance, int monsterId, Position position, string mapName = null, DungeonStage stage = null)
		{
			var targetMapName = mapName ?? this.MapName;
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(monsterId, out var monsterData))
			{
				throw new ArgumentException($"SpawnMonster: Monster '{monsterId}' not found.");
			}

			if (!ZoneServer.Instance.World.TryGetMap(this.MapName, out var map))
			{
				throw new ArgumentException($"SpawnMonster: MapName '{this.MapName}' not found.");
			}

			if (!map.Ground.IsValidPosition(position) && map.Ground.TryGetNearestValidPosition(position, 50, out var validPos))
			{
				position = validPos;
			}

			var monster = new Mob(monsterData.Id, RelationType.Enemy)
			{
				Position = position,
				SpawnPosition = position,
				Layer = instance.Layer,
				Tendency = TendencyType.Aggressive
			};

			var movement = new MovementComponent(monster) { ShowMinimapMarker = true };
			monster.Components.Add(movement);
			monster.Components.Add(new AiComponent(monster, "BasicMonster"));

			map.AddMonster(monster);

			// Use provided stage, or fall back to current stage
			var targetStage = stage ?? instance.CurrentStage;
			targetStage?.AddMonster(instance, monster);

			return monster;
		}

		protected virtual void SpawnExitPortal(InstanceDungeon instance)
		{
			if (ZoneServer.Instance.World.TryGetMap(this.MapName, out var map))
			{
				var portal = new Npc(MonsterId.MissionGate, "Exit Portal", instance.Characters.FirstOrDefault().Position.GetRandomInRange2D(20, 30), instance.Characters.FirstOrDefault().Direction.Backwards)
				{
					Layer = instance.Layer
				};
				portal.AddEffect(new AttachEffect(AnimationName.Portal, 1, EffectLocation.Top));
				map.AddMonster(portal);

				portal.SetClickTrigger("ExitPortalDialog", async (dialog) =>
				{
					dialog.Player.Warp(dialog.Player.GetCityReturnLocation());
					this.DungeonEnded(instance, false);
				});
			}
		}

		/// <summary>
		/// Called when any monster is killed. This handles all stage progression logic.
		/// Supports both exact kill count objectives and percentage-based objectives.
		/// </summary>
		public virtual void MonsterKilled(InstanceDungeon instance, Mob monster)
		{
			if (instance.CurrentStage == null) return;

			var objectiveProgressed = false;

			foreach (var objective in instance.CurrentStage.Objectives)
			{
				// Handle standard kill objectives (exact count per monster type)
				if (objective is KillObjective killObjective
					&& !killObjective.IsCompleted
					&& killObjective.MonsterIds.Contains(monster.Id))
				{
					killObjective.IncrementProgress();
					objectiveProgressed = true;
				}
				// Handle percentage-based kill objectives
				else if (objective is PercentageKillObjective percentageObjective
					&& !percentageObjective.IsCompleted)
				{
					percentageObjective.RecordKill();
					objectiveProgressed = true;
				}
			}

			// Also notify KillMonstersStage specifically for its internal tracking
			if (instance.CurrentStage is KillMonstersStage killMonstersStage)
			{
				killMonstersStage.OnMonsterKilled();
				// KillMonstersStage uses its own completion logic via GetDeadMonstersPercentage()
				// so we always consider progress made when a monster dies in this stage type
				objectiveProgressed = true;
			}

			if (objectiveProgressed && instance.CurrentStage.IsObjectiveComplete())
			{
				// Fire-and-forget the async stage transition to avoid blocking the event handler.
				CallSafe(instance.MoveToNextStage(this));
			}

			// Note: UpdateDungeonRewardHud is now called from InstanceDungeon.RegisterMonsterKill()
			// to ensure it's called even for monsters not tracked by objectives
		}

		/// <summary>
		/// Called when a boss is killed in the dungeon instance.
		/// Override this to add custom boss death behavior.
		/// </summary>
		/// <param name="instance">The dungeon instance</param>
		/// <param name="boss">The boss that was killed</param>
		public virtual void BossKilled(InstanceDungeon instance, Mob boss)
		{
			// Update the reward HUD immediately
			instance.UpdateDungeonRewardHud();

			Send.ZC_ADDON_MSG(boss, AddonMessage.BOSS_CLEAR, boss.Id, boss.Handle.ToString());
			Send.ZC_NORMAL.SlowMotion(boss, 0.3f, 3);

			// Check if dungeon objectives are complete
			if (instance.CurrentStage != null && instance.CurrentStage.IsObjectiveComplete())
			{
				CallSafe(instance.MoveToNextStage(this));
			}
		}

		public virtual Npc SpawnNpc(InstanceDungeon instance, int monsterId, string name, Position position, Direction direction, string mapName = null)
		{
			if (!ZoneServer.Instance.World.TryGetMap(this.MapName, out var map))
			{
				throw new ArgumentException($"SpawnNpc: MapName '{this.MapName}' not found.");
			}

			var npc = new Npc(monsterId, name, position, direction)
			{
				Layer = instance.Layer
			};

			map.AddMonster(npc);
			instance.CurrentStage?.AddNpc(instance, npc);

			return npc;
		}

		/// <summary>
		/// Called by InstanceDungeon when all stages are complete.
		/// Handles entry count tracking to prevent double-increment.
		/// </summary>
		public void DungeonComplete(InstanceDungeon instance)
		{
			if (instance.IsComplete) return;
			instance.IsComplete = true;
			instance.Vars.Set(CompletionTimeVarName, DateTime.UtcNow);

			this.OnDungeonComplete(instance);

			// Check if entry count should be incremented on complete and hasn't been already
			var incrementOnComplete = ZoneServer.Instance.Conf.World.InstancedDungeonIncrementEntryOnComplete;
			var alreadyIncremented = instance.Vars.GetBool(InstanceDungeon.EntryCountIncrementedVarName);

			foreach (var character in instance.Characters)
			{
				if (character == null) continue;

				// Increment entry count on complete if configured and not already incremented on enter
				if (incrementOnComplete && !alreadyIncremented && character.Connection != null)
				{
					character.Dungeon.IncreaseEntryCount(instance.DungeonId, 1);
				}

				// Null-safe connection access
				if (character.Connection != null)
				{
					Send.ZC_NORMAL.IndunAddonMsgParam(character.Connection, 2, "CLEAR", 1);
				}
				character.AddonMessage("NOTICE_Dm_raid_clear", this.Name, 10);

				// Start activity monitoring
				this.StartActivityMonitoring(character, instance);

				// Give rewards immediately
				this.GiveDungeonRewards(instance, character);
			}

			// Mark entry count as incremented to prevent double-increment
			if (incrementOnComplete && !alreadyIncremented)
			{
				instance.Vars.Set(InstanceDungeon.EntryCountIncrementedVarName, true);
			}

			// Display the results screen
			this.DisplayDungeonResults(instance);

			// Spawn exit portal
			this.SpawnExitPortal(instance);

			// Destroy the auto-match session before clearing character variables.
			// Must happen first because DestroyDungeonSession reads SessionIdVarName
			// from party members, and we're about to clear it.
			long autoMatchId = 0;
			if (instance.Owner != null)
				autoMatchId = instance.Owner.Variables.Perm.GetLong(AutoMatchZoneManager.SessionIdVarName);

			if (autoMatchId == 0)
			{
				foreach (var character in instance.Characters)
				{
					if (character == null) continue;
					autoMatchId = character.Variables.Perm.GetLong(AutoMatchZoneManager.SessionIdVarName);
					if (autoMatchId != 0) break;
				}
			}

			if (autoMatchId != 0)
				ZoneServer.Instance.World.AutoMatch.DestroyDungeonSession(autoMatchId);

			// Start auto-warp timer for all characters
			foreach (var character in instance.Characters)
			{
				if (character == null) continue;

				// Only process if the character is still associated with THIS instance
				// This prevents stale dungeon completions from interfering with new instances
				if (!_instancesByCharacter.TryGetValue(character.DbId, out var mappedInstance) || mappedInstance != instance)
					continue;

				// Cancel any existing warp token before creating a new one
				if (_warpCancellationTokens.TryRemove(character, out var existingCts))
				{
					existingCts.Cancel();
					existingCts.Dispose();
				}

				var cts = new CancellationTokenSource();
				_warpCancellationTokens[character] = cts;
				CallSafe(this.AutoWarpCharacter(character, instance, cts.Token));

				// Clear all instance tracking so player can start a new dungeon immediately
				_instancesByCharacter.TryRemove(character.DbId, out _);
				character.Variables.Perm.SetString(ActiveInstanceVarName, null);
				character.Variables.Perm.SetInt(AutoMatchZoneManager.DungeonIdVarName, 0);
				character.Variables.Perm.SetLong(AutoMatchZoneManager.SessionIdVarName, 0);
			}

			// Remove owner association only if this instance is still the one mapped
			if (instance.Owner != null)
			{
				if (_instancesByOwner.TryGetValue(instance.Owner.DbId, out var mappedInstance) && mappedInstance == instance)
				{
					_instancesByOwner.TryRemove(instance.Owner.DbId, out _);
				}
			}
		}

		/// <summary>
		/// Displays the dungeon completion results to all players.
		/// </summary>
		private void DisplayDungeonResults(InstanceDungeon instance)
		{
			var completionRewardMultiply = instance.GetCompletionRewardMultiplier();
			var monsterKillPercentage = instance.InstanceKillPercentage;
			var rank = completionRewardMultiply; // Using multiplier as rank for simplicity

			var rankText = rank switch
			{
				5 => "S",
				4 => "A",
				3 => "B",
				2 => "C",
				_ => "D"
			};

			foreach (var character in instance.Characters)
			{
				if (character == null) continue;

				// Show completion message with rank
				character.AddonMessage("NOTICE_Dm_clear",
					$"{{nl}}Rank: {rankText}{{nl}}Monsters Defeated: {monsterKillPercentage}%{{nl}}Reward Multiplier: x{completionRewardMultiply}",
					10);
			}
		}

		/// <summary>
		/// Auto-warps a character out of the dungeon after the configured delay.
		/// Properly disposes the CTS on completion.
		/// </summary>
		private async Task AutoWarpCharacter(Character character, InstanceDungeon instance, CancellationToken token)
		{
			try
			{
				await Task.Delay(AutoWarpDelay, token);

				if (token.IsCancellationRequested || character.MapId != instance.MapId) return;

				character.Warp(character.GetCityReturnLocation());

				// After warping, check if dungeon is complete and all characters have left
				// If so, end the dungeon properly instead of waiting for timeout
				if (instance.IsComplete)
				{
					await Task.Delay(500, token); // Small delay to ensure warp is processed

					var anyMemberInside = instance.Characters.Any(c => c != null && c.MapId == instance.MapId);
					if (!anyMemberInside)
					{
						// Use DungeonEnded to properly clean up character variables and auto-match session
						this.DungeonEnded(instance, false);
					}
				}
			}
			catch (TaskCanceledException) { }
			finally
			{
				// Clean up and dispose the token
				if (_warpCancellationTokens.TryRemove(character, out var cts))
				{
					cts.Dispose();
				}
			}
		}

		public virtual void OnDungeonComplete(InstanceDungeon instance) { }

		/// <summary>
		/// Called for each character as part of the DungeonEnded cleanup process.
		/// Properly disposes cancellation tokens.
		/// </summary>
		public virtual void OnDungeonExit(Character character)
		{
			// Clear permanent instance reference
			character.Variables.Perm.SetString(ActiveInstanceVarName, null);

			// Stop and dispose activity monitoring token
			if (_activityCheckTokens.TryRemove(character, out var cts))
			{
				cts.Cancel();
				cts.Dispose();
			}

			// Stop and dispose warp token if active
			if (_warpCancellationTokens.TryRemove(character, out var warpCts))
			{
				warpCts.Cancel();
				warpCts.Dispose();
			}
		}

		/// <summary>
		/// Called when a character leaves the dungeon map.
		/// Handles cleanup and timeout management.
		/// </summary>
		protected virtual void OnMapLeave(Character character)
		{
			// Cancel and dispose auto-warp if active
			if (_warpCancellationTokens.TryRemove(character, out var cts))
			{
				cts.Cancel();
				cts.Dispose();
			}

			// Cancel and dispose activity monitoring
			if (_activityCheckTokens.TryRemove(character, out var activityCts))
			{
				activityCts.Cancel();
				activityCts.Dispose();
			}

			if (_instancesByCharacter.TryGetValue(character.DbId, out var instance))
			{
				// Only process if character is actually leaving the dungeon map
				// (not leaving their origin map during initial warp to the dungeon)
				// Note: We use character.Map?.Id instead of character.MapId because MapId
				// is updated to the destination map BEFORE the character leaves the origin map,
				// but character.Map still points to the origin map when this event fires.
				var currentMapId = character.Map?.Id ?? 0;
				if (currentMapId != instance.MapId)
					return;

				// Check if any member is still inside the dungeon
				var anyMemberInside = instance.Characters.Any(member => member != null && member.Map?.Id == instance.MapId && member.DbId != character.DbId);

				if (!anyMemberInside)
				{
					// No online members inside - start timeout instead of immediate destruction
					// The instance will be destroyed after InstanceTimeoutSeconds if no one rejoins
					this.StartInstanceTimeout(instance);
					Log.Debug("DungeonScript: Last player '{0}' left instance '{1}'. Timeout started.", character.Name, instance.Id);
				}
			}
		}

		/// <summary>
		/// Central cleanup function for the entire dungeon instance.
		/// Handles all cleanup including auto-match session, character mappings, and resources.
		/// </summary>
		public virtual void DungeonEnded(InstanceDungeon instance, bool sendFailMessage)
		{
			// Cancel any pending instance timeout
			this.CancelInstanceTimeout(instance);

			// Stop all background stages
			instance.Cleanup();

			// Destroy the auto-match session if it exists (check any character for the session ID)
			long autoMatchId = 0;
			if (instance.Owner != null)
			{
				autoMatchId = instance.Owner.Variables.Perm.GetLong(AutoMatchZoneManager.SessionIdVarName);
			}
			else
			{
				// If owner is null, try to find the session ID from any character
				foreach (var character in instance.Characters)
				{
					if (character == null) continue;
					autoMatchId = character.Variables.Perm.GetLong(AutoMatchZoneManager.SessionIdVarName);
					if (autoMatchId != 0) break;
				}
			}

			if (autoMatchId != 0)
			{
				ZoneServer.Instance.World.AutoMatch.DestroyDungeonSession(autoMatchId);
			}

			// Complete the current stage if any
			instance.CurrentStage?.Complete();

			// Clean up all characters
			foreach (var character in instance.Characters)
			{
				if (character == null) continue;

				// Only clean up if the character is still associated with THIS instance
				// This prevents stale timeout monitors from interfering with new instances
				if (!_instancesByCharacter.TryGetValue(character.DbId, out var mappedInstance) || mappedInstance != instance)
					continue;

				// Clear dungeon-related variables
				character.Variables.Perm.SetInt(AutoMatchZoneManager.DungeonIdVarName, 0);
				character.Variables.Perm.SetLong(AutoMatchZoneManager.SessionIdVarName, 0);

				this.OnDungeonExit(character);

				// Null-safe connection access
				if (sendFailMessage && character.Connection != null)
					Send.ZC_NORMAL.IndunAddonMsgParam(character.Connection, 2, "FAIL", 1);

				character.StopLayer();
				_instancesByCharacter.TryRemove(character.DbId, out _);

				// If the character is offline, persist the cleared dungeon
				// variables to the database. Without this, the var changes
				// only exist on the orphaned in-memory object and are lost
				// because OnClosed already saved stale values on disconnect.
				if (!character.IsOnline)
				{
					try
					{
						ZoneServer.Instance.Database.SaveCharacterData(character);
					}
					catch (Exception ex)
					{
						Log.Error("DungeonScript: Failed to save cleared dungeon vars for offline character '{0}': {1}", character.Name, ex);
					}
				}
			}

			// Remove owner association if owner exists
			// Only remove if this instance is still the one mapped
			if (instance.Owner != null)
			{
				if (_instancesByOwner.TryGetValue(instance.Owner.DbId, out var mappedOwnerInstance) && mappedOwnerInstance == instance)
				{
					_instancesByOwner.TryRemove(instance.Owner.DbId, out _);
				}
			}

			Log.Info("DungeonScript: Instance '{0}' ended.", instance.Id);
		}

		#region Event Handlers

		private void OnMapEntryInternal(object sender, PlayerEventArgs e)
		{
			if (e.Character.Map?.Data?.ClassName == this.MapName)
			{
				this.OnMapEntry(e.Character);
			}
		}

		private void OnMapLeaveInternal(object sender, PlayerEventArgs e)
		{
			if (_instancesByCharacter.ContainsKey(e.Character.DbId))
			{
				this.OnMapLeave(e.Character);
			}
		}

		/// <summary>
		/// Handles a player leaving their party while in a dungeon.
		/// Properly handles ownership transfer with edge case where no new owner exists.
		/// </summary>
		private void OnPlayerLeftParty(object sender, PlayerEventArgs e)
		{
			var character = e.Character;
			if (character == null)
				return;

			// Only handle if this script owns the instance the character is in
			if (!_instancesByCharacter.TryGetValue(character.DbId, out var instance))
				return;

			// Check remaining members BEFORE removing this character
			// We need to determine if this is the last member leaving
			var remainingMembersCount = _instancesByCharacter.Count(kvp => kvp.Value == instance);
			var isLastMember = remainingMembersCount <= 1; // 1 = just this character

			// If this is the last member, call DungeonEnded BEFORE any cleanup
			// This ensures Owner is still valid and Characters list contains this character
			// so all cleanup (OnDungeonExit, auto-match session destroy, etc.) works correctly
			if (isLastMember)
			{
				Log.Info("DungeonScript: Last member '{0}' leaving instance '{1}'. Destroying instance.", character.Name, instance.Id);
				this.DungeonEnded(instance, false);
				character.Warp(character.GetCityReturnLocation());
				return;
			}

			// Not the last member - do partial cleanup for this character only
			// Only remove if the character is still mapped to THIS instance
			// This prevents stale party leave events from interfering with new instances
			if (_instancesByCharacter.TryGetValue(character.DbId, out var mappedInstance) && mappedInstance == instance)
			{
				_instancesByCharacter.TryRemove(character.DbId, out _);
				character.Variables.Perm.SetString(ActiveInstanceVarName, null);
				character.Variables.Perm.SetInt(AutoMatchZoneManager.DungeonIdVarName, 0);
				character.Variables.Perm.SetLong(AutoMatchZoneManager.SessionIdVarName, 0);

				// Call OnDungeonExit for this character to clean up their tokens
				this.OnDungeonExit(character);

				// Remove character from instance's character list
				instance.Characters.Remove(character);
			}

			// If the leaving character is the instance owner, transfer ownership to another member
			if (instance.Owner == character)
			{
				// Find another remaining member to transfer ownership to
				var newOwner = _instancesByCharacter
					.Where(kvp => kvp.Value == instance)
					.Select(kvp => instance.Characters.FirstOrDefault(c => c?.DbId == kvp.Key))
					.FirstOrDefault(c => c != null);

				// Remove old owner association first
				_instancesByOwner.TryRemove(character.DbId, out _);

				if (newOwner != null)
				{
					// Transfer ownership to new member
					_instancesByOwner[newOwner.DbId] = instance;
					instance.Owner = newOwner;
					Log.Info("DungeonScript: Instance '{0}' ownership transferred from '{1}' to '{2}'.", instance.Id, character.Name, newOwner.Name);
				}
				else
				{
					// This shouldn't happen since we checked isLastMember above
					// but handle it defensively
					instance.Owner = null;
					Log.Warning("DungeonScript: Instance '{0}' has no valid owner after '{1}' left.", instance.Id, character.Name);
				}
			}

			character.Warp(character.GetCityReturnLocation());
			Log.Info("DungeonScript: Character '{0}' left party while in dungeon. Warping out.", character.Name);
		}

		#endregion

		protected abstract List<DungeonStage> GetDungeonStages();

		#region Clean Up

		/// <summary>
		/// Disposes of all resources used by this dungeon script.
		/// Properly cancels and disposes all cancellation tokens.
		/// </summary>
		public override void Dispose()
		{
			// End all active instances
			foreach (var instance in _instancesByOwner.Values.ToList())
			{
				if (!instance.IsComplete)
				{
					this.DungeonEnded(instance, false);
				}
			}

			// Cancel and dispose all warp cancellation tokens
			foreach (var kvp in _warpCancellationTokens)
			{
				kvp.Value.Cancel();
				kvp.Value.Dispose();
			}
			_warpCancellationTokens.Clear();

			// Cancel and dispose all activity check tokens
			foreach (var kvp in _activityCheckTokens)
			{
				kvp.Value.Cancel();
				kvp.Value.Dispose();
			}
			_activityCheckTokens.Clear();

			// Cancel and dispose all instance timeout tokens
			foreach (var kvp in _instanceTimeoutTokens)
			{
				kvp.Value.Cancel();
				kvp.Value.Dispose();
			}
			_instanceTimeoutTokens.Clear();

			// Clear any pending instance creation tasks
			foreach (var kvp in _instanceCreationTasks)
			{
				kvp.Value.TrySetCanceled();
			}
			_instanceCreationTasks.Clear();

			// Unsubscribe from events
			ZoneServer.Instance.ServerEvents.PlayerEnteredMap.Unsubscribe(this.OnMapEntryInternal);
			ZoneServer.Instance.ServerEvents.PlayerLeftMap.Unsubscribe(this.OnMapLeaveInternal);
			ZoneServer.Instance.ServerEvents.PlayerLeftParty.Unsubscribe(this.OnPlayerLeftParty);

			// Clear instance mappings
			_instancesByOwner.Clear();
			_instancesByCharacter.Clear();

			// Remove this script from the registry
			Scripts.TryRemove(this.Id, out _);

			base.Dispose();
		}

		#endregion

		#region Rewards

		private void GiveDungeonRewards(InstanceDungeon instance, Character character)
		{
			var dungeonData = instance.InstanceDungeonData;
			var killPercentage = instance.InstanceKillPercentage;
			var killPercentageForRewards = Math.Min(killPercentage, 100);
			var silverAmount = dungeonData.RewardSilver;
			var rewardsString = string.Empty;
			var multiplyToken = 0;
			var rank = GetRankFromAchievementRate(killPercentageForRewards);

			// Calculate exp based on tier system:
			// - RewardContribution defines the step size (e.g., 20 = every 20% killed is a tier)
			// - RewardExp is the exp percentage per tier (e.g., 50 = 50% of total monster exp per tier)
			// - Final exp = TotalMonsterExp * (tierCount * RewardExp / 100)
			var contributionStep = dungeonData.RewardContribution > 0 ? dungeonData.RewardContribution : 20;
			var expPercentPerTier = dungeonData.RewardExp;
			var tierCount = killPercentageForRewards / contributionStep;
			var accumulatedExpRate = tierCount * expPercentPerTier;

			// Calculate final exp from total monster exp killed, applying reward exp rate multiplier
			var rewardExpRate = ZoneServer.Instance.Conf.World.InstancedDungeonRewardExpRate / 100.0;
			var baseExp = (long)(instance.TotalMonsterExp * (accumulatedExpRate / 100.0) * rewardExpRate);
			var jobExp = (long)(instance.TotalMonsterJobExp * (accumulatedExpRate / 100.0) * rewardExpRate);

			// completionRewardMultiply is used for item rewards
			var completionRewardMultiply = instance.GetCompletionRewardMultiplier();

			if (silverAmount > 0)
				character.AddItem(ItemId.Silver, silverAmount);

			if (baseExp > 0)
				character.GiveExp(baseExp, jobExp, null);

			if (dungeonData.RewardContribution > 0)
				character.AddAchievementPoint("INDUN", dungeonData.RewardContribution);

			for (var i = 0; i < dungeonData.RewardsItems.Count; i++)
			{
				character.AddItem(dungeonData.RewardsItems[i].ClassName, completionRewardMultiply <= 0 ? 1 : completionRewardMultiply);

				if (i == 0)
					rewardsString += dungeonData.RewardsItems[i].ClassName;
				else
					rewardsString += ";" + dungeonData.RewardsItems[i].ClassName;
			}

			// Calculate total exp earned in dungeon (monster kills + completion bonus)
			var totalBaseExp = instance.TotalMonsterExp + baseExp;
			var totalJobExp = instance.TotalMonsterJobExp + jobExp;

			// Use instance-wide kill percentage for rewards, showing total exp earned
			character.AddonMessage(AddonMessage.INDUN_REWARD_RESULT, killPercentage + "#" + silverAmount + "#" + rewardsString + "#" + completionRewardMultiply + "#" + totalBaseExp + "#" + totalJobExp + "#" + multiplyToken + "#" + rank + "#", 0);
		}

		private static int GetRankFromAchievementRate(int achievementRate)
		{
			if (achievementRate >= 81)
				return 0;
			if (achievementRate >= 61)
				return 1;
			if (achievementRate >= 41)
				return 2;
			if (achievementRate >= 21)
				return 3;
			return 4;
		}

		#endregion
	}

	/// <summary>
	/// Used to define which dungeon scripts handle which dungeon, based on
	/// a dungeon id.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class DungeonScriptAttribute : Attribute
	{
		/// <summary>
		/// Returns the dungeon id of dungeon script.
		/// </summary>
		public string DungeonId { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="dungeonId"></param>
		public DungeonScriptAttribute(string dungeonId)
		{
			this.DungeonId = dungeonId;
		}
	}
}
