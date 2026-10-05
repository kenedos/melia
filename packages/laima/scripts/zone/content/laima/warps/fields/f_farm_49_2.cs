//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Shaton Farm
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_farm_49_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Shaton Farm to Myrkiti Farm
		AddWarp(2, "FARM492_TO_FARM493", 135, From("f_farm_49_2", 1976, 1472), To("f_farm_47_3", -1621, -827));

		// Shaton Farm to Spring Light Woods
		AddWarp(3, "FARM49_2_SIAULIAI46_1", 0, From("f_farm_49_2", 670, -2144), To("f_siauliai_46_1", 601, 1198));
	}
}
