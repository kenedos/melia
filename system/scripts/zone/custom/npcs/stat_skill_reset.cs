using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Scripting.Shortcuts;

public class CustomNpcStatSkillReset : GeneralScript
{
	private const int VisItemId = ItemId.Vis;

	private const int BasePrice = 100000;
	private const int MaxPrice = 1600000;

	private const int SkillPointCoinItemId = ItemId.Misc_0533;
	private const int SkillPointPrice = 100;
	private const int MaxSkillPointPurchases = 5;

	protected override void Load()
	{
		if (!Melia.Zone.Feature.IsEnabled("CustomNpcs"))
			return;

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
		var skillPointPurchases = player.Variables.Perm.GetInt("SkillPointPurchaseCount");
		var statPrice = CalculatePrice(statResetCount);
		var skillPrice = CalculatePrice(skillResetCount);
		var abilityPrice = CalculatePrice(abilityResetCount);

		var statPriceStr = FormatSilver(statPrice);
		var skillPriceStr = FormatSilver(skillPrice);

		var greeting = L("Greetings, adventurer. I am Seraphina, an angel of the goddess Laima. I can help you redistribute your stat points or reset your skill points, for a fee that increases with each use.");

		var selection = await dialog.Select(greeting,
			Option(LF("Stat Reset ({0} Silver)", statPriceStr), "stat_reset"),
			Option(LF("Skill Reset ({0} Silver)", skillPriceStr), "skill_reset"),
			Option(LF("Ability Reset ({0} Silver)", FormatSilver(abilityPrice)), "ability_reset"),
			Option(LF("Skill Point ({0} Wings of Vaivora Coins, {1}/{2})", SkillPointPrice, skillPointPurchases, MaxSkillPointPurchases), "skill_point"),
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
			case "skill_point":
				await this.HandleSkillPointPurchase(dialog, player, skillPointPurchases);
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

	private async Task HandleAbilityReset(Dialog dialog, Character player, int price, int resetCount)
	{
		var confirmMsg = LF("An ability reset will cost {0} Silver.\nYou have used this service {1} time(s) before.", FormatSilver(price), resetCount);

		if (!player.HasSilver(price))
		{
			await dialog.Msg(LF("{0}\n\nYou don't have enough Silver. You need {1} Silver.", confirmMsg, FormatSilver(price)));
			return;
		}

		var confirm = await dialog.Select(confirmMsg,
			Option(L("Yes, reset my abilities"), "confirm"),
			Option(L("No, nevermind"), "cancel")
		);

		if (confirm != "confirm")
		{
			await dialog.Msg(L("Perhaps another time, then."));
			return;
		}

		player.RemoveItem(VisItemId, price);
		player.ResetAbilities();

		Send.ZC_ABILITY_LIST(player);
		Send.ZC_OBJECT_PROPERTY(player);

		player.Variables.Perm.Set("AbilityResetCount", resetCount + 1);

		await dialog.Msg(LF("Your abilities have been reset successfully.\n\nYour next ability reset will cost {0} Silver.", FormatSilver(CalculatePrice(resetCount + 1))));
	}

	private async Task HandleSkillPointPurchase(Dialog dialog, Character player, int purchaseCount)
	{
		if (purchaseCount >= MaxSkillPointPurchases)
		{
			await dialog.Msg(LF("You have already received all {0} skill points I can grant.", MaxSkillPointPurchases));
			return;
		}

		var confirm = await dialog.Select(LF("For {0} Wings of Vaivora Coins I can grant 1 skill point to each of your jobs.\nYou have used this service {1}/{2} times.", SkillPointPrice, purchaseCount, MaxSkillPointPurchases),
			Option(L("Yes, grant my skill points"), "confirm"),
			Option(L("No, nevermind"), "cancel")
		);

		if (confirm != "confirm")
		{
			await dialog.Msg(L("Perhaps another time, then."));
			return;
		}

		if (player.Inventory.CountItem(SkillPointCoinItemId) < SkillPointPrice)
		{
			await dialog.Msg(LF("You need {0} Wings of Vaivora Coins.", SkillPointPrice));
			return;
		}

		player.RemoveItem(SkillPointCoinItemId, SkillPointPrice);

		foreach (var job in player.Jobs.GetList())
			player.Jobs.ModifySkillPoints(job.Id, 1);

		player.Variables.Perm.Set("SkillPointPurchaseCount", purchaseCount + 1);
		player.Variables.Perm.SetInt(Character.BonusSkillPointsVarName, player.GetBonusSkillPoints() + 1);

		await dialog.Msg(L("The goddess has heard you. Your skill points have been granted."));
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
