//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Escanciu Village
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_remains_39WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Escanciu Village to Goddess' Ancient Garden
		AddWarp(19, "REMAINS39_REMAINS38", 45, From("f_remains_39", -479, -656), To("f_remains_38", 1440, 1630));

		// Escanciu Village to Roxona Market
		AddWarp(20, "REMAINS39_FLASH60", 90, From("f_remains_39", 1264, -322), To("f_flash_60", -1351, -1103));
	}
}
