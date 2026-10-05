using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using Yggdrasil.Geometry;

public sealed class EarthTowerRuntime
{
	private readonly Map _map;
	private readonly Random _random;
	private readonly List<Mob> _floorMonsters;
	private readonly List<Mob> _defenseMonsters;
	private readonly int _layer;

	private EarthTowerFloorDefinition _currentFloorDefinition;

	private int _currentFloor;
	private int _currentWave;
	private int _spawnedMonsterCount;
	private int _killedMonsterCount;

	private bool _isRunning;
	private bool _floorCompleted;

	private Mob _defenseObject;
	private TimeSpan _defenseElapsed;
	private TimeSpan _defenseWaveTimer;
	private Mob _boss;

	private int _defenseWave;
	private bool _defenseFailed;

	public bool DefenseFailed => _defenseFailed;
	public int CurrentFloor => _currentFloor;
	public int CurrentWave => _currentWave;
	public int SpawnedMonsterCount => _spawnedMonsterCount;
	public int KilledMonsterCount => _killedMonsterCount;
	public bool IsRunning => _isRunning;
	public bool FloorCompleted => _floorCompleted;
	public int DefenseWave => _defenseWave;
	public TimeSpan DefenseElapsed => _defenseElapsed;

	public TimeSpan DefenseTimeRemaining
	{
		get
		{
			var remaining = EarthTowerFloorController.DefenseFloorDuration - _defenseElapsed;

			if (remaining < TimeSpan.Zero)
				return TimeSpan.Zero;

			return remaining;
		}
	}

	public EarthTowerRuntime(Map map, int layer)
	{
		_map = map;
		_layer = layer;
		_random = new Random();
		_floorMonsters = new List<Mob>();
		_defenseMonsters = new List<Mob>();

		_currentFloor = 0;
		_currentWave = 0;
		_spawnedMonsterCount = 0;
		_killedMonsterCount = 0;

		_isRunning = false;
		_floorCompleted = false;
	}

	public void StartFloor(int floor)
	{
		if (!EarthTowerFloorController.Floors.TryGetValue(floor, out var floorDefinition))
			throw new ArgumentException($"Invalid Earth Tower floor: {floor}.");

		this.CleanupFloor();

		_currentFloor = floor;
		_currentFloorDefinition = floorDefinition;

		_currentWave = 0;
		_spawnedMonsterCount = 0;
		_killedMonsterCount = 0;

		_defenseElapsed = TimeSpan.Zero;
		_defenseWaveTimer = TimeSpan.Zero;
		_defenseWave = 0;
		_defenseFailed = false;

		_isRunning = true;
		_floorCompleted = false;

		switch (_currentFloorDefinition.Type)
		{
			case EarthTowerFloorType.Mobs:
				this.StartMobFloor();
				break;

			case EarthTowerFloorType.Defense:
				this.StartDefenseFloor();
				break;

			case EarthTowerFloorType.Boss:
				this.StartBossFloor();
				break;
		}
	}

	public void Update(TimeSpan elapsed)
	{
		if (!_isRunning || _floorCompleted || _currentFloorDefinition == null)
			return;

		switch (_currentFloorDefinition.Type)
		{
			case EarthTowerFloorType.Mobs:
				this.UpdateMobFloor();
				break;

			case EarthTowerFloorType.Defense:
				this.UpdateDefenseFloor(elapsed);
				break;

			case EarthTowerFloorType.Boss:
				this.UpdateBossFloor();
				break;
		}

		_defenseMonsters.RemoveAll(monster => monster.IsDead);
	}

	private void StartMobFloor()
	{
		this.SpawnMobWave();
	}

	private void UpdateMobFloor()
	{
		if (_killedMonsterCount >= EarthTowerFloorController.MobFloorTotalKills)
			this.CompleteMobFloor();
	}

	private void SpawnMobWave()
	{
		if (!_isRunning || _floorCompleted || _spawnedMonsterCount > 0)
			return;

		_currentWave = 1;

		var spawnPositions = EarthTowerFloorController.GenerateWaveSpawnPositions(_map, _random);

		if (spawnPositions.Count < EarthTowerFloorController.MobFloorMonstersPerWave)
			throw new InvalidOperationException($"Earth Tower floor {_currentFloor}: only {spawnPositions.Count} valid spawn positions were generated.");

		for (var i = 0; i < EarthTowerFloorController.MobFloorMonstersPerWave; i++)
		{
			var monsterId = EarthTowerFloorController.GetRandomMonsterId(_random);
			var spawnPosition = spawnPositions[i];

			this.SpawnFloorMonster(monsterId, spawnPosition);
		}
	}

	private void SpawnFloorMonster(int monsterId, Position position)
	{
		var monster = new Mob(monsterId, RelationType.Enemy);
		
		monster.Layer = _layer;
		monster.Position = position;
		monster.SpawnPosition = position;
		monster.Direction = new Direction(_random.Next(360));
		monster.Tendency = TendencyType.Aggressive;

		monster.Components.Add(new MovementComponent(monster));

		var monsterData = ZoneServer.Instance.Data.MonsterDb.Find(monsterId);

		if (monsterData != null && !string.IsNullOrEmpty(monsterData.AiName) && AiScript.Exists(monsterData.AiName))
			monster.Components.Add(new AiComponent(monster, monsterData.AiName));
		else
			monster.Components.Add(new AiComponent(monster, "BasicMonster"));

		monster.Died += this.OnFloorMonsterKilled;

		_map.AddMonster(monster);

		_floorMonsters.Add(monster);
		_spawnedMonsterCount++;
	}

	private void OnFloorMonsterKilled(Mob monster, ICombatEntity killer)
	{
		if (!_isRunning || _floorCompleted)
			return;

		if (_currentFloorDefinition == null || _currentFloorDefinition.Type != EarthTowerFloorType.Mobs)
			return;

		if (!_floorMonsters.Contains(monster))
			return;

		monster.Died -= this.OnFloorMonsterKilled;
		_floorMonsters.Remove(monster);
		_killedMonsterCount++;
	}

	private void CompleteMobFloor()
	{
		if (_floorCompleted)
			return;

		foreach (var monster in _floorMonsters)
		{
			monster.Died -= this.OnFloorMonsterKilled;

			if (!monster.IsDead)
				_map.RemoveMonster(monster);
		}

		_floorMonsters.Clear();

		this.CompleteFloor();
	}

	private void CompleteFloor()
	{
		if (_floorCompleted)
			return;

		_floorCompleted = true;
		_isRunning = false;
	}

	public void CleanupFloor()
	{
		this.RemoveAllFloorEntities();
		this.ResetFloorState();
	}

	private void StartDefenseFloor()
	{
		_defenseElapsed = TimeSpan.Zero;
		_defenseWaveTimer = TimeSpan.Zero;
		_defenseWave = 0;
		_defenseFailed = false;

		this.SpawnDefenseObject();
		this.SpawnDefenseWave();
	}

	private void SpawnDefenseObject()
	{
		var position = EarthTowerFloorController.DefenseObjectPosition;

		_defenseObject = new Mob(MonsterId.Npc_Zachariel_Lantern_2, RelationType.Neutral);

		_defenseObject.Layer = _layer;
		_defenseObject.Position = position;
		_defenseObject.SpawnPosition = position;
		_defenseObject.Direction = new Direction(0);

		var propertyOverrides = new PropertyOverrides();

		propertyOverrides.Add(
			PropertyName.HPCount,
			EarthTowerFloorController.DefenseObjectHp
		);

		_defenseObject.ApplyOverrides(propertyOverrides);
		_defenseObject.Properties.InvalidateAll();
		_defenseObject.HealToFull();

		_defenseObject.Died += this.OnDefenseObjectDestroyed;

		_map.AddMonster(_defenseObject);

		_defenseObject.Faction = FactionType.Our_Forces;
	}

	private void UpdateDefenseFloor(TimeSpan elapsed)
	{
		if (_defenseObject == null)
			return;

		if (_defenseObject.IsDead)
		{
			this.FailDefenseFloor();
			return;
		}

		_defenseElapsed += elapsed;
		_defenseWaveTimer += elapsed;

		if (_defenseElapsed >= EarthTowerFloorController.DefenseFloorDuration)
		{
			this.CompleteDefenseFloor();
			return;
		}

		if (_defenseWaveTimer >= EarthTowerFloorController.DefenseWaveInterval)
		{
			_defenseWaveTimer = TimeSpan.Zero;

			var aliveMonsters = this.GetAliveDefenseMonsterCount();

			if (aliveMonsters < EarthTowerFloorController.DefenseMaxAliveMonsters)
				this.SpawnDefenseWave();
		}
	}

	private void SpawnDefenseWave()
	{
		if (_defenseObject == null || _defenseObject.IsDead)
			return;

		var aliveMonsters = this.GetAliveDefenseMonsterCount();
		var availableSlots = EarthTowerFloorController.DefenseMaxAliveMonsters - aliveMonsters;

		if (availableSlots <= 0)
			return;

		var monstersToSpawn = Math.Min(
			EarthTowerFloorController.DefenseMonstersPerWave,
			availableSlots
		);

		_defenseWave++;

		var positions = EarthTowerFloorController.GenerateSpawnPositions(
			_map,
			_random,
			monstersToSpawn
		);

		for (var i = 0; i < positions.Count; i++)
		{
			var monsterId = EarthTowerFloorController.GetRandomMonsterId(_random);

			this.SpawnDefenseMonster(
				monsterId,
				positions[i]
			);
		}
	}

	private void SpawnDefenseMonster(int monsterId, Position position)
	{
		var monster = new Mob(monsterId, RelationType.Enemy);

		monster.Layer = _layer;
		monster.Position = position;
		monster.SpawnPosition = position;
		monster.Direction = new Direction(_random.Next(360));
		monster.Tendency = TendencyType.Aggressive;

		monster.Components.Add(new MovementComponent(monster));
		monster.Components.Add(new LifeTimeComponent(monster, TimeSpan.FromMinutes(5)));

		var monsterData = ZoneServer.Instance.Data.MonsterDb.Find(monsterId);

		if (monsterData != null && !string.IsNullOrEmpty(monsterData.AiName) && AiScript.Exists(monsterData.AiName))
			monster.Components.Add(new AiComponent(monster, monsterData.AiName));
		else
			monster.Components.Add(new AiComponent(monster, "BasicMonster"));

		_map.AddMonster(monster);

		_defenseMonsters.Add(monster);

		Task.Delay(500).ContinueWith(_ =>
		{
			if (!monster.IsDead && _defenseObject != null && !_defenseObject.IsDead)
				monster.InsertHate(_defenseObject, 150);
		});
	}

	private void OnDefenseObjectDestroyed(Mob monster, ICombatEntity killer)
	{
		if (!_isRunning || _floorCompleted)
			return;

		this.FailDefenseFloor();
	}

	private void FailDefenseFloor()
	{
		if (!_isRunning)
			return;

		_defenseFailed = true;
		_isRunning = false;
	}

	private void CompleteDefenseFloor()
	{
		if (_floorCompleted)
			return;

		_floorCompleted = true;
		_isRunning = false;
	}

	private void StartBossFloor()
	{
		if (_currentFloorDefinition.BossMonsterId <= 0)
			throw new InvalidOperationException($"Earth Tower floor {_currentFloor} does not have a valid boss monster id.");

		this.SpawnBoss(_currentFloorDefinition.BossMonsterId);
	}

	private void SpawnBoss(int monsterId)
	{
		var position = EarthTowerFloorController.BossSpawnPosition;

		_boss = new Mob(monsterId, RelationType.Enemy);

		_boss.Layer = _layer;
		_boss.Position = position;
		_boss.SpawnPosition = position;
		_boss.Direction = new Direction(_random.Next(360));
		_boss.Tendency = TendencyType.Aggressive;

		_boss.Components.Add(new MovementComponent(_boss));

		var monsterData = ZoneServer.Instance.Data.MonsterDb.Find(monsterId);

		if (monsterData != null && !string.IsNullOrEmpty(monsterData.AiName) && AiScript.Exists(monsterData.AiName))
			_boss.Components.Add(new AiComponent(_boss, monsterData.AiName));
		else
			_boss.Components.Add(new AiComponent(_boss, "BasicMonster"));

		_boss.Died += this.OnBossKilled;

		_map.AddMonster(_boss);
	}

	private void UpdateBossFloor()
	{
		if (_boss == null)
			return;

		if (_boss.IsDead && !_floorCompleted)
			this.CompleteFloor();
	}

	private void OnBossKilled(Mob monster, ICombatEntity killer)
	{
		if (!_isRunning || _floorCompleted)
			return;

		if (_boss != monster)
			return;

		_boss.Died -= this.OnBossKilled;

		this.CompleteFloor();
	}

	private void RemoveAllFloorEntities()
	{
		if (_defenseObject != null)
		{
			_defenseObject.Died -= this.OnDefenseObjectDestroyed;

			if (!_defenseObject.IsDead)
				_map.RemoveMonster(_defenseObject);

			_defenseObject = null;
		}

		if (_boss != null)
		{
			_boss.Died -= this.OnBossKilled;

			if (!_boss.IsDead)
				_map.RemoveMonster(_boss);

			_boss = null;
		}

		foreach (var monster in _floorMonsters)
		{
			monster.Died -= this.OnFloorMonsterKilled;

			if (!monster.IsDead)
				_map.RemoveMonster(monster);
		}

		_floorMonsters.Clear();

		foreach (var monster in _defenseMonsters)
		{
			if (!monster.IsDead)
				_map.RemoveMonster(monster);
		}

		_defenseMonsters.Clear();
	}

	private void ResetFloorState()
	{
		_currentFloorDefinition = null;

		_currentWave = 0;
		_spawnedMonsterCount = 0;
		_killedMonsterCount = 0;

		_defenseElapsed = TimeSpan.Zero;
		_defenseWaveTimer = TimeSpan.Zero;
		_defenseWave = 0;
		_defenseFailed = false;

		_isRunning = false;
		_floorCompleted = false;
	}

	private int GetAliveDefenseMonsterCount()
	{
		var count = 0;

		foreach (var monster in _defenseMonsters)
		{
			if (!monster.IsDead)
				count++;
		}

		return count;
	}
}
