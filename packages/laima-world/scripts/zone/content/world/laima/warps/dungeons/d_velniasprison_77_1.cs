//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Demon Prison District 5
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Zone.Scripting;
using Melia.Zone.World.Maps;
using static Melia.Zone.Scripting.Shortcuts;

public class DVelniasprison771WarpsScript : GeneralScript
{
	protected override void Load()
	{
		// Tatenye Prison to Ruklys Street
		AddWarpPortal(From("d_velniasprison_77_1", -802, -1042), To("f_flash_61", -576, 1414));
	}
}
