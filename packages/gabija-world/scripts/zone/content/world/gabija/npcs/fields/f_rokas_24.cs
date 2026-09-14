//--- Melia Script ----------------------------------------------------------
// Gateway of the Great King
//--- Description -----------------------------------------------------------
// NPCs found in and around Gateway of the Great King.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas24NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(3, 40120, "Statue of Goddess Vakarine", "f_rokas_24", 913, 123, -1882, 0, "WARP_F_ROKAS_24", "STOUP_CAMP", "STOUP_CAMP");
		
		// Merchant Davio
		//-------------------------------------------------------------------------
		AddNpc(24, 20154, "Merchant Davio", "f_rokas_24", 955, 124, -1829, 0, "ROKAS24_DABIO", "", "");
	}
}
