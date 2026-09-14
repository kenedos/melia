//--- Melia Script ----------------------------------------------------------
// Delmore Manor
//--- Description -----------------------------------------------------------
// NPCs found in and around Delmore Manor.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class Ep141FCastle2NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddStatPointStatue(97, "EP14_1_F_CASTLE_2_06_EV_NPC", "ep14_1_f_castle_2", -312.5249, -31.53057, 1890.051, 99);
		
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(98, "WARP_EP14_1_F_CASTLE_2", "ep14_1_f_castle_2", 2009.172, 1.219482, 989.8714, -46);
	}
}
