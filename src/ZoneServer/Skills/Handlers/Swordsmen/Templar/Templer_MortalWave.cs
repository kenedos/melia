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
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Zone.Skills.SplashAreas;

namespace Melia.Zone.Packages.Laima.Skills.Swordsmen.Templar
{
	[Package("laima")]
	[SkillHandler(SkillId.Templer_MortalWave)]
	public class Templer_MortalWaveOverride : ITargetSkillHandler, IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int MultiHitCount = 7;
		private const int MaximumEnhanceLevel = 100;
		private const float AreaLength = 100f;
		private const float AreaWidth = 100f;
		private const float EnhancePerLevel = 0.005f;
		private const float MaximumLevelBonus = 0.10f;
		private static readonly TimeSpan InitialHitDelay = TimeSpan.FromMilliseconds(200);
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(100);

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
				directionX = caster.Direction.Cos;
				directionZ = caster.Direction.Sin;
			}
			else
			{
				directionX /= directionLength;
				directionZ /= directionLength;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				this.RejectCast(skill, caster, originPos);
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var forceId = ForceId.GetNew();

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, skill, originPos, directionX, directionZ, forceId));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, float directionX, float directionZ, int forceId)
		{
			try
			{
				await skill.Wait(InitialHitDelay);

				for (var hitIndex = 0; hitIndex < MultiHitCount; hitIndex++)
				{
					if (caster.IsDead)
						break;

					var targets = this.GetTargetsInSquare(caster, originPos, directionX, directionZ);

					foreach (var target in targets)
						this.ApplyHit(caster, target, skill, forceId);

					if (hitIndex < MultiHitCount - 1)
						await skill.Wait(DelayBetweenHits);
				}
			}
			finally
			{
				caster.SetAttackState(false);
			}
		}

		private IList<ICombatEntity> GetTargetsInSquare(ICombatEntity caster, Position originPos, float directionX, float directionZ)
		{
			var searchRadius = MathF.Sqrt(AreaLength * AreaLength + AreaWidth * AreaWidth);
			var halfWidth = AreaWidth / 2f;

			return caster.Map
				.GetAttackableEnemiesIn(caster, new Circle(originPos, searchRadius))
				.Where(target => target != null && !target.IsDead)
				.Where(target => this.IsInsideSquare(target.Position, originPos, directionX, directionZ, halfWidth))
				.Distinct()
				.ToList();
		}

		private bool IsInsideSquare(Position targetPos, Position originPos, float directionX, float directionZ, float halfWidth)
		{
			var offsetX = targetPos.X - originPos.X;
			var offsetZ = targetPos.Z - originPos.Z;
			var forwardDistance = offsetX * directionX + offsetZ * directionZ;

			if (forwardDistance < 0f || forwardDistance > AreaLength)
				return false;

			var sideDistance = MathF.Abs(offsetX * -directionZ + offsetZ * directionX);
			return sideDistance <= halfWidth;
		}

		private void ApplyHit(ICombatEntity caster, ICombatEntity target, Skill skill, int forceId)
		{
			var skillHitResult = SCR_SkillHit(caster, target, skill);
			this.ApplyEnhanceAttribute(caster, skillHitResult);
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(20), TimeSpan.Zero);
			skillHit.ForceId = forceId;

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);
		}

		private void ApplyEnhanceAttribute(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			if (caster is not Character character || !character.Abilities.TryGet(AbilityId.Templar17, out var ability) || !ability.Active)
				return;

			var abilityLevel = Math.Clamp(ability.Level, 0, MaximumEnhanceLevel);
			var enhanceRate = abilityLevel * EnhancePerLevel;

			if (abilityLevel >= MaximumEnhanceLevel)
				enhanceRate += MaximumLevelBonus;

			skillHitResult.Damage *= 1f + enhanceRate;
		}

		private void RejectCast(Skill skill, ICombatEntity caster, Position originPos)
		{
			caster.SetAttackState(false);
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, originPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos);
		}
	}
}
