//--- Melia Script ----------------------------------------------------------
// Gacha Items
//--- Description -----------------------------------------------------------
// Item scripts that give random rewards from reward groups.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;
using Yggdrasil.Util;

public class GachaItemScripts : GeneralScript
{
	/// <summary>
	/// Gives a random reward from the specified reward group.
	/// Uses the CubeGachaDb (reward_freedungeon) to select items.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="rewardGroupClsName">The reward group name to select from</param>
	/// <param name="numArg1"></param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_GACHA(Character character, Item item, string rewardGroupClsName, float numArg1, float numArg2)
	{
		var result = GiveReward(character, rewardGroupClsName);
		return result ? ItemUseResult.Okay : ItemUseResult.Fail;
	}

	/// <summary>
	/// Gives a random reward from the specified group using weighted selection.
	/// </summary>
	/// <param name="character">The character to give the reward to</param>
	/// <param name="group">The reward group name</param>
	/// <returns>True if a reward was given, false otherwise</returns>
	private static bool GiveReward(Character character, string group)
	{
		// Get all items in the reward group
		var rewardList = new List<(string ItemName, int Count, float Ratio)>();
		var totalRatio = 0f;

		var possibleItems = ZoneServer.Instance.Data.CubeGachaDb.FindAll(a => a.Group == group);
		foreach (var possibleItem in possibleItems)
		{
			// Verify the item exists
			var itemData = ZoneServer.Instance.Data.ItemDb.FindByClass(possibleItem.ItemName);
			if (itemData != null)
			{
				rewardList.Add((possibleItem.ItemName, possibleItem.Count, possibleItem.Ratio));
				totalRatio += possibleItem.Ratio;
			}
		}

		if (rewardList.Count == 0)
		{
			Log.Warning("SCR_USE_GACHA: No valid items found in reward group '{0}'", group);
			return false;
		}

		// Weighted random selection
		var result = RandomProvider.Get().NextDouble() * totalRatio;
		var cumulativeRatio = 0f;

		foreach (var (itemName, count, ratio) in rewardList)
		{
			cumulativeRatio += ratio;
			if (result <= cumulativeRatio)
			{
				character.AddItem(itemName, count);
				return true;
			}
		}

		// Fallback - give the last item if we somehow didn't match
		var lastItem = rewardList[rewardList.Count - 1];
		character.AddItem(lastItem.ItemName, lastItem.Count);
		return true;
	}
}
