//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Dvasia Peak
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_thorn_22WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Dvasia Peak to Dvasia Peak
		AddWarp(3, "THORN22_1_THORN22_2", 183, From("d_thorn_22", -1020, -891), To("d_thorn_22", -1296, -163));

		// Dvasia Peak to Dvasia Peak
		AddWarp(4, "THORN22_2_THORN22_1", 23, From("d_thorn_22", -1216.202, -386.6535), To("d_thorn_22", -987, -1142));

		// Dvasia Peak to Dvasia Peak
		AddWarp(5, "THORN22_2_THORN22_3", 162, From("d_thorn_22", -1035, 664), To("d_thorn_22", -453, 903));

		// Dvasia Peak to Dvasia Peak
		AddWarp(6, "THORN22_3_THORN22_2", -30, From("d_thorn_22", -689, 846), To("d_thorn_22", -1172, 452));

		// Dvasia Peak to Gytis Settlement Area
		AddWarp(7, "THORN22_SIAUL50_1", 180, From("d_thorn_22", 994, 1096), To("f_siauliai_50_1", -1692, -282));

		// Dvasia Peak to Sunset Flag Forest
		AddWarp(8, "THORN22_THORN23", 0, From("d_thorn_22", -983, -2228), To("d_thorn_23", 2402, 2067));

		// Dvasia Peak to Gate Route
		AddWarp(9, "THORN22_THORN19", 270, From("d_thorn_22", -2273, -1497), To("d_thorn_19", 1337, -3255));
	}
}
