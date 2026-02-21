//--- Melia Script ----------------------------------------------------------
// Equip Items
//--- Description -----------------------------------------------------------
// Item scripts that add and remove buffs on equipping items.
//---------------------------------------------------------------------------

using System;
using System.Linq;
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
		else if (ZoneServer.Instance.Data.HairTypeDb.TryFind(character.Gender, strArg, out var hairData))
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

		return ItemEquipResult.Okay;
	}

	[ScriptableFunction]
	public ItemUnequipResult SCP_ON_UNEQUIP_ITEM(Character character, Item item, EquipSlot equipSlot)
	{
		var strArg = item.Data.Script?.StrArg ?? "";

		if (ZoneServer.Instance.Data.BuffDb.TryFind(strArg, out var buffData))
			character.Buffs.Remove(buffData.Id);
		else if (ZoneServer.Instance.Data.HairTypeDb.TryFind(character.Gender, strArg, out var hairData))
			Send.ZC_NORMAL.UpdateCharacterLook(character, item.Id, equipSlot, 0);
		else if (ZoneServer.Instance.Data.HeadTypeDb.TryFind(character.Gender, strArg, out var headData))
			Send.ZC_NORMAL.UpdateCharacterLook(character, item.Id, equipSlot, 0);

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
		var skillLevel = item.Data.Script.NumArg1;

		if (!ZoneServer.Instance.Data.SkillDb.TryFind(skillClassName, out var skillData))
		{
			Log.Warning($"Character '{character.Name}' equipped Gem Id '{item.Id}' with no available skill in database: '{skillClassName}'");
			return ItemEquipResult.Okay;
		}

		// Checks if character can learn skill
		// Use the highest job level among all jobs the character has
		// Character must have a job of the same class (e.g., Wizard class for Cryomancer skills)
		var allJobs = character.Jobs.GetList();
		var highestJobLevel = allJobs.Max(j => j.Level);
		var characterJobClasses = allJobs.Select(j => j.Id.ToClass()).Distinct().ToList();
		var entries = ZoneServer.Instance.Data.SkillTreeDb.FindJobs(skillData.Id, highestJobLevel);

		var canLearnSkill = false;
		foreach (var entry in entries)
		{
			if (characterJobClasses.Contains(entry.JobId.ToClass()))
			{
				canLearnSkill = true;
				break;
			}
		}

		if (!canLearnSkill)
			return ItemEquipResult.Okay;

		// Checks if character already has skill
		if (character.TryGetSkill(skillData.Id, out var skill))
		{
			skill.Properties.Modify(PropertyName.GemLevel_BM, skillLevel);
			skill.Properties.InvalidateAll();
		}
		else
		{
			skill = new Skill(character, skillData.Id, 0, true);
			skill.Properties.Modify(PropertyName.GemLevel_BM, skillLevel);
			skill.Properties.InvalidateAll();
			character.Skills.Add(skill);
		}
		Send.ZC_NORMAL.SkillProperties(character.Connection, 0, skill);
		Send.ZC_COMMON_SKILL_LIST(character);
		Send.ZC_NORMAL.SetSkillsProperties(character.Connection);
		Send.ZC_NORMAL.UpdateSkillUI(character);

		// Log.Debug($"Equipped Gem '{item.Data.ClassName}' with skill: '{item.Data.EquipSkill}' skill level is now: '{skill.Level}'");

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
		var skillLevel = item.Data.Script.NumArg1;

		if (!ZoneServer.Instance.Data.SkillDb.TryFind(skillClassName, out var skillData))
		{
			Log.Warning($"Character '{character.Name}' unequipped Gem Id '{item.Id}' with no available skill in database: '{skillClassName}'");
			return ItemUnequipResult.Okay;
		}
		if (!character.TryGetSkill(skillData.Id, out var skill))
		{
			return ItemUnequipResult.Okay;
		}

		var currentGemBM = (int)skill.Properties.GetFloat(PropertyName.GemLevel_BM, 0);
		var amountToRemove = Math.Min(skillLevel, currentGemBM);
		if (amountToRemove > 0)
			skill.Properties.Modify(PropertyName.GemLevel_BM, -amountToRemove);
		skill.Properties.InvalidateAll();

		if (skill.Level == 0 && skill.LevelByDB == 0)
			character.Skills.Remove(skill.Id);

		Send.ZC_NORMAL.SkillProperties(character.Connection, 0, skill);
		Send.ZC_COMMON_SKILL_LIST(character);
		Send.ZC_NORMAL.SetSkillsProperties(character.Connection);

		// if (character.HasSkill(skillData.Id))
		// 	Log.Debug($"Unequipped Gem '{item.Data.ClassName}' with skill: '{item.Data.EquipSkill}' skill level is now: '{skill.Level}'");
		// else
		// 	Log.Debug($"Unequipped Gem '{item.Data.ClassName}' with skill: '{item.Data.EquipSkill}' and skill has been removed");

		return ItemUnequipResult.Okay;
	}
}
