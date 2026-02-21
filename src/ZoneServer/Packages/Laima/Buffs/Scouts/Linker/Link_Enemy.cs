using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scouts.Linker
{
	/// <summary>
	/// Handler for the Link_Enemy (de)buff, which links enemy targets together
	/// to share received damage.
	/// Linked targets take reduced damage (-30% base) offset by skill level (+3% per level)
	/// and enhanced by Joint Penalty: Enhance ability (+0.5% per ability level).
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Link_Enemy)]
	public class Link_EnemyOverride : BuffHandler, IBuffCombatDefenseAfterCalcHandler
	{
		private const float BaseDamageReduction = -0.30f;
		private const float DamageIncreasePerSkillLevel = 0.03f;
		private const float AbilityBonusPerLevel = 0.005f;
		private const float MaxHorizontalDistance = 200f;
		private const int DistanceCheckIntervalMs = 500;

		/// <summary>
		/// Called when the buff is activated. Sets up the update interval for distance checking.
		/// </summary>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(DistanceCheckIntervalMs);
		}

		/// <summary>
		/// Applies the buff's effect during the combat calculations.
		/// Enemy link propagates damage to all linked enemies with damage
		/// modification based on skill level and enhancement ability.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
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
						linkTargets.Add(member);
					}
				}
			}

			if (!linkTargets.Any())
				return;

			// Calculate damage multiplier based on skill level and ability
			// Formula: DamageMult = 1 + (-0.30 + 0.03 * SkillLv * (1 + 0.005 * AbilityLevel))
			// SkillLv 1: 73%, SkillLv 10: 100%, SkillLv 10 + Lv100 ability: 115%
			var skillLevel = buff.NumArg1;
			var abilityMultiplier = 1f;

			if (buff.Caster is ICombatEntity caster && caster.TryGetActiveAbilityLevel(AbilityId.Linker22, out var abilityLevel))
			{
				abilityMultiplier = 1f + (abilityLevel * AbilityBonusPerLevel);
			}

			var damageMultiplier = 1f + BaseDamageReduction + (DamageIncreasePerSkillLevel * skillLevel * abilityMultiplier);

			// Apply damage modifier to the target being hit
			skillHitResult.Damage *= damageMultiplier;

			// Share the modified damage to all other linked enemies
			var sharedDamage = skillHitResult.Damage;

			foreach (var linkTarget in linkTargets)
			{
				if (linkTarget.IsDead || linkTarget.Handle == target.Handle)
					continue;

				// Skip targets that no longer have the link buff (wandered off)
				if (!linkTarget.IsBuffActive(BuffId.Link_Enemy))
					continue;

				// Only damage enemies of the attacker
				if (attacker.IsEnemy(linkTarget))
				{
					linkTarget.TakeDamage(sharedDamage, attacker);

					var hitInfo = new HitInfo(attacker, linkTarget, skill, sharedDamage, skillHitResult.Result);
					Send.ZC_HIT_INFO(attacker, linkTarget, hitInfo);
				}
			}
		}

		/// <summary>
		/// Periodically checks if the target has wandered too far from the anchor.
		/// If so, removes the buff from this target only.
		/// </summary>
		public override void WhileActive(Buff buff)
		{
			// Only check non-anchor targets
			if (buff.Vars.GetBool("Melia.Link.IsAnchor"))
				return;

			if (!buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles) || memberHandles.Count == 0)
				return;

			// First member is always the anchor
			var anchorHandle = memberHandles[0];
			if (buff.Target.Map == null || !buff.Target.Map.TryGetCombatEntity(anchorHandle, out var anchor))
			{
				// Anchor is gone, remove this buff
				buff.Target.StopBuff(BuffId.Link_Enemy);
				return;
			}

			// Check horizontal distance from anchor
			var distance = buff.Target.Position.Get2DDistance(anchor.Position);
			if (distance > MaxHorizontalDistance)
			{
				// Remove visual effect for this target before stopping buff
				if (buff.Vars.TryGet<int>("Melia.Link.Id", out var linkId) && buff.Caster != null)
				{
					var memberIndex = memberHandles.IndexOf(buff.Target.Handle);
					if (memberIndex > 0)
					{
						buff.Caster.RemoveEffect($"Link_{linkId}_{memberIndex}");
					}
				}

				buff.Target.StopBuff(BuffId.Link_Enemy);
			}
		}

		/// <summary>
		/// When the link buff ends, destroy the visual link effects.
		/// </summary>
		/// <param name="buff">The buff instance that is ending.</param>
		public override void OnEnd(Buff buff)
		{
			// Only the anchor removes visual effects
			if (buff.Vars.GetBool("Melia.Link.IsAnchor"))
			{
				buff.Target.RemoveEffect("Melia.Link.Chain");

				if (buff.Vars.TryGet<int>("Melia.Link.Id", out var linkId) && linkId != 0)
				{
					var topology = buff.Vars.GetInt("Melia.Link.Topology", 0);

					if (topology == 1) // Star topology
					{
						// Remove all star topology visual links (Link_{linkId}_1, Link_{linkId}_2, etc.)
						if (buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var members))
						{
							for (var i = 1; i < members.Count; i++)
							{
								buff.Caster?.RemoveEffect($"Link_{linkId}_{i}");
							}
						}
					}
					else // Chain topology
					{
						buff.Caster?.RemoveEffect($"Link_{linkId}");
					}
				}
			}
		}
	}
}
