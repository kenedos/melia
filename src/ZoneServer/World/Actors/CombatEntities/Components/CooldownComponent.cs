using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Scheduling;
using Yggdrasil.Util;

namespace Melia.Zone.World.Actors.CombatEntities.Components
{
	/// <summary>
	/// Cooldown manager component.
	/// </summary>
	public class CooldownComponent : CombatEntityComponent, IUpdateable
	{
		private readonly object _syncLock = new object();
		private readonly Dictionary<CooldownId, Cooldown> _cooldowns = new Dictionary<CooldownId, Cooldown>();
		private readonly Dictionary<SkillId, TimeSpan> _cooldownReductions = new Dictionary<SkillId, TimeSpan>();
		private readonly List<Cooldown> _over = new List<Cooldown>();

		/// <summary>
		/// Creates new component.
		/// </summary>
		/// <param name="entity"></param>
		public CooldownComponent(ICombatEntity entity) : base(entity)
		{
		}

		/// <summary>
		/// Starts a cooldown with the given duration. If the cooldown
		/// is already active, it's restarted with the new duration.
		/// </summary>
		/// <param name="cooldownId"></param>
		/// <param name="duration"></param>
		public Cooldown Start(CooldownId cooldownId, TimeSpan duration)
		{
			var cooldown = new Cooldown(cooldownId, duration);

			lock (_syncLock)
			{
				if (_cooldowns.TryGetValue(cooldownId, out var existingCooldown))
				{
					cooldown = existingCooldown;
					cooldown.Change(duration);
				}
				else
				{
					_cooldowns[cooldownId] = cooldown;
				}
			}

			if (this.Entity is Character character)
				Send.ZC_COOLDOWN_CHANGED(character, cooldown);

			return cooldown;
		}

		/// <summary>
		/// Starts a cooldown for a specific skill, applying any active reductions.
		/// </summary>
		/// <param name="skill">The skill to start the cooldown for.</param>
		/// <returns>The started Cooldown instance.</returns>
		public Cooldown Start(Skill skill)
		{
			var duration = skill.Data.CooldownTime;

			// Apply any reductions specific to this skill.
			if (_cooldownReductions.TryGetValue(skill.Id, out var reduction))
			{
				duration = Math2.Max(TimeSpan.Zero, duration - reduction);
			}

			var cdrRate = this.Entity.GetTempVar("Melia.Skill.CooldownReduction");
			if (cdrRate > 0)
			{
				duration *= (1 - cdrRate);
			}

			return this.Start(skill.Data.CooldownGroup, duration);
		}


		/// <summary>
		/// Reduces an existing cooldown by the given amount.
		/// </summary>
		/// <remarks>
		/// If no cooldown is active for the given id, this method does nothing.
		/// </remarks>
		/// <param name="cooldownId"></param>
		/// <param name="reduction"></param>
		public void ReduceCooldown(CooldownId cooldownId, TimeSpan reduction)
		{
			var remaining = this.GetRemain(cooldownId);
			if (remaining == TimeSpan.Zero)
				return;

			var duration = Math2.Max(TimeSpan.Zero, remaining - reduction);

			this.Start(cooldownId, duration);
		}

		/// <summary>
		/// Adds the cooldown without updating the client. Overwrites
		/// existing cooldowns.
		/// </summary>
		/// <param name="cooldown"></param>
		public void Restore(Cooldown cooldown)
		{
			lock (_syncLock)
				_cooldowns[cooldown.Id] = cooldown;
		}

		/// <summary>
		/// Returns a list of all cooldowns.
		/// </summary>
		/// <returns></returns>
		public Cooldown[] GetAll()
		{
			lock (_syncLock)
				return _cooldowns.Values.ToArray();
		}

		/// <summary>
		/// Returns true if the given cooldown is active. Always returns false if
		/// a cooldown id of 0 is given.
		/// </summary>
		/// <param name="cooldownId"></param>
		/// <returns></returns>
		public bool IsOnCooldown(CooldownId cooldownId)
		{
			if (cooldownId == 0)
				return false;

			lock (_syncLock)
			{
				if (_cooldowns.TryGetValue(cooldownId, out var cooldown))
					return DateTime.Now < cooldown.EndTime;

				return false;
			}
		}

		/// <summary>
		/// Returns the remaining time until the given cooldown is over.
		/// Returns Zero if the cooldown is not active.
		/// </summary>
		/// <param name="cooldownId"></param>
		/// <returns></returns>
		public TimeSpan GetRemain(CooldownId cooldownId)
		{
			lock (_syncLock)
			{
				if (_cooldowns.TryGetValue(cooldownId, out var cooldown))
					return cooldown.Remaining;
			}

			return TimeSpan.Zero;
		}

		/// <summary>
		/// Remove a given cooldown id and reset it's cooldown.
		/// </summary>
		/// <param name="cooldownId"></param>
		public bool Remove(CooldownId cooldownId)
		{
			var isRemoved = false;
			lock (_syncLock)
			{
				isRemoved = _cooldowns.Remove(cooldownId);
			}

			if (this.Entity is Character character)
				Send.ZC_COOLDOWN_CHANGED(character, cooldownId);

			return isRemoved;
		}

		public void RemoveAll()
		{
			var removedCooldowns = new List<CooldownId>();
			lock (_syncLock)
			{
				foreach (var cooldown in _cooldowns.Values)
				{
					removedCooldowns.Add(cooldown.Id);
				}
				_cooldowns.Clear();
			}
			if (this.Entity is Character character)
			{
				foreach (var cooldownId in removedCooldowns)
					Send.ZC_COOLDOWN_CHANGED(character, cooldownId);
			}
		}

		/// <summary>
		/// Updates the cooldowns and removes them when they're over.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			lock (_syncLock)
			{
				_over.Clear();

				foreach (var cooldown in _cooldowns.Values)
				{
					cooldown.Update(elapsed);

					if (cooldown.Remaining == TimeSpan.Zero)
						_over.Add(cooldown);
				}

				foreach (var cooldown in _over)
				{
					_cooldowns.Remove(cooldown.Id);
					cooldown.OnCooldownChanged?.Invoke();
				}
			}
		}

		/// <summary>
		/// Applies a flat time reduction to a specific skill's cooldown.
		/// This reduction is considered when the skill's cooldown is next started.
		/// If a reduction already exists, this will overwrite it.
		/// </summary>
		/// <param name="skillId">The ID of the skill to apply the reduction to.</param>
		/// <param name="reduction">The amount of time to reduce the cooldown by.</param>
		public void AddReduction(SkillId skillId, TimeSpan reduction)
		{
			lock (_syncLock)
			{
				_cooldownReductions[skillId] = reduction;
			}
		}

		/// <summary>
		/// Removes a previously applied cooldown reduction for a specific skill.
		/// </summary>
		/// <param name="skillId">The ID of the skill to remove the reduction from.</param>
		public void ResetReduction(SkillId skillId)
		{
			lock (_syncLock)
			{
				_cooldownReductions.Remove(skillId);
			}
		}
	}

	public class Cooldown
	{
		/// <summary>
		/// Returns the cooldown's class id.
		/// </summary>
		public CooldownId Id { get; }

		/// <summary>
		/// Returns the cooldown's total duration.
		/// </summary>
		public TimeSpan Duration { get; private set; }

		/// <summary>
		/// Returns the time until the cooldown is over.
		/// </summary>
		public TimeSpan Remaining { get; private set; }

		/// <summary>
		/// Returns the time when the cooldown started.
		/// </summary>
		public DateTime StartTime { get; private set; }

		/// <summary>
		/// Returns the time when the cooldown will be over.
		/// </summary>
		public DateTime EndTime => this.StartTime + this.Duration;

		/// <summary>
		/// Returns an event on cooldown finishing.
		/// </summary>
		public Action OnCooldownChanged { get; set; }

		/// <summary>
		/// Creates new cooldown.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="duration"></param>
		public Cooldown(CooldownId id, TimeSpan duration)
			: this(id, duration, duration, DateTime.Now)
		{
		}

		/// <summary>
		/// Creates new cooldown.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="remaining"></param>
		/// <param name="duration"></param>
		/// <param name="startTime"></param>
		public Cooldown(CooldownId id, TimeSpan remaining, TimeSpan duration, DateTime startTime)
		{
			this.Id = id;
			this.Duration = duration;
			this.Remaining = remaining;
			this.StartTime = startTime;
		}

		/// <summary>
		/// Changes the cooldown's duration, effectively restarting it
		/// with the new duration.
		/// </summary>
		/// <param name="duration"></param>
		public void Change(TimeSpan duration)
		{
			this.Duration = duration;
			this.Remaining = duration;
			this.StartTime = DateTime.Now;
		}

		/// <summary>
		/// Updates the cooldown's remaining time.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			this.Remaining = Math2.Max(TimeSpan.Zero, this.Remaining - elapsed);
		}
	}
}
