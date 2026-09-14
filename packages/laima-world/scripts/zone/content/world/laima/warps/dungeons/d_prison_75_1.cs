//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Narcon Prison
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_prison_75_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Narcon Prison to Klaipeda
		AddWarp(1, "PRISON75_TO_KLAIPE", 180, From("d_prison_75_1", -505, 1665), To("f_castle_65_3", 1088, 157));
	}
}
