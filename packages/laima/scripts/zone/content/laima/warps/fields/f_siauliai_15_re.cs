//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Woods of the Linked Bridges
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_siauliai_15_reWarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Woods of the Linked Bridges to Orsha
		AddWarp(1, "SIAULIAI15RE_TO_ORSHA", 90, From("f_siauliai_15_re", 3388.76, 100.2132), To("c_orsha", -1357, 353));

		// Woods of the Linked Bridges to Paupys Crossing
		AddWarp(2, "SIAULIAI15RE_TO_SIAULIAI11RE", 266, From("f_siauliai_15_re", -3510.328, -91.57547), To("f_siauliai_11_re", 2584, -593));
	}
}
