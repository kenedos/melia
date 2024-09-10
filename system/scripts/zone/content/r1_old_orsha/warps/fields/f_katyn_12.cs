//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Letas Stream
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_katyn_12WarpsScript : GeneralScript
{
	public override void Load()
	{
		// Letas Stream to Karolis Springs
		AddWarp(2, "KATYN_12_KATYN_10", 0, From("f_katyn_12", 91.85896, -1597.516), To("f_katyn_10", -2072, 507));

		// Letas Stream to Pelke Shrine Ruins
		AddWarp(3, "KATYN_12_3CMLAKE_83", 115, From("f_katyn_12", 2241.357, 1140.637), To("f_3cmlake_83", 811, -31));

		// Letas Stream to Gateway of the Great King
		AddWarp(4, "KATYN_12_ROKAS_24", 180, From("f_katyn_12", -1489.632, 2365.628), To("f_rokas_24", 1556, -1028));
	}
}
