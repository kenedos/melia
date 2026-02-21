using System.IO;
using Yggdrasil.Configuration;
using Yggdrasil.Logging;

namespace Melia.Shared.Configuration.Files
{
	/// <summary>
	/// Represents log.conf.
	/// </summary>
	public class LogConfFile : ConfFile
	{
		public LogLevel Filter { get; protected set; }
		public bool LogChat { get; protected set; }
		public bool LogItems { get; protected set; }

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

			this.Filter = (LogLevel)this.GetInt("log_filter", 0);
			this.LogChat = this.GetBool("log_chat", true);
			this.LogItems = this.GetBool("log_items", true);
		}
	}
}
