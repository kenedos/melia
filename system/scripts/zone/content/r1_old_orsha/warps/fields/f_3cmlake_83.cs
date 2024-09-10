//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Pelke Shrine Ruins
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_3cmlake_83WarpsScript : GeneralScript
{
	public override void Load()
	{
		// Pelke Shrine Ruins to Absenta Reservoir
		AddWarp(23, "3CMLAKE_83_TO_3CMLAKE_84", -89, From("f_3cmlake_83", -1801.169, 216.6266), To("f_3cmlake_84", 972, -908));

		// Pelke Shrine Ruins to Letas Stream
		AddWarp(39, "3CMLAKE_83_KATYN_12", 90, From("f_3cmlake_83", 894.5733, -36.48208), To("f_katyn_12", 2109, 1085));

		// Pelke Shrine Ruins to Arcus Forest
		AddWarp(47, "3CMLAKE83_BRACKEN43_1", 136, From("f_3cmlake_83", 145.6394, 869.1643), To("f_bracken_43_1", -2391, -156));
	}
}
