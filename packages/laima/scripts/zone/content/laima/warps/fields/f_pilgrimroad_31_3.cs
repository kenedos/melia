//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Mochia Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_pilgrimroad_31_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Mochia Forest to Feretory Hills
		AddWarp(1, "PILGRIM313_TO_PILGRIM311", -79, From("f_pilgrimroad_31_3", -1874, -1345), To("f_pilgrimroad_31_1", 1641, -429));

		// Mochia Forest to Sutatis Trade Route
		AddWarp(2, "PILGRIM313_TO_PILGRIM312", 185, From("f_pilgrimroad_31_3", 991.4526, 1720.131), To("f_pilgrimroad_31_2", -1717, -1594));
	}
}
