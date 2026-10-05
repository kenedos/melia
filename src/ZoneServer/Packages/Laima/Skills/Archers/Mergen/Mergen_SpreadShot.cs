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
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Archers.Mergen
{
	/// <summary>
	/// Spread Shot.
	/// Fires arrows in a frontal cone and may ricochet to nearby enemies.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Mergen_Unload)]
	public class Mergen_SpreadShot : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int MaximumTargetsPerCone = 5;
		private const int MaximumEnhanceLevel = 100;
		private const float ConeRange = 180f;
		private const float ConeAngle = 90f;
		private const float RicochetRange = 150f;
		private const float RicochetChancePerSkillLevel = 1f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			if (caster is not Character character || character.IsDead || caster.Map == null)
				return;

			var directionX = farPos.X - originPos.X;
			var directionZ = farPos.Z - originPos.Z;
			var directionLength = MathF.Sqrt(directionX * directionX + directionZ * directionZ);

			if (directionLength <= 0f)
				return;

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			directionX /= directionLength;
			directionZ /= directionLength;

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var targetsHit = new HashSet<ICombatEntity>();
			var frontalTargets = this.GetTargetsInsideCone(caster, originPos, directionX, directionZ);

			foreach (var target in frontalTargets)
				this.HitOriginalTarget(skill, character, target, targetsHit);

			if (character.IsAbilityActive(AbilityId.Mergen8))
			{
				var backwardTargets = this.GetTargetsInsideCone(caster, originPos, -directionX, -directionZ);

				foreach (var target in backwardTargets)
					this.HitOriginalTarget(skill, character, target, targetsHit);
			}

			caster.SetAttackState(false);
		}

		private List<ICombatEntity> GetTargetsInsideCone(ICombatEntity caster, Position originPos, float directionX, float directionZ)
		{
			var maximumTargets = MergenZenithHelper.GetMaximumAoeTargets(caster, MaximumTargetsPerCone);

			return caster.Map.GetAttackableEnemiesIn(caster, new Circle(originPos, ConeRange))
				.Where(target => target != null && !target.IsDead)
				.Where(target => this.IsInsideCone(target.Position, originPos, directionX, directionZ))
				.OrderBy(target => originPos.Get2DDistance(target.Position))
				.Take(maximumTargets)
				.ToList();
		}

		private bool IsInsideCone(Position targetPosition, Position originPosition, float directionX, float directionZ)
		{
			var targetX = targetPosition.X - originPosition.X;
			var targetZ = targetPosition.Z - originPosition.Z;
			var targetDistance = MathF.Sqrt(targetX * targetX + targetZ * targetZ);

			if (targetDistance <= 0f || targetDistance > ConeRange)
				return false;

			targetX /= targetDistance;
			targetZ /= targetDistance;

			var dotProduct = directionX * targetX + directionZ * targetZ;
			var minimumDotProduct = MathF.Cos(ConeAngle * 0.5f * MathF.PI / 180f);

			return dotProduct >= minimumDotProduct;
		}

		private void HitOriginalTarget(Skill skill, Character caster, ICombatEntity target, HashSet<ICombatEntity> targetsHit)
		{
			if (target == null || target.IsDead || !targetsHit.Add(target))
				return;

			this.ApplyDamage(skill, caster, target);
			this.TryRicochet(skill, caster, target, targetsHit);
		}

		private void TryRicochet(Skill skill, Character caster, ICombatEntity originalTarget, HashSet<ICombatEntity> targetsHit)
		{
			if (!caster.IsAbilityActive(AbilityId.Mergen9))
				return;

			var ricochetChance = Math.Clamp(skill.Level * RicochetChancePerSkillLevel, 0f, 100f);

			if (RandomProvider.Get().Next(100) >= ricochetChance)
				return;

			var ricochetTarget = caster.Map.GetAttackableEnemiesInPosition(caster, originalTarget.Position, RicochetRange)
				.Where(target => target != null && !target.IsDead && target != originalTarget && !targetsHit.Contains(target))
				.OrderBy(target => originalTarget.Position.Get2DDistance(target.Position))
				.FirstOrDefault();

			if (ricochetTarget == null)
				return;

			targetsHit.Add(ricochetTarget);
			this.ApplyDamage(skill, caster, ricochetTarget);
		}

		private void ApplyDamage(Skill skill, Character caster, ICombatEntity target)
		{
			var skillHitResult = SCR_SkillHit(caster, target, skill);
			var damageMultiplier = this.GetEnhanceMultiplier(caster);
			damageMultiplier *= MergenZenithHelper.GetDamageMultiplier(caster);
			skillHitResult.Damage *= damageMultiplier;

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
			skillHit.HitEffect = HitEffect.Impact;
			skillHit.ApplyDamage();

			Send.ZC_HIT_INFO(caster, target, skillHit.HitInfo);
		}

		private float GetEnhanceMultiplier(Character caster)
		{
			var enhanceLevel = Math.Clamp(caster.Abilities.GetLevel(AbilityId.Mergen2), 0, MaximumEnhanceLevel);

			if (enhanceLevel <= 0)
				return 1f;

			var bonusPercent = enhanceLevel * 0.5f;

			if (enhanceLevel >= MaximumEnhanceLevel)
				bonusPercent += 10f;

			return 1f + bonusPercent / 100f;
		}
	}
}
