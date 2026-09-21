//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Mage Tower 2F
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_firetower_42WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Mage Tower 2F to Mage Tower 1F
		AddWarp(8, "FIRETOWER42_TO_FIRETOWER41", 90, From("d_firetower_42", 2547.03, -125.39), To("d_firetower_41", 2885, -1409));

		// Mage Tower 2F to Mage Tower 3F
		AddWarp(9, "FIRETOWER42_TO_FIRETOWER43", 95, From("d_firetower_42", -933, -2391), To("d_firetower_43", -2508, -258));
	}
}
