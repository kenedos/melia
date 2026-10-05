//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Orsha
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class c_orshaWarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Orsha to Syla Forest
		AddWarp(113, "ORSHA_TO_WHITETREES23_3", 0, From("c_orsha", 176, -1388), To("f_whitetrees_23_3", -565, 1455));

		// Orsha to Woods of the Linked Bridges
		AddWarp(114, "ORSHA_TO_SIAULIAI15_RE", 258, From("c_orsha", -1422.59, 368.31), To("f_siauliai_15_re", 3333, 98));

		// Orsha to Nobreer Forest
		AddWarp(115, "ORSHA_TO_WHITETREES21_2", 90, From("c_orsha", 1443.58, 343.89), To("f_whitetrees_21_2", -1312, 456));

		// Orsha to Izoliacjia Plateau
		AddWarp(116, "ORSHA_TO_WHITETREES22_3", 0, From("c_orsha", -1367.0168, -758.8992), To("f_whitetrees_22_3", 1708.2073, 1055.3889));
	}
}
