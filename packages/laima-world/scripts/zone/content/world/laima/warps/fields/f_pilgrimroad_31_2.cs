//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Sutatis Trade Route
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_pilgrimroad_31_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Sutatis Trade Route to Mochia Forest
		AddWarp(1, "PILGRIM312_TO_PILGRIM313", -74, From("f_pilgrimroad_31_2", -1813.89, -1645.46), To("f_pilgrimroad_31_3", 999, 1617));

		// Sutatis Trade Route to Galeed Plateau
		AddWarp(2, "PILGRIM_31_2_TO_ROKAS_36_1", 173, From("f_pilgrimroad_31_2", -173.0157, 2386.148), To("f_rokas_36_1", -1295, -439));

		// Sutatis Trade Route to Mochia Forest
		AddWarp(3, "PILGRIM312_TO_PILGRIM313", 86, From("f_pilgrimroad_31_2", 1247, 1303), To("f_pilgrimroad_31_3", 999, 1617));
	}
}
