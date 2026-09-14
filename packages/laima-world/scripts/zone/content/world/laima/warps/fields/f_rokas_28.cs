//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Tiltas Valley
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_rokas_28WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Tiltas Valley to Fedimian
		AddWarp(48, "ROKAS28_FEDIMIAN", 45, From("f_rokas_28", 886, -515), To("c_fedimian", 810, 1060));

		// Tiltas Valley to Royal Mausoleum 3F
		AddWarp(1030, "ROKAS28_TO_ZACHARIEL34", 252, From("f_rokas_28", 1191.129, 2320.436), To("d_zachariel_34", -1685, 1152));

		// Tiltas Valley to King's Plateau
		AddWarp(1031, "ROKAS28_TO_ROKAS30", 270, From("f_rokas_28", -1895, -545), To("f_rokas_30", 970, -1045));
	}
}
