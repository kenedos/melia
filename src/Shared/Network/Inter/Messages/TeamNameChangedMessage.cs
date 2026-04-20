using System;
using Yggdrasil.Network.Communication;

namespace Melia.Shared.Network.Inter.Messages
{
	/// <summary>
	/// Broadcast to notify servers that an account's team name was changed,
	/// so they can update cached copies without requiring a restart.
	/// </summary>
	[Serializable]
	public class TeamNameChangedMessage : ICommMessage
	{
		/// <summary>
		/// Returns the affected account's id.
		/// </summary>
		public long AccountId { get; }

		/// <summary>
		/// Returns the team name prior to the change.
		/// </summary>
		public string OldTeamName { get; }

		/// <summary>
		/// Returns the new team name.
		/// </summary>
		public string NewTeamName { get; }

		/// <summary>
		/// Creates new message.
		/// </summary>
		/// <param name="accountId"></param>
		/// <param name="oldTeamName"></param>
		/// <param name="newTeamName"></param>
		public TeamNameChangedMessage(long accountId, string oldTeamName, string newTeamName)
		{
			this.AccountId = accountId;
			this.OldTeamName = oldTeamName;
			this.NewTeamName = newTeamName;
		}
	}
}
