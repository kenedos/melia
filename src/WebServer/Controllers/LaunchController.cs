using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using EmbedIO;
using EmbedIO.Routing;
using Melia.Shared.Data.Database;
using Melia.Shared.Network.Crypto;
using Melia.Web.Const;
using Melia.Web.Util;
using Yggdrasil.Logging;

namespace Melia.Web.Controllers
{
	/// <summary>
	/// Controller for serving game launcher files, patch information, and server lists.
	/// </summary>
	public class LaunchController : BaseController
	{
		private readonly Codec _codec = new();
		private readonly List<string> _unencryptedPatchFiles = new()
		{
			"partial/data.file.list.txt",
			"updater/updater.list.txt",
		};

		/// <summary>
		/// Helper class to store patch information, used by GetPatchList.
		/// </summary>
		public class PatchInfo
		{
			public bool IsExecutable { get; set; }
			public string Name { get; set; }
			public string Hash { get; set; }
		}

		/// <summary>
		/// Serves a list of server groups in XML format, with barrack servers
		/// that the client may connect to upon login.
		/// </summary>
		/// <remarks>
		/// The list is based on the server groups defined in the server data.
		/// Every group is essentially a collection of primarily barracks and
		/// zone servers, with the former serving as the entry points. Picture
		/// these groups as independent worlds, like for different regions.
		/// </remarks>
		/// <example>
		///   <serverlist>
		///       <server GROUP_ID="1001" TRAFFIC="0" ENTER_LIMIT="100" NAME="Melia" Server0_IP="127.0.0.1" Server0_Port="2000"/>
		///   </serverlist>
		/// </example>
		[Route(HttpVerbs.Get, "/serverlist.xml")]
		public async Task GetServerList()
		{
			var serverGroupDataList = WebServer.Instance.Data.ServerDb.Entries.Values.OrderBy(a => a.Id);

			using (var str = new Utf8StringWriter())
			using (var xml = new XmlTextWriter(str))
			{
				xml.Formatting = Formatting.Indented;

				xml.WriteStartDocument();
				xml.WriteWhitespace("\n");
				xml.WriteStartElement("serverlist");
				xml.WriteWhitespace("\n");

				foreach (var groupData in serverGroupDataList)
				{
					xml.WriteWhitespace("\t");
					xml.WriteStartElement("server");
					xml.WriteAttributeString("GROUP_ID", groupData.Id.ToString());
					xml.WriteAttributeString("TRAFFIC", "0");
					xml.WriteAttributeString("ENTER_LIMIT", "100");
					xml.WriteAttributeString("NAME", groupData.Name);

					var barracksServersData = groupData.Servers.Where(a => a.Type == ServerType.Barracks).ToArray();
					for (var i = 0; i < barracksServersData.Length; ++i)
					{
						var serverData = barracksServersData[i];
						xml.WriteAttributeString($"Server{i}_IP", serverData.Ip);
						xml.WriteAttributeString($"Server{i}_Port", serverData.Port.ToString());
					}

					xml.WriteEndElement();
					xml.WriteWhitespace("\n");
				}

				xml.WriteEndElement();
				xml.WriteWhitespace("\n");
				xml.WriteEndDocument();

				await this.SendText(MimeTypes.Application.Xml, str.ToString());
			}
		}

		/// <summary>
		/// Serves a config file that determines, among other things,
		/// whether to enable HacksShield and Steam logins.
		/// </summary>
		[Route(HttpVerbs.Get, "/static__Config.txt")]
		public async Task GetStaticConfig()
		{
			var staticConfigFile = "user/web/toslive/patch/static__Config.txt";
			if (!File.Exists(staticConfigFile))
				staticConfigFile = "system/web/toslive/patch/static__Config.txt";

			if (!File.Exists(staticConfigFile))
			{
				throw HttpException.NotFound("static__Config.txt not found.");
			}

			await this.SendText(MimeTypes.Text.Plain, await File.ReadAllTextAsync(staticConfigFile));
		}

		/// <summary>
		/// Serves the client version file.
		/// </summary>
		[Route(HttpVerbs.Get, "/version.txt")]
		public async Task GetPatcherVersion()
		{
			var patcherVersionFile = "user/web/toslive/patch/version.txt";
			if (!File.Exists(patcherVersionFile))
				patcherVersionFile = "system/web/toslive/patch/version.txt";

			if (!File.Exists(patcherVersionFile))
			{
				throw HttpException.NotFound("version.txt not found.");
			}

			await this.SendText(MimeTypes.Text.Plain, await File.ReadAllTextAsync(patcherVersionFile));
		}

		/// <summary>
		/// Serves a list of patches in XML format.
		/// </summary>
		[Route(HttpVerbs.Get, "/patchlist.xml")]
		public async Task GetPatchList()
		{
			var patchFolder = @"user/web/toslive/patch/";
			var xmlFilePath = Path.Combine(patchFolder, "patchlist.xml");

			var existingHashes = File.Exists(xmlFilePath)
				? FileUtils.LoadHashesFromXml(xmlFilePath)
				: new List<PatchInfo>();

			var patchFiles = Directory.Exists(patchFolder)
				? Directory.GetFiles(patchFolder, "*.ipf")
				: Array.Empty<string>();

			using (var str = new Utf8StringWriter())
			using (var xml = new XmlTextWriter(str))
			{
				xml.Formatting = Formatting.Indented;
				xml.WriteStartDocument();
				xml.WriteStartElement("patchlist");

				foreach (var patch in existingHashes)
				{
					xml.WriteStartElement(patch.IsExecutable ? "client" : "prepatch");
					xml.WriteAttributeString("name", patch.Name);
					xml.WriteAttributeString("hash", patch.Hash);
					xml.WriteEndElement();
				}

				foreach (var patchFile in patchFiles)
				{
					var hash = FileUtils.CalculateSHA256(patchFile);
					xml.WriteStartElement("patch");
					xml.WriteAttributeString("name", Path.GetFileName(patchFile));
					xml.WriteAttributeString("hash", hash);
					xml.WriteEndElement();
				}

				xml.WriteEndElement();
				xml.WriteEndDocument();

				await this.SendText(MimeTypes.Application.Xml, str.ToString());
			}
		}

		[Route(HttpVerbs.Get, "/updater/patcher.version.txt")]
		public async Task GetPatcherVersionR1()
		{
			var patcherVersion = new StringBuilder();
			patcherVersion.AppendLine("0    ");
			patcherVersion.AppendLine("");
			var result = Encoding.UTF8.GetBytes(patcherVersion.ToString());
			result = _codec.EncryptFile(result);

			await this.SendText(MimeTypes.Text.Plain, Encoding.Default.GetString(result));
		}

		[Route(HttpVerbs.Get, "/updater/patcher.version_2.txt")]
		public async Task GetPatcherV2()
		{
			await this.SendText(MimeTypes.Text.Plain, "0");
		}

		[Route(HttpVerbs.Get, "/updater/updater.list.txt")]
		public async Task GetUpdaterList()
		{
			var filePath = "user/web/toslive/patch/updater/updater.list.txt";
			if (!File.Exists(filePath))
				filePath = "system/web/toslive/patch/updater/updater.list.txt";

			byte[] result;
			if (File.Exists(filePath))
			{
				result = await File.ReadAllBytesAsync(filePath);
			}
			else
			{
				var updaterList = new StringBuilder();
				updaterList.AppendLine("tos.exe");
				updaterList.AppendLine("patch.ini");
				updaterList.AppendLine("");
				result = Encoding.Default.GetBytes(updaterList.ToString());
			}

			this.SendBinary(MimeTypes.Text.Plain, result);
		}

		[Route(HttpVerbs.Get, "/{type}/{file}")]
		public async Task GetPatcherFile(string type, string file)
		{
			try
			{
				var relativePath = $"{type}/{file}";
				var filePath = $"user/web/toslive/patch/{relativePath}";
				if (!File.Exists(filePath))
					filePath = $"system/web/toslive/patch/{relativePath}";

				if (File.Exists(filePath))
				{
					var fileBytes = await File.ReadAllBytesAsync(filePath);

					if (!_unencryptedPatchFiles.Contains(relativePath))
						fileBytes = _codec.EncryptFile(fileBytes);

					this.SendBinary(MimeTypes.Text.Plain, fileBytes);
				}
				else
				{
					throw HttpException.NotFound($"File not found: {relativePath}");
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				throw HttpException.NotFound();
			}
		}

		[Route(HttpVerbs.Get, "/{file}")]
		public async Task GetPatchFile(string file)
		{
			if (file.Contains('/') || file.Contains('\\') || file.Contains(".."))
			{
				throw HttpException.BadRequest("Invalid file name.");
			}

			try
			{
				var filePath = $"user/web/toslive/patch/{file}";
				if (!File.Exists(filePath))
					filePath = $"system/web/toslive/patch/{file}";

				if (File.Exists(filePath))
				{
					var patchFile = await File.ReadAllBytesAsync(filePath);
					this.SendBinary(MimeTypes.Application.Ipf, patchFile);
				}
				else
				{
					throw HttpException.NotFound($"File not found: {file}");
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				throw HttpException.NotFound();
			}
		}
	}
}
