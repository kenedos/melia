//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Cobalt Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_huevillage_58_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Cobalt Forest to Septyni Glen
		AddWarp(3, "HUEVILLAGE58_3_TO_HUEVILLAGE58_4", 207, From("f_huevillage_58_3", -1129.885, -29.77028), To("f_huevillage_58_4", 1306, -996));

		// Cobalt Forest to Tenant's Farm
		AddWarp(45, "HUEVILLAGE58_3_TO_FARM47_2", 6, From("f_huevillage_58_3", -223.0459, -1545.6), To("f_farm_47_1", 241.17201, 1374.7689));

		// Cobalt Forest to Vieta Gorge
		AddWarp(46, "HUEVILLAGE58_3_TO_HUEVILLAGE58_2", 0, From("f_huevillage_58_3", 386, -1774), To("f_huevillage_58_2", -877, 14));
	}
}
