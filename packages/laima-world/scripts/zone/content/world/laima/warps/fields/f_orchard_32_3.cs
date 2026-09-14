//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Bellai Rainforest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_orchard_32_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Bellai Rainforest to Delmore Outskirts
		AddWarp(1, "ORCHARD323_TO_CASTLE653", 5, From("f_orchard_32_3", 407.6729, -1619.674), To("f_castle_65_3", -166, 1617));

		// Bellai Rainforest to Zeraha
		AddWarp(2, "ORCHARD_32_3_TO_ORCHARD_34_2", 225, From("f_orchard_32_3", 56.92821, 1703.937), To("f_orchard_34_2", 1415, 1299));
	}
}
