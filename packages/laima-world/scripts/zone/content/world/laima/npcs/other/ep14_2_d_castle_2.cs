//--- Melia Script ----------------------------------------------------------
// Hidden Passage
//--- Description -----------------------------------------------------------
// NPCs found in and around Hidden Passage.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class Ep142DCastle2NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(2, "WARP_EP14_2_DCASTLE_2", "ep14_2_d_castle_2", 213.0273, 0.3000107, -1052.083, 90);
		
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(3, 40110, "Statue of Goddess Zemyna", "ep14_2_d_castle_2", 793.483, 138.5508, 1271.432, 90, "EP14_2_DCASLTE2_ZEMINA", "EP14_2_DCASLTE2_ZEMINA", "EP14_2_DCASLTE2_ZEMINA");
	}
}
