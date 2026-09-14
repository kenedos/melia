//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Elgos Abbey Main Building
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_abbey_35_4WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Elgos Abbey Main Building to Galeed Plateau
		AddWarp(1, "ABBEY_35_4_TO_ROKAS_36_1", -85, From("d_abbey_35_4", -1715.557, -549.7579), To("f_rokas_36_1", 1675, 364));

		// Elgos Abbey Main Building to Elgos Monastery Annex
		AddWarp(7, "ABBEY_35_4_ABBEY_35_3", 178, From("d_abbey_35_4", 16.46743, 1459.366), To("d_abbey_35_3", 3, -1489));
	}
}
