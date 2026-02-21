//--- Melia Script ----------------------------------------------------------
// Card Functions
//--- Description -----------------------------------------------------------
// Scriptable functions that handle card specific behaviors.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Buffs;
using Melia.Zone.Items.Effects;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Melia.Zone.Network;
using Yggdrasil.Logging;
using Yggdrasil.Util;

public class CardFunctionsScript : GeneralScript
{
	[ScriptableFunction]
	public static void SCR_CARDEFFECT_STATUS_RATE_HP_SP(Character character, ICombatEntity target, Item item, float TypeValue, string arg1, string arg2, string arg3)
	{
		SCR_CARDEFFECT_STATUS_RATE(character, target, item, TypeValue, PropertyName.RHP_BM, PropertyName.RHP, "None");
		SCR_CARDEFFECT_STATUS_RATE(character, target, item, TypeValue, PropertyName.RSP_BM, PropertyName.RSP, "None");
	}

	[ScriptableFunction]
	public static void SCR_CARDEFFECT_STATUS_RATE(Character character, ICombatEntity target, Item item, float TypeValue, string arg1, string arg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			CardPropertyModifier.Instance.RemoveAllModifiers(character, item.ObjectId);
			return;
		}

		if (character.Properties.Has(arg1) && character.Properties.Has(arg2))
		{
			var coefficient = arg3 == "None" ? 1 : float.Parse(arg3);
			var value = TypeValue / 100 * coefficient;

			CardPropertyModifier.Instance.AddPropertyModifier(character, item.ObjectId, arg1, value);
		}
	}

	[ScriptableFunction]
	public static void SCR_CARDEFFECT_STATUS_PLUS(Character character, ICombatEntity target, Item item, float TypeValue, string arg1, string arg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			CardPropertyModifier.Instance.RemoveAllModifiers(character, item.ObjectId);
			return;
		}

		var coefficient = arg3 == "None" ? 1 : float.Parse(arg3);
		var value = (float)Math.Floor(TypeValue * coefficient);
		var finalValue = arg1.Contains("RATE", StringComparison.OrdinalIgnoreCase) ? value / 100f : value;
		CardPropertyModifier.Instance.AddPropertyModifier(character, item.ObjectId, arg1, finalValue);
	}

	[ScriptableFunction]
	public static void SCR_CARDEFFECT_ADD_BUFF_MONSTER(Character character, ICombatEntity target, Item item, float TypeValue, string buffName, string buffArg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			ItemHookRegistry.Instance.UnregisterItem(character, item.ObjectId);
			return;
		}

		if (!Enum.TryParse<BuffId>(buffName, out var buffType))
			return;

		if (!CardMetadataRegistry.Instance.TryGet(item.ObjectId, out var metadata))
			return;

		var duration = string.IsNullOrEmpty(arg3) || arg3 == "None" ? 5000 : int.Parse(arg3);
		var useType = (CardUseType)metadata.CardUseType;
		var hookType = ItemHookHelper.GetHookTypeForCardUseType(useType);

		ItemHookRegistry.Instance.RegisterHook(character, item, hookType,
			(itm, attacker, tgt, skill, modifier, skillHitResult) =>
			{
				if (!tgt.IsEnemy(character))
					return;

				var chance = 0.005f * TypeValue;
				var roll = RandomProvider.Get().NextDouble();
				if (roll >= chance)
					return;

				tgt.StartBuff(buffType, 1, skillHitResult.Damage, TimeSpan.FromMilliseconds(duration), character);
			});
	}

	[ScriptableFunction]
	public static void SCR_CARDEFFECT_ADD_BUFF_PC(Character character, ICombatEntity target, Item item, float TypeValue, string buffName, string buffArg2, string arg3)
	{
		if (!Enum.TryParse<BuffId>(buffName, out var buffType))
			return;

		// Unequip - remove buff and unregister hooks
		if (TypeValue <= 0)
		{
			ItemHookRegistry.Instance.UnregisterItem(character, item.ObjectId);
			character.RemoveBuff(buffType);
			return;
		}

		// Get card metadata to check useType
		if (!CardMetadataRegistry.Instance.TryGet(item.ObjectId, out var metadata))
		{
			// Fallback to immediate buff application if no metadata
			var arg2 = 0;
			if (!string.IsNullOrEmpty(buffArg2) && !int.TryParse(buffArg2, out arg2))
				Log.Warning($"SCR_CARDEFFECT_ADD_BUFF_PC: Unsupported buffArg2: {buffArg2} in card {item.Id}-{item.Data.Name}");

			var time = int.Parse(arg3);
			character.StartBuff(buffType, 1, arg2, TimeSpan.FromMilliseconds(time), character);
			return;
		}

		var useType = (CardUseType)metadata.CardUseType;

		// For "Always" type, apply buff immediately (permanent while equipped)
		if (useType == CardUseType.Always)
		{
			var arg2 = 0;
			if (!string.IsNullOrEmpty(buffArg2) && !int.TryParse(buffArg2, out arg2))
				Log.Warning($"SCR_CARDEFFECT_ADD_BUFF_PC: Unsupported buffArg2: {buffArg2} in card {item.Id}-{item.Data.Name}");

			var time = int.Parse(arg3);
			character.StartBuff(buffType, 1, arg2, TimeSpan.FromMilliseconds(time), character);
			return;
		}

		// For conditional types (Attacked, Attack, etc.), register a hook
		var duration = int.Parse(arg3);
		var hookType = ItemHookHelper.GetHookTypeForCardUseType(useType);

		// Get condition parameters from metadata
		var conditionFunction = metadata.ConditionFunction;
		// Probability: use numArg1 if set, otherwise defaults to TypeValue (card level)
		var triggerChance = metadata.ConditionNumArg1 > 0 ? metadata.ConditionNumArg1 : TypeValue;

		ItemHookRegistry.Instance.RegisterHook(character, item, hookType,
			(itm, attacker, tgt, skill, modifier, skillHitResult) =>
			{
				// Check SCR_CARDCHECK_NORMAL probability
				// Original: arg2 >= IMCRandom(1, 100) means if arg2=10, random must be 1-10 (10% chance)
				if (conditionFunction == "SCR_CARDCHECK_NORMAL" || string.IsNullOrEmpty(conditionFunction))
				{
					if (triggerChance < 100)
					{
						var roll = RandomProvider.Get().Next(1, 101); // 1 to 100 inclusive
						if (triggerChance < roll)
							return;
					}
				}

				// Apply the buff (will refresh duration if already active)
				var arg2Val = 0;
				if (!string.IsNullOrEmpty(buffArg2) && buffArg2 != "None")
					int.TryParse(buffArg2, out arg2Val);

				character.StartBuff(buffType, 1, arg2Val, TimeSpan.FromMilliseconds(duration), character);
			});
	}

	[ScriptableFunction]
	public static void SCR_CARDEFFECT_ADD_DAMAGE_RATE_BM(Character character, ICombatEntity target, Item item, float TypeValue, string buffName, string buffArg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			ItemHookRegistry.Instance.UnregisterItem(character, item.ObjectId);
			return;
		}

		// Get card metadata from registry
		if (!CardMetadataRegistry.Instance.TryGet(item.ObjectId, out var metadata))
			return;

		// Parse damage bonus - arg3 is the base value per card level/star
		// Total damage bonus = (arg3 * CardLevel) / 100
		// Example: arg3=2.5, CardLevel=10 -> (2.5 * 10) / 100 = 0.25 (25% bonus)
		var coefficient = float.Parse(arg3);
		var damageBonus = (coefficient * TypeValue) / 100f;  // Multiply by card level, then convert to decimal

		// Get card's useType from metadata
		var useType = (CardUseType)metadata.CardUseType;
		var hookType = ItemHookHelper.GetHookTypeForCardUseType(useType);

		// Get condition function and arguments from metadata
		var conditionFunction = metadata.ConditionFunction ?? "";
		var conditionArg = metadata.ConditionArg ?? "";

		// Create condition checker based on the card's condition script
		Func<ICombatEntity, ICombatEntity, Skill, bool> condition = conditionFunction switch
		{
			"SCR_CARDCHECK_MONRACE" => ItemHookHelper.CheckMonsterRace(conditionArg),
			"SCR_CARDCHECK_MONSIZE" => ItemHookHelper.CheckMonsterSize(conditionArg),
			"SCR_CARDCHECK_MONSIZE_MIDDLE" => ItemHookHelper.CheckMonsterSize(conditionArg),
			"SCR_CARDCHECK_MONATTRIBUTE" => ItemHookHelper.CheckMonsterAttribute(conditionArg),
			"SCR_CARDCHECK_MONRANK" => ItemHookHelper.CheckMonsterRank(conditionArg),
			_ => (attacker, target, skill) => true  // No condition = always active
		};

		// Register the damage hook
		ItemHookHelper.RegisterConditionalDamageHook(character, item, hookType, condition, damageBonus);
	}

	[ScriptableFunction]
	public static void SCR_CARDEFFECT_ADD_BUFF_PC_PLUS(Character character, ICombatEntity target, Item item, float TypeValue, string buffName, string buffArg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			ItemHookRegistry.Instance.UnregisterItem(character, item.ObjectId);
			return;
		}

		if (!Enum.TryParse<BuffId>(buffName, out var buffType))
			return;

		if (!CardMetadataRegistry.Instance.TryGet(item.ObjectId, out var metadata))
			return;

		var duration = int.Parse(arg3);
		var useType = (CardUseType)metadata.CardUseType;
		var hookType = ItemHookHelper.GetHookTypeForCardUseType(useType);

		ItemHookRegistry.Instance.RegisterHook(character, item, hookType,
			(itm, attacker, tgt, skill, modifier, skillHitResult) =>
			{
				character.StartBuff(buffType, 1, 0, TimeSpan.FromMilliseconds(duration), character);
			});
	}

	[ScriptableFunction]
	public static void SCR_CARDEFFECT_ADD_BUFF_PC_PLUS_MANTICEN(Character character, ICombatEntity target, Item item, float TypeValue, string buffName, string buffArg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			ItemHookRegistry.Instance.UnregisterItem(character, item.ObjectId);
			return;
		}

		if (!Enum.TryParse<BuffId>(buffName, out var buffType))
			return;

		var duration = int.Parse(arg3);
		var coefficient = 0.3f;
		var value = TypeValue * coefficient;

		ItemHookRegistry.Instance.RegisterHook(character, item, ItemHookType.ItemUse,
			(itm, attacker, tgt, skill, modifier, skillHitResult) =>
			{
				character.StartBuff(buffType, value, 0, TimeSpan.FromMilliseconds(duration), character);
			});
	}

	[ScriptableFunction]
	public static void SCR_CARDEFFECT_ADD_BUFF_PC_RATE(Character character, ICombatEntity target, Item item, float TypeValue, string buffName, string status, string arg3)
	{
		if (TypeValue <= 0)
		{
			ItemHookRegistry.Instance.UnregisterItem(character, item.ObjectId);
			return;
		}

		if (!Enum.TryParse<BuffId>(buffName, out var buffType))
			return;

		var duration = int.Parse(arg3);

		ItemHookRegistry.Instance.RegisterHook(character, item, ItemHookType.ItemUse,
			(itm, attacker, tgt, skill, modifier, skillHitResult) =>
			{
				var buff = character.StartBuff(buffType, TypeValue, 0, TimeSpan.FromMilliseconds(duration), character);
				buff.Vars.SetString("Melia.Card.PropertyName", status);
			});
	}

	[ScriptableFunction]
	public static void SCR_CARDEFFECT_STATUS_ALL_PLUS(Character character, ICombatEntity target, Item item, float TypeValue, string arg1, string arg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			CardPropertyModifier.Instance.RemoveAllModifiers(character, item.ObjectId);
			return;
		}

		var arg1List = new List<string>();
		if (arg1 != "None")
		{
			arg1List = arg1.Split('/').ToList();
		}

		var arg3List = new List<float>();
		if (arg3 != "None")
		{
			arg3List = arg3.Split('/').Select(float.Parse).ToList();
		}

		if (arg1List != null && arg1List.Count >= 1)
		{
			for (var i = 0; i < arg1List.Count; i++)
			{
				var propertyName = arg1List[i];
				var coefficient = (arg3List.Count > i) ? arg3List[i] : 1;
				var value = (float)Math.Floor(TypeValue * coefficient);
				var finalValue = propertyName.Contains("RATE", StringComparison.OrdinalIgnoreCase) ? value / 100f : value;

				CardPropertyModifier.Instance.AddPropertyModifier(character, item.ObjectId, propertyName, finalValue);
			}
		}
	}

	[ScriptableFunction]
	public static void SCR_CARDEFFECT_STATUS_DUAL_PLUS(Character character, ICombatEntity target, Item item, float TypeValue, string arg1, string arg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			CardPropertyModifier.Instance.RemoveAllModifiers(character, item.ObjectId);
			return;
		}

		var arg1List = new List<string>();
		if (arg1 != "None")
		{
			arg1List = arg1.Split('/').ToList();
		}

		var arg3List = new List<float>();
		if (arg3 != "None")
		{
			arg3List = arg3.Split('/').Select(float.Parse).ToList();
		}

		if (arg1List != null && arg1List.Count >= 1)
		{
			for (var i = 0; i < arg1List.Count; i++)
			{
				var propertyName = arg1List[i];
				var coefficient = (arg3List.Count > i) ? arg3List[i] : 1;
				var value = (float)Math.Floor(TypeValue * coefficient);
				var finalValue = propertyName.Contains("RATE", StringComparison.OrdinalIgnoreCase) ? value / 100f : value;

				CardPropertyModifier.Instance.AddPropertyModifier(character, item.ObjectId, propertyName, finalValue);
			}
		}
	}

	/// <summary>
	/// Restores SP when killing monsters (used by Kill card type) or taking damage (Attacked type)
	/// </summary>
	[ScriptableFunction]
	public static void SCR_CARDEFFECT_RECOVER_SP(Character character, ICombatEntity target, Item card, float TypeValue, string arg1, string arg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			ItemHookRegistry.Instance.UnregisterItem(character, card.ObjectId);
			return;
		}

		// Get card metadata from registry
		if (!CardMetadataRegistry.Instance.TryGet(card.ObjectId, out var metadata))
			return;

		// Parse SP recovery amount from arg1
		if (!float.TryParse(arg1, out var spAmount))
			return;

		// Get card's useType from metadata
		var useType = (CardUseType)metadata.CardUseType;

		// Get condition function and arguments from metadata
		var conditionFunction = metadata.ConditionFunction ?? "";
		var conditionArg = metadata.ConditionArg ?? "";
		var cooldownMs = (int)metadata.ConditionNumArg1;

		// Create condition checker based on the card's condition script
		Func<ICombatEntity, ICombatEntity, Skill, bool> condition = conditionFunction switch
		{
			"SCR_CARDCHECK_MONRACE" => ItemHookHelper.CheckMonsterRace(conditionArg),
			"SCR_CARDCHECK_MONSIZE" => ItemHookHelper.CheckMonsterSize(conditionArg),
			"SCR_CARDCHECK_MONSIZE_COOLDOWN" => ItemHookHelper.CheckMonsterSize(conditionArg),
			"SCR_CARDCHECK_MONATTRIBUTE" => ItemHookHelper.CheckMonsterAttribute(conditionArg),
			"SCR_CARDCHECK_MONRANK" => ItemHookHelper.CheckMonsterRank(conditionArg),
			_ => (attacker, tgt, skill) => true  // No condition = always active
		};

		// Register appropriate hook based on useType
		if (useType == CardUseType.Kill)
		{
			// Register Kill hook
			ItemHookRegistry.Instance.RegisterHook(character, card, ItemHookType.Kill,
				(itm, attacker, tgt, skill, modifier, skillHitResult) =>
				{
					if (condition(attacker, tgt, skill))
					{
						character.Heal(0, spAmount);
					}
				});
		}
		else if (useType == CardUseType.Attacked)
		{
			// Register Defense hook
			var hookType = ItemHookType.DefenseAfterBonuses;

			// Handle cooldown for MONSIZE_COOLDOWN
			DateTime lastTrigger = DateTime.MinValue;

			ItemHookRegistry.Instance.RegisterHook(character, card, hookType,
				(itm, attacker, tgt, skill, modifier, skillHitResult) =>
				{
					// Check cooldown if applicable
					if (conditionFunction.Contains("COOLDOWN"))
					{
						var now = DateTime.Now;
						if ((now - lastTrigger).TotalMilliseconds < cooldownMs)
							return;
						lastTrigger = now;
					}

					if (condition(attacker, tgt, skill))
					{
						character.Heal(0, spAmount);
					}
				});
		}
	}

	/// <summary>
	/// Restores HP when taking damage (used by Attacked card type)
	/// </summary>
	[ScriptableFunction]
	public static void SCR_CARDEFFECT_RECOVER_HP(Character character, ICombatEntity target, Item card, float TypeValue, string arg1, string arg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			ItemHookRegistry.Instance.UnregisterItem(character, card.ObjectId);
			return;
		}

		// Get card metadata from registry
		if (!CardMetadataRegistry.Instance.TryGet(card.ObjectId, out var metadata))
			return;

		// Parse HP recovery amount from arg1
		if (!float.TryParse(arg1, out var hpAmount))
			return;

		// Get card's useType from metadata
		var useType = (CardUseType)metadata.CardUseType;

		// Get condition function and arguments from metadata
		var conditionFunction = metadata.ConditionFunction ?? "";
		var conditionArg = metadata.ConditionArg ?? "";
		var cooldownMs = (int)metadata.ConditionNumArg1;

		// Create condition checker based on the card's condition script
		Func<ICombatEntity, ICombatEntity, Skill, bool> condition = conditionFunction switch
		{
			"SCR_CARDCHECK_MONRACE" => ItemHookHelper.CheckMonsterRace(conditionArg),
			"SCR_CARDCHECK_MONSIZE" => ItemHookHelper.CheckMonsterSize(conditionArg),
			"SCR_CARDCHECK_MONSIZE_COOLDOWN" => ItemHookHelper.CheckMonsterSize(conditionArg),
			"SCR_CARDCHECK_MONATTRIBUTE" => ItemHookHelper.CheckMonsterAttribute(conditionArg),
			"SCR_CARDCHECK_MONRANK" => ItemHookHelper.CheckMonsterRank(conditionArg),
			_ => (attacker, tgt, skill) => true  // No condition = always active
		};

		// Only Attacked useType makes sense for HP recovery
		if (useType == CardUseType.Attacked)
		{
			// Register Defense hook
			var hookType = ItemHookType.DefenseAfterBonuses;

			// Handle cooldown for MONSIZE_COOLDOWN
			DateTime lastTrigger = DateTime.MinValue;

			ItemHookRegistry.Instance.RegisterHook(character, card, hookType,
				(itm, attacker, tgt, skill, modifier, skillHitResult) =>
				{
					// Check cooldown if applicable
					if (conditionFunction.Contains("COOLDOWN"))
					{
						var now = DateTime.Now;
						if ((now - lastTrigger).TotalMilliseconds < cooldownMs)
							return;
						lastTrigger = now;
					}

					if (condition(attacker, tgt, skill))
					{
						character.Heal(hpAmount, 0);
					}
				});
		}
	}

	/// <summary>
	/// Adds attack speed bonus
	/// </summary>
	[ScriptableFunction]
	public static void SCR_CARDEFFECT_ATK_SPEED(Character character, ICombatEntity target, Item item, float TypeValue, string arg1, string arg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			CardPropertyModifier.Instance.RemoveAllModifiers(character, item.ObjectId);
			return;
		}

		var coefficient = string.IsNullOrEmpty(arg3) || arg3 == "None" ? 1 : float.Parse(arg3);
		var value = (float)Math.Floor(TypeValue * coefficient);
		var finalValue = PropertyName.ASPD_BM.Contains("RATE", StringComparison.OrdinalIgnoreCase) ? value / 100f : value;

		CardPropertyModifier.Instance.AddPropertyModifier(character, item.ObjectId, PropertyName.ASPD_BM, finalValue);
	}

	/// <summary>
	/// Modifies aggro/threat generation rate
	/// </summary>
	[ScriptableFunction]
	public static void SCR_CARDEFFECT_ADD_HATE_RATE(Character character, ICombatEntity target, Item item, float TypeValue, string arg1, string arg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			CardPropertyModifier.Instance.RemoveAllModifiers(character, item.ObjectId);
			return;
		}

		var coefficient = string.IsNullOrEmpty(arg3) || arg3 == "None" ? 1 : float.Parse(arg3);
		var value = TypeValue * coefficient / 100f;

		CardPropertyModifier.Instance.AddPropertyModifier(character, item.ObjectId, PropertyName.HateRate_BM, value);
	}

	/// <summary>
	/// Buff PC based on stat (on potion use)
	/// Uses the appropriate CARD_* buff based on buffName parameter (e.g., CARD_DEX, CARD_CON, CARD_MNA)
	/// Note: CARD_DEX, CARD_INT, CARD_STR don't exist in the client, so they fall back to CARD_MNA (CARD_SPR)
	/// </summary>
	[ScriptableFunction]
	public static void SCR_CARDEFFECT_ADD_BUFF_PC_STAT(Character character, ICombatEntity target, Item item, float TypeValue, string buffName, string propertyName, string arg3)
	{
		if (TypeValue <= 0)
		{
			ItemHookRegistry.Instance.UnregisterItem(character, item.ObjectId);
			return;
		}

		// Parse the buff name to get the correct BuffId
		// Non-existent stat buffs fall back to CARD_MNA (only CARD_CON and CARD_MNA exist in the client)
		if (!Enum.TryParse<BuffId>(buffName, out var buffType))
		{
			buffType = BuffId.CARD_MNA;
		}

		var duration = int.Parse(arg3);
		ItemHookRegistry.Instance.RegisterHook(character, item, ItemHookType.ItemUse,
			(itm, attacker, tgt, skill, modifier, skillHitResult) =>
			{
				character.StartBuff(buffType, TypeValue, 0, TimeSpan.FromMilliseconds(duration), character, SkillId.Normal_Attack,
					buff => buff.Vars.SetString("Melia.Card.PropertyName", propertyName));
			});
	}

	/// <summary>
	/// Increases damage against targets with specific buff
	/// </summary>
	[ScriptableFunction]
	public static void SCR_CARDEFFECT_DAMAGE_RATE_BY_BUFF(Character character, ICombatEntity target, Item item, float TypeValue, string buffKeyword, string arg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			ItemHookRegistry.Instance.UnregisterItem(character, item.ObjectId);
			return;
		}

		if (!CardMetadataRegistry.Instance.TryGet(item.ObjectId, out var metadata))
			return;

		var coefficient = string.IsNullOrEmpty(arg3) || arg3 == "None" ? 1 : float.Parse(arg3);
		var damageBonus = (coefficient * TypeValue) / 100f;

		var useType = (CardUseType)metadata.CardUseType;
		var hookType = ItemHookHelper.GetHookTypeForCardUseType(useType);

		var condition = ItemHookHelper.CheckTargetHasBuff(buffKeyword);
		ItemHookHelper.RegisterConditionalDamageHook(character, item, hookType, condition, damageBonus);
	}

	/// <summary>
	/// Treats attacks as back attacks (physical damage only)
	/// Multiple cards stack their trigger chance (e.g., 3x level 10 = 30% chance)
	/// Back attack damage bonus is applied in calc_combat.cs
	/// </summary>
	[ScriptableFunction]
	public static void SCR_CARDEFFECT_AS_BACK_ATTACK(Character character, ICombatEntity target, Item item, float TypeValue, string arg1, string arg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			ItemHookRegistry.Instance.UnregisterItem(character, item.ObjectId);
			return;
		}

		if (!CardMetadataRegistry.Instance.TryGet(item.ObjectId, out var metadata))
			return;

		var useType = (CardUseType)metadata.CardUseType;
		var hookType = ItemHookHelper.GetHookTypeForCardUseType(useType);

		ItemHookRegistry.Instance.RegisterHook(character, item, hookType,
			(itm, attacker, tgt, skill, modifier, skillHitResult) =>
			{
				// Only works for physical damage, not magical
				if (skill.Data.ClassType == SkillClassType.Magic)
					return;

				// Get all equipped cards and find all back attack cards
				var allCards = character.Inventory.GetCards();
				var backAttackCards = new List<(Item card, float chance)>();

				foreach (var cardKvp in allCards)
				{
					var card = cardKvp.Value;
					if (card == null)
						continue;

					// Check if this card uses SCR_CARDEFFECT_AS_BACK_ATTACK
					if (!ZoneServer.Instance.Data.EquipCardDb.TryFind(card.Data.ClassName, out var cardData))
						continue;

					if (cardData.Script?.Function != "SCR_CARDEFFECT_AS_BACK_ATTACK")
						continue;

					var cardLevel = Math.Max(card.CardLevel, 1);

					// Get chance from metadata
					var cardChance = (float)cardLevel; // Default to card level
					if (CardMetadataRegistry.Instance.TryGet(card.ObjectId, out var cardMeta))
					{
						if (cardMeta.ConditionNumArg1 > 0)
							cardChance = cardMeta.ConditionNumArg1;
					}

					backAttackCards.Add((card, cardChance));
				}

				// No back attack cards found
				if (backAttackCards.Count == 0)
					return;

				// Only the card with the lowest ObjectId processes the trigger
				// This prevents multiple triggers from multiple equipped cards
				var primaryCard = backAttackCards.OrderBy(x => x.card.ObjectId).First();
				if (itm.ObjectId != primaryCard.card.ObjectId)
					return;

				// Calculate total chance from all back attack cards
				var totalChance = backAttackCards.Sum(x => x.chance);

				// Check probability (capped at 100%)
				if (totalChance < 100)
				{
					var roll = RandomProvider.Get().Next(1, 101); // 1 to 100 inclusive
					if (totalChance < roll)
						return;
				}

				modifier.ForcedBackAttack = true;
			});
	}

	/// <summary>
	/// Creates shield with fixed value when attacked
	/// Registers a defense hook that triggers on taking damage
	/// Shield values stack across multiple cards, but trigger chance remains the same
	/// </summary>
	[ScriptableFunction]
	public static void SCR_CARDEFFECT_SHIELD(Character character, ICombatEntity target, Item item, float TypeValue,
  string arg1, string arg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			ItemHookRegistry.Instance.UnregisterItem(character, item.ObjectId);
			return;
		}

		// Get card metadata for condition checks
		float triggerChance = 10; // Default 10%
		var hookType = ItemHookType.DefenseBeforeCalc;

		if (CardMetadataRegistry.Instance.TryGet(item.ObjectId, out var metadata))
		{
			var useType = (CardUseType)metadata.CardUseType;
			hookType = useType == CardUseType.Always ? ItemHookType.DefenseBeforeCalc : ItemHookHelper.GetHookTypeForCardUseType(useType);
			triggerChance = metadata.ConditionNumArg1 > 0 ? metadata.ConditionNumArg1 : 10;
		}

		ItemHookRegistry.Instance.RegisterHook(character, item, hookType,
			(itm, attacker, tgt, skill, modifier, skillHitResult) =>
			{
				// Get all equipped cards and find all shield cards
				var allCards = character.Inventory.GetCards();
				var shieldCards = new List<(Item card, float shieldValue)>();

				foreach (var cardKvp in allCards)
				{
					var card = cardKvp.Value;
					if (card == null)
						continue;

					// Check if this card uses SCR_CARDEFFECT_SHIELD
					if (!ZoneServer.Instance.Data.EquipCardDb.TryFind(card.Data.ClassName, out var cardData))
						continue;

					if (cardData.Script?.Function != "SCR_CARDEFFECT_SHIELD")
						continue;

					// Calculate this card's shield value
					var cardCoefficient = 1f;
					if (!string.IsNullOrEmpty(cardData.Script.StrArg3) && cardData.Script.StrArg3 != "None")
						float.TryParse(cardData.Script.StrArg3, out cardCoefficient);

					var cardLevel = Math.Max(card.CardLevel, 1);
					var cardShieldValue = cardLevel * cardCoefficient;

					shieldCards.Add((card, cardShieldValue));
				}

				// No shield cards found
				if (shieldCards.Count == 0)
					return;

				// Only the card with the lowest ObjectId processes the trigger
				// This prevents multiple triggers from multiple equipped cards
				var primaryCard = shieldCards.OrderBy(x => x.card.ObjectId).First();
				if (itm.ObjectId != primaryCard.card.ObjectId)
					return;

				// Calculate total shield value from all shield cards
				var totalShieldValue = shieldCards.Sum(x => x.shieldValue);

				// Check SCR_CARDCHECK_NORMAL probability (single roll for all cards)
				if (triggerChance < 100)
				{
					var roll = RandomProvider.Get().Next(1, 101); // 1 to 100 inclusive
					if (triggerChance < roll)
						return;
				}

				// Apply the shield buff with the combined value from all cards
				character.StartBuff(BuffId.CARD_Shield, totalShieldValue, 0, TimeSpan.FromSeconds(10), character);
			});
	}

	/// <summary>
	/// Creates shield based on percentage of max HP when attacked
	/// Used by Master_card_MariaLeed
	/// </summary>
	[ScriptableFunction]
	public static void SCR_CARDEFFECT_SHIELD_PERHP(Character character, ICombatEntity target, Item item, float TypeValue,
  string arg1, string arg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			ItemHookRegistry.Instance.UnregisterItem(character, item.ObjectId);
			return;
		}

		// arg3 is the percentage coefficient (e.g., 0.005 = 0.5% of MHP per card level)
		var coefficient = string.IsNullOrEmpty(arg3) || arg3 == "None" ? 0.01f : float.Parse(arg3);

		// Get card metadata for condition checks
		if (!CardMetadataRegistry.Instance.TryGet(item.ObjectId, out var metadata))
		{
			ItemHookRegistry.Instance.RegisterHook(character, item, ItemHookType.DefenseAfterBonuses,
				(itm, attacker, tgt, skill, modifier, skillHitResult) =>
				{
					var shieldValue = character.Properties.GetFloat(PropertyName.MHP) * coefficient * TypeValue;
					character.StartBuff(BuffId.CARD_Shield, shieldValue, 0, TimeSpan.FromSeconds(10), character);
				});
			return;
		}

		var useType = (CardUseType)metadata.CardUseType;
		var hookType = useType == CardUseType.Always ? ItemHookType.DefenseAfterBonuses : ItemHookHelper.GetHookTypeForCardUseType(useType);

		// Get condition parameters from metadata
		var conditionFunction = metadata.ConditionFunction;
		// Probability: use numArg1 if set, otherwise defaults to TypeValue (card level)
		var triggerChance = metadata.ConditionNumArg1 > 0 ? metadata.ConditionNumArg1 : TypeValue;

		ItemHookRegistry.Instance.RegisterHook(character, item, hookType,
			(itm, attacker, tgt, skill, modifier, skillHitResult) =>
			{
				// Check SCR_CARDCHECK_NORMAL probability
				if (conditionFunction == "SCR_CARDCHECK_NORMAL" || string.IsNullOrEmpty(conditionFunction))
				{
					if (triggerChance < 100)
					{
						var roll = RandomProvider.Get().Next(1, 101);
						if (triggerChance < roll)
							return;
					}
				}

				// Calculate shield value based on percentage of max HP
				var shieldValue = character.Properties.GetFloat(PropertyName.MHP) * coefficient * TypeValue;
				character.StartBuff(BuffId.CARD_Shield, shieldValue, 0, TimeSpan.FromSeconds(10), character);
			});
	}

	/// <summary>
	/// Auto-revival on death (e.g., Durahan card)
	/// Registers a Dead hook that triggers resurrection
	/// </summary>
	[ScriptableFunction]
	public static void SCR_CARDEFFECT_REVIVAL(Character character, ICombatEntity target,
		Item item, float TypeValue, string arg1, string arg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			ItemHookRegistry.Instance.UnregisterItem(character, item.ObjectId);
			return;
		}

		// Get card metadata for condition checks
		if (!CardMetadataRegistry.Instance.TryGet(item.ObjectId, out var metadata))
		{
			// No metadata, register basic hook without condition
			ItemHookRegistry.Instance.RegisterHook(character, item, ItemHookType.Dead,
				(itm, attacker, tgt, skill, modifier, skillHitResult) =>
				{
					if (!character.IsDead)
						return;

					// Auto-resurrect with full HP
					character.Resurrect(ResurrectOptions.TryAgain);
				});
			return;
		}

		var useType = (CardUseType)metadata.CardUseType;
		var hookType = ItemHookHelper.GetHookTypeForCardUseType(useType);

		// Probability: use numArg1 if set, otherwise defaults to TypeValue (card level)
		var triggerChance = metadata.ConditionNumArg1 > 0 ? metadata.ConditionNumArg1 : TypeValue;
		// Max revival count (default 2 per session, tracked via ExProps)
		var maxRevivalCount = 2;

		ItemHookRegistry.Instance.RegisterHook(character, item, hookType,
			(itm, attacker, tgt, skill, modifier, skillHitResult) =>
			{
				if (!character.IsDead)
					return;

				// SCR_CARDCHECK_DEAD: Check revival count limit
				var revivalCount = (int)character.GetTempVar("DURAHAN_CARD_COUNT");
				if (revivalCount >= maxRevivalCount)
					return;

				// Check trigger probability
				if (triggerChance < 100)
				{
					var roll = RandomProvider.Get().Next(1, 101);
					if (triggerChance < roll)
						return;
				}

				// Increment revival count
				character.SetTempVar("DURAHAN_CARD_COUNT", revivalCount + 1);

				// Auto-resurrect with full HP
				character.Resurrect(ResurrectOptions.TryAgain);
			});
	}

	/// <summary>
	/// Increases item effectiveness (potions) when using potions
	/// Registers a potion use hook that temporarily increases potion effectiveness
	/// </summary>
	[ScriptableFunction]
	public static void SCR_CARDEFFECT_ITEM_EFFECTIVE(Character character, ICombatEntity target, Item item, float TypeValue, string itemType, string arg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			ItemHookRegistry.Instance.UnregisterItem(character, item.ObjectId);
			return;
		}

		if (TypeValue > 0)
		{
			var coefficient = string.IsNullOrEmpty(arg3) || arg3 == "None" ? 1 : float.Parse(arg3);
			var value = TypeValue * coefficient / 100f;

			var propertyName = itemType switch
			{
				"HPPOTION" => PropertyName.HPPotion_BM,
				"SPPOTION" => PropertyName.SPPotion_BM,
				"STAPOTION" => PropertyName.STAPotion_BM,
				_ => ""
			};

			if (string.IsNullOrEmpty(propertyName))
				return;

			ItemHookRegistry.Instance.RegisterHook(character, item, ItemHookType.ItemUse,
					(itm, attacker, tgt, skill, modifier, skillHitResult) =>
					{
						var finalValue = propertyName.Contains("RATE", StringComparison.OrdinalIgnoreCase) ? value / 100f : value;
						CardPropertyModifier.Instance.AddPropertyModifier(character, item.ObjectId, propertyName, finalValue);
					});
		}
	}

	/// <summary>
	/// Provides resistance against specific damage types (Strike/Slash/Aries)
	/// Uses defense hooks similar to how ADD_DAMAGE_RATE_BM works but for defense
	/// </summary>
	[ScriptableFunction]
	public static void SCR_CARDEFFECT_DAMAGE_TYPE_RESISTANCE(Character character, ICombatEntity target, Item item, float TypeValue, string damageType, string arg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			ItemHookRegistry.Instance.UnregisterItem(character, item.ObjectId);
			return;
		}

		if (!CardMetadataRegistry.Instance.TryGet(item.ObjectId, out var metadata))
			return;

		var coefficient = string.IsNullOrEmpty(arg3) || arg3 == "None" ? 1 : float.Parse(arg3);
		var reductionRate = (coefficient * TypeValue) / 100f;

		// Parse the damage type to check against
		if (!Enum.TryParse<SkillAttackType>(damageType, true, out var attackType))
			return;

		// Register defense hook to reduce damage from specific attack types
		ItemHookRegistry.Instance.RegisterHook(character, item, ItemHookType.DefenseAfterBonuses,
			(itm, attacker, tgt, skill, modifier, skillHitResult) =>
			{
				// Check if the skill's attack type matches the resistance type
				var skillAttackType = modifier.AttackType == SkillAttackType.None ? skill.AttackType : modifier.AttackType;

				// Use the right-hand weapon's attack type if a weapon is equipped
				if (attacker.TryGetEquipItem(EquipSlot.RightHand, out var weapon))
				{
					skillAttackType = weapon.Data.AttackType;
				}

				if (skillAttackType == attackType)
				{
					skillHitResult.Damage *= (1f - reductionRate);
				}
			});
	}

	[ScriptableFunction]
	public static void SCR_CARD_EFFECT_RESIST_DEBUFF_RATE(Character character, ICombatEntity target, Item item, float TypeValue, string arg1, string arg2, string arg3)
	{
		if (TypeValue <= 0)
		{
			CardPropertyModifier.Instance.RemoveAllModifiers(character, item.ObjectId);
			ItemHookRegistry.Instance.UnregisterItem(character, item.ObjectId);
			return;
		}

		// Split debuff types by '/' (e.g., "Poison/Blind")
		var debuffTypes = arg1.Split('/');

		// Split resist percentages by '/' (e.g., "5/10")
		var resistPercentages = arg2.Split('/');

		// Create a mapping of debuff type -> resist rate
		var debuffResistMap = new Dictionary<string, float>();
		for (var i = 0; i < debuffTypes.Length; i++)
		{
			var debuffType = debuffTypes[i].Trim();
			var resistRate = 0f;

			// If we have a corresponding percentage in arg2, use it
			if (i < resistPercentages.Length && float.TryParse(resistPercentages[i].Trim(), out var percentValue))
			{
				resistRate = (percentValue * TypeValue) / 100f;
			}
			// Otherwise fall back to TypeValue
			else
			{
				resistRate = TypeValue / 100f;
			}

			debuffResistMap[debuffType] = resistRate;
		}

		// Register debuff resist hook
		ItemHookRegistry.Instance.RegisterDebuffResistHook(character, item, (card, buffId) =>
		{
			// Check if the buff has the matching debuff type tag
			if (!ZoneServer.Instance.Data.BuffDb.TryFind(buffId, out var buffData))
				return 0f;

			// Check if the buff's tags contain any of the debuff types
			foreach (var kvp in debuffResistMap)
			{
				if (buffData.Tags.Contains(kvp.Key, StringComparer.OrdinalIgnoreCase))
					return kvp.Value;
			}

			return 0f;
		});
	}
}
