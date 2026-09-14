//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Knidos Jungle
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_bracken_63_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Knidos Jungle to Khonot Forest
		AddWarp(3, "BRACKEN_63_2_BRACKEN_42_1", -24, From("f_bracken_63_2", 436.9442, -1777.703), To("f_bracken_42_1", 113, 986));

		// Knidos Jungle to Dadan Jungle
		AddWarp(2, "BRACKEN_63_2_BRACKEN_63_3", -88, From("f_bracken_63_2", -2175.037, -463.1476), To("f_bracken_63_3", 1577, 19));

		// Knidos Jungle to Koru Jungle
		AddWarp(1, "BRACKEN_63_2_BRACKEN_63_1", 173, From("f_bracken_63_2", 691.0748, 2266.504), To("f_bracken_63_1", -1849, -1370));
	}
}
