using System;
using Yggdrasil.Network.Communication;

namespace Melia.Shared.Network.Inter.Messages
{
	/// <summary>
	/// Shutdown command types.
	/// </summary>
	public enum ShutdownType
	{
		/// <summary>
		/// Immediate shutdown with brief delay for message display.
		/// </summary>
		Immediate,

		/// <summary>
		/// Graceful shutdown with countdown broadcasts.
		/// </summary>
		Graceful,

		/// <summary>
		/// Cancel a pending shutdown.
		/// </summary>
		Cancel
	}

	/// <summary>
	/// Instruction to initiate a shutdown on target servers.
	/// </summary>
	[Serializable]
	public class ShutdownMessage : ICommMessage
	{
		/// <summary>
		/// The reason for the shutdown.
		/// </summary>
		public string Reason { get; }

		/// <summary>
		/// The delay before shutdown (for immediate) or countdown duration (for graceful).
		/// </summary>
		public TimeSpan Delay { get; }

		/// <summary>
		/// The type of shutdown to perform.
		/// </summary>
		public ShutdownType Type { get; }

		/// <summary>
		/// The name of the administrator who initiated the shutdown.
		/// </summary>
		public string Initiator { get; }

		/// <summary>
		/// Creates a new shutdown message.
		/// </summary>
		/// <param name="reason">Reason for the shutdown.</param>
		/// <param name="delay">Delay/countdown duration.</param>
		/// <param name="type">Type of shutdown.</param>
		/// <param name="initiator">Name of the initiator.</param>
		public ShutdownMessage(string reason, TimeSpan delay = default, ShutdownType type = ShutdownType.Immediate, string initiator = "System")
		{
			if (delay == default)
				delay = TimeSpan.FromSeconds(5);

			this.Reason = reason ?? "maintenance";
			this.Delay = delay;
			this.Type = type;
			this.Initiator = initiator;
		}

		/// <summary>
		/// Creates an immediate shutdown message.
		/// </summary>
		public static ShutdownMessage Immediate(string reason, string initiator = "System")
			=> new ShutdownMessage(reason, TimeSpan.FromSeconds(5), ShutdownType.Immediate, initiator);

		/// <summary>
		/// Creates a graceful shutdown message with countdown.
		/// </summary>
		public static ShutdownMessage Graceful(string reason, int delayMinutes, string initiator = "System")
			=> new ShutdownMessage(reason, TimeSpan.FromMinutes(delayMinutes), ShutdownType.Graceful, initiator);

		/// <summary>
		/// Creates a cancel shutdown message.
		/// </summary>
		public static ShutdownMessage Cancel(string initiator = "System")
			=> new ShutdownMessage("cancelled", TimeSpan.Zero, ShutdownType.Cancel, initiator);
	}
}
