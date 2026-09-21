//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Demon Prison District 4
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_velniasprison_51_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Demon Prison District 4 to Demon Prison District 3
		AddWarp(4, "VELNIASP513_TO_VELNIASP514", 90, From("d_velniasprison_51_3", -3317, 161, -807), To("d_velniasprison_51_4", -460, 580, 1220));

		// Demon Prison District 4 to Demon Prison District 5
		AddWarp(5, "VELNIASP513_TO_VELNIASP515", 90, From("d_velniasprison_51_3", 2867, 131, -751), To("d_velniasprison_51_5", -1152, 54, -510));
	}
}
