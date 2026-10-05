//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Fantasy Library 3.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFantasyLibrary483WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Valandis Room 2 para Sausis Room 10
		AddWarpPortal(From("d_fantasylibrary_48_3", -916.7413f, -627.95654f), To("d_fantasylibrary_48_2", 62.70913f, -100f));

		// Valandis Room 2 para Valandis Room 3
		AddWarpPortal(From("d_fantasylibrary_48_3", -1398.8575f, 1758.44121f), To("d_fantasylibrary_48_4", -1210.1432f, 978.0416f));
	}
}
