using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Melia.Shared.Game.Const;
using Melia.Test.Balance.Sfr;

namespace Melia.Test.Balance.Buff
{
	/// <summary>
	/// One buff-granting press the pass can price, with everything the model
	/// needs about it.
	/// </summary>
	public class BuffSubject
	{
		/// <summary>
		/// Class name of the skill that grants the buff, which is the row the
		/// pass reads and writes.
		/// </summary>
		public string SkillClassName { get; init; }

		public SkillId SkillId { get; init; }

		/// <summary>
		/// The class the skill belongs to, for the per-class budget.
		/// </summary>
		public string ClassName { get; init; }

		/// <summary>
		/// Buffs the skill's handler can apply, in source order.
		/// </summary>
		public BuffId[] Buffs { get; init; }

		/// <summary>
		/// The caption ratio slots the row declares, each seeded at
		/// BuffDials.ProbeSeed rather than at whatever the row currently
		/// carries.
		/// </summary>
		/// <remarks>
		/// Which slots exist is read from the file - that is what "declares a
		/// caption ratio" means. What magnitude each holds is not: seeding from
		/// the file would make the solver preserve whatever ratio already sat
		/// between two slots instead of solving each on its own, which is
		/// exactly the wrong-crit-vs-block-split failure a seed-dependent pass
		/// cannot see.
		/// </remarks>
		public IReadOnlyDictionary<int, float> Slots { get; init; }

		/// <summary>
		/// The base and per-level terms BuffDials.PinnedRatios holds a slot at,
		/// which the pass writes as they stand but never solves.
		/// </summary>
		/// <remarks>
		/// Kept out of Slots so the solver cannot move them, and installed live
		/// in every window regardless, so what they are worth still counts
		/// against the buff's budget.
		/// </remarks>
		public IReadOnlyDictionary<int, (float Base, float ByLevel)> PinnedSlots { get; init; } = new Dictionary<int, (float, float)>();

		/// <summary>
		/// The magnitude each pinned slot reaches at the skill's own cap, which
		/// is what a window installs it at.
		/// </summary>
		public IReadOnlyDictionary<int, float> PinnedMagnitudes { get; init; } = new Dictionary<int, float>();

		/// <summary>
		/// Stacks the window holds this buff's OverbuffCounter at, or zero to
		/// leave it wherever applying it left it.
		/// </summary>
		public int Stacks { get; init; }

		/// <summary>
		/// The magnitude each declared slot actually holds in the file right
		/// now, at the skill's own cap.
		/// </summary>
		/// <remarks>
		/// Not read by the pricer - Slots is what that solves from. This is for
		/// the diagnostics that ask what the roster currently does: BuffValueTests
		/// measures a row at the magnitude it is actually live at, and a pricing
		/// pass would defeat the point of asking that by substituting its own
		/// seed instead.
		/// </remarks>
		public IReadOnlyDictionary<int, float> WrittenMagnitudes { get; init; }

		/// <summary>
		/// Whether the press puts its buff on the caster's party rather than
		/// only on the caster.
		/// </summary>
		public bool IsPartyWide { get; init; }

		/// <summary>
		/// The skill's level cap on a fully advanced job: 5 for a base job,
		/// and 15 / 10 / 5 by circle for an advanced one.
		/// </summary>
		public int MaxLevel { get; init; }

		/// <summary>
		/// Seconds the buff lasts, or zero when it is a toggle that never
		/// expires on its own.
		/// </summary>
		public float DurationSeconds { get; init; }

		/// <summary>
		/// Seconds between presses, from the skill's own cooldown and
		/// overheat.
		/// </summary>
		public float CycleSeconds { get; init; }

		/// <summary>
		/// The share of the rotation the buff is actually up for.
		/// </summary>
		/// <remarks>
		/// A toggle and a buff that outlasts its own cooldown are both simply
		/// permanent, which is the case the price has to be hardest on.
		/// </remarks>
		public float Uptime
			=> this.DurationSeconds <= 0 || this.CycleSeconds <= 0
				? 1f
				: Math.Min(1f, this.DurationSeconds / this.CycleSeconds);

		public bool IsAnchor => this.SkillClassName == BuffDials.AnchorSkill;

		public override string ToString()
			=> $"{this.SkillClassName} [{string.Join(", ", this.Buffs)}] " +
			   $"slots {string.Join("/", this.Slots.OrderBy(s => s.Key).Select(s => $"{s.Key}:{s.Value:0.##}"))} " +
			   $"cap {this.MaxLevel} uptime {this.Uptime:0.00}";
	}

	/// <summary>
	/// Which buff-granting presses are in scope, and what the model knows
	/// about each without measuring anything.
	/// </summary>
	/// <remarks>
	/// Scope is "the row declares at least one caption ratio". That is the
	/// declaration a handler makes when it is converted to read its magnitudes
	/// from data, so scope grows exactly as the conversion does and nothing
	/// needs a second list to be kept in step. A handler still carrying its
	/// constants is invisible here rather than mispriced.
	/// </remarks>
	public static class BuffScope
	{
		/// <summary>
		/// Roots holding the skill handlers whose sources name the buffs they
		/// apply.
		/// </summary>
		private static readonly string[] SkillHandlerRoots =
		[
			"src/ZoneServer/Skills/Handlers",
			"src/ZoneServer/Packages/Laima/Skills",
		];

		/// <summary>
		/// Roots holding the pad handlers, whose sources name the buffs a
		/// skill applies through the ground rather than directly.
		/// </summary>
		private static readonly string[] PadHandlerRoots =
		[
			"src/ZoneServer/Pads/Handlers",
			"src/ZoneServer/Packages/Laima/Pads",
		];

		private static readonly Regex SkillHandlerAttribute = new(@"\[SkillHandler\(SkillId\.(\w+)\)\]", RegexOptions.Compiled);

		private static readonly Regex PadHandlerAttribute = new(@"\[PadHandler\(PadName\.(\w+)\)\]", RegexOptions.Compiled);

		/// <summary>
		/// A pad the skill's handler creates, whose own handler is then read
		/// for the buffs the press really lands.
		/// </summary>
		private static readonly Regex PadReference = new(@"PadName\.(\w+)", RegexOptions.Compiled);

		/// <summary>
		/// A buff a pad handler applies, through any of PadHelper's apply
		/// helpers or a direct StartBuff.
		/// </summary>
		/// <remarks>
		/// The helper's own name is captured alongside the buff so the remove
		/// helpers can be dropped: PadTargetBuffRemove and PadRemoveBuff both
		/// name the buff they take back off, and reading either as an
		/// application makes every pad look like it grants what it cleans up.
		/// </remarks>
		private static readonly Regex PadBuffApplication = new(@"\b(Pad\w*Buff\w*|StartBuff\w*)\([^;]*?BuffId\.(\w+)", RegexOptions.Compiled);

		/// <summary>
		/// A buff the handler actually applies, rather than one it merely
		/// names.
		/// </summary>
		/// <remarks>
		/// Matching a bare BuffId reference made every handler that checks for
		/// a buff, stops one or excludes one look like its owner.
		/// SwashBuckling_Debuff read as "granted by six Peltasta skills" and was
		/// filed as unpriceable on that basis, when only Peltasta_SwashBuckling
		/// starts it and the other five are the base handlers it overrides.
		/// </remarks>
		private static readonly Regex BuffApplication = new(@"StartBuff\w*\(\s*BuffId\.(\w+)", RegexOptions.Compiled);

		/// <summary>
		/// A press that puts its buff on the caster's party rather than only on
		/// the caster.
		/// </summary>
		/// <remarks>
		/// Scanned rather than measured, and it is the one thing here that has
		/// to be: the harness has no second connected character, so
		/// Connection.Party is null and a party press applies to the caster
		/// alone however it is dispatched. Without this a party buff and a self
		/// buff are indistinguishable, and Priest_Blessing prices as though it
		/// buffed one person.
		/// </remarks>
		private static readonly Regex PartyApplication = new(@"member\.StartBuff|foreach\s*\(\s*var\s+member", RegexOptions.Compiled);

		private static readonly object _syncLock = new();
		private static BuffSubject[] _subjects;
		private static Dictionary<string, BuffId[]> _grants;
		private static HashSet<string> _partyWide;

		/// <summary>
		/// Every buff-granting press the pass can price.
		/// </summary>
		public static BuffSubject[] Subjects
		{
			get
			{
				lock (_syncLock)
					return _subjects ??= Discover();
			}
		}

		/// <summary>
		/// Returns the subject for one skill, or null when it declares no
		/// caption ratios.
		/// </summary>
		/// <param name="skillClassName"></param>
		/// <summary>
		/// Narrows a subject list to what a comma-separated filter names, or
		/// returns it whole when the filter is empty.
		/// </summary>
		/// <remarks>
		/// A list rather than a single name, so fixing one skill does not mean
		/// re-pricing the roster and a related handful can be checked together.
		/// The anchor is measured regardless of what is named here - it sets
		/// every other buff's budget, so a filtered run is still calibrated the
		/// same way a full one is.
		/// </remarks>
		/// <param name="subjects"></param>
		/// <param name="only"></param>
		public static BuffSubject[] Filter(BuffSubject[] subjects, string only)
		{
			if (string.IsNullOrWhiteSpace(only))
				return subjects;

			var names = only
				.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
				.ToHashSet(StringComparer.OrdinalIgnoreCase);

			var filtered = subjects.Where(s => names.Contains(s.SkillClassName)).ToArray();

			if (filtered.Length == 0)
				throw new ArgumentException($"No buff in scope matches '{only}'.", nameof(only));

			return filtered;
		}

		public static BuffSubject Find(string skillClassName)
			=> Subjects.FirstOrDefault(s => s.SkillClassName.Equals(skillClassName, StringComparison.OrdinalIgnoreCase));

		/// <summary>
		/// Builds the scope from the skill data and the handler sources.
		/// </summary>
		private static BuffSubject[] Discover()
		{
			var grants = Grants();
			var subjects = new List<BuffSubject>();

			foreach (var (skillName, entry) in SfrData.Skills)
			{
				var maxLevel = SfrData.SkillMaxLevel(skillName);
				var slots = new Dictionary<int, float>();
				var pinned = new Dictionary<int, (float Base, float ByLevel)>();
				var pinnedMagnitudes = new Dictionary<int, float>();
				var written = new Dictionary<int, float>();
				var held = BuffDials.PinnedRatios.GetValueOrDefault(skillName);

				for (var slot = 1; slot <= 3; ++slot)
				{
					// A pinned slot's magnitude comes from the dial, not the
					// row, so the pin holds whatever the file was left carrying.
					if (held != null && held.TryGetValue(slot, out var pin))
					{
						pinned[slot] = pin;
						pinnedMagnitudes[slot] = pin.Base + pin.ByLevel * maxLevel;
						written[slot] = pinnedMagnitudes[slot];
						continue;
					}

					var baseValue = entry.Num($"captionRatio{slot}", 0);
					var byLevel = entry.Num($"captionRatio{slot}ByLevel", 0);

					// Whether the row declares this slot is read from the file -
					// that is what scope means. What the file's magnitude on it
					// currently is is not: every declared slot solves from the
					// same arbitrary seed, never from what it already carries.
					// WrittenMagnitudes keeps the real number alongside it, for
					// the diagnostics that ask what is actually live rather than
					// what the next pass should price.
					if (baseValue == 0 && byLevel == 0)
						continue;

					slots[slot] = BuffDials.ProbeSeed;
					written[slot] = baseValue + byLevel * maxLevel;
				}

				if (slots.Count == 0)
					continue;

				if (!Enum.TryParse<SkillId>(skillName, out var skillId))
					continue;

				subjects.Add(new BuffSubject
				{
					SkillClassName = skillName,
					SkillId = skillId,
					ClassName = SfrData.ClassOf(skillName),
					Buffs = grants.GetValueOrDefault(skillName, []),
					IsPartyWide = PartyWide().Contains(skillName),
					Slots = slots,
					PinnedSlots = pinned,
					PinnedMagnitudes = pinnedMagnitudes,
					Stacks = BuffDials.StackCounts.GetValueOrDefault(skillName, 0),
					WrittenMagnitudes = written,
					MaxLevel = maxLevel,
					DurationSeconds = entry.Num("captionTime", 0) + entry.Num("captionTimeByLevel", 0) * maxLevel,
					CycleSeconds = CycleSeconds(entry),
				});
			}

			return subjects.OrderBy(s => s.SkillClassName, StringComparer.Ordinal).ToArray();
		}

		/// <summary>
		/// Returns the seconds between presses, from the skill's own cooldown
		/// and overheat charges.
		/// </summary>
		/// <param name="entry"></param>
		private static float CycleSeconds(SkillEntryData entry)
		{
			var cooldown = entry.Num("cooldownTime", 0) / 1000f;
			var charges = Math.Max(1f, entry.Num("overheatCount", 0));

			return cooldown / charges;
		}

		/// <summary>
		/// Returns the skills whose press puts its buff on the whole party.
		/// </summary>
		private static HashSet<string> PartyWide()
		{
			lock (_syncLock)
			{
				if (_partyWide != null)
					return _partyWide;

				_partyWide = [];

				foreach (var root in SkillHandlerRoots)
				{
					var path = Path.Combine(SfrData.Root, root);

					if (!Directory.Exists(path))
						continue;

					foreach (var file in Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories))
					{
						var text = File.ReadAllText(file);
						var handler = SkillHandlerAttribute.Match(text);

						if (handler.Success && PartyApplication.IsMatch(text))
							_partyWide.Add(handler.Groups[1].Value);
					}
				}

				return _partyWide;
			}
		}

		/// <summary>
		/// Maps each skill class name to the buffs its handler references.
		/// </summary>
		/// <remarks>
		/// Scanned rather than measured, and deliberately so: this is only the
		/// fallback list for a press the probe cannot dispatch. A press that
		/// dispatches has its buffs read off the character afterwards, which
		/// is authoritative and needs no source at all.
		/// </remarks>
		private static Dictionary<string, BuffId[]> Grants()
		{
			lock (_syncLock)
			{
				if (_grants != null)
					return _grants;

				_grants = [];

				var padGrants = PadGrants();

				foreach (var root in SkillHandlerRoots)
				{
					var path = Path.Combine(SfrData.Root, root);

					if (!Directory.Exists(path))
						continue;

					foreach (var file in Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories))
					{
						var text = File.ReadAllText(file);
						var handler = SkillHandlerAttribute.Match(text);

						if (!handler.Success)
							continue;

						var buffs = BuffApplication.Matches(text)
							.Select(m => Enum.TryParse<BuffId>(m.Groups[1].Value, out var id) ? id : BuffId.None)
							.Where(id => id != BuffId.None)
							.Union(PadReference.Matches(text)
								.SelectMany(m => padGrants.GetValueOrDefault(m.Groups[1].Value, [])))
							.Distinct()
							.ToArray();

						if (buffs.Length == 0)
							continue;

						var skillName = handler.Groups[1].Value;

						// A Laima override and the handler it replaces both
						// match, and both name the same buffs, so the union is
						// what a press can apply either way.
						_grants[skillName] = _grants.TryGetValue(skillName, out var existing)
							? existing.Union(buffs).ToArray()
							: buffs;
					}
				}

				return _grants;
			}
		}

		/// <summary>
		/// Maps each pad name to the buffs its handler applies.
		/// </summary>
		/// <remarks>
		/// A press that buffs through the ground names no buff of its own -
		/// its handler creates a pad and the pad's handler is what applies
		/// anything. Without this every such press reads as granting nothing
		/// and cannot be priced at all, which is what held Foretell and every
		/// other pad buff out of the pass.
		/// </remarks>
		private static Dictionary<string, BuffId[]> PadGrants()
		{
			var grants = new Dictionary<string, BuffId[]>();

			foreach (var root in PadHandlerRoots)
			{
				var path = Path.Combine(SfrData.Root, root);

				if (!Directory.Exists(path))
					continue;

				foreach (var file in Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories))
				{
					var text = File.ReadAllText(file);
					var handler = PadHandlerAttribute.Match(text);

					if (!handler.Success)
						continue;

					var buffs = PadBuffApplication.Matches(text)
						.Where(m => !m.Groups[1].Value.Contains("Remove"))
						.Select(m => Enum.TryParse<BuffId>(m.Groups[2].Value, out var id) ? id : BuffId.None)
						.Where(id => id != BuffId.None)
						.Distinct()
						.ToArray();

					if (buffs.Length == 0)
						continue;

					var padName = handler.Groups[1].Value;

					grants[padName] = grants.TryGetValue(padName, out var existing)
						? existing.Union(buffs).ToArray()
						: buffs;
				}
			}

			return grants;
		}
	}
}
