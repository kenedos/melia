//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Fedimian
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class c_fedimianWarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Fedimian to Rokas Ridge
		AddWarp(11, "FEDMIAN_TO_ROKAS25", 80, From("c_fedimian", 782, -160), To("f_rokas_25", -2307, -1166));

		// Fedimian to Zachariel Crossroads
		AddWarp(116, "FEDMIAN_ROKAS31", -77, From("c_fedimian", -1000.59, 31.61), To("f_rokas_31", 885, -1059));

		// Fedimian to Fedimian Public House
		AddWarp(111, "FEDIMIAN_REQUEST1", 180, From("c_fedimian", -844, -100), To("c_request_1", 122, -127));

		// Fedimian to Tiltas Valley
		AddWarp(133, "FEDIMIAN_TO_ROKAS28", 180, From("c_fedimian", 831, 1133), To("f_rokas_28", 786, -457));
	}
}
