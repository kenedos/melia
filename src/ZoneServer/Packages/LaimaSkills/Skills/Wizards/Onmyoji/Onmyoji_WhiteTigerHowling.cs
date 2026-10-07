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
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizards.Onmyoji
{
	/// <summary>
	/// Handler for the Onmyoji skill Howling White Tiger, a roar that strikes
	/// the enemies around the Onmyoji 3 times, frightens them for 7 seconds
	/// and stuns them for 1.
	/// </summary>
	/// <remarks>
	/// With Howling White Tiger: Virtuous Roar, the party around the Onmyoji
	/// moves 10 faster for 10 seconds.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Onmyoji_WhiteTigerHowling)]
	public class Onmyoji_WhiteTigerHowlingOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float Range = 200f;
		private static readonly TimeSpan[] HitTimes = [TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(300), TimeSpan.FromMilliseconds(500)];
		private static readonly TimeSpan FearDuration = TimeSpan.FromSeconds(7);
		private static readonly TimeSpan StunDuration = TimeSpan.FromSeconds(1);
		private static readonly TimeSpan RoarDuration = TimeSpan.FromSeconds(10);

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
			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, Range).Take(maxTargets).ToList();

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			if (caster.IsAbilityActive(AbilityId.Onmyoji8))
			{
				foreach (var ally in PartySkillHelper.GetAlliesInRange(caster, caster.Position, Range))
					ally.StartBuff(BuffId.WhiteTigerHowling_Buff, skill.Level, 0, RoarDuration, caster, skill.Id);
			}

			skill.Run(this.Attack(skill, caster, targets));
		}

		/// <summary>
		/// Strikes the targets once per roar, frightening and stunning them
		/// on every strike.
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
					hitTarget.StartBuff(BuffId.Stun, skill.Level, 0, StunDuration, caster, skill.Id);
					hitTarget.StartBuff(BuffId.Fear, skill.Level, 0, FearDuration, caster, skill.Id);

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
