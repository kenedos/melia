//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Feretory Hills
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_pilgrimroad_31_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Feretory Hills to Delmore Manor
		AddWarp(1, "PILGRIM311_TO_CASTLE652", 267, From("f_pilgrimroad_31_1", -1239.37, -833.89), To("f_castle_65_2", 1122, 2280));

		// Feretory Hills to Mochia Forest
		AddWarp(3, "PILGRIM311_TO_PILGRIM313", 90, From("f_pilgrimroad_31_1", 1758.92, -436.23), To("f_pilgrimroad_31_3", -1800, -1317));
	}
}
