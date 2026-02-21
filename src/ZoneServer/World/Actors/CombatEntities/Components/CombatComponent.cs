using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Extensions;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World.Actors.CombatEntities.Components
{
	/// <summary>
	/// An entity component that encapsulates combat-related methods
	/// and properties.
	/// </summary>
	public class CombatComponent : CombatEntityComponent, IUpdateable
	{
		private static readonly TimeSpan AttackStateDuration = TimeSpan.FromSeconds(10);
		private static readonly TimeSpan StaggerCooldownDuration = TimeSpan.FromSeconds(30);

		private readonly object _hitLock = new();
		private readonly Dictionary<int, float> _damageTaken = new();
		private readonly Dictionary<int, int> _hitsTaken = new();
		private readonly List<int> _targets = new();

		private Skill _castingSkill;
		private bool _isCasting;

		/// <summary>
		/// Returns the entity's attack state.
		/// </summary>
		public bool AttackState { get; private set; }

		/// <summary>
		/// Returns the entity's guard state.
		/// </summary>
		public bool IsGuarding { get; set; }

		/// <summary>
		/// Returns the entity's safe state.
		/// </summary>
		public bool IsSafe { get; set; }

		/// <summary>
		/// Returns the last time the entity was involved in combat in
		/// any way.
		/// </summary>
		public DateTime LastCombatTime { get; private set; }

		/// <summary>
		/// Raised when combat state changes.
		/// </summary>
		public event Action<ICombatEntity, bool> CombatStateChanged;

		/// <summary>
		/// Is the entity currently staggered?
		/// </summary>
		public bool IsStaggered { get; private set; }

		/// <summary>
		/// Returns the last time the entity was staggered.
		/// </summary>
		public DateTime LastStaggerTime { get; private set; }

		/// <summary>
		/// Raised when the entity is staggered.
		/// </summary>
		public event Action<ICombatEntity> Staggered;

		/// <summary>
		/// Creates new component for entity.
		/// </summary>
		/// <param name="entity"></param>
		public CombatComponent(ICombatEntity entity) : base(entity)
		{
		}

		/// <summary>
		/// Sets the entity's attack state.
		/// </summary>
		/// <param name="state"></param>
		public void SetAttackState(bool state)
		{
			var prevState = this.AttackState;

			this.AttackState = state;
			this.LastCombatTime = DateTime.UtcNow;

			Send.ZC_PC_ATKSTATE(this.Entity, state);

			if (prevState != state)
				CombatStateChanged?.Invoke(this.Entity, state);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			this.UpdateAttackState();
			this.UpdateStaggerState();
		}

		/// <summary>
		/// Updates the entity's attack state.
		/// </summary>
		private void UpdateAttackState()
		{
			if (!this.AttackState)
				return;

			var timePassed = DateTime.UtcNow - this.LastCombatTime;
			if (timePassed > AttackStateDuration)
				this.SetAttackState(false);
		}

		/// <summary>
		/// Registers a hit from the given attacker.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="damage"></param>
		public void RegisterHit(ICombatEntity attacker, float damage)
		{
			lock (_hitLock)
			{
				if (!_damageTaken.TryGetValue(attacker.Handle, out var totalDamage))
					totalDamage = 0;

				if (!_hitsTaken.TryGetValue(attacker.Handle, out var totalHits))
					totalHits = 0;

				_damageTaken[attacker.Handle] = totalDamage + damage;
				_hitsTaken[attacker.Handle] = totalHits + 1;
			}
		}

		/// <summary>
		/// Returns the attacker that has dealt the most damage to this
		/// entity and is still nearby and alive.
		/// </summary>
		/// <returns></returns>
		public ICombatEntity GetTopAttackerByDamage()
		{
			lock (_hitLock)
			{
				foreach (var handle in from kv in _damageTaken.OrderByDescending(a => a.Value)
									   let handle = kv.Key
									   select handle)
				{
					if (!this.Entity.Map.TryGetCombatEntity(handle, out var attacker))
						continue;
					if (attacker.IsDead)
						continue;
					return attacker;
				}
			}

			return null;
		}

		/// <summary>
		/// Returns the top N attackers by damage dealt, ordered from highest to lowest.
		/// Only returns attackers that are still nearby and alive.
		/// </summary>
		/// <param name="count">Maximum number of attackers to return.</param>
		/// <returns>List of tuples containing the attacker and their total damage dealt.</returns>
		public List<(ICombatEntity Attacker, float Damage)> GetTopAttackersByDamage(int count)
		{
			var result = new List<(ICombatEntity, float)>();

			lock (_hitLock)
			{
				foreach (var kv in _damageTaken.OrderByDescending(a => a.Value))
				{
					if (result.Count >= count)
						break;

					if (!this.Entity.Map.TryGetCombatEntity(kv.Key, out var attacker))
						continue;

					if (attacker.IsDead)
						continue;

					result.Add((attacker, kv.Value));
				}
			}

			return result;
		}

		/// <summary>
		/// Returns the attacker that has hit this entity the most and
		/// is still nearby and alive.
		/// </summary>
		/// <returns></returns>
		public ICombatEntity GetTopAttackerByHits()
		{
			lock (_hitLock)
			{
				foreach (var handle in from kv in _hitsTaken.OrderByDescending(a => a.Value)
									   let handle = kv.Key
									   select handle)
				{
					if (!this.Entity.Map.TryGetCombatEntity(handle, out var attacker))
						continue;
					if (attacker.IsDead)
						continue;
					return attacker;
				}
			}

			return null;
		}

		/// <summary>
		/// Sets the target to be casting a specific skill.
		/// </summary>
		/// <param name="casting"></param>
		/// <param name="skill"></param>

		public void SetCasting(bool casting, Skill skill)
		{
			this._isCasting = casting;

			if (casting)
				this._castingSkill = skill;
			else
				this._castingSkill = null;
		}

		/// <summary>
		/// Gets if the target is casting any skill.
		/// Return via out the skill being casted.
		/// </summary>
		/// <returns></returns>
		public bool IsCasting()
		{
			return this._isCasting;
		}

		/// <summary>
		/// Gets if the target is casting the given skill.
		/// </summary>
		/// <returns></returns>
		public bool IsCastingSkill(Skill skill)
		{
			if (this._isCasting && this._castingSkill == skill)
				return true;

			return false;
		}

		/// <summary>
		/// Returns true if entity is casting and can move while
		/// casting.
		/// </summary>
		/// <returns></returns>
		public bool IsMoveableCasting()
		{
			if (this._isCasting && this._castingSkill.Data.EnableCastMove)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Attempts to interrupt the currently casting skill taking into
		/// consideration if it has interruptible cast time or not.
		/// Returns true if the casting was interrupted.
		/// </summary>
		/// <returns></returns>
		public bool TryInterruptCasting(out Skill skill)
		{
			skill = this._castingSkill;

			if (!this.IsCastingSkill(skill))
				return false;

			if (!skill.IsCastInterruptible)
				return false;

			this.InterruptCasting();

			return true;
		}

		/// <summary>
		/// Interrupts the currently casting skill, regardless if it's
		/// cast time can naturally be interrupted or not. This is useful when
		/// applying debuffs such as stun or freeze.
		/// </summary>
		public void InterruptCasting()
		{
			var skill = this._castingSkill;

			skill?.Cancel();

			this.Entity.SetCastingState(false, skill);
			Send.ZC_NORMAL.CancelDynamicCast(this.Entity);
			// TODO: Additional Testing.
			// These packets might not be needed, it worked fine just sending CancelDynamicCast.
			// But might be needed with other players around to not freeze in casting animation.
			Send.ZC_SKILL_CAST_CANCEL(this.Entity);
			Send.ZC_SKILL_USE_CANCEL(this.Entity);
			Send.ZC_SKILL_DISABLE(this.Entity);
		}

		public void AddTarget(ICombatEntity target)
		{
			lock (_targets)
				_targets.Add(target.Handle);
		}

		public void ClearTargets()
		{
			lock (_targets)
				_targets.Clear();
		}

		public ICombatEntity[] GetTargets()
		{
			var results = new List<ICombatEntity>();
			lock (_targets)
			{
				foreach (var targetHandle in _targets)
					if (this.Entity.Map.TryGetCombatEntity(targetHandle, out var entity))
						results.Add(entity);
			}

			return results.ToArray();
		}

		public ICombatEntity GetRandomTarget()
		{
			lock (_targets)
			{
				if (this.Entity.Map.TryGetCombatEntity(_targets.Random(), out var entity))
					return entity;
			}
			return null;
		}

		/// <summary>
		/// Applies stagger to the entity.
		/// </summary>
		public void ApplyStagger()
		{
			if (!this.Entity.CanStagger() || this.IsStaggered)
				return;
			this.IsStaggered = true;
			this.LastStaggerTime = DateTime.UtcNow;
			this.Entity.StartBuff(BuffId.Stun, TimeSpan.FromSeconds(5));

			this.InterruptCasting();

			// Cancel any running skill for monsters/entities using BaseSkillComponent
			this.Entity.Components.Get<BaseSkillComponent>()?.CancelCurrentSkill();

			this.Staggered?.Invoke(this.Entity);
			Task.Delay(TimeSpan.FromSeconds(5))
				.ContinueWith(t =>
				{
					this.EndStagger();
				});
		}

		/// <summary>
		/// Ends stagger on the entity.
		/// </summary>
		public void EndStagger()
		{
			this.IsStaggered = false;
			this.Entity.RemoveBuff(BuffId.Stun);
		}

		/// <summary>
		/// Reset stagger to max amount and update the client.
		/// </summary>
		private void ResetStagger()
		{
			var maxStagger = (int)this.Entity.Properties.GetFloat(PropertyName.MShield);
			this.Entity.Properties.Modify(PropertyName.Shield, maxStagger);
			var stagger = (int)this.Entity.Properties.GetFloat(PropertyName.Shield);
			Send.ZC_UPDATE_SHIELD(this.Entity, stagger, 1);
		}

		/// <summary>
		/// Updates the entity's attack state.
		/// </summary>
		private void UpdateStaggerState()
		{
			if (!this.Entity.CanStagger())
				return;
			var maxStagger = (int)this.Entity.Properties.GetFloat(PropertyName.MShield);
			var stagger = (int)this.Entity.Properties.GetFloat(PropertyName.Shield);
			if (maxStagger <= 0 || stagger > 0)
				return;

			var timePassed = DateTime.UtcNow - this.LastStaggerTime;
			if (timePassed > StaggerCooldownDuration)
				this.ResetStagger();
		}
	}
}
