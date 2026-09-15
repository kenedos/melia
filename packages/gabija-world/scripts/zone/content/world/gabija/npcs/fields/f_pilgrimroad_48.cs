//--- Melia Script ----------------------------------------------------------
// Manahas
//--- Description -----------------------------------------------------------
// NPCs found in and around Manahas.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad48NpcScript : GeneralScript
{
	protected override void Load()
	{
		// [Kedoran Merchant Alliance]{nl}  Leopoldas
		//-------------------------------------------------------------------------
		AddNpc(4, 147484, "[Kedoran Merchant Alliance]{nl}  Leopoldas", "f_pilgrimroad_48", -165.5285, 546.9031, 1501.194, 6, "PILGRIM_48_LEOPOLDAS", "", "");
		
		// [Kedoran Merchant Alliance]{nl}   Merrisa
		//-------------------------------------------------------------------------
		AddNpc(5, 152064, "[Kedoran Merchant Alliance]{nl}   Merrisa", "f_pilgrimroad_48", -174.2117, 382.695, -205.1203, 90, "PILGRIM_48_JURATE", "", "");
		
		// [Kedoran Merchant Alliance]{nl} Margellius
		//-------------------------------------------------------------------------
		AddNpc(6, 147485, "[Kedoran Merchant Alliance]{nl} Margellius", "f_pilgrimroad_48", -6.034502, 382.684, -138.5854, -34, "PILGRIM_48_MARCELIJUS", "", "");
		
		// [Kedoran Merchant Alliance]{nl}   Gerda
		//-------------------------------------------------------------------------
		AddNpc(7, 147473, "[Kedoran Merchant Alliance]{nl}   Gerda", "f_pilgrimroad_48", -53.00045, 546.9031, 1594.12, 90, "PILGRIM_48_GERDA", "", "");
		
		// [Kedoran Merchant Alliance]{nl}  Serapinas
		//-------------------------------------------------------------------------
		AddNpc(8, 147483, "[Kedoran Merchant Alliance]{nl}  Serapinas", "f_pilgrimroad_48", 5.860531, 546.9031, 1596.749, -25, "PILGRIM_48_SERAPINAS", "", "");
		
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(20, "WARP_F_PILGRIMROAD_48", "f_pilgrimroad_48", -260.2179, 382.684, 19.69579, 45);
	}
}
