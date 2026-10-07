using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Archers.Mergen
{
	/// <summary>
	/// Handler for the Down Fall debuff, which strikes the target with a
	/// volley of five arrows at every interval.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.DownFall_Debuff)]
	public class Mergen_DownFall_DebuffOverride : BuffHandler
	{
		private const int ArrowsPerVolley = 5;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime((int)(GetCaptionRatio(buff, 2) * 1000));
		}

		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			if (target.IsDead || buff.Caster is not ICombatEntity caster || caster.IsDead)
				return;

			if (!caster.TryGetSkill(SkillId.Mergen_DownFall, out var skill))
				return;

			var skillHitResult = SCR_SkillHit(caster, target, skill, SkillModifier.MultiHit(ArrowsPerVolley));
			target.TakeDamage(skillHitResult.Damage, caster);

			Send.ZC_SKILL_HIT_INFO(caster, new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
		}
	}
}
