//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Overlong Bridge Valley
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_rokas_26WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Overlong Bridge Valley to Gateway of the Great King
		AddWarp(1, "ROKAS26_ROKAS24", 134, From("f_rokas_26", 2301, 113), To("f_rokas_24", 934, -2946));

		// Overlong Bridge Valley to Dina Bee Farm
		AddWarp(2, "ROKAS26_SIAULIAI46_4", 270, From("f_rokas_26", -1066, -1760), To("f_siauliai_46_4", 1398, 326));

		// Overlong Bridge Valley to Akmens Ridge
		AddWarp(3, "ROKAS26_ROKAS27_1", 0, From("f_rokas_26", 1152, -2301), To("f_rokas_27", 398, -649));

		// Overlong Bridge Valley to Akmens Ridge
		AddWarp(4, "ROKAS26_ROKAS27_2", 0, From("f_rokas_26", 475, -2520), To("f_rokas_27", -108, -1267));
	}
}
