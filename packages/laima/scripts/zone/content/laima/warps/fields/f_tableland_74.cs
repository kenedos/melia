//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Steel Heights
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_tableland_74WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Steel Heights to Kalejimas Visiting Room
		AddWarp(7, "TABLELAND74_PRISON78", 168, From("f_tableland_74", 2040.011, 2474.699), To("d_prison_78", 1045, -734));

		// Steel Heights to Sventimas Exile
		AddWarp(8, "TABLELAND74_TABLELAND72", 0, From("f_tableland_74", -2354, -1621), To("f_tableland_72", 778, 1855));
	}
}
