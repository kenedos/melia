using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Zealot
{
	[Package("laima")]
	[BuffHandler(BuffId.EmphasisTrust_Debuff)]
	public class EmphasisTrust_DebuffOverride : BuffHandler, IBuffOnHitInfoCreatedHandler
	{
		private const string RemainingTriggersVariable = "Zealot.EmphasisTrust.RemainingTriggers";
		private const int MaximumEnhanceLevel = 100;
		private static readonly TimeSpan HitAnimationTime = TimeSpan.FromMilliseconds(100);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Vars.SetInt(RemainingTriggersVariable, Math.Max(1, (int)buff.NumArg2));
		}

		public override void OnEnd(Buff buff)
		{
		}

		public void OnHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			if (buff.Target == null || buff.Target.IsDead || skillHitInfo == null || skillHitInfo.HitResult.Damage <= 0)
				return;

			if (skillHitInfo.Skill.Id == SkillId.Zealot_EmphasisTrust)
				return;

			if (buff.Caster is not Character caster || caster.IsDead || caster.Map != buff.Target.Map)
			{
				buff.Target.RemoveBuff(buff.Id);
				return;
			}

			if (!buff.Vars.TryGetInt(RemainingTriggersVariable, out var remainingTriggers) || remainingTriggers <= 0)
			{
				buff.Target.RemoveBuff(buff.Id);
				return;
			}

			var skillLevel = Math.Clamp((int)buff.NumArg1, 1, 10);

			if (!caster.TryGetSkill(SkillId.Zealot_EmphasisTrust, out var skill))
				skill = new Skill(caster, SkillId.Zealot_EmphasisTrust, skillLevel);

			var modifier = SkillModifier.Default;
			modifier.DamageMultiplier *= this.GetEnhanceMultiplier(caster);

			var result = SCR_SkillHit(caster, buff.Target, skill, modifier);
			buff.Target.TakeDamage(result.Damage, caster);

			var hit = new SkillHitInfo(caster, buff.Target, skill, result, HitAnimationTime, TimeSpan.Zero);
			Send.ZC_SKILL_HIT_INFO(caster, new[] { hit });

			remainingTriggers--;
			buff.Vars.SetInt(RemainingTriggersVariable, remainingTriggers);

			if (remainingTriggers <= 0)
				buff.Target.RemoveBuff(buff.Id);
		}

		private float GetEnhanceMultiplier(Character caster)
		{
			var abilityLevel = Math.Min(caster.Abilities.GetLevel(AbilityId.Zealot11), MaximumEnhanceLevel);

			if (abilityLevel <= 0)
				return 1f;

			var enhancePercent = abilityLevel * 0.5f;

			if (abilityLevel >= MaximumEnhanceLevel)
				enhancePercent += 10f;

			return 1f + enhancePercent / 100f;
		}
	}
}
