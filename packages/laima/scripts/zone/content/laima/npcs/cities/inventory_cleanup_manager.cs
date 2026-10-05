using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using static Melia.Zone.Scripting.Shortcuts;

public class InventoryCleanupManagerNpcScript : GeneralScript
{
	private const int SkillGemFragmentItemId = 11030259;
	private const int TwoStarGemAbrasiveItemId = 645637;

	private static readonly HashSet<int> ColoredGemIds = new()
	{
		643501, // Red Gem
		643502, // Blue Gem
		643503, // Green Gem
		643504, // Yellow Gem
		643817, // White Gem
	};

	protected override void Load()
	{
		AddNpc(161003, L("[Gem Extractor] Taric"), "Gem Extractor", "c_Klaipe", 440, 180, 270, async dialog =>
		{
			dialog.SetTitle(L("Gem Extractor"));
			await this.ShowMainMenu(dialog);
		});
	}

	private async Task ShowMainMenu(Dialog dialog)
	{
		while (true)
		{
			var character = dialog.Player;

			var skillGemCount = GetSkillGemCount(character);
			var coloredGemCount = GetColoredGemCount(character);
			var skillGemFragments = skillGemCount;
			var gemAbrasives = coloredGemCount / 5;

			var response = await dialog.Select(
				L(string.Format(
					"I can recycle unused gems from your inventory.{{nl}}" +
					"{{nl}}" +
					"Skill Gems: {0}{{nl}}" +
					"→ {1} Skill Gem Fragment(s){{nl}}" +
					"{{nl}}" +
					"Colored Gems: {2}{{nl}}" +
					"→ {3} 2-star Gem Abrasive(s){{nl}}" +
					"{{nl}}" +
					"Exchange rates:{{nl}}" +
					"1 Skill Gem → 1 Skill Gem Fragment{{nl}}" +
					"5 Colored Gems → 1 2-star Gem Abrasive",
					skillGemCount,
					skillGemFragments,
					coloredGemCount,
					gemAbrasives
				)),
				Option(L("Recycle Skill Gems"), "skill"),
				Option(L("Recycle Colored Gems"), "colored"),
				Option(L("Recycle Everything"), "all"),
				Option(L("Cancel"), "cancel")
			);

			switch (response)
			{
				case "skill":
					await this.ConfirmSkillGemCleanup(dialog);
					break;

				case "colored":
					await this.ConfirmColoredGemCleanup(dialog);
					break;

				case "all":
					await this.ConfirmAllCleanup(dialog);
					break;

				default:
					return;
			}
		}
	}

	private async Task ConfirmSkillGemCleanup(Dialog dialog)
	{
		var character = dialog.Player;
		var count = GetSkillGemCount(character);

		if (count <= 0)
		{
			await dialog.Msg(L("You don't have any Skill Gems available for recycling."));
			return;
		}

		var response = await dialog.Select(
			L(string.Format(
				"You currently have {0} Skill Gem(s).{{nl}}" +
				"{{nl}}" +
				"They will be exchanged for:{{nl}}" +
				"{1} Skill Gem Fragment(s).{{nl}}" +
				"{{nl}}" +
				"This action cannot be undone.",
				count,
				count
			)),
			Option(L("Recycle"), "confirm"),
			Option(L("Cancel"), "cancel")
		);

		if (response != "confirm")
			return;

		var converted = RecycleSkillGems(character);

		if (converted <= 0)
		{
			await dialog.Msg(L("No Skill Gems could be recycled."));
			return;
		}

		await dialog.Msg(
			L(string.Format(
				"Recycling complete!{{nl}}" +
				"{{nl}}" +
				"Skill Gems recycled: {0}{{nl}}" +
				"Skill Gem Fragments received: {0}",
				converted
			))
		);
	}

	private async Task ConfirmColoredGemCleanup(Dialog dialog)
	{
		var character = dialog.Player;
		var count = GetColoredGemCount(character);
		var abrasiveCount = count / 5;
		var gemsToConsume = abrasiveCount * 5;
		var remainder = count - gemsToConsume;

		if (abrasiveCount <= 0)
		{
			await dialog.Msg(
				L(string.Format(
					"You need at least 5 Colored Gems to recycle them.{{nl}}" +
					"You currently have {0}.",
					count
				))
			);

			return;
		}

		var response = await dialog.Select(
			L(string.Format(
				"You currently have {0} Colored Gem(s).{{nl}}" +
				"{{nl}}" +
				"Gems consumed: {1}{{nl}}" +
				"2-star Gem Abrasives received: {2}{{nl}}" +
				"Colored Gems remaining: {3}{{nl}}" +
				"{{nl}}" +
				"All colors count toward the same exchange.{{nl}}" +
				"This action cannot be undone.",
				count,
				gemsToConsume,
				abrasiveCount,
				remainder
			)),
			Option(L("Recycle"), "confirm"),
			Option(L("Cancel"), "cancel")
		);

		if (response != "confirm")
			return;

		var converted = RecycleColoredGems(character);

		if (converted <= 0)
		{
			await dialog.Msg(L("No Colored Gems could be recycled."));
			return;
		}

		await dialog.Msg(
			L(string.Format(
				"Recycling complete!{{nl}}" +
				"{{nl}}" +
				"Colored Gems recycled: {0}{{nl}}" +
				"2-star Gem Abrasives received: {1}",
				converted * 5,
				converted
			))
		);
	}

	private async Task ConfirmAllCleanup(Dialog dialog)
	{
		var character = dialog.Player;
		var skillGemCount = GetSkillGemCount(character);
		var coloredGemCount = GetColoredGemCount(character);
		var abrasiveCount = coloredGemCount / 5;
		var coloredGemsToConsume = abrasiveCount * 5;

		if (skillGemCount <= 0 && abrasiveCount <= 0)
		{
			await dialog.Msg(L("You don't have any recyclable gems."));
			return;
		}

		var response = await dialog.Select(
			L(string.Format(
				"The following items will be recycled:{{nl}}" +
				"{{nl}}" +
				"Skill Gems: {0}{{nl}}" +
				"→ {0} Skill Gem Fragment(s){{nl}}" +
				"{{nl}}" +
				"Colored Gems: {1}{{nl}}" +
				"→ {2} 2-star Gem Abrasive(s){{nl}}" +
				"{{nl}}" +
				"This action cannot be undone.",
				skillGemCount,
				coloredGemsToConsume,
				abrasiveCount
			)),
			Option(L("Recycle Everything"), "confirm"),
			Option(L("Cancel"), "cancel")
		);

		if (response != "confirm")
			return;

		var convertedSkillGems = RecycleSkillGems(character);
		var convertedColoredGems = RecycleColoredGems(character);

		await dialog.Msg(
			L(string.Format(
				"Recycling complete!{{nl}}" +
				"{{nl}}" +
				"Skill Gems recycled: {0}{{nl}}" +
				"Skill Gem Fragments received: {0}{{nl}}" +
				"{{nl}}" +
				"Colored Gems recycled: {1}{{nl}}" +
				"2-star Gem Abrasives received: {2}",
				convertedSkillGems,
				convertedColoredGems * 5,
				convertedColoredGems
			))
		);
	}

	private static int GetSkillGemCount(Character character)
	{
		return character.Inventory
			.GetItems()
			.Values
			.Where(IsSkillGem)
			.Sum(item => item.Amount);
	}

	private static int GetColoredGemCount(Character character)
	{
		return character.Inventory
			.GetItems()
			.Values
			.Where(IsColoredGem)
			.Sum(item => item.Amount);
	}

	private static int RecycleSkillGems(Character character)
	{
		var items = character.Inventory
			.GetItems()
			.Values
			.Where(IsSkillGem)
			.ToList();

		var converted = 0;

		foreach (var item in items)
		{
			var amount = item.Amount;

			if (amount <= 0)
				continue;

			var result = character.Inventory.Remove(
				item,
				amount,
				InventoryItemRemoveMsg.Used
			);

			if (result != InventoryResult.Success)
				continue;

			converted += amount;
		}

		if (converted > 0)
		{
			character.Inventory.Add(
				SkillGemFragmentItemId,
				converted,
				InventoryAddType.PickUp,
				"Gem Recycler - Skill Gems"
			);
		}

		return converted;
	}

	private static int RecycleColoredGems(Character character)
	{
		var total = GetColoredGemCount(character);
		var abrasiveCount = total / 5;

		if (abrasiveCount <= 0)
			return 0;

		var amountToRemove = abrasiveCount * 5;

		var items = character.Inventory
			.GetItems()
			.Values
			.Where(IsColoredGem)
			.ToList();

		var remaining = amountToRemove;
		var removed = 0;

		foreach (var item in items)
		{
			if (remaining <= 0)
				break;

			var amount = Math.Min(item.Amount, remaining);

			if (amount <= 0)
				continue;

			var result = character.Inventory.Remove(
				item,
				amount,
				InventoryItemRemoveMsg.Used
			);

			if (result != InventoryResult.Success)
				continue;

			removed += amount;
			remaining -= amount;
		}

		var actualAbrasiveCount = removed / 5;

		if (actualAbrasiveCount > 0)
		{
			character.Inventory.Add(
				TwoStarGemAbrasiveItemId,
				actualAbrasiveCount,
				InventoryAddType.PickUp,
				"Gem Recycler - Colored Gems"
			);
		}

		return actualAbrasiveCount;
	}

	private static bool IsSkillGem(Item item)
	{
		return item != null &&
			item.Data != null &&
			item.Data.Group == ItemGroup.Gem &&
			item.Data.EquipExpGroup == EquipExpGroup.Gem_Skill;
	}

	private static bool IsColoredGem(Item item)
	{
		return item != null &&
			ColoredGemIds.Contains(item.Id);
	}
}
