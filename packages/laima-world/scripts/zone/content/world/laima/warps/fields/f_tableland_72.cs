//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Sventimas Exile
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_tableland_72WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Sventimas Exile to Kadumel Cliff
		// AddWarp(6, "TABLELAND72_TABLELAND73", 183, From("f_tableland_72", 777.5479, 2020.532), To("f_tableland_73", 1321, -1205));

		// Sventimas Exile to Grand Yard Mesa
		AddWarp(7, "TABLELAND72_TABLELAND71", 48, From("f_tableland_72", 1029.782, -1489.055), To("f_tableland_71", -797, 1233));

		// Sventimas Exile to Sentry Bailey
		AddWarp(8, "TABLELAND72_UNDERFORTRESS65", 99, From("f_tableland_72", 2131.741, 222.4766), To("d_underfortress_65", -1677, 1114));

		// Sventimas Exile to Steel Heights
		AddWarp(9, "TABLELAND72_TABLELAND74", 180, From("f_tableland_72", 772, 1980), To("f_tableland_74", -2354, -1506));
	}
}
