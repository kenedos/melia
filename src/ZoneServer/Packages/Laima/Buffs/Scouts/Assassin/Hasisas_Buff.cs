using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Assassin
{
	/// <summary>
	/// Handle for the Hasisas Buff, which increases the target's Attack speed and Crit damage
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: 1 if the evasion bonus is applied
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Hasisas_Buff)]
	public class Hasisas_BuffOverride : BuffHandler, IBuffCombatAttackBeforeCalcHandler
	{
		private const float CritBonusRateBase = 0.3f;
		private const float CritBonusRatePerLevel = 0.04f;
		private const float MoveSpeedBonus = 5f;
		private const float HpLossRate = 0.10f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var critBonus = this.GetCritBonus(buff);

			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, MoveSpeedBonus);

			buff.Vars.SetInt("Hasisas.TickCounter", 0);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}

		public override void WhileActive(Buff buff)
		{
			this.ReduceHp(buff);
		}
		public void OnAttackBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			var critBonus = this.GetCritBonus(buff);
			modifier.CritDamageMultiplier += critBonus;
		}

		/// <summary>
		/// Reduces tha buff target's HP.
		/// </summary>
		/// <param name="buff"></param>
		private void ReduceHp(Buff buff)
		{
			if (buff.Target.IsDead)
				return;

			// Assassin2 increases the number of ticks before you take
			// damage.  This is done using a counter in a buff variable.
			// The tick limit is set when applying the buff
			var tickCounter = buff.Vars.GetInt("Hasisas.TickCounter");
			tickCounter++;

			if (tickCounter > buff.Vars.GetInt("Hasisas.TickLimit"))
			{
				// reset the counter and continue
				buff.Vars.SetInt("Hasisas.TickCounter", 0);
			}
			else
			{
				// update the counter and return
				buff.Vars.SetInt("Hasisas.TickCounter", tickCounter);
				return;
			}

			var maxHp = buff.Target.Properties.GetFloat(PropertyName.MHP);
			var halfMaxHp = maxHp / 2;
			var hp = buff.Target.Hp;

			// Reduces hp up to half max hp threshold
			var loss = Math.Min(hp - halfMaxHp, maxHp * HpLossRate);
			if (loss <= 0)
				return;

			// TODO: We probably don't need handling for monsters,
			//   but this should still get updated once we have a
			//   general HP modifier. Or perhaps it should be a
			//   damage hit? TBD.
			if (buff.Target is Character character)
				character.ModifyHp(-loss);
		}

		/// <summary>
		/// Returns the crit bonus to use.
		/// </summary>
		/// <param name="buff"></param>
		/// <returns></returns>
		private float GetCritBonus(Buff buff)
		{
			var bonus = CritBonusRateBase + CritBonusRatePerLevel * buff.NumArg1;

			var byAbility = 1f;
			if (buff.Target.TryGetActiveAbility(AbilityId.Assassin1, out var ability))
				byAbility += ability.Level * 0.005f;
			bonus *= byAbility;

			return bonus;
		}
	}
}
