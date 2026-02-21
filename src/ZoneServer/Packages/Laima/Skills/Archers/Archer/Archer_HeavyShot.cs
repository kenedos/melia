using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Archer
{
	/// <summary>
	/// Handles the Archer skill Heavy Shot.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Archer_HeavyShot)]
	public class Archer_HeavyShotOverride : IForceSkillHandler
	{
		/// <summary>
		/// Handles the skill, deal damage and knockback.
		/// </summary>
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

			var damageDelay = TimeSpan.FromMilliseconds(45);
			var skillHitDelay = TimeSpan.Zero;

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
			skillHit.ForceId = ForceId.GetNew();

			if (skillHitResult.Damage > 0 && target.IsKnockdownable())
			{
				skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, target, skill);
				skillHit.HitInfo.Type = HitType.KnockBack;
				target.ApplyKnockback(caster, skill, skillHit);
			}

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);
		}
	}
}
