using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Melia.Test.Balance.Sfr
{
	/// <summary>
	/// The SFR pricing pass, and the guard that keeps its anchor still.
	/// </summary>
	/// <remarks>
	/// Nothing here prices without a live measurement any more, so every test
	/// in this class needs the headless ZoneServer BalanceHost boots.
	/// PriceRoster is the generation procedure proper, and is a multi-minute
	/// run rather than a sub-second one. Only the write is opt-in, because it
	/// rewrites skills_overrides.txt.
	/// </remarks>
	[Collection(BalanceCollection.Name)]
	public partial class SfrPricingTests
	{
		/// <summary>
		/// Environment variable that lets the pass write.
		/// </summary>
		public const string ApplyVariable = "BALANCE_SFR_APPLY";

		/// <summary>
		/// Environment variable naming one skill to explain instead of the
		/// whole roster.
		/// </summary>
		public const string SkillVariable = "BALANCE_SFR_SKILL";

		/// <summary>
		/// Report the roster run leaves behind, alongside the sweep's own.
		/// </summary>
		public const string ReportName = "sfr-prices.md";

		/// <summary>
		/// Report the repeatability check leaves behind. Its own file, so a run
		/// of it never overwrites the roster report someone is reading.
		/// </summary>
		public const string RepeatReportName = "sfr-repeat.md";

		private readonly ITestOutputHelper _output;
		private readonly List<string> _lines = [];

		/// <summary>
		/// The one skill the run was narrowed to, or null when it prices the
		/// whole roster.
		/// </summary>
		internal static string SingleSkill => Environment.GetEnvironmentVariable(SkillVariable);

		/// <summary>
		/// Creates the fixture.
		/// </summary>
		/// <param name="host"></param>
		/// <param name="output"></param>
		public SfrPricingTests(BalanceHost host, ITestOutputHelper output)
			=> _output = output;

		/// <summary>
		/// Writes a line to the test output and to the report being built.
		/// </summary>
		/// <remarks>
		/// The report file is the reliable channel. dotnet test shows
		/// ITestOutputHelper only at detailed logger verbosity, and xunit
		/// captures both stdout and stderr, so there is no way to print here by
		/// default.
		/// </remarks>
		/// <param name="line"></param>
		private void Write(string line = "")
		{
			_output.WriteLine(line);
			_lines.Add(line);
		}

		/// <summary>
		/// Saves everything written so far next to the sweep's reports, so the
		/// run leaves a record whatever the logger is set to.
		/// </summary>
		/// <param name="name"></param>
		private string SaveReport(string name = ReportName)
		{
			// Anchored on the project root rather than the working directory:
			// nothing boots the server here, so the run never navigates there.
			var directory = Path.Combine(SfrData.Root, SweepReport.OutputDirectory);

			Directory.CreateDirectory(directory);

			var path = Path.Combine(directory, name);

			File.WriteAllText(path, string.Join(Environment.NewLine, _lines), Encoding.UTF8);

			return Path.GetFullPath(path);
		}

		/// <summary>
		/// The anchor holds the roster's level, so it moving means every other
		/// skill moved with it.
		/// </summary>
		[Fact]
		public void AnchorHoldsItsFactor()
		{
			if (!BalanceSuites.SfrEnabled)
			{
				_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.SfrVariable));
				return;
			}

			if (!string.IsNullOrEmpty(SingleSkill))
			{
				_output.WriteLine($"Skipped. {SkillVariable} narrows this run to {SingleSkill}.");
				return;
			}

			using var pool = new ArenaPool(SfrDials.ExplainPoolSize);

			var press = SkillPressProbe.MeasureAll(SfrDials.AnchorSkill, measureDefense: false, pool: pool, measureOffense: false);
			SfrPricer.SetAnchorMeasurement(press);

			var price = SfrPricer.Price(SfrDials.AnchorSkill, null, press);

			Assert.Equal((int)SfrDials.AnchorFactor, price.Factor);

			// calc_skill.cs reads factor + factorByLevel * level, so this is
			// what the skill actually deals the moment it is learned.
			var levelOne = price.Factor + price.FactorByLevel;

			Assert.InRange(levelOne, 113f, 117f);
		}

		/// <summary>
		/// Every scenario the weights name has to exist in the matrix, and
		/// every priced scenario has to carry a weight.
		/// </summary>
		[Fact]
		public void ScenarioWeightsMatchTheMatrix()
		{
			if (!BalanceSuites.SfrEnabled)
			{
				_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.SfrVariable));
				return;
			}

			var matrix = ScenarioMatrix.All.Select(s => s.Id).ToHashSet();

			foreach (var id in SfrDials.ScenarioWeights.Keys)
				Assert.Contains(id, matrix);

			foreach (var id in SfrDials.SpreadScenarios)
				Assert.Contains(id, SfrDials.ScenarioWeights.Keys);

			foreach (var id in SfrDials.PeakScenarios)
				Assert.Contains(id, SfrDials.ScenarioWeights.Keys);

			Assert.Equal(SfrDials.ScenarioWeights.Count, SfrGeometry.PricedScenarios.Count());
		}

		/// <summary>
		/// Prices the roster, and writes it when the apply variable is set.
		/// </summary>
		[Fact]
		public void PriceRoster()
		{
			if (!BalanceSuites.SfrEnabled)
			{
				_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.SfrVariable));
				return;
			}

			var single = Environment.GetEnvironmentVariable(SkillVariable);

			if (!string.IsNullOrEmpty(single))
			{
				Explain(single);
				return;
			}

			var write = Environment.GetEnvironmentVariable(ApplyVariable) == "1";
			var started = DateTime.UtcNow;
			var result = SfrPricer.ApplyAll(write);

			Write($"{result.Changes.Count} skills priced, {result.SpChanges.Count} SP costs priced, {result.NotPriceable} not priceable, " +
				$"simulated in {(DateTime.UtcNow - started).TotalMinutes:0.0} min " +
				$"({SfrPricer.LastPoolBuildTime.TotalSeconds:0}s building {SfrDials.ArenaPoolSize} arenas, " +
				$"{SfrPricer.LastMeasureTime.TotalSeconds:0}s measuring on {SfrDials.SkillWorkers} workers)");
			Write("");
			Write($"{"circle",-8}{"count",6}{"cap",8}{"premium",9}{"slope",9}  median factor");

			foreach (var circle in result.Changes.Keys.Select(SfrData.SkillCircle).Distinct().OrderBy(c => c))
			{
				var named = result.Changes.Keys.Where(n => SfrData.SkillCircle(n) == circle).ToArray();
				var caps = named.Select(SfrData.SkillMaxLevel).Distinct().OrderBy(v => v);
				var factors = named.Select(n => result.Changes[n].Factor).OrderBy(f => f).ToArray();
				var premiums = named.Select(SfrData.CirclePremium).Distinct().OrderBy(v => v);
				var shares = named.Select(SfrData.SlopeShare).Distinct().OrderBy(v => v);

				Write($"{circle,-8}{named.Length,6}{string.Join("/", caps),8}{string.Join("/", premiums.Select(v => v.ToString("0.00"))),9}"
					+ $"{string.Join("/", shares.Select(v => v.ToString("0.00"))),9}  {factors[factors.Length / 2]}");
			}

			var untreed = result.Changes.Keys.Where(n => !SfrData.HasTreeRow(n)).OrderBy(n => n).ToArray();

			if (untreed.Length > 0)
			{
				Write("");
				Write($"{untreed.Length} skill(s) have no skilltree row - priced as circle 1: {string.Join(", ", untreed)}");
			}

			Write("");
			Write("largest raises (ratio is against the old value, and only reported):");

			foreach (var pair in result.Changes.OrderByDescending(c => c.Value.Ratio).Take(12))
				Write($"   {pair.Key,-34} x{pair.Value.Ratio:0.00} -> factor {pair.Value.Factor}, factorByLevel {pair.Value.FactorByLevel:0.0}");

			Write("");
			Write("largest cuts:");

			foreach (var pair in result.Changes.OrderBy(c => c.Value.Ratio).Take(12))
				Write($"   {pair.Key,-34} x{pair.Value.Ratio:0.00} -> factor {pair.Value.Factor}, factorByLevel {pair.Value.FactorByLevel:0.0}");

			if (result.SpChanges.Count > 0)
			{
				var costs = result.SpChanges.Values.Select(c => c.Sp).OrderBy(v => v).ToArray();

				Write("");
				Write($"SP costs: median {costs[costs.Length / 2]}, range {costs[0]}-{costs[^1]}"
					+ $"  (anchor {SfrDials.SpAnchorCost:0}, arcane x{SfrDials.SpArcaneMultiplier:0.00},"
					+ $" circle x{string.Join("/", SfrDials.CirclePremium.OrderBy(p => p.Key).Select(p => p.Value.ToString("0.00")))},"
					+ $" buff x{SfrDials.SpBuffMultiplier:0.0},"
					+ $" channel x{SfrDials.SpChannelMultiplier:0.00})");
				Write("largest SP raises:");

				foreach (var pair in result.SpChanges.OrderByDescending(c => c.Value.Ratio).Take(8))
					Write($"   {pair.Key,-34} x{pair.Value.Ratio:0.00} -> basicSp {pair.Value.Sp}, lvUpSpendSp {pair.Value.SpByLevel:0.##}");

				Write("largest SP cuts:");

				foreach (var pair in result.SpChanges.OrderBy(c => c.Value.Ratio).Take(8))
					Write($"   {pair.Key,-34} x{pair.Value.Ratio:0.00} -> basicSp {pair.Value.Sp}, lvUpSpendSp {pair.Value.SpByLevel:0.##}");
			}

			if (result.SpRepeatCharges.Count > 0)
			{
				Write("");
				Write($"{result.SpRepeatCharges.Count} skill(s) charge their SP cost more than once per press - the written cost is per charge:");

				foreach (var entry in result.SpRepeatCharges.OrderByDescending(s => s.Charges))
					Write($"   {entry.Skill,-34} {entry.Charges:0.0} charges -> basicSp {entry.Sp}");
			}

			if (result.SpUnmeasuredChannels.Count > 0)
			{
				Write("");
				Write($"{result.SpUnmeasuredChannels.Count} channel(s) kept their SP cost - the press was never measured, so how often it charges is unknown:");

				foreach (var entry in result.SpUnmeasuredChannels.OrderBy(s => s.Skill))
					Write($"   {entry.Skill,-34} basicSp {entry.OldSp:0.#}");
			}

			if (result.HardOverrides.Count > 0)
			{
				Write("");
				Write($"{result.HardOverrides.Count} skill(s) carry a hard SFR override (SfrDials.SkillSfrMultipliers):");

				foreach (var entry in result.HardOverrides.OrderBy(o => o.Skill))
					Write($"   {entry.Skill,-34} x{entry.Multiplier:0.00} of the model's own price");
			}

			if (result.NewlyPriced.Count > 0)
			{
				Write("");
				Write($"{result.NewlyPriced.Count} skill(s) came off factor 0 - a live press showed them dealing damage:");

				foreach (var entry in result.NewlyPriced.OrderByDescending(n => n.Factor))
					Write($"   {entry.Skill,-34} factor {entry.Factor}");
			}

			if (result.Amplifiers.Count > 0)
			{
				Write("");
				Write($"{result.Amplifiers.Count} skill(s) were measured making the caster's other damage land harder - charged for it:");

				foreach (var entry in result.Amplifiers.OrderByDescending(a => a.Amplification).Take(20))
					Write($"   {entry.Skill,-34} +{entry.Amplification:P0} on everything else -> x{entry.Multiplier:0.00} of its own factor");
			}

			if (result.Gathered.Count > 0)
			{
				var premiums = result.Gathered.Select(g => g.Premium).OrderBy(v => v).ToArray();

				Write("");
				Write($"{result.Gathered.Count} skill(s) demand more than natural spawn density - median x{premiums[premiums.Length / 2]:0.00}, "
					+ $"top x{premiums[^1]:0.00} ({SfrDials.GatheringReference:0.0}x natural earns x{1f + SfrDials.GatheringPremium:0.00}, "
					+ $"capped at x{SfrDials.GatheringMax:0.00}; natural is one monster per {SfrGeometry.NaturalMobArea:N0} sq units):");

				foreach (var entry in result.Gathered.OrderByDescending(g => g.Premium).Take(20))
					Write($"   {entry.Skill,-34} {entry.Capacity:0.0} targets in {entry.Area:N0} sq units = {entry.Demand:0.0}x natural -> x{entry.Premium:0.00}");
			}

			if (result.Overrunning.Count > 0)
			{
				Write("");
				Write($"{result.Overrunning.Count} skill(s) still delivering when their own cycle was up - counted over the cycle:");

				foreach (var entry in result.Overrunning.OrderByDescending(o => o.Span - o.Cycle))
					Write($"   {entry.Skill,-34} span {entry.Span:0.0}s over a {entry.Cycle:0.0}s cycle, {entry.Hits:0.0} hit(s) counted");
			}

			if (result.Unmeasured.Count > 0)
			{
				Write("");
				Write($"NOT MEASURED - {result.Unmeasured.Count} skill(s) kept their existing factor:");

				foreach (var group in result.Unmeasured.GroupBy(u => u.Reason).OrderByDescending(g => g.Count()))
				{
					Write($"   {group.Key}:");

					foreach (var entry in group.OrderBy(u => u.Skill))
						Write($"      {entry.Skill,-34} factor {entry.OldFactor:0.#}");
				}
			}

			Write("");
			Write(write ? "written to " + SfrData.OverridesPath : "dry run - nothing written");

			_output.WriteLine("report saved to " + SaveReport());

			Assert.NotEmpty(result.Changes);
		}

		/// <summary>
		/// Environment variable naming the skill the repeatability check
		/// measures. Defaults to the anchor.
		/// </summary>
		public const string RepeatVariable = "BALANCE_SFR_REPEAT";

		/// <summary>
		/// Measures the anchor and one skill twice each and fails if either
		/// prices differently the second time.
		/// </summary>
		/// <remarks>
		/// The pass may not stabilize itself against what it wrote last time,
		/// so run-to-run stability is a property of the measurement and has to
		/// be checked there. This is that check, and it is seconds rather than
		/// the minutes a roster diff costs.
		///
		/// The anchor is measured twice because its own price is pinned: a
		/// wobble in it never shows in its own number and lands instead in the
		/// scale every other skill is multiplied by, so a skill coming back 9%
		/// different between two roster runs is more often the anchor having
		/// moved than that skill. The two are reported separately for exactly
		/// that reason - the anchor ratio is the roster's level, the skill
		/// ratio is the skill.
		///
		/// Both riders run, unlike Explain's fast loop: the defensive and
		/// amplifier probes are the noisiest readings in the model and leaving
		/// them out would check the quiet half.
		/// </remarks>
		[Fact]
		public void PriceIsRepeatable()
		{
			if (!BalanceSuites.SfrEnabled)
			{
				_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.SfrVariable));
				return;
			}

			if (!string.IsNullOrEmpty(SingleSkill))
			{
				_output.WriteLine($"Skipped. {SkillVariable} narrows this run to {SingleSkill}.");
				return;
			}

			var skillName = Environment.GetEnvironmentVariable(RepeatVariable);

			if (string.IsNullOrEmpty(skillName))
				skillName = SfrDials.AnchorSkill;

			using var pool = new ArenaPool(SfrDials.ExplainPoolSize);

			var anchors = new SfrMeasuredPress[2];
			var presses = new SfrMeasuredPress[2];

			// All four at once, so the two repeats of a skill run under the
			// same concurrency the roster pass puts them under. Measured
			// serially they would each have the machine to themselves, which is
			// the one condition a scheduling-dependent reading looks stable in.
			SkillPressProbe.RunAll(
				() => anchors[0] = SkillPressProbe.MeasureAll(SfrDials.AnchorSkill, pool: pool),
				() => anchors[1] = SkillPressProbe.MeasureAll(SfrDials.AnchorSkill, pool: pool),
				() => presses[0] = SkillPressProbe.MeasureAll(skillName, pool: pool),
				() => presses[1] = SkillPressProbe.MeasureAll(skillName, pool: pool));

			// Read before the anchor is pinned: SetAnchorMeasurement zeroes the
			// two rider readings on the press it is handed, so asking afterwards
			// reports a pair of zeroes rather than what the probes measured.
			var anchorReadings = Readings(anchors[0], anchors[1]).ToArray();

			SfrPricer.SetAnchorMeasurement(anchors[0]);
			var firstScale = SfrPricer.Calibration();

			SfrPricer.SetAnchorMeasurement(anchors[1]);
			var secondScale = SfrPricer.Calibration();

			Write($"anchor {SfrDials.AnchorSkill}: calibration scale {firstScale:0.0000} then {secondScale:0.0000}"
				+ $"  (x{secondScale / Math.Max(firstScale, 1e-6f):0.000} on every priced skill)");

			// Priced against one pinned anchor, so what is compared is the two
			// presses and not the scale that moved under them.
			SfrPricer.SetAnchorMeasurement(anchors[0]);

			var first = SfrPricer.Price(skillName, null, presses[0]);
			var second = SfrPricer.Price(skillName, null, presses[1]);

			Write($"{skillName}: factor {first.Factor} then {second.Factor}");
			Write("");
			Write($"{"reading",-22}{"first",12}{"second",12}");

			foreach (var line in Readings(presses[0], presses[1]))
				Write(line);

			foreach (var line in anchorReadings)
				Write("anchor " + line);

			_output.WriteLine("report saved to " + SaveReport(RepeatReportName));

			Assert.Equal(firstScale, secondScale, 4);
			Assert.Equal(first.Factor, second.Factor);
		}

		/// <summary>
		/// Returns one line per reading the price is built from, so a failure
		/// names the term that moved rather than only the factor.
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		private static IEnumerable<string> Readings(SfrMeasuredPress first, SfrMeasuredPress second)
		{
			string Line(string name, float a, float b)
				=> $"{name,-22}{a,12:0.000}{b,12:0.000}{(Math.Abs(a - b) > 1e-4f ? "   MOVED" : "")}";

			yield return Line("hit equivalents", first.HitEquivalents, second.HitEquivalents);
			yield return Line("swings prevented", first.SwingsPrevented, second.SwingsPrevented);
			yield return Line("amplification", first.DamageAmplification, second.DamageAmplification);
			yield return Line("sp charge slope", first.SpChargeSlope, second.SpChargeSlope);
			yield return Line("damage span", first.DamageSpanSeconds, second.DamageSpanSeconds);
			yield return Line("burst fraction", first.BurstFraction, second.BurstFraction);

			foreach (var scenario in first.Targets.Keys.OrderBy(k => k.Length).ThenBy(k => k))
				yield return Line("targets " + scenario, first.Targets[scenario], second.Targets.GetValueOrDefault(scenario));

			// Per trial, because the swings the defence probe reads are a mean
			// over matched pairs: a mean that moved says nothing about whether
			// one pair did or all of them.
			for (var trial = 0; trial < first.DefenseControls.Length; ++trial)
			{
				yield return Line($"control {trial}", first.DefenseControls[trial], Element(second.DefenseControls, trial));
				yield return Line($"treatment {trial}", first.DefenseTreatments[trial], Element(second.DefenseTreatments, trial));
			}
		}

		/// <summary>
		/// Returns the value at the given position, or zero when the run being
		/// compared against has no such trial.
		/// </summary>
		/// <param name="values"></param>
		/// <param name="index"></param>
		private static float Element(float[] values, int index)
			=> index < values.Length ? values[index] : 0f;

		/// <summary>
		/// Environment variable that also runs the defensive/CC probe for the
		/// explained skill. Off by default: it costs SfrDials.DefenseProbeTrials
		/// full encounter windows on its own, which defeats the point of this
		/// being the fast single-skill loop.
		/// </summary>
		public const string DefenseVariable = "BALANCE_SFR_DEFENSE";

		/// <summary>
		/// Environment variable that also runs the amplification probe for the
		/// explained skill. Off by default for the same reason the defensive
		/// one is: it costs SfrDials.OffenseProbeTrials control/treatment pairs
		/// on top of the scenarios.
		/// </summary>
		public const string OffenseVariable = "BALANCE_SFR_OFFENSE";

		/// <summary>
		/// Writes out every term behind one skill's price, measuring only that
		/// skill.
		/// </summary>
		/// <remarks>
		/// This is the fast loop the model's iterated against: one skill,
		/// across the priced scenario matrix, rather than the full roster.
		/// It calibrates against a freshly measured anchor - the anchor costs
		/// a second full skill's worth of scenarios, but there is no cheaper
		/// scale left to fall back to now that nothing here scans a handler.
		///
		/// Both measurements run on an arena pool, and against each other, so
		/// the wall time is the longest single window rather than the sum of
		/// forty of them. Unpooled, MeasureAll runs its scenarios, factor
		/// points and defence trials one at a time, and SfrDefenseProbe takes
		/// its serial path, which pays the full DefenseProbeTrials on every
		/// skill instead of scouting a no-CC press out in two pairs.
		/// </remarks>
		/// <param name="skillName"></param>
		private void Explain(string skillName)
		{
			var measureDefense = Environment.GetEnvironmentVariable(DefenseVariable) == "1";
			var measureOffense = Environment.GetEnvironmentVariable(OffenseVariable) == "1";

			using var pool = new ArenaPool(SfrDials.ExplainPoolSize);

			SfrMeasuredPress press = null;
			var anchorPresses = new SfrMeasuredPress[SfrDials.AnchorTrials];
			Exception failure = null;

			var work = new List<Action>
			{
				() =>
				{
					try
					{
						press = SkillPressProbe.MeasureAll(skillName, measureDefense: measureDefense, pool: pool, measureOffense: measureOffense);
					}
					catch (Exception ex)
					{
						failure = ex;
					}
				},
			};

			// The anchor is measured as many times here as the roster run
			// measures it, so the two calibrate on the same statistic and an
			// explained factor matches the written one.
			for (var trial = 0; trial < SfrDials.AnchorTrials; ++trial)
			{
				var at = trial;
				work.Add(() => anchorPresses[at] = SkillPressProbe.MeasureAll(SfrDials.AnchorSkill, measureDefense: false, pool: pool, measureOffense: false));
			}

			SkillPressProbe.RunAll(work.ToArray());

			if (failure != null)
			{
				Write($"{skillName} could not be measured live: {failure.Message}");
				_output.WriteLine("report saved to " + SaveReport());
				return;
			}

			SfrPricer.CalibrateOnMedian(anchorPresses);

			Write($"measured: DirectHits {press.DirectHits} (fromDamage {press.HitsFromDamage}), " +
				$"HitsTruncated {press.HitsTruncated}, Delivered {press.Delivered}");

			if (press.HitsFailure != null)
				Write($"  hit count failed: {press.HitsFailure}");

			foreach (var scenario in press.Scenarios.OrderBy(s => s.Key))
			{
				var s = scenario.Value;
				Write($"  {s.ScenarioId}: {s}");
			}

			SfrPrice r;

			try
			{
				r = SfrPricer.Price(skillName, null, press);
			}
			catch (Exception ex)
			{
				Write($"{skillName}: {ex.Message}");

				// The press still charges the bar even when the damage model
				// will not price it, so SP is written on its own here.
				if (Environment.GetEnvironmentVariable(ApplyVariable) == "1")
					Apply(skillName, null, TrySp(skillName, press));

				_output.WriteLine("report saved to " + SaveReport());
				return;
			}

			Write($"{r.Skill}  ({r.Class}, circle {r.Circle}, max level {r.Levels}, circle premium x{r.CirclePremium:0.00}, "
				+ $"slope share {SfrData.SlopeShare(r.Skill):0.00}, channel x{r.ChannelPremium:0.00})");
			Write($"  occupancy t   {r.Occupancy:0.00} s      cycle T {r.Cycle:0.00} s      u {r.Utilization:0.00}");
			Write($"  hits/press   {r.Hits:0.0}      basic swings/s {r.BasicRate:0.00}");
			Write($"  damage span  {r.DamageSpan:0.0} s   burst {r.BurstFraction:0.00} of total   divisor {r.Divisor:0.0}");
			Write($"  counted over {r.CountWindow:0.0} s of a {r.FullDamageSpan:0.0} s delivery"
				+ (r.Overruns ? "   OVERRUNS ITS CYCLE" : ""));
			Write($"  riders        {(r.RiderKinds.Length > 0 ? string.Join(", ", r.RiderKinds) : "none")}"
				+ $" (defensive x{r.RiderMultiplier:0.00}, amplifier x{r.AmplifierMultiplier:0.00})");
			Write($"  e ceiling     {r.Efficiency:0.00}   ({(r.IsChannel ? "channel" : "cast")} {r.Cast:0.00} s)");
			Write($"  cast premium  x{r.CastPremium:0.00}  ({(r.CastPremiumKinds.Length > 0 ? string.Join(", ", r.CastPremiumKinds) : "none")})");
			Write("  targets       " + string.Join("  ", r.Targets.OrderBy(t => t.Key.Length).ThenBy(t => t.Key)
				.Select(t => $"{t.Key} {t.Value.Mine:0.0}/{t.Value.Theirs:0.0}")));
			Write($"  weighted reach {r.WeightedReach:0.00}  peak-blended {r.ChargedReach:0.00}"
				+ $"  charged as {MathF.Pow(Math.Max(r.ChargedReach, 1e-6f), SfrDials.AoeExponent):0.00}"
				+ (r.SpreadCapped ? "   SPREAD-CAPPED" : ""));
			Write($"  gathering     {r.TargetCapacity:0.0} targets in {r.AreaCovered:N0} sq units"
				+ $" = {r.GatheringDemand:0.0}x natural density -> x{r.GatheringPremium:0.00}"
				+ $"  (area read on {string.Join("/", SfrDials.GatheringAreaScenarios)};"
				+ $" {SfrDials.GatheringReference:0.0}x earns x{1f + SfrDials.GatheringPremium:0.00})");
			Write("");

			if (r.HardMultiplier != 1f)
				Write($"  HARD SFR OVERRIDE: x{r.HardMultiplier:0.00} of the model's own price (SfrDials.SkillSfrMultipliers)");

			Write($"  total SFR at max level   {r.Sfr:0}%");
			Write($"  factor: {r.Factor}, factorByLevel: {r.FactorByLevel:0.0}");
			Write($"  SP target {r.Sp.Target:0.0} over {r.Sp.Charges:0.0} charge(s)"
				+ $"{(r.Sp.Measured ? " (measured)" : " (assumed)")}"
				+ $"  {(r.Sp.Kinds.Length > 0 ? string.Join(", ", r.Sp.Kinds) : "plain")}");
			Write($"  basicSp: {r.Sp.Cost}, lvUpSpendSp: {r.Sp.CostByLevel:0.##}");

			if (Environment.GetEnvironmentVariable(ApplyVariable) == "1")
				Apply(skillName, r, r.Sp);

			_output.WriteLine("report saved to " + SaveReport());
		}

		/// <summary>
		/// Returns the skill's SP price, or null when it cannot be priced.
		/// </summary>
		/// <param name="skillName"></param>
		/// <param name="press"></param>
		private static SfrSpPrice TrySp(string skillName, SfrMeasuredPress press)
		{
			try
			{
				return SfrPricer.PriceSp(skillName, press);
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Writes one skill's priced factor and SP cost into the overrides
		/// file, on the same terms the roster run writes them.
		/// </summary>
		/// <param name="skillName"></param>
		/// <param name="price"></param>
		/// <param name="sp"></param>
		private void Apply(string skillName, SfrPrice price, SfrSpPrice sp)
		{
			var priced = new Dictionary<string, (int Factor, float FactorByLevel)>();
			var spPriced = new Dictionary<string, SfrSpPrice>();

			if (price != null)
				priced[skillName] = (price.Factor, price.FactorByLevel);

			// A channel bills its cost every tick, so writing the whole press
			// budget as the per-tick cost over-charges it by however many ticks
			// it runs, and only a measured charge count rules that out.
			var billsPerTick = sp != null && sp.Charges <= 1f
				&& SfrData.Skills.TryGetValue(skillName, out var entry) && SfrPricer.IsChannel(entry);

			if (sp != null && !billsPerTick)
				spPriced[skillName] = sp;

			if (billsPerTick)
				Write($"  SP not written: {skillName} channels and its charge count was not measured");

			if (priced.Count == 0 && spPriced.Count == 0)
			{
				Write($"  nothing written for {skillName}");
				return;
			}

			var written = SfrPricer.ApplyPrices(priced, spPriced);

			if (written == 0)
			{
				Write($"  nothing written: {skillName} has no line in {Path.GetFileName(SfrData.OverridesPath)}");
				return;
			}

			Write($"  written to {Path.GetFileName(SfrData.OverridesPath)}: "
				+ string.Join(", ", new[]
				{
					price != null ? $"factor {price.Factor}, factorByLevel {price.FactorByLevel:0.0}" : null,
					spPriced.Count > 0 ? $"basicSp {sp.Cost}, lvUpSpendSp {sp.CostByLevel:0.##}" : null,
				}.Where(part => part != null)));
		}
	}
}
