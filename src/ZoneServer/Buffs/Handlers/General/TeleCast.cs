using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the TeleCast buff, which plays a return animation
	/// when it expires, unless it has been overbuffed.
	/// </summary>
	[BuffHandler(BuffId.TeleCast)]
	public class TeleCast : BuffHandler
	{
		public override void OnEnd(Buff buff)
		{
			// The original script checks if `over <= 1`. The `over` parameter
			// represents the stack count. If the buff has 1 stack (or less),
			// it means it hasn't been overbuffed, so the animation should play.
			if (buff.OverbuffCounter <= 1 && buff.Target is Character character)
			{
				character.PlayAnimation("SKL_TELEKINESIS_RETURN", false, 1, 0);
			}
		}
	}
}
