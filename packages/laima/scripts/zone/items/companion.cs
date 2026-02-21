//--- Melia Script ----------------------------------------------------------
// Furniture Items
//--- Description -----------------------------------------------------------
// Item-related scripts that grant companion creation.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using static Melia.Zone.Scripting.Shortcuts;

public class CompanionScripts : GeneralScript
{
	//[ScriptableFunction]
	public ItemUseResult SCR_USE_PREMIUM_EGG(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Connection.CurrentDialog != null)
			return ItemUseResult.Fail;

		if (character.ActiveCompanion != null)
			return ItemUseResult.Fail;

		// if (character.Connection.Account.CharacterSlots + 1 < Max Slots Allow) {
		// character.SystemMessage("DontHaveSlot");
		// return ItemUseResult.Fail; }


		character.StartDialog(item, async (dialog) =>
		{
			if (ZoneServer.Instance.Data.MonsterDb.TryFind(strArg, out var monster))
			{
				var name = await dialog.Input(ScpArgMsg("InputCompanionName") + "*@*" + ScpArgMsg("InputCompanionName"));
				var companion = new Companion(character, monster.Id, RelationType.Friendly);
				companion.Name = name;
				character.Companions.CreateCompanion(companion);
				await dialog.ExecuteScript(ClientScripts.PET_ADOPT_SUCCESS);
				companion.SetCompanionState(true);
				character.ConsumeItem(item.ObjectId);
			}
			await Task.Yield();
		});

		return ItemUseResult.OkayNotConsumed;
	}

	[ScriptableFunction]
	public ItemUseResult PET_FOOD_USE(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var companion = character.Companions.ActiveCompanion;
		if (companion == null)
		{
			character.ServerMessage("You must have an active companion to use this item.");
			return ItemUseResult.Fail;
		}

		if (companion.IsDead)
		{
			return ItemUseResult.Fail;
		}

		var cooldowns = character.Components.Get<CooldownComponent>();
		if (cooldowns.IsOnCooldown(item.Data.CooldownId))
		{
			return ItemUseResult.Fail;
		}

		var staminaAmount = (int)numArg1;
		var foodType = (CompanionFoodType)(int)numArg2;
		companion.Feed(staminaAmount, foodType, strArg);

		if (item.Data.HasCooldown)
		{
			var cooldownTime = item.Data.CooldownTime;
			cooldownTime *= ZoneServer.Instance.Conf.World.ItemCooldownRate;

			if (cooldownTime > TimeSpan.Zero)
				cooldowns.Start(item.Data.CooldownId, cooldownTime);
		}

		return ItemUseResult.Okay;
	}
}
