using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Shared.World;

namespace Melia.Zone.Skills.Handlers.Common
{
	/// <summary>
	/// Handles ranged skills that target a single entity.
	/// </summary>
	[SkillHandler(SkillId.Bow_Attack, SkillId.Magic_Attack, SkillId.Magic_Attack_TH,
		SkillId.Bow_Hanging_Attack, SkillId.Pistol_Attack, SkillId.Cannon_Normal_Attack,
		SkillId.CrossBow_Attack, SkillId.CrossBow_Attack2,
		SkillId.Bow_Attack2, SkillId.Pistol_Attack2, SkillId.Cannon_Attack,
		SkillId.DoubleGun_Attack, SkillId.DoubleBullet_Attack, SkillId.Musket_Attack)]
	public class TargetSkill : ITargetSkillHandler, IForceSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			if ((skill.Id == SkillId.Pistol_Attack2 || skill.Id == SkillId.DoubleBullet_Attack) && caster.TryGetBuff(BuffId.Limacon_Buff, out _))
			{
				if (!this.TrySpendLimaconAttackSp(caster))
					return;
			}

			if ((skill.Id == SkillId.Pistol_Attack2 || skill.Id == SkillId.DoubleBullet_Attack) && caster.TryGetBuff(BuffId.DoubleBullet_Toggle_Buff, out _))
			{
				if (!this.TrySpendDoubleBulletAttackSp(caster))
					return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			if (target == null)
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.InSkillUseRange(skill, target))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var aniTime = TimeSpan.FromMilliseconds(skill.Id != SkillId.Common_DaggerAries ? 330 : 250);
			var skillHitDelay = skill.Properties.HitDelay;

			aniTime = TimeSpan.FromMilliseconds(aniTime.TotalMilliseconds / skill.Properties.GetFloat(PropertyName.SklSpdRate));
			skillHitDelay = TimeSpan.FromMilliseconds(skillHitDelay.TotalMilliseconds / skill.Properties.GetFloat(PropertyName.SklSpdRate));

			var skillHitResult = SCR_SkillHit(caster, target, skill);

			//Apply Limacon: Enhance
			this.ApplyLimaconEnhance(skill, caster, skillHitResult);

			// Apply [Arts] Limacon: Enhanced Upgrade.
			this.ApplyLimaconEnhancedUpgrade(skill, caster, skillHitResult);

			// Apply [Arts] Serial Bullet: Enhanced Upgrade.
			this.ApplySerialBulletEnhancedUpgrade(skill, caster, skillHitResult);

			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, aniTime, skillHitDelay);

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			if ((skill.Id == SkillId.Pistol_Attack2 || skill.Id == SkillId.DoubleBullet_Attack) && caster.TryGetBuff(BuffId.Limacon_Buff, out _))
			{
				if (!this.TrySpendLimaconAttackSp(caster))
					return;
			}

			if ((skill.Id == SkillId.Pistol_Attack2 || skill.Id == SkillId.DoubleBullet_Attack) && caster.TryGetBuff(BuffId.DoubleBullet_Toggle_Buff, out _))
			{
				if (!this.TrySpendDoubleBulletAttackSp(caster))
					return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			if (target == null)
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.InSkillUseRange(skill, target))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill, null);
				return;
			}

			var aniTime = TimeSpan.FromMilliseconds(skill.Id != SkillId.Common_DaggerAries ? 330 : 250);
			var skillHitDelay = skill.Properties.HitDelay;

			aniTime = TimeSpan.FromMilliseconds(aniTime.TotalMilliseconds / skill.Properties.GetFloatSafe(PropertyName.SklSpdRate));
			skillHitDelay = TimeSpan.FromMilliseconds(skillHitDelay.TotalMilliseconds / skill.Properties.GetFloatSafe(PropertyName.SklSpdRate));

			var modifier = SkillModifier.Default;

			if (caster.TryGetBuff(BuffId.DoubleAttack_Buff, out var doubleAttackBuff) && RandomProvider.Get().Next(100) < doubleAttackBuff.NumArg2)
			{
				modifier.HitCount += 1;
			}

			// Serial Bullet bonus attack.
			if ((skill.Id == SkillId.Pistol_Attack2 || skill.Id == SkillId.DoubleBullet_Attack) &&
				caster.TryGetBuff(BuffId.DoubleBullet_Toggle_Buff, out var buff))
			{
				modifier.BonusDamage += 100 * buff.NumArg1;
			}

			var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

			//Apply Limacon: Enhance
			this.ApplyLimaconEnhance(skill, caster, skillHitResult);

			// Apply [Arts] Limacon: Enhanced Upgrade.
			this.ApplyLimaconEnhancedUpgrade(skill, caster, skillHitResult);

			// Apply [Arts] Serial Bullet: Enhanced Upgrade.
			this.ApplySerialBulletEnhancedUpgrade(skill, caster, skillHitResult);

			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, aniTime, skillHitDelay);

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);
		}

		/// <summary>
		/// Applies [Arts] Limacon: Enhanced Upgrade to Limacon's main attack damage.
		/// </summary>
		private void ApplyLimaconEnhancedUpgrade(Skill skill, ICombatEntity caster, SkillHitResult skillHitResult)
		{
			const AbilityId LimaconEnhancedUpgradeAbilityId = AbilityId.Schwarzereiter23;

			if (skill.Id != SkillId.Pistol_Attack2)
				return;

			if (!caster.TryGetBuff(BuffId.Limacon_Buff, out _))
				return;

			if (caster is not Character character)
				return;

			if (!character.IsAbilityActive(LimaconEnhancedUpgradeAbilityId))
				return;

			skillHitResult.Damage *= 1.3f;
		}

		/// <summary>
		/// Applies [Arts] Serial Bullet: Enhanced Upgrade to Serial Bullet attack damage.
		/// </summary>
		private void ApplySerialBulletEnhancedUpgrade(Skill skill, ICombatEntity caster, SkillHitResult skillHitResult)
		{
			const AbilityId SerialBulletEnhancedUpgradeAbilityId = AbilityId.Schwarzereiter29;

			// The arts only affects Serial Bullet damage.
			if (skill.Id != SkillId.DoubleBullet_Attack)
				return;

			// The arts only works while Serial Bullet is active.
			if (!caster.TryGetBuff(BuffId.DoubleBullet_Toggle_Buff, out _))
				return;

			// Only characters can have abilities.
			if (caster is not Character character)
				return;

			// The arts must be learned and active.
			if (!character.IsAbilityActive(SerialBulletEnhancedUpgradeAbilityId))
				return;

			// Apply final damage bonus from [Arts] Serial Bullet: Enhanced Upgrade.
			skillHitResult.Damage *= 1.3f;
		}

		private bool TrySpendLimaconAttackSp(ICombatEntity caster)
		{
			var spCost = 6;

			if (!caster.TrySpendSp(spCost))
			{
				caster.StopBuff(BuffId.Limacon_Buff);
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return false;
			}

			return true;
		}

		private bool TrySpendDoubleBulletAttackSp(ICombatEntity caster)
		{
			var spCost = 6f;
			var currentSp = caster.Properties.GetFloat(PropertyName.SP);

			if (currentSp < spCost)
			{
				caster.StopBuff(BuffId.DoubleBullet_Toggle_Buff);
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return false;
			}

			if (!caster.TrySpendSp(spCost))
			{
				caster.StopBuff(BuffId.DoubleBullet_Toggle_Buff);
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return false;
			}

			return true;
		}

		/// <summary>
		/// Applies Limacon: Enhance damage bonus to Limacon's main attack.
		/// </summary>
		private void ApplyLimaconEnhance(Skill skill, ICombatEntity caster, SkillHitResult skillHitResult)
		{
			const AbilityId LimaconEnhanceAbilityId = AbilityId.Schwarzereiter13;

			// Limacon: Enhance only affects Limacon's main attack.
			if (skill.Id != SkillId.Pistol_Attack2)
				return;

			// Limacon must be active.
			if (!caster.TryGetBuff(BuffId.Limacon_Buff, out _))
				return;

			// Only characters can have abilities.
			if (caster is not Character character)
				return;

			var abilityById = character.Abilities.GetLevel(AbilityId.Schwarzereiter13);
			var abilityByClass = character.Abilities.Get("Schwarzereiter12");

			if (abilityById <= 0)
				return;

			// Standard Enhance formula: +0.5% damage per ability level.
			var damageMultiplier = 1f + (0.005f * abilityById);

			skillHitResult.Damage *= damageMultiplier;
		}
	}
}
