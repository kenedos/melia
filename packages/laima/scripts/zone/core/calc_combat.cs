//--- Melia Script ----------------------------------------------------------
// Combat Calculation Script
//--- Description -----------------------------------------------------------
// Functions that calculate combat-related values, such as damage.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Buffs.Handlers;
using Melia.Zone.Items.Effects;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Yggdrasil.Extensions;
using Yggdrasil.Logging;
using Yggdrasil.Util;

public class CombatCalculationsScript : GeneralScript
{
	/// <summary>
	/// Returns a random attack value between the min and max values
	/// for the type that matches the given skill (PATK or MATK).
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GetRandomAtk(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var rnd = RandomProvider.Get();

		float min, max;

		if (skill.Data.ClassType == SkillClassType.Responsive)
		{
			// Responsive skills use the higher attack stat (PATK or MATK)
			var minPatk = attacker.Properties.GetFloat(PropertyName.MINPATK) + modifier.BonusPAtk;
			var maxPatk = attacker.Properties.GetFloat(PropertyName.MAXPATK) + modifier.BonusPAtk;
			var avgPatk = (minPatk + maxPatk) / 2f;

			var minMatk = attacker.Properties.GetFloat(PropertyName.MINMATK) + modifier.BonusMAtk;
			var maxMatk = attacker.Properties.GetFloat(PropertyName.MAXMATK) + modifier.BonusMAtk;
			var avgMatk = (minMatk + maxMatk) / 2f;

			if (avgPatk > avgMatk)
			{
				min = minPatk;
				max = maxPatk;
			}
			else
			{
				min = minMatk;
				max = maxMatk;
			}
		}
		else if (skill.Data.ClassType <= SkillClassType.Missile)
		{
			// Physical skills
			min = attacker.Properties.GetFloat(PropertyName.MINPATK) + modifier.BonusPAtk;
			max = attacker.Properties.GetFloat(PropertyName.MAXPATK) + modifier.BonusPAtk;
		}
		else
		{
			// Magical skills
			min = attacker.Properties.GetFloat(PropertyName.MINMATK) + modifier.BonusMAtk;
			max = attacker.Properties.GetFloat(PropertyName.MAXMATK) + modifier.BonusMAtk;
		}

		if (min > max)
			return max;

		return rnd.Between(min, max);
	}

	/// <summary>
	/// Simulates elemental attack properties for monsters with elemental attributes.
	/// For elemental monsters, 50% of their base attack becomes elemental damage,
	/// and this function stores that as a simulated property (e.g., Fire_Atk) in skill.Vars.
	/// </summary>
	/// <param name="attacker">The attacking entity</param>
	/// <param name="baseAttack">The base attack value to split</param>
	/// <param name="skill">The skill being used (stores simulated properties)</param>
	/// <returns>The adjusted base attack (50% for elemental monsters, 100% for others)</returns>
	private float SetMonsterAttributeAtk(ICombatEntity attacker, float baseAttack, Skill skill)
	{
		// Only monsters need simulation
		if (attacker is not Mob mob)
			return baseAttack;

		// Skip neutral/melee monsters (they deal 100% neutral damage)
		if (mob.Data.Attribute == AttributeType.None || mob.Data.Attribute == AttributeType.Melee)
			return baseAttack;

		// For elemental monsters: 50% neutral + 50% elemental
		// Store the elemental portion as a simulated property
		var elementalAtk = baseAttack * 0.5f;
		var elementPropertyName = $"{mob.Data.Attribute}_Atk";
		skill.Vars.Set(elementPropertyName, elementalAtk);

		// Return the neutral portion (50%)
		return baseAttack * 0.5f;
	}

	/// <summary>
	/// Calculates the damage for the given skill if used by the attacker
	/// on the target, factoring in attack and defense properties.
	/// </summary>
	/// <param name="attacker">The combatant attacking the target.</param>
	/// <param name="target">The combatant that's being attacked.</param>
	/// <param name="skill">The skill used for this attack hit.</param>
	/// <param name="skillHitResult">
	/// The result of the hit, which is modified directly to indicate
	/// how the attack turned out. After the call it will reflect the
	/// damage dealt, whether it was a crit, or the attacks was perhaps
	/// dodged, etc. None of this is automatically applied to the target.
	/// </param>
	/// <returns>Returns the resulting damage rounded down.</returns>
	[ScriptableFunction]
	public float SCR_CalculateDamage(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (attacker == null || target == null || skill == null || skill.Data == null || modifier == null || skillHitResult == null)
		{
			Log.Warning("SCR_CalculateDamage: One or more arguments are null (attacker={0}, target={1}, skill={2}, skill.Data={3}, modifier={4}, skillHitResult={5}).",
				attacker != null, target != null, skill != null, skill?.Data != null, modifier != null, skillHitResult != null);
			return 0;
		}

		var SCR_GetRandomAtk = ScriptableFunctions.Combat.Get("SCR_GetRandomAtk");
		var SCR_GetAddAttack = ScriptableFunctions.Combat.Get("SCR_GetAddAttack");
		var SCR_GetDodgeChance = ScriptableFunctions.Combat.Get("SCR_GetDodgeChance");
		var SCR_GetBlockChance = ScriptableFunctions.Combat.Get("SCR_GetBlockChance");
		var SCR_GetCritChance = ScriptableFunctions.Combat.Get("SCR_GetCritChance");
		var SCR_HitCountMultiplier = ScriptableFunctions.Combat.Get("SCR_HitCountMultiplier");
		var SCR_SizeTypeBonus = ScriptableFunctions.Combat.Get("SCR_SizeTypeBonus");
		var SCR_AttributeMultiplier = ScriptableFunctions.Combat.Get("SCR_AttributeMultiplier");
		var SCR_AttackTypeMultiplier = ScriptableFunctions.Combat.Get("SCR_AttackTypeMultiplier");
		var SCR_RaceMultiplier = ScriptableFunctions.Combat.Get("SCR_RaceMultiplier");
		var SCR_Combat_BeforeCalc = ScriptableFunctions.Combat.Get("SCR_Combat_BeforeCalc");
		var SCR_Combat_AfterCalc = ScriptableFunctions.Combat.Get("SCR_Combat_AfterCalc");
		var SCR_Combat_BeforeBonuses = ScriptableFunctions.Combat.Get("SCR_Combat_BeforeBonuses");
		var SCR_Combat_AfterBonuses = ScriptableFunctions.Combat.Get("SCR_Combat_AfterBonuses");

		var rnd = RandomProvider.Get();

		SCR_Combat_BeforeCalc(attacker, target, skill, modifier, skillHitResult);

		// Item hooks - BeforeCalc
		ItemHookRegistry.Instance.InvokeAttackHooks(ItemHookType.AttackBeforeCalc, attacker, target, skill, modifier, skillHitResult);
		ItemHookRegistry.Instance.InvokeDefenseHooks(ItemHookType.DefenseBeforeCalc, attacker, target, skill, modifier, skillHitResult);

		// Check dodge
		var isMagicSkill = skill.Data.ClassType == SkillClassType.Magic;
		var dodgeChance = SCR_GetDodgeChance(attacker, target, skill, modifier, skillHitResult);
		if (!isMagicSkill && rnd.Next(100) < dodgeChance)
		{
			skillHitResult.Result = HitResultType.Dodge;
			return 0;
		}

		// Check block
		// Block needs to be calculated before criticals happen,
		// but the damage must be reduced after defense reductions and modifiers
		var blockChance = SCR_GetBlockChance(attacker, target, skill, modifier, skillHitResult);
		if (!isMagicSkill && rnd.Next(100) < blockChance)
		{
			skillHitResult.Result = HitResultType.Block;

			// Nullify damage on successful classic block
			if (!Feature.IsEnabled("NonNullifyBlocks"))
				return 0;
		}

		// Get attack, including bonuses
		var baseAttack = SCR_GetRandomAtk(attacker, target, skill, modifier, skillHitResult);
		baseAttack = SetMonsterAttributeAtk(attacker, baseAttack, skill);
		var additionalAttack = SCR_GetAddAttack(attacker, target, skill, modifier, skillHitResult);

		var attack = baseAttack + additionalAttack;

		var defPropertyName = skill.Data.ClassType != SkillClassType.Magic ? PropertyName.DEF : PropertyName.MDEF;
		var defense = target.Properties.GetFloat(defPropertyName);

		if (Feature.IsEnabled("NewDefenseFormula"))
		{
			defense -= Math2.Clamp(0, defense, defense * modifier.DefensePenetrationRate);

			var percentIncreaseFactor = 1f + (modifier.DamageMultiplier - 1f);
			var logFactor = (float)Math.Min(1, Math.Log10(Math.Pow(attack / (defense + 1), 0.8) + 1));

			skillHitResult.Damage = percentIncreaseFactor * attack * logFactor;
		}
		else
		{
			skillHitResult.Damage = attack;

			var skillAtkAdd = skill.Properties.GetFloat(PropertyName.SkillAtkAdd);
			skillHitResult.Damage += skillAtkAdd;

			skillHitResult.Damage *= modifier.DamageMultiplier;

			defense -= Math2.Clamp(0, defense, defense * modifier.DefensePenetrationRate);
			skillHitResult.Damage = Math.Max(1, skillHitResult.Damage - defense);
		}

		// Apply the skill factor, raising the damage based on the skill's
		// damage multiplier
		var skillFactor = skill.Properties.GetFloat(PropertyName.SkillFactor);
		if (skillFactor <= 0)
		{
			Log.Warning($"SCR_CalculateDamage: {skill.Id} skill factor is {skillFactor}");
		}
		skillHitResult.Damage *= skillFactor / 100f;

		// After skill factor flat bonuses
		var flatIncrease = modifier.BonusDamage + skill.Properties.GetFloat(PropertyName.SkillAtkAdd);
		skillHitResult.Damage += flatIncrease;

		// Check Crit
		// Crit damage bonus with extra effects (NOT The base crit multiplier!)
		var crtChance = SCR_GetCritChance(attacker, target, skill, modifier, skillHitResult);
		if (rnd.Next(100) < crtChance && skillHitResult.Result != HitResultType.Block)
		{
			skillHitResult.Damage *= modifier.CritDamageMultiplier;
			skillHitResult.Result = HitResultType.Crit;
		}

		SCR_Combat_BeforeBonuses(attacker, target, skill, modifier, skillHitResult);

		// Item hooks - BeforeBonuses
		ItemHookRegistry.Instance.InvokeAttackHooks(ItemHookType.AttackBeforeBonuses, attacker, target, skill, modifier, skillHitResult);
		ItemHookRegistry.Instance.InvokeDefenseHooks(ItemHookType.DefenseBeforeBonuses, attacker, target, skill, modifier, skillHitResult);

		var sizeBonusDamage = SCR_SizeTypeBonus(attacker, target, skill, modifier, skillHitResult);
		if (sizeBonusDamage != 0)
		{
			skillHitResult.Damage += sizeBonusDamage;
		}

		var attrMultiplier = SCR_AttributeMultiplier(attacker, target, skill, modifier, skillHitResult);
		if (attrMultiplier != 1)
		{
			skillHitResult.Damage *= attrMultiplier;
			var attribute = modifier.AttackAttribute == AttributeType.None ? skill.Data.Attribute : modifier.AttackAttribute;
			if (attribute != AttributeType.None)
				Send.ZC_NORMAL.PlayTextEffect(target, attacker, "SHOW_SKILL_ATTRIBUTE", attrMultiplier * 100 - 100, $"{attribute}");
		}

		var atkTypeMultiplier = SCR_AttackTypeMultiplier(attacker, target, skill, modifier, skillHitResult);
		if (atkTypeMultiplier != 1)
		{
			skillHitResult.Damage *= atkTypeMultiplier;
			//if (attacker is Character character) Send.ZC_NORMAL.PlayTextEffectLocal(character.Connection, target, attacker, "SHOW_SKILL_ATTRIBUTE_WEAPON", atkTypeMultiplier * 100 - 100, $"{skill.AttackType}");
		}

		var raceMultiplier = SCR_RaceMultiplier(attacker, target, skill, modifier, skillHitResult);
		if (raceMultiplier != 1)
		{
			skillHitResult.Damage *= raceMultiplier;
		}

		var hitCountMultiplier = SCR_HitCountMultiplier(attacker, target, skill, modifier, skillHitResult);
		if (hitCountMultiplier != 1)
		{
			skillHitResult.Damage *= hitCountMultiplier;
			skillHitResult.HitCount = (int)Math.Round(skillHitResult.HitCount * hitCountMultiplier);
		}

		SCR_Combat_AfterBonuses(attacker, target, skill, modifier, skillHitResult);

		// Item hooks - AfterBonuses
		ItemHookRegistry.Instance.InvokeAttackHooks(ItemHookType.AttackAfterBonuses, attacker, target, skill, modifier, skillHitResult);
		ItemHookRegistry.Instance.InvokeDefenseHooks(ItemHookType.DefenseAfterBonuses, attacker, target, skill, modifier, skillHitResult);

		// Back attack bonus (physical damage only)
		// Applies when attacker is behind target
		// OR when ForcedBackAttack is set (e.g., Werewolf card)
		var isBackAttack = !isMagicSkill && (attacker.IsBehind(target) || modifier.ForcedBackAttack);
		if (isBackAttack)
		{
			var backAttackMultiplier = 1.20f;
			skillHitResult.Damage *= backAttackMultiplier;
		}

		// Block damage reduction (if block is not ignoring full damage)
		if (skillHitResult.Result == HitResultType.Block)
		{
			var blockMultiplier = 0.50f;

			if (target.IsBuffActive(BuffId.HighGuard_Abil_Buff))
				blockMultiplier += 0.20f;

			skillHitResult.Damage -= (skillHitResult.Damage * blockMultiplier);
			Send.ZC_NORMAL.PlayTextEffect(target, attacker, "SHOW_DMG_BLOCK", skillHitResult.Damage, "");
		}

		// Critical damage bonus
		if (skillHitResult.Result == HitResultType.Crit)
		{
			skillHitResult.Damage *= 1.5f;

			var crtAtk = attacker.Properties.GetFloat(PropertyName.CRTATK);
			skillHitResult.Damage += crtAtk;

			Send.ZC_NORMAL.PlayTextEffect(target, attacker, "SHOW_DMG_CRI", skillHitResult.Damage, "");
		}

		skillHitResult.Damage *= modifier.FinalDamageMultiplier;

		SCR_Combat_AfterCalc(attacker, target, skill, modifier, skillHitResult);

		// Let monster-specific functions override the damage calculation,
		// but do it after the basic calculations have been done, so we
		// can utilize them. For example, we can double or half damage
		// that way, or let crit animations happen, even if we might
		// reduce the damage to 1.
		if (target is Mob targetMob)
		{
			if (ScriptableFunctions.Combat.TryGet("SCR_CalculateDamage_Monster_" + targetMob.Data.ClassName, out var mobCalcFunc))
				mobCalcFunc(attacker, target, skill, modifier, skillHitResult);
			else if (targetMob.Properties.TryGetFloat(PropertyName.HPCount, out _) && skillHitResult.Damage > 0)
				skillHitResult.Damage = 1;
		}

		return (int)skillHitResult.Damage;
	}

	/// <summary>
	/// Returns the healing value of skill casted by caster on target.
	/// </summary>
	/// <param name="caster"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_CalculateHeal(ICombatEntity caster, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var SCR_GetRandomAtk = ScriptableFunctions.Combat.Get("SCR_GetRandomAtk");

		var byAtk = SCR_GetRandomAtk(caster, target, skill, modifier, skillHitResult);
		byAtk /= 4;

		var byHealPower = caster.Properties.GetFloat(PropertyName.HEAL_PWR);
		var bySkillFactor = skill.Properties.GetFloat(PropertyName.SkillFactor);

		// We assume all heals have 5% of target's max HP as base value
		var byMaxHp = (target.MaxHp * 0.05f);

		var healAmount = (float)Math.Floor(byMaxHp + ((byHealPower * bySkillFactor) / 100f) + byAtk);

		// TODO: Add hooks to this method

		// Caster buffs/debuffs
		if (caster.TryGetBuff(BuffId.PatronSaint_Buff, out var patronSaitBuff))
		{
			var healBonus = patronSaitBuff.NumArg2;
			healAmount *= 1f + healBonus;
		}

		// Target buffs/debuffs
		if (target.TryGetBuff(BuffId.Daino_Buff, out var dainoBuff))
		{
			var healBonus = dainoBuff.NumArg2;
			healAmount *= 1f + healBonus;
		}
		if (caster.TryGetAbility(AbilityId.Priest10, out var healBonusAbility))
		{
			healAmount *= 1f + (healBonusAbility.Level * 0.02f);
		}
		if (target.TryGetBuff(BuffId.UC_curse, out var _))
		{
			healAmount *= 0f;
		}
		// CurseOfWeakness_Debuff: Reduces healing received by 30% + 3% * SkillLv
		if (target.TryGetBuff(BuffId.CurseOfWeakness_Debuff, out var curseOfWeaknessBuff))
		{
			var skillLevel = curseOfWeaknessBuff.NumArg1;
			// Base reduction: 30% + 3% * SkillLv
			var baseReduction = 0.30f + (0.03f * skillLevel);

			// Bokor31 ability: 0.05% multiplier per ability level
			var abilityMultiplier = 1f;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Bokor31, out var abilityLevel))
				abilityMultiplier += abilityLevel * 0.0005f;

			// Calculate final reduction, capped at 90%
			var healingReduction = Math.Min(0.90f, baseReduction * abilityMultiplier);
			healAmount *= (1f - healingReduction);
		}
		// BleedingPierce_Abil_Debuff (Hunter25): Reduces healing received by 50%
		if (target.TryGetBuff(BuffId.BleedingPierce_Abil_Debuff, out var _))
		{
			healAmount *= 0.5f;
		}

		// Cap healing to zero (cannot be negative)
		healAmount = Math.Max(0, healAmount);

		return healAmount;
	}

	/// <summary>
	/// Returns a multiplier for additional elemental attack based on buffs/debuffs.
	/// Applied individually to each elemental additional attack (Fire_Atk, Ice_Atk, etc.).
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GetAddAttackMultiplier(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var multiplier = 1f;

		// Check for Conviction_Debuff - increases elemental additional attack
		if (target.TryGetBuff(BuffId.Conviction_Debuff, out var convictionBuff))
		{
			var attackAttribute = modifier.AttackAttribute == AttributeType.None ? skill.Data.Attribute : modifier.AttackAttribute;

			// Apply multiplier to elemental attacks only
			if (attackAttribute == AttributeType.Fire ||
				attackAttribute == AttributeType.Ice ||
				attackAttribute == AttributeType.Lightning ||
				attackAttribute == AttributeType.Earth ||
				attackAttribute == AttributeType.Poison)
			{
				multiplier += 0.20f + (convictionBuff.NumArg1 * 0.03f);
			}
		}

		// Check for ResistElements_Buff - decreases elemental additional attack
		if (target.TryGetBuff(BuffId.ResistElements_Buff, out var resistElementsBuff))
		{
			var attackAttribute = modifier.AttackAttribute == AttributeType.None ? skill.Data.Attribute : modifier.AttackAttribute;

			// Apply reduction to elemental attacks only
			if (attackAttribute == AttributeType.Fire ||
				attackAttribute == AttributeType.Ice ||
				attackAttribute == AttributeType.Lightning ||
				attackAttribute == AttributeType.Earth ||
				attackAttribute == AttributeType.Poison)
			{
				// Base reduction: 25% + 2.5% * SkillLv
				var baseReduction = 0.25f + (resistElementsBuff.NumArg1 * 0.025f);

				// Paladin37 ability: 0.5% multiplier per ability level (1.5x at level 100)
				var abilityMultiplier = 1f;
				if (target.TryGetActiveAbilityLevel(AbilityId.Paladin37, out var abilityLevel))
					abilityMultiplier += abilityLevel * 0.005f;

				// Calculate final reduction, capped at 90%
				var finalReduction = Math.Min(0.90f, baseReduction * abilityMultiplier);

				// Reduce additional attack multiplier (minimum 0.1 to keep at least 10%)
				multiplier *= (1f - finalReduction);
			}
		}

		// Future: Add other buff/debuff multipliers here

		return multiplier;
	}

	/// <summary>
	/// Returns additional property attack that the attacker may have
	/// over the target, considering target's property resistances.
	/// This includes:
	/// - Player elemental/attack type/race/size bonuses from equipment
	/// - Monster simulated elemental attacks
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	///	<param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GetAddAttack(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var totalAdditionalAttack = 0f;

		// Calculate attribute attack bonuses
		var attributeTypeStrList = new[] { "Fire", "Ice", "Earth", "Lightning", "Poison", "Holy", "Dark", "Soul" };
		var SCR_GetAddAttackMultiplier = ScriptableFunctions.Combat.Get("SCR_GetAddAttackMultiplier");
		foreach (var attributeTypeStr in attributeTypeStrList)
		{
			var atkPropertyName = $"{attributeTypeStr}_Atk";
			var resistPropertyName = $"Res{attributeTypeStr}";

			// Check for simulated monster attribute
			// then fall back to player property
			var atk = skill.Vars.TryGet(atkPropertyName, out float simulatedAtk)
				? simulatedAtk
				: attacker.Properties.GetFloat(atkPropertyName, 0);

			// Gets target resistance
			var atkResist = 0f;
			if (target.Properties.TryGetFloat(resistPropertyName, out var resist))
				atkResist += resist;

			var attributeAtkBonus = atk - atkResist;

			// For monsters simulated attribute attacks, we cannot reduce
			// their atk below zero.
			//
			// For players using AttributeATk - AttributeDefense we CAN have
			// negative additional attack values, as we want to potentially
			// reduce a fireball's damage all the way to 0 if we have enough ResFire.
			if (simulatedAtk > 0)
			{
				attributeAtkBonus = Math.Max(0, attributeAtkBonus);
			}

			// Apply attribute multiplier
			// Note: Workaround by creating new skill and forcing the
			// attribute type
			var sk = new Skill(attacker, SkillId.Normal_Attack);
			var skMod = new SkillModifier();
			var parsed = Enum.TryParse(attributeTypeStr, out AttributeType attributeType);
			skMod.AttackAttribute = parsed ? attributeType : AttributeType.None;
			var attributeMultiplier = SCR_AttributeMultiplier(attacker, target, sk, skMod, null);
			attributeAtkBonus *= attributeMultiplier;

			// Apply additional attack multiplier for buffs/debuffs (ResistElements, Conviction, ..)
			var addAtkMultiplier = SCR_GetAddAttackMultiplier(attacker, target, sk, skMod, skillHitResult);
			attributeAtkBonus *= addAtkMultiplier;

			// Add the bonus if attacker's bonus exists or if the skill is of
			// given attribute, reducing incoming damage from a negative bonus
			var attackerAttr = modifier.AttackAttribute == AttributeType.None ? skill.Data.Attribute : modifier.AttackAttribute;
			if ((atk > 0) || (parsed && (attackerAttr == attributeType)))
				totalAdditionalAttack += attributeAtkBonus;
		}

		// Monsters only have elemental attacks, not attack type/race/size bonuses
		if (attacker is not Character)
			return totalAdditionalAttack;

		// Calculate attack type bonuses (Players only)
		var attackTypeStrList = new[] { "Slash", "Aries", "Strike" };
		foreach (var attackTypeStr in attackTypeStrList)
		{
			if (skill.Data.AttackType == SkillAttackType.Magic)
				continue;

			var atkPropertyName = $"{attackTypeStr}_Atk";
			var resistPropertyName = $"Def{attackTypeStr}";

			var atk = attacker.Properties.GetFloat(atkPropertyName, 0);
			var atkResist = target.Properties.GetFloat(resistPropertyName, 0);

			var attackTypeBonus = atk - atkResist;

			// Apply attack type multiplier
			// Note: Workaround by creating new skill and forcing the
			// attack type
			var sk = new Skill(attacker, SkillId.Normal_Attack);
			var skMod = new SkillModifier();
			var parsed = Enum.TryParse(attackTypeStr, out SkillAttackType attackType);
			skMod.AttackType = parsed ? attackType : SkillAttackType.None;
			var atkTypeMultiplier = SCR_AttackTypeMultiplier(attacker, target, sk, skMod, null);
			attackTypeBonus *= atkTypeMultiplier;

			// Add the bonus if attacker's bonus exists or if the skill is of
			// given attack type, reducing incoming damage from a negative bonus
			var attackerAtkType = modifier.AttackType == SkillAttackType.None ? skill.AttackType : modifier.AttackType;
			if ((atk > 0) || (parsed && (attackerAtkType == attackType)))
				totalAdditionalAttack += attackTypeBonus;
		}

		// Calculate other attack property bonuses
		// RaceType
		if (target.Race != RaceType.None)
			totalAdditionalAttack += attacker.Properties.GetFloat(target.Race.ToString() + "_Atk", 0);

		// Size
		switch (target.EffectiveSize)
		{
			case SizeType.S:
				totalAdditionalAttack += attacker.Properties.GetFloat(PropertyName.SmallSize_Atk, 0);
				break;
			case SizeType.M:
				totalAdditionalAttack += attacker.Properties.GetFloat(PropertyName.MiddleSize_Atk, 0);
				break;
			case SizeType.L:
			case SizeType.XL:
				totalAdditionalAttack += attacker.Properties.GetFloat(PropertyName.LargeSize_Atk, 0);
				break;
			case SizeType.XXL:
				totalAdditionalAttack += attacker.Properties.GetFloat(PropertyName.BOSS_ATK, 0);
				break;
		}

		// Armor Material Type
		switch (target.ArmorMaterial)
		{
			case ArmorMaterialType.Cloth:
				totalAdditionalAttack += attacker.Properties.GetFloat(PropertyName.ADD_CLOTH, 0);
				break;
			case ArmorMaterialType.Leather:
				totalAdditionalAttack += attacker.Properties.GetFloat(PropertyName.ADD_LEATHER, 0);
				break;
			case ArmorMaterialType.Iron:
				totalAdditionalAttack += attacker.Properties.GetFloat(PropertyName.ADD_IRON, 0);
				break;
			case ArmorMaterialType.Ghost:
				totalAdditionalAttack += attacker.Properties.GetFloat(PropertyName.ADD_GHOST, 0);
				break;
		}

		// Other additional ATK Adds (i.e. boss cards)
		totalAdditionalAttack += attacker.Properties.GetFloat(PropertyName.Add_Damage_Atk, 0);

		return totalAdditionalAttack;
	}

	/// <summary>
	/// Returns a multiplier for the hit count based on the skill used
	/// and the entity's states.
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	///	<param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_HitCountMultiplier(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		// We originally used this method to "hardcode" hit counts for certain
		// skills and scenarios, but the decision what hit count to use was
		// since moved to the skill handlers. We'll keep this function around
		// for the moment though, if only to allow overriding it from scripts.

		return modifier.HitCount;
	}

	/// <summary>
	/// Returns a damage multiplier for the skill used on the target.
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_SizeTypeBonus(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (skill.Data.ClassType == SkillClassType.Magic)
			return 0;

		if (attacker is not Character character)
			return 0;

		var weapon = character.Inventory.GetEquip(EquipSlot.RightHand);

		var targetSize = SizeType.M;
		if (target is Mob mob)
			targetSize = mob.Data.Size;

		if (targetSize == SizeType.S) return weapon.Data.SmallSizeBonus;
		if (targetSize == SizeType.M) return weapon.Data.MediumSizeBonus;
		if (targetSize == SizeType.L) return weapon.Data.LargeSizeBonus;

		return 0;
	}

	/// <summary>
	/// Returns a damage multiplier for the skill used on the target.
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_AttributeMultiplier(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var attackerAttr = modifier.AttackAttribute == AttributeType.None ? skill.Data.Attribute : modifier.AttackAttribute;
		var targetAttr = modifier.DefenseAttribute == AttributeType.None ? target.Attribute : modifier.DefenseAttribute;

		//Log.Debug($"Attacker attribute: {attackerAttr}");
		//Log.Debug($"Defender attribute: {targetAttr}");

		if (!Feature.IsEnabled("AttributeBonusRevamp"))
		{
			if (attackerAttr == AttributeType.Fire)
			{
				if (targetAttr == AttributeType.Fire) return 0.75f;
				if (targetAttr == AttributeType.Earth) return 1.5f;
			}
			else if (attackerAttr == AttributeType.Ice)
			{
				if (targetAttr == AttributeType.Fire) return 1.5f;
				if (targetAttr == AttributeType.Ice) return 0.75f;
			}
			else if (attackerAttr == AttributeType.Lightning)
			{
				if (targetAttr == AttributeType.Ice) return 1.5f;
				if (targetAttr == AttributeType.Lightning) return 0.75f;
			}
			else if (attackerAttr == AttributeType.Earth)
			{
				if (targetAttr == AttributeType.Lightning) return 1.5f;
				if (targetAttr == AttributeType.Earth) return 0.75f;
			}
			else if (attackerAttr == AttributeType.Poison)
			{
				if (targetAttr == AttributeType.Fire) return 1.125f;
				if (targetAttr == AttributeType.Ice) return 1.125f;
				if (targetAttr == AttributeType.Lightning) return 1.125f;
				if (targetAttr == AttributeType.Earth) return 1.125f;
				if (targetAttr == AttributeType.Poison) return 0.75f;
			}
			else if (attackerAttr == AttributeType.Holy)
			{
				if (targetAttr == AttributeType.Holy) return 0.75f;
				if (targetAttr == AttributeType.Dark) return 1.5f;
			}
			else if (attackerAttr == AttributeType.Dark)
			{
				if (targetAttr == AttributeType.Holy) return 1.5f;
				if (targetAttr == AttributeType.Dark) return 0.75f;
			}
			else if (attackerAttr == AttributeType.Soul)
			{
				if (targetAttr == AttributeType.Holy) return 1.25f;
				if (targetAttr == AttributeType.Dark) return 1.25f;
				if (targetAttr == AttributeType.Soul) return 1.5f;
			}
		}
		else
		{
			if (attackerAttr == AttributeType.Fire)
			{
				if (targetAttr == AttributeType.Earth) return 1.75f;
				if (targetAttr == AttributeType.Fire) return 0.25f;
			}
			else if (attackerAttr == AttributeType.Ice)
			{
				if (targetAttr == AttributeType.Fire) return 1.75f;
				if (targetAttr == AttributeType.Ice) return 0.25f;
			}
			else if (attackerAttr == AttributeType.Lightning)
			{
				if (targetAttr == AttributeType.Ice) return 2f;
				if (targetAttr == AttributeType.Lightning) return 0.25f;
				if (targetAttr == AttributeType.Earth) return 0.5f;
			}
			else if (attackerAttr == AttributeType.Earth)
			{
				if (targetAttr == AttributeType.Lightning) return 1.75f;
				if (targetAttr == AttributeType.Earth) return 0.25f;
			}
			else if (attackerAttr == AttributeType.Poison)
			{
				if (targetAttr == AttributeType.Earth) return 1.75f;
				if (targetAttr == AttributeType.Poison) return 0.25f;
			}
			else if (attackerAttr == AttributeType.Holy)
			{
				if (targetAttr == AttributeType.Dark) return 2f;
				if (targetAttr == AttributeType.Holy) return 0.25f;
			}
			else if (attackerAttr == AttributeType.Dark)
			{
				if (targetAttr == AttributeType.Holy) return 2f;
				if (targetAttr == AttributeType.Dark) return 0.25f;
			}
		}

		return 1;
	}

	/// <summary>
	/// Returns a damage multiplier based on the skill's attack type
	/// and the target's armor.
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_AttackTypeMultiplier(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var attackType = modifier.AttackType == SkillAttackType.None ? skill.AttackType : modifier.AttackType;
		var targetArmor = modifier.DefenseArmorType == ArmorMaterialType.None ? target.ArmorMaterial : modifier.DefenseArmorType;

		if (Feature.IsEnabled("AttackTypeBonusRevamp2"))
		{
			if (attackType == SkillAttackType.Slash)
			{
				if (targetArmor == ArmorMaterialType.Cloth) return 1.05f;
			}
			else if (attackType == SkillAttackType.Aries)
			{
				if (targetArmor == ArmorMaterialType.Leather) return 1.05f;
			}
			else if (attackType == SkillAttackType.Strike)
			{
				if (targetArmor == ArmorMaterialType.Iron) return 1.05f;
			}
			else if (attackType == SkillAttackType.Arrow)
			{
				if (targetArmor == ArmorMaterialType.Cloth) return 1.05f;
			}
			else if (attackType == SkillAttackType.Gun)
			{
				if (targetArmor == ArmorMaterialType.Leather) return 1.05f;
			}
			else if (attackType == SkillAttackType.Cannon)
			{
				if (targetArmor == ArmorMaterialType.Iron) return 1.05f;
			}
		}
		else if (Feature.IsEnabled("AttackTypeBonusRevamp1"))
		{
			if (attackType == SkillAttackType.Slash)
			{
				if (targetArmor == ArmorMaterialType.Cloth) return 1.5f;
				if (targetArmor == ArmorMaterialType.Iron) return 0.5f;
				if (targetArmor == ArmorMaterialType.Ghost) return 0.5f;
			}
			else if (attackType == SkillAttackType.Aries)
			{
				if (targetArmor == ArmorMaterialType.Cloth) return 0.5f;
				if (targetArmor == ArmorMaterialType.Leather) return 1.5f;
				if (targetArmor == ArmorMaterialType.Ghost) return 0.5f;
			}
			else if (attackType == SkillAttackType.Strike)
			{
				if (targetArmor == ArmorMaterialType.Leather) return 0.5f;
				if (targetArmor == ArmorMaterialType.Iron) return 1.5f;
				if (targetArmor == ArmorMaterialType.Ghost) return 0.5f;
			}
			else if (attackType == SkillAttackType.Magic)
			{
				if (targetArmor == ArmorMaterialType.Ghost) return 1.5f;
			}
		}
		else
		{
			// Armor Type
			if (attackType == SkillAttackType.Slash)
			{
				if (targetArmor == ArmorMaterialType.Cloth) return 1.25f;
			}
			else if (
				(attackType == SkillAttackType.Aries)
				)
			{
				if (targetArmor == ArmorMaterialType.Leather) return 1.25f;
			}
			else if (
				(attackType == SkillAttackType.Strike)
				)
			{
				if (targetArmor == ArmorMaterialType.Iron) return 1.25f;
			}

			// Ghost
			if (
				(attackType == SkillAttackType.Slash) ||
				(attackType == SkillAttackType.Aries) ||
				(attackType == SkillAttackType.Strike) ||
				(attackType == SkillAttackType.Arrow) ||
				(attackType == SkillAttackType.Gun) ||
				(attackType == SkillAttackType.Cannon)
				)
			{
				if (targetArmor == ArmorMaterialType.Ghost) return 0.5f;
			}
			else if (attackType == SkillAttackType.Magic)
			{
				if (targetArmor == ArmorMaterialType.Ghost) return 1.25f;
			}

			// Plate Mastery
			if (
				(attackType == SkillAttackType.Arrow) ||
				(attackType == SkillAttackType.Gun)
				)
			{
				if (target is Character character)
				{
					if (character.IsWearingFullArmorSetOfType(ArmorMaterialType.Iron))
						return 0.7f;
				}
			}
		}

		return 1;
	}

	/// <summary>
	/// Returns a damage multiplier based on the given entity's races.
	/// </summary>
	/// <remarks>
	/// The default implementation of this function currently always
	/// returns 1.
	/// </remarks>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_RaceMultiplier(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (skill.Data.ClassType != SkillClassType.Magic)
			return 1;

		var attackerRace = attacker.Race;
		var targetRace = target.Race;

		// ...

		return 1;
	}

	/// <summary>
	/// Determines the result of the skill being used on the target,
	/// returning the damage that should be dealt and information
	/// about the hit, such as whether it was a crit.
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public SkillHitResult SCR_SkillHit(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier)
	{
		var SCR_CalculateDamage = ScriptableFunctions.Combat.Get("SCR_CalculateDamage");

		if (attacker != null
			&& attacker.Components != null
			&& attacker.IsBuffActiveByKeyword(BuffTag.Cloaking))
			attacker.StopBuffByTag(BuffTag.Cloaking);

		var result = new SkillHitResult();
		result.Damage = SCR_CalculateDamage(attacker, target, skill, modifier, result);

		return result;
	}

	/// <summary>
	/// Returns the chance for the target to dodge a hit from the attacker.
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GetDodgeChance(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var attackType = modifier.AttackType == SkillAttackType.None ? skill.Data.AttackType : modifier.AttackType;
		if (attackType == SkillAttackType.Magic)
			return 0f;
		if (modifier.ForcedHit)
			return 0f;
		if (modifier.ForcedEvade)
			return 100f;

		var dr = target.Properties.GetFloat(PropertyName.DR);
		var hr = attacker.Properties.GetFloat(PropertyName.HR);
		hr *= modifier.HitRateMultiplier;

		var diff = dr - hr;
		if (diff <= 0)
			return 0f;

		var k = 40f;
		var dodgeChance = 80f * diff / (diff + hr + k);

		return dodgeChance;
	}

	/// <summary>
	/// Returns the chance for the target to block a hit from the attacker.
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GetBlockChance(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var attackType = modifier.AttackType == SkillAttackType.None ? skill.Data.AttackType : modifier.AttackType;
		if (attackType == SkillAttackType.Magic || skill.Data.ClassType == SkillClassType.TrueDamage)
			return 0f;
		if (modifier.Unblockable)
			return 0f;
		if (modifier.ForcedBlock)
			return 100f;

		var block = target.Properties.GetFloat(PropertyName.BLK);
		var blockBreak = attacker.Properties.GetFloat(PropertyName.BLK_BREAK);
		blockBreak *= modifier.BlockPenetrationMultiplier;

		// The block chance cap appears to have been as much in flux as the bonus,
		// which makes sense if blocks were once able to nullify damage entirely.
		// As such, we're going to assume a base cap of 60% for nullifying and
		// 90% for the newer blocking type that only lowers the damage. For PvP,
		// the non-nullify cap is apparently supposed to be 30%.
		var maxChance = 60f;
		if (Feature.IsEnabled("IncreasedBlockRate"))
			maxChance = 90f;

		var diff = block - blockBreak;
		if (diff <= 0)
			return 0f;

		var k = 35f;
		var blockChance = maxChance * diff / (diff + blockBreak + k);

		return blockChance;
	}

	/// <summary>
	/// Returns the chance for the target to take a critical hit from the attacker.
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GetCritChance(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var attackType = modifier.AttackType == SkillAttackType.None ? skill.Data.AttackType : modifier.AttackType;
		if (attackType == SkillAttackType.Magic)
			return 0f;
		if (modifier.ForcedCritical)
			return 100f;

		var critDodgeRate = target.Properties.GetFloat(PropertyName.CRTDR);
		var critHitRate = attacker.Properties.GetFloat(PropertyName.CRTHR);
		critHitRate += modifier.BonusCritChance;

		if (critHitRate <= 0)
			return modifier.MinCritChance;
		if (critDodgeRate <= 0)
			return 100f;

		var diff = critHitRate - critDodgeRate;
		if (diff <= 0)
			return modifier.MinCritChance;

		var k = 45f;
		var critChance = 100f * diff / (diff + critDodgeRate + k);
		critChance *= modifier.CritRateMultiplier;

		return Math.Max(modifier.MinCritChance, critChance);
	}

	/// <summary>
	/// Calculates the final chance of a status effect being applied,
	/// taking the target's resistances into account.
	/// </summary>
	[ScriptableFunction]
	public float SCR_Calc_Status_Chance(ICombatEntity caster, ICombatEntity target, Skill skill, BuffId buffId, float initialValue)
	{
		if (!ZoneServer.Instance.Data.BuffDb.TryFind(buffId, out var buffData))
			return initialValue;

		var maxResistance = 0f;

		// Find the highest applicable resistance from all the buff's tags.
		foreach (var tag in buffData.Tags)
		{
			// Construct property name from tag, e.g., "Stun" -> "Res_Stun_Debuff_BM"
			var resistancePropName = $"Res_{tag}_Debuff_BM";
			if (target.Properties.TryGetFloat(resistancePropName, out var resistanceValue))
			{
				maxResistance = Math.Max(maxResistance, resistanceValue);
			}
		}

		// Resistance is a percentage value (e.g., 20.0 for 20%)
		var resistanceMultiplier = 1f - (maxResistance / 100f);

		// Apply resistance, ensuring the chance doesn't become negative
		var finalValue = initialValue * resistanceMultiplier;
		if (resistanceMultiplier != 1f)
			Log.Debug($"Resistance Chance applied to {initialValue} * {resistanceMultiplier} = {finalValue}");
		return Math.Max(0, finalValue);
	}

	/// <summary>
	/// Calculates the final duration of a status effect, taking the
	/// target's resistances into account.
	/// </summary>
	[ScriptableFunction]
	public float SCR_Calc_Status_Duration(ICombatEntity caster, ICombatEntity target, Skill skill, BuffId buffId, float initialValue)
	{
		if (!ZoneServer.Instance.Data.BuffDb.TryFind(buffId, out var buffData))
			return initialValue;

		var maxResistance = 0f;

		// Find the highest applicable resistance from all the buff's tags.
		foreach (var tag in buffData.Tags)
		{
			// Construct property name from tag, e.g., "Stun" -> "Res_Stun_Debuff_BM"
			var resistancePropName = $"Res_{tag}_Debuff_BM";
			if (target.Properties.TryGetFloat(resistancePropName, out var resistanceValue))
			{
				maxResistance = Math.Max(maxResistance, resistanceValue);
			}
		}

		// Resistance is a percentage value (e.g., 20.0 for 20%)
		var resistanceMultiplier = 1f - (maxResistance / 100f);

		// Apply resistance, ensuring the duration doesn't become negative
		var finalValue = initialValue * resistanceMultiplier;

		if (resistanceMultiplier != 1f)
			Log.Debug($"Resistance Duration applied to {initialValue} * {resistanceMultiplier} = {finalValue}");
		return Math.Max(0, finalValue);
	}
}
