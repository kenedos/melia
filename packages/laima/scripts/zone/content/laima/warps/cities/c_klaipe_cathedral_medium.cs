//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Laima's Sanctuary
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class c_klaipe_cathedral_mediumWarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Laima's Sanctuary to Klaipeda
		AddWarp(1, "CATHEDRAL_MEDIUM_KLAPEDA", 90, From("c_klaipe_cathedral_medium", 604, 313), To("c_Klaipe", -1036, -582));

		// Laima's Sanctuary to Laima's Sanctuary Interior
		AddWarp(2, "CATHEDRAL_MEDIUM_CATHEDRAL_MEDIUM_IN", 270, From("c_klaipe_cathedral_medium", 26, 0), To("c_klaipe_cathedral_medium_in", 109, 7));
	}
}
