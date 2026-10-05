//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Fantasy Library 5.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFantasyLibrary485WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Valandis Room 91 para Valandis Room 3
		AddWarpPortal(From("d_fantasylibrary_48_5", -2024.6901f, -1011.165351f), To("d_fantasylibrary_48_4", 165.17297f, 430.61171f));
	}
}
