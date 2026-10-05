//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Poslinkis Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_katyn_13WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Poslinkis Forest to Arrow Path
		AddWarp(1, "KATYN13_KATYN13_3", 90, From("f_katyn_13", 1743, 467), To("f_katyn_13_3", -1787, -342));

		// Poslinkis Forest to Owl Burial Ground
		AddWarp(2, "KATYN13_KATYN7_2", 90, From("f_katyn_13", 72, -2129), To("f_katyn_7_2", 363, 1689));
	}
}
