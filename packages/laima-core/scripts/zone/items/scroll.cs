//--- Melia Script ----------------------------------------------------------
// Skill Scroll Usage
//--- Description -----------------------------------------------------------
// Handles using skill scrolls created by Pardoner's Simony skill.
// Allows players to use skills from scrolls without learning them.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;

public class SkillScrollUsageScript : GeneralScript
{
	/// <summary>
	/// Handles using a skill scroll item.
	/// Called when a player uses a Scroll_SkillItem.
	/// </summary>
	/// <param name="character">The character using the scroll.</param>
	/// <param name="item">The scroll item being used.</param>
	/// <returns>Transaction result.</returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_SKILL_SCROLL(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (item == null)
		{
			Log.Debug("SCR_USE_SKILL_SCROLL: Failed - Item is null.");
			return ItemUseResult.Fail;
		}

		// Get skill type and level from scroll properties
		var skillType = (int)item.Properties.GetFloat("SkillType", 0);
		var skillLevel = (int)item.Properties.GetFloat("SkillLevel", 0);

		if (skillType <= 0 || skillLevel <= 0)
		{
			Log.Debug("SCR_USE_SKILL_SCROLL: Failed - Invalid scroll properties. SkillType: {0}, SkillLevel: {1}",
				skillType, skillLevel);
			return ItemUseResult.Fail;
		}

		// Get skill data
		if (!ZoneServer.Instance.Data.SkillDb.TryFind((SkillId)skillType, out var skillData))
		{
			Log.Debug("SCR_USE_SKILL_SCROLL: Failed - Could not find skill data for type: {0}", skillType);
			return ItemUseResult.Fail;
		}

		// Check special restrictions (e.g., Barrier in Colony War)
		if (!IsEnableUseSkillScroll(character, skillData.ClassName))
		{
			return ItemUseResult.Fail;
		}

		// Check if scroll is locked or expired
		if (item.IsLocked)
		{
			Log.Debug("SCR_USE_SKILL_SCROLL: Failed - Scroll is locked.");
			return ItemUseResult.Fail;
		}

		if (item.IsExpired)
		{
			Log.Debug("SCR_USE_SKILL_SCROLL: Failed - Scroll is expired.");
			character.ServerMessage("This scroll has expired.");
			return ItemUseResult.Fail;
		}

		// Execute the skill from the scroll
		_ = ExecuteScrollSkill(character, item, skillData, skillLevel);

		return ItemUseResult.Okay;
	}

	/// <summary>
	/// Executes the skill from the scroll.
	/// </summary>
	private async Task ExecuteScrollSkill(Character character, Item scrollItem, SkillData skillData, int skillLevel)
	{
		try
		{
			// Create a temporary skill instance for execution
			var tempSkill = new Skill(character, skillData.Id, skillLevel);

			// Determine skill execution based on type
			switch (skillData.Type)
			{
				case SkillType.Buff:
					await ExecuteBuffSkill(character, tempSkill, skillData);
					break;

				case SkillType.Attack:
				case SkillType.Magic:
					await ExecuteAttackSkill(character, tempSkill, skillData);
					break;

				default:
					// For other skill types, try to execute as a self-buff
					await ExecuteBuffSkill(character, tempSkill, skillData);
					break;
			}

			// Consume the scroll after successful use
			character.Inventory.Remove(scrollItem, 1, InventoryItemRemoveMsg.Used);

			Log.Debug("SCR_USE_SKILL_SCROLL: Successfully used scroll for skill {0} Lv.{1} by character {2}",
				skillData.ClassName, skillLevel, character.Name);
		}
		catch (Exception ex)
		{
			Log.Error("SCR_USE_SKILL_SCROLL: Error executing scroll skill - {0}", ex.Message);
			character.ServerMessage("Failed to use the scroll.");
		}
	}

	/// <summary>
	/// Executes a buff-type skill from scroll.
	/// </summary>
	private async Task ExecuteBuffSkill(Character character, Skill skill, SkillData skillData)
	{
		// Play skill animation
		Send.ZC_SKILL_MELEE_GROUND(character, skill, character.Position);

		await Task.Delay(TimeSpan.FromMilliseconds(300));

		// Get buff info from SimonyDb if available
		var buffId = BuffId.None;
		var duration = TimeSpan.FromMinutes(30); // Default 30 minutes

		if (ZoneServer.Instance.Data.SimonyDb.TryFindBySkillId(skillData.Id, out var simonyData))
		{
			buffId = simonyData.BuffId;
			if (simonyData.BuffDuration > 0)
				duration = TimeSpan.FromMilliseconds(simonyData.BuffDuration);
		}
		else
		{
			// Fallback to mapping
			buffId = GetBuffIdForSkill(skillData.Id);
		}

		if (buffId != BuffId.None)
		{
			character.StartBuff(buffId, skill.Level, 0f, duration, character);
		}
	}

	/// <summary>
	/// Executes an attack-type skill from scroll.
	/// </summary>
	private async Task ExecuteAttackSkill(Character character, Skill skill, SkillData skillData)
	{
		// Get target position (in front of character)
		var targetPos = character.Position.GetRelative(character.Direction, 50);

		Send.ZC_SKILL_READY(character, skill, 1, character.Position, targetPos);
		Send.ZC_SKILL_MELEE_GROUND(character, skill, targetPos, ForceId.GetNew(), null);

		await Task.Delay(TimeSpan.FromMilliseconds(500));

		// The actual damage would be handled by the skill handler system
	}

	/// <summary>
	/// Executes a ground-type skill from scroll.
	/// </summary>
	private async Task ExecuteGroundSkill(Character character, Skill skill, SkillData skillData)
	{
		var targetPos = character.Position.GetRelative(character.Direction, 30);

		Send.ZC_SKILL_READY(character, skill, 1, character.Position, targetPos);
		Send.ZC_SKILL_MELEE_GROUND(character, skill, targetPos, ForceId.GetNew(), null);

		await Task.Delay(TimeSpan.FromMilliseconds(300));
	}

	/// <summary>
	/// Checks if the skill scroll can be used in the current context.
	/// </summary>
	private bool IsEnableUseSkillScroll(Character character, string skillClassName)
	{
		// Barrier cannot be used near Colony War towers
		if (skillClassName == "Paladin_Barrier")
		{
			if (character.Map.IsGTW)
			{
				// Check if in colony tower range
				// This would need proper implementation based on your GvG system
				if (IsInColonyTowerRange(character))
				{
					character.ServerMessage("Cannot use this scroll near Colony Tower.");
					return false;
				}
			}
		}

		return true;
	}

	/// <summary>
	/// Checks if character is within range of a colony tower.
	/// </summary>
	private bool IsInColonyTowerRange(Character character)
	{
		// Implementation depends on your colony/GvG system
		// This is a placeholder that should be replaced with actual logic
		return false;
	}

	/// <summary>
	/// Gets the buff ID associated with a skill.
	/// </summary>
	private BuffId GetBuffIdForSkill(SkillId skillId)
	{
		return skillId switch
		{
			SkillId.Paladin_Barrier => BuffId.Barrier_Buff,
			SkillId.Paladin_ResistElements => BuffId.ResistElements_Buff,
			SkillId.Paladin_StoneSkin => BuffId.StoneSkin_Buff,
			SkillId.Pardoner_IncreaseMagicDEF => BuffId.IncreaseMagicDEF_Buff,
			SkillId.Pardoner_Indulgentia => BuffId.Indulgentia_Buff,
			// Add more skill -> buff mappings as needed
			_ => BuffId.None,
		};
	}

	/// <summary>
	/// Gets the duration for a buff from a scroll.
	/// </summary>
	private TimeSpan GetBuffDuration(Skill skill)
	{
		// Base duration of 30 minutes for scroll buffs
		var baseDuration = TimeSpan.FromMinutes(30);

		// Could be modified by skill level or other factors
		return baseDuration;
	}
}
