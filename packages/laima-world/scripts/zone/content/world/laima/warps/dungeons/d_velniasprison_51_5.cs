//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Demon Prison District 5
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Zone.Scripting;
using Melia.Zone.World.Maps;
using static Melia.Zone.Scripting.Shortcuts;

public class DVelniasprison515WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Demon Prison District 5 to Demon Prison District 4
		AddWarpPortal(From("d_velniasprison_51_5", -234, -1165), To("d_velniasprison_51_3", 2777, -643));

		AddWarpPortal(From("d_velniasprison_51_5", -1393, -179), To("d_velniasprison_51_5", -2266, 28));
		AddWarpPortal(From("d_velniasprison_51_5", -2358, 23), To("d_velniasprison_51_5", -1393, -85));

		AddWarpPortal(From("d_velniasprison_51_5", -334, -542), To("d_velniasprison_51_5", -98, -299));
		AddWarpPortal(From("d_velniasprison_51_5", -100, -399), To("d_velniasprison_51_5", -445, -540));

		AddWarpPortal(From("d_velniasprison_51_5", 2269, -209), To("d_velniasprison_51_5", 1293, -226));
		AddWarpPortal(From("d_velniasprison_51_5", 1287, -304), To("d_velniasprison_51_5", 2281, -110));

		AddWarpPortal(From("d_velniasprison_51_5", 1567, 242), To("d_velniasprison_51_5", -690, -844));
		AddWarpPortal(From("d_velniasprison_51_5", -662, -937), To("d_velniasprison_51_5", 1545, 171));

		AddWarpPortal(From("d_velniasprison_51_5", -1164, -552), To("d_velniasprison_51_5", -197, 1015));
		AddWarpPortal(From("d_velniasprison_51_5", -221, 942), To("d_velniasprison_51_5", -1048, -568));

		AddWarpPortal(From("d_velniasprison_51_5", 118, -638), To("d_velniasprison_51_5", -2095, 43));

		AddWarpPortal(From("d_velniasprison_51_5", 1075, -679), To("d_velniasprison_51_5", 2247, 169));
	}
}
