using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Common
{
	/// <summary>
	/// Handler for the Common_Hold debuff, which prevents movement.
	/// </summary>
	[BuffHandler(BuffId.Common_Hold)]
	public class Common_Hold : BuffHandler
	{
		public override void OnExtend(Buff buff)
		{
			buff.Target.AddState(StateType.Held, buff.Duration);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Held);
		}
	}
}
