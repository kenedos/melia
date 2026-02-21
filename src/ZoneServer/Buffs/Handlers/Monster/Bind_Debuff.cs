using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Trap, Cannot move..
	/// </summary>
	[BuffHandler(BuffId.Bind_Debuff)]
	public class Bind_Debuff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			Send.ZC_NORMAL.SetActorColor(target, 100, 100, 100, 255, 0f, 1);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			Send.ZC_NORMAL.SetActorColor(target, 255, 255, 255, 255, 0f, 1);
		}
	}
}
