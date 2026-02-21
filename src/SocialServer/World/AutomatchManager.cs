using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Melia.Shared.Data.Database;
using Melia.Shared.Network.Inter.Messages;
using Melia.Social;
using Melia.Social.Network;
using Melia.Social.World;
using Yggdrasil.Logging;
using Yggdrasil.Network.Communication;

namespace Melia.Social.World
{
	/// <summary>
	/// Manages Auto Matches queues, players can join to search
	/// for other players and automatically start a content.
	/// Parties are queued as atomic units and matched together.
	/// </summary>
	public class AutoMatchQueueManager
	{
		private static long AutoMatchId = 0x1000000;

		/// <summary>
		/// Generate a unique id for auto match.
		/// </summary>
		public long GetNewAutoMatchId()
			=> Interlocked.Increment(ref AutoMatchId);

		private Dictionary<int, List<AutoMatchInfo>> _queues = new Dictionary<int, List<AutoMatchInfo>>();
		private const int DefaultMaxPlayersPerInstance = 5;
		private const int MinPartiesWithReadyState = 2;

		private readonly Timer _memberUpdateTimer;
		private const int MemberUpdateIntervalMs = 2000;

		/// <summary>
		/// Creates a new AutoMatchQueueManager and starts the periodic member update timer.
		/// </summary>
		public AutoMatchQueueManager()
		{
			_memberUpdateTimer = new Timer(this.BroadcastMemberUpdates, null, MemberUpdateIntervalMs, MemberUpdateIntervalMs);
		}

		/// <summary>
		/// Used once a solo player joins the auto match.
		/// </summary>
		/// <param name="dungeonId">ID of the dungeon (ClassID).</param>
		/// <param name="autoMatchInfo">AutoMatchInfo reference (single player as party of 1).</param>
		public void JoinAutoMatch(int dungeonId, AutoMatchInfo autoMatchInfo)
		{
			lock (_queues)
			{
				foreach (var memberDbId in autoMatchInfo.PartyMemberDbIds)
				{
					if (this.IsCharacterInAnyQueue(memberDbId))
					{
						Log.Warning("Character DbId '{0}' attempted to join auto match but is already in queue.", memberDbId);
						return;
					}
				}

				if (!_queues.ContainsKey(dungeonId))
				{
					_queues[dungeonId] = new List<AutoMatchInfo>();
					Log.Info("Creating a new Queue for DungeonId '{0}'", dungeonId);
				}

				_queues[dungeonId].Add(autoMatchInfo);
				Log.Info("JoinAutoMatch: Added party (leader: {0}, size: {1}) to DungeonId '{2}' AutoMatch Queue",
					autoMatchInfo.LeaderCharacterDbId, autoMatchInfo.PartySize, dungeonId);

				this.BroadcastMemberUpdates(null);
				this.TryStartContent(dungeonId);
			}
		}

		/// <summary>
		/// Used once a party joins the auto match as an atomic unit.
		/// </summary>
		/// <param name="dungeonId">ID of the dungeon (ClassID).</param>
		/// <param name="autoMatchInfo">AutoMatchInfo containing the full party.</param>
		public void JoinAutoMatchAsParty(int dungeonId, AutoMatchInfo autoMatchInfo)
		{
			this.JoinAutoMatch(dungeonId, autoMatchInfo);
		}

		/// <summary>
		/// Checks if a character is already in any queue.
		/// </summary>
		private bool IsCharacterInAnyQueue(long characterDbId)
		{
			foreach (var queue in _queues.Values)
			{
				if (queue.Any(info => info.ContainsCharacter(characterDbId)))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Used once a player/party exits the auto match.
		/// Removes the entire party from the queue.
		/// </summary>
		/// <param name="characterDbId">Character DbId (any member of the party).</param>
		public void ExitAutoMatch(long characterDbId)
		{
			lock (_queues)
			{
				bool partyFound = false;

				foreach (var key in _queues.Keys.ToList())
				{
					var partyToRemove = _queues[key].FirstOrDefault(a => a.ContainsCharacter(characterDbId));
					if (partyToRemove != null)
					{
						_queues[key].Remove(partyToRemove);
						partyFound = true;
						Log.Info("Party (leader: {0}, size: {1}) has left the queue for DungeonId: {2}",
							partyToRemove.LeaderCharacterDbId, partyToRemove.PartySize, key);

						if (_queues[key].Count == 0)
						{
							_queues.Remove(key);
							Log.Info("Queue for DungeonId: {0} is now empty and removed.", key);
						}
						else
						{
							this.BroadcastMemberUpdates(null);
						}
						break;
					}
				}

				if (!partyFound)
				{
					Log.Info("Character DbId: {0} attempted to leave but was not found in any queue.", characterDbId);
				}
			}
		}

		/// <summary>
		/// Broadcasts queue status update to all users in the queue.
		/// </summary>
		private void BroadcastQueueUpdate(int dungeonId)
		{
			if (!_queues.ContainsKey(dungeonId))
				return;

			var totalPlayers = this.GetTotalPlayersInQueue(dungeonId);
			var readyParties = this.GetReadyPartiesCount(dungeonId);

			foreach (var partyInfo in _queues[dungeonId])
			{
				foreach (var user in partyInfo.Users)
				{
					if (user != null)
					{
						Send.SC_FROM_INTEGRATE.AutoMatchQueueTotalUsers(user, totalPlayers);
						Send.SC_FROM_INTEGRATE.AutoMatchQueueUpdate(user, readyParties);
					}
				}
			}
		}

		/// <summary>
		/// Periodically broadcasts member details to all zone servers.
		/// ZoneServers will then send DungeonAutoMatchPartyCount to appropriate clients.
		/// </summary>
		private void BroadcastMemberUpdates(object state)
		{
			lock (_queues)
			{
				if (_queues.Count == 0)
					return;

				foreach (var kvp in _queues)
				{
					var dungeonId = kvp.Key;
					var queue = kvp.Value;

					var members = new List<AutoMatchMemberInfo>();
					foreach (var partyInfo in queue)
					{
						for (var i = 0; i < partyInfo.Users.Count && i < partyInfo.PartyMemberDbIds.Count; i++)
						{
							var user = partyInfo.Users[i];
							var characterDbId = partyInfo.PartyMemberDbIds[i];

							if (user == null)
								continue;

							if (user.Character.Id == 0)
								continue;

							var memberInfo = new AutoMatchMemberInfo(
								user.AccountId,
								user.Character.Id,
								characterDbId,
								(int)user.Character.JobId,
								user.Character.Level,
								partyInfo.IsCharacterReady(characterDbId)
							);
							members.Add(memberInfo);
						}
					}

					if (members.Count > 0)
					{
						var message = new AutoMatchMembersUpdateMessage(dungeonId, members);
						SocialServer.Instance.Communicator.Send("Coordinator", message.BroadcastTo("AllZones"));
					}
				}
			}
		}

		/// <summary>
		/// Attempts to start content if a valid combination of parties can fill a dungeon.
		/// </summary>
		private void TryStartContent(int dungeonId)
		{
			if (!_queues.ContainsKey(dungeonId))
				return;

			var queue = _queues[dungeonId];
			var maxPlayers = this.GetMaxPlayersForDungeon(dungeonId);

			var readyParties = queue.Where(p => p.IsReadyToMatchUnderstaffed).ToList();
			var readyPlayers = readyParties.Sum(p => p.PartySize);
			if (readyPlayers >= 2 && readyPlayers <= maxPlayers)
			{
				this.StartGameInstance(dungeonId, readyParties);
				return;
			}

			var matchedParties = this.FindMatchingParties(queue, maxPlayers);
			if (matchedParties != null && matchedParties.Count > 0)
			{
				this.StartGameInstance(dungeonId, matchedParties);
			}
		}

		/// <summary>
		/// Gets the maximum number of players for a dungeon from the database.
		/// </summary>
		private int GetMaxPlayersForDungeon(int dungeonId)
		{
			if (SocialServer.Instance.Data.InstanceDungeonDb.Entries.TryGetValue(dungeonId, out var dungeonData))
				return dungeonData.MaxPlayers > 0 ? dungeonData.MaxPlayers : DefaultMaxPlayersPerInstance;

			return DefaultMaxPlayersPerInstance;
		}

		/// <summary>
		/// Finds a combination of parties that fit within the max player limit.
		/// Processes parties in order of arrival (FIFO).
		/// </summary>
		private List<AutoMatchInfo> FindMatchingParties(List<AutoMatchInfo> queue, int maxPlayers)
		{
			var result = new List<AutoMatchInfo>();
			var totalPlayers = 0;

			foreach (var party in queue)
			{
				if (totalPlayers + party.PartySize <= maxPlayers)
				{
					result.Add(party);
					totalPlayers += party.PartySize;

					if (totalPlayers == maxPlayers)
						break;
				}
			}

			if (totalPlayers == maxPlayers)
				return result;

			return null;
		}

		/// <summary>
		/// Starts a game instance for the matched parties.
		/// </summary>
		/// <param name="dungeonId">The dungeon ID.</param>
		/// <param name="matchedParties">List of parties to start the instance with.</param>
		private void StartGameInstance(int dungeonId, List<AutoMatchInfo> matchedParties)
		{
			foreach (var party in matchedParties)
			{
				_queues[dungeonId].Remove(party);
			}

			if (_queues[dungeonId].Count == 0)
			{
				_queues.Remove(dungeonId);
			}

			var allCharacterDbIds = new List<long>();
			foreach (var party in matchedParties)
			{
				allCharacterDbIds.AddRange(party.PartyMemberDbIds);
			}

			var totalPlayers = allCharacterDbIds.Count;
			var partyCount = matchedParties.Count;

			Log.Info("Game instance started for DungeonId: {0} with {1} players from {2} parties",
				dungeonId, totalPlayers, partyCount);

			var initAutoMatchContentMessage = new InitAutoMatchContentMessage(allCharacterDbIds, dungeonId, this.GetNewAutoMatchId());
			SocialServer.Instance.Communicator.Send("Coordinator", initAutoMatchContentMessage.BroadcastTo("AllZones"));

			Log.Info("New dungeon instance message sent to all zone servers - dungeonId: {0}.", dungeonId);
		}

		/// <summary>
		/// Updates a character's ready state (willing to start with less than full).
		/// The party's ready state is only set to true when ALL members are ready.
		/// </summary>
		/// <param name="characterDbId">Character DbId of the player who clicked ready.</param>
		public void UpdateToReadyState(long characterDbId)
		{
			lock (_queues)
			{
				bool partyFound = false;

				foreach (var key in _queues.Keys.ToList())
				{
					var party = _queues[key].FirstOrDefault(a => a.ContainsCharacter(characterDbId));
					if (party != null)
					{
						party.SetCharacterReady(characterDbId);
						partyFound = true;

						party.IsReadyToMatchUnderstaffed = party.AreAllMembersReady();

						if (party.IsReadyToMatchUnderstaffed)
							Log.Info("Party (leader: {0}) is now fully ready for Match 2-4 Players.", party.LeaderCharacterDbId);
						else
							Log.Info("Character DbId '{0}' in party (leader: {1}) clicked ready ({2}/{3} members ready).", characterDbId, party.LeaderCharacterDbId, party.ReadyCharacterDbIds.Count, party.PartySize);

						this.BroadcastMemberUpdates(null);

						this.TryStartContent(key);
						break;
					}
				}

				if (!partyFound)
				{
					Log.Info("Character DbId '{0}' attempted to set ready state but was not found in any queue.", characterDbId);
				}
			}
		}

		/// <summary>
		/// Clears all queues from the manager.
		/// </summary>
		public void ClearAllQueues()
		{
			lock (_queues)
			{
				_queues.Clear();
			}
			Log.Info("All queues have been cleared.");
		}

		/// <summary>
		/// Gets the total number of players across all parties in a queue.
		/// </summary>
		/// <param name="dungeonId">The ID of the dungeon (ClassID).</param>
		/// <returns>Total player count.</returns>
		public int GetTotalPlayersInQueue(int dungeonId)
		{
			lock (_queues)
			{
				if (!_queues.ContainsKey(dungeonId))
					return 0;

				return _queues[dungeonId].Sum(p => p.PartySize);
			}
		}

		/// <summary>
		/// Gets the number of parties in a queue.
		/// </summary>
		/// <param name="dungeonId">The ID of the dungeon (ClassID).</param>
		/// <returns>Party count.</returns>
		public int GetPartiesInQueue(int dungeonId)
		{
			lock (_queues)
			{
				return _queues.ContainsKey(dungeonId) ? _queues[dungeonId].Count : 0;
			}
		}

		/// <summary>
		/// Gets the number of parties that are ready to start with less players.
		/// </summary>
		/// <param name="dungeonId">The ID of the dungeon (ClassID).</param>
		/// <returns>Ready party count.</returns>
		public int GetReadyPartiesCount(int dungeonId)
		{
			lock (_queues)
			{
				if (!_queues.ContainsKey(dungeonId))
					return 0;

				return _queues[dungeonId].Count(p => p.IsReadyToMatchUnderstaffed);
			}
		}

		/// <summary>
		/// Gets the amount of players on a queue by a given dungeonId.
		/// </summary>
		/// <param name="dungeonId">The ID of the dungeon (ClassID).</param>
		/// <returns></returns>
		public int GetPlayersInQueue(int dungeonId)
		{
			return this.GetTotalPlayersInQueue(dungeonId);
		}

		/// <summary>
		/// Gets the amount of players that are on ready state on a queue by a given dungeonId.
		/// </summary>
		/// <param name="dungeonId">The ID of the dungeon (ClassID).</param>
		/// <returns></returns>
		public int GetPlayersReadyInQueue(int dungeonId)
		{
			lock (_queues)
			{
				if (!_queues.ContainsKey(dungeonId))
					return 0;

				return _queues[dungeonId].Where(p => p.IsReadyToMatchUnderstaffed).Sum(p => p.PartySize);
			}
		}

		/// <summary>
		/// Gets a list of all parties in a queue.
		/// </summary>
		/// <param name="dungeonId">The ID of the dungeon (ClassID).</param>
		/// <returns>A list of all auto match parties in queue.</returns>
		public List<AutoMatchInfo> GetPartiesInQueueList(int dungeonId)
		{
			lock (_queues)
			{
				return _queues.ContainsKey(dungeonId) ? _queues[dungeonId].ToList() : new List<AutoMatchInfo>();
			}
		}
	}

	/// <summary>
	/// Contains the data of a party/group while in auto match queue.
	/// A solo player is treated as a party of 1.
	/// </summary>
	public class AutoMatchInfo
	{
		/// <summary>
		/// Gets or sets the leader's character db id.
		/// </summary>
		public long LeaderCharacterDbId { get; set; }

		/// <summary>
		/// Gets or sets all party member character db ids (including leader).
		/// </summary>
		public List<long> PartyMemberDbIds { get; set; }

		/// <summary>
		/// Gets the party size.
		/// </summary>
		public int PartySize => this.PartyMemberDbIds?.Count ?? 0;

		/// <summary>
		/// Gets or sets the ready state that makes
		/// the queue starts if two or more parties are ready.
		/// </summary>
		public bool IsReadyToMatchUnderstaffed { get; set; }

		/// <summary>
		/// Gets or sets the set of character db ids that have clicked "match 2 to 4".
		/// </summary>
		public HashSet<long> ReadyCharacterDbIds { get; set; }

		/// <summary>
		/// Gets or sets the users associated with this party (for sending updates).
		/// </summary>
		public List<SocialUser> Users { get; set; }

		/// <summary>
		/// Creates a new, blank AutoMatchInfo.
		/// </summary>
		public AutoMatchInfo()
		{
			this.PartyMemberDbIds = new List<long>();
			this.Users = new List<SocialUser>();
			this.ReadyCharacterDbIds = new HashSet<long>();
		}

		/// <summary>
		/// Creates a new AutoMatchInfo for a solo player.
		/// </summary>
		public AutoMatchInfo(long characterDbId, SocialUser user)
		{
			this.LeaderCharacterDbId = characterDbId;
			this.PartyMemberDbIds = new List<long> { characterDbId };
			this.Users = new List<SocialUser> { user };
			this.IsReadyToMatchUnderstaffed = false;
			this.ReadyCharacterDbIds = new HashSet<long>();
		}

		/// <summary>
		/// Creates a new AutoMatchInfo for a party.
		/// </summary>
		public AutoMatchInfo(long leaderCharacterDbId, List<long> partyMemberDbIds, List<SocialUser> users)
		{
			this.LeaderCharacterDbId = leaderCharacterDbId;
			this.PartyMemberDbIds = partyMemberDbIds ?? new List<long>();
			this.Users = users ?? new List<SocialUser>();
			this.IsReadyToMatchUnderstaffed = false;
			this.ReadyCharacterDbIds = new HashSet<long>();
		}

		/// <summary>
		/// Checks if this party contains the given character.
		/// </summary>
		public bool ContainsCharacter(long characterDbId)
		{
			return this.PartyMemberDbIds.Contains(characterDbId);
		}

		/// <summary>
		/// Checks if the given character has clicked "match 2 to 4".
		/// </summary>
		public bool IsCharacterReady(long characterDbId)
		{
			return this.ReadyCharacterDbIds.Contains(characterDbId);
		}

		/// <summary>
		/// Marks a character as ready for matching with less than full players.
		/// </summary>
		public void SetCharacterReady(long characterDbId)
		{
			this.ReadyCharacterDbIds.Add(characterDbId);
		}

		/// <summary>
		/// Checks if all party members have clicked "match 2 to 4".
		/// </summary>
		public bool AreAllMembersReady()
		{
			if (this.PartyMemberDbIds == null || this.PartyMemberDbIds.Count == 0)
				return false;

			foreach (var memberDbId in this.PartyMemberDbIds)
			{
				if (!this.ReadyCharacterDbIds.Contains(memberDbId))
					return false;
			}
			return true;
		}
	}
}
