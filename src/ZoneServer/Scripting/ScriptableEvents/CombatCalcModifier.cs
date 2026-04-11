using System;
using Melia.Shared.Game.Const;

namespace Melia.Zone.Scripting.ScriptableEvents
{
	/// <summary>
	/// Declares a method as a combat calculation modification handler for
	/// the specified phase and identifier.
	/// </summary>
	/// <remarks>
	/// Used to automatically set up events for affecting combat
	/// calculations, such as buffs, skills, and abilities. Methods marked
	/// with this attribute are turned into scriptable functions with the
	/// signature "SCR_Combat_{Phase}_{Identifier}" where supported, which
	/// are called as needed during combat calculations.
	///
	/// For maximum flexibility, the phases and identifiers are not
	/// strictly defined and use strings instead. This allows for the
	/// creation of custom phases and identifiers as needed. For standard
	/// values, see <see cref="CombatCalcPhase"/>.
	/// </remarks>
	/// <example>
	/// [CombatCalcModifier(CombatCalcPhase.BeforeCalc_Attack, SkillId.Ranger_SteadyAim)]
	/// public static float OnCombatBeforeCalc(...)
	///
	/// The above method will be registered as a scriptable function with
	/// the name "SCR_Combat_BeforeCalc_Attack_Ranger_SteadyAim" and called
	/// during the BeforeCalc_Attack phase of combat calculations if the
	/// attacker or the target has the relevant skills.
	/// </example>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class CombatCalcModifierAttribute : ScriptableFunctionAttribute
	{
		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="phase"></param>
		/// <param name="identifier"></param>
		public CombatCalcModifierAttribute(string phase, string identifier)
			: base($"SCR_Combat_{phase}_{identifier}")
		{
		}

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="phase"></param>
		/// <param name="identifier"></param>
		public CombatCalcModifierAttribute(string phase, int identifier)
			: this(phase, identifier.ToString())
		{
		}

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="phase"></param>
		/// <param name="skillId"></param>
		public CombatCalcModifierAttribute(string phase, SkillId skillId)
			: this(phase, skillId.ToString())
		{
		}

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="phase"></param>
		/// <param name="buffId"></param>
		public CombatCalcModifierAttribute(string phase, BuffId buffId)
			: this(phase, buffId.ToString())
		{
		}

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="phase"></param>
		/// <param name="abilityId"></param>
		public CombatCalcModifierAttribute(string phase, AbilityId abilityId)
			: this(phase, abilityId.ToString())
		{
		}
	}

	public static class CombatCalcPhase
	{
		/// <summary>
		/// Occurs before the combat calculations begin.
		/// </summary>
		public const string BeforeCalc = "BeforeCalc";

		/// <summary>
		/// Occurs after the basic combat calculations have been
		/// performed, but before bonuses such as race and size
		/// multipliers are applied.
		/// </summary>
		public const string BeforeBonuses = "BeforeBonuses";

		/// <summary>
		/// Occurs after the basic combat calculations have been performed
		/// and all bonuses have been applied. Only crit and block
		/// calculations follow this phase, before AfterCalc is triggered.
		/// </summary>
		public const string AfterBonuses = "AfterBonuses";

		/// <summary>
		/// Occurs after all combat calculations have been performed,
		/// including bonuses.
		/// </summary>
		public const string AfterCalc = "AfterCalc";

		/// <summary>
		/// Occurs when a physical attack is dodged by the target.
		/// </summary>
		public const string OnDodge = "OnDodge";

		/// <summary>
		/// Occurs when a physical attack is blocked by the target.
		/// </summary>
		public const string OnBlock = "OnBlock";

		// Attack-specific phases

		/// <summary>
		/// Occurs before combat calculations, checked on the attacker.
		/// </summary>
		public const string BeforeCalc_Attack = "BeforeCalc_Attack";

		/// <summary>
		/// Occurs after combat calculations, checked on the attacker.
		/// </summary>
		public const string AfterCalc_Attack = "AfterCalc_Attack";

		/// <summary>
		/// Occurs before bonuses, checked on the attacker.
		/// </summary>
		public const string BeforeBonuses_Attack = "BeforeBonuses_Attack";

		/// <summary>
		/// Occurs after bonuses, checked on the attacker.
		/// </summary>
		public const string AfterBonuses_Attack = "AfterBonuses_Attack";

		// Defense-specific phases

		/// <summary>
		/// Occurs before combat calculations, checked on the target.
		/// </summary>
		public const string BeforeCalc_Defense = "BeforeCalc_Defense";

		/// <summary>
		/// Occurs after combat calculations, checked on the target.
		/// </summary>
		public const string AfterCalc_Defense = "AfterCalc_Defense";

		/// <summary>
		/// Occurs before bonuses, checked on the target.
		/// </summary>
		public const string BeforeBonuses_Defense = "BeforeBonuses_Defense";

		/// <summary>
		/// Occurs after bonuses, checked on the target.
		/// </summary>
		public const string AfterBonuses_Defense = "AfterBonuses_Defense";

		// Companion attack phases

		/// <summary>
		/// Occurs before combat calculations when a companion attacks.
		/// </summary>
		public const string BeforeCalc_CompanionAttack = "BeforeCalc_CompanionAttack";

		/// <summary>
		/// Occurs after combat calculations when a companion attacks.
		/// </summary>
		public const string AfterCalc_CompanionAttack = "AfterCalc_CompanionAttack";

		/// <summary>
		/// Occurs before bonuses when a companion attacks.
		/// </summary>
		public const string BeforeBonuses_CompanionAttack = "BeforeBonuses_CompanionAttack";

		/// <summary>
		/// Occurs after bonuses when a companion attacks.
		/// </summary>
		public const string AfterBonuses_CompanionAttack = "AfterBonuses_CompanionAttack";

		// Companion defense phases

		/// <summary>
		/// Occurs before combat calculations when a companion defends.
		/// </summary>
		public const string BeforeCalc_CompanionDefense = "BeforeCalc_CompanionDefense";

		/// <summary>
		/// Occurs after combat calculations when a companion defends.
		/// </summary>
		public const string AfterCalc_CompanionDefense = "AfterCalc_CompanionDefense";

		/// <summary>
		/// Occurs before bonuses when a companion defends.
		/// </summary>
		public const string BeforeBonuses_CompanionDefense = "BeforeBonuses_CompanionDefense";

		/// <summary>
		/// Occurs after bonuses when a companion defends.
		/// </summary>
		public const string AfterBonuses_CompanionDefense = "AfterBonuses_CompanionDefense";
	}
}
