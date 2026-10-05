//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Seir Rainforest
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class d_limestonecave_73_1Script : GeneralScript
{
	protected override void Load()
	{
		// Hunting Ground Tavorh Cave to Seir Rainforest
		AddWarpPortal(From("d_limestonecave_73_1", -700, -700), To("f_orchard_32_4", -1615, -773));
	}
}
