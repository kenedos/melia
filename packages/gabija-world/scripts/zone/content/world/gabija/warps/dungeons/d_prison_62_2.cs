//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Ashaq Underground Prison 2F
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_prison_62_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Ashaq Underground Prison 2F to Ashaq Underground Prison 1F
		AddWarp(1, "PRISON622_TO_PRISON621", 180, From("d_prison_62_2", -3.470654, 1622.89), To("d_prison_62_1", -572, 260));

		// Ashaq Underground Prison 2F to Ashaq Underground Prison 3F
		AddWarp(2, "PRISON622_TO_PRISON623", 183, From("d_prison_62_2", 163.9651, -1131.809), To("d_prison_62_3", 896, -580));

		// Ashaq Underground Prison 2F to Ashaq Underground Prison 2F
		AddWarp(16, "PRISON622_TO_PRISON622_1", -11, From("d_prison_62_2", 648.3535, -1323.258), To("d_prison_62_2", 20, -1716));

		// Ashaq Underground Prison 2F to Ashaq Underground Prison 2F
		AddWarp(17, "PRISON622_1_TO_PRISON622", 90, From("d_prison_62_2", 42.7582, -1909.789), To("d_prison_62_2", 669, -1195));
	}
}
