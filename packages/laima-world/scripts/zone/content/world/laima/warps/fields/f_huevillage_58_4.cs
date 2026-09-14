//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Septyni Glen
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_huevillage_58_4WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Septyni Glen to Cobalt Forest
		AddWarp(24, "HUEVILLAGE58_4_TO_HUEVILLAGE58_3", -7, From("f_huevillage_58_4", 1292.37, -1060.92), To("f_huevillage_58_3", -1080, -97));

		// Septyni Glen to Dina Bee Farm
		AddWarp(25, "HUEVILLAGE58_4_TO_THORN19", 175, From("f_huevillage_58_4", 993.48, 1273.34), To("f_siauliai_46_4", 318, -1000));

		// Septyni Glen to Tenet Garden
		AddWarp(26, "HUEVILLAGE584_GELE574", 90, From("f_huevillage_58_4", 1458, 360), To("f_gele_57_4", -2162, 988));
	}
}
