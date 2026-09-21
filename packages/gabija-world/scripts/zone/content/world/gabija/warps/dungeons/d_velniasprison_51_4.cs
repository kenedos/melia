//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Demon Prison District 3
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_velniasprison_51_4WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Demon Prison District 3 to Demon Prison District 2
		AddWarp(4, "VELNIASP514_TO_VELNIASP512", 90, From("d_velniasprison_51_4", -1080, 604, 1619), To("d_velniasprison_51_2", 1458, 183, 380));

		// Demon Prison District 3 to Demon Prison District 4
		AddWarp(5, "VELNIASP514_TO_VELNIASP513", 90, From("d_velniasprison_51_4", -460, 580, 1262), To("d_velniasprison_51_3", -3280, 161, -807));

		// The district's own teleport circles
		AddWarp(6, "VELNIASP_514_GROUP_1_1", 90, From("d_velniasprison_51_4", -1637, 574, 945), To("d_velniasprison_51_4", -1742, 444, 710));
		AddWarp(7, "VELNIASP_514_GROUP_1_2", 90, From("d_velniasprison_51_4", -1742, 444, 710), To("d_velniasprison_51_4", -1637, 574, 945));

		AddWarp(8, "VELNIASP_514_GROUP_2_1", 90, From("d_velniasprison_51_4", -858, 588, 569), To("d_velniasprison_51_4", -684.79, 334.60, 570.14));
		AddWarp(9, "VELNIASP_514_GROUP_2_2", 90, From("d_velniasprison_51_4", -684.79, 334.60, 570.14), To("d_velniasprison_51_4", -858, 588, 569));
	}
}
