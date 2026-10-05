//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Demon Prison District 2
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Zone.Scripting;
using Melia.Zone.World.Maps;
using static Melia.Zone.Scripting.Shortcuts;

public class DVelniasprison512WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Demon Prison District 2 to Demon Prison District 1
		AddWarpPortal(From("d_velniasprison_51_2", 1129, 1904), To("d_velniasprison_51_1", -4, -1647));

		AddWarpPortal(From("d_velniasprison_51_2", 1119, 1557), To("d_velniasprison_51_2", 1081, 855));
		AddWarpPortal(From("d_velniasprison_51_2", 1081, 930), To("d_velniasprison_51_2", 1116, 1638));

		AddWarpPortal(From("d_velniasprison_51_2", 485, 435), To("d_velniasprison_51_2", -115, 458));
		AddWarpPortal(From("d_velniasprison_51_2", -36, 454), To("d_velniasprison_51_2", 619, 424));

		AddWarpPortal(From("d_velniasprison_51_2", 1054, -92), To("d_velniasprison_51_2", 1088, -1149));
		AddWarpPortal(From("d_velniasprison_51_2", 1110, -1066), To("d_velniasprison_51_2", 1054, -8));


		// Demon Prison District 2 to Demon Prison District 3
		AddWarpPortal(From("d_velniasprison_51_2", 1459, 427), To("d_velniasprison_51_4", -1064, 1441));
	}
}
