using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Handler for StartUp while it charges, which halves the damage taken
	/// and breaks with a 2 second stun and 30% of the remaining HP once 30%
	/// of the max HP has been taken.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.StartUp_Charging_Buff)]
	public class BlossomBlader_StartUp_Charging_BuffOverride : BuffHandler
	{
		private const float DamageTakenRate = 0.5f;
		private const float BreakRate = 0.3f;
		private const float HpPenaltyRate = 0.3f;
		private const string DamageTakenVar = "Melia.BlossomBlader.StartUpDamageTaken";
		private static readonly TimeSpan StunDuration = TimeSpan.FromSeconds(2);

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.StartUp_Charging_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.StartUp_Charging_Buff, out var buff))
				return;

			skillHitResult.Damage *= DamageTakenRate;

			var damageTaken = buff.Vars.GetFloat(DamageTakenVar) + skillHitResult.Damage;
			buff.Vars.SetFloat(DamageTakenVar, damageTaken);

			if (damageTaken < target.MaxHp * BreakRate)
				return;

			skillHitResult.Damage += target.Hp * HpPenaltyRate;

			target.StopBuff(BuffId.StartUp_Charging_Buff);
			target.StartBuff(BuffId.Stun, 1, 0, StunDuration, attacker, buff.SkillId);
			Send.ZC_SKILL_CAST_CANCEL(target);
		}
	}

	/// <summary>
	/// Handler for StartUp once released, which raises critical damage by
	/// the skill's ratio in percent, scaled by how long it was charged.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill level
	/// NumArg2: Charge, from 0 to 1
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.StartUp_Buff)]
	public class BlossomBlader_StartUp_BuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.StartUp_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skillHitResult.Result != HitResultType.Crit || !attacker.TryGetBuff(BuffId.StartUp_Buff, out var buff))
				return;

			skillHitResult.Damage *= 1 + GetCaptionRatio(buff, 1) * buff.NumArg2 / 100f;
		}
	}
}
