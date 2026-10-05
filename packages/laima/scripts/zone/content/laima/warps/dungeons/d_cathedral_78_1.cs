//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Neighport Church East Building
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_cathedral_78_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Neighport Church East Building to Stogas Plateau
		AddWarpPortal(From("d_cathedral_78_1", 1354, 144), To("f_tableland_28_2", -381, -638));
	}
}
