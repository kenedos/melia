using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Bolas Hold Debuff, which prevents movement.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Bolas_Hold_Debuff)]
	public class Bolas_Hold_DebuffOverride : BuffHandler
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
