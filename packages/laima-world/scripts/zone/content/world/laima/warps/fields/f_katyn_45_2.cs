//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Grynas Training Camp
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_katyn_45_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Grynas Training Camp to Grynas Hills
		AddWarp(1, "KATYN45_2_KATYN45_1", 270, From("f_katyn_45_2", -1344, 1681), To("f_katyn_45_3", -73, -1650));

		// Grynas Training Camp to Letas Stream
		AddWarp(1, "KATYN45_2_KATYN_12", 0, From("f_katyn_45_2", 1669, -2194), To("f_katyn_12", -1508, 2249));
	}
}
