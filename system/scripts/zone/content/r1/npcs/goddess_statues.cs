//--- Melia Script ----------------------------------------------------------
// GoddessStatues
//--- Description -----------------------------------------------------------
// Goddess Statues NPCs around the world.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class GoddessStatuesNpcScript : GeneralScript
{
	public override void Load()
	{
		// Klaipeda
		//-------------------------------------------------------------------------
		AddNpc(10017, 154039, "Statue of Goddess Ausrine", "c_Klaipe", -206.574, 148.8251, 98.63973, 45, "WARP_C_KLAIPE", "STOUP_CAMP", "STOUP_CAMP");
	}
}
