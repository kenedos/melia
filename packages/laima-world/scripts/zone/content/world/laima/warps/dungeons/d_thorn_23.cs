//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Sunset Flag Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_thorn_23WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Sunset Flag Forest to Sunset Flag Forest
		AddWarp(17, "THORN23_1_THORN23_2", 215, From("d_thorn_23", -1528, -1792), To("d_thorn_23", -1655, -1542));

		// Sunset Flag Forest to Sunset Flag Forest
		AddWarp(18, "THORN23_2_THORN23_1", 25, From("d_thorn_23", -1631, -1637), To("d_thorn_23", -1458, -1875));

		// Sunset Flag Forest to Sunset Flag Forest
		AddWarp(19, "THORN23_2_THORN23_3", 144, From("d_thorn_23", 2295, 937), To("d_thorn_23", 2670, 1029));

		// Sunset Flag Forest to Sunset Flag Forest
		AddWarp(20, "THORN23_3_THORN23_2", -83, From("d_thorn_23", 2581, 1036), To("d_thorn_23", 2206, 868));

		// Sunset Flag Forest to Dvasia Peak
		AddWarp(21, "THORN23_THORN22", 179, From("d_thorn_23", 2429, 2176), To("d_thorn_22", -968, -2125));

		// Sunset Flag Forest to Gate Route
		AddWarp(22, "THORN23_THORN19", 180, From("d_thorn_23", -591, 880), To("d_thorn_19", -188, -3929));
	}
}
