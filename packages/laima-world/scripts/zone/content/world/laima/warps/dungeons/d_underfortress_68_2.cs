//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Sicarius 2F
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_underfortress_68_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Sicarius 2F to Sicarius 1F
		AddWarp(8, "UNDERFORTRESS_68_2_TO_UNDERFORTRESS_68_1", 266, From("d_underfortress_68_2", -1111.805, -88.86646), To("d_underfortress_68_1", 1474, 111));
	}
}
