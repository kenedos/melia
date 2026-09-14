//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Alemeth Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_orchard_34_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Alemeth Forest to Dadan Jungle
		AddWarp(1, "ORCHARD_34_1_TO_ORCHARD_34_1", 45, From("f_orchard_34_1", 1160, -1072), To("f_bracken_63_3", -1308, 316));

		// Alemeth Forest to Zeraha
		AddWarp(3, "ORCHARD34_1_ORCHARD_34_2", 182, From("f_orchard_34_1", -478.7312, 1532.682), To("f_orchard_34_2", 1451, 1317));

		// Alemeth Forest to Barha Forest
		AddWarp(90, "ORCHARD34_1_ORCHARD_34_3", 90, From("f_orchard_34_1", 1035, 278), To("f_orchard_34_3", -1539, 511));
	}
}
