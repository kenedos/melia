using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Buffs.Wizards.Necromancer
{
	/// <summary>
	/// Handles Gather Corpse poison damage over time.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.NecromancerPoison_Debuff)]
	public class NecromancerPoison_DebuffOverride : BuffHandler, IDamageOverTimeBuffHandler
	{
		private const int UpdateIntervalMilliseconds = 1000;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(UpdateIntervalMilliseconds);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target == null || buff.Target.IsDead)
				return;

			if (buff.Caster is not ICombatEntity caster || caster.IsDead)
				return;

			if (!caster.TryGetSkill(SkillId.Necromancer_GatherCorpse, out var skill))
				return;

			var skillHitResult = SCR_SkillHit(caster, buff.Target, skill);

			buff.Target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, buff.Target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
			skillHit.HitEffect = HitEffect.Impact;
			skillHit.ApplyDamage();

			Send.ZC_HIT_INFO(caster, buff.Target, skillHit.HitInfo);
		}
	}
}
