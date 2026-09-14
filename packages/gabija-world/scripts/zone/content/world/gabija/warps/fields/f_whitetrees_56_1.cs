//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Mishekan Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_whitetrees_56_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Mishekan Forest to White tree 22_3
		AddWarp(1, "WHITETREES561_WHITETREES223", 171, From("f_whitetrees_56_1", 141.1313, 1754.506), To("f_whitetrees_22_3", 0, 0));

		// Mishekan Forest to Naptali Monastery
		AddWarp(2, "WHITETREES561_ABBEY225", -82, From("f_whitetrees_56_1", -1291.762, 297.6292), To("d_abbey_22_5", 0, 0));

		// Mishekan Forest to Syla Forest
		AddWarp(3, "WHITETREES561_WHITETREES233", 8, From("f_whitetrees_56_1", 1459.003, -727.3435), To("f_whitetrees_23_3", -553, 1436));
	}
}
