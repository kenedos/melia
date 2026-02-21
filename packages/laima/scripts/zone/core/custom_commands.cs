//--- Melia Script ----------------------------------------------------------
// Custom Command Scripts
//--- Description -----------------------------------------------------------
// Handles "Custom Command" requests from the client.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.ObjectProperties;
using Melia.Shared.Util;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Quests;
using Yggdrasil.Logging;

public class CustomCommandFunctionsScript : GeneralScript
{
	[ScriptableFunction]
	public CustomCommandResult SCR_MARKET_UI_OPEN(Character character, int numArg1, int numArg2, int numArg3)
	{
		return CustomCommandResult.Okay;
	}

	[ScriptableFunction]
	public CustomCommandResult SCR_LAST_INFOSET_OPEN(Character character, int numArg1, int numArg2, int numArg3)
	{
		// Example: SCR_LAST_INFOSET_OPEN(2, 0, 0)

		// Sent from "chase info", notifying us about the last
		// selected index, with 1 being "achieve" and 2 "quest".
		// Potentially related to adventure book? In any case,
		// we're going to ignore this for now, to get rid of
		// unhandled message upon login.

		return CustomCommandResult.Okay;
	}

	[ScriptableFunction]
	public CustomCommandResult SCR_GUILD_PROMOTE_NOTICE_COUNT(Character character, int numArg1, int numArg2, int numArg3)
	{
		// Example: SCR_GUILD_PROMOTE_NOTICE_COUNT(0, 0, 0)

		// Sent from "system menu" if player is not in a guild.
		// I can only assume that responding to it would make
		// some kind of "OMG! JOIN A GUILD!" message appear,
		// so we're going to promptly ignore this.

		return CustomCommandResult.Okay;
	}

	[ScriptableFunction]
	public CustomCommandResult SCR_HAT_VISIBLE_STATE(Character character, int numArg1, int numArg2, int numArg3)
	{
		var headgearIndex = numArg1;

		switch (headgearIndex)
		{
			case 0: character.VisibleEquip ^= VisibleEquip.Headgear1; break;
			case 1: character.VisibleEquip ^= VisibleEquip.Headgear2; break;
			case 2: character.VisibleEquip ^= VisibleEquip.Headgear3; break;
		}

		Send.ZC_UPDATED_PCAPPEARANCE(character);
		Send.ZC_NORMAL.HeadgearVisibilityUpdate(character);

		return CustomCommandResult.Okay;
	}

	[ScriptableFunction]
	public CustomCommandResult SCR_HAIR_WIG_VISIBLE_STATE(Character character, int numArg1, int numArg2, int numArg3)
	{
		character.VisibleEquip ^= VisibleEquip.Wig;

		var wigVisible = (character.VisibleEquip & VisibleEquip.Wig) != 0;
		var hairItem = character.Inventory.GetEquip(EquipSlot.Hair);

		// Send appearance update to others and self
		Send.ZC_UPDATED_PCAPPEARANCE(character);
		Send.ZC_NORMAL.WigVisibilityUpdate(character);

		// Send UpdateCharacterLook to show/hide the wig hair style
		if (hairItem != null && hairItem.Id != 12101) // 12101 is default "no hair costume"
		{
			var strArg = hairItem.Data?.Script?.StrArg ?? "";
			if (!string.IsNullOrEmpty(strArg))
			{
				if (wigVisible)
				{
					// Wig visible - show wig hair style
					if (ZoneServer.Instance.Data.HairTypeDb.TryFind(character.Gender, strArg, out var hairData))
						Send.ZC_NORMAL.UpdateCharacterLook(character, hairItem.Id, EquipSlot.Hair, hairData.Index);
					else if (ZoneServer.Instance.Data.HeadTypeDb.TryFind(character.Gender, strArg, out var headData))
						Send.ZC_NORMAL.UpdateCharacterLook(character, hairItem.Id, EquipSlot.Hair, headData.Index);
				}
				else
				{
					// Wig hidden - show default hair (index 0)
					Send.ZC_NORMAL.UpdateCharacterLook(character, hairItem.Id, EquipSlot.Hair, 0);
				}
			}
		}

		return CustomCommandResult.Okay;
	}

	[ScriptableFunction]
	public CustomCommandResult SCR_SUBWEAPON_VISIBLE_STATE(Character character, int numArg1, int numArg2, int numArg3)
	{
		character.VisibleEquip ^= VisibleEquip.SubWeapon;

		Send.ZC_UPDATED_PCAPPEARANCE(character);
		Send.ZC_NORMAL.SubWeaponVisibilityUpdate(character);

		return CustomCommandResult.Okay;
	}

	[ScriptableFunction]
	public CustomCommandResult SCR_PET_ACTIVATE(Character character, int numArg1, int numArg2, int numArg3)
	{
		var companions = character.Companions.GetList();
		if (companions.Count == 0)
			return CustomCommandResult.Fail;
		var companion = companions.FirstOrDefault();
		if (companion == null && !character.Companions.TryGetCompanion(a => a.CompanionData.JobId == (int)JobId.Falconer, out companion))
			return CustomCommandResult.Fail;

		var nextActivateSec = companion.Vars.Get<DateTime>("NextActivateTime", DateTime.MinValue);
		if (DateTime.Now < nextActivateSec)
			return CustomCommandResult.Okay;

		var ownerFaction = character.Faction;
		if (ownerFaction == FactionType.Peaceful && (character.Map.IsPVP || ZoneServer.Instance.World.IsPVP))
		{
			var etc = character.Etc.Properties;
			if (etc[PropertyName.Team_Mission] == 0)
			{
				//companion.ChangeFaction(FactionType.Peaceful);
				//SetHideCheckScript(companion, "HIDE_FROM_PVP_PLAYERS");
				companion.StartBuff(BuffId.GuildBattle_Observe, TimeSpan.Zero, companion);
			}
		}

		companion.Vars.Set("NextActivateTime", DateTime.Now + TimeSpan.FromSeconds(5 - 1));

		if (companion.IsActivated)
		{
			if (companion.IsRiding)
			{
				character.SystemMessage("DownFromPetFirst");
				return CustomCommandResult.Okay;
			}
			companion.SetCompanionState(false);
		}
		else
		{
			companion.SetCompanionState(true);
		}

		return CustomCommandResult.Okay;
	}

	[ScriptableFunction]
	public CustomCommandResult SCR_COMPANION_GIVE_FEED(Character character, int numArg1, int numArg2, int numArg3)
	{
		var companionHandle = numArg1;
		var itemInvIndex = numArg2;

		if (!character.Companions.TryGetCompanion(a => a.Handle == companionHandle, out var companion))
		{
			return CustomCommandResult.Fail;
		}

		if (companion.IsDead)
		{
			return CustomCommandResult.Fail;
		}

		if (!character.Inventory.TryGetItemByIndex(itemInvIndex, out var item))
		{
			return CustomCommandResult.Fail;
		}

		if (!item.Data.HasScript || item.Data.Script.Function != "PET_FOOD_USE")
		{
			return CustomCommandResult.Fail;
		}

		if (item.IsLocked)
		{
			return CustomCommandResult.Fail;
		}

		var cooldowns = character.Components.Get<CooldownComponent>();
		if (cooldowns.IsOnCooldown(item.Data.CooldownId))
		{
			return CustomCommandResult.Fail;
		}

		var staminaAmount = (int)item.Data.Script.NumArg1;
		var foodType = (CompanionFoodType)(int)item.Data.Script.NumArg2;
		var animationName = item.Data.Script.StrArg;

		companion.Feed(staminaAmount, foodType, animationName);

		if (item.Data.HasCooldown)
		{
			var cooldownTime = item.Data.CooldownTime;
			cooldownTime *= ZoneServer.Instance.Conf.World.ItemCooldownRate;

			if (cooldownTime > TimeSpan.Zero)
				cooldowns.Start(item.Data.CooldownId, cooldownTime);
		}

		character.Inventory.Remove(item, 1, InventoryItemRemoveMsg.Used);

		return CustomCommandResult.Okay;
	}

	[ScriptableFunction]
	public CustomCommandResult SCR_COMPANION_STROKE(Character character, int numArg1, int numArg2, int numArg3)
	{
		if (!character.TryGetActiveCompanion(out var companion))
		{
			return CustomCommandResult.Fail;
		}

		Send.ZC_ENABLE_CONTROL(character.Connection, "PlaySumAni", false);

		//bool saniRet = PlaySumAni(self, compa, "pet", 80);
		//WaitSumAniEnd(self);

		Send.ZC_NORMAL.AttackCancelBow(character);

		Send.ZC_ENABLE_CONTROL(character.Connection, "PlaySumAni", true);

		if (!character.TryGetCompanionProperty(companion, "FriendPointTime", out var friendPointTime, "None"))
			friendPointTime = "None";

		var nextAbleTime = DateTime.MinValue;
		if (friendPointTime != "None")
			friendPointTime.TryGetPropertyStringToDateTime(out nextAbleTime);
		var sysTime = DateTime.Now;

		if (sysTime > nextAbleTime)
		{
			var nextSec = 20 * 60;
			nextAbleTime = DateTime.Now.AddSeconds(nextSec);
			var strNextTime = nextAbleTime.ToPropertyDateTimeString();
			if (strNextTime == null)
			{
				strNextTime = "None";
			}

			character.TryGetCompanionProperty(companion, "FriendPoint", out var friendPointStr, "0");
			var friendPoint = int.Parse(friendPointStr);
			var nextPoint = friendPoint + 10;
			character.TryGetCompanionProperty(companion, "FriendLevel", out var friendLevelStr, "1");
			var friendLevel = int.Parse(friendLevelStr);

			var nextLevel = ZoneServer.Instance.Data.ExpDb.GetLevel(ExpType.Pet, nextPoint);
			var changeLv = 0;
			if (nextLevel != friendLevel)
				changeLv = nextLevel;

			companion.AttachEffect("F_sys_heart", 2, EffectLocation.Top);

			character.SetCompanionProperty(companion, "FriendPointTime", strNextTime);
			character.SetCompanionProperty(companion, "FriendPoint", nextPoint);
			if (changeLv != 0)
			{
				character.SetCompanionProperty(companion, "FriendLevel", changeLv);
				companion.AttachEffect("F_pc_level_up", 3, EffectLocation.Middle);
			}

			Task.Delay(2000).ContinueWith(_ =>
			{
				companion.DetachEffect("F_sys_heart");
			});
		}

		return CustomCommandResult.Okay;
	}

	[ScriptableFunction]
	public CustomCommandResult SCR_CLICK_CHANGEJOB_BUTTON(Character character, int numArg1, int numArg2, int numArg3)
	{
		var jobId = (JobId)numArg1;
		var username = character.Connection.Account.Name;

		if (ZoneServer.Instance.Conf.World.NoAdvancement)
		{
			Log.Warning("CZ_CUSTOM_COMMAND: User '{0}' tried to switch jobs, despite job advancement being disabled.", username);
			return CustomCommandResult.Fail;
		}

		if (!ZoneServer.Instance.Data.JobDb.TryFind(jobId, out var jobData))
		{
			Log.Warning("CZ_CUSTOM_COMMAND: User '{0}' requested job change to missing job '{1}'.", username, jobId);
			return CustomCommandResult.Fail;
		}

		if (jobData.Rank >= 99)
		{
			Log.Warning("CZ_CUSTOM_COMMAND: User '{0}' requested job '{1}' isn't allowed via UI.", username, jobId);
			return CustomCommandResult.Fail;
		}

		if (character.Job.Level < character.Job.MaxLevel)
		{
			Log.Warning("CZ_CUSTOM_COMMAND: User '{0}' requested job change before reaching their current job's max level of {1}.", username, character.Job.MaxLevel);
			return CustomCommandResult.Fail;
		}

		if (character.JobClass != jobData.JobClassId)
		{
			Log.Warning("CZ_CUSTOM_COMMAND: User '{0}' requested job change to job '{1}', a '{2}' job, while being a '{3}'.", username, jobId, jobData.JobClassId, character.JobClass);
			return CustomCommandResult.Fail;
		}

		if (character.Jobs.Has(jobId))
		{
			Log.Warning("CZ_CUSTOM_COMMAND: User '{0}' requested job change to job '{1}' despite already having it.", username, jobId);
			return CustomCommandResult.Fail;
		}

		if (character.Jobs.GetCurrentRank() >= ZoneServer.Instance.Conf.World.JobMaxRank)
		{
			Log.Warning("CZ_CUSTOM_COMMAND: User '{0}' requested job change to job '{1}' at or above the max rank of {2}.", username, jobId, ZoneServer.Instance.Conf.World.JobMaxRank);
			return CustomCommandResult.Fail;
		}

		if (ZoneServer.Instance.Conf.World.UseJobQuests)
		{
			if (!character.Variables.Perm.GetBool("JobAdvancement", false))
			{
				// For now if quest doesn't exist just change job.
				// At some point we'll add other job quests.
				var questId = new QuestId("Laima.JobQuest", (int)jobId);
				if (QuestScript.Exists(questId))
					character.Quests.Start(questId);
				else
					character.ChangeJob(jobId);
			}
			else
			{
				character.WorldMessage("You already have an job advancement quest in progress. Please finish or abandon the quest.");
			}
		}
		else
		{
			character.ChangeJob(jobId);
		}

		return CustomCommandResult.Okay;
	}

	// The JANSORI_COUNT custom command was once used to disable a certain
	// tooltip. When the player clicked on it, this command was sent and
	// and set a session object to remember not to show the tooltip again.
	// This command was removed at some point though. We'll leave this here
	// for reference for the time being.
	//[ScriptableFunction]
	//public CustomCommandResult JANSORI_COUNT(Character character, int numArg1, int numArg2, int numArg3)
	//{
	//	var classId = numArg1;
	//	var cmdArg = numArg2;

	//	// Disable "You can buy items" tooltip, sent after
	//	// opening a shop.
	//	if (classId == 5 && cmdArg == 1)
	//	{
	//		// The property is on the session object "Jansori".
	//		var jansori = character.SessionObjects.Get(SessionObjectId.Jansori);
	//		jansori.Properties.SetFloat(PropertyName.Shop_Able_Clicked, 1);

	//		Send.ZC_OBJECT_PROPERTY(character.Connection, jansori, PropertyName.Shop_Able_Clicked);
	//	}

	//	return CustomCommandResult.Okay;
	//}
}
