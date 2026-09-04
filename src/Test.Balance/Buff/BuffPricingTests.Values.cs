using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Melia.Test.Balance.Sfr;
using Xunit;
using Xunit.Abstractions;

namespace Melia.Test.Balance.Buff
{
	public partial class BuffPricingTests
	{
		/// <summary>
		/// The buff measurement pass: what each converted buff is worth, in the
		/// same units the damage model's rotation multiplier is expressed in.
		/// </summary>
		/// <remarks>
		/// This is the reading the pricer will be built on, and it is deliberately
		/// separate from it: a value that cannot be explained is not one to write
		/// back. Nothing here writes to skills_overrides.txt.
		/// </remarks>
		[Collection(BalanceCollection.Name)]
		public class Values
		{
			/// <summary>
			/// Report the run leaves behind, alongside the damage pass's own.
			/// </summary>
			public const string ReportName = "buff-values.md";

			private readonly ITestOutputHelper _output;
			private readonly List<string> _lines = [];

			/// <summary>
			/// Creates the fixture.
			/// </summary>
			/// <param name="host"></param>
			/// <param name="output"></param>
			public Values(BalanceHost host, ITestOutputHelper output)
				=> _output = output;

			/// <summary>
			/// Measures every buff in scope and reports what each is worth.
			/// </summary>
			[Fact]
			public void MeasureBuffs()
			{
				if (!BalanceSuites.BuffEnabled)
				{
					_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.BuffVariable));
					return;
				}

				var only = Environment.GetEnvironmentVariable(SkillVariable);
				var subjects = BuffScope.Subjects;

				subjects = BuffScope.Filter(subjects, only);

				Assert.NotEmpty(subjects);

				this.Write("# Buff values");
				this.Write();
				this.Write($"{subjects.Length} buff-granting press(es) in scope, measured at character level " +
					$"{BuffDials.ProbeLevel}, each at the magnitude its row reaches at its own cap, over " +
					$"{BuffDials.Trials} pairs of {BuffDials.WindowMs / 1000}s windows.");
				this.Write();
				this.Write($"`value = gain_off * gain_def ^ {BuffDials.DefenseWeight}`, and " +
					"`M_buff = uptime * (value - 1)` is what the class budget sees.");
				this.Write();

				using var pool = new ArenaPool(BuffDials.ArenaPoolSize);

				var measured = new ConcurrentDictionary<string, (BuffSubject Subject, BuffValueResult Result)>();
				var queue = new ConcurrentQueue<BuffSubject>(subjects);

				void Worker()
				{
					while (queue.TryDequeue(out var subject))
						measured[subject.SkillClassName] = (subject,
							BuffValueProbe.Measure(subject, slotScale: 1f, pool: pool, slotsOverride: subject.WrittenMagnitudes));
				}

				BuffValueProbe.RunWorkers(Worker);

				var results = subjects
					.Where(s => measured.ContainsKey(s.SkillClassName))
					.Select(s => measured[s.SkillClassName])
					.ToList();

				this.Write("| skill | class | cap | uptime | dealt | taken | off | def | value | M_buff |");
				this.Write("|---|---|---|---|---|---|---|---|---|---|");

				foreach (var (subject, result) in results.Where(r => r.Result.Error == null).OrderByDescending(r => r.Result.Value))
				{
					var contribution = subject.Uptime * (result.Value - 1f);

					this.Write($"| `{subject.SkillClassName}` | {subject.ClassName} | {subject.MaxLevel} | " +
						$"{subject.Uptime:0.00} | {result.ControlDealt:N0} -> {result.TreatmentDealt:N0} ({result.ControlDealtHits:N0} hits) | " +
						$"{result.ControlTaken:N0} -> {result.TreatmentTaken:N0} ({result.ControlTakenHits:N0} hits) | " +
						$"{result.GainOffense:0.000}x | {result.GainDefense:0.000}x | " +
						$"**{result.Value:0.000}** | {contribution:+0.000;-0.000} |");
				}

				var failed = results.Where(r => r.Result.Error != null).ToArray();

				if (failed.Length > 0)
				{
					this.Write();
					this.Write("## Not measured");
					this.Write();

					foreach (var (subject, result) in failed)
						this.Write($"- `{subject.SkillClassName}` - {result.Error}");
				}

				var inert = results.Where(r => r.Result.Error == null && !r.Result.HasEffect).ToArray();

				if (inert.Length > 0)
				{
					this.Write();
					this.Write("## Measured, but moved nothing");
					this.Write();
					this.Write("A buff whose whole value is movement speed, cooldowns or healing lands here, and so " +
						"does one whose handler is not reading its caption ratios yet.");
					this.Write();

					foreach (var (subject, _) in inert)
						this.Write($"- `{subject.SkillClassName}`");
				}

				_output.WriteLine($"report -> {this.SaveReport()}");
			}

			/// <summary>
			/// Runs the buff-free window on both sides of every pair, so whatever
			/// the pass reports here is the probe's own noise rather than a buff.
			/// </summary>
			/// <remarks>
			/// This is the test that says whether a reading means anything. A gain
			/// of 1.10 on a real buff is only evidence if the null case sits near
			/// 1.00; the first version of this probe read a 40% defensive swing on
			/// Priest_Blessing, which grants no defense at all.
			/// </remarks>
			[Fact]
			public void NoiseFloorIsFlat()
			{
				if (!BalanceSuites.BuffEnabled)
				{
					_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.BuffVariable));
					return;
				}

				using var pool = new ArenaPool(SfrDials.ExplainPoolSize);

				var anchor = BuffScope.Find(BuffDials.AnchorSkill);
				var result = BuffValueProbe.Measure(anchor, pool: pool, applyBuff: false);

				this.Write("# Buff probe noise floor");
				this.Write();
				this.Write($"Both halves of every pair run without the buff, over {BuffDials.Trials} pairs of " +
					$"{BuffDials.WindowMs / 1000}s windows against {BuffDials.MobCount} monsters.");
				this.Write();
				this.Write($"- dealt: {result.ControlDealt:N0} -> {result.TreatmentDealt:N0} over {result.ControlDealtHits:N0} hits");
				this.Write($"- taken: {result.ControlTaken:N0} -> {result.TreatmentTaken:N0} over {result.ControlTakenHits:N0} hits");
				this.Write($"- offense {result.GainOffense:0.0000}x, defense {result.GainDefense:0.0000}x, value {result.Value:0.0000}");

				_output.WriteLine($"report -> {this.SaveReport("buff-noise-floor.md")}");

				Assert.Null(result.Error);
				Assert.InRange(result.GainOffense, 0.99f, 1.01f);
				Assert.InRange(result.GainDefense, 0.99f, 1.01f);
			}

			/// <summary>
			/// Scope is what the data declares, so a converted buff has to appear in
			/// it and the anchor has to be one of them.
			/// </summary>
			[Fact]
			public void ScopeFollowsTheData()
			{
				if (!BalanceSuites.BuffEnabled)
				{
					_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.BuffVariable));
					return;
				}

				var anchor = BuffScope.Find(BuffDials.AnchorSkill);

				Assert.NotNull(anchor);
				Assert.NotEmpty(anchor.Buffs);
				Assert.NotEmpty(anchor.Slots);

				// The anchor is a base-job skill, which never advances a circle.
				Assert.Equal(5, anchor.MaxLevel);

				foreach (var subject in BuffScope.Subjects)
				{
					Assert.NotEmpty(subject.Slots);
					Assert.InRange(subject.Uptime, 0f, 1f);
					Assert.InRange(subject.MaxLevel, 1, 15);
				}
			}

			/// <summary>
			/// Writes a line to the test output and to the report being built.
			/// </summary>
			/// <param name="line"></param>
			private void Write(string line = "")
			{
				_output.WriteLine(line);
				_lines.Add(line);
			}

			/// <summary>
			/// Saves everything written so far next to the damage pass's reports.
			/// </summary>
			/// <param name="name"></param>
			private string SaveReport(string name = ReportName)
			{
				var directory = Path.Combine(SfrData.Root, SweepReport.OutputDirectory);

				Directory.CreateDirectory(directory);

				var path = Path.Combine(directory, name);

				File.WriteAllText(path, string.Join(Environment.NewLine, _lines), Encoding.UTF8);

				return Path.GetFullPath(path);
			}
		}
	}
}
