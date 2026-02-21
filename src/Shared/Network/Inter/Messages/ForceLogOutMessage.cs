using System;
using Yggdrasil.Network.Communication;

namespace Melia.Shared.Network.Inter.Messages
{
	/// <summary>
	/// Instruction to log out/disconnect a player.
	/// </summary>
	[Serializable]
	public class ForceLogOutMessage : ICommMessage
	{
		/// <summary>
		/// Returns the id of the account to log out.
		/// </summary>
		public long AccountId { get; }

		/// <summary>
		/// Creates new message.
		/// </summary>
		/// <param name="accountId"></param>
		public ForceLogOutMessage(long accountId)
		{
			this.AccountId = accountId;
		}
	}

	/// <summary>
	/// Instruction to log out/disconnect a player.
	/// </summary>
	[Serializable]
	public class ForceLogOutEveryoneMessage : ICommMessage
	{
		/// <summary>
		/// Returns the reason for everyone being logged out.
		/// </summary>
		public string Reason { get; }

		/// <summary>
		/// Creates new message.
		/// </summary>
		/// <param name="reason"></param>
		public ForceLogOutEveryoneMessage(string reason)
		{
			this.Reason = reason;
		}
	}
}
