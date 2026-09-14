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
		// Ramstis Ridge to Fedimian
		AddWarp(30, "ROKAS25_FEDIMIAN", -49, From("f_rokas_25", -2368, -1192), To("c_fedimian", 695, -132));
	}
}
