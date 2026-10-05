using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Abilities.Handlers.Archers.Cannoneer;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Archers.Cannoneer
{
	[Package("laima")]
	[SkillHandler(SkillId.Cannoneer_SmokeGrenade)]
	public class Cannoneer_SmokeGrenadeOverride : IGroundSkillHandler, IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int MaximumSkillLevel = 10;
		private const int BaseMaximumTargets = 20;
		private const float DefaultEffectRadius = 100f;

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
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			var targetPosition = this.GetTargetPosition(skill, farPos);
			var effectRadius = Math.Max(DefaultEffectRadius, skill.Data.WaveLength);
			var area = new Circle(targetPosition, effectRadius);
			IEnumerable<ICombatEntity> targets = caster.Map.GetAttackableEnemiesIn(caster, area)
				.Where(enemy => enemy != null && !enemy.IsDead)
				.OrderBy(enemy => targetPosition.Get2DDistance(enemy.Position));

			if (!Cannoneer_SmokeGrenadeExceedLimitAbility.IsActive(character))
				targets = targets.Take(BaseMaximumTargets);

			var targetList = targets.ToList();
			var skillLevel = Math.Clamp(skill.Level, 1, MaximumSkillLevel);
			var duration = TimeSpan.FromSeconds(skillLevel + 1);
			var enhanceMultiplier = Cannoneer_SmokeGrenadeEnhanceAbility.GetDamageMultiplier(character);

			skill.IncreaseOverheat();
			caster.TurnTowards(targetPosition);
			caster.SetAttackState(true);

			try
			{
				var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

				Send.ZC_SKILL_READY(caster, skill, skillHandle, originPos, targetPosition);
				Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, targetPosition);
				Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPosition);

				var hits = new List<SkillHitInfo>();

				foreach (var enemy in targetList)
				{
					var skillHitResult = SCR_SkillHit(caster, enemy, skill);
					skillHitResult.Damage *= enhanceMultiplier;

					enemy.TakeDamage(skillHitResult.Damage, caster);
					enemy.StartBuff(BuffId.SmokeGrenade_Debuff, skillLevel, 0f, duration, caster, skill.Id);

					var skillHit = new SkillHitInfo(caster, enemy, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
					skillHit.ForceId = ForceId.GetNew();
					hits.Add(skillHit);
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
			finally
			{
				caster.SetAttackState(false);
				Send.ZC_NORMAL.Skill_45(caster);
				Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
				Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
			}
		}

		private Position GetTargetPosition(Skill skill, Position farPos)
		{
			if (skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPosition))
				return targetPosition;

			return farPos;
		}
	}
}
