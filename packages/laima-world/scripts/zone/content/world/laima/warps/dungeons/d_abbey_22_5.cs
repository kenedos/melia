//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Narvas Temple Annex
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_abbey_22_5WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Narvas Temple Annex to Narvas Temple
		AddWarp(1, "ABBEY22_5_ABBEY22_4", 176, From("d_abbey_22_5", 28.36045, 1276.747), To("d_abbey_22_4", -269, -1218));
	}
}
