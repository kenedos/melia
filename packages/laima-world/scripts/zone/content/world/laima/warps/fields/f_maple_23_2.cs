//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Pystis Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_maple_23_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Pystis Forest to Emmet Forest
		AddWarp(1, "MAPLE23_2_TO_WHITETREES23_1", 144, From("f_maple_23_2", 1600.112, 369.1022), To("f_whitetrees_23_1", -964, -1204));

		// Pystis Forest to Syla Forest
		AddWarp(3, "MAPLE23_2_TO_WHITETREES23_3", 258, From("f_maple_23_2", -1491.322, 450.3834), To("f_whitetrees_23_3", 326, -1215));

		// Pystis Forest to Southern Parias Forest
		AddWarp(4, "MAPLE23_2_TO_MAPLE24_2", 89, From("f_maple_23_2", -901.704, -1639.13), To("f_maple_24_2", 1427.7964, 1176.3547));
	}
}
