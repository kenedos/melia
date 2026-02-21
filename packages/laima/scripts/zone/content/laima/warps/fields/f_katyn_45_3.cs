//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Grynas Hills
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_katyn_45_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Grynas Hills to Gele Plateau
		AddWarp(1, "KATYN45_3_GELE57_2", 270, From("f_katyn_45_3", -2144, 1840), To("f_gele_57_2", 1510, -412));

		// Grynas Hills to Grynas Training Camp
		AddWarp(2, "KATYN45_3_KATYN45_2", 360, From("f_katyn_45_3", -79, -1799), To("f_katyn_45_2", -1250.6573, 1676.9062));

		// Grynas Hills to Grynas Trails
		AddWarp(3, "KATYN45_3_KATYN45_1", 90, From("f_katyn_45_3", 2074.8604, 1440.3363), To("f_katyn_45_1", -873.8473, -1579.3231));
	}
}
