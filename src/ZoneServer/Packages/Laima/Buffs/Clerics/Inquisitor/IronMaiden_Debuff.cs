using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Inquisitor;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Inquisitor
{
	[Package("laima")]
	[BuffHandler(BuffId.IronMaiden_Debuff)]
	public class IronMaiden_DebuffOverride : BuffHandler
	{
		private const int UpdateIntervalMilliseconds = 1000;
		private const float MaximumHpDamageRate = 0.03f;
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target == null)
				return;

			var movementSpeed = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.MSPD));
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, -movementSpeed);
			buff.SetUpdateTime(UpdateIntervalMilliseconds);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target == null || buff.Target.IsDead || buff.Caster is not Character caster || caster.IsDead)
				return;

			if (!this.ShouldDealDamage(buff.Target, caster))
				return;

			var maximumHp = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.MHP));

			if (maximumHp <= 0)
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, 1, 10);

			if (!caster.TryGetSkill(SkillId.Inquisitor_IronMaiden, out var skill))
				skill = new Skill(caster, SkillId.Inquisitor_IronMaiden, skillLevel);

			var damage = Math.Max(1, (int)(maximumHp * MaximumHpDamageRate * Inquisitor_IronMaidenEnhanceAbility.GetDamageMultiplier(caster)));
			var result = SCR_SkillHit(caster, buff.Target, skill, SkillModifier.Default);
			result.Damage = damage;

			if (result.Result != HitResultType.Dodge)
				buff.Target.TakeDamage(damage, caster);
			else
				result.Damage = 0;

			var hit = new SkillHitInfo(caster, buff.Target, skill, result, HitAnimationTime, TimeSpan.Zero);
			Send.ZC_SKILL_HIT_INFO(caster, new[] { hit });
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}

		private bool ShouldDealDamage(ICombatEntity target, Character caster)
		{
			if (caster.IsBuffActive(BuffId.Judgment_Buff))
				return true;

			if (!Enum.TryParse<RaceType>(target.Properties.GetString(PropertyName.RaceType), true, out var race))
				return false;

			return race == RaceType.Velnias;
		}
	}
}
