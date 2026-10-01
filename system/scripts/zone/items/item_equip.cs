//--- Melia Script ----------------------------------------------------------
// Equip Items
//--- Description -----------------------------------------------------------
// Item scripts that handle on-equip and on-unequip effects including
// buff application, skill level bonuses, and property modifications.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;

public class ItemEquipScript : GeneralScript
{

	[ScriptableFunction]
	public ItemEquipResult SCP_ON_EQUIP_ITEM(Character character, Item item, EquipSlot equipSlot)
	{
		var strArg = item.Data.Script?.StrArg ?? "";

		if (ZoneServer.Instance.Data.BuffDb.TryFind(strArg, out var buffData))
			character.StartBuff(buffData.Id, TimeSpan.Zero);
		else if (ZoneServer.Instance.Data.HairTypeDb.TryFindByClassName(strArg, out var hairData))
		{
			// For wigs (Hair slot), only send the hair style if visibility is on
			if (equipSlot == EquipSlot.Hair && (character.VisibleEquip & VisibleEquip.Wig) == 0)
				return ItemEquipResult.Okay;
			Send.ZC_NORMAL.UpdateCharacterLook(character, item.Id, equipSlot, hairData.Index);
		}
		else if (ZoneServer.Instance.Data.HeadTypeDb.TryFind(character.Gender, strArg, out var headData))
		{
			// For wigs (Hair slot), only send the head style if visibility is on
			if (equipSlot == EquipSlot.Hair && (character.VisibleEquip & VisibleEquip.Wig) == 0)
				return ItemEquipResult.Okay;
			Send.ZC_NORMAL.UpdateCharacterLook(character, item.Id, equipSlot, headData.Index);
		}

		// Apply item-specific equip effects
		this.ApplyEquipEffects(character, item);

		// Grant transform-costume skill if this item is in CostumeTransformDb
		this.TryGrantCostumeTransformSkill(character, item);

		return ItemEquipResult.Okay;
	}

	/// <summary>
	/// If the item is a transform costume, grants the associated skill (e.g.
	/// "MagicalGirl_MagicalBoyNox") so the player can cast the transform buff.
	/// No-op for non-transform items or when the skill is already present.
	/// </summary>
	private void TryGrantCostumeTransformSkill(Character character, Item item)
	{
		if (!CostumeTransformDb.TryFindByBase(item.Data.ClassName, out var xform))
			return;

		if (!ZoneServer.Instance.Data.SkillDb.TryFind(xform.SkillName, out var skillData))
		{
			Log.Warning("Costume '{0}' references unknown skill '{1}'.", item.Data.ClassName, xform.SkillName);
			return;
		}

		if (character.Skills.Get(skillData.Id) != null)
			return;

		var skill = new Skill(character, skillData.Id, 1, true);
		character.Skills.Add(skill);
	}

	/// <summary>
	/// If the item is a transform costume, removes the associated skill — but
	/// only if no other equipped item still maps to the same skill (the
	/// male/female variants often share a skill).
	/// </summary>
	private void TryRemoveCostumeTransformSkill(Character character, Item item)
	{
		if (!CostumeTransformDb.TryFindByBase(item.Data.ClassName, out var xform))
			return;

		// The transform buff (e.g. "TosRangerFormChange_Red_Buff") isn't caught
		// by the generic strArg->buff lookup in SCP_ON_UNEQUIP_ITEM because the
		// item's strArg points at the skill make name, not the buff. Remove it
		// explicitly here so the character reverts to the base look on unequip.
		if (ZoneServer.Instance.Data.BuffDb.TryFind(xform.BuffName, out var buffData))
			character.Buffs.Remove(buffData.Id);

		if (!ZoneServer.Instance.Data.SkillDb.TryFind(xform.SkillName, out var skillData))
			return;

		// Another equipped costume may share the same skill; if so, keep it.
		foreach (var equip in character.Inventory.GetEquip().Values)
		{
			if (equip == null || equip == item || equip.Data == null)
				continue;

			if (CostumeTransformDb.TryFindByBase(equip.Data.ClassName, out var other)
				&& string.Equals(other.SkillName, xform.SkillName, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
		}

		if (character.Skills.Get(skillData.Id) != null)
			character.Skills.Remove(skillData.Id);
	}

	[ScriptableFunction]
	public ItemUnequipResult SCP_ON_UNEQUIP_ITEM(Character character, Item item, EquipSlot equipSlot)
	{
		var strArg = item.Data.Script?.StrArg ?? "";

		if (ZoneServer.Instance.Data.BuffDb.TryFind(strArg, out var buffData))
			character.Buffs.Remove(buffData.Id);
		else if (ZoneServer.Instance.Data.HairTypeDb.TryFindByClassName(strArg, out var hairData))
			Send.ZC_NORMAL.UpdateCharacterLook(character, item.Id, equipSlot, 0);
		else if (ZoneServer.Instance.Data.HeadTypeDb.TryFind(character.Gender, strArg, out var headData))
			Send.ZC_NORMAL.UpdateCharacterLook(character, item.Id, equipSlot, 0);

		// Remove item-specific equip effects
		this.RemoveEquipEffects(character, item);

		// Revoke transform-costume skill if this item is in CostumeTransformDb
		this.TryRemoveCostumeTransformSkill(character, item);

		return ItemUnequipResult.Okay;
	}

	/// <summary>
	/// Gives gem's skill to the character
	/// </summary>
	/// <remarks>
	/// This function is specific to skill gems.
	/// </remarks>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="equipSlot"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemEquipResult SCR_GEM_EQUIP(Character character, Item item, EquipSlot equipSlot)
	{
		if (item.Data.Group != ItemGroup.Gem || item.Data.EquipExpGroup != EquipExpGroup.Gem_Skill)
			return ItemEquipResult.Okay;

		var skillClassName = item.Data.EquipSkill;

		if (!ZoneServer.Instance.Data.SkillDb.TryFind(skillClassName, out var skillData))
		{
			Log.Warning($"Character '{character.Name}' equipped Gem Id '{item.Id}' with no available skill in database: '{skillClassName}'");
			return ItemEquipResult.Okay;
		}

		this.UpdateGemSkill(character, skillData, skillClassName);

		return ItemEquipResult.Okay;
	}

	/// <summary>
	/// Magic amulet item use script. Called when trying to "use" an amulet
	/// directly instead of dragging it onto equipment.
	/// </summary>
	/// <remarks>
	/// The actual socketing is handled by CZ_ITEM_USE_TO_ITEM packet handler.
	/// This function exists to prevent "Missing script function" messages.
	/// </remarks>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="strArg"></param>
	/// <param name="numArg1"></param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_MAGICAMULET_EQUIP(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		// Magic amulets should be dragged onto equipment, not used directly.
		// The actual socketing is handled by CZ_ITEM_USE_TO_ITEM packet.
		character.SystemMessage("DragAmuletToEquip");
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Remove gem's skill from character.
	/// </summary>
	/// <remarks>
	/// This function is specific to skill gems.
	/// </remarks>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="equipSlot"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUnequipResult SCR_GEM_UNEQUIP(Character character, Item item, EquipSlot equipSlot)
	{
		if (item.Data.Group != ItemGroup.Gem || item.Data.EquipExpGroup != EquipExpGroup.Gem_Skill)
			return ItemUnequipResult.Okay;

		var skillClassName = item.Data.EquipSkill;

		if (!ZoneServer.Instance.Data.SkillDb.TryFind(skillClassName, out var skillData))
		{
			Log.Warning($"Character '{character.Name}' unequipped Gem Id '{item.Id}' with no available skill in database: '{skillClassName}'");
			return ItemUnequipResult.Okay;
		}

		this.UpdateGemSkill(character, skillData, skillClassName);

		return ItemUnequipResult.Okay;
	}

	/// <summary>
	/// Sets the skill's gem level bonus to what the character's currently
	/// equipped skill gems grant, creating or removing the skill as needed.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="skillData"></param>
	/// <param name="skillClassName"></param>
	private void UpdateGemSkill(Character character, SkillData skillData, string skillClassName)
	{
		var gemLevel = 0;
		if (this.CanGemAffectSkill(character, skillData))
			gemLevel = this.GetGemSkillLevel(character, skillClassName);

		if (!character.TryGetSkill(skillData.Id, out var skill))
		{
			if (gemLevel <= 0)
				return;

			skill = new Skill(character, skillData.Id, 0, true);
			character.Skills.Add(skill);
		}

		skill.Properties.SetFloat(PropertyName.GemLevel_BM, gemLevel);
		skill.Properties.InvalidateAll();

		skill.RecalculateDependentBuffs();

		if (skill.Level == 0 && skill.LevelByDB == 0)
			character.Skills.Remove(skill.Id);

		Send.ZC_NORMAL.SkillProperties(character.Connection, 0, skill);
		Send.ZC_COMMON_SKILL_LIST(character);
		Send.ZC_NORMAL.SetSkillsProperties(character.Connection);
		Send.ZC_NORMAL.UpdateSkillUI(character);
	}

	/// <summary>
	/// Returns whether skill gems may grant levels for the given skill to
	/// the character.
	/// </summary>
	/// <remarks>
	/// A skill the character learned with skill points always qualifies.
	/// Unlearned skills only qualify if gems are configured to grant new
	/// skills, and then only if a job the character holds has the skill in
	/// its tree and has reached its unlock level.
	/// </remarks>
	/// <param name="character"></param>
	/// <param name="skillData"></param>
	/// <returns></returns>
	private bool CanGemAffectSkill(Character character, SkillData skillData)
	{
		if (character.TryGetSkill(skillData.Id, out var skill) && skill.LevelByDB > 0)
			return true;

		if (!ZoneServer.Instance.Conf.World.SkillGemsGrantNewSkills)
			return false;

		foreach (var job in character.Jobs.GetList())
		{
			var entries = ZoneServer.Instance.Data.SkillTreeDb.FindSkills(job.Id, job.EffectiveLevel);
			if (entries.Any(a => a.SkillId == skillData.Id))
				return true;
		}

		return false;
	}

	/// <summary>
	/// Returns the total skill level granted by all skill gems the character
	/// currently has socketed into equipped items for the given skill.
	/// </summary>
	/// <remarks>
	/// Unless duplicate stacking is enabled, only the highest level gem for
	/// the skill counts, regardless of which equipment it's socketed into.
	/// </remarks>
	/// <param name="character"></param>
	/// <param name="skillClassName"></param>
	/// <returns></returns>
	private int GetGemSkillLevel(Character character, string skillClassName)
	{
		var stackDuplicates = ZoneServer.Instance.Conf.World.StackDuplicateSkillGems;
		var total = 0;

		foreach (var equip in character.Inventory.GetEquip().Values)
		{
			if (equip == null || !equip.HasSockets)
				continue;

			foreach (var gem in equip.GetUsedGemSockets())
			{
				if (gem?.Data == null)
					continue;

				if (gem.Data.Group != ItemGroup.Gem || gem.Data.EquipExpGroup != EquipExpGroup.Gem_Skill)
					continue;

				if (!string.Equals(gem.Data.EquipSkill, skillClassName, StringComparison.OrdinalIgnoreCase))
					continue;

				var level = (int)(gem.Data.Script?.NumArg1 ?? 0);

				if (stackDuplicates)
					total += level;
				else
					total = Math.Max(total, level);
			}
		}

		return total;
	}

	//===================================================================
	// Item-specific equip/unequip effects
	//===================================================================

	/// <summary>
	/// Applies item-specific effects when an item is equipped.
	/// Handles SPCI_SKILLUP, SPCI_JOB_ALL_SKILLUP, and
	/// SPCI_EQUIP_ADD_EXPROP_NUM from client SpcItem triggers.
	/// </summary>
	private void ApplyEquipEffects(Character character, Item item)
	{
		// Apply registry-based effects (ExProp, PropMod, Buff)
		ItemEquipEffects.ApplyEffects(character, item.Id);

		// NECK04_103: +40% HP recovery as magic crit attack, base HP recovery to 0
		if (item.Data.ClassName == "NECK04_103")
		{
			var rhp = character.Properties.GetFloat(PropertyName.RHP);
			var bonus = (float)Math.Floor(rhp * 0.4f);
			character.Variables.Temp.SetFloat("Melia.NECK04_103.CrtMAtk", bonus);
			character.Variables.Temp.SetFloat("Melia.NECK04_103.Rhp", rhp);
			character.Properties.Modify(PropertyName.CRTMATK_BM, bonus);
			character.Properties.Modify(PropertyName.RHP_BM, -rhp);
		}

		// Refresh skill UI if this item provides skill level bonuses
		if (ItemEquipEffects.HasSkillEffects(item.Id))
			RefreshSkillLevels(character);
	}

	/// <summary>
	/// Removes item-specific effects when an item is unequipped.
	/// Reverses all effects applied by ApplyEquipEffects.
	/// </summary>
	private void RemoveEquipEffects(Character character, Item item)
	{
		// Remove registry-based effects (ExProp, PropMod, Buff)
		ItemEquipEffects.RemoveEffects(character, item.Id);

		// NECK04_103: reverse the HP recovery conversion
		if (item.Data.ClassName == "NECK04_103")
		{
			var bonus = character.Variables.Temp.GetFloat("Melia.NECK04_103.CrtMAtk");
			var rhp = character.Variables.Temp.GetFloat("Melia.NECK04_103.Rhp");
			character.Properties.Modify(PropertyName.CRTMATK_BM, -bonus);
			character.Properties.Modify(PropertyName.RHP_BM, rhp);
			character.Variables.Temp.Remove("Melia.NECK04_103.CrtMAtk");
			character.Variables.Temp.Remove("Melia.NECK04_103.Rhp");
		}

		// Refresh skill UI if this item provides skill level bonuses
		if (ItemEquipEffects.HasSkillEffects(item.Id))
			RefreshSkillLevels(character);
	}

	//===================================================================
	// Helper: Refresh skill UI after equip changes
	//===================================================================

	/// <summary>
	/// Invalidates all skill properties and sends UI updates to the client.
	/// Called after equipping/unequipping items that affect skill levels.
	/// The actual level bonuses are auto-calculated by ItemEquipEffects
	/// in SCR_Get_SkillLv.
	/// </summary>
	private static void RefreshSkillLevels(Character character)
	{
		foreach (var skill in character.Skills.GetList())
		{
			skill.Properties.InvalidateAll();
			skill.RecalculateDependentBuffs();
		}

		if (character.Connection != null)
		{
			Send.ZC_NORMAL.SetSkillsProperties(character.Connection);
			Send.ZC_NORMAL.UpdateSkillUI(character);
		}
	}

}
