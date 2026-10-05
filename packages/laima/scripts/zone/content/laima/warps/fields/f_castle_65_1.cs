//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Delmore Hamlet
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_castle_65_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Delmore Hamlet to Absenta Reservoir
		AddWarp(1, "CASTLE651_TO_3CMLAKE84", 92, From("f_castle_65_1", 1518.875, -757.2734), To("f_3cmlake_84", -1830, -386));

		// Delmore Hamlet to Delmore Manor
		AddWarp(2, "CASTLE651_TO_CASTLE652", -89, From("f_castle_65_1", -2031.778, 812.2982), To("f_castle_65_2", 2139, -157));
	}
}
