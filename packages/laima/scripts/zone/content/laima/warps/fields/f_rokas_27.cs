//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Akmens Ridge
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_rokas_27WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Akmens Ridge to Overlong Bridge Valley
		AddWarp(1, "ROKAS27_ROKAS26_1", 270, From("f_rokas_27", 317, -646), To("f_rokas_26", 1147, -2191));

		// Akmens Ridge to Overlong Bridge Valley
		AddWarp(2, "ROKAS27_ROKAS26_2", 225, From("f_rokas_27", -173, -1198), To("f_rokas_26", 429, -2383));

		// Akmens Ridge to Tenet Garden
		AddWarp(3, "ROKAS27_GELE574", 0, From("f_rokas_27", -177, -3640), To("f_gele_57_4", -910, 2369));
	}
}
