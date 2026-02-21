using System;
using Yggdrasil.Network.Communication;

namespace Melia.Shared.Network.Inter.Messages
{
	/// <summary>
	/// Guild update types.
	/// </summary>
	public enum GuildUpdateType
	{
		/// <summary>
		/// A new guild was created.
		/// </summary>
		Created,

		/// <summary>
		/// A guild was disbanded.
		/// </summary>
		Disbanded,

		/// <summary>
		/// A member joined the guild.
		/// </summary>
		MemberJoined,

		/// <summary>
		/// A member left the guild.
		/// </summary>
		MemberLeft,

		/// <summary>
		/// Guild data was updated (name, notice, etc.).
		/// </summary>
		Updated,
	}

	/// <summary>
	/// Contains information about a guild update event.
	/// </summary>
	[Serializable]
	public class GuildUpdateMessage : ICommMessage
	{
		/// <summary>
		/// Returns the type of update.
		/// </summary>
		public GuildUpdateType UpdateType { get; set; }

		/// <summary>
		/// Returns the guild's database id.
		/// </summary>
		public long GuildId { get; set; }

		/// <summary>
		/// Returns the character's database id (for member join/leave events).
		/// </summary>
		public long CharacterId { get; set; }

		/// <summary>
		/// Returns the account's database id (for member join/leave events).
		/// </summary>
		public long AccountId { get; set; }

		/// <summary>
		/// Creates a new guild update message.
		/// </summary>
		/// <param name="updateType">The type of update.</param>
		/// <param name="guildId">The guild's database id.</param>
		/// <param name="characterId">The character's database id (optional).</param>
		/// <param name="accountId">The account's database id (optional).</param>
		public GuildUpdateMessage(GuildUpdateType updateType, long guildId, long characterId = 0, long accountId = 0)
		{
			this.UpdateType = updateType;
			this.GuildId = guildId;
			this.CharacterId = characterId;
			this.AccountId = accountId;
		}
	}
}
