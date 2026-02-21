using System;
using System.Collections.Generic;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Barbarian
{
	/// <summary>
	/// Handler for the Barbarian skill Embowel.
	/// Dual-hit single packet attack that applies bleeding if one target, or knockback to all but one if multiple targets.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Barbarian_Embowel)]
	public class Barbarian_EmbowelOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handles the Embowel skill execution.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SKILL_READY(caster, skill, skillHandle, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 30, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var damageDelay = TimeSpan.FromMilliseconds(175);

			var targetList = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var hits = new List<SkillHitInfo>();

			foreach (var target in targetList.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.Default;
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, TimeSpan.Zero);
				skillHit.HitEffect = HitEffect.Impact;

				hits.Add(skillHit);
			}

			// Apply knockback if multiple targets were hit
			if (hits.Count > 1)
			{
				// Apply knockback to all but the first target
				for (var i = 1; i < hits.Count; i++)
				{
					var hit = hits[i];
					if (hit.HitInfo.Damage > 0 && hit.Target.IsKnockdownable())
					{
						hit.KnockBackInfo = new KnockBackInfo(caster.Position, hit.Target, HitType.KnockDown, 180, 15);
						hit.HitInfo.Type = HitType.KnockDown;
						hit.Target.ApplyKnockdown(caster, skill, hit);
					}
				}
			}

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, hits);
		}
	}
}
