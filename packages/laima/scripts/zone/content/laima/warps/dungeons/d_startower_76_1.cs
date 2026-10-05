//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Natarh Watchtower
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_startower_76_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Natarh Watchtower to Dina Bee Farm
		AddWarpPortal(From("d_startower_76_1", -1097, 722), To("f_siauliai_46_4", 1365, -328));

		// Natarh Watchtower to Nazarene Tower
		AddWarpPortal(From("d_startower_76_1", 2765, 222), To("d_startower_76_2", -2387, -196));
	}
}
