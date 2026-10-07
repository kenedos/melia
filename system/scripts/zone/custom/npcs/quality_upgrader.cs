//--- Melia Script ----------------------------------------------------------
// Item Quality Upgrader
//--- Description -----------------------------------------------------------
// Raises an equipped Unique item to Legend, or a Legend item to Goddess,
// by consuming an identical copy of the same grade and powders.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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

public class CustomNpcQualityUpgrader : GeneralScript
{
	private static readonly EquipSlot[] AllowedSlots =
	{
		EquipSlot.Top, EquipSlot.Pants, EquipSlot.Gloves, EquipSlot.Shoes,
		EquipSlot.RightHand, EquipSlot.LeftHand, EquipSlot.RightHandSub, EquipSlot.LeftHandSub,
		EquipSlot.Necklace, EquipSlot.Bracelet1, EquipSlot.Bracelet2,
	};

	protected override void Load()
	{
		if (!Feature.IsEnabled("CustomNpcs"))
			return;

		AddNpc(57223, L("[Quality Upgrader] Helena"), "c_Klaipe", -255, 315, 0, this.HelenaDialog);
	}

	private async Task HelenaDialog(Dialog dialog)
	{
		var character = dialog.Player;
		dialog.SetTitle(L("Quality Upgrader"));

		var equipped = character.Inventory.GetEquip()
			.Where(a => AllowedSlots.Contains(a.Key) && a.Value != null && !a.Value.IsLocked && TryGetCost(GetGrade(a.Value), out _, out _, out _))
			.Select(a => a.Value)
			.ToList();

		if (equipped.Count == 0)
		{
			await dialog.Msg(L("Bring me an equipped Unique or Legend piece and an identical copy of the same grade, and I'll raise its quality with Sierra and Nucle Powder."));
			return;
		}

		var options = equipped.Select((a, i) => Option($"{a.Data.Name} ({GetGrade(a)})", i.ToString())).ToList();
		options.Add(Option(L("Never mind"), "exit"));

		var selection = await dialog.Select(L("Which piece should I work on?"), options);
		if (selection == "exit")
			return;

		var item = equipped[int.Parse(selection)];
		var grade = GetGrade(item);
		TryGetCost(grade, out var targetGrade, out var sierra, out var nucle);

		var confirm = await dialog.Select(LF("{0}: {1} to {2}.{nl}{nl}Cost: 1 identical {1} copy, {3} Sierra Powder, {4} Nucle Powder.", item.Data.Name, grade, targetGrade, sierra, nucle),
			Option(L("Upgrade"), "yes"),
			Option(L("Cancel"), "no"));

		if (confirm != "yes")
			return;

		var copy = character.Inventory.GetItems(a => a.ObjectId != item.ObjectId && a.Id == item.Id && !a.IsLocked && GetGrade(a) == grade).Values.FirstOrDefault();
		if (copy == null)
		{
			await dialog.Msg(LF("You need an unlocked {0} copy of this item in your inventory.", grade));
			return;
		}

		if (character.Inventory.CountItem(ItemId.Misc_Ore23) < sierra || character.Inventory.CountItem(ItemId.Misc_Ore22) < nucle)
		{
			await dialog.Msg(LF("You need {0} Sierra Powder and {1} Nucle Powder.", sierra, nucle));
			return;
		}

		if (character.Inventory.Remove(copy, 1, InventoryItemRemoveMsg.Given) != InventoryResult.Success)
			return;

		character.Inventory.Remove(ItemId.Misc_Ore23, sierra, InventoryItemRemoveMsg.Given);
		character.Inventory.Remove(ItemId.Misc_Ore22, nucle, InventoryItemRemoveMsg.Given);

		item.Properties.SetFloat(PropertyName.ItemGrade, (int)targetGrade);
		item.Properties.InvalidateAll();
		Send.ZC_OBJECT_PROPERTY(character, item);
		character.InvalidateProperties();

		await dialog.Msg(LF("Done! Your {0} is now {1}.", item.Data.Name, targetGrade));
	}

	private static ItemGrade GetGrade(Item item)
		=> (ItemGrade)(int)item.Properties.GetFloat(PropertyName.ItemGrade);

	private static bool TryGetCost(ItemGrade grade, out ItemGrade targetGrade, out int sierra, out int nucle)
	{
		switch (grade)
		{
			case ItemGrade.Unique:
				targetGrade = ItemGrade.Legend;
				sierra = 50;
				nucle = 500;
				return true;

			case ItemGrade.Legend:
				targetGrade = ItemGrade.Goddess;
				sierra = 200;
				nucle = 1000;
				return true;

			default:
				targetGrade = ItemGrade.None;
				sierra = 0;
				nucle = 0;
				return false;
		}
	}
}
