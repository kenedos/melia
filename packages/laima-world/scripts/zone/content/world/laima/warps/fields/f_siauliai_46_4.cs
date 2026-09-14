//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Dina Bee Farm
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_siauliai_46_4WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Dina Bee Farm to Septyni Glen
		AddWarp(1, "SIAULIAI46_4_HUEVILLAGE58_4", 360, From("f_siauliai_46_4", 303, -1135), To("f_huevillage_58_4", 991, 1194));

		// Dina Bee Farm to Glade Hill Road
		AddWarp(2, "SIAULIAI46_4_THORN39_1", 180, From("f_siauliai_46_4", -277, 1036), To("d_thorn_39_2", -20, -1731));

		// Dina Bee Farm to Overlong Bridge Valley
		AddWarp(3, "SIAULIAI46_4_ROKAS26", 90, From("f_siauliai_46_4", 1494, 326), To("f_rokas_26", -957, -1757));
	}
}
