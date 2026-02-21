using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Elite Boss Buff, An enormous monster, like the leader of the group..
	/// </summary>
	[BuffHandler(BuffId.Elite_Boss_Buff)]
	public class Elite_Boss_Buff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
