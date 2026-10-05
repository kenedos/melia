//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in King's Plateau
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_rokas_30WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// King's Plateau to Tiltas Valley
		AddWarp(59, "ROKAS30_ROKAS30", 45, From("f_rokas_30", 1019, -1097), To("f_rokas_28", -1809, -567));

		// King's Plateau to Gateway of the Great King
		AddWarp(60, "ROKAS30_ROKAS31", 263, From("f_rokas_30", -1773, -116), To("f_rokas_24", 1553, -1033));

		// King's Plateau to Mage's Tower 1F - Requires Quest 1004 completion (handled in quest file)
	}
}
