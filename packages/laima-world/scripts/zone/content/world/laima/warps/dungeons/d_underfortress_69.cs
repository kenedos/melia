//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Fortress Battlegrounds
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_underfortress_69WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Fortress Battlegrounds to Storage Quarter
		AddWarp(1, "UNDERFORTRESS69_UNDERFORTRESS68", 104, From("d_underfortress_69", 2245.019, 24.88506), To("d_underfortress_68", 1785, 1726));

		// Fortress Battlegrounds to Fortress Battlegrounds
		AddNpc(147501, "Teleporter", "d_underfortress_69", -1942, 54, 45, async dialog =>
		{
			dialog.Player.Warp("d_underfortress_69", -1423, 821, 101);
		});

		// Fortress Battlegrounds to Fortress Battlegrounds
		AddNpc(147501, "Teleporter", "d_underfortress_69", -1508, 84, 45, async dialog =>
		{
			dialog.Player.Warp("d_underfortress_69", -2027, 819, 29);
		});
	}
}
