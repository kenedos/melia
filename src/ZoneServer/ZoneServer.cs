using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using Melia.Shared;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.IES;
using Melia.Shared.L10N;
using Melia.Shared.Network;
using Melia.Shared.Network.Inter.Messages;
using Melia.Zone.Abilities;
using Melia.Zone.Buffs;
using Melia.Zone.Commands;
using Melia.Zone.Data;
using Melia.Zone.Database;
using Melia.Zone.Events;
using Melia.Zone.Network;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Shared;
using Melia.Zone.Services;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Spawning;
using Melia.Zone.Util;
using Melia.Zone.World;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Network.Communication;
using Yggdrasil.Network.TCP;
using Yggdrasil.Util;
using Log = Yggdrasil.Logging.Log;

namespace Melia.Zone
{
	/// <summary>
	/// Represents a zone server.
	/// </summary>
	public class ZoneServer : Server
	{
		public readonly static ZoneServer Instance = new();

		private TcpConnectionAcceptor<ZoneConnection> _acceptor;
		private AutoSaveService _autoSaveService;
		private OrphanCleanupService _orphanCleanupService;

		public override ServerType Type => ServerType.Zone;

		/// <summary>
		/// Returns a reference to the server's packet handlers.
		/// </summary>
		public PacketHandler PacketHandler { get; } = new();

		/// <summary>
		/// Returns reference to the server's database interface.
		/// </summary>
		public ZoneDb Database { get; } = new();

		/// <summary>
		/// Returns reference to the server's world manager.
		/// </summary>
		public WorldManager World { get; } = new();

		/// <summary>
		/// Returns reference to the server's skill handlers.
		/// </summary>
		public SkillHandlers SkillHandlers { get; } = new();

		/// <summary>
		/// Returns reference to the server's buff handlers.
		/// </summary>
		public BuffHandlers BuffHandlers { get; } = new();

		/// <summary>
		/// Returns reference to the server's ability handlers.
		/// </summary>
		public AbilityHandlers AbilityHandlers { get; } = new();

		/// <summary>
		/// Returns reference to the server's item handlers.
		/// </summary>
		//public ItemHandlers ItemHandlers { get; } = new();

		/// <summary>
		/// Returns reference to the server's pad handlers.
		/// </summary>
		public PadHandlers PadHandlers { get; } = new();

		/// <summary>
		/// Returns reference to the server's chat command manager.
		/// </summary>
		public ChatCommands ChatCommands { get; } = new();

		/// <summary>
		/// Returns a reference to the server's event manager.
		/// </summary>
		public ServerEvents ServerEvents { get; } = new();

		/// <summary>
		/// Returns a reference to the server's game event manager.
		/// </summary>
		public GameEventManager GameEvents { get; } = new GameEventManager();

		/// <summary>
		/// Manager for achievements and point tracking.
		/// </summary>
		public AchievementService Achievements { get; } = new AchievementService();

		/// <summary>
		/// Manager for periodic dungeon reset tasks.
		/// </summary>
		public DungeonResetService DungeonReset { get; } = new DungeonResetService();

		/// <summary>
		/// Returns the dialog function handlers.
		/// </summary>
		public DialogFunctions DialogFunctions { get; } = new DialogFunctions();

		/// <summary>
		/// Returns the trigger function handlers.
		/// </summary>
		public TriggerFunctions TriggerFunctions { get; } = new TriggerFunctions();

		/// <summary>
		/// Returns reference to the server's IES mods.
		/// </summary>
		public IesModList IesMods { get; } = new();

		public Stopwatch ServerTime { get; } = new Stopwatch();


		/// <summary>
		/// Runs the server.
		/// </summary>
		/// <param name="args"></param>
		public override void Run(string[] args)
		{
			this.GetServerId(args, out var groupId, out var serverId);
			var title = string.Format("Zone ({0}, {1})", groupId, serverId);

			ConsoleUtil.WriteHeader(ConsoleHeader.ProjectName, title, ConsoleColor.DarkGreen, ConsoleHeader.Logo, ConsoleHeader.Credits);
			ConsoleUtil.LoadingTitle();

			// Set up zone server specific logging or we might run into
			// issues with multiple servers trying to write files at the
			// same time.
			Log.Init($"ZoneServer_{groupId}_{serverId}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}");

			this.NavigateToRoot();

			this.LoadConf();
			this.LoadPackages();
			this.LoadVersionInfo();
			if (this.Data.OpDb != null)
				this.PacketHandler.LoadMethods();
			this.LoadLocalization(this.Conf);
			this.LoadData(this.Type);
			this.LoadServerList(this.Data.ServerDb, this.Type, groupId, serverId);
			this.InitDatabase(this.Database, this.Conf);
			this.InitSkills();
			this.InitWorld();
			this.LoadDialogFunctions();
			this.LoadTriggerFunctions();
			this.LoadScripts("zone");
			this.LoadIesMods();
			this.StartWorld();

			SaveQueue.Start();
			this.StartAutoSaveService();
			this.StartOrphanCleanupService();
			if (this.Conf.World.EnableProceduralQuests)
			{
				NpcSpawnManager.LoadSpawnData();
				ResourceNodeSpawnManager.LoadAllNodeData();
				PoiDataManager.LoadPoiData();
			}

			this.StartCommunicator();
			this.StartAcceptor();

			ConsoleUtil.RunningTitle();
			new ZoneConsoleCommands().Wait();
		}

		/// <summary>
		/// Starts accepting connections.
		/// </summary>
		private void StartAcceptor()
		{
			_acceptor = new TcpConnectionAcceptor<ZoneConnection>(this.ServerInfo.Port);
			_acceptor.ConnectionChecker = (conn) => this.CheckConnection(conn, this.Database);
			_acceptor.ConnectionAccepted += this.OnConnectionAccepted;
			_acceptor.ConnectionRejected += this.OnConnectionRejected;
			_acceptor.Listen();

			Log.Status("Server ready, listening on {0}.", _acceptor.Address);
		}

		/// <summary>
		/// Starts the communicator and attempts to connect to the
		/// coordinator.
		/// </summary>
		private void StartCommunicator()
		{
			Log.Info("Attempting to connect to coordinator...");

			var commName = $"{this.ServerInfo.Type}:{this.ServerList.GroupData.Id}:{this.ServerInfo.Id}";

			this.Communicator = new Communicator(commName);
			this.Communicator.Disconnected += this.Communicator_OnDisconnected;
			this.Communicator.MessageReceived += this.Communicator_OnMessageReceived;

			this.ConnectToCoordinator();
		}

		/// <summary>
		/// Attempts to establish a connection to the coordinator.
		/// </summary>
		private void ConnectToCoordinator()
		{
			var barracksServerInfo = this.GetServerInfo(ServerType.Barracks, 1);
			var authentication = this.Conf.Inter.Authentication;

			try
			{
				this.Communicator.Connect("Coordinator", authentication, barracksServerInfo.InterIp, barracksServerInfo.InterPort);

				this.Communicator.Subscribe("Coordinator", "ServerUpdates");
				this.Communicator.Subscribe("Coordinator", "AllServers");
				this.Communicator.Subscribe("Coordinator", "AllZones");

				this.ServerInfo.Status = ServerStatus.Online;
				this.UpdateServerInfo();

				Log.Info("Successfully connected to coordinator.");
			}
			catch (Exception ex)
			{
				Log.Error("Failed to connect to coordinator, trying again in 5 seconds...");
				Log.Error(ex.Message);
				Thread.Sleep(5000);

				this.ConnectToCoordinator();
			}
		}

		/// <summary>
		/// Called when the connection to the coordinator was lost.
		/// </summary>
		/// <param name="commName"></param>
		private void Communicator_OnDisconnected(string commName)
		{
			Log.Error("Lost connection to coordinator, will try to reconnect in 5 seconds...");
			Thread.Sleep(5000);

			this.ConnectToCoordinator();
		}

		/// <summary>
		/// Called when a message was received from the coordinator.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="message"></param>
		private void Communicator_OnMessageReceived(string sender, ICommMessage message)
		{
			//Log.Debug("Message received from '{0}': {1}", sender, message);

			switch (message)
			{
				case ShutdownMessage shutdownMessage:
				{
					// Delegate to the ServerShutdownManager for graceful shutdown handling
					// This supports immediate, graceful (with countdown), and cancel operations
					ServerShutdownManager.Instance.HandleShutdownMessage(shutdownMessage);
					break;
				}
				case ServerUpdateMessage serverUpdateMessage:
				{
					this.ServerList.Update(serverUpdateMessage);
					break;
				}
				case NoticeTextMessage noticeTextMessage:
				{
					Send.ZC_TEXT(noticeTextMessage.Type, noticeTextMessage.Text);
					break;
				}
				case KickMessage kickMessage:
				{
					IEnumerable<Character> characters;

					if (kickMessage.TargetType == KickTargetType.Player)
					{
						var targetCharacter = this.World.GetCharacterByTeamName(kickMessage.TargetName);
						if (targetCharacter == null)
							break;

						characters = [targetCharacter];
					}
					else if (kickMessage.TargetType == KickTargetType.Map)
					{
						if (!this.World.TryGetMap(kickMessage.TargetName, out var map))
							break;

						characters = map.GetCharacters();
					}
					else
					{
						throw new InvalidDataException($"Invalid kick target type '{kickMessage.TargetType}'.");
					}

					foreach (var character in characters)
					{
						character.MsgBox(Localization.Get("You were kicked by {0}."), kickMessage.OriginName);
						character.Connection.Close(this.Conf.World.ConnectionCloseDelay);
					}
					break;
				}
				case ForceLogOutMessage logoutMessage:
				{
					var character = this.World.GetCharacter(c => c.Connection?.Account?.Id == logoutMessage.AccountId && !c.IsAutoTrading);
					character?.Connection?.Close();
					break;
				}
				case GuildUpdateMessage guildUpdateMessage:
				{
					Log.Debug("Received guild update from '{0}': Type={1}, GuildId={2}, CharacterId={3}",
						sender, guildUpdateMessage.UpdateType, guildUpdateMessage.GuildId, guildUpdateMessage.CharacterId);
					break;
				}
				case MarketUpdateMessage marketUpdateMessage:
				{
					// Zone Server ignores market updates
					break;
				}
				case ForceLogOutEveryoneMessage logoutMessage:
				{
					var characters = this.World.GetCharacters();
					foreach (var character in characters)
					{
						character.MsgBox(Localization.Get("Logging Out: {0}."), logoutMessage.Reason);
						character.Connection?.Close(this.Conf.World.ConnectionCloseDelay);
					}
					break;
				}
				case InitAutoMatchContentMessage initAutoMatchContentMessage:
				{
					this.HandleInitAutoMatchContent(initAutoMatchContentMessage);
					break;
				}
				case AutoMatchMembersUpdateMessage autoMatchMembersUpdateMessage:
				{
					this.World.AutoMatch.HandleMembersUpdate(autoMatchMembersUpdateMessage);
					break;
				}
			}
		}

		/// <summary>
		/// Handles the auto match content initialization message from SocialServer.
		/// Creates a party for matched players and warps them to the dungeon.
		/// </summary>
		private void HandleInitAutoMatchContent(InitAutoMatchContentMessage message)
		{
			// Find all characters on this zone server that are part of the match
			var matchedCharacters = new List<Character>();
			foreach (var characterDbId in message.CharacterDbIds)
			{
				var character = this.World.GetCharacter(c => c.DbId == characterDbId);
				if (character != null)
					matchedCharacters.Add(character);
			}

			if (matchedCharacters.Count == 0)
			{
				Log.Debug("InitAutoMatchContentMessage: No matched characters found on this zone server.");
				return;
			}

			// Remove all matched characters from their existing parties
			foreach (var character in matchedCharacters)
			{
				if (character.HasParty)
				{
					var existingParty = character.Connection.Party;
					existingParty.RemoveMember(character);
				}
			}

			// Use the first matched character as the leader
			var leader = matchedCharacters[0];

			// Create a dungeon session with the AutoMatchZoneManager
			var session = this.World.AutoMatch.CreateDungeonSession(leader, message.AutoMatchId, message.DungeonId);

			// Add remaining characters to the session/party
			for (var i = 1; i < matchedCharacters.Count; i++)
			{
				session.AddMember(matchedCharacters[i]);
			}

			// Store player count and clear any stale instance references on all characters
			foreach (var character in matchedCharacters)
			{
				character.Variables.Temp.SetInt(AutoMatchZoneManager.PlayersCountVarName, message.CharacterDbIds.Count);
				character.Variables.Perm.SetString(DungeonScript.ActiveInstanceVarName, null);
			}

			Log.Info("InitAutoMatchContentMessage: Created party for {0} players for dungeon id '{1}' (AutoMatchId: {2}).",
				matchedCharacters.Count, message.DungeonId, message.AutoMatchId);

			// Warp all players to the dungeon using the centralized dungeon warp method
			DungeonScript.WarpPartyToDungeon(matchedCharacters, message.DungeonId);
		}

		/// <summary>
		/// Sends an update about the server's status to the coordinator.
		/// </summary>
		public void UpdateServerInfo()
		{
			var playerCount = this.World.GetCharacterCount();

			var rates = new ServerRates
			{
				ExpRate = this.Conf.World.ExpRate,
				JobExpRate = this.Conf.World.ExpRate,
				DropRate = this.Conf.World.GeneralDropRate,
				EquipRate = this.Conf.World.EquipmentDropRate,
				GemRate = this.Conf.World.GemDropRate,
				RecipeRate = this.Conf.World.RecipeDropRate,
			};

			//var message = new ServerUpdateMessage(this.Type, serverId, playerCount, ServerStatus.Online, rates);
			//zoneServer.Communicator.Send("Coordinator", message);
			base.UpdateServerInfo(this.ServerInfo.Status, playerCount, rates);
		}

		/// <summary>
		/// Loads skill handlers.
		/// </summary>
		private void InitSkills()
		{
			Log.Info("Initializing handlers...");
			this.SkillHandlers.Init(this.Packages);
			this.BuffHandlers.Init(this.Packages);
			this.PadHandlers.Init(this.Packages);
			this.AbilityHandlers.Init(this.Packages);
		}

		/// <summary>
		/// Loads maps and initializes them.
		/// </summary>
		private void InitWorld()
		{
			using (Debug.Profile($"Initializing World", 5000))
			{
				Log.Info("Initializing world...");
				this.World.Initialize();
			}
			using (Debug.Profile($"Initializing Game Events", 500))
			{
				Log.Info("Initializing game events...");
				this.GameEvents.Initialize();
			}
			using (Debug.Profile($"Initializing Dungeon Reset Service", 100))
			{
				Log.Info("Initializing dungeon reset service...");
				this.DungeonReset.Initialize();
			}
			using (Debug.Profile($"Initializing Achievement Service", 100))
			{
				Log.Info("Initializing achievement service...");
				this.Achievements.Initialize();
			}
			Log.Info("  done loading {0} maps.", this.World.Count);
		}

		/// <summary>
		/// Starts the world's update loop, aka the hearbeat.
		/// </summary>
		private void StartWorld()
		{
			this.ServerTime.Start();
			Log.Info("Starting world update...");
			this.World.Start();
		}

		/// <summary>
		/// Sets up IES mods.
		/// </summary>
		private void LoadIesMods()
		{
			// This method is temporary until we have a more proper way
			// way of handling IES mods.

			// Centurion was apparently disabled during the beta phase
			// in 2015 and replaced with Fencer, and while it was supposed
			// get added back in on a higher rank, that never happened (?).
			// To enable it, we need to adjust the job rank to make it
			// selectable and give the skills a max level.
			if (!Feature.IsEnabled(FeatureId.CenturionRemoved))
			{
				this.IesMods.Add("Job", 1005, "Rank", 2);
				this.IesMods.Add("SkillTree", 10502, "MaxLevel", 5);
				this.IesMods.Add("SkillTree", 10503, "MaxLevel", 5);
				this.IesMods.Add("SkillTree", 10504, "MaxLevel", 5);
				this.IesMods.Add("SkillTree", 10505, "MaxLevel", 5);
				this.IesMods.Add("SkillTree", 10506, "MaxLevel", 5);
				this.IesMods.Add("SkillTree", 10507, "MaxLevel", 5);
				this.IesMods.Add("SkillTree", 10508, "MaxLevel", 5);
				this.IesMods.Add("SkillTree", 10509, "MaxLevel", 5);
			}

			this.IesMods.Add("SharedConst", 177, "Value", this.Conf.World.StorageFee); // WAREHOUSE_PRICE
			this.IesMods.Add("SharedConst", 10004, "Value", this.Conf.World.StorageExtCost); // WAREHOUSE_EXTEND_PRICE
			this.IesMods.Add("SharedConst", 10006, "Value", this.Conf.World.StorageExtCount); // WAREHOUSE_EXTEND_SLOT_COUNT
			this.IesMods.Add("SharedConst", 10010, "Value", this.Conf.World.StorageMaxExtensions); // WAREHOUSE_MAX_COUNT
			this.IesMods.Add("SharedConst", 10012, "Value", this.Conf.World.TeamStorageExtCost); // ACCOUNT_WAREHOUSE_EXTEND_PRICE
			this.IesMods.Add("SharedConst", 10013, "Value", this.Conf.World.TeamStorageMaxSilverExpands); // ACCOUNT_WAREHOUSE_MAX_EXTEND_COUNT
			this.IesMods.Add("SharedConst", 10021, "Value", this.Conf.World.TeamStorageMinimumLevelRequired); // ACCOUNT_WAREHOUSE_LIMIT_LEVEL

			this.IesMods.Add("SharedConst", 102, "Value", this.Conf.World.MaxLevel);
			this.IesMods.Add("SharedConst", 103, "Value", this.Conf.World.MaxCompanionLevel);
			this.IesMods.Add("SharedConst", 104, "Value", this.Conf.World.MaxBaseJobLevel);
			this.IesMods.Add("SharedConst", 105, "Value", this.Conf.World.MaxAdvanceJobLevel);
			this.IesMods.Add("SharedConst", 100050, "Value", this.Conf.World.JobMaxRank); // JOB_CHANGE_MAX_RANK

			// Magical Amulets are invisible by default, this makes them visible.
			if (Feature.IsEnabled("MagicalAmulet"))
			{
				for (var i = 0; i < 26; i++)
					this.IesMods.Add("Item", 648001 + i, "MarketCategory", "Misc_Usual");
			}

			if (Feature.IsEnabled("UnlockAllCompanions"))
			{
				foreach (var companion in Instance.Data.CompanionDb.Entries.Values)
				{
					if (companion.Id == 1 || !Instance.Data.MonsterDb.TryFind(companion.ClassName, out _))
						continue;
					this.IesMods.Add("Companion", companion.Id, "SellPrice", "SCR_GET_VELHIDER_PRICE");
					this.IesMods.Add("Companion", companion.Id, "ShopGroup", "Normal");
				}
			}

			//foreach (var item in this.Data.ItemDb.Entries.Values)
			//	this.IesMods.Add("Item", item.Id, "UserTrade", "YES");
		}

		/// <summary>
		/// Sets up Dialog Functions.
		/// </summary>
		private void LoadDialogFunctions()
		{
			Log.Info("Loading dialog functions...");

			try
			{
				this.DialogFunctions.LoadMethods();
				Log.Info("  loaded {0} dialog functions.", this.DialogFunctions.Count);
			}
			catch (Exception ex)
			{
				Log.Error("Failed to load dialog functions: {0}", ex);
				ConsoleUtil.Exit(1);
			}
		}

		/// <summary>
		/// Sets up Trigger Functions.
		/// </summary>
		private void LoadTriggerFunctions()
		{
			Log.Info("Loading trigger functions...");

			try
			{
				this.TriggerFunctions.LoadMethods();
				Log.Info("  loaded {0} trigger functions.", this.TriggerFunctions.Count);
			}
			catch (Exception ex)
			{
				Log.Error("Failed to load dialog functions: {0}", ex);
				ConsoleUtil.Exit(1);
			}
		}

		/// <summary>
		/// Called when a new connection is accepted.
		/// </summary>
		/// <param name="conn"></param>
		private void OnConnectionAccepted(ZoneConnection conn)
		{
			Log.Info("New connection accepted from '{0}'.", conn.Address);
		}

		/// <summary>
		/// Called when a new connection was rejected.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="reason"></param>
		private void OnConnectionRejected(ZoneConnection conn, string reason)
		{
			Log.Info("Connection rejected from '{0}'.", conn.Address);
		}

		private void StartAutoSaveService()
		{
			try
			{
				var autoSaveSlots = this.Conf.World.AutoSaveSlots;
				var autoSaveIntervalPerSlot = TimeSpan.FromMinutes(this.Conf.World.AutoSaveIntervalMinutes);
				var orphanCleanupCycles = this.Conf.World.OrphanCleanupEnabled ? this.Conf.World.OrphanCleanupCycles : 0;

				_autoSaveService = new AutoSaveService(this, this.Database, autoSaveSlots, autoSaveIntervalPerSlot, orphanCleanupCycles);
			}
			catch (Exception ex)
			{
				Log.Error("Failed to initialize AutoSave Service: {0}", ex);
			}
		}

		private void StartOrphanCleanupService()
		{
			if (!this.Conf.World.OrphanCleanupEnabled)
			{
				Log.Info("OrphanCleanupService is disabled in configuration.");
				return;
			}

			try
			{
				var batchSize = this.Conf.World.OrphanCleanupBatchSize;

				_orphanCleanupService = new OrphanCleanupService(this.Database, batchSize);
			}
			catch (Exception ex)
			{
				Log.Error("Failed to initialize OrphanCleanup Service: {0}", ex);
			}
		}

		/// <summary>
		/// Gets the orphan cleanup service instance for triggering cleanup.
		/// </summary>
		public OrphanCleanupService OrphanCleanupService => _orphanCleanupService;

		private void StopServices()
		{
			Log.Info("Stopping server services...");

			// Stop accepting new connections
			this._acceptor?.Stop();
			Log.Info("Acceptor stopped.");

			// Stop world update loop (gracefully if possible)
			this.World?.Heartbeat.Stop();
			Log.Info("World stopped.");

			// Stop SaveQueue (drain remaining saves)
			SaveQueue.Stop();
			Log.Info("SaveQueue stopped.");

			// Dispose AutoSave Service
			_autoSaveService?.Dispose();
			Log.Info("AutoSave Service stopped.");

			// Dispose OrphanCleanup Service
			_orphanCleanupService?.Dispose();
			Log.Info("OrphanCleanup Service stopped.");

			// Disconnect communicator
			//Communicator?.Disconnect();
			Log.Info("Communicator disconnected.");

			// Other cleanup...
			Log.Info("Server services stopped.");
		}
	}
}
