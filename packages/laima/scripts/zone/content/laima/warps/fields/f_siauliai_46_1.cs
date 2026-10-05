//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Spring Light Woods
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_siauliai_46_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Spring Light Woods to Gate Route
		AddWarp(1, "SIAULIAI46_1_THORN19", 70, From("f_siauliai_46_1", -624, -1920), To("d_thorn_19", 274, 3055));

		// Spring Light Woods to Shaton Farm
		AddWarp(2, "SIAULIAI46_1_FARM49_2", 180, From("f_siauliai_46_1", 580, 1316), To("f_farm_49_2", 721, -2021));
	}
}
