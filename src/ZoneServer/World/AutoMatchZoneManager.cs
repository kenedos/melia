using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Network.Inter.Messages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Logging;
using Yggdrasil.Network.Communication;

namespace Melia.Zone.World
{
	/// <summary>
	/// Manages the lifecycle of Auto Match sessions for the zone server.
	/// This service is responsible for creating parties, managing session state,
	/// and cleaning up resources after a match is complete.
	/// </summary>
	public class AutoMatchZoneManager
	{
		#region Constants

		/// <summary>Variable name for storing the auto-match session ID.</summary>
		public const string SessionIdVarName = "Melia.AutoMatch.SessionId";

		/// <summary>Variable name for storing the account ID (for disconnect cleanup).</summary>
		public const string AccountIdVarName = "Melia.AutoMatch.AccountId";

		/// <summary>Variable name for storing the active matchmaking dungeon ID.</summary>
		public const string DungeonIdVarName = "Melia.AutoMatch.DungeonId";

		/// <summary>Variable name for storing the player count in auto-match.</summary>
		public const string PlayersCountVarName = "Melia.AutoMatch.PlayersCount";

		#endregion

		private readonly object _lock = new object();

		// The single source of truth for all active auto-match sessions.
		private readonly Dictionary<long, AutoMatchSession> _sessionsById = new Dictionary<long, AutoMatchSession>();

		// A reverse-lookup dictionary for high-performance lookups of a character's session.
		private readonly Dictionary<long, AutoMatchSession> _sessionByCharacterId = new Dictionary<long, AutoMatchSession>();

		/// <summary>
		/// Creates a formal session for an existing party and marks its state as Queued.
		/// This is the replacement for the old 'AddPartyToQueue' method.
		/// </summary>
		public AutoMatchSession QueuePartySession(Party party, int dungeonId)
		{
			lock (_lock)
			{
				if (IsPartyInQueue(party))
					return GetSessionForCharacter(party.GetLeader());

				var leader = party.GetLeader();
				if (leader == null)
				{
					Log.Warning("QueuePartySession: leader is null");
					return null;
				}

				// Use the party's unique ObjectId as the Session ID for local queue management.
				// This ensures a party can't be in the queue more than once.
				long sessionId = party.ObjectId;
				var session = new AutoMatchSession(sessionId, dungeonId, party);

				_sessionsById[sessionId] = session;
				var members = party.GetPartyMembers();

				foreach (var member in members)
				{
					if (member == null) continue;
					_sessionByCharacterId[member.DbId] = session;
					member.Variables.Perm.SetLong(SessionIdVarName, sessionId);

					// Store account ID for use when character disconnects (needed for cancel message)
					if (member.Connection?.Account != null)
					{
						member.Variables.Temp.SetLong(AccountIdVarName, member.Connection.Account.ObjectId);
					}
				}

				return session;
			}
		}

		/// <summary>
		/// Removes a party's session from the queue.
		/// This is the replacement for 'RemovePartyFromQueue'.
		/// Note: This does NOT delete the party itself, only the queue session.
		/// </summary>
		public void DequeueParty(Party party)
		{
			lock (_lock)
			{
				var leader = party.GetLeader();
				if (leader == null) return;

				// Find the session using the leader and destroy it without deleting the underlying party.
				if (_sessionByCharacterId.TryGetValue(leader.DbId, out var session) && session.State == AutoMatchState.Queued)
				{
					// Clean up all members' references to this session.
					foreach (var member in party.GetPartyMembers())
					{
						if (member == null) continue;
						member.Variables.Perm.SetLong(SessionIdVarName, 0);
						member.Variables.Temp.SetLong(AccountIdVarName, 0);
						_sessionByCharacterId.Remove(member.DbId);
					}
					_sessionsById.Remove(session.Id);
				}
			}
		}

		/// <summary>
		/// Removes a single character from the auto-match queue.
		/// Called when a character disconnects to clean up their queue state.
		/// Sends a cancel message to the Coordinator so they're removed from the central queue.
		/// </summary>
		/// <param name="character">The character to remove from the queue.</param>
		public void RemoveCharacterFromQueue(Character character)
		{
			if (character == null)
				return;

			lock (_lock)
			{
				// Check if the character is in a queue session
				if (!_sessionByCharacterId.TryGetValue(character.DbId, out var session))
					return;

				// Get the stored account ID (stored when joining queue)
				var accountId = character.Variables.Temp.GetLong(AccountIdVarName);

				// Send cancel message to Coordinator
				if (accountId != 0)
				{
					var commMessage = new AutoMatchMessage(character.DbId, accountId, 0, false);
					ZoneServer.Instance.Communicator.Send("Coordinator", commMessage.BroadcastTo("Chat"));
				}
				else
				{
					Log.Warning("RemoveCharacterFromQueue: Character '{0}' in queue but no stored account ID.", character.Name);
				}

				// Clear local state
				character.Variables.Perm.SetLong(SessionIdVarName, 0);
				character.Variables.Temp.SetLong(AccountIdVarName, 0);
				character.Variables.Perm.SetInt(DungeonIdVarName, 0);
				_sessionByCharacterId.Remove(character.DbId);

				// If there are no remaining members in the session, remove the session entirely
				var remainingMembers = _sessionByCharacterId.Count(kvp => kvp.Value == session);
				if (remainingMembers == 0)
				{
					_sessionsById.Remove(session.Id);
					Log.Info("AutoMatchZoneManager: Session '{0}' removed as all members have left.", session.Id);
				}
			}
		}

		/// <summary>
		/// Checks if a character is currently in the auto-match queue.
		/// </summary>
		public bool IsCharacterInQueue(Character character)
		{
			if (character == null)
				return false;

			lock (_lock)
			{
				return _sessionByCharacterId.ContainsKey(character.DbId);
			}
		}

		/// <summary>
		/// Checks if a party has an active session in the queue.
		/// This is the replacement for 'IsPartyInQueue'.
		/// </summary>
		public bool IsPartyInQueue(Party party)
		{
			lock (_lock)
			{
				var leader = party.GetLeader();
				if (leader == null)
					return false;

				if (_sessionByCharacterId.TryGetValue(leader.DbId, out var session))
					return session.State == AutoMatchState.Queued;

				return false;
			}
		}

		/// <summary>
		/// Creates a new AutoMatchSession, forms a new party, and adds the leader.
		/// Used when a match is FOUND and a dedicated party is being created.
		/// </summary>
		public AutoMatchSession CreateDungeonSession(Character leader, long autoMatchId, int dungeonId)
		{
			lock (_lock)
			{
				if (_sessionByCharacterId.ContainsKey(leader.DbId))
				{
					return _sessionByCharacterId[leader.DbId];
				}

				var partyManager = ZoneServer.Instance.World.Parties;
				var newParty = partyManager.Create(leader);
				var session = new AutoMatchSession(autoMatchId, dungeonId, newParty) { State = AutoMatchState.InProgress };

				_sessionsById[autoMatchId] = session;
				_sessionByCharacterId[leader.DbId] = session;
				leader.Variables.Perm.SetLong(SessionIdVarName, autoMatchId);

				return session;
			}
		}

		/// <summary>
		/// Completely removes an auto-match session and DISBANDS its associated party.
		/// This should be called when a dungeon ends.
		/// </summary>
		public void DestroyDungeonSession(long autoMatchId)
		{
			lock (_lock)
			{
				if (_sessionsById.TryGetValue(autoMatchId, out var session))
				{
					foreach (var member in session.Party.GetPartyMembers())
					{
						if (member == null)
							continue;

						member.Variables.Perm.SetLong(SessionIdVarName, 0);
						member.Variables.Temp.SetLong(AccountIdVarName, 0);
						_sessionByCharacterId.Remove(member.DbId);
					}

					ZoneServer.Instance.World.Parties.Delete(session.Party);
					_sessionsById.Remove(autoMatchId);
				}
			}
		}

		// --- General Helper Methods ---

		/// <summary>
		/// Gets an auto-match session by its unique ID.
		/// </summary>
		public AutoMatchSession GetSession(long sessionId)
		{
			lock (_lock)
			{
				_sessionsById.TryGetValue(sessionId, out var session);
				return session;
			}
		}

		/// <summary>
		/// Efficiently gets the auto-match session for a specific character.
		/// </summary>
		public AutoMatchSession GetSessionForCharacter(Character character)
		{
			lock (_lock)
			{
				_sessionByCharacterId.TryGetValue(character.DbId, out var session);
				return session;
			}
		}

		/// <summary>
		/// Handles the auto match members update message from SocialServer.
		/// Sends DungeonAutoMatchPartyCount to all players in the queue on this zone.
		/// </summary>
		public void HandleMembersUpdate(AutoMatchMembersUpdateMessage message)
		{
			if (message.Members == null || message.Members.Count == 0)
				return;

			// Build the member strings for the packet
			var memberStrings = new List<string>();
			foreach (var member in message.Members)
			{
				var readyStatus = member.IsReady ? "YES" : "NO";
				var memberStr = $"{member.AccountId}/{member.JobId}/{member.Level}/{member.CharacterId}/{readyStatus}/";
				memberStrings.Add(memberStr);
			}

			// Find all characters on this zone server that are in the queue and send them the update
			foreach (var member in message.Members)
			{
				var character = ZoneServer.Instance.World.GetCharacter(c => c.DbId == member.CharacterDbId);
				if (character?.Connection == null)
					continue;

				// Send all members in a single packet
				Send.ZC_NORMAL.DungeonAutoMatchPartyCount(character.Connection, memberStrings);
			}
		}

		/// <summary>
		/// Queues a party for auto-matching. Used by both CZ_REQ_REGISTER_TO_INDUN and
		/// CZ_REQ_MOVE_TO_INDUN with AutoMatchWithParty type.
		/// </summary>
		public void QueuePartyForAutoMatch(Character character, InstanceDungeonData dungeonData, int instanceDungeonId)
		{
			var conn = character.Connection;
			if (conn?.Account == null)
			{
				Log.Warning("QueuePartyForAutoMatch: Character '{0}' has no active connection.", character.Name);
				return;
			}

			// Check if auto-match is enabled for this dungeon
			if (!dungeonData.AutoMatchEnable)
			{
				Log.Info("QueuePartyForAutoMatch: User '{0}' attempted to join AutoMatchQueue for dungeonId '{1}' but auto match is disabled for this dungeon.", conn.Account.Name, instanceDungeonId);
				return;
			}

			var party = conn.Party;

			if (party == null)
			{
				character.SystemMessage("HadNotMyParty");
				return;
			}

			if (!party.IsLeader(character))
			{
				Send.ZC_ADDON_MSG(character, AddonMessage.FAIL_START_PARTY_MATCHING, 0, "None");
				return;
			}

			// Check entry count for all party members
			var partyMembersForMatch = party.GetPartyMembers();
			foreach (var memberCharacter in partyMembersForMatch)
			{
				if (memberCharacter == null)
					continue;

				if (memberCharacter.Dungeon.GetCurrentEntryCount(instanceDungeonId) >= memberCharacter.Dungeon.GetMaxEntryCount(instanceDungeonId))
				{
					memberCharacter.SystemMessage("CannotJoinIndunYet");
					if (memberCharacter != character)
						character.ServerMessage("A party member has exceeded their dungeon entry limit.");
					Send.ZC_ADDON_MSG(character, AddonMessage.FAIL_START_PARTY_MATCHING, 0, "None");
					return;
				}
			}

			// Collect all party member IDs and account IDs
			var partyMemberDbIds = new List<long>();
			var partyMemberAccountIds = new List<long>();
			foreach (var memberCharacter in partyMembersForMatch)
			{
				if (memberCharacter?.Connection?.Account == null)
					continue;

				partyMemberDbIds.Add(memberCharacter.DbId);
				partyMemberAccountIds.Add(memberCharacter.Connection.Account.ObjectId);
			}

			// Send the entire party as an atomic unit to the queue
			var partyMatchMessage = new AutoMatchPartyMessage(
				character.DbId,
				partyMemberDbIds,
				partyMemberAccountIds,
				instanceDungeonId,
				true
			);
			ZoneServer.Instance.Communicator.Send("Coordinator", partyMatchMessage.BroadcastTo("Chat"));

			// Track party session locally
			this.QueuePartySession(party, instanceDungeonId);

			var partyDungeonLocalName = $"@dicID_^*${dungeonData.ClassName}$*^";

			// Show auto-matching UI to all party members
			foreach (var memberCharacter in partyMembersForMatch)
			{
				if (memberCharacter?.Connection == null)
					continue;

				Send.ZC_NORMAL.DungeonAutoMatching(memberCharacter.Connection, instanceDungeonId);
				Send.ZC_NORMAL.DungeonAutoMatchWithParty(memberCharacter.Connection, 1, memberCharacter.Level, dungeonData.MaxLevel, 0, partyDungeonLocalName);
			}
		}
	}
}
