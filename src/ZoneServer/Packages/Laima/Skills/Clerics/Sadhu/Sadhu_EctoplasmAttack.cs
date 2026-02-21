using System;
using System.Collections.Generic;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Sadhu
{
	/// <summary>
	/// Handler for the Sadhu skill Ectoplasm Attack.
	/// Spirit basic attack using MATK with Soul attribute. 129% factor.
	/// Only usable in spirit form (OOBE_Soulmaster_Buff active).
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sadhu_EctoplasmAttack)]
	public class Sadhu_EctoplasmAttackOverride : IMeleeGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.IsBuffActive(BuffId.OOBE_Soulmaster_Buff))
				return;

			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, caster.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, farPos);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 25, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			var aoeTargets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var damageDelay = TimeSpan.FromMilliseconds(300);

			var hits = new List<SkillHitInfo>();
			foreach (var target in aoeTargets.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.MultiHit(3);
				modifier.AttackAttribute = AttributeType.Soul;
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, TimeSpan.Zero);
				hits.Add(skillHit);
			}

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, hits);
		}
	}
}
