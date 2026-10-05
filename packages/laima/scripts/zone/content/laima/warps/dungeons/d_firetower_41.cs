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
		// Mage Tower 1F to King's Plateau
		AddWarpPortal(From("d_firetower_41", -2493, -1427), To("f_rokas_30", -1349, -768));

		// Mage Tower 1F to Mage Tower 2F
		AddWarpPortal(From("d_firetower_41", 2961, -1407), To("d_firetower_42", 2550, -462));
	}
}
