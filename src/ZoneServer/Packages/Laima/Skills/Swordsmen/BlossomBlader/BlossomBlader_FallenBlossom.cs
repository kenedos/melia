using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Fallen Blossom.
	/// Instant execution without client animation locking.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.BlossomBlader_FallenBlossom)]
	public class BlossomBlader_FallenBlossom : IGroundSkillHandler
	{
		private const int MaximumFloweringStacks = 5;
		private const int StartUpFloweringStacks = 3;
		private const int FallenBlossomHitCount = 5;
		private const int BaseMaximumTargets = 2;
		private const int AbsoluteMaximumTargets = 10;
		private const int MaximumEnhanceLevel = 100;
		private const float MaximumStackSlowPercent = 30f;
		private const int MinimumSealFloweringStacks = 3;
		private const float CooldownReductionSecondsPerAoe = 1.0f;

		private static readonly TimeSpan MaximumStackSlowDuration = TimeSpan.FromSeconds(5);
		private static readonly TimeSpan FloweringDuration = TimeSpan.FromSeconds(30);
		private static readonly TimeSpan SealSilenceDuration = TimeSpan.FromSeconds(3);

		private static readonly SkillId[] ComboSkills =
		{
			SkillId.BlossomBlader_ControlBlade,
			SkillId.BlossomBlader_BlossomSlash,
			SkillId.BlossomBlader_Flash,
			SkillId.BlossomBlader_FallenBlossom
		};

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity packetTarget)
		{
			if (caster == null || caster.IsDead)
				return;

			var targets = this.FindFloweringTargets(caster, skill, farPos, packetTarget);

			if (targets.Count == 0)
			{
				this.CancelSkillUse(caster, "No enemy affected by your Flowering was found.");
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				this.CancelSkillUse(caster, "Not enough SP.");
				return;
			}

			var primaryTarget = targets[0];

			var damageMultiplier = this.GetEnhanceMultiplier(caster);
			var totalHitCount = FallenBlossomHitCount + BlossomBladerStartUpHelper.GetAdditionalHitCount(caster);
			var hits = new List<SkillHitInfo>();
			var killedTarget = false;

			foreach (var target in targets)
			{
				if (!target.TryGetBuff(BuffId.Flowering_Debuff, out var floweringDebuff)
					|| floweringDebuff.Caster != caster)
				{
					continue;
				}

				floweringDebuff = this.ApplyStartUpFallenBlossom(caster, target, floweringDebuff);
				var floweringStacks = Math.Min(Math.Max(floweringDebuff.OverbuffCounter, 1), MaximumFloweringStacks);

				for (var hitIndex = 0; hitIndex < totalHitCount; hitIndex++)
				{
					if (target.IsDead)
						break;

					var modifier = SkillModifier.Default;
					modifier.DamageMultiplier *= damageMultiplier;
					BlossomBladerFloweringSwiftHelper.Apply(caster, target, skill, modifier);

					var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

					if (skillHitResult.Result == HitResultType.Dodge)
						continue;

					target.TakeDamage(skillHitResult.Damage, caster);
					killedTarget |= target.IsDead;

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, skill.Data.DefaultHitDelay, skill.Data.DefaultHitDelay);
					skillHit.HitEffect = HitEffect.Impact;
					hits.Add(skillHit);
				}

				if (!target.IsDead && floweringStacks >= MaximumFloweringStacks)
					target.StartBuff(BuffId.Slow_Debuff, MaximumStackSlowPercent, 0, MaximumStackSlowDuration, caster, skill.Id);

				this.ApplyFallenBlossomSeal(caster, target, skill, floweringStacks);

				target.RemoveBuff(BuffId.Flowering_Debuff);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);

			skill.IncreaseOverheat();

			if (killedTarget)
			{
				BlossomBladerCooldownHelper.ResetComboCooldowns(caster);
			}
			else
			{
				this.ApplyAoeCooldownReduction(caster);
			}

			this.MoveToPrimaryTarget(caster, primaryTarget, skill.Data.MaxRange);

			// Libera o personagem imediatamente no cliente sem locked state
			caster.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(caster);
		}

		private void ApplyAoeCooldownReduction(ICombatEntity caster)
		{
			var aoeAttackRatio = Math.Max(0f, caster.Properties.GetFloat(PropertyName.SR));
			if (aoeAttackRatio <= 0f)
				return;

			var reductionSpan = TimeSpan.FromSeconds(aoeAttackRatio * CooldownReductionSecondsPerAoe);

			foreach (var skillId in ComboSkills)
			{
				if (caster.TryGetSkill(skillId, out var affectedSkill))
				{
					affectedSkill.ReduceCooldown(reductionSpan);
				}
			}
		}

		private IList<ICombatEntity> FindFloweringTargets(ICombatEntity caster, Skill skill, Position targetPosition, ICombatEntity packetTarget)
		{
			var maximumTargets = this.GetMaximumTargets(caster);
			var candidates = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, skill.Data.MaxRange)
				.Where(target => this.HasOwnFlowering(caster, target))
				.Distinct()
				.ToList();

			var primaryTarget = packetTarget != null && candidates.Contains(packetTarget)
				? packetTarget
				: candidates.OrderBy(target => targetPosition.Get2DDistance(target.Position)).FirstOrDefault();

			if (primaryTarget == null)
				return new List<ICombatEntity>();

			return candidates
				.OrderBy(target => target == primaryTarget ? 0 : 1)
				.ThenBy(target => primaryTarget.Position.Get2DDistance(target.Position))
				.Take(maximumTargets)
				.ToList();
		}

		private bool HasOwnFlowering(ICombatEntity caster, ICombatEntity target)
		{
			return target != null
				&& !target.IsDead
				&& target.TryGetBuff(BuffId.Flowering_Debuff, out var floweringDebuff)
				&& floweringDebuff.Caster == caster;
		}

		private int GetMaximumTargets(ICombatEntity caster)
		{
			var aoeAttackRatio = Math.Max(0f, caster.Properties.GetFloat(PropertyName.SR));
			return Math.Clamp(BaseMaximumTargets + (int)MathF.Floor(aoeAttackRatio), 1, AbsoluteMaximumTargets);
		}

		private Buff ApplyStartUpFallenBlossom(ICombatEntity caster, ICombatEntity target, Buff floweringDebuff)
		{
			if (!BlossomBladerStartUpHelper.IsActive(caster))
				return floweringDebuff;

			if (!caster.TryGetActiveAbility(AbilityId.Blossomblader10, out _))
				return floweringDebuff;

			for (var stackIndex = 0; stackIndex < StartUpFloweringStacks; stackIndex++)
			{
				if (floweringDebuff.IsFullyOverbuffed)
					break;

				var updatedDebuff = target.StartBuff(BuffId.Flowering_Debuff, floweringDebuff.NumArg1, floweringDebuff.NumArg2, FloweringDuration, caster, SkillId.BlossomBlader_Flowering);

				if (updatedDebuff == null)
					break;

				floweringDebuff = updatedDebuff;
			}

			return floweringDebuff;
		}

		private void MoveToPrimaryTarget(ICombatEntity caster, ICombatEntity primaryTarget, float maximumDistance)
		{
			if (primaryTarget == null || primaryTarget.IsDead || primaryTarget.Map != caster.Map)
				return;

			var targetPosition = primaryTarget.Position;

			if (caster.Position.Get2DDistance(targetPosition) > maximumDistance)
				return;

			caster.SetPosition(targetPosition);
		}

		private float GetEnhanceMultiplier(ICombatEntity caster)
		{
			if (caster is not Character character)
				return 1f;

			var enhanceLevel = Math.Clamp(character.Abilities.GetLevel(AbilityId.Blossomblader3), 0, MaximumEnhanceLevel);

			if (enhanceLevel <= 0)
				return 1f;

			var bonusPercent = enhanceLevel * 0.5f;

			if (enhanceLevel >= MaximumEnhanceLevel)
				bonusPercent += 10f;

			return 1f + bonusPercent / 100f;
		}

		private void ApplyFallenBlossomSeal(ICombatEntity caster, ICombatEntity target, Skill skill, int floweringStacks)
		{
			if (target == null || target.IsDead || floweringStacks < MinimumSealFloweringStacks)
				return;

			if (!caster.TryGetActiveAbility(AbilityId.Blossomblader4, out _))
				return;

			target.StartBuff(BuffId.Silence_Debuff, 1, 0, SealSilenceDuration, caster, skill.Id);
		}

		private void CancelSkillUse(ICombatEntity caster, string message = null)
		{
			if (caster is Character character && !string.IsNullOrEmpty(message))
				character.ServerMessage(Localization.Get(message));

			Send.ZC_SKILL_DISABLE(caster);
		}
	}
}
