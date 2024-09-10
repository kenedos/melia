//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Karolis Springs
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_katyn_10WarpsScript : GeneralScript
{
	public override void Load()
	{
		// Karolis Springs to Dadan Jungle
		AddWarp(2, "KATYN_10_BRACKEN_63_3", 90, From("f_katyn_10", 4560.204, -1508.193), To("f_bracken_63_3", -1351, 408));

		// Karolis Springs to Letas Stream
		AddWarp(3, "KATYN_10_KATYN_12", 270, From("f_katyn_10", -2196.182, 467.24), To("f_katyn_12", 39, -1429));
	}
}
