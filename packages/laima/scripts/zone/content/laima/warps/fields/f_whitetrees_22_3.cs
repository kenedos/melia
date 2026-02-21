//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Izoliacjia Plateau
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_whitetrees_22_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Izoliacjia Plateau to Mishekan Forest
		AddWarp(1, "WHITETREES22_3_WHITETREES56_1", 225, From("f_whitetrees_22_3", -171.7528, 1423.126), To("f_whitetrees_56_1", 1423.6913, -609.6343));

		// Izoliacjia Plateau to Narvas Temple
		AddWarp(2, "WHITETREES22_3_ABBEY22_4", -89, From("f_whitetrees_22_3", -1685.871, 304.4066), To("d_abbey_22_4", 1775, 1304));

		// Izoliacjia Plateau to Orsha
		AddWarp(38, "WHITETREES22_3_ORSHA", 90, From("f_whitetrees_22_3", 1765, 1063), To("c_orsha", -1367, -679));
	}
}
