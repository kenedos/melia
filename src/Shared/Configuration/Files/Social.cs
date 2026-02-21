using System.IO;
using Yggdrasil.Configuration;

namespace Melia.Shared.Configuration.Files
{
	/// <summary>
	/// Represents social.conf.
	/// </summary>
	public class SocialConf : ConfFile
	{
		public int RoomMemberMaxCount { get; protected set; }

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

			this.RoomMemberMaxCount = this.GetInt("room_member_max_count", 20);
		}
	}
}
