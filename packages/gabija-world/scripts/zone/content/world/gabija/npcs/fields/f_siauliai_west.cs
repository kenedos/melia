//--- Melia Script ----------------------------------------------------------
// West Siauliai Woods
//--- Description -----------------------------------------------------------
// NPCs found in and around West Siauliai Woods.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliaiWestNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(4, "WARP_F_SIAULIAI_WEST", "f_siauliai_west", -525, 260, -435, 0);
		
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(2026, 20026, "Statue of Goddess Zemyna", "f_siauliai_west", 1705.19, 285.05, 390.19, 90, "", "SIAUL_WEST_LAIMONAS3_TRIGGER", "");

		// Camp Guards
		//-------------------------------------------------------------------------
		AddNpc(10020, L("Camp Guard"), "SIAUL_WEST_CAMP_GUARD_1", "f_siauliai_west", -589, -822, 70, async dialog =>
		{
			dialog.SetTitle(L("Camp Guard"));

			await dialog.Msg(L("Nobody gets past this line without the knight's word. That's all there is to it."));
		});

		AddNpc(10020, L("Camp Guard"), "SIAUL_WEST_CAMP_GUARD_2", "f_siauliai_west", -509, -822, 288, async dialog =>
		{
			dialog.SetTitle(L("Camp Guard"));

			await dialog.Msg(L("Keep to the road and keep your voice down. We've had enough noise out of those woods."));
		});
	}
}
