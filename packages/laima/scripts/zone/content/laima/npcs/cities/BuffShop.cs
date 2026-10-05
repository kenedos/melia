using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using static Melia.Zone.Scripting.Shortcuts;

namespace ScriptsZoneLaima.content.laima.npcs.cities
{
	public class BuffShopNpcScript : GeneralScript
	{
		private const int BuffPrice = 25_000;
		private const int AllBuffsPrice = BuffPrice * 4;
		private static readonly TimeSpan BuffDuration = TimeSpan.FromMinutes(30);

		protected override void Load()
		{
			AddNpc(12063, L("[Buff Shop] Cristiano Buffer"), "Cristiano Buffer", "c_Klaipe", -760, 570, 0, async dialog =>
			{
				dialog.SetTitle(L("Buff Shop"));
				await this.ShowBuffSelection(dialog);
			});
		}

		private async Task ShowBuffSelection(Dialog dialog)
		{
			while (true)
			{
				var response = await dialog.Select(
					L(
						"Welcome to the Buff Shop.{nl}{nl}" +
						"Each buff costs 50,000 Silver and lasts 30 minutes.{nl}" +
						"All four buffs can be purchased together for 200,000 Silver.{nl}{nl}" +
						"Choose a buff."
					),
					Option(L("Grace: Additional Holy Hits"), "sacrament"),
					Option(L("Grace: Additional Damage"), "blessing"),
					Option(L("Grace: Increase Magic Defense"), "magic_defense"),
					Option(L("Grace: Increase Physical Defense"), "physical_defense"),
					Option(L("Buy All Buffs - 200,000 Silver"), "buy_all"),
					Option(L("Exit"), "exit")
				);

				switch (response)
				{
					case "sacrament":
						await this.ConfirmPurchase(dialog, BuffId.SpellShop_Sacrament_Buff, "Grace: Additional Holy Hits");
						break;

					case "blessing":
						await this.ConfirmPurchase(dialog, BuffId.SpellShop_Blessing_Buff, "Grace: Additional Damage");
						break;

					case "magic_defense":
						await this.ConfirmPurchase(dialog, BuffId.SpellShop_IncreaseMagicDEF_Buff, "Grace: Increase Magic Defense");
						break;

					case "physical_defense":
						await this.ConfirmPurchase(dialog, BuffId.SpellShop_Aspersion_Buff, "Grace: Increase Physical Defense");
						break;

					case "buy_all":
						await this.ConfirmPurchaseAll(dialog);
						break;

					case "exit":
					default:
						return;
				}
			}
		}

		private async Task ConfirmPurchase(Dialog dialog, BuffId buffId, string buffName)
		{
			var response = await dialog.Select(
				L(
					"Selected buff: " + buffName + "{nl}" +
					"Duration: 30 minutes{nl}" +
					"Price: 50,000 Silver{nl}{nl}" +
					"Do you want to purchase this buff?"
				),
				Option(L("Purchase"), "purchase"),
				Option(L("Cancel"), "cancel")
			);

			if (response != "purchase")
				return;

			await this.PurchaseBuff(dialog, buffId, buffName);
		}

		private async Task ConfirmPurchaseAll(Dialog dialog)
		{
			var response = await dialog.Select(
				L(
					"Purchase all four Grace buffs?{nl}{nl}" +
					"Grace: Additional Holy Hits{nl}" +
					"Grace: Additional Damage{nl}" +
					"Grace: Increase Magic Defense{nl}" +
					"Grace: Increase Physical Defense{nl}{nl}" +
					"Duration: 30 minutes{nl}" +
					"Total price: 200,000 Silver"
				),
				Option(L("Purchase All"), "purchase_all"),
				Option(L("Cancel"), "cancel")
			);

			if (response != "purchase_all")
				return;

			await this.PurchaseAllBuffs(dialog);
		}

		private async Task PurchaseBuff(Dialog dialog, BuffId buffId, string buffName)
		{
			var character = dialog.Player;
			var inventory = character.Inventory;
			var availableSilver = inventory.CountItem(ItemId.Silver);

			if (availableSilver < BuffPrice)
			{
				await dialog.Msg(L(
					"You do not have enough Silver.{nl}{nl}" +
					"Required: 50,000 Silver{nl}" +
					"Available: " + availableSilver + " Silver"
				));

				return;
			}

			var removedAmount = inventory.Remove(ItemId.Silver, BuffPrice, InventoryItemRemoveMsg.PaidWith);

			if (removedAmount != BuffPrice)
			{
				await dialog.Msg(L("The purchase could not be completed.{nl}Your Silver amount changed during the transaction."));
				return;
			}

			character.StartBuff(buffId, 5f, 0f, BuffDuration, character);

			await dialog.Msg(L(buffName + " has been applied.{nl}Duration: 30 minutes."));
		}

		private async Task PurchaseAllBuffs(Dialog dialog)
		{
			var character = dialog.Player;
			var inventory = character.Inventory;
			var availableSilver = inventory.CountItem(ItemId.Silver);

			if (availableSilver < AllBuffsPrice)
			{
				await dialog.Msg(L(
					"You do not have enough Silver.{nl}{nl}" +
					"Required: 200,000 Silver{nl}" +
					"Available: " + availableSilver + " Silver"
				));

				return;
			}

			var removedAmount = inventory.Remove(ItemId.Silver, AllBuffsPrice, InventoryItemRemoveMsg.PaidWith);

			if (removedAmount != AllBuffsPrice)
			{
				await dialog.Msg(L("The purchase could not be completed.{nl}Your Silver amount changed during the transaction."));
				return;
			}

			character.StartBuff(BuffId.SpellShop_Sacrament_Buff, 5f, 0f, BuffDuration, character);
			character.StartBuff(BuffId.SpellShop_Blessing_Buff, 5f, 0f, BuffDuration, character);
			character.StartBuff(BuffId.SpellShop_IncreaseMagicDEF_Buff, 5f, 0f, BuffDuration, character);
			character.StartBuff(BuffId.SpellShop_Aspersion_Buff, 5f, 0f, BuffDuration, character);

			await dialog.Msg(L("All four Grace buffs have been applied.{nl}Duration: 30 minutes."));
		}
	}
}
