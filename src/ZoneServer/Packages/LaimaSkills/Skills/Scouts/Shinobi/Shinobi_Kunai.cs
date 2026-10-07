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

namespace Melia.Zone.Skills.Handlers.Scouts.Shinobi
{
	/// <summary>
	/// Handler for the Shinobi skill Kunai, a fan of five kunai thrown at
	/// the enemies ahead.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Shinobi_Kunai)]
	public class Shinobi_KunaiOverride : IGroundSkillHandler
	{
		private const float Range = 105f;
		private const float Angle = 60f;
		private static readonly TimeSpan[] KunaiAniTimes = [TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(150), TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(250), TimeSpan.FromMilliseconds(300)];

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);

			skill.Run(Throw(skill, caster));
			ShinobiSkillHelper.ReplicateOnClones(caster, skill.Id, Throw);
		}

		/// <summary>
		/// Throws the five kunai, each at the enemies the AoE Attack Ratio
		/// lets it reach.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="attacker"></param>
		/// <returns></returns>
		private static Task Throw(Skill skill, ICombatEntity attacker)
		{
			var area = new Fan(attacker.Position, attacker.Direction, Range, Angle);
			var targets = attacker.Map.GetAttackableEnemiesIn(attacker, area).LimitBySDR(attacker, skill).ToList();
			var hits = new List<SkillHitInfo>();

			for (var kunai = 0; kunai < KunaiAniTimes.Length; kunai++)
			{
				for (var i = 0; i < targets.Count; i++)
				{
					var target = targets[i];

					var skillHitResult = SCR_SkillHit(attacker, target, skill);
					target.TakeDamage(skillHitResult.Damage, attacker);

					var skillHit = new SkillHitInfo(attacker, target, skill, skillHitResult, KunaiAniTimes[kunai], TimeSpan.Zero);
					skillHit.HitFrameIndex = (byte)kunai;
					skillHit.TargetIndex = (byte)i;
					skillHit.ForceId = ForceId.GetNew();

					hits.Add(skillHit);
				}
			}

			Send.ZC_SKILL_MELEE_GROUND(attacker, skill, attacker.Position, hits);

			return Task.CompletedTask;
		}
	}
}
