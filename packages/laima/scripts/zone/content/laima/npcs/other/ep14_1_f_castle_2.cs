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
		AddNpc(97, 40110, "Statue of Goddess Zemyna", "ep14_1_f_castle_2", -312.5249, -31.53057, 1890.051, 99, "EP14_1_F_CASTLE_2_06_EV_NPC", "EP14_1_F_CASTLE_2_06_EV_NPC", "EP14_1_F_CASTLE_2_06_EV_NPC");
		
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(98, 40120, "Statue of Goddess Vakarine", "ep14_1_f_castle_2", 2009.172, 1.219482, 989.8714, -46, "WARP_EP14_1_F_CASTLE_2", "STOUP_CAMP", "STOUP_CAMP");
	}
}
