//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Royal Mausoleum 3F
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_zachariel_34WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Royal Mausoleum 3F to Royal Mausoleum 2F
		AddWarp(19, "ZACHARIEL34_ZACHARIEL33", 191, From("d_zachariel_34", 3568, 208), To("d_zachariel_33", -275, 1823));

		// Royal Mausoleum 3F to Royal Mausoleum 4F
		AddWarp(22, "ZACHARIEL34_3_ZACHARIEL35_3", -83, From("d_zachariel_34", -2943, 143), To("d_zachariel_35", 1610, -1376));

		// Royal Masoleum 3F to Guards Graveyard
		AddWarp(22, "ZACHARIEL34_3_CATACOMB01", 270, From("d_zachariel_34", -2023, -1093), To("id_catacomb_01", 297, 2852));

		// Royal Masoleum 3F to Tiltas Valley
		AddWarp(22, "ZACHARIEL34_3_CATACOMB01", 270, From("d_zachariel_34", -1756, 1152), To("f_rokas_28", 1276, 2278));
	}
}
