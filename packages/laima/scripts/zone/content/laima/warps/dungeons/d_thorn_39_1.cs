//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Viltis Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_thorn_39_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Viltis Forest to Glade Hillroad
		AddWarp(1, "THORN391_TO_THORN392", 156, From("d_thorn_39_1", -744.733, 1666.92), To("d_thorn_39_2", -2057, -1512));
	}
}
