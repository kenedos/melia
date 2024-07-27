//--- Melia Script ----------------------------------------------------------
// West Siauliai Woods
//--- Description -----------------------------------------------------------
// NPCs found in and around West Siauliai Woods.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliaiWestNpcScript : GeneralScript
{
	public override void Load()
	{
		AddNpc(20113, "[Templar Master] Knight Commander Uska", "Uska", "f_siauliai_west", -303, 422, 254, async dialog =>
		{
			if (await dialog.Hooks("BeforeDialog"))
				await dialog.Msg("We need your help. The monsters have overrun Klaipeda and this the final defense line.");
			else
			{
				await dialog.Msg("Welcome.{nl}You also dreamt of the goddess.");
			}
		});

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------

		AddNpc(147392, "Lv1 Treasure Chest", "f_siauliai_west", -580, -1417, 180.0, async dialog =>
		{
			await OpenChest(dialog.Player, dialog.Npc);
			dialog.Player.Inventory.Add(ItemId.BRC02_114, 1, InventoryAddType.PickUp);
		});

		// Collection Chest
		AddNpc(147393, "Collection Chest", "f_siauliai_west", 1334, -1109, 45.0, async dialog =>
		{
			await OpenChest(dialog.Player, dialog.Npc);
			dialog.Player.Inventory.Add(ItemId.COLLECT_118, 1, InventoryAddType.PickUp);
		});
	}
}
