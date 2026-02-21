//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Mesafasla
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_tableland_28_1WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Mesafasla to Stogas Plateau
		AddWarp(3, "TABLELAND281_TO_TABLELAND282", 32, From("f_tableland_28_1", -2888.828, 711.914), To("f_tableland_28_2", 1660, 1760));

		// Mesafasla to Vedas Plateau
		AddWarp(1, "TABLELAND28_1_TABLELAND11_1", -11, From("f_tableland_28_1", 549.7599, -861.6209), To("f_tableland_11_1", -2238, 2192));
	}
}
