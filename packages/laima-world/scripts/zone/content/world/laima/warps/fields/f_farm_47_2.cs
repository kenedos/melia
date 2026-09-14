//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Aqueduct Bridge Area
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_farm_47_2WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Aqueduct Bridge Area to Myrkiti Farm
		AddWarp(1, "FARM_47_2_TO_FARM_47_3", 112, From("f_farm_47_2", 2559.683, -1202.66), To("f_farm_47_3", -2021, 427));

		// Aqueduct Bridge Area to Tenants' Farm
		AddWarp(2, "FARM_47_2_TO_FARM_47_1", 92, From("f_farm_47_2", 1888.028, 1061.506), To("f_farm_47_1", -1477, -51));

		// Aqueduct Bridge Area to Greene Manor
		AddWarp(3, "FARM_47_2_TO_FARM_49_1", -87, From("f_farm_47_2", -1805.981, 977.8495), To("f_farm_49_1", 2082, 806));

		// Aqueduct Bridge Area to Demon Prison District 1
		AddWarpPortal(From("f_farm_47_2", -1616, -1188), To("d_velniasprison_51_1", 20, 1653));
	}
}
