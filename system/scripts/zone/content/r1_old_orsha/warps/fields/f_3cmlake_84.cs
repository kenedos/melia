//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Absenta Reservoir
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_3cmlake_84WarpsScript : GeneralScript
{
	public override void Load()
	{
		// Absenta Reservoir to Pelke Shrine Ruins
		AddWarp(13, "3CMLAKE_84_TO_3CMLAKE_83", 84, From("f_3cmlake_84", 1093.878, -927.1453), To("f_3cmlake_83", -1704, 204));

		// Absenta Reservoir to Sienakal Graveyard
		AddWarp(24, "3CMLAKE_84_CATACOMB_33_1", 179, From("f_3cmlake_84", -1305.041, 1658.804), To("id_catacomb_33_1", 796, -1095));

		// Absenta Reservoir to Delmore Hamlet
		AddWarp(25, "3CMLAKE84_TO_CASTLE651", 270, From("f_3cmlake_84", -1959.931, -379.1844), To("f_castle_65_1", 1305, -726));
	}
}
