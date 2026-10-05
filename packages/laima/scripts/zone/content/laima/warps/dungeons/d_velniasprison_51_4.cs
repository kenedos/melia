//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Demon Prison District 4
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Zone.Scripting;
using Melia.Zone.World.Maps;
using static Melia.Zone.Scripting.Shortcuts;

public class DVelniasprison514WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Demon Prison District 3 to Demon Prison District 2
		AddWarpPortal(From("d_velniasprison_51_4", -1064, 1527), To("d_velniasprison_51_2", 1364, 429));


		AddWarpPortal(From("d_velniasprison_51_4", 397, 819), To("d_velniasprison_51_4", 494, -225));
		AddWarpPortal(From("d_velniasprison_51_4", 590, -204), To("d_velniasprison_51_4", 297, 824));

		AddWarpPortal(From("d_velniasprison_51_4", -1063, 17), To("d_velniasprison_51_4", -2624, 1013));
		AddWarpPortal(From("d_velniasprison_51_4", -2677, 1105), To("d_velniasprison_51_4", -950, 19));

		AddWarpPortal(From("d_velniasprison_51_4", -1751, 653), To("d_velniasprison_51_4", -1490, 1002));
		AddWarpPortal(From("d_velniasprison_51_4", -1593, 1002), To("d_velniasprison_51_4", -1865, 664));


		// Demon Prison District 3 to Demon Prison District 4
		AddWarpPortal(From("d_velniasprison_51_4", -464, 1269), To("d_velniasprison_51_3", -3214, -697));
	}
}
