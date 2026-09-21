//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Demon Prison District 1
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_velniasprison_51_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Demon Prison District 1 to Aqueduct Bridge Area
		AddWarp(9, "VELNIASP511_TO_FARM472", 90, From("d_velniasprison_51_1", -118, 167, 108), To("f_farm_47_2", -1660, 0.34, -1230));

		// Demon Prison District 1 to Demon Prison District 2
		AddWarp(10, "VELNIASP511_TO_VELNIASP512", 90, From("d_velniasprison_51_1", 182, 170, -117), To("d_velniasprison_51_2", 1123, 297, 1840));

		// The district's own teleport circles
		AddWarp(2, "VELNIASP_511_GROUP_2_1", 90, From("d_velniasprison_51_1", -623, 223, -11), To("d_velniasprison_51_1", -970, 345, 123));
		AddWarp(3, "VELNIASP_511_GROUP_2_2", 90, From("d_velniasprison_51_1", -970, 345, 123), To("d_velniasprison_51_1", -623, 223, -11));

		AddWarp(4, "VELNIASP_511_GROUP_1_1", 90, From("d_velniasprison_51_1", -38, 223, 580), To("d_velniasprison_51_1", 72, 260, 901));
		AddWarp(5, "VELNIASP_511_GROUP_1_2", 90, From("d_velniasprison_51_1", 72, 260, 901), To("d_velniasprison_51_1", -38, 223, 580));

		AddWarp(6, "VELNIASP_511_GROUP_3_1", 90, From("d_velniasprison_51_1", 672, 224, -2), To("d_velniasprison_51_1", 908, 347, -3));
		AddWarp(7, "VELNIASP_511_GROUP_3_2", 90, From("d_velniasprison_51_1", 908, 347, -3), To("d_velniasprison_51_1", 672, 224, -2));
	}
}
