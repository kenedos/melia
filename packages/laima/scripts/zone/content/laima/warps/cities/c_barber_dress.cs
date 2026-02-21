//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Beauty Shop
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class c_barber_dressWarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Beauty Shop to Klaipeda
		AddWarp(3, "BEAUTYSHOP_TO_KLAPEDA", 0, From("c_barber_dress", -5.580346, -111.731), To("c_Klaipe", -1011, 588));
	}
}
