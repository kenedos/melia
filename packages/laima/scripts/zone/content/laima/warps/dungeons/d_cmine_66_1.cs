//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Nevellet Quarry 1F
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_cmine_66_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Nevellet Quarry 1F to Dadan Jungle
		AddWarp(1, "CMINE661_TO_BRACKEN633", 2, From("d_cmine_66_1", 95.23591, -1804.916), To("f_bracken_63_3", 528, 1543));

		// Nevellet Quarry 1F to Nevellet Quarry 2F
		AddWarp(2, "CMINE661_TO_CMINE662", 156, From("d_cmine_66_1", 318.2595, 1874.544), To("d_cmine_66_2", 2186, -20));
	}
}
