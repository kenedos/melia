using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Scouts.Squire
{
	/// <summary>
	/// Handler for the Arrest debuff, which binds the target in place.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Arrest)]
	public class Squire_Arrest_BuffOverride : BuffHandler
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
