using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Melia.Test.Balance.Sfr;

namespace Melia.Test.Balance.Buff
{
	/// <summary>
	/// One buff's priced magnitudes, with every term behind them.
	/// </summary>
	public class BuffPrice
	{
		public string SkillClassName { get; init; }
		public string ClassName { get; init; }
		public int Circle { get; init; }
		public int MaxLevel { get; init; }
		public float Uptime { get; init; }

		/// <summary>
		/// The buff's authored duration, or zero when the row carries no
		/// captionTime and the uptime above is a default rather than a
		/// reading.
		/// </summary>
		public float DurationSeconds { get; init; }

		public float CirclePremium { get; init; }

		/// <summary>
		/// What this buff's own exception in BuffDials.Premiums allowed it,
		/// where 1 is the roster's rule.
		/// </summary>
		public float SkillPremium { get; init; }

		/// <summary>
		/// The rotation contribution the buff is priced onto, in the units
		/// BALANCE.md's u*(e-1) is in.
		/// </summary>
		public float TargetContribution { get; init; }

		/// <summary>
		/// What the buff has to be worth while it is up to land on that
		/// contribution at its own uptime.
		/// </summary>
		public float TargetValue { get; init; }

		/// <summary>
		/// The common scale the solver put every declared slot at, where 1 is
		/// what the data currently carries.
		/// </summary>
		public float SlotScale { get; init; }

		/// <summary>
		/// What the buff measured at that scale.
		/// </summary>
		public float Value { get; init; }

		public float GainOffense { get; init; }
		public float GainDefense { get; init; }

		/// <summary>
		/// What the buff contributes at that scale, which is the target when
		/// the solver converged.
		/// </summary>
		public float Contribution => this.Uptime * (this.Value - 1f);

		/// <summary>
		/// What the buff measured at each character level of the sweep, which
		/// is where a flat bonus shows its decay.
		/// </summary>
		public IReadOnlyDictionary<int, float> LevelValues { get; init; }

		/// <summary>
		/// What the buff measured under each scenario, which is where a party
		/// buff shows what it is really worth.
		/// </summary>
		public IReadOnlyDictionary<string, float> ScenarioValues { get; init; }

		/// <summary>
		/// Measurements the solve took, the first two of which are the pair
		/// the power law is fitted through.
		/// </summary>
		public int Measurements { get; init; }

		public bool Converged { get; init; }

		/// <summary>
		/// Whether the press put its effect on the enemy rather than on the
		/// caster.
		/// </summary>
		public bool OnEnemy { get; init; }

		/// <summary>
		/// The values to write, by slot: the base the row carries and the
		/// per-level term the growth rule derives from it.
		/// </summary>
		public IReadOnlyDictionary<int, (float Base, float ByLevel)> Slots { get; init; }

		/// <summary>
		/// How far the written magnitudes move what the buff currently
		/// carries.
		/// </summary>
		public float Ratio { get; init; }

		public override string ToString()
			=> $"{this.SkillClassName} x{this.SlotScale:0.000} -> value {this.Value:0.000} " +
			   $"(target {this.TargetValue:0.000}) " +
			   string.Join(" ", this.Slots.OrderBy(s => s.Key).Select(s => $"r{s.Key} {s.Value.Base:0.##}+{s.Value.ByLevel:0.##}/lv"));
	}

	/// <summary>
	/// Everything one pricing pass produced.
	/// </summary>
	public class BuffApplyResult
	{
		/// <summary>
		/// The buff the roster is calibrated onto, priced by construction
		/// rather than solved.
		/// </summary>
		public BuffPrice Anchor { get; set; }

		/// <summary>
		/// What the anchor is worth per rotation at its pinned ratio, which is
		/// every other buff's target before its circle premium.
		/// </summary>
		public float AnchorContribution { get; set; }

		public List<BuffPrice> Prices { get; } = [];

		/// <summary>
		/// Buffs the pass measured but refused to price, with the reason. They
		/// keep whatever the file already carries.
		/// </summary>
		public List<(string Skill, string Reason)> NotPriced { get; } = [];

		public bool Written { get; set; }
	}

	/// <summary>
	/// One scale's readings across every character level the buff's class can
	/// be measured at.
	/// </summary>
	public class BuffLevelSweep
	{
		public IReadOnlyDictionary<int, BuffValueResult> Readings { get; init; }

		/// <summary>
		/// What the buff was worth under each scenario, blended into Value by
		/// the scenarios' own weights.
		/// </summary>
		public IReadOnlyDictionary<string, float> Scenarios { get; init; }

		/// <summary>
		/// The scenarios' values, blended, which is what the buff is priced on.
		/// </summary>
		public float Value { get; init; }

		public float GainOffense { get; init; }
		public float GainDefense { get; init; }
		public bool OnEnemy { get; init; }

		/// <summary>
		/// What the buff was worth at each level.
		/// </summary>
		public IReadOnlyDictionary<int, float> PerLevel
			=> this.Readings.ToDictionary(r => r.Key, r => r.Value.Value);
	}

	/// <summary>
	/// Solves each buff's magnitudes onto the rotation budget, and writes them
	/// back to the skill row that owns them.
	/// </summary>
	/// <remarks>
	/// The damage model's equivalent is SfrPricer, and every rule it follows
	/// carries over. Nothing prices without a live measurement, the pass is
	/// idempotent - no deadband, no smoothing, no value carried forward - and
	/// the whole roster hangs off one anchor, so a change to a dial moves the
	/// spread between buffs and never the roster's level.
	///
	/// What is solved rather than computed is the common scale on a buff's
	/// caption ratios. The pricer never learns what a slot means: a percentage
	/// stays a percentage and a flat bonus stays flat, and only their common
	/// magnitude moves. That is what makes a buff with three unrelated axes
	/// tunable by one number.
	/// </remarks>
	public static class BuffPricer
	{
		private static readonly object _syncLock = new();
		private static float? _anchorContribution;

		/// <summary>
		/// What the anchor is worth per rotation at its pinned ratio, once it
		/// has been measured.
		/// </summary>
		public static float? AnchorContribution
		{
			get
			{
				lock (_syncLock)
					return _anchorContribution;
			}
		}

		/// <summary>
		/// Registers the anchor's reading, which is the scale every other buff
		/// is priced against.
		/// </summary>
		/// <remarks>
		/// The anchor is priced without its defensive half, for the same reason
		/// SfrPricer zeroes Bash's rider: a wobble in the anchor is not one
		/// buff's price, it is a scale on all of them, and Swordman_GungHo
		/// grants no defense to read in the first place.
		/// </remarks>
		/// <param name="measured"></param>
		public static void SetAnchorMeasurement(BuffLevelSweep measured)
		{
			if (measured == null)
				throw new ArgumentNullException(nameof(measured));

			var anchor = BuffScope.Find(BuffDials.AnchorSkill)
				?? throw new InvalidOperationException($"{BuffDials.AnchorSkill}: the anchor declares no caption ratios.");

			lock (_syncLock)
				_anchorContribution = anchor.Uptime * (measured.GainOffense - 1f);
		}

		/// <summary>
		/// Clears the registered anchor, so the next pass measures its own.
		/// </summary>
		public static void ResetAnchor()
		{
			lock (_syncLock)
				_anchorContribution = null;
		}

		/// <summary>
		/// Returns the scale that puts the anchor's written base ratio on
		/// BuffDials.AnchorRatio.
		/// </summary>
		/// <remarks>
		/// Solved arithmetically rather than measured: the anchor's magnitude
		/// is chosen, not derived, and pinning it is the whole reason the
		/// roster has a level at all.
		/// </remarks>
		/// <param name="subject"></param>
		/// <param name="seed"></param>
		public static float AnchorScale(BuffSubject subject, IReadOnlyDictionary<int, float> seed = null)
		{
			var slot = (seed ?? subject.Slots).OrderBy(s => s.Key).First();

			if (slot.Value == 0)
				throw new InvalidOperationException($"{subject.SkillClassName}: the anchor's ratio reads zero, so nothing can be scaled onto it.");

			return BuffDials.AnchorRatio * Math.Max(1, subject.MaxLevel) / slot.Value;
		}

		/// <summary>
		/// Returns the base and per-level terms a scale writes for one slot.
		/// </summary>
		/// <remarks>
		/// Seed and written value are the same quantity - the magnitude at the
		/// skill's own cap, which is where SFR prices a factor too - so the pair
		/// written reads back at cap as exactly what the probe measured. That is
		/// what makes the pass a fixed point: a row already on its price seeds at
		/// its own magnitude, solves to a scale of one, and writes itself back
		/// unchanged.
		///
		/// Growing from zero the cap is reached in maxLevel steps, so the
		/// per-level term is the magnitude over the cap; under the
		/// base-plus-growth rule the base carries half and the growth the other
		/// half, which is SfrDials' own doubling rule.
		/// </remarks>
		/// <param name="subject"></param>
		/// <param name="slot"></param>
		/// <param name="scale"></param>
		public static (float Base, float ByLevel) SlotValues(BuffSubject subject, int slot, float scale)
			=> Split(subject.Slots[slot] * scale, Math.Max(1, subject.MaxLevel));

		/// <summary>
		/// Returns the base and per-level terms one magnitude at cap is written
		/// as.
		/// </summary>
		/// <param name="atCap"></param>
		/// <param name="cap"></param>
		private static (float Base, float ByLevel) Split(float atCap, int cap)
		{
			return BuffDials.GrowsFromZero
				? (0f, Round(atCap / cap))
				: (Round(atCap / 2f), Round(atCap / 2f / cap));
		}

		/// <summary>
		/// Adds the rows a buff's pinned slots write to what its solve produced.
		/// </summary>
		/// <remarks>
		/// Written from the dial rather than from a scale, so the row and
		/// BuffDials.PinnedRatios cannot drift, and the pass stays idempotent
		/// over them for the same reason: the number does not depend on the
		/// measurement.
		/// </remarks>
		/// <param name="subject"></param>
		/// <param name="slots"></param>
		private static IReadOnlyDictionary<int, (float Base, float ByLevel)> WithPinned(BuffSubject subject,
			IReadOnlyDictionary<int, (float Base, float ByLevel)> slots)
		{
			if (subject.PinnedSlots == null || subject.PinnedSlots.Count == 0)
				return slots;

			var merged = slots.ToDictionary(s => s.Key, s => s.Value);

			foreach (var (slot, pin) in subject.PinnedSlots)
				merged[slot] = (Round(pin.Base), Round(pin.ByLevel));

			return merged;
		}

		/// <summary>
		/// Returns the contribution a buff is allowed, from the anchor's own
		/// and the buff's circle.
		/// </summary>
		/// <remarks>
		/// One budget for every buff on the roster, which is what makes "this
		/// class's buff is worth far more than that one's" a constraint rather
		/// than an observation. The per-class split BUFF_BALANCE.md section 6.4
		/// describes needs the report's class weights and is not applied here;
		/// until it is, a support class's buff and a damage class's are held to
		/// the same number.
		/// </remarks>
		/// <param name="subject"></param>
		public static float TargetContribution(BuffSubject subject)
		{
			var anchor = AnchorContribution
				?? throw new InvalidOperationException("TargetContribution: the anchor has not been measured yet (SetAnchorMeasurement).");

			var (circle, skill) = Premiums(subject);

			return anchor * circle * skill;
		}

		/// <summary>
		/// Returns what the buff's circle and its own exception are allowed to
		/// multiply its budget by.
		/// </summary>
		/// <param name="subject"></param>
		public static (float Circle, float Skill) Premiums(BuffSubject subject)
		{
			var circle = 1f;

			if (BuffDials.ApplyCirclePremium)
				circle = SfrData.CirclePremium(subject.SkillClassName);

			return (circle, BuffDials.Premiums.GetValueOrDefault(subject.SkillClassName, 1f));
		}

		/// <summary>
		/// Solves one buff onto its budget and returns what to write.
		/// </summary>
		/// <remarks>
		/// Seeded from a scale of one every time, never from what the file
		/// currently carries, so the pass cannot read its own output. The scale
		/// and the row it writes move together - a row already sitting on its
		/// price solves to a scale of one and writes the same numbers back.
		/// </remarks>
		/// <param name="subject"></param>
		/// <param name="pool"></param>
		public static BuffPrice Price(BuffSubject subject, ArenaPool pool = null)
		{
			var premium = Premiums(subject);

			if (subject.IsAnchor)
			{
				var scale = AnchorScale(subject);
				var pinned = Sweep(subject, scale, pool);

				return Build(subject, scale, pinned, subject.Uptime * (pinned.GainOffense - 1f), pinned.GainOffense, premium, 1, true);
			}

			var target = TargetContribution(subject);
			var targetValue = 1f + target / Math.Max(subject.Uptime, 1e-6f);

			if (targetValue <= 1f)
				throw new InvalidOperationException($"{subject.SkillClassName}: the budget prices it at or below neutral.");

			// A buff with more than one slot is solved one slot at a time, each
			// isolated against the rest and each carrying an even share of the
			// budget, rather than as one shared scale riding whatever raw ratio
			// the slots happened to carry - never measured, only inherited,
			// which is what let Finestra's crit slot solve to 1% while its
			// block slot carried the other 21%. N independent knobs, each
			// targeted at the N-th root of the buff's target value, so their
			// product lands back on it: this is what makes Finestra a critical
			// buff and a block buff in equal measure, and it costs a same-axis
			// multi-slot buff nothing either, since two slots that both move
			// only offense combine the same way two that split offense and
			// defense do.
			if (subject.Slots.Count > 1)
			{
				var perSlotTarget = MathF.Pow(targetValue, 1f / subject.Slots.Count);
				var solutions = new Dictionary<int, AxisSolution>();

				foreach (var slot in subject.Slots.Keys)
				{
					var isolated = new Dictionary<int, float> { [slot] = subject.Slots[slot] };
					solutions[slot] = SolveScale(subject, perSlotTarget, pool, isolated, r => r.Value);
				}

				var slots = new Dictionary<int, (float Base, float ByLevel)>();
				var combinedSeed = new Dictionary<int, float>();

				foreach (var (slot, solution) in solutions)
				{
					slots[slot] = SlotValues(subject, slot, solution.Scale);
					combinedSeed[slot] = subject.Slots[slot] * solution.Scale;
				}

				var combined = Sweep(subject, 1f, pool, combinedSeed);

				return Build(subject, solutions.Values.Average(s => s.Scale), combined, target, combined.Value, premium,
					solutions.Values.Sum(s => s.Measurements), solutions.Values.All(s => s.Converged), slots);
			}

			var solved = SolveScale(subject, targetValue, pool, subject.Slots, r => r.Value);

			return Build(subject, solved.Scale, solved.Reading, target, solved.Reading.Value, premium, solved.Measurements, solved.Converged);
		}

		/// <summary>
		/// One slot's solved scale, with what it took to find it.
		/// </summary>
		private readonly record struct AxisSolution(float Scale, BuffLevelSweep Reading, int Measurements, bool Converged);

		/// <summary>
		/// Finds the scale that puts a group of slots' reading, per the given
		/// selector, on the target value.
		/// </summary>
		/// <remarks>
		/// The search itself: probe, escalate past the noise floor, fit a local
		/// power law through two points and iterate. Shared by the single-slot
		/// path and every isolated slot of a multi-slot buff, so all of them are
		/// the same search over a different slot group and a different target.
		/// </remarks>
		/// <param name="subject"></param>
		/// <param name="targetValue"></param>
		/// <param name="pool"></param>
		/// <param name="slots"></param>
		/// <param name="read"></param>
		private static AxisSolution SolveScale(BuffSubject subject, float targetValue, ArenaPool pool,
			IReadOnlyDictionary<int, float> slots, Func<BuffLevelSweep, float> read)
		{
			var measurements = 0;
			var samples = new List<(float Scale, float Excess)>();

			BuffLevelSweep best = null;
			var bestScale = 1f;
			var bestMiss = float.MaxValue;

			var next = 1f;
			var escalations = 0;

			for (var iteration = 0; iteration <= BuffDials.SolveIterations; ++iteration)
			{
				var reading = Sweep(subject, next, pool, slots);
				measurements++;

				var value = read(reading);
				var excess = value - 1f;

				// A reading under the noise floor is not evidence the buff is
				// worth nothing. Every axis here is a clamped gap - block is
				// (BLK - BLK_BREAK), dodge and critical the same shape - so a
				// magnitude that has not yet crossed the gap reads as exactly
				// zero, and the seed's size decides whether it has. Escalate
				// first, and only conclude when the widest scale still reads
				// neutral.
				if (excess <= BuffDials.EffectTolerance)
				{
					if (next >= BuffDials.MaxSlotScale || escalations >= BuffDials.EscalationSteps)
						throw new InvalidOperationException($"{subject.SkillClassName}: measured at or below neutral up to x{next:0.##}, so there is nothing to price.");

					next = Math.Min(BuffDials.MaxSlotScale, next * BuffDials.EscalationStep);
					escalations++;
					iteration--;

					continue;
				}

				var miss = Math.Abs(value - targetValue) / targetValue;

				if (miss < bestMiss)
					(best, bestScale, bestMiss) = (reading, next, miss);

				if (miss <= BuffDials.ConvergenceTolerance)
					break;

				samples.Add((next, excess));

				// Two points fit the local power law, and the second is a
				// probe rather than an attempt at the answer.
				next = samples.Count == 1
					? next * BuffDials.ProbeScaleStep
					: Solve(samples, targetValue - 1f);

				next = Math.Clamp(next, BuffDials.MinSlotScale, BuffDials.MaxSlotScale);

				if (samples.Any(s => Math.Abs(s.Scale - next) < 1e-4f))
					break;
			}

			// A reading the scale cannot move is not this pass's to write: the
			// magnitude behind it is coming from somewhere other than the
			// caption ratios, and writing the closest scale would report a
			// price the buff never took.
			if (bestMiss > BuffDials.ConvergenceTolerance && samples.Count > 1
				&& samples.Max(s => Math.Abs(s.Excess - samples[0].Excess)) <= BuffDials.EffectTolerance)
			{
				throw new InvalidOperationException($"{subject.SkillClassName}: the scale does not move its reading, " +
					"so its magnitude is not coming from the caption ratios.");
			}

			if (bestScale <= BuffDials.MinSlotScale || bestScale >= BuffDials.MaxSlotScale)
				throw new InvalidOperationException($"{subject.SkillClassName}: needs a scale of x{bestScale:0.00} to reach its budget, which is outside what may be written.");

			return new AxisSolution(bestScale, best, measurements, bestMiss <= BuffDials.ConvergenceTolerance);
		}

		/// <summary>
		/// Measures one scale across the whole character-level grid and folds
		/// the readings into one number.
		/// </summary>
		/// <remarks>
		/// A buff is not worth the same at every level and half of what one
		/// grants is now flat: +15 PATK is most of a level-15 character's
		/// attack and a rounding error on a level-99 one, while the percentage
		/// half holds its worth throughout. Pricing off one level would set the
		/// whole roster by whichever level happened to be picked. The grid is
		/// ScenarioMatrix's own, so a buff and a damage skill are read across
		/// the same curve, and a class that cannot exist at 15 is measured only
		/// where it can.
		///
		/// The levels are averaged flat rather than weighted. A weighting is a
		/// claim about where players spend their time, and nothing here
		/// measures that.
		/// </remarks>
		/// <param name="subject"></param>
		/// <param name="scale"></param>
		/// <param name="pool"></param>
		/// <param name="slotsOverride">
		/// Slot seeds to measure instead of subject.Slots, for isolating one
		/// slot of a multi-slot buff. Any slot it omits reads zero.
		/// </param>
		public static BuffLevelSweep Sweep(BuffSubject subject, float scale, ArenaPool pool = null,
			IReadOnlyDictionary<int, float> slotsOverride = null)
		{
			var job = JobCatalog.Entries.FirstOrDefault(e => e.SkillPrefix == subject.ClassName)
				?? throw new InvalidOperationException($"{subject.SkillClassName}: no job entry for class '{subject.ClassName}'.");

			var levels = ScenarioMatrix.CharacterLevelsFor(job);
			var readings = new Dictionary<int, BuffValueResult>();
			var scenarios = new Dictionary<string, float>();

			BuffValueResult Read(BuffScenario scenario, int level)
			{
				var reading = BuffValueProbe.Measure(subject, job, scale, characterLevel: level, pool: pool, scenario: scenario,
					slotsOverride: slotsOverride);

				return reading.Error != null
					? throw new InvalidOperationException($"{subject.SkillClassName}: {reading.Error}")
					: reading;
			}

			// The level grid runs on the first scenario and the rest run at one
			// level, rather than every scenario at every level. The full cross
			// product is five times the cost for a second reading of the same
			// two effects - what the levels are there to catch is a flat bonus
			// decaying, and one scenario shows that as well as five do.
			foreach (var level in levels)
				readings[level] = Read(BuffScenarios.All[0], level);

			scenarios[BuffScenarios.All[0].Id] = readings.Values.Average(r => r.Value);

			foreach (var scenario in BuffScenarios.All.Skip(1))
			{
				var reading = Read(scenario, BuffDials.ProbeLevel);

				// A scenario whose rotation landed nothing has measured nothing,
				// and counting it as 1.000 would drag every buff's value toward
				// neutral. The skill rotation does this to a class whose first
				// damage skill the harness cannot dispatch.
				if (reading.ControlDealt <= 0)
					continue;

				scenarios[scenario.Id] = reading.Value;
			}

			return new BuffLevelSweep
			{
				Readings = readings,
				Scenarios = scenarios,
				Value = BuffScenarios.Blend(scenarios),
				GainOffense = readings.Values.Average(r => r.GainOffense),
				GainDefense = readings.Values.Average(r => r.GainDefense),
				OnEnemy = readings.Values.Any(r => r.OnEnemy),
			};
		}

		/// <summary>
		/// Returns the scale a local power law puts the target excess at.
		/// </summary>
		/// <remarks>
		/// value - 1 is taken as proportional to k^p, with p fitted through the
		/// last two readings. A buff whose axis saturates has p below one and
		/// needs a wider scale than a linear read would suggest, which is the
		/// case that makes solving necessary at all.
		/// </remarks>
		/// <param name="samples"></param>
		/// <param name="targetExcess"></param>
		private static float Solve(List<(float Scale, float Excess)> samples, float targetExcess)
		{
			var (scaleA, excessA) = samples[^2];
			var (scaleB, excessB) = samples[^1];

			var power = 1f;

			if (scaleA > 0 && scaleB > 0 && excessA > 0 && excessB > 0 && Math.Abs(scaleB - scaleA) > 1e-6f)
			{
				var fitted = (float)(Math.Log(excessB / excessA) / Math.Log(scaleB / scaleA));

				if (float.IsFinite(fitted) && fitted > 0.05f)
					power = fitted;
			}

			return scaleB * (float)Math.Pow(targetExcess / excessB, 1f / power);
		}

		/// <summary>
		/// Assembles the priced row from a solved scale and its reading.
		/// </summary>
		/// <param name="subject"></param>
		/// <param name="scale"></param>
		/// <param name="reading"></param>
		/// <param name="target"></param>
		/// <param name="value"></param>
		/// <param name="premium"></param>
		/// <param name="measurements"></param>
		/// <param name="converged"></param>
		private static BuffPrice Build(BuffSubject subject, float scale, BuffLevelSweep reading, float target, float value,
			(float Circle, float Skill) premium, int measurements, bool converged,
			IReadOnlyDictionary<int, (float Base, float ByLevel)> slotsOverride = null)
		{
			var slots = WithPinned(subject, slotsOverride ?? subject.Slots.Keys.OrderBy(s => s)
				.ToDictionary(slot => slot, slot => SlotValues(subject, slot, scale)));

			return new BuffPrice
			{
				SkillClassName = subject.SkillClassName,
				ClassName = subject.ClassName,
				Circle = SfrData.SkillCircle(subject.SkillClassName),
				MaxLevel = subject.MaxLevel,
				Uptime = subject.Uptime,
				DurationSeconds = subject.DurationSeconds,
				CirclePremium = premium.Circle,
				SkillPremium = premium.Skill,
				TargetContribution = target,
				TargetValue = 1f + target / Math.Max(subject.Uptime, 1e-6f),
				SlotScale = scale,
				Value = value,
				GainOffense = reading.GainOffense,
				GainDefense = reading.GainDefense,
				OnEnemy = reading.OnEnemy,
				LevelValues = reading.PerLevel,
				ScenarioValues = reading.Scenarios,
				Measurements = measurements,
				Converged = converged,
				Slots = slots,
				Ratio = scale,
			};
		}

		/// <summary>
		/// Prices every buff in scope, and writes the roster when asked to.
		/// </summary>
		/// <remarks>
		/// Scope is what declares caption ratios, less whatever BuffDials.Excluded
		/// names. The data carries no marker that separates a buff from a damage
		/// skill honestly - the factor column reads the inert 100 on pure buffs
		/// and a block bonus or a heal on others - so the list is the curation
		/// point and it is meant to stay short.
		/// </remarks>
		/// <param name="write"></param>
		/// <param name="only"></param>
		/// <param name="poolSize"></param>
		public static BuffApplyResult ApplyAll(bool write, string only = null, int? poolSize = null)
		{
			var result = new BuffApplyResult();
			var subjects = BuffScope.Subjects;

			var anchor = BuffScope.Find(BuffDials.AnchorSkill)
				?? throw new InvalidOperationException($"{BuffDials.AnchorSkill}: the anchor declares no caption ratios.");

			using var pool = new ArenaPool(poolSize ?? BuffDials.ArenaPoolSize);

			// The anchor sets every other buff's target, so it is measured alone
			// and before the queue rather than inside it.
			ResetAnchor();
			SetAnchorMeasurement(Sweep(anchor, AnchorScale(anchor), pool));

			result.AnchorContribution = AnchorContribution.Value;
			result.Anchor = Price(anchor, pool);

			subjects = BuffScope.Filter(subjects, only);

			var priced = new ConcurrentBag<BuffPrice>();
			var held = new ConcurrentBag<(string Skill, string Reason)>();
			var heldPins = new ConcurrentBag<(string Name, IReadOnlyDictionary<int, (float Base, float ByLevel)> Slots)>();
			var queue = new ConcurrentQueue<BuffSubject>();

			// A pin is authored rather than solved, so a row the pass cannot
			// price still carries whatever BuffDials.PinnedRatios holds it at.
			void Hold(BuffSubject subject, string reason)
			{
				held.Add((subject.SkillClassName, reason));

				if (subject.PinnedSlots.Count > 0)
					heldPins.Add((subject.SkillClassName, subject.PinnedSlots));
			}

			foreach (var subject in subjects)
			{
				if (subject.IsAnchor)
					priced.Add(result.Anchor);
				else if (BuffDials.Excluded.TryGetValue(subject.SkillClassName, out var excluded))
					Hold(subject, excluded);
				else
					queue.Enqueue(subject);
			}

			void Worker()
			{
				while (queue.TryDequeue(out var subject))
				{
					try
					{
						var price = Price(subject, pool);

						// A solve that never reached its target has not measured
						// a magnitude, it has picked the least bad of the scales
						// it happened to try - and that scale can point the wrong
						// way, shrinking a buff that read under budget. Held for
						// the same reason a buff with no readable axis is.
						if (!price.Converged)
						{
							Hold(subject, $"solved short of the tolerance, landing {price.Value:0.000} against " +
								$"{price.TargetValue:0.000} after {price.Measurements} measurement(s)");

							continue;
						}

						priced.Add(price);
					}
					catch (Exception ex)
					{
						Hold(subject, ex.Message.Replace(subject.SkillClassName + ": ", ""));
					}
				}
			}

			BuffValueProbe.RunWorkers(Worker);

			// Sorted on the way out, because which worker finished first is not
			// something the report or the written file should depend on.
			result.Prices.AddRange(priced.OrderBy(p => p.SkillClassName, StringComparer.Ordinal));
			result.NotPriced.AddRange(held.OrderBy(h => h.Skill, StringComparer.Ordinal));

			if (write)
			{
				Write(result.Prices.Select(p => (p.SkillClassName, p.Slots)).Concat(heldPins));
				result.Written = true;
			}

			return result;
		}

		/// <summary>
		/// Writes the priced magnitudes back to the rows that own them.
		/// </summary>
		/// <param name="prices"></param>
		private static void Write(IEnumerable<(string Name, IReadOnlyDictionary<int, (float Base, float ByLevel)> Slots)> prices)
		{
			var byName = prices.ToDictionary(p => p.Name, p => p.Slots);
			var lines = File.ReadAllLines(SfrData.OverridesPath);

			var rewritten = lines.Select(line =>
			{
				var name = Regex.Match(line, @"className: ""([^""]+)""");

				if (!name.Success || !byName.TryGetValue(name.Groups[1].Value, out var slots))
					return line;

				foreach (var (slot, values) in slots.OrderByDescending(s => s.Key))
				{
					line = SfrPricer.SetField(line, $"captionRatio{slot}", values.Base.ToString(CultureInfo.InvariantCulture));
					line = SfrPricer.SetField(line, $"captionRatio{slot}ByLevel", values.ByLevel.ToString(CultureInfo.InvariantCulture),
						after: $"captionRatio{slot}");
				}

				return line;
			});

			File.WriteAllLines(SfrData.OverridesPath, rewritten);
		}

		/// <summary>
		/// Rounds a written magnitude to what a tooltip can render.
		/// </summary>
		/// <remarks>
		/// One decimal, except where that would round a live magnitude away
		/// altogether: Thaumaturge_SwellHands' second slot is PATK per point of
		/// caster INT, and a coefficient below 0.05 is a real number rather than
		/// a rounding artefact.
		/// </remarks>
		/// <param name="value"></param>
		private static float Round(float value)
		{
			var rounded = MathF.Round(value, 1);

			return rounded == 0 && value != 0 ? MathF.Round(value, 3) : rounded;
		}
	}
}
