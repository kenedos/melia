//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Novaha Assembly Hall
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_abbey_64_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Novaha Assembly Hall to Dadan Jungle
		AddWarp(1, "ABBEY_64_1_BRACKEN_63_3", 90, From("d_abbey_64_1", 911.2021, 857.131), To("f_bracken_63_3", 261, -1122));

		// Novaha Assembly Hall to Novaha Annex
		AddWarp(2, "ABBEY_64_1_ABBAY_64_2", -89, From("d_abbey_64_1", -1502.51, -1706.217), To("d_abbey_64_2", 1557, -43));
	}
}
