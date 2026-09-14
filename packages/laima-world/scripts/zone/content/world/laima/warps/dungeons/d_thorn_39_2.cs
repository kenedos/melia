//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Glade Hillroad
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_thorn_39_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Glade Hillroad to Viltis Forest
		AddWarp(1, "THORN392_TO_THORN391", 0, From("d_thorn_39_2", -2057, -1589), To("d_thorn_39_1", -789, 1608));

		// Glade Hillroad to Dina Bee Farm
		AddWarp(2, "THORN392_TO_SIAULIAI46_4", -40, From("d_thorn_39_2", -75.92445, -1810.932), To("f_siauliai_46_4", -274, 936));

		// Glade Hillroad to Laukyme Swamp
		AddWarp(3, "THORN392_TO_THORN393", 263, From("d_thorn_39_2", -2372.47, 1310.14), To("d_thorn_39_3", 1284, -2188));
	}
}
