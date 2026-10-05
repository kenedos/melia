using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for Bullet Marker Double Gun basic attack.
	///
	/// SkillId: 63
	///
	/// While Double Gun Stance is active:
	/// - Executes DoubleGun_Attack.
	/// - Uses Double Gun Stance skill factor.
	/// - Applies Double Gun Stance: Enhance.
	/// - Generates 1 Overheating stack on successful hit.
	/// - Freeze Bullet gives a 30% chance to freeze the target.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.DoubleGun_Attack)]
	public class BulletMarker_DoubleGunAttack : IForceSkillHandler
	{
		private const float DoubleGunStanceBaseFactor = 63.6f;
		private const float DoubleGunStanceFactorPerLevel = 24f;
		private const int DoubleGunStanceMaxLevel = 10;
		private const double SilverPelletChance = 0.30;
		private static readonly TimeSpan SilverPelletHitAnimationTime = TimeSpan.FromMilliseconds(50);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (target == null || target.IsDead)
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.TryGetBuff(BuffId.DoubleGunStance_Buff, out _))
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.InSkillUseRange(skill, target))
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!character.TryGetSkill(SkillId.Bulletmarker_DoubleGunStance, out var doubleGunStance) || doubleGunStance.Level <= 0)
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var forceId = ForceId.GetNew();

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, forceId, null);

			var skillHitResult = SCR_SkillHit(caster, target, skill);

			this.ApplyDoubleGunStanceFactor(doubleGunStance, skillHitResult);
			this.ApplyEnhanceAbility(character, skillHitResult);

			target.TakeDamage(skillHitResult.Damage, caster);
			this.TryApplyFreeze(character, target);
			this.TryApplySilverPellet(character, target);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(200), TimeSpan.Zero);
			skillHit.ForceId = forceId;

			Send.ZC_SKILL_HIT_INFO(caster, skillHit);

			BulletMarkerOverheatingHelper.AddBasicAttackStack(character, skill);

			caster.SetAttackState(false);
		}

		private void ApplyDoubleGunStanceFactor(Skill doubleGunStance, SkillHitResult skillHitResult)
		{
			var skillLevel = Math.Clamp(doubleGunStance.Level, 1, DoubleGunStanceMaxLevel);
			var skillFactor = DoubleGunStanceBaseFactor + DoubleGunStanceFactorPerLevel * (skillLevel - 1);
			var skillFactorRate = skillFactor / 100f;

			skillHitResult.Damage *= skillFactorRate;
		}

		private void ApplyEnhanceAbility(Character character, SkillHitResult skillHitResult)
		{
			if (!character.Abilities.TryGet(AbilityId.Bulletmarker26, out var ability) || !ability.Active)
				return;

			var enhanceRate = ability.Level * 0.005f;

			if (ability.Level >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}

		private void TryApplyFreeze(Character character, ICombatEntity target)
		{
			if (!character.TryGetBuff(BuffId.FreezeBullet_Buff, out _))
				return;

			if (Random.Shared.NextDouble() > 0.30)
				return;

			target.StartBuff(BuffId.Freeze, 1, 0f, TimeSpan.FromSeconds(3), character);
		}

		private void TryApplySilverPellet(Character character, ICombatEntity target)
		{
			if (!character.TryGetBuff(BuffId.FreezeBullet_Buff, out _))
				return;

			if (!character.Abilities.TryGet(AbilityId.Bulletmarker24, out var ability) || !ability.Active)
				return;

			if (Random.Shared.NextDouble() >= SilverPelletChance)
				return;

			if (!character.TryGetSkill(SkillId.Bulletmarker_SilverBulletAttack, out var silverBulletSkill))
				return;

			var silverHitResult = SCR_SkillHit(character, target, silverBulletSkill);

			target.TakeDamage(silverHitResult.Damage, character);

			var silverHit = new SkillHitInfo(character, target, silverBulletSkill, silverHitResult, SilverPelletHitAnimationTime, TimeSpan.Zero);

			Send.ZC_SKILL_FORCE_TARGET(character, target, silverBulletSkill, silverHit);
		}
	}
}
