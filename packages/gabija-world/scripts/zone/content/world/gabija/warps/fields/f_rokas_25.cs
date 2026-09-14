//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Ramstis Ridge
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_rokas_25WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Ramstis Ridge to Gateway of the Great King
		AddWarp(30, "ROKAS25_ROKAS24", -49, From("f_rokas_25", -2368, -1192), To("f_rokas_24", 902, 1167));

		// Ramstis Ridge to Overlong Bridge Valley
		AddWarp(31, "ROKAS25_ROKAS26", 90, From("f_rokas_25", 2769, -1105), To("f_rokas_26", 404, -2375));

		// Ramstis Ridge to Royal Mausoleum 1F
		AddWarp(39, "ROKAS25_ZACHARIEL32", 210, From("f_rokas_25", 2049, -286), To("d_zachariel_32", -1324, -2198));
	}
}
