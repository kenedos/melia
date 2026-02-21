using System.IO;
using Yggdrasil.Configuration;

namespace Melia.Shared.Configuration.Files
{
	/// <summary>
	/// Represents inter.conf
	/// </summary>
	public class InterConfFile : ConfFile
	{
		public string Authentication { get; protected set; }

		/// <summary>
		/// Loads conf file and its options from the given path.
		/// </summary>
		/// <param name="filePath"></param>
		public void Load(string filePath, params string[] extraIncludes)
		{
			this.Include(filePath);

			foreach (var path in extraIncludes)
			{
				if (File.Exists(path))
					this.Include(path);
			}

			this.Authentication = this.GetString("authentication", "change_me");
		}
	}
}
