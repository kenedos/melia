//--- Melia Script ----------------------------------------------------------
// Dina Bee Farm
//--- Description -----------------------------------------------------------
// NPCs found in and around Dina Bee Farm.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters.Components;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai464NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(6, 40120, "Statue of Goddess Vakarine", "f_siauliai_46_4", -435.1169, 148.2241, -1247.06, 91, "WARP_F_SIAULIAI_46_4", "STOUP_CAMP", "STOUP_CAMP");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(34, 147392, "Lv1 Treasure Chest", "f_siauliai_46_4", 1459.06, 188.66, -121.23, 90, "TREASUREBOX_LV_F_SIAULIAI_46_434", "", "");

		// Rasa - Beekeeper (exchanges wild honeycombs for potions)
		//-------------------------------------------------------------------------
		AddNpc(147476, L("[Beekeeper] Rasa"), "f_siauliai_46_4", -123, -1176, 270, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Rasa"));

			var combCount = character.Inventory.CountItem(ItemId.SIAULIAI_46_3_MQ_01_ITEM);

			await dialog.Msg(L("The wild hives out in the grove get fat off the same flowers I tend, but the bees there are mean. If you crack one open and bring me the honeycomb, I'll trade you potions for it."));

			if (combCount <= 0)
			{
				await dialog.Msg(L("Come back when you've got a wild honeycomb. Look for the unkempt hives - they're easy to spot once you know what to listen for."));
				return;
			}

			var response = await dialog.Select(LF("You have {0} wild honeycomb(s). Trade them all?", combCount),
				Option(L("Trade them all"), "trade"),
				Option(L("Keep them for now"), "leave")
			);

			if (response != "trade")
			{
				await dialog.Msg(L("Suit yourself. Hold onto them - they don't spoil."));
				return;
			}

			character.Inventory.Remove(ItemId.SIAULIAI_46_3_MQ_01_ITEM, combCount, InventoryItemRemoveMsg.Given);
			character.Inventory.Add(640004, combCount, InventoryAddType.PickUp); // Large HP Potion
			character.Inventory.Add(640007, combCount, InventoryAddType.PickUp); // Large SP Potion

			await dialog.Msg(LF("{0} honeycomb(s) traded. Pleasure doing business - bring more whenever the bees haven't sent you running.", combCount));
		});
	}
}
