//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Carlyle's Mausoleum
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class id_catacomb_33_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Carlyle's Mausoleum to Sienakal Graveyard
		AddWarp(1, "CATACOMB_33_2_TO_CATACOMB_33_1", 225, From("id_catacomb_33_2", -1574.815, 648.8157), To("id_catacomb_33_1", 742, 1846));
	}
}
