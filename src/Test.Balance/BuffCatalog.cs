using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Melia.Shared.Game.Const;
using Melia.Zone;

namespace Melia.Test.Balance
{
	/// <summary>
	/// A buff or debuff one of the in-scope classes can actually put on
	/// something with a skill.
	/// </summary>
	public class BuffEntry
	{
		public BuffId Id { get; init; }

		/// <summary>
		/// Classes whose skill handlers reference this buff.
		/// </summary>
		public string[] Grantedby { get; init; }

		/// <summary>
		/// Handler type that implements it.
		/// </summary>
		public string Handler { get; init; }

		public string Owner => string.Join("/", this.Grantedby);

		public override string ToString() => $"{this.Id} ({this.Owner})";
	}

	/// <summary>
	/// The buffs and debuffs in scope: the ones the 45 classes grant through
	/// their own skills.
	/// </summary>
	/// <remarks>
	/// Derived by scanning those classes' skill handler sources for BuffId
	/// references, because nothing at runtime links a skill to the buffs it
	/// applies - a handler just calls StartBuff wherever its logic says to.
	/// Monster, GM, item and consumable buffs are therefore excluded by
	/// construction rather than by a name filter.
	/// </remarks>
	public static class BuffCatalog
	{
		/// <summary>
		/// Roots holding per-class handlers, relative to the repo root
		/// RunHeadless navigates to.
		/// </summary>
		/// <remarks>
		/// Pads and buffs are scanned as well as skills: a class can put a
		/// buff on something through a ground effect or from inside another
		/// buff, and scanning skill handlers alone missed those - Thaumaturge's
		/// Swell buffs among them.
		/// </remarks>
		private static readonly string[] HandlerRoots =
		[
			"src/ZoneServer/Skills/Handlers",
			"src/ZoneServer/Buffs/Handlers",
			"src/ZoneServer/Pads/Handlers",
			"src/ZoneServer/Packages/Laima/Skills",
			"src/ZoneServer/Packages/Laima/Buffs",
			"src/ZoneServer/Packages/Laima/Pads",
		];

		private static readonly Regex BuffReference = new(@"BuffId\.(\w+)", RegexOptions.Compiled);

		private static readonly object _buildLock = new();
		private static BuffEntry[] _entries;

		/// <summary>
		/// Buffs the in-scope classes grant, that also have a handler.
		/// </summary>
		public static BuffEntry[] Entries
		{
			get
			{
				lock (_buildLock)
				{
					return _entries ??= Discover();
				}
			}
		}

		/// <summary>
		/// Scans the in-scope classes' skill handlers and collects the buffs
		/// they reference.
		/// </summary>
		private static BuffEntry[] Discover()
		{
			var byBuff = new Dictionary<BuffId, SortedSet<string>>();

			foreach (var root in HandlerRoots)
			{
				if (!Directory.Exists(root))
					continue;

				foreach (var directory in Directory.GetDirectories(root, "*", SearchOption.AllDirectories))
				{
					var className = ResolveClass(Path.GetFileName(directory));

					if (className == null)
						continue;

					foreach (var file in Directory.GetFiles(directory, "*.cs"))
					{
						foreach (Match match in BuffReference.Matches(File.ReadAllText(file)))
						{
							if (!Enum.TryParse<BuffId>(match.Groups[1].Value, out var buffId) || buffId == BuffId.None)
								continue;

							if (!byBuff.TryGetValue(buffId, out var classes))
								byBuff[buffId] = classes = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

							classes.Add(className);
						}
					}
				}
			}

			var entries = new List<BuffEntry>();

			foreach (var pair in byBuff)
			{
				// A buff without a handler does nothing when applied, so
				// measuring it would only produce a row of zeroes.
				if (!ZoneServer.Instance.BuffHandlers.TryGetHandler(pair.Key, out var handler))
					continue;

				entries.Add(new BuffEntry
				{
					Id = pair.Key,
					Grantedby = pair.Value.ToArray(),
					Handler = handler.GetType().Name,
				});
			}

			if (entries.Count == 0)
				throw new InvalidOperationException($"No class buffs found. Are the handler sources present under '{HandlerRoots[0]}'?");

			return entries.OrderBy(e => e.Id.ToString()).ToArray();
		}

		/// <summary>
		/// Returns the in-scope class a handler folder belongs to, or null
		/// if the folder is not one of them.
		/// </summary>
		/// <remarks>
		/// Folder names follow the JobId rather than the skill prefix in
		/// several cases - Swordsman/Swordman, Krivis/Kriwi, Outlaw/OutLaw -
		/// so both spellings are accepted.
		/// </remarks>
		/// <param name="folderName"></param>
		private static string ResolveClass(string folderName)
		{
			foreach (var job in JobCatalog.Entries)
			{
				if (folderName.Equals(job.SkillPrefix, StringComparison.OrdinalIgnoreCase))
					return job.SkillPrefix;

				if (folderName.Equals(job.JobId.ToString(), StringComparison.OrdinalIgnoreCase))
					return job.SkillPrefix;
			}

			return null;
		}
	}
}
