using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
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
	/// <summary>
	/// Handler for the Onmyoji skill Wind Shikigami, gusts that strike the
	/// enemies in a long line in front of the Onmyoji 5 times.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Onmyoji_WaterShikigami)]
	public class Onmyoji_WaterShikigamiOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float Length = 250f;
		private const float Width = 100f;
		private static readonly TimeSpan[] HitTimes = [TimeSpan.Zero, TimeSpan.FromMilliseconds(150), TimeSpan.FromMilliseconds(300), TimeSpan.FromMilliseconds(450), TimeSpan.FromMilliseconds(600)];

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: Length, width: Width, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea).Take(maxTargets).ToList();

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Attack(skill, caster, targets));
		}

		/// <summary>
		/// Strikes the targets once per gust.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="targets"></param>
		/// <returns></returns>
		private async Task Attack(Skill skill, ICombatEntity caster, List<ICombatEntity> targets)
		{
			var elapsed = TimeSpan.Zero;

			foreach (var hitTime in HitTimes)
			{
				await skill.Wait(hitTime - elapsed);
				elapsed = hitTime;

				if (caster.IsDead)
					return;

				var hits = new List<SkillHitInfo>();

				foreach (var hitTarget in targets.Where(t => !t.IsDead))
				{
					var skillHitResult = SCR_SkillHit(caster, hitTarget, skill);
					hitTarget.TakeDamage(skillHitResult.Damage, caster);

					hits.Add(new SkillHitInfo(caster, hitTarget, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
		}
	}
}
