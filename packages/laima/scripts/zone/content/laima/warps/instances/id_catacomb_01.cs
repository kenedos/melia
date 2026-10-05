//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Guards Graveyard
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class id_catacomb_01WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Guards Graveyard to Guards Graveyard
		AddWarp(6, "WS_CATACOMB01_1_CATACOMB01_2", 179, From("id_catacomb_01", -250, -134), To("id_catacomb_01", -258, 570));

		// Guards Graveyard to Guards Graveyard
		AddWarp(7, "WS_CATACOMB01_2_CATACOMB01_1", 0, From("id_catacomb_01", -260, 478), To("id_catacomb_01", -257, -214));

		// Guards Graveyard to Tenet Garden
		AddWarp(5, "CATACOMB_01_TO_GELE_57_4", 0, From("id_catacomb_01", -247.2492, -3548.954), To("f_gele_57_4", -505, 2040));

		// Guards Graveyard to Royal Masoleum F3
		AddWarp(5, "CATACOMB_01_TO_ZACHARIEL34", 225, From("id_catacomb_01", 240, 2909), To("d_zachariel_34", -1937, -1092));
	}
}
