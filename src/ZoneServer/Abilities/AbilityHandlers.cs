using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities
{
	/// <summary>
	/// Ability handler collection and manager.
	/// </summary>
	public class AbilityHandlers
	{
		private readonly Dictionary<AbilityId, IAbilityHandler> _handlers = new();
		private readonly Dictionary<AbilityId, HandlerPriority> _priorities = new();
		private readonly Dictionary<AbilityId, IAbilityPropertyHandler> _propertyHandlers = new();

		/// <summary>
		/// Initializes the ability handlers, loading all it can find in
		/// the executing assembly.
		/// </summary>
		/// <param name="packages"></param>
		public void Init(PackageManager packages)
		{
			this.LoadHandlersFromAssembly(packages);
		}

		/// <summary>
		/// Loads ability handlers marked with an ability handler attribute in
		/// the current assembly.
		/// </summary>
		/// <param name="packages"></param>
		private void LoadHandlersFromAssembly(PackageManager packages)
		{
			var handlerTypes = Assembly.GetExecutingAssembly().GetTypes()
				.Where(t => typeof(IAbilityHandler).IsAssignableFrom(t) && !t.IsInterface)
				.Where(t => packages.ShouldRegister(t));

			// Process non-package types first, then package types, so
			// that package handlers naturally override base handlers
			// at equal priority.
			var ordered = handlerTypes
				.OrderBy(t => Attribute.IsDefined(t, typeof(PackageAttribute)) ? 1 : 0);

			foreach (var type in ordered)
			{
				foreach (var attr in type.GetCustomAttributes<AbilityHandlerAttribute>())
				{
					var handler = (IAbilityHandler)Activator.CreateInstance(type);
					var abilityIds = attr.Ids;

					foreach (var abilityId in abilityIds)
					{
						if (_priorities.TryGetValue(abilityId, out var priority) && priority > attr.Priority)
							continue;

						this.Register(abilityId, handler);
						_priorities[abilityId] = attr.Priority;
					}
				}
			}
		}

		/// <summary>
		/// Registers a ability handler for the given ability id.
		/// </summary>
		/// <param name="abilityId"></param>
		/// <param name="handler"></param>
		public void Register(AbilityId abilityId, IAbilityHandler handler)
		{
			lock (_handlers)
				_handlers[abilityId] = handler;

			// Also register as property handler if it implements the interface
			if (handler is IAbilityPropertyHandler propertyHandler)
			{
				lock (_propertyHandlers)
					_propertyHandlers[abilityId] = propertyHandler;
			}

			this.LoadCombatEvents(abilityId, handler);
		}

		/// <summary>
		/// Calls the property handler's OnActivate method for the ability.
		/// </summary>
		/// <param name="ability"></param>
		/// <param name="character"></param>
		public void ActivatePropertyHandler(Ability ability, Character character)
		{
			lock (_propertyHandlers)
			{
				if (_propertyHandlers.TryGetValue(ability.Id, out var handler))
					handler.OnActivate(ability, character);
			}
		}

		/// <summary>
		/// Calls the property handler's OnDeactivate method for the ability.
		/// </summary>
		/// <param name="ability"></param>
		/// <param name="character"></param>
		public void DeactivatePropertyHandler(Ability ability, Character character)
		{
			lock (_propertyHandlers)
			{
				if (_propertyHandlers.TryGetValue(ability.Id, out var handler))
					handler.OnDeactivate(ability, character);
			}
		}

		/// <summary>
		/// Returns true if there's a property handler for the given ability.
		/// </summary>
		/// <param name="abilityId"></param>
		/// <returns></returns>
		public bool HasPropertyHandler(AbilityId abilityId)
		{
			lock (_propertyHandlers)
				return _propertyHandlers.ContainsKey(abilityId);
		}

		/// <summary>
		/// Sets up events for the combat events/hooks the handler implements.
		/// </summary>
		/// <param name="abilityId"></param>
		/// <param name="handler"></param>
		private void LoadCombatEvents(AbilityId abilityId, IAbilityHandler handler)
		{
			// Implement hooks via scriptable functions that call the given
			// handler for now. In terms of performance this isn't the absolute
			// best solution, but it is very flexible, and using scriptable
			// functions is idiomatic inside our combat scripting system.

			// Remove any existing combat hooks for this ability first to ensure
			// override handlers completely replace base handlers
			this.RemoveCombatEvents(abilityId);

			void registerAttackFunc(string name, CombatCalcHookFunction func)
			{
				ScriptableFunctions.Combat.Register(name, (attacker, target, skill, modifier, skillHitResult) =>
				{
					if (attacker.TryGetAbility(abilityId, out var ability) && ability.Active)
						func(ability, attacker, target, skill, modifier, skillHitResult);

					return 0;
				});
			}

			void registerDefenseFunc(string name, CombatCalcHookFunction func)
			{
				ScriptableFunctions.Combat.Register(name, (attacker, target, skill, modifier, skillHitResult) =>
				{
					if (target.TryGetAbility(abilityId, out var ability))
						func(ability, attacker, target, skill, modifier, skillHitResult);

					return 0;
				});
			}

			if (handler is IAbilityCombatAttackBeforeCalcHandler beforeCalcAttackHandler) registerAttackFunc("SCR_Combat_BeforeCalc_Attack_" + abilityId, beforeCalcAttackHandler.OnAttackBeforeCalc);
			if (handler is IAbilityCombatDefenseBeforeCalcHandler beforeCalcDefenseHandler) registerDefenseFunc("SCR_Combat_BeforeCalc_Defense_" + abilityId, beforeCalcDefenseHandler.OnDefenseBeforeCalc);

			if (handler is IAbilityCombatAttackAfterCalcHandler afterCalcAttackHandler) registerAttackFunc("SCR_Combat_AfterCalc_Attack_" + abilityId, afterCalcAttackHandler.OnAttackAfterCalc);
			if (handler is IAbilityCombatDefenseAfterCalcHandler afterCalcDefenseHandler) registerDefenseFunc("SCR_Combat_AfterCalc_Defense_" + abilityId, afterCalcDefenseHandler.OnDefenseAfterCalc);

			if (handler is IAbilityCombatAttackBeforeBonusesHandler beforeBonusesAttackHandler) registerAttackFunc("SCR_Combat_BeforeBonuses_Attack_" + abilityId, beforeBonusesAttackHandler.OnAttackBeforeBonuses);
			if (handler is IAbilityCombatDefenseBeforeBonusesHandler beforeBonusesDefenseHandler) registerDefenseFunc("SCR_Combat_BeforeBonuses_Defense_" + abilityId, beforeBonusesDefenseHandler.OnDefenseBeforeBonuses);

			if (handler is IAbilityCombatAttackAfterBonusesHandler afterBonusesAttackHandler) registerAttackFunc("SCR_Combat_AfterBonuses_Attack_" + abilityId, afterBonusesAttackHandler.OnAttackAfterBonuses);
			if (handler is IAbilityCombatDefenseAfterBonusesHandler afterBonusesDefenseHandler) registerDefenseFunc("SCR_Combat_AfterBonuses_Defense_" + abilityId, afterBonusesDefenseHandler.OnDefenseAfterBonuses);

			// Companion-specific combat hooks
			if (handler is IAbilityCombatCompanionAttackBeforeCalcHandler companionBeforeCalcAttackHandler) registerAttackFunc("SCR_Combat_BeforeCalc_CompanionAttack_" + abilityId, companionBeforeCalcAttackHandler.OnCompanionAttackBeforeCalc);
			if (handler is IAbilityCombatCompanionDefenseBeforeCalcHandler companionBeforeCalcDefenseHandler) registerDefenseFunc("SCR_Combat_BeforeCalc_CompanionDefense_" + abilityId, companionBeforeCalcDefenseHandler.OnCompanionDefenseBeforeCalc);

			if (handler is IAbilityCombatCompanionAttackAfterCalcHandler companionAfterCalcAttackHandler) registerAttackFunc("SCR_Combat_AfterCalc_CompanionAttack_" + abilityId, companionAfterCalcAttackHandler.OnCompanionAttackAfterCalc);
			if (handler is IAbilityCombatCompanionDefenseAfterCalcHandler companionAfterCalcDefenseHandler) registerDefenseFunc("SCR_Combat_AfterCalc_CompanionDefense_" + abilityId, companionAfterCalcDefenseHandler.OnCompanionDefenseAfterCalc);

			if (handler is IAbilityCombatCompanionAttackBeforeBonusesHandler companionBeforeBonusesAttackHandler) registerAttackFunc("SCR_Combat_BeforeBonuses_CompanionAttack_" + abilityId, companionBeforeBonusesAttackHandler.OnCompanionAttackBeforeBonuses);
			if (handler is IAbilityCombatCompanionDefenseBeforeBonusesHandler companionBeforeBonusesDefenseHandler) registerDefenseFunc("SCR_Combat_BeforeBonuses_CompanionDefense_" + abilityId, companionBeforeBonusesDefenseHandler.OnCompanionDefenseBeforeBonuses);

			if (handler is IAbilityCombatCompanionAttackAfterBonusesHandler companionAfterBonusesAttackHandler) registerAttackFunc("SCR_Combat_AfterBonuses_CompanionAttack_" + abilityId, companionAfterBonusesAttackHandler.OnCompanionAttackAfterBonuses);
			if (handler is IAbilityCombatCompanionDefenseAfterBonusesHandler companionAfterBonusesDefenseHandler) registerDefenseFunc("SCR_Combat_AfterBonuses_CompanionDefense_" + abilityId, companionAfterBonusesDefenseHandler.OnCompanionDefenseAfterBonuses);
		}

		/// <summary>
		/// Removes all combat event hooks for the given ability.
		/// </summary>
		/// <param name="abilityId"></param>
		private void RemoveCombatEvents(AbilityId abilityId)
		{
			// Remove all possible combat hook functions that may have been registered
			ScriptableFunctions.Combat.Remove("SCR_Combat_BeforeCalc_Attack_" + abilityId);
			ScriptableFunctions.Combat.Remove("SCR_Combat_BeforeCalc_Defense_" + abilityId);
			ScriptableFunctions.Combat.Remove("SCR_Combat_AfterCalc_Attack_" + abilityId);
			ScriptableFunctions.Combat.Remove("SCR_Combat_AfterCalc_Defense_" + abilityId);
			ScriptableFunctions.Combat.Remove("SCR_Combat_BeforeBonuses_Attack_" + abilityId);
			ScriptableFunctions.Combat.Remove("SCR_Combat_BeforeBonuses_Defense_" + abilityId);
			ScriptableFunctions.Combat.Remove("SCR_Combat_AfterBonuses_Attack_" + abilityId);
			ScriptableFunctions.Combat.Remove("SCR_Combat_AfterBonuses_Defense_" + abilityId);

			// Companion hooks
			ScriptableFunctions.Combat.Remove("SCR_Combat_BeforeCalc_CompanionAttack_" + abilityId);
			ScriptableFunctions.Combat.Remove("SCR_Combat_BeforeCalc_CompanionDefense_" + abilityId);
			ScriptableFunctions.Combat.Remove("SCR_Combat_AfterCalc_CompanionAttack_" + abilityId);
			ScriptableFunctions.Combat.Remove("SCR_Combat_AfterCalc_CompanionDefense_" + abilityId);
			ScriptableFunctions.Combat.Remove("SCR_Combat_BeforeBonuses_CompanionAttack_" + abilityId);
			ScriptableFunctions.Combat.Remove("SCR_Combat_BeforeBonuses_CompanionDefense_" + abilityId);
			ScriptableFunctions.Combat.Remove("SCR_Combat_AfterBonuses_CompanionAttack_" + abilityId);
			ScriptableFunctions.Combat.Remove("SCR_Combat_AfterBonuses_CompanionDefense_" + abilityId);
		}

		private delegate void CombatCalcHookFunction(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult);

		/// <summary>
		/// Returns true if a handler was registered for the given ability.
		/// </summary>
		/// <param name="abilityId"></param>
		/// <returns></returns>
		public bool Has(AbilityId abilityId)
		{
			lock (_handlers)
				return _handlers.ContainsKey(abilityId);
		}

		/// <summary>
		/// Returns the ability handler for the given ability. Returns null if
		/// no handler was found.
		/// </summary>
		/// <param name="abilityId"></param>
		/// <returns></returns>
		public IAbilityHandler GetHandler(AbilityId abilityId)
		{
			lock (_handlers)
			{
				if (_handlers.TryGetValue(abilityId, out var handler))
					return handler;
			}

			return null;
		}

		/// <summary>
		/// Returns handler for the given ability via out. Returns false if
		/// no handler was found.
		/// </summary>
		/// <param name="abilityId"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		public bool TryGetHandler(AbilityId abilityId, out IAbilityHandler handler)
		{
			lock (_handlers)
				return _handlers.TryGetValue(abilityId, out handler);
		}
	}
}
