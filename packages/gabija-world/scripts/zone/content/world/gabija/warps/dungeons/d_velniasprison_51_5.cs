//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Demon Prison District 5
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_velniasprison_51_5WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Demon Prison District 5 to Demon Prison District 4
		AddWarp(6, "VELNIASP515_TO_VELNIASP513", 90, From("d_velniasprison_51_5", -1152, 54, -552), To("d_velniasprison_51_3", 2867, 131, -710));

		// The district's own teleport circles
		AddWarp(2, "VELNIASP_515_GROUP_2_2", 90, From("d_velniasprison_51_5", -104, 30, -423), To("d_velniasprison_51_5", -461, 6, -542));
		AddWarp(3, "VELNIASP_515_GROUP_2_1", 90, From("d_velniasprison_51_5", -461, 6, -542), To("d_velniasprison_51_5", -104, 30, -423));

		AddWarp(4, "VELNIASP_515_GROUP_1_1", 90, From("d_velniasprison_51_5", -1586, 157, 192), To("d_velniasprison_51_5", -1952, 156, 73));
		AddWarp(5, "VELNIASP_515_GROUP_1_2", 90, From("d_velniasprison_51_5", -1952, 156, 73), To("d_velniasprison_51_5", -1586, 157, 192));
	}
}
