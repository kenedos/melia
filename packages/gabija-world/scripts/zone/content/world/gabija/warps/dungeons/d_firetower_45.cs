//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Mage Tower 5F
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_firetower_45WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Mage Tower 5F to Mage Tower 4F
		AddWarp(10, "FIRETOWER45_TO_FIRETOWER44", 90, From("d_firetower_45", -1215.60, -2087.14), To("d_firetower_44", -2645, 62));
	}
}
