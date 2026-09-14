using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.SplashAreas;

namespace Melia.Zone.Skills.Handlers.Scouts.Corsair
{
	/// <summary>
	/// Handler for the Corsair skill Quick and Dead (Pistol Shot).
	/// Rapid-fire pistol attack dealing 4 hits to targets in range.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Corsair_PistolShot)]
	public class Corsair_PistolShotOverride : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			caster.SetAttackState(true);

			SkillRepeatHelper.Request(skill, caster, () => this.Attack(skill, caster, farPos));
		}

		/// <summary>
		/// Executes a single use of the skill, returning false if the caster
		/// can't pay for it.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="farPos"></param>
		private bool Attack(Skill skill, ICombatEntity caster, Position farPos)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return false;
			}

			skill.IncreaseOverheat();

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(skill, caster, farPos));

			return true;
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(100));

			var hits = new List<SkillHitInfo>();

			var splashParam = skill.GetSplashParameters(caster, caster.Position, farPos, length: 80f, width: 25f, angle: 110);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var aoeTargets = caster.Map.GetAttackableEnemiesIn(caster, splashArea)
				.OrderByDescending(t => t.IsBuffActive(BuffId.IronHooked))
				.LimitBySDR(caster, skill)
				.ToList();

			var hitCount = 8;
			for (var i = 0; i < hitCount; i++)
			{
				foreach (var target in aoeTargets)
				{
					if (target.IsDead)
						continue;

					var modifier = SkillModifier.Default;

					if (target.IsBuffActive(BuffId.IronHooked))
						modifier.CritRateMultiplier += 2.0f;

					var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
					target.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
					skillHit.HitEffect = HitEffect.Impact;
					hits.Add(skillHit);

					Send.ZC_HIT_INFO(caster, target, skillHit.HitInfo);
				}

				if (i < hitCount - 1)
					await skill.Wait(TimeSpan.FromMilliseconds(60));
			}
		}
	}
}
