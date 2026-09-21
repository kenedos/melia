//--- Melia Script ----------------------------------------------------------
// Penitence Route Quest NPCs
//--- Description -----------------------------------------------------------
// The priest studying Naktis' curse halfway along the Penitence Route, whose
// report the Main Chamber is waiting on.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad55QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Sq03 = new QuestId(20308);

	protected override void Load()
	{
		// Priest Gadan
		//-------------------------------------------------------------------------
		// The client's gentype names him Roana; the quest that sends the player
		// here calls him Gadan, so the port keeps the name the player reads.
		AddNpc(147386, L("Priest Gadan"), "PILGRIMROAD55_SQ05", "f_pilgrimroad_55", -696.96, 593.11, 356, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Gadan"));

			if (character.Quests.IsActive(Sq03) && character.Inventory.CountItem(ItemId.PRIST_REPORT02) == 0)
			{
				await dialog.Msg(L("Aden sent you? Then he is still at it, and still alive."));
				await dialog.Msg(L("Take my report to him. The Penitence Route is no place to be carrying paper around."));
				character.Inventory.Add(ItemId.PRIST_REPORT02, 1, InventoryAddType.PickUp);
				return;
			}

			await dialog.Msg(L("The curse thins out along this road, but it never quite lets go."));
		});
	}
}
