using System.Collections.Generic;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World
{
	public enum AutoMatchState
	{
		/// <summary>
		/// The group is in the queue, waiting for players or a match.
		/// </summary>
		Queued,
		/// <summary>
		/// The match has been found and the group is in the dungeon.
		/// </summary>
		InProgress,
		/// <summary>
		/// The dungeon is complete and the group is being cleaned up.
		/// </summary>
		Finished
	}

	/// <summary>
	/// Represents a self-contained auto-match group session.
	/// It manages the party and its state throughout the auto-match lifecycle.
	/// </summary>
	public class AutoMatchSession
	{
		public long Id { get; }
		public int DungeonId { get; }
		public Party Party { get; }
		public AutoMatchState State { get; set; }

		public AutoMatchSession(long id, int dungeonId, Party party)
		{
			Id = id;
			DungeonId = dungeonId;
			Party = party;
			State = AutoMatchState.Queued;
		}

		/// <summary>
		/// Adds a character to this auto-match session and its underlying party.
		/// </summary>
		public void AddMember(Character character)
		{
			Party.AddMember(character);
			character.Variables.Perm.SetLong(AutoMatchZoneManager.SessionIdVarName, Id);
		}

		/// <summary>
		/// Removes a character from this auto-match session and its underlying party.
		/// </summary>
		public void RemoveMember(Character character)
		{
			this.Party.RemoveMember(character);
			character.Variables.Perm.SetLong(AutoMatchZoneManager.SessionIdVarName, 0);
		}
	}
}
