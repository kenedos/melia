//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Fedimian Suburbs
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_remains_40WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Fedimian Suburbs to Escanciu Village
		AddWarp(100, "WS_REMAINS40_TO_REMAINS39", 45, From("f_remains_40", 111, -2598), To("f_remains_39", 944, 1554));

		// Fedimian Suburbs to Fedimian
		AddWarp(101, "WS_REMAINS40_TO_FEDMIAN", 205, From("f_remains_40", -2393, -1377), To("c_fedimian", 630, -89));
	}
}
