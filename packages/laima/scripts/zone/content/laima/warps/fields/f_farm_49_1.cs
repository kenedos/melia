//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Greene Manor
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_farm_49_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Greene Manor to Aqueduct Bridge Area
		AddWarp(1, "FARM491_TO_FARM472", 106, From("f_farm_49_1", 2378, 909), To("f_farm_47_2", -1578, 998));
	}
}
