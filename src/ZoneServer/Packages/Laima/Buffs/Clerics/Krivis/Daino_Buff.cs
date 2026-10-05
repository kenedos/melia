using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Daino, increases healing received.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Daino_Buff)]
	public class Daino_BuffOverride : BuffHandler
	{
	}
}
