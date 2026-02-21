using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Components;
using Yggdrasil.Logging;

namespace Melia.Zone.Scripting.AI
{
	public abstract partial class AiScript
	{
		// These could be passed in or configured per AI script instance if needed
		protected virtual float DefaultSkillCastRange => 30f; // Fallback if skill range can't be determined

		public class SkillPriority
		{
			public SkillId Id { get; set; }
			public int Priority { get; set; } // Higher is better
			public Func<ICombatEntity, bool> Condition { get; set; } // e.g., target is low HP, self is buffed
			public bool IsBuff { get; set; } = false;
			public bool IsHeal { get; set; } = false;
		}
		protected List<SkillPriority> _skillRotation;
		/// <summary>
		/// Main combat routine for an AiScript-controlled entity.
		/// Manages buffs, healing (if applicable), and offensive skill usage against a target.
		/// </summary>
		public IEnumerable EngageTargetAction(
		List<SkillPriority> skillRotation,
		Func<Skill, ICombatEntity, bool> canUseSkillCheck,
		Func<ICombatEntity, Skill> selectOffensiveSkillFunc,
		Func<Skill> selectUtilitySkillFunc,
		Action<ICombatEntity> maintainCombatLogic,
		float maxChaseDistance)
		{
			if (this.EntityGone(_target))
			{
				if (this.ShowDebug)
					Log.Debug($"AiScript '{this.Entity.Name}' EngageTargetAction: Target invalid at start.");
				_target = null;
				yield break;
			}
			if (this.ShowDebug)
				Log.Debug($"AiScript '{this.Entity.Name}' EngageTargetAction: Engaging '{_target.Name}'.");
			this.SetRunning(true);
			if (this.Entity is Character selfChar && selfChar.IsSitting) selfChar.ToggleSitting();

			while (!this.EntityGone(_target)
			&& this.IsHating(_target) // Re-check hate every loop
			&& this.InRangeOf(_target, maxChaseDistance))
			{
				if (this.Entity.IsDead)
				{
					if (this.ShowDebug)
						Log.Debug($"AiScript '{this.Entity.Name}' died during EngageTargetAction.");
					_target = null; // Ensure target is cleared
					yield break;
				}

				// 0. Wait if target is knocked down (inspired by Lua's LIB_WHEN_DOWN)
				if (_target.IsKnockedDown())
				{
					yield return this.TurnTowards(_target);
					yield return this.Wait(500, 1000); // Wait for them to get up
					continue; // Re-evaluate next loop
				}

				// 1. Mid-combat maintenance (buffs, self-heals based on immediate needs)
				maintainCombatLogic?.Invoke(_target); // Call the passed-in maintain logic

				// 2. Target Validity Check (might have been affected by MaintainCombat or external events)
				if (this.EntityGone(_target) ||
				!this.IsHostileTowards(_target)) // Added IsHostileTowards check
				{
					if (this.ShowDebug)
						Log.Debug($"AiScript '{this.Entity.Name}' EngageTargetAction: Target '{_target?.Name ?? "Unknown"}' became invalid mid-loop.");
					_target = null;
					yield break;
				}

				// 3. Check if we're silenced or otherwise unable to use skills
				var cannotAttack = this.Entity.IsLocked(LockType.Attack);

				// 4. Utility Skill (Self-buffs, targeted heals on master if applicable) - Skip them
				if (!cannotAttack)
				{
					var utilitySkill = selectUtilitySkillFunc?.Invoke();
					if (utilitySkill != null)
					{
						// Determine target for utility skill (self or master, if applicable)
						var master = this.GetMaster();
						var actualUtilityTarget = this.Entity;
						if (utilitySkill.IsHeal && master != null && !master.IsDead && this.Entity.GetDistance(master) <= utilitySkill.Data.MaxRange - 1f)
						{
							actualUtilityTarget = master;
						}

						if (canUseSkillCheck(utilitySkill, actualUtilityTarget))
						{
							if (this.ShowDebug)
								Log.Debug($"AiScript '{this.Entity.Name}' using utility skill '{utilitySkill.Data.ClassName}' on '{actualUtilityTarget.Name}'.");
							yield return this.UseSkill(utilitySkill, actualUtilityTarget); // Use base AiScript.UseSkill
																						   // Add a small delay after utility skills, especially buffs, to prevent immediate re-evaluation.
							if (utilitySkill.Data.Type == SkillType.Buff)
								yield return this.Wait(utilitySkill.Data.CooldownTime + TimeSpan.FromMilliseconds(200));

							// Re-check primary target after utility, it might have changed situation
							if (this.EntityGone(_target)) break;
						}
					}
				}

				// 5. Offensive Action
				var offensiveSkill = !cannotAttack ? selectOffensiveSkillFunc?.Invoke(_target) : null;

				if (offensiveSkill == null || cannotAttack)
				{
					// If silenced or no skill available, maintain positioning and basic movement
					var fallbackRange = this.DefaultSkillCastRange;

					// Only try to get basic attack range if not silenced
					if (!cannotAttack && skillRotation != null)
					{
						var basicAttack = skillRotation.FirstOrDefault(p => p.Priority == 1)?.Id;
						if (basicAttack.HasValue && this.Entity.TryGetSkill(basicAttack.Value, out var basicSkillInstance))
						{
							fallbackRange = this.GetAttackRange(basicSkillInstance);
						}
					}

					// Always maintain movement towards target even when silenced
					if (!this.InRangeOf(_target, fallbackRange))
					{
						yield return this.MoveToAttack(_target, fallbackRange);
					}
					else
					{
						yield return this.TurnTowards(_target);

						if (cannotAttack)
						{
							Send.ZC_SKILL_DISABLE(this.Entity);

							yield return this.Wait(500, 800);
						}
						else
						{
							// Normal wait when no skill available
							yield return this.Wait(400, 700);
						}
					}

					if (this.ShowDebug)
					{
						if (cannotAttack)
							Log.Debug($"AiScript '{this.Entity.Name}' is silenced, maintaining position near '{_target.Name}'.");
						else
							Log.Debug($"AiScript '{this.Entity.Name}' no specific offensive skill for '{_target.Name}', waiting/ranging.");
					}
					continue;
				}

				var skillCastRange = this.GetAttackRange(offensiveSkill);
				if (!this.InRangeOf(_target, skillCastRange))
				{
					yield return this.MoveToAttack(_target, skillCastRange);
				}

				// Re-check target and usability after potential movement
				if (_target != null && !this.EntityGone(_target) && !_target.IsDead &&
				this.InRangeOf(_target, skillCastRange) && canUseSkillCheck(offensiveSkill, _target))
				{
					yield return this.TurnTowards(_target);
					if (this.ShowDebug)
						Log.Debug($"AiScript '{this.Entity.Name}' using skill '{offensiveSkill.Data.ClassName}' on '{_target.Name}'. Range: {skillCastRange}");
					yield return this.UseSkill(offensiveSkill, _target);
				}
				else
				{
					if (this.ShowDebug && _target != null)
						Log.Debug($"AiScript '{this.Entity.Name}' cannot use '{offensiveSkill.Data.ClassName}' on '{_target.Name}' or target out of range/invalid after move.");
					yield return this.Wait(150, 250);
				}
			} // End main combat loop

			if (this.ShowDebug)
				Log.Debug($"AiScript '{this.Entity.Name}' EngageTargetAction: Combat ended with '{_target?.Name ?? "target (now null/gone)"}'.");
			_target = null; // Ensure target is cleared on exit
		}
	}
}
