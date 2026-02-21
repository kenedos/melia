using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Versioning;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Extensions;
using Yggdrasil.Scheduling;
using Yggdrasil.Util;
using Melia.Zone.Items.Effects;

namespace Melia.Zone.World.Actors.CombatEntities.Components
{
	/// <summary>
	/// Buff collection and manager for an entity.
	/// </summary>
	public class BuffComponent : CombatEntityComponent, IUpdateable
	{
		private readonly Dictionary<BuffId, Buff> _buffs = new();

		private readonly HashSet<BuffId> _noTextEffect =
		[
			BuffId.Rest,
			BuffId.SitRest,
			BuffId.DashRun,
			BuffId.RidingCompanion,
			BuffId.TakingOwner,
		];

		/// <summary>
		/// Raised when a buff starts.
		/// </summary>
		public event Action<ICombatEntity, Buff> BuffStarted;

		/// <summary>
		/// Raised when a buff ends.
		/// </summary>
		public event Action<ICombatEntity, Buff> BuffEnded;

		/// <summary>
		/// Raised when a buff is updated (e.g., stack count changes).
		/// </summary>
		public event Action<ICombatEntity, Buff> BuffUpdated;

		/// <summary>
		/// Creates new instance for character.
		/// </summary>
		/// <param name="entity"></param>
		public BuffComponent(ICombatEntity entity) : base(entity)
		{
		}

		/// <summary>
		/// Returns the amount of buffs in the collection.
		/// </summary>
		public int Count { get { lock (_buffs) return _buffs.Count; } }

		/// <summary>
		/// Adds given buff and updates the client, replaces the
		/// buff if it was already active.
		/// </summary>
		/// <param name="buff"></param>
		private void Add(Buff buff)
		{
			lock (_buffs)
				_buffs[buff.Id] = buff;

			buff.IncreaseOverbuff();
			buff.Activate(ActivationType.Start);

			Send.ZC_BUFF_ADD(this.Entity, buff);

			this.BuffStarted?.Invoke(this.Entity, buff);
		}

		/// <summary>
		/// Increases the buff's overbuff and updates the client.
		/// </summary>
		/// <param name="buff"></param>
		private void Overbuff(Buff buff)
		{
			var overbuff = buff.OverbuffCounter;
			buff.IncreaseOverbuff();

			// Start again if the overbuff counter changed. Buffs that
			// don't overbuff, such as DashRun, must not get started
			// again, because their effects would get applied over
			// and over.
			if (overbuff != buff.OverbuffCounter)
			{
				buff.Activate(ActivationType.Overbuff);
			}
			// If we don't start the buff again, we need to at least
			// extend its duration. Otherwise it may end before the
			// time displayed by the client.
			else
			{
				buff.Extend();
			}

			Send.ZC_BUFF_UPDATE(this.Entity, buff);

			// Fire event to update properties on client (same as Add does)
			this.BuffStarted?.Invoke(this.Entity, buff);
		}

		/// <summary>
		/// Adds and activates given buffs. If a buff already exists,
		/// it gets overbuffed.
		/// </summary>
		/// <param name="buffs"></param>
		public void AddOrUpdate(params Buff[] buffs)
		{
			foreach (var addBuff in buffs)
			{
				if (!this.TryGet(addBuff.Id, out var buff))
					this.Add(addBuff);
				else
					this.Overbuff(buff);
			}
		}

		/// <summary>
		/// Adds buff without starting it again or updating the client.
		/// Use for restoring saved buffs on load.
		/// </summary>
		/// <param name="buff"></param>
		public void Restore(Buff buff)
		{
			lock (_buffs)
				_buffs[buff.Id] = buff;

			// Apply stored modifiers from buff Vars to entity properties.
			// This is needed because buff modifier properties are not persisted
			// to avoid desync issues between property and buff state.
			this.RestoreModifiersFromVars(buff);
		}

		/// <summary>
		/// Restores property modifiers from buff's stored Vars.
		/// Called after loading a buff from database to reapply its effects.
		/// </summary>
		/// <param name="buff"></param>
		private void RestoreModifiersFromVars(Buff buff)
		{
			foreach (var entry in buff.Vars.GetList())
			{
				if (entry.Key.StartsWith(BuffHandler.ModifierVarPrefix) && entry.Value is float value)
				{
					var propertyName = entry.Key.Substring(BuffHandler.ModifierVarPrefix.Length);
					buff.Target.Properties.Modify(propertyName, value);
				}
			}
		}

		/// <summary>
		/// Removes buff, returns false if it didn't exist. 
		/// Updates the client on success.
		/// </summary>
		/// <param name="buff"></param>
		/// <returns></returns>
		private bool Remove(Buff buff, bool silently = false)
		{
			lock (_buffs)
			{
				if (!_buffs.Remove(buff.Id))
					return false;
			}

			// Need to do this before buff.End because this method checks
			// for variables inside the buff, which are removed when the
			// buff ends.
			var affectsSpeed = buff.AffectsMovementSpeed();

			buff.End();

			this.BuffEnded?.Invoke(this.Entity, buff);

			if (!silently)
			{
				Send.ZC_BUFF_REMOVE(this.Entity, buff);
				Send.ZC_MSPD(this.Entity);
			}

			return true;
		}

		/// <summary>
		/// Removes buff with given id, returns false if it
		/// didn't exist. Updates the client on success.
		/// </summary>
		/// <param name="buffId"></param>
		/// <returns></returns>
		public bool Remove(BuffId buffId)
		{
			if (!this.TryGet(buffId, out var buff))
				return false;

			return this.Remove(buff);
		}

		/// <summary>
		/// Removes buff with given id, returns false if it
		/// didn't exist. Updates the client on success.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public bool Remove(Func<Buff, bool> predicate)
		{
			var removableBuffs = this.GetAll(predicate);
			if (removableBuffs.Count == 0)
				return false;

			this.RemoveAll(predicate);
			return true;
		}

		/// <summary>
		/// Removes buff with given tags.
		/// </summary>
		public void RemoveBuff(params string[] buffTag)
		{
			var removableBuffs = this.GetAll(a => a.Data.Tags.HasAny(buffTag));
			if (removableBuffs.Count == 0)
				return;

			foreach (var buff in removableBuffs)
				this.Remove(buff);
		}

		/// <summary>
		/// Removes a random removable buff. Returns the id of the buff that was
		/// removed, or 0 if no buff was removed.
		/// </summary>
		/// <remarks>
		/// Only considers buffs of type Buff, not Debuff, that are removable by
		/// skills according to the buffs' data.
		/// </remarks>
		/// <returns></returns>
		public BuffId RemoveRandomBuff()
		{
			var removableBuffs = this.GetAll(a => a.Data.Type == BuffType.Buff && a.Data.RemoveBySkill);
			if (removableBuffs.Count == 0)
				return 0;

			var buff = removableBuffs.Random();
			this.Remove(buff);

			return buff.Id;
		}

		/// <summary>
		/// Removes a random removable debuff. Returns the id of the buff that was
		/// removed, or 0 if no buff was removed.
		/// </summary>
		/// <remarks>
		/// Only considers buffs of type Debuff, not Buff, that are removable by
		/// skills according to the buffs' data.
		/// </remarks>
		/// <returns></returns>
		public BuffId RemoveRandomDebuff()
		{
			var removableDeBuffs = this.GetAll(a => a.Data.Type == BuffType.Debuff && a.Data.RemoveBySkill);
			if (removableDeBuffs.Count == 0)
				return 0;

			var buff = removableDeBuffs.Random();
			this.Remove(buff);

			return buff.Id;
		}

		/// <summary>
		/// Stops and removes all active buffs.
		/// </summary>
		public void RemoveAll()
		{
			var buffs = this.GetList();
			foreach (var buff in buffs)
				this.Remove(buff, true);
			Send.ZC_BUFF_CLEAR(this.Entity);
		}

		/// <summary>
		/// Removes all buffs that match given predicate.
		/// </summary>
		/// <param name="predicate"></param>
		public void RemoveAll(Func<Buff, bool> predicate)
		{
			var buffs = this.GetList();
			foreach (var buff in buffs.Where(buff => predicate(buff)))
			{
				this.Remove(buff);
			}
		}

		/// <summary>
		/// Returns buff with given id, or null if it didn't
		/// exist.
		/// </summary>
		/// <param name="buffId"></param>
		/// <returns></returns>
		public Buff Get(BuffId buffId)
		{
			lock (_buffs)
			{
				_buffs.TryGetValue(buffId, out var result);
				return result;
			}
		}

		/// <summary>
		/// Returns buff with given id via out, returns false if the
		/// buff wasn't found.
		/// </summary>
		/// <param name="buffId"></param>
		/// <param name="buff"></param>
		/// <returns></returns>
		public bool TryGet(BuffId buffId, out Buff buff)
		{
			buff = this.Get(buffId);
			return buff != null;
		}

		/// <summary>
		/// Returns buff with given class name, or null if it didn't
		/// exist.
		/// </summary>
		/// <param name="buffClassName"></param>
		/// <returns></returns>
		public Buff Get(string buffClassName)
		{
			lock (_buffs)
				return _buffs.Values.FirstOrDefault(a => a.Data.ClassName == buffClassName);
		}

		/// <summary>
		/// Returns a list with all buffs.
		/// </summary>
		/// <returns></returns>
		public List<Buff> GetList()
		{
			lock (_buffs)
				return _buffs.Values.ToList();
		}

		/// <summary>
		/// Returns a list of all active buffs that match the given predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public bool Exists(Func<Buff, bool> predicate)
		{
			lock (_buffs)
				return _buffs.Values.Any(predicate);
		}

		/// <summary>
		/// Returns a list of all active buffs that match the given predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public List<Buff> GetAll(Func<Buff, bool> predicate)
		{
			lock (_buffs)
				return _buffs.Values.Where(predicate).ToList();
		}

		/// <summary>
		/// Returns the number of active buffs that match the given predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public int CountActive(Func<Buff, bool> predicate)
		{
			lock (_buffs)
				return _buffs.Values.Count(predicate);
		}

		/// <summary>
		/// Returns true if any buff exists.
		/// </summary>
		/// <param name="buffIds"></param>
		/// <returns></returns>
		public bool HasAny(params BuffId[] buffIds)
		{
			lock (_buffs)
			{
				return Array.Exists(buffIds, _buffs.ContainsKey);
			}
		}

		/// <summary>
		/// Returns true if the buff exists.
		/// </summary>
		/// <param name="buffId"></param>
		/// <returns></returns>
		public bool Has(BuffId buffId)
		{
			lock (_buffs)
				return _buffs.ContainsKey(buffId);
		}

		/// <summary>
		/// Returns the overbuff counter for the given buff. Returns 0
		/// if the buff isn't active.
		/// </summary>
		/// <param name="buffId"></param>
		/// <returns></returns>
		public int GetOverbuffCount(BuffId buffId)
		{
			if (!this.TryGet(buffId, out var buff))
				return 0;

			return buff.OverbuffCounter;
		}

		/// <summary>
		/// Starts the buff with the given name or overbuffs it if it's
		/// already active. Returns the new or modified buff.
		/// </summary>
		/// <param name="buffClassName"></param>
		/// <param name="numArg1"></param>
		/// <param name="numArg2"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">
		/// Thrown if the buff doesn't exist in the data.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Thrown if the buff doesn't have a handler.
		/// </exception>
		public Buff Start(string buffClassName, float numArg1, float numArg2, TimeSpan duration, ICombatEntity caster)
		{
			if (!ZoneServer.Instance.Data.BuffDb.TryFind(a => a.ClassName == buffClassName, out var buffData))
				throw new ArgumentException($"Buff with class name '{buffClassName}' not found.");

			// I'm split on whether we should let buffs run without a
			// handler, but this method will primarily used from item
			// scripts, and we don't want items to be used when they
			// start a buff that isn't implemented.
			if (!ZoneServer.Instance.BuffHandlers.Has(buffData.Id))
				throw new BuffNotImplementedException(buffData.Id);

			return this.Start(buffData.Id, numArg1, numArg2, duration, caster);
		}

		/// <summary>
		/// Starts the buff with the given id, returns the created buff.
		/// If the buff was already active, it gets overbuffed.
		/// </summary>
		/// <remarks>
		/// Uses the duration from the buff's data by default.
		/// </remarks>
		/// <param name="buffId"></param>
		/// <returns></returns>
		public Buff Start(BuffId buffId)
			=> this.Start(buffId, TimeSpan.MinValue);

		/// <summary>
		/// Starts the buff with the given id. If the buff is already active,
		/// it gets overbuffed. Returns the created or modified buff.
		/// </summary>
		/// <param name="buffId"></param>
		/// <param name="duration">Custom duration of the buff.</param>
		/// <returns></returns>
		public Buff Start(BuffId buffId, TimeSpan duration)
			=> this.Start(buffId, 0, 0, duration, this.Entity);

		/// <summary>
		/// Starts the buff with the given id. If the buff is already active,
		/// it gets overbuffed. Returns the created or modified buff. May
		/// return null if the buff was resisted for some reason.
		/// </summary>
		/// <remarks>
		/// May return null if buff cannot be applied
		/// </remarks>
		/// <param name="buffId"></param>
		/// <param name="numArg1"></param>
		/// <param name="numArg2"></param>
		/// <param name="duration">Custom duration of the buff.</param>
		/// <param name="caster">The entity that casted the buff.</param>
		/// <param name="skillId">The id of the skill associated with the buff.</param>
		/// <returns></returns>
		public Buff Start(BuffId buffId, float numArg1, float numArg2, TimeSpan duration, IActor caster, SkillId skillId = SkillId.Normal_Attack)
			=> this.Start(buffId, numArg1, numArg2, duration, caster, skillId, null);

		/// <summary>
		/// Starts the buff with the given id. If the buff is already active,
		/// it gets overbuffed. Returns the created or modified buff. May
		/// return null if the buff was resisted for some reason.
		/// </summary>
		/// <remarks>
		/// May return null if buff cannot be applied
		/// </remarks>
		/// <param name="buffId"></param>
		/// <param name="numArg1"></param>
		/// <param name="numArg2"></param>
		/// <param name="duration">Custom duration of the buff.</param>
		/// <param name="caster">The entity that casted the buff.</param>
		/// <param name="skillId">The id of the skill associated with the buff.</param>
		/// <param name="initializer">Optional action to initialize buff variables before activation.</param>
		/// <returns></returns>
		public Buff Start(BuffId buffId, float numArg1, float numArg2, TimeSpan duration, IActor caster, SkillId skillId, Action<Buff> initializer)
		{
			if (Versions.Protocol < 200 && !ZoneServer.Instance.Data.BuffDb.TryFind(a => a.Id == buffId, out var _))
				return null;

			if (this.TryResistDebuff(buffId, caster))
				return null;

			if (!ZoneServer.Instance.Data.BuffDb.TryFind(a => a.Id == buffId, out var buffData))
				throw new ArgumentException($"Buff Id '{buffId}' not found.");

			Buff buff;
			lock (_buffs)
			{
				if (!_buffs.TryGetValue(buffId, out buff))
				{
					buff = new Buff(buffId, numArg1, numArg2, duration, TimeSpan.Zero, this.Entity, caster ?? this.Entity, skillId);
					_buffs[buff.Id] = buff;
					buff.IncreaseOverbuff();
					initializer?.Invoke(buff);
					buff.Activate(ActivationType.Start);
				}
				else
				{
					var overbuff = buff.OverbuffCounter;
					buff.IncreaseOverbuff();

					// Store the new numArg2 value so handlers can access it
					// (e.g., for DoT stacking where damage should be added)
					buff.NumArg2 = numArg2;

					initializer?.Invoke(buff);

					if (overbuff != buff.OverbuffCounter)
					{
						buff.Activate(ActivationType.Overbuff);
					}
					else
					{
						buff.Extend();
					}
				}
			}

			// Send packets after releasing the lock to avoid potential deadlocks
			if (buff != null)
			{
				if (!_buffs.ContainsKey(buffId) || buff.OverbuffCounter == 1)
				{
					if (!_noTextEffect.Contains(buffId))
						Send.ZC_NORMAL.PlayTextEffect(this.Entity, caster, "SHOW_BUFF_TEXT", (int)buffId, "");
					Send.ZC_BUFF_ADD(this.Entity, buff);
				}
				else
				{
					Send.ZC_BUFF_UPDATE(this.Entity, buff);
				}

				this.BuffStarted?.Invoke(this.Entity, buff);
			}

			return buff;
		}

		/// <summary>
		/// Returns true if the caster should resist the given buff,
		/// based on its current state and other active buffs.
		/// </summary>
		/// <param name="buffId"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private bool TryResistDebuff(BuffId buffId, IActor caster)
		{
			// TODO: Ideally, this should happen from the buff handler,
			//   and we might also want to move the check somewhere else,
			//   so we're still able to force-apply buffs if necessary.

			var selfBuff = caster == this.Entity;
			if (selfBuff)
				return false;

			var isDebuff = ZoneServer.Instance.Data.BuffDb.TryFind(buffId, out var buffData) && buffData.Type == BuffType.Debuff;
			if (!isDebuff)
				return false;

			if (this.Has(BuffId.Skill_MomentaryImmune_Buff))
				return true;

			if (this.Has(BuffId.Rampage_Buff) && buffData.Removable)
				return true;

			if (this.Has(BuffId.Cure_Buff))
				return true;

			// Cannot apply debuffs to bosses when they have shield
			if (this.Entity is Mob mob && mob.Rank == MonsterRank.Boss && mob.Shield > 0)
				return true;

			if (this.TryGet(BuffId.Cyclone_Buff_ImmuneAbil, out var cycloneImmuneBuff)
				&& RandomProvider.Get().Next(100) < cycloneImmuneBuff.NumArg1 * 15)
				return true;

			if (this.TryGet(BuffId.Ausirine_Buff, out var ausirineBuff))
			{
				var skillLevel = ausirineBuff.NumArg1;
				var resistanceChance = 30 + (3 * skillLevel);
				if (RandomProvider.Get().Next(100) < resistanceChance)
					return true;
			}

			// Check card/item debuff resistance
			if (this.Entity is Characters.Character character)
			{
				var resistRate = ItemHookRegistry.Instance.GetDebuffResistance(character, buffId);
				if (resistRate > 0f)
				{
					var roll = RandomProvider.Get().NextDouble();
					if (roll < resistRate)
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Stops the buff with the given id.
		/// </summary>
		/// <param name="buffIds"></param>
		public void Stop(params BuffId[] buffIds)
		{
			foreach (var buffId in buffIds)
				if (this.TryGet(buffId, out var buff))
					this.Remove(buff);
		}

		/// <summary>
		/// Check buffs and remove expired buffs
		/// </summary>
		public void Update(TimeSpan elapsed)
		{
			List<Buff> toUpdate = null;
			List<Buff> toRemove = null;
			var now = DateTime.Now;

			lock (_buffs)
			{
				foreach (var buff in _buffs.Values)
				{
					if (buff.HasUpdateTime)
					{
						toUpdate ??= new List<Buff>();
						toUpdate.Add(buff);
					}
					if (buff.HasDuration && now >= buff.RemovalTime)
					{
						toRemove ??= new List<Buff>();
						toRemove.Add(buff);
					}
					if (buff.Target.IsDead && buff.Data.RemoveOnDeath)
					{
						toRemove ??= new List<Buff>();
						toRemove.Add(buff);
					}
				}
			}

			if (toUpdate != null)
			{
				foreach (var buff in toUpdate)
				{
					if (this.Has(buff.Id))
						buff.Update(elapsed);
				}
			}

			if (toRemove != null)
			{
				foreach (var buff in toRemove)
					this.Remove(buff);
			}
		}

		/// <summary>
		/// Removes buffs that aren't saved on disconnect or map change.
		/// </summary>
		public void StopTempBuffs()
		{
			List<Buff> toRemove = null;

			lock (_buffs)
			{
				foreach (var buff in _buffs.Values)
				{
					if (!buff.Data.Save)
					{
						if (toRemove == null)
							toRemove = new List<Buff>();

						toRemove.Add(buff);
					}
				}
			}

			if (toRemove != null)
			{
				foreach (var buff in toRemove)
					this.Remove(buff);
			}
		}

		/// <summary>
		/// Notifies that a buff has been updated (e.g., stack count changed).
		/// Sends update packet and raises BuffUpdated event.
		/// </summary>
		/// <param name="buff"></param>
		public void NotifyBuffUpdate(Buff buff)
		{
			Send.ZC_BUFF_UPDATE(this.Entity, buff);

			// Fire event to update properties on client
			this.BuffUpdated?.Invoke(this.Entity, buff);
		}
	}

	/// <summary>
	/// Exception for when a buff handler is not implemented.
	/// </summary>
	public class BuffNotImplementedException : Exception
	{
		/// <summary>
		/// Returns the id of the buff that wasn't implemented.
		/// </summary>
		public BuffId BuffId { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="buffId"></param>
		public BuffNotImplementedException(BuffId buffId) : base($"Buff handler for '{buffId}' not implemented.")
		{
			this.BuffId = buffId;
		}
	}
}
