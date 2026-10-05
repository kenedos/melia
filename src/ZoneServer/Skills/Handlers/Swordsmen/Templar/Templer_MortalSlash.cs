using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Swordsmen.Templar
{
	[Package("laima")]
	[SkillHandler(SkillId.Templer_MortalSlash)]
	public class Templer_MortalSlashOverride : ITargetSkillHandler, IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int MultiHitCount = 4;
		private const int MaximumEnhanceLevel = 100;
		private const float AreaLength = 100f;
		private const float AreaWidth = 40f;
		private const float EnhancePerLevel = 0.005f;
		private const float MaximumLevelBonus = 0.10f;
		private static readonly TimeSpan InitialHitDelay = TimeSpan.FromMilliseconds(200);
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(75);
		private static readonly TimeSpan CommandCooldownReduction = TimeSpan.FromSeconds(2);

		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			this.Cast(skill, caster, caster.Position, target?.Position ?? caster.Position);
		}

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
			if (caster == null || caster.IsDead || caster.Map == null)
				return;

			var directionX = farPos.X - originPos.X;
			var directionZ = farPos.Z - originPos.Z;
			var directionLength = MathF.Sqrt(directionX * directionX + directionZ * directionZ);

			if (directionLength <= 0.001f)
			{
				this.RejectCast(skill, caster, originPos, farPos);
				return;
			}

			directionX /= directionLength;
			directionZ /= directionLength;

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				this.RejectCast(skill, caster, originPos, farPos);
				return;
			}

			skill.IncreaseOverheat();
			this.ApplyCommandCooldownReduction(caster, skill);
			caster.SetAttackState(true);

			var targets = this.GetTargetsInArea(caster, originPos, directionX, directionZ);
			var primaryTarget = targets.OrderBy(caster.GetDistance).FirstOrDefault();
			var targetHandle = primaryTarget?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill, originPos, directionX, directionZ));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, float directionX, float directionZ)
		{
			try
			{
				await skill.Wait(InitialHitDelay);

				for (var hitIndex = 0; hitIndex < MultiHitCount; hitIndex++)
				{
					if (caster.IsDead || caster.Map == null)
						break;

					var targets = this.GetTargetsInArea(caster, originPos, directionX, directionZ);

					foreach (var target in targets)
						this.ApplyHit(caster, target, skill);

					if (hitIndex < MultiHitCount - 1)
						await skill.Wait(DelayBetweenHits);
				}
			}
			finally
			{
				caster.SetAttackState(false);
			}
		}

		private IList<ICombatEntity> GetTargetsInArea(ICombatEntity caster, Position originPos, float directionX, float directionZ)
		{
			var halfWidth = AreaWidth / 2f;
			var searchRadius = MathF.Sqrt(AreaLength * AreaLength + halfWidth * halfWidth);
			var searchArea = new Circle(originPos, searchRadius);

			return caster.Map
				.GetAttackableEnemiesIn(caster, searchArea)
				.Where(target => target != null && !target.IsDead && caster.CanAttack(target))
				.Where(target => this.IsInsideFrontArea(target.Position, originPos, directionX, directionZ, halfWidth))
				.Distinct()
				.ToList();
		}

		private bool IsInsideFrontArea(Position targetPosition, Position originPosition, float directionX, float directionZ, float halfWidth)
		{
			var offsetX = targetPosition.X - originPosition.X;
			var offsetZ = targetPosition.Z - originPosition.Z;
			var forwardDistance = offsetX * directionX + offsetZ * directionZ;

			if (forwardDistance < 0f || forwardDistance > AreaLength)
				return false;

			var sideDistance = MathF.Abs(offsetX * -directionZ + offsetZ * directionX);
			return sideDistance <= halfWidth;
		}

		private void ApplyHit(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			var skillHitResult = SCR_SkillHit(caster, target, skill);
			this.ApplyEnhanceAttribute(caster, skillHitResult);
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(
				caster,
				target,
				skill,
				skillHitResult,
				TimeSpan.FromMilliseconds(20),
				TimeSpan.Zero
			);

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);
		}

		private void ApplyEnhanceAttribute(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			if (caster is not Character character || !character.Abilities.TryGet(AbilityId.Templar1, out var ability) || !ability.Active)
				return;

			var abilityLevel = Math.Clamp(ability.Level, 0, MaximumEnhanceLevel);
			var enhanceRate = abilityLevel * EnhancePerLevel;

			if (abilityLevel >= MaximumEnhanceLevel)
				enhanceRate += MaximumLevelBonus;

			skillHitResult.Damage *= 1f + enhanceRate;
		}

		private void ApplyCommandCooldownReduction(ICombatEntity caster, Skill skill)
		{
			if (caster is not Character character || !character.Abilities.TryGet(AbilityId.Templar3, out var ability) || !ability.Active)
				return;

			skill.ReduceCooldown(CommandCooldownReduction);
		}

		private void RejectCast(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			caster.SetAttackState(false);
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos);
		}
	}
}
