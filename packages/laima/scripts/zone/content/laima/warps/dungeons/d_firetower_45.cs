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
		AddWarpPortal(From("d_firetower_45", -1223, -2061), To("d_firetower_44", -2648, 69));
	}
}
