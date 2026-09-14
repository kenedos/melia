//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Rancid Labyrinth
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_catacomb_80_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Rancid Labyrinth F3 to Rancid Labyrinth F2
		AddWarp(1, "CATACOMB80_3_TO_CATACOMB80_2", 225, From("d_catacomb_80_3", -946.1943, -2180.591), To("d_catacomb_80_2", 39, -2172));

		// Rancid Labyrinth F3 to Rancid Labyrinth F3
		AddWarpPortal(From("d_catacomb_80_3", 28, 2344), To("d_catacomb_80_3", 197, 2031));

		// Rancid Labyrinth F3 to Rancid Labyrinth F1
		AddWarpPortal(From("d_catacomb_80_3", -706, 963), To("d_catacomb_80_1", -950, -1701));

		// Rancid Labyrinth F3 to Rancid Labyrinth F2
		AddWarpPortal(From("d_catacomb_80_3", -1341, -977), To("d_catacomb_80_2", 1405, 133));
	}
}
