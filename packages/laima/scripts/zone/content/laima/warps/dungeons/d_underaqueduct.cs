//--- Melia Script ----------------------------------------------------------
// Warps
//--- Description -----------------------------------------------------------
// Sets up warps in Outer Wall Sewers
//---------------------------------------------------------------------------
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

namespace ScriptsZoneLaima.content.laima.warps.dungeons
{
	internal class d_underaqueduct : GeneralScript
	{
		protected override void Load()
		{
			// Outer Wall Sewers to Inner Enceinte District
			AddWarpPortal(From("d_underaqueduct", -159, 650), To("f_flash_64", 60, -1596));
		}
	}
}
