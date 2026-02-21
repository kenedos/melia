using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using Melia.Shared.Configuration;
using Melia.Shared.Data;
using Melia.Shared.Data.Database;
using Melia.Shared.Database;
using Melia.Shared.Game.Properties;
using Melia.Shared.L10N;
using Melia.Shared.Network;
using Melia.Shared.Network.Inter.Messages;
using Melia.Shared.Packages;
using Melia.Shared.Versioning;
using Melia.Shared.Versioning.MEnums;
using Yggdrasil.Data;
using Yggdrasil.Extensions;
using Yggdrasil.Logging;
using Yggdrasil.Network.Communication;
using Yggdrasil.Network.TCP;
using Yggdrasil.Scripting;
using Yggdrasil.Util;

namespace Melia.Shared
{
	/// <summary>
	/// Base class for server applications.
	/// </summary>
	public abstract class Server
	{
		/// <summary>
		/// Returns this server's server info.
		/// </summary>
		public ServerInfo ServerInfo { get; private set; }

		/// <summary>
		/// Returns a reference to all conf files.
		/// </summary>
		public ConfFiles Conf { get; } = new ConfFiles();

		/// <summary>
		/// Returns a reference to the server's loaded data.
		/// </summary>
		public MeliaData Data { get; } = new MeliaData();

		/// <summary>
		/// Returns a reference to the server's script loader.
		/// </summary>
		protected ScriptLoader ScriptLoader { get; private set; }

		/// <summary>
		/// Returns a reference to the package manager.
		/// </summary>
		public PackageManager Packages { get; } = new PackageManager();

		/// <summary>
		/// Returns a reference to the server's string localizer manager.
		/// </summary>
		public MultiLocalizer MultiLocalization { get; } = new MultiLocalizer();

		/// <summary>
		/// Returns a reference to the server list.
		/// </summary>
		public ServerList ServerList { get; } = new ServerList();

		/// <summary>
		/// Returns the server type.
		/// </summary>
		public abstract ServerType Type { get; }

		/// <summary>
		/// Returns the server's communicator
		/// </summary>
		public Communicator Communicator { get; protected set; }

		public string Version
		{
			get
			{
				var version = Assembly.GetExecutingAssembly().GetName().Version;
				return $"{version.Major}.{version.Minor}.{version.Build}";
			}
		}

		/// <summary>
		/// Starts the server.
		/// </summary>
		/// <param name="args"></param>
		public abstract void Run(string[] args);

		/// <summary>
		/// Changes current directory to the project's root folder.
		/// </summary>
		protected void NavigateToRoot()
		{
			// First go to the assemblies directory and then back from
			// there until we find the root folder.
			var appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			Directory.SetCurrentDirectory(appDirectory);

			var folderNames = new[] { "bin", "user", "system" };
			var tries = 10;

			for (var i = 0; i < tries; ++i)
			{
				if (folderNames.All(a => Directory.Exists(a)))
					return;

				Directory.SetCurrentDirectory("../");
			}

			throw new DirectoryNotFoundException($"Failed to navigate to root folder. (Missing: {string.Join(", ", folderNames)})");
		}

		/// <summary>
		/// Loads all configuration files.
		/// </summary>
		/// <returns></returns>
		public ConfFiles LoadConf()
		{
			Log.Info("Loading configuration...");

			this.Conf.Load();
			Log.SetFilter(this.Conf.Log.Filter);

			if (this.Conf.Inter.Authentication == "change_me")
				Log.Warning("You're using the default password for inter-server communication. It is highly recommended that you change it in inter.conf.");

			return this.Conf;
		}

		/// <summary>
		/// Loads localization files and updates cultural settings.
		/// </summary>
		/// <returns></returns>
		protected void LoadLocalization(ConfFiles conf)
		{
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(conf.Localization.Culture);
			CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo(conf.Localization.CultureUi);
			Thread.CurrentThread.CurrentCulture = CultureInfo.DefaultThreadCurrentCulture;
			Thread.CurrentThread.CurrentUICulture = CultureInfo.DefaultThreadCurrentUICulture;

			var serverLanguage = conf.Localization.Language;
			var relativeFolderPath = "localization";
			var systemFolderPath = Path.Combine("system", relativeFolderPath);
			var userFolderPath = Path.Combine("system", relativeFolderPath);

			Log.Info("Loading localization...");

			// Load everything from user first, then check system, without
			// overriding the ones loaded from user
			if (Directory.Exists(userFolderPath))
			{
				foreach (var filePath in Directory.EnumerateFiles(userFolderPath, "*.po", SearchOption.AllDirectories))
				{
					var languageName = Path.GetFileNameWithoutExtension(filePath);
					this.MultiLocalization.Load(languageName, filePath);

					Log.Info("  loaded {0}.", languageName);
				}
			}

			if (Directory.Exists(systemFolderPath))
			{
				foreach (var filePath in Directory.EnumerateFiles(systemFolderPath, "*.po", SearchOption.AllDirectories))
				{
					var languageName = Path.GetFileNameWithoutExtension(filePath);
					if (this.MultiLocalization.Contains(languageName))
						continue;

					this.MultiLocalization.Load(languageName, filePath);

					Log.Info("  loaded {0}.", languageName);
				}
			}

			Log.Info("  setting default language to {0}.", serverLanguage);

			// Try to set the default localizer, and warn the user about
			// missing localizers if the selected server language isn't
			// US english.
			if (!this.MultiLocalization.Contains(serverLanguage))
			{
				if (serverLanguage != "en-US")
					Log.Warning("Localization file '{0}.po' not found.", serverLanguage);
			}
			else
			{
				this.MultiLocalization.SetDefault(serverLanguage);
			}

			Localization.SetLocalizer(this.MultiLocalization.GetDefault());
		}

		/// <summary>
		/// Initializes database connection with data from Conf.
		/// </summary>
		protected void InitDatabase(MeliaDb db, ConfFiles conf)
		{
			try
			{
				Log.Info("Initializing database...");
				db.Init(conf.Database.Host, conf.Database.User, conf.Database.Pass, conf.Database.Db);
			}
			catch (Exception ex)
			{
				Log.Error("Failed to initialize database: {0}", ex.Message);
				ConsoleUtil.Exit(1, true);
			}
		}

		/// <summary>
		/// Loads data from files.
		/// </summary>
		public void LoadData(ServerType serverType)
		{
			Log.Info("Loading data...");

			try
			{
				// Originally we were using a bitmask to specify which dbs
				// to load, but that's not very scalable, because even with
				// 64 bits, we'd run out of bits eventually. We could use a
				// list of enums, but that's kind of annoying to manage.
				// So we'll be lazy for now and just if/else it.
				// We could also move it to the servers to clean it up a
				// bit. TBD.

				if (serverType == ServerType.ISC)
				{
					this.LoadDb(this.Data.ServerDb, "db/servers.txt");
				}
				else if (serverType == ServerType.Barracks)
				{
					this.LoadDb(this.Data.BarrackDb, "db/barracks.txt");
					this.LoadDb(this.Data.ExpDb, "db/exp.txt");
					this.LoadDb(this.Data.FeatureDb, "db/features.txt");
					this.LoadDb(this.Data.InvBaseIdDb, "db/invbaseids.txt");
					this.LoadDb(this.Data.ItemDb, "db/items.txt");
					this.LoadDb(this.Data.JobDb, "db/jobs.txt");
					this.LoadDb(this.Data.MapDb, "db/maps.txt");
					this.LoadDb(this.Data.PropertiesDb, "db/properties.txt");
					this.LoadDb(this.Data.ServerDb, "db/servers.txt");
					this.LoadDb(this.Data.HairTypeDb, "db/hair_types.txt");
					this.LoadDb(this.Data.HeadTypeDb, "db/head_types.txt");
					this.LoadDb(this.Data.SkinToneDb, "db/skin_tones.txt");
					this.LoadDb(this.Data.StanceConditionDb, "db/stanceconditions.txt");

					PropertyTable.Load(this.Data.PropertiesDb);
				}
				else if (serverType == ServerType.Zone)
				{
					this.LoadDb(this.Data.AbilityDb, "db/abilities.txt");
					this.LoadDb(this.Data.AbilityTreeDb, "db/abilitytree.txt");
					this.LoadDb(this.Data.AccountOptionDb, "db/account_options.txt");
					this.LoadDb(this.Data.AchievementDb, "db/achievements.txt");
					this.LoadDb(this.Data.AchievementPointDb, "db/achievement_points.txt");
					this.LoadDb(this.Data.BarrackDb, "db/barracks.txt");
					this.LoadDb(this.Data.BuffDb, "db/buffs.txt");
					this.LoadDb(this.Data.CabinetDb, "db/cabinet_items.txt");
					this.LoadDb(this.Data.ChatEmoticonDb, "db/chat_emoticons.txt");
					this.LoadDb(this.Data.ChatMacroDb, "db/chatmacros.txt");
					this.LoadDb(this.Data.CompanionDb, "db/companions.txt");
					this.LoadDb(this.Data.CooldownDb, "db/cooldowns.txt");
					this.LoadDb(this.Data.CubeGachaDb, "db/cube_gacha.txt");
					this.LoadDb(this.Data.CustomCommandDb, "db/customcommands.txt");
					this.LoadDb(this.Data.DialogDb, "db/dialogues.txt");
					this.LoadDb(this.Data.DialogTxDb, "db/dialog_tx_scripts.txt");
					this.LoadDb(this.Data.EquipCardDb, "db/equip_cards.txt");
					this.LoadDb(this.Data.ExpDb, "db/exp.txt");
					this.LoadDb(this.Data.FactionDb, "db/factions.txt");
					this.LoadDb(this.Data.FeatureDb, "db/features.txt");
					this.LoadDb(this.Data.GroundDb, "db/ground.dat");
					this.LoadDb(this.Data.HairTypeDb, "db/hair_types.txt");
					this.LoadDb(this.Data.HeadTypeDb, "db/head_types.txt");
					this.LoadDb(this.Data.HelpDb, "db/help.txt");
					this.LoadDb(this.Data.InvBaseIdDb, "db/invbaseids.txt");
					this.LoadDb(this.Data.ItemDb, "db/items.txt");
					this.LoadDb(this.Data.ItemExpDb, "db/item_exp.txt");
					this.LoadDb(this.Data.ItemGradeDb, "db/item_grades.txt");
					this.LoadDb(this.Data.ItemMonsterDb, "db/itemmonsters.txt");
					this.LoadDb(this.Data.ItemSummonBossDb, "db/item_summonboss.txt");
					this.LoadDb(this.Data.JobDb, "db/jobs.txt");
					this.LoadDb(this.Data.MapDb, "db/maps.txt");
					this.LoadDb(this.Data.MonsterDb, "db/monsters.txt");
					this.LoadDb(this.Data.NormalTxDb, "db/normal_tx_scripts.txt");
					this.LoadDb(this.Data.PacketStringDb, "db/packetstrings.txt");
					this.LoadDb(this.Data.PropertiesDb, "db/properties.txt");
					this.LoadDb(this.Data.QuestDb, "db/quests.txt");
					this.LoadDb(this.Data.RecipeDb, "db/recipes.txt");
					this.LoadDb(this.Data.ResurrectionPointDb, "db/resurrection_points.txt");
					this.LoadDb(this.Data.SelectItemDb, "db/select_items.txt");
					this.LoadDb(this.Data.ServerDb, "db/servers.txt");
					this.LoadDb(this.Data.SessionObjectDb, "db/sessionobjects.txt");
					this.LoadDb(this.Data.ShopDb, "db/shops.txt");
					this.LoadDb(this.Data.SimonyDb, "db/simony.txt");
					this.LoadDb(this.Data.SkillDb, "db/skills.txt");
					this.LoadDb(this.Data.SkillTreeDb, "db/skilltree.txt");
					this.LoadDb(this.Data.SkinToneDb, "db/skin_tones.txt");
					this.LoadDb(this.Data.SocketPriceDb, "db/socket_prices.txt");
					this.LoadDb(this.Data.StanceConditionDb, "db/stanceconditions.txt");
					this.LoadDb(this.Data.SystemMessageDb, "db/system_messages.txt");
					//this.LoadDb(this.Data.TpItemDb, "db/tp_items.txt");
					this.LoadDb(this.Data.TradeShopDb, "db/trade_shop.txt");
					this.LoadDb(this.Data.WarpDb, "db/warps.txt");

					// Load collections after properties and items, to enable
					// data checks
					this.LoadDb(this.Data.CollectionDb, "db/collections.txt");
					this.LoadDb(this.Data.FurnitureDb, "db/furniture.txt");
					this.LoadDb(this.Data.InstanceDungeonDb, "db/instance_dungeons.txt");
					this.LoadDb(this.Data.ItemSetDb, "db/itemsets.txt");
					if (Versions.Protocol > 500)
						this.LoadDb(this.Data.RedemptionDb, "db/redemption_sets.txt");

					this.LoadDb(this.Data.GlobalDropDb, "db/global_drops.txt");
					this.LoadDb(this.Data.MapBonusDropsDb, "db/map_bonus_drops.txt");
					this.LoadDb(this.Data.TreasureDropDb, "db/treasure_drops.txt");
					this.LoadDb(this.Data.TreasureSpawnPointDb, "db/treasure_spawn_points.txt");
					this.LoadDb(this.Data.MinigameSpawnPointDb, "db/minigame_spawn_points.txt");

					PropertyTable.Load(this.Data.PropertiesDb);
				}
				else if (serverType == ServerType.Social)
				{
					this.LoadDb(this.Data.PropertiesDb, "db/properties.txt");
					this.LoadDb(this.Data.ServerDb, "db/servers.txt");
					this.LoadDb(this.Data.SystemMessageDb, "db/system_messages.txt");
				}
				else if (serverType == ServerType.Web)
				{
					this.LoadDb(this.Data.ServerDb, "db/servers.txt");
					this.LoadDb(this.Data.InvBaseIdDb, "db/invbaseids.txt");
					this.LoadDb(this.Data.ItemDb, "db/items.txt");
					this.LoadDb(this.Data.MapDb, "db/maps.txt");
					this.LoadDb(this.Data.MonsterDb, "db/monsters.txt");
					this.LoadDb(this.Data.JobDb, "db/jobs.txt");
					this.LoadDb(this.Data.SkillDb, "db/skills.txt");
					this.LoadDb(this.Data.SkillTreeDb, "db/skilltree.txt");
				}
				else if (serverType == ServerType.Lada)
				{
					this.LoadDb(this.Data.PropertiesDb, "db/properties.txt");
					this.LoadDb(this.Data.AbilityDb, "db/abilities.txt");
					this.LoadDb(this.Data.AbilityTreeDb, "db/abilitytree.txt");
					this.LoadDb(this.Data.AccountOptionDb, "db/account_options.txt");
					this.LoadDb(this.Data.AchievementDb, "db/achievements.txt");
					this.LoadDb(this.Data.AchievementPointDb, "db/achievement_points.txt");
					this.LoadDb(this.Data.BarrackDb, "db/barracks.txt");
					this.LoadDb(this.Data.BuffDb, "db/buffs.txt");
					this.LoadDb(this.Data.CabinetDb, "db/cabinet_items.txt");
					this.LoadDb(this.Data.ChatEmoticonDb, "db/chat_emoticons.txt");
					this.LoadDb(this.Data.ChatMacroDb, "db/chatmacros.txt");
					this.LoadDb(this.Data.CompanionDb, "db/companions.txt");
					this.LoadDb(this.Data.CooldownDb, "db/cooldowns.txt");
					this.LoadDb(this.Data.CubeGachaDb, "db/cube_gacha.txt");
					this.LoadDb(this.Data.CustomCommandDb, "db/customcommands.txt");
					this.LoadDb(this.Data.DialogDb, "db/dialogues.txt");
					this.LoadDb(this.Data.DialogTxDb, "db/dialog_tx_scripts.txt");
					this.LoadDb(this.Data.EquipCardDb, "db/equip_cards.txt");
					this.LoadDb(this.Data.ExpDb, "db/exp.txt");
					this.LoadDb(this.Data.FactionDb, "db/factions.txt");
					this.LoadDb(this.Data.FeatureDb, "db/features.txt");
					this.LoadDb(this.Data.GroundDb, "db/ground.dat");
					this.LoadDb(this.Data.HairTypeDb, "db/hair_types.txt");
					this.LoadDb(this.Data.HeadTypeDb, "db/head_types.txt");
					this.LoadDb(this.Data.HelpDb, "db/help.txt");
					this.LoadDb(this.Data.InstanceDungeonDb, "db/instance_dungeons.txt");
					this.LoadDb(this.Data.InvBaseIdDb, "db/invbaseids.txt");
					this.LoadDb(this.Data.ItemDb, "db/items.txt");
					this.LoadDb(this.Data.ItemExpDb, "db/item_exp.txt");
					this.LoadDb(this.Data.ItemGradeDb, "db/item_grades.txt");
					this.LoadDb(this.Data.ItemMonsterDb, "db/itemmonsters.txt");
					this.LoadDb(this.Data.ItemSummonBossDb, "db/item_summonboss.txt");
					this.LoadDb(this.Data.JobDb, "db/jobs.txt");
					this.LoadDb(this.Data.MapDb, "db/maps.txt");
					this.LoadDb(this.Data.MonsterDb, "db/monsters.txt");
					this.LoadDb(this.Data.NormalTxDb, "db/normal_tx_scripts.txt");
					this.LoadDb(this.Data.PacketStringDb, "db/packetstrings.txt");
					this.LoadDb(this.Data.QuestDb, "db/quests.txt");
					this.LoadDb(this.Data.RecipeDb, "db/recipes.txt");
					this.LoadDb(this.Data.ResurrectionPointDb, "db/resurrection_points.txt");
					this.LoadDb(this.Data.SelectItemDb, "db/select_items.txt");
					this.LoadDb(this.Data.ServerDb, "db/servers.txt");
					this.LoadDb(this.Data.SessionObjectDb, "db/sessionobjects.txt");
					this.LoadDb(this.Data.ShopDb, "db/shops.txt");
					this.LoadDb(this.Data.SkillDb, "db/skills.txt");
					this.LoadDb(this.Data.SkillTreeDb, "db/skilltree.txt");
					this.LoadDb(this.Data.SkinToneDb, "db/skin_tones.txt");
					this.LoadDb(this.Data.SocketPriceDb, "db/socket_prices.txt");
					this.LoadDb(this.Data.StanceConditionDb, "db/stanceconditions.txt");
					this.LoadDb(this.Data.SystemMessageDb, "db/system_messages.txt");
					//this.LoadDb(this.Data.TpItemDb, "db/tp_items.txt");
					this.LoadDb(this.Data.TradeShopDb, "db/trade_shop.txt");
					this.LoadDb(this.Data.WarpDb, "db/warps.txt");

					// Load collections after properties and items, to enable
					// data checks
					this.LoadDb(this.Data.CollectionDb, "db/collections.txt");
					this.LoadDb(this.Data.RedemptionDb, "db/redemption_sets.txt");

					this.LoadDb(this.Data.GlobalDropDb, "db/global_drops.txt");
					this.LoadDb(this.Data.MapBonusDropsDb, "db/map_bonus_drops.txt");
					this.LoadDb(this.Data.TreasureDropDb, "db/treasure_drops.txt");
					this.LoadDb(this.Data.TreasureSpawnPointDb, "db/treasure_spawn_points.txt");
					this.LoadDb(this.Data.MinigameSpawnPointDb, "db/minigame_spawn_points.txt");

					this.LoadCustomDb(this.Data.ItemIconDb, "user/tools/lada/db/item_icons.txt");
					this.LoadCustomDb(this.Data.MonsterIconDb, "user/tools/lada/db/monster_icons.txt");
				}

			}
			catch (DatabaseErrorException ex)
			{
				Log.Error(ex.ToString());
				ConsoleUtil.Exit(1);
			}
			catch (FileNotFoundException ex)
			{
				Log.Error(ex.Message);
				ConsoleUtil.Exit(1);
			}
			catch (Exception ex)
			{
				Log.Error("Error while loading data: " + ex);
				ConsoleUtil.Exit(1);
			}
		}

		/// <summary>
		/// Loads a database using the 3-tier system: system → packages → user.
		/// For indexed databases, each tier merges on top of the previous.
		/// For non-indexed databases, the highest tier present replaces all lower tiers.
		/// </summary>
		private void LoadDb(IDatabase db, string fileName, bool isOptional = false)
		{
			db.Clear();

			// --- Define file paths ---
			var baseSystemPath = Path.Combine("system", fileName).Replace('\\', '/');
			var baseUserPath = Path.Combine("user", fileName).Replace('\\', '/');
			string systemPathToLoad;
			string userPathToLoad = null;

			var useVersioning = Versions.Client != 0;
			if (useVersioning)
			{
				var versionString = Versions.Client.ToString();
				var versionedSystemDir = Path.Combine("system", "versions", versionString);
				var versionedSystemPath = Path.Combine(versionedSystemDir, fileName).Replace('\\', '/');
				var versionedUserPath = Path.Combine("user", versionString, fileName).Replace('\\', '/');

				systemPathToLoad = (Directory.Exists(versionedSystemDir) && File.Exists(versionedSystemPath))
					? versionedSystemPath
					: baseSystemPath;

				if (File.Exists(versionedUserPath))
					userPathToLoad = versionedUserPath;
				else if (File.Exists(baseUserPath))
					userPathToLoad = baseUserPath;
			}
			else
			{
				systemPathToLoad = baseSystemPath;
				if (File.Exists(baseUserPath))
					userPathToLoad = baseUserPath;
			}

			// --- Load Base (System) Data ---
			if (!File.Exists(systemPathToLoad))
			{
				Log.Error("LoadDb: Base data file '{0}' not found.", systemPathToLoad);
				if (!isOptional)
					ConsoleUtil.Exit(1);
				return;
			}

			db.LoadFile(systemPathToLoad);
			foreach (var ex in db.GetWarnings())
				Log.Warning(ex);

			var isIndexedDb = db.GetType().Name.Contains("Indexed") || db.GetType().BaseType?.Name.Contains("Indexed") == true;

			// --- Load Package Data (between system and user) ---
			var packageFileLoaded = false;
			foreach (var package in this.Packages.Packages)
			{
				var packagePath = package.GetDbFilePath(Path.GetFileName(fileName));
				if (packagePath == null)
					continue;

				if (!isIndexedDb)
					db.Clear();

				db.LoadFile(packagePath);
				foreach (var ex in db.GetWarnings())
					Log.Warning(ex);
				packageFileLoaded = true;
			}

			// --- Load Override (User) Data ---
			var userFileLoaded = false;
			if (userPathToLoad != null)
			{
				if (!isIndexedDb)
					db.Clear();

				db.LoadFile(userPathToLoad);
				foreach (var ex in db.GetWarnings())
					Log.Warning(ex);
				userFileLoaded = true;
			}

			// --- Final Logging ---
			var logMessage = new StringBuilder();
			logMessage.AppendFormat("  done loading {0} {1} from {2}", db.Count, db.Count == 1 ? "entry" : "entries", Path.GetFileName(fileName));
			if (packageFileLoaded || userFileLoaded)
			{
				var sources = new StringBuilder();
				if (packageFileLoaded)
					sources.Append("package");
				if (userFileLoaded)
				{
					if (sources.Length > 0)
						sources.Append(", ");
					sources.Append(userPathToLoad);
				}
				logMessage.Append($" (with overrides from {sources})");
			}
			Log.Info(logMessage.ToString());
		}

		/// <summary>
		/// Loads db from exact path.
		/// Logs problems as warnings.
		/// </summary>
		private void LoadCustomDb(IDatabase db, string fileName)
		{
			if (!File.Exists(fileName))
			{
				Log.Error("LoadDataFile: File '{0}' not found.", fileName);
				ConsoleUtil.Exit(1);
				return;
			}

			db.Clear();
			db.LoadFile(fileName);
			foreach (var ex in db.GetWarnings())
				Log.Warning(ex);

			if (db.Count == 1)
				Log.Info("  done loading {0} entry from {1}.", db.Count, fileName);
			else
				Log.Info("  done loading {0} entries from {1}.", db.Count, fileName);
		}

		/// <summary>
		/// Loads given conf class and stops start up when an error
		/// occurs.
		/// </summary>
		/// <param name="conf"></param>
		protected void LoadConf(ConfFiles conf)
		{
			try
			{
				Log.Info("Loading configuration...");
				conf.Load();
			}
			catch (Exception ex)
			{
				Log.Error("Failed to load configuration: {0}", ex.Message);
				ConsoleUtil.Exit(1, true);
			}
		}

		/// <summary>
		/// Loads all scripts from the scripts lists in the given scripts
		/// sub-folder.
		/// </summary>
		/// <param name="scriptFolderName"></param>
		public void LoadScripts(string scriptFolderName)
		{
			if (this.ScriptLoader != null)
			{
				Log.Error("The script loader has been created already.");
				return;
			}

			Log.Info("Loading scripts...");

			var listFilePath = Path.Combine("system", "scripts", scriptFolderName, "scripts.txt");

			if (!File.Exists(listFilePath))
			{
				Log.Error("Script list not found: " + listFilePath);
				return;
			}

			var timer = Stopwatch.StartNew();

			try
			{
				var cachePath = (string)null;
				if (this.Conf.Scripts.CacheScripts)
					cachePath = Path.Combine("user", "cache", "scripts", scriptFolderName + ".dll");

				var userPath = Path.Combine("user", "scripts", scriptFolderName);
				var systemPath = Path.Combine("system", "scripts", scriptFolderName);

				this.ScriptLoader = new ScriptLoader(cachePath);

				this.ScriptLoader.References.Add(typeof(JsonSerializer).Assembly.Location);
				this.ScriptLoader.References.Add(typeof(HttpClient).Assembly.Location);
				this.ScriptLoader.References.Add(typeof(Uri).Assembly.Location);

				// Write package script entries into scripts_packages.txt,
				// which is already required by the system scripts.txt.
				this.WritePackageScriptsList(scriptFolderName);

				// Add package script directories as search paths so
				// relative paths in package scripts.txt resolve correctly.
				var searchPaths = this.BuildScriptSearchPaths(userPath, systemPath, scriptFolderName);

				this.ScriptLoader.LoadFromListFile(listFilePath, searchPaths);

				foreach (var ex in this.ScriptLoader.ReferenceExceptions)
					Log.Warning(ex);

				foreach (var ex in this.ScriptLoader.LoadingExceptions)
					Log.Error(ex);
			}
			catch (CompilerErrorException ex)
			{
				this.DisplayScriptErrors(ex);
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			Log.Info("  loaded {0} scripts from {3} files in {2:n2}s ({1} init fails).", this.ScriptLoader.LoadedCount, this.ScriptLoader.FailCount, timer.Elapsed.TotalSeconds, this.ScriptLoader.FileCount);
		}

		/// <summary>
		/// Generates package script loading files for the given server type.
		/// When packages with scripts are enabled, creates a scripts_override.txt
		/// that the divert directive in scripts.txt picks up, loading package
		/// scripts INSTEAD of system scripts to avoid class name conflicts.
		/// When no packages are enabled, removes the override so divert falls
		/// through to default loading.
		/// </summary>
		private void WritePackageScriptsList(string scriptFolderName)
		{
			var systemScriptDir = Path.Combine("system", "scripts", scriptFolderName);
			var overridePath = Path.Combine(systemScriptDir, "scripts_override.txt");
			var packagesListPath = Path.Combine(systemScriptDir, "scripts_packages.txt");

			// Always write scripts_packages.txt (required by scripts.txt when
			// no divert is active). Keep it empty since package scripts are
			// loaded via the override/divert mechanism instead.
			using (var writer = new StreamWriter(packagesListPath))
			{
				writer.WriteLine("// Auto-generated at startup. Do not edit.");
			}

			// Collect packages that have scripts for this server type
			var packageScriptEntries = new List<(string Name, string Path)>();
			foreach (var package in this.Packages.Packages)
			{
				var path = package.GetScriptsListPath(scriptFolderName);
				if (path != null)
					packageScriptEntries.Add((package.Name, path));
			}

			// No packages with scripts — remove override so divert falls
			// through to default system script loading
			if (packageScriptEntries.Count == 0)
			{
				if (File.Exists(overridePath))
					File.Delete(overridePath);
				return;
			}

			// Generate scripts_override.txt that replaces the default
			// scripts.txt via divert
			using (var writer = new StreamWriter(overridePath))
			{
				writer.WriteLine("// Auto-generated at startup. Do not edit.");
				writer.WriteLine("// When packages are enabled, this file is loaded via divert");
				writer.WriteLine("// INSTEAD of the default scripts.txt entries, so that package");
				writer.WriteLine("// scripts replace system scripts rather than conflicting.");
				writer.WriteLine("//---------------------------------------------------------------------------");
				writer.WriteLine();

				// System-only scripts not provided by packages
				writer.WriteLine("commands/**/*");

				foreach (var (name, path) in packageScriptEntries)
				{
					writer.WriteLine();
					writer.WriteLine("// Package: {0}", name);
					var relativePath = Path.GetRelativePath(systemScriptDir, path).Replace('\\', '/');
					writer.WriteLine("require \"{0}\"", relativePath);
				}
			}
		}

		/// <summary>
		/// Builds the priority search path array for script loading:
		/// user → packages → system.
		/// </summary>
		private string[] BuildScriptSearchPaths(string userPath, string systemPath, string scriptFolderName)
		{
			var paths = new List<string> { userPath };

			foreach (var package in this.Packages.Packages)
			{
				var pkgScriptPath = Path.Combine(package.ScriptsDirectory, scriptFolderName);
				if (Directory.Exists(pkgScriptPath))
					paths.Add(pkgScriptPath);
			}

			paths.Add(systemPath);
			return paths.ToArray();
		}

		/// <summary>
		/// Reloads previously loaded scripts.
		/// </summary>
		public void ReloadScripts()
		{
			Log.Info("Reloading scripts...");

			var timer = Stopwatch.StartNew();

			try
			{
				this.ScriptLoader.Reload();
			}
			catch (CompilerErrorException ex)
			{
				this.DisplayScriptErrors(ex);
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}

			Log.Info("  reloaded {0} scripts from {3} files in {2:n2}s ({1} init fails).", this.ScriptLoader.LoadedCount, this.ScriptLoader.FailCount, timer.Elapsed.TotalSeconds, this.ScriptLoader.FileCount);
		}

		/// <summary>
		/// Displays the script errors in the console.
		/// </summary>
		/// <param name="ex"></param>
		private void DisplayScriptErrors(CompilerErrorException ex)
		{
			foreach (var err in ex.Errors)
			{
				if (string.IsNullOrWhiteSpace(err.FileName))
				{
					Log.Error("While loading scripts: " + err.ErrorText);
				}
				else
				{
					var relativefileName = err.FileName;
					var cwd = Directory.GetCurrentDirectory();
					if (relativefileName.StartsWith(cwd, StringComparison.InvariantCultureIgnoreCase))
						relativefileName = relativefileName.Substring(cwd.Length + 1);

					var lines = File.ReadAllLines(err.FileName);
					var sb = new StringBuilder();

					// Error msg
					sb.AppendLine("In {0} on line {1}, column {2}", relativefileName, err.Line, err.Column);
					sb.AppendLine("          {0}", err.ErrorText);

					// Display lines around the error
					var startLine = Math.Max(1, err.Line - 1);
					var endLine = Math.Min(lines.Length, startLine + 2);
					for (var i = startLine; i <= endLine; ++i)
					{
						// Make sure we don't get out of range.
						// (ReadAllLines "trims" the input)
						var line = (i <= lines.Length) ? lines[i - 1] : "";

						sb.AppendLine("  {2} {0:0000}: {1}", i, line, (err.Line == i ? '*' : ' '));
					}

					if (err.IsWarning)
						Log.Warning(sb.ToString());
					else
						Log.Error(sb.ToString());
				}
			}
		}

		/// <summary>
		/// Loads the server list from the database.
		/// </summary>
		/// <param name="serverDb"></param>
		/// <param name="serverType"></param>
		/// <param name="groupId"></param>
		/// <param name="serverId"></param>
		protected void LoadServerList(ServerDb serverDb, ServerType serverType, int groupId, int serverId)
		{
			Log.Info("Loading server list...");

			this.ServerList.Load(serverDb, groupId);
			this.ServerInfo = this.GetServerInfo(serverType, serverId);
		}

		/// <summary>
		/// Reads the server id from the arguments and returns it.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="groupId"></param>
		/// <param name="serverId"></param>
		/// <returns></returns>
		protected void GetServerId(string[] args, out int groupId, out int serverId)
		{
			groupId = 1001;
			serverId = 1;

			if (args.Length > 0 && int.TryParse(args[0], out var gid))
				groupId = gid;

			if (args.Length > 1 && int.TryParse(args[1], out var sid))
				serverId = sid;
		}

		/// <summary>
		/// Returns the server info for given type and id.
		/// </summary>
		/// <param name="type"></param>
		/// <param="serverId"></param>
		/// <returns></returns>
		protected ServerInfo GetServerInfo(ServerType type, int serverId)
		{
			if (!this.ServerList.TryGet(type, serverId, out var serverData))
			{
				Log.Error("No server data for '{0}:{1}' found in 'db/servers.txt'.", type, serverId);
				ConsoleUtil.Exit(1);
			}

			return serverData;
		}

		/// <summary>
		/// Checks whether the given connection is valid and should be accepted.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="database"></param>
		/// <returns></returns>
		protected ConnectionCheck CheckConnection(TcpConnection conn, MeliaDb database)
		{
			if (database.CheckIpBan(conn.Address))
				return new ConnectionCheck(ConnectionCheckResult.Reject, "IP banned");

			return new ConnectionCheck(ConnectionCheckResult.Accept, "Accepted");
		}

		/// <summary>
		/// Sends an update about the server's status to the coordinator.
		/// </summary>
		public virtual void UpdateServerInfo(ServerStatus status, int playerCount = 0, ServerRates rates = null)
		{
			var serverId = this.ServerInfo.Id;

			var message = new ServerUpdateMessage(this.Type, serverId, playerCount, status, rates);
			this.Communicator.Send("Coordinator", message);
		}

		/// <summary>
		/// Loads enabled packages from packages.conf.
		/// </summary>
		protected void LoadPackages()
		{
			Log.Info("Loading packages...");
			this.Packages.Load(this.Conf.Packages.EnabledPackages);

			if (this.Packages.Packages.Count == 0)
				Log.Info("  no packages enabled.");
		}

		/// <summary>
		/// Loads version info and versioned enums from files.
		/// </summary>
		public void LoadVersionInfo()
		{
			Log.Info("Loading version information...");

			if (!File.Exists("system/versions/version.txt"))
			{
				Log.Error("Version file 'version.txt' not found.");
				return;
			}

			try
			{
				// Get global variables
				var preprocessor = new Preprocessor();
				preprocessor.ProcessFile("system/versions/version.txt");

				if (!preprocessor.TryGetDefined("VERSION", out var version))
					throw new MissingFieldException("Variable 'VERSION' not found in version.txt");
				else if (!(version is int))
					throw new Exception($"Invalid version '{version}'.");

				if (!preprocessor.TryGetDefined("PROTOCOL_VERSION", out var protocolVersion))
					throw new MissingFieldException("Variable 'PROTOCOL_VERSION' not found in version.txt");
				else if (!(protocolVersion is int))
					throw new Exception($"Invalid protocol version '{protocolVersion}'.");

				Versions.Client = (int)version;
				Versions.Protocol = (int)protocolVersion;

				// Check protocol version for validity here so we don't have
				// to add default throws everywhere we use it. There aren't
				// gonna be dozens of possible versions, so as long as we
				// check the known ones here and don't mess up using them,
				// we're good.
				switch (Versions.Protocol)
				{
					case 0: // Oldest Client
					case 100: // Open Beta Steam Client
					case 201: // "Chinese" Client
					case 1000: // Latest
						break;

					default:
						throw new Exception($"Unknown protocol version '{Versions.Protocol}'.");
				}

				Log.Info("  Version: {0}", Versions.Client);
				Log.Info("  Protocol Version: {0}", Versions.Protocol);

				Log.Info("Loading ops...");

				var fileName = "op_" + Versions.Client + ".txt";
				var systemPath = Path.Combine("system", "versions", "ops", fileName);
				if (File.Exists(systemPath))
					this.LoadOps(this.Data.OpDb, "op_" + Versions.Client + ".txt");
				else
				{
					Log.Info("Loading latest OPs");
					this.LoadOps(this.Data.OpDb, "op.txt");
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				ConsoleUtil.Exit(1);
			}
		}

		/// <summary>
		/// Loads the Op Codes from system, then overrides with user files.
		/// </summary>
		/// <param name="db"></param>
		/// <param name="fileName"></param>
		private void LoadOps(OpDb db, string fileName)
		{
			try
			{
				if (db == null)
					db = new OpDb();
				db.Clear();

				var systemPath = Path.Combine("system", "versions", "ops", fileName);
				var userPath = Path.Combine("user", "versions", "ops", fileName);

				// Load base system file
				if (!File.Exists(systemPath))
				{
					Log.Error("LoadOps: File '{0}' not found.", systemPath);
					ConsoleUtil.Exit(1);
					return;
				}
				db.LoadFile(systemPath);
				foreach (var ex in db.GetWarnings())
					Log.Warning(ex);

				// Load user override file if it exists
				bool userFileLoaded = false;
				if (File.Exists(userPath))
				{
					db.LoadFile(userPath);
					foreach (var ex in db.GetWarnings())
						Log.Warning(ex);
					userFileLoaded = true;
				}

				var logMessage = new StringBuilder();
				logMessage.AppendFormat("  done loading {0} {1} from {2}", db.Count, db.Count == 1 ? "op" : "ops", fileName);
				if (userFileLoaded)
				{
					logMessage.Append($" (with overrides from {userPath})");
				}
				Log.Info(logMessage.ToString());
			}
			catch (DatabaseErrorException ex)
			{
				Log.Error(ex);
				ConsoleUtil.Exit(1);
			}
		}
	}
}
