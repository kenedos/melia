using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Handler for the Summon Familiar buff (sorcerer_bat).
	/// Manages the lifecycle of summoned familiar bats.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.sorcerer_bat)]
	public class sorcerer_batOverride : BuffHandler
	{
		private const string BatListKey = "Melia.SorcererBatList";

		/// <summary>
		/// Called when the buff is activated.
		/// The bats are already created in the skill handler, so we just
		/// need to store references for cleanup.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="activationType"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			// Get familiars that were just summoned by the skill handler
			var familiars = character.Summons.GetSummons(s => s.Id == (int)MonsterId.Familiar);

			// Store the handles for tracking
			var batHandles = familiars.Select(f => f.Handle).ToList();
			buff.Vars.Set(BatListKey, batHandles);
		}

		/// <summary>
		/// Called periodically while the buff is active.
		/// Checks if any bats are still alive.
		/// </summary>
		/// <param name="buff"></param>
		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			// Check if any bats still exist
			var familiars = character.Summons.GetSummons(s => s.Id == (int)MonsterId.Familiar);

			// If no bats remain, end the buff early
			if (familiars.Count == 0)
			{
				character.StopBuff(BuffId.sorcerer_bat);
			}
		}

		/// <summary>
		/// Called when the buff ends (either naturally or forcibly).
		/// Kills all remaining bats.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			// Get all remaining familiars and kill them
			var familiars = character.Summons.GetSummons(s => s.Id == (int)MonsterId.Familiar);

			foreach (var familiar in familiars)
			{
				// Kill the familiar - this will trigger the Summon_Died event
				// which handles removal from the summon list
				familiar.Kill(character);
			}

			// Clean up the stored list
			buff.Vars.Remove(BatListKey);
		}
	}
}
