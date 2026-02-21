//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Inner Enceinte District
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_flash_64WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Inner Enceinte District to Downtown
		AddWarp(1, "FLASH64_FLASH63", -9, From("f_flash_64", 12, -2596), To("f_flash_63", -224, 1342));

		// Inner Enceinte District to Sentry Bailey
		AddWarp(31, "FLASH64_UNDERFORTRESS65", 146, From("f_flash_64", -2.366955, 2338.651), To("d_underfortress_65", 611, -2229));

		// Inner Enceinte District to Ibre Plateau
		AddWarp(33, "FLASH64_TABLELAND70", 198, From("f_flash_64", -374.3372, 2044.522), To("f_tableland_70", 4504, -4430));
	}
}
