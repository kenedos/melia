//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Letas Stream
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_katyn_12WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Letas Stream to Grynas Training Camp
		AddWarp(1, "KATYN_12_KATYN_45_2", 180, From("f_katyn_12", -1489, 2356), To("f_katyn_45_2", 1664, -2096));

		// Letas Stream to Arrow Path
		AddWarp(2, "KATYN_12_KATYN_13_3", 0, From("f_katyn_12", 93, -1582), To("f_katyn_13_3", 140, 409));

		// Letas Stream to Pelke Shrine Ruins
		AddWarp(3, "KATYN_12_3CMLAKE_83", 315, From("f_katyn_12", -3156, 123), To("f_3cmlake_83", 782, -22));
	}
}
