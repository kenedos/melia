using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Prophecy buff, which provides immunity against
	/// all removable debuffs for a number of hits equal to skill level.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Prophecy_Buff)]
	public class Oracle_Prophecy_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
