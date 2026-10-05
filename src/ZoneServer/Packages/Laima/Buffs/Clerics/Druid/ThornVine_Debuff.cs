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
	[BuffHandler(BuffId.ThornVine_Debuff)]
	public class ThornVine_DebuffOverride : BuffHandler
	{
		private const string BleedingCheckedVariable = "Druid.Thorn.BleedingChecked";
		private const int UpdateIntervalMilliseconds = 1000;
		private const int BleedingDurationSeconds = 10;
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target == null)
				return;

			var movementSpeed = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.MSPD));
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, -movementSpeed);
			buff.Vars.SetInt(BleedingCheckedVariable, 0);
			buff.SetUpdateTime(UpdateIntervalMilliseconds);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target == null || buff.Target.IsDead)
				return;

			if (buff.Caster is not Character caster || caster.IsDead)
				return;

			var skill = this.GetSkill(caster, buff);
			var modifier = SkillModifier.Default;
			modifier.DamageMultiplier *= Druid_ThornEnhanceAbility.GetDamageMultiplier(caster);

			var result = SCR_SkillHit(caster, buff.Target, skill, modifier);

			if (result.Result != HitResultType.Dodge && result.Damage > 0)
			{
				buff.Target.TakeDamage(result.Damage, caster);
				this.TryApplyBleeding(buff, caster, skill, result.Damage);
			}

			var hit = new SkillHitInfo(caster, buff.Target, skill, result, HitAnimationTime, TimeSpan.Zero);
			Send.ZC_SKILL_HIT_INFO(caster, new[] { hit });
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target != null)
				RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}

		private void TryApplyBleeding(Buff buff, Character caster, Skill skill, float damagePerTick)
		{
			if (buff.Vars.GetInt(BleedingCheckedVariable) != 0)
				return;

			buff.Vars.SetInt(BleedingCheckedVariable, 1);

			var chance = Druid_ThornBleedingAbility.GetChance(caster);

			if (chance <= 0 || Random.Shared.Next(1, 101) > chance)
				return;

			buff.Target.StartBuff(BuffId.HeavyBleeding, skill.Level, damagePerTick, TimeSpan.FromSeconds(BleedingDurationSeconds), caster, skill.Id);
		}

		private Skill GetSkill(Character caster, Buff buff)
		{
			var skillLevel = Math.Max(1, (int)buff.NumArg1);

			if (caster.TryGetSkill(SkillId.Druid_ThornVine, out var skill))
				return skill;

			return new Skill(caster, SkillId.Druid_ThornVine, skillLevel);
		}
	}
}
