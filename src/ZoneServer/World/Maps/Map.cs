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
using Melia.Zone.World.Spawning;
using Yggdrasil.Geometry;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Logging;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World.Maps
{
	public class Map : IUpdateable
	{
		private volatile int _layer = DefaultLayer;

		private static volatile int _dormancyBatchCount;
		private static int _dormancyBatchMobs;
		private static readonly object _dormancyLogLock = new();

		#region Constants
		public const int DefaultLayer = 0;
		public const int VisibleRange = 500;
		private const int MaxMonsterAddsPerTick = 5;
		private static readonly TimeSpan EntityUpdateGracePeriod = TimeSpan.FromMinutes(5);
		#endregion

		#region Collections
		protected readonly Dictionary<int, ICombatEntity> _combatEntities = new();
		protected readonly Dictionary<int, Character> _characters = new();
		protected readonly Dictionary<int, IMonster> _monsters = new();
		protected readonly Dictionary<int, ITriggerableArea> _triggerableAreas = new();
		protected readonly Dictionary<int, Pad> _pads = new();

		protected readonly List<DynamicObstacle> _obstacles = new();
		protected readonly object _obstaclesLock = new();

		private readonly ConcurrentQueue<IMonster> _addMonsters = new();
		private int _characterCount;
		private DateTime _lastPlayerLeftTime = DateTime.MinValue;
		private readonly DateTime _createdTime = DateTime.Now;
		protected readonly Dictionary<int, PropertyOverrides> _monsterPropertyOverrides = new();
		protected readonly List<SpawnBuffEntry> _spawnBuffs = new();

		private readonly List<IUpdateable> _updateEntities = new();
		private readonly List<Character> _updateVisibleCharacters = new();

		// Spatial index for efficient range queries
		private EntitySpatialIndex _spatialIndex;

		// Buffer to avoid allocating new lists
		[ThreadStatic]
		private static List<ICombatEntity> _spatialShapeQueryBuffer;

		[ThreadStatic]
		private static List<ICombatEntity> _broadcastAllBuffer;

		[ThreadStatic]
		private static List<ICombatEntity> _broadcastQueryBuffer;

		[ThreadStatic]
		private static HashSet<IZoneConnection> _broadcastSentConnections;

		#endregion

		#region Properties
		public int WorldId { get; protected set; }
		public string ClassName { get; protected set; }
		public int Id { get; protected set; }
		public MapData Data { get; protected set; }
		public Ground Ground { get; } = new Ground();
		public IPathfinder Pathfinder { get; private set; }

		public int CharacterCount => _characterCount;
		public int MonsterCount { get { lock (_monsters) return _monsters.Count; } }
		public bool HasCharacters => _characterCount > 0;

		// Collision
		private const int CollisionCheckPointCount = 9;

		[ThreadStatic]
		private static Vector2F[] CollisionCheckBuffer;

		[ThreadStatic]
		private static List<ICombatEntity> CollideActorsQueryBuffer;


		public bool IsPVP { get; set; }
		public bool IsRaid { get; set; }
		public bool IsGTW { get; protected set; }
		public bool IsCity { get; set; }
		public bool IsTOSHeroZone => this.Data?.Tags.Has(SkillTag.ExpertSkill) ?? false;
		public bool IsInstance => this.Data?.Type == MapType.Instance;
		public bool TeleportDisabled { get; internal set; }
		public bool IsDormant { get; private set; }

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
				this.Pathfinder = new NavMeshPathfinder(this);
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
			if (this.IsDormant)
			{
				FlushDormancyLog();
				return;
			}

			if (!this.HasCharacters && !this.IsCity && !this.IsInstance)
			{
				// Player left recently — enter dormancy after grace period
				if (_lastPlayerLeftTime != DateTime.MinValue && (DateTime.Now - _lastPlayerLeftTime) >= EntityUpdateGracePeriod)
				{
					this.EnterDormancy();
					return;
				}

				// Map never had a player — enter dormancy after startup grace period
				if (_lastPlayerLeftTime == DateTime.MinValue && (DateTime.Now - _createdTime) >= EntityUpdateGracePeriod)
				{
					this.EnterDormancy();
					return;
				}
			}

			this.Disappearances();
			this.UpdateVisibility();
			this.UpdateEntities(elapsed);
		}

		private void UpdateEntities(TimeSpan elapsed)
		{
			// Process pending monster additions (throttled to prevent
			// packet storms when many monsters spawn simultaneously).
			// Item drops bypass the throttle so they appear instantly
			// when a mob dies, rather than trickling in over multiple
			// ticks.
			List<ItemMonster> newItemMonsters = null;
			var monstersAdded = 0;
			while (_addMonsters.TryPeek(out var next))
			{
				// Throttle non-item monster additions to prevent packet
				// storms, but always process item drops immediately so
				// they all appear at once when a mob dies.
				if (next is not ItemMonster && monstersAdded >= MaxMonsterAddsPerTick)
					break;

				if (!_addMonsters.TryDequeue(out var monster))
					break;

				this.AddMonsterInternal(monster);

				if (monster is ItemMonster itemMonster)
					(newItemMonsters ??= new()).Add(itemMonster);
				else
					monstersAdded++;
			}

			// Batch-merge items after all additions for this tick,
			// so drops that exceed the threshold are merged all at
			// once rather than progressively across multiple ticks.
			if (newItemMonsters != null)
			{
				foreach (var itemMonster in newItemMonsters)
				{
					bool exists;
					lock (_monsters)
						exists = _monsters.ContainsKey(itemMonster.Handle);
					if (exists)
						this.TryMergeNearbyItems(itemMonster);
				}
			}

			lock (_updateEntities)
			{
				// Collect updateables - update monsters if players are on
				// the map or if players left recently (grace period for
				// mobs to return to spawn, despawn via lifetime, etc.)
				var withinGracePeriod = _lastPlayerLeftTime != DateTime.MinValue && (DateTime.Now - _lastPlayerLeftTime) < EntityUpdateGracePeriod;
				if (this.HasCharacters || withinGracePeriod)
				{
					lock (_monsters)
					{
						foreach (var monster in _monsters.Values)
						{
							if (monster is IUpdateable updatable)
								_updateEntities.Add(updatable);
						}
					}
				}

				// Update pads before characters so enter/leave detection
				// happens first
				lock (_pads)
				{
					foreach (var updatable in _pads.Values)
						_updateEntities.Add(updatable);
				}

				lock (_characters)
				{
					foreach (var updatable in _characters.Values)
						_updateEntities.Add(updatable);
				}

				foreach (var entity in _updateEntities)
					entity.Update(elapsed);

				_updateEntities.Clear();
			}
		}

		private void Disappearances()
		{
			var now = DateTime.Now;

			// Process monster disappearances (like base Melia)
			var toDisappear = new List<IMonster>();
			lock (_monsters)
			{
				foreach (var monster in _monsters.Values)
				{
					if (monster.DisappearTime < now)
						toDisappear.Add(monster);
				}
			}

			foreach (var monster in toDisappear)
			{
				monster.OnDisappear?.Invoke();
				ZoneServer.Instance.ServerEvents.MonsterDisappears.Raise(new MonsterEventArgs(monster));
				this.RemoveMonster(monster);
			}

			// Process character disappearances
			var toDisappearChars = new List<Character>();
			lock (_characters)
			{
				foreach (var character in _characters.Values)
				{
					if (character.DisappearTime < now)
						toDisappearChars.Add(character);
				}
			}

			foreach (var character in toDisappearChars)
			{
				character.OnDisappear?.Invoke();
				this.RemoveCharacter(character);
			}

			// Process pad disappearances
			var toDisappearPads = new List<Pad>();
			lock (_pads)
			{
				foreach (var pad in _pads.Values)
				{
					if (pad.DisappearTime < now)
						toDisappearPads.Add(pad);
				}
			}

			foreach (var pad in toDisappearPads)
			{
				pad.OnDisappear?.Invoke();
				this.RemovePad(pad);
			}
		}

		private void UpdateVisibility()
		{
			lock (_updateVisibleCharacters)
			{
				lock (_characters)
				{
					foreach (var character in _characters.Values)
						_updateVisibleCharacters.Add(character);
				}

				foreach (var character in _updateVisibleCharacters)
					character.LookAround();

				_updateVisibleCharacters.Clear();
			}
		}

		#endregion

		#region Dormancy

		/// <summary>
		/// Removes all spawner-managed mobs and pads from the map,
		/// notifies their spawners, and marks the map as dormant.
		/// NPCs, cities, and instance maps are not affected.
		/// </summary>
		private void EnterDormancy()
		{
			if (this.IsDormant)
				return;

			// Collect spawner-managed mobs (not NPCs or warps)
			var mobsToRemove = new List<Mob>();
			lock (_monsters)
			{
				foreach (var monster in _monsters.Values)
				{
					if (monster is Mob mob && monster is not Npc)
						mobsToRemove.Add(mob);
				}
			}

			// Group by spawner so we can notify each one
			var spawnerCounts = new Dictionary<ISpawner, int>();
			foreach (var mob in mobsToRemove)
			{
				if (mob.Spawner is ISpawner spawner)
				{
					spawnerCounts.TryGetValue(spawner, out var count);
					spawnerCounts[spawner] = count + 1;
				}
			}

			// Remove the mobs
			foreach (var mob in mobsToRemove)
				this.RemoveMonster(mob);

			// Notify spawners of the removal counts
			foreach (var kvp in spawnerCounts)
				kvp.Key.NotifyDormancy(kvp.Value);

			// Remove all pads
			var padsToRemove = new List<Pad>();
			lock (_pads)
				padsToRemove.AddRange(_pads.Values);

			foreach (var pad in padsToRemove)
				this.RemovePad(pad);

			// Drain pending monster queue
			while (_addMonsters.TryDequeue(out _)) { }

			this.IsDormant = true;

			lock (_dormancyLogLock)
			{
				_dormancyBatchCount++;
				_dormancyBatchMobs += mobsToRemove.Count;
			}
		}

		/// <summary>
		/// Wakes the map from dormancy, allowing spawners to repopulate.
		/// </summary>
		private void WakeUp()
		{
			this.IsDormant = false;
			_lastPlayerLeftTime = DateTime.MinValue;

			Log.Info("Map '{0}' waking up.", this.ClassName);
		}

		/// <summary>
		/// Logs a summary of maps that entered dormancy since the last
		/// flush. Called from dormant map updates so the log appears
		/// shortly after the batch completes.
		/// </summary>
		private static void FlushDormancyLog()
		{
			if (_dormancyBatchCount == 0)
				return;

			lock (_dormancyLogLock)
			{
				if (_dormancyBatchCount == 0)
					return;

				Log.Info("{0} map(s) entered dormancy ({1} mobs removed).", _dormancyBatchCount, _dormancyBatchMobs);
				_dormancyBatchCount = 0;
				_dormancyBatchMobs = 0;
			}
		}

		#endregion

		#region Character Management
		/// <summary>
		/// Adds the character to the map and raises the player entered event.
		/// </summary>
		public void AddCharacter(Character character)
		{
			if (this.IsDormant)
				this.WakeUp();

			character.Map = this;

			lock (_characters)
				_characters[character.Handle] = character;

			Interlocked.Increment(ref _characterCount);

			if (character is ICombatEntity combatEntity)
			{
				lock (_combatEntities)
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
			lock (_characters)
				_characters.Remove(character.Handle);

			Interlocked.Decrement(ref _characterCount);

			lock (_combatEntities)
				_combatEntities.Remove(character.Handle);

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
		public Character GetCharacter(Func<Character, bool> predicate)
		{
			lock (_characters)
			{
				foreach (var character in _characters.Values)
				{
					if (predicate(character))
						return character;
				}
			}
			return null;
		}

		/// <summary>
		/// Returns the character with the given handle via out.
		/// Returns false if not found.
		/// </summary>
		public bool TryGetCharacter(int handle, out Character character)
		{
			lock (_characters)
				return _characters.TryGetValue(handle, out character);
		}

		/// <summary>
		/// Returns all non-dummy characters on the map.
		/// </summary>
		public Character[] GetCharacters() => this.GetCharacters(c => c.Connection is not DummyConnection);

		/// <summary>
		/// Returns all characters matching the predicate.
		/// </summary>
		public Character[] GetCharacters(Func<Character, bool> predicate)
		{
			lock (_characters)
				return _characters.Values.Where(predicate).ToArray();
		}

		/// <summary>
		/// Returns all characters visible to the given character.
		/// </summary>
		public Character[] GetVisibleCharacters(Character character) => this.GetCharacters(character.CanSee);

		/// <summary>
		/// Adds all characters visible to the given character to the result list.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="result"></param>
		public void GetVisibleCharacters(Character character, List<Character> result)
		{
			lock (_characters)
			{
				foreach (var otherCharacter in _characters.Values)
				{
					if (otherCharacter != character && character.CanSee(otherCharacter))
						result.Add(otherCharacter);
				}
			}
		}

		/// <summary>
		/// Adds all characters visible to the given character to the result set.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="result"></param>
		public void GetVisibleCharacters(Character character, HashSet<Character> result)
		{
			lock (_characters)
			{
				foreach (var otherCharacter in _characters.Values)
				{
					if (otherCharacter != character && character.CanSee(otherCharacter))
						result.Add(otherCharacter);
				}
			}
		}
		#endregion

		#region Monster Management
		/// <summary>
		/// Queues a monster to be added to the map on the next update tick.
		/// </summary>
		public void AddMonster(IMonster monster)
		{
			// Only block spawner-managed mobs on dormant maps. NPCs,
			// treasure chests, minigame entities, and other non-spawner
			// mobs are allowed through so they can be queued for when
			// the map wakes up.
			if (this.IsDormant && monster is Mob mob && mob.Spawner != null)
				return;

			_addMonsters.Enqueue(monster);
		}

		private void AddMonsterInternal(IMonster monster)
		{
			monster.Map = this;

			lock (_monsters)
				_monsters[monster.Handle] = monster;

			if (monster is ICombatEntity entity)
			{
				lock (_combatEntities)
					_combatEntities[monster.Handle] = entity;

				_spatialIndex?.Insert(entity);
			}

			if (monster is ITriggerableArea trigger)
			{
				lock (_triggerableAreas)
					_triggerableAreas[monster.Handle] = trigger;
			}

			monster.Components.Get<TriggerComponent>()?.OnAddedToMap();
			monster.FromGround = false;
		}

		/// <summary>
		/// Attempts to merge a newly dropped item with nearby items of the
		/// same type on the ground. If 5+ of the same stackable item exist
		/// within 30 units on the same layer/owner, they are consolidated
		/// into a single stack at the average position, capped at MaxStack.
		/// </summary>
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

			lock (_monsters)
			{
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

			lock (_monsters)
				_monsters.Remove(monster.Handle);

			lock (_combatEntities)
				_combatEntities.Remove(monster.Handle);

			lock (_triggerableAreas)
				_triggerableAreas.Remove(monster.Handle);

			if (monster is ICombatEntity entity)
				_spatialIndex?.Remove(entity);

			if (monster.Components.TryGet<AiComponent>(out var ai))
			{
				ai.Script?.ClearEventAlerts();
				ai.Script?.ClearTarget();
			}

			monster.Map = null;
			this.UpdateVisibility();

			if (monster is Mob mob)
				mob.Cleanup();
		}

		/// <summary>
		/// Removes all monsters and pads on the given layer from the map.
		/// </summary>
		public void RemoveEntitiesOnLayer(int layer)
		{
			var toRemove = new List<IMonster>();
			lock (_monsters)
			{
				foreach (var monster in _monsters.Values)
				{
					if (monster.Layer == layer)
						toRemove.Add(monster);
				}
			}
			foreach (var monster in toRemove)
				this.RemoveMonster(monster);

			var toRemovePads = new List<Pad>();
			lock (_pads)
			{
				foreach (var pad in _pads.Values)
				{
					if (pad.Layer == layer)
						toRemovePads.Add(pad);
				}
			}
			foreach (var pad in toRemovePads)
				this.RemovePad(pad);
		}

		/// <summary>
		/// Adds all triggerable areas that overlap with the given
		/// position to the result list.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="result"></param>
		public void GetTriggerableAreasAt(Position pos, List<ITriggerableArea> result)
		{
			lock (_triggerableAreas)
			{
				foreach (var area in _triggerableAreas.Values)
				{
					if (area.Area?.IsInside(pos) ?? false)
						result.Add(area);
				}
			}
		}

		/// <summary>
		/// Adds all triggerable areas that overlap with the given
		/// position to the result set.
		/// </summary>
		public void GetTriggerableAreasAt(Position pos, HashSet<ITriggerableArea> result)
		{
			lock (_triggerableAreas)
			{
				foreach (var area in _triggerableAreas.Values)
				{
					if (area.Area?.IsInside(pos) ?? false)
						result.Add(area);
				}
			}
		}

		/// <summary>
		/// Returns the monster with the given handle, or null if not found.
		/// </summary>
		public IMonster GetMonster(int handle)
		{
			lock (_monsters)
				return _monsters.TryGetValue(handle, out var monster) ? monster : null;
		}

		/// <summary>
		/// Returns all attackable combat entities within the given range.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="position"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public List<ICombatEntity> GetAttackableEntitiesInRange(ICombatEntity attacker, Position position, float radius)
		{
			var result = new List<ICombatEntity>();

			lock (_combatEntities)
			{
				foreach (var entity in _combatEntities.Values)
				{
					if (!entity.Position.InRange2D(position, radius))
						continue;

					if (!attacker.CanDamage(entity))
						continue;

					result.Add(entity);
				}
			}

			return result;
		}

		/// <summary>
		/// Returns the first monster matching the predicate, or null.
		/// </summary>
		public IMonster GetMonster(Func<IMonster, bool> predicate)
		{
			lock (_monsters)
			{
				foreach (var monster in _monsters.Values)
				{
					if (predicate(monster))
						return monster;
				}
			}

			return null;
		}

		/// <summary>
		/// Returns all attackable combat entities within the given shape.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="shape"></param>
		/// <returns></returns>
		public List<ICombatEntity> GetAttackableEntitiesIn(ICombatEntity attacker, IShapeF shape)
		{
			var result = new List<ICombatEntity>();

			lock (_combatEntities)
			{
				foreach (var entity in _combatEntities.Values)
				{
					if (!attacker.CanDamage(entity))
						continue;

					if (!shape.IsInside(entity.Position))
						continue;

					result.Add(entity);
				}
			}

			return result;
		}

		/// <summary>
		/// Returns the monster with the given handle via out.
		/// Returns false if not found.
		/// </summary>
		public bool TryGetMonster(int handle, out IMonster monster)
		{
			lock (_monsters)
				return _monsters.TryGetValue(handle, out monster);
		}

		/// <summary>
		/// Returns the first monster matching the predicate via out.
		/// Returns false if not found.
		/// </summary>
		public bool TryGetMonster(Func<IMonster, bool> predicate, out IMonster monster)
		{
			lock (_monsters)
			{
				foreach (var m in _monsters.Values)
				{
					if (predicate(m))
					{
						monster = m;
						return true;
					}
				}
			}

			monster = null;
			return false;
		}

		/// <summary>
		/// Returns all monsters currently on the map.
		/// </summary>
		public IMonster[] GetMonsters()
		{
			lock (_monsters)
				return _monsters.Values.ToArray();
		}

		/// <summary>
		/// Returns all actors with the given type in the area.
		/// </summary>
		public List<TActor> GetActorsIn<TActor>(IShapeF area) where TActor : IActor
		{
			var result = new List<TActor>();
			this.GetActorsIn<TActor>(area, result);
			return result;
		}

		/// <summary>
		/// Fills the result list with all actors of the given type in the area.
		/// </summary>
		public void GetActorsIn<TActor>(IShapeF area, List<TActor> result) where TActor : IActor
		{
			lock (_monsters)
			{
				foreach (var monster in _monsters.Values)
				{
					if (monster is TActor actor && area.IsInside(actor.Position))
						result.Add(actor);
				}
			}

			lock (_characters)
			{
				foreach (var character in _characters.Values)
				{
					if (character is TActor actor && area.IsInside(actor.Position))
						result.Add(actor);
				}
			}
		}

		/// <summary>
		/// Returns all monsters matching the predicate.
		/// </summary>
		public List<IMonster> GetMonsters(Func<IMonster, bool> predicate)
		{
			var result = new List<IMonster>();

			lock (_monsters)
			{
				foreach (var monster in _monsters.Values)
				{
					if (predicate(monster))
						result.Add(monster);
				}
			}

			return result;
		}

		/// <summary>
		/// Returns all monsters visible to the given character.
		/// </summary>
		public List<IMonster> GetVisibleMonsters(Character character) => this.GetMonsters(character.CanSee);

		/// <summary>
		/// Adds all monsters visible to the given character to the result list.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="result"></param>
		public void GetVisibleMonsters(Character character, List<IMonster> result)
		{
			lock (_monsters)
			{
				foreach (var monster in _monsters.Values)
				{
					if (character.CanSee(monster))
						result.Add(monster);
				}
			}
		}

		/// <summary>
		/// Adds all monsters visible to the given character to the result set.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="result"></param>
		public void GetVisibleMonsters(Character character, HashSet<IMonster> result)
		{
			lock (_monsters)
			{
				foreach (var monster in _monsters.Values)
				{
					if (character.CanSee(monster))
						result.Add(monster);
				}
			}
		}

		/// <summary>
		/// Returns all pads visible to the given character.
		/// </summary>
		public Pad[] GetVisiblePads(Character character) => this.GetPads(character.CanSee);

		/// <summary>
		/// Adds all pads visible to the given character to the result list.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="result"></param>
		public void GetVisiblePads(Character character, List<Pad> result)
		{
			lock (_pads)
			{
				foreach (var pad in _pads.Values)
				{
					if (character.CanSee(pad))
						result.Add(pad);
				}
			}
		}

		/// <summary>
		/// Adds all pads visible to the given character to the result set.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="result"></param>
		public void GetVisiblePads(Character character, HashSet<Pad> result)
		{
			lock (_pads)
			{
				foreach (var pad in _pads.Values)
				{
					if (character.CanSee(pad))
						result.Add(pad);
				}
			}
		}

		/// <summary>
		/// Removes all scripted entities (monsters) from the map.
		/// </summary>
		public void RemoveScriptedEntities()
		{
			var toRemove = new List<IMonster>();
			lock (_monsters)
				toRemove.AddRange(_monsters.Values);

			foreach (var monster in toRemove)
				this.RemoveMonster(monster);

			lock (_spawnBuffs)
				_spawnBuffs.Clear();
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

			lock (_pads)
				_pads[pad.Handle] = pad;

			this.UpdateVisibility();
			pad.Components.Get<TriggerComponent>()?.OnAddedToMap();
		}

		/// <summary>
		/// Removes a pad from the map and deactivates its trigger.
		/// </summary>
		public void RemovePad(Pad pad)
		{
			pad.Components.Get<TriggerComponent>()?.OnRemovingFromMap();

			lock (_pads)
				_pads.Remove(pad.Handle);

			pad.Map = null;
			this.UpdateVisibility();
		}

		/// <summary>
		/// Returns the pad with the given handle via out.
		/// Returns false if not found.
		/// </summary>
		public bool TryGetPad(int handle, out Pad pad)
		{
			lock (_pads)
				return _pads.TryGetValue(handle, out pad);
		}

		/// <summary>
		/// Returns all pads within range of or overlapping the given position.
		/// </summary>
		public Pad[] GetPadsAt(Position pos, float range)
		{
			var result = new List<Pad>();
			lock (_pads)
			{
				foreach (var pad in _pads.Values)
				{
					if (pad.Position.InRange2D(pos, range) || (pad.Area?.IsInside(pos) ?? false))
						result.Add(pad);
				}
			}
			return result.ToArray();
		}

		/// <summary>
		/// Returns all pads matching the given predicate.
		/// </summary>
		public Pad[] GetPads(Func<Pad, bool> func)
		{
			var result = new List<Pad>();
			lock (_pads)
			{
				foreach (var pad in _pads.Values)
				{
					if (func(pad))
						result.Add(pad);
				}
			}
			return result.ToArray();
		}

		/// <summary>
		/// Returns all pads that can hit the given entity based on faction
		/// rules and area overlap.
		/// </summary>
		public Pad[] GetHittablePadsAt(ICombatEntity entity)
		{
			if (!entity.IsHitByPad())
				return Array.Empty<Pad>();

			var result = new List<Pad>();
			lock (_pads)
			{
				foreach (var pad in _pads.Values)
				{
					if (pad.Area?.IsInside(entity.Position) ?? false)
						result.Add(pad);
				}
			}
			return result.ToArray();
		}
		#endregion

		#region Combat Entity Management
		/// <summary>
		/// Returns the combat entity with the given handle, or null if
		/// not found.
		/// </summary>
		public ICombatEntity GetCombatEntity(int handle)
		{
			lock (_monsters)
			{
				if (_monsters.TryGetValue(handle, out var monster) && monster is ICombatEntity entity)
					return entity;
			}

			lock (_characters)
			{
				if (_characters.TryGetValue(handle, out var entity))
					return entity;
			}

			return null;
		}

		/// <summary>
		/// Returns the combat entity with the given handle via out.
		/// Returns false if not found.
		/// </summary>
		public bool TryGetCombatEntity(int handle, out ICombatEntity entity)
		{
			entity = this.GetCombatEntity(handle);
			return entity != null;
		}

		/// <summary>
		/// Returns any actor (monster or character) with the given handle
		/// via out. Returns false if not found.
		/// </summary>
		public bool TryGetActor(int handle, out IActor actor)
		{
			lock (_monsters)
			{
				if (_monsters.TryGetValue(handle, out var monster))
				{
					actor = monster;
					return true;
				}
			}

			lock (_characters)
			{
				if (_characters.TryGetValue(handle, out var character))
				{
					actor = character;
					return true;
				}
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
		public ITriggerableArea[] GetTriggerableAreasAt(Position pos)
		{
			var result = new List<ITriggerableArea>();
			lock (_triggerableAreas)
			{
				foreach (var area in _triggerableAreas.Values)
				{
					if (area.Area?.IsInside(pos) ?? false)
						result.Add(area);
				}
			}
			return result.ToArray();
		}

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
			var result = this.GetActorsIn<ItemMonster>(shape);
			result.Sort((a, b) => position.Get2DDistance(a.Position).CompareTo(position.Get2DDistance(b.Position)));
			return result;
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
				var queryBuffer = _spatialQueryBuffer ??= new List<ICombatEntity>();
				queryBuffer.Clear();
				_spatialIndex.QueryCircle(position, radius + MaxAgentRadius, queryBuffer);
				candidates = queryBuffer;
			}
			else
			{
				lock (_combatEntities)
					candidates = _combatEntities.Values.ToList();
			}

			var result = new List<ICombatEntity>();
			foreach (var e in candidates)
			{
				var effectiveRadius = radius + e.AgentRadius;
				var dx = e.Position.X - position.X;
				var dz = e.Position.Z - position.Z;
				if (dx * dx + dz * dz <= effectiveRadius * effectiveRadius && attacker.CanDamage(e))
					result.Add(e);
			}

			result.Sort((a, b) => position.Get2DDistance(a.Position).CompareTo(position.Get2DDistance(b.Position)));
			return result;
		}

		/// <summary>
		/// Fills the provided buffer with all enemies that can be attacked
		/// in a circular area around the target position. Does not sort.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="position"></param>
		/// <param name="radius"></param>
		/// <param name="buffer">Buffer to fill with results. Cleared before use.</param>
		[ThreadStatic]
		private static List<ICombatEntity> _spatialQueryBuffer;

		public void GetAttackableEnemiesInPosition(ICombatEntity attacker, Position position, float radius, List<ICombatEntity> buffer)
		{
			buffer.Clear();

			IEnumerable<ICombatEntity> candidates;

			if (_spatialIndex != null)
			{
				var queryBuffer = _spatialQueryBuffer ??= new List<ICombatEntity>();
				queryBuffer.Clear();
				_spatialIndex.QueryCircle(position, radius + MaxAgentRadius, queryBuffer);
				candidates = queryBuffer;
			}
			else
			{
				lock (_combatEntities)
					candidates = _combatEntities.Values.ToList();
			}

			foreach (var e in candidates)
			{
				var effectiveRadius = radius + e.AgentRadius;
				var dx = e.Position.X - position.X;
				var dz = e.Position.Z - position.Z;

				if (dx * dx + dz * dz <= effectiveRadius * effectiveRadius && attacker.CanDamage(e))
					buffer.Add(e);
			}
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
		public List<ICombatEntity> GetAttackableEnemiesIn(ICombatEntity attacker, IShapeF shape, int maxTargets = 0)
		{
			if (attacker is Character rangePreviewCharacter && rangePreviewCharacter.Variables.Temp.GetBool("Melia.RangePreview"))
				Debug.ShowShape(this, shape, TimeSpan.FromSeconds(1));

			var queryBuffer = _spatialShapeQueryBuffer ??= new List<ICombatEntity>();
			queryBuffer.Clear();

			if (_spatialIndex != null)
			{
				_spatialIndex.QueryShape(shape, queryBuffer);
			}
			else
			{
				lock (_combatEntities)
				{
					foreach (var e in _combatEntities.Values)
						queryBuffer.Add(e);
				}
			}

			var result = new List<ICombatEntity>();
			foreach (var e in queryBuffer)
			{
				if (!attacker.CanDamage(e))
					continue;
				if (!shape.IsInsideOrInRange(e.Position, e.AgentRadius))
					continue;
				result.Add(e);
			}

			result.Sort((a, b) => attacker.GetDistance(a).CompareTo(attacker.GetDistance(b)));
			if (maxTargets > 0 && result.Count > maxTargets)
				result.RemoveRange(maxTargets, result.Count - maxTargets);
			return result;
		}

		/// <summary>
		/// Returns all enemies that can be attacked inside the given shape,
		/// excluding the specified entities.
		/// </summary>
		public List<ICombatEntity> GetAttackableEnemiesIn(ICombatEntity attacker, IShapeF shape, int maxTargets, ICombatEntity exclude1)
		{
			if (attacker is Character rangePreviewCharacter && rangePreviewCharacter.Variables.Temp.GetBool("Melia.RangePreview"))
				Debug.ShowShape(this, shape, TimeSpan.FromSeconds(1));

			var queryBuffer = _spatialShapeQueryBuffer ??= new List<ICombatEntity>();
			queryBuffer.Clear();

			if (_spatialIndex != null)
			{
				_spatialIndex.QueryShape(shape, queryBuffer);
			}
			else
			{
				lock (_combatEntities)
				{
					foreach (var e in _combatEntities.Values)
						queryBuffer.Add(e);
				}
			}

			var result = new List<ICombatEntity>();
			foreach (var e in queryBuffer)
			{
				if (e == exclude1)
					continue;
				if (!attacker.CanDamage(e))
					continue;
				if (!shape.IsInsideOrInRange(e.Position, e.AgentRadius))
					continue;
				result.Add(e);
			}

			result.Sort((a, b) => attacker.GetDistance(a).CompareTo(attacker.GetDistance(b)));
			if (maxTargets > 0 && result.Count > maxTargets)
				result.RemoveRange(maxTargets, result.Count - maxTargets);
			return result;
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
			var queryBuffer = _spatialShapeQueryBuffer ??= new List<ICombatEntity>();
			queryBuffer.Clear();

			if (_spatialIndex != null)
			{
				_spatialIndex.QueryShape(shape, queryBuffer);
			}
			else
			{
				lock (_combatEntities)
				{
					foreach (var e in _combatEntities.Values)
						queryBuffer.Add(e);
				}
			}

			var result = new List<ICombatEntity>();
			foreach (var entity in queryBuffer)
			{
				if (!entity.IsAlly(ally) || entity == ally || entity.IsDead)
					continue;
				if (!shape.IsInsideOrInRange(entity.Position, entity.AgentRadius))
					continue;
				result.Add(entity);
			}

			result.Sort((a, b) => ally.GetDistance(a).CompareTo(ally.GetDistance(b)));
			if (maxTargets > 0 && result.Count > maxTargets)
				result.RemoveRange(maxTargets, result.Count - maxTargets);
			return result;
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
			var queryBuffer = _spatialShapeQueryBuffer ??= new List<ICombatEntity>();
			queryBuffer.Clear();

			if (_spatialIndex != null)
			{
				_spatialIndex.QueryShape(shape, queryBuffer);
			}
			else
			{
				lock (_combatEntities)
				{
					foreach (var e in _combatEntities.Values)
						queryBuffer.Add(e);
				}
			}

			var result = new List<ICombatEntity>();
			foreach (var entity in queryBuffer)
			{
				if (!entity.IsDeadAlly(ally) || entity == ally)
					continue;
				if (!shape.IsInsideOrInRange(entity.Position, entity.AgentRadius))
					continue;
				result.Add(entity);
			}

			result.Sort((a, b) => ally.GetDistance(a).CompareTo(ally.GetDistance(b)));
			if (maxTargets > 0 && result.Count > maxTargets)
				result.RemoveRange(maxTargets, result.Count - maxTargets);
			return result;
		}

		/// <summary>
		/// Returns warp NPC that should be used when at given position.
		/// </summary>
		/// <param name="pos"></param>
		public bool TryGetNearbyWarp(Position pos, out WarpMonster result)
		{
			lock (_monsters)
			{
				foreach (var monster in _monsters.Values)
				{
					if (monster is WarpMonster warp && warp.Position.InRange2D(pos, 35))
					{
						result = warp;
						return true;
					}
				}
			}

			result = null;
			return false;
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
			this.GetActorsIn(area, predicate, result);
			return result;
		}

		/// <summary>
		/// Fills the provided buffer with all actors in a given area,
		/// avoiding list allocation.
		/// </summary>
		public void GetActorsIn<TActor>(IShapeF area, Func<TActor, bool> predicate, List<TActor> buffer) where TActor : IActor
		{
			buffer.Clear();

			if (_spatialIndex != null && typeof(ICombatEntity).IsAssignableFrom(typeof(TActor)))
			{
				var queryBuffer = _spatialShapeQueryBuffer ??= new List<ICombatEntity>();
				queryBuffer.Clear();
				_spatialIndex.QueryShape(area, queryBuffer);
				foreach (var entity in queryBuffer)
				{
					if (entity is TActor actor && area.IsInsideOrInRange(actor.Position, entity.AgentRadius) && (predicate?.Invoke(actor) ?? true))
						buffer.Add(actor);
				}
			}
			else
			{
				lock (_monsters)
				{
					foreach (var monster in _monsters.Values)
					{
						var radius = (monster as ICombatEntity)?.AgentRadius ?? 0;
						if (monster is TActor actor && area.IsInsideOrInRange(actor.Position, radius) && (predicate?.Invoke(actor) ?? true))
							buffer.Add(actor);
					}
				}

				lock (_characters)
				{
					foreach (var character in _characters.Values)
					{
						if (character is TActor actor && area.IsInsideOrInRange(actor.Position, ((ICombatEntity)character).AgentRadius) && (predicate?.Invoke(actor) ?? true))
							buffer.Add(actor);
					}
				}
			}

			lock (_pads)
			{
				foreach (var pad in _pads.Values)
				{
					if (pad is TActor actor && area.IsInside(actor.Position) && (predicate?.Invoke(actor) ?? true))
						buffer.Add(actor);
				}
			}
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

		/// <summary>
		/// Returns true if the given position is walkable for an entity
		/// with the given radius, accounting for ground validity and
		/// dynamic obstacles.
		/// </summary>
		public bool IsWalkablePosition(Position position, float radius) =>
			this.Ground.IsValidCirclePosition(position, radius) && !this.CollidesWithObstacles(position, radius);

		/// <summary>
		/// Returns true if a circle at the given position collides with
		/// any dynamic obstacles. Bool-only check, no list allocation.
		/// </summary>
		public bool CollidesWithObstacles(Position position, float radius)
		{
			var checkPositions = CollisionCheckBuffer ??= new Vector2F[CollisionCheckPointCount];
			this.FillCollisionCheckPositions(position, radius, checkPositions);

			lock (_obstaclesLock)
			{
				foreach (var obstacle in _obstacles)
				{
					for (var i = 0; i < CollisionCheckPointCount; i++)
					{
						if (obstacle.Shape.IsInside(checkPositions[i]))
							return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Returns true if the given entity's collision circle at a
		/// position overlaps with other entities. Bool-only check, no
		/// list allocation.
		/// </summary>
		public bool CollidesWithActors(ICombatEntity requester, Position position)
		{
			var searchRadius = requester.AgentRadius + 100f;

			if (_spatialIndex != null)
			{
				var queryBuffer = CollideActorsQueryBuffer ??= new List<ICombatEntity>();
				queryBuffer.Clear();
				_spatialIndex.QueryCircle(position, searchRadius, queryBuffer);

				foreach (var otherEntity in queryBuffer)
				{
					if (otherEntity.Handle == requester.Handle)
						continue;

					var dx = position.X - otherEntity.Position.X;
					var dz = position.Z - otherEntity.Position.Z;
					var minSep = requester.AgentRadius + otherEntity.AgentRadius;

					if (dx * dx + dz * dz < minSep * minSep)
						return true;
				}
			}
			else
			{
				lock (_combatEntities)
				{
					foreach (var otherEntity in _combatEntities.Values)
					{
						if (otherEntity.Handle == requester.Handle)
							continue;

						var dx = position.X - otherEntity.Position.X;
						var dz = position.Z - otherEntity.Position.Z;
						var minSep = requester.AgentRadius + otherEntity.AgentRadius;

						if (dx * dx + dz * dz < minSep * minSep)
							return true;
					}
				}
			}

			return false;
		}

		private void FillCollisionCheckPositions(Position position, float radius, Vector2F[] buffer)
		{
			buffer[0] = new(position.X, position.Z);                    // Center
			buffer[1] = new(position.X + radius, position.Z);           // Right
			buffer[2] = new(position.X - radius, position.Z);           // Left
			buffer[3] = new(position.X, position.Z + radius);           // Top
			buffer[4] = new(position.X, position.Z - radius);           // Bottom
			buffer[5] = new(position.X + radius, position.Z + radius);  // Top-Right
			buffer[6] = new(position.X - radius, position.Z + radius);  // Top-Left
			buffer[7] = new(position.X + radius, position.Z - radius);  // Bottom-Right
			buffer[8] = new(position.X - radius, position.Z - radius);  // Bottom-Left
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
				var queryBuffer = _spatialQueryBuffer ??= new List<ICombatEntity>();
				queryBuffer.Clear();
				_spatialIndex.QueryCircle(position, searchRadius, queryBuffer);
				candidates = queryBuffer;
			}
			else
			{
				lock (_combatEntities)
					candidates = _combatEntities.Values.ToList();
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
		public void AddPropertyOverrides(int monsterClassId, PropertyOverrides propertyOverrides)
		{
			lock (_monsterPropertyOverrides)
				_monsterPropertyOverrides[monsterClassId] = propertyOverrides;
		}

		/// <summary>
		/// Returns property overrides for the given monster class via out.
		/// Returns false if no overrides are defined.
		/// </summary>
		public bool TryGetPropertyOverrides(int monsterClassId, out PropertyOverrides propertyOverrides)
		{
			lock (_monsterPropertyOverrides)
				return _monsterPropertyOverrides.TryGetValue(monsterClassId, out propertyOverrides);
		}
		#endregion

		#region Spawn Buffs
		/// <summary>
		/// Registers a buff to be applied to monsters when they spawn
		/// on this map.
		/// </summary>
		/// <param name="entry"></param>
		public void AddSpawnBuff(SpawnBuffEntry entry)
		{
			lock (_spawnBuffs)
				_spawnBuffs.Add(entry);
		}

		/// <summary>
		/// Returns a snapshot of the spawn buff entries for this map.
		/// </summary>
		/// <returns></returns>
		public SpawnBuffEntry[] GetSpawnBuffs()
		{
			lock (_spawnBuffs)
				return _spawnBuffs.ToArray();
		}
		#endregion

		#region Specialized Queries
		/// <summary>
		/// Returns the nearest warp within range of the position that
		/// warps when approached, or null if none found.
		/// </summary>
		public WarpMonster GetNearbyWarp(Position pos)
		{
			lock (_monsters)
			{
				foreach (var monster in _monsters.Values)
				{
					if (monster is WarpMonster warp && warp.WarpWhenNearby && warp.Position.InRange2D(pos, 35))
						return warp;
				}
			}
			return null;
		}

		/// <summary>
		/// Returns all warp monsters on the map.
		/// </summary>
		public List<WarpMonster> GetWarps()
		{
			var result = new List<WarpMonster>();
			lock (_monsters)
			{
				foreach (var monster in _monsters.Values)
				{
					if (monster is WarpMonster warp)
						result.Add(warp);
				}
			}
			return result;
		}

		/// <summary>
		/// Returns all warp monsters matching the predicate.
		/// </summary>
		public List<WarpMonster> GetWarps(Func<WarpMonster, bool> predicate)
		{
			var result = new List<WarpMonster>();
			lock (_monsters)
			{
				foreach (var monster in _monsters.Values)
				{
					if (monster is WarpMonster warp && (predicate == null || predicate(warp)))
						result.Add(warp);
				}
			}
			return result;
		}

		/// <summary>
		/// Returns all NPC monsters on the map.
		/// </summary>
		public List<MonsterInName> GetNpcs()
		{
			var result = new List<MonsterInName>();
			lock (_monsters)
			{
				foreach (var monster in _monsters.Values)
				{
					if (monster is MonsterInName npc)
						result.Add(npc);
				}
			}
			return result;
		}

		/// <summary>
		/// Returns all NPC monsters matching the predicate.
		/// </summary>
		public List<MonsterInName> GetNpcs(Func<MonsterInName, bool> predicate)
		{
			var result = new List<MonsterInName>();
			lock (_monsters)
			{
				foreach (var monster in _monsters.Values)
				{
					if (monster is MonsterInName npc && (predicate == null || predicate(npc)))
						result.Add(npc);
				}
			}
			return result;
		}

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

			var nearest = positions[0];
			var nearestDist = nearest.Get2DDistance(pos);
			for (var i = 1; i < positions.Count; i++)
			{
				var dist = positions[i].Get2DDistance(pos);
				if (dist < nearestDist)
				{
					nearestDist = dist;
					nearest = positions[i];
				}
			}
			return nearest;
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
				var queryBuffer = _spatialQueryBuffer ??= new List<ICombatEntity>();
				queryBuffer.Clear();
				_spatialIndex.QueryCircle(source.Position, VisibleRange, queryBuffer);
				candidates = queryBuffer;
			}
			else
			{
				lock (_combatEntities)
					candidates = _combatEntities.Values.ToList();
			}

			foreach (var combatEntity in candidates)
			{
				if (!combatEntity.Position.InRange2D(source.Position, VisibleRange))
					continue;

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
			lock (_combatEntities)
			{
				foreach (var combatEntity in _combatEntities.Values)
				{
					if (!combatEntity.Components.TryGet<AiComponent>(out var aiComponent))
						continue;

					aiComponent.Script.QueueEventAlert(alert);
				}
			}
		}

		/// <summary>
		/// Broadcasts a packet to all characters on the map.
		/// </summary>
		public virtual void Broadcast(Packet packet)
		{
			var buffer = _broadcastAllBuffer ??= new List<ICombatEntity>();
			buffer.Clear();

			lock (_characters)
			{
				foreach (var character in _characters.Values)
					buffer.Add(character);
			}

			foreach (var entity in buffer)
				((Character)entity).Connection.Send(packet);
		}

		/// <summary>
		/// Broadcasts a packet to all characters within visible range of
		/// the source actor, optionally excluding the source itself.
		/// </summary>
		public virtual void Broadcast(Packet packet, IActor source, bool includeSource = true)
		{
			var sentConnections = _broadcastSentConnections ??= new HashSet<IZoneConnection>();
			sentConnections.Clear();

			if (_spatialIndex != null)
			{
				var queryBuffer = _broadcastQueryBuffer ??= new List<ICombatEntity>();
				queryBuffer.Clear();
				_spatialIndex.QueryCircle(source.Position, VisibleRange, queryBuffer);

				foreach (var entity in queryBuffer)
				{
					if (entity is not Character character)
						continue;

					if (!includeSource && character == source)
						continue;

					if (character.Layer != source.Layer)
						continue;

					var conn = character.Connection;
					if (conn == null)
						continue;

					if (!sentConnections.Add(conn))
						continue;

					conn.Send(packet);
				}
				return;
			}

			var charBuffer = _broadcastQueryBuffer ??= new List<ICombatEntity>();
			charBuffer.Clear();

			lock (_characters)
			{
				foreach (var character in _characters.Values)
					charBuffer.Add(character);
			}

			foreach (var entity in charBuffer)
			{
				var character = (Character)entity;

				if (!character.Position.InRange2D(source.Position, VisibleRange))
					continue;

				if (!includeSource && character == source)
					continue;

				if (character.Layer != source.Layer)
					continue;

				var conn = character.Connection;
				if (conn == null)
					continue;

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
		public Character[] GetCharactersInside(IShapeF shape)
		{
			var result = new List<Character>();
			lock (_characters)
			{
				foreach (var character in _characters.Values)
				{
					if (shape.IsInsideOrInRange(character.Position, ((ICombatEntity)character).AgentRadius))
						result.Add(character);
				}
			}
			return result.ToArray();
		}

		/// <summary>
		/// Returns all characters whose position is outside the given shape.
		/// </summary>
		public Character[] GetCharactersOutside(IShapeF shape)
		{
			var result = new List<Character>();
			lock (_characters)
			{
				foreach (var character in _characters.Values)
				{
					if (!shape.IsInsideOrInRange(character.Position, ((ICombatEntity)character).AgentRadius))
						result.Add(character);
				}
			}
			return result.ToArray();
		}

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
			var result = new List<Character>();
			lock (_characters)
			{
				foreach (var a in _characters.Values)
				{
					if (radius > 0 && !a.Position.InRange2D(position, radius))
						continue;
					if (a.Connection.Party?.ObjectId != party.ObjectId)
						continue;
					if (a.IsDead != !areAlive)
						continue;
					result.Add(a);
				}
			}
			return result;
		}

		/// <summary>
		/// Returns all party members of the character that are on this map,
		/// regardless of distance or alive status.
		/// </summary>
		public List<Character> GetPartyMembers(Character character)
		{
			if (character.Connection.Party == null) return new List<Character>();

			var party = character.Connection.Party;
			var result = new List<Character>();
			lock (_characters)
			{
				foreach (var a in _characters.Values)
				{
					if (a.Connection.Party?.ObjectId == party.ObjectId)
						result.Add(a);
				}
			}
			return result;
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
				var queryBuffer = _spatialQueryBuffer ??= new List<ICombatEntity>();
				queryBuffer.Clear();
				_spatialIndex.QueryCircle(attacker.Position, radius + MaxAgentRadius, queryBuffer);
				candidates = queryBuffer;
			}
			else
			{
				lock (_combatEntities)
					candidates = _combatEntities.Values.ToList();
			}

			var result = new List<ICombatEntity>();
			foreach (var entity in candidates)
			{
				if (radius > 0 && !entity.Position.InRange2D(attacker.Position, radius + entity.AgentRadius))
					continue;
				if (!attacker.CanDamage(entity))
					continue;
				result.Add(entity);
			}

			result.Sort((a, b) => a.Position.Get2DDistance(attacker.Position).CompareTo(b.Position.Get2DDistance(attacker.Position)));
			if (maxResult > 0 && result.Count > maxResult)
				result.RemoveRange(maxResult, result.Count - maxResult);
			return result;
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
				var queryBuffer = _spatialQueryBuffer ??= new List<ICombatEntity>();
				queryBuffer.Clear();
				_spatialIndex.QueryCircle(position, radius + MaxAgentRadius, queryBuffer);
				candidates = queryBuffer;
			}
			else
			{
				lock (_combatEntities)
					candidates = _combatEntities.Values.ToList();
			}

			ICombatEntity nearest = null;
			var nearestDist = double.MaxValue;
			foreach (var entity in candidates)
			{
				if (!entity.Position.InRange2D(position, radius + entity.AgentRadius))
					continue;
				if (!attacker.CanDamage(entity))
					continue;
				var dist = entity.Position.Get2DDistance(position);
				if (dist < nearestDist)
				{
					nearestDist = dist;
					nearest = entity;
				}
			}
			return nearest;
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
				var queryBuffer = _spatialQueryBuffer ??= new List<ICombatEntity>();
				queryBuffer.Clear();
				_spatialIndex.QueryCircle(entity.Position, radius + MaxAgentRadius, queryBuffer);
				candidates = queryBuffer;
			}
			else
			{
				lock (_combatEntities)
					candidates = _combatEntities.Values.ToList();
			}

			var result = new List<ICombatEntity>();
			foreach (var a in candidates)
			{
				if (entity.Handle == a.Handle)
					continue;
				if (!a.Position.InRange2D(entity.Position, radius + a.AgentRadius))
					continue;
				if (!attacker.CanDamage(a))
					continue;
				result.Add(a);
			}

			result.Sort((a, b) => a.Position.Get2DDistance(entity.Position).CompareTo(b.Position.Get2DDistance(entity.Position)));
			if (maxResult > 0 && result.Count > maxResult)
				result.RemoveRange(maxResult, result.Count - maxResult);
			return result;
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
