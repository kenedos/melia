using System;
using Yggdrasil.Network.Communication;

namespace Melia.Shared.Network.Inter.Messages
{
	/// <summary>
	/// Message to send a whisper to a specific player.
	/// </summary>
	[Serializable]
	public class WhisperMessage : ICommMessage
	{
		/// <summary>
		/// Returns the sender's name.
		/// </summary>
		public string SenderName { get; }

		/// <summary>
		/// Returns the target player's team name.
		/// </summary>
		public string TargetTeamName { get; }

		/// <summary>
		/// Returns the message content.
		/// </summary>
		public string Message { get; }

		/// <summary>
		/// Creates a new whisper message.
		/// </summary>
		/// <param name="senderName">The sender's name.</param>
		/// <param name="targetTeamName">The target player's team name.</param>
		/// <param name="message">The message content.</param>
		public WhisperMessage(string senderName, string targetTeamName, string message)
		{
			this.SenderName = senderName;
			this.TargetTeamName = targetTeamName;
			this.Message = message;
		}
	}
}
