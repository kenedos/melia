//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Fantasy Library 2.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFantasyLibrary482WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Sausis Room 10 para Sausis Room 9
		AddWarpPortal(From("d_fantasylibrary_48_2", 819.0876, 882.28186f), To("d_fantasylibrary_48_1", -525, -1250));

		// Sausis Room 10 para Valandis Room 2
		AddWarpPortal(From("d_fantasylibrary_48_2", 62.70913f, -100f), To("d_fantasylibrary_48_3", -916.7413f, -627.95654f));
	}
}
