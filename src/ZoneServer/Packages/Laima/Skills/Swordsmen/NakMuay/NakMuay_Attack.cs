using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Swordsmen.NakMuay
{
	[Package("laima")]
	[SkillHandler(SkillId.NakMuay_Attack)]
	public class NakMuay_AttackOverride : ITargetSkillHandler, IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			this.Cast(skill, caster, target);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, target);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			foreach (var target in targets.Where(t => t != null && !t.IsDead))
				this.Cast(skill, caster, target);
		}

		private void Cast(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (target == null || target.IsDead)
				return;

			if (!caster.TryGetBuff(BuffId.RamMuay_Buff, out _))
				return;

			caster.TurnTowards(target);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, caster.Position, target.Position);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, caster.Position, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, target);

			var skillHitResult = SCR_SkillHit(caster, target, skill);

			this.ApplyRamMuayEnhance(caster, skillHitResult);

			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(
				caster,
				target,
				skill,
				skillHitResult,
				TimeSpan.FromMilliseconds(20),
				TimeSpan.Zero);

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);

			caster.SetAttackState(false);
		}

		private void ApplyRamMuayEnhance(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			if (caster is not Character character)
				return;

			if (!character.Abilities.TryGet(AbilityId.NakMuay5, out var ability) || !ability.Active)
				return;

			var enhanceRate = ability.Level * 0.005f;

			if (ability.Level >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}
	}
}
