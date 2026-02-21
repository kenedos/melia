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
		// Mishekan Forest to Izoliacjia Plateau
		AddWarp(1, "WHITETREES561_WHITETREES233", 8, From("f_whitetrees_56_1", 1459.003, -727.3435), To("f_whitetrees_22_3", -107, 1364));

		// Mishekan Forest to Khonot Forest
		AddWarp(2, "WHITETREES561_BRACKEN42_1", 270, From("f_whitetrees_56_1", -1292.8319, 312.18533), To("f_bracken_42_1", 1991.6152, -187.15211));
	}
}
