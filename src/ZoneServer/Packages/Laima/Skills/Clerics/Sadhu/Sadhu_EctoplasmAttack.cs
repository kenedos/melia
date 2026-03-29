using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Sadhu
{
	/// <summary>
	/// Handler for the Sadhu skill Ectoplasm Attack.
	/// Spirit basic attack with 3 multi-hits and Soul attribute.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sadhu_EctoplasmAttack)]
	public class Sadhu_EctoplasmAttackOverride : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.IsBuffActive(BuffId.OOBE_Soulmaster_Buff))
				return;

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.Attack(skill, caster, originPos, farPos));
		}

		private async Task Attack(Skill skill, ICombatEntity caster, Position castPosition, Position targetPosition)
		{
			var aniTime = TimeSpan.FromMilliseconds(330);
			var skillHitDelay = skill.Properties.HitDelay;

			var spdRate = skill.Properties.GetFloat(PropertyName.SklSpdRate);
			var reducedSpdRate = 1f + (spdRate - 1f) / 2f;

			aniTime = TimeSpan.FromMilliseconds(aniTime.TotalMilliseconds / reducedSpdRate);
			skillHitDelay = TimeSpan.FromMilliseconds(skillHitDelay.TotalMilliseconds / reducedSpdRate);

			await skill.Wait(skillHitDelay);

			var splashParam = skill.GetSplashParameters(caster, castPosition, targetPosition, length: 30, width: 30, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			var hits = new List<SkillHitInfo>();

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.MultiHit(3);
				modifier.AttackAttribute = AttributeType.Soul;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, aniTime, skillHitDelay);
				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
