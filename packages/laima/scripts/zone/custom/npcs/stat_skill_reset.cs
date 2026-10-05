using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using static Melia.Zone.Scripting.Shortcuts;

public class CustomNpcStatSkillReset : GeneralScript
{
	private const int VisItemId = ItemId.Vis;

	private const int BasePrice = 100000;
	private const int MaxPrice = 1600000;
	private const int WingsOfVaivoraCoinItemId = 647016;
	private const int SkillPointPrice = 100;
	private const int MaxSkillPointPurchases = 5;
	private const string SkillPointPurchaseCountVar = "SkillPointPurchaseCount";

	protected override void Load()
	{
		AddNpc(57582, L("[Angel] Seraphina"), "c_Klaipe", 295, 83, 90, this.SerafinaDialog);
		AddNpc(57582, L("[Angel] Seraphina"), "c_fedimian", -410, -262, 45, this.SerafinaDialog);
		AddNpc(57582, L("[Angel] Seraphina"), "c_orsha", -65, 232, 90, this.SerafinaDialog);
	}

	private async Task SerafinaDialog(Dialog dialog)
	{
		var player = dialog.Player;

		dialog.SetTitle(L("Seraphina"));

		var statResetCount = player.Variables.Perm.GetInt("StatResetCount");
		var skillResetCount = player.Variables.Perm.GetInt("SkillResetCount");
		var abilityResetCount = player.Variables.Perm.GetInt("AbilityResetCount");
		var skillPointPurchaseCount = player.Variables.Perm.GetInt(SkillPointPurchaseCountVar);
		var statPrice = CalculatePrice(statResetCount);
		var skillPrice = CalculatePrice(skillResetCount);
		var abilityPrice = CalculatePrice(abilityResetCount);

		var statPriceStr = FormatSilver(statPrice);
		var skillPriceStr = FormatSilver(skillPrice);
		var abilityPriceStr = FormatSilver(abilityPrice);


		var greeting = L("Greetings, adventurer. I am Seraphina, an angel of the goddess Laima. I can help you redistribute your stat points or reset your skill points, for a fee that increases with each use.");

		var selection = await dialog.Select(greeting,
			Option(LF("Stat Reset ({0} Silver)", statPriceStr), "stat_reset"),
			Option(LF("Skill Reset ({0} Silver)", skillPriceStr), "skill_reset"),
			Option(LF("Ability Reset ({0} Silver)", abilityPriceStr), "ability_reset"),
			Option(LF("Buy Skill Point ({0} Wings of Vaivora Coins) - {1}/{2}", SkillPointPrice, skillPointPurchaseCount, MaxSkillPointPurchases), "buy_skill_point"),
			Option(L("No thanks."), "cancel")
		);

		switch (selection)
		{
			case "stat_reset":
				await this.HandleReset(dialog, player, true, statPrice, statResetCount);
				break;
			case "skill_reset":
				await this.HandleReset(dialog, player, false, skillPrice, skillResetCount);
				break;
			case "ability_reset":
				await this.HandleAbilityReset(dialog, player, abilityPrice, abilityResetCount);
				break;
			case "buy_skill_point":
				await this.HandleSkillPointPurchase(dialog, player);
				break;
			case "cancel":
				await dialog.Msg(L("May the blessings of the goddess Laima be with you."));
				break;
		}
	}
	private async Task HandleReset(Dialog dialog, Character player, bool isStatReset, int price, int resetCount)
	{
		var resetTypeName = isStatReset ? L("stat") : L("skill");
		var resetCountVariable = isStatReset ? "StatResetCount" : "SkillResetCount";

		var confirmMsg = LF("A {0} reset will cost {1} Silver.\nYou have used this service {2} time(s) before.", resetTypeName, FormatSilver(price), resetCount);

		if (price >= MaxPrice)
			confirmMsg += "\n" + L("You have reached the maximum price. Further resets will not increase in cost.");

		if (!player.HasSilver(price))
		{
			await dialog.Msg(LF("{0}\n\nYou don't have enough Silver. You need {1} Silver.", confirmMsg, FormatSilver(price)));
			return;
		}

		var confirm = await dialog.Select(confirmMsg,
			Option(LF("Yes, reset my {0}s", resetTypeName), "confirm"),
			Option(L("No, nevermind"), "cancel")
		);

		if (confirm == "confirm")
		{
			player.RemoveItem(VisItemId, price);

			if (isStatReset)
			{
				player.ResetStats();
			}
			else
			{
				player.ResetSkills();
				player.AddonMessage(AddonMessage.RESET_SKL_UP);
			}

			player.Variables.Perm.Set(resetCountVariable, resetCount + 1);

			var nextPrice = CalculatePrice(resetCount + 1);
			await dialog.Msg(LF("Your {0}s have been reset successfully.\n\nYour next {0} reset will cost {1} Silver.", resetTypeName, FormatSilver(nextPrice)));
		}
		else
		{
			await dialog.Msg(L("Perhaps another time, then."));
		}
	}

	private async Task HandleSkillPointPurchase(Dialog dialog, Character player)
	{
		var purchaseCount = player.Variables.Perm.GetInt(SkillPointPurchaseCountVar);

		if (purchaseCount >= MaxSkillPointPurchases)
		{
			await dialog.Msg(LF("You have already reached the Skill Point purchase limit ({0}/{1}).", MaxSkillPointPurchases, MaxSkillPointPurchases));
			return;
		}

		var jobs = player.Jobs.GetList().ToArray();

		if (jobs.Length == 0)
		{
			await dialog.Msg(L("You don't have any jobs that can receive a Skill Point."));
			return;
		}

		var coinBalance = player.Inventory.CountItem(WingsOfVaivoraCoinItemId);

		if (coinBalance < SkillPointPrice)
		{
			await dialog.Msg(LF("You need {0} Wings of Vaivora Coins, but you only have {1}.", SkillPointPrice, coinBalance));
			return;
		}

		var confirm = await dialog.Select(LF("I can grant 1 Skill Point to each of your unlocked jobs for {0} Wings of Vaivora Coins.\n\nYou have used this service {1}/{2} times.", SkillPointPrice, purchaseCount, MaxSkillPointPurchases),
			Option(L("Yes, grant my Skill Point"), "confirm"),
			Option(L("No, nevermind"), "cancel"));

		if (confirm != "confirm")
		{
			await dialog.Msg(L("Perhaps another time, then."));
			return;
		}

		purchaseCount = player.Variables.Perm.GetInt(SkillPointPurchaseCountVar);

		if (purchaseCount >= MaxSkillPointPurchases)
		{
			await dialog.Msg(LF("You have already reached the Skill Point purchase limit ({0}/{1}).", MaxSkillPointPurchases, MaxSkillPointPurchases));
			return;
		}

		if (player.Inventory.CountItem(WingsOfVaivoraCoinItemId) < SkillPointPrice)
		{
			await dialog.Msg(LF("You no longer have the required {0} Wings of Vaivora Coins.", SkillPointPrice));
			return;
		}

		if (!TryConsumeVaivoraCoins(player, SkillPointPrice))
		{
			await dialog.Msg(L("The Wings of Vaivora Coins could not be consumed."));
			return;
		}

		foreach (var job in jobs)
			player.Jobs.ModifySkillPoints(job.Id, 1);

		var newPurchaseCount = purchaseCount + 1;
		player.Variables.Perm.Set(SkillPointPurchaseCountVar, newPurchaseCount);

		player.AddonMessage("INV_ITEM_LIST_GET");
	}

	private static bool TryConsumeVaivoraCoins(Character player, int amount)
	{
		if (amount <= 0 || player.Inventory.CountItem(WingsOfVaivoraCoinItemId) < amount)
			return false;

		var remainingAmount = amount;
		var currencyStacks = player.Inventory.GetItems().Values.Where(item => item != null && item.Id == WingsOfVaivoraCoinItemId && item.Amount > 0).ToArray();

		foreach (var currencyStack in currencyStacks)
		{
			if (remainingAmount <= 0)
				break;

			var amountToRemove = Math.Min(currencyStack.Amount, remainingAmount);
			var removeResult = player.Inventory.Remove(currencyStack.ObjectId, amountToRemove, InventoryItemRemoveMsg.Used);

			if (removeResult != InventoryResult.Success)
				return false;

			remainingAmount -= amountToRemove;
		}

		return remainingAmount == 0;
	}

	private async Task HandleAbilityReset(Dialog dialog, Character player, int price, int resetCount)
	{
		var confirmMsg =
			LF(
				"An Ability reset will cost {0} Silver.\nYou have used this service {1} time(s) before.",
				FormatSilver(price),
				resetCount);

		if (price >= MaxPrice)
			confirmMsg += "\n" + L("You have reached the maximum price. Further resets will not increase in cost.");

		if (!player.HasSilver(price))
		{
			await dialog.Msg(
				LF(
					"{0}\n\nYou don't have enough Silver. You need {1} Silver.",
					confirmMsg,
					FormatSilver(price)));

			return;
		}

		var confirm = await dialog.Select(
			confirmMsg,
			Option(L("Yes, reset my abilities"), "confirm"),
			Option(L("No, nevermind"), "cancel"));

		if (confirm != "confirm")
		{
			await dialog.Msg(L("Perhaps another time, then."));
			return;
		}

		player.RemoveItem(VisItemId, price);

		player.ResetAbilities();

		Send.ZC_ABILITY_LIST(player);
		Send.ZC_OBJECT_PROPERTY(player);

		player.Variables.Perm.Set(
			"AbilityResetCount",
			resetCount + 1);

		var nextPrice = CalculatePrice(resetCount + 1);

		await dialog.Msg(
			LF(
				"Your abilities have been reset successfully.\n\nYour next Ability reset will cost {0} Silver.",
				FormatSilver(nextPrice)));
	}

	private static int CalculatePrice(int resetCount)
	{
		var price = (long)(BasePrice * Math.Pow(2, resetCount));

		if (price > MaxPrice)
			price = MaxPrice;

		return (int)price;
	}

	private static string FormatSilver(int amount)
	{
		return amount.ToString("N0");
	}
}
