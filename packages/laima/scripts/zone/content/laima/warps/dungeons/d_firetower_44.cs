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
		AddWarpPortal(From("d_firetower_44", 1804, 695), To("d_firetower_43", 1804, 582));

		// Mage Tower 4F to Mage Tower 5F
		AddWarpPortal(From("d_firetower_44", -2720, 72), To("d_firetower_45", -1232, -1969));
	}
}
