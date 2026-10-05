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
	/// Blossom Slash.
	/// Instant execution without client animation locking.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.BlossomBlader_BlossomSlash)]
	public class BlossomBlader_BlossomSlash : IGroundSkillHandler
	{
		private const int MaximumEnhanceLevel = 100;
		private const int BlossomSlashHitCount = 12;
		private const int BaseMaximumTargets = 6;
		private const int AbsoluteMaximumTargets = 14;
		private const float AoeDexDamageDivisor = 25000f;
		private const float MaximumAoeDexDamageBonus = 0.40f;
		private const float StartUpBlossomSlashDamageMultiplier = 1.30f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity packetTarget)
		{
			if (caster == null || caster.IsDead)
				return;

			var targets = this.FindFloweringTargets(caster, skill, farPos, packetTarget);

			if (targets.Count == 0)
			{
				if (caster is Character character)
					character.ServerMessage(Localization.Get("No target affected by your Flowering was found."));

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

			var damageMultiplier = this.GetEnhanceMultiplier(caster);
			damageMultiplier *= this.GetStartUpBlossomSlashMultiplier(caster);
			damageMultiplier *= 1f + this.GetAoeDexDamageBonus(caster);

			var totalHitCount = BlossomSlashHitCount + BlossomBladerStartUpHelper.GetAdditionalHitCount(caster);
			var hits = new List<SkillHitInfo>();
			var killedTarget = false;

			foreach (var target in targets)
			{
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

					if (target.IsDead)
						killedTarget = true;

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, skill.Data.DefaultHitDelay, skill.Data.DefaultHitDelay);
					skillHit.HitEffect = HitEffect.Impact;
					hits.Add(skillHit);
				}
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);

			skill.IncreaseOverheat();

			if (killedTarget)
				BlossomBladerCooldownHelper.ResetComboCooldowns(caster);

			// Libera o personagem imediatamente no cliente sem locked state
			caster.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(caster);
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

		private float GetAoeDexDamageBonus(ICombatEntity caster)
		{
			var aoeAttackRatio = Math.Max(0f, caster.Properties.GetFloat(PropertyName.SR));
			var dex = Math.Max(0f, caster.Properties.GetFloat(PropertyName.DEX));
			return Math.Min(aoeAttackRatio * dex / AoeDexDamageDivisor, MaximumAoeDexDamageBonus);
		}

		private float GetEnhanceMultiplier(ICombatEntity caster)
		{
			if (caster is not Character character)
				return 1f;

			var enhanceLevel = Math.Clamp(character.Abilities.GetLevel(AbilityId.Blossomblader8), 0, MaximumEnhanceLevel);

			if (enhanceLevel <= 0)
				return 1f;

			var bonusPercent = enhanceLevel * 0.5f;

			if (enhanceLevel >= MaximumEnhanceLevel)
				bonusPercent += 10f;

			return 1f + bonusPercent / 100f;
		}

		private float GetStartUpBlossomSlashMultiplier(ICombatEntity caster)
		{
			if (!BlossomBladerStartUpHelper.IsActive(caster))
				return 1f;

			if (!caster.TryGetActiveAbility(AbilityId.Blossomblader17, out _))
				return 1f;

			return StartUpBlossomSlashDamageMultiplier;
		}
	}
}
