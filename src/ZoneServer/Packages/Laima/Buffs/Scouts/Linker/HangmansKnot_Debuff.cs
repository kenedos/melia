using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scouts.Linker
{
	/// <summary>
	/// Handler for the Hangman's Knot debuff. Holds the target in place
	/// after being gathered, preventing movement for the debuff duration.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.HangmansKnot_Debuff)]
	public class HangmansKnot_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.AddState(StateType.Held);
			buff.Target.Lock(LockType.Attack);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Held);
			buff.Target.Unlock(LockType.Attack);
		}
	}
}
