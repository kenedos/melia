using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.HandlersOverrides.Wizards.Chronomancer
{
	[Package("laima")]
	[BuffHandler(BuffId.Stop_Debuff)]
	public class Stop_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.AddState(StateType.Held, buff.Duration);
			buff.Target.Lock(LockType.Attack, buff.Duration);
			buff.Target.Lock(LockType.GetHit, buff.Duration);
			buff.Target.SetSafeState(true);
		}

		public override void OnExtend(Buff buff)
		{
			buff.Target.AddState(StateType.Held, buff.Duration);
			buff.Target.Lock(LockType.Attack, buff.Duration);
			buff.Target.Lock(LockType.GetHit, buff.Duration);
			buff.Target.SetSafeState(true);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Held);
			buff.Target.Unlock(LockType.Attack);
			buff.Target.Unlock(LockType.GetHit);
			buff.Target.SetSafeState(false);
		}
	}
}
