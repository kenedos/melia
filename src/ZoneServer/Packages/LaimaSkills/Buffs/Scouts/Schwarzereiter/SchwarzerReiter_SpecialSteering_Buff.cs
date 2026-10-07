using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the hidden Special Steering buff, which builds Motion
	/// while its owner moves mounted and clears it on dismount.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Schwarzereiter26_Buff)]
	public class SchwarzerReiter_SpecialSteering_BuffOverride : BuffHandler
	{
		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			if (!character.IsRiding)
			{
				character.StopBuff(BuffId.Specialmove_Buff);
				return;
			}

			if (character.Movement.IsMoving)
				character.StartBuff(BuffId.Specialmove_Buff, 1, 0, TimeSpan.Zero, character);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.StopBuff(BuffId.Specialmove_Buff);
		}
	}
}
