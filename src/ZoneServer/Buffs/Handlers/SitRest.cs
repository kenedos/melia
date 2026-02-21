using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Rest, Increased recovery of HP, SP and STA..
	/// </summary>
	[BuffHandler(BuffId.SitRest)]
	public class SitRest : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is Character character)
				character.SetSitting(true);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is Character character)
				character.SetSitting(false);
		}
	}
}
