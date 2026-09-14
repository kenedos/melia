//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Crystal Mine 3F
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_cmine_6WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Crystal Mine 3F to Crystal Mine 2F
		AddWarp(1, "WS_ACT4_3_ACT4_2", 252, From("d_cmine_6", -2147, -1590), To("d_cmine_02", 1911, 1516));

		// Crystal Mine 3F to Crystal Mine 3F
		AddWarp(2, "WS_ACT4_3_ACT4_BOSS", 115, From("d_cmine_6", 170.2973, -161.5383), To("d_cmine_6", 1747, 2110));

		// Crystal Mine 3F to Crystal Mine 3F
		AddWarp(3, "WS_ACT4_BOSS_ACT4_3", -67, From("d_cmine_6", 1625, 2069), To("d_cmine_6", 58, -210));

		// Crystal Mine 3F to Crystal Mine Lot 2 - 1F
		AddWarp(3, "WS_ACT4_BOSS_ACT4_3", 45, From("d_cmine_6", 1590.2147, 1294.9232), To("d_cmine_8", -2905.0688, -302.97736));
	}
}
