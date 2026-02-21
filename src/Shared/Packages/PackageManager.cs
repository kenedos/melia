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
		private readonly List<PackageInfo> _packages = new();

		/// <summary>
		/// Returns the list of loaded packages.
		/// </summary>
		public IReadOnlyList<PackageInfo> Packages => _packages;

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

			return this.IsEnabled(attr.PackageName);
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
				Log.Info("  loaded package '{0}'.", packageName);
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
		/// Returns the path to the package's SQL updates directory.
		/// </summary>
		public string SqlDirectory => Path.Combine(this.Directory, "sql");

		/// <summary>
		/// Creates a new package info instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="directory"></param>
		public PackageInfo(string name, string directory)
		{
			this.Name = name;
			this.Directory = directory;
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
