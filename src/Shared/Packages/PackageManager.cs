using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Yggdrasil.Logging;

namespace Melia.Shared.Packages
{
	/// <summary>
	/// Manages feature packages that extend the server with additional
	/// data, scripts, and SQL updates.
	/// </summary>
	public class PackageManager
	{
		/// <summary>
		/// Package name that marks handlers of the system itself, which
		/// register whenever any package is enabled.
		/// </summary>
		public const string SystemName = "system";

		private static readonly string[] ExclusiveTypes = { "core", "skills", "world" };

		private readonly List<PackageInfo> _packages = new();

		/// <summary>
		/// Returns the list of loaded packages.
		/// </summary>
		public IReadOnlyList<PackageInfo> Packages => _packages;

		/// <summary>
		/// Returns true if two enabled packages share an exclusive type.
		/// </summary>
		public bool HasConflicts { get; private set; }

		/// <summary>
		/// Returns true if the given package name is enabled.
		/// </summary>
		/// <param name="packageName"></param>
		/// <returns></returns>
		public bool IsEnabled(string packageName)
			=> _packages.Any(p => string.Equals(p.Name, packageName, StringComparison.OrdinalIgnoreCase));

		/// <summary>
		/// Returns true if the given type should be registered based on
		/// its [Package] attribute. Types without the attribute are always
		/// registered. Types with the attribute are only registered if
		/// the named package is enabled.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool ShouldRegister(Type type)
		{
			var attr = (PackageAttribute)Attribute.GetCustomAttribute(type, typeof(PackageAttribute));
			if (attr == null)
				return true;

			return attr.PackageNames.Any(a => a == SystemName ? _packages.Count > 0 : this.IsEnabled(a));
		}

		/// <summary>
		/// Loads enabled packages from the packages directory.
		/// </summary>
		/// <param name="enabledPackages"></param>
		public void Load(string[] enabledPackages)
		{
			if (enabledPackages == null || enabledPackages.Length == 0)
				return;

			var packagesDir = "packages";
			if (!Directory.Exists(packagesDir))
			{
				Log.Warning("Packages directory '{0}' not found.", packagesDir);
				return;
			}

			foreach (var packageName in enabledPackages)
			{
				var packageDir = Path.Combine(packagesDir, packageName);
				if (!Directory.Exists(packageDir))
				{
					Log.Warning("Package '{0}' not found at '{1}'.", packageName, packageDir);
					continue;
				}

				var info = new PackageInfo(packageName, packageDir);
				_packages.Add(info);
				Log.Info("  loaded package '{0}' ({1}).", packageName, info.Type);
			}

			this.CheckConflicts();
		}

		/// <summary>
		/// Logs an error for every exclusive package type that more than
		/// one enabled package belongs to.
		/// </summary>
		private void CheckConflicts()
		{
			foreach (var type in ExclusiveTypes)
			{
				var names = _packages.Where(p => p.Type == type).Select(p => p.Name).ToList();
				if (names.Count < 2)
					continue;

				Log.Error("Packages '{0}' are all of type '{1}', and only one package of that type can be enabled.", string.Join("', '", names), type);
				this.HasConflicts = true;
			}
		}
	}

	/// <summary>
	/// Contains information about a loaded package.
	/// </summary>
	public class PackageInfo
	{
		/// <summary>
		/// Returns the name of the package.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Returns the root directory of the package.
		/// </summary>
		public string Directory { get; }

		/// <summary>
		/// Returns the path to the package's conf directory.
		/// </summary>
		public string ConfDirectory => Path.Combine(this.Directory, "conf");

		/// <summary>
		/// Returns the path to the package's db directory.
		/// </summary>
		public string DbDirectory => Path.Combine(this.Directory, "db");

		/// <summary>
		/// Returns the path to the package's scripts directory.
		/// </summary>
		public string ScriptsDirectory => Path.Combine(this.Directory, "scripts");

		/// <summary>
		/// Returns the path to the package's localization directory.
		/// </summary>
		public string LocalizationDirectory => Path.Combine(this.Directory, "localization");

		/// <summary>
		/// Returns the path to the package's SQL updates directory.
		/// </summary>
		public string SqlDirectory => Path.Combine(this.Directory, "sql");

		/// <summary>
		/// Returns the type declared in the package's package.conf:
		/// core, skills, world, or addon if none is declared.
		/// </summary>
		public string Type { get; private set; } = "addon";

		/// <summary>
		/// Creates a new package info instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="directory"></param>
		public PackageInfo(string name, string directory)
		{
			this.Name = name;
			this.Directory = directory;
			this.ReadType();
		}

		/// <summary>
		/// Reads the type option from the package's package.conf.
		/// </summary>
		private void ReadType()
		{
			var path = Path.Combine(this.Directory, "package.conf");
			if (!File.Exists(path))
				return;

			foreach (var line in File.ReadLines(path))
			{
				var trimmed = line.Trim();
				if (trimmed.StartsWith("//"))
					continue;

				var index = trimmed.IndexOf(':');
				if (index < 0 || trimmed.Substring(0, index).Trim() != "type")
					continue;

				this.Type = trimmed.Substring(index + 1).Trim().ToLowerInvariant();
				return;
			}
		}

		/// <summary>
		/// Returns the path to a conf file in this package,
		/// or null if it doesn't exist.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public string GetConfFilePath(string fileName)
		{
			var path = Path.Combine(this.ConfDirectory, fileName).Replace('\\', '/');
			return File.Exists(path) ? path : null;
		}

		/// <summary>
		/// Returns the path to a database file in this package,
		/// or null if it doesn't exist.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public string GetDbFilePath(string fileName)
		{
			var path = Path.Combine(this.DbDirectory, fileName).Replace('\\', '/');
			return File.Exists(path) ? path : null;
		}

		/// <summary>
		/// Returns the path to the package's scripts.txt for the
		/// given server type, or null if it doesn't exist.
		/// </summary>
		/// <param name="serverFolder"></param>
		/// <returns></returns>
		public string GetScriptsListPath(string serverFolder)
		{
			var path = Path.Combine(this.ScriptsDirectory, serverFolder, "scripts.txt").Replace('\\', '/');
			return File.Exists(path) ? path : null;
		}
	}
}
