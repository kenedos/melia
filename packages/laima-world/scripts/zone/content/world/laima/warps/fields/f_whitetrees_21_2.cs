//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Nobreer Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_whitetrees_21_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Nobreer Forest to Emmet Forest
		AddWarp(9, "WHITETREES_21_2_WHITETREES_23_1", 90, From("f_whitetrees_21_2", 1119, -1222), To("f_whitetrees_23_1", -1566.8406, -174.78769));

		// Nobreer Forest to Orsha
		AddWarp(11, "WHITETREES_21_2_WHITETREES_22_1", 225, From("f_whitetrees_21_2", -1370.229, 517.3204), To("c_orsha", 1377, 345));
	}
}
