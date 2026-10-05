//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Koru Jungle
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_bracken_63_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Koru Jungle to Paupys Crossing
		AddWarp(1, "BRACKEN_63_1_SIAULIAI_11_RE", 138, From("f_bracken_63_1", 1440.367, 1228.065), To("f_siauliai_11_re", -1366, 887));

		// Koru Jungle to Knidos Jungle
		AddWarp(2, "BRACKEN_63_1_BRACKEN_63_2", -89, From("f_bracken_63_1", -1984.144, -1359.99), To("f_bracken_63_2", 686, 2076));
	}
}
