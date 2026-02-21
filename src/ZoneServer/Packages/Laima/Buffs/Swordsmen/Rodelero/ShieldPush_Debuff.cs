using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Rodelero
{
	/// <summary>
	/// Handler for ShieldPush_Debuff (Unbalance).
	/// Prevents the target from moving but does not lock attacks.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.ShieldPush_Debuff)]
	public class ShieldPush_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.AddState(StateType.Held);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Held);
		}
	}
}
