using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Buffs.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Reduces incoming damage by 50% while StartUp is charging and interrupts it after excessive damage.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.StartUp_Charging_Buff)]
	public class StartUp_Charging_BuffOverride : BuffHandler, IBuffOnHitInfoCreatedHandler
	{
		private const float IncomingDamageReduction = 0.50f;
		private const float InterruptionThresholdPercent = 0.30f;
		private const float HpPenaltyPercent = 0.30f;
		private const string AccumulatedDamageVariable = "Melia.StartUp.AccumulatedDamage";
		private const string InterruptedVariable = "Melia.StartUp.Interrupted";
		private static readonly TimeSpan StunDuration = TimeSpan.FromSeconds(2);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (activationType == ActivationType.Start)
				buff.Vars.SetFloat(AccumulatedDamageVariable, 0);
		}

		public override void OnEnd(Buff buff)
		{
		}

		public void OnHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			if (buff.Vars.GetBool(InterruptedVariable))
				return;

			var originalDamage = skillHitInfo.HitInfo.Damage;
			var accumulatedDamage = buff.Vars.GetFloat(AccumulatedDamageVariable) + originalDamage;
			var maximumHp = buff.Target.Properties.GetFloat(PropertyName.MHP);
			var interruptionThreshold = maximumHp * InterruptionThresholdPercent;

			skillHitInfo.HitInfo.Damage *= 1f - IncomingDamageReduction;
			buff.Vars.SetFloat(AccumulatedDamageVariable, accumulatedDamage);

			if (accumulatedDamage < interruptionThreshold)
				return;

			buff.Vars.SetBool(InterruptedVariable, true);

			if (buff.Caster is ICombatEntity caster && caster.TryGetSkill(SkillId.BlossomBlader_StartUp, out var skill))
				skill.Vars.SetBool("Melia.StartUp.Interrupted", true);

			buff.Target.RemoveBuff(BuffId.StartUp_Charging_Buff);

			var hpPenalty = maximumHp * HpPenaltyPercent;
			buff.Target.TakeDamage(hpPenalty, skillHitInfo.Attacker);
			buff.Target.StartBuff(BuffId.IllusionBlast_Stun_Debuff, 1, 0, StunDuration, skillHitInfo.Attacker, SkillId.BlossomBlader_StartUp);

			Send.ZC_SKILL_CAST_CANCEL(buff.Target);
		}
	}
}
