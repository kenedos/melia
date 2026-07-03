//--- Melia Script ----------------------------------------------------------
// Transcendence Master Shop
//--- Description -----------------------------------------------------------
// NPC responsible for transcending equipped items using Blessed Gems.
// This version does not use the official itemtranscend HUD because that HUD
// leaves the client locked without the full official protocol implemented.
//---------------------------------------------------------------------------

using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class TranscendenceMasterShopScript : GeneralScript
{
	protected override void Load()
	{
		//-------------------------------------------------------------------------
		// [Transcendence Master]
		//-------------------------------------------------------------------------
		AddNpc(20105, L("[Transcendence Master] Elder"), "Elder", "c_Klaipe", 440, 139, 270, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Elder"));
			dialog.SetPortrait("KLAPEDA_BLACKSMITH");

			// Main menu.
			var response = await dialog.Select(
				L("I can strengthen your equipment through Transcendence using Blessed Gems."),
				Option(L("Transcend Equipment"), "transcend"),
				Option(L("Cancel"), "cancel")
			);

			if (response != "transcend")
				return;

			// Get all real equipped items.
			var equips = character.Inventory
				.GetEquip()
				.Values
				.Where(item =>
					item != null &&
					item.ObjectId > 0 &&
					item.Data.Type == ItemType.Equip)
				.ToList();

			if (equips.Count == 0)
			{
				await dialog.Msg(L("You don't have any equipment equipped."));
				return;
			}

			// Build the item selection list.
			var options = equips
				.Select((item, index) =>
				{
					var stage = (int)item.Properties.GetFloat(PropertyName.Transcend, 0);

					return Option(
						L($"{item.Data.Name}   [Stage {stage}]"),
						index.ToString()
					);
				})
				.ToList();

			options.Add(Option(L("Cancel"), "cancel"));

			var selected = await dialog.Select(
				L("Select the equipment you want to transcend."),
				options.ToArray()
			);

			if (selected == "cancel")
				return;

			// Validate selected index.
			if (!int.TryParse(selected, out var selectedIndex))
			{
				await dialog.Msg(L("Invalid selection."));
				return;
			}

			if (selectedIndex < 0 || selectedIndex >= equips.Count)
			{
				await dialog.Msg(L("Invalid selection."));
				return;
			}

			var selectedItem = equips[selectedIndex];

			// Calculate next transcendence stage and Blessed Gem cost.
			var currentStage = (int)selectedItem.Properties.GetFloat(PropertyName.Transcend, 0);
			var nextStage = currentStage + 1;
			var requiredGems = TranscendenceHelper.GetRequiredBlessedGems(nextStage);

			if (requiredGems <= 0)
			{
				await dialog.Msg(L("This equipment cannot be transcended any further."));
				return;
			}

			// Confirmation message.
			var confirm = await dialog.Select(
			L(
				$"Transcendence Confirmation\n\n" +
				$"Equipment: {selectedItem.Data.Name}\n" +
				$"Current Stage: {currentStage}\n" +
				$"Next Stage: {nextStage}\n" +
				$"Blessed Gems Required: {requiredGems}\n\n" +
				$"Proceed with Transcendence?"
			),
			Option(L("Proceed"), "yes"),
			Option(L("Cancel"), "no")
			);

			if (confirm != "yes")
				return;

			// Execute transcendence.
			var success = TranscendenceHelper.TryTranscendItem(character, selectedItem, out var resultMessage);

			if (!success)
			{
				await dialog.Msg(L(resultMessage));
				return;
			}

			// Friendly success message.
			var finalStage = (int)selectedItem.Properties.GetFloat(PropertyName.Transcend, 0);

			await dialog.Msg(
			L(
				$"══════════════════════════════\n" +
				$"      Transcendence Complete\n" +
				$"══════════════════════════════\n\n" +
				$"Equipment\n" +
				$"{selectedItem.Data.Name}\n\n" +
				$"Transcendence Stage\n" +
				$"{currentStage} → {finalStage}\n\n" +
				$"Blessed Gems Used\n" +
				$"{requiredGems}\n\n" +
				$"Your equipment has become even stronger!"
			));
		});
	}
}
