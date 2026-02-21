//--- Melia Script ----------------------------------------------------------
// Fashion Redemption NPC
//--- Description -----------------------------------------------------------
// Allows players to exchange collected "trash" items for fashionable gear.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using static Melia.Zone.Scripting.Shortcuts;

public class FashionRedemptionScript : GeneralScript
{
	// protected override void Load()
	// {
	// 	AddNpc(20056, L("[Fashionista] Emilia"), "Emilia", "c_Klaipe", 300, 90, 90, NpcDialog);
	// }

	private async Task NpcDialog(Dialog dialog)
	{
		var npc = dialog.Npc;
		var character = dialog.Player;
		var title = L(npc.UniqueName);
		var portrait = npc.UniqueName.ToUpper() + "_DLG";
		if (ZoneServer.Instance.Data.MonsterDb.TryFind(npc.Id, out var monsterData))
			portrait = monsterData.ClassName.ToUpper() + "_DLG";
		var inventory = character.Inventory;
		var redemptionSetDb = ZoneServer.Instance.Data.RedemptionDb;

		dialog.SetTitle(title);
		dialog.SetPortrait(portrait);

		await dialog.Msg(L("Welcome, adventurer! I have some special items available for those who have diligently gathered certain... resources."));

		// --- List Available Sets ---
		var availableSets = redemptionSetDb.Entries.Values
			.Where(set => !character.Variables.Perm.Has(set.SetName)) // Check if player has all rewards from a set 
			.Select(set => Option(L(set.SetName), set.SetName))
			.ToList();

		if (availableSets.Count == 0)
		{
			await dialog.Msg(L("It seems you've already redeemed everything I have to offer!  Come back later, perhaps I'll have new items in stock."));
			return;
		}

		var options = availableSets;
		options.Add(Option(L("Nothing right now."), "end"));

		var selection = await dialog.Select(L("What can I see?"), options.ToArray());

		if (selection == "end")
		{
			await dialog.Msg(L("Come back if you find something interesting."));
			return;
		}


		if (!redemptionSetDb.TryFind(selection, out var redemptionSet))
		{
			await dialog.Msg(L("Hmm, something seems to be wrong with my records for that set."));
			return;
		}

		var itemId = redemptionSet.MaterialItemId;
		var itemCount = inventory.CountItem(itemId);

		foreach (var rewardItemEntry in redemptionSet.RewardItems)
		{
			if (rewardItemEntry.Gender != Gender.All && rewardItemEntry.Gender != character.Gender)
				continue;

			foreach (var (rewardItemId, amount) in rewardItemEntry.Items)
			{
				if (character.Variables.Perm.GetBool(redemptionSet.SetName + rewardItemId))
					continue;

				var requiredAmount = redemptionSet.RequiredAmounts[rewardItemId];

				if (itemCount >= requiredAmount && !character.Variables.Perm.Has(redemptionSet.SetName + rewardItemId))
				{
					await dialog.Msg(LF("You've collected enough {0} for a {1}! I can craft one for you.",
						L(GetItemName(itemId)), L(GetItemName(rewardItemId))));

					inventory.Remove(itemId, requiredAmount, InventoryItemRemoveMsg.Given);
					inventory.Add(rewardItemId, amount, InventoryAddType.New);

					character.Variables.Perm.SetBool(redemptionSet.SetName + rewardItemId, true);

					if (redemptionSet.RewardItems
							.Where(ri => ri.Gender == Gender.All || ri.Gender == character.Gender)
							.SelectMany(ri => ri.Items)
							.All(kvp => character.Variables.Perm.GetBool(redemptionSet.SetName + kvp.Key)))
					{
						character.Variables.Perm.SetBool(redemptionSet.SetName, true);
						await dialog.Msg(L("You've completed this set! Congratulations!"));
					}
					await dialog.Msg(L("There you go! Come back if you need anything else."));
					return;
				}
			}
		}

		await dialog.Msg(LF("You have {0} {1}. That's not enough for anything yet.", itemCount, L(GetItemName(itemId))));
	}

	// Helper function to get localized item name
	private string GetItemName(int itemId)
	{
		if (ZoneServer.Instance.Data.ItemDb.TryFind(itemId, out var item))
			return $"{item.Name}";
		else
			return $"[Item Id: {itemId}]";
	}
}
