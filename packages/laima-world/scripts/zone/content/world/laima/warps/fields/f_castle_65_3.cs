//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Delmore Outskirts
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_castle_65_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Delmore Outskirts to Delmore Manor
		AddWarp(1, "CASTLE653_TO_CASTLE652", 3, From("f_castle_65_3", -1782.135, -1543.984), To("f_castle_65_2", -1272, 185));

		// Delmore Outskirts to Bellai Rainforest
		AddWarp(2, "CASTLE653_TO_ORCHARD323", 170, From("f_castle_65_3", -158.1526, 1714.325), To("f_orchard_32_3", 371, -1488));

		// Delmore Outskirts to Topes Fortress 1F
		AddWarp(3, "CASTLE653_TO_CASTLE671", 185, From("f_castle_65_3", 1129.019, 296.992), To("d_castle_67_1", -1640, -1352));
	}
}
