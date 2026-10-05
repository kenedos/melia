using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizards.Onmyoji
{
	[Package("laima")]
	[SkillHandler(SkillId.Onmyoji_WaterShikigami)]
	public class Onmyoji_WaterShikigamiOverride : IGroundSkillHandler, IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int MaximumTargets = 12;
		private const float AttackRange = 150f;
		private const float AttackAngle = 90f;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
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
			if (caster == null)
				return;

			try
			{
				if (caster.IsDead || caster.Map == null)
					return;

				if (!caster.TrySpendSp(skill))
				{
					caster.ServerMessage(Localization.Get("Not enough SP."));
					return;
				}

				skill.IncreaseOverheat();
				caster.SetAttackState(true);
				caster.TurnTowards(farPos);

				var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
				Send.ZC_SKILL_READY(caster, skill, skillHandle, originPos, farPos);
				Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, Position.Zero);
				Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

				var splashParameters = skill.GetSplashParameters(caster, originPos, farPos, AttackRange, AttackRange, AttackAngle);
				var splashArea = skill.GetSplashArea(skill.Data.SplashType, splashParameters);
				var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea)
					.Where(target => target != null && !target.IsDead)
					.OrderBy(target => target.Position.Get2DDistance(farPos))
					.Take(MaximumTargets)
					.ToList();
				var totalHits = Math.Max(1, skill.Data.MultiHitCount);
				if (caster.IsAbilityActive(AbilityId.Onmyoji10))
					totalHits *= 2;

				var hits = new List<SkillHitInfo>();
				for (var hitIndex = 0; hitIndex < totalHits; hitIndex++)
				{
					foreach (var target in targets)
					{
						if (target.IsDead)
							continue;

						var skillHitResult = SCR_SkillHit(caster, target, skill);
						target.TakeDamage(skillHitResult.Damage, caster);
						hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
					}
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
			finally
			{
				caster.SetAttackState(false);
				Send.ZC_SKILL_DISABLE(caster);
			}
		}
	}
}
