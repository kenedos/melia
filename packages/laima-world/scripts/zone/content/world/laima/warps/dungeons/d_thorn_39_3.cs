//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Laukyme Swamp
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_thorn_39_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Laukyme Swamp to Glade Hillroad
		AddWarp(1, "THORN393_TO_THORN392", 2, From("d_thorn_39_3", 1285.156, -2261.436), To("d_thorn_39_2", -2310, 1304));

		// Laukyme Swamp to Tyla Monastery
		AddWarp(2, "THORN393_TO_ABBEY394", 242, From("d_thorn_39_3", -1937.176, 1262.35), To("d_abbey_39_4", 1439, -1575));

		// Laukyme Swamp to Rancid Labyrinth
		AddWarp(4, "THORN393_TO_CATACOMB80_1", 254, From("d_thorn_39_3", 74.70003, -1006.845), To("d_catacomb_80_1", -24, -1221));
	}
}
