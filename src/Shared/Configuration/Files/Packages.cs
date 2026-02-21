using Yggdrasil.Configuration;

namespace Melia.Shared.Configuration.Files
{
	/// <summary>
	/// Represents packages.conf.
	/// </summary>
	public class PackagesConf : ConfFile
	{
		/// <summary>
		/// Returns the list of enabled package names.
		/// </summary>
		public string[] EnabledPackages { get; protected set; }

		/// <summary>
		/// Loads conf file and its options from the given path.
		/// </summary>
		/// <param name="filePath"></param>
		public void Load(string filePath)
		{
			this.Include(filePath);

			var value = this.GetString("enabled_packages", "");
			this.EnabledPackages = value.Split(',', System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries);
		}
	}
}
