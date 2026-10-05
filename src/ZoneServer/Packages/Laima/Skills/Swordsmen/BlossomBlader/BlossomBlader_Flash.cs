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
	/// Flash.
	/// Instant execution without client animation locking.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.BlossomBlader_Flash)]
	public class BlossomBlader_Flash : IGroundSkillHandler
	{
		private const int MaximumEnhanceLevel = 100;
		private const int FlashHitCount = 3;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity packetTarget)
		{
			if (caster == null || caster.IsDead)
				return;

			if (!caster.TrySpendSp(skill))
			{
				if (caster is Character character)
					character.ServerMessage(Localization.Get("Not enough SP."));

				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, farPos, skill.Data.MaxRange)
				.Where(target => target != null && !target.IsDead)
				.ToList();

			var damageMultiplier = this.GetEnhanceMultiplier(caster);
			var totalHitCount = FlashHitCount + BlossomBladerStartUpHelper.GetAdditionalHitCount(caster);
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

			caster.SetPosition(farPos);
			skill.IncreaseOverheat();

			if (killedTarget)
				BlossomBladerCooldownHelper.ResetComboCooldowns(caster);

			// Libera o personagem imediatamente no cliente sem locked state
			caster.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(caster);
		}

		private float GetEnhanceMultiplier(ICombatEntity caster)
		{
			if (caster is not Character character)
				return 1f;

			var enhanceLevel = Math.Clamp(character.Abilities.GetLevel(AbilityId.Blossomblader1), 0, MaximumEnhanceLevel);

			if (enhanceLevel <= 0)
				return 1f;

			var bonusPercent = enhanceLevel * 0.5f;

			if (enhanceLevel >= MaximumEnhanceLevel)
				bonusPercent += 10f;

			return 1f + bonusPercent / 100f;
		}
	}
}
