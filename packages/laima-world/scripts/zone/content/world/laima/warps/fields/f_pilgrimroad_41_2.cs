//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Salvia Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_pilgrimroad_41_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Salvia Forest to Genar Field
		AddWarp(1, "PILGRIM41_2_PILGRIM49", 238, From("f_pilgrimroad_41_2", -1859.174, 597.5132), To("f_pilgrimroad_49", 2319, 110));

		// Salvia Forest to Grynas Trail
		AddWarp(2, "PILGRIM41_2_KATYN45_1", 330, From("f_pilgrimroad_41_2", -1004.76324, -105.63251), To("f_katyn_45_1", 1527, 607));

		// Salvia Forest to Sekta Forest
		AddWarp(3, "PILGRIM41_2_PILGRIM41_4", 5, From("f_pilgrimroad_41_2", 7.247588, -1548.198), To("f_pilgrimroad_41_4", -20, 1190));

		// Salvia Forest to Rasvoy Lake
		AddWarp(4, "PILGRIM41_2_PILGRIM41_3", 90, From("f_pilgrimroad_41_2", 2007.936, -1021.727), To("f_pilgrimroad_41_3", -1422, 696));

		// Salvia Forest to Khonot Forest
		AddWarp(5, "PILGRIM41_2_TO_BRACKEN42_1", 188, From("f_pilgrimroad_41_2", 107.4228, 1605.379), To("f_bracken_42_1", -1016, -626));
	}
}
