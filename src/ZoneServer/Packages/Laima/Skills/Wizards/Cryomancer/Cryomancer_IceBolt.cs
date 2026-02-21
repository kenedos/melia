using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Cryomancer
{
	/// <summary>
	/// Handler for the Cryomancer skill Ice Bolt.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cryomancer_IceBolt)]
	public class Cryomancer_IceBoltOverride : IForceSkillHandler, IDynamicCasted
	{
		private const int MaxTargets = 4;
		private const float BaseFreezeChance = 50f;
		private const float FreezeChancePerLevel = 2f;
		private const float FreezeDurationMilliSeconds = 4000;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			if (target == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var damageDelay = TimeSpan.Zero;
			var skillHitDelay = TimeSpan.Zero;
			var splashArea = new Circle(target.Position, skill.Properties.GetFloat(PropertyName.SplRange));

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea)
				.Where(t => t != target)
				.OrderBy(t => t.Position.Get2DDistance(farPos))
				.Take(MaxTargets - 1)
				.ToList();

			targets.Insert(0, target);

			var skillHits = new List<SkillHitInfo>();

			var freezeChance = BaseFreezeChance + (skill.Level * FreezeChancePerLevel);

			if (caster.TryGetActiveAbility(AbilityId.Cryomancer9, out var abilCryomancer9))
				freezeChance = (int)Math.Floor(freezeChance * (1 + abilCryomancer9.Level * 0.05));

			if (caster.TryGetActiveAbility(AbilityId.Cryomancer2, out var abilCryomancer2))
				freezeChance = (int)Math.Floor(freezeChance * (1 + abilCryomancer2.Level * 0.10));

			var random = RandomProvider.Get();
			foreach (var currentTarget in targets)
			{
				var skillHitResult = SCR_SkillHit(caster, currentTarget, skill, SkillModifier.MultiHit(2));
				currentTarget.TakeDamage(skillHitResult.Damage, caster);
				var skillHit = new SkillHitInfo(caster, currentTarget, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.ForceId = ForceId.GetNew();

				if (skillHitResult.Damage > 0 && currentTarget != target && currentTarget.IsKnockdownable())
				{
					skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, skillHit.Target, HitType.KnockBack, 90, 10);
					skillHit.HitInfo.Type = HitType.KnockBack;
					currentTarget.ApplyKnockback(caster, skill, skillHit);
				}

				skillHits.Add(skillHit);

				Send.ZC_NORMAL.PlayForceEffect(caster, target, currentTarget, skillHit.ForceId, "I_force110_ice", 0.5f, "", "", 1, "", "SLOW", 300, 1, 1, 1, 0);

				if (currentTarget.IsDead)
					continue;

				if ((random.Next(100) < freezeChance) && skillHitResult.Damage > 0)
				{
					currentTarget.StartBuff(BuffId.Cryomancer_Freeze, TimeSpan.FromMilliseconds(FreezeDurationMilliSeconds), caster);
				}
			}

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHits);
		}
	}
}
