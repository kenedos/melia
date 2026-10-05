//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Southern Parias Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_maple_24_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Southern Parias Forest to Central Parias Forest
		AddWarp(3, "F_MAPLE_242_TO_F_MAPLE_241", 225, From("f_maple_24_2", -1356.7867, -514.83765), To("f_maple_24_1", 1726, -155));

		// Southern Parias Forest to Pystis Forest
		AddWarp(4, "F_MAPLE_242_TO_F_MAPLE_232", 225, From("f_maple_24_2", 1375.7166, 1268.7068), To("f_maple_23_2", -991.898, -1622.7117));
	}
}
