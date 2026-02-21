using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Yggdrasil.Configuration;

namespace Melia.Shared.Configuration.Files
{
	/// <summary>
	/// Represents web.conf.
	/// </summary>
	public class WebConfFile : ConfFile
	{
		/// <summary>
		/// Returns true if accounts can be created via the API.
		/// </summary>
		public bool EnableApiAccountCreation { get; private set; }

		/// <summary>
		/// Returns a list of configured CGI processors.
		/// </summary>
		public List<CgiProcessor> CgiProcessors { get; private set; }

		public string PhpCgiFilePath { get; protected set; }
		public int GuildPort { get; private set; }
		public int MarketPort { get; private set; }

		public bool EnableEmailService { get; set; }
		public string SmtpHost { get; set; }
		public int SmtpPort { get; set; }
		public string SmtpUsername { get; set; }
		public string SmtpPassword { get; set; }
		public bool SmtpUseSsl { get; set; }
		public string SmtpSenderEmail { get; set; }
		public string SmtpSenderName { get; set; }
		public string SmtpBaseUrl { get; set; }

		/// <summary>
		/// Returns whether the admin panel is enabled.
		/// </summary>
		public bool AdminPanelEnabled { get; private set; }

		/// <summary>
		/// Returns the minimum authority level required to access the admin panel.
		/// </summary>
		public int AdminMinAuthority { get; private set; }

		/// <summary>
		/// Returns the admin session timeout in seconds.
		/// </summary>
		public int AdminSessionTimeout { get; private set; }

		/// <summary>
		/// Returns whether the player control panel is enabled.
		/// </summary>
		public bool CpEnabled { get; private set; }

		/// <summary>
		/// Returns the control panel session timeout in seconds.
		/// </summary>
		public int CpSessionTimeout { get; private set; }

		/// <summary>
		/// Returns the unstuck feature cooldown in seconds.
		/// </summary>
		public int CpUnstuckCooldown { get; private set; }

		/// <summary>
		/// Returns the unstuck safe map class name.
		/// </summary>
		public string CpUnstuckMap { get; private set; }

		/// <summary>
		/// Returns the unstuck safe X coordinate.
		/// </summary>
		public float CpUnstuckX { get; private set; }

		/// <summary>
		/// Returns the unstuck safe Y coordinate.
		/// </summary>
		public float CpUnstuckY { get; private set; }

		/// <summary>
		/// Returns the unstuck safe Z coordinate.
		/// </summary>
		public float CpUnstuckZ { get; private set; }

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

			this.EnableApiAccountCreation = this.GetBool("enable_api_account_creation", false);
			this.CgiProcessors = this.GetCgiProcessors();
			this.GuildPort = this.GetInt("guild_port", 9004);
			this.MarketPort = this.GetInt("market_port", 9005);

			this.EnableEmailService = this.GetBool("enable_smtp", false);
			this.SmtpHost = this.GetString("smtp_host", "");
			this.SmtpPort = this.GetInt("smtp_port", 587);
			this.SmtpUsername = this.GetString("smtp_username", "");
			this.SmtpPassword = this.GetString("smtp_password", "");
			this.SmtpUseSsl = this.GetBool("smtp_use_ssl", true);
			this.SmtpSenderEmail = this.GetString("smtp_sender_email", "noreply@example.com");
			this.SmtpSenderName = this.GetString("smtp_sender_name", "Melia Server");
			this.SmtpBaseUrl = this.GetString("smtp_base_url", "http://localhost");

			this.PhpCgiFilePath = this.GetString("php_cgi_bin", Path.Combine("user", "tools", "php", "php-cgi.exe"));

			// Admin Panel
			this.AdminPanelEnabled = this.GetBool("admin_panel_enabled", true);
			this.AdminMinAuthority = this.GetInt("admin_min_authority", 50);
			this.AdminSessionTimeout = this.GetInt("admin_session_timeout", 3600);

			// Player Control Panel
			this.CpEnabled = this.GetBool("cp_enabled", true);
			this.CpSessionTimeout = this.GetInt("cp_session_timeout", 3600);
			this.CpUnstuckCooldown = this.GetInt("cp_unstuck_cooldown", 300);
			this.CpUnstuckMap = this.GetString("cp_unstuck_map", "c_Klaipe");
			this.CpUnstuckX = this.GetFloat("cp_unstuck_x", -430f);
			this.CpUnstuckY = this.GetFloat("cp_unstuck_y", 261f);
			this.CpUnstuckZ = this.GetFloat("cp_unstuck_z", -650f);
		}

		/// <summary>
		/// Returns a list of CGI processors based on the configuration options.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		private List<CgiProcessor> GetCgiProcessors()
		{
			var result = new List<CgiProcessor>();

			foreach (var option in _options)
			{
				var optionName = option.Key.ToLowerInvariant();
				if (!optionName.StartsWith("cgi_processor_"))
					continue;

				var split = option.Value.ToString().Split(';', StringSplitOptions.RemoveEmptyEntries);
				if (split.Length != 3)
					throw new InvalidOperationException($"Invalid CGI processor configuration: {option.Value}");

				var name = split[0].Trim();
				var exts = split[1].Trim().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(ext => ext.Trim()).ToArray();
				var path = split[2].Trim();

				var processor = new CgiProcessor(name, path, exts);
				result.Add(processor);
			}

			return result;
		}

		/// <summary>
		/// Represents a CGI processor configuration.
		/// </summary>
		public class CgiProcessor
		{
			/// <summary>
			/// Returns the name of the processor, which is primarily used for
			/// logging and debugging purposes.
			/// </summary>
			public string Name { get; }

			/// <summary>
			/// Returns the path to the processor executable.
			/// </summary>
			public string Path { get; }

			/// <summary>
			/// Returns the file extensions that the processor can handle.
			/// </summary>
			public string[] FileExtensions { get; }

			/// <summary>
			/// Creates new instance.
			/// </summary>
			/// <param name="name"></param>
			/// <param name="path"></param>
			/// <param name="fileExtensions"></param>
			public CgiProcessor(string name, string path, string[] fileExtensions)
			{
				this.Name = name;
				this.Path = path;
				this.FileExtensions = fileExtensions;
			}
		}
	}
}
