using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// Handler for the Rune Caster skill Rune of Justice, which strikes the
	/// enemies in a straight line in front of the Rune Caster 5 times.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.RuneCaster_Tiwaz)]
	public class RuneCaster_TiwazOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float Length = 170f;
		private const float Width = 20f;
		private static readonly (int Time, int AniTime)[] HitTimings = [(200, 400), (300, 500), (400, 600), (500, 700), (600, 800)];

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			RuneCasterSkillHelper.ApplySkilledCasting(caster);

			var area = new Square(caster.Position, caster.Direction, Length, Width);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, area).LimitBySDR(caster, skill).ToList();

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Attack(skill, caster, targets));
		}

		/// <summary>
		/// Strikes the targets once per blow.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="targets"></param>
		/// <returns></returns>
		private async Task Attack(Skill skill, ICombatEntity caster, List<ICombatEntity> targets)
		{
			var elapsed = 0;

			foreach (var timing in HitTimings)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(timing.Time - elapsed));
				elapsed = timing.Time;

				if (caster.IsDead)
					return;

				var aniTime = TimeSpan.FromMilliseconds(timing.AniTime - timing.Time);
				var hits = new List<SkillHitInfo>();

				foreach (var hitTarget in targets.Where(t => !t.IsDead))
				{
					var skillHitResult = SCR_SkillHit(caster, hitTarget, skill);
					hitTarget.TakeDamage(skillHitResult.Damage, caster);

					hits.Add(new SkillHitInfo(caster, hitTarget, skill, skillHitResult, aniTime, TimeSpan.Zero));
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
		}
	}
}
