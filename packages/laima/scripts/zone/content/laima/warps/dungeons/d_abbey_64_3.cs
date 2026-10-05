//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Novaha Institute
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_abbey_64_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Novaha Institute to Novaha Annex
		AddWarp(1, "ABBEY_64_3_ABBEY_64_2", 90, From("d_abbey_64_3", 1533.872, 227.4609), To("d_abbey_64_2", -1821, 103));
	}
}
