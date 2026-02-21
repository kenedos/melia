using System.IO;
using Yggdrasil.Configuration;

namespace Melia.Shared.Configuration.Files
{
	/// <summary>
	/// Represents localization.conf.
	/// </summary>
	public class LocalizationConf : ConfFile
	{
		public string Language { get; protected set; }
		public string Culture { get; protected set; }
		public string CultureUi { get; protected set; }

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

			this.Language = this.GetString("language", "en-US");
			this.Culture = this.GetString("culture", "en-US");
			this.CultureUi = this.GetString("culture_ui", "en-US");
		}
	}
}
