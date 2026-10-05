//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Rancid Labyrinth
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_catacomb_80_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Rancid Labyrinth to Laukyme Swamp
		AddWarp(1, "CATACOMB80_1_TO_THORN39_3", 180, From("d_catacomb_80_1", -25, -1124), To("d_thorn_39_3", 141, -1030));

		// Rancid Labyrinth to Rancid Labyrinth F2
		AddWarpPortal(From("d_catacomb_80_1", -56, -2521), To("d_catacomb_80_2", 34, -1664));
		AddWarpPortal(From("d_catacomb_80_1", 586, 357), To("d_catacomb_80_2", 34, -1664));
		AddWarpPortal(From("d_catacomb_80_1", -569, 351), To("d_catacomb_80_2", 34, -1664));
	}
}
