//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Rancid Labyrinth
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_catacomb_80_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Rancid Labyrinth F2 to Rancid Labyrinth F3
		AddWarp(1, "CATACOMB80_2_TO_CATACOMB_80_3_1", 270, From("d_catacomb_80_2", -498, -1666), To("d_catacomb_80_3", -863, -2252));
		AddWarp(1, "CATACOMB80_2_TO_CATACOMB_80_3_2", 0, From("d_catacomb_80_2", 44, -2282), To("d_catacomb_80_3", -863, -2252));

		// Rancid Labyrinth F2 to Rancid Labyrinth F3
		AddWarpPortal(From("d_catacomb_80_2", 25, -904), To("d_catacomb_80_3", 15, 919));
		AddWarpPortal(From("d_catacomb_80_2", -1807, 130), To("d_catacomb_80_3", 1044, -69));
		AddWarpPortal(From("d_catacomb_80_2", -40, 1885), To("d_catacomb_80_3", -143, 2036));
		AddWarpPortal(From("d_catacomb_80_2", 1097, 148), To("d_catacomb_80_3", 15, 921));
		AddWarpPortal(From("d_catacomb_80_2", -1132, 143), To("d_catacomb_80_3", 15, 921));
		AddWarpPortal(From("d_catacomb_80_2", 1614, -445), To("d_catacomb_80_3", 22, -926));

		// Rancid Labyrinth F2 to Rancid Labyrinth F2
		AddWarpPortal(From("d_catacomb_80_2", 1850, 0), To("d_catacomb_80_2", -42, 1683));
		AddWarpPortal(From("d_catacomb_80_2", 1620, 1003), To("d_catacomb_80_2", -39, 1042));
		AddWarpPortal(From("d_catacomb_80_2", 841, -907), To("d_catacomb_80_2", -1634, 1012));
		AddWarpPortal(From("d_catacomb_80_2", -1652, 514), To("d_catacomb_80_2", 34, -1664));
	}
}
