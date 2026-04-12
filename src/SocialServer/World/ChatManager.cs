using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MySqlConnector;
using Melia.Shared.Database;
using Melia.Social.Database;
using Melia.Social.Network;
using Yggdrasil.Db.MySql.SimpleCommands;

namespace Melia.Social.World
{
	public class ChatManager
	{
		private readonly Dictionary<long, ChatRoom> _rooms = new();

		private static long ChatId = 0x1FB0F00000000;

		/// <summary>
		/// Generate a unique id for chat rooms.
		/// </summary>
		public long GetNewChatId()
			=> Interlocked.Increment(ref ChatId);

		/// <summary>
		/// Add a chat room
		/// </summary>
		/// <param name="chatRoom"></param>
		public void AddChatRoom(ChatRoom chatRoom)
		{
			lock (_rooms)
				_rooms.Add(chatRoom.Id, chatRoom);
		}

		/// <summary>
		/// Remove a chat room with a given chat room id.
		/// </summary>
		/// <param name="id"></param>
		public void RemoveChatRoom(long id)
		{
			if (_rooms[id].Type == ChatRoomType.Group) {
				using (var conn = SocialServer.Instance.Database.GetConnection())
				using (var cmd = new MySqlCommand("DELETE FROM `chat_rooms` WHERE `roomId` = @roomId", conn)) {
					cmd.Parameters.AddWithValue("@roomId", _rooms[id].DbId);
					cmd.ExecuteNonQuery();
				}
			}

			lock (_rooms)
				_rooms.Remove(id);
		}

		/// <summary>
		/// Returns a 1:1 chat room between two accounts via out.
		/// Returns false if no personal chat room between the
		/// two users was found.
		/// </summary>
		/// <param name="accountId1"></param>
		/// <param name="accountId2"></param>
		/// <param name="chatRoom"></param>
		/// <returns></returns>
		public bool TryGetChatRoom(long accountId1, long accountId2, out ChatRoom chatRoom)
		{
			lock (_rooms)
			{
				foreach (var room in _rooms.Values)
				{
					if (room.Type != ChatRoomType.OneToOne)
						continue;

					var members = room.GetMembers();
					if (members.Length != 2)
						continue;

					if (members.Any(a => a.AccountId == accountId1) && members.Any(a => a.AccountId == accountId2))
					{
						chatRoom = room;
						return true;
					}
				}
			}

			chatRoom = null;
			return false;
		}

		/// <summary>
		/// Return true or false if a chat room with a given id is found.
		/// </summary>
		/// <param name="chatId"></param>
		/// <param name="chatRoom"></param>
		/// <returns></returns>
		public bool TryGetChatRoom(long chatId, out ChatRoom chatRoom)
		{
			lock (_rooms)
				return _rooms.TryGetValue(chatId, out chatRoom);
		}

		/// <summary>
		/// Creates a new chat room for the given user.
		/// </summary>
		/// <param name="creator"></param>
		/// <param name="chatId"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public ChatRoom CreateChatRoom(SocialUser creator, long chatId = 0, ChatRoomType type = ChatRoomType.Group)
		{
			var room = new ChatRoom(chatId, "", type);
			this.AddChatRoom(room);
			if (type == ChatRoomType.Group) {
				using (var conn = SocialServer.Instance.Database.GetConnection())
				using (var trans = conn.BeginTransaction()) {
					using (var cmd = new InsertCommand("INSERT INTO `chat_rooms` {parameters}", conn, trans)) {
						cmd.Set("type", type);
						cmd.Set("creatorId", creator.Id);
						cmd.Set("name", "New Chat");
						cmd.Execute();
						room.DbId = cmd.LastId;
					}
					trans.Commit();
				}
			}

			room.AddMember(creator);

			if (chatId == 0)
				room.AddMessage(new ChatMessage(creator, "!@#$NewRoomHasBeenCreated#@!"));

			return room;
		}

		/// <summary>
		/// Returns a list of chat rooms that the given user is a member of.
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public ChatRoom[] FindChatRooms(SocialUser user)
		{
			lock (_rooms)
				return _rooms.Values.Where(a => a.IsMember(user.TeamName)).ToArray();
		}

		/// <summary>
		/// Loads or creates existing and default chat rooms.
		/// </summary>
		public void LoadChats()
		{
			var room = new ChatRoom("Main", ChatRoomType.Group);
			room.MaxMemberCount = 9999;
			room.CreateInvite(0);

			this.AddChatRoom(room);

			using (var conn = SocialServer.Instance.Database.GetConnection()) {
				var roomsToLoad = new List<(ChatRoom room, long dbId)>();
				using (var cmd = new MySqlCommand("SELECT * FROM `chat_rooms` WHERE `type` = @type", conn)) {
					cmd.Parameters.AddWithValue("@type", 3);
					using (var reader = cmd.ExecuteReader()) {
						while (reader.Read()) {
							var dbRoom = new ChatRoom(reader.GetStringSafe("name"), ChatRoomType.Group);
							dbRoom.DbId = reader.GetInt64("roomId");
							dbRoom.OwnerId = reader.GetInt64("creatorId");
							this.AddChatRoom(dbRoom);
							roomsToLoad.Add((dbRoom, dbRoom.DbId));
						}
					}
				}
				if (roomsToLoad.Any() == false)
					return;
				var roomIds = roomsToLoad.Select(room => room.dbId).ToList();
				var idParams = roomIds.Select((id, i) => $"@roomId{i}").ToArray();
				using (var cmd = new MySqlCommand($"SELECT * FROM `chat_members` WHERE `roomId` IN ({string.Join(",", idParams)})", conn)) {
					for (int i = 0 ; i < roomIds.Count(); i++)
						cmd.Parameters.AddWithValue(idParams[i], roomIds[i]);
					
					using (var reader = cmd.ExecuteReader()) {
						var membersByRoom = new Dictionary<long, List<ChatMember>>();
						while (reader.Read()) {
							var roomId = reader.GetInt64("roomId");
							var chatMember = new ChatMember(
								roomId,
								reader.GetInt64("userId"),
								reader.GetStringSafe("teamName")
							);
							if (membersByRoom.ContainsKey(roomId) == false)
								membersByRoom[roomId] = new List<ChatMember>();
							membersByRoom[roomId].Add(chatMember);
						}
						foreach (var (roomToLoad, dbId) in roomsToLoad) {
							if (membersByRoom.TryGetValue(dbId, out var members)) {
								foreach (var member in members)
									roomToLoad.AddMember(member);
							}
						}
					}
				}
			}
		}
	}
}
