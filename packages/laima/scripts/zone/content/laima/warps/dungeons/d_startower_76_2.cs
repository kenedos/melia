//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Natarene Tower
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_startower_76_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Nazarene Tower to Nazarh Wathctower
		AddWarpPortal(From("d_startower_76_2", -2407, -196), To("d_startower_76_1", 2805, 222));
	}
}
