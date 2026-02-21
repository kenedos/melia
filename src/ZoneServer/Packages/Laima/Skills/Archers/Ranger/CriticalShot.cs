using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Ranger
{
	/// <summary>
	/// Handler for the Ranger skill Critical Shot.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Ranger_CriticalShot)]
	public class Ranger_CriticalShotOverride : IForceSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);
		}

		/// <summary>
		/// Handles the skill, deal damage and knockback.
		/// </summary>
		public async static Task Activate(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				// No message here, the skill just doesn't activate.
				return;
			}

			if (target == null)
				return;

			if (!caster.InSkillUseRange(skill, target))
				return;

			// Note that cooldown is set in TryActivateDoubleTake
			//skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var hitDelay = TimeSpan.FromMilliseconds(600);
			var skillHitDelay = TimeSpan.Zero;
			var modifier = SkillModifier.MultiHit(2);
			modifier.BonusCritChance = 50;

			await skill.Wait(hitDelay);

			var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
			target.TakeDamage(skillHitResult.Damage, caster);

			var hit = new HitInfo(caster, target, skill, skillHitResult);
			hit.ForceId = ForceId.GetNew();
			hit.ResultType = HitResultType.NoHitScript;

			Send.ZC_NORMAL.PlayForceEffect(hit.ForceId, caster, caster, target, "I_arrow009_red", 0.7f, "arrow_cast", "F_hit_good", 1, "arrow_blow", "SLOW", 800);
			Send.ZC_HIT_INFO(caster, target, hit);
		}

		/// <summary>
		/// Attempt to activate the skill.  Should be called from
		/// Ranger skills that can fire it.
		/// </summary>
		/// <param name="usedSkill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		public static void TryActivateDoubleTake(Skill usedSkill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TryGetSkill(SkillId.Ranger_CriticalShot, out var skill))
				return;

			if (skill.IsOnCooldown || target.IsDead)
				return;

			skill.IncreaseOverheat();
			skill.Run(Activate(skill, caster, target));
		}
	}
}
