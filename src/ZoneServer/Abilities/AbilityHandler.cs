using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Logging;

namespace Melia.Zone.Abilities
{
	/// <summary>
	/// Base interface for ability handlers.
	/// </summary>
	/// <remarks>
	/// The ability handlers are primarily intended for use with combat
	/// event handlers, as the abilities themself rarely have any actual
	/// behavior. Though when they do, an ability handler might be the
	/// place to put it.
	/// </remarks>
	public interface IAbilityHandler
	{
	}

	/// <summary>
	/// Interface for ability handlers that apply property modifiers when activated.
	/// </summary>
	public interface IAbilityPropertyHandler : IAbilityHandler
	{
		/// <summary>
		/// Called when the ability is activated (learned for passive abilities,
		/// or toggled on for active abilities).
		/// </summary>
		/// <param name="ability">The ability being activated.</param>
		/// <param name="character">The character who has the ability.</param>
		void OnActivate(Ability ability, Character character);

		/// <summary>
		/// Called when the ability is deactivated (removed/unlearned for passive
		/// abilities, or toggled off for active abilities).
		/// </summary>
		/// <param name="ability">The ability being deactivated.</param>
		/// <param name="character">The character who has the ability.</param>
		void OnDeactivate(Ability ability, Character character);
	}

	/// <summary>
	/// Base class for ability handlers that apply property modifiers.
	/// </summary>
	public abstract class AbilityPropertyHandler : IAbilityPropertyHandler
	{
		/// <summary>
		/// Prefix used for storing property modifiers in ability Vars.
		/// </summary>
		public const string ModifierVarPrefix = "Melia.AbilityModifier.";

		/// <summary>
		/// Called when the ability is activated.
		/// </summary>
		public abstract void OnActivate(Ability ability, Character character);

		/// <summary>
		/// Called when the ability is deactivated.
		/// </summary>
		public abstract void OnDeactivate(Ability ability, Character character);

		/// <summary>
		/// Returns the name of the variable used to store modifiers for
		/// the given property.
		/// </summary>
		private static string GetModifierVarName(string propertyName)
			=> ModifierVarPrefix + propertyName;

		/// <summary>
		/// Modifies the property on the character and saves the value in the ability,
		/// to be able to later undo the change.
		/// </summary>
		/// <param name="ability"></param>
		/// <param name="character"></param>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		protected static void AddPropertyModifier(Ability ability, Character character, string propertyName, float value)
		{
			if (!PropertyTable.Exists(character.Properties.Namespace, propertyName))
			{
				Log.Warning($"AbilityPropertyHandler.AddPropertyModifier: {ability.Id} tried to add to property {propertyName} but doesn't exist in namespace: {character.Properties.Namespace}.");
				return;
			}

			var varName = GetModifierVarName(propertyName);
			var oldValue = 0f;

			ability.Properties.TryGetFloat(varName, out oldValue);

			var newValue = oldValue + value;
			ability.Properties.SetFloat(varName, newValue);
			character.Properties.Modify(propertyName, value);

			Send.ZC_OBJECT_PROPERTY(character, propertyName);
		}

		/// <summary>
		/// Removes the property modifier from the character.
		/// </summary>
		/// <param name="ability"></param>
		/// <param name="character"></param>
		/// <param name="propertyName"></param>
		protected static void RemovePropertyModifier(Ability ability, Character character, string propertyName)
		{
			if (!PropertyTable.Exists(character.Properties.Namespace, propertyName))
			{
				Log.Warning($"AbilityPropertyHandler.RemovePropertyModifier: {ability.Id} tried to remove property {propertyName} but doesn't exist in namespace: {character.Properties.Namespace}.");
				return;
			}

			var varName = GetModifierVarName(propertyName);

			if (ability.Properties.TryGetFloat(varName, out var value))
			{
				character.Properties.Modify(propertyName, -value);
				ability.Properties.Remove(varName);

				Send.ZC_OBJECT_PROPERTY(character, propertyName);
			}
		}

		/// <summary>
		/// Updates the property modifier value. Removes old value and applies new one.
		/// </summary>
		/// <param name="ability"></param>
		/// <param name="character"></param>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		protected static void SetPropertyModifier(Ability ability, Character character, string propertyName, float value)
		{
			RemovePropertyModifier(ability, character, propertyName);
			AddPropertyModifier(ability, character, propertyName, value);
		}
	}

	public interface IAbilityCombatAttackBeforeCalcHandler { void OnAttackBeforeCalc(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult); }
	public interface IAbilityCombatDefenseBeforeCalcHandler { void OnDefenseBeforeCalc(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult); }

	public interface IAbilityCombatAttackAfterCalcHandler { void OnAttackAfterCalc(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult); }
	public interface IAbilityCombatDefenseAfterCalcHandler { void OnDefenseAfterCalc(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult); }

	public interface IAbilityCombatAttackBeforeBonusesHandler { void OnAttackBeforeBonuses(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult); }
	public interface IAbilityCombatDefenseBeforeBonusesHandler { void OnDefenseBeforeBonuses(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult); }

	public interface IAbilityCombatAttackAfterBonusesHandler { void OnAttackAfterBonuses(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult); }
	public interface IAbilityCombatDefenseAfterBonusesHandler { void OnDefenseAfterBonuses(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult); }

	// Companion-specific combat hook interfaces - only called when a companion attacks/defends and the owner has this active ability
	public interface IAbilityCombatCompanionAttackBeforeCalcHandler { void OnCompanionAttackBeforeCalc(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult); }
	public interface IAbilityCombatCompanionDefenseBeforeCalcHandler { void OnCompanionDefenseBeforeCalc(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult); }

	public interface IAbilityCombatCompanionAttackAfterCalcHandler { void OnCompanionAttackAfterCalc(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult); }
	public interface IAbilityCombatCompanionDefenseAfterCalcHandler { void OnCompanionDefenseAfterCalc(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult); }

	public interface IAbilityCombatCompanionAttackBeforeBonusesHandler { void OnCompanionAttackBeforeBonuses(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult); }
	public interface IAbilityCombatCompanionDefenseBeforeBonusesHandler { void OnCompanionDefenseBeforeBonuses(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult); }

	public interface IAbilityCombatCompanionAttackAfterBonusesHandler { void OnCompanionAttackAfterBonuses(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult); }
	public interface IAbilityCombatCompanionDefenseAfterBonusesHandler { void OnCompanionDefenseAfterBonuses(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult); }
}
