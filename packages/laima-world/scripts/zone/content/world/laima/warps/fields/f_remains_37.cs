//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Stele Road
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_remains_37WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Stele Road to Goddess' Ancient Garden
		AddWarp(31, "REMAINS37_REMAINS38", 176, From("f_remains_37", 564, 3367), To("f_remains_38", -1382, -2101));

		// Stele Road to Nuoridin Falls
		// AddWarp(61, "REMAINS37_REMAINS37_1", 160, From("f_remains_37", -72.25984, 2797.046), To("f_remains_37_1", 2061, -689));

		// Stele Road to Gateway of the Great King
		AddWarp(62, "REMAINS37_ROKAS24", 0, From("f_remains_37", 478, -2630), To("f_rokas_24", -1793, 1853));

		// Stele Road to Seir Rainforest
		AddWarp(63, "REMAINS37_ORCHARD_32_4", 0, From("f_remains_37", 1164, -2717), To("f_orchard_32_4", -1258, -177));
	}
}
