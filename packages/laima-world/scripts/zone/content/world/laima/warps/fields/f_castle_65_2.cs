//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Delmore Manor
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_castle_65_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Delmore Manor to Delmore Hamlet
		AddWarp(1, "CASTLE652_TO_CASTLE651", 89, From("f_castle_65_2", 2234.639, -155.3014), To("f_castle_65_1", -1973, 813));

		// Delmore Manor to Delmore Outskirts
		AddWarp(2, "CASTLE652_TO_CASTLE653", 268, From("f_castle_65_2", -1341.433, 169.3732), To("f_castle_65_3", -1782, -1436));

		// Delmore Manor to Feretory Hills
		AddWarp(3, "CASTLE652_TO_PILGRIM311", 179, From("f_castle_65_2", 1128.304, 2440.078), To("f_pilgrimroad_31_1", -1118, -858));
	}
}
