//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Mage Tower 4F
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_firetower_44WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Mage Tower 4F to Mage Tower 3F
		AddWarp(7, "FIRETOWER44_TO_FIRETOWER43", 90, From("d_firetower_44", 584.49, -1318.89), To("d_firetower_43", 1740, 694));

		// Mage Tower 4F to Mage Tower 5F
		AddWarp(8, "FIRETOWER44_TO_FIRETOWER45", 90, From("d_firetower_44", -2718.14, 62.12), To("d_firetower_45", -1215, -2010));
	}
}
