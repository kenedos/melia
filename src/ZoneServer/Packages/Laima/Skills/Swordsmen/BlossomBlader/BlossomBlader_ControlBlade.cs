using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
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
	/// Control Blade.
	/// Opens the Blossom Blader combo by attacking multiple enemies and applying Flowering.
	/// The maximum number of targets scales with the character's AoE Attack Ratio.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.BlossomBlader_ControlBlade)]
	public class BlossomBlader_ControlBlade : IGroundSkillHandler
	{
		private const int MaximumEnhanceLevel = 100;
		private const int ControlBladeHitCount = 11;
		private const int BaseMaximumTargets = 6;
		private const int AbsoluteMaximumTargets = 14;
		private const float TargetSearchRadius = 120f;
		private const int StartUpControlBladeCooldownReductionSeconds = 1;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity packetTarget)
		{
			if (caster == null || caster.IsDead)
				return;

			var primaryTarget = this.FindPrimaryTarget(caster, farPos, packetTarget);

			if (primaryTarget == null)
			{
				if (caster is Character character)
					character.ServerMessage(Localization.Get("No valid target was found."));

				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				if (caster is Character character)
					character.ServerMessage(Localization.Get("Not enough SP."));

				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			var targets = this.FindTargets(caster, primaryTarget);
			var targetPosition = primaryTarget.Position;
			caster.TurnTowards(targetPosition);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, targetPosition);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPosition);

			var damageMultiplier = this.GetEnhanceMultiplier(caster);
			var totalHitCount = ControlBladeHitCount + BlossomBladerStartUpHelper.GetAdditionalHitCount(caster);
			var hits = new List<SkillHitInfo>();
			var killedTarget = false;

			foreach (var target in targets)
			{
				var landedAtLeastOneHit = false;

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
					landedAtLeastOneHit = true;

					if (target.IsDead)
						killedTarget = true;

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, skill.Data.DefaultHitDelay, skill.Data.DefaultHitDelay);
					skillHit.HitEffect = HitEffect.Impact;
					hits.Add(skillHit);
				}

				// 1 - Aplica Flowering se acertar o alvo e ele continuar vivo
				if (landedAtLeastOneHit && !target.IsDead)
					BlossomBladerFloweringHelper.Apply(caster, target, skill);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);

			skill.IncreaseOverheat();
			this.TryApplyStartUpControlBlade(caster, skill);

			if (killedTarget)
				BlossomBladerCooldownHelper.ResetComboCooldowns(caster);

			caster.SetAttackState(false);
		}

		private ICombatEntity FindPrimaryTarget(ICombatEntity caster, Position targetPosition, ICombatEntity packetTarget)
		{
			if (packetTarget != null && !packetTarget.IsDead)
				return packetTarget;

			return caster.Map.GetAttackableEnemiesInPosition(caster, targetPosition, 20f)
				.Where(target => target != null && !target.IsDead)
				.OrderBy(target => targetPosition.Get2DDistance(target.Position))
				.FirstOrDefault();
		}

		private IList<ICombatEntity> FindTargets(ICombatEntity caster, ICombatEntity primaryTarget)
		{
			var maximumTargets = this.GetMaximumTargets(caster);
			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, primaryTarget.Position, TargetSearchRadius)
				.Where(target => target != null && !target.IsDead)
				.Distinct()
				.OrderBy(target => target == primaryTarget ? 0 : 1)
				.ThenBy(target => primaryTarget.Position.Get2DDistance(target.Position))
				.Take(maximumTargets)
				.ToList();

			if (!targets.Contains(primaryTarget))
			{
				if (targets.Count >= maximumTargets)
					targets.RemoveAt(targets.Count - 1);

				targets.Insert(0, primaryTarget);
			}

			return targets;
		}

		private int GetMaximumTargets(ICombatEntity caster)
		{
			var aoeAttackRatio = Math.Max(0f, caster.Properties.GetFloat(PropertyName.SR));
			return Math.Clamp(BaseMaximumTargets + (int)MathF.Floor(aoeAttackRatio), 1, AbsoluteMaximumTargets);
		}

		private void TryApplyStartUpControlBlade(ICombatEntity caster, Skill skill)
		{
			if (!BlossomBladerStartUpHelper.IsActive(caster))
				return;

			if (!caster.TryGetActiveAbility(AbilityId.Blossomblader9, out _))
				return;

			skill.ReduceCooldown(TimeSpan.FromSeconds(StartUpControlBladeCooldownReductionSeconds));
		}

		private float GetEnhanceMultiplier(ICombatEntity caster)
		{
			if (caster is not Character character)
				return 1f;

			var enhanceLevel = Math.Clamp(character.Abilities.GetLevel(AbilityId.Blossomblader5), 0, MaximumEnhanceLevel);

			if (enhanceLevel <= 0)
				return 1f;

			var bonusPercent = enhanceLevel * 0.5f;

			if (enhanceLevel >= MaximumEnhanceLevel)
				bonusPercent += 10f;

			return 1f + bonusPercent / 100f;
		}
	}
}
