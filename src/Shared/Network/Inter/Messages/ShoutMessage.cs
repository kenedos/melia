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
	/// Instruction to broadcast a shout to all users.
	/// </summary>
	[Serializable]
	public class ShoutMessage : ICommMessage
	{
		/// <summary>
		/// Returns the team name of the user.
		/// </summary>
		public string TeamName { get; }

		/// <summary>
		/// Returns the message text.
		/// </summary>
		public string Text { get; }
		
		/// <summary>
		/// Returns the message server type.
		/// </summary>
		public string ServerType { get; }

		/// <summary>
		/// Creates new message.
		/// </summary>
		/// <param name="teamName"></param>
		/// <param name="text"></param>
		/// <param name="serverType"></param>
		public ShoutMessage(string teamName, string text, string serverType = "GLOBAL")
		{
			this.TeamName = teamName;
			this.Text = text;
			this.ServerType = serverType;
		}
	}
}
