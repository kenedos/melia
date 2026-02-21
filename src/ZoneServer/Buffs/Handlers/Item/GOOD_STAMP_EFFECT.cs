using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for GOOD_STAMP_EFFECT, which changes the target's surface type.
	/// </summary>
	[BuffHandler(BuffId.GOOD_STAMP_EFFECT)]
	public class GOOD_STAMP_EFFECT : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			// Assuming there's a method to override surface type, possibly on the actor or a component.
			// target.OverrideSurfaceType("good_stamp");
			target.PlayEffect("F_pc_welldone_ground_A", 1.5f);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;
			// target.OverrideSurfaceType("None");
		}
	}
}
