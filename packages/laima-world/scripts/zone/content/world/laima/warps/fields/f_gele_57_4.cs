//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Tenet Garden
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_gele_57_4WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Tenet Garden to Nefritas Cliff
		AddWarp(1, "GELE574_TO_GELE573", 6, From("f_gele_57_4", -841, -155), To("f_gele_57_3", 199, 1086));

		// Tenet Garden to Tenet Church 1F
		AddWarp(3, "GELE574_TO_CHAPEL576", 183, From("f_gele_57_4", 1296.24, 2145.48), To("d_chapel_57_6", -1638, 448));

		// Tenet Garden to Tenet Church B1
		AddWarp(4, "GELE574_TO_CHAPEL575", 181, From("f_gele_57_4", 1072.8, 2046.17), To("d_chapel_57_5", -1258, 1095));

		// Tenet Garden to Guards Graveyard
		AddWarp(56, "GELE_57_4_TO_CATACOMB_01", 180, From("f_gele_57_4", -442.254, 2141.916), To("id_catacomb_01", -241, -3428));

		// Tenet Garden to Akmens Ridge
		AddWarp(5, "GELE574_ROKAS27", 180, From("f_gele_57_4", -910, 2474), To("f_rokas_27", -182, -3532));

		// Tenet Garden to Septyni Glen
		AddWarp(6, "GELE574_HUEVILLAGE584", 270, From("f_gele_57_4", -2304, 988), To("f_huevillage_58_4", 1392, 358));
	}
}
