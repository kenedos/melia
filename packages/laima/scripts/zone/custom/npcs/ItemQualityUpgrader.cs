//--- Melia Script ----------------------------------------------------------
// Item Quality Upgrader Shop
//--- Description -----------------------------------------------------------
// NPC responsible for upgrading the quality of equipped items.
//---------------------------------------------------------------------------

using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class ItemQualityUpgraderNpc : GeneralScript
{
	protected override void Load()
	{
		AddNpc(57223, L("[Item Quality Upgrader] Helena"), "ItemQualityUpgrader", "c_Klaipe", -255, 315, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Helena"));
			dialog.SetPortrait("KLAPEDA_BLACKSMITH");

			var response = await dialog.Select(
				L("I can improve the quality of your equipped item using identical equipment and special powders."),
				Option(L("Upgrade Item Quality"), "upgrade"),
				Option(L("Cancel"), "cancel")
			);

			if (response != "upgrade")
				return;

			var equips = character.Inventory
				.GetEquip()
				.Where(entry =>
					entry.Value != null &&
					QualityUpgradeHelper.CanBeUpgraded(entry.Value) &&
					QualityUpgradeHelper.IsAllowedEquipSlot(entry.Key))
				.Select(entry => entry.Value)
				.GroupBy(item => item.ObjectId)
				.Select(group => group.First())
				.ToList();

			if (equips.Count == 0)
			{
				await dialog.Msg(L("Equip a Unique or Legend item in a valid equipment slot first."));
				return;
			}

			var options = equips
				.Select((item, index) => Option(
					L($"{item.Data.Name}   [{QualityUpgradeHelper.GetGradeName(QualityUpgradeHelper.GetGrade(item))}]"),
					index.ToString()
				))
				.ToList();

			options.Add(Option(L("Cancel"), "cancel"));

			var selected = await dialog.Select(
				L("Select the equipped item whose quality you want to upgrade."),
				options.ToArray()
			);

			if (selected == "cancel")
				return;

			if (!int.TryParse(selected, out var selectedIndex) || selectedIndex < 0 || selectedIndex >= equips.Count)
			{
				await dialog.Msg(L("Invalid selection."));
				return;
			}

			var selectedItem = equips[selectedIndex];

			if (!QualityUpgradeHelper.TryGetRequirements(selectedItem, out var targetGrade, out var requiredItems, out var sierraCost, out var nucleCost))
			{
				await dialog.Msg(L("This item cannot have its quality upgraded."));
				return;
			}

			var sourceGrade = QualityUpgradeHelper.GetGrade(selectedItem);
			var confirm = await dialog.Select(
				L(
					$"Quality Upgrade Confirmation\n\n" +
					$"Equipped Item: {selectedItem.Data.Name}\n" +
					$"Quality: {QualityUpgradeHelper.GetGradeName(sourceGrade)} → {QualityUpgradeHelper.GetGradeName(targetGrade)}\n" +
					$"Identical Copies Required: {requiredItems - 1}\n" +
					$"Sierra Powder Required: {sierraCost}\n" +
					$"Nucle Powder Required: {nucleCost}\n\n" +
					$"The equipped item will be preserved. Proceed?"
				),
				Option(L("Proceed"), "yes"),
				Option(L("Cancel"), "no")
			);

			if (confirm != "yes")
				return;

			var success = QualityUpgradeHelper.TryUpgradeItem(character, selectedItem, out var resultMessage);
			if (!success)
			{
				await dialog.Msg(L(resultMessage));
				return;
			}

			await dialog.Msg(
				L(
					$"Quality Upgrade Complete\n\n" +
					$"Equipment: {selectedItem.Data.Name}\n" +
					$"Quality: {QualityUpgradeHelper.GetGradeName(sourceGrade)} → {QualityUpgradeHelper.GetGradeName(targetGrade)}\n" +
					$"Identical Copies Used: {requiredItems - 1}\n" +
					$"Sierra Powder Used: {sierraCost}\n" +
					$"Nucle Powder Used: {nucleCost}"
				)
			);
		});
	}
}
