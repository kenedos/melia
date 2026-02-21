using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Network;
using Melia.Shared.World;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Melia.Zone.World.Maps.Pathfinding;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Maps.Spatial;
using Yggdrasil.Geometry;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Logging;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World.Maps
{
	public class Map : IUpdateable
	{
		private volatile int _layer = DefaultLayer;

		#region Constants
		public const int DefaultLayer = 0;
		public const int VisibleRange = 500;
		private const int MaxMonsterAddsPerTick = 5;
		private static readonly TimeSpan EntityUpdateGracePeriod = TimeSpan.FromMinutes(5);
		#endregion

		#region Collections - Thread-safe collections for better performance
		protected readonly ConcurrentDictionary<int, ICombatEntity> _combatEntities = new();
		protected readonly ConcurrentDictionary<int, Character> _characters = new();
		protected readonly ConcurrentDictionary<int, IMonster> _monsters = new();
		protected readonly ConcurrentDictionary<int, ITriggerableArea> _triggerableAreas = new();
		protected readonly ConcurrentDictionary<int, Pad> _pads = new();

		// Keep these as regular collections with locks for specific operations
		protected readonly List<DynamicObstacle> _obstacles = new();
		private readonly object _obstaclesLock = new();

		private readonly ConcurrentQueue<IMonster> _addMonsters = new();
		private DateTime _lastPlayerLeftTime = DateTime.MinValue;
		protected readonly ConcurrentDictionary<int, PropertyOverrides> _monsterPropertyOverrides = new();

		// Pooled lists for temporary operations
		protected readonly ThreadLocal<List<IUpdateable>> _updateEntitiesPool = new(() => new List<IUpdateable>());
		protected readonly ThreadLocal<List<Character>> _updateVisibleCharactersPool = new(() => new List<Character>());

		// Spatial index for efficient range queries
		private EntitySpatialIndex _spatialIndex;
		#endregion

		#region Properties
		public int WorldId { get; protected set; }
		public string ClassName { get; protected set; }
		public int Id { get; protected set; }
		public MapData Data { get; protected set; }
		public Ground Ground { get; } = new Ground();
		public IPathfinder Pathfinder { get; private set; }

		// Thread-safe property accessors
		public int CharacterCount => _characters.Count;
		public int MonsterCount => _monsters.Count;
		public bool HasCharacters => this.CharacterCount > 0;

		public bool IsPVP { get; set; }
		public bool IsRaid { get; set; }
		public bool IsGTW { get; protected set; }
		public bool IsCity { get; set; }
		public bool IsTOSHeroZone => this.Data?.Tags.Has(SkillTag.ExpertSkill) ?? false;
		public bool IsInstance => this.Data?.Type == MapType.Instance;
		public bool TeleportDisabled { get; internal set; }

		public float AverageMonsterLevel { get; private set; }
		public static Map Limbo { get; } = new Limbo();
		#endregion

		#region Events
		public event Action<Character> PlayerEnters;
		public event Action<Character> PlayerLeaves;
		#endregion

		#region Constructor and Initialization
		public Map(int id, string name)
		{
			this.Id = id;
			this.WorldId = id;
			this.ClassName = name;
			this.Load();
		}

		private void Load()
		{
			this.Data = ZoneServer.Instance.Data.MapDb.Find(this.Id);

			this.ClassName ??= this.Data.ClassName;

			if (this.Data != null)
				this.IsCity = this.Data.Type == MapType.City;

			// Load ground data
			if (this.Data != null && ZoneServer.Instance.Data.GroundDb.TryFind(this.Data.ClassName, out var groundData))
				this.Ground.Load(groundData);

			// Initialize spatial index for efficient range queries
			_spatialIndex = new EntitySpatialIndex(this.Ground.Left, this.Ground.Bottom, this.Ground.Right, this.Ground.Top);

			// Initialize pathfinder
			this.InitializePathfinder();
		}

		private void InitializePathfinder()
		{
			if (ZoneServer.Instance.Conf.World.MonstersUsePathfinding)
			{
				this.Pathfinder = this.Ground.GraphNodes?.Length > 0
					? new NavGraphNodePathfinder(this)
					: new DynamicGridPathfinder(this);
			}
			else
			{
				this.Pathfinder = new NonePathfinder();
			}
		}
		#endregion

		#region Update Methods
		public virtual void Update(TimeSpan elapsed)
		{
			using (Debug.Profile($"Map.Update: {this.ClassName}", 150))
			{
				this.ProcessDisappearances();
				this.UpdateVisibility();
				this.UpdateEntities(elapsed);
			}
		}

		private void UpdateEntities(TimeSpan elapsed)
		{
			// Process pending monster additions (throttled to prevent
			// packet storms when many monsters spawn simultaneously)
			for (var i = 0; i < MaxMonsterAddsPerTick && _addMonsters.TryDequeue(out var monster); i++)
				this.AddMonsterInternal(monster);

			var updateList = _updateEntitiesPool.Value;
			updateList.Clear();

			try
			{
				// Collect updateables - update monsters if players are on
				// the map or if players left recently (grace period for
				// mobs to return to spawn, despawn via lifetime, etc.)
				var withinGracePeriod = _lastPlayerLeftTime != DateTime.MinValue && (DateTime.Now - _lastPlayerLeftTime) < EntityUpdateGracePeriod;
				if (this.HasCharacters || withinGracePeriod)
				{
					updateList.AddRange(_monsters.Values.OfType<IUpdateable>());
				}

				// Update pads before characters so enter/leave detection
				// happens first
				updateList.AddRange(_pads.Values);
				updateList.AddRange(_characters.Values);

				// Update all entities
				foreach (var entity in updateList)
					entity.Update(elapsed);
			}
			finally
			{
				updateList.Clear();
			}
		}

		private void ProcessDisappearances()
		{
			var now = DateTime.Now;

			// Process character disappearances
			var expiredCharacters = _characters.Values.Where(c => c.DisappearTime < now).ToList();
			foreach (var character in expiredCharacters)
			{
				character.OnDisappear?.Invoke();
				this.RemoveCharacter(character);
			}

			// Process monster disappearances
			var expiredMonsters = _monsters.Values.Where(m => m.DisappearTime < now).ToList();
			foreach (var monster in expiredMonsters)
			{
				monster.OnDisappear?.Invoke();
				ZoneServer.Instance.ServerEvents.MonsterDisappears.Raise(new MonsterEventArgs(monster));
				this.RemoveMonster(monster);
			}

			// Process pad disappearances
			var expiredPads = _pads.Values.Where(p => p.DisappearTime < now).ToList();
			foreach (var pad in expiredPads)
			{
				pad.OnDisappear?.Invoke();
				this.RemovePad(pad);
			}
		}

		private void UpdateVisibility()
		{
			var visibilityList = _updateVisibleCharactersPool.Value;
			visibilityList.Clear();

			try
			{
				visibilityList.AddRange(_characters.Values);

				foreach (var character in visibilityList)
					character.LookAround();
			}
			finally
			{
				visibilityList.Clear();
			}
		}

		#endregion

		#region Character Management
		/// <summary>
		/// Adds the character to the map and raises the player entered event.
		/// </summary>
		public void AddCharacter(Character character)
		{
			character.Map = this;
			_characters[character.Handle] = character;

			if (character is ICombatEntity combatEntity)
			{
				_combatEntities[character.Handle] = combatEntity;
				_spatialIndex?.Insert(combatEntity);
			}

			_lastPlayerLeftTime = DateTime.MinValue;

			ZoneServer.Instance.UpdateServerInfo();
			ZoneServer.Instance.ServerEvents.PlayerEnteredMap.Raise(new PlayerEventArgs(character));
			PlayerEnters?.Invoke(character);
		}

		/// <summary>
		/// Removes the character from the map and raises the player left event.
		/// </summary>
		public void RemoveCharacter(Character character)
		{
			_characters.TryRemove(character.Handle, out _);
			_combatEntities.TryRemove(character.Handle, out _);
			_spatialIndex?.Remove(character);

			ZoneServer.Instance.ServerEvents.PlayerLeftMap.Raise(new PlayerEventArgs(character));
			character.Map = null;

			if (!this.HasCharacters)
				_lastPlayerLeftTime = DateTime.Now;

			ZoneServer.Instance.UpdateServerInfo();
			PlayerLeaves?.Invoke(character);
		}

		/// <summary>
		/// Returns the first non-dummy character with the given team name,
		/// or null if not found.
		/// </summary>
		public Character GetCharacterByTeamName(string teamName) => this.GetCharacter(a => a.TeamName == teamName && a.Connection is not DummyConnection);

		/// <summary>
		/// Returns the first character matching the predicate, or null.
		/// </summary>
		public Character GetCharacter(Func<Character, bool> predicate) => _characters.Values.FirstOrDefault(predicate);

		/// <summary>
		/// Returns the character with the given handle via out.
		/// Returns false if not found.
		/// </summary>
		public bool TryGetCharacter(int handle, out Character character) => _characters.TryGetValue(handle, out character);

		/// <summary>
		/// Returns all non-dummy characters on the map.
		/// </summary>
		public Character[] GetCharacters() => this.GetCharacters(c => c.Connection is not DummyConnection);

		/// <summary>
		/// Returns all characters matching the predicate.
		/// </summary>
		public Character[] GetCharacters(Func<Character, bool> predicate) => _characters.Values.Where(predicate).ToArray();

		/// <summary>
		/// Returns all characters visible to the given character.
		/// </summary>
		public Character[] GetVisibleCharacters(Character character) => this.GetCharacters(character.CanSee);
		#endregion

		#region Monster Management
		/// <summary>
		/// Queues a monster to be added to the map on the next update tick.
		/// </summary>
		public void AddMonster(IMonster monster)
		{
			_addMonsters.Enqueue(monster);
		}

		private void AddMonsterInternal(IMonster monster)
		{
			monster.Map = this;
			_monsters[monster.Handle] = monster;

			if (monster is ICombatEntity entity)
			{
				_combatEntities[monster.Handle] = entity;
				_spatialIndex?.Insert(entity);
			}

			if (monster is ITriggerableArea trigger)
				_triggerableAreas[monster.Handle] = trigger;

			if (monster is ItemMonster itemMonster)
				this.TryMergeNearbyItems(itemMonster);

			monster.Components.Get<TriggerComponent>()?.OnAddedToMap();
			monster.FromGround = false;
		}

		private void TryMergeNearbyItems(ItemMonster newItem)
		{
			if (!newItem.Item.IsStackable)
				return;

			var itemId = newItem.Item.Id;
			var layer = newItem.Layer;
			var pos = newItem.Position;
			var ownerId = newItem.Item.OwnerCharacterId;

			var itemMergeRange = 30f;
			var itemMergeThreshold = 5;

			var nearbyItems = new List<ItemMonster>();

			foreach (var monster in _monsters.Values)
			{
				if (monster == newItem || monster is not ItemMonster im)
					continue;
				if (im.PickedUp || im.Item.Id != itemId || im.Layer != layer || im.Item.OwnerCharacterId != ownerId)
					continue;
				if (!im.Position.InRange2D(pos, itemMergeRange))
					continue;
				nearbyItems.Add(im);
			}

			if (nearbyItems.Count + 1 < itemMergeThreshold)
				return;

			var totalX = pos.X;
			var totalY = pos.Y;
			var totalZ = pos.Z;
			long totalAmount = newItem.Item.Amount;

			foreach (var im in nearbyItems)
			{
				totalX += im.Position.X;
				totalY += im.Position.Y;
				totalZ += im.Position.Z;
				totalAmount += im.Item.Amount;
			}

			var count = nearbyItems.Count + 1;
			var avgPos = new Position(totalX / count, totalY / count, totalZ / count);

			newItem.Item.Amount = (int)Math.Min(totalAmount, newItem.Item.Data.MaxStack);
			newItem.Position = avgPos;

			foreach (var im in nearbyItems)
				this.RemoveMonster(im);
		}

		/// <summary>
		/// Removes a monster from the map, cleaning up its components
		/// and AI state.
		/// </summary>
		public void RemoveMonster(IMonster monster)
		{
			monster.Components.Get<TriggerComponent>()?.OnRemovingFromMap();
			monster.Components.Get<MovementComponent>()?.RemoveMarker();

			_monsters.TryRemove(monster.Handle, out _);
			_combatEntities.TryRemove(monster.Handle, out _);
			_triggerableAreas.TryRemove(monster.Handle, out _);

			if (monster is ICombatEntity entity)
				_spatialIndex?.Remove(entity);

			if (monster.Components.TryGet<AiComponent>(out var ai))
			{
				ai.Script?.ClearEventAlerts();
				ai.Script?.ClearTarget();
			}

			monster.Map = null;
		}

		/// <summary>
		/// Removes all monsters and pads on the given layer from the map.
		/// </summary>
		public void RemoveEntitiesOnLayer(int layer)
		{
			var toRemove = _monsters.Values.Where(m => m.Layer == layer).ToList();
			foreach (var monster in toRemove)
				this.RemoveMonster(monster);
			var toRemovePads = _pads.Values.Where(p => p.Layer == layer).ToList();
			foreach (var pad in toRemovePads)
				this.RemovePad(pad);
		}

		/// <summary>
		/// Returns the monster with the given handle, or null if not found.
		/// </summary>
		public IMonster GetMonster(int handle) => _monsters.TryGetValue(handle, out var monster) ? monster : null;

		/// <summary>
		/// Returns the first monster matching the predicate, or null.
		/// </summary>
		public IMonster GetMonster(Func<IMonster, bool> predicate) => _monsters.Values.FirstOrDefault(predicate);

		/// <summary>
		/// Returns the monster with the given handle via out.
		/// Returns false if not found.
		/// </summary>
		public bool TryGetMonster(int handle, out IMonster monster) => _monsters.TryGetValue(handle, out monster);

		/// <summary>
		/// Returns the first monster matching the predicate via out.
		/// Returns false if not found.
		/// </summary>
		public bool TryGetMonster(Func<IMonster, bool> predicate, out IMonster monster)
		{
			foreach (var m in _monsters.Values)
			{
				if (predicate(m))
				{
					monster = m;
					return true;
				}
			}
			monster = null;
			return false;
		}

		/// <summary>
		/// Returns all monsters currently on the map.
		/// </summary>
		public IMonster[] GetMonsters() => _monsters.Values.ToArray();

		/// <summary>
		/// Returns all monsters matching the predicate.
		/// </summary>
		public IMonster[] GetMonsters(Func<IMonster, bool> predicate) => _monsters.Values.Where(predicate).ToArray();

		/// <summary>
		/// Returns all monsters visible to the given character.
		/// </summary>
		public IMonster[] GetVisibleMonsters(Character character) => this.GetMonsters(character.CanSee);

		/// <summary>
		/// Returns all pads visible to the given character.
		/// </summary>
		public Pad[] GetVisiblePads(Character character) => this.GetPads(character.CanSee);

		/// <summary>
		/// Removes all scripted entities (monsters) from the map.
		/// </summary>
		public void RemoveScriptedEntities()
		{
			var toRemove = _monsters.Values.ToList();
			foreach (var monster in toRemove)
				this.RemoveMonster(monster);
		}
		#endregion

		#region Obstacle Management
		/// <summary>
		/// Adds a dynamic obstacle to the map.
		/// </summary>
		public void AddObstacle(DynamicObstacle obstacle)
		{
			lock (_obstaclesLock)
			{
				_obstacles.Add(obstacle);
				obstacle.Map = this;
			}
		}

		/// <summary>
		/// Adds multiple dynamic obstacles to the map.
		/// </summary>
		public void AddObstacles(IEnumerable<DynamicObstacle> obstacles)
		{
			lock (_obstaclesLock)
			{
				foreach (var obstacle in obstacles)
				{
					_obstacles.Add(obstacle);
					obstacle.Map = this;
				}
			}
		}

		/// <summary>
		/// Removes a dynamic obstacle from the map.
		/// </summary>
		public void RemoveObstacle(DynamicObstacle obstacle)
		{
			if (obstacle == null) return;

			lock (_obstaclesLock)
			{
				_obstacles.Remove(obstacle);
				obstacle.Map = null;
			}
		}

		/// <summary>
		/// Returns all dynamic obstacles currently on the map.
		/// </summary>
		public DynamicObstacle[] GetObstacles()
		{
			lock (_obstaclesLock)
				return _obstacles.ToArray();
		}
		#endregion

		#region Pad Management
		/// <summary>
		/// Adds a pad to the map and activates its trigger.
		/// </summary>
		public void AddPad(Pad pad)
		{
			pad.Map = this;
			_pads[pad.Handle] = pad;
			pad.Components.Get<TriggerComponent>()?.OnAddedToMap();
		}

		/// <summary>
		/// Removes a pad from the map and deactivates its trigger.
		/// </summary>
		public void RemovePad(Pad pad)
		{
			pad.Components.Get<TriggerComponent>()?.OnRemovingFromMap();
			_pads.TryRemove(pad.Handle, out _);
			pad.Map = null;
		}

		/// <summary>
		/// Returns the pad with the given handle via out.
		/// Returns false if not found.
		/// </summary>
		public bool TryGetPad(int handle, out Pad pad) => _pads.TryGetValue(handle, out pad);

		/// <summary>
		/// Returns all pads within range of or overlapping the given position.
		/// </summary>
		public Pad[] GetPadsAt(Position pos, float range) =>
			_pads.Values.Where(a => a.Position.InRange2D(pos, range) || (a.Area?.IsInside(pos) ?? false)).ToArray();

		/// <summary>
		/// Returns all pads matching the given predicate.
		/// </summary>
		public Pad[] GetPads(Func<Pad, bool> func) => _pads.Values.Where(func).ToArray();

		/// <summary>
		/// Returns all pads that can hit the given entity based on faction
		/// rules and area overlap.
		/// </summary>
		public Pad[] GetHittablePadsAt(ICombatEntity entity) =>
			_pads.Values.Where(a => entity.IsHitByPad() && (a.Area?.IsInside(entity.Position) ?? false)).ToArray();
		#endregion

		#region Combat Entity Management
		/// <summary>
		/// Returns the combat entity with the given handle, or null if
		/// not found.
		/// </summary>
		public ICombatEntity GetCombatEntity(int handle)
		{
			return _combatEntities.TryGetValue(handle, out var entity) ? entity : null;
		}

		/// <summary>
		/// Returns the combat entity with the given handle via out.
		/// Returns false if not found.
		/// </summary>
		public bool TryGetCombatEntity(int handle, out ICombatEntity entity) =>
			_combatEntities.TryGetValue(handle, out entity);

		/// <summary>
		/// Returns any actor (monster or character) with the given handle
		/// via out. Returns false if not found.
		/// </summary>
		public bool TryGetActor(int handle, out IActor actor)
		{
			if (_monsters.TryGetValue(handle, out var monster))
			{
				actor = monster;
				return true;
			}

			if (_characters.TryGetValue(handle, out var character))
			{
				actor = character;
				return true;
			}

			actor = null;
			return false;
		}
		#endregion

		#region Spatial Queries - Optimized with better algorithms
		/// <summary>
		/// Returns all triggerable areas whose trigger zone contains
		/// the given position.
		/// </summary>
		public ITriggerableArea[] GetTriggerableAreasAt(Position pos) =>
			_triggerableAreas.Values.Where(a => a.Area?.IsInside(pos) ?? false).ToArray();

		/// <summary>
		/// Returns all items in a circular area around the target position
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="position"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public List<ItemMonster> GetItemsInPosition(Position position, float radius)
		{
			var shape = new CircleF(position, radius);
			return this.GetActorsIn<ItemMonster>(shape).OrderBy(e => position.Get2DDistance(e.Position)).ToList();
		}


		/// <summary>
		/// Returns all enemies that can be attacked in a circular area
		/// around the target position
		/// </summary>
		/// <remarks>
		/// Returns list ordered by distance to position
		/// </remarks>
		/// <param name="attacker"></param>
		/// <param name="position"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public List<ICombatEntity> GetAttackableEnemiesInPosition(ICombatEntity attacker, Position position, float radius)
		{
			IEnumerable<ICombatEntity> candidates;

			if (_spatialIndex != null)
			{
				candidates = _spatialIndex.QueryCircle(position, radius + MaxAgentRadius);
			}
			else
			{
				candidates = _combatEntities.Values;
			}

			var query = candidates
				.Where(e =>
				{
					var effectiveRadius = radius + e.AgentRadius;
					var dx = e.Position.X - position.X;
					var dz = e.Position.Z - position.Z;
					return attacker.CanAttack(e) && dx * dx + dz * dz <= effectiveRadius * effectiveRadius;
				})
				.OrderBy(e => position.Get2DDistance(e.Position));

			return query.ToList();
		}

		/// <summary>
		/// Maximum agent radius used to expand spatial query bounds so
		/// that large entities near shape edges are not missed.
		/// </summary>
		private const float MaxAgentRadius = 40f;

		/// <summary>
		/// Returns all enemies that can be attacked inside the given shape
		/// </summary>
		/// <remarks>
		/// Returns list ordered by distance to attacker
		/// </remarks>
		/// <param name="attacker"></param>
		/// <param name="shape"></param>
		/// <param name="maxTargets"></param>
		/// <param name="exclude"></param>
		/// <returns></returns>
		public List<ICombatEntity> GetAttackableEnemiesIn(ICombatEntity attacker, IShapeF shape, int maxTargets = 0, params ICombatEntity[] exclude)
		{
			if (attacker is Character rangePreviewCharacter && rangePreviewCharacter.Variables.Temp.GetBool("Melia.RangePreview"))
				Debug.ShowShape(this, shape, TimeSpan.FromSeconds(1));

			IEnumerable<ICombatEntity> candidates;

			if (_spatialIndex != null)
			{
				candidates = _spatialIndex.QueryShape(shape);
			}
			else
			{
				candidates = _combatEntities.Values;
			}

			var excludeSet = exclude?.Length > 0 ? new HashSet<ICombatEntity>(exclude) : null;
			var query = candidates
				.Where(e => (excludeSet == null || !excludeSet.Contains(e)) &&
						   attacker.CanAttack(e) &&
						   shape.IsInsideOrInRange(e.Position, e.AgentRadius))
				.OrderBy(e => attacker.GetDistance(e));

			return maxTargets > 0 ? query.Take(maxTargets).ToList() : query.ToList();
		}

		/// <summary>
		/// Returns all alive allies (friendly, party or guild relations) in the given shape
		/// </summary>
		/// <remarks>
		/// Returns list ordered by distance to attacker
		/// </remarks>
		/// <param name="ally"></param>
		/// <param name="shape"></param>
		/// <param name="maxTargets"></param>
		/// <returns></returns>
		public List<ICombatEntity> GetAliveAlliedEntitiesIn(ICombatEntity ally, IShapeF shape, int maxTargets = 0)
		{
			IEnumerable<ICombatEntity> candidates;

			if (_spatialIndex != null)
			{
				candidates = _spatialIndex.QueryShape(shape);
			}
			else
			{
				candidates = _combatEntities.Values;
			}

			var query = candidates
					.Where(entity => (
						entity.IsAlly(ally)) &&
						entity != ally &&
						!entity.IsDead &&
						shape.IsInsideOrInRange(entity.Position, entity.AgentRadius))
					.OrderBy(a => ally.GetDistance(a));

			return maxTargets > 0 ? query.Take(maxTargets).ToList() : query.ToList();
		}

		/// <summary>
		/// Returns all dead allies (friendly, party or guild relations) in the given shape
		/// </summary>
		/// <remarks>
		/// Returns list ordered by distance to attacker
		/// </remarks>
		/// <param name="ally"></param>
		/// <param name="shape"></param>
		/// <param name="maxTargets"></param>
		/// <returns></returns>
		public List<ICombatEntity> GetDeadAlliedEntitiesIn(ICombatEntity ally, IShapeF shape, int maxTargets = 0)
		{
			IEnumerable<ICombatEntity> candidates;

			if (_spatialIndex != null)
			{
				candidates = _spatialIndex.QueryShape(shape);
			}
			else
			{
				candidates = _combatEntities.Values;
			}

			var query = candidates
					.Where(entity => (
						entity.IsDeadAlly(ally)) &&
						entity != ally &&
						shape.IsInsideOrInRange(entity.Position, entity.AgentRadius))
					.OrderBy(a => ally.GetDistance(a));

			return maxTargets > 0 ? query.Take(maxTargets).ToList() : query.ToList();
		}

		/// <summary>
		/// Returns all actors in a given area
		/// </summary>
		/// <typeparam name="TActor"></typeparam>
		/// <param name="area"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public List<TActor> GetActorsIn<TActor>(IShapeF area, Func<TActor, bool> predicate = null) where TActor : IActor
		{
			var result = new List<TActor>();

			// Use spatial index for ICombatEntity queries (covers monsters and characters)
			if (_spatialIndex != null && typeof(ICombatEntity).IsAssignableFrom(typeof(TActor)))
			{
				var candidates = _spatialIndex.QueryShape(area);
				foreach (var entity in candidates)
				{
					if (entity is TActor actor && area.IsInsideOrInRange(actor.Position, entity.AgentRadius) && (predicate?.Invoke(actor) ?? true))
						result.Add(actor);
				}
			}
			else
			{
				// Fallback: Check monsters
				foreach (var monster in _monsters.Values)
				{
					var radius = (monster as ICombatEntity)?.AgentRadius ?? 0;
					if (monster is TActor actor && area.IsInsideOrInRange(actor.Position, radius) && (predicate?.Invoke(actor) ?? true))
						result.Add(actor);
				}

				// Check characters
				foreach (var character in _characters.Values)
				{
					if (character is TActor actor && area.IsInsideOrInRange(actor.Position, ((ICombatEntity)character).AgentRadius) && (predicate?.Invoke(actor) ?? true))
						result.Add(actor);
				}
			}

			// Check pads (not in spatial index)
			foreach (var pad in _pads.Values)
			{
				if (pad is TActor actor && area.IsInside(actor.Position) && (predicate?.Invoke(actor) ?? true))
					result.Add(actor);
			}

			return result;
		}

		/// <summary>
		/// Returns all actors of type TActor within the given range of a
		/// center position, optionally filtered by predicate.
		/// </summary>
		public List<TActor> GetActorsInRange<TActor>(Position center, float range, Func<TActor, bool> predicate = null) where TActor : IActor
		{
			var circle = new CircleF(center, range);
			return this.GetActorsIn(circle, predicate);
		}
		#endregion

		#region Collision and Pathfinding
		[Obsolete("Use IsWalkablePosition(ICombatEntity, Position) instead for accurate collision checks.")]
		public bool IsWalkablePosition(Position position, float radius) =>
			this.Ground.IsValidCirclePosition(position, radius) && !this.TryCollideObstacles(position, radius, out _);

		/// <summary>
		/// Returns true if the given entity can stand at the given position,
		/// accounting for ground validity, obstacles, and actor collisions.
		/// </summary>
		public bool IsWalkablePosition(ICombatEntity entity, Position position) =>
			this.Ground.IsValidCirclePosition(position, entity.AgentRadius) &&
			!this.TryCollideObstacles(position, entity.AgentRadius, out _) &&
			!this.TryCollideActors(entity, position, out _);

		/// <summary>
		/// Checks whether a circle at the given position collides with any
		/// dynamic obstacles. Returns the collided obstacles via out.
		/// </summary>
		public bool TryCollideObstacles(Position position, float radius, out List<DynamicObstacle> collidedObstacles)
		{
			var checkPositions = this.GetCollisionCheckPositions(position, radius);
			collidedObstacles = new List<DynamicObstacle>();

			lock (_obstaclesLock)
			{
				foreach (var obstacle in _obstacles)
				{
					if (checkPositions.Any(point => obstacle.Shape.IsInside(point)))
					{
						collidedObstacles.Add(obstacle);
					}
				}
			}

			return collidedObstacles.Count > 0;
		}

		private Vector2F[] GetCollisionCheckPositions(Position position, float radius)
		{
			return new Vector2F[]
			{
				new(position.X, position.Z),                    // Center
				new(position.X + radius, position.Z),           // Right
				new(position.X - radius, position.Z),           // Left
				new(position.X, position.Z + radius),           // Top
				new(position.X, position.Z - radius),           // Bottom
				new(position.X + radius, position.Z + radius),  // Top-Right
				new(position.X - radius, position.Z + radius),  // Top-Left
				new(position.X + radius, position.Z - radius),  // Bottom-Right
				new(position.X - radius, position.Z - radius)   // Bottom-Left
			};
		}

		/// <summary>
		/// Checks whether the given entity's collision circle at a position
		/// overlaps with other entities. Returns the collided entities via out.
		/// </summary>
		public bool TryCollideActors(ICombatEntity requester, Position position, out List<ICombatEntity> collidedEntities)
		{
			collidedEntities = new List<ICombatEntity>();

			// Use spatial index with conservative radius to find nearby entities
			var searchRadius = requester.AgentRadius + 100f;
			IEnumerable<ICombatEntity> candidates;

			if (_spatialIndex != null)
			{
				candidates = _spatialIndex.QueryCircle(position, searchRadius);
			}
			else
			{
				candidates = _combatEntities.Values;
			}

			foreach (var otherEntity in candidates)
			{
				if (otherEntity.Handle == requester.Handle)
					continue;

				var distance = position.Get2DDistance(otherEntity.Position);
				var minSeparation = requester.AgentRadius + otherEntity.AgentRadius;

				if (distance < minSeparation)
					collidedEntities.Add(otherEntity);
			}

			return collidedEntities.Count > 0;
		}
		#endregion

		#region Utility Methods
		/// <summary>
		/// Returns a random walkable position within range of the start
		/// position via out. Returns false if no valid position was found.
		/// </summary>
		public bool TryGetRandomPositionInRange(Position startPos, float range, out Position randomPos)
		{
			randomPos = startPos.GetRandomInRange2D((int)range);
			return this.Ground.IsValidPosition(randomPos);
		}

		/// <summary>
		/// Returns the last walkable position along the path from start to
		/// end for an entity with the given radius, stepping until the path
		/// is blocked.
		/// </summary>
		public Position GetLastWalkablePosition(Position start, float actorRadius, Position end)
		{
			var dir = start.GetDirection(end);
			var stepSize = Math.Max(2.5f, actorRadius * 0.5f);
			var currentPos = start;
			var lastValidPos = currentPos;

			if (this.Ground.TryGetHeightAt(currentPos, out var height))
				lastValidPos.Y = height;

			while (currentPos.Get2DDistance(end) > stepSize)
			{
				currentPos = currentPos.GetRelative(dir, stepSize);

				if (!this.IsWalkablePosition(currentPos, actorRadius))
					return lastValidPos;

				if (this.Ground.TryGetHeightAt(currentPos, out var newHeight))
				{
					lastValidPos = currentPos;
					lastValidPos.Y = newHeight;
				}
				else
				{
					return lastValidPos;
				}
			}

			if (this.IsWalkablePosition(end, actorRadius) && this.Ground.TryGetHeightAt(end, out var destHeight))
			{
				end.Y = destHeight;
				return end;
			}

			return lastValidPos;
		}

		/// <summary>
		/// Returns a new unique layer number for this map.
		/// </summary>
		public int GetNewLayer() => Interlocked.Increment(ref _layer);
		#endregion

		#region Property Overrides
		/// <summary>
		/// Stores property overrides to apply to monsters of the given
		/// class when they are spawned on this map.
		/// </summary>
		public void AddPropertyOverrides(int monsterClassId, PropertyOverrides propertyOverrides) =>
			_monsterPropertyOverrides[monsterClassId] = propertyOverrides;

		/// <summary>
		/// Returns property overrides for the given monster class via out.
		/// Returns false if no overrides are defined.
		/// </summary>
		public bool TryGetPropertyOverrides(int monsterClassId, out PropertyOverrides propertyOverrides) =>
			_monsterPropertyOverrides.TryGetValue(monsterClassId, out propertyOverrides);
		#endregion

		#region Specialized Queries
		/// <summary>
		/// Returns the nearest warp within range of the position that
		/// warps when approached, or null if none found.
		/// </summary>
		public WarpMonster GetNearbyWarp(Position pos) =>
			_monsters.Values.OfType<WarpMonster>()
				.FirstOrDefault(a => a.WarpWhenNearby && a.Position.InRange2D(pos, 35));

		/// <summary>
		/// Returns all warp monsters on the map.
		/// </summary>
		public IEnumerable<WarpMonster> GetWarps() => _monsters.Values.OfType<WarpMonster>();

		/// <summary>
		/// Returns all warp monsters matching the predicate.
		/// </summary>
		public IEnumerable<WarpMonster> GetWarps(Func<WarpMonster, bool> predicate) =>
			_monsters.Values.OfType<WarpMonster>().Where(predicate ?? (_ => true));

		/// <summary>
		/// Returns all NPC monsters on the map.
		/// </summary>
		public IEnumerable<MonsterInName> GetNpcs() => _monsters.Values.OfType<MonsterInName>();

		/// <summary>
		/// Returns all NPC monsters matching the predicate.
		/// </summary>
		public IEnumerable<MonsterInName> GetNpcs(Func<MonsterInName, bool> predicate) =>
			_monsters.Values.OfType<MonsterInName>().Where(predicate ?? (_ => true));

		/// <summary>
		/// Returns the nearest resurrection/safe position to the given
		/// position. Optionally prioritizes resurrection points over the
		/// map's default position.
		/// </summary>
		public Position GetSafePositionNear(Position pos, bool favorResurrectionPoints)
		{
			var positions = ZoneServer.Instance.Data.ResurrectionPointDb.FindPositions(this.ClassName);

			if (positions.Count == 0)
				return this.Data.DefaultPosition;

			if (!favorResurrectionPoints)
				positions.Add(this.Data.DefaultPosition);

			return positions.OrderBy(a => a.Get2DDistance(pos)).First();
		}
		#endregion

		#region AI and Broadcasting
		/// <summary>
		/// Sends an AI event alert to all entities within visible range
		/// of the source actor.
		/// </summary>
		public void AlertNearbyAis(IActor source, IAiEventAlert alert)
		{
			IEnumerable<ICombatEntity> candidates;

			if (_spatialIndex != null)
			{
				candidates = _spatialIndex.QueryCircle(source.Position, VisibleRange);
			}
			else
			{
				candidates = _combatEntities.Values
					.Where(e => e.Position.InRange2D(source.Position, VisibleRange));
			}

			foreach (var combatEntity in candidates)
			{
				if (!combatEntity.Components.TryGet<AiComponent>(out var aiComponent))
					continue;

				aiComponent.Script.QueueEventAlert(alert);
			}
		}

		/// <summary>
		/// Sends an AI event alert to all entities on the map.
		/// </summary>
		public void AlertAis(IAiEventAlert alert)
		{
			foreach (var combatEntity in _combatEntities.Values)
			{
				if (combatEntity.Components.TryGet<AiComponent>(out var aiComponent))
					aiComponent.Script.QueueEventAlert(alert);
			}
		}

		/// <summary>
		/// Broadcasts a packet to all characters on the map.
		/// </summary>
		public virtual void Broadcast(Packet packet)
		{
			foreach (var character in _characters.Values)
				character.Connection.Send(packet);
		}

		/// <summary>
		/// Broadcasts a packet to all characters within visible range of
		/// the source actor, optionally excluding the source itself.
		/// </summary>
		public virtual void Broadcast(Packet packet, IActor source, bool includeSource = true)
		{
			IEnumerable<Character> candidates;

			if (_spatialIndex != null)
			{
				candidates = _spatialIndex.QueryCircle(source.Position, VisibleRange)
					.OfType<Character>();
			}
			else
			{
				candidates = _characters.Values
					.Where(a => a.Position.InRange2D(source.Position, VisibleRange));
			}

			HashSet<IZoneConnection> sentConnections = null;

			foreach (var character in candidates)
			{
				if (!includeSource && character == source)
					continue;

				if (character.Layer != source.Layer)
					continue;

				var conn = character.Connection;
				if (conn == null)
					continue;

				sentConnections ??= new HashSet<IZoneConnection>();
				if (!sentConnections.Add(conn))
					continue;

				conn.Send(packet);
			}
		}
		#endregion

		#region Advanced Queries
		/// <summary>
		/// Returns all characters whose position is inside or within agent
		/// radius of the given shape.
		/// </summary>
		public Character[] GetCharactersInside(IShapeF shape) => this.GetCharacters(a => shape.IsInsideOrInRange(a.Position, ((ICombatEntity)a).AgentRadius));

		/// <summary>
		/// Returns all characters whose position is outside the given shape.
		/// </summary>
		public Character[] GetCharactersOutside(IShapeF shape) => this.GetCharacters(a => !shape.IsInsideOrInRange(a.Position, ((ICombatEntity)a).AgentRadius));

		/// <summary>
		/// Returns all alive (or dead) party members of the character
		/// within the given radius of the character's position.
		/// </summary>
		public List<Character> GetPartyMembersInRange(Character character, float radius, bool areAlive = true) =>
			this.GetPartyMembersInRange(character, character.Position, radius, areAlive);

		/// <summary>
		/// Returns all alive (or dead) party members of the character
		/// within the given radius of a specific position.
		/// </summary>
		public List<Character> GetPartyMembersInRange(Character character, Position position, float radius, bool areAlive = true)
		{
			if (character.Connection.Party == null) return new List<Character>();

			var party = character.Connection.Party;
			return _characters.Values
				.Where(a => (radius == 0 || a.Position.InRange2D(position, radius)) &&
						   a.Connection.Party?.ObjectId == party.ObjectId &&
						   a.IsDead == !areAlive)
				.ToList();
		}

		/// <summary>
		/// Returns all party members of the character that are on this map,
		/// regardless of distance or alive status.
		/// </summary>
		public List<Character> GetPartyMembers(Character character)
		{
			if (character.Connection.Party == null) return new List<Character>();

			var party = character.Connection.Party;
			return _characters.Values
				.Where(a => a.Connection.Party?.ObjectId == party.ObjectId)
				.ToList();
		}

		/// <summary>
		/// Returns all entities that the attacker can attack, optionally
		/// within the given radius and limited to a maximum count.
		/// </summary>
		public List<ICombatEntity> GetAttackableEntities(ICombatEntity attacker, float radius = 0, int maxResult = 0)
		{
			IEnumerable<ICombatEntity> candidates;

			if (_spatialIndex != null && radius > 0)
			{
				candidates = _spatialIndex.QueryCircle(attacker.Position, radius + MaxAgentRadius);
			}
			else
			{
				candidates = _combatEntities.Values;
			}

			var query = candidates
				.Where(entity => (radius == 0 || entity.Position.InRange2D(attacker.Position, radius + entity.AgentRadius)) &&
							   attacker.CanAttack(entity))
				.OrderBy(a => a.Position.Get2DDistance(attacker.Position));

			return maxResult > 0 ? query.Take(maxResult).ToList() : query.ToList();
		}

		/// <summary>
		/// Returns the nearest attackable entity to the given position
		/// within the given radius, or null if none found.
		/// </summary>
		public ICombatEntity GetNearestAttackableEntity(ICombatEntity attacker, Position position, float radius)
		{
			IEnumerable<ICombatEntity> candidates;

			if (_spatialIndex != null)
			{
				candidates = _spatialIndex.QueryCircle(position, radius + MaxAgentRadius);
			}
			else
			{
				candidates = _combatEntities.Values;
			}

			return candidates
				.Where(entity => entity.Position.InRange2D(position, radius + entity.AgentRadius) && attacker.CanAttack(entity))
				.OrderBy(a => a.Position.Get2DDistance(position))
				.FirstOrDefault();
		}

		/// <summary>
		/// Returns all attackable entities within range of a specific
		/// entity, excluding that entity itself.
		/// </summary>
		public List<ICombatEntity> GetAttackableEntitiesInRangeAroundEntity(ICombatEntity attacker, ICombatEntity entity, float radius, int maxResult = 0)
		{
			IEnumerable<ICombatEntity> candidates;

			if (_spatialIndex != null)
			{
				candidates = _spatialIndex.QueryCircle(entity.Position, radius + MaxAgentRadius);
			}
			else
			{
				candidates = _combatEntities.Values;
			}

			var query = candidates
				.Where(a => entity.Handle != a.Handle &&
						   a.Position.InRange2D(entity.Position, radius + a.AgentRadius) &&
						   attacker.CanAttack(a))
				.OrderBy(a => a.Position.Get2DDistance(entity.Position));

			return maxResult > 0 ? query.Take(maxResult).ToList() : query.ToList();
		}
		#endregion

		#region Line of Sight
		/// <summary>
		/// Returns true if there is a clear walkable path between two
		/// positions for an entity with the given radius and step height.
		/// </summary>
		public bool IsLineOfSightWalkable(Position start, Position end, float actorRadius, float? maxStepHeight = null)
		{
			maxStepHeight ??= actorRadius;

			var direction = start.GetDirection(end);
			var distance = start.Get2DDistance(end);
			var stepSize = Math.Max(2.5f, actorRadius * 0.5f);

			var currentPos = start;
			if (!this.Ground.TryGetHeightAt(currentPos, out var lastHeight))
				return false; // Start position is not on the ground

			for (float traveled = stepSize; traveled < distance; traveled += stepSize)
			{
				currentPos = start.GetRelative(direction, traveled);

				if (!this.IsWalkablePosition(currentPos, actorRadius))
					return false; // Path is blocked

				if (!this.Ground.TryGetHeightAt(currentPos, out var currentHeight))
					return false; // Stepped into a hole

				if (Math.Abs(currentHeight - lastHeight) > maxStepHeight)
					return false; // Slope too steep

				lastHeight = currentHeight;
			}

			// Final check at destination
			if (!this.IsWalkablePosition(end, actorRadius))
				return false;

			if (!this.Ground.TryGetHeightAt(end, out var endHeight))
				return false;

			return Math.Abs(endHeight - lastHeight) <= maxStepHeight;
		}
		#endregion

		#region Cleanup
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_updateEntitiesPool?.Dispose();
				_updateVisibleCharactersPool?.Dispose();
				_spatialIndex?.Dispose();
			}
		}

		/// <summary>
		/// Disposes the map, releasing spatial index and pooled resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		#region Spatial Index
		/// <summary>
		/// Updates an entity's position in the spatial index.
		/// Called when an entity moves significantly.
		/// </summary>
		/// <param name="entity">The entity that moved.</param>
		/// <param name="oldPosition">The entity's previous position.</param>
		/// <param name="newPosition">The entity's new position.</param>
		internal void UpdateEntitySpatialPosition(ICombatEntity entity, Position oldPosition, Position newPosition)
		{
			_spatialIndex?.Update(entity, oldPosition, newPosition);
		}

		/// <summary>
		/// Returns whether the map has a spatial index for efficient range queries.
		/// </summary>
		public bool HasSpatialIndex => _spatialIndex != null;
		#endregion

		#region Layer
		/// <summary>
		/// Suspends the AI of all monsters on the given layer.
		/// </summary>
		internal void FreezeLayer(int layer)
		{
			var layerMonsters = this.GetMonsters(mob => mob.Layer == layer);
			foreach (var monster in layerMonsters)
			{
				if (monster.Components.TryGet<AiScript>(out var ai))
					ai.Suspended = true;
			}
		}

		/// <summary>
		/// Resumes the AI of all monsters on the given layer.
		/// </summary>
		internal void UnfreezeLayer(int layer)
		{
			var layerMonsters = this.GetMonsters(mob => mob.Layer == layer);
			foreach (var monster in layerMonsters)
			{
				if (monster.Components.TryGet<AiScript>(out var ai))
					ai.Suspended = false;
			}
		}
		#endregion
	}
}
