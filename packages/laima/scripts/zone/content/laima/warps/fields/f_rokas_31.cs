//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Zachariel Crossroads
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_rokas_31WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Zachariel Crossroads to Fedimian
		AddWarp(1, "ROKAS31_FEDIMIAN", 139, From("f_rokas_31", 940, -998), To("c_fedimian", -947, 52));

		// Zachariel Crossroads to Royal Mausoleum 1F - DISABLED (now handled by Royal Guard NPC)
		// AddWarp(2, "ROKAS31_ZACHARIEL32", 225, From("f_rokas_31", -1271, 715), To("d_zachariel_32", 32, -2294));
	}
}
