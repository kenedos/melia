using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Druid;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Druid
{
	[Package("laima")]
	[BuffHandler(BuffId.Carnivory_Debuff)]
	public class Carnivory_DebuffOverride : BuffHandler
	{
		private const int UpdateIntervalMilliseconds = 1000;
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(UpdateIntervalMilliseconds);
		}

		public override void OnEnd(Buff buff)
		{
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target == null || buff.Target.IsDead)
				return;

			if (buff.Caster is not Character caster || caster.IsDead)
				return;

			var skill = this.GetSkill(caster, buff);
			var modifier = SkillModifier.Default;
			modifier.DamageMultiplier *= Druid_CarnivoryEnhanceAbility.GetDamageMultiplier(caster);

			var result = SCR_SkillHit(caster, buff.Target, skill, modifier);

			if (result.Result != HitResultType.Dodge && result.Damage > 0)
				buff.Target.TakeDamage(result.Damage, caster);

			var hit = new SkillHitInfo(caster, buff.Target, skill, result, HitAnimationTime, TimeSpan.Zero);
			Send.ZC_SKILL_HIT_INFO(caster, new[] { hit });
		}

		private Skill GetSkill(Character caster, Buff buff)
		{
			var skillLevel = Math.Max(1, (int)buff.NumArg1);

			if (caster.TryGetSkill(SkillId.Druid_Carnivory, out var skill))
				return skill;

			return new Skill(caster, SkillId.Druid_Carnivory, skillLevel);
		}
	}
}
