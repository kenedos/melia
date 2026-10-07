using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Clerics.Zealot
{
	/// <summary>
	/// Handler for the Fanatic Illusion buff, which shocks enemies around
	/// the Zealot every second while draining SP.
	/// </summary>
	/// <remarks>
	/// With [Arts] Fanatic Illusion: Blind Faith the range doubles and it
	/// strikes every 0.5 seconds for half the damage.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.FanaticIllusion_Buff)]
	public class Zealot_FanaticIllusion_BuffOverride : BuffHandler
	{
		private const float ShockRange = 60f;
		private const float AccuracyRatePerLevel = 0.10f;
		private const float BlindFaithDamageRate = 0.5f;
		private const string LastDrainVar = "Melia.Zealot.FanaticIllusionDrain";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.ResLightning_BM, GetCaptionRatio(buff, 2));
			buff.SetUpdateTime(buff.Target.IsAbilityActive(AbilityId.Zealot16) ? 500 : 1000);
		}

		public override void WhileActive(Buff buff)
		{
			var caster = buff.Target;
			if (caster.IsDead)
				return;

			var isBlindFaith = caster.IsAbilityActive(AbilityId.Zealot16);
			var drainNow = !isBlindFaith || buff.Vars.GetBool(LastDrainVar);
			buff.Vars.SetBool(LastDrainVar, !drainNow);

			if (drainNow && !caster.TrySpendSp(GetCaptionRatio(buff, 1)))
			{
				caster.StopBuff(BuffId.FanaticIllusion_Buff);
				return;
			}

			if (!caster.TryGetSkill(SkillId.Zealot_FanaticIllusion, out var skill))
				return;

			var range = isBlindFaith ? ShockRange * 2 : ShockRange;
			var maxTargets = (int)GetCaptionRatio(buff, 3);
			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, range).Take(maxTargets);
			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				var modifier = SkillModifier.Default;

				if (isBlindFaith)
					modifier.DamageMultiplier *= BlindFaithDamageRate;

				if (caster.IsAbilityActive(AbilityId.Zealot6))
					modifier.HitRateMultiplier += AccuracyRatePerLevel;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.ResLightning_BM);
		}
	}
}
