using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Linker
{
	/// <summary>
	/// Handler for the Link_Physical buff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Link_Physical)]
	public class Link_PhysicalOverride : BuffHandler, IBuffCombatDefenseAfterCalcHandler
	{
		private const float BaseDamageMultiplier = 1.30f;
		private const float DamageReductionPerSkillLevel = 0.03f;
		private const float MinDamageMultiplier = 0.90f;
		private const float MaxHorizontalDistance = 250f;
		private const int DistanceCheckIntervalMs = 500;
		private const float LowHpThreshold = 0.25f;

		/// <summary>
		/// Called when the buff is activated. Stores the initial map ID and sets up update interval.
		/// </summary>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(DistanceCheckIntervalMs);

			// Store the map ID when buff starts for map change detection
			if (buff.Target.Map != null)
				buff.Vars.Set("Melia.Link.MapId", buff.Target.Map.Id);
		}

		/// <summary>
		/// Applies the buff's effect during the combat calculations.
		/// Physical link shares damage to all linked party members.
		/// </summary>
		public void OnDefenseAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles))
				return;

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

			if (linkTargets.Count < 2)
				return;

			var skillLevel = buff.NumArg1;
			var damageMultiplier = this.CalculateDamageMultiplier(skillLevel);

			var totalDamage = skillHitResult.Damage * damageMultiplier;

			var unbindLevel = this.GetUnbindSkillLevel(buff);
			float targetShare;
			if (unbindLevel > 0)
			{
				targetShare = this.ApplyUnbindDamageDistribution(buff, attacker, target, skill, totalDamage, linkTargets, unbindLevel);
			}
			else
			{
				targetShare = this.ApplyEqualDamageDistribution(attacker, target, skill, totalDamage, linkTargets);
			}

			skillHitResult.Damage = targetShare;
		}

		/// <summary>
		/// Calculates the damage multiplier based on skill level.
		/// Starts at 130% and decreases by 3% per level, minimum 90%.
		/// </summary>
		private float CalculateDamageMultiplier(float skillLevel)
		{
			var multiplier = BaseDamageMultiplier - (DamageReductionPerSkillLevel * skillLevel);
			return Math.Max(multiplier, MinDamageMultiplier);
		}

		/// <summary>
		/// Gets the Unbind skill level from the link caster.
		/// </summary>
		private int GetUnbindSkillLevel(Buff buff)
		{
			if (buff.Caster is Character caster && caster.TryGetSkill(SkillId.Linker_Unbind, out var unbindSkill))
				return unbindSkill.Level;

			return 0;
		}

		/// <summary>
		/// Applies damage distribution with Unbind's HP-based protection.
		/// Returns the original target's share of damage.
		/// </summary>
		private float ApplyUnbindDamageDistribution(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, float totalDamage, List<ICombatEntity> linkTargets, int unbindLevel)
		{
			var totalCurrentHp = linkTargets.Sum(t => t.Properties.GetFloat(PropertyName.HP));
			if (totalCurrentHp <= 0)
				return 0;

			var memberCount = linkTargets.Count;
			if (memberCount <= 0)
				return 0;

			var unbindFactor = Math.Min(unbindLevel / 15f, 1f);
			var equalShare = 1f / memberCount;
			var targetShare = 0f;

			foreach (var linkTarget in linkTargets)
			{
				var currentHp = linkTarget.Properties.GetFloat(PropertyName.HP);
				var hpShare = currentHp / totalCurrentHp;

				var finalShare = (equalShare * (1f - unbindFactor)) + (hpShare * unbindFactor);
				var damage = totalDamage * finalShare;

				if (damage > 0)
				{
					if (linkTarget.Handle == target.Handle)
					{
						targetShare = damage;
					}
					else
					{
						linkTarget.TakeDamage(damage, attacker);

						var hitInfo = new HitInfo(attacker, linkTarget, skill, damage, HitResultType.Hit);
						Send.ZC_HIT_INFO(attacker, linkTarget, hitInfo);
					}
				}
			}

			return targetShare;
		}

		/// <summary>
		/// Applies equal damage distribution when Unbind is not active.
		/// Returns the original target's share of damage.
		/// </summary>
		private float ApplyEqualDamageDistribution(ICombatEntity attacker, ICombatEntity target, Skill skill, float totalDamage, List<ICombatEntity> linkTargets)
		{
			var dividedDamage = totalDamage / linkTargets.Count;
			var targetShare = dividedDamage;

			foreach (var linkTarget in linkTargets)
			{
				if (linkTarget.Handle != target.Handle)
				{
					linkTarget.TakeSimpleHit(dividedDamage, attacker, skill.Id);
				}
			}

			return targetShare;
		}

		/// <summary>
		/// Periodically checks for chain breaking conditions.
		/// </summary>
		public override void WhileActive(Buff buff)
		{
			var isCaster = buff.Vars.GetBool("Melia.Link.IsCaster");

			// Check if this is the caster's buff
			if (isCaster)
			{
				// Caster dead → break ALL chains
				if (buff.Target.IsDead)
				{
					this.RemoveAllChains(buff);
					return;
				}

				// Caster changed map → break ALL chains
				var storedMapId = buff.Vars.GetInt("Melia.Link.MapId", 0);
				if (buff.Target.Map == null || buff.Target.Map.Id != storedMapId)
				{
					this.RemoveAllChains(buff);
					return;
				}
			}
			else
			{
				// Member dead → break that member's chain only
				if (buff.Target.IsDead)
				{
					this.RemoveMemberChain(buff);
					return;
				}

				// Member low HP (25% or less) → break that member's chain only
				var currentHp = buff.Target.Properties.GetFloat(PropertyName.HP);
				var maxHp = buff.Target.Properties.GetFloat(PropertyName.MHP);
				if (maxHp > 0 && (currentHp / maxHp) <= LowHpThreshold)
				{
					this.RemoveMemberChain(buff);
					return;
				}

				// Member changed map → break that member's chain only
				var storedMapId = buff.Vars.GetInt("Melia.Link.MapId", 0);
				if (buff.Target.Map == null || buff.Target.Map.Id != storedMapId)
				{
					this.RemoveMemberChain(buff);
					return;
				}

				// Check if caster is still valid
				if (buff.Caster == null || buff.Caster is not ICombatEntity casterEntity)
				{
					this.RemoveMemberChain(buff);
					return;
				}

				// Member out of range (>250) from caster → break that member's chain only
				var distance = buff.Target.Position.Get2DDistance(casterEntity.Position);
				if (distance > MaxHorizontalDistance)
				{
					this.RemoveMemberChain(buff);
					return;
				}
			}
		}

		/// <summary>
		/// Removes only this member's chain (visual effect and buff).
		/// </summary>
		private void RemoveMemberChain(Buff buff)
		{
			// Remove visual effect for this member (star topology: Link_{linkId}_{memberIndex})
			if (buff.Vars.TryGet<int>("Melia.Link.Id", out var linkId) &&
				buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles) &&
				buff.Caster != null)
			{
				var memberIndex = memberHandles.IndexOf(buff.Target.Handle);
				if (memberIndex > 0) // Index 0 is caster, members start at 1
				{
					buff.Caster.RemoveEffect($"Link_{linkId}_{memberIndex}");
				}
			}

			buff.Target.StopBuff(BuffId.Link_Physical);
		}

		/// <summary>
		/// Removes all chains from all members (when caster dies or changes map).
		/// </summary>
		private void RemoveAllChains(Buff buff)
		{
			if (!buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles))
			{
				buff.Target.StopBuff(BuffId.Link_Physical);
				return;
			}

			// Remove buff from all linked members
			foreach (var handle in memberHandles)
			{
				if (buff.Target.Map != null && buff.Target.Map.TryGetCombatEntity(handle, out var member))
				{
					member.StopBuff(BuffId.Link_Physical);
				}
			}
		}

		/// <summary>
		/// When the link buff ends, destroy the visual link effects.
		/// </summary>
		public override void OnEnd(Buff buff)
		{
			// Only the caster removes all visual effects
			if (buff.Vars.GetBool("Melia.Link.IsCaster"))
			{
				buff.Target.RemoveEffect("Melia.Link.Chain");

				if (buff.Vars.TryGet<int>("Melia.Link.Id", out var linkId) && linkId != 0)
				{
					// Star topology: remove all visual links (Link_{linkId}_1, Link_{linkId}_2, etc.)
					if (buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var members))
					{
						for (var i = 1; i < members.Count; i++)
						{
							buff.Caster?.RemoveEffect($"Link_{linkId}_{i}");
						}
					}
				}
			}
		}
	}
}
