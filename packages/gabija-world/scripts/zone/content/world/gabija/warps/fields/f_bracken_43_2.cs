//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Phamer Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_bracken_43_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Phamer Forest to Zeteor Coast
		AddWarp(1, "BRACKEN432_CORAL441", 155, From("f_bracken_43_2", 1428.865, 1285.8), To("f_coral_44_1", 0, 0));

		// Phamer Forest to Ghibulinas Forest
		AddWarp(2, "BRACKEN432_BRACKEN433", 106, From("f_bracken_43_2", 1499.943, -490.5141), To("f_bracken_43_3", 317, 1586));

		// Phamer Forest to Arcus Forest
		AddWarp(3, "BRACKEN432_BRACKEN431", 2, From("f_bracken_43_2", -448.0723, -1270.588), To("f_bracken_43_1", 916, 1548));
	}
}
