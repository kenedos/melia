//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Demon Prison District 3
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Zone.Scripting;
using Melia.Zone.World.Maps;
using static Melia.Zone.Scripting.Shortcuts;

public class DVelniasprison513WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Demon Prison District 4 to Demon Prison District 3
		AddWarpPortal(From("d_velniasprison_51_3", -3310, -696), To("d_velniasprison_51_4", -464, 1185));

		AddWarpPortal(From("d_velniasprison_51_3", 2955, 906), To("d_velniasprison_51_3", -2889, 914));
		AddWarpPortal(From("d_velniasprison_51_3", -3025, 909), To("d_velniasprison_51_3", 2840, 904));

		AddWarpPortal(From("d_velniasprison_51_3", -1914, 1129), To("d_velniasprison_51_3", 18, 815));
		AddWarpPortal(From("d_velniasprison_51_3", 12, 708), To("d_velniasprison_51_3", -1914, 1028));

		AddWarpPortal(From("d_velniasprison_51_3", 42, 1196), To("d_velniasprison_51_3", 1830, -1110));
		AddWarpPortal(From("d_velniasprison_51_3", 1800, -1199), To("d_velniasprison_51_3", 34, 1047));

		// Demon Prison District 4 to Demon Prison District 5
		AddWarpPortal(From("d_velniasprison_51_3", 2872, -647), To("d_velniasprison_51_5", -177, -1108));
	}
}
