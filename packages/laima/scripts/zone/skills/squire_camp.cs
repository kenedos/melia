//--- Melia Script ----------------------------------------------------------
// Base Camp
//--- Description -----------------------------------------------------------
// Handles building, extending and removing a Squire's Base Camp.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Scripting;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;

public class SquireCampScript : GeneralScript
{
	private const int CampMonsterId = 57446;
	private const string CampKitClassName = "misc_campkit";
	private const int CampKitAmount = 10;
	private const int BuildPrice = 1000;
	private const int ExtendPrice = 2000;

	private const string BuildDisplayText = "!@#$BUILD_ING#@!";
	private const string BuildAnimation = "SKL_SQUIRE_TENT_BORN";
	private readonly static TimeSpan BuildTime = TimeSpan.FromSeconds(23);

	private readonly static TimeSpan BaseDuration = TimeSpan.FromHours(1);
	private readonly static TimeSpan DurationPerSkillLevel = TimeSpan.FromMinutes(30);

	private readonly static Dictionary<long, Mob> ActiveCamps = new();

	/// <summary>
	/// Builds a Base Camp where the character is standing.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="numArg1">The id of the skill the camp is built with.</param>
	/// <param name="numArg2">Whether an existing camp is to be replaced.</param>
	/// <param name="numArg3"></param>
	[ScriptableFunction("SCR_BUILD_CAMP")]
	public CustomCommandResult SCR_BUILD_CAMP(Character character, int numArg1, int numArg2, int numArg3)
	{
		if (!character.TryGetSkill((SkillId)numArg1, out var skill) || skill.Id != SkillId.Squire_Camp)
			return CustomCommandResult.Fail;

		var mapType = character.Map.Data.Type;
		if (mapType != MapType.Field && mapType != MapType.Dungeon)
		{
			character.SystemMessage("DontBuildCampThisAria");
			return CustomCommandResult.Okay;
		}

		if (GetCamp(character) != null && numArg2 == 0)
		{
			character.SystemMessage("DontBuildCampThisAria");
			return CustomCommandResult.Okay;
		}

		if (!TryGetCampKitId(out var campKitId))
			return CustomCommandResult.Fail;

		if (character.Inventory.CountItem(campKitId) < CampKitAmount)
		{
			character.SystemMessage("NotEnoughRecipe");
			return CustomCommandResult.Okay;
		}

		if (character.Inventory.CountItem(ItemId.Silver) < BuildPrice)
		{
			character.SystemMessage("NotEnoughMoney");
			return CustomCommandResult.Okay;
		}

		var skillLevel = skill.Level;

		character.Components.Get<TimeActionComponent>().Start(BuildDisplayText, "None", BuildAnimation, BuildTime,
			(builder, timeAction) => FinishBuild(builder, timeAction, campKitId, skillLevel));

		return CustomCommandResult.Okay;
	}

	/// <summary>
	/// Puts the camp down once the build finished, and charges for it.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="timeAction"></param>
	/// <param name="campKitId"></param>
	/// <param name="skillLevel"></param>
	private static void FinishBuild(Character character, TimeAction timeAction, int campKitId, int skillLevel)
	{
		if (timeAction.Result != TimeActionResult.Completed)
			return;

		if (character.Inventory.Remove(campKitId, CampKitAmount, InventoryItemRemoveMsg.Used) != CampKitAmount)
			return;

		if (character.RemoveItem(ItemId.Silver, BuildPrice) != BuildPrice)
		{
			character.AddItem(campKitId, CampKitAmount);
			return;
		}

		CreateCamp(character, skillLevel);
	}

	/// <summary>
	/// Takes down the character's Base Camp.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="numArg1">The handle of the camp to remove.</param>
	/// <param name="numArg2"></param>
	/// <param name="numArg3"></param>
	[ScriptableFunction("SCR_REMOVE_CAMP")]
	public CustomCommandResult SCR_REMOVE_CAMP(Character character, int numArg1, int numArg2, int numArg3)
	{
		var camp = GetCamp(character);
		if (camp == null || camp.Handle != numArg1)
			return CustomCommandResult.Fail;

		RemoveCamp(character);

		return CustomCommandResult.Okay;
	}

	/// <summary>
	/// Extends how long the character's Base Camp stands for.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="numArg1">The handle of the camp to extend.</param>
	/// <param name="numArg2"></param>
	/// <param name="numArg3"></param>
	[ScriptableFunction("SCR_EXTEND_CAMP_TIME")]
	public CustomCommandResult SCR_EXTEND_CAMP_TIME(Character character, int numArg1, int numArg2, int numArg3)
	{
		var camp = GetCamp(character);
		if (camp == null || camp.Handle != numArg1)
			return CustomCommandResult.Fail;

		if (!character.TryGetSkill(SkillId.Squire_Camp, out var skill))
			return CustomCommandResult.Fail;

		if (character.Inventory.CountItem(ItemId.Silver) < ExtendPrice)
		{
			character.SystemMessage("NotEnoughMoney");
			return CustomCommandResult.Okay;
		}

		if (character.RemoveItem(ItemId.Silver, ExtendPrice) != ExtendPrice)
			return CustomCommandResult.Fail;

		camp.DisappearTime += GetCampDuration(skill.Level);

		return CustomCommandResult.Okay;
	}

	/// <summary>
	/// Returns how long a camp built with the given skill level stands.
	/// </summary>
	/// <param name="skillLevel"></param>
	private static TimeSpan GetCampDuration(int skillLevel)
		=> BaseDuration + DurationPerSkillLevel * skillLevel;

	/// <summary>
	/// Returns the character's camp, if they have one standing.
	/// </summary>
	/// <remarks>
	/// A camp outlives its owner leaving the map, since its storage is meant
	/// to be reachable without them, so an entry here can name one the map
	/// has already reaped.
	/// </remarks>
	/// <param name="character"></param>
	private static Mob GetCamp(Character character)
	{
		lock (ActiveCamps)
		{
			if (!ActiveCamps.TryGetValue(character.ObjectId, out var camp))
				return null;

			if (DateTime.Now < camp.DisappearTime)
				return camp;

			ActiveCamps.Remove(character.ObjectId);
		}

		return null;
	}

	/// <summary>
	/// Removes the character's camp from the map.
	/// </summary>
	/// <param name="character"></param>
	private static void RemoveCamp(Character character)
	{
		var camp = GetCamp(character);
		if (camp == null)
			return;

		lock (ActiveCamps)
			ActiveCamps.Remove(character.ObjectId);

		camp.Map.RemoveMonster(camp);

		if (character.Connection != null)
			Send.ZC_CAMPINFO(character.Connection, character.Connection.Account.Id);
	}

	/// <summary>
	/// Puts a camp down in front of the character.
	/// </summary>
	/// <param name="creator"></param>
	/// <param name="skillLevel"></param>
	private static void CreateCamp(Character creator, int skillLevel)
	{
		RemoveCamp(creator);

		var camp = new Mob(CampMonsterId, RelationType.Neutral);
		camp.Faction = FactionType.Neutral;
		camp.Position = creator.Position;
		camp.Direction = creator.Direction;
		camp.Layer = creator.Layer;
		camp.OwnerHandle = creator.Handle;
		camp.DisappearTime = DateTime.Now + GetCampDuration(skillLevel);

		creator.Map.AddMonster(camp);

		lock (ActiveCamps)
			ActiveCamps[creator.ObjectId] = camp;

		Send.ZC_CAMPINFO(creator.Connection, creator.Connection.Account.Id, creator.Map.Data.Id);
	}

	/// <summary>
	/// Resolves the id of the item a camp is built from.
	/// </summary>
	/// <param name="itemId"></param>
	private static bool TryGetCampKitId(out int itemId)
	{
		itemId = 0;

		if (!ZoneServer.Instance.Data.ItemDb.TryFind(CampKitClassName, out var itemData))
			return false;

		itemId = itemData.Id;
		return true;
	}
}
