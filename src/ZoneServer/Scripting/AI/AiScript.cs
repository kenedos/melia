using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Ai.Enumerable;
using Yggdrasil.Logging;
using Yggdrasil.Scheduling;
using Yggdrasil.Scripting;
using Yggdrasil.Util;

namespace Melia.Zone.Scripting.AI
{
	/// <summary>
	/// A script that sets up and controls a character or monster.
	/// </summary>
	public abstract partial class AiScript : EnumerableAi, IScript, IUpdateable
	{
		private bool _initiated;
		private bool _isDeadNotified;

		private int _masterHandle;
		private int _followTargetHandle;

		private DateTime _lastPlayerSeenTime;
		private readonly TimeSpan _inactivityDelay = TimeSpan.FromSeconds(2);

		protected DateTime _lastAttackedTime;
		protected int _lastAttackerHandle;

		private DateTime _lastSkillUseTime = DateTime.MinValue;
		private TimeSpan _lastSkillDuration = TimeSpan.Zero;

		private TendencyType _tendency;
		protected float _viewRange = 300;
		private float _hateRange = 150;
		private float _hateGainPerSecond = 20;
		private float _hateLossPerSecond = 5;
		private readonly float _hatePerHit = 150;
		// How much hate needed for monster to become aggressive
		private float _minAggroHateLevel = 100;
		// Threshold at which hate increases start having diminishing returns
		private float _overHateThreshold = 150;
		// Diminishing return rate
		private float _overHateRate = 1 / 20f;

		protected int _currentPhase = 0;
		protected float[] _phaseThresholds = [];

		protected float _helpCallRange = 200;
		protected float _helpCallChance = 0.01f; // 1% chance
		protected TimeSpan _helpCallCooldown = TimeSpan.FromSeconds(30);
		protected DateTime _lastHelpCallTime = DateTime.MinValue;

		protected int MaxChaseDistance = 400;
		protected int MaxMasterDistance = 200;
		protected int MaxRoamDistance = 1000;

		protected AttackerRangeType RangeType = AttackerRangeType.Melee;

		protected bool EnableReturnHome = true;

		private readonly HashSet<int> _hateLevelsToRemove = new();
		private readonly Dictionary<int, float> _hateLevels = new();
		private readonly HashSet<FactionType> _hatedFactions = new();
		private readonly HashSet<int> _hatedMonsters = new();

		private readonly Dictionary<string, List<Action>> _duringActions = new();
		private readonly Queue<IAiEventAlert> _eventAlerts = new();

		protected ICombatEntity? _target;
		private DateTime _targetAcquiredTime;

		// State tracking for advanced conditions
		private readonly Dictionary<string, object> _tempVars = new();
		protected SkillId _lastUsedSkill = SkillId.None;
		protected readonly List<SkillId> _usedSkillHistory = new(10);

		// Cached components (hot-path)
		private MovementComponent? _movement;

		public ICombatEntity? Target => _target;
		public bool Suspended { get; set; }
		public bool ShowDebug { get; set; }

		/// <summary>
		/// Returns the entity that this script is controlling.
		/// </summary>
		public ICombatEntity Entity { get; private set; }

		/// <summary>
		/// Returns the owner of the entity that this script is controlling.
		/// </summary>
		public ICombatEntity Owner { get; private set; }

		/// <summary>
		/// Returns the name of the currently running routine if it was named.
		/// </summary>
		public string CurrentRoutine { get; protected set; }

		/// <summary>
		/// Initializes AI for the given entity, setting the initial hostility and tendency.
		/// </summary>
		/// <param name="combatEntity"></param>
		internal void InitFor(ICombatEntity combatEntity)
		{
			this.Entity = combatEntity;
			this.SetTendency(combatEntity.Tendency);

			// Cache hot-path components
			_movement = this.Entity.Components.Get<MovementComponent>();

			if (ZoneServer.Instance.Data.FactionDb.TryFind(this.Entity.Faction, out var factionData))
				this.HatesFaction(factionData.Hostile);

			this.InitializeSkillRotation();
			this.Setup();

			_initiated = true;
		}

		/// <summary>
		/// Switches the AI's faction and the associated hate.
		/// </summary>
		/// <param name="faction"></param>
		protected void SwitchFaction(FactionType faction)
		{
			if (this.Entity is Mob mob)
				mob.Faction = faction;

			this.ClearHate();

			if (ZoneServer.Instance.Data.FactionDb.TryFind(faction, out var factionData))
				this.HatesFaction(factionData.Hostile);
		}

		/// <summary>
		/// Set the view range of AI.
		/// </summary>
		/// <param name="viewRange"></param>
		public void SetViewRange(float viewRange)
		{
			this._viewRange = viewRange;
		}

		/// <summary>
		/// Set the aggro range of AI.
		/// </summary>
		/// <param name="range"></param>
		public void SetAggroRange(float range)
		{
			_hateRange = range;
		}

		/// <summary>
		/// Gets the aggro range of AI
		/// </summary>
		/// <returns></returns>
		public float GetAggroRange()
		{
			return _hateRange;
		}

		/// <summary>
		/// Executes the AI, furthering the current routine.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			using (Debug.Profile($"AiScript.Update: {this.Entity.Name} ({this.Entity.Handle}), Routine: {this.CurrentRoutine ?? "Idle"}", 50))
			{
				try
				{
					if (!_initiated)
						throw new InvalidOperationException("AI has not been initiated.");

					if (this.Entity.IsDead)
					{
						if (!_isDeadNotified)
						{
							_isDeadNotified = true;
							this.OnDeath();
						}
						// Reset target on death to stop any lingering routines.
						if (_target != null)
						{
							_target = null;
						}
						return;
					}

					if (this.Suspended)
						return;

					// The main logic of the AI tick
					this.UpdateHate(elapsed);
					this.UpdatePhase();
					this.HandleEventAlerts();
					this.ExecuteDuringActions();

					// This is the call that executes the current coroutine (e.g., Follow, Attack)
					this.Heartbeat();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Exception during AiScript.Update for entity '{this.Entity.Name}' (Handle: {this.Entity.Handle}): {ex}");
				}
			}
		}
		protected virtual void CheckEnemies()
		{
			var mostHated = this.GetMostHated();
			if (mostHated != null && (_target != mostHated || _target == null))
			{
				if (_target != mostHated)
				{
					_targetAcquiredTime = DateTime.UtcNow; // Set time when target changes
				}
				_target = mostHated;
				this.StartRoutine("StopAndAttack", this.StopAndAttack());
			}
		}
		protected virtual void CheckMaster()
		{
			if (_target == null)
				return;

			if (!this.TryGetMaster(out var master))
				return;

			// Reset aggro if the master left
			if (this.EntityGone(master) || !this.InRangeOf(master, MaxMasterDistance))
			{
				_target = null;
				// Clear all hate immediately when master is gone - summon should fully reset
				if (EnableReturnHome)
					this.StartRoutine("ReturnHome", this.ReturnHome(clearAllHateImmediately: true));
				else
				{
					this.RemoveAllHate();
					this.StartRoutine("Idle", this.Idle());
				}
			}
		}

		protected virtual void CheckTarget()
		{
			// We cannot get a target if we cannot attack
			if (this.Entity.IsLocked(LockType.Attack))
			{
				return;
			}

			if (this.EntityGone(_target) || !this.InRangeOf(_target, MaxChaseDistance))
			{
				_target = null;
				if (EnableReturnHome)
					this.StartRoutine("ReturnHome", this.ReturnHome());
				else
					this.StartRoutine("Idle", this.Idle());
				return;
			}

			if (!this.IsHating(_target))
			{
				_target = null;
				if (EnableReturnHome)
					this.StartRoutine("ReturnHome", this.ReturnHome());
				else
					this.StartRoutine("Idle", this.Idle());
			}
		}

		/// <summary>
		/// Returns true if the entity has any fear-causing debuff active.
		/// </summary>
		protected virtual bool IsFeared()
		{
			// Check for all fear-causing debuffs
			if (this.Entity.IsBuffActive(BuffId.Pollution_Debuff))
				return true;

			if (this.Entity.IsBuffActive(BuffId.Fear))
				return true;

			if (this.Entity.IsBuffActive(BuffId.Growling_fear_Debuff))
				return true;

			// Add other fear debuffs here as needed
			// if (this.Entity.IsBuffActive(BuffId.OtherFearDebuff))
			//     return true;

			return false;
		}

		/// <summary>
		/// Checks if the monster has a fear debuff and runs away from nearby characters.
		/// </summary>
		protected virtual void CheckFear()
		{
			// Check if the entity has any fear debuff
			if (!this.IsFeared())
				return;

			// Don't interrupt if currently using a skill (attacking)
			// Wait for the current attack animation to complete
			if (this.IsUsingSkill())
				return;

			// Also check if we're still in the skill's animation/recovery period
			// This ensures the full attack animation completes before fleeing
			var timeSinceLastSkill = DateTime.UtcNow - _lastSkillUseTime;
			if (timeSinceLastSkill < _lastSkillDuration)
				return;

			// Find all characters within 200 range
			var nearbyCharacters = this.Entity.Map.GetAttackableEnemiesInPosition(this.Entity, this.Entity.Position, 200)
				.OfType<Character>()
				.Where(c => !c.IsDead)
				.OrderBy(c => c.Position.Get2DDistance(this.Entity.Position))
				.ToList();

			// If there are nearby characters, run away from the closest one
			if (nearbyCharacters.Any())
			{
				var closestCharacter = nearbyCharacters.First();

				// Calculate away direction from the closest character
				var awayVector = (this.Entity.Position - closestCharacter.Position).Normalize2D();
				var fleeDistance = 150f; // Run 150 units away
				var idealDestination = this.Entity.Position + (awayVector * fleeDistance);

				// Try to find valid ground near the ideal flee destination
				if (this.Entity.Map.Ground.TryGetNearestValidPosition(idealDestination, this.Entity.AgentRadius, out var validDestination, maxDistance: 100f))
				{
					this.StartRoutine("FearFlee", this.FearFlee(validDestination));
				}
			}
		}

		/// <summary>
		/// Makes the monster flee in fear continuously while the debuff is active.
		/// </summary>
		protected virtual IEnumerable FearFlee(Position initialDestination)
		{
			this.SetRunning(true);

			// Continue fleeing while any fear buff is active
			while (this.IsFeared())
			{
				// Find the closest character to run away from
				var nearbyCharacters = this.Entity.Map.GetAttackableEnemiesInPosition(this.Entity, this.Entity.Position, 200)
					.OfType<Character>()
					.Where(c => !c.IsDead)
					.OrderBy(c => c.Position.Get2DDistance(this.Entity.Position))
					.ToList();

				if (nearbyCharacters.Any())
				{
					var closestCharacter = nearbyCharacters.First();

					// Calculate away direction from the closest character
					var awayVector = (this.Entity.Position - closestCharacter.Position).Normalize2D();
					var fleeDistance = 150f;
					var idealDestination = this.Entity.Position + (awayVector * fleeDistance);

					// Find nearest valid ground for fleeing
					if (this.Entity.Map.Ground.TryGetNearestValidPosition(idealDestination, this.Entity.AgentRadius, out var validDestination, maxDistance: 100f))
					{
						yield return this.MoveTo(validDestination, wait: false);
					}
				}

				// Small delay before recalculating flee direction
				yield return this.Wait(300);
			}

			// Fear wore off, return to normal behavior
			if (_target != null)
				this.StartRoutine("Attack", this.Attack());
			else
				this.StartRoutine("Idle", this.Idle());
		}
		protected IEnumerable ReturnHome(bool clearAllHateImmediately = false)
		{
			// If requested (e.g., master gone), clear all hate immediately
			if (clearAllHateImmediately)
				this.RemoveAllHate();

			_target = null;

			if (this.Entity is IMonster monster && monster.SpawnPosition != Position.Zero)
			{
				const float homeRadius = 30f;
				if (monster.Position.Get2DDistance(monster.SpawnPosition) > homeRadius)
				{
					this.SetRunning(true);
					// Move back to a random spot within the "home" radius around the spawn point
					yield return this.MoveTo(monster.SpawnPosition.GetRandomInRange2D(1, (int)homeRadius - 5));
				}
			}

			// Ensure proper state reset, especially for Elite/Boss monsters
			if (this.Entity is Mob mob)
			{
				// Clear any lingering debuffs that might prevent proper AI function
				if (mob.IsLocked(LockType.Movement) || mob.IsLocked(LockType.Attack))
				{
					// Wait a moment for locks to clear
					yield return this.Wait(500);
				}

				// Check for enemies while returning home (unless hate was already cleared)
				if (!clearAllHateImmediately)
				{
					yield return this.Wait(200);
					this.CheckEnemies();
					// If CheckEnemies found a target, it will have started StopAndAttack
					// So we should exit this routine
					if (_target != null)
						yield break;
				}
			}

			// Only clear all hate if we're actually going idle (no targets found)
			// and didn't already clear it at the start
			if (!clearAllHateImmediately)
				this.RemoveAllHate();

			// Now transition to Idle - we must use yield break after StartRoutine
			this.StartRoutine("Idle", this.Idle());
			yield break;
		}
		protected virtual IEnumerable StopAndAttack(string emote = "I_emo_exclamation")
		{
			this.ExecuteOnce(this.Emoticon(emote));
			this.ExecuteOnce(this.TurnTowards(_target));

			if (this.Entity.Components.TryGet<CombatComponent>(out var combatComponent))
				combatComponent.SetAttackState(true);

			yield return this.StopMove();
			this.StartRoutine("Attack", this.Attack());
			yield break;
		}

		protected virtual IEnumerable StopAndIdle()
		{
			yield return this.StopMove();
			this.StartRoutine("Idle", this.Idle());
			yield break;
		}
		protected virtual IEnumerable Attack()
		{
			this.SetRunning(true);

			while (!this.EntityGone(_target) && this.IsHating(_target))
			{
				// Check if we can attack
				var cannotAttack = this.Entity.IsLocked(LockType.Attack);
				if (cannotAttack)
				{
					// When silenced, keep following the target but don't try skills
					var followRange = 80f; // Stay close but not too close

					if (!this.InRangeOf(_target, followRange))
					{
						// Keep moving towards target
						yield return this.MoveToAttack(_target, followRange);
					}
					else
					{
						// We're close enough, just face them and wait
						yield return this.TurnTowards(_target);
					}

					// Important: yield to prevent blocking the thread
					yield return this.Wait(100, 200);
					continue; // Re-check the loop conditions
				}

				// Try to get a skill to use
				if (!this.TryGetRandomSkill(out var skill))
				{
					// No skills available, wait a bit and retry
					yield return this.Wait(250);
					continue;
				}

				// Get attack range for the skill
				var attackRange = this.GetAttackRange(skill);

				// Move into range if needed
				if (!this.InRangeOf(_target, attackRange))
				{
					if (RangeType == AttackerRangeType.Melee)
						yield return this.MoveToAttack(_target, attackRange);
					else if (RangeType == AttackerRangeType.Ranged)
						yield return this.MoveToRangedAttack(_target, attackRange);
					else
					{
						if (this.Entity is Mob mob)
							Log.Warning("No attacker range type defined for MonsterId: '" + mob.Id + "' and ClassName: '" + mob.ClassName + "'");
						else
							Log.Warning("No attacker range type defined for: " + this.Entity.Name);
					}

					// After moving, re-check if target is still valid
					if (this.EntityGone(_target) || !this.IsHating(_target))
					{
						_target = null;
						if (EnableReturnHome)
							this.StartRoutine("ReturnHome", this.ReturnHome());
						else
							this.StartRoutine("Idle", this.Idle());
						yield break;
					}
				}

				// Attack if in range and able
				if (this.InRangeOf(_target, attackRange) && this.CanUseSkill(skill, _target))
				{
					yield return this.UseSkill(skill, _target);

					// After skill completes, check if we're feared
					// If feared AND not still animating, immediately transition to Idle
					// so CheckFear can start the flee routine on the next tick
					if (this.IsFeared() && !this.IsUsingSkill())
					{
						// Let CheckFear handle the flee routine on the next tick
						// by transitioning to Idle where CheckFear is also active
						_target = null;
						this.StartRoutine("Idle", this.Idle());
						yield break;
					}
				}

			}

			_target = null;
			this.CheckEnemies();

			if (_target != null)
				yield break;

			if (EnableReturnHome)
				this.StartRoutine("ReturnHome", this.ReturnHome());
			else
				this.StartRoutine("Idle", this.Idle());
			yield break;
		}
		protected virtual IEnumerable Idle()
		{
			this.ResetMoveSpeed();

			var master = this.GetMaster();
			if (master != null)
			{
				yield return this.Animation("IDLE");
				yield return this.Follow(master);
				yield break;
			}

			// Leashing check: if too far from spawn, return home.
			if (EnableReturnHome && this.Entity is IMonster monster && monster.SpawnPosition != Position.Zero
				&& monster.Position.Get2DDistance(monster.SpawnPosition) > MaxRoamDistance)
			{
				// Start ReturnHome as a new routine instead of yielding to it
				this.StartRoutine("ReturnHome", this.ReturnHome());
				yield break;
			}

			yield return this.Wait(4000, 8000);

			this.SwitchRandom();
			if (this.Case(80))
				yield return this.MoveRandom();
			else
				yield return this.Animation("IDLE");
		}
		/// <summary>
		/// Alerts nearby allies to attack the entity's attacker.
		/// Elite and Boss monsters have higher chances and shorter cooldowns.
		/// </summary>
		/// <param name="attacker"></param>
		protected virtual void TryCallForHelp(ICombatEntity attacker)
		{
			// Summons should never call for help
			if (this.Entity is Summon summon)
				return;

			// Determine call chance and cooldown based on monster rank
			var actualCallChance = _helpCallChance;
			var actualCooldown = _helpCallCooldown;

			if (this.Entity is Mob mob)
			{
				if (mob.Rank == MonsterRank.Boss)
				{
					// Bosses: 20% chance, 15 second cooldown
					actualCallChance = 0.20f;
					actualCooldown = TimeSpan.FromSeconds(15);
				}
				else if (mob.IsBuffActive(BuffId.EliteMonsterBuff))
				{
					// Elite monsters: 10% chance, 20 second cooldown
					actualCallChance = 0.10f;
					actualCooldown = TimeSpan.FromSeconds(20);
				}
				// Regular monsters use default values (1% chance, 30 second cooldown)
			}

			// Check cooldown
			if ((DateTime.UtcNow - _lastHelpCallTime) < actualCooldown)
				return;

			// Random chance check
			if (RandomProvider.Get().NextDouble() > actualCallChance)
				return;

			// Find nearby allies of same type
			var allies = this.Entity.Map.GetAttackableEnemiesInPosition(attacker, this.Entity.Position, _helpCallRange)
				.OfType<Mob>()
				.Where(m =>
					m.Id == ((Mob)this.Entity).Id &&
					m.Handle != this.Entity.Handle &&
					!m.IsDead &&
					m.Components.TryGet<AiComponent>(out var ai) && ai.Script.Target == null // Only help allies that aren't already fighting
				);

			if (!allies.Any())
				return;

			// Visual feedback
			this.ExecuteOnce(this.Emoticon("I_emo_exclamation"));

			// Different call messages based on rank
			if (this.Entity is Mob callingMob)
			{
				if (callingMob.Rank == MonsterRank.Boss)
					this.ExecuteOnce(this.Say("Destroy the intruder!"));
				else if (callingMob.IsBuffActive(BuffId.EliteMonsterBuff))
					this.ExecuteOnce(this.Say("Rally to me!"));
				else
					this.ExecuteOnce(this.Say("To me, brothers!"));
			}

			// Alert allies with stronger hate for elite/boss monsters
			var hateAmount = 150f;
			if (this.Entity is Mob mobCaller)
			{
				if (mobCaller.Rank == MonsterRank.Boss)
					hateAmount = 300f; // Bosses generate more hate when calling for help
				else if (mobCaller.IsBuffActive(BuffId.EliteMonsterBuff))
					hateAmount = 200f; // Elites generate moderate additional hate
			}

			foreach (var ally in allies)
			{
				if (ally.Components.TryGet<AiComponent>(out var ai))
				{
					// Queue hate increase on attacker
					ai.Script.QueueEventAlert(new HateIncreaseAlert(attacker, hateAmount));
				}
			}

			_lastHelpCallTime = DateTime.UtcNow;
		}
		/// <summary>
		/// Checks HP and updates phase state automatically
		/// </summary>
		protected virtual void UpdatePhase()
		{
			if (this.Entity.IsDead)
				return;

			var hpPercent = (float)this.Entity.Hp / this.Entity.MaxHp;

			for (var i = _currentPhase; i < _phaseThresholds.Length; i++)
			{
				if (hpPercent <= _phaseThresholds[i])
				{
					_currentPhase = i + 1;
					this.OnPhaseChange(_currentPhase);
				}
			}
		}

		/// <summary>
		/// Called when phase changes (e.g., 75% -> 50% HP)
		/// </summary>
		/// <param name="newPhase"></param>
		protected virtual void OnPhaseChange(int newPhase)
		{
		}

		/// <summary>
		/// Updates hate levels for potentialy nearby enemies.
		/// </summary>
		/// <param name="elapsed"></param>
		private void UpdateHate(TimeSpan elapsed)
		{
			var potentialEnemiesList = this.Entity.Map
				.GetAttackableEnemiesInPosition(this.Entity, this.Entity.Position, _viewRange);

			this.RemoveNonNearbyHate(elapsed, potentialEnemiesList);
			this.IncreaseNearbyHate(elapsed, potentialEnemiesList);
		}

		/// <summary>
		/// Slowly removes hate levels of enemies that are no longer nearby.
		/// </summary>
		/// <param name="elapsed"></param>
		/// <param name="potentialEnemies"></param>
		private void RemoveNonNearbyHate(TimeSpan elapsed, IEnumerable<ICombatEntity> potentialEnemies)
		{
			_hateLevelsToRemove.Clear();

			var nearbyHandles = new HashSet<int>(potentialEnemies.Select(a => a.Handle));

			foreach (var entry in _hateLevels)
			{
				var handle = entry.Key;

				if (!nearbyHandles.Contains(handle))
				{
					// Check if the entity is dead or gone from the map
					var entity = this.Entity.Map.GetCombatEntity(handle);
					if (entity == null || entity.IsDead || entity.IsBuffActive(BuffId.Pet_Dead))
					{
						// Immediately remove hate for dead or gone entities (including dead pets)
						_hateLevelsToRemove.Add(handle);
					}
					else
					{
						// Gradually decay hate for entities that are just out of range
						var currentHate = entry.Value;

						// Adjust rate decay. That's how much hate is lost per second
						var decayAmount = (float)(elapsed.TotalSeconds * _hateLossPerSecond);
						var newHate = currentHate - decayAmount;

						if (newHate <= 0)
							_hateLevelsToRemove.Add(handle);
						else
							_hateLevels[handle] = newHate;
					}
				}
			}

			foreach (var handle in _hateLevelsToRemove)
				_hateLevels.Remove(handle);
		}

		/// <summary>
		/// Clears all hate levels.
		/// </summary>
		protected void RemoveAllHate()
		{
			_hateLevels.Clear();
		}

		/// <summary>
		/// Increase hate levels of enemies that are nearby.
		/// </summary>
		/// <param name="elapsed"></param>
		/// <param name="potentialEnemies"></param>
		private void IncreaseNearbyHate(TimeSpan elapsed, IList<ICombatEntity> potentialEnemies)
		{
			// Only increase hate for nearby enemies if the AI has
			// aggressive tendencies
			// Note: Commented out in Laima because we want those peaceful
			// monsters to also eventually attack nearby players
			// if (_tendency == TendencyType.Peaceful)
			//	return;

			// Increase hate for enemies that the entity is hostile towards
			for (int i = 0; i < potentialEnemies.Count; i++)
			{
				var potentialEnemy = potentialEnemies[i];

				if (!potentialEnemy.Position.InRange2D(this.Entity.Position, _hateRange))
					continue;

				if (!this.IsHostileTowards(potentialEnemy))
					continue;

				if (!this.CanAccumulateHate(potentialEnemy))
					continue;

				var handle = potentialEnemy.Handle;
				var amount = (float)(_hateGainPerSecond * elapsed.TotalSeconds);

				// --- Custom in Laima ---
				var enemyInAttackState = potentialEnemy.Components.Get<CombatComponent>()?.AttackState ?? false;
				if (this.Entity.Tendency == TendencyType.Peaceful)
				{
					if (enemyInAttackState)
						amount /= 5f;
					else
						continue; // BUGFIX: don't early-return the entire method
				}
				if (!this.IsHating(potentialEnemy) && this.Entity.Tendency == TendencyType.Aggressive)
					amount *= 2.5f;
				// -----------------------

				this.IncreaseHate(potentialEnemy, amount);
			}
		}

		/// <summary>
		/// Increases hate towards the entity with the given entity.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="amount"></param>
		protected void IncreaseHate(ICombatEntity entity, float amount)
		{
			if (this.Entity.IsBuffActive(BuffId.Lachrymator_Debuff))
				return;

			var handle = entity.Handle;

			// Hate increases faster if entity has the Liberate buff.
			// This means instant aggro from aggressive monsters and a
			// higher chance to keep it.
			if (entity.TryGetBuff(BuffId.Liberate_Buff, out var liberateBuff))
			{
				var skillLv = liberateBuff.NumArg1;
				var liberateThreatMultiplier = skillLv;

				if (liberateBuff.NumArg2 == (int)AbilityId.Swordman32)
					liberateThreatMultiplier *= 5;
				else if (liberateBuff.NumArg2 == (int)AbilityId.Swordman31)
					liberateThreatMultiplier *= 2;

				amount *= liberateThreatMultiplier;
			}
			// Hate increases if entity has Bully buff.
			if (entity.TryGetBuff(BuffId.Bully_Buff, out var bullyBuff))
			{
				var skillLv = bullyBuff.NumArg1;
				var threatMultiplier = 2;

				amount *= threatMultiplier;
			}

			// Hate decreases if entity has Fade buff.
			if (entity.TryGetBuff(BuffId.Fade_Buff, out var fadeBuff))
			{
				var skillLv = fadeBuff.NumArg1;
				var fadeThreatMultiplier = 0.8f - (skillLv * 0.1f);
				fadeThreatMultiplier = Math.Max(0.1f, fadeThreatMultiplier);

				amount *= fadeThreatMultiplier;
			}

			// Hate increases if this monster has Decomposition debuff and the entity is the caster.
			if (this.Entity.TryGetBuff(BuffId.Decomposition_Debuff, out var decompositionDebuff))
			{
				if (decompositionDebuff.Caster == entity)
				{
					var skillLv = decompositionDebuff.NumArg1;
					var decompositionThreatMultiplier = 1f + (skillLv / 2);

					amount *= decompositionThreatMultiplier;
				}
			}

			// Increase the hate level at the normal rate up to the
			// min aggro level. Once we reach that point we lower
			// the hate increase so it will still accumulate for
			// an enemy, but not at such a rate that other enemies
			// couldn't potentially keep up. In theory this should
			// make it possible to steal aggro, but not too easily.

			if (!_hateLevels.TryGetValue(handle, out var curHate))
				_hateLevels[handle] = curHate = 0;

			var newHate = curHate + amount;

			// If we're below the min aggro level, increase hate normally
			if (newHate <= _minAggroHateLevel)
			{
				newHate = curHate + amount;
			}
			// If we're going past the min aggro level, but we passed the
			// threshold just now, go to the min level and add the adjusted
			// amount on top of it
			else if (curHate <= _overHateThreshold)
			{
				var hateAboveThreshold = newHate - _overHateThreshold;
				newHate = _overHateThreshold + hateAboveThreshold * _overHateRate;
			}
			// If we've been past the min aggro level already, add the adjusted
			// amount
			else
			{
				newHate = curHate + amount * _overHateRate;
			}

			// If we wanted to be clever we could do the following, but the above
			// is easier to grasp.
			//var addAdjusted = Math.Max(0, curHate + amount - _minAggroHateLevel);
			//var addNormal = amount - addAdjusted;
			//var newHate = curHate + addNormal;
			//newHate += addAdjusted * _overHateRate;

			_hateLevels[handle] = newHate;

			// Debug
			// Console.WriteLine("Monster {0} hate level for {1} is now {2}.", this.Entity, entity.Name, _hateLevels[handle]);
		}

		/// <summary>
		/// Returns true if the AI's entity is hostile towards the given
		/// entity based on the factions they belong to, or based on
		/// PvP/duel status.
		/// </summary>
		/// <param name="otherEntity"></param>
		/// <returns></returns>
		public virtual bool IsHostileTowards(ICombatEntity otherEntity)
		{
			if (_hatedFactions.Contains(otherEntity.Faction))
				return true;

			if (otherEntity is Mob mob && _hatedMonsters.Contains(mob.Id))
				return true;

			// Check PvP/duel relations via the relation system
			// This handles owned summons/companions in duels and PvP maps
			if (this.Entity.IsEnemy(otherEntity))
				return true;

			return false;
		}

		/// <summary>
		/// Makes AI hostile towards the given factions.
		/// </summary>
		/// <param name="faction"></param>
		protected void HatesFaction(params FactionType[] factions)
			=> this.HatesFaction((IEnumerable<FactionType>)factions);

		/// <summary>
		/// Makes AI hostile towards the given factions.
		/// </summary>
		/// <param name="faction"></param>
		protected void HatesFaction(IEnumerable<FactionType> factions)
		{
			_hatedFactions.UnionWith(factions);
		}

		/// <summary>
		/// Makes AI hostile towards the given monsters.
		/// </summary>
		/// <param name="monsterIds"></param>
		protected void HatesMonster(params int[] monsterIds)
		{
			_hatedMonsters.UnionWith(monsterIds);
		}

		/// <summary>
		/// Removes all hate factors, such as hostility towards factions and monsters.
		/// </summary>
		protected void ClearHate()
		{
			_hatedFactions.Clear();
			_hatedMonsters.Clear();
		}

		/// <summary>
		/// Returns true if the given entity can accumulate hate, based on its
		/// current state.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		protected bool CanAccumulateHate(ICombatEntity entity)
		{
			if (entity.IsBuffActiveByKeyword(BuffTag.Cloaking))
				return false;

			// Dead pets should never accumulate hate
			if (entity.IsBuffActive(BuffId.Pet_Dead))
				return false;

			// Provocation Immunity prevents hate from all except its caster
			// as long as the caster remains in range
			if (this.Entity.TryGetBuff(BuffId.ProvocationImmunity_Debuff, out var piDebuff))
			{
				var caster = (ICombatEntity)piDebuff.Caster;

				if (entity != caster && !this.EntityGone(caster) && this.InRangeOf(caster, 300))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Returns true if the given entity is a valid target to be hated and
		/// targetted, based on its current state.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		protected bool CanBeHated(ICombatEntity entity)
		{
			if (entity.IsBuffActiveByKeyword(BuffTag.Cloaking))
				return false;

			// Dead pets should never be hated or targetted
			if (entity.IsBuffActive(BuffId.Pet_Dead))
				return false;

			return true;
		}

		/// <summary>
		/// Returns the enemy with the highest hate level in range. Returns null
		/// if no nearby enemies have reached the minimum aggro level.
		/// </summary>
		/// <returns></returns>
		protected ICombatEntity GetMostHated()
		{
			// This buff overrides the most hated target as long as the caster
			// remains in range.
			if (this.Entity.TryGetBuff(BuffId.ProvocationImmunity_Debuff, out var piDebuff))
			{
				var caster = (ICombatEntity)piDebuff.Caster;

				if (!this.EntityGone(caster) && this.InRangeOf(caster, 300))
					return caster;
			}

			var highestHate = 0f;
			ICombatEntity mostHated = null;

			_hateLevelsToRemove.Clear();

			foreach (var entry in _hateLevels)
			{
				var handle = entry.Key;
				var hate = entry.Value;

				if (hate <= highestHate)
					continue;

				var entity = this.Entity.Map.GetCombatEntity(handle);

				if (entity == null)
				{
					_hateLevelsToRemove.Add(handle);
					continue;
				}

				if (!this.CanBeHated(entity))
					continue;

				highestHate = hate;
				mostHated = entity;
			}

			// Clean up stale entries (entities gone)
			foreach (var handle in _hateLevelsToRemove)
				_hateLevels.Remove(handle);

			if (highestHate < _minAggroHateLevel)
				return null;

			return mostHated;
		}

		/// <summary>
		/// Returns true if the hate towards the given entity is above
		/// the aggro threshold.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		protected bool IsHating(ICombatEntity entity)
		{
			// Always hating the person that casted this buff
			if (this.Entity.TryGetBuff(BuffId.ProvocationImmunity_Debuff, out var piDebuff))
			{
				if (entity == piDebuff.Caster)
					return true;
			}

			if (!_hateLevels.TryGetValue(entity.Handle, out var hate))
				return false;

			if (!this.CanBeHated(entity))
				return false;

			return (hate >= _minAggroHateLevel);
		}

		/// <summary>
		/// Executed once during the AI's first tick.
		/// </summary>
		protected virtual void Setup()
		{
		}

		/// <summary>
		/// Called once when the entity dies. Can be overridden for special death behaviors.
		/// </summary>
		protected virtual void OnDeath()
		{
		}

		/// <summary>
		/// Called when the entity takes damage. Can be overridden for reactive behaviors.
		/// Equivalent to a TakeDamage hook.
		/// </summary>
		/// <param name="attacker">The entity that dealt the damage.</param>
		/// <param name="damage">The amount of damage taken.</param>
		protected virtual void OnTakeDamage(ICombatEntity attacker, float damage)
		{
		}

		/// <summary>
		/// Sets up the given action to execute on every tick while the
		/// routine is active.
		/// </summary>
		/// <param name="routineName"></param>
		/// <param name="action"></param>
		protected void During(string routineName, Action action)
		{
			if (!_duringActions.TryGetValue(routineName, out var list))
				_duringActions[routineName] = list = new List<Action>();

			if (!list.Contains(action))
				list.Add(action);
		}

		/// <summary>
		/// Handles events that happened since the last tick.
		/// </summary>
		private void HandleEventAlerts()
		{
			lock (_eventAlerts)
			{
				while (_eventAlerts.Count > 0)
				{
					var eventAlert = _eventAlerts.Dequeue();
					this.ReactToAlert(eventAlert);
				}
			}
		}

		/// <summary>
		/// Makes AI react to the given alert.
		/// </summary>
		/// <param name="eventAlert"></param>
		private void ReactToAlert(IAiEventAlert eventAlert)
		{
			switch (eventAlert)
			{
				case HitEventAlert hitEventAlert:
				{
					var entityWasAttacked = (hitEventAlert.Target.Handle == this.Entity.Handle);
					var masterWasAttacked = (hitEventAlert.Target.Handle == _masterHandle);
					var masterDidAttack = (hitEventAlert.Attacker.Handle == _masterHandle);

					if (entityWasAttacked)
					{
						_lastAttackedTime = DateTime.UtcNow;
						_lastAttackerHandle = hitEventAlert.Attacker.Handle;

						this.OnTakeDamage(hitEventAlert.Attacker, hitEventAlert.Damage);
						this.TryCallForHelp(hitEventAlert.Attacker);
					}

					if (entityWasAttacked || masterWasAttacked)
					{
						this.IncreaseHate(hitEventAlert.Attacker, _hatePerHit);

						// If we don't have a target, or we're returning home, check for enemies to start combat
						// This handles the case where the target was cleared above, was already null, 
						// or we're returning home and need to re-engage when attacked
						if (_target == null || this.CurrentRoutine == "ReturnHome")
						{
							this.CheckEnemies();
						}
						// If we're attacking but the attacker has more hate now, re-evaluate
						else if (this.CurrentRoutine == "Attack")
						{
							var mostHated = this.GetMostHated();
							if (mostHated != null && mostHated != _target)
							{
								_target = mostHated;
							}
						}
					}
					else if (masterDidAttack)
						this.IncreaseHate(hitEventAlert.Target, _hatePerHit);

					break;
				}

				case HateResetAlert hateResetAlert:
				{
					if (hateResetAlert.Target != null)
					{
						var targetHandle = hateResetAlert.Target.Handle;
						_hateLevels.Remove(targetHandle);
					}
					else
						this.RemoveAllHate();
					break;
				}

				case HateIncreaseAlert insertHateAlert:
				{
					var hateToAdd = insertHateAlert.Amount;

					this.IncreaseHate(insertHateAlert.Target, hateToAdd);

					// Re-evaluate target
					var mostHated = this.GetMostHated();
					if (mostHated != null && mostHated != _target)
					{
						_target = mostHated;
						this.StartRoutine("StopAndAttack", this.StopAndAttack());
					}
					else if (_target == null && mostHated != null)
					{
						_target = mostHated;
						this.StartRoutine("StopAndAttack", this.StopAndAttack());
					}

					break;
				}

				case MoveToAlert moveToAlert:
				{
					var position = moveToAlert.Position;

					this.StartRoutine(this.MoveStraight(position));

					if (moveToAlert.SuspendAI)
					{
						this.Suspended = true;
						Task.Delay(moveToAlert.MoveTime).ContinueWith(_ =>
						{
							this.Suspended = false;
						});
					}
					break;
				}

				case ChangeTendencyEventAlert changeTendencyEventAlert:
				{
					_tendency = changeTendencyEventAlert.Tendency;
					break;
				}

				case CancelSkillAlert:
				{
					this.Entity.Interrupt();
					break;
				}

				case SuspendAiAlert:
				{
					this.Suspended = true;
					break;
				}

				case ResumeAiAlert:
				{
					this.Suspended = false;
					// Stop any current movement when resuming
					_movement?.Stop();
					break;
				}

				case KnockdownAlert knockdownAlert:
				{
					// Entity is being knocked down - suspend AI temporarily
					// The knockdown state is handled by the combat system,
					// but we can react to it here if needed
					this.Entity.Interrupt();

					// If we had a target and were attacking, we may want to 
					// re-engage after recovering from knockdown
					if (_target != null && !this.EntityGone(_target))
					{
						// The entity will naturally resume attacking after knockdown recovery
						// through the normal AI tick cycle
					}
					break;
				}
			}
		}

		/// <summary>
		/// Queues up an alert about something that happened for the AI to
		/// potentially react to.
		/// </summary>
		/// <param name="eventAlert"></param>
		public void QueueEventAlert(IAiEventAlert eventAlert)
		{
			lock (_eventAlerts)
				_eventAlerts.Enqueue(eventAlert);
		}

		/// <summary>
		/// Clears all pending event alerts.
		/// </summary>
		public void ClearEventAlerts()
		{
			lock (_eventAlerts)
				_eventAlerts.Clear();
		}

		/// <summary>
		/// Clears the current target reference.
		/// </summary>
		public void ClearTarget()
		{
			_target = null;
		}

		/// <summary>
		/// Sets the range in which the AI will detect enemies.
		/// </summary>
		/// <param name="range"></param>
		protected void SetHateRange(float range)
		{
			_hateRange = range;
		}

		/// <summary>
		/// Sets the amount of hate the AI accumulates per second for
		/// a potential enemy.
		/// </summary>
		/// <param name="hateGainPerSecond"></param>
		/// <param name="hateLossPerSecond"></param>
		protected void SetHatePerSecond(float hateGainPerSecond, float hateLossPerSecond)
		{
			_hateGainPerSecond = hateGainPerSecond;
			_hateLossPerSecond = hateLossPerSecond;
		}

		/// <summary>
		/// Sets the rate at which which the AI accumulates hate for
		/// an enemy that is already past the minimum aggro level.
		/// </summary>
		/// <param name="overHateRate"></param>
		protected void SetOverHateRate(float overHateRate)
		{
			_overHateRate = overHateRate;
		}

		/// <summary>
		/// Sets the minimum hate level required for the AI to consider
		/// a potential enemy an actual enemy.
		/// </summary>
		/// <param name="minAggroHateLevel"></param>
		protected void SetMinHate(float minAggroHateLevel)
		{
			_minAggroHateLevel = minAggroHateLevel;
		}

		/// <summary>
		/// Sets the AI's tendency to attack.
		/// </summary>
		/// <param name="tendency"></param>
		protected void SetTendency(TendencyType tendency)
		{
			_tendency = tendency;
		}

		/// <summary>
		/// Sets the range in which the AI can see potential enemies.
		/// </summary>
		/// <param name="viewRange"></param>
		protected void SetViewDistance(float viewRange)
		{
			_viewRange = viewRange;
		}

		/// <summary>
		/// Sets the entity the AI follows around and supports.
		/// </summary>
		/// <param name="masterEntity"></param>
		public virtual void SetMaster(ICombatEntity masterEntity)
		{
			if (masterEntity != null)
			{
				_masterHandle = masterEntity.Handle;
				this.SwitchFaction(masterEntity.Faction);
			}
			else
			{
				_masterHandle = 0;
				this.SwitchFaction(FactionType.Law);
			}
		}

		/// <summary>
		/// Returns the AI's master, or null if it doesn't have one.
		/// </summary>
		/// <returns></returns>
		public ICombatEntity GetMaster()
		{
			if (_masterHandle == 0)
				return null;

			return this.Entity.Map.GetCombatEntity(_masterHandle);
		}

		/// <summary>
		/// Returns the AI's follow target, or null if it doesn't have one.
		/// </summary>
		/// <returns></returns>
		public ICombatEntity GetFollowTarget()
		{
			if (_followTargetHandle == 0)
				return null;

			return this.Entity.Map.GetCombatEntity(_followTargetHandle);
		}

		/// <summary>
		/// Returns the entity's master via out. Returns false if the
		/// entity doesn't have a master.
		/// </summary>
		/// <param name="master"></param>
		/// <returns></returns>
		public bool TryGetMaster(out ICombatEntity master)
		{
			master = this.GetMaster();
			return (master != null);
		}

		/// <summary>
		/// Executes the actions set up to occur while a specific routine
		/// is running.
		/// </summary>
		private void ExecuteDuringActions()
		{
			var currentRoutine = this.CurrentRoutine;
			if (currentRoutine == null)
				return;

			if (!_duringActions.TryGetValue(currentRoutine, out var actions))
				return;

			foreach (var action in actions)
			{
				action();

				// Stop if the action changed the current routine
				if (this.CurrentRoutine != currentRoutine)
					break;
			}
		}

		/// <summary>
		/// Called whenever the AI finishes a routine and doesn't
		/// know what to do next.
		/// </summary>
		protected override void Root()
		{
		}

		/// <summary>
		/// Starts executing the given routine and saves its name.
		/// </summary>
		/// <param name="routineName"></param>
		/// <param name="routine"></param>
		public void StartRoutine(string routineName, IEnumerable routine)
		{
			this.CurrentRoutine = routineName;
			this.StartRoutine(routine);
		}

		/// <summary>
		/// Returns true if the entity is dead or gone.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		protected bool EntityGone(ICombatEntity entity)
		{
			if (entity == null)
				return true;

			if (entity.IsDead)
				return true;

			if (this.Entity.Map.GetCombatEntity(entity.Handle) == null)
				return true;

			return false;
		}

		/// <summary>
		/// Gets a valid position adjacent to the given target within range.
		/// </summary>
		/// <remarks>
		/// Returns target's position if no valid adjacent position could be
		/// found within a reasonable amount of time.
		/// </remarks>
		/// <param name="target"></param>
		/// <param name="range"></param>
		/// <returns></returns>
		protected Position GetAdjacentPosition(ICombatEntity target, float range)
		{
			var rnd = RandomProvider.Get();

			var ground = target.Map.Ground;
			var targetPos = target.Position;

			for (var i = 0; i < 10; i++)
			{
				var pos = targetPos.GetRandomInRange2D((int)range, rnd);
				if (ground.IsValidPosition(pos))
					return pos;
			}

			return targetPos;
		}

		/// <summary>
		/// Returns true if the entity is in range of the given target.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="range"></param>
		/// <returns></returns>
		protected bool InRangeOf(IActor entity, float range)
		{
			return this.Entity.Position.InRange2D(entity.Position, (int)range);
		}

		/// <summary>
		/// Sets whether the entity is running, which potentially affects
		/// its movement speed.
		/// </summary>
		/// <param name="running"></param>
		protected void SetRunning(bool running)
		{
			var moveSpeedType = running ? MoveSpeedType.Run : MoveSpeedType.Walk;
			_movement?.SetMoveSpeedType(moveSpeedType);
		}

		/// <summary>
		/// Sets the entity's movement speed to the given fixed value.
		/// </summary>
		/// <param name="mspd"></param>
		protected void SetFixedMoveSpeed(float mspd)
		{
			_movement?.SetFixedMoveSpeed(mspd);
		}

		/// <summary>
		/// Resets any movement speed changes made.
		/// </summary>
		protected void ResetMoveSpeed()
		{
			_movement?.SetMoveSpeedType(MoveSpeedType.Walk);
			_movement?.SetFixedMoveSpeed(0);
		}

		/// <summary>
		/// Removes AI's entity from the world if it's a monster.
		/// </summary>
		protected void Despawn()
		{
			if (this.Entity is IMonster monster)
				monster.Map.RemoveMonster(monster);
		}

		/// <summary>
		/// Populates the _skillRotation list with all available skills for the monster.
		/// By default, all skills are added with a base priority.
		/// Specific AI scripts can override this method to create more complex rotations with
		/// different priorities and conditions.
		/// </summary>
		protected virtual void InitializeSkillRotation()
		{
			_skillRotation = new List<SkillPriority>();
			if (this.Entity is Mob mob && mob.Data.Skills.Any())
			{
				// By default, add all of the monster's skills to the rotation with a base priority.
				// This allows the AI to use any of its skills, not just the first one.
				foreach (var mobSkill in mob.Data.Skills)
				{
					// We need skill data to determine if a skill is a buff or heal.
					if (ZoneServer.Instance.Data.SkillDb.TryFind(mobSkill.SkillId, out var skillData))
					{
						// Create a temporary skill instance to access properties like IsHeal,
						// which might depend on skill effects not present in the base SkillData.
						// This instance is not added to the entity's skill component here.
						// We assume level 1 for monsters, consistent with other parts of the AI script.
						var tempSkill = new Skill(this.Entity, mobSkill.SkillId, 1);

						_skillRotation.Add(new SkillPriority
						{
							Id = mobSkill.SkillId,
							Priority = 1, // All skills get a default priority of 1.
							IsBuff = skillData.Type == SkillType.Buff,
							IsHeal = tempSkill.IsHeal
						});
					}
				}
			}
		}

		protected Skill GetOrCreateSkill(SkillId skillId)
		{
			if (skillId == SkillId.None) return null;

			if (this.Entity.TryGetSkill(skillId, out var skill))
			{
				return skill;
			}

			if (this.Entity.Components.TryGet<BaseSkillComponent>(out var skillComponent))
			{
				var newSkill = new Skill(this.Entity, skillId, 1);
				skillComponent.AddSilent(newSkill);
				return newSkill;
			}

			Log.Warning($"AiScript '{this.Entity.Name}' failed to GetOrCreateSkill '{skillId}' because it has no BaseSkillComponent.");
			return null;
		}

		/// <summary>
		/// Selects the highest-priority usable offensive skill from the rotation.
		/// </summary>
		protected virtual Skill SelectBestOffensiveSkill(ICombatEntity target)
		{
			if (_skillRotation == null || target == null) return null;

			var bestSkill = _skillRotation
				.Where(p => !p.IsBuff && !p.IsHeal && (p.Condition == null || p.Condition(target)))
				.OrderByDescending(p => p.Priority)
				.Select(p => this.GetOrCreateSkill(p.Id))
				.FirstOrDefault(s => s != null && this.CanUseSkill(s, target));

			return bestSkill;
		}

		/// <summary>
		/// Selects the highest-priority usable utility (buff/heal) skill from the rotation.
		/// </summary>
		protected virtual Skill SelectBestUtilitySkill()
		{
			if (_skillRotation == null) return null;

			var self = this.Entity;

			var bestSkill = _skillRotation
				.Where(p => p.IsBuff || p.IsHeal)
				.OrderByDescending(p => p.Priority)
				.Select(p =>
				{
					var skill = this.GetOrCreateSkill(p.Id);
					if (skill == null) return null;
					var master = this.GetMaster();

					var target = (p.IsHeal && master != null && !this.EntityGone(master)) ? master : self;

					return (p.Condition == null || p.Condition(target)) && this.CanUseSkill(skill, target) ? skill : null;
				})
				.FirstOrDefault(s => s != null);

			return bestSkill;
		}
		/// <summary>
		/// Checks if a skill can be used by the entity on the target.
		/// </summary>
		protected virtual bool CanUseSkill(Skill skill, ICombatEntity target)
		{
			if (this.Entity.IsLocked(LockType.Attack) || this.Entity.IsCasting())
				return false;

			if (target.IsLocked(LockType.GetHit))
				return false;

			if (this.Entity.IsDead || target.IsDead || skill.IsOnCooldown || this.Entity.IsGuarding() || this.Entity.IsKnockedDown() || this.Entity.IsKnockedBack())
				return false;

			return true;
		}

		/// <summary>
		/// Returns true if the entity is currently using a skill.
		/// </summary>
		protected bool IsUsingSkill()
		{
			return this.Entity.IsCasting();
		}

		#region TempVar Helpers
		/// <summary>
		/// Gets a value from the AI's internal state dictionary. Equivalent to Lua's GetExProp.
		/// </summary>
		protected T GetTempVar<T>(string key, T defaultValue = default)
		{
			if (_tempVars.TryGetValue(key, out var value) && value is T typedValue)
			{
				return typedValue;
			}
			return defaultValue;
		}

		/// <summary>
		/// Sets a value in the AI's internal state dictionary. Equivalent to Lua's SetExProp.
		/// </summary>
		protected void SetTempVar(string key, object value)
		{
			_tempVars[key] = value;
		}

		/// <summary>
		/// Removes a value from the AI's internal state dictionary.
		/// </summary>
		protected void RemoveTempVar(string key)
		{
			_tempVars.Remove(key);
		}
		#endregion

		/// <summary>
		/// Defines if an attacker is melee or ranged.
		/// </summary>
		protected enum AttackerRangeType
		{
			Melee = 1,
			Ranged = 2,
		}
	}
}
