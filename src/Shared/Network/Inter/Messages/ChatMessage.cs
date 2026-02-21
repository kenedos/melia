using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Yggdrasil.Network.Communication;

namespace Melia.Shared.Network.Inter.Messages
{
	/// <summary>
	/// Instruction to broadcast a notice text to all players.
	/// </summary>
	[Serializable]
	public class ChatMessage : ICommMessage
	{
		/// <summary>
		/// Returns the sender to broadcast.
		/// </summary>
		public string Sender { get; }
		/// <summary>
		/// Returns the sender's account id to broadcast.
		/// </summary>
		public long AccountId { get; }
		/// <summary>
		/// Returns the text to broadcast.
		/// </summary>
		public string Text { get; }
		public string ServerType { get; }

		/// <summary>
		/// Creates a new shout message.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="accountId"></param>
		/// <param name="text"></param>
		/// <param name="serverType"></param>
		public ChatMessage(string sender, long accountId, string text, string serverType = "GLOBAL")
		{
			this.Sender = sender;
			this.AccountId = accountId;
			this.Text = text;
			this.ServerType = serverType;
		}
	}
}
