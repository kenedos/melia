//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Seir Rainforest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_orchard_32_4WarpsScript : GeneralScript
{
	public override void Load()
	{
		// Seir Rainforest to Zeraha
		AddWarp(1, "ORCHARD_32_4_TO_ORCHARD_34_2", 110, From("f_orchard_32_4", 1889.321, 500.1698), To("f_orchard_34_2", -1647, 1674));

		// Seir Rainforest to Fedimian
		AddWarp(2, "ORCHARD_32_4_TO_FEDIMIAN", 0, From("f_orchard_32_4", -22.81654, -742.8499), To("c_fedimian", -835, 63));

		// Seir Rainforest to Sicarius 1F
		AddWarp(3, "ORCHARD_32_4_TO_UNDERFOREST_68_1", 170, From("f_orchard_32_4", 1561.419, 1706.481), To("d_underfortress_68_1", -1541, 125));
	}
}
