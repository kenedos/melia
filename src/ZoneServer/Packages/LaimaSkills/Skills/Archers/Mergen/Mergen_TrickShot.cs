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
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Mergen
{
	/// <summary>
	/// Handler for the Mergen skill Triple Arrow, which shoots three arrows
	/// that explode on the enemies they hit.
	/// </summary>
	/// <remarks>
	/// With Triple Arrow: Zephyr the arrows don't explode and the volley
	/// is fired twice.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Mergen_TrickShot)]
	public class Mergen_TrickShotOverride : IGroundSkillHandler
	{
		private const float ExplosionRange = 30f;
		private static readonly int[] ArrowTimes = [0, 220, 320];
		private static readonly TimeSpan AniTime = TimeSpan.FromMilliseconds(420);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 175, width: 13, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Attack(skill, caster, splashArea));
		}

		/// <summary>
		/// Fires the volley, twice with Zephyr.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="splashArea"></param>
		private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			var isZephyr = caster.IsAbilityActive(AbilityId.Mergen27);
			var volleys = isZephyr ? 2 : 1;
			var explodeSkill = new Skill(caster, SkillId.Mergen_TrickShot_Explode, skill.Level);
			var elapsed = 0;

			for (var volley = 0; volley < volleys; volley++)
			{
				for (var arrow = 0; arrow < ArrowTimes.Length; arrow++)
				{
					var arrowTime = volley == 0 ? ArrowTimes[arrow] : ArrowTimes[arrow] + ArrowTimes[^1];
					await skill.Wait(TimeSpan.FromMilliseconds(arrowTime - elapsed));
					elapsed = arrowTime;

					var hits = new List<SkillHitInfo>();

					foreach (var target in caster.Map.GetAttackableEnemiesIn(caster, splashArea).LimitBySDR(caster, skill))
					{
						var skillHitResult = SCR_SkillHit(caster, target, skill);
						target.TakeDamage(skillHitResult.Damage, caster);

						var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, AniTime, TimeSpan.Zero);
						skillHit.ForceId = ForceId.GetNew();
						skillHit.HitFrameIndex = (byte)arrow;
						hits.Add(skillHit);

						if (!isZephyr)
							hits.AddRange(this.Explode(explodeSkill, caster, target));
					}

					Send.ZC_SKILL_HIT_INFO(caster, hits);
				}
			}
		}

		/// <summary>
		/// Damages the enemies around the target the arrow hit.
		/// </summary>
		/// <param name="explodeSkill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		private IEnumerable<SkillHitInfo> Explode(Skill explodeSkill, ICombatEntity caster, ICombatEntity target)
		{
			var explosionArea = new CircleF(target.Position, ExplosionRange);

			foreach (var explosionTarget in caster.Map.GetAttackableEnemiesIn(caster, explosionArea).LimitBySDR(caster, explodeSkill))
			{
				var skillHitResult = SCR_SkillHit(caster, explosionTarget, explodeSkill);
				explosionTarget.TakeDamage(skillHitResult.Damage, caster);

				yield return new SkillHitInfo(caster, explosionTarget, explodeSkill, skillHitResult, AniTime, TimeSpan.Zero);
			}
		}
	}
}
