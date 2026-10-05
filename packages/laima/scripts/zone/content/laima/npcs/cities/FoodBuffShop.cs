using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using static Melia.Zone.Scripting.Shortcuts;

namespace ScriptsZoneLaima.content.laima.npcs.cities
{
	public class FoodBuffShopNpcScript : GeneralScript
	{
		private const int BuffPrice = 25_000;
		private const int AllBuffsPrice = BuffPrice * 6;
		private const int FoodBuffLevel = 10;
		private static readonly TimeSpan BuffDuration = TimeSpan.FromMinutes(30);

		protected override void Load()
		{
			AddNpc(57457, L("[Food Table Buff Shop] Rato Tulio"), "Rato Tulio", "c_Klaipe", -800, 530, 0, async dialog =>
			{
				dialog.SetTitle(L("Food Table Buff Shop"));
				await this.ShowBuffSelection(dialog);
			});
		}

		private async Task ShowBuffSelection(Dialog dialog)
		{
			while (true)
			{
				var response = await dialog.Select(
					L(
						"Welcome to the Food Table Buff Shop.{nl}{nl}" +
						"Each food buff costs 50,000 Silver and lasts 30 minutes.{nl}" +
						"All six food buffs can be purchased together for 300,000 Silver.{nl}{nl}" +
						"Choose a food buff."
					),
					Option(L("Salad"), "salad"),
					Option(L("Sandwich"), "sandwich"),
					Option(L("Soup"), "soup"),
					Option(L("Yogurt"), "yogurt"),
					Option(L("BBQ Skewer"), "bbq"),
					Option(L("Champagne"), "champagne"),
					Option(L("Buy All Buffs - 300,000 Silver"), "buy_all"),
					Option(L("Exit"), "exit")
				);

				switch (response)
				{
					case "salad":
						await this.ConfirmPurchase(dialog, BuffId.squire_food1_buff, "Salad");
						break;

					case "sandwich":
						await this.ConfirmPurchase(dialog, BuffId.squire_food2_buff, "Sandwich");
						break;

					case "soup":
						await this.ConfirmPurchase(dialog, BuffId.squire_food3_buff, "Soup");
						break;

					case "yogurt":
						await this.ConfirmPurchase(dialog, BuffId.squire_food4_buff, "Yogurt");
						break;

					case "bbq":
						await this.ConfirmPurchase(dialog, BuffId.squire_food5_buff, "BBQ Skewer");
						break;

					case "champagne":
						await this.ConfirmPurchase(dialog, BuffId.squire_food6_buff, "Champagne");
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
					"Selected food: " + buffName + "{nl}" +
					"Food Table level: " + FoodBuffLevel + "{nl}" +
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
					"Purchase all six Food Table buffs?{nl}{nl}" +
					"Salad{nl}" +
					"Sandwich{nl}" +
					"Soup{nl}" +
					"Yogurt{nl}" +
					"BBQ Skewer{nl}" +
					"Champagne{nl}{nl}" +
					"Food Table level: " + FoodBuffLevel + "{nl}" +
					"Duration: 30 minutes{nl}" +
					"Total price: 300,000 Silver"
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

			character.StartBuff(buffId, FoodBuffLevel, 0f, BuffDuration, character);

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
					"Required: 300,000 Silver{nl}" +
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

			character.StartBuff(BuffId.squire_food1_buff, FoodBuffLevel, 0f, BuffDuration, character);
			character.StartBuff(BuffId.squire_food2_buff, FoodBuffLevel, 0f, BuffDuration, character);
			character.StartBuff(BuffId.squire_food3_buff, FoodBuffLevel, 0f, BuffDuration, character);
			character.StartBuff(BuffId.squire_food4_buff, FoodBuffLevel, 0f, BuffDuration, character);
			character.StartBuff(BuffId.squire_food5_buff, FoodBuffLevel, 0f, BuffDuration, character);
			character.StartBuff(BuffId.squire_food6_buff, FoodBuffLevel, 0f, BuffDuration, character);

			await dialog.Msg(L("All six Food Table buffs have been applied.{nl}Duration: 30 minutes."));
		}
	}
}
