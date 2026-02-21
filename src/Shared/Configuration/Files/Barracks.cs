using System.IO;
using Melia.Shared.World;
using Yggdrasil.Configuration;
using Yggdrasil.Logging;

namespace Melia.Shared.Configuration.Files
{
	/// <summary>
	/// Represents barracks.conf.
	/// </summary>
	public class BarracksConfFile : ConfFile
	{
		/// <summary>
		/// When enabled, client integrity checks are applied.
		/// </summary>
		public bool VerifyIpf { get; private set; }

		/// <summary>
		/// When enabled, auto-update the ipf checksum.
		/// </summary>
		public bool IpfChecksumAutoUpdate { get; private set; }

		/// <summary>
		/// Checksum to prevent clients using modified or out-dated IPF files.
		/// </summary>
		public string IpfChecksum { get; set; }

		/// <summary>
		/// Map new players start on.
		/// </summary>
		public string StartMap { get; private set; }

		/// <summary>
		/// Position new players start at.
		/// </summary>
		public Position StartPosition { get; private set; }

		/// <summary>
		/// Returns whether account creation via "new__" prefix is enabled.
		/// </summary>
		public bool EnableAccountCreation { get; private set; }

		/// <summary>
		/// How many additional barracks slots players start with.
		/// </summary>
		public int StartAdditionalSlotCount { get; private set; }

		/// <summary>
		/// Delay in milliseconds before closing connections.
		/// Allows clients to receive final packets before disconnect.
		/// </summary>
		public int ConnectionCloseDelay { get; private set; }

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

			this.IpfChecksum = this.GetString("ipf_checksum", "");
			this.VerifyIpf = this.GetBool("ipf_verifying", false);
			this.IpfChecksumAutoUpdate = this.GetBool("ipf_checksum_auto_update", false);

			this.StartMap = this.GetString("start_map", "f_siauliai_west");
			this.StartPosition = this.GetPosition("start_position", new Position(-628, 260, -1025));

			this.EnableAccountCreation = this.GetBool("enable_account_creation", false);
			this.StartAdditionalSlotCount = this.GetInt("additional_slot_count", 0);

			this.ConnectionCloseDelay = this.GetInt("connection_close_delay", 100);
		}

		/// <summary>
		/// Parses the option as a position and returns it.
		/// </summary>
		/// <param name="option"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		private Position GetPosition(string option, Position defaultValue)
		{
			var value = this.GetString(option, "-628, 260, -1025");

			var split = value.Split(',');
			if (split.Length != 3)
			{
				Log.Warning("barracks.conf: Invalid position format for '{0}'.", option);
				return defaultValue;
			}

			if (!int.TryParse(split[0], out var x) || !int.TryParse(split[1], out var y) || !int.TryParse(split[2], out var z))
			{
				Log.Warning("barracks.conf: Invalid coordinates in '{0}'.", option);
				return defaultValue;
			}

			return new Position(x, y, z);
		}
	}
}
