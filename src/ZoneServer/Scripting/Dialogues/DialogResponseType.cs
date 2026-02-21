namespace Melia.Zone.Scripting.Dialogues
{
	/// <summary>
	/// Defines the type of response the dialog is expecting from the client.
	/// Used to prevent stale packets from being processed incorrectly.
	/// </summary>
	public enum DialogResponseType
	{
		/// <summary>
		/// Not expecting any specific response.
		/// </summary>
		None,

		/// <summary>
		/// Expecting CZ_DIALOG_ACK (acknowledgment to continue dialog).
		/// </summary>
		Ack,

		/// <summary>
		/// Expecting CZ_DIALOG_SELECT (selection from options).
		/// </summary>
		Select,

		/// <summary>
		/// Expecting CZ_DIALOG_STRINGINPUT (text input).
		/// </summary>
		StringInput,
	}
}
