//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Sienakal Graveyard
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class id_catacomb_33_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Sienakal Graveyard to Absenta Reservoir
		AddWarp(1, "CATACOMB_33_1_3CMLAKE_84", 0, From("id_catacomb_33_1", 792.8137, -1283.743), To("f_3cmlake_84", -1306, 1510));

		// Sienakal Graveyard to Carlyle's Mausoleum
		AddWarp(2, "CATACOMB_33_1_TO_CATACOMB_33_2", 180, From("id_catacomb_33_1", 745.4448, 2116.098), To("id_catacomb_33_2", -1530, 594));
	}
}
