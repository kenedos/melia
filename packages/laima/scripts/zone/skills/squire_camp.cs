//--- Melia Script ----------------------------------------------------------
// Base Camp
//--- Description -----------------------------------------------------------
// Handles building, using, extending and removing a Squire's Base Camp.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Scripting.Shortcuts;

public class SquireCampScript : GeneralScript
{
	private const int CampMonsterId = 57446;
	private const string CampKitClassName = "misc_campkit";
	private const int CampKitAmount = 10;
	private const int BuildPrice = 1000;
	private const int ExtendPrice = 2000;

	private const string CampDialogName = "SQUIRE_BASECAMP";
	private const string SuppliesShopName = "BaseCampSupplies";
	private const float CampRange = 100;

	private const int SmallHpPotionItemId = 640002;
	private const int SmallSpPotionItemId = 640005;
	private const int SmallHpPotionPrice = 80;
	private const int SmallSpPotionPrice = 120;

	private const string BuildDisplayText = "!@#$BUILD_ING#@!";
	private const string BuildAnimation = "SKL_SQUIRE_TENT_BORN";
	private readonly static TimeSpan BuildTime = TimeSpan.FromSeconds(23);

	private readonly static TimeSpan BaseDuration = TimeSpan.FromHours(1);
	private readonly static TimeSpan DurationPerSkillLevel = TimeSpan.FromMinutes(30);

	protected override void Load()
	{
		CreateSuppliesShop();
	}

	/// <summary>
	/// Creates the shop a Base Camp keeps stocked.
	/// </summary>
	private void CreateSuppliesShop()
	{
		CreateShop(SuppliesShopName, shop =>
		{
			shop.AddItem(SmallHpPotionItemId, amount: 1, price: SmallHpPotionPrice);
			shop.AddItem(SmallSpPotionItemId, amount: 1, price: SmallSpPotionPrice);
		});
	}

	/// <summary>
	/// Builds a Base Camp where the character is standing.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="numArg1">The id of the skill the camp is built with.</param>
	/// <param name="numArg2"></param>
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
		if (!BaseCampHelper.TryGet(character, out var camp) || camp.Npc.Handle != numArg1)
			return CustomCommandResult.Fail;

		BaseCampHelper.Remove(camp);

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
		if (!BaseCampHelper.TryGet(character, out var camp) || camp.Npc.Handle != numArg1)
			return CustomCommandResult.Fail;

		return TryExtendCamp(character, camp) ? CustomCommandResult.Okay : CustomCommandResult.Fail;
	}

	/// <summary>
	/// Charges the character for another stretch of their camp's life and
	/// returns whether it was extended.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="camp"></param>
	private static bool TryExtendCamp(Character character, BaseCamp camp)
	{
		if (character.Inventory.CountItem(ItemId.Silver) < ExtendPrice)
		{
			character.SystemMessage("NotEnoughMoney");
			return false;
		}

		if (character.RemoveItem(ItemId.Silver, ExtendPrice) != ExtendPrice)
			return false;

		camp.ExpirationTime += GetCampDuration(camp.SkillLevel);

		return true;
	}

	/// <summary>
	/// Returns how long a camp built with the given skill level stands.
	/// </summary>
	/// <param name="skillLevel"></param>
	private static TimeSpan GetCampDuration(int skillLevel)
		=> BaseDuration + DurationPerSkillLevel * skillLevel;

	/// <summary>
	/// Puts a camp down where the character is standing.
	/// </summary>
	/// <param name="creator"></param>
	/// <param name="skillLevel"></param>
	private static void CreateCamp(Character creator, int skillLevel)
	{
		var hadCamp = BaseCampHelper.TryGet(creator, out _);

		var name = LF("{0}'s Base Camp", creator.Name);

		var npc = new Npc(CampMonsterId, name, creator.Position, creator.Direction);
		npc.Layer = creator.Layer;
		npc.OwnerHandle = creator.Handle;
		npc.DisappearTime = DateTime.Now + GetCampDuration(skillLevel);
		npc.Properties.SetFloat(PropertyName.Range, CampRange);
		npc.SetClickTrigger(CampDialogName, CampDialog);

		creator.Map.AddMonster(npc);

		BaseCampHelper.Register(new BaseCamp(npc, creator, skillLevel));

		Send.ZC_CAMPINFO(creator.Connection, creator.Connection.Account.Id, creator.Map.Data.Id);

		if (hadCamp)
			creator.ServerMessage(L("Your previous Base Camp was taken down."));
	}

	/// <summary>
	/// Offers everything a Base Camp does to whoever clicked it.
	/// </summary>
	/// <param name="dialog"></param>
	private static async Task CampDialog(Dialog dialog)
	{
		var character = dialog.Player;

		if (!BaseCampHelper.TryGetByHandle(dialog.Npc.Handle, out var camp))
			return;

		dialog.SetTitle(L("Base Camp"));

		var isOwner = camp.OwnerObjectId == character.ObjectId;
		var remaining = BaseCampHelper.GetRemainingTime(camp);

		var options = new List<DialogOption>
		{
			Option(L("Use the storage"), "storage"),
			Option(L("Buy supplies"), "shop"),
			Option(L("Rest at the camp"), "rest"),
			Option(L("Travel"), "travel"),
		};

		if (isOwner)
		{
			options.Add(Option(L("Extend the camp"), "extend"));
			options.Add(Option(L("Take the camp down"), "remove"));
		}

		options.Add(Option(L("Leave"), "leave"));

		var text = LF("{0}'s Base Camp in {1}.{{nl}}It stands for another {2} hour(s) and {3} minute(s).",
			camp.OwnerName, BaseCampHelper.GetMapName(camp), (int)remaining.TotalHours, remaining.Minutes);

		var response = await dialog.Select(text, options);

		switch (response)
		{
			case "storage":
				await dialog.OpenPersonalStorage();
				break;

			case "shop":
				await dialog.OpenShop(SuppliesShopName);
				break;

			case "rest":
				StartCampBuff(character, camp);
				break;

			case "travel":
				await OpenWarpDestinations(dialog);
				break;

			case "extend":
				if (TryExtendCamp(character, camp))
				{
					var extended = BaseCampHelper.GetRemainingTime(camp);
					await dialog.Msg(LF("The camp stands for another {0} hour(s) and {1} minute(s).",
						(int)extended.TotalHours, extended.Minutes));
				}
				break;

			case "remove":
				if (await dialog.YesNo(L("Take the camp down?")))
					BaseCampHelper.Remove(camp);
				break;
		}
	}

	/// <summary>
	/// Grants the character the camp's experience buff.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="camp"></param>
	private static void StartCampBuff(Character character, BaseCamp camp)
	{
		if (!ZoneServer.Instance.Data.SkillDb.TryFind(SkillId.Squire_Camp, out var skillData))
			return;

		var duration = TimeSpan.FromSeconds(skillData.CaptionTime + skillData.CaptionTimeByLevel * camp.SkillLevel);

		character.StartBuff(BuffId.BaseCamp_Buff, camp.SkillLevel, 0, duration, character, SkillId.Squire_Camp);
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
