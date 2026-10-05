using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Archers.Wugushi
{
	/// <summary>
	/// Handler for the Wide Miasma stealth buff.
	/// Hides caster from monsters. Breaks when dealing damage
	/// (handled in calc_combat.cs via Cloaking tag).
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.WideMiasma_Buff)]
	public class WideMiasma_BuffOverride : BuffHandler
	{
	}
}
