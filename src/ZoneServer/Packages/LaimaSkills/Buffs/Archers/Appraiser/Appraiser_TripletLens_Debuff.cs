using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Archers.Appraiser
{
	/// <summary>
	/// Handler for the Triplet Lense debuff, which damages the target
	/// every second with the caster's Triplet Lense.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.TripletLens_Debuff)]
	public class Appraiser_TripletLens_DebuffOverride : BuffHandler
	{
		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			if (target.IsDead || buff.Caster is not ICombatEntity caster || caster.IsDead)
				return;

			if (!caster.TryGetSkill(SkillId.Appraiser_TripletLens, out var skill))
				return;

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
			Send.ZC_SKILL_HIT_INFO(caster, skillHit);
		}
	}
}
