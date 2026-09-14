using System;

namespace Melia.Test.Balance.Sfr
{
	/// <summary>
	/// The defense curve and the factor axis of SCR_CalculateDamage, as pure
	/// functions, and the closed-form inverse that turns a wanted damage back
	/// into a factor.
	/// </summary>
	/// <remarks>
	/// Traced from SCR_SkillHit into SCR_CalculateDamage in
	/// packages/laima-core/scripts/zone/core/calc_combat.cs. The NewDefenseFormula
	/// branch mitigates the attack first, on a ratio the factor is not part of,
	/// and multiplies the skill factor in afterwards. Damage is therefore affine
	/// in the factor, not curved by it, and inverting needs no iteration.
	/// </remarks>
	public static class SfrDamageCurve
	{
		/// <summary>
		/// The exponent the attack-to-defense ratio is raised to.
		/// </summary>
		public const float DefenseExponent = 1.2f;

		/// <summary>
		/// The factor a measurement press is run at, so every skill is measured
		/// on the same footing.
		/// </summary>
		public const float BaselineFactor = 100f;

		/// <summary>
		/// Returns the share of the attack that survives the defense curve.
		/// </summary>
		/// <param name="attack"></param>
		/// <param name="defense"></param>
		public static float Mitigation(float attack, float defense)
		{
			var ratio = attack / Math.Max(1f, defense);
			var scaled = MathF.Pow(ratio, DefenseExponent);

			return scaled / (scaled + 1f);
		}

		/// <summary>
		/// Returns the damage that reaches the factor multiplication, which is
		/// everything the factor is charged against.
		/// </summary>
		/// <param name="attack"></param>
		/// <param name="defense"></param>
		public static float MitigatedAttack(float attack, float defense)
			=> Math.Max(1f, attack * Mitigation(attack, defense));

		/// <summary>
		/// One skill's damage as a straight line in its factor.
		/// </summary>
		/// <remarks>
		/// Slope is the mitigated attack times everything applied after the
		/// factor; Flat is the post-factor additive terms - SkillAtkAdd,
		/// BonusDamage, the size bonus - carried through the same multipliers.
		/// </remarks>
		/// <param name="Slope"></param>
		/// <param name="Flat"></param>
		public readonly record struct FactorLine(float Slope, float Flat)
		{
			/// <summary>
			/// Returns the damage this line predicts at the given factor.
			/// </summary>
			/// <param name="factor"></param>
			public float DamageAt(float factor)
				=> this.Slope * factor + this.Flat;
		}

		/// <summary>
		/// Returns the line through two measurements taken at different
		/// factors.
		/// </summary>
		/// <remarks>
		/// Two points determine the line exactly, because the relation is
		/// affine by construction. This is a solve, not a fit.
		/// </remarks>
		/// <param name="factorA"></param>
		/// <param name="damageA"></param>
		/// <param name="factorB"></param>
		/// <param name="damageB"></param>
		public static FactorLine Solve(float factorA, float damageA, float factorB, float damageB)
		{
			var span = factorB - factorA;

			if (Math.Abs(span) < 1e-6f)
				throw new ArgumentException("The two measurements must be taken at different factors.", nameof(factorB));

			var slope = (damageB - damageA) / span;

			return new FactorLine(slope, damageA - slope * factorA);
		}

		/// <summary>
		/// Returns the line implied by a single measurement, assuming no
		/// post-factor additive term.
		/// </summary>
		/// <remarks>
		/// Every skill in the data whose atkAdd is zero and whose handler adds
		/// no BonusDamage sits exactly on this, and Solve against a second
		/// measurement is what proves it for one that does not.
		/// </remarks>
		/// <param name="factor"></param>
		/// <param name="damage"></param>
		public static FactorLine Proportional(float factor, float damage)
			=> new(damage / Math.Max(factor, 1e-6f), 0f);

		/// <summary>
		/// Returns the factor that produces the wanted damage, in one step.
		/// </summary>
		/// <param name="line"></param>
		/// <param name="targetDamage"></param>
		public static float SolveFactor(FactorLine line, float targetDamage)
		{
			if (Math.Abs(line.Slope) < 1e-9f)
				throw new InvalidOperationException("The measurement carries no factor slope, so no factor produces the wanted damage.");

			return Math.Max(0f, (targetDamage - line.Flat) / line.Slope);
		}

		/// <summary>
		/// Returns the factor that produces the wanted damage, from a single
		/// baseline measurement.
		/// </summary>
		/// <param name="baselineFactor"></param>
		/// <param name="damageAtBaseline"></param>
		/// <param name="targetDamage"></param>
		public static float SolveFactor(float baselineFactor, float damageAtBaseline, float targetDamage)
			=> SolveFactor(Proportional(baselineFactor, damageAtBaseline), targetDamage);
	}
}
