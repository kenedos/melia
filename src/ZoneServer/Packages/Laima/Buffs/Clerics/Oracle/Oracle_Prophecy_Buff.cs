using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Prophecy buff, which makes the target immune to
	/// removable debuffs. The immunity itself is applied by the buff
	/// component's debuff resistance check.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Prophecy_Buff)]
	public class Oracle_Prophecy_BuffOverride : BuffHandler
	{
	}
}
