using System.Collections.Generic;
using System.IO;
using Melia.Shared.Configuration.Files;

namespace Melia.Shared.Configuration
{
	/// <summary>
	/// Wrapper around all conf files for easy access.
	/// </summary>
	public class ConfFiles
	{
		/// <summary>
		/// commands.conf
		/// </summary>
		public CommandsConfFile Commands { get; } = new();

		/// <summary>
		/// database.conf
		/// </summary>
		public DatabaseConfFile Database { get; } = new();

		/// <summary>
		/// inter.conf
		/// </summary>
		public InterConfFile Inter { get; } = new();

		/// <summary>
		/// log.conf
		/// </summary>
		public LogConfFile Log { get; } = new();

		/// <summary>
		/// localization.conf
		/// </summary>
		public LocalizationConf Localization { get; } = new();

		/// <summary>
		/// scripts.conf
		/// </summary>
		public ScriptsConf Scripts { get; } = new();

		/// <summary>
		/// social.conf
		/// </summary>
		public SocialConf Social { get; } = new();

		/// <summary>
		/// barracks.conf
		/// </summary>
		public BarracksConfFile Barracks { get; } = new();

		/// <summary>
		/// web.conf
		/// </summary>
		public WebConfFile Web { get; } = new();

		/// <summary>
		/// world.conf
		/// </summary>
		public WorldConfFile World { get; } = new();

		/// <summary>
		/// packages.conf
		/// </summary>
		public PackagesConf Packages { get; } = new();

		/// <summary>
		/// Loads all conf files, with package conf overrides applied
		/// between system and user tiers.
		/// </summary>
		public virtual void Load()
		{
			this.Packages.Load("system/conf/packages.conf");

			var packageConfDirs = new List<string>();
			foreach (var name in this.Packages.EnabledPackages)
			{
				var confDir = Path.Combine("packages", name, "conf").Replace('\\', '/');
				if (Directory.Exists(confDir))
					packageConfDirs.Add(confDir);
			}

			this.Barracks.Load("system/conf/barracks.conf", GetPackageConfs(packageConfDirs, "barracks.conf"));
			this.Commands.Load("system/conf/commands.conf", GetPackageConfs(packageConfDirs, "commands.conf"));
			this.Database.Load("system/conf/database.conf", GetPackageConfs(packageConfDirs, "database.conf"));
			this.Inter.Load("system/conf/inter.conf", GetPackageConfs(packageConfDirs, "inter.conf"));
			this.Log.Load("system/conf/log.conf", GetPackageConfs(packageConfDirs, "log.conf"));
			this.Localization.Load("system/conf/localization.conf", GetPackageConfs(packageConfDirs, "localization.conf"));
			this.Scripts.Load("system/conf/scripts.conf", GetPackageConfs(packageConfDirs, "scripts.conf"));
			this.Social.Load("system/conf/social.conf", GetPackageConfs(packageConfDirs, "social.conf"));
			this.Web.Load("system/conf/web.conf", GetPackageConfs(packageConfDirs, "web.conf"));
			this.World.Load("system/conf/world.conf", GetPackageConfs(packageConfDirs, "world.conf"));
		}

		/// <summary>
		/// Returns the paths to a named conf file across all enabled
		/// package conf directories, filtering to only those that exist.
		/// </summary>
		private static string[] GetPackageConfs(List<string> packageConfDirs, string fileName)
		{
			var paths = new List<string>();

			foreach (var dir in packageConfDirs)
			{
				var path = Path.Combine(dir, fileName).Replace('\\', '/');
				if (File.Exists(path))
					paths.Add(path);
			}

			return paths.ToArray();
		}
	}
}
