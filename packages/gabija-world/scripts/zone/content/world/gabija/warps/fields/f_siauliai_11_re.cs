//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Paupys Crossing
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_siauliai_11_reWarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Paupys Crossing to Woods of the Linked Bridges
		AddWarp(1, "SIAULIAI11RE_TO_SIAULIAI15RE", 90, From("f_siauliai_11_re", 2706.208, -608.4064), To("f_siauliai_15_re", -3427, -103));

		// Paupys Crossing to Koru Jungle
		AddWarp(2, "SIAULIAI11RE_TO_BRACKEN631", -77, From("f_siauliai_11_re", -1475.452, 855.1358), To("f_bracken_63_1", 1339, 1150));

		// Paupys Crossing to Ashaq Underground Prison 1F
		AddWarp(3, "SIAULIAI11RE_TO_PRISON621", 108, From("f_siauliai_11_re", 509.8523, 1657.525), To("d_prison_62_1", -400, -1361));
	}
}
