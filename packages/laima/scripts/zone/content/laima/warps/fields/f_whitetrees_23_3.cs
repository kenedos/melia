//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Syla Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_whitetrees_23_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Syla Forest to Pystis Forest
		AddWarp(1, "WHITETREES23_3_TO_MAPLE23_2", 60, From("f_whitetrees_23_3", 374.4368, -1220.554), To("f_maple_23_2", -1321, 291));

		// Syla Forest to Central Parias Forest
		AddWarp(2, "WHITETREES23_3_TO_MAPLE24_1", -70, From("f_whitetrees_23_3", -1896.812, -482.626), To("f_maple_24_1", 1584, 1369));

		// Syla Forest to Orsha
		AddWarp(3, "WHITETREES23_3_TO_ORSHA", 171, From("f_whitetrees_23_3", -552.1576, 1559.02), To("c_orsha", 183, -1327));
	}
}
