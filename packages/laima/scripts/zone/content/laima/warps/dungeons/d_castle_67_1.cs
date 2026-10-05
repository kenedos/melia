//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Topes Fortress 1F
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_castle_67_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Topes Fortress 1F to Delmore Outskirts
		AddWarp(1, "CASTLE671_TO_CASTLE653", 6, From("d_castle_67_1", -1634.536, -1578.95), To("f_castle_65_3", 1088, 157));

		// Topes Fortress 1F to Topes Fortress 2F
		AddWarp(3, "CASTLE_67_1_TO_CASTLE_67_2", 112, From("d_castle_67_1", 877.967, 1210.322), To("d_castle_67_2", -1779, -1298));
	}
}
