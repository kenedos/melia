//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Sjarejo Chamber
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_zachariel_79_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Sjarejo Chamber to Wood of Linked Bridges
		AddWarpPortal(From("d_zachariel_79_1", 150, -2100), To("f_siauliai_15_re", -407, 2255));

		// Sjarejo Chamber to Netanmalek Mausoleum
		AddWarpPortal(From("d_zachariel_79_1", 559, 650), To("d_zachariel_79_2", 40, -2750));
	}
}
