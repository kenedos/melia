//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Ashaq Underground Prison 1F
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_prison_62_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Ashaq Underground Prison 1F to Paupys Crossing
		AddWarp(1, "PRISON621_TO_SIAULIAI11RE", 270, From("d_prison_62_1", -573.2256, -1387.141), To("f_siauliai_11_re", 424, 1613));

		// Ashaq Underground Prison 1F to Ashaq Underground Prison 2F
		AddWarp(2, "PRISON621_TO_PRISON622", 180, From("d_prison_62_1", -563.5114, 355.3305), To("d_prison_62_2", 2, 1531));
	}
}
