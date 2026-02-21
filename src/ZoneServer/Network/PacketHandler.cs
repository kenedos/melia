using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.L10N;
using Melia.Shared.Network;
using Melia.Shared.Network.Helpers;
using Melia.Shared.Network.Inter.Messages;
using Melia.Shared.ObjectProperties;
using Melia.Shared.Versioning;
using Melia.Shared.World;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Items.Effects;
using Melia.Zone.Network.Helpers;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.Services;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Maps;
using Melia.Zone.World.Storages;
using Melia.Zone.Util;
using Yggdrasil.Extensions;
using Yggdrasil.Logging;
using Yggdrasil.Network.Communication;
using static Melia.Shared.Util.TaskHelper;
using static Melia.Zone.Database.ZoneDb;

namespace Melia.Zone.Network
{
	public partial class PacketHandler : PacketHandler<IZoneConnection>
	{
		/// <summary>
		/// Sent wrongfully if a channel wasn't available and the client
		/// tries to log in again afterwards.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CB_LOGIN)]
		public void CB_LOGIN(IZoneConnection conn, Packet packet)
		{
			// Close connection, which should then make the client try to
			// connect to the barracks server instead.
			conn.Close();
		}

		/// <summary>
		/// Sent after connecting to channel.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CONNECT)]
		public void CZ_CONNECT(IZoneConnection conn, Packet packet)
		{
			long accountId;
			long characterId;
			string accountName;
			string sessionKey;
			var fromBarracks1 = true;
			if (Versions.Protocol > 500)
			{
				var bin1 = packet.GetBin(1024);
				sessionKey = packet.GetString(64);

				// When using passprt login, this is the account id as string,
				// and it's 18 (?) bytes long.
				accountName = packet.GetString(56);

				var mac = packet.GetString(48);
				var socialId = packet.GetLong();
				var l1 = packet.GetLong();
				accountId = packet.GetLong();
				characterId = packet.GetLong();
				var i1 = packet.GetInt();
				var i2 = packet.GetInt();
				var i3 = packet.GetInt();
				var s1 = packet.GetShort();
				var s2 = packet.GetShort();
				var s3 = packet.GetShort();
				fromBarracks1 = packet.GetBool();
				var fromBarracks2 = packet.GetBool();
				var b2 = packet.GetByte();
				var b3 = packet.GetByte();
				var b1 = packet.GetByte(); // [i373230 (2023-05-10)] Might've been added before
			}
			else
			{
				var s1 = packet.GetShort();
				var s2 = packet.GetShort();
				accountId = packet.GetLong();
				characterId = packet.GetLong();

				// When using passprt login, this is the account id as string,
				// and it's 18 (?) bytes long.
				accountName = packet.GetString(33); // ?

				var unk = packet.GetBin(1037);
				sessionKey = packet.GetString(64);
			}

			var enqueuedAt = Stopwatch.GetTimestamp();

			SaveQueue.Enqueue(() => this.ProcessConnect(conn, accountId, characterId, accountName, sessionKey, fromBarracks1, enqueuedAt));
		}

		private void ProcessConnect(IZoneConnection conn, long accountId, long characterId, string accountName, string sessionKey, bool fromBarracks1, long enqueuedAt)
		{
			var queuedMs = (Stopwatch.GetTimestamp() - enqueuedAt) * 1000 / Stopwatch.Frequency;
			var sw = Stopwatch.StartNew();
			long authMs = 0, cleanupMs = 0, loadMs = 0;
			long cleanupSaveMs = 0, cleanupRemoveMs = 0, cleanupCloseMs = 0;
			string charName = null;
			long charDbId = 0;

			try
			{
				if (ServerShutdownManager.Instance.IsShutdownPending)
				{
					var remaining = ServerShutdownManager.Instance.GetRemainingTimeFormatted();
					Log.Info("Rejected login attempt from '{0}' - server is shutting down ({1} remaining).", accountName, remaining);
					Send.ZC_CONNECT_FAILED(conn, 1, "Server is shutting down. Please try again later.");
					return;
				}

				conn.Account = ZoneServer.Instance.Database.GetAccount(accountName);
				if (conn.Account == null)
				{
					Log.Warning("Stopped attempt to login with invalid account '{0}'. Closing connection.", accountName);
					conn.Close();
					return;
				}

				if (!ZoneServer.Instance.Database.CheckSessionKey(conn.Account.Id, sessionKey))
				{
					Log.Warning("Stopped attempt to login on account '{0}' with invalid session key '{1}'. Closing connection.", accountName, sessionKey);
					conn.Close();
					return;
				}

				authMs = sw.ElapsedMilliseconds;

				var cleanupStart = sw.ElapsedMilliseconds;
				var characterDbId = characterId > ObjectIdRanges.Characters ? characterId - ObjectIdRanges.Characters : characterId;

				var existingCharacter = ZoneServer.Instance.World.GetCharacter(c => c.DbId == characterDbId);
				if (existingCharacter != null && !existingCharacter.IsAutoTrading)
				{
					Log.Info($"CZ_CONNECT: Cleaning up existing character '{existingCharacter.Name}' (ID: {characterDbId}) for reconnect.");

					// Set SavedForWarp BEFORE closing the old connection so that
					// OnClosed's deferred save skips the redundant save.
					existingCharacter.SavedForWarp = true;

					var removeStart = sw.ElapsedMilliseconds;
					existingCharacter.Map?.RemoveCharacter(existingCharacter);
					cleanupRemoveMs = sw.ElapsedMilliseconds - removeStart;

					var closeStart = sw.ElapsedMilliseconds;
					existingCharacter.Connection?.Close();
					cleanupCloseMs = sw.ElapsedMilliseconds - closeStart;

					// Run the save in the background with a bounded wait so the
					// reconnect isn't blocked by a slow DB transaction. If the
					// save doesn't finish within 5 seconds, proceed anyway — the
					// DB already has autosave data, and the background save will
					// still complete eventually.
					var charToSave = existingCharacter;
					var saveStart = sw.ElapsedMilliseconds;
					var saveTask = Task.Run(() =>
					{
						try
						{
							ZoneServer.Instance.Database.SaveCharacterData(charToSave);
						}
						catch (Exception ex)
						{
							Log.Error($"CZ_CONNECT: Background save failed for '{charToSave.Name}' ({charToSave.DbId}): {ex}");
						}
					});

					const int SaveTimeoutMs = 5000;
					if (!saveTask.Wait(SaveTimeoutMs))
					{
						Log.Warning($"CZ_CONNECT: Save for '{charToSave.Name}' ({charToSave.DbId}) didn't finish within {SaveTimeoutMs}ms. Proceeding with reconnect; save continues in background.");
					}
					cleanupSaveMs = sw.ElapsedMilliseconds - saveStart;
				}

				if (existingCharacter != null && existingCharacter.IsAutoTrading)
				{
					var saveStart = sw.ElapsedMilliseconds;
					ZoneServer.Instance.Database.SaveCharacterData(existingCharacter);
					cleanupSaveMs = sw.ElapsedMilliseconds - saveStart;
				}

				cleanupMs = sw.ElapsedMilliseconds - cleanupStart;

				var loadStart = sw.ElapsedMilliseconds;

				var character = ZoneServer.Instance.Database.GetCharacter(conn.Account.Id, characterId);
				if (character == null)
				{
					Log.Warning("User '{0}' tried to use a non-existing character, '{1}'. Closing connection.", accountName, characterId);
					conn.Close();
					return;
				}

				charName = character.Name;
				charDbId = character.DbId;

				var map = ZoneServer.Instance.World.GetMap(character.MapId);
				if (map == null)
				{
					Log.Error($"CZ_CONNECT: User '{accountName}' logged on with invalid map ID '{character.MapId}'. Cannot place character in world.");
					Send.ZC_CONNECT_FAILED(conn, 1, $"The map with id {character.MapId} is currently unavailable.");
					return;
				}

				loadMs = sw.ElapsedMilliseconds - loadStart;

				var ghost = map.GetCharacter(c => c.DbId == character.DbId && c.Handle != character.Handle);
				if (ghost != null)
				{
					Send.ZC_LEAVE(ghost);
					ghost.IsAutoTrading = false;
					ghost.IsOnline = false;
					map.RemoveCharacter(ghost);
				}

				character.Connection = conn;
				conn.SelectedCharacter = character;

				var existingParty = ZoneServer.Instance.World.Parties.FindPartyByAccountId(conn.Account.Id, out var oldMember);
				if (existingParty != null && oldMember != null)
				{
					if (oldMember.DbId == character.DbId)
					{
						conn.Party = existingParty;
					}
					else
					{
						existingParty.ReplaceCharacter(oldMember, character);
						conn.Party = existingParty;
					}
				}

				// conn.Guild = ZoneServer.Instance.World.GetGuild(character.GuildId); // Removed: Guild type deleted

				ZoneServer.Instance.Database.AfterLoad(conn.Account, character);
				ZoneServer.Instance.ServerEvents.PlayerLoggedIn.Raise(new PlayerEventArgs(character));

				map.AddCharacter(character);

				conn.LoggedIn = true;
				conn.LastHeartBeat = DateTime.Now;
				conn.SessionKey = sessionKey;
				character.IsOnline = true;

				ZoneServer.Instance.Database.SaveSessionKey(character.DbId, conn.SessionKey);
				ZoneServer.Instance.Database.UpdateLoginState(conn.Account.Id, character.DbId, LoginState.Zone);

				if (fromBarracks1)
				{
					Send.ZC_CONNECT_OK(conn, character);
				}
				else
				{
					Send.ZC_CONNECT_OK(conn, character);
				}

				CallSafe(character.ShowMainChatOnLogin());
			}
			finally
			{
				sw.Stop();
				var setupMs = sw.ElapsedMilliseconds - authMs - cleanupMs - loadMs;
				if (charName != null)
				{
					var cleanupStr = cleanupMs > 0 ? $", cleanup={cleanupMs} [save={cleanupSaveMs}, remove={cleanupRemoveMs}, close={cleanupCloseMs}]" : "";
					Log.Info($"CZ_CONNECT: '{charName}' (ID: {charDbId}) ready in {sw.ElapsedMilliseconds}ms [queued={queuedMs}, auth={authMs}{cleanupStr}, load={loadMs}, setup={setupMs}]");
				}
			}
		}

		/// <summary>
		/// Sent mid-loading, after the player entered the world.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_GAME_READY)]
		public void CZ_GAME_READY(IZoneConnection conn, Packet packet)
		{
			var serverId = packet.GetShort();

			var character = conn.SelectedCharacter;
			var gameReadyArgs = new PlayerGameReadyEventArgs(character);

			ZoneServer.Instance.ServerEvents.PlayerGameReady.Raise(gameReadyArgs);
			if (gameReadyArgs.CancelHandling)
				return;

			// Initialize item set bonuses now that equipment is loaded
			character.ItemSets?.RecalculateAllSetBonuses();

			conn.GameReady = true;

			if (Versions.Protocol >= 500)
			{
				Send.ZC_STANCE_CHANGE(character);
				Send.ZC_NORMAL.AdventureBook(conn);
				Send.ZC_SET_CHATBALLOON_SKIN(character);

				Send.ZC_IES_MODIFY_LIST(conn);
				Send.ZC_ITEM_INVENTORY_DIVISION_LIST(character);
				Send.ZC_SESSION_OBJECTS(character);
				Send.ZC_OPTION_LIST(conn);
				Send.ZC_SKILLMAP_LIST(character);
				//Send.ZC_ACHIEVE_POINT_LIST(character);
				Send.ZC_SPLIT_ACHIEVE_POINT_LIST(character);
				Send.ZC_SPLIT_ACHIEVE_SET(character);
				Send.ZC_CHAT_MACRO_LIST(character);
				Send.ZC_MAP_REVEAL_LIST(conn);
				Send.ZC_NPC_STATE_LIST(character);
				Send.ZC_HELP_LIST(character);
				Send.ZC_MYPAGE_MAP(conn);
				Send.ZC_GUESTPAGE_MAP(conn);
				Send.ZC_NORMAL.UpdateSkillUI(character);
				// Official server sends Skintone Object Property around here
				Send.ZC_ITEM_EQUIP_LIST(character);
				Send.ZC_NORMAL.SetSkillsProperties(conn);
				Send.ZC_SKILL_LIST(character);
				Send.ZC_COMMON_SKILL_LIST(character);
				Send.ZC_NORMAL.UpdateSkillUI(character);
				Send.ZC_ABILITY_LIST(character);
				Send.ZC_COOLDOWN_LIST(character, null);
				Send.ZC_NORMAL.ItemCollectionList(character);
				Send.ZC_NORMAL.Unknown_E4(character);
				Send.ZC_NORMAL.Unknown_134(character);
				Send.ZC_OBJECT_PROPERTY(conn, character);
				Send.ZC_OBJECT_PROPERTY(conn, character.Etc);
				Send.ZC_START_GAME(conn);
				Send.ZC_UPDATE_ALL_STATUS(character, 0);
				Send.ZC_SET_WEBSERVICE_URL(conn);
				Send.ZC_SEND_NONE_TARGETING_LIST(character, [300010, 300009, 57709, 12084, 57710, 57195, 57194, 57197]);
				Send.ZC_MOVE_SPEED(character);
				Send.ZC_STAMINA(character, character.Stamina);
				Send.ZC_UPDATE_SP(character, character.Sp, false);
				Send.ZC_RES_DAMAGEFONT_SKIN(conn, character);
				Send.ZC_RES_DAMAGEEFFECT_SKIN(conn, character);
				Send.ZC_LOGIN_TIME(conn, DateTime.Now);
				Send.ZC_MYPC_ENTER(character);

	
				if (conn.Party != null) // Guild check removed: Guild type deleted
					Send.ZC_NORMAL.ShowParty(character);

				if (conn.Party != null)
				{
					Send.ZC_PARTY_INFO(character, conn.Party);
					Send.ZC_PARTY_LIST(conn.Party);
					conn.Party.UpdateMember(character, true);
				}
				// Removed: Guild block - Guild type deleted during Laima merge
				// if (conn.Guild != null) { ... }

				Send.ZC_NORMAL.UsedMedalTotal(conn, conn.Account.Medals);
				Send.ZC_CASTING_SPEED(character);
				Send.ZC_QUICK_SLOT_LIST(character);
				Send.ZC_NORMAL.JobCount(character);
				Send.ZC_UPDATED_PCAPPEARANCE(character);
				Send.ZC_EQUIP_CARD_INFO(character);
				Send.ZC_EQUIP_GEM_INFO(character);
				Send.ZC_NORMAL.HeadgearVisibilityUpdate(character);
				Send.ZC_ADDITIONAL_SKILL_POINT(character);
				Send.ZC_SET_DAYLIGHT_INFO(character);
				//Send.ZC_DAYLIGHT_FIXED(character);
				Send.ZC_SEND_APPLY_HUD_SKIN_MYSELF(conn, character);

				Send.ZC_NORMAL.AccountProperties(character);
				Send.ZC_NORMAL.SetSessionKey(conn);

				// ---- <PremiumStuff> --------------------------------------------------

				Send.ZC_SEND_CASH_VALUE(conn);
				Send.ZC_SEND_PREMIUM_STATE(conn, conn.Account.Premium.Token);

				if (conn.Account.Premium.CanUseBuff)
					character.StartBuff(BuffId.Premium_Token);

				// ---- </PremiumStuff> -------------------------------------------------

				// The ability points are longer read from the properties for
				// whatever reason. We have to use the "custom commander info"
				// now. Yay.
				Send.ZC_CUSTOM_COMMANDER_INFO(character, CommanderInfoType.AbilityPoints, character.Properties.AbilityPoints);

				// It's currently unknown what exactly ZC_UPDATE_SKL_SPDRATE_LIST
				// does, but the data is necessary for the client to display the
				// overheat bubbles on the skill icons, so we'll send the skills
				// that have an overheat count.
				var skillUpdateList = character.Skills.GetList(a => a.Data.OverheatCount > 0);
				Send.ZC_UPDATE_SKL_SPDRATE_LIST(character, skillUpdateList);
				Send.ZC_NORMAL.AccountProperties(character);

				// Send companion info and schedule activation
				// after scripts have run on the player (i.e. set layer)
				if (character.HasCompanions)
				{
					foreach (var companion in character.Companions.GetList())
					{
						Send.ZC_NORMAL.Pet_AssociateHandleWorldId(character, companion);
						Send.ZC_OBJECT_PROPERTY(character.Connection, companion);
					}
					Send.ZC_NORMAL.PetInfo(character);
					character.ScheduleCompanionActivation();
				}

				Send.ZC_ANCIENT_CARD_RESET(conn);

				// Re-apply item scripts on login.
				// Adds Buffs and Stats?
				character.Inventory.ProcessEquipScripts();
				character.Inventory.ProcessCardScripts();

				// Send updates for the buffs loaded from db, so the client
				// will display the restored buffs
				foreach (var buff in character.Buffs.GetList())
					buff.NotifyUpdate();

				// Send updates for the cooldowns loaded from db, so the client
				// will display the restored cooldowns
				Send.ZC_COOLDOWN_LIST(character, character.Components.Get<CooldownComponent>().GetAll());

				// Handle party reconnection if the character has one
				if (conn.Party != null)
				{
					// Deletes the party if the leader is null for some reason (this may happens on instance dungeons parties)
					if (conn.Party.Owner == null)
					{
						var partyManager = ZoneServer.Instance.World.Parties;

						foreach (var partyMemberCharacter in conn.Party.GetPartyMembers())
						{
							conn.Party.RemoveMember(partyMemberCharacter);
						}

						// Removing party member above generates events, which
						// can also cause the party to become null here (deleted
						// elsewhere), so we make an additional party null
						// check before attempting to delete the party.
						if (conn.Party?.MemberCount == 0)
							partyManager.Delete(conn.Party);
					}
					else
					{
						Send.ZC_PARTY_INFO(character, conn.Party);
						Send.ZC_PARTY_LIST(conn.Party);
						conn.Party.UpdateMember(character, true);
					}
				}

				character.AddonMessage(AddonMessage.ENABLE_PCBANG_SHOP, null, 1);
				Send.ZC_PCBANG_SHOP_RENTAL(conn);

				character.AdventureBook.UpdateClient();
			}
			else
			{
				Send.ZC_IES_MODIFY_LIST(conn);
				Send.ZC_SESSION_OBJECTS(character);
				Send.ZC_WIKI_LIST(conn);
				Send.ZC_ITEM_INVENTORY_LIST(character);
				// ZC_NORMAL
				Send.ZC_OPTION_LIST(conn);
				Send.ZC_SKILLMAP_LIST(character);
				Send.ZC_ACHIEVE_POINT_LIST(character);
				Send.ZC_CHAT_MACRO_LIST(character);
				Send.ZC_UI_INFO_LIST(conn);
				Send.ZC_NPC_STATE_LIST(character);
				// ZC_HELP_LIST
				// ZC_MYPAGE_MAP
				// ZC_GUESTPAGE_MAP
				Send.ZC_START_INFO(conn);
				Send.ZC_ITEM_EQUIP_LIST(character);
				Send.ZC_SKILL_LIST(character);
				Send.ZC_ABILITY_LIST(character);
				Send.ZC_COOLDOWN_LIST(character, null);
				// ZC_NORMAL...
				if (Versions.Client >= KnownVersions.OpenBeta)
				{
					Send.ZC_QUICK_SLOT_LIST(character);
					Send.ZC_NORMAL.Unknown_9A(conn);
					Send.ZC_NORMAL.Unknown_CF(character);
					Send.ZC_NORMAL.Unknown_DE(conn);
					Send.ZC_NORMAL.Unknown_EB(character);
					Send.ZC_NORMAL.Unknown_128(character);
				}
				Send.ZC_START_GAME(conn);
				Send.ZC_OBJECT_PROPERTY(character);
				Send.ZC_LOGIN_TIME(conn, DateTime.Now);
				Send.ZC_MYPC_ENTER(character);
				// ZC_NORMAL...
				// ZC_OBJECT_PROPERTY...
				// ZC_SKILL_ADD...
				Send.ZC_JOB_PTS(character, character.Job);
				Send.ZC_MOVE_SPEED(character);

				// Re-apply item scripts on login.
				// Adds Buffs and Stats?
				character.Inventory.ProcessEquipScripts();
				character.Inventory.ProcessCardScripts();

				// Send companion info and schedule activation after scripts have set layer
				if (character.HasCompanions)
				{
					foreach (var companion in character.Companions.GetList())
					{
						Send.ZC_NORMAL.Pet_AssociateHandleWorldId(character, companion);
						Send.ZC_OBJECT_PROPERTY(character.Connection, companion);
					}
					Send.ZC_NORMAL.PetInfo(character);
					character.ScheduleCompanionActivation();
				}
			}

			character.IsWarping = false;
			character.OpenEyes();

			ZoneServer.Instance.ServerEvents.PlayerReady.Raise(new PlayerEventArgs(character));
		}

		/// <summary>
		/// Response to ZC_MOVE_ZONE that notifies us that the client is
		/// ready to move to the next zone.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_MOVE_ZONE_OK)]
		public void CZ_MOVE_ZONE_OK(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;

			if (character != null)
			{
				var enqueuedAt = Stopwatch.GetTimestamp();
				SaveQueue.Enqueue(() =>
				{
					var queuedMs = (Stopwatch.GetTimestamp() - enqueuedAt) * 1000 / Stopwatch.Frequency;
					var sw = Stopwatch.StartNew();
					character.FinalizeWarp();
					sw.Stop();
					Log.Info($"FinalizeWarp: '{character.Name}' (ID: {character.DbId}) completed in {sw.ElapsedMilliseconds}ms [queued={queuedMs}]");
				});
			}
		}

		/// <summary>
		/// Sent at the end of the loading screen.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CAMPINFO)]
		public void CZ_CAMPINFO(IZoneConnection conn, Packet packet)
		{
			var accountId = packet.GetLong();

			//Send.ZC_CAMPINFO(conn);
		}

		/// <summary>
		/// Sent when chatting publically.
		/// </summary>
		/// <remarks>
		/// Sent together with CZ_CHAT_LOG.
		/// </remarks>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CHAT)]
		public void CZ_CHAT(IZoneConnection conn, Packet packet)
		{
			var len = packet.GetShort(); // length of payload, without garbage
			var msg = packet.GetString();

			var character = conn.SelectedCharacter;

			// Try to execute message as a chat command, don't send if it
			// was handled as one
			if (ZoneServer.Instance.ChatCommands.TryExecute(character, msg))
				return;

			if (character.IsLocked(LockType.Speak))
			{
				character.ServerMessage(Localization.Get("You are not allowed to speak right now."));
				return;
			}

			Send.ZC_CHAT(character, msg);
			ZoneServer.Instance.ServerEvents.PlayerChat.Raise(new PlayerChatEventArgs(character, msg));
		}

		/// <summary>
		/// Sent when chatting.
		/// </summary>
		/// <remarks>
		/// Sent together with CZ_CHAT.
		/// When whispering only this one is sent?
		/// </remarks>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CHAT_LOG)]
		public void CZ_CHAT_LOG(IZoneConnection conn, Packet packet)
		{
			var len = packet.GetShort();
			var msg = packet.GetString();

			var character = conn.SelectedCharacter;

			if (ZoneServer.Instance.Conf.Log.LogChat)
			{
				LogChatType logChatType;
				string targetName = null;
				var msgParts = msg.Split(new[] { ' ' }, 3);

				// Determine chat type based on message prefix.
				if (msg.StartsWith("/p ", StringComparison.OrdinalIgnoreCase) || msg.StartsWith("/party ", StringComparison.OrdinalIgnoreCase))
				{
					logChatType = LogChatType.Party;
				}
				else if (msg.StartsWith("/g ", StringComparison.OrdinalIgnoreCase) || msg.StartsWith("/guild ", StringComparison.OrdinalIgnoreCase))
				{
					logChatType = LogChatType.Guild;
				}
				else if (msg.StartsWith("/w ", StringComparison.OrdinalIgnoreCase) || msg.StartsWith("/whisper ", StringComparison.OrdinalIgnoreCase))
				{
					logChatType = LogChatType.Whisper;
					// Extract target name for whispers.
					if (msgParts.Length > 1)
					{
						targetName = msgParts[1];
					}
				}
				else if (msg.StartsWith("/"))
				{
					logChatType = LogChatType.Command;
				}
				else
				{
					logChatType = LogChatType.Normal;
				}

				// Log the entire raw message for context.
				ZoneServer.Instance.Database.LogChat(character.DbId, character.Name, (int)logChatType, msg, targetName);
			}
		}

		/// <summary>
		/// Sent when clicking [Select Character].
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_MOVE_BARRACK)]
		public void CZ_MOVE_BARRACK(IZoneConnection conn, Packet packet)
		{
			var unkByte = packet.GetByte();
			var character = conn.SelectedCharacter;

			character.CancelOutOfBody();

			// Save character data now, before the client disconnects and
			// reconnects to the barracks server. The barracks generates a
			// new session key on login, which invalidates the old key and
			// would cause the deferred save in CleanUpAndSave to skip
			// saving entirely.
			conn.SaveAccountAndCharacter();
			character.SavedForWarp = true;

			Log.Info("User '{0}' is leaving for character selection.", conn.Account.Name);

			Send.ZC_SAVE_INFO(conn);
			Send.ZC_MOVE_BARRACK(conn);
			// TODO: Temp fix, essentially a race condition between Login States between Barracks and Zone Server.
			// Force the correct state before the client switches to Barracks.
			ZoneServer.Instance.Database.UpdateLoginState(conn.Account.Id, 0, LoginState.LoggedOut);
		}

		/// <summary>
		/// Sent when clicking [Logout].
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_LOGOUT)]
		public void CZ_LOGOUT(IZoneConnection conn, Packet packet)
		{
			var unkByte = packet.GetByte();

			conn.SelectedCharacter.CancelOutOfBody();

			if (conn.LoggedIn)
			{
				Log.Info("User '{0}' is logging out.", conn.Account.Name);

				Send.ZC_SAVE_INFO(conn);
				Send.ZC_LOGOUT_OK(conn);
			}
		}

		/// <summary>
		/// Sent when character jumps.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_JUMP)]
		public void CZ_JUMP(IZoneConnection conn, Packet packet)
		{
			var unkByte1 = packet.GetByte();
			if (Versions.Protocol > 500)
			{
				var position = packet.GetPosition();
				var direction = packet.GetDirection();
				var unkFloat = packet.GetFloat(); // timestamp?
				var bin = packet.GetBin(13);
				var unkByte2 = packet.GetByte();

				var character = conn.SelectedCharacter;

				if (character.IsDead)
					return;

				character.Movement.NotifyJump(position, direction, unkFloat, unkByte2);
			}
			else
			{
				var character = conn.SelectedCharacter;

				if (character.IsDead)
					return;

				character.Movement.NotifyJump(character.Position, character.Direction, (float)ZoneServer.Instance.World.WorldTime.Elapsed.TotalSeconds, unkByte1);
			}
		}

		/// <summary>
		/// Sent repeatedly while moving.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_KEYBOARD_MOVE)]
		public void CZ_KEYBOARD_MOVE(IZoneConnection conn, Packet packet)
		{
			if (Versions.Client < 354444)
				packet.GetByte(); // [i354444] Removed
			var position = packet.GetPosition();
			var direction = packet.GetDirection();
			if (Versions.Protocol < 500)
				packet.GetBin(6);
			var f1 = packet.GetFloat(); // timestamp?
			if (Versions.Protocol > 500)
				packet.GetBin(31);

			var character = conn.SelectedCharacter;

			if (!character.CanMove())
				return;

			if (character.IsDead)
			{
				//Log.Warning("CZ_KEYBOARD_MOVE: User '{0}' tried to move while dead.", conn.Account.Name);
				return;
			}

			if (character.IsWarping)
			{
				return;
			}

			character.Movement.NotifyMove(position, direction, f1);
			character.Components.Get<TimeActionComponent>().End(TimeActionResult.CancelledByMove);
		}

		/// <summary>
		/// Sent when stopping movement.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_MOVE_STOP)]
		public void CZ_MOVE_STOP(IZoneConnection conn, Packet packet)
		{
			var unkByte = packet.GetByte();
			var position = packet.GetPosition();
			var direction = packet.GetDirection();
			var unkFloat = packet.GetFloat(); // timestamp?

			var character = conn.SelectedCharacter;

			// Prevents a bug where players can move and stop moving while
			// talking to a warp multiple times making one of the stop move
			// packets be sent after they've warped on the next map causing
			// their position to be set wrongfully in the new map.
			if (character == null || character.IsWarping)
				return;

			// TODO: Sanity checks.

			character.Movement.NotifyStopMove(position, direction);
		}

		/// <summary>
		/// Sent when the character is in the air (jumping/falling).
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ON_AIR)]
		public void CZ_ON_AIR(IZoneConnection conn, Packet packet)
		{
			// TODO: Sanity checks.

			conn.SelectedCharacter.Movement.NotifyGrounded(false);
		}

		/// <summary>
		/// Sent when landing on the ground, after being in the air.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ON_GROUND)]
		public void CZ_ON_GROUND(IZoneConnection conn, Packet packet)
		{
			// TODO: Sanity checks.

			conn.SelectedCharacter.Movement.NotifyGrounded(true);
		}

		/// <summary>
		/// Sent repeatedly during jumping.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_MOVEMENT_INFO)]
		public void CZ_MOVEMENT_INFO(IZoneConnection conn, Packet packet)
		{
			var unkByte = packet.GetByte();
			var position = packet.GetPosition();

			var character = conn.SelectedCharacter;

			// TODO: Sanity checks.
			// TODO: Is there a broadcast for this?

			//conn.SelectedCharacter.SetPosition(position);
			if (character.Components.TryGet<AttachmentComponent>(out var attachment) && attachment.IsAttached)
				return;
			character?.Movement?.NotifyStopMove(position, character.Direction);
		}

		/// <summary>
		/// Sent when trying to sit down.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REST_SIT)]
		public void CZ_REST_SIT(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;

			// If currently riding, dismount instead of sitting
			if (character.IsRiding)
			{
				character.RemoveBuff(BuffId.RidingCompanion);
				return;
			}

			// Check if sitting is allowed
			if (!character.CanSit())
				return;

			// Toggle sitting buff
			character.ToggleBuff(BuffId.SitRest, 0, 0, TimeSpan.Zero, character);
		}

		/// <summary>
		/// Sent when equipping an item.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ITEM_EQUIP)]
		public void CZ_ITEM_EQUIP(IZoneConnection conn, Packet packet)
		{
			var worldId = packet.GetLong();
			var slot = (EquipSlot)packet.GetByte();

			var character = conn.SelectedCharacter;
			var result = character.Inventory.Equip(slot, worldId);

			if (result == InventoryResult.ItemNotFound)
				Log.Warning("CZ_ITEM_EQUIP: User '{0}' tried to equip item he doesn't have ({1}).", conn.Account.Name, worldId);
			else if (result == InventoryResult.InvalidSlot)
				Log.Warning("CZ_ITEM_EQUIP: User '{0}' tried to equip item in invalid slot ({1}).", conn.Account.Name, worldId);
		}

		/// <summary>
		/// Sent when uequipping an item.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ITEM_UNEQUIP)]
		public void CZ_ITEM_UNEQUIP(IZoneConnection conn, Packet packet)
		{
			var slot = (EquipSlot)packet.GetByte();

			var character = conn.SelectedCharacter;
			var result = character.Inventory.Unequip(slot);

			if (result == InventoryResult.ItemNotFound)
				Log.Warning("CZ_ITEM_UNEQUIP: User '{0}' tried to unequip non-existent item from {1}.", conn.Account.Name, slot);
			else if (result == InventoryResult.InvalidSlot)
				Log.Warning("CZ_ITEM_UNEQUIP: User '{0}' tried to unequip item from invalid slot ({1}).", conn.Account.Name, slot);
		}

		/// <summary>
		/// Request to unequip all equipped items.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_UNEQUIP_ITEM_ALL)]
		public void CZ_UNEQUIP_ITEM_ALL(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;
			character.Inventory.UnequipAll();
		}

		/// <summary>
		/// Sent when dropping an item from the inventory.
		/// This can also be sent when dragging skills from the skill tree,
		/// in which case there's no valid item and we silently ignore it.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ITEM_DROP)]
		public void CZ_ITEM_DROP(IZoneConnection conn, Packet packet)
		{
			// This method does not exist in Melia and in Laima
			// we detected it only happens on very specific scenarios such as
			// a player trying to drag and drop an equipment item from
			// a NPC shop after their inventory has been modified once whilst
			// the NPC shop window remains open. This packet is NOT sent by
			// client if the inventory is not modified first, leading me to
			// think this is actually a client bug and unused packet that we
			// should ignore and use CZ_ITEM_DELETE instead.
			return;
		}

		/// <summary>
		/// Sent when removing an item from the inventory.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ITEM_DELETE)]
		public void CZ_ITEM_DELETE(IZoneConnection conn, Packet packet)
		{
			var worldId = packet.GetLong();
			var amount = packet.GetInt();

			var character = conn.SelectedCharacter;

			// Ignore item delete requests while dead
			if (character.IsDead)
				return;

			var item = character.Inventory.GetItem(worldId);

			if (item == null)
			{
				Log.Warning("CZ_ITEM_DELETE: User '{0}' tried to delete non-existent item.", conn.Account.Name);
				return;
			}

			if (item.IsLocked)
			{
				// The client should stop the player from attempting to do this.
				Log.Warning("CZ_ITEM_DELETE: User '{0}' tried to delete locked item.", conn.Account.Name);
				return;
			}

			var fullStack = (amount >= item.Amount);

			var result = character.Inventory.Remove(item, amount, InventoryItemRemoveMsg.Destroyed);
			if (result != InventoryResult.Success)
			{
				Log.Warning("CZ_ITEM_DELETE: Removing an item for '{0}' failed despite checks.", conn.Account.Name);
				return;
			}

			// Drop item
			if (ZoneServer.Instance.Conf.World.Littering)
			{
				var dropDir = character.Direction;

				if (ZoneServer.Instance.Conf.World.TargetedLittering && character.Variables.Temp.Has("MouseX"))
				{
					var mouseX = character.Variables.Temp.Get("MouseX", 0f);
					var mouseY = character.Variables.Temp.Get("MouseY", 0f);
					var centerX = character.Variables.Temp.Get("ScreenWidth", 0f) / 2;
					var centerY = character.Variables.Temp.Get("ScreenHeight", 0f) / 2;

					dropDir = new Direction(mouseY - centerY, mouseX - centerX).AddDegreeAngle(-45);
				}

				// If the entire stack was discarded, we can simply drop
				// the item. If only a part of the stack was discarded,
				// we need to create a new stack, with the selected amount.
				// TODO: We might need to copy values and properties from
				//   the original stack to the new stack.
				var dropItem = (fullStack ? item : new Item(item.Id, amount));
				dropItem.SetRePickUpProtection(character);
				dropItem.Drop(character.Map, character.Position, dropDir, 30, character.Layer);
			}
		}

		/// <summary>
		/// Request to enchant an item.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_PREMIUM_ENCHANTCHIP)]
		public void CZ_PREMIUM_ENCHANTCHIP(IZoneConnection conn, Packet packet)
		{
			var itemId = packet.GetLong();
			var enchantId = packet.GetLong();

			var character = conn.SelectedCharacter;

			if (!character.Inventory.TryGetItem(itemId, out var item))
			{
				Log.Warning("CZ_PREMIUM_ENCHANTCHIP: User '{0}' tried to enchant a non-existent item.", conn.Account.Name);
				return;
			}
			if (!character.Inventory.TryGetItem(enchantId, out var enchantItem))
			{
				Log.Warning("CZ_PREMIUM_ENCHANTCHIP: User '{0}' tried to enchant with a non-existent item.", conn.Account.Name);
				return;
			}

			// Validate enchant item is an enchantchip
			if (!enchantItem.Data.ClassName.Contains("Enchantchip"))
			{
				Log.Warning("CZ_PREMIUM_ENCHANTCHIP: User '{0}' tried to use a non-enchant item.", conn.Account.Name);
				return;
			}

			// Validate item is headgear
			if (item.Data.EquipType1 != EquipType.Hat)
			{
				character.ServerMessage("Enchant scrolls can only be used on headgear.");
				return;
			}

			// Check potential
			if (item.Potential <= 0)
			{
				character.SystemMessage("NoMorePotential");
				return;
			}

			// Item Lock
			Send.ZC_EXEC_CLIENT_SCP(conn, string.Format(ClientScripts.REINFORCE_131014_ITEM_LOCK, itemId, "YES"));

			// Generate random hat options
			var minOptions = (int)enchantItem.Data.Script.NumArg1;
			var maxOptions = (int)enchantItem.Data.Script.NumArg2;
			if (minOptions <= 0) minOptions = 1;
			if (maxOptions <= 0) maxOptions = 2;
			item.GenerateRandomHatOptions(minOptions, maxOptions + 1);

			// Reduce potential by 1
			item.Properties.Modify(PropertyName.PR, -1);

			// Update item properties
			Send.ZC_OBJECT_PROPERTY(character.Connection, item);

			// Consume the enchant scroll
			character.Inventory.Remove(enchantItem.ObjectId);

			// Reset Item Lock
			Send.ZC_EXEC_CLIENT_SCP(conn, string.Format(ClientScripts.REINFORCE_131014_ITEM_LOCK, "None", "YES"));

			Send.ZC_EXEC_CLIENT_SCP(conn, string.Format(ClientScripts.HAIRENCHANT_SUCCESS, itemId, enchantItem.DbId));
		}

		/// <summary>
		/// Request to save a chat macro.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CHAT_MACRO)]
		public void CZ_CHAT_MACRO(IZoneConnection conn, Packet packet)
		{
			var index = packet.GetInt();
			var message = packet.GetString(128);
			var pose = packet.GetInt();

			if ((index > 10) || (index < 0))
			{
				Log.Warning("CZ_CHAT_MACRO: User '{0}' tried to save a chat macro for an invalid index ({1}).", conn.Account.Name, index);
				return;
			}

			// The client sends the entire list of chat macros each as a single packet.
			// Empty macros are also sent, but there's no reason to persist them.
			if (string.IsNullOrEmpty(message) && pose == 0)
				return;

			var macro = new ChatMacro(index, message, pose);
			conn.Account.AddChatMacro(macro);
		}

		/// <summary>
		/// Sent when dragging an item on top of another one in the same
		/// category to switch their positions.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SWAP_ETC_INV_CHANGE_INDEX)]
		public void CZ_SWAP_ETC_INV_CHANGE_INDEX(IZoneConnection conn, Packet packet)
		{
			var invType = (InventoryType)packet.GetByte();
			var worldId1 = packet.GetLong();
			var index1 = packet.GetInt();
			var worldId2 = packet.GetLong();
			var index2 = packet.GetInt();

			var character = conn.SelectedCharacter;
			var result = character.Inventory.Swap(worldId1, worldId2);

			if (result == InventoryResult.ItemNotFound)
				Log.Warning("CZ_SWAP_ETC_INV_CHANGE_INDEX: User '{0}' tried to swap non-existent item(s) ({1}, {2}).", conn.Account.Name, worldId1, worldId2);
			else if (result == InventoryResult.InvalidOperation)
				Log.Warning("CZ_SWAP_ETC_INV_CHANGE_INDEX: User '{0}' tried to swap two items from different categories ({1}, {2}).", conn.Account.Name, worldId1, worldId2);
		}

		/// <summary>
		/// Sent when clicken the Arrange Inventory button.
		/// </summary>
		/// <remarks>
		/// Named "CZ_SORT_INV_CHANGE_INDEX" (0xCE4) in iCBT1, size 11.
		/// Name changed to "CZ_SORT_INV" in iCBT2 (pre-launch),
		/// one byte added, presumedly for the order.
		/// </remarks>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SORT_INV, Op.CZ_SORT_INV_CHANGE_INDEX)]
		public void CZ_SORT_INV(IZoneConnection conn, Packet packet)
		{
			var unkByte = packet.GetByte();
			var order = InventoryOrder.Weight;
			if (Versions.Client >= 10622)
				order = (InventoryOrder)packet.GetByte(); // [i10622 (2015-10-22)] Added

			var character = conn.SelectedCharacter;

			// TODO: Add cooldown?

			character.Inventory.Sort(order);
		}

		/// <summary>
		/// Request to increase the size of a specific storage.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_EXTEND_WAREHOUSE)]
		public void CZ_EXTEND_WAREHOUSE(IZoneConnection conn, Packet packet)
		{
			var type = (InventoryType)packet.GetByte();

			var character = conn.SelectedCharacter;

			switch (type)
			{
				case InventoryType.PersonalStorage:
				{
					var storage = character.CurrentStorage as PersonalStorage;

					var result = storage.TryExtendStorage(PersonalStorage.ExtensionSize);
					if (result != StorageResult.Success)
						Log.Warning("CZ_EXTEND_WAREHOUSE: User '{0}' tried to extend their personal storage, but failed ({1}).", conn.Account.Name, result);
					break;
				}
				case InventoryType.TeamStorage:
				{
					var storage = character.CurrentStorage as TeamStorage;

					var result = storage.TryExtendStorage(TeamStorage.ExtensionSize);
					if (result != StorageResult.Success)
						Log.Warning("CZ_EXTEND_WAREHOUSE: User '{0}' tried to extend their team storage, but failed ({1}).", conn.Account.Name, result);
					break;
				}
				default:
				{
					character.ServerMessage(Localization.Get("Something went wrong while extending the storage, please report this issue."));
					Log.Warning("CZ_EXTEND_WAREHOUSE: User '{0}' tried to extend an unsupported warehouse type ({1}).", conn.Account.Name, type);
					break;
				}
			}
		}

		/// <summary>
		/// Sent on logout to save hotkeys.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_QUICKSLOT_LIST)]
		public void CZ_QUICKSLOT_LIST(IZoneConnection conn, Packet packet)
		{
			var packetSize = packet.GetShort();
			var compressedSize = packet.GetInt();

			var serialized = new StringBuilder("#");
			var rows = (byte)20;

			packet.UncompressData(compressedSize, p =>
			{
				rows = p.GetByte();

				for (var i = 0; i < 50; i++)
				{
					var type = (QuickSlotType)p.GetByte();
					var classId = p.GetInt();
					var objectId = p.GetLong();

					serialized.AppendFormat("{0},{1},{2}#", type, classId, objectId);
				}

				for (var i = 0; i < 4; i++)
				{
					var type = (QuickSlotType)p.GetByte();
					var classId = p.GetInt();
					var objectId = p.GetLong();

					serialized.AppendFormat("{0},{1},{2}#", type, classId, objectId);
				}

				var b3 = p.GetByte();
				var b4 = p.GetByte();
			});

			// What do you mean "this is a terrible way of saving the
			// hotkeys"? I bet this is how all great games do it! Yes!
			// I'm certain of it! There's absolutely no reason to refactor
			// any of this! It's perfect! Perfect, I tell you!

			var character = conn.SelectedCharacter;
			character.Variables.Perm.SetByte("Melia.QuickSlotRows", Math.Clamp(rows, (byte)20, (byte)40));
			character.Variables.Perm.SetString("Melia.QuickSlotList", serialized.ToString());
		}

		/// <summary>
		/// Sent when clicking a pose.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_POSE)]
		public void CZ_POSE(IZoneConnection conn, Packet packet)
		{
			var pose = packet.GetInt();
			var position = packet.GetPosition();
			var direction = packet.GetDirection();

			var character = conn.SelectedCharacter;

			// TODO: Sanity checks.
			if (character.IsDead)
				return;
			if (character.IsSitting)
				character.ToggleSitting();

			Send.ZC_POSE(character, pose);
		}

		/// <summary>
		/// Sent to rotate the character's body. 
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ROTATE)]
		public void CZ_ROTATE(IZoneConnection conn, Packet packet)
		{
			var i1 = packet.GetInt();
			var direction = packet.GetDirection();
			var character = conn.SelectedCharacter;

			character?.Rotate(direction);
		}

		/// <summary>
		/// Sent to rotate the character's head. 
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_HEAD_ROTATE)]
		public void CZ_HEAD_ROTATE(IZoneConnection conn, Packet packet)
		{
			var i1 = packet.GetInt();
			var direction = packet.GetDirection();
			var character = conn.SelectedCharacter;

			character?.RotateHead(direction);
		}

		/// <summary>
		/// Sent when the character requests to use an item.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ITEM_USE)]
		public void CZ_ITEM_USE(IZoneConnection conn, Packet packet)
		{
			var worldId = packet.GetLong();
			var handle = packet.GetInt();

			var character = conn.SelectedCharacter;
			var cooldowns = character.Components.Get<CooldownComponent>();


			// Don't think there's any phoenix down items.
			if (character.IsDead)
			{
				return;
			}

			// Get item
			var item = character.Inventory.GetItem(worldId);
			if (item == null)
			{
				Log.Warning("CZ_ITEM_USE: User '{0}' tried to use a non-existent item.", conn.Account.Name);
				return;
			}

			// Do not allow use of locked items
			if (item.IsLocked)
			{
				Log.Warning("CZ_ITEM_USE: User '{0}' tried to use a locked item.", conn.Account.Name);
				return;
			}

			// Cooldown sanity check, the client shouldn't allow this
			if (cooldowns.IsOnCooldown(item.Data.CooldownId))
			{
				Log.Warning("CZ_ITEM_USE: User '{0}' tried to use an item while its group was on cooldown.", conn.Account.Name);
				return;
			}

			// Don't need to log cards, because they don't have a script.
			if (item.Data.CardGroup != CardGroup.None)
				return;

			// Nothing to do if the item doesn't have a script
			if (!item.Data.HasScript)
			{
				Log.Warning("CZ_ITEM_USE: User '{0}' tried to use an item without script.", conn.Account.Name);
				return;
			}

			// Try to execute script
			var script = item.Data.Script;

			if (!ScriptableFunctions.Item.TryGet(script.Function, out var scriptFunc))
			{
				character.ServerMessage(Localization.Get("This item has not been implemented yet."));
				Log.Debug("CZ_ITEM_USE: Missing script function: {0}(\"{1}\", {2}, {3})", script.Function, script.StrArg, script.NumArg1, script.NumArg2);
				return;
			}

			try
			{
				var result = scriptFunc(character, item, script.StrArg, script.NumArg1, script.NumArg2);
				if (result == ItemUseResult.Fail)
				{
					character.ServerMessage(Localization.Get("Item usage failed."));
					return;
				}

				// Trigger UseItem card effects (e.g., potion use → buff)
				ItemHookRegistry.Instance.InvokeItemUseHooks(character, item);

				// Set cooldown if applicable
				if (item.Data.HasCooldown)
				{
					var cooldownTime = item.Data.CooldownTime;
					cooldownTime *= ZoneServer.Instance.Conf.World.ItemCooldownRate;

					if (cooldownTime > TimeSpan.Zero)
						cooldowns.Start(item.Data.CooldownId, cooldownTime);
				}

				// Remove consumeable items on success
				if (item.Data.Type == ItemType.Consume && result != ItemUseResult.OkayNotConsumed)
					character.Inventory.Remove(item, 1, InventoryItemRemoveMsg.Used);

				if (character.IsSitting)
					character.RemoveBuff(BuffId.SitRest);

				if (result == ItemUseResult.Okay)
				{
					Send.ZC_ITEM_USE(character, item.Id);

					// Track achievement points for item use (potions, etc.) via server event
					ZoneServer.Instance.ServerEvents.PlayerUsedItem.Raise(new PlayerUsedItemEventArgs(character, item));
				}
			}
			catch (BuffNotImplementedException ex)
			{
				character.ServerMessage(Localization.Get("This item has not been fully implemented yet."));
				Log.Debug("CZ_ITEM_USE: Buff handler '{4}' missing for script execution of '{0}(\"{1}\", {2}, {3})'", script.Function, script.StrArg, script.NumArg1, script.NumArg2, ex.BuffId);
			}
			catch (Exception ex)
			{
				character.ServerMessage(Localization.Get("Apologies, something went wrong there. Please report this issue."));
				Log.Debug("CZ_ITEM_USE: Exception while executing script function '{0}(\"{1}\", {2}, {3})': {4}", script.Function, script.StrArg, script.NumArg1, script.NumArg2, ex);
			}
		}

		/// <summary>
		/// Sent when the character requests to use an item.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ITEM_USE_TO_ITEM)]
		public void CZ_ITEM_USE_TO_ITEM(IZoneConnection conn, Packet packet)
		{
			var item1WorldId = packet.GetLong();
			var item2WorldId = packet.GetLong();
			var unkInt = packet.GetInt();

			var character = conn.SelectedCharacter;

			// Get item
			var itemUsed = character.Inventory.GetItem(item1WorldId);
			if (itemUsed == null)
			{
				Log.Warning("CZ_ITEM_USE_TO_ITEM: User '{0}' tried to use a non-existent item.", conn.Account.Name);
				return;
			}

			// Do not allow use of locked items
			if (itemUsed.IsLocked)
			{
				Log.Warning("CZ_ITEM_USE_TO_ITEM: User '{0}' tried to use a locked item.", conn.Account.Name);
				return;
			}

			// Get item
			var itemUsedOn = character.Inventory.GetItem(item2WorldId);
			if (itemUsedOn == null)
			{
				Log.Warning("CZ_ITEM_USE_TO_ITEM: User '{0}' tried to use a non-existent item.", conn.Account.Name);
				return;
			}

			// Do not allow use of locked items
			if (itemUsedOn.IsLocked)
			{
				Log.Warning("CZ_ITEM_USE_TO_ITEM: User '{0}' tried to use a locked item.", conn.Account.Name);
				return;
			}

			if (itemUsed.Data.Group == ItemGroup.MagicAmulet)
			{
				// Magic amulet socketing
				if (itemUsedOn.Data.Type != ItemType.Equip)
				{
					character.SystemMessage("NoMoreSocket");
					return;
				}

				// Check if target has potential
				var potential = itemUsedOn.Properties.GetFloat(PropertyName.PR);
				if (potential <= 0)
				{
					character.SystemMessage("NoMorePotential");
					return;
				}

				// Find empty socket
				var maxSockets = (int)itemUsedOn.Properties.GetFloat(PropertyName.MaxSocket_MA, 0);
				var socketIndex = -1;
				for (var i = 0; i < maxSockets; i++)
				{
					var socketProp = $"MagicAmulet_{i}";
					if (itemUsedOn.Properties.GetFloat(socketProp, 0) == 0)
					{
						socketIndex = i;
						break;
					}
				}

				if (socketIndex == -1)
				{
					character.SystemMessage("NoMoreSocket");
					return;
				}

				// Socket the amulet
				var socketProperty = $"MagicAmulet_{socketIndex}";
				itemUsedOn.Properties.SetFloat(socketProperty, itemUsed.Id);
				itemUsedOn.Properties.SetFloat(PropertyName.PR, potential - 1);

				character.Inventory.Remove(item1WorldId);
				Send.ZC_OBJECT_PROPERTY(conn, itemUsedOn, socketProperty, PropertyName.PR);
			}
			else if (itemUsed.Data.Group == ItemGroup.Gem)
			{
				if (Feature.IsEnabled("GemRevamp"))
				{
					if ((itemUsed.Id < 643506 && (itemUsedOn.Data.Group != ItemGroup.Weapon && itemUsedOn.Data.Group != ItemGroup.SubWeapon))
						|| (itemUsed.Id > 643505 && (itemUsedOn.Data.Group == ItemGroup.Weapon || itemUsedOn.Data.Group == ItemGroup.SubWeapon)))
					{
						character.SystemMessage("GemNotEquip", false);
						return;
					}
				}

				itemUsedOn.SocketGem(itemUsed);
				character.Inventory.Remove(item1WorldId);
				Send.ZC_EQUIP_GEM_INFO(character);
			}
			else if (itemUsed.Data.ClassName.Contains("Enchantchip"))
			{
				// Enchant scrolls can only be used on headgear (hats)
				if (itemUsedOn.Data.EquipType1 != EquipType.Hat)
				{
					character.ServerMessage("Enchant scrolls can only be used on headgear.");
					return;
				}

				// Check potential
				if (itemUsedOn.Potential <= 0)
				{
					character.SystemMessage("NoMorePotential");
					return;
				}

				// Get min/max options from item script args
				var minOptions = (int)itemUsed.Data.Script.NumArg1;
				var maxOptions = (int)itemUsed.Data.Script.NumArg2;
				if (minOptions <= 0) minOptions = 1;
				if (maxOptions <= 0) maxOptions = 2;

				itemUsedOn.GenerateRandomHatOptions(minOptions, maxOptions + 1);
				itemUsedOn.Properties.Modify(PropertyName.PR, -1);
				character.Inventory.Remove(item1WorldId);
				Send.ZC_OBJECT_PROPERTY(conn, itemUsedOn);
			}
			else
			{
				Log.Warning("CZ_ITEM_USE_TO_ITEM: User '{0}' used item to item unsupported use type {1}.", conn.Account.Name, itemUsed.Data.Group);
				return;
			}
		}

		/// <summary>
		/// Sent when "clicking" an NPC.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CLICK_TRIGGER)]
		public void CZ_CLICK_TRIGGER(IZoneConnection conn, Packet packet)
		{
			var handle = packet.GetInt();
			var unkByte = packet.GetByte();

			var character = conn.SelectedCharacter;

			if (character.IsOutOfBody())
				return;

			var monster = character.Map.GetMonster(handle);

			if (monster == null)
			{
				Log.Warning("CZ_CLICK_TRIGGER: User '{0}' tried to talk to unknown monster.", conn.Account.Name);
				return;
			}

			// Enemies usually can't be talked to unless specific scripts enable it.
			if (monster.MonsterType == RelationType.Enemy)
			{
				// Optional: Check if the enemy specifically has a DialogComponent enabled
				// For now, adhere to standard logic:
				Log.Warning("CZ_CLICK_TRIGGER: User '{0}' tried to talk to an actual monster.", conn.Account.Name);
				return;
			}

			// Attempt to get the dialog component
			if (string.IsNullOrEmpty(monster.DialogName))
			{
				Log.Warning("CZ_CLICK_TRIGGER: User '{0}' tried to talk to a monster without dialog.", conn.Account.Name);
				return;
			}

			// Check for existing dialogs
			if (conn.CurrentDialog != null && conn.CurrentDialog.State != DialogState.Ended)
			{
				// Don't acutally log this, as it might happen naturally.
				//Log.Debug("CZ_CLICK_TRIGGER: User '{0}' is already in a dialog.", conn.Account.Name);
				return;
			}

			character.StartDialog(monster);
		}

		/// <summary>
		/// Sent when selecting a dialog option or entering a number into a
		/// number range dialog.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_DIALOG_SELECT)]
		public void CZ_DIALOG_SELECT(IZoneConnection conn, Packet packet)
		{
			var option = packet.GetByte();

			// Check state
			if (conn.CurrentDialog == null)
			{
				Log.Debug("CZ_DIALOG_SELECT: User '{0}' is not in a dialog.", conn.Account.Name);
				return;
			}

			// Resume dialog with the option as a string. We use a string
			// because we can use one method for both selections and inputs
			// this way.
			conn.CurrentDialog.Resume(option.ToString(), DialogResponseType.Select);
		}

		/// <summary>
		/// Sent to continue dialog or close storage.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_DIALOG_ACK)]
		public void CZ_DIALOG_ACK(IZoneConnection conn, Packet packet)
		{
			var ack = (DialogAcknowledgement)packet.GetInt();

			var character = conn.SelectedCharacter;
			var storage = character.CurrentStorage;

			// If storage was open, close it
			if (storage.IsBrowsing && ack == DialogAcknowledgement.Okay)
			{
				storage.Close();
				conn.CurrentDialog?.Cancel();
				conn.CurrentDialog = null;
				return;
			}

			// Cutscene Tracker
			if (character.Tracks.ActiveTrack != null)
			{
				var track = character.Tracks.ActiveTrack;
				Send.ZC_DIALOG_CLOSE(conn);
				Send.ZC_NORMAL.SetTrackFrame(character, track.Frame);

				return;
			}

			// Check state
			if (conn.CurrentDialog == null)
			{
				// Handle MeliaCustomShop dialog close - buyer has ActiveShop but no CurrentDialog
				if (conn.ActiveShop != null)
				{
					conn.ActiveShop = null;
					Send.ZC_DIALOG_CLOSE(conn);
					Send.ZC_LEAVE_TRIGGER(conn);
					Send.ZC_ENABLE_CONTROL(conn, "AUTOSELLER", true);
					Send.ZC_LOCK_KEY(character, "AUTOSELLER", false);
				}
				// Don't log, can happen due to key spamming at the end
				// of a dialog.
				//Log.Debug("CZ_DIALOG_ACK: User '{0}' is not in a dialog.", conn.Account.Name);
				return;
			}

			// The type seems to indicate what the client wants to do,
			// 1 being sent when continuing normally and 0 or -1 when
			// escape is pressed, to cancel the dialog.
			if (ack == DialogAcknowledgement.Okay)
			{
				conn.CurrentDialog.Resume(null, DialogResponseType.Ack);
			}
			else
			{
				Send.ZC_DIALOG_CLOSE(conn);
				Send.ZC_LEAVE_TRIGGER(conn);
				conn.CurrentDialog?.Cancel();
				conn.CurrentDialog = null;
			}
		}

		/// <summary>
		/// Sent after entering a string in an input dialog.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_DIALOG_STRINGINPUT)]
		public void CZ_DIALOG_STRINGINPUT(IZoneConnection conn, Packet packet)
		{
			var input = packet.GetString(128);

			// Check state
			if (conn.CurrentDialog == null)
			{
				Log.Debug("CZ_DIALOG_STRINGINPUT: User '{0}' is not in a dialog.", conn.Account.Name);
				return;
			}

			conn.CurrentDialog.Resume(input, DialogResponseType.StringInput);
		}

		/// <summary>
		/// Sent when changing an option in the settings. Or when the
		/// client randomly decides to spam you with all options.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CHANGE_CONFIG)]
		public void CZ_CHANGE_CONFIG(IZoneConnection conn, Packet packet)
		{
			var optionId = (AccountOptionId)packet.GetInt();
			var value = packet.GetInt();

			if (!Enum.IsDefined(typeof(AccountOptionId), optionId))
			{
				Log.Debug("CZ_CHANGE_CONFIG: Unknown account option '{0}'.", optionId);
				return;
			}

			conn.Account.Settings.Set(optionId, value);
		}

		/// <summary>
		/// ?
		/// </summary>
		/// <remarks>
		/// This packet is spammed near the warp from Siauliai to Kaipeda,
		/// purpose unknown. I guess it expects some kind of response,
		/// more research required.
		/// </remarks>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REVEAL_NPC_STATE)]
		public void CZ_REVEAL_NPC_STATE(IZoneConnection conn, Packet packet)
		{
			var unkInt = packet.GetInt();
		}

		/// <summary>
		/// Sent when attacking enemies.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CLIENT_HIT_LIST)]
		public void CZ_CLIENT_HIT_LIST(IZoneConnection conn, Packet packet)
		{
			var packetSize = packet.GetShort();
			var i1 = packet.GetInt();
			var targetHandleCount = packet.GetInt();
			var originPos = packet.GetPosition();
			var farPos = packet.GetPosition();
			var direction = packet.GetDirection();
			var skillId = (SkillId)packet.GetInt();
			var b1 = packet.GetByte();
			var f3 = packet.GetFloat();
			var speedRate = packet.GetFloat();
			float hitDelay = 0;
			if (Versions.Protocol > 500)
				hitDelay = packet.GetFloat();
			var targetHandles = packet.GetList(targetHandleCount, packet.GetInt);

			var character = conn.SelectedCharacter;

			// Check skill
			if (!character.Skills.TryGet(skillId, out var skill))
			{
				Log.Warning("CZ_CLIENT_HIT_LIST: User '{0}' tried to use a skill they don't have ({1}).", conn.Account.Name, skillId);
				return;
			}

			// Check cooldown
			if (skill.IsOnCooldown)
			{
				Log.Warning("CZ_CLIENT_HIT_LIST: User '{0}' tried to use a skill that's on cooldown ({1}).", conn.Account.Name, skillId);
				character.ServerMessage(Localization.Get("You may not use this yet."));
				return;
			}

			// Get targets
			var targets = new List<ICombatEntity>();
			foreach (var handle in targetHandles)
			{
				if (!character.Map.TryGetCombatEntity(handle, out var target))
				{
					Log.Warning("CZ_CLIENT_HIT_LIST: User '{0}' tried to attack non-existant target '{1}'.", conn.Account.Name, handle);
					continue;
				}

				if (!character.CanAttack(target))
				{
					Log.Warning("CZ_CLIENT_HIT_LIST: User '{0}' tried to attack invalid target '{1}'.", conn.Account.Name, handle);
					continue;
				}

				targets.Add(target);
			}

			// Try to use skill
			try
			{
				character.Direction = direction;
				switch (skill.Data.UseType)
				{
					case SkillUseType.MeleeGround:
					{
						if (!ZoneServer.Instance.SkillHandlers.TryGetHandler<IMeleeGroundSkillHandler>(skillId, out var handler))
						{
							character.ServerMessage(Localization.Get("This skill has not been implemented yet."));
							Log.Warning("CZ_CLIENT_HIT_LIST: No handler for skill '{0}' found.", skillId);
							return;
						}

						handler.Handle(skill, character, originPos, farPos, targets.ToArray());
						break;
					}
					case SkillUseType.Force:
					{
						if (!ZoneServer.Instance.SkillHandlers.TryGetHandler<IForceSkillHandler>(skillId, out var handler))
						{
							character.ServerMessage(Localization.Get("This skill has not been implemented yet."));
							Log.Warning("CZ_CLIENT_HIT_LIST: No handler for skill '{0}' found.", skillId);
							return;
						}

						handler.Handle(skill, character, originPos, farPos, targets.FirstOrDefault());
						break;
					}
					default:
					{
						Log.Warning("CZ_CLIENT_HIT_LIST: User '{0}' tried to use skill '{1}' of unknown use type '{2}'.", conn.Account.Name, skillId, skill.Data.UseType);
						break;
					}
				}
			}
			catch (ArgumentException ex)
			{
				Log.Error("CZ_CLIENT_HIT_LIST: Failed to execute the handler for '{0}'. Error: {1}", skillId, ex);
			}
		}

		/// <summary>
		/// Sent when attacking a target with a skill, incl. the default
		/// magic attack and bows.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SKILL_TARGET)]
		public void CZ_SKILL_TARGET(IZoneConnection conn, Packet packet)
		{
			var b1 = packet.GetByte();
			var skillId = (SkillId)packet.GetInt();
			var targetHandle = packet.GetInt();
			var b2 = packet.GetByte();

			var character = conn.SelectedCharacter;

			// Check skill
			if (!character.Skills.TryGet(skillId, out var skill))
			{
				Log.Warning("CZ_SKILL_TARGET: User '{0}' tried to use a skill they don't have ({1}).", conn.Account.Name, skillId);
				return;
			}

			// Check cooldown
			if (skill.IsOnCooldown)
			{
				Log.Warning("CZ_SKILL_TARGET: User '{0}' tried to use a skill that's on cooldown ({1}).", conn.Account.Name, skillId);
				character.ServerMessage(Localization.Get("You may not use this yet."));
				return;
			}

			// Check target
			// TODO: Should the target be checked properly? Is it possible
			//   to use this handler without target? We should document
			//   such things.
			// var target = character.Map.GetCombatEntity(targetHandle);
			if (!character.Map.TryGetCombatEntity(targetHandle, out var target))
			{
				Log.Warning("CZ_SKILL_TARGET: User '{0}' tried to use a skill on a non-existing target.", conn.Account.Name);
				return;
			}

			// Try to use skill
			try
			{
				switch (skill.Data.UseType)
				{
					case SkillUseType.Force:
					{
						if (!ZoneServer.Instance.SkillHandlers.TryGetHandler<IForceSkillHandler>(skillId, out var handler))
						{
							character.ServerMessage(Localization.Get("This skill has not been implemented yet."));
							Log.Warning("CZ_SKILL_TARGET: No force skill handler for skill '{0}' found.", skillId);
							return;
						}
						handler.Handle(skill, character, character.Position, target.Position, target);
						break;
					}
					default:
					{
						if (!ZoneServer.Instance.SkillHandlers.TryGetHandler<ITargetSkillHandler>(skillId, out var handler))
						{
							character.ServerMessage(Localization.Get("This skill has not been implemented yet."));
							Log.Warning("CZ_SKILL_TARGET: No handler for skill '{0}' found.", skillId);
							return;
						}

						handler.Handle(skill, character, target);
						break;
					}
				}
			}
			catch (ArgumentException ex)
			{
				Log.Error("CZ_SKILL_TARGET: Failed to execute the handler for '{0}'. Error: {1}", skillId, ex);
			}
		}

		/// <summary>
		/// Sent when attacking a target with a skill, incl. the default
		/// magic attack and bows.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SKILL_TARGET_ANI)]
		public void CZ_SKILL_TARGET_ANI(IZoneConnection conn, Packet packet)
		{
			var b1 = packet.GetByte();
			var skillId = (SkillId)packet.GetInt();
			var direction = packet.GetDirection();

			var character = conn.SelectedCharacter;

			// The packet is sent after the attack animation of the default
			// magic attack when there's no target. In this case skillId is
			// 0. It's currently unknown what exactly is supposed to happen
			// in that case, but we probably don't want to execute the skill
			// handler.
			if (skillId == 0)
			{
				// Cancel the attack
				Send.ZC_NORMAL.SkillTargetAnimationEnd(character);
				return;
			}

			// Check skill
			if (!character.Skills.TryGet(skillId, out var skill))
			{
				Log.Warning("CZ_SKILL_TARGET_ANI: User '{0}' tried to use a skill they don't have ({1}).", conn.Account.Name, skillId);
				return;
			}

			// Try to use skill
			try
			{
				character.Direction = direction;
				switch (skill.Data.UseType)
				{
					case SkillUseType.Force:
					{
						if (!ZoneServer.Instance.SkillHandlers.TryGetHandler<IForceSkillHandler>(skillId, out var handler))
						{
							character.ServerMessage(Localization.Get("This skill has not been implemented yet."));
							Log.Warning("CZ_SKILL_TARGET: No force skill handler for skill '{0}' found.", skillId);
							return;
						}
						handler.Handle(skill, character, character.Position, character.Position, null);
						break;
					}
					default:
					{
						if (!ZoneServer.Instance.SkillHandlers.TryGetHandler<ITargetSkillHandler>(skillId, out var handler))
						{
							character.ServerMessage(Localization.Get("This skill has not been implemented yet."));
							Log.Warning("CZ_SKILL_TARGET_ANI: No handler for skill '{0}' found.", skillId);
							return;
						}

						handler.Handle(skill, character, null);
						break;
					}
				}
			}
			catch (ArgumentException ex)
			{
				Log.Error("CZ_SKILL_TARGET_ANI: Failed to execute the handler for '{0}'. Error: {1}", skillId, ex);
			}
		}

		/// <summary>
		/// This packet is used to cast skills in the ground.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SKILL_GROUND)]
		public void CZ_SKILL_GROUND(IZoneConnection conn, Packet packet)
		{
			var unk1 = packet.GetByte();
			var id = packet.GetLong();
			var originPos = packet.GetPosition();
			var farPos = packet.GetPosition();
			var direction = packet.GetDirection();
			var targetHandle = packet.GetInt();
			var i1 = packet.GetInt();
			var unk2 = packet.GetByte();
			var i2 = packet.GetInt();

			var character = conn.SelectedCharacter;
			var skillId = SkillId.None;
			Skill skill;

			if (id >= ObjectIdRanges.Items && character.Inventory.TryGetItem(id, out var item))
			{
				item.Properties.TryGetFloat(PropertyName.SkillType, out var skillType);
				skillId = (SkillId)skillType;
				item.Properties.TryGetFloat(PropertyName.SkillLevel, out var skillLevel);

				if (skillId == SkillId.None && item.Data.ClassName.StartsWith("Scroll_SkillItem_"))
					Enum.TryParse(item.Data.ClassName.Substring("Scroll_SkillItem_".Length), out skillId);
				if (skillLevel == 0)
				{
					var skillTree = ZoneServer.Instance.Data.SkillTreeDb.Find(entry => entry.SkillId == skillId);
					skillLevel = skillTree?.MaxLevel ?? 1;
				}

				skill = new Skill(character, skillId, (int)skillLevel, isItemSkill: true);
				character.Inventory.Remove(id, 1, InventoryItemRemoveMsg.Used);
			}
			else
			{
				skillId = (SkillId)id;

				// Check skill
				if (!character.Skills.TryGet(skillId, out skill))
				{
					Log.Warning("CZ_SKILL_GROUND: User '{0}' tried to use a skill they don't have ({1}).", conn.Account.Name, skillId);
					return;
				}
			}

			// Check cooldown
			if (skill.IsOnCooldown)
			{
				Log.Warning("CZ_SKILL_GROUND: User '{0}' tried to use a skill that's on cooldown ({1}).", conn.Account.Name, skillId);
				character.ServerMessage(Localization.Get("You may not use this yet."));
				return;
			}

			// Probably just a sanity check since the client doesn't allow this.
			if (!skill.HasRequiredStance)
			{
				character.ServerMessage(Localization.Get("Cannot be used with this weapon."));
				return;
			}

			// Check target
			ICombatEntity target = null;
			if (targetHandle != 0 && character.Map.TryGetActor(targetHandle, out var actor) && actor is ICombatEntity ce)
				target = ce;

			// Try to use skill
			try
			{
				character.Direction = direction;
				switch (skill.Data.UseType)
				{
					case SkillUseType.ForceGround:
					{
						if (!ZoneServer.Instance.SkillHandlers.TryGetHandler<IForceGroundSkillHandler>(skillId, out var handler))
						{
							character.ServerMessage(Localization.Get("This skill has not been implemented yet."));
							Log.Warning("CZ_SKILL_GROUND: No force ground handler for skill '{0}' found.", skillId);
							return;
						}

						skill.PrepareCancellation();

						handler.Handle(skill, character, originPos, farPos, target);
						break;
					}
					case SkillUseType.MeleeGround:
					{
						if (!ZoneServer.Instance.SkillHandlers.TryGetHandler<IMeleeGroundSkillHandler>(skillId, out var handler))
						{
							character.ServerMessage(Localization.Get("This skill has not been implemented yet."));
							Log.Warning("CZ_SKILL_GROUND: No melee ground handler for skill '{0}' found.", skillId);
							return;
						}

						skill.PrepareCancellation();

						handler.Handle(skill, character, originPos, farPos, target);
						break;
					}
					default:
					{
						character.ServerMessage(Localization.Get("This skill has not been implemented yet."));
						Log.Warning("CZ_SKILL_GROUND: No handler for skill '{0}' found.", skillId);
						break;
					}
				}
			}
			catch (ArgumentException ex)
			{
				Log.Error("CZ_SKILL_GROUND: Failed to execute the handler for '{0}'. Error: {1}", skillId, ex);
			}
		}

		/// <summary>
		/// Request from a player to use a skill on their own character.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SKILL_SELF)]
		public void CZ_SKILL_SELF(IZoneConnection conn, Packet packet)
		{
			var b1 = packet.GetByte();
			var skillId = (SkillId)packet.GetInt();
			var originPos = packet.GetPosition();
			var direction = packet.GetDirection();
			var b2 = packet.GetByte();

			var character = conn.SelectedCharacter;

			// Check skill
			if (!character.Skills.TryGet(skillId, out var skill))
			{
				Log.Warning("CZ_SKILL_SELF: User '{0}' tried to use a skill they don't have ({1}).", conn.Account.Name, skillId);
				return;
			}

			// Check cooldown
			if (skill.IsOnCooldown)
			{
				Log.Warning("CZ_SKILL_SELF: User '{0}' tried to use a skill that's on cooldown ({1}).", conn.Account.Name, skillId);
				character.ServerMessage(Localization.Get("You may not use this yet."));
				return;
			}

			// Try to use skill
			try
			{
				if (!ZoneServer.Instance.SkillHandlers.TryGetHandler<ISelfSkillHandler>(skillId, out var handler))
				{
					character.ServerMessage(Localization.Get("This skill has not been implemented yet."));
					Log.Warning("CZ_SKILL_SELF: No handler for skill '{0}' found.", skillId);
					return;
				}

				handler.Handle(skill, character, originPos, direction);
			}
			catch (ArgumentException ex)
			{
				Log.Error("CZ_SKILL_SELF: Failed to execute the handler for '{0}'. Error: {1}", skillId, ex);
			}
		}

		/// <summary>
		/// Sent when character starts casting a hold to cast skill.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_DYNAMIC_CASTING_START)]
		public void CZ_DYNAMIC_CASTING_START(IZoneConnection conn, Packet packet)
		{
			var skillId = (SkillId)packet.GetInt();
			var maxCastTime = packet.GetFloat();

			var character = conn.SelectedCharacter;

			// Disconnected player?
			if (character == null)
				return;

			// Check skill
			if (!character.Skills.TryGet(skillId, out var skill))
			{
				Log.Warning("CZ_DYNAMIC_CASTING_START: User '{0}' tried to use a skill they don't have ({1}).", conn.Account.Name, skillId);
				return;
			}

			if (!ZoneServer.Instance.SkillHandlers.TryGetHandler<IDynamicCasted>(skillId, out var handler))
			{
				character.ServerMessage(Localization.Get("This skill has not been implemented yet."));
				Log.Warning("CZ_DYNAMIC_CASTING_START: No handler for skill '{0}' found.", skillId);
				return;
			}

			character.SetCastingState(true, skill);
			character.Variables.Temp.Set("Melia.Cast.Skill", skill);
			handler.StartDynamicCast(skill, character, maxCastTime);
		}

		/// <summary>
		/// Sent when character casting ends after holding to cast skill.
		/// This is sent even if the skill is held to the maximum duration.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_DYNAMIC_CASTING_END)]
		public void CZ_DYNAMIC_CASTING_END(IZoneConnection conn, Packet packet)
		{
			var skillId = (SkillId)packet.GetInt();
			var castTime = packet.GetFloat(); // Max Cast Hold Time?

			var character = conn.SelectedCharacter;

			// Check skill
			if (!character.Skills.TryGet(skillId, out var skill))
			{
				Log.Warning("CZ_DYNAMIC_CASTING_END: User '{0}' tried to cast a skill they don't have ({1}).", conn.Account.Name, skillId);
				return;
			}

			if (!ZoneServer.Instance.SkillHandlers.TryGetHandler<IDynamicCasted>(skillId, out var handler))
			{
				character.ServerMessage(Localization.Get("This skill has not been implemented yet."));
				Log.Warning("CZ_DYNAMIC_CASTING_END: No handler for skill '{0}' found.", skillId);
				return;
			}

			character.SetCastingState(false, skill);
			handler.EndDynamicCast(skill, character, castTime);
			character.Variables.Temp.Remove("Melia.Cast.Skill");
		}

		/// <summary>
		/// Dummy Handler
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SKILL_CANCEL_SCRIPT)]
		public void CZ_SKILL_CANCEL_SCRIPT(IZoneConnection conn, Packet packet)
		{
			if (Versions.Protocol > 500)
			{
				var skillId = (SkillId)packet.GetInt();

				var character = conn.SelectedCharacter;

				// Check skill
				if (!character.Skills.TryGet(skillId, out var skill))
				{
					Log.Warning("CZ_SKILL_CANCEL_SCRIPT: User '{0}' tried to use a skill they don't have ({1}).", conn.Account.Name, skillId);
					return;
				}

				if (!ZoneServer.Instance.SkillHandlers.TryGetHandler<ICancelSkillHandler>(skillId, out var handler))
				{
					character.ServerMessage(Localization.Get("This skill has not been implemented yet."));
					Log.Warning("CZ_SKILL_CANCEL_SCRIPT: No handler for skill '{0}' found.", skillId);
					return;
				}

				handler.Handle(skill, character);
			}
		}

		/// <summary>
		/// Sent when character is using the ground position selection tool
		/// starts.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SELECT_GROUND_POS_START)]
		public void CZ_SELECT_GROUND_POS_START(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;

			// TODO: keep track of state?
		}

		/// <summary>
		/// Sent when character is using the ground position selection tool ends
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SELECT_GROUND_POS_END)]
		public void CZ_SELECT_GROUND_POS_END(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;

			// TODO: keep track of state?
		}

		/// <summary>
		/// Sent after selecting a target ground position for a skill.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SKILL_TOOL_GROUND_POS)]
		public void CZ_SKILL_TOOL_GROUND_POS(IZoneConnection conn, Packet packet)
		{
			var pos = packet.GetPosition();
			var skillId = (SkillId)packet.GetInt();
			var b1 = packet.GetByte();

			var character = conn.SelectedCharacter;

			if (!character.Skills.TryGet(skillId, out var skill))
			{
				Log.Warning("CZ_SKILL_TOOL_GROUND_POS: User '{0}' tried to send a position for a skill they don't have.");
				return;
			}

			skill.Vars.Set("Melia.ToolGroundPos", pos);
		}

		/// <summary>
		/// Sent when opening storage and requesting item list in the storage.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_ITEM_LIST)]
		public void CZ_REQ_ITEM_LIST(IZoneConnection conn, Packet packet)
		{
			var type = (InventoryType)packet.GetByte();

			var character = conn.SelectedCharacter;

			if (type == InventoryType.PersonalStorage && character.CurrentStorage is PersonalStorage storage && storage.IsBrowsing)
			{
				var items = storage.GetItems();
				Send.ZC_SOLD_ITEM_DIVISION_LIST(character, type, items);
				foreach (var socketedItems in items.Values.Where(a => a.HasSockets))
					Send.ZC_EQUIP_GEM_INFO(character, socketedItems);
			}
			else if (type == InventoryType.TeamStorage && character.CurrentStorage is TeamStorage teamStorage && teamStorage.IsBrowsing)
			{
				var items = teamStorage.GetItems();
				Send.ZC_SOLD_ITEM_DIVISION_LIST(character, type, items);
				foreach (var socketedItems in items.Values.Where(a => a.HasSockets))
					Send.ZC_EQUIP_GEM_INFO(character, socketedItems);
			}
			else
				Send.ZC_SOLD_ITEM_DIVISION_LIST(character, type, new Dictionary<int, Item>());
		}

		/// <summary>
		/// Sent when retrieving or storing items to storage.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_WAREHOUSE_CMD)]
		public void CZ_WAREHOUSE_CMD(IZoneConnection conn, Packet packet)
		{
			var type = (InventoryType)packet.GetByte();
			var worldId = packet.GetLong();
			var i1 = packet.GetInt();
			var amount = packet.GetInt();
			var i2 = packet.GetInt();
			var interaction = (StorageInteraction)packet.GetByte();

			var character = conn.SelectedCharacter;

			if (!Enum.IsDefined(typeof(StorageInteraction), interaction))
			{
				Log.Warning("CZ_WAREHOUSE_CMD: No valid interaction type for value: '{0}'", interaction);
				return;
			}

			if (type == InventoryType.PersonalStorage)
			{
				var inventory = character.Inventory;
				var storage = character.CurrentStorage as PersonalStorage;

				var interactionCost = ZoneServer.Instance.Conf.World.StorageFee;
				var silver = inventory.CountItem(ItemId.Silver);

				if (silver < interactionCost)
				{
					Log.Warning("CZ_WAREHOUSE_CMD: User '{0}' tried to store or retrieve storage items without silver", conn.Account.Name);
					return;
				}

				if (storage == null)
				{
					Log.Warning("CZ_WAREHOUSE_CMD: User '{0}' tried to manage their personal storage wrong storage type open.", conn.Account.Name);
					return;
				}

				if (!storage.IsBrowsing)
				{
					Log.Warning("CZ_WAREHOUSE_CMD: User '{0}' tried to manage their personal storage without it being open.", conn.Account.Name);
					return;
				}

				if (interaction == StorageInteraction.Store && storage.StoreItem(worldId, amount) == StorageResult.Success)
					inventory.Remove(ItemId.Silver, interactionCost, InventoryItemRemoveMsg.Given);
				else if (interaction == StorageInteraction.Retrieve && storage.RetrieveItem(worldId, amount) == StorageResult.Success)
					inventory.Remove(ItemId.Silver, interactionCost, InventoryItemRemoveMsg.Given);
			}
			else if (type == InventoryType.TeamStorage)
			{
				var inventory = character.Inventory;
				var storage = character.CurrentStorage as TeamStorage;

				var interactionCost = ZoneServer.Instance.Conf.World.StorageFee;
				var silver = inventory.CountItem(ItemId.Silver);

				if (silver < interactionCost)
				{
					Log.Warning("CZ_WAREHOUSE_CMD: User '{0}' tried to store or retrieve team items without silver", conn.Account.Name);
					return;
				}

				if (!storage.IsBrowsing)
				{
					Log.Warning("CZ_WAREHOUSE_CMD: User '{0}' tried to manage their team storage without it being open.", conn.Account.Name);
					return;
				}


				if (interaction == StorageInteraction.Store)
				{
					var item = inventory.GetItem(worldId);
					if (item == null)
					{
						Log.Warning("CZ_WAREHOUSE_CMD: User '{0}' tried to store an item that doesn't exist (worldId: {1}).", conn.Account.Name, worldId);
						return;
					}

					if (item.Id == ItemId.Silver)
					{
						storage.StoreSilver(amount);
					}
					else if (storage.StoreItem(worldId, amount) == StorageResult.Success)
					{
						inventory.Remove(ItemId.Silver, interactionCost, InventoryItemRemoveMsg.Given);
					}
				}
				else if (interaction == StorageInteraction.Retrieve && storage.RetrieveItem(worldId, amount) == StorageResult.Success)
				{
					inventory.Remove(ItemId.Silver, interactionCost, InventoryItemRemoveMsg.Given);
				}
			}
			else
			{
				Log.Warning("CZ_WAREHOUSE_CMD: Unknown storage type '{0}'.", type);
			}
		}

		[PacketHandler(Op.CZ_REQUEST_GODDESS_ROULETT)]
		public void CZ_REQUEST_GODDESS_ROULETT(IZoneConnection conn, Packet packet)
		{
			var rouletteType = packet.GetInt(); // 3
			var character = conn.SelectedCharacter;

			var itemGrade = "C";
			var item = "misc_reinforce_percentUp_480_NoTrade";
			var itemAmount = 3;

			character.ModifyAccountProperty(PropertyName.GODDESS_RAINBOW_ROULETTE_USE_ROULETTE_COUNT, 1);
			character.AddonMessage(AddonMessage.GODDESS_ROULETTE_START, string.Format("16/{0}/{1}/{2}", itemGrade, item, itemAmount));
		}

		/// <summary>
		/// Sent when clicking Confirm in a shop, with items in the "Bought" list.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ITEM_BUY)]
		public void CZ_ITEM_BUY(IZoneConnection conn, Packet packet)
		{
			var purchases = new Dictionary<int, int>();

			var size = packet.GetShort();
			var count = packet.GetInt();
			for (var i = 0; i < count; ++i)
			{
				var productId = packet.GetInt();
				var amount = packet.GetInt();

				if (!purchases.ContainsKey(productId))
					purchases[productId] = amount;
				else
					purchases[productId] += amount;
			}

			var character = conn.SelectedCharacter;
			var dialog = conn.CurrentDialog;
			ShopData shopData;
			Npc npc = default;

			if (dialog != null)
			{
				npc = dialog?.Npc;
				shopData = dialog?.Shop;
			}
			else
			{
				shopData = conn.ActiveShop;
			}

			// Check amount
			if (count > 10)
			{
				Log.Warning("CZ_ITEM_BUY: User '{0}' tried to buy more than 10 items at once.", conn.Account.Name);
				return;
			}

			// Check open shop
			if (shopData == null)
			{
				Log.Warning("CZ_ITEM_BUY: User '{0}' tried to buy something with no shop open.", conn.Account.Name);
				return;
			}

			var discountMultiplier = 1.0f; // 1.0 means no discount (100% price)
			if (npc != null)
			{
				var shopFactionId = npc.GetRegionFaction(); // Determine shop's faction

				if (!string.IsNullOrEmpty(shopFactionId))
				{
					var reputation = ZoneServer.Instance.World.Factions.GetReputation(character, shopFactionId);
					var tier = ZoneServer.Instance.World.Factions.GetTier(reputation);

					// Define discounts per tier (as multipliers: 0.90 = 10% discount)
					switch (tier)
					{
						case ReputationTier.Hated: discountMultiplier = 1.25f; break;
						case ReputationTier.Disliked: discountMultiplier = 1.10f; break;
						case ReputationTier.Neutral: break;
						case ReputationTier.Liked: discountMultiplier = 0.90f; break;
						case ReputationTier.Honored: discountMultiplier = 0.75f; break;
						default: discountMultiplier = 1.0f; break;
					}
				}
			}

			// Prepare items and get cost
			var totalCost = 0;
			var baseCost = 0;  // Amount seller receives (without tax)
			var purchaseList = new List<Tuple<ItemData, int>>();
			foreach (var purchase in purchases)
			{
				var productId = purchase.Key;
				var amount = purchase.Value;

				// Get product
				var productData = shopData.GetProduct(productId);
				if (productData == null)
				{
					Log.Warning("CZ_ITEM_BUY: User '{0}' tried to buy product that's not in the shop ({1}, {2}).", conn.Account.Name, shopData.Name, productId);
					return;
				}

				// Get item
				var itemData = ZoneServer.Instance.Data.ItemDb.Find(productData.ItemId);
				if (itemData == null)
				{
					Log.Warning("CZ_ITEM_BUY: User '{0}' tried to buy item that's not in the db ({1}, {2}).", conn.Account.Name, shopData.Name, productData.ItemId);
					return;
				}

				if (!shopData.IsCustom)
				{
					var singlePrice = (int)(itemData.Price * productData.PriceMultiplier);
					totalCost += singlePrice * amount;
					baseCost += singlePrice * amount;
				}
				else
				{
					var meetsRequirement = true;
					if (!string.IsNullOrEmpty(productData.RequiredFactionId))
					{
						// Compare player's current integer rep with the required integer value
						var playerRep = ZoneServer.Instance.World.Factions.GetReputation(character, productData.RequiredFactionId);
						if (playerRep < productData.RequiredTierValue)
						{
							meetsRequirement = false;
						}
					}

					if (!meetsRequirement)
						continue;

					var basePrice = (int)(productData.Price * productData.PriceMultiplier * discountMultiplier);
					totalCost += basePrice * amount;
					baseCost += basePrice * amount;
				}
				purchaseList.Add(new Tuple<ItemData, int>(itemData, amount));
			}

			if (shopData.OwnerHandle == 0)
			{
				// Check money first
				if (character.Inventory.CountItem(ItemId.Silver) < totalCost)
				{
					Log.Warning("CZ_ITEM_BUY: User '{0}' tried to buy items without having enough money.", conn.Account.Name);
					return;
				}

				// Give items first and track actual cost
				var actualCost = 0;
				foreach (var purchase in purchases)
				{
					var productId = purchase.Key;
					var amount = purchase.Value;

					var productData = shopData.GetProduct(productId);
					if (productData == null)
						continue;

					var itemData = ZoneServer.Instance.Data.ItemDb.Find(productData.ItemId);
					if (itemData == null)
						continue;

					character.Inventory.Add(itemData.Id, amount, InventoryAddType.Buy);

					var singlePrice = (int)(productData.Price * productData.PriceMultiplier * discountMultiplier);
					actualCost += singlePrice * amount;
				}

				// Only charge for items actually received
				if (actualCost > 0)
					character.Inventory.Remove(ItemId.Silver, actualCost, InventoryItemRemoveMsg.Given);
			}
			else
			{
				// Find shop owner.
				if (!character.Map.TryGetCharacter(shopData.OwnerHandle, out var shopOwner))
				{
					Log.Warning("CZ_ITEM_BUY: User '{0}' failed to find shop owner by handle {1}.", conn.Account.Name, shopData.OwnerHandle);
					return;
				}

				// Check and reduce money
				if (character.Inventory.CountItem(ItemId.Silver) < totalCost)
				{
					Log.Warning("CZ_ITEM_BUY: User '{0}' tried to buy items without having enough money.", conn.Account.Name);
					return;
				}

				// For custom shops (sellshops), we need to verify items by world ID
				// and use proper locking to prevent race conditions
				if (shopData.IsCustom)
				{
					// Process each purchase with proper locking and world ID verification
					var successfulPurchases = new List<Tuple<ProductData, int, Dictionary<long, int>>>();
					var actualTotalCost = 0;
					var actualBaseCost = 0;

					foreach (var purchase in purchases)
					{
						var productId = purchase.Key;
						var amount = purchase.Value;

						var productData = shopData.GetProduct(productId);
						if (productData == null)
						{
							Log.Warning("CZ_ITEM_BUY: User '{0}' tried to buy product not in shop ({1}, {2}).", conn.Account.Name, shopData.Name, productId);
							continue;
						}

						lock (productData)
						{
							// Check if enough stock
							if (productData.RequiredAmount < amount)
							{
								Log.Warning("CZ_ITEM_BUY: User '{0}' tried to buy more than available ({1} > {2}).", conn.Account.Name, amount, productData.RequiredAmount);
								continue;
							}

							// Verify items exist by world ID
							if (productData.ItemWorldIds.Count == 0)
							{
								Log.Warning("CZ_ITEM_BUY: Product has no tracked world IDs for user '{0}'.", conn.Account.Name);
								continue;
							}

							// Build map of worldId -> amountToRemove
							var itemsToRemove = new Dictionary<long, int>();
							var amountToVerify = amount;

							foreach (var worldId in productData.ItemWorldIds)
							{
								if (amountToVerify <= 0)
									break;

								if (shopOwner.Inventory.TryGetItem(worldId, out var item))
								{
									var takeAmount = Math.Min(item.Amount, amountToVerify);
									itemsToRemove[worldId] = takeAmount;
									amountToVerify -= takeAmount;
								}
							}

							if (amountToVerify > 0)
							{
								Log.Warning("CZ_ITEM_BUY: Seller '{0}' missing {1} items for purchase.", shopOwner.Name, amountToVerify);
								continue;
							}

							// Calculate cost for this product
							var productPrice = (int)(productData.Price * productData.PriceMultiplier * discountMultiplier);
							actualTotalCost += productPrice * amount;
							actualBaseCost += productPrice * amount;

							successfulPurchases.Add(new Tuple<ProductData, int, Dictionary<long, int>>(productData, amount, itemsToRemove));
						}
					}

					// Check if any purchases are valid
					if (successfulPurchases.Count == 0)
					{
						Log.Warning("CZ_ITEM_BUY: No valid purchases for user '{0}'.", conn.Account.Name);
						return;
					}

					// Re-verify buyer has enough silver for actual cost
					if (character.Inventory.CountItem(ItemId.Silver) < actualTotalCost)
					{
						Log.Warning("CZ_ITEM_BUY: User '{0}' doesn't have enough silver ({1} < {2}).", conn.Account.Name, character.Inventory.CountItem(ItemId.Silver), actualTotalCost);
						return;
					}

					// Execute all transactions
					foreach (var successfulPurchase in successfulPurchases)
					{
						var productData = successfulPurchase.Item1;
						var amount = successfulPurchase.Item2;
						var itemsToRemove = successfulPurchase.Item3;

						lock (productData)
						{
							// Double-check stock one more time
							if (productData.RequiredAmount < amount)
							{
								Log.Warning("CZ_ITEM_BUY: Race condition - product sold out for '{0}'.", conn.Account.Name);
								continue;
							}

							// Remove items from seller by world ID
							var totalRemoved = 0;
							var remainingWorldIds = new List<long>();
							var itemsToTransfer = new List<Item>();

							foreach (var worldId in productData.ItemWorldIds)
							{
								if (itemsToRemove.TryGetValue(worldId, out var removeAmount))
								{
									if (shopOwner.Inventory.TryGetItem(worldId, out var item))
									{
										// Create copy with properties BEFORE removing
										var newItem = new Item(item, removeAmount);

										if (shopOwner.Inventory.Remove(worldId, removeAmount, InventoryItemRemoveMsg.Sold) == InventoryResult.Success)
										{
											totalRemoved += removeAmount;
											itemsToTransfer.Add(newItem);
										}

										// If item still has remaining amount, keep the world ID
										if (item.Amount > removeAmount)
											remainingWorldIds.Add(worldId);
									}
								}
								else
								{
									// This world ID wasn't used, keep it
									remainingWorldIds.Add(worldId);
								}
							}

							// Give items to buyer (with properties preserved)
							foreach (var transferItem in itemsToTransfer)
								character.Inventory.Add(transferItem, InventoryAddType.Buy);

							// Update product state
							productData.RequiredAmount -= totalRemoved;
							productData.Amount -= totalRemoved;  // Also update Amount for UI
							productData.ItemWorldIds.Clear();
							productData.ItemWorldIds.AddRange(remainingWorldIds);

							// Remove sold out products
							if (productData.RequiredAmount <= 0 || productData.Amount <= 0)
								shopData.Products.Remove(productData.Id);
						}
					}

					// Transfer silver
					character.Inventory.Remove(ItemId.Silver, actualTotalCost, InventoryItemRemoveMsg.Given);
					shopOwner.Inventory.Add(ItemId.Silver, actualBaseCost, InventoryAddType.Sell);
				}
				else
				{
					// Non-custom shops (buyshops) - track actual items transferred
					var actualCostTransferred = 0;

					foreach (var purchase in purchases)
					{
						var productId = purchase.Key;
						var amount = purchase.Value;

						var productData = shopData.GetProduct(productId);
						if (productData == null)
						{
							Log.Warning("CZ_ITEM_BUY: User '{0}' tried to buy product not in shop ({1}, {2}).", conn.Account.Name, shopData.Name, productId);
							continue;
						}

						var itemData = ZoneServer.Instance.Data.ItemDb.Find(productData.ItemId);
						if (itemData == null)
						{
							Log.Warning("CZ_ITEM_BUY: User '{0}' tried to buy item not in db ({1}).", conn.Account.Name, productData.ItemId);
							continue;
						}

						if (shopOwner.Inventory.Remove(itemData.Id, amount, InventoryItemRemoveMsg.Sold) == amount)
						{
							character.Inventory.Add(itemData.Id, amount, InventoryAddType.Buy);

							var singlePrice = (int)(productData.Price * productData.PriceMultiplier * discountMultiplier);
							actualCostTransferred += singlePrice * amount;

							if (productData.Amount > 0)
								productData.Amount -= amount;

							if (productData.Amount <= 0)
								shopData.Products.Remove(productId);
						}
					}

					// Only transfer silver for items that were actually bought
					if (actualCostTransferred > 0)
					{
						character.Inventory.Remove(ItemId.Silver, actualCostTransferred, InventoryItemRemoveMsg.Given);
						shopOwner.Inventory.Add(ItemId.Silver, actualCostTransferred, InventoryAddType.Sell);
					}
				}
				Send.ZC_ADDON_MSG(character, AddonMessage.INV_ITEM_CHANGE_COUNT, 0, null);
				Send.ZC_ADDON_MSG(shopOwner, AddonMessage.INV_ITEM_CHANGE_COUNT, 0, null);

				// Check if shop is empty FIRST before updating UIs
				if (shopData.Products.Count == 0)
				{
					// Close buyer's shop dialog FIRST (before broadcast so they don't react to it)
					Send.ZC_DIALOG_CLOSE(conn);
					conn.ActiveShop = null;
					Send.ZC_ENABLE_CONTROL(conn, "AUTOSELLER", true);
					Send.ZC_LOCK_KEY(character, "AUTOSELLER", false);

					// Match the exact order from manual shop close (CZ_REGISTER_AUTOSELLER with itemCount -1)
					// Use the conn overload to send only to seller, not broadcast to everyone
					shopData.IsClosed = true;
					Send.ZC_AUTOSELLER_LIST(shopOwner.Connection, shopOwner);
					Send.ZC_AUTOSELLER_TITLE(shopOwner);
					Send.ZC_NORMAL.ShopAnimation(shopOwner.Connection, shopOwner, "Squire_Repair", 1, 0);
					shopOwner.Connection.ShopCreated = null;

					// Close seller's shop management UI (personal_sell_shop_my frame for sell shops)
					Send.ZC_EXEC_CLIENT_SCP(shopOwner.Connection, "ui.CloseFrame('personal_sell_shop_my')");

					// Notify seller their shop closed
					shopOwner.ServerMessage("All items sold. Shop closed.");

					Log.Debug("CZ_ITEM_BUY: Shop empty, closing shop for {0}", shopOwner.Name);
				}
				else
				{
					// Shop still has items - update both UIs

					// Update seller's shop UI
					if (shopOwner.Connection.ShopCreated != null)
						Send.ZC_AUTOSELLER_LIST(shopOwner);

					// Update buyer's shop UI
					if (shopData.IsCustom)
					{
						// For custom shops, resend the MeliaCustomShop data
						Send.ZC_EXEC_CLIENT_SCP(conn, "Melia.Comm.BeginRecv('CustomShop')");

						var sb = new StringBuilder();
						foreach (var productData in shopData.Products.Values)
						{
							sb.AppendFormat("{{ {0},{1},{2},{3} }},", productData.Id, productData.ItemId, productData.Amount, productData.Price);

							if (sb.Length > ClientScript.ScriptMaxLength * 0.8)
							{
								Send.ZC_EXEC_CLIENT_SCP(conn, $"Melia.Comm.Recv('CustomShop', {{ {sb} }})");
								sb.Clear();
							}
						}

						if (sb.Length > 0)
						{
							Send.ZC_EXEC_CLIENT_SCP(conn, $"Melia.Comm.Recv('CustomShop', {{ {sb} }})");
							sb.Clear();
						}

						Send.ZC_EXEC_CLIENT_SCP(conn, "Melia.Comm.ExecData('CustomShop', M_SET_CUSTOM_SHOP)");
						Send.ZC_EXEC_CLIENT_SCP(conn, "Melia.Comm.EndRecv('CustomShop')");
						Send.ZC_DIALOG_CLOSE(conn);
						Send.ZC_DIALOG_TRADE(conn, "MeliaCustomShop");
					}
					else
					{
						// For non-custom shops, use autoseller list
						Send.ZC_AUTOSELLER_LIST(conn, shopOwner);
					}
				}
			}

			// Temporary fix for differences in prices between client and
			// server. Equip prices are being calculated somehow, with their
			// price in the db usually being 0. This msg will reset the shop
			// panel, to display the proper balance after confirming the
			// transaction.
			// 08-29-21 Update, Item Database is updated but equipment for the most part are still priced at 0
			Send.ZC_ADDON_MSG(character, AddonMessage.FAIL_SHOP_BUY, 0, null);
		}

		/// <summary>
		/// Sent when clicking Confirm in a shop, with items in the "Sold" list.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ITEM_SELL)]
		public void CZ_ITEM_SELL(IZoneConnection conn, Packet packet)
		{
			var itemsToSell = new Dictionary<long, int>();

			var size = packet.GetShort();
			var count = packet.GetInt();
			for (var i = 0; i < count; ++i)
			{
				var worldId = packet.GetLong();
				var amount = packet.GetInt();
				var unkInt = packet.GetInt();

				itemsToSell[worldId] = amount;
			}

			var character = conn.SelectedCharacter;

			// Check amount
			if (count > 10)
			{
				Log.Warning("CZ_ITEM_SELL: User '{0}' tried sell more than 10 items at once.", conn.Account.Name);
				return;
			}

			// Remove items and count revenue
			var totalMoney = 0;
			var itemsSold = new Dictionary<int, Item>();

			foreach (var itemToSell in itemsToSell)
			{
				var worldId = itemToSell.Key;
				var amount = itemToSell.Value;

				// Get item
				var item = character.Inventory.GetItem(worldId);
				if (item == null)
				{
					Log.Warning("CZ_ITEM_SELL: User '{0}' tried to sell a non-existent item.", conn.Account.Name);
					return;
				}

				if (item.IsLocked)
					continue;

				// Check amount
				if (item.Amount < amount)
				{
					Log.Warning("CZ_ITEM_SELL: User '{0}' tried to sell more of an item than they own.", conn.Account.Name);
					return;
				}

				// Try to remove item
				if (character.Inventory.Remove(item, amount, InventoryItemRemoveMsg.Sold) != InventoryResult.Success)
				{
					Log.Warning("CZ_ITEM_SELL: Failed to sell an item from user '{0}' .", conn.Account.Name);
					continue;
				}

				totalMoney += item.Data.SellPrice * amount;
				itemsSold.Add(itemsSold.Count, item);
			}

			// Give money
			if (totalMoney > 0)
				character.Inventory.Add(ItemId.Silver, totalMoney, InventoryAddType.Sell);

			// Need to keep track of items sold, server sends this list to the client
			Send.ZC_SOLD_ITEM_DIVISION_LIST(character, InventoryType.Sold, itemsSold);
		}

		/// <summary>
		/// Information sent to be saved?
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SAVE_INFO)]
		public void CZ_SAVE_INFO(IZoneConnection conn, Packet packet)
		{
			// TODO: Research what information needs to be saved here and implement it.
		}

		/// <summary>
		/// Sent when attempting to logout or switch characters.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_RUN_GAMEEXIT_TIMER)]
		public void CZ_RUN_GAMEEXIT_TIMER(IZoneConnection conn, Packet packet)
		{
			var destination = packet.GetString();
			var parameter = packet.GetString(); // [i373230]
			var character = conn.SelectedCharacter;

			switch (destination)
			{
				case "Logout":
				case "Barrack":
				case "Channel":
				case "Exit":
					if (conn.SelectedCharacter.Inventory.HasExpiringItems)
					{
						Send.ZC_ADDON_MSG(character, AddonMessage.EXPIREDITEM_ALERT_OPEN, 0, destination);
					}

					Send.ZC_ADDON_MSG(conn.SelectedCharacter, AddonMessage.GAMEEXIT_TIMER_END, 0, "None");
					break;
				default:
					Log.Warning("CZ_RUN_GAMEEXIT_TIMER: User '{0}' tried to transfer to '{1}' which is an unknown state.", conn.Account.Name, destination);
					return;
			}

			Log.Info("User '{0}' is transferring to '{1}' state.", conn.Account.Name, destination);
		}

		/// <summary>
		/// Sent when a player tries to join a guild (self invite).
		/// Dummy Handler
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SELF_INVITE_NEWBIE_GUILD)]
		public void CZ_SELF_INVITE_NEWBIE_GUILD(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;

			Send.ZC_SYSTEM_MSG(character, 102502);
		}

		/// <summary>
		/// Sent when a player tries to take a screenshot.
		/// Dummy Handler
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SCREENSHOT_HASH)]
		public void CZ_SCREENSHOT_HASH(IZoneConnection conn, Packet packet)
		{
			var account = conn.SelectedCharacter;
			// Track screenshot meta data (where/when it was taken)?
		}

		/// <summary>
		/// Sent when a player tries to enter instance dungeon via UI.
		/// Dummy Handler
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_MOVE_TO_INDUN)]
		public void CZ_REQ_MOVE_TO_INDUN(IZoneConnection conn, Packet packet)
		{
			var enterType = (DungeonEnterType)packet.GetInt();
			var character = conn.SelectedCharacter;

			// Fail if player is already inside a dungeon
			if (ZoneServer.Instance.Data.InstanceDungeonDb.TryGetByMapClassName(character.Map.ClassName, out _))
			{
				character.SystemMessage("CannotInCurrentState");
				return;
			}

			// Fail if player is dead
			if (character.IsDead)
			{
				character.SystemMessage("CannotInCurrentState");
				return;
			}

			var instanceDungeonId = character.Variables.Perm.GetInt(AutoMatchZoneManager.DungeonIdVarName);

			// Get IndunData
			var dungeonData = ZoneServer.Instance.Data.InstanceDungeonDb.Find(instanceDungeonId);
			if (dungeonData == null)
			{
				Log.Warning("CZ_REQ_MOVE_TO_INDUN: User '{0}' sent an unknown dungeon id ({1}).", conn.Account.Name, instanceDungeonId);
				return;
			}

			if (!DungeonScript.TryGet(dungeonData.MiniGame, out var dungeonScript))
			{
				Log.Warning("CZ_REQ_MOVE_TO_INDUN: User '{0}' sent an unimplemented dungeon id ({1}).", conn.Account.Name, instanceDungeonId);
			}

			// Check if player has a stored active instance reference
			var activeInstanceId = character.Variables.Perm.GetString(DungeonScript.ActiveInstanceVarName);
			if (!string.IsNullOrEmpty(activeInstanceId))
			{
				// Verify the instance actually still exists
				var existingInstance = DungeonScript.GetInstance(character.DbId);
				if (existingInstance != null)
				{
					if (activeInstanceId != dungeonData.MiniGame)
					{
						character.SystemMessage("You already have an active dungeon instance.");
						return;
					}
				}
				else
				{
					// Instance no longer exists (e.g. server restarted), clear the stale reference
					character.Variables.Perm.SetString(DungeonScript.ActiveInstanceVarName, null);
				}
			}

			if (dungeonData.Map == null
				&& dungeonScript != null
				&& !string.IsNullOrEmpty(dungeonScript.MapName)
				&& ZoneServer.Instance.Data.MapDb.TryFind(dungeonScript.MapName, out var dungeonMap))
			{
				dungeonData.MapName = dungeonScript.MapName;
				dungeonData.Map = dungeonMap;
			}

			if (dungeonData.Map == null)
			{
				Log.Warning("CZ_REQ_MOVE_TO_INDUN: User '{0}' sent is trying to join a dungeon with unknown map.", conn.Account.Name);
				return;
			}

			// Character level too low
			if (dungeonData.Level > character.Level)
			{
				character.SystemMessage("NeedMorePcLevel");
				return;
			}

			// Character level too high
			if (dungeonData.MaxLevel != 0 && character.Level > dungeonData.MaxLevel)
			{
				character.SystemMessage("CantJoinHighLevel_ChallengeMode");
				return;
			}

			// Resets AutoMatch session unique id
			character.Variables.Perm.SetLong(AutoMatchZoneManager.SessionIdVarName, 0);
			character.Variables.Temp.SetInt(AutoMatchZoneManager.PlayersCountVarName, 0);

			switch (enterType)
			{
				case DungeonEnterType.EnterNow:
					// Create a party for solo players
					if (!character.HasParty)
					{
						ZoneServer.Instance.World.Parties.Create(character);
					}

					// Move player to dungeon if we already have an active dungeon
					// This also will prevent from trying to join other dungeons
					// while the ActiveDungeon is not null
					var currentDungeon = DungeonScript.GetInstance(character.DbId);
					if (currentDungeon != null)
					{
						if (character.Dungeon.GetCurrentEntryCount(instanceDungeonId) >= character.Dungeon.GetMaxEntryCount(instanceDungeonId))
						{
							character.SystemMessage("CannotJoinIndunYet");
							return;
						}

						character.Warp(currentDungeon.MapId, currentDungeon.StartPosition);
						return;
					}

					// Only the party leader is allowed to start a new dungeon
					if (!character.Connection.Party.IsLeader(character))
					{
						return;
					}

					// Validate entry count for all party members
					var partyMembers = character.Connection.Party.GetPartyMembers();
					var eligibleMembers = new List<Character>();

					foreach (var memberCharacter in partyMembers)
					{
						if (memberCharacter == null)
							continue;

						if (memberCharacter.Dungeon.GetCurrentEntryCount(instanceDungeonId) >= memberCharacter.Dungeon.GetMaxEntryCount(instanceDungeonId))
						{
							memberCharacter.SystemMessage("CannotJoinIndunYet");
							if (memberCharacter != character)
								character.ServerMessage("A party member has exceeded their dungeon entry limit.");
							return;
						}

						// Clear auto-match variables
						memberCharacter.Variables.Perm.SetLong(AutoMatchZoneManager.SessionIdVarName, 0);
						memberCharacter.Variables.Temp.SetInt(AutoMatchZoneManager.PlayersCountVarName, 0);

						eligibleMembers.Add(memberCharacter);
					}

					// Warp eligible members using centralized dungeon warp
					DungeonScript.WarpPartyToDungeon(eligibleMembers, instanceDungeonId);
					break;
				case DungeonEnterType.AutoMatch:
					if (!dungeonData.AutoMatchEnable)
					{
						Log.Warning("CZ_REQ_MOVE_TO_INDUN: User '{0}' has attempt to join AutoMatchQueue for dungeonId '{1}' but auto match is disabled for this dungeon.", conn.Account.Name, instanceDungeonId);
						return;
					}

					// Prevent individual auto-match if player's party is already in the queue
					if (character.HasParty && ZoneServer.Instance.World.AutoMatch.IsPartyInQueue(conn.Party))
					{
						character.SystemMessage("CannotInCurrentState");
						return;
					}

					// Prevent individual auto-match if player already has an active auto match session
					if (ZoneServer.Instance.World.AutoMatch.GetSessionForCharacter(character) != null)
					{
						character.SystemMessage("CannotInCurrentState");
						return;
					}

					if (character.Dungeon.GetCurrentEntryCount(instanceDungeonId) >= character.Dungeon.GetMaxEntryCount(instanceDungeonId))
					{
						character.SystemMessage("CannotJoinIndunYet");
						return;
					}

					// Sends the request to join auto match message to Coordinator
					var commMessage = new AutoMatchMessage(character.DbId, conn.Account.ObjectId, instanceDungeonId, true);
					ZoneServer.Instance.Communicator.Send("Coordinator", commMessage.BroadcastTo("Chat"));

					// Joins auto matching by yourself
					Send.ZC_NORMAL.DungeonAutoMatching(conn, instanceDungeonId);

					break;

				// The "wait for party members to accept" step is not implemented.
				// Instead, we directly queue the party for auto matching when the leader clicks
				// the "Auto Match with Current Members" button.
				case DungeonEnterType.AutoMatchWithParty:
					ZoneServer.Instance.World.AutoMatch.QueuePartyForAutoMatch(character, dungeonData, instanceDungeonId);
					break;

				default:
					Log.Warning("CZ_REQ_MOVE_TO_INDUN: User '{0}' attempt to enter with a invalid type ({1}).", conn.Account.Name, enterType);
					return;
			}
		}

		/// <summary>
		/// Sent when a party leader clicks "Auto Match with Current Members" in the indunenter UI.
		/// Directly queues the party for auto-matching without requiring party member acceptance.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_REGISTER_TO_INDUN)]
		public void CZ_REQ_REGISTER_TO_INDUN(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;

			// Fail if player is already inside a dungeon
			if (ZoneServer.Instance.Data.InstanceDungeonDb.TryGetByMapClassName(character.Map.ClassName, out _))
			{
				character.SystemMessage("CannotInCurrentState");
				return;
			}

			// Fail if player is dead
			if (character.IsDead)
			{
				character.SystemMessage("CannotInCurrentState");
				return;
			}

			var instanceDungeonId = character.Variables.Perm.GetInt(AutoMatchZoneManager.DungeonIdVarName);

			// Get IndunData
			var dungeonData = ZoneServer.Instance.Data.InstanceDungeonDb.Find(instanceDungeonId);
			if (dungeonData == null)
			{
				Log.Warning("CZ_REQ_REGISTER_TO_INDUN: User '{0}' sent an unknown dungeon id ({1}).", conn.Account.Name, instanceDungeonId);
				return;
			}

			ZoneServer.Instance.World.AutoMatch.QueuePartyForAutoMatch(character, dungeonData, instanceDungeonId);
		}

		/// <summary>
		/// Contains newly uncovered areas of a map.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_MAP_REVEAL_INFO)]
		public void CZ_MAP_REVEAL_INFO(IZoneConnection conn, Packet packet)
		{
			var mapId = packet.GetInt();
			var visible = packet.GetBin(128);
			var percentage = packet.GetFloat();

			var character = conn.SelectedCharacter;

			// Check if the map exists
			var mapData = ZoneServer.Instance.Data.MapDb.Find(mapId);
			if (mapData == null)
			{
				Log.Warning("CZ_MAP_REVEAL_INFO: User '{0}' tried to send an update for map '{1}', which doesn't exist.", conn.Account.Name, mapId);
				return;
			}

			// Check if character is actually on the map
			// The existence check prevents flooding, and this one ensures
			// players can only reveal maps they are actually on.
			// Eventually we might want to improve this further,
			// checking the character's position.
			if (conn.SelectedCharacter.MapId != mapId)
			{
				Log.Warning("CZ_MAP_REVEAL_INFO: User '{0}' tried to send an update for a different map than they're on.", conn.Account.Name);
				return;
			}

			// Check the percentage for validity
			if (percentage < 0 || percentage > 100)
			{
				Log.Warning("CZ_MAP_REVEAL_INFO: User '{0}' tried to update the visibility for map '{1}' beyond an acceptable percentage.", conn.Account.Name, mapId);
				return;
			}

			var revealedMap = new RevealedMap(mapId, visible, percentage);
			conn.Account.AddRevealedMap(revealedMap);

			// Try to give exploration reward
			character.TryGiveMapExplorationReward(mapId, percentage);
		}

		/// <summary>
		/// Reports to the server a percentage of the map that has been explored.
		/// </summary>
		/// <remarks>
		/// This packet was last seen in logs from 2017 and is apparently no longer
		/// used. The map percentage is now communicated via CZ_MAP_REVEAL_INFO.
		/// </remarks>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_MAP_SEARCH_INFO)]
		public void CZ_MAP_SEARCH_INFO(IZoneConnection conn, Packet packet)
		{
			var map = packet.GetString(41);
			var percentage = packet.GetFloat();

			var mapData = ZoneServer.Instance.Data.MapDb.Find(map);
			if (mapData == null)
			{
				Log.Warning("CZ_MAP_SEARCH_INFO: User '{0}' tried to update the map '{1}' which doesn't exist.", conn.Account.Name, map);
				return;
			}

			if (percentage < 0 || percentage > 100)
			{
				Log.Warning("CZ_MAP_SEARCH_INFO: User '{0}' tried to update the visibility for map '{1}' beyond an acceptable percentage.", conn.Account.Name, map);
				return;
			}

			// Originally null was passed as "explored", but then the server
			// would try to save the null to the database if the map data
			// didn't exist yet.

			var revealedMap = new RevealedMap(mapData.Id, [], percentage);
			conn.Account.AddRevealedMap(revealedMap);
		}

		/// <summary>
		/// Indicates a request from the client to trade with another character.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_EXCHANGE_REQUEST)]
		public void CZ_EXCHANGE_REQUEST(IZoneConnection conn, Packet packet)
		{
			var targetHandle = packet.GetInt();

			var character = conn.SelectedCharacter;

			if (character == null)
				return;

			if (!character.Map.TryGetCharacter(targetHandle, out var targetCharacter))
			{
				Log.Warning("CZ_EXCHANGE_REQUEST: User '{0}' trade partner not found.", conn.Account.Name);
				return;
			}

			ZoneServer.Instance.World.Trades.RequestTrade(character, targetCharacter);
		}

		/// <summary>
		/// Indicates an accepted request from the client to trade with another character.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_EXCHANGE_ACCEPT)]
		public void CZ_EXCHANGE_ACCEPT(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;

			if (!character.IsTrading)
			{
				Log.Warning("CZ_EXCHANGE_ACCEPT:  User '{0}' tried to accept a non-existent trade.", conn.Account.Name);
				return;
			}

			ZoneServer.Instance.World.Trades.StartTrade(character);
		}

		/// <summary>
		/// Request to offer an item for trade
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_EXCHANGE_OFFER)]
		public void CZ_EXCHANGE_OFFER(IZoneConnection conn, Packet packet)
		{
			var i1 = packet.GetInt();
			var worldId = packet.GetLong();
			var amount = packet.GetInt();
			var i3 = packet.GetInt();

			var character = conn.SelectedCharacter;

			if (character == null)
				return;

			if (!character.IsTrading)
			{
				Log.Warning("CZ_EXCHANGE_OFFER: User '{0}' tried to trade without actually trading.", conn.Account.Name);
				return;
			}

			ZoneServer.Instance.World.Trades.OfferTradeItem(character, worldId, amount);
		}

		/// <summary>
		/// Initial trade agreement request
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_EXCHANGE_AGREE)]
		public void CZ_EXCHANGE_AGREE(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;

			if (character == null)
				return;

			ZoneServer.Instance.World.Trades.ConfirmTrade(character);
		}

		/// <summary>
		/// Final trade agreement request
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_EXCHANGE_FINALAGREE)]
		public void CZ_EXCHANGE_FINALAGREE(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;

			if (character == null)
				return;

			ZoneServer.Instance.World.Trades.FinalConfirmTrade(character);
		}

		/// <summary>
		/// Cancel trade request
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_EXCHANGE_CANCEL)]
		public void CZ_EXCHANGE_CANCEL(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;

			if (character == null)
				return;

			ZoneServer.Instance.World.Trades.CancelTrade(character);
		}

		/// <summary>
		/// Handles various commands found in the custom command data.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CUSTOM_COMMAND)]
		public void CZ_CUSTOM_COMMAND(IZoneConnection conn, Packet packet)
		{
			var commandId = packet.GetInt();
			var numArg1 = packet.GetInt();
			var numArg2 = packet.GetInt();
			var numArg3 = packet.GetInt();

			var character = conn.SelectedCharacter;

			// Get data
			if (!ZoneServer.Instance.Data.CustomCommandDb.TryFind(commandId, out var data))
			{
				Log.Warning("CZ_CUSTOM_COMMAND: User '{0}' sent an unknown command id ({1}).", conn.Account.Name, commandId);
				return;
			}

			// Get handler
			if (!ScriptableFunctions.CustomCommand.TryGet(data.Script, out var scriptFunc))
			{
				Log.Debug("CZ_CUSTOM_COMMAND: No handler registered for custom command script '{0}({1}, {2}, {3})'", data.Script, numArg1, numArg2, numArg3);
				return;
			}


			// Try to execute command
			try
			{
				var result = scriptFunc(character, numArg1, numArg2, numArg3);
				if (result == CustomCommandResult.Fail)
				{
					Log.Debug("CZ_CUSTOM_COMMAND: Execution of script '{0}({1}, {2}, {3})' failed.", data.Script, numArg1, numArg2, numArg3);

				}
			}
			catch (Exception ex)
			{
				Log.Debug("CZ_CUSTOM_COMMAND: Exception while executing script '{0}({1}, {2}, {3})': {4}", data.Script, numArg1, numArg2, numArg3, ex);
			}
		}

		/// <summary>
		/// Request to reset a character's job, or rather to switch
		/// out one job for another.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_RANKRESET_SYSTEM)]
		public void CZ_REQ_RANKRESET_SYSTEM(IZoneConnection conn, Packet packet)
		{
			var oldJobId = (JobId)packet.GetShort();
			var newJobId = (JobId)packet.GetShort();

			if (ZoneServer.Instance.Conf.World.NoRankReset)
			{
				Log.Warning("CZ_REQ_RANKRESET_SYSTEM: User '{0}' tried to switch jobs, despite job advancement being disabled.", conn.Account.Name);
				return;
			}

			var character = conn.SelectedCharacter;

			if (!character.Jobs.TryGet(oldJobId, out var oldJob))
			{
				Log.Warning("CZ_REQ_RANKRESET_SYSTEM: User '{0}' requested job reset for a job they don't have ({1})'.", conn.Account.Name, oldJobId);
				return;
			}

			if (character.Jobs.TryGet(newJobId, out _))
			{
				Log.Warning("CZ_REQ_RANKRESET_SYSTEM: User '{0}' tried to switch to a job they already have ({1})'.", conn.Account.Name, newJobId);
				return;
			}

			if (oldJobId.ToClass() != newJobId.ToClass())
			{
				Log.Warning("CZ_REQ_RANKRESET_SYSTEM: User '{0}' tried to switch to a job outside of their class tree ({1} -> {2})'.", conn.Account.Name, oldJobId, newJobId);
				return;
			}

			// Remove the skills associated with the old job. This could
			// be easier and safer if we were to save the job a skill was
			// learned for with the skill data.
			var oldJobTreeData = ZoneServer.Instance.Data.SkillTreeDb.FindSkills(oldJob.Id, oldJob.Level);

			foreach (var treeData in oldJobTreeData)
			{
				if (!character.Skills.TryGet(treeData.SkillId, out var skill))
					continue;

				character.Skills.Remove(skill.Id);
			}

			// Remove old job and grant new one
			var newJob = new Job(character, newJobId, oldJob.Circle);
			newJob.TotalExp = oldJob.TotalExp;
			newJob.SkillPoints = oldJob.Level;

			character.Jobs.Remove(oldJobId);
			character.Jobs.Add(newJob);
			character.JobId = newJob.Id;

			// I'd prefer to let the player keep playing after the switch,
			// but the intended behavior is apparently that you get DCed
			// and have to log back in. We'll mimic this for now, though
			// we could probably do it better. The only problem I noticed
			// so far is that the "Are you sure?" prompt doesn't disappear,
			// but that we can solve with client scripting.

			//Send.ZC_PC(character, PcUpdateType.Job, (int)newJob.Id, newJob.Level);
			//Send.ZC_JOB_PTS(character, newJob);
			//Send.ZC_NORMAL.PlayEffect(character, "F_pc_class_change");

			ZoneServer.Instance.ServerEvents.PlayerAdvancedJob.Raise(new PlayerEventArgs(character));

			// The intended behavior is to trigger a clean DC from the
			// client with a move to barracks, but if we *need* the
			// player to DC, we might want to force it, because users
			// could make the client skip this packet and stay online.
			Send.ZC_MOVE_BARRACK(conn);
		}

		/// <summary>
		/// Sent when using a Tx Item.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_TX_ITEM)]
		public void CZ_REQ_TX_ITEM(IZoneConnection conn, Packet packet)
		{
			var size = packet.GetShort();
			var txClassId = packet.GetInt();
			var worldId = packet.GetLong();
			var l1 = packet.GetLong();
			var l2 = packet.GetLong();
			var argCount = packet.GetByte();
			var numArgs = packet.GetList(argCount, packet.GetInt);

			var character = conn.SelectedCharacter;

			// Get data
			if (!ZoneServer.Instance.Data.DialogTxDb.TryFind(txClassId, out var data))
			{
				character.ServerMessage(Localization.Get("Apologies, something went wrong there. Please report this issue."));
				Log.Warning("CZ_REQ_TX_ITEM: User '{0}' sent an unknown dialog transaction id: {1}", conn.Account.Name, txClassId);
				return;
			}

			// Get handler
			if (!ScriptableFunctions.ItemTx.TryGet(data.Script, out var scriptFunc))
			{
				character.ServerMessage(Localization.Get("This action has not been implemented yet."));
				Log.Debug("CZ_REQ_TX_ITEM: No handler registered for transaction script '{0}'", data.Script);
				return;
			}

			// Get item from character
			var item = character.Inventory.GetItem(worldId);
			if (item == null)
			{
				Log.Warning("CZ_REQ_TX_ITEM: User '{0}' tried to use an item they don't have.", conn.Account.Name);
				return;
			}

			// Try to execute script
			try
			{
				var result = scriptFunc(character, item, numArgs);
				if (result == ItemTxResult.Fail)
				{
					character.ServerMessage(Localization.Get("Apologies, something went wrong there."));
					Log.Debug("CZ_REQ_TX_ITEM: Execution of script '{0}' failed.", data.Script);
					return;
				}
				character.Inventory.Remove(worldId, msg: InventoryItemRemoveMsg.Used);
			}
			catch (Exception ex)
			{
				character.ServerMessage(Localization.Get("Apologies, something went wrong there. Please report this issue."));
				Log.Debug("CZ_REQ_TX_ITEM: Exception while executing script '{0}': {1}", data.Script, ex);
			}
		}

		/// <summary>
		/// Sent after a loading screen is completed.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_LOAD_COMPLETE)]
		public void CZ_LOAD_COMPLETE(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;

			conn.LoadComplete = true;

			Send.ZC_LOAD_COMPLETE(conn);

			character.AddonMessage(AddonMessage.RECEIVE_SERVER_NATION);

			// Removed: Personal House loading - PersonalHouse type deleted during Laima merge
			// If character is on a housing map, warp them out since houses aren't available
			if (character.Map.Id >= 7000 && character.Map.Id <= 7002)
			{
				var lastMapId = (int)character.Properties.GetFloat(PropertyName.LastWarpMapID, 1001);
				if (ZoneServer.Instance.World.Maps.TryGet(lastMapId, out var map) && map.Ground.TryGetRandomPosition(out var rndPos))
					character.Warp(map.Id, rndPos);
			}

			foreach (var quest in character.Quests.GetList())
			{
				//if (!quest.InProgress || quest.SessionObjectStaticData == null)
				//	continue;
				//character.AddSessionObject(quest.SessionObjectStaticData.Id);
			}

			foreach (var skill in character.Skills.GetList(s => s.IsPassive))
			{
				if (ZoneServer.Instance.SkillHandlers.TryGetPassiveSkillHandler<IPassiveSkillHandler>(skill.Id, out var handler))
					handler.Handle(skill, skill.Owner);
			}

			//character.ShowHelp("TUTO_MOVE_KB");
			//character.ShowHelp("TUTO_MOVE_JUMP");
		}

		/// <summary>
		/// Sent when checking for attendance rewards
		/// Dummy
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ATTENDANCE_REWARD_CLICK)]
		public void CZ_ATTENDANCE_REWARD_CLICK(IZoneConnection conn, Packet packet)
		{
			var eventAttendanceId = packet.GetInt();
			var character = conn.SelectedCharacter;

			if (character.Attendance.CanRetrieveReward(eventAttendanceId, out var eventAttendanceData))
			{
				var rewardAttendanceData = character.Attendance.GetNextRewardAttendanceData(eventAttendanceData.ClassName);
				if (rewardAttendanceData != null)
					character.Attendance.AddAttendanceReward(rewardAttendanceData.Id);
			}
		}

		/// <summary>
		/// Create guild by web?
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CREATE_GUILD_BY_WEB)]
		public void CZ_CREATE_GUILD_BY_WEB(IZoneConnection conn, Packet packet)
		{
			var name = packet.GetString();
			var character = conn.SelectedCharacter;

			// Removed: Guild type deleted during Laima merge
			// Guild creation not available
			Log.Warning("CZ_CREATE_GUILD_BY_WEB: Guild system not available. User '{0}'.", conn.Account.Name);
		}

		/// <summary>
		/// Invite a character to a guild.
		/// Stub: Guild type deleted during Laima merge.
		/// </summary>
		[PacketHandler(Op.CZ_GUILD_MEMBER_INVITE_BY_WEB)]
		public void CZ_GUILD_MEMBER_INVITE_BY_WEB(IZoneConnection conn, Packet packet)
		{
			var teamName = packet.GetString();
			// Removed: Guild type deleted during Laima merge
			Log.Warning("CZ_GUILD_MEMBER_INVITE_BY_WEB: Guild system not available. User '{0}'.", conn.Account.Name);
		}

		/// <summary>
		/// Sent when (un)locking an item.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_INV_ITEM_LOCK)]
		public void CZ_INV_ITEM_LOCK(IZoneConnection conn, Packet packet)
		{
			var worldId = packet.GetLong();
			var lockItem = packet.GetBool();

			var character = conn.SelectedCharacter;

			var item = character.Inventory.GetItem(worldId);
			if (item == null)
			{
				Log.Warning("CZ_INV_ITEM_LOCK: User '{0}' tried to lock non-existent item.", conn.Account.Name);
				return;
			}

			item.IsLocked = lockItem;

			// Officials send the dict key as the item name, we might want
			// to add those to the item data.
			// <Item> item locked.
			// <Item> item unlocked.
			var sysMsg = lockItem ? "{Item}LockSuccess" : "{Item}UnlockSuccess";
			character.SystemMessage(sysMsg, new MsgParameter("Item", item.Data.Name));
			Send.ZC_ITEM_LOCK_STATE(character, item);
		}

		/// <summary> 
		/// Sent upon login, purpose unknown. (Dummy handler)
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_FIXED_NOTICE_SHOW)]
		public void CZ_FIXED_NOTICE_SHOW(IZoneConnection conn, Packet packet)
		{
			// No parameters
		}

		/// <summary>
		/// Sent when trying to use a pet egg instead of the item script.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_CHANGE_NAME)]
		public void CZ_REQUEST_CHANGE_NAME(IZoneConnection conn, Packet packet)
		{
			var worldId = packet.GetLong();
			var name = packet.GetString(32);
			var type = packet.GetString();

			var character = conn.SelectedCharacter;


			switch (type)
			{
				case "Companionhire":
				{
					var item = character.Inventory.GetItem(worldId);
					if (item == null)
					{
						Log.Warning("CZ_REQUEST_CHANGE_NAME: User '{0}' tried to use non-existent item.", conn.Account.Name);
						return;
					}

					if (!ZoneServer.Instance.Data.MonsterDb.TryFind(item.Data.Script.StrArg, out var monster))
					{
						Log.Warning("CZ_REQUEST_CHANGE_NAME: User '{0}' tried to create a non-existent monster {1} as a companion.", conn.Account.Name, item.Data.Script.StrArg);
						return;
					}

					var companion = new Companion(character, monster.Id, RelationType.Friendly);
					companion.Name = name;
					companion.InitProperties();
					character.Companions.CreateCompanion(companion);
					character.ExecuteClientScript(ClientScripts.PET_ADOPT_SUCCESS);
					companion.SetCompanionState(true);
					character.Inventory.Remove(item.ObjectId, 1, InventoryItemRemoveMsg.Used);
					break;
				}
				default:
					Log.Warning("CZ_REQUEST_CHANGE_NAME: User '{0}' sent an unknown type {1}.", conn.Account.Name, type);
					break;
			}
		}

		/// <summary>
		/// Sent upon logout or when closing the automatch window.
		/// Cancels individual dungeon matching. If the player is in a party
		/// auto-match session, only the leader can cancel.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CANCEL_INDUN_MATCHING)]
		public void CZ_CANCEL_INDUN_MATCHING(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;
			var party = conn.Party;

			// If in a party auto-match session, only the leader can cancel
			if (party != null && ZoneServer.Instance.World.AutoMatch.IsPartyInQueue(party))
				return;

			// Solo queue cancel
			ZoneServer.Instance.World.AutoMatch.RemoveCharacterFromQueue(character);

			// Sends the request to exit auto match message to Coordinator
			var commMessage = new AutoMatchMessage(conn.SelectedCharacter.DbId, conn.Account.ObjectId, 0, false);
			ZoneServer.Instance.Communicator.Send("Coordinator", commMessage.BroadcastTo("Chat"));

			// Notify the client about the auto match state
			Send.ZC_NORMAL.DungeonAutoMatching(conn, 0);
		}

		/// <summary>
		/// Sent when a player attempts to cancel party auto-matching.
		/// Only the party leader can cancel.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CANCEL_INDUN_PARTY_MATCHING)]
		public void CZ_CANCEL_INDUN_PARTY_MATCHING(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;
			var party = conn.Party;

			if (character == null || party == null)
				return;

			if (!party.IsLeader(character, true))
				return;

			if (!ZoneServer.Instance.World.AutoMatch.IsPartyInQueue(party))
				return;

			var partyMembers = party.GetPartyMembers();

			foreach (var memberCharacter in partyMembers)
			{
				if (memberCharacter?.Connection?.Account == null)
					continue;

				// Sends the request to exit auto match message to Coordinator
				var commMessage = new AutoMatchMessage(memberCharacter.DbId, memberCharacter.Connection.Account.ObjectId, 0, false);
				ZoneServer.Instance.Communicator.Send("Coordinator", commMessage.BroadcastTo("Chat"));

				// Notify the client about the auto match state
				Send.ZC_NORMAL.DungeonAutoMatching(memberCharacter.Connection, 0);
				Send.ZC_NORMAL.DungeonAutoMatchWithParty(memberCharacter.Connection, 0, 0, 0, 0, "");
				Send.ZC_ADDON_MSG(memberCharacter, AddonMessage.INDUN_ASK_PARTY_MATCH, 0, "None");
			}

			ZoneServer.Instance.World.AutoMatch.DequeueParty(party);
		}

		/// <summary>
		/// Confirm that the player will be ready to start the auto match
		/// queue if there are only 2 or less players in queue alone.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_UNDERSTAFF_ENTER_ALLOW)]
		public void CZ_REQ_UNDERSTAFF_ENTER_ALLOW(IZoneConnection conn, Packet packet)
		{
			var commMessage = new AutoMatchReadyMessage(conn.SelectedCharacter.DbId);
			ZoneServer.Instance.Communicator.Send("Coordinator", commMessage.BroadcastTo("Chat"));
		}

		/// <summary>
		/// Confirm that the party will be ready to start the auto match
		/// queue if there are only 2 or less players in queue.
		/// When the party leader clicks this, automatically set all party members as ready
		/// since a party will always have at least 2 players.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_UNDERSTAFF_ENTER_ALLOW_WITH_PARTY)]
		public void CZ_REQ_UNDERSTAFF_ENTER_ALLOW_WITH_PARTY(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;
			var party = conn.Party;

			// If character has a party, send ready messages for all party members
			if (party != null)
			{
				// Only the party leader can set the party to ready for understaffed match
				if (!party.IsLeader(character))
				{
					Log.Warning("CZ_REQ_UNDERSTAFF_ENTER_ALLOW_WITH_PARTY: Character '{0}' attempted to set party ready but is not the party leader.", character.Name);
					return;
				}

				var partyMembers = party.GetPartyMembers();
				foreach (var member in partyMembers)
				{
					if (member == null)
						continue;

					var commMessage = new AutoMatchReadyMessage(member.DbId);
					ZoneServer.Instance.Communicator.Send("Coordinator", commMessage.BroadcastTo("Chat"));
				}

				Log.Info("CZ_REQ_UNDERSTAFF_ENTER_ALLOW_WITH_PARTY: Party (leader: '{0}', size: {1}) all members set to ready.",
					character.Name, partyMembers.Count);
			}
			else
			{
				// Fallback for solo player (shouldn't happen with party handler, but just in case)
				Log.Warning("CZ_REQ_UNDERSTAFF_ENTER_ALLOW_WITH_PARTY: Character '{0}' sent party ready request but has no party.", character.Name);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		//[PacketHandler(Op.CZ_PVP_COMMAND)]
		public void CZ_PVP_COMMAND(IZoneConnection conn, Packet packet)
		{
			// No parameters
		}

		/// <summary>
		/// Sent upon logout. Presumably cancels "dungeon registration"?
		/// (Dummy handler)
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CLEAR_INDUN_REG)]
		public void CZ_CLEAR_INDUN_REG(IZoneConnection conn, Packet packet)
		{
			// No parameters
		}

		/// <summary>
		/// Fishing rank request
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_FISHING_RANK)]
		public void CZ_REQ_FISHING_RANK(IZoneConnection conn, Packet packet)
		{
			var type = packet.GetString(64);

			if (type == "SuccessCount" || type == "GoldenFish" || type == "FishRubbing")
				Send.ZC_NORMAL.FishingRankData(conn, type);
		}

		/// <summary>
		/// Attempts to sync the character position with the server and other entities on the map.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SYNC_POS)]
		public void CZ_SYNC_POS(IZoneConnection conn, Packet packet)
		{
			var handle = packet.GetInt();
			var position = packet.GetPosition();

			var character = conn.SelectedCharacter;

			// Sanity checks...
			// Sync position for the character with the handle? ...
			if (character.Handle != handle)
				return;

			character.SetPosition(position);
		}

		/// <summary>
		/// Sent upon login. (Dummy handler)
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_ADVENTURE_BOOK_RANK)]
		public void CZ_REQ_ADVENTURE_BOOK_RANK(IZoneConnection conn, Packet packet)
		{
			var str = packet.GetString(128);
			var i1 = packet.GetInt();

			// TODO: Send.ZC_ADVENTURE_BOOK_INFO
			Send.ZC_NORMAL.AdventureBookRank(conn);
		}

		/// <summary>
		/// Request to execute a transaction script function with a string
		/// argument.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_NORMAL_TX)]
		public void CZ_REQ_NORMAL_TX(IZoneConnection conn, Packet packet)
		{
			var classId = packet.GetShort();
			var strArg = packet.GetString(Versions.Protocol > 500 ? 33 : 16);

			var character = conn.SelectedCharacter;

			// Get data
			if (!ZoneServer.Instance.Data.NormalTxDb.TryFind(classId, out var data))
			{
				Log.Warning("CZ_REQ_NORMAL_TX: User '{0}' sent an unknown dialog transaction id: {1}", conn.Account.Name, classId);
				return;
			}

			// Get handler
			if (!ScriptableFunctions.NormalTx.TryGet(data.Script, out var scriptFunc))
			{
				Log.Debug("CZ_REQ_NORMAL_TX: No handler registered for transaction script '{0}(\"{1}\")'", data.Script, strArg);
				return;
			}

			// Try to execute transaction
			try
			{
				var result = scriptFunc(character, strArg);
				if (result == NormalTxResult.Fail)
				{
					Log.Debug("CZ_REQ_NORMAL_TX: Execution of script '{0}(\"{1}\")' failed.", data.Script, strArg);
				}
			}
			catch (Exception ex)
			{
				Log.Debug("CZ_REQ_NORMAL_TX: Exception while executing script '{0}(\"{1}\")': {2}", data.Script, strArg, ex);
			}
		}

		/// <summary>
		/// Request to execute a transaction script function with numeric
		/// arguments.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_NORMAL_TX_NUMARG)]
		public void CZ_REQ_NORMAL_TX_NUMARG(IZoneConnection conn, Packet packet)
		{
			var size = packet.GetShort();
			var classId = packet.GetShort();
			var argCount = packet.GetInt();
			var numArgs = packet.GetList(argCount, packet.GetInt);

			var character = conn.SelectedCharacter;

			// Get data
			if (!ZoneServer.Instance.Data.NormalTxDb.TryFind(classId, out var data))
			{
				Log.Warning("CZ_REQ_NORMAL_TX_NUMARG: User '{0}' sent an unknown dialog transaction id: {1}", conn.Account.Name, classId);
				return;
			}

			// Get handler
			if (!ScriptableFunctions.NormalTxNum.TryGet(data.Script, out var scriptFunc))
			{
				Log.Debug("CZ_REQ_NORMAL_TX_NUMARG: No handler registered for transaction script '{0}({1})'", data.Script, string.Join(", ", numArgs));
				return;
			}

			// Try to execute transaction
			try
			{
				var result = scriptFunc(character, numArgs);
				if (result == NormalTxResult.Fail)
				{
					Log.Debug("CZ_REQ_NORMAL_TX_NUMARG: Execution of script '{0}({1})' failed.", data.Script, string.Join(", ", numArgs));
				}
			}
			catch (Exception ex)
			{
				Log.Debug("CZ_REQ_NORMAL_TX_NUMARG: Exception while executing script '{0}({1})': {2}", data.Script, string.Join(", ", numArgs), ex);
			}
		}

		/// <summary>
		/// Transaction requests from the player as per the TX item data.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_DIALOG_TX)]
		public void CZ_DIALOG_TX(IZoneConnection conn, Packet packet)
		{
			var size = packet.GetShort();
			var classId = packet.GetInt();
			var itemCount = packet.GetShort();
			var numArgCount = packet.GetShort();
			var strArgCount = packet.GetShort();
			var dialogTxItems = packet.GetList(itemCount, packet.GetDialogTxItem);
			var numArgs = packet.GetList(numArgCount, packet.GetInt);
			var strArgs = packet.GetList(strArgCount, packet.GetLpString);

			var character = conn.SelectedCharacter;

			// Get data
			if (!ZoneServer.Instance.Data.DialogTxDb.TryFind(classId, out var data))
			{
				character.ServerMessage(Localization.Get("Apologies, something went wrong there. Please report this issue."));
				Log.Warning("CZ_DIALOG_TX: User '{0}' sent an unknown dialog transaction id: {1}", conn.Account.Name, classId);
				return;
			}

			DialogTxScriptFunc scriptFunc;

			// Workaround for debugging Scriptable Functions.
			//if (data.Script == "SCR_TX_REPAIR")
			//scriptFunc = Test.SCR_TX_REPAIR;

			// Get handler
			if (!ScriptableFunctions.DialogTx.TryGet(data.Script, out scriptFunc))
			{
				character.ServerMessage(Localization.Get("This action has not been implemented yet."));
				Log.Debug("CZ_DIALOG_TX: No handler registered for transaction script '{0}'", data.Script);
				return;
			}

			// Get items from character
			var txItems = new Scripting.DialogTxItem[itemCount];
			for (var i = 0; i < dialogTxItems.Length; ++i)
			{
				var dialogTxItem = dialogTxItems[i];

				var item = character.Inventory.GetInventoryItem(dialogTxItem.ItemObjectId);
				if (item == null)
				{
					Log.Warning("CZ_DIALOG_TX: User '{0}' tried to use an item they don't have.", conn.Account.Name);
					return;
				}

				if (item.Amount < dialogTxItem.Amount)
				{
					Log.Warning("CZ_DIALOG_TX: User '{0}' tried to use more items than they have.", conn.Account.Name);
					return;
				}

				txItems[i] = new Scripting.DialogTxItem(item, dialogTxItem.Amount);
			}

			// Try to execute transaction
			var args = new DialogTxArgs(character, txItems, numArgs, strArgs);

			try
			{
				var result = scriptFunc(character, args);
				if (result == DialogTxResult.Fail)
				{
					character.ServerMessage(Localization.Get("Apologies, something went wrong there. Please report this issue."));
					Log.Debug("CZ_DIALOG_TX: Execution of script '{0}' failed.", data.Script);

				}
			}
			catch (Exception ex)
			{
				character.ServerMessage(Localization.Get("Apologies, something went wrong there. Please report this issue."));
				Log.Debug("CZ_DIALOG_TX: Exception while executing script '{0}': {1}", data.Script, ex);
			}
		}

		/// <summary>
		/// ? (Dummy)
		/// </summary>
		/// <remarks>
		/// The client sends this packet repeatedly until it gets an
		/// appropriate response.
		/// </remarks>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_GUILD_INDEX)]
		public void CZ_REQUEST_GUILD_INDEX(IZoneConnection conn, Packet packet)
		{
			var accountId = packet.GetLong();

			var account = ZoneServer.Instance.World.GetCharacter(c => c.AccountDbId == accountId);
			if (account != null)
			{
				var character = account.Connection.SelectedCharacter;
				// Removed: Guild type deleted during Laima merge
				Send.ZC_RESPONSE_GUILD_INDEX(conn, character, null);
			}
		}

		/// <summary>
		/// Sent during login after an unexpected disconnect.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_DISCONNECT_REASON_FOR_LOG)]
		public void CZ_DISCONNECT_REASON_FOR_LOG(IZoneConnection conn, Packet packet)
		{
			var size = packet.GetShort();
			var valueCount = packet.GetInt();
			var i2 = packet.GetInt();
			var i3 = packet.GetInt();

			Log.Debug("CZ_DISCONNECT_REASON_FOR_LOG:");

			for (var i = 0; i < valueCount; ++i)
			{
				var name = packet.GetLpString();
				var value = packet.GetLpString();

				Log.Debug("  {0}: {1}", name, value);
			}
		}

		/// <summary>
		/// Sent regularly from the client (every 10 seconds).
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_HEARTBEAT)]
		public void CZ_HEARTBEAT(IZoneConnection conn, Packet packet)
		{
			var secondsSinceStart = packet.GetFloat();

			// If it's been more than 60 seconds since last heart beat, disconnect the client.
			if (conn.LastHeartBeat < DateTime.Now.AddSeconds(-60))
				conn.Close();
			conn.LastHeartBeat = DateTime.Now;
		}

		[PacketHandler(Op.CZ_WAREHOUSE_TAKE_LIST)]
		public void CZ_WAREHOUSE_TAKE_LIST(IZoneConnection conn, Packet packet)
		{
			var size = packet.GetShort();
			var type = (InventoryType)packet.GetByte();
			var itemCount = packet.GetInt();
			var i0 = packet.GetInt();

			var character = conn.SelectedCharacter;

			if (type == InventoryType.TeamStorage)
			{
				if (character.CurrentStorage is not TeamStorage storage || !storage.IsBrowsing)
				{
					Log.Warning("CZ_WAREHOUSE_TAKE_LIST: User '{0}' tried to manage their team storage without it being open.", conn.Account.Name);
					return;
				}

				// Retrieve silver
				var silverItem = storage.GetSilver();

				for (var i = 0; i < itemCount; i++)
				{
					var worldId = packet.GetLong();
					var amount = packet.GetInt();
					var i1 = packet.GetInt();

					// Note: For some reason, client may send worldId zero
					// when trying to retrieve silver.
					if ((silverItem != null && silverItem.ObjectId == worldId) || worldId == 0)
					{
						if (storage.RetrieveSilver(amount) != StorageResult.Success)
						{
							// Log.Debug("CZ_WAREHOUSE_TAKE_LIST: User '{0}' failed to retrieve silver {1} with amount {2}.", conn.Account.Name, worldId, amount);
						}
					}
					else if (storage.RetrieveItem(worldId, amount) != StorageResult.Success)
					{
						Log.Warning("CZ_WAREHOUSE_TAKE_LIST: User '{0}' failed to retrieve item {1} with amount {2}.", conn.Account.Name, worldId, amount);
					}
				}
			}
		}

		/// <summary>
		/// Sent when dashing.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_DASHRUN)]
		public void CZ_DASHRUN(IZoneConnection conn, Packet packet)
		{
			var b1 = packet.GetByte();
			var start = packet.GetByte(); // 1 = start dash, 0 = stop dash

			var character = conn.SelectedCharacter;
			if (character == null)
				return;

			if (character.IsWarping || character.IsSitting)
				return;

			// Only allow dashing for swordsmen, unless the respective
			// feature was enabled.
			if (character.JobClass != JobClass.Swordsman && !Feature.IsEnabled(FeatureId.DashingForAll))
				return;

			// Only start the buff when the client signals to start dashing
			// start = 1 for normal dash, start = 3 for mounted dash
			if (start != 1 && start != 3)
				return;

			// For some reason this packet is sent multiple times while
			// the character is dashing, which is a potential problem if
			// DashRun gets stacked and started again, but the buff manager
			// should handle it. Alternatively, we could also add a check
			// here, to see if DashRun is already active. What's better
			// is TBD.
			character.StartBuff(BuffId.DashRun);
		}

		/// <summary>
		/// Sent during loading screen.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_MYTHIC_DUNGEON_REQUEST_CURRENT_SEASON)]
		public void CZ_MYTHIC_DUNGEON_REQUEST_CURRENT_SEASON(IZoneConnection conn, Packet packet)
		{
		}

		/// <summary>
		/// Sent on game loaded.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_QUICKSLOT_LIST)]
		public void CZ_REQ_QUICKSLOT_LIST(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;
			Send.ZC_QUICK_SLOT_LIST(character);
		}

		/// <summary>
		/// Swap items in storage.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SWAP_ITEM_IN_WAREHOUSE)]
		public void CZ_SWAP_ITEM_IN_WAREHOUSE(IZoneConnection conn, Packet packet)
		{
			var fromSlot = packet.GetInt();
			var toSlot = packet.GetInt();
			var item1ObjectId = packet.GetLong();
			var item2ObjectId = packet.GetLong();

			var character = conn.SelectedCharacter;
			var storage = character.CurrentStorage;

			if (!storage.IsBrowsing)
			{
				Log.Warning("CZ_SWAP_ITEM_IN_WAREHOUSE: User '{0}' tried to manage their personal storage without it being open.", conn.Account.Name);
				return;
			}

			storage.Swap(fromSlot, toSlot);
		}

		/// <summary>
		/// Sent on game loaded.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_DO_CLIENT_MOVE_CHECK)]
		public void CZ_DO_CLIENT_MOVE_CHECK(IZoneConnection conn, Packet packet)
		{
		}

		/// <summary>
		/// Sent on Popo Shop Opening
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_PCBANG_SHOP_UI)]
		public void CZ_REQ_PCBANG_SHOP_UI(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;

			Send.ZC_PCBANG_SHOP_POINTSHOP_CATALOG(character);
			Send.ZC_PCBANG_SHOP_COMMON(character);
			Send.ZC_PCBANG_SHOP_POINTSHOP_BUY_COUNT(character);
		}

		/// <summary>
		/// Sent on purchasing an item from the Popo Shop
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_PCBANG_SHOP_PURCHASE)]
		public void CZ_REQ_PCBANG_SHOP_PURCHASE(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;
			// If can afford send ZC_PCBANG_POINT otherwise send
			// Not enough Popo Points
			character.SystemMessage("PCBangLackOfPoint");
		}

		/// <summary>
		/// Sent on Popo Shop Refreshing
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_PCBANG_SHOP_REFRESH)]
		public void CZ_REQ_PCBANG_SHOP_REFRESH(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;
			Send.ZC_PCBANG_SHOP_POINTSHOP_CATALOG(character);
			Send.ZC_PCBANG_SHOP_COMMON(character);
			Send.ZC_PCBANG_SHOP_POINTSHOP_BUY_COUNT(character);
		}

		/// <summary>
		/// Sent on opening Beauty Shop
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_BEAUTYSHOP_INFO)]
		public void CZ_REQ_BEAUTYSHOP_INFO(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;
			Send.ZC_RES_BEAUTYSHOP_PURCHASED_HAIR_LIST(character);
		}

		/// <summary>
		/// Sent on opening Beauty Shop Try New Items
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SEND_BEAUTYSHOP_TRYITON_LIST)]
		public void CZ_SEND_BEAUTYSHOP_TRYITON_LIST(IZoneConnection conn, Packet packet)
		{
			var size = packet.GetShort();
			var count = packet.GetInt();

			BeautyStyle[] beautyStyles;
			if (count > 0)
			{
				beautyStyles = new BeautyStyle[count];
				for (var i = 0; i < count; i++)
				{
					beautyStyles[i] = new BeautyStyle
					{
						StyleName = packet.GetString(256),
						StyleMod = packet.GetString(256),
						StyleType = packet.GetString(256),
						Value = packet.GetInt()
					};
				}
			}
			var character = conn.SelectedCharacter;
			//
			//Send.ZC_NORMAL.Unknown_0D();
			//Send.ZC_RES_BEAUTYSHOP_PURCHASED_HAIR_LIST(character);
		}

		/// <summary>
		/// Sent on Opening Skill Window
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_COMMON_SKILL_LIST)]
		public void CZ_REQ_COMMON_SKILL_LIST(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;

			Send.ZC_COMMON_SKILL_LIST(character);
		}

		/// <summary>
		/// Send when clicking on a time action Cancel button.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_STOP_TIMEACTION)]
		public void CZ_STOP_TIMEACTION(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;
			var result = packet.GetByte();

			character?.Components?.Get<TimeActionComponent>()?.Resume();
		}

		/// <summary>
		/// Sent on Opening Commander/Inventory Window
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_COMMANDER_INFO)]
		public void CZ_REQ_COMMANDER_INFO(IZoneConnection conn, Packet packet)
		{
			Send.ZC_TRUST_INFO(conn);
		}

		/// <summary>
		/// Sent on attempting to create a guild
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CHECK_USE_NAME)]
		public void CZ_CHECK_USE_NAME(IZoneConnection conn, Packet packet)
		{
			var type = packet.GetString(58);
			var name = packet.GetString(64);
			var character = conn.SelectedCharacter;

			switch (type)
			{
				case "GuildName":
					// TODO: Validate Name versus database
					if (!ZoneServer.Instance.Database.GuildNameExists(name))
						Send.ZC_ADDON_MSG(character, AddonMessage.ENABLE_CREATE_GUILD_NAME, 0, name);
					break;
			}
		}

		/// <summary>
		/// Sent on Opening Map from Quest Log
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_FIELD_BOSS_EXIST)]
		public void CZ_REQ_FIELD_BOSS_EXIST(IZoneConnection conn, Packet packet)
		{
			Send.ZC_RESPONSE_FIELD_BOSS_EXIST(conn);
		}

		/// <summary>
		/// Sent when a shop is opened?
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_OPEN_SHOP_LOG)]
		public void CZ_OPEN_SHOP_LOG(IZoneConnection conn, Packet packet)
		{
			// TODO: Log the shop type?
		}

		/// <summary>
		/// Sent when setting Custom Greeting Message
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_PC_COMMENT_CHANGE)]
		public void CZ_PC_COMMENT_CHANGE(IZoneConnection conn, Packet packet)
		{
			var type = packet.GetInt(); // 0?
			var message = packet.GetLpString();

			var character = conn.SelectedCharacter;

			if (character != null)
			{
				character.GreetingMessage = message;
				//Send.ZC_NORMAL.SetGreetingMessage(character);
			}
		}

		/// <summary>
		/// Sent to continue dialog?
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_LEARN_ABILITY)]
		public void CZ_REQ_LEARN_ABILITY(IZoneConnection conn, Packet packet)
		{
			var abilityLevelAdds = new Dictionary<int, int>();

			var size = packet.GetShort();
			var category = packet.GetString(32);
			var count = packet.GetInt();

			for (var i = 0; i < count; i++)
			{
				var abilityId = packet.GetInt();
				var level = packet.GetInt();

				abilityLevelAdds[abilityId] = level;
			}

			var character = conn.SelectedCharacter;

			var abilityTreeEntries = ZoneServer.Instance.Data.AbilityTreeDb.Find(category);
			if (!abilityTreeEntries.Any())
			{
				Log.Warning("CZ_REQ_LEARN_ABILITY: User '{0}' tried to learn abilities from an unknown category ({1}).", character.Username, category);
				return;
			}

			foreach (var kv in abilityLevelAdds)
			{
				var classId = kv.Key;
				var addLevels = Math.Max(0, kv.Value);

				var abilityTreeData = abilityTreeEntries.FirstOrDefault(a => a.ClassId == classId);
				if (abilityTreeData == null)
				{
					Log.Warning("CZ_REQ_LEARN_ABILITY: User '{0}' tried to learn the unknown ability '{1}' from category '{2}'.", character.Username, classId, category);
					return;
				}

				var abilityData = ZoneServer.Instance.Data.AbilityDb.Find(abilityTreeData.AbilityId);
				if (abilityData == null)
				{
					Log.Warning("CZ_REQ_LEARN_ABILITY: Ability data '{0}' not found for ability '{1}' from category '{2}'.", abilityTreeData.AbilityId, classId, category);
					return;
				}

				var currentLevel = character.Abilities.GetLevel(abilityData.Id);
				var newLevel = currentLevel + addLevels;
				var maxLevel = abilityTreeData.MaxLevel;

				var totalPrice = 0;
				var totalTime = 0;

				// Skill Upgrade abilities require a Mystic Tome, if you don't have one, it sends this message.
				//character.SystemMessage("NotEnoughMasterPiece");

				if (abilityTreeData.HasUnlockScript)
				{
					if (!ScriptableFunctions.AbilityUnlock.TryGet(abilityTreeData.UnlockScriptName, out var unlockFunc))
					{
						Log.Warning("CZ_REQ_LEARN_ABILITY: Ability unlock function '{0}' not found.", abilityTreeData.UnlockScriptName);
						return;
					}

					var canLearn = unlockFunc(character, abilityTreeData.UnlockScriptArgStr, abilityTreeData.UnlockScriptArgNum, abilityData);
					if (!canLearn)
					{
						Log.Warning("CZ_REQ_LEARN_ABILITY: User '{0}' tried to learn an ability they haven't unlocked yet (Ability: {1}, Unlock: {2}).", character.Username, abilityData.ClassName, abilityTreeData.UnlockScriptName);
						return;
					}
				}

				if (abilityTreeData.HasPriceTimeScript)
				{
					if (!ScriptableFunctions.AbilityPrice.TryGet(abilityTreeData.PriceTimeScript, out var priceTimeFunc))
					{
						Log.Warning("CZ_REQ_LEARN_ABILITY: Ability calculation function '{0}' not found.", abilityTreeData.PriceTimeScript);
						return;
					}

					for (var i = currentLevel + 1; i <= newLevel; ++i)
					{
						priceTimeFunc(character, abilityData, i, maxLevel, out var price, out var time);
						totalPrice += price;
						totalTime += time;
					}
				}

				if (character.Properties.AbilityPoints < totalPrice)
				{
					// Don't warn about this, as the client allows the
					// player to send the request even if they don't
					// have enough points.
					//Log.Warning("CZ_REQ_LEARN_ABILITY: User '{0}' didn't have enough ability points to learn all abilities.", character.Username);

					character.MsgBox(Localization.Get("You don't have enough points."));
					return;
				}

				character.Abilities.Learn(abilityData.Id, newLevel);
				var abilitypoints = character.ModifyAbilityPoints(-totalPrice);

				Send.ZC_ADDON_MSG(character, AddonMessage.SUCCESS_LEARN_ABILITY, abilitypoints, abilityData.ClassName);
				Send.ZC_ADDON_MSG(character, AddonMessage.RESET_ABILITY_UP, 0, null);
			}
			character.Skills.InvalidateAll();
		}

		/// <summary>
		/// Request to toggle an ability on or off.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_TOGGLE_ABILITY)]
		public void CZ_REQ_TOGGLE_ABILITY(IZoneConnection conn, Packet packet)
		{
			var abilityId = (AbilityId)packet.GetInt();

			var character = conn.SelectedCharacter;
			var ability = character.Abilities.Get(abilityId);

			if (ability == null)
			{
				Log.Warning("CZ_REQ_TOGGLE_ABILITY: User '{0}' tried to toggle an ability they don't have ({1}).", character.Username, abilityId);
				return;
			}

			ability.Active = !ability.Active;

			Send.ZC_OBJECT_PROPERTY(conn, ability, PropertyName.ActiveState);
			Send.ZC_ADDON_MSG(character, "RESET_ABILITY_ACTIVE", ability.Active ? 1 : 0, ability.Data.ClassName);
		}

		[PacketHandler(Op.CZ_OPEN_HELP)]
		public void CZ_OPEN_HELP(IZoneConnection conn, Packet packet)
		{
			var helpClassName = packet.GetString();

			var character = conn.SelectedCharacter;
			// TODO: Decide to open help or not.
			character.Tutorials.Show(helpClassName);
		}

		/// <summary>
		/// Purpose unknown, potentially related to Heroic Tale feature.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_DRAW_TOSHERO_EMBLEM)]
		public void CZ_REQUEST_DRAW_TOSHERO_EMBLEM(IZoneConnection conn, Packet packet)
		{
			Send.ZC_ADDON_MSG(conn.SelectedCharacter, "TOSHERO_ZONE_ENTER", 0, null);
		}

		/// <summary>
		/// Request to view another character's information.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_PROPERTY_COMPARE)]
		public void CZ_PROPERTY_COMPARE(IZoneConnection conn, Packet packet)
		{
			var handle = packet.GetInt();
			var openWindow = packet.GetBool();
			var like = packet.GetBool();

			var character = conn.SelectedCharacter;

			if (!character.Map.TryGetCharacter(handle, out var targetCharacter))
			{
				Log.Warning("CZ_PROPERTY_COMPARE: Attempted to compare an unknown character '{0}'.", handle);
				return;
			}

			// The response does not appear to include the number of likes.
			// Instead, it seems like the client is supposed to get that
			// information from the relation server, as there's a request
			// op for it. This is not sent currently though.

			Send.ZC_PROPERTY_COMPARE(conn, targetCharacter, openWindow, like);
			if (like)
			{
				//TODO Send poses and rotate?
				Send.ZC_NORMAL.SkillProjectile(character, character.Position.GetRandomInRange2D(50, 100),
					"I_like_force#Dummy_bufficon", 1f, null, 1f, 3.5f, TimeSpan.FromMilliseconds(100f));
				character.PlayEffect("F_sys_like3#Bip01 Pelvis", 2);
				character.SystemMessage("{Name}Like{Who}", new MsgParameter("Name", character.TeamName), new MsgParameter("Who", targetCharacter.TeamName));
			}
		}

		/// <summary>
		/// Sent when selecting a new language.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SELECTED_LANGUAGE)]
		public void CZ_SELECTED_LANGUAGE(IZoneConnection conn, Packet packet)
		{
			var language = packet.GetShort();

			// 0 = English, 1 = German, 2 = Portugese,
			// 4 = Indonesian, 5 = Russian, 6 = Thai
		}

		/// <summary>
		/// Request to create an auto seller shop.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REGISTER_AUTOSELLER)]
		public void CZ_REGISTER_AUTOSELLER(IZoneConnection conn, Packet packet)
		{
			var size = packet.GetShort();
			var shopName = packet.GetString(64);
			var itemCount = packet.GetInt();
			var group = packet.GetInt();

			var character = conn.SelectedCharacter;
			ShopData shop;

			if (!ZoneServer.Instance.Data.PacketStringDb.TryFind(group, out var packetString))
			{
				Log.Debug($"CZ_REGISTER_AUTOSELLER: Unknown shop group {group} received.");
				return;
			}

			var skillId = 0;
			switch (packetString.Name)
			{
				case "Buff":
				case "Oblation":
					skillId = packet.GetInt();
					break;
			}

			if (character.Connection.ShopCreated != null)
			{
				if (itemCount != -1)
				{
					Log.Warning("CZ_REGISTER_AUTOSELLER: Already has a shop open.");
					return;
				}
				shop = character.Connection.ShopCreated;
				shop.IsClosed = true;
				Send.ZC_AUTOSELLER_LIST(conn, character);
				Send.ZC_AUTOSELLER_TITLE(character);
				Send.ZC_NORMAL.ShopAnimation(conn, character, "Squire_Repair", 1, 0);
				character.Connection.ShopCreated = null;
			}
			else
			{

				if (group != -1)
				{
					// BUY SHOP - Owner wants to buy items from visitors
					shop = new ShopData
					{
						Name = shopName,
						EffectId = group
					};
					switch (group)
					{
						case 8657:
							shop.Type = PersonalShopType.SpellShop;
							break;
						case 3076:
							shop.Type = PersonalShopType.ItemAwakening;
							break;
						case 4376:
							shop.Type = PersonalShopType.Repair;
							break;
						case 8565:
							shop.Type = PersonalShopType.Portal;
							break;
						// Oblation
						case 8590:
							shop.Type = PersonalShopType.Oblation;
							break;
						default:
							Log.Debug("Unknown Shop Type: {0}", group);
							shop.Type = PersonalShopType.Personal;
							break;
					}

					if (Versions.Protocol > 500)
					{
						if (skillId != (int)SkillId.Pardoner_Oblation)
						{
							for (var i = 0; i < itemCount; i++)
							{
								var itemId = packet.GetInt();
								var index = packet.GetInt();
								var requiredAmount = packet.GetInt();
								var price = packet.GetInt();
								var amount = packet.GetInt();
								if (skillId == 0)
									packet.GetBin(260);
								else
									packet.GetBin(256);
								var product = new ProductData();
								if (skillId == 0)
								{
									product.ItemId = itemId;
									product.Price = price;
									product.Amount = amount;
									product.RequiredAmount = requiredAmount;
									shop.Products.Add(index, product);
								}
								else
								{
									product.ItemId = itemId;
									product.Price = requiredAmount;
									shop.AddProduct(product);
								}
							}
						}
					}
					else
					{
						for (var i = 0; i < itemCount; i++)
						{
							var index = packet.GetInt();
							var itemId = packet.GetInt();
							var requiredAmount = packet.GetInt();
							var price = packet.GetInt();
							var amount = packet.GetInt();
							var product = new ProductData();
							product.ItemId = itemId;
							product.Price = price;
							product.Amount = amount;
							product.RequiredAmount = requiredAmount;
							shop.Products.Add(index, product);
						}
					}
				}
				else
				{
					// SELL SHOP - Owner wants to sell their items to visitors (group == -1)
					var shopBuilder = new ShopBuilder(shopName);
					if (Versions.Protocol > 500)
					{
						for (var i = 0; i < itemCount; i++)
						{
							var itemId = packet.GetInt();
							var index = packet.GetInt();
							var requiredAmount = packet.GetInt();
							var price = packet.GetInt();
							var amount = packet.GetInt();
							if (skillId == 0)
								packet.GetBin(260);
							else
								packet.GetBin(256);

							// IMPORTANT: For sell shops, verify the seller has the items and track by world ID
							// Get the actual items from inventory to store their unique world IDs
							var items = character.Inventory.GetItems(item => item.Id == itemId);
							var worldIds = new List<long>();
							var totalAmount = 0;

							foreach (var itemEntry in items)
							{
								var item = itemEntry.Value;
								var takeAmount = Math.Min(item.Amount, requiredAmount - totalAmount);
								if (takeAmount > 0)
								{
									worldIds.Add(item.ObjectId); // Store unique world ID once per stack
									totalAmount += takeAmount;
								}

								if (totalAmount >= requiredAmount)
									break;
							}

							if (totalAmount < requiredAmount)
							{
								character.SystemMessage("YouDontHaveEnoughItems");
								Log.Warning("CZ_REGISTER_AUTOSELLER: Player doesn't have enough items to sell. ItemId: {0}, Required: {1}, Found: {2}", itemId, requiredAmount, totalAmount);
								return;
							}

							shopBuilder.AddSellItem(itemId, requiredAmount, price, worldIds.ToArray());
						}
						shop = shopBuilder.Build();
						shop.Type = PersonalShopType.PersonalSell;
						shop.EffectId = packetString.Id;
					}
					else
					{
						for (var i = 0; i < itemCount; i++)
						{
							var index = packet.GetInt();
							var itemId = packet.GetInt();
							var requiredAmount = packet.GetInt();
							var price = packet.GetInt();
							var amount = packet.GetInt();

							// IMPORTANT: For sell shops, verify the seller has the items and track by world ID
							// Get the actual items from inventory to store their unique world IDs
							var items = character.Inventory.GetItems(item => item.Id == itemId);
							var worldIds = new List<long>();
							var totalAmount = 0;

							foreach (var itemEntry in items)
							{
								var item = itemEntry.Value;
								var takeAmount = Math.Min(item.Amount, requiredAmount - totalAmount);
								if (takeAmount > 0)
								{
									worldIds.Add(item.ObjectId); // Store unique world ID once per stack
									totalAmount += takeAmount;
								}

								if (totalAmount >= requiredAmount)
									break;
							}

							if (totalAmount < requiredAmount)
							{
								character.SystemMessage("YouDontHaveEnoughItems");
								Log.Warning("CZ_REGISTER_AUTOSELLER: Player doesn't have enough items to sell. ItemId: {0}, Required: {1}, Found: {2}", itemId, requiredAmount, totalAmount);
								return;
							}

							shopBuilder.AddSellItem(itemId, requiredAmount, price, worldIds.ToArray());
						}
						shop = shopBuilder.Build();
						shop.Type = PersonalShopType.PersonalSell;
						shop.EffectId = packetString.Id;
					}
				}
				shop.OwnerHandle = character.Handle;
				character.Connection.ShopCreated = shop;
				Send.ZC_AUTOSELLER_LIST(conn, character);
				Send.ZC_NORMAL.Shop_Unknown11C(conn, "Squire", shop.Type);
				Send.ZC_NORMAL.ShopAnimation(conn, character, "Squire_Repair", 1, 1);
				Send.ZC_AUTOSELLER_TITLE(character);

				Log.Debug("CZ_REGISTER_AUTOSELLER: {0}, {1} item(s), Type: {2}", shopName, itemCount, shop.Type);
			}
		}

		/// <summary>
		/// Request to interact with a player shop (buy/sell items).
		/// NOTE: Sellshops also use CZ_ITEM_BUY for the MeliaCustomShop dialog purchases.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_BUY_AUTOSELLER_ITEMS)]
		public void CZ_BUY_AUTOSELLER_ITEMS(IZoneConnection conn, Packet packet)
		{
			var size = packet.GetShort();
			var shopType = packet.GetInt();
			var shopOwnerHandle = packet.GetInt();
			var itemCount = packet.GetInt();
			var character = conn.SelectedCharacter;

			// Check distance
			if (!character.Map.TryGetCharacter(shopOwnerHandle, out var shopOwner) || !shopOwner.Position.InRange2D(character.Position, 150))
			{
				character.SystemMessage("FarFromFoodTable");
				return;
			}

			if (shopOwner.Connection.ShopCreated == null)
			{
				Log.Warning("CZ_BUY_AUTOSELLER_ITEMS: {0} has no shop open.", conn.Account.Name);
				return;
			}

			var shop = conn.ActiveShop = shopOwner.Connection.ShopCreated;

			for (var i = 0; i < itemCount; i++)
			{
				var indexOrSkillId = packet.GetInt();
				var worldId = packet.GetLong();
				var itemAmount = packet.GetInt();
				var i0 = packet.GetInt();

				if (!shop.IsCustom)
					ShopBuilder.HandleSellToBuyShop(conn, character, shopOwner, shop, indexOrSkillId, worldId, itemAmount);
				else
					ShopBuilder.HandleBuyFromSellShop(conn, character, shopOwner, shop, indexOrSkillId, worldId, itemAmount);
			}

			ShopBuilder.CloseShopIfEmpty(conn, shopOwner, shop);
		}

		/// <summary>
		/// Request to open/view a player shop.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_OPEN_AUTOSELLER)]
		public void CZ_OPEN_AUTOSELLER(IZoneConnection conn, Packet packet)
		{
			var shopType = packet.GetInt();
			var shopOwnerHandle = packet.GetInt();
			var optionSelected = packet.GetInt();
			var amount = packet.GetInt();
			var character = conn.SelectedCharacter;

			// Check distance
			if (!character.Map.TryGetCharacter(shopOwnerHandle, out var shopOwner) || !shopOwner.Position.InRange2D(character.Position, 25))
			{
				character.SystemMessage("FarFromFoodTable");
				return;
			}

			if (shopOwner.Connection.ShopCreated == null)
			{
				Log.Warning("CZ_OPEN_AUTOSELLER: {0} has no shop open.", conn.Account.Name);
				return;
			}

			var shop = conn.ActiveShop = shopOwner.Connection.ShopCreated;

			// ============================================================
			// BUYSHOP (IsCustom=false) - Visitor wants to SELL items TO shop owner
			// Uses ZC_AUTOSELLER_LIST packet only (Laima3 style)
			// ============================================================
			if (!shop.IsCustom)
			{
				Send.ZC_AUTOSELLER_LIST(conn, shopOwner);
				return;
			}

			// ============================================================
			// SELLSHOP (IsCustom=true) - Visitor wants to BUY items FROM shop owner
			// Uses MeliaCustomShop dialog system
			// ============================================================

			// Owner clicking their own sellshop - show management UI
			if (character.Handle == shopOwner.Handle)
			{
				Send.ZC_EXEC_CLIENT_SCP(conn, string.Format(
					"MY_AUTOSELL_LIST('PersonalShop', {0})",
					(int)PersonalShopType.PersonalSell));
				return;
			}

			// Visitor opening a sellshop - send custom shop data via Melia.Comm and open dialog
			Send.ZC_EXEC_CLIENT_SCP(conn, "Melia.Comm.BeginRecv('CustomShop')");

			var sb = new StringBuilder();
			foreach (var productData in shop.Products.Values)
			{
				// Get item properties for tooltip display
				var propsStr = "nil";
				if (productData.ItemWorldIds.Count > 0)
				{
					var firstWorldId = productData.ItemWorldIds[0];
					if (shopOwner.Inventory.TryGetItem(firstWorldId, out var item))
					{
						try
						{
							propsStr = item.SerializePropertiesToLua();
						}
						catch (Exception ex)
						{
							Log.Warning("Failed to serialize item properties for shop: {0}", ex.Message);
							propsStr = "nil";
						}
					}
				}

				// Format: { productId, itemId, amount, price, properties }
				sb.AppendFormat("{{ {0},{1},{2},{3},{4} }},", productData.Id, productData.ItemId, productData.Amount, productData.Price, propsStr);

				if (sb.Length > ClientScript.ScriptMaxLength * 0.8)
				{
					Send.ZC_EXEC_CLIENT_SCP(conn, $"Melia.Comm.Recv('CustomShop', {{ {sb} }})");
					sb.Clear();
				}
			}

			if (sb.Length > 0)
			{
				Send.ZC_EXEC_CLIENT_SCP(conn, $"Melia.Comm.Recv('CustomShop', {{ {sb} }})");
				sb.Clear();
			}

			Send.ZC_EXEC_CLIENT_SCP(conn, "Melia.Comm.ExecData('CustomShop', M_SET_CUSTOM_SHOP)");
			Send.ZC_EXEC_CLIENT_SCP(conn, "Melia.Comm.EndRecv('CustomShop')");
			Send.ZC_DIALOG_TRADE(conn, "MeliaCustomShop");
		}

		/// <summary>
		/// Request to close an open player shop.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_AUTTOSELLER_BUYER_CLOSE)]
		public void CZ_AUTOSELLER_BUYER_CLOSE(IZoneConnection conn, Packet packet)
		{
			var shopType = packet.GetInt();
			var shopOwnerHandle = packet.GetInt();

			var character = conn.SelectedCharacter;

			if (conn.ActiveShop == null)
			{
				Log.Warning("CZ_AUTOSELLER_BUYER_CLOSE: {0} has no shop open.", conn.Account.Name);
				return;
			}
			var shop = conn.ActiveShop;

			if (!character.Map.TryGetCharacter(shopOwnerHandle, out var shopOwner))
			{
				Log.Warning("CZ_AUTOSELLER_BUYER_CLOSE: {0} shop owner not found.", conn.Account.Name);
				return;
			}

			if (shop != shopOwner.Connection.ShopCreated)
			{
				Log.Warning("CZ_AUTOSELLER_BUYER_CLOSE: {0} tried to close a different shop than the one open.", conn.Account.Name);
				return;
			}

			conn.ActiveShop = null;
			Send.ZC_ENABLE_CONTROL(conn, "AUTOSELLER", true);
			Send.ZC_LOCK_KEY(character, "AUTOSELLER", false);
			switch (shopType)
			{
				case 2:
					Send.ZC_EXEC_CLIENT_SCP(conn, ClientScripts.SQUIRE_REPAIR_CANCEL);
					break;
				default:
					Log.Warning("CZ_AUTOSELLER_BUYER_CLOSE: {0} shop type close not implemented.", shopType);
					break;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		//[PacketHandler(Op.CZ_CARDBATTLE_CMD)]
		public void CZ_CARDBATTLE_CMD(IZoneConnection conn, Packet packet)
		{

		}

		/// <summary>
		/// Client ping request, sent through (//ping).
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_PING)]
		public void CZ_PING(IZoneConnection conn, Packet packet)
		{
			Send.ZC_PING(conn);
		}

		/// <summary>
		/// Set a character label (title).
		/// </summary>
		[PacketHandler(Op.CZ_CHANGE_TITLE)]
		public void CZ_CHANGE_TITLE(IZoneConnection conn, Packet packet)
		{
			var title = packet.GetString(64);

			Send.ZC_NORMAL.ActorLabel(conn.SelectedCharacter, title);
		}

		/// <summary>
		/// Request to pick up an item.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_ITEM_GET)]
		public void CZ_REQ_ITEM_GET(IZoneConnection conn, Packet packet)
		{
			var handle = packet.GetInt();

			var character = conn.SelectedCharacter;
			var monster = character.Map.GetMonster(handle);

			// Check for monster validity
			if (monster == null)
			{
				// Don't warn as it happens quite frequently when two
				// players stand in range of a dropped item.
				// Log.Warning("CZ_REQ_ITEM_GET: User '{0}' tried to pick up an item that doesn't exist.", conn.Account.Name);
				return;
			}

			if (monster is not ItemMonster itemMonster)
			{
				Log.Warning("CZ_REQ_ITEM_GET: User '{0}' tried to pick up a monster that is not an item.", conn.Account.Name);
				return;
			}

			// Accept pick ups only once the character is close enough.
			var pickUpRadius = ZoneServer.Instance.Conf.World.PickUpRadius;
			if (!monster.Position.InRange2D(character.Position, pickUpRadius))
				return;

			// Check if character is allowed to pick up the item.
			if (!itemMonster.CanBePickedUpBy(character))
				return;

			character.PickUp(itemMonster);
		}

		/// <summary>
		/// Request to change a character's guarding state.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_GUARD)]
		public void CZ_GUARD(IZoneConnection conn, Packet packet)
		{
			var active = packet.GetBool();
			var direction = packet.GetDirection();

			var character = conn.SelectedCharacter;
			character.Direction = direction;

			if (active && !character.CanGuard())
				return;

			character.SetGuardState(active);
			character.InvalidateProperties(PropertyName.BLK);
			Send.ZC_GUARD(character, active, direction);
		}

		[PacketHandler(Op.CZ_REQ_ACC_WARE_VIS_LOG)]
		public void CZ_REQ_ACC_WARE_VIS_LOG(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;

			if (character.CurrentStorage is not TeamStorage storage || !storage.IsBrowsing)
			{
				Log.Warning("CZ_REQ_ACC_WARE_VIS_LOG: User '{0}' tried to manage their team storage without it being open.", conn.Account.Name);
				return;
			}

			var transList = storage.GetSilverTransactions();

			Send.ZC_NORMAL.StorageSilverTransaction(character, transList, true);
		}

		/// <summary>
		/// Request for the latest channel traffic data, sent when the
		/// player opens the channel selection in-game.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_CHANNEL_TRAFFICS)]
		public void CZ_REQ_CHANNEL_TRAFFICS(IZoneConnection conn, Packet packet)
		{
			var serverGroupId = packet.GetShort();

			Send.ZC_NORMAL.ChannelTraffic(conn.SelectedCharacter);
		}

		[PacketHandler(Op.CZ_SKILL_USE_HEIGHT)]
		public void CZ_SKILL_USE_HEIGHT(IZoneConnection conn, Packet packet)
		{
			var height = packet.GetFloat();

			var character = conn.SelectedCharacter;

			character.Variables.Temp.SetFloat("Skill.Use.Height", height);
		}

		/// <summary>
		/// Request to change the channel.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CHANGE_CHANNEL)]
		public void CZ_CHANGE_CHANNEL(IZoneConnection conn, Packet packet)
		{
			var channelId = packet.GetShort();

			conn.SelectedCharacter.WarpChannel(channelId);
		}

		/// <summary>
		/// Sent when attacking with sub-weapon.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_HARDCODED_ITEM)]
		public void CZ_HARDCODED_ITEM(IZoneConnection conn, Packet packet)
		{
			var s1 = packet.GetShort();
			var itemId = packet.GetLong();

			// Do something with this information? It sends the id of
			// the sub-weapon, so perhaps the client is telling us which
			// weapon to attack with, but it uses a different skill for
			// sub-weapon attacks, so we don't need this information. The
			// same packet also appears to be sent twice for some reason.
			// We'll just leave this empty for now.
		}

		/// <summary>
		/// Send as a notification for taking certain actions, such as preparing
		/// to teleport.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CLIENT_DIRECT)]
		public void CZ_CLIENT_DIRECT(IZoneConnection conn, Packet packet)
		{
			var type = packet.GetInt();
			var argStr = packet.GetString(16);

			var character = conn.SelectedCharacter;

			Send.ZC_CLIENT_DIRECT(character, type, argStr);
		}

		/// <summary>
		/// When warping from warp function
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CLIENT_DIRECT)]
		public void ZC_CLIENT_DIRECT(IZoneConnection conn, Packet packet)
		{
			var command = packet.GetByte();

			// ZC_SET_POS
			// ZC_RESET_VIEW
			// ZC_ENTER_PC
			// ZC_ADD_HP
			// ZC_UPDATE_SP
			// ZC_RESURRECT_SAVE_POINT_ACK
			var character = conn.SelectedCharacter;
			if (command == 1)
			{
				Send.ZC_RESET_VIEW(conn);
				Send.ZC_SET_POS(character);
				Send.ZC_ENTER_PC(conn, character);
				Send.ZC_NORMAL.Revive(character);
				character.Heal(1, 1);
				//character.Heal(HealType.Hp, character.MaxHp / 2);
				//character.Heal(HealType.Sp, character.Sp);
				Send.ZC_RESURRECT_SAVE_POINT_ACK(character);

			}
		}

		/// <summary>
		/// Client request to summon a companion
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SUMMON_PET)]
		public void CZ_SUMMON_PET(IZoneConnection conn, Packet packet)
		{
			var companionId = packet.GetLong();
			var petId = packet.GetInt();
			var i1 = packet.GetInt();

			if (companionId != 0)
			{
				var character = conn.SelectedCharacter;
				var companion = character.Companions.GetCompanion(companionId);

				if (companion != null && companion.Id == petId)
					companion.SetCompanionState(!companion.IsActivated);
			}
		}

		/// <summary>
		/// Client request to toggle companion auto-attack mode
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_PET_AUTO_ATK)]
		public void CZ_PET_AUTO_ATK(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;
			if (character == null)
				return;

			// Find the first active companion
			var companion = character.Companions.GetList().FirstOrDefault(c => c.IsActivated);
			if (companion == null)
				return;

			// Toggle aggressive mode
			companion.IsAggressiveMode = !companion.IsAggressiveMode;

			// Send response to client
			Send.ZC_PET_AUTO_ATK(character, companion);
		}

		[PacketHandler(Op.CZ_OBJECT_MOVE)]
		public void CZ_OBJECT_MOVE(IZoneConnection conn, Packet packet)
		{
			var handle = packet.GetInt();
			var attachToHandle = packet.GetInt();
			var packetString1 = packet.GetInt();
			var handleAttachedTo = packet.GetInt();
			var position = packet.GetPosition();

			var character = conn.SelectedCharacter;

			if (character.Map.TryGetActor(handle, out var actor) && actor is ISubActor subActor)
			{
				if (subActor.OwnerHandle != character.Handle)
					return;
				character.Position = position;
				subActor.Position = position;
				character.Movement.NotifyMove(position, character.Direction, 0);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CONTROL_OBJECT_ROTATE)]
		public void CZ_CONTROL_OBJECT_ROTATE(IZoneConnection conn, Packet packet)
		{
			var handle = packet.GetInt();
			var direction = packet.GetDirection();

			var character = conn.SelectedCharacter;

			if (character.Map.TryGetActor(handle, out var actor) && actor is ISubActor subActor)
			{
				if (subActor.OwnerHandle != character.Handle)
					return;
				subActor.Direction = direction;
				character.Direction = direction;
			}
		}

		/// <summary>
		/// Related to riding a snowball
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SUMMON_COMMAND)]
		public void CZ_SUMMON_COMMAND(IZoneConnection conn, Packet packet)
		{
			// TODO: Figure out what this is used for.
		}

		/// <summary>
		/// Ride pet request
		/// </summary>
		[PacketHandler(Op.CZ_VEHICLE_RIDE)]
		public void CZ_VEHICLE_RIDE(IZoneConnection conn, Packet packet)
		{
			var petHandle = packet.GetInt();
			var isRiding = packet.GetByte() == 1;

			var character = conn.SelectedCharacter;

			// Validate the companion
			var monster = character.Map.GetMonster(petHandle);
			if (monster is not Companion companion)
				return;

			// Verify ownership
			if (companion.Owner.ObjectId != character.ObjectId)
				return;

			// Check if companion is activated
			if (!companion.IsActivated)
				return;

			// Check if companion is a bird (hawks can't be ridden)
			if (companion.IsBird)
				return;

			// Check if companion is dead
			if (companion.IsDead)
				return;

			if (isRiding)
			{
				// Can't mount while sitting
				if (character.IsSitting)
					character.SetSitting(false);

				// Can't mount if already riding
				if (character.IsRiding)
					return;

				// Can't mount if attached to something else (e.g., HangingShot)
				if (character.Components.TryGet<AttachmentComponent>(out var attachment) && attachment.IsAttached)
					return;

				character.StartBuff(BuffId.RidingCompanion, TimeSpan.Zero, companion);
			}
			else
			{
				character.RemoveBuff(BuffId.RidingCompanion);
			}
		}

		/// <summary>
		/// Client requests to "Equip" an achievement title
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ACHIEVE_EQUIP)]
		public void CZ_ACHIEVE_EQUIP(IZoneConnection conn, Packet packet)
		{
			var achievementId = packet.GetInt();

			var character = conn.SelectedCharacter;

			// Validate and equip the title
			if (character.EquipTitle(achievementId))
			{
				Send.ZC_ACHIEVE_EQUIP(character, achievementId);
			}
			else
			{
				Log.Warning("Character {0} attempted to equip invalid/unlocked title: {1}", character.Name, achievementId);
			}
		}

		/// <summary>
		/// Client requests to continue a track (cutscene)
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_DIRECTION_PROCESS)]
		public async void CZ_DIRECTION_PROCESS(IZoneConnection conn, Packet packet)
		{
			var i1 = packet.GetInt();
			var handle = packet.GetInt();
			var frame = packet.GetInt();

			var character = conn.SelectedCharacter;

			if (character != null && character.Handle == handle && character.Tracks.ActiveTrack != null)
			{
				// Need to get all possible quest dialogs based on quest state
				var track = character.Tracks.ActiveTrack;
				await character.Tracks.Progress(track.Id, frame);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_DIRECTION_MOVE_STATE)]
		public void CZ_DIRECTION_MOVE_STATE(IZoneConnection conn, Packet packet)
		{
			var packetSize = packet.GetShort();
			int count = ((packet.Length - 24) / 28) - 2;

			var character = conn.SelectedCharacter;
			var track = character.Tracks.ActiveTrack;
			if (character != null && track != null)
			{
				// Need to get all possible quest dialogs based on quest state
				track.Frame = -2;
				Send.ZC_NORMAL.SetTrackFrame(character, track.Frame);
				if (count != track.Actors.Length)
				{
					Log.Warning("CZ_DIRECTION_MOVE_STATE: Count mismatch {0} != {1}", count, track.Actors.Length);
				}
				foreach (var entity in track.Actors)
				{
					var direction = packet.GetDirection();
					var position = packet.GetPosition();
					var f1 = packet.GetFloat();
					var f2 = packet.GetFloat();

					//if (entity.Direction != Direction.South && position != Position.Zero)
					//      entity.SetDirection(direction);
					if (position != Position.Zero && entity is Character character1)
					{
						character1.SetPosition(position);
						Send.ZC_SET_POS(character1);
					}
				}
				Send.ZC_NORMAL.SetupCutscene(character, false, false, true);

				// End the track now that the client cinematic is complete
				character.Tracks.End(track.Id);
				//character.Tracks.Progress(track.Id, frame);
			}
		}

		/// <summary>
		/// Check quest state for NPCs
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_QUEST_NPC_STATE_CHECK)]
		public void CZ_QUEST_NPC_STATE_CHECK(IZoneConnection conn, Packet packet)
		{
			var questId = packet.GetInt();

			var character = conn.SelectedCharacter;

			if (character != null)
			{
				//character.HasQuest(questId)
			}
		}

		/// <summary>
		/// Rank System Time Table ?
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_RANK_SYSTEM_TIME_TABLE)]
		public void CZ_REQUEST_RANK_SYSTEM_TIME_TABLE(IZoneConnection conn, Packet packet)
		{
			var i1 = packet.GetInt();

			Send.ZC_RESPONSE_RANK_SYSTEM_TIME_TABLE(conn);
		}

		/// <summary>
		/// Request Current Week # for Weekly Boss
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_WEEKLY_BOSS_NOW_WEEK_NUM)]
		public void CZ_REQUEST_WEEKLY_BOSS_NOW_WEEK_NUM(IZoneConnection conn, Packet packet)
		{
			Send.ZC_WEEKLY_BOSS_NOW_WEEK_NUM(conn, 1);
		}

		[PacketHandler(Op.CZ_REQUEST_WEEKLY_BOSS_ABSOLUTE_REWARD_LIST)]
		public void CZ_REQUEST_WEEKLY_BOSS_ABSOLUTE_REWARD_LIST(IZoneConnection conn, Packet packet)
		{
			var weekNum = packet.GetInt();

			// TODO: check date time by weekNum
			Send.ZC_WEEKLY_BOSS_ABSOLUTE_REWARD_LIST(conn);
		}

		[PacketHandler(Op.CZ_REQUEST_WEEKLY_BOSS_ENABLE_CLASS_RANKING_SEASON)]
		public void CZ_REQUEST_WEEKLY_BOSS_ENABLE_CLASS_RANKING_SEASON(IZoneConnection connection, Packet packet)
		{
			var weekNum = packet.GetInt();

			// TODO: Validate if this weekNum is Ranking Season or not?
			Send.ZC_REQUEST_WEEKLY_BOSS_ENABLE_CLASS_RANKING_SEASON(connection.SelectedCharacter, false);
		}

		[PacketHandler(Op.CZ_REQUEST_WEEKLY_BOSS_RANKING_REWARD_LIST)]
		public void CZ_REQUEST_WEEKLY_BOSS_RANKING_REWARD_LIST(IZoneConnection conn, Packet packet)
		{
			var weekNum = packet.GetInt();

			Send.ZC_WEEKLY_BOSS_RANKING_REWARD_LIST(conn);
		}

		[PacketHandler(Op.CZ_REQUEST_WEEKLY_BOSS_RANKING_INFO_LIST)]
		public void CZ_REQUEST_WEEKLY_BOSS_RANKING_INFO_LIST(IZoneConnection conn, Packet packet)
		{
			var weekNum = packet.GetInt();

			//Send.ZC_WEEKLY_BOSS_RANK_INFO(conn);
		}

		/// <summary>
		/// Request Current Week # for Guild Raid (Boruta)
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_BORUTA_NOW_WEEK_NUM)]
		public void CZ_REQUEST_BORUTA_NOW_WEEK_NUM(IZoneConnection conn, Packet packet)
		{
			Send.ZC_RESPONSE_BORUTA_NOW_WEEK_NUM(conn, 1);
		}

		/// <summary>
		/// Weekly Boss Start Time
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_WEEKLY_BOSS_START_TIME)]
		public void CZ_REQUEST_WEEKLY_BOSS_START_TIME(IZoneConnection conn, Packet packet)
		{
			var weekNum = packet.GetInt();

			//TODO check date time by weekNum
			Send.ZC_WEEKLY_BOSS_START_TIME(conn.SelectedCharacter);
		}

		/// <summary>
		/// Weekly Boss End Time
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_WEEKLY_BOSS_END_TIME)]
		public void CZ_REQUEST_WEEKLY_BOSS_END_TIME(IZoneConnection conn, Packet packet)
		{
			var weekNum = packet.GetInt();

			//TODO check date time by weekNum
			Send.ZC_WEEKLY_BOSS_END_TIME(conn.SelectedCharacter);
		}

		[PacketHandler(Op.CZ_REQUEST_WEEKLY_BOSS_PATTERN_INFO)]
		public void CZ_REQUEST_WEEKLY_BOSS_PATTERN_INFO(IZoneConnection conn, Packet packet)
		{
			var weekNum = packet.GetInt();

			//TODO check date time by weekNum
			var boss = ZoneServer.Instance.Data.MonsterDb.Entries.Values.Where(mob => mob.Rank == MonsterRank.Boss).Random();
			Send.ZC_WEEKLY_BOSS_PATTERN_INFO(conn.SelectedCharacter, boss);
		}

		/// <summary>
		/// A request to combine ancient cards
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ANCIENT_CARD_COMBINE)]
		public void CZ_ANCIENT_CARD_COMBINE(IZoneConnection conn, Packet packet)
		{
			var cardCount = packet.GetInt();
			var itemWorldIds = packet.GetList(cardCount, packet.GetLong);

			var character = conn.SelectedCharacter;

			//conn.Account.AssisterCabinet.TryGet();
		}

		/// <summary>
		/// A request to evolve an ancient card.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ANCIENT_CARD_EVOLVE)]
		public void CZ_ANCIENT_CARD_EVOLVE(IZoneConnection conn, Packet packet)
		{
			var cardCount = packet.GetInt();
			var itemWorldIds = packet.GetList(cardCount, packet.GetLong);

			var character = conn.SelectedCharacter;

			//conn.Account.AssisterCabinet.TryGet();
		}

		[PacketHandler(Op.CZ_CANCEL_PAIR_ANIMATION)]
		public void CZ_CANCEL_PAIR_ANIMATION(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;
			if (character.IsPaired)
			{
				character.IsPaired = false;
				Send.ZC_ENABLE_CONTROL(conn, "MOVE_PC", true);
				Send.ZC_PLAY_PAIR_ANIMATION(character, "None", false);
				Send.ZC_FLY(character);
			}
		}

		/// <summary>
		/// Requests the server to perform various commands upon a dummy PC.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_DUMMYPC_INFO)]
		public void CZ_REQ_DUMMYPC_INFO(IZoneConnection conn, Packet packet)
		{
			var handle = packet.GetInt();
			var command = packet.GetInt();

			if (!conn.SelectedCharacter.Map.TryGetCharacter(handle, out var targetCharacter))
			{
				Log.Warning("CZ_REQ_DUMMYPC_INFO: Failed to find character with handle {0}, for account {1}.", handle, conn.Account.Name);
				return;
			}

			switch (command)
			{
				case 4:
					Send.ZC_DIALOG_SELECT(conn, targetCharacter.Name + "*@* ", "@dicID_^*$ETC_20170313_027457$*^", "@dicID_^*$ETC_20150317_004134$*^", "@dicID_^*$ETC_20150317_004148$*^");
					break;
			}
		}

		/// <summary>
		/// Visit Barrack
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_VISIT_BARRACK)]
		public void CZ_VISIT_BARRACK(IZoneConnection conn, Packet packet)
		{
			var teamName = packet.GetString();
			var character = ZoneServer.Instance.World.GetCharacterByTeamName(teamName);

			if (character == null)
			{
				Log.Warning("CZ_VISIT_BARRACK: Failed to find character with team name {0}, for account {1}.", teamName, conn.Account.Name);
				return;
			}
			Send.ZC_SAVE_INFO(conn);
			Send.ZC_MOVE_BARRACK(conn);
		}

		/// <summary>
		/// Change hair dye
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CHANGE_HEAD)]
		public void CZ_CHANGE_HEAD(IZoneConnection conn, Packet packet)
		{
			var newHairColor = packet.GetString();
			var character = conn.SelectedCharacter;

			if (ZoneServer.Instance.Data.HairTypeDb.TryFind(character.Gender, character.Hair, out var currentHair))
			{
				var newHair = ZoneServer.Instance.Data.HairTypeDb.Find(a => a.ClassName == currentHair.ClassName && a.Color == newHairColor);
				if (newHair != null)
					character.ChangeHair(newHair.Index);
			}
		}

		/// <summary>
		/// Request player fight
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_FRIENDLY_FIGHT)]
		public void CZ_REQ_FRIENDLY_FIGHT(IZoneConnection conn, Packet packet)
		{
			var handle = packet.GetInt();
			var duelAccepted = packet.GetByte() == 1;

			var requester = conn.SelectedCharacter;
			if (requester == null)
			{
				Log.Warning("CZ_REQ_FRIENDLY_FIGHT: User '{0}'s character not found.", conn.Account.Name);
				return;
			}

			if (!requester.Map.TryGetCharacter(handle, out var receiver))
			{
				Log.Warning("CZ_REQ_FRIENDLY_FIGHT: User '{0}' tried to respond to a player with handle ({1}) that doesn't exist on the map.", conn.Account.Name, handle);
				return;
			}

			if (!duelAccepted)
			{
				ZoneServer.Instance.World.Duels.RequestDuel(requester, receiver);
			}
			else
			{
				if (requester.IsDueling || receiver.IsDueling)
					return;

				var duel = ZoneServer.Instance.World.Duels.CreateDuel(requester, receiver);
				ZoneServer.Instance.World.Duels.StartDuel(duel);
			}
		}

		/// <summary>
		/// Solo Dungeon Enter (DUMMY)
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SOLO_INDUN_ENTER)]
		public void CZ_SOLO_INDUN_ENTER(IZoneConnection conn, Packet packet)
		{
			var dungeonId = packet.GetInt(); // 202
			var i1 = packet.GetInt(); // 1
			var i2 = packet.GetInt(); // 0

			var character = conn.SelectedCharacter;

			// Fail if player is dead
			if (character.IsDead)
			{
				character.SystemMessage("CannotInCurrentState");
				return;
			}

			var dungeon = ZoneServer.Instance.Data.InstanceDungeonDb.Find(dungeonId);
			if (dungeon == null)
			{
				Log.Warning("CZ_SOLO_INDUN_ENTER: User '{0}' tried to enter dungeon with id ({1}) that doesn't exist.", conn.Account.Name, dungeonId);
				return;
			}

			// TODO Check Entry Qualifications
		}

		[PacketHandler(Op.CZ_QUEST_CHECK_SAVE)]
		public void CZ_QUEST_CHECK_SAVE(IZoneConnection conn, Packet packet)
		{
			var questId = packet.GetInt();
			//var quest = ZoneServer.Instance.Data.QuestDb.Find(questId);

			//if (quest == null)
			{
				//Log.Warning("CZ_QUEST_CHECK_SAVE: User '{0}' tried to save non-existent quest with Id: {1}.", conn.Account.Name, questId);
			}
			// TODO Do something with this?
		}

		/// <summary>
		/// Request Used Medal Total
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_USED_MEDAL_TOTAL)]
		public void CZ_REQUEST_USED_MEDAL_TOTAL(IZoneConnection conn, Packet packet)
		{
			var medals = conn.Account.Medals;

			Send.ZC_NORMAL.UsedMedalTotal(conn, medals);
		}

		/// <summary>
		/// Sent when offering to Goddess Grace Event
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_BID_FIELDBOSS_WORLD_EVENT)]
		public void CZ_BID_FIELDBOSS_WORLD_EVENT(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;
			var fixedCost = 50000;
			if (character.Inventory.CountItem(ItemId.Silver) < fixedCost)
			{
				Send.ZC_SYSTEM_MSG(character, 10236);
				return;
			}
			// Pick a random item based on weights?
			/**
			var possibleItems = new List<WeightedListItem<string>>
			{
				new WeightedListItem<string>("VakarineCertificateCoin_100p", 10),
				new WeightedListItem<string>("VakarineCertificateCoin_1000p", 1),
			};
			var weightedItems = new WeightedList<string>(possibleItems);
			var itemClassName = weightedItems.Next();
			var itemData = ZoneServer.Instance.Data.ItemDb.FindByClass(itemClassName);
			if (itemData != null)
			{
				Send.ZC_NORMAL.SteamAchievement(character, SteamAchievement.TOS_STEAM_ACHIEVEMENT_GOD_PROTECTION);
				character.RemoveItem(ItemId.Silver, fixedCost);
				character.AddItem(itemData.Id);
				Send.ZC_LOSE_FIELDBOSS_WORLD_EVENT_ITEM(character, itemData.Id);
			}
			**/
		}

		[PacketHandler(Op.CZ_REQ_SOLO_DUNGEON_REWARD)]
		public void CZ_REQ_SOLO_DUNGEON_REWARD(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;

			// If no rewards send message
			Send.ZC_SYSTEM_MSG(character, 513007);
		}

		/// <summary>
		/// Request to arrange furniture (place furniture)
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_HOUSING_REQUEST_ARRANGEMENT_FURNITURE)]
		public void CZ_HOUSING_REQUEST_ARRANGEMENT_FURNITURE(IZoneConnection conn, Packet packet)
		{
			// No-op: Houses not available (PersonalHouse type deleted during Laima merge)
		}

		/// <summary>
		/// Skills which require the user to select a cell list.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_SKILL_CELL_LIST)]
		public void CZ_SKILL_CELL_LIST(IZoneConnection conn, Packet packet)
		{
			var s1 = packet.GetShort(); // 64
			var castPosition = packet.GetPosition();
			var castDirection = packet.GetDirection();
			var cellCount = packet.GetInt();

			var character = conn.SelectedCharacter;

			if (character == null)
			{
				Log.Warning("CZ_SKILL_CELL_LIST: Account '{0}' tried to use a non-existing character.", conn.Account.Name);
				return;
			}

			var skill = character.Variables.Temp.Get<Skill>("Melia.Cast.Skill");
			if (skill == null)
			{
				Log.Warning("CZ_SKILL_CELL_LIST: Account '{0}' tried to use skill cell list without a skill.", conn.Account.Name);
				return;
			}

			skill.Vars.Set("Melia.Skill.CellCastPosition", castPosition);
			skill.Vars.Set("Melia.Skill.CellCastDirection", castDirection);
			skill.Vars.SetInt("Melia.Skill.CellCount", cellCount);
			var cells = new List<SkillCellPosition>();
			for (var index = 0; index < cellCount; ++index)
			{
				var cellZ = packet.GetInt();
				var cellX = packet.GetInt();
				cells.Add(new SkillCellPosition(cellX, cellZ));
			}
			skill.Vars.Set("Melia.Skill.CellList", cells);
		}

		/// <summary>
		/// Request to start or stop playing the flute while resting.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_READY_FLUTING)]
		public void CZ_REQ_READY_FLUTING(IZoneConnection conn, Packet packet)
		{
			var enabled = packet.GetBool();

			var character = conn.SelectedCharacter;

			if (!character.Jobs.Has(JobId.PiedPiper))
			{
				Log.Warning("CZ_REQ_READY_FLUTING: User '{0}' tried to play the flute without being a Pied Piper.", conn.Account.Name);
				return;
			}

			if (!character.IsSitting)
			{
				Log.Warning("CZ_REQ_READY_FLUTING: User '{0}' tried to play the flute while not sitting.", conn.Account.Name);
				return;
			}

			Send.ZC_READY_FLUTING(character, enabled);
		}

		/// <summary>
		/// Request to play a note on the flute while resting.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_PLAY_FLUTING)]
		public void CZ_REQ_PLAY_FLUTING(IZoneConnection conn, Packet packet)
		{
			var note = packet.GetInt();
			var octave = packet.GetInt();
			var semitone = packet.GetBool();

			var character = conn.SelectedCharacter;

			if (!character.Jobs.Has(JobId.PiedPiper))
			{
				Log.Warning("CZ_REQ_READY_FLUTING: User '{0}' tried to play the flute without being a Pied Piper.", conn.Account.Name);
				return;
			}

			if (!character.IsSitting)
			{
				Log.Warning("CZ_REQ_READY_FLUTING: User '{0}' tried to play the flute while not sitting.", conn.Account.Name);
				return;
			}

			Send.ZC_PLAY_FLUTING(character, note, octave, semitone, true);
		}

		/// <summary>
		/// Request to stop playing a note on the flute while resting.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_STOP_FLUTING)]
		public void CZ_REQ_STOP_FLUTING(IZoneConnection conn, Packet packet)
		{
			var note = packet.GetInt();
			var octave = packet.GetInt();
			var semitone = packet.GetBool();

			var character = conn.SelectedCharacter;

			if (!character.Jobs.Has(JobId.PiedPiper))
			{
				Log.Warning("CZ_REQ_READY_FLUTING: User '{0}' tried to play the flute without being a Pied Piper.", conn.Account.Name);
				return;
			}

			if (!character.IsSitting)
			{
				Log.Warning("CZ_REQ_READY_FLUTING: User '{0}' tried to play the flute while not sitting.", conn.Account.Name);
				return;
			}

			// If the user starts playing a note, but doesn't stop
			// playing it, or they send a different note to stop,
			// the note will keep playing for a moment until stopping
			// on its own. We could catch this by saving the notes on
			// start, but since you can play multiple notes at once,
			// that will require a bit more effort than simply setting
			// a couple variables which we then get here. We'd need
			// to keep track of all notes being played, stop specific
			// ones, and stop all if anything goes wrong.

			Send.ZC_STOP_FLUTING(character, note, octave, semitone);
		}

		/// <summary>
		/// Request to select class representation
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CHANGE_REPRESENTATION_CLASS)]
		public void CZ_CHANGE_REPRESENTATION_CLASS(IZoneConnection conn, Packet packet)
		{
			var jobId = (JobId)packet.GetInt();
			var character = conn.SelectedCharacter;

			if (!character.Jobs.Has(jobId))
			{
				Log.Warning("CZ_CHANGE_REPRESENTATION_CLASS: User '{0}' tried to select a class they don't have {1}.", conn.Account.Name, jobId.ToString());
				return;
			}
			character.JobId = jobId;
			character.AddonMessage(AddonMessage.UPDATE_REPRESENTATION_CLASS_ICON, "None", (int)jobId);
		}

		/// <summary>
		/// Packet with unknown purpose that is spammed by the client
		/// while the player character is dead.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_InteractionCancel)]
		public void CZ_InteractionCancel(IZoneConnection conn, Packet packet)
		{
			if (conn == null)
				return;

			// The packet is spammed with a frequency of about 1-2 packets
			// per millisecond. It's 64 bytes long, with the last 5 looking
			// like random garbage data, though the packet doesn't seem to
			// contain any useful information in general. Its name seems
			// to be "CZ_InteractionCancel", though it doesn't appear in
			// the op code list.
			var character = conn.SelectedCharacter;

			if (character == null)
				return;

			character.ShowHelp("TUTO_RESURRECTION");
		}

		/// <summary>
		/// Request from a player to revive their character.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_RESURRECT)]
		public void CZ_RESURRECT(IZoneConnection conn, Packet packet)
		{
			var optionIdx = packet.GetByte();
			var l1 = 0L;
			if (Versions.Protocol > 500)
				l1 = packet.GetLong();

			var character = conn.SelectedCharacter;
			var option = (ResurrectOptions)(1 << optionIdx);

			if (!character.IsDead)
			{
				Log.Warning("CZ_RESURRECT: User '{0}' tried to revive their character while not dead.", conn.Account.Name);
				return;
			}

			if (character.Map.IsPVP && !Feature.IsEnabled("AllowPVPResurrection"))
			{
				Log.Warning("CZ_RESURRECT: User '{0}' tried to revive their character in a PVP map.", conn.Account.Name);
				return;
			}

			character.Resurrect(option);
		}

		/// <summary>
		/// Request to apply a certain HUD skin.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_APPLY_HUD_SKIN)]
		public void CZ_REQ_APPLY_HUD_SKIN(IZoneConnection conn, Packet packet)
		{
			var skinId = packet.GetInt();

			var character = conn.SelectedCharacter;

			if (skinId < 0 || skinId > 5)
			{
				Log.Warning("CZ_REQ_APPLY_HUD_SKIN: User '{0}' tried to apply an invalid skin id {1}.", conn.Account.Name, skinId);
				return;
			}

			// TODO: Check if player owns the HUD skin?
			character.Variables.Perm.SetInt("Melia.HudSkin", skinId);

			Send.ZC_SEND_APPLY_HUD_SKIN_MYSELF(conn, character);
			if (conn.Party != null)
				Send.ZC_SEND_APPLY_HUD_SKIN_PARTY(conn, character, conn.Party);
			Send.ZC_SEND_APPLY_HUD_SKIN_OTHER(conn, character);
		}

		/// <summary>
		/// Request the current HUD skin.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_CURRENT_HUD_SKIN)]
		public void CZ_REQ_CURRENT_HUD_SKIN(IZoneConnection conn, Packet packet)
		{
			// TODO: Don't know if this is used for anything else.
		}

		/// <summary>
		/// Request to refresh quick slot data from the server.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_QUICKSLOT_REFRESH)]
		public void CZ_REQ_QUICKSLOT_REFRESH(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;
			Send.ZC_QUICK_SLOT_LIST(character);
		}

		/// <summary>
		/// Request to cancel/remove a buff.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_BUFF_REMOVE)]
		public void CZ_BUFF_REMOVE(IZoneConnection conn, Packet packet)
		{
			var buffId = (BuffId)packet.GetInt();

			var character = conn.SelectedCharacter;

			if (!character.TryGetBuff(buffId, out var buff))
			{
				Log.Warning("CZ_BUFF_REMOVE: User '{0}' tried to remove a buff they don't have ({1}).", conn.Account.Name, buffId);
				return;
			}

			if (!buff.Data.Removable)
			{
				Log.Warning("CZ_BUFF_REMOVE: User '{0}' tried to remove a buff that can't be removed ({1}).", conn.Account.Name, buffId);
				return;
			}

			character.StopBuff(buffId);
		}

		/// <summary>
		/// Request to open dungeon entrance UI.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_LEVEL_DUNGEON_ENTER)]
		public void CZ_REQ_LEVEL_DUNGEON_ENTER(IZoneConnection conn, Packet packet)
		{
			var instanceDungeonId = packet.GetInt();

			// Get data
			if (!ZoneServer.Instance.Data.InstanceDungeonDb.TryFind(instanceDungeonId, out var data))
			{
				Log.Warning("CZ_REQ_LEVEL_DUNGEON_ENTER: User '{0}' sent an unknown dungeon id ({1}).", conn.Account.Name, instanceDungeonId);
				return;
			}

			var character = conn.SelectedCharacter;

			// Fail if player is already inside a dungeon
			if (ZoneServer.Instance.Data.InstanceDungeonDb.TryGetByMapClassName(character.Map.ClassName, out _))
			{
				character.SystemMessage("CannotInCurrentState");
				return;
			}

			// Check if player has an active dungeon instance
			var currentDungeon = DungeonScript.GetInstance(character.DbId);
			if (currentDungeon != null)
			{
				// Warp player back to their active dungeon
				character.Warp(currentDungeon.MapId, currentDungeon.StartPosition);
				return;
			}

			character.Variables.Perm.SetInt(AutoMatchZoneManager.DungeonIdVarName, instanceDungeonId);
			Send.ZC_NORMAL.InstanceDungeonMatchMaking(character, instanceDungeonId, 0, 1, 1, 1);
		}


		[PacketHandler(Op.CZ_REQ_CancelGachaCube)]
		public void CZ_REQ_CancelGachaCube(IZoneConnection conn, Packet packet)
		{
			// Tracking Gacha Cube Opening?
		}
	}
}
