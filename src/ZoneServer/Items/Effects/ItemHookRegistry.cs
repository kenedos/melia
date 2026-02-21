using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

namespace Melia.Zone.Items.Effects
{
	/// <summary>
	/// Registry for dynamically registered item hooks
	/// </summary>
	public class ItemHookRegistry
	{
		// Maps: Character -> ItemObjectId -> HookType -> List<Hooks>
		private readonly Dictionary<Character, Dictionary<long, Dictionary<ItemHookType, List<ItemCombatHook>>>> _hooks = new();

		// Maps: Character -> ItemObjectId -> List<DebuffResistHooks>
		private readonly Dictionary<Character, Dictionary<long, List<ItemDebuffResistHook>>> _debuffResistHooks = new();

		public static ItemHookRegistry Instance { get; } = new ItemHookRegistry();

		/// <summary>
		/// Registers a combat hook for a specific item instance
		/// </summary>
		public void RegisterHook(Character character, Item item, ItemHookType hookType, ItemCombatHook hook)
		{
			lock (_hooks)
			{
				if (!_hooks.ContainsKey(character))
					_hooks[character] = new();

				if (!_hooks[character].ContainsKey(item.ObjectId))
					_hooks[character][item.ObjectId] = new();

				if (!_hooks[character][item.ObjectId].ContainsKey(hookType))
					_hooks[character][item.ObjectId][hookType] = new();

				_hooks[character][item.ObjectId][hookType].Add(hook);
			}
		}

		/// <summary>
		/// Registers a debuff resistance hook for a specific item instance
		/// </summary>
		public void RegisterDebuffResistHook(Character character, Item item, ItemDebuffResistHook hook)
		{
			lock (_debuffResistHooks)
			{
				if (!_debuffResistHooks.ContainsKey(character))
					_debuffResistHooks[character] = new();

				if (!_debuffResistHooks[character].ContainsKey(item.ObjectId))
					_debuffResistHooks[character][item.ObjectId] = new();

				_debuffResistHooks[character][item.ObjectId].Add(hook);
			}
		}

		/// <summary>
		/// Removes all hooks for a specific item
		/// </summary>
		public void UnregisterItem(Character character, long itemObjectId)
		{
			lock (_hooks)
			{
				if (_hooks.TryGetValue(character, out var characterHooks))
					characterHooks.Remove(itemObjectId);
			}

			lock (_debuffResistHooks)
			{
				if (_debuffResistHooks.TryGetValue(character, out var characterDebuffHooks))
					characterDebuffHooks.Remove(itemObjectId);
			}
		}

		/// <summary>
		/// Removes all hooks for a character (called on logout/disconnect)
		/// </summary>
		public void UnregisterCharacter(Character character)
		{
			lock (_hooks)
				_hooks.Remove(character);

			lock (_debuffResistHooks)
				_debuffResistHooks.Remove(character);
		}

		/// <summary>
		/// Invokes all registered hooks of a type for the attacker
		/// </summary>
		public void InvokeAttackHooks(ItemHookType hookType, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker is not Character character)
				return;

			lock (_hooks)
			{
				if (!_hooks.TryGetValue(character, out var characterHooks))
					return;

				foreach (var (itemObjectId, hooksByType) in characterHooks)
				{
					if (!hooksByType.TryGetValue(hookType, out var hooks))
						continue;

					// Get the item instance - check inventory first, then cards
					if (!character.Inventory.TryGetItem(itemObjectId, out var item))
					{
						// Check if it's a card
						var cards = character.Inventory.GetCards();
						if (!cards.Any(kvp => kvp.Value.ObjectId == itemObjectId))
							continue;

						item = cards.First(kvp => kvp.Value.ObjectId == itemObjectId).Value;
					}

					// Invoke all hooks for this item
					foreach (var hook in hooks)
						hook(item, attacker, target, skill, modifier, skillHitResult);
				}
			}
		}

		/// <summary>
		/// Invokes all registered hooks of a type for the defender
		/// </summary>
		public void InvokeDefenseHooks(ItemHookType hookType, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target is not Character character)
				return;

			lock (_hooks)
			{
				if (!_hooks.TryGetValue(character, out var characterHooks))
					return;

				foreach (var (itemObjectId, hooksByType) in characterHooks)
				{
					if (!hooksByType.TryGetValue(hookType, out var hooks))
						continue;

					// Get the item instance - check inventory first, then cards
					if (!character.Inventory.TryGetItem(itemObjectId, out var item))
					{
						// Check if it's a card
						var cards = character.Inventory.GetCards();
						if (!cards.Any(kvp => kvp.Value.ObjectId == itemObjectId))
							continue;

						item = cards.First(kvp => kvp.Value.ObjectId == itemObjectId).Value;
					}

					foreach (var hook in hooks)
						hook(item, attacker, target, skill, modifier, skillHitResult);
				}
			}
		}

		/// <summary>
		/// Invokes Kill hooks when a character kills a monster
		/// </summary>
		public void InvokeKillHooks(Character character, ICombatEntity target)
		{
			lock (_hooks)
			{
				if (!_hooks.TryGetValue(character, out var characterHooks))
					return;

				// Get cards from inventory
				var cards = character.Inventory.GetCards();

				// Iterate through all registered hooks for this character
				foreach (var (itemObjectId, hooksByType) in characterHooks)
				{
					if (!hooksByType.TryGetValue(ItemHookType.Kill, out var hooks))
						continue;

					// Get the card item
					var card = cards.Values.FirstOrDefault(c => c.ObjectId == itemObjectId);
					if (card == null)
						continue;

					// Invoke all Kill hooks for this card
					foreach (var hook in hooks)
						hook(card, character, target, null, null, null);
				}
			}
		}

		/// <summary>
		/// Invokes Dead hooks when a character dies (e.g., auto-revival cards)
		/// </summary>
		public void InvokeDeadHooks(Character character, ICombatEntity killer)
		{
			lock (_hooks)
			{
				if (!_hooks.TryGetValue(character, out var characterHooks))
					return;

				// Get cards from inventory
				var cards = character.Inventory.GetCards();

				// Iterate through all registered hooks for this character
				foreach (var (itemObjectId, hooksByType) in characterHooks)
				{
					if (!hooksByType.TryGetValue(ItemHookType.Dead, out var hooks))
						continue;

					// Get the card item
					var card = cards.Values.FirstOrDefault(c => c.ObjectId == itemObjectId);
					if (card == null)
						continue;

					// Invoke all Dead hooks for this card
					foreach (var hook in hooks)
						hook(card, killer, character, null, null, null);
				}
			}
		}

		/// <summary>
		/// Invokes UseItem event hooks when a character uses an item (potion, etc.)
		/// </summary>
		public void InvokeItemUseHooks(Character character, Item usedItem)
		{
			// Only process potion items
			var isPotionUse = usedItem.Data.HasScript && usedItem.Data.Script.Function switch
			{
				"SCR_USE_ITEM_AddHP1" => true,
				"SCR_USE_ITEM_AddSP1" => true,
				"SCR_USE_ITEM_AddSTA1" => true,
				"SCR_USE_ITEM_AddHPSP1" => true,
				"SCR_USE_ITEM_DotBuff" => true,
				_ => false
			};

			if (!isPotionUse)
				return;

			lock (_hooks)
			{
				if (!_hooks.TryGetValue(character, out var characterHooks))
					return;

				foreach (var (itemObjectId, hooksByType) in characterHooks)
				{
					if (!hooksByType.TryGetValue(ItemHookType.ItemUse, out var hooks))
						continue;

					// Get the card item from cards collection
					var cards = character.Inventory.GetCards();
					var item = cards.Values.FirstOrDefault(c => c.ObjectId == itemObjectId);
					if (item == null)
						continue;

					// Check if card condition matches the used item
					if (!CardMetadataRegistry.Instance.TryGet(itemObjectId, out var metadata))
						continue;

					if (!this.CheckItemUseCondition(metadata, usedItem))
						continue;

					// Invoke all PotionUse hooks for this item
					foreach (var hook in hooks)
						hook(item, character, character, null, null, null);
				}
			}
		}

		/// <summary>
		/// Checks if the UseItem condition matches
		/// </summary>
		private bool CheckItemUseCondition(CardMetadata metadata, Item usedItem)
		{
			if (string.IsNullOrEmpty(metadata.ConditionFunction))
				return true;

			var conditionFunction = metadata.ConditionFunction;
			var conditionArg = metadata.ConditionArg ?? "";

			if (conditionFunction == "SCR_CARDCHECK_POTION_USE")
			{
				if (!usedItem.Data.HasScript)
					return false;

				var scriptFunction = usedItem.Data.Script.Function;

				// For DotBuff, check the strArg to determine potion type
				if (scriptFunction == "SCR_USE_ITEM_DotBuff")
				{
					var buffName = usedItem.Data.Script.StrArg;
					return conditionArg switch
					{
						"HPPOTION" => buffName.Contains("HealHP", StringComparison.OrdinalIgnoreCase),
						"SPPOTION" => buffName.Contains("HealSP", StringComparison.OrdinalIgnoreCase),
						"STAPOTION" => buffName.Contains("HealSTA", StringComparison.OrdinalIgnoreCase) || buffName.Contains("Stamina", StringComparison.OrdinalIgnoreCase),
						_ => false
					};
				}

				return conditionArg switch
				{
					"HPPOTION" => scriptFunction == "SCR_USE_ITEM_AddHP1" || scriptFunction == "SCR_USE_ITEM_AddHPSP1",
					"SPPOTION" => scriptFunction == "SCR_USE_ITEM_AddSP1" || scriptFunction == "SCR_USE_ITEM_AddHPSP1",
					"STAPOTION" => scriptFunction == "SCR_USE_ITEM_AddSTA1",
					_ => false
				};
			}

			return true;
		}

		/// <summary>
		/// Calculates the total debuff resistance rate for a character against a specific buff.
		/// Returns a value between 0.0 and 1.0 representing the % chance to resist the debuff.
		/// </summary>
		public float GetDebuffResistance(Character character, Shared.Game.Const.BuffId buffId)
		{
			var totalResistRate = 0f;

			lock (_debuffResistHooks)
			{
				if (!_debuffResistHooks.TryGetValue(character, out var characterDebuffHooks))
					return 0f;

				foreach (var (itemObjectId, hooks) in characterDebuffHooks)
				{
					// Get the item instance - check inventory first, then cards
					if (!character.Inventory.TryGetItem(itemObjectId, out var item))
					{
						// Check if it's a card
						var cards = character.Inventory.GetCards();
						if (!cards.Any(kvp => kvp.Value.ObjectId == itemObjectId))
							continue;

						item = cards.First(kvp => kvp.Value.ObjectId == itemObjectId).Value;
					}

					// Invoke all debuff resist hooks for this item
					foreach (var hook in hooks)
					{
						var resistRate = hook(item, buffId);
						totalResistRate += resistRate;
					}
				}
			}

			// Cap at 100% resistance
			return Math.Min(totalResistRate, 1.0f);
		}
	}
}
