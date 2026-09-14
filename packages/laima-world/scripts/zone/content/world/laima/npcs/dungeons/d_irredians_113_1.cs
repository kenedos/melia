//--- Melia Script ----------------------------------------------------------
// Irredian Shelter
//--- Description -----------------------------------------------------------
// NPCs found in and around Irredian Shelter.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DIrredians1131NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(6, "WARP_D_IRREDIANS113_1", "d_irredians_113_1", -517.5229, 92.14448, -981.6158, 54);
	}
}
