using System;

namespace Melia.Shared.Packages
{
	/// <summary>
	/// Marks a handler class as belonging to a specific package.
	/// Handlers with this attribute are only registered when any of the
	/// named packages is enabled in packages.conf.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class PackageAttribute : Attribute
	{
		/// <summary>
		/// Returns the names of the packages this handler belongs to.
		/// </summary>
		public string[] PackageNames { get; }

		/// <summary>
		/// Creates a new package attribute for the given package names.
		/// </summary>
		/// <param name="packageNames"></param>
		public PackageAttribute(params string[] packageNames)
		{
			this.PackageNames = packageNames;
		}
	}
}
