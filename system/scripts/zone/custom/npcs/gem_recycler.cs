//--- Melia Script ----------------------------------------------------------
// Gem Recycler
//--- Description -----------------------------------------------------------
// Trades unlocked skill gems for Skill Gem Fragments and level 1 colored
// gems for 2-Star Gem Abrasives.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using static Melia.Zone.Scripting.Shortcuts;

public class CustomNpcGemRecycler : GeneralScript
{
	private const int ColoredGemsPerAbrasive = 5;

	private static readonly HashSet<int> ColoredGemIds = new()
	{
		ItemId.Gem_Circle_1,
		ItemId.Gem_Square_1,
		ItemId.Gem_Diamond_1,
		ItemId.Gem_Star_1,
		ItemId.Gem_White_1,
	};

	protected override void Load()
	{
		if (!Feature.IsEnabled("CustomNpcs"))
			return;

		AddNpc(161003, L("[Gem Recycler] Taric"), "c_Klaipe", 440, 180, 270, this.TaricDialog);
	}

	private async Task TaricDialog(Dialog dialog)
	{
		var character = dialog.Player;
		dialog.SetTitle(L("Gem Recycler"));

		while (true)
		{
			var skillGems = CountItems(character, IsSkillGem);
			var coloredGems = CountItems(character, IsColoredGem);

			var selection = await dialog.Select(LF("I can recycle the gems you don't need. Locked items are left alone.{nl}{nl}Skill Gems: {0} (1 Skill Gem Fragment each){nl}Level 1 colored gems: {1} ({2} for one 2-Star Gem Abrasive)", skillGems, coloredGems, ColoredGemsPerAbrasive),
				Option(L("Recycle skill gems"), "skill"),
				Option(L("Recycle colored gems"), "colored"),
				Option(L("Leave"), "exit"));

			if (selection == "exit")
				return;

			if (selection == "skill")
			{
				if (skillGems == 0)
				{
					await dialog.Msg(L("You have no skill gems I can recycle."));
					continue;
				}

				if (!await Confirm(dialog, LF("Trade all {0} skill gems for {0} Skill Gem Fragments? This can't be undone.", skillGems)))
					continue;

				var removed = RemoveItems(character, IsSkillGem, int.MaxValue);
				character.AddItem(ItemId.Piece_Sklgem_Selectbox, removed);
				await dialog.Msg(LF("Done. You received {0} Skill Gem Fragments.", removed));
			}
			else
			{
				var abrasives = coloredGems / ColoredGemsPerAbrasive;
				if (abrasives == 0)
				{
					await dialog.Msg(LF("I need at least {0} level 1 colored gems.", ColoredGemsPerAbrasive));
					continue;
				}

				if (!await Confirm(dialog, LF("Trade {0} colored gems for {1} 2-Star Gem Abrasives? This can't be undone.", abrasives * ColoredGemsPerAbrasive, abrasives)))
					continue;

				var removed = RemoveItems(character, IsColoredGem, abrasives * ColoredGemsPerAbrasive);
				character.AddItem(ItemId.Misc_GemExpStone_RandomQuest1, removed / ColoredGemsPerAbrasive);
				await dialog.Msg(LF("Done. You received {0} 2-Star Gem Abrasives.", removed / ColoredGemsPerAbrasive));
			}
		}
	}

	private static async Task<bool> Confirm(Dialog dialog, string text)
	{
		return await dialog.Select(text, Option(L("Recycle"), "yes"), Option(L("Cancel"), "no")) == "yes";
	}

	private static bool IsSkillGem(Item item)
		=> !item.IsLocked && item.Data.EquipExpGroup == EquipExpGroup.Gem_Skill;

	private static bool IsColoredGem(Item item)
		=> !item.IsLocked && ColoredGemIds.Contains(item.Id);

	private static int CountItems(Character character, Func<Item, bool> predicate)
		=> character.Inventory.GetItems(predicate).Values.Sum(a => a.Amount);

	private static int RemoveItems(Character character, Func<Item, bool> predicate, int max)
	{
		var removed = 0;

		foreach (var item in character.Inventory.GetItems(predicate).Values.ToList())
		{
			if (removed >= max)
				break;

			var amount = Math.Min(item.Amount, max - removed);
			if (character.Inventory.Remove(item, amount, InventoryItemRemoveMsg.Given) == InventoryResult.Success)
				removed += amount;
		}

		return removed;
	}
}
