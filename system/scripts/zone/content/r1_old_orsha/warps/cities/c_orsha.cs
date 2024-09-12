//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Orsha
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class c_orshaWarpsScript : GeneralScript
{
	public override void Load()
	{
		// Orsha to Lemprasa Pond
		AddWarp(113, "ORSHA_TO_SIAULIAI16", 258, From("c_orsha", -1422.59, 368.31), To("f_siauliai_16", 2248, 341));

		// Orsha to Woods of the Linked Bridges
		AddWarp(114, "ORSHA_TO_SIAULIAI15RE", 90, From("c_orsha", 1443.58, 343.89), To("f_siauliai_15_re", 3280, 62));
	}
}
