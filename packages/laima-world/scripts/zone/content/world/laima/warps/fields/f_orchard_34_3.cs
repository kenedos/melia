//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Barha Forest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class f_orchard_34_3WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Barha Forest to Alemeth Forest
		AddWarp(32, "ORCHARD34_3_ORCHARD34_1", 270, From("f_orchard_34_3", -1649.754, 523.5295), To("f_orchard_34_1", 938, 280));

		// Barha Forest to Penitence Route
		// AddWarp(33, "ORCHARD34_3_PILGRIM55", 200, From("f_orchard_34_3", -639.8013, 1358.713), To("f_pilgrimroad_55", 1685, -634));

		// Barha Forest to Barha Forest
		AddNpc(147501, "Teleporter", "f_orchard_34_3", -1299, -1110, 45, async dialog =>
		{
			dialog.Player.Warp("f_orchard_34_3", -971, 10, -2165);
		});

		// Barha Forest to Barha Forest
		AddNpc(147501, "Teleporter", "f_orchard_34_3", -998, -2186, 45, async dialog =>
		{
			dialog.Player.Warp("f_orchard_34_3", -1309, 71, -1149);
		});
	}
}
