//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Sicarius 1F
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_underfortress_68_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Sicarius 1F to Seir Rainforest
		AddWarp(23, "UNDERFOREST_68_1_TO_ORCHARD_32_4", 266, From("d_underfortress_68_1", -1650.531, 116.4954), To("f_orchard_32_4", 1521, 1578));

		// Sicarius 1F to Sicarius 2F
		AddWarp(10, "UNDERFORTRESS_68_1_TO_UNDERFORTRESS_68_2", 90, From("d_underfortress_68_1", 1650.488, 117.6324), To("d_underfortress_68_2", -989, -88));
	}
}
