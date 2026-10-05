//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Gateway of the Great King
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_rokas_24WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Gateway of the Great King to King's Plateau
		AddWarp(1, "ROKAS24_ROKAS30", 90, From("f_rokas_24", 1647, -1035), To("f_rokas_30", -1708, -118));

		// Gateway of the Great King to Seir Rainforest
		AddWarp(2, "ROKAS24_ROKAS25", 176, From("f_rokas_24", 904, 1240), To("f_orchard_32_4", -22, -588));

		// Gateway of the Great King to Overlong Bridge Valley
		AddWarp(3, "ROKAS24_ROKAS26", 0, From("f_rokas_24", 960, -3037), To("f_rokas_26", 2203, 83));

		// Gateway of the Great King to Stele Road
		AddWarp(4, "ROKAS24_REMAINS37", 270, From("f_rokas_24", -1883, 1864), To("f_remains_37", 491, -2497));
	}
}
