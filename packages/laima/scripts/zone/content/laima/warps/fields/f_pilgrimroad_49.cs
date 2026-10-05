//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Genar Field
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_pilgrimroad_49WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Thaumas Trail to Grynas Trails
		AddWarp(1, "PILGRIM49_KATYN45_1", 315, From("f_pilgrimroad_49", 1528, -1658), To("f_katyn_45_1", -930, 910));

		// Thaumas Trail to Nefritas Cliff
		AddWarp(2, "PILGRIM49_GELE57_3", 315, From("f_pilgrimroad_49", -2314.767, -647.9147), To("f_gele_57_3", 1394, -690));

		// Thaumas Trail to Salvia Forest
		AddWarp(3, "PILGRIM49_PILGRIM41_2", 90, From("f_pilgrimroad_49", 2378.388, 184.8524), To("f_pilgrimroad_41_2", -1742, 520));
	}
}
