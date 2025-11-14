using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using Melia.Zone.World.Spawning;
using Yggdrasil.Geometry;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

/// <summary>
/// Marble Shooter minigame.
/// Players must shoot marbles into an altar while avoiding fireballs that orbit around it.
/// </summary>
public class MarbleShooterScript : IMinigameScript
{
	public IMinigameInstance CreateInstance(Map map, Position position, Direction direction)
	{
		return new MarbleShooterInstance(map, position, direction);
	}
}

public class MarbleShooterInstance : MinigameBase
{
	// Configuration (constants) - Using same NPCs as torch minigame for visual consistency
	private const int AltarMonsterId = MonsterId.SHADOW_CURSE_PURIFICATION;
	private const int MarbleMonsterId = MonsterId.Magictrap_Core_S;
	private const int FireballMonsterId = MonsterId.HiddenCreateSphere;

	// Reward Tiers Configuration (same as defend the torch)
	private static readonly (int MinLevel, int MaxLevel, int ItemId, int Amount)[] RewardTiers = new[]
	{
		(1, 20, 640080, 10),
		(21, 30, 640081, 5),
		(31, 40, 640081, 10),
		(41, 50, 640082, 5),
		(51, 60, 640082, 10),
		(51, 60, 640083, 5),
		(61, 70, 640083, 10),
		(71, 80, 640084, 5),
		(81, 90, 640084, 10),
		(91, 100, 640085, 5),
		(101, 110, 640085, 10),
		(111, 120, 640086, 5),
		(121, 999, 640086, 10),
	};

	// State
	private Mob _altar;
	private readonly List<Npc> _marbles;
	private readonly List<FireballData> _fireballs;
	private readonly List<Mob> _spawnedMonsters;
	private readonly HashSet<int> _participatingPlayerIds;
	private readonly HashSet<int> _interactingMarbles;
	private readonly List<ShootingMarbleData> _shootingMarbles;
	private readonly List<int> _availableMonsterIds;
	private int _averageMonsterLevel;
	private int _monstersPerWave;
	private int _marblesHitAltar;
	private TimeSpan _timeSinceLastMonsterWave;
	private bool _isCompleted;
	private bool _isStarted;
	private bool _notified2MinutesLeft;
	private bool _notified1MinuteLeft;
	private bool _notified30SecondsLeft;

	private class FireballData
	{
		public Mob Fireball { get; set; }
		public float Radius { get; set; }
		public float Angle { get; set; }
		public float Speed { get; set; }
		public bool IsClockwise { get; set; }
		public Position TargetPosition { get; set; }
		public float TargetAngle { get; set; }
	}

	private class ShootingMarbleData
	{
		public Npc Marble { get; set; }
		public float VelocityX { get; set; }
		public float VelocityZ { get; set; }
	}

	public MarbleShooterInstance(Map map, Position position, Direction direction)
		: base(map, position, direction)
	{
		_marbles = new List<Npc>();
		_fireballs = new List<FireballData>();
		_spawnedMonsters = new List<Mob>();
		_participatingPlayerIds = new HashSet<int>();
		_interactingMarbles = new HashSet<int>();
		_shootingMarbles = new List<ShootingMarbleData>();
		_availableMonsterIds = new List<int>();
		_marblesHitAltar = 0;
		_timeSinceLastMonsterWave = TimeSpan.Zero;
		_isCompleted = false;
		_isStarted = false;
		_notified2MinutesLeft = false;
		_notified1MinuteLeft = false;
		_notified30SecondsLeft = false;
	}

	protected override void OnStart()
	{
		// Spawn the altar
		_altar = new Mob(AltarMonsterId, RelationType.Neutral);
		_altar.Name = "Mystic Altar";
		_altar.Position = this.SpawnPosition;
		_altar.Direction = this.SpawnDirection;
		_altar.Faction = FactionType.Neutral; // Neutral so nothing attacks it

		this.Map.AddMonster(_altar);

		// Spawn initial marbles
		this.SpawnMarbles();

		// Send initial notice - tell players to get close to start
		var rewardRange = ZoneServer.Instance.Conf.World.MarbleShooterRewardRange;
		this.SendNoticeInRange(this.SpawnPosition, rewardRange,
			"A Mystic Altar has appeared! Get close to start the Marble Shooter challenge!", "Clear", 8);
	}

	public override void Update(TimeSpan elapsed)
	{
		// During waiting phase, check for player activation but don't accumulate time
		if (!_isStarted)
		{
			// Check if altar is still alive
			if (_altar == null || _altar.IsDead)
			{
				this.End();
				return;
			}

			// Check for nearby players to start the minigame
			var activationRange = ZoneServer.Instance.Conf.World.MarbleShooterActivationRange;
			var nearbyPlayers = this.GetCharactersInRange(this.SpawnPosition, activationRange);
			if (nearbyPlayers.Count > 0)
			{
				this.StartMinigame();
				// Now that we've started, call the base Update to begin normal processing
				base.Update(elapsed);
			}
			return;
		}

		// Once started, use the base Update which will call OnUpdate
		base.Update(elapsed);
	}

	private void StartMinigame()
	{
		_isStarted = true;

		// Activate the minigame so ElapsedTime starts accumulating
		this.IsActive = true;

		// Initialize available monsters from map spawners
		// This is done here (not in OnStart) to ensure monster spawners are loaded
		// Important for script reload - minigame spawner might load before monster spawners
		this.InitializeAvailableMonsters();

		// Spawn fireballs based on average monster level
		// This is done here after InitializeAvailableMonsters() to ensure correct difficulty
		this.SpawnFireballs();

		var rewardRange = ZoneServer.Instance.Conf.World.MarbleShooterRewardRange;
		var durationMinutes = ZoneServer.Instance.Conf.World.MarbleShooterDurationMinutes;
		this.SendNoticeInRange(this.SpawnPosition, rewardRange,
			string.Format("Marble Shooter Challenge! Shoot {0} marbles into the altar! You have {1} minutes!",
				ZoneServer.Instance.Conf.World.MarbleShooterRequiredHits, durationMinutes),
			"Clear", 8);
	}

	protected override void OnUpdate(TimeSpan elapsed)
	{
		if (_altar == null || _altar.IsDead || _isCompleted)
		{
			this.End();
			return;
		}

		// Check if minigame duration has elapsed
		var minigameDuration = TimeSpan.FromMinutes(ZoneServer.Instance.Conf.World.MarbleShooterDurationMinutes);
		if (this.ElapsedTime >= minigameDuration)
		{
			this.OnMinigameFailed();
			return;
		}

		// Send time notifications at specific intervals
		var timeRemaining = minigameDuration - this.ElapsedTime;
		if (!_notified2MinutesLeft && timeRemaining <= TimeSpan.FromMinutes(2))
		{
			_notified2MinutesLeft = true;
			this.SendTimeNotification(2);
		}
		else if (!_notified1MinuteLeft && timeRemaining <= TimeSpan.FromMinutes(1))
		{
			_notified1MinuteLeft = true;
			this.SendTimeNotification(1);
		}
		else if (!_notified30SecondsLeft && timeRemaining <= TimeSpan.FromSeconds(30))
		{
			_notified30SecondsLeft = true;
			this.SendTimeNotification(0.5f);
		}

		// Track participating players
		var rewardRange = ZoneServer.Instance.Conf.World.MarbleShooterRewardRange;
		var currentNearbyPlayers = this.GetCharactersInRange(this.SpawnPosition, rewardRange);
		foreach (var player in currentNearbyPlayers)
		{
			_participatingPlayerIds.Add(player.Handle);
		}

		// Update fireballs orbiting
		this.UpdateFireballs(elapsed);

		// Update shooting marbles
		this.UpdateShootingMarbles(elapsed);

		// Spawn monster waves
		_timeSinceLastMonsterWave += elapsed;
		var waveInterval = TimeSpan.FromSeconds(ZoneServer.Instance.Conf.World.MarbleShooterMonsterWaveIntervalSeconds);
		if (_timeSinceLastMonsterWave >= waveInterval && currentNearbyPlayers.Count > 0)
		{
			this.SpawnMonsterWave();
			_timeSinceLastMonsterWave = TimeSpan.Zero;
		}

		// Clean up dead monsters
		_spawnedMonsters.RemoveAll(m => m.IsDead);

		// Check win condition
		var requiredHits = ZoneServer.Instance.Conf.World.MarbleShooterRequiredHits;
		if (_marblesHitAltar >= requiredHits)
		{
			this.OnMinigameSuccess();
		}
	}

	private void InitializeAvailableMonsters()
	{
		var worldConf = ZoneServer.Instance.Conf.World;

		// Get all spawners and filter by those that spawn on this map
		// Use spawner.Maps property instead of spawn areas to avoid issues after script reload
		var allSpawners = ZoneServer.Instance.World.GetSpawners().OfType<MonsterSpawner>();
		var spawners = allSpawners.Where(s => s.Maps.Contains(this.Map.Id)).ToList();

		if (spawners.Count > 0)
		{
			var monsterIds = new HashSet<int>();
			var totalLevel = 0;
			var monsterCount = 0;

			foreach (var spawner in spawners)
			{
				// Only consider spawners that spawn frequently (avoid rare spawns)
				var averageSpawnCount = (spawner.MinAmount + spawner.MaxAmount) / 2.0f;
				if (averageSpawnCount < 3.0f)
					continue;

				var monsterId = spawner.MonsterData.Id;
				monsterIds.Add(monsterId);
				totalLevel += spawner.MonsterData.Level;
				monsterCount++;
			}

			_availableMonsterIds.AddRange(monsterIds);

			// Calculate average level
			_averageMonsterLevel = monsterCount > 0 ? totalLevel / monsterCount : 1;

			// Always spawn 3 monsters per wave
			_monstersPerWave = 3;
		}

		// Fallback to hardcoded monsters if no spawners found
		if (_availableMonsterIds.Count == 0)
		{
			_availableMonsterIds.AddRange(new[]
			{
				MonsterId.Onion,
				MonsterId.Hanaming,
				MonsterId.InfroRocktor,
				MonsterId.Leaf_Diving,
			});
			_monstersPerWave = 3;
			_averageMonsterLevel = 1;
		}
	}

	private void SpawnMarbles()
	{
		var marbleMinRange = ZoneServer.Instance.Conf.World.MarbleShooterMarbleMinRange;
		var marbleMaxRange = ZoneServer.Instance.Conf.World.MarbleShooterMarbleMaxRange;
		var marbleCount = ZoneServer.Instance.Conf.World.MarbleShooterMarbleCount;

		for (var i = 0; i < marbleCount; i++)
		{
			this.SpawnSingleMarble(marbleMinRange, marbleMaxRange);
		}
	}

	private bool IsPositionValidWithClearance(Position pos, float clearanceDistance)
	{
		// Check center position
		if (!this.Map.Ground.IsValidPosition(pos))
			return false;

		// Check cardinal directions (North, South, East, West) and diagonals (NE, NW, SE, SW)
		var directions = new[]
		{
			new Position(pos.X, pos.Y, pos.Z + clearanceDistance), // North
			new Position(pos.X, pos.Y, pos.Z - clearanceDistance), // South
			new Position(pos.X + clearanceDistance, pos.Y, pos.Z), // East
			new Position(pos.X - clearanceDistance, pos.Y, pos.Z), // West
			new Position(pos.X + clearanceDistance, pos.Y, pos.Z + clearanceDistance), // NE
			new Position(pos.X - clearanceDistance, pos.Y, pos.Z + clearanceDistance), // NW
			new Position(pos.X + clearanceDistance, pos.Y, pos.Z - clearanceDistance), // SE
			new Position(pos.X - clearanceDistance, pos.Y, pos.Z - clearanceDistance), // SW
		};

		foreach (var checkPos in directions)
		{
			if (!this.Map.Ground.IsValidPosition(checkPos))
				return false;
		}

		return true;
	}

	private void SpawnSingleMarble(float minRange, float maxRange)
	{
		// Find a valid spawn position
		Position spawnPos = this.SpawnPosition;
		var foundValidPos = false;
		var minDistanceBetweenMarbles = 30f; // Minimum distance between marbles
		var wallClearanceDistance = 15f; // Minimum clearance from walls

		for (var k = 0; k < 100; k++)
		{
			var randomAngle = this.Rnd.Next(360) * Math.PI / 180;
			var randomDistance = (float)this.Rnd.Next((int)minRange, (int)maxRange + 1);
			var randomX = this.SpawnPosition.X + (float)(Math.Cos(randomAngle) * randomDistance);
			var randomZ = this.SpawnPosition.Z + (float)(Math.Sin(randomAngle) * randomDistance);
			var randPos = new Position(randomX, this.SpawnPosition.Y, randomZ);

			// Check if position is valid on the map and has clearance from walls
			if (!this.IsPositionValidWithClearance(randPos, wallClearanceDistance))
				continue;

			// Check if too close to existing marbles
			var tooCloseToOtherMarble = false;
			foreach (var existingMarble in _marbles)
			{
				var distance = Math.Sqrt(
					Math.Pow(existingMarble.Position.X - randPos.X, 2) +
					Math.Pow(existingMarble.Position.Z - randPos.Z, 2));

				if (distance < minDistanceBetweenMarbles)
				{
					tooCloseToOtherMarble = true;
					break;
				}
			}

			if (!tooCloseToOtherMarble)
			{
				spawnPos = randPos;
				foundValidPos = true;
				break;
			}
		}

		// If no valid position found, don't spawn this marble
		if (!foundValidPos)
		{
			return;
		}

		// Create marble as an NPC for interaction
		var marble = AddNpc(MarbleMonsterId, "Magic Marble", this.Map.ClassName, spawnPos.X, spawnPos.Z, this.Rnd.Next(360), async dialog =>
		{
			var character = dialog.Player;
			var marbleNpc = dialog.Npc;

			// Check if marble is already being interacted with or is shooting
			if (_interactingMarbles.Contains(marbleNpc.Handle))
				return;
			if (_shootingMarbles.Any(sm => sm.Marble.Handle == marbleNpc.Handle))
				return;

			// Mark marble as being interacted with
			_interactingMarbles.Add(marbleNpc.Handle);

			try
			{
				var result = await character.TimeActions.StartAsync("Aiming marble...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(2));

				if (result == TimeActionResult.Completed)
				{
					// Shoot the marble
					this.ShootMarble(marbleNpc, character);
				}
			}
			finally
			{
				// Remove from interacting set when done
				_interactingMarbles.Remove(marbleNpc.Handle);
			}
		});

		// Set additional properties
		marble.Faction = FactionType.Neutral; // Neutral so players and monsters can't attack it

		// Enable movement for this NPC (normally NPCs cannot move)
		marble.AllowMovement = true;

		// Set a base movement speed for marbles (they'll get velocity applied when shot)
		// For NPCs, we need to initialize the base walk speed properties
		// that the movement system uses to calculate movement time
		// Clamp speed between 20 and 120
		var marbleBaseSpeed = Math.Max(20f, Math.Min(120f, ZoneServer.Instance.Conf.World.MarbleShooterMarbleBaseSpeed));
		marble.Properties.SetFloat(PropertyName.WlkMSPD, marbleBaseSpeed);
		marble.Properties.SetFloat(PropertyName.RunMSPD, marbleBaseSpeed);
		marble.Properties.SetFloat(PropertyName.MSPD, marbleBaseSpeed);
		Send.ZC_MSPD(marble);

		// Add MovementComponent so marbles can move and send packets to clients
		marble.Components.Add(new MovementComponent(marble));

		_marbles.Add(marble);
	}

	private void SpawnFireballs()
	{
		// Determine number of fireballs based on average monster level
		var fireballCount = 0;
		if (_averageMonsterLevel <= 30)
			fireballCount = 0;
		else if (_averageMonsterLevel <= 40)
			fireballCount = 1;
		else if (_averageMonsterLevel <= 70)
			fireballCount = 2;
		else
			fireballCount = 3;

		if (fireballCount == 0)
			return;

		// Calculate fireball speed
		var speedMultiplier = ZoneServer.Instance.Conf.World.MarbleShooterFireballSpeedMultiplier;
		var fireballSpeed = _averageMonsterLevel * speedMultiplier;
		// Clamp speed between 20 and 100
		fireballSpeed = Math.Max(20f, Math.Min(100f, fireballSpeed));

		// Get min and max fireball range for randomization
		var minFireballRange = ZoneServer.Instance.Conf.World.MarbleShooterFireballMinRange;
		var maxFireballRange = ZoneServer.Instance.Conf.World.MarbleShooterFireballMaxRange;

		// Spawn fireballs with even angular distribution
		var angleStep = 360f / fireballCount;
		for (var i = 0; i < fireballCount; i++)
		{
			var startAngle = i * angleStep;
			var isClockwise = this.Rnd.Next(2) == 0;

			// Pick a random orbit radius between min and max range for each fireball
			var orbitRadius = (float)this.Rnd.Next((int)minFireballRange, (int)maxFireballRange + 1);

			// Try to find a valid starting position
			var validPosition = false;
			var attempts = 0;
			Position position = this.SpawnPosition;

			while (!validPosition && attempts < 36) // Try different angles if initial position is invalid
			{
				var testAngle = startAngle + (attempts * 10); // Adjust angle by 10 degrees each attempt
				var radians = testAngle * Math.PI / 180;
				var x = _altar.Position.X + (float)(Math.Cos(radians) * orbitRadius);
				var z = _altar.Position.Z + (float)(Math.Sin(radians) * orbitRadius);
				var testPos = new Position(x, _altar.Position.Y, z);

				if (this.Map.Ground.IsValidPosition(testPos))
				{
					position = testPos;
					startAngle = testAngle % 360;
					validPosition = true;
				}
				attempts++;
			}

			// If no valid position found, skip this fireball
			if (!validPosition)
				continue;

			var fireballData = new FireballData
			{
				Radius = orbitRadius,
				Angle = startAngle,
				Speed = fireballSpeed,
				IsClockwise = isClockwise,
				TargetPosition = position,
				TargetAngle = startAngle
			};

			var fireball = new Mob(FireballMonsterId, RelationType.Neutral);
			fireball.Name = "Void";
			fireball.Position = position;
			// Neutral so it doesn't attack/get attacked
			fireball.Faction = FactionType.Neutral;

			// Add components
			fireball.Components.Add(new MovementComponent(fireball));

			this.Map.AddMonster(fireball);
			fireballData.Fireball = fireball;
			_fireballs.Add(fireballData);
		}
	}

	private void UpdateFireballs(TimeSpan elapsed)
	{
		var destroyDistance = ZoneServer.Instance.Conf.World.MarbleShooterInteractionDistance;

		foreach (var fireballData in _fireballs)
		{
			if (fireballData.Fireball.IsDead)
				continue;

			// Check if fireball is close to its target position
			var distanceToTarget = Math.Sqrt(
				Math.Pow(fireballData.Fireball.Position.X - fireballData.TargetPosition.X, 2) +
				Math.Pow(fireballData.Fireball.Position.Z - fireballData.TargetPosition.Z, 2));

			// Only calculate new target when close to current target (within 10 units for smoother circles)
			if (distanceToTarget < 10)
			{
				// Store original state for potential reversion
				var originalAngle = fireballData.TargetAngle;
				var originalDirection = fireballData.IsClockwise;

				// Calculate target angle 10 degrees ahead for smooth circular movement
				var angleChange = 10f;
				var newTargetAngle = fireballData.TargetAngle;

				if (fireballData.IsClockwise)
					newTargetAngle += angleChange;
				else
					newTargetAngle -= angleChange;

				// Keep angle in 0-360 range
				while (newTargetAngle >= 360)
					newTargetAngle -= 360;
				while (newTargetAngle < 0)
					newTargetAngle += 360;

				// Calculate new target position around the altar using FIXED radius
				var radians = newTargetAngle * Math.PI / 180;
				var x = _altar.Position.X + (float)(Math.Cos(radians) * fireballData.Radius);
				var z = _altar.Position.Z + (float)(Math.Sin(radians) * fireballData.Radius);
				var newTargetPosition = new Position(x, _altar.Position.Y, z);

				// Check if new position is valid (not hitting a wall)
				if (!this.Map.Ground.IsValidPosition(newTargetPosition))
				{
					// Hit a wall, reverse direction
					fireballData.IsClockwise = !originalDirection;

					// Try moving in the opposite direction
					newTargetAngle = originalAngle;
					if (fireballData.IsClockwise)
						newTargetAngle += angleChange;
					else
						newTargetAngle -= angleChange;

					// Keep angle in 0-360 range
					while (newTargetAngle >= 360)
						newTargetAngle -= 360;
					while (newTargetAngle < 0)
						newTargetAngle += 360;

					// Recalculate position with new direction using FIXED radius
					radians = newTargetAngle * Math.PI / 180;
					x = _altar.Position.X + (float)(Math.Cos(radians) * fireballData.Radius);
					z = _altar.Position.Z + (float)(Math.Sin(radians) * fireballData.Radius);
					newTargetPosition = new Position(x, _altar.Position.Y, z);

					// If still invalid after reversal, keep current target
					if (!this.Map.Ground.IsValidPosition(newTargetPosition))
					{
						newTargetPosition = fireballData.TargetPosition;
						newTargetAngle = originalAngle;
					}
				}

				// Update target position and angle
				fireballData.TargetPosition = newTargetPosition;
				fireballData.TargetAngle = newTargetAngle;

				// Use MovementComponent to move to the new target position
				if (fireballData.Fireball.Components.TryGet<MovementComponent>(out var movement))
				{
					movement.MoveStraight(newTargetPosition);
				}
			}

			// Check collision with shooting marbles
			foreach (var shootingMarble in _shootingMarbles.ToArray())
			{
				var marble = shootingMarble.Marble;

				var distance = Math.Sqrt(
					Math.Pow(marble.Position.X - fireballData.Fireball.Position.X, 2) +
					Math.Pow(marble.Position.Z - fireballData.Fireball.Position.Z, 2));

				if (distance <= destroyDistance)
				{
					// Marble hit by fireball
					this.KillMarbleWithEffect(marble);
					_shootingMarbles.Remove(shootingMarble);
				}
			}
		}
	}

	private void ShootMarble(Npc marble, Character player)
	{
		// Calculate shoot direction (opposite of player's position relative to marble)
		var directionX = marble.Position.X - player.Position.X;
		var directionZ = marble.Position.Z - player.Position.Z;
		var magnitude = (float)Math.Sqrt(directionX * directionX + directionZ * directionZ);

		if (magnitude > 0)
		{
			directionX /= magnitude;
			directionZ /= magnitude;
		}

		// Calculate speed (same as fireballs)
		var speedMultiplier = ZoneServer.Instance.Conf.World.MarbleShooterMarbleSpeedMultiplier;
		var marbleSpeed = _averageMonsterLevel * speedMultiplier;
		// Clamp speed between 20 and 120
		marbleSpeed = Math.Max(20f, Math.Min(120f, marbleSpeed));

		// Add to shooting marbles list
		_shootingMarbles.Add(new ShootingMarbleData
		{
			Marble = marble,
			VelocityX = directionX * marbleSpeed,
			VelocityZ = directionZ * marbleSpeed
		});
	}

	private void KillMarbleWithEffect(Npc marble)
	{
		// Play visual effect for all nearby players
		var nearbyPlayers = this.GetCharactersInRange(marble.Position, 500);
		foreach (var player in nearbyPlayers)
		{
			Send.ZC_NORMAL.PlayEffect(player.Connection, marble, heightOffset: EffectLocation.Top, animationName: "F_light029_white");
		}

		// Remove marble from list and map
		_marbles.Remove(marble);
		this.Map.RemoveMonster(marble);

		// Respawn a new marble
		var marbleMinRange = ZoneServer.Instance.Conf.World.MarbleShooterMarbleMinRange;
		var marbleMaxRange = ZoneServer.Instance.Conf.World.MarbleShooterMarbleMaxRange;
		this.SpawnSingleMarble(marbleMinRange, marbleMaxRange);
	}

	private void UpdateShootingMarbles(TimeSpan elapsed)
	{
		var hitDistance = ZoneServer.Instance.Conf.World.MarbleShooterInteractionDistance;
		var toRemove = new List<ShootingMarbleData>();

		foreach (var shootingMarble in _shootingMarbles)
		{
			var marble = shootingMarble.Marble;

			// Calculate the speed for this marble based on its velocity
			var speed = (float)Math.Sqrt(shootingMarble.VelocityX * shootingMarble.VelocityX +
				shootingMarble.VelocityZ * shootingMarble.VelocityZ);

			// Calculate destination far enough ahead (0.5 seconds) to meet minimum movement distance of 10 units
			// MovementComponent rejects movements less than 10 units
			var moveTime = 0.5f;
			var deltaX = shootingMarble.VelocityX * moveTime;
			var deltaZ = shootingMarble.VelocityZ * moveTime;
			var newPos = new Position(
				marble.Position.X + deltaX,
				marble.Position.Y,
				marble.Position.Z + deltaZ);

			// Check if the new position is valid before moving
			if (!this.Map.Ground.IsValidPosition(newPos))
			{
				// Marble hit a wall or went out of bounds, destroy it
				this.KillMarbleWithEffect(marble);
				toRemove.Add(shootingMarble);

				continue; // Skip further processing for this marble
			}

			// Use MovementComponent to move marble (sends packets to clients)
			// Use MoveStraight instead of MoveTo to avoid pathfinding (marbles should move in straight lines)
			if (marble.Components.TryGet<MovementComponent>(out var movement))
			{
				movement.MoveStraight(newPos);
			}

			// Check distance to altar
			var distance = Math.Sqrt(
				Math.Pow(marble.Position.X - _altar.Position.X, 2) +
				Math.Pow(marble.Position.Z - _altar.Position.Z, 2));

			if (distance <= hitDistance)
			{
				// Marble hit the altar!
				_marblesHitAltar++;

				// Notify players
				var rewardRange = ZoneServer.Instance.Conf.World.MarbleShooterRewardRange;
				var requiredHits = ZoneServer.Instance.Conf.World.MarbleShooterRequiredHits;
				this.SendNoticeInRange(this.SpawnPosition, rewardRange,
					string.Format("Marble hit! ({0}/{1})", _marblesHitAltar, requiredHits),
					"Clear", 3);

				// Remove marble
				this.Map.RemoveMonster(marble);
				_marbles.Remove(marble);
				toRemove.Add(shootingMarble);

				// Only respawn a new marble if we haven't reached the win condition yet
				if (_marblesHitAltar < requiredHits)
				{
					var marbleMinRange = ZoneServer.Instance.Conf.World.MarbleShooterMarbleMinRange;
					var marbleMaxRange = ZoneServer.Instance.Conf.World.MarbleShooterMarbleMaxRange;
					this.SpawnSingleMarble(marbleMinRange, marbleMaxRange);
				}
			}

			// Check if marble went too far from spawn
			var distanceFromSpawn = Math.Sqrt(
				Math.Pow(marble.Position.X - this.SpawnPosition.X, 2) +
				Math.Pow(marble.Position.Z - this.SpawnPosition.Z, 2));

			if (distanceFromSpawn > 500)
			{
				// Remove marble and respawn
				this.Map.RemoveMonster(marble);
				_marbles.Remove(marble);
				toRemove.Add(shootingMarble);

				// Respawn a new marble
				var marbleMinRange = ZoneServer.Instance.Conf.World.MarbleShooterMarbleMinRange;
				var marbleMaxRange = ZoneServer.Instance.Conf.World.MarbleShooterMarbleMaxRange;
				this.SpawnSingleMarble(marbleMinRange, marbleMaxRange);
			}
		}

		foreach (var item in toRemove)
		{
			_shootingMarbles.Remove(item);
		}
	}

	private void SpawnMonsterWave()
	{
		var monstersToSpawn = _monstersPerWave;

		// Spawn at random positions around the altar
		for (var i = 0; i < monstersToSpawn; i++)
		{
			Position spawnPos = this.SpawnPosition;
			var foundValidPos = false;

			// Try random positions around altar
			for (var k = 0; k < 100; k++)
			{
				var randomAngle = this.Rnd.Next(360) * Math.PI / 180;
				var randomDistance = 150 + this.Rnd.Next(100);
				var randomX = this.SpawnPosition.X + (float)(Math.Cos(randomAngle) * randomDistance);
				var randomZ = this.SpawnPosition.Z + (float)(Math.Sin(randomAngle) * randomDistance);
				var randPos = new Position(randomX, this.SpawnPosition.Y, randomZ);

				if (this.Map.Ground.IsValidPosition(randPos))
				{
					spawnPos = randPos;
					foundValidPos = true;
					break;
				}
			}

			if (!foundValidPos)
			{
				spawnPos = this.SpawnPosition;
			}

			// Select random monster from available monsters
			var monsterId = _availableMonsterIds[this.Rnd.Next(_availableMonsterIds.Count)];
			var monster = new Mob(monsterId, RelationType.Enemy);
			monster.Position = spawnPos;
			monster.SpawnPosition = spawnPos;
			monster.Direction = new Direction(this.Rnd.Next(360));

			// Add required components
			monster.Components.Add(new MovementComponent(monster));
			monster.Components.Add(new LifeTimeComponent(monster, TimeSpan.FromMinutes(5)));

			// Add AI component
			var monsterData = ZoneServer.Instance.Data.MonsterDb.Find(monsterId);
			if (monsterData != null && !string.IsNullOrEmpty(monsterData.AiName) && AiScript.Exists(monsterData.AiName))
				monster.Components.Add(new AiComponent(monster, monsterData.AiName));
			else
				monster.Components.Add(new AiComponent(monster, "BasicMonster"));

			// Make monsters aggressive
			monster.Tendency = TendencyType.Aggressive;

			this.Map.AddMonster(monster);
			_spawnedMonsters.Add(monster);
		}
	}

	private void SendTimeNotification(float minutesRemaining)
	{
		if (_isCompleted || !this.IsActive)
			return;

		var rewardRange = ZoneServer.Instance.Conf.World.MarbleShooterRewardRange;
		string message;

		if (minutesRemaining >= 1)
			message = string.Format("{0} minute{1} remaining!", (int)minutesRemaining, minutesRemaining > 1 ? "s" : "");
		else
			message = "30 seconds remaining!";

		this.SendNoticeInRange(this.SpawnPosition, rewardRange, message, "scroll", 5);
	}

	private void OnMinigameSuccess()
	{
		_isCompleted = true;

		var rewardRange = ZoneServer.Instance.Conf.World.MarbleShooterRewardRange;
		this.SendNoticeInRange(this.SpawnPosition, rewardRange,
			"Victory! You've successfully completed the Marble Shooter challenge!", "Clear", 10);

		// Give rewards
		this.GiveRewards();

		// Clean up
		this.Cleanup();
		this.End();
	}

	private void OnMinigameFailed()
	{
		_isCompleted = true;

		var rewardRange = ZoneServer.Instance.Conf.World.MarbleShooterRewardRange;
		this.SendNoticeInRange(this.SpawnPosition, rewardRange,
			"Time's up! The Marble Shooter challenge has ended.", "scroll", 10);

		// Clean up
		this.Cleanup();
		this.End();
	}

	private (int ItemId, int Amount)? GetRewardTierForLevel(int averageLevel)
	{
		foreach (var tier in RewardTiers)
		{
			if (averageLevel >= tier.MinLevel && averageLevel <= tier.MaxLevel)
			{
				return (tier.ItemId, tier.Amount);
			}
		}
		return null;
	}

	private void GiveRewards()
	{
		var rewardRange = ZoneServer.Instance.Conf.World.MarbleShooterRewardRange;

		// Find the appropriate reward tier
		var rewardTier = this.GetRewardTierForLevel(_averageMonsterLevel);
		if (rewardTier == null)
			return;

		var rewardItemId = rewardTier.Value.ItemId;
		var rewardAmount = rewardTier.Value.Amount;

		// Get all participating players who are still near
		var nearbyPlayers = this.GetCharactersInRange(this.SpawnPosition, rewardRange);

		foreach (var player in nearbyPlayers)
		{
			if (_participatingPlayerIds.Contains(player.Handle))
			{
				try
				{
					player.Inventory.Add(rewardItemId, rewardAmount, InventoryAddType.PickUp);
					player.AddonMessage("NOTICE_Dm_Clear", "You received a reward for completing the challenge!", 8);
				}
				catch (Exception ex)
				{
					player.AddonMessage("NOTICE_Dm_scroll", "Failed to receive reward.", 5);
				}
			}
		}
	}

	private void Cleanup()
	{
		// Remove altar
		if (_altar != null && !_altar.IsDead)
		{
			this.Map.RemoveMonster(_altar);
		}

		// Remove marble NPCs
		foreach (var marble in _marbles)
		{
			this.Map.RemoveMonster(marble);
		}
		_marbles.Clear();

		// Remove fireballs
		foreach (var fireballData in _fireballs)
		{
			if (!fireballData.Fireball.IsDead)
			{
				this.Map.RemoveMonster(fireballData.Fireball);
			}
		}
		_fireballs.Clear();

		// Spawned monsters will expire on their own via LifeTimeComponent
		_spawnedMonsters.Clear();
	}

	protected override void OnEnded()
	{
		this.Cleanup();
	}
}
