using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using Yggdrasil.Logging;
using System.Collections.Generic;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;

namespace Melia.Zone.World
{
	public sealed class WorldBossManager
	{
		public static WorldBossManager Instance { get; private set; }

		private static readonly int[] SpawnHours =
		{
			0, 4, 8, 12, 16, 20,
		};

		private readonly Random _random = new Random();
		private DateTime? _lastSpawnSlot;
		private Mob _activeBoss;
		private WorldBossLocation _activeLocation;
		private const string QuestClassName = "world_boss";
		private const int QuestId = 1001;
		private const string ParticipationsVariable = "Laima.WorldBoss.Participations";
		private readonly HashSet<Character> _participants = new HashSet<Character>();

		public bool HasActiveBoss => _activeBoss != null && !_activeBoss.IsDead;
		public int ActiveMonsterId { get; private set; }
		public string ActiveMapClassName => _activeLocation?.MapClassName;
		public string ActiveMapName => _activeLocation?.MapName;
		public Mob ActiveBoss => _activeBoss;
		private const float WorldBossRecoveryMultiplier = 0.0005f;
		private const float WorldBossRecoveryInterval = 90000f;

		public void Initialize()
		{
			Instance = this;
			ZoneServer.Instance.ServerEvents.HourTick.Subscribe(this.OnHourTick);
			Log.Info("WorldBossManager: Initialized. Spawn hours: 00:00, 06:00, 12:00 and 18:00 Brasília time.");
		}

		private void OnHourTick(object sender, TimeEventArgs args)
		{
			var brasiliaTime = args.Now.ToUniversalTime().AddHours(-3);

			if (Array.IndexOf(SpawnHours, brasiliaTime.Hour) < 0)
				return;

			var spawnSlot = new DateTime(brasiliaTime.Year, brasiliaTime.Month, brasiliaTime.Day, brasiliaTime.Hour, 0, 0);

			if (_lastSpawnSlot.HasValue && _lastSpawnSlot.Value == spawnSlot)
				return;

			_lastSpawnSlot = spawnSlot;

			if (_activeBoss != null)
			{
				Log.Info("WorldBossManager: Removing previous World Boss {0} from {1} for scheduled replacement.", ActiveMonsterId, ActiveMapName);
				this.KillActiveBoss(false);
			}

			if (!this.SpawnWorldBoss())
				Log.Warning("WorldBossManager: Scheduled World Boss spawn failed for slot {0:yyyy-MM-dd HH:mm}.", spawnSlot);

			this.SpawnWorldBoss();
		}

		public bool SpawnWorldBoss()
		{
			if (HasActiveBoss)
				return false;

			if (!this.TryGetSpawnLocation(out var location, out var map, out var position))
			{
				Log.Warning("WorldBossManager: No valid World Boss spawn location could be found.");
				return false;
			}

			var monsterId = WorldBossPool.GetRandomMonsterId(_random);

			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(monsterId, out var monsterData))
			{
				Log.Warning("WorldBossManager: Monster {0} was not found in MonsterDb.", monsterId);
				return false;
			}

			var boss = new Mob(monsterData.Id, RelationType.Enemy);
			boss.Position = position;
			boss.SpawnPosition = position;
			boss.Direction = new Direction(_random.Next(360));
			boss.Tendency = TendencyType.Aggressive;
			boss.Components.Add(new MovementComponent(boss));

			if (!string.IsNullOrEmpty(monsterData.AiName) && AiScript.Exists(monsterData.AiName))
				boss.Components.Add(new AiComponent(boss, monsterData.AiName));
			else
				boss.Components.Add(new AiComponent(boss, "BasicMonster"));

			var currentRecovery = boss.Properties.GetFloat(PropertyName.RHP);
			var currentRecoveryBonus = boss.Properties.GetFloat(PropertyName.RHP_BM);
			var recoveryReduction = currentRecovery * (1f - WorldBossRecoveryMultiplier);
			boss.Properties.SetFloat(PropertyName.RHP_BM, currentRecoveryBonus - recoveryReduction);

			var recoveryComponent = boss.Components.Get<RecoveryComponent>();

			if (recoveryComponent != null)
				recoveryComponent.SetHpRecoveryTime(TimeSpan.FromMilliseconds(WorldBossRecoveryInterval));

			_participants.Clear();

			boss.Died += this.OnWorldBossKilled;
			map.AddMonster(boss);

			_activeBoss = boss;
			_activeLocation = location;
			ActiveMonsterId = monsterId;

			Log.Info("WorldBossManager: {0} ({1}) spawned at {2} [{3}] ({4:0.0}, {5:0.0}, {6:0.0}).", monsterData.Name, monsterId, location.MapName, location.MapClassName, position.X, position.Y, position.Z);
			Send.ZC_NORMAL.WorldMessage(1, $"[WORLD BOSS] {monsterData.Name} has appeared at {location.MapName}!");

			return true;
		}

		private bool TryGetSpawnLocation(out WorldBossLocation location, out Map map, out Position position)
		{
			location = null;
			map = null;
			position = default;

			if (WorldBossLocations.Locations.Length == 0)
				return false;

			var startIndex = _random.Next(WorldBossLocations.Locations.Length);

			for (var i = 0; i < WorldBossLocations.Locations.Length; i++)
			{
				var index = (startIndex + i) % WorldBossLocations.Locations.Length;
				var candidate = WorldBossLocations.Locations[index];

				if (!ZoneServer.Instance.World.TryGetMap(candidate.MapClassName, out var candidateMap))
					continue;

				if (!candidateMap.Ground.HasData())
					continue;

				if (!candidateMap.Ground.TryGetRandomPosition(out var candidatePosition))
					continue;

				location = candidate;
				map = candidateMap;
				position = candidatePosition;
				return true;
			}

			return false;
		}

		public void RegisterParticipation(Mob boss, ICombatEntity attacker)
		{
			if (_activeBoss == null || boss != _activeBoss || boss.IsDead || attacker == null)
				return;

			var character = this.GetAttackingCharacter(attacker);

			if (character == null || !character.IsOnline || character.Connection == null)
				return;

			var questId = new QuestId(QuestClassName, QuestId);

			if (!character.Quests.IsActive(questId))
				return;

			_participants.Add(character);
		}

		private Character GetAttackingCharacter(ICombatEntity attacker)
		{
			if (attacker is Character character)
				return character;

			if (attacker.Components.Get<AiComponent>()?.Script.GetMaster() is Character master)
				return master;

			if (attacker is Summon summon && summon.Owner is Character summonOwner)
				return summonOwner;

			return null;
		}

		private void OnWorldBossKilled(Mob boss, ICombatEntity killer)
		{
			if (_activeBoss == null || boss != _activeBoss)
				return;

			boss.Died -= this.OnWorldBossKilled;

			var monsterName = ZoneServer.Instance.Data.MonsterDb.TryFind(ActiveMonsterId, out var monsterData) ? monsterData.Name : "World Boss";
			var mapName = _activeLocation?.MapName ?? "Unknown";
			var rewardedPlayers = 0;

			foreach (var character in _participants)
			{
				if (character == null || !character.IsOnline || character.Connection == null)
					continue;

				var questId = new QuestId(QuestClassName, QuestId);

				if (!character.Quests.IsActive(questId))
					continue;

				var participations = character.Variables.Perm.GetInt(ParticipationsVariable, 0);
				character.Variables.Perm.Set(ParticipationsVariable, participations + 1);

				WorldBossParticipationObjective.ReportParticipation(character);

				character.ServerMessage("World Boss participation registered. Return to the World Boss Manager to claim your 5 Golden Coins.");
				rewardedPlayers++;
			}

			Log.Info("WorldBossManager: {0} ({1}) was defeated at {2}. Participants rewarded: {3}.", monsterName, ActiveMonsterId, mapName, rewardedPlayers);
			Send.ZC_NORMAL.WorldMessage(1, $"[WORLD BOSS] {monsterName} has been defeated at {mapName}!");

			this.ClearActiveBoss();
		}

		public string GetActiveBossInfo()
		{
			if (!HasActiveBoss)
				return null;

			var monsterName = ZoneServer.Instance.Data.MonsterDb.TryFind(ActiveMonsterId, out var monsterData) ? monsterData.Name : "Unknown";
			var mapName = _activeLocation?.MapName ?? "Unknown";
			var mapClassName = _activeLocation?.MapClassName ?? "Unknown";

			return $"{monsterName} ({ActiveMonsterId}) at {mapName} [{mapClassName}]";
		}

		public bool KillActiveBoss(bool announce)
		{
			if (_activeBoss == null)
			{
				this.ClearActiveBoss();
				return false;
			}

			var boss = _activeBoss;
			var monsterId = ActiveMonsterId;
			var monsterName = ZoneServer.Instance.Data.MonsterDb.TryFind(monsterId, out var monsterData) ? monsterData.Name : "World Boss";
			var mapName = _activeLocation?.MapName ?? "Unknown";

			boss.Died -= this.OnWorldBossKilled;

			if (boss.Map != null)
				boss.Map.RemoveMonster(boss);

			this.ClearActiveBoss();

			Log.Info("WorldBossManager: {0} ({1}) was removed from {2}.", monsterName, monsterId, mapName);

			if (announce)
				Send.ZC_NORMAL.WorldMessage(1, $"[WORLD BOSS] {monsterName} has been removed from {mapName}.");

			return true;
		}

		public void ClearActiveBoss()
		{
			if (_activeBoss != null)
				_activeBoss.Died -= this.OnWorldBossKilled;

			_activeBoss = null;
			_activeLocation = null;
			ActiveMonsterId = 0;
			_participants.Clear();
		}
	}
}
