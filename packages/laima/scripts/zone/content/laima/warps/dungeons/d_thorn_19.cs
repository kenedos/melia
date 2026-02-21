//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Gate Route
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_thorn_19WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Gate Route to Spring Light Woods
		AddWarp(1, "THORN19_SIAULIAI46_1", 225, From("d_thorn_19", 215, 3115), To("f_siauliai_46_1", -689, -1872));

		// Gate Route to Sunset Flag Forest
		AddWarp(2, "THORN19_THORN23", 270, From("d_thorn_19", -271, -3912), To("d_thorn_23", -578, 772));

		// Gate Route to Dvasia Peak
		AddWarp(3, "THORN19_THORN22", 180, From("d_thorn_19", 1470, -3213), To("d_thorn_22", -2155, -1404));

		// Gate Route to Kvailas Forest
		AddWarp(4, "THORN19_THORN21", 270, From("d_thorn_19", 1983, -1007), To("d_thorn_21", -281, -44));
	}
}
