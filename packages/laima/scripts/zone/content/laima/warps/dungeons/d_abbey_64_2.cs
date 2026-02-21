//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Novaha Annex
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_abbey_64_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Novaha Annex to Novaha Assembly Hall
		AddWarp(301, "ABBEY_64_2_ABBEY_64_1", 90, From("d_abbey_64_2", 1660, -28), To("d_abbey_64_1", -1356, -1692));

		// Novaha Annex to Novaha Institute
		AddWarp(302, "ABBEY_64_2_ABBEY_64_3", 268, From("d_abbey_64_2", -1967.758, 71.18753), To("d_abbey_64_3", 1356, 225));
	}
}
