//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Grynas Trails
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_katyn_45_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Grynas Trails to Genar Field
		AddWarp(6, "KATYN45_1_PILGRIM49", 180, From("f_katyn_45_1", -912, 1047), To("f_pilgrimroad_49", 1568, -1634));

		// Grynas Trails to Grynas Hills
		AddWarp(4, "KATYN45_1_KATYN45_3", 0, From("f_katyn_45_1", -878.62024, -1685.3461), To("f_katyn_45_3", 1904.9453, 1444.3137));

		// Grynas Trails to Salvia Forest
		AddWarp(5, "KATYN45_1_KATYN45_2", 92, From("f_katyn_45_1", 1626.654, 606.5546), To("f_pilgrimroad_41_2", -962.2044, -40.74028));
	}
}
