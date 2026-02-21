using System;
using Yggdrasil.Network.Communication;

namespace Melia.Shared.Network.Inter.Messages
{
	/// <summary>
	/// Message to request script reload on zone servers.
	/// </summary>
	[Serializable]
	public class ReloadScriptsMessage : ICommMessage
	{
		/// <summary>
		/// Returns the name of who initiated the reload.
		/// </summary>
		public string InitiatorName { get; }

		/// <summary>
		/// Creates a new reload scripts message.
		/// </summary>
		/// <param name="initiatorName">Name of who initiated the reload.</param>
		public ReloadScriptsMessage(string initiatorName = "Admin")
		{
			this.InitiatorName = initiatorName;
		}
	}
}
