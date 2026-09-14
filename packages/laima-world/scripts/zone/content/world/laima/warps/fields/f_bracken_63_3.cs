//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Dadan Jungle
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_bracken_63_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Dadan Jungle to Knidos Jungle
		AddWarp(1, "BRACKEN_63_3_BRACKEN_63_2", 48, From("f_bracken_63_3", 1650.106, -48.37425), To("f_bracken_63_2", -2034, -461));

		// Dadan Jungle to Novaha Assembly Hall
		AddWarp(2, "BRACKEN_63_3_ABBAY_64_1", 33, From("f_bracken_63_3", 325.1867, -1196.709), To("d_abbey_64_1", 827, 853));

		// Dadan Jungle to Alemeth Forest
		AddWarp(3, "BRACKEN_63_3_ORCHARD_34_1", 225, From("f_bracken_63_3", -1377, 456), To("f_orchard_34_1", 1059, -1020));
	}
}
