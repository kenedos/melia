//--- Melia Script ----------------------------------------------------------
// Spring Light Woods
//--- Description -----------------------------------------------------------
// NPCs found in and around Spring Light Woods.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai461NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Merchant Dulke
		//-------------------------------------------------------------------------
		AddNpc(11, 20102, "Merchant Dulke", "f_siauliai_46_1", -1776.323, 261.9619, -1027.201, 91, "SIAULIAI_46_1_SQ_03_NPC", "", "");
		
		// Merchant Item Parcel
		//-------------------------------------------------------------------------
		AddNpc(14, 47160, "Merchant Item Parcel", "f_siauliai_46_1", -646.7023, 261.9619, -1296.51, 90, "SIAULIAI_46_1_SQ_03_BAG01", "", "");
		
		// Merchant Item Parcel
		//-------------------------------------------------------------------------
		AddNpc(15, 47160, "Merchant Item Parcel", "f_siauliai_46_1", -280.2346, 261.9619, -369.4779, 90, "SIAULIAI_46_1_SQ_03_BAG02", "", "");
		
		// Merchant Item Parcel
		//-------------------------------------------------------------------------
		AddNpc(16, 47161, "Merchant Item Parcel", "f_siauliai_46_1", 945.7625, 258.7639, -168.5284, 90, "SIAULIAI_46_1_SQ_03_BAG03", "", "");
	}
}
