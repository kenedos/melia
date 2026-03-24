//----- Melia Script ----------------------------------------------------------
//-- Money Exchanger
//----- Description -----------------------------------------------------------
//-- Trades silver for paper currency items and vice versa.
//-----------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using static Melia.Zone.Scripting.Shortcuts;

public class CustomNpcMoneyExchanger : GeneralScript
{
	protected override void Load()
	{
		AddNpc(157039, L("[Silver Exchanger] Sandra"), "c_Klaipe", 312, -1, 90, NpcDialog);
		AddNpc(157039, L("[Silver Exchanger] Sandra"), "c_orsha", 214, -10, 0, NpcDialog);
		AddNpc(157039, L("[Silver Exchanger] Sandra"), "c_fedimian", -427, -524, 180, NpcDialog);
	}

	private async Task NpcDialog(Dialog dialog)
	{
		var pc = dialog.Player;
		dialog.SetPortrait("Dlg_port_Sandra");

		var selection = await dialog.Select(
			L("Hi there! I can help you exchange your silver for Goddess Bills. With those, you'll be able to better commercialize your goods with other people! Paper money is the best!"),
			Option(L("Exchange Silver"), "silver"),
			Option(L("Exchange Bills"), "bill"),
			Option(L("Nothing"), "nothing")
		);

		if (selection == "nothing")
		{
			await dialog.Msg(L("Please come back any time."));
			return;
		}

		if (selection == "silver")
		{
			await dialog.Msg(L("Want some shiny paper money? That's the best decision you will ever make!"));

			var currentSilver = pc.Inventory.CountItem(ItemId.Vis);
			var inputAmount = await dialog.Input(LF("Looks like you have {0} silver. How much do you want to exchange?", currentSilver));

			if (!int.TryParse(inputAmount, out var amount) || amount < 1000)
			{
				await dialog.Msg(L("Sorry, I can only exchange 1000 silver or more!"));
				return;
			}

			if (amount > currentSilver)
			{
				await dialog.Msg(L("You don't have that much silver! Check your pockets again."));
				return;
			}

			var ks = amount / 1000;
			pc.RemoveItem(ItemId.Vis, ks * 1000);

			while (ks > 0)
			{
				if (ks >= 500)
				{
					pc.AddItem(ItemId.Event_1704_Cash_500000, 1);
					ks -= 500;
				}
				else if (ks >= 100)
				{
					pc.AddItem(ItemId.Event_1704_Cash_100000, 1);
					ks -= 100;
				}
				else if (ks >= 50)
				{
					pc.AddItem(ItemId.Event_1704_Cash_50000, 1);
					ks -= 50;
				}
				else if (ks >= 10)
				{
					pc.AddItem(ItemId.Event_1704_Cash_10000, 1);
					ks -= 10;
				}
				else if (ks >= 5)
				{
					pc.AddItem(ItemId.Event_1704_Cash_5000, 1);
					ks -= 5;
				}
				else
				{
					pc.AddItem(ItemId.Event_1704_Cash_1000, ks);
					ks = 0;
				}
			}

			await dialog.Msg(L("Thanks for your business!"));
		}
		else if (selection == "bill")
		{
			await dialog.Msg(L("Eh? You want silver? But paper is the future! No, really! Would I lie to you? ... Well, suit yourself."));

			var bills = new[] { ItemId.Event_1704_Cash_500000, ItemId.Event_1704_Cash_100000, ItemId.Event_1704_Cash_50000, ItemId.Event_1704_Cash_10000, ItemId.Event_1704_Cash_5000, ItemId.Event_1704_Cash_1000 };
			var steps = new[] { 500, 100, 50, 10, 5, 1 };
			var totalAvailableSilver = 0;

			for (var i = 0; i < bills.Length; i++)
			{
				var available = pc.Inventory.CountItem(bills[i]);
				totalAvailableSilver += available * steps[i] * 1000;
			}

			var inputAmount = await dialog.Input(LF("You have bills worth {0} silver. How much silver do you want?", totalAvailableSilver));

			if (!int.TryParse(inputAmount, out var requestedAmount) || requestedAmount < 1000)
			{
				await dialog.Msg(L("Sorry, I have a 1000 silver minimum."));
				return;
			}

			var amountToExchange = Math.Min(requestedAmount, totalAvailableSilver);
			var ks = amountToExchange / 1000;
			var remainingKs = ks;
			var exchangedSilver = 0;

			for (var i = 0; i < bills.Length && remainingKs > 0; i++)
			{
				var step = steps[i];
				var available = pc.Inventory.CountItem(bills[i]);
				var needed = Math.Min(remainingKs / step, available);

				if (needed > 0)
				{
					pc.RemoveItem(bills[i], needed);
					exchangedSilver += needed * step * 1000;
					remainingKs -= needed * step;
				}
			}

			pc.AddItem(ItemId.Vis, exchangedSilver);

			if (requestedAmount > totalAvailableSilver)
			{
				await dialog.Msg(LF("I could only give you {0} silver for your bills. That's all you had!", exchangedSilver));
			}
			else
			{
				await dialog.Msg(LF("Here's your {0} silver. Thanks for the business!", exchangedSilver));
			}
		}
	}
}
