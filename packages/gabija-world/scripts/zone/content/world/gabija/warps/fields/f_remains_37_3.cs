//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Istora Ruins
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_remains_37_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Istora Ruins to Namu Temple Ruins
		AddWarp(1, "REMAINS37_3_REMAINS37_2", 1, From("f_remains_37_3", -544.7141, -1603.678), To("f_remains_37_2", -796, 1517));
	}
}
