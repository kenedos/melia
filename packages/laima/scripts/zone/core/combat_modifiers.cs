//--- Melia Script ----------------------------------------------------------
// Combat Modifier Script
//--- Description -----------------------------------------------------------
// Collection of functions that modify combat calculations based on
// combatants's buffs, skills, abilities, and other properties.
//---------------------------------------------------------------------------

using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Logging;

public class CombatModifierCalculationsScript : GeneralScript
{
	/// <summary>
	/// Calls scriptable functions for active buffs, skills, abilities, etc,
	/// before combat calculations.
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Combat_BeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		CallAll(nameof(SCR_Combat_BeforeCalc), attacker, target, skill, modifier, skillHitResult);
		return 0;
	}

	/// <summary>
	/// Calls scriptable functions for active buffs, skills, abilities, etc,
	/// after combat calculations.
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Combat_AfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		CallAll(nameof(SCR_Combat_AfterCalc), attacker, target, skill, modifier, skillHitResult);
		return 0;
	}

	/// <summary>
	/// Calls scriptable functions for active buffs, skills, abilities, etc,
	/// during combat calculations.
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Combat_BeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		CallAll(nameof(SCR_Combat_BeforeBonuses), attacker, target, skill, modifier, skillHitResult);
		return 0;
	}

	/// <summary>
	/// Calls scriptable functions for active buffs, skills, abilities, etc,
	/// during combat calculations.
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Combat_AfterBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		CallAll(nameof(SCR_Combat_AfterBonuses), attacker, target, skill, modifier, skillHitResult);
		return 0;
	}

	/// <summary>
	/// Calls scriptable functions for buffs, skills, abilities, etc. for both
	/// the attacker and target.
	/// </summary>
	/// <param name="baseFuncName"></param>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	private void CallAll(string baseFuncName, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		CallForBuffs(baseFuncName, attacker, attacker, target, skill, modifier, skillHitResult);
		CallForBuffs(baseFuncName, target, attacker, target, skill, modifier, skillHitResult);

		CallForPassiveSkills(baseFuncName, attacker, attacker, target, skill, modifier, skillHitResult);
		CallForPassiveSkills(baseFuncName, target, attacker, target, skill, modifier, skillHitResult);

		CallForAbilities(baseFuncName, attacker, attacker, target, skill, modifier, skillHitResult);
		CallForAbilities(baseFuncName, target, attacker, target, skill, modifier, skillHitResult);
	}

	/// <summary>
	/// Calls the given scriptable function for all active buffs on the given entity.
	/// If the entity is a Companion, also checks the owner's active buffs with companion-specific hooks.
	/// </summary>
	/// <param name="baseFuncName"></param>
	/// <param name="entity"></param>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	private void CallForBuffs(string baseFuncName, ICombatEntity entity, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var type = entity == attacker ? "Attack" : "Defense";
		baseFuncName += "_" + type;

		// Check entity's own active buffs
		if (entity.Components.TryGet<BuffComponent>(out var buffs))
		{
			var activeBuffs = buffs.GetList();

			foreach (var buff in activeBuffs)
			{
				var funcName = baseFuncName + "_" + buff.Id;

				if (!ScriptableFunctions.Combat.TryGet(funcName, out var func))
					continue;

				func(attacker, target, skill, modifier, skillHitResult);
			}
		}

		// If entity is a companion, check the owner's active buffs with companion-specific function names
		if (entity is Companion companion && companion.Owner != null)
		{
			if (companion.Owner.Components.TryGet<BuffComponent>(out var ownerBuffs))
			{
				var companionType = entity == attacker ? "CompanionAttack" : "CompanionDefense";
				var companionBaseFuncName = baseFuncName.Replace(type, companionType);

				var ownerActiveBuffs = ownerBuffs.GetList();

				foreach (var buff in ownerActiveBuffs)
				{
					var funcName = companionBaseFuncName + "_" + buff.Id;

					if (!ScriptableFunctions.Combat.TryGet(funcName, out var func))
						continue;

					func(attacker, target, skill, modifier, skillHitResult);
				}
			}
		}
	}

	/// <summary>
	/// Calls the given scriptable function for all passive skills on the given entity.
	/// If the entity is a Companion, also checks the owner's passive skills with companion-specific hooks.
	/// </summary>
	/// <param name="baseFuncName"></param>
	/// <param name="entity"></param>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	private void CallForPassiveSkills(string baseFuncName, ICombatEntity entity, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var type = entity == attacker ? "Attack" : "Defense";
		baseFuncName += "_" + type;

		// Check entity's own passive skills
		if (entity.Components.TryGet<SkillComponent>(out var skills))
		{
			var passiveSkills = skills.GetList(a => a.IsPassive);

			foreach (var passiveSkill in passiveSkills)
			{
				var funcName = baseFuncName + "_" + passiveSkill.Data.ClassName;

				if (!ScriptableFunctions.Combat.TryGet(funcName, out var func))
					continue;

				func(attacker, target, skill, modifier, skillHitResult);
			}
		}

		// If entity is a companion, check the owner's passive skills with companion-specific function names
		if (entity is Companion companion && companion.Owner != null)
		{
			if (companion.Owner.Components.TryGet<SkillComponent>(out var ownerSkills))
			{
				var companionType = entity == attacker ? "CompanionAttack" : "CompanionDefense";
				var companionBaseFuncName = baseFuncName.Replace(type, companionType);

				var ownerPassiveSkills = ownerSkills.GetList(a => a.IsPassive);

				foreach (var passiveSkill in ownerPassiveSkills)
				{
					var funcName = companionBaseFuncName + "_" + passiveSkill.Data.ClassName;

					if (!ScriptableFunctions.Combat.TryGet(funcName, out var func))
						continue;

					func(attacker, target, skill, modifier, skillHitResult);
				}
			}
		}
	}

	/// <summary>
	/// Calls the given scriptable function for all active abilities on the given entity.
	/// If the entity is a Companion, also checks the owner's active abilities with companion-specific hooks.
	/// </summary>
	/// <param name="baseFuncName"></param>
	/// <param name="entity"></param>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	private void CallForAbilities(string baseFuncName, ICombatEntity entity, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var type = entity == attacker ? "Attack" : "Defense";
		baseFuncName += "_" + type;

		// Check entity's own active abilities
		if (entity.Components.TryGet<AbilityComponent>(out var abilities))
		{
			var activeAbilities = abilities.GetList(a => a.Active);

			foreach (var ability in activeAbilities)
			{
				var funcName = baseFuncName + "_" + ability.Id;

				if (!ScriptableFunctions.Combat.TryGet(funcName, out var func))
					continue;

				func(attacker, target, skill, modifier, skillHitResult);
			}
		}

		// If entity is a companion, check the owner's active abilities with companion-specific function names
		if (entity is Companion companion && companion.Owner != null)
		{
			if (companion.Owner.Components.TryGet<AbilityComponent>(out var ownerAbilities))
			{
				var companionType = entity == attacker ? "CompanionAttack" : "CompanionDefense";
				var companionBaseFuncName = baseFuncName.Replace(type, companionType);

				var ownerActiveAbilities = ownerAbilities.GetList(a => a.Active);

				foreach (var ability in ownerActiveAbilities)
				{
					var funcName = companionBaseFuncName + "_" + ability.Id;

					if (!ScriptableFunctions.Combat.TryGet(funcName, out var func))
						continue;

					func(attacker, target, skill, modifier, skillHitResult);
				}
			}
		}
	}

	/// <summary>
	/// Calls the given scriptable function for all items on the given entity.
	/// </summary>
	/// <param name="baseFuncName"></param>
	/// <param name="entity"></param>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	private void CallForItems(string baseFuncName, ICombatEntity entity, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (!entity.Components.TryGet<InventoryComponent>(out var inventory))
			return;

		var type = entity == attacker ? "Attack" : "Defense";
		baseFuncName += "_" + type;

		var equipment = inventory.GetEquip();

		foreach (var equip in equipment)
		{
			var funcName = baseFuncName + "_" + equip.Value.Id;

			if (!ScriptableFunctions.Combat.TryGet(funcName, out var func))
				continue;

			func(attacker, target, skill, modifier, skillHitResult);
		}
	}
}
