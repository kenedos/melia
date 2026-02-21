//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Nevellet Quarry 2F
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_cmine_66_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Nevellet Quarry 2F to Nevellet Quarry 1F
		AddWarp(1, "CMINE662_TO_CMINE661", 90, From("d_cmine_66_2", 2292.302, -15.74974), To("d_cmine_66_1", 285, 1823));
	}
}
