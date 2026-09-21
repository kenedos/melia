//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Mage Tower 1F
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_firetower_41WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Mage Tower 1F to Fedimian Suburbs
		AddWarp(20, "FIRETOWER41_TO_REMAINS40", 90, From("d_firetower_41", -2301.33, -1410.27), To("f_remains_40", 3470, 2726));

		// Mage Tower 1F to Mage Tower 2F
		AddWarp(21, "FIRETOWER41_TO_FIRETOWER42", 90, From("d_firetower_41", 2956.43, -1409.30), To("d_firetower_42", 2480, -125));
	}
}
