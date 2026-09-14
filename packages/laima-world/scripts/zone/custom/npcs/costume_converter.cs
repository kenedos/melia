//--- Melia Script ----------------------------------------------------------
// Costume Converter NPC
//--- Description -----------------------------------------------------------
// An eccentric fashion designer who can convert costumes between male and
// female versions.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Items;
using static Melia.Zone.Scripting.Shortcuts;

public class CostumeConverterNpc : GeneralScript
{
	protected override void Load()
	{
		AddNpc(161003, L("[Fashion Virtuoso] Fabrizio"), "c_barber_dress", -51, -11, 90, NpcDialog);
	}

	private async Task NpcDialog(Dialog dialog)
	{
		var pc = dialog.Player;

		dialog.SetTitle(L("Fabrizio"));

		await dialog.Msg(L("Darling! DARLING! What a MAGNIFICENT creature you are! I am Fabrizio, the one and only Fashion Virtuoso of the realm!"));

		var selection = await dialog.Select(L("Tell me, what brings you to my fabulous presence today?"),
			Option(L("I need a costume converted"), "convert"),
			Option(L("Who are you exactly?"), "about"),
			Option(L("Nothing, just passing by"), "leave")
		);

		switch (selection)
		{
			case "convert":
				await HandleConversion(dialog, pc);
				break;

			case "about":
				await dialog.Msg(L("Oh, you DON'T know?! I am simply DEVASTATED! But also thrilled to enlighten you!"));
				await dialog.Msg(L("I am Fabrizio! Master of fabric, sultan of stitches, the EMPEROR of elegant ensembles!"));
				await dialog.Msg(L("I possess the extraordinary gift of transforming costumes between their masculine and feminine forms. A little nip here, a tuck there, some ABSOLUTELY divine magic..."));
				await dialog.Msg(L("And voila! Your dashing knight's armor becomes a stunning warrior princess gown! Or vice versa, naturally. I don't discriminate - fashion is UNIVERSAL!"));
				break;

			case "leave":
				await dialog.Msg(L("Leaving so soon?! But we've barely scratched the surface of your fashion POTENTIAL! Do come back when you're ready to be TRANSFORMED, darling!"));
				break;
		}
	}

	private async Task HandleConversion(Dialog dialog, Melia.Zone.World.Actors.Characters.Character pc)
	{
		var convertibleCostumes = pc.Inventory.GetItems(item =>
			item.Data.EquipSlot == "OUTER" &&
			(item.Data.Name.Contains("(Male)") || item.Data.Name.Contains("(Female)"))
		).Values.ToList();

		if (!convertibleCostumes.Any())
		{
			await dialog.Msg(L("Oh no, no, NO! I've searched through your belongings and found NOTHING I can work with!"));
			await dialog.Msg(L("Bring me costumes with proper gendered designs, darling. The ones marked as Male or Female variants. THEN we can create magic together!"));
			return;
		}

		await dialog.Msg(L("Ooh, let me take a PEEK at what treasures you've brought me today..."));
		await dialog.Msg(L("Yes, YES! I see some pieces with POTENTIAL! Which one shall we transform?"));

		var item = await SelectCostumeWithPagination(dialog, convertibleCostumes);

		if (item == null)
		{
			await dialog.Msg(L("Changed your mind? Perfectly understandable, darling. Fashion decisions should NEVER be rushed! Come back when inspiration strikes!"));
			return;
		}

		var oldName = item.Data.Name;
		string targetName;
		string fromGender;
		string toGender;

		if (oldName.Contains("(Male)"))
		{
			targetName = oldName.Replace("(Male)", "(Female)");
			fromGender = L("masculine");
			toGender = L("feminine");
		}
		else
		{
			targetName = oldName.Replace("(Female)", "(Male)");
			fromGender = L("feminine");
			toGender = L("masculine");
		}

		var newItemData = ZoneServer.Instance.Data.ItemDb.Entries.Values
			.FirstOrDefault(i => i.Name == targetName && i.EquipSlot == "OUTER");

		if (newItemData == null)
		{
			await dialog.Msg(LF("I examined your {0} from EVERY angle...", oldName));
			await dialog.Msg(L("But alas! This particular piece has no counterpart in the opposite style. Some designs are simply TOO unique to be reimagined!"));
			await dialog.Msg(L("It's not a failure, darling - it's EXCLUSIVITY! Wear it with PRIDE!"));
			return;
		}

		await dialog.Msg(LF("Aha! Your gorgeous '{0}'!", oldName));
		await dialog.Msg(LF("I can transform this {0} masterpiece into its {1} counterpart: '{2}'!", fromGender, toGender, newItemData.Name));

		var confirm = await dialog.Select(L("Shall I work my magic? This transformation is IRREVERSIBLE... well, unless you come back to me again!"),
			Option(L("Yes, transform it!"), "yes"),
			Option(L("No, I changed my mind"), "no")
		);

		if (confirm != "yes")
		{
			await dialog.Msg(L("No?! But... but it would have been SPECTACULAR! *sigh* Very well. The artist in me weeps, but I respect your decision. Come back when you're ready for GREATNESS!"));
			return;
		}

		pc.Inventory.Remove(item, 1, InventoryItemRemoveMsg.Given);
		pc.Inventory.Add(newItemData.Id, 1, InventoryAddType.PickUp);

		await dialog.Msg(L("Stand back, darling! Watch the MASTER at work!"));
		await dialog.Msg(L("*snip snip* *dramatic flourish* *a sprinkle of fairy dust*"));
		await dialog.Msg(L("...AND WE'RE DONE!"));
		await dialog.Msg(LF("BEHOLD! Your '{0}' has been REBORN as '{1}'!", oldName, newItemData.Name));
		await dialog.Msg(L("Is it not MAGNIFICENT?! Is it not DIVINE?! Go forth and DAZZLE the world, you beautiful creature!"));
	}

	private async Task<Item> SelectCostumeWithPagination(Dialog dialog, List<Item> costumes)
	{
		const int ItemsPerPage = 7;
		var currentPage = 0;
		var totalPages = (costumes.Count + ItemsPerPage - 1) / ItemsPerPage;

		while (true)
		{
			var pageItems = costumes.Skip(currentPage * ItemsPerPage).Take(ItemsPerPage).ToList();
			var options = new List<DialogOption>();

			foreach (var costume in pageItems)
			{
				options.Add(Option(costume.Data.Name, costume.ObjectId.ToString()));
			}

			if (currentPage > 0)
				options.Add(Option(L("< Previous page"), "prev"));

			if (currentPage < totalPages - 1)
				options.Add(Option(L("Next page >"), "next"));

			options.Add(Option(L("Actually, never mind"), "cancel"));

			var pageInfo = totalPages > 1 ? LF(" (Page {0}/{1})", currentPage + 1, totalPages) : "";
			var selection = await dialog.Select(L("Select a costume to convert:") + pageInfo, options.ToArray());

			if (selection == "cancel")
				return null;

			if (selection == "next")
			{
				currentPage++;
				continue;
			}

			if (selection == "prev")
			{
				currentPage--;
				continue;
			}

			if (long.TryParse(selection, out var objectId))
			{
				var selectedItem = costumes.FirstOrDefault(c => c.ObjectId == objectId);
				if (selectedItem != null)
					return selectedItem;
			}

			return null;
		}
	}
}
