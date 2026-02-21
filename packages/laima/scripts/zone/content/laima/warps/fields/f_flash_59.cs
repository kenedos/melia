//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Verkti Square
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_flash_59WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Verkti Square to Roxona Market
		AddWarp(4, "FLASH59_FLASH60", 181, From("f_flash_59", 157, 765), To("f_flash_60", 1142, -123));

		// Verkti Square to Ruklys Street
		AddWarp(5, "FLASH59_FLASH61", 184, From("f_flash_59", 1372.925, 1083.849), To("f_flash_61", -893, -233));

		// Verkti Square to Downtown
		AddWarp(6, "FLASH59_FLASH63", 90, From("f_flash_59", 1425.89, 635.2833), To("f_flash_63", -466, -2211));

		// Verkti Square to Seir Rainforest
		AddWarp(6, "FLASH59_FLASH63", 270, From("f_flash_59", -1729, -299), To("f_orchard_32_4", 1530, 1623));
	}
}
