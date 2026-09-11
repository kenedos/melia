//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Laima's Sanctuary Interior
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class c_klaipe_cathedral_medium_inWarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Laima's Sanctuary Interior to Laima's Sanctuary
		AddWarp(1, "CATHEDRAL_MEDIUM_IN_CATHEDRAL_MEDIUM", 90, From("c_klaipe_cathedral_medium_in", 189, 7), To("c_klaipe_cathedral_medium", 95, 1));
	}
}
