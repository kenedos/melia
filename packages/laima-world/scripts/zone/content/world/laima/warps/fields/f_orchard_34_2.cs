//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Zeraha
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_orchard_34_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Zeraha to Alemeth Forest
		AddWarp(8, "ORCHARD_34_2_TO_ORCHARD_32_1", 90, From("f_orchard_34_2", 1533.507, 1335.476), To("f_orchard_34_1", -475, 1444));

		// Zeraha to Seir Rainforest
		AddWarp(9, "ORCHARD_34_2_TO_ORCHARD_32_4", 225, From("f_orchard_34_2", -1676.843, 1774.691), To("f_orchard_32_4", 1791, 465));
	}
}
