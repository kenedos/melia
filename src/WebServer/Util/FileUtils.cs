using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using static Melia.Web.Controllers.LaunchController;

namespace Melia.Web.Util
{
	/// <summary>
	/// Utility class for file operations.
	/// </summary>
	public static class FileUtils
	{
		/// <summary>
		/// Extracts the patch number from the filename (portion before
		/// the first underscore).
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string ExtractPatchNumber(string fileName)
		{
			var underscoreIndex = fileName.IndexOf('_');
			if (underscoreIndex > 0 && underscoreIndex < fileName.Length - 1)
			{
				return fileName.Substring(0, underscoreIndex);
			}
			return null;
		}

		/// <summary>
		/// Loads existing patch hashes from a patchlist XML file.
		/// </summary>
		/// <param name="xmlFilePath"></param>
		/// <returns></returns>
		public static List<PatchInfo> LoadHashesFromXml(string xmlFilePath)
		{
			List<PatchInfo> patchList = [];

			if (File.Exists(xmlFilePath))
			{
				var doc = new XmlDocument();
				doc.Load(xmlFilePath);

				foreach (XmlNode node in doc.SelectNodes("//patch"))
				{
					var name = node.Attributes["name"].Value;
					var hash = node.Attributes["hash"].Value;
					patchList.Add(new PatchInfo { Name = name, Hash = hash });
				}

				foreach (XmlNode node in doc.SelectNodes("//client"))
				{
					var name = node.Attributes["name"].Value;
					var hash = node.Attributes["hash"].Value;
					patchList.Add(new PatchInfo { Name = name, Hash = hash, IsExecutable = true });
				}
			}

			return patchList;
		}

		/// <summary>
		/// Computes SHA-256 hash of the file at the given path.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static string CalculateSHA256(string filePath)
		{
			using (var sha256 = SHA256.Create())
			{
				using (var stream = File.OpenRead(filePath))
				{
					var hashBytes = sha256.ComputeHash(stream);
					return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
				}
			}
		}

		/// <summary>
		/// Computes MD5 hash of the given byte array.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string GetMD5Hash(byte[] input)
		{
			var md5 = MD5.Create();
			var array = md5.ComputeHash(input);
			var stringBuilder = new StringBuilder();

			for (var i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Reads a file and returns its contents as a byte array.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static byte[] FileToByteArray(string fileName)
		{
			byte[] fileData = null;

			using (var fs = File.OpenRead(fileName))
			{
				using (var binaryReader = new BinaryReader(fs))
				{
					fileData = binaryReader.ReadBytes((int)fs.Length);
				}
			}
			return fileData;
		}
	}
}
