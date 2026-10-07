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
	/// Handler for the Cannoneer skill Cannon Shot, which fires two
	/// cannonballs at the target that also hit the enemies around it.
	/// </summary>
	/// <remarks>
	/// [Arts] Cannon Shot: Howitzer fires at the ground instead, with a
	/// wider blast.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Cannoneer_CannonShot)]
	public class Cannoneer_CannonShotOverride : IForceSkillHandler, IDynamicCasted
	{
		private const int HitCount = 2;
		private const float BlastRange = 50f;
		private const float HowitzerBlastRange = 85f;
		private static readonly TimeSpan AniTime = TimeSpan.FromMilliseconds(500);
		private static readonly TimeSpan HowitzerAniTime = TimeSpan.FromMilliseconds(1000);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			var howitzer = caster.IsAbilityActive(AbilityId.Cannoneer32);

			if (!howitzer && target == null)
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var blastPos = target?.Position ?? farPos;

			if (!CannoneerSkillHelper.InCannonRange(caster, skill, blastPos))
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
			caster.TurnTowards(blastPos);
			caster.SetAttackState(true);

			var blastRange = howitzer ? HowitzerBlastRange : BlastRange;
			var aniTime = howitzer ? HowitzerAniTime : AniTime;

			var hits = new List<SkillHitInfo>();

			foreach (var hitTarget in CannoneerSkillHelper.GetSplashTargets(caster, skill, blastPos, blastRange, target))
			{
				var skillHitResult = SCR_SkillHit(caster, hitTarget, skill, SkillModifier.MultiHit(HitCount));
				hitTarget.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, hitTarget, skill, skillHitResult, aniTime, TimeSpan.Zero));
			}

			if (howitzer)
				Send.ZC_SKILL_MELEE_GROUND(caster, skill, blastPos, hits);
			else
				Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, hits);
		}
	}
}
