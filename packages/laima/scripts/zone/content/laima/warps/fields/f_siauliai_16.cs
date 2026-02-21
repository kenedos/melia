//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Lemprasa Pond
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_siauliai_16WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Lemprasa Pond to Orsha
		AddWarp(1, "SIAULIAI16_TO_ORSHA", 114, From("f_siauliai_16", 2353.054, 392.2148), To("c_orsha", -1239, 336));
	}
}
