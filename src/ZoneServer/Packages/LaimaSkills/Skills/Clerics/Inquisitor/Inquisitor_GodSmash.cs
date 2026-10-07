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
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Inquisitor
{
	/// <summary>
	/// Handler for the Inquisitor skill God Smash, an overhead smash that
	/// ignores 15% of the enemies' defense.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Inquisitor_GodSmash)]
	public class Inquisitor_GodSmashOverride : IGroundSkillHandler
	{
		private const float Length = 70f;
		private const float Width = 30f;
		private const float DefensePenetration = 0.15f;
		private const float DemonPunisherPerLevel = 0.10f;
		private const float IronMaidenMultiplier = 2f;
		private static readonly TimeSpan HitDelay = TimeSpan.FromMilliseconds(300);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			caster.TryGetActiveAbilityLevel(AbilityId.Inquisitor12, out var demonPunisherLevel);
			var crushesMaidens = caster.IsAbilityActive(AbilityId.Inquisitor18);

			var area = new Square(caster.Position, caster.Direction, Length, Width);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, area).LimitBySDR(caster, skill);
			var hits = new List<SkillHitInfo>();

			foreach (var hitTarget in targets)
			{
				var modifier = new SkillModifier();
				modifier.DefensePenetrationRate += DefensePenetration;

				if (demonPunisherLevel > 0 && InquisitorSkillHelper.IsPunishing(caster, hitTarget))
					modifier.DamageMultiplier += demonPunisherLevel * DemonPunisherPerLevel;

				if (crushesMaidens && hitTarget.IsBuffActive(BuffId.IronMaiden_Debuff))
					modifier.FinalDamageMultiplier *= IronMaidenMultiplier;

				var skillHitResult = SCR_SkillHit(caster, hitTarget, skill, modifier);
				hitTarget.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, hitTarget, skill, skillHitResult, HitDelay, TimeSpan.Zero));
			}

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, hits);
		}
	}
}
