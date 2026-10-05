using System;

namespace Melia.Shared.Packages
{
	/// <summary>
	/// Marks a handler class as belonging to a specific package.
	/// Handlers with this attribute are only registered when the
	/// named package is enabled in packages.conf.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class PackageAttribute : Attribute
	{
		/// <summary>
		/// Returns the name of the package this handler belongs to.
		/// </summary>
		public string PackageName { get; }

		/// <summary>
		/// Creates a new package attribute for the given package name.
		/// </summary>
		/// <param name="packageName"></param>
		public PackageAttribute(string packageName)
		{
			this.PackageName = packageName;
		}
	}
}
