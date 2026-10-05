//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Klaipeda
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class c_KlaipeWarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Klaipeda to West Siauliai Woods
		AddWarp(10004, "WS_KLAPEDA_SIAULST1", 0, From("c_Klaipe", -194.36829, -1172.699), To("f_siauliai_west", 1630, -733));

		// Klaipeda to Miner's Village
		AddWarp(10003, "WS_KLAPEDA_SIAULST3", 135, From("c_Klaipe", 799, 331), To("f_siauliai_out", 526, -2169));

		// Klaipeda to Bokor Master's Home
		AddWarp(102, "WS_KLAPEDA_BOCORS", 225, From("c_Klaipe", -976, -502), To("c_voodoo", 24, -80));

		// Klaipeda to Highlander Master's Training Hall
		AddWarp(104, "WS_KLAPEDA_HIGHLANDER", 225, From("c_Klaipe", 223, -95), To("c_highlander", 4, 173));

		// Klaipeda to Pyromancer Master's Lab
		AddWarp(105, "WS_KLAPEDA_FIREMAGE", 200, From("c_Klaipe", -2, -290), To("c_firemage", 195, 5));

		// Klaipeda to Gytis Settlement Area
		AddWarp(10015, "KLAPEDA_TO_SIAUL50_1", 154, From("c_Klaipe", 240.6537, 895.663), To("f_siauliai_50_1", 1565, -1468));

		// Klaipeda to Beauty Shop
		AddWarp(10063, "KLAPEDA_TO_BEAUTYSHOP", 180, From("c_Klaipe", -1055.639, 635.0443), To("c_barber_dress", -14, -58));
	}
}
