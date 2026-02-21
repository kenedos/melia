using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Base
{
	/// <summary>
	/// Base class for damage-over-time (DoT) buffs/debuffs that snapshot damage
	/// when applied, preventing the weird behavior of blocking/dodging DoT ticks.
	/// </summary>
	/// <remarks>
	/// This class ensures that damage is calculated ONCE when the buff is applied
	/// (based on the attacker's stats at that moment) and then that same damage
	/// is applied consistently throughout the buff duration.
	///
	/// When the same DoT buff is reapplied (overbuff/extend), a NEW damage instance
	/// is created with its own lifetime. The tick damage is the sum of all active
	/// instances. Each instance expires independently based on the buff duration
	/// at the time it was applied.
	///
	/// Example:
	/// - Second 0: Apply 500 bleed damage for 4 seconds
	/// - Second 1: 500 damage tick
	/// - Second 2: 500 damage tick
	/// - Second 3: 500 damage tick, apply another 250 bleed (refreshes buff to 4s)
	/// - Second 4: 750 damage tick (500 from first + 250 from second)
	/// - Second 5: 250 damage tick (first instance expired)
	/// - Second 6: 250 damage tick
	/// - Second 7: 250 damage tick (second instance expires, buff ends)
	///
	/// Usage:
	/// - Inherit from this class for poison, bleed, burn, and similar DoT effects
	/// - The damage will be read from buff.NumArg2 if provided, otherwise calculated
	/// - Override GetSnapshotDamage() if you need custom damage calculation
	/// - Override GetHitType() if you need a specific hit type (Poison, Bleed, etc.)
	/// - Override GetSkillId() if the buff uses a different skill ID than buff.SkillId
	/// </remarks>
	public abstract class DamageOverTimeBuffHandler : BuffHandler
	{
		private const string DamageInstancesVarName = "Melia.DoT.DamageInstances";
		private const string InstanceAddedFlagName = "Melia.DoT.InstanceAddedThisCycle";

		/// <summary>
		/// Represents a single damage instance with its own expiration time.
		/// </summary>
		private class DamageInstance
		{
			public float Damage { get; set; }
			public DateTime ExpirationTime { get; set; }
		}

		/// <summary>
		/// Adds a new damage instance when the buff is activated.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="activationType"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			this.AddDamageInstance(buff);

			// Mark that we added an instance this cycle to prevent
			// OnExtend from adding it again
			buff.Vars.SetInt(InstanceAddedFlagName, 1);
		}

		/// <summary>
		/// Called when the buff is extended (when max overbuff is reached).
		/// Adds a new damage instance only if OnActivate wasn't called.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnExtend(Buff buff)
		{
			// Check if we already added an instance in OnActivate this cycle
			if (buff.Vars.TryGetInt(InstanceAddedFlagName, out var added) && added == 1)
			{
				// Clear the flag for the next cycle
				buff.Vars.SetInt(InstanceAddedFlagName, 0);
				return;
			}

			// OnActivate wasn't called (max overbuff reached), so add the instance here
			this.AddDamageInstance(buff);
		}

		/// <summary>
		/// Adds a new damage instance to the buff's instance list.
		/// </summary>
		/// <param name="buff"></param>
		private void AddDamageInstance(Buff buff)
		{
			float newDamage;
			if (buff.NumArg2 > 0)
			{
				newDamage = buff.NumArg2;
			}
			else
			{
				newDamage = this.GetSnapshotDamage(buff);
			}

			if (newDamage <= 0)
				return;

			var instances = this.GetDamageInstances(buff);

			var instance = new DamageInstance
			{
				Damage = newDamage,
				ExpirationTime = DateTime.Now.Add(buff.Duration)
			};

			instances.Add(instance);
		}

		/// <summary>
		/// Gets the list of damage instances from the buff's variables.
		/// Creates a new list if one doesn't exist.
		/// </summary>
		/// <param name="buff"></param>
		/// <returns></returns>
		private List<DamageInstance> GetDamageInstances(Buff buff)
		{
			if (!buff.Vars.TryGet<List<DamageInstance>>(DamageInstancesVarName, out var instances))
			{
				instances = new List<DamageInstance>();
				buff.Vars.Set(DamageInstancesVarName, instances);
			}

			return instances;
		}

		/// <summary>
		/// Applies the total damage from all active instances to the target each tick.
		/// Removes expired instances. If target is linked via Joint Penalty or Physical Link,
		/// damage is shared among all linked targets.
		/// </summary>
		/// <param name="buff"></param>
		public override void WhileActive(Buff buff)
		{
			if (buff.Target.IsDead)
				return;

			var attacker = buff.Caster;
			var target = buff.Target;
			var now = DateTime.Now;

			var instances = this.GetDamageInstances(buff);

			// Remove expired instances and calculate total damage
			float totalDamage = 0;
			for (var i = instances.Count - 1; i >= 0; i--)
			{
				var instance = instances[i];
				if (now >= instance.ExpirationTime)
				{
					instances.RemoveAt(i);
				}
				else
				{
					totalDamage += instance.Damage;
				}
			}

			if (totalDamage <= 0)
				return;

			// Check if target is linked via Joint Penalty (enemy link) and share damage
			var enemyLinkedTargets = this.GetEnemyLinkedTargets(buff, attacker, target);
			if (enemyLinkedTargets != null && enemyLinkedTargets.Count >= 2)
			{
				this.ApplyEnemyLinkDamage(buff, attacker, target, totalDamage, enemyLinkedTargets);
				this.OnDamageTick(buff);
				return;
			}

			// Check if target is linked via Physical Link (party link) and share damage
			var partyLinkedTargets = this.GetPartyLinkedTargets(buff, target);
			if (partyLinkedTargets != null && partyLinkedTargets.Count >= 2)
			{
				this.ApplyPartyLinkDamage(buff, attacker, target, totalDamage, partyLinkedTargets);
				this.OnDamageTick(buff);
				return;
			}

			// No link, apply full damage to target only
			target.TakeSimpleHit(totalDamage, attacker, this.GetSkillId(buff), this.GetHitType(buff));

			// Allow derived classes to perform additional actions on tick
			this.OnDamageTick(buff);
		}

		/// <summary>
		/// Gets all valid enemy linked targets via Joint Penalty (Link_Enemy).
		/// Returns null if target is not linked.
		/// </summary>
		private List<ICombatEntity> GetEnemyLinkedTargets(Buff buff, IActor attacker, ICombatEntity target)
		{
			if (attacker is not ICombatEntity casterEntity)
				return null;

			if (!target.TryGetBuff(BuffId.Link_Enemy, out var linkBuff))
				return null;

			if (!linkBuff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles))
				return null;

			var linkTargets = new List<ICombatEntity>();
			if (target.Map != null)
			{
				foreach (var handle in memberHandles)
				{
					if (target.Map.TryGetCombatEntity(handle, out var member))
					{
						if (!member.IsDead && member.IsBuffActive(BuffId.Link_Enemy) && casterEntity.IsEnemy(member))
							linkTargets.Add(member);
					}
				}
			}

			return linkTargets;
		}

		/// <summary>
		/// Gets all valid party linked targets via Physical Link (Link_Physical).
		/// Returns null if target is not linked.
		/// </summary>
		private List<ICombatEntity> GetPartyLinkedTargets(Buff buff, ICombatEntity target)
		{
			if (!target.TryGetBuff(BuffId.Link_Physical, out var linkBuff))
				return null;

			if (!linkBuff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles))
				return null;

			var linkTargets = new List<ICombatEntity>();
			if (target.Map != null)
			{
				foreach (var handle in memberHandles)
				{
					if (target.Map.TryGetCombatEntity(handle, out var member))
					{
						if (!member.IsDead && member.IsBuffActive(BuffId.Link_Physical))
							linkTargets.Add(member);
					}
				}
			}

			return linkTargets;
		}

		/// <summary>
		/// Applies DoT damage to enemy linked targets (Joint Penalty),
		/// modified by Joint Penalty's damage multiplier.
		/// </summary>
		private void ApplyEnemyLinkDamage(Buff buff, IActor attacker, ICombatEntity target, float damage, List<ICombatEntity> linkTargets)
		{
			if (!target.TryGetBuff(BuffId.Link_Enemy, out var linkBuff))
				return;

			// Calculate damage multiplier based on Joint Penalty skill level and ability
			var skillLevel = linkBuff.NumArg1;
			var abilityMultiplier = 1f;

			if (linkBuff.Caster is ICombatEntity linkCaster && linkCaster.TryGetActiveAbilityLevel(AbilityId.Linker22, out var abilityLevel))
				abilityMultiplier = 1f + (abilityLevel * 0.005f);

			var damageMultiplier = 1f + (-0.30f) + (0.03f * skillLevel * abilityMultiplier);

			// Apply damage multiplier then divide equally among all linked targets
			var dividedDamage = (damage * damageMultiplier);

			// Apply divided damage to ALL linked targets (including original target)
			foreach (var linkTarget in linkTargets)
			{
				linkTarget.TakeSimpleHit(dividedDamage, attacker, this.GetSkillId(buff), this.GetHitType(buff));
			}
		}

		/// <summary>
		/// Applies DoT damage to party linked targets (Physical Link),
		/// with damage multiplier based on skill level and Unbind passive.
		/// Damage multiplier: 130% base, -3% per skill level, minimum 90%.
		/// Unbind redistributes damage based on current HP ratios.
		/// </summary>
		private void ApplyPartyLinkDamage(Buff buff, IActor attacker, ICombatEntity target, float damage, List<ICombatEntity> linkTargets)
		{
			if (!target.TryGetBuff(BuffId.Link_Physical, out var linkBuff))
				return;

			// Calculate damage multiplier: 130% base, -3% per skill level, minimum 90%
			var skillLevel = linkBuff.NumArg1;
			var damageMultiplier = 1.30f - (0.03f * skillLevel);
			damageMultiplier = Math.Max(damageMultiplier, 0.90f);

			var totalDamage = damage * damageMultiplier;

			// Check for Unbind skill level on the link caster
			var unbindLevel = 0;
			if (linkBuff.Caster is Character linkCaster && linkCaster.TryGetSkill(SkillId.Linker_Unbind, out var unbindSkill))
				unbindLevel = unbindSkill.Level;

			if (unbindLevel > 0)
			{
				// Apply Unbind HP-based damage distribution
				this.ApplyUnbindDamageDistribution(buff, attacker, totalDamage, linkTargets, unbindLevel);
			}
			else
			{
				// Equal damage distribution
				var dividedDamage = totalDamage / linkTargets.Count;
				foreach (var linkTarget in linkTargets)
				{
					linkTarget.TakeSimpleHit(dividedDamage, attacker, this.GetSkillId(buff), this.GetHitType(buff));
				}
			}
		}

		/// <summary>
		/// Applies damage with Unbind's HP-based protection for Physical Link.
		/// Lower HP party members take less shared damage.
		/// At Unbind Level 0: Equal distribution
		/// At Unbind Level 10: Pure HP-based (damage proportional to current HP)
		/// Levels 1-9: Linear interpolation between equal and HP-based
		/// </summary>
		private void ApplyUnbindDamageDistribution(Buff buff, IActor attacker, float totalDamage, List<ICombatEntity> linkTargets, int unbindLevel)
		{
			var totalCurrentHp = linkTargets.Sum(t => t.Properties.GetFloat(PropertyName.HP));
			if (totalCurrentHp <= 0)
				return;

			var memberCount = linkTargets.Count;
			if (memberCount <= 0)
				return;

			// unbindFactor: 0 at level 0, 1.0 at level 15
			var unbindFactor = Math.Min(unbindLevel / 15f, 1f);
			var equalShare = 1f / memberCount;

			foreach (var linkTarget in linkTargets)
			{
				var currentHp = linkTarget.Properties.GetFloat(PropertyName.HP);
				var hpShare = currentHp / totalCurrentHp;

				// Interpolate between equal distribution and HP-based distribution
				var finalShare = (equalShare * (1f - unbindFactor)) + (hpShare * unbindFactor);
				var damage = totalDamage * finalShare;

				if (damage > 0)
				{
					linkTarget.TakeSimpleHit(damage, attacker, this.GetSkillId(buff), this.GetHitType(buff));
				}
			}
		}

		/// <summary>
		/// Calculates the damage to snapshot when the buff is applied.
		/// Override this to provide custom damage calculation logic.
		/// </summary>
		/// <param name="buff"></param>
		/// <returns>The damage value to snapshot</returns>
		protected virtual float GetSnapshotDamage(Buff buff)
		{
			// Default behavior: Use SCR_SkillHit to calculate initial damage
			if (buff.Caster is not ICombatEntity caster)
				return 0;

			if (!caster.TryGetSkill(buff.SkillId, out var skill))
				return 0;

			var target = buff.Target;
			var skillHitResult = SCR_SkillHit(caster, target, skill);

			return skillHitResult.Damage;
		}

		/// <summary>
		/// Returns the skill ID to use for the damage tick.
		/// Override this if the buff should use a different skill ID.
		/// </summary>
		/// <param name="buff"></param>
		/// <returns>The skill ID for damage display</returns>
		protected virtual SkillId GetSkillId(Buff buff)
		{
			return buff.SkillId;
		}

		/// <summary>
		/// Returns the hit type for the damage tick (Normal, Poison, Bleed, Fire, etc.).
		/// Override this to specify the damage type.
		/// </summary>
		/// <param name="buff"></param>
		/// <returns>The hit type for visual effects</returns>
		protected virtual HitType GetHitType(Buff buff)
		{
			return HitType.Normal;
		}

		/// <summary>
		/// Called after each damage tick. Override this to perform additional
		/// actions when damage is applied (e.g., spread mechanics, additional debuffs).
		/// </summary>
		/// <param name="buff"></param>
		protected virtual void OnDamageTick(Buff buff)
		{
			// Default: do nothing
		}
	}
}
