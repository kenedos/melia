using System;
using System.Collections.Generic;
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

namespace Melia.Zone.Skills.Handlers.Archers.Cannoneer
{
	/// <summary>
	/// Handler for the Cannoneer skill Cannon Barrage, which fires three
	/// volleys of two cannonballs at the target and the enemies around it,
	/// stunning them while Bazooka is active.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Cannoneer_CannonBarrage)]
	public class Cannoneer_CannonBarrageOverride : IForceSkillHandler, IDynamicCasted
	{
		private const int HitsPerVolley = 2;
		private const float BlastRange = 30f;
		private static readonly TimeSpan[] VolleyAniTimes = [TimeSpan.FromMilliseconds(350), TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(650)];
		private static readonly TimeSpan StunDuration = TimeSpan.FromSeconds(2);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (target == null)
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!CannoneerSkillHelper.InCannonRange(caster, skill, target.Position))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var targets = CannoneerSkillHelper.GetSplashTargets(caster, skill, target.Position, BlastRange, target);
			var stun = caster.IsBuffActive(BuffId.Bazooka_Buff);
			var hits = new List<SkillHitInfo>();

			for (var volley = 0; volley < VolleyAniTimes.Length; volley++)
			{
				for (var i = 0; i < targets.Count; i++)
				{
					var hitTarget = targets[i];

					var skillHitResult = SCR_SkillHit(caster, hitTarget, skill, SkillModifier.MultiHit(HitsPerVolley));
					hitTarget.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, hitTarget, skill, skillHitResult, VolleyAniTimes[volley], TimeSpan.Zero);
					skillHit.HitFrameIndex = (byte)volley;
					skillHit.TargetIndex = (byte)i;

					hits.Add(skillHit);
				}
			}

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, hits);

			if (!stun)
				return;

			foreach (var hitTarget in targets)
				hitTarget.StartBuff(BuffId.Stun, 1, 0, StunDuration, caster, skill.Id);
		}
	}
}
