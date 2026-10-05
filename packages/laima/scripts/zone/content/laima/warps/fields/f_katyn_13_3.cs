//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Arrow Path
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_katyn_13_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Arrow Path to Poslinkis Forest
		AddWarp(1, "KATYN13_3_KATYN13", 270, From("f_katyn_13_3", -1913, -348), To("f_katyn_13", 1623, 467));

		// Arrow Path to Letas Stream
		AddWarp(2, "KATYN13_3_KATYN12", 180, From("f_katyn_13_3", 112, 534), To("f_katyn_12", 87, -1473));
	}
}
