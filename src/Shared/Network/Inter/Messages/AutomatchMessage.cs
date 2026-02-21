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
	/// Instruction related to dungeon/content auto match for a single player.
	/// </summary>
	[Serializable]
	public class AutoMatchMessage : ICommMessage
	{
		/// <summary>
		/// Gets or sets if the player is joining (true) or exiting the queue (false).
		/// </summary>
		public bool IsJoiningQueue { get; set; }

		/// <summary>
		/// Gets or sets the character's database id.
		/// </summary>
		public long CharacterDbId { get; set; }

		/// <summary>
		/// Gets or sets the account's object id.
		/// </summary>
		public long AccountId { get; set; }

		/// <summary>
		/// Gets or sets the dungeon's id (ClassID).
		/// </summary>
		public int DungeonId { get; set; }

		public AutoMatchMessage() { }

		/// <summary>
		/// Creates new message.
		/// </summary>
		/// <param name="characterDbId"></param>
		/// <param name="accountDbId"></param>
		/// <param name="dungeonId"></param>
		/// <param name="isJoiningQueue"></param>
		public AutoMatchMessage(long characterDbId, long accountDbId, int dungeonId, bool isJoiningQueue)
		{
			this.CharacterDbId = characterDbId;
			this.DungeonId = dungeonId;
			this.IsJoiningQueue = isJoiningQueue;
			this.AccountId = accountDbId;
		}
	}

	/// <summary>
	/// Instruction related to dungeon/content auto match for a party.
	/// The entire party is queued as an atomic unit.
	/// </summary>
	[Serializable]
	public class AutoMatchPartyMessage : ICommMessage
	{
		/// <summary>
		/// Gets or sets if the party is joining (true) or exiting the queue (false).
		/// </summary>
		public bool IsJoiningQueue { get; set; }

		/// <summary>
		/// Gets or sets the leader's character database id.
		/// </summary>
		public long LeaderCharacterDbId { get; set; }

		/// <summary>
		/// Gets or sets all party member character database ids (including leader).
		/// </summary>
		public List<long> PartyMemberDbIds { get; set; }

		/// <summary>
		/// Gets or sets all party member account ids (matching order of PartyMemberDbIds).
		/// </summary>
		public List<long> PartyMemberAccountIds { get; set; }

		/// <summary>
		/// Gets or sets the dungeon's id (ClassID).
		/// </summary>
		public int DungeonId { get; set; }

		public AutoMatchPartyMessage() { }

		/// <summary>
		/// Creates new message.
		/// </summary>
		public AutoMatchPartyMessage(long leaderCharacterDbId, List<long> partyMemberDbIds, List<long> partyMemberAccountIds, int dungeonId, bool isJoiningQueue)
		{
			this.LeaderCharacterDbId = leaderCharacterDbId;
			this.PartyMemberDbIds = partyMemberDbIds;
			this.PartyMemberAccountIds = partyMemberAccountIds;
			this.DungeonId = dungeonId;
			this.IsJoiningQueue = isJoiningQueue;
		}
	}

	/// <summary>
	/// Instruction that will handle the player ready state on auto match (Match 2 to 4 Plaeyrs option)
	/// </summary>
	[Serializable]
	public class AutoMatchReadyMessage : ICommMessage
	{
		/// <summary>
		/// Gets or sets the character's database id.
		/// </summary>
		public long CharacterDbId { get; set; }

		public AutoMatchReadyMessage() { }

		/// <summary>
		/// Creates new message.
		/// </summary>
		/// <param name="characterDbId"></param>
		public AutoMatchReadyMessage(long characterDbId)
		{
			this.CharacterDbId = characterDbId;
		}
	}

	/// <summary>
	/// Represents a single member in the auto match queue for UI display.
	/// </summary>
	[Serializable]
	public class AutoMatchMemberInfo
	{
		/// <summary>
		/// Gets or sets the account's object id.
		/// </summary>
		public long AccountId { get; set; }

		/// <summary>
		/// Gets or sets the character's object id.
		/// </summary>
		public long CharacterId { get; set; }

		/// <summary>
		/// Gets or sets the character's database id.
		/// </summary>
		public long CharacterDbId { get; set; }

		/// <summary>
		/// Gets or sets the character's job id.
		/// </summary>
		public int JobId { get; set; }

		/// <summary>
		/// Gets or sets the character's level.
		/// </summary>
		public int Level { get; set; }

		/// <summary>
		/// Gets or sets whether this member is ready (Match 2-4 Players option).
		/// </summary>
		public bool IsReady { get; set; }

		public AutoMatchMemberInfo() { }

		public AutoMatchMemberInfo(long accountId, long characterId, long characterDbId, int jobId, int level, bool isReady)
		{
			this.AccountId = accountId;
			this.CharacterId = characterId;
			this.CharacterDbId = characterDbId;
			this.JobId = jobId;
			this.Level = level;
			this.IsReady = isReady;
		}
	}

	/// <summary>
	/// Broadcasts the current auto match queue members to all zone servers.
	/// ZoneServers will send DungeonAutoMatchPartyCount to appropriate clients.
	/// </summary>
	[Serializable]
	public class AutoMatchMembersUpdateMessage : ICommMessage
	{
		/// <summary>
		/// Gets or sets the dungeon's id (ClassID).
		/// </summary>
		public int DungeonId { get; set; }

		/// <summary>
		/// Gets or sets the list of all members currently in the queue.
		/// </summary>
		public List<AutoMatchMemberInfo> Members { get; set; }

		public AutoMatchMembersUpdateMessage()
		{
			this.Members = new List<AutoMatchMemberInfo>();
		}

		public AutoMatchMembersUpdateMessage(int dungeonId, List<AutoMatchMemberInfo> members)
		{
			this.DungeonId = dungeonId;
			this.Members = members ?? new List<AutoMatchMemberInfo>();
		}
	}

	/// <summary>
	/// A request for the total player count.
	/// </summary>
	[Serializable]
	public class InitAutoMatchContentMessage : ICommMessage
	{
		/// <summary>
		/// Returns the list of character's database id.
		/// </summary>
		public List<long> CharacterDbIds { get; set; }

		/// <summary>
		/// Returns the dungeon Id that for the auto match content,
		/// This will be used to retrieve the map.
		/// </summary>
		public int DungeonId { get; set; }

		/// <summary>
		/// Returns the unique id for auto match session.
		/// </summary>
		public long AutoMatchId { get; set; }
		
		/// <summary>
		/// Blank constructor.
		/// </summary>
		public InitAutoMatchContentMessage() { }

		/// <summary>
		/// Creates a new initialize auto match content message.
		/// </summary>
		/// <param name="characterDbIds"></param>
		/// <param name="targetZoneServerId"></param>
		public InitAutoMatchContentMessage(List<long> characterDbIds, int dungeonId, long autoMatchId)
		{
			this.CharacterDbIds = characterDbIds;
			this.DungeonId = dungeonId;
			this.AutoMatchId = autoMatchId;
		}
	}
}
