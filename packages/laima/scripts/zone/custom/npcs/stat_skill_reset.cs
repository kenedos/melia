using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Scripting.Shortcuts;

public class CustomNpcStatSkillReset : GeneralScript
{
	private const int VisItemId = ItemId.Vis;

	private const int BasePrice = 100000;
	private const int MaxPrice = 1600000;

	protected override void Load()
	{
		AddNpc(57582, L("[Angel] Seraphina"), "c_Klaipe", 295, 83, 90, this.SerafinaDialog);
		AddNpc(57582, L("[Angel] Seraphina"), "c_fedimian", -410, -262, 45, this.SerafinaDialog);
		AddNpc(57582, L("[Angel] Seraphina"), "c_orsha", -65, 232, 90, this.SerafinaDialog);
	}

	private async Task SerafinaDialog(Dialog dialog)
	{
		var player = dialog.Player;

		dialog.SetTitle("Seraphina");

		var statResetCount = player.Variables.Perm.GetInt("StatResetCount");
		var skillResetCount = player.Variables.Perm.GetInt("SkillResetCount");
		var statPrice = CalculatePrice(statResetCount);
		var skillPrice = CalculatePrice(skillResetCount);

		var statPriceStr = FormatSilver(statPrice);
		var skillPriceStr = FormatSilver(skillPrice);

		var greeting = "Greetings, adventurer. I am Seraphina, an angel of the goddess Laima. "
			+ "I can help you redistribute your stat points or reset your skill points, "
			+ "for a fee that increases with each use.";

		var selection = await dialog.Select(greeting,
			Option(LF("Stat Reset ({0} Silver)", statPriceStr), "stat_reset"),
			Option(LF("Skill Reset ({0} Silver)", skillPriceStr), "skill_reset"),
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
			case "cancel":
				await dialog.Msg("May the blessings of the goddess Laima be with you.");
				break;
		}
	}
	private async Task HandleReset(Dialog dialog, Character player, bool isStatReset, int price, int resetCount)
	{
		var resetTypeName = isStatReset ? "stat" : "skill";
		var resetCountVariable = isStatReset ? "StatResetCount" : "SkillResetCount";

		var confirmMsg = LF("A {0} reset will cost {1} Silver.\nYou have used this service {2} time(s) before.", resetTypeName, FormatSilver(price), resetCount);

		if (price >= MaxPrice)
			confirmMsg += "\nYou have reached the maximum price. Further resets will not increase in cost.";

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
			await dialog.Msg("Perhaps another time, then.");
		}
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
