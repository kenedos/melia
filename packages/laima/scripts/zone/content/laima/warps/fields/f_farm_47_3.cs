//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Myrkiti Farm
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_farm_47_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Myrkiti Farm to Baron Allerno
		AddWarp(1, "FARM_47_3_TO_FARM_47_4", 90, From("f_farm_47_3", 1034.62, 56.88991), To("f_siauliai_47_4", -2034, -134));

		// Myrkiti Farm to Aqueduct Bridge Area
		AddWarp(2, "FARM_47_3_TO_FARM_47_2", -81, From("f_farm_47_3", -2181.688, 430.9418), To("f_farm_47_2", 2403, -1256));

		// Myrkiti Farm to Shaton Farm
		AddWarp(3, "FARM_47_3_TO_FARM_49_2", 0, From("f_farm_47_3", -1628, -924), To("f_farm_49_2", 1930, 1429));
	}
}
