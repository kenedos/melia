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
	/// Handler for the Rune Caster skill Rune of Earth, which raises stones
	/// from the ground at the target location 4 times, 2 hits each.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.RuneCaster_Isa)]
	public class RuneCaster_IsaOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float Radius = 50f;
		private const int HitsPerStone = 2;
		private static readonly (int Time, int AniTime)[] HitTimings = [(100, 300), (140, 340), (180, 380), (220, 420)];

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}

			if (!caster.InSkillUseRange(skill, targetPos))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			RuneCasterSkillHelper.ApplySkilledCasting(caster);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, new Circle(targetPos, Radius)).LimitBySDR(caster, skill).ToList();

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, targetPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, targetPos, caster.Direction, targetPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos, ForceId.GetNew(), null);

			skill.Run(this.Attack(skill, caster, targets));
		}

		/// <summary>
		/// Strikes the targets once per stone.
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
					var skillHitResult = SCR_SkillHit(caster, hitTarget, skill, SkillModifier.MultiHit(HitsPerStone));
					hitTarget.TakeDamage(skillHitResult.Damage, caster);

					hits.Add(new SkillHitInfo(caster, hitTarget, skill, skillHitResult, aniTime, TimeSpan.Zero));
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
		}
	}
}
