//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Sekta Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_pilgrimroad_41_4WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Sekta Forest to Salvia Forest
		AddWarp(2, "PILGRIM41_4_PILGRIM41_2", 183, From("f_pilgrimroad_41_4", -102.1826, 1347.569), To("f_pilgrimroad_41_2", 0, -1407));
	}
}
