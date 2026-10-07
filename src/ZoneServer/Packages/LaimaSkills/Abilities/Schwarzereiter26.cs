using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Schwarzer Reiter: Special Steering ability, which builds Motion
	/// while riding.
	/// </summary>
	[Package("laima-skills")]
	[AbilityHandler(AbilityId.Schwarzereiter26)]
	public class Schwarzereiter26Override : IAbilityPropertyHandler
	{
		public void OnActivate(Ability ability, Character character)
		{
			character.StartBuff(BuffId.Schwarzereiter26_Buff, TimeSpan.Zero);
		}

		public void OnDeactivate(Ability ability, Character character)
		{
			character.StopBuff(BuffId.Schwarzereiter26_Buff);
		}
	}
}
