//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Dingofasil District
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_flash_58WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Dingofasil District to Coastal Fortress
		AddWarp(1, "FLASH_58_TO_FLASH_29_1", 3, From("f_flash_58", -159.4244, -2051.354), To("f_flash_29_1", 69, 1258));
	}
}
