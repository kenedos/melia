//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Demon Prison District 1
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Zone.Scripting;
using Melia.Zone.World.Maps;
using static Melia.Zone.Scripting.Shortcuts;

public class DVelniasprison511WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Demon Prison District 1 to Aqueduct Bridge Area
		AddWarpPortal(From("d_velniasprison_51_1", 16, 1730), To("f_farm_47_2", -1540, -1202));

		AddWarpPortal(From("d_velniasprison_51_1", 73, 965), To("d_velniasprison_51_1", 21, 445));
		AddWarpPortal(From("d_velniasprison_51_1", 21, 499), To("d_velniasprison_51_1", 72, 1034));

		AddWarpPortal(From("d_velniasprison_51_1", -656, -5), To("d_velniasprison_51_1", -2117, 9));
		AddWarpPortal(From("d_velniasprison_51_1", -2196, 1), To("d_velniasprison_51_1", -590, -9));

		AddWarpPortal(From("d_velniasprison_51_1", 589, -9), To("d_velniasprison_51_1", 2147, -34));
		AddWarpPortal(From("d_velniasprison_51_1", 2218, -34), To("d_velniasprison_51_1", 511, -9));

		AddWarpPortal(From("d_velniasprison_51_1", 45, -998), To("d_velniasprison_51_1", 21, -435));
		AddWarpPortal(From("d_velniasprison_51_1", 21, -514), To("d_velniasprison_51_1", 45, -1046));

		// Demon Prison District 1 to Demon Prison District 2
		AddWarpPortal(From("d_velniasprison_51_1", -7, -1709), To("d_velniasprison_51_2", 1129, 1820));
	}
}
