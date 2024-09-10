//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Dadan Jungle
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_bracken_63_3WarpsScript : GeneralScript
{
	public override void Load()
	{
		// Dadan Jungle to Knidos Jungle
		AddWarp(1, "BRACKEN_63_3_BRACKEN_63_2", 48, From("f_bracken_63_3", 1650.106, -48.37425), To("f_bracken_63_2", -2034, -461));

		// Dadan Jungle to Novaha Assembly Hall
		AddWarp(2, "BRACKEN_63_3_ABBAY_64_1", 33, From("f_bracken_63_3", 325.1867, -1196.709), To("d_abbey_64_1", 827, 853));

		// Dadan Jungle to Karolis Springs
		AddWarp(3, "BRACKEN_63_3_KATYN_10", 208, From("f_bracken_63_3", -1445.727, 544.7219), To("f_katyn_10", 4354, -1507));

		// Dadan Jungle to Nevellet Quarry 1F
		AddWarp(21, "BRACKEN633_TO_CMINE661", 165, From("f_bracken_63_3", 528.4972, 1608.387), To("d_cmine_66_1", 104, -1710));
	}
}
