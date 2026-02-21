using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Archers.Sapper
{
	/// <summary>
	/// Handler for the PunjiStake Debuff, which prevents movement.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.PunjiStake_Debuff)]
	public class PunjiStake_DebuffOverride : BuffHandler
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
