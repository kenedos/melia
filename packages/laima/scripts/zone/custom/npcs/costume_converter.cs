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
		AddNpc(161003, "[Fashion Virtuoso] Fabrizio", "c_barber_dress", -51, -11, 90, NpcDialog);
	}

	private async Task NpcDialog(Dialog dialog)
	{
		var pc = dialog.Player;

		dialog.SetTitle("Fabrizio");

		await dialog.Msg("Darling! DARLING! What a MAGNIFICENT creature you are! I am Fabrizio, the one and only Fashion Virtuoso of the realm!");

		var selection = await dialog.Select("Tell me, what brings you to my fabulous presence today?",
			Option("I need a costume converted", "convert"),
			Option("Who are you exactly?", "about"),
			Option("Nothing, just passing by", "leave")
		);

		switch (selection)
		{
			case "convert":
				await HandleConversion(dialog, pc);
				break;

			case "about":
				await dialog.Msg("Oh, you DON'T know?! I am simply DEVASTATED! But also thrilled to enlighten you!");
				await dialog.Msg("I am Fabrizio! Master of fabric, sultan of stitches, the EMPEROR of elegant ensembles!");
				await dialog.Msg("I possess the extraordinary gift of transforming costumes between their masculine and feminine forms. A little nip here, a tuck there, some ABSOLUTELY divine magic...");
				await dialog.Msg("And voila! Your dashing knight's armor becomes a stunning warrior princess gown! Or vice versa, naturally. I don't discriminate - fashion is UNIVERSAL!");
				break;

			case "leave":
				await dialog.Msg("Leaving so soon?! But we've barely scratched the surface of your fashion POTENTIAL! Do come back when you're ready to be TRANSFORMED, darling!");
				break;
		}
	}

	private async Task HandleConversion(Dialog dialog, Melia.Zone.World.Actors.Characters.Character pc)
	{
		// Find costumes that have (Male) or (Female) in their name
		var convertibleCostumes = pc.Inventory.GetItems(item =>
			item.Data.EquipSlot == "OUTER" &&
			(item.Data.Name.Contains("(Male)") || item.Data.Name.Contains("(Female)"))
		).Values.ToList();

		if (!convertibleCostumes.Any())
		{
			await dialog.Msg("Oh no, no, NO! I've searched through your belongings and found NOTHING I can work with!");
			await dialog.Msg("Bring me costumes with proper gendered designs, darling. The ones marked as Male or Female variants. THEN we can create magic together!");
			return;
		}

		await dialog.Msg("Ooh, let me take a PEEK at what treasures you've brought me today...");
		await dialog.Msg("Yes, YES! I see some pieces with POTENTIAL! Which one shall we transform?");

		// Paginated costume selection
		var item = await SelectCostumeWithPagination(dialog, convertibleCostumes);

		if (item == null)
		{
			await dialog.Msg("Changed your mind? Perfectly understandable, darling. Fashion decisions should NEVER be rushed! Come back when inspiration strikes!");
			return;
		}

		// Determine the target name
		var oldName = item.Data.Name;
		string targetName;
		string fromGender;
		string toGender;

		if (oldName.Contains("(Male)"))
		{
			targetName = oldName.Replace("(Male)", "(Female)");
			fromGender = "masculine";
			toGender = "feminine";
		}
		else
		{
			targetName = oldName.Replace("(Female)", "(Male)");
			fromGender = "feminine";
			toGender = "masculine";
		}

		// Find the converted item by name
		var newItemData = ZoneServer.Instance.Data.ItemDb.Entries.Values
			.FirstOrDefault(i => i.Name == targetName && i.EquipSlot == "OUTER");

		if (newItemData == null)
		{
			await dialog.Msg($"I examined your {oldName} from EVERY angle...");
			await dialog.Msg("But alas! This particular piece has no counterpart in the opposite style. Some designs are simply TOO unique to be reimagined!");
			await dialog.Msg("It's not a failure, darling - it's EXCLUSIVITY! Wear it with PRIDE!");
			return;
		}

		// Confirm the conversion
		await dialog.Msg($"Aha! Your gorgeous '{oldName}'!");
		await dialog.Msg($"I can transform this {fromGender} masterpiece into its {toGender} counterpart: '{newItemData.Name}'!");

		var confirm = await dialog.Select("Shall I work my magic? This transformation is IRREVERSIBLE... well, unless you come back to me again!",
			Option("Yes, transform it!", "yes"),
			Option("No, I changed my mind", "no")
		);

		if (confirm != "yes")
		{
			await dialog.Msg("No?! But... but it would have been SPECTACULAR! *sigh* Very well. The artist in me weeps, but I respect your decision. Come back when you're ready for GREATNESS!");
			return;
		}

		// Perform the conversion
		pc.Inventory.Remove(item, 1, InventoryItemRemoveMsg.Given);
		pc.Inventory.Add(newItemData.Id, 1, InventoryAddType.PickUp);

		await dialog.Msg("Stand back, darling! Watch the MASTER at work!");
		await dialog.Msg("*snip snip* *dramatic flourish* *a sprinkle of fairy dust*");
		await dialog.Msg("...AND WE'RE DONE!");
		await dialog.Msg($"BEHOLD! Your '{oldName}' has been REBORN as '{newItemData.Name}'!");
		await dialog.Msg("Is it not MAGNIFICENT?! Is it not DIVINE?! Go forth and DAZZLE the world, you beautiful creature!");
	}

	private async Task<Item> SelectCostumeWithPagination(Dialog dialog, List<Item> costumes)
	{
		const int ItemsPerPage = 7; // 7 items + prev + next + cancel = 10 max options
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
				options.Add(Option("< Previous page", "prev"));

			if (currentPage < totalPages - 1)
				options.Add(Option("Next page >", "next"));

			options.Add(Option("Actually, never mind", "cancel"));

			var pageInfo = totalPages > 1 ? $" (Page {currentPage + 1}/{totalPages})" : "";
			var selection = await dialog.Select($"Select a costume to convert:{pageInfo}", options.ToArray());

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
