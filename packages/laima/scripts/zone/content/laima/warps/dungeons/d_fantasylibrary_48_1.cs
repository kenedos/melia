//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Fantasy Library.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFantasyLibrary481WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Sausis Room 9 para Nobreer Forest
		AddWarpPortal(From("d_fantasylibrary_48_1", -725, -280), To("f_whitetrees_21_2", -700, -700));

		// Sausis Room 9 para Sausis Room 10
		AddWarpPortal(From("d_fantasylibrary_48_1", -525, -1250), To("d_fantasylibrary_48_2", 819.0876, 832.28186f));
	}
}
