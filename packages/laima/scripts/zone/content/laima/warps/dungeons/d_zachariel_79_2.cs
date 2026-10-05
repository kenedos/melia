//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Netanmalek Mausoleum
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_zachariel_79_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Netanmalek Mausoleum to Sjarejo Chamber
		AddWarpPortal(From("d_zachariel_79_2", 40, -2800), To("d_zachariel_79_1", 559, 690));
	}
}
