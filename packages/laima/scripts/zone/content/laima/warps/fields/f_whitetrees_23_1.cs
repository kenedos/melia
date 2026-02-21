//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Emmet Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_whitetrees_23_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Emmet Forest to Pystis Forest
		AddWarp(2, "WHITETREES23_1_TO_MAPLE23_2", -52, From("f_whitetrees_23_1", -1023.053, -1223.455), To("f_maple_23_2", 1444, 267));

		// Emmet Forest to Nobreer Forest
		AddWarp(3, "WHITETREES23_1_TO_WHITETREES21_2", 270, From("f_whitetrees_23_1", -1690.3729, -154.08543), To("f_whitetrees_21_2", 987.7971, -1207.4673));
	}
}
