//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Northern Parias Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_maple_24_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Northern Parias Forest to Central Parias Forest
		AddWarp(5, "F_MAPLE_243_TO_F_MAPLE_241", 86, From("f_maple_24_3", 1004.863, 12.73128), To("f_maple_24_1", -1450, 653));
	}
}
