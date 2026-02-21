//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in West Siauliai Woods
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_siauliai_westWarpsScript : GeneralScript
{
	protected override void Load()
	{
		// West Siauliai Woods to Klaipeda
		AddWarp(28, "WS_SIAULST1_KLAPEDA", 68, From("f_siauliai_west", 1691, -755), To("c_Klaipe", -181, -1123));

		// West Siauliai Woods to West Siauliai Woods
		AddWarp(2031, "TO_SIAULIAI_WEST", -24, From("f_siauliai_west", 2755.275, 443.1412), To("f_siauliai_west", 1412, -362));
	}
}
