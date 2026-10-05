//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Fantasy Library 4.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFantasyLibrary484WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Valandis Room 3 para Valandis Room 2
		AddWarpPortal(From("d_fantasylibrary_48_4", -1210.1432f, 978.0416f), To("d_fantasylibrary_48_3", -1398.8575f, 1758.44121f));

		// Valandis Room 3 para Valandis Room 91
		AddWarpPortal(From("d_fantasylibrary_48_4", 165.17297f, 430.61171f), To("d_fantasylibrary_48_5", -2024.6901f, -1011.165351f));
	}
}
