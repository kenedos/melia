using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Event_Penalty buff, which blinds the character if they jump.
	/// </summary>
	[BuffHandler(BuffId.Event_Penalty)]
	public class Event_Penalty : BuffHandler
	{
		private static readonly TimeSpan BlindDuration = TimeSpan.FromSeconds(5);

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is Character character && character.IsJumping())
			{
				character.StartBuff(BuffId.Blind, 1, 0, BlindDuration, character);
			}
		}
	}
}
