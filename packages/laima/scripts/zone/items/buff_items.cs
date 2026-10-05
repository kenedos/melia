//--- Melia Script ----------------------------------------------------------
// Buff Items
//--- Description -----------------------------------------------------------
// Item scripts that apply or remove buffs.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

public class BuffItemScripts : GeneralScript
{
	/// <summary>
	/// Applies the Drug_Haste buff to the character.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="buffName">Unused - always applies Drug_Haste</param>
	/// <param name="numArg1">Buff level (typically 2 or 3)</param>
	/// <param name="numArg2">Duration in milliseconds</param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_HasteBuff(Character character, Item item, string buffName, float numArg1, float numArg2)
	{
		character.Buffs.Start(BuffId.Drug_Haste, numArg1, 0, TimeSpan.FromMilliseconds(numArg2), character);
		return ItemUseResult.Okay;
	}

	/// <summary>
	/// Removes the ChallengeMode_Completed buff if the character is in a
	/// basic field dungeon or city map.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="strArg"></param>
	/// <param name="numArg1"></param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ChallengeModeReset(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		// Check if character is in a basic field, dungeon, or city
		// IS_BASIC_FIELD_DUNGEON: IsIndun != 1 AND IsPVPServer != 1 AND IsMissionInst != 1
		//   AND (MapType == 'Field' OR MapType == 'Dungeon')
		var mapData = character.Map.Data;
		var isBasicFieldDungeon = !character.Map.IsInstance &&
								  (mapData.Type == MapType.Field || mapData.Type == MapType.Dungeon);
		var isCity = mapData.Type == MapType.City;

		if (isBasicFieldDungeon || isCity)
		{
			if (character.IsBuffActive(BuffId.ChallengeMode_Completed))
			{
				character.RemoveBuff(BuffId.ChallengeMode_Completed);
				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, "ChallengeModeReset_MSG2");
				return ItemUseResult.Okay;
			}
		}

		// Item not consumed if buff wasn't removed
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Applies a buff and adds achievement points for potion usage.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="buffName">The name of the buff to apply</param>
	/// <param name="numArg1">Buff level</param>
	/// <param name="numArg2">Duration in milliseconds</param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_AddBuff_ABILPOTION(Character character, Item item, string buffName, float numArg1, float numArg2)
	{
		character.Buffs.Start(buffName, numArg1, 0, TimeSpan.FromMilliseconds(numArg2), character);
		character.AddAchievementPoint("Potion", 1);
		return ItemUseResult.Okay;
	}
}
