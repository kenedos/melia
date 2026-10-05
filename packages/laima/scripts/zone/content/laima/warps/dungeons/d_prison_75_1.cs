//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Narcon Prison
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_prison_75_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Narcon Prison to Gytis Settlement Area
		AddWarpPortal(From("d_prison_75_1", -505, 1665), To("f_siauliai_50_1", 41, -1900));
	}
}
