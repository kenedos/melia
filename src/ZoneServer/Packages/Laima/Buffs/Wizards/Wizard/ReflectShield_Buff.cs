using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Wizard
{
	/// <summary>
	/// Handler for the Reflect Shield buff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.ReflectShield_Buff)]
	public class ReflectShield_BuffOverride : BuffHandler, IBuffCombatDefenseBeforeCalcHandler
	{
		/// <summary>
		/// Applies the buff's effects during the combat calculations.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		public void OnDefenseBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			var skillLevel = buff.NumArg1;
			var byAbility = 1 + (buff.NumArg2 * 0.05f);
			var multiplierReduction = (0.1f + skillLevel * 0.04f);
			multiplierReduction *= byAbility;

			// We originally reduced the damage directly from inside the combat
			// calculations, on BeforeBonuses, but setting the multiplier seems
			// much easier. Is this correct? Who knows.

			multiplierReduction = Math.Min(0.8f, multiplierReduction);

			modifier.DamageMultiplier -= multiplierReduction;

			var maxSp = target.Properties.GetFloat(PropertyName.MSP);
			var currentSp = target.Properties.GetFloat(PropertyName.SP);
			var spRate = 0.05f;
			var spConsume = maxSp * spRate;

			// Always uses remaining sp until zero
			spConsume = Math.Min(currentSp, spConsume);

			target.TrySpendSp(spConsume);
		}

		/// <summary>
		/// Called regularly, for effects that occur during while the
		/// buff is active.
		/// </summary>
		/// <param name="buff"></param>
		public override void WhileActive(Buff buff)
		{
			if (buff.Target is Character player)
			{
				if (player.Sp == 0)
					player.RemoveBuff(BuffId.ReflectShield_Buff);
			}
		}
	}
}
