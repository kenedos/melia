//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Seir Rainforest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_orchard_32_4WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Seir Rainforest to Zeraha
		AddWarp(1, "ORCHARD_32_4_TO_ORCHARD_34_2", 110, From("f_orchard_32_4", 1889.321, 500.1698), To("f_orchard_34_2", -1647, 1674));

		// Seir Rainforest to Gateway of the Great King
		AddWarp(2, "ORCHARD_32_4_TO_FEDIMIAN", 0, From("f_orchard_32_4", -22.81654, -742.8499), To("f_rokas_24", 901, 1163));

		// Seir Rainforest to Verkti Square
		AddWarp(3, "ORCHARD_32_4_TO_F_FLASH_59", 175, From("f_orchard_32_4", 1554, 1708), To("f_flash_59", -1636, -297));

		// Seir Rainforest to Stele Road
		AddWarp(4, "ORCHARD_32_4_REMAINS37", 270, From("f_orchard_32_4", -1380, -189), To("f_remains_37", 1162, -2617));
	}
}
