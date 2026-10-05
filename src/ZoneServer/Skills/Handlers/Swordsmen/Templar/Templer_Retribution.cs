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
	[SkillHandler(SkillId.Templer_Retribution)]
	public class Templer_RetributionOverride : ITargetSkillHandler, IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int MaximumEnhanceLevel = 100;
		private const float AreaRadius = 100f;
		private const float EnhancePerLevel = 0.005f;
		private const float MaximumLevelBonus = 0.10f;
		private static readonly TimeSpan InitialHitDelay = TimeSpan.FromMilliseconds(200);
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(100);
		private static readonly TimeSpan StunDuration = TimeSpan.FromMilliseconds(1500);

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

			var targetPosition = this.GetTargetPosition(skill, farPos);

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				this.RejectCast(skill, caster, originPos, targetPosition);
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			caster.TurnTowards(targetPosition);
			Send.ZC_SKILL_READY(caster, skill, skillHandle, originPos, targetPosition);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, targetPosition);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPosition);
			skill.Run(this.HandleSkill(caster, skill, targetPosition));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position areaPosition)
		{
			try
			{
				await skill.Wait(InitialHitDelay);

				var hitCount = Math.Max(1, skill.Data.MultiHitCount);

				for (var hitIndex = 0; hitIndex < hitCount; hitIndex++)
				{
					if (caster.IsDead || caster.Map == null)
						break;

					var targets = this.GetTargetsInArea(caster, areaPosition);

					foreach (var target in targets)
					{
						this.ApplyHit(caster, target, skill);

						if (hitIndex == 0 && !target.IsDead)
							target.StartBuff(BuffId.Stun, StunDuration, caster);
					}

					if (hitIndex < hitCount - 1)
						await skill.Wait(DelayBetweenHits);
				}
			}
			finally
			{
				caster.SetAttackState(false);
				Send.ZC_SKILL_DISABLE(caster);
			}
		}

		private IList<ICombatEntity> GetTargetsInArea(ICombatEntity caster, Position areaPosition)
		{
			var area = new Circle(areaPosition, AreaRadius);

			return caster.Map
				.GetAttackableEnemiesIn(caster, area)
				.Where(target => target != null && !target.IsDead && caster.CanAttack(target))
				.Distinct()
				.ToList();
		}

		private Position GetTargetPosition(Skill skill, Position farPos)
		{
			if (skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPosition))
				return targetPosition;

			return farPos;
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
			if (caster is not Character character || !character.Abilities.TryGet(AbilityId.Templar18, out var ability) || !ability.Active)
				return;

			var abilityLevel = Math.Clamp(ability.Level, 0, MaximumEnhanceLevel);
			var enhanceRate = abilityLevel * EnhancePerLevel;

			if (abilityLevel >= MaximumEnhanceLevel)
				enhanceRate += MaximumLevelBonus;

			skillHitResult.Damage *= 1f + enhanceRate;
		}

		private void RejectCast(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			caster.SetAttackState(false);
			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SKILL_READY(caster, skill, skillHandle, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos);
			Send.ZC_SKILL_DISABLE(caster);
		}
	}
}
