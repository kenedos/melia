//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Mage Tower 3F
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_firetower_43WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Mage Tower 3F to Mage Tower 2F
		AddWarp(8, "FIRETOWER43_TO_FIRETOWER42", 65, From("d_firetower_43", -2473, -294), To("d_firetower_42", -1010, -2410));

		// Mage Tower 3F to Mage Tower 4F
		AddWarp(9, "FIRETOWER43_TO_FIRETOWER44", 90, From("d_firetower_43", 1808.94, 694.11), To("d_firetower_44", 655, -1318));
	}
}
