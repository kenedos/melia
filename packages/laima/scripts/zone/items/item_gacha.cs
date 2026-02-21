using System.Collections.Generic;
using System;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Yggdrasil.Util;
using Yggdrasil.Extensions;
using Melia.Shared.Data.Database;
using System.Linq;

public class ItemGachaScripts : GeneralScript
{
	/// <summary>
	/// Unlocks an achievement by adding 1 achievement point specified by the item.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="strArg"></param>
	/// <param name="numArg1"></param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction("SCR_USE_GHACHA_TPCUBE")]
	public ItemUseResult SCR_USE_GHACHA_TPCUBE(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.IsRiding || character.IsBuffActive(BuffId.SwellLeftArm_Buff) || character.IsBuffActive(BuffId.SwellRightArm_Buff))
		{
			character.SystemMessage("DonUseItemOnRIde");
			return ItemUseResult.Fail;
		}

		// TODO: Implement GachaDb
		//ZoneServer.Instance.Data.GachaDb

		return ItemUseResult.Okay;
	}

	[ScriptableFunction("SCR_FIRST_USE_GHACHA_CUBE")]
	public static ItemUseResult SCR_FIRST_USE_GHACHA_CUBE(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		character.AddonMessage(AddonMessage.CLOSE_GACHA_CUBE, "NO");
		SCR_ITEM_GACHA(character, item, strArg, 1);
		return ItemUseResult.Okay;
	}

	public static void CLEAR_GACHA_COMMAND(Character character, string rewardGroup)
	{
		character.AddonMessage(AddonMessage.CLOSE_GACHA_CUBE, "NO");
	}

	public static bool CHECK_GACHA_DUPLICATE(Character character, bool enableDuplicate, string itemName)
	{
		//if (enableDuplicate)
		//	return true;
		//if (IsDuplicateReward(character, itemName))
		//	return false;
		return true;
	}

	public static void SCR_ITEM_GACHA(
		Character character,
		Item cubeItem,
		string rewardGroup,
		int btnVisible,
		int reopenCount = -1)
	{
		var enableDuplicate = cubeItem.Data.CubeDuplicate;
		var rewardList = new List<CubeGachaData>();
		var rewardCnt = new List<int>();
		var ratioList = new List<float>();
		var rewardGroupName = new List<ItemGroup>();
		var listIndex = 0;
		var totalRatio = 0f;
		var totalPrice = 0;

		if (rewardGroup == null && cubeItem.Data.Script != null)
		{
			rewardGroup = cubeItem.Data.Script.StrArg;
			totalPrice = (int)cubeItem.Data.Script.NumArg1;
		}

		if (rewardGroup == null)
		{
			CLEAR_GACHA_COMMAND(character, rewardGroup);
			return;
		}

		if (ZoneServer.Instance.World.IsSeason)
			totalPrice = (int)Math.Floor(totalPrice / 2.0);

		if (reopenCount == 1)
		{
			if (cubeItem.TryGetProp("ReopenDiscountRatio", out var discountRatio, 1))
			{
				discountRatio = 1 - (discountRatio / 100);
			}

			totalPrice = (int)Math.Floor(totalPrice * discountRatio);
		}

		if (!character.HasSilver(totalPrice))
		{
			CLEAR_GACHA_COMMAND(character, rewardGroup);
			return;
		}

		var possibleItems = ZoneServer.Instance.Data.CubeGachaDb.FindAll(a => a.Group == rewardGroup);
		foreach (var possibleItem in possibleItems)
		{
			var possibleItemData = ZoneServer.Instance.Data.ItemDb.FindByClass(possibleItem.ItemName);
			if (possibleItemData != null && CHECK_GACHA_DUPLICATE(character, enableDuplicate, possibleItem.ItemName))
			{
				rewardList.Add(possibleItem);
				rewardCnt.Add(possibleItem.Count);
				ratioList.Add(possibleItem.Ratio);
				rewardGroupName.Add(possibleItemData.Group);
				listIndex++;
				totalRatio += possibleItem.Ratio;
			}
		}

		if (listIndex <= 0)
		{
			CLEAR_GACHA_COMMAND(character, rewardGroup);
			return;
		}

		var reward = "";
		var rewardCount = 0;
		double checkTime = DateTime.Now.GetUnixTimestamp() + 1;
		while (string.IsNullOrEmpty(reward))
		{
			if (checkTime < DateTime.Now.GetUnixTimestamp())
			{
				CLEAR_GACHA_COMMAND(character, rewardGroup);
				return;
			}
			reward = null;
			var result = RandomProvider.Next(1, (int)totalRatio);
			result *= (int)(ZoneServer.Instance.Conf.World.CubeDropRate / 100);
			for (var i = 0; i < ratioList.Count - 1; i++)
			{
				if (result <= ratioList[i])
				{
					reward = rewardList[i].ItemName;
					rewardCount = rewardCnt[i];
					rewardGroup = rewardGroupName[i].ToString();
					break;
				}
				else
				{
					ratioList[i + 1] += ratioList[i];
				}
			}

			if (reward != null)
				break;
		}

		if (reward != null)
		{
			if (totalPrice > 0)
				character.RemoveItem(ItemId.Vis, totalPrice);
			character.AddItem(reward, rewardCount);

			if (rewardGroup != null)
			{
				if (!cubeItem.Data.CubeReopen)
				{
					CLEAR_GACHA_COMMAND(character, rewardGroup);
					btnVisible = 0;
				}
				var script = string.Format("GACHA_CUBE_SUCEECD(\'{0}\', \'{1}\', \'{2}\',{3})", cubeItem.Id, reward, btnVisible, reopenCount);
				character.ExecuteClientScript(script);
			}
			else
			{
				var script = string.Format("GACHA_CUBE_SUCEECD_EX(\'{0}\', \'{1}\', \'{2}\',{3})", cubeItem.Id, reward, btnVisible, reopenCount);
				character.ExecuteClientScript(script);
				//UpdateCubeCmd(character, reward);
			}

			if (!enableDuplicate)
			{
				//UpdateCubeReward(character, reward);
			}
		}
	}

	public static void SCR_ITEM_GACHA_CHALLENGE(
		Character character,
		Item cubeItem,
		string rewardGroup,
		int btnVisible,
		int reopenCount = -1)
	{
		var enableDuplicate = cubeItem.Data.CubeDuplicate;
		var rewardID = "reward_freedungeon";
		var rewardList = new List<CubeGachaData>();
		var rewardCnt = new List<int>();
		var ratioList = new List<float>();
		var rewardGroupName = new List<ItemGroup>();
		var listIndex = 0;
		var totalRatio = 0f;
		var totalPrice = 0;
		string rewGroup = null;

		var basicGradeList = new List<Tuple<string, int>>
		{
			Tuple.Create("C", 7000),
			Tuple.Create("B", 2000),
			Tuple.Create("A", 1000)
		};

		var gradeList = new List<Tuple<string, int>>();
		var totalGradeRatio = 0;
		foreach (var gradeTemp in basicGradeList)
		{
			var gradeName = gradeTemp.Item1;
			var gradeRatio = totalGradeRatio + gradeTemp.Item2;
			totalGradeRatio += gradeTemp.Item2;
			gradeList.Add(Tuple.Create(gradeName, gradeRatio));
		}

		var grade = "C";
		var gradeRandom = RandomProvider.Next(1, totalGradeRatio);

		foreach (var gradeTemp in gradeList)
		{
			if (gradeTemp.Item2 >= gradeRandom)
			{
				grade = gradeTemp.Item1;
				break;
			}
		}

		if (rewardGroup == null && cubeItem.Data.Script != null)
		{
			rewardGroup = cubeItem.Data.Script.StrArg;
			totalPrice = (int)cubeItem.Data.Script.NumArg1;
		}

		if (rewardGroup == null)
		{
			CLEAR_GACHA_COMMAND(character, rewardGroup);
			return;
		}

		if (ZoneServer.Instance.World.IsSeason)
			totalPrice = (int)Math.Floor(totalPrice / 2.0);

		if (!character.HasSilver(totalPrice))
		{
			CLEAR_GACHA_COMMAND(character, rewardGroup);
			return;
		}

		var possibleItems = ZoneServer.Instance.Data.CubeGachaDb.FindAll(a => a.Group == rewardGroup).OrderBy(a => a.Id).ToArray();
		var cnt = possibleItems.Count();

		for (var i = 0; i < cnt; i++)
		{
			var possibleItem = possibleItems[i];
			var possibleItemData = ZoneServer.Instance.Data.ItemDb.FindByClass(possibleItem.ItemName);
			if (possibleItemData != null && CHECK_GACHA_DUPLICATE(character, enableDuplicate, possibleItem.ItemName))
			{
				rewardList.Add(new CubeGachaData
				{
					ItemName = possibleItem.ItemName,
					Count = possibleItem.Count,
					Ratio = possibleItem.Ratio,
					//Grade = possibleItem.Grade
				});

				rewardGroupName.Add(possibleItemData.Group);
				rewardCnt.Add(possibleItem.Count);
				ratioList.Add(possibleItem.Ratio);
				listIndex++;
				totalRatio += possibleItem.Ratio;
			}
		}

		// Get reward
		if (listIndex <= 0)
		{
			CLEAR_GACHA_COMMAND(character, rewardGroup);
			return;
		}

		string reward = null;
		int rewardCount = 0;
		double checkTime = DateTime.Now.GetUnixTimestamp() + 1;
		while (reward == null)
		{
			if (checkTime < DateTime.Now.GetUnixTimestamp())
			{
				CLEAR_GACHA_COMMAND(character, rewardGroup);
				return;
			}

			reward = null;
			var result = RandomProvider.Next(1, (int)totalRatio);
			for (var i = 0; i < ratioList.Count; i++)
			{
				if (result <= ratioList[i])
				{
					reward = rewardList[i].ItemName;
					rewardCount = rewardCnt[i];
					rewardGroup = rewardGroupName[i].ToString();
					break;
				}
				else
				{
					ratioList[i + 1] += ratioList[i];
				}
			}

			if (reward != null)
			{
				break;
			}
		}

		// Give reward
		if (reward != null)
		{
			if (totalPrice > 0)
				character.RemoveItem(ItemId.Vis, totalPrice);
			character.AddItem(reward, rewardCount);

			if (rewardGroup != null)
			{
				if (cubeItem.Data.CubeReopen)
				{
					CLEAR_GACHA_COMMAND(character, rewardGroup);
					btnVisible = 0;
				}

				var sucScp = string.Format("GACHA_CUBE_SUCEECD(\'{0}\', \'{1}\', \'{2}\')", cubeItem.Id, reward, btnVisible);
				character.ExecuteClientScript(sucScp);
			}
			else
			{
				var sucScp = string.Format("GACHA_CUBE_SUCEECD_EX(\'{0}\', \'{1}\', \'{2}\')", cubeItem.Id, reward, btnVisible);
				character.ExecuteClientScript(sucScp);
				//UpdateCubeCmd(pc, reward);
			}

			if (!enableDuplicate)
			{
				//UpdateCubeReward(pc, reward);
			}
		}
	}
}
