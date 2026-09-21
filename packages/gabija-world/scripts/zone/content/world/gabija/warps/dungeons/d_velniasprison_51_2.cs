//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Demon Prison District 2
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_velniasprison_51_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Demon Prison District 2 to Demon Prison District 1
		AddWarp(9, "VELNIASP512_TO_VELNIASP511", 90, From("d_velniasprison_51_2", 1123, 297, 1884), To("d_velniasprison_51_1", 182, 170, -160));

		// Demon Prison District 2 to Demon Prison District 3
		AddWarp(10, "VELNIASP512_TO_VELNIASP513", 90, From("d_velniasprison_51_2", 1458, 183, 429), To("d_velniasprison_51_4", -1080, 604, 1570));

		// The district's own teleport circles
		AddWarp(3, "VELNIASP_512_GROUP_3_2", 90, From("d_velniasprison_51_2", 1105, 361, -998), To("d_velniasprison_51_2", 1187, 255, -175));
		AddWarp(4, "VELNIASP_512_GROUP_3_1", 90, From("d_velniasprison_51_2", 1187, 255, -175), To("d_velniasprison_51_2", 1105, 361, -998));

		AddWarp(5, "VELNIASP_512_GROUP_2_2", 90, From("d_velniasprison_51_2", 46, 255, 448), To("d_velniasprison_51_2", 421, 255, 441));
		AddWarp(6, "VELNIASP_512_GROUP_2_1", 90, From("d_velniasprison_51_2", 421, 255, 441), To("d_velniasprison_51_2", 46, 255, 448));

		AddWarp(7, "VELNIASP_512_GROUP_1_1", 90, From("d_velniasprison_51_2", 1056, 254, 990), To("d_velniasprison_51_2", 1102, 297, 1550));
		AddWarp(8, "VELNIASP_512_GROUP_1_2", 90, From("d_velniasprison_51_2", 1102, 297, 1550), To("d_velniasprison_51_2", 1056, 254, 990));
	}
}
