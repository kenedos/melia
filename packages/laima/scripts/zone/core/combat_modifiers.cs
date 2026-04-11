//--- Melia Script ----------------------------------------------------------
// Combat Modifier Script
//--- Description -----------------------------------------------------------
// Collection of functions that modify combat calculations based on
// combatants's buffs, skills, abilities, and other properties.
//---------------------------------------------------------------------------

using System.Collections.Generic;
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
	/// Calls scriptable functions for active buffs, skills, abilities, etc,
	/// when a physical attack is dodged by the target.
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Combat_OnDodge(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		CallAll(nameof(SCR_Combat_OnDodge), attacker, target, skill, modifier, skillHitResult);
		return 0;
	}

	/// <summary>
	/// Calls scriptable functions for active buffs, skills, abilities, etc,
	/// when a physical attack is blocked by the target.
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Combat_OnBlock(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		CallAll(nameof(SCR_Combat_OnBlock), attacker, target, skill, modifier, skillHitResult);
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
		// Old System
		CallForBuffs(baseFuncName, attacker, attacker, target, skill, modifier, skillHitResult);
		CallForBuffs(baseFuncName, target, attacker, target, skill, modifier, skillHitResult);

		CallForPassiveSkills(baseFuncName, attacker, attacker, target, skill, modifier, skillHitResult);
		CallForPassiveSkills(baseFuncName, target, attacker, target, skill, modifier, skillHitResult);

		CallForAbilities(baseFuncName, attacker, attacker, target, skill, modifier, skillHitResult);
		CallForAbilities(baseFuncName, target, attacker, target, skill, modifier, skillHitResult);

		// New System
		CallForBuffs(baseFuncName, attacker, target, skill, modifier, skillHitResult);
		CallForPassiveSkills(baseFuncName, attacker, target, skill, modifier, skillHitResult);
		CallForAbilities(baseFuncName, attacker, target, skill, modifier, skillHitResult);
		CallForEquip(baseFuncName, attacker, target, skill, modifier, skillHitResult);
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
	/// Calls the given scriptable function for all active buffs (new system).
	/// Also checks companion owners' active buffs.
	/// </summary>
	private void CallForBuffs(string baseFuncName, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var buffIds = new HashSet<BuffId>();

		if (attacker.Components.TryGet<BuffComponent>(out var buffs))
			buffIds.UnionWith(buffs.GetList().Select(b => b.Id));

		if (target.Components.TryGet<BuffComponent>(out buffs))
			buffIds.UnionWith(buffs.GetList().Select(b => b.Id));

		// Check companion owners' active buffs
		if (attacker is Companion companionAtk && companionAtk.Owner != null)
		{
			if (companionAtk.Owner.Components.TryGet<BuffComponent>(out var ownerBuffs))
				buffIds.UnionWith(ownerBuffs.GetList().Select(b => b.Id));
		}

		if (target is Companion companionTgt && companionTgt.Owner != null)
		{
			if (companionTgt.Owner.Components.TryGet<BuffComponent>(out var ownerBuffs))
				buffIds.UnionWith(ownerBuffs.GetList().Select(b => b.Id));
		}

		foreach (var buffId in buffIds)
		{
			var funcName = baseFuncName + "_" + buffId;

			if (ScriptableFunctions.CombatModifier.TryGet(funcName, out var func))
				func(attacker, target, skill, modifier, skillHitResult);
		}
	}

	/// <summary>
	/// Calls the given scriptable function for all passive skills (new system).
	/// Also checks companion owners' passive skills.
	/// </summary>
	private void CallForPassiveSkills(string baseFuncName, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var skillIds = new HashSet<SkillId>();

		if (attacker.Components.TryGet<SkillComponent>(out var skills))
			skillIds.UnionWith(skills.GetList(a => a.IsPassive).Select(s => s.Id));

		if (target.Components.TryGet<SkillComponent>(out skills))
			skillIds.UnionWith(skills.GetList(a => a.IsPassive).Select(s => s.Id));

		// Check companion owners' passive skills
		if (attacker is Companion companionAtk && companionAtk.Owner != null)
		{
			if (companionAtk.Owner.Components.TryGet<SkillComponent>(out var ownerSkills))
				skillIds.UnionWith(ownerSkills.GetList(a => a.IsPassive).Select(s => s.Id));
		}

		if (target is Companion companionTgt && companionTgt.Owner != null)
		{
			if (companionTgt.Owner.Components.TryGet<SkillComponent>(out var ownerSkills))
				skillIds.UnionWith(ownerSkills.GetList(a => a.IsPassive).Select(s => s.Id));
		}

		foreach (var skillId in skillIds)
		{
			var funcName = baseFuncName + "_" + skillId;

			if (ScriptableFunctions.CombatModifier.TryGet(funcName, out var func))
				func(attacker, target, skill, modifier, skillHitResult);
		}
	}

	/// <summary>
	/// Calls the given scriptable function for all active abilities (new system).
	/// Also checks companion owners' active abilities.
	/// </summary>
	private void CallForAbilities(string baseFuncName, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var abilityIds = new HashSet<AbilityId>();

		if (attacker.Components.TryGet<AbilityComponent>(out var abilities))
			abilityIds.UnionWith(abilities.GetList(a => a.Active).Select(a => a.Id));

		if (target.Components.TryGet<AbilityComponent>(out abilities))
			abilityIds.UnionWith(abilities.GetList(a => a.Active).Select(a => a.Id));

		// Check companion owners' active abilities
		if (attacker is Companion companionAtk && companionAtk.Owner != null)
		{
			if (companionAtk.Owner.Components.TryGet<AbilityComponent>(out var ownerAbilities))
				abilityIds.UnionWith(ownerAbilities.GetList(a => a.Active).Select(a => a.Id));
		}

		if (target is Companion companionTgt && companionTgt.Owner != null)
		{
			if (companionTgt.Owner.Components.TryGet<AbilityComponent>(out var ownerAbilities))
				abilityIds.UnionWith(ownerAbilities.GetList(a => a.Active).Select(a => a.Id));
		}

		foreach (var abilityId in abilityIds)
		{
			var funcName = baseFuncName + "_" + abilityId;

			if (ScriptableFunctions.CombatModifier.TryGet(funcName, out var func))
				func(attacker, target, skill, modifier, skillHitResult);
		}
	}

	/// <summary>
	/// Calls the given scriptable function for all active abilities.
	/// </summary>
	/// <param name="baseFuncName"></param>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="modifier"></param>
	/// <param name="skillHitResult"></param>
	private void CallForEquip(string baseFuncName, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var equipIds = new HashSet<int>();

		if (attacker.Components.TryGet<InventoryComponent>(out var inventory))
			equipIds.UnionWith(inventory.GetActualEquipIds());

		if (target.Components.TryGet<InventoryComponent>(out inventory))
			equipIds.UnionWith(inventory.GetActualEquipIds());

		foreach (var equipId in equipIds)
		{
			var funcName = baseFuncName + "_" + equipId;

			if (ScriptableFunctions.CombatModifier.TryGet(funcName, out var func))
				func(attacker, target, skill, modifier, skillHitResult);
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
