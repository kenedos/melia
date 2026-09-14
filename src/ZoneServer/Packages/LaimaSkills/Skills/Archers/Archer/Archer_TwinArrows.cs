using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Archers.Ranger;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Archer
{
	/// <summary>
	/// Handles the Archer skill Twin Arrow.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Archer_TwinArrows)]
	public class Archer_TwinArrowsOverride : IForceSkillHandler
	{
		/// <summary>
		/// Handles the skill, do two consecutive hits on the enemy.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			caster.SetAttackState(true);

			if (target == null)
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill, null);
				return;
			}

			if (!caster.InSkillUseRange(skill, target))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill, null);
				return;
			}

			SkillRepeatHelper.Request(skill, caster, target, hitTarget => Attack(skill, caster, hitTarget), () => Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, ForceId.GetNew(), null));
		}

		/// <summary>
		/// Executes a single hit against the target, returning false if the
		/// caster can't pay for it.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		private static bool Attack(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return false;
			}

			skill.IncreaseOverheat();

			var aniTime = TimeSpan.FromMilliseconds(45);
			var skillHitDelay = skill.Properties.HitDelay;

			// TODO: Add more 50% damage to enemies using cloth armor type

			var skillHitResult = SCR_SkillHit(caster, target, skill, SkillModifier.MultiHit(2));
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, aniTime, skillHitDelay);

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);

			Ranger_CriticalShotOverride.TryActivateDoubleTake(skill, caster, target);

			return true;
		}
	}
}
