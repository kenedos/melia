using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Melia.Test.Balance.Sfr;
using Xunit;
using Xunit.Abstractions;

namespace Melia.Test.Balance.Buff
{
	/// <summary>
	/// The buff pricing pass, and the guard that keeps its anchor still.
	/// </summary>
	/// <remarks>
	/// BuffValueTests measures what a buff is worth; this prices it. The two
	/// stay apart because a reading that cannot be explained is not one to
	/// write back, and only the write here is opt-in - it rewrites
	/// skills_overrides.txt.
	/// </remarks>
	[Collection(BalanceCollection.Name)]
	public class BuffPricingTests
	{
		/// <summary>
		/// Environment variable that lets the pass write.
		/// </summary>
		public const string ApplyVariable = "BALANCE_BUFF_APPLY";

		/// <summary>
		/// Environment variable naming the buffs to price instead of all of
		/// them, comma-separated.
		/// </summary>
		/// <remarks>
		/// Only PriceBuffs reads it. AnchorHoldsItsRatio and WritingIsIdempotent
		/// are roster-wide assertions and run their own full passes, so a
		/// single-skill run has to name the test as well as the skill:
		///
		///   BALANCE_BUFF_SKILL=Scout_DoubleAttack dotnet test ... --filter BuffPricingTests.PriceBuffs
		///
		/// Filtering never narrows what is written by more than it measured -
		/// Write only rewrites the rows it priced, so a one-skill apply leaves
		/// every other row untouched.
		/// </remarks>
		public const string SkillVariable = "BALANCE_BUFF_SKILL";

		/// <summary>
		/// Report the run leaves behind, alongside the damage pass's own.
		/// </summary>
		public const string ReportName = "buff-prices.md";

		private readonly ITestOutputHelper _output;
		private readonly List<string> _lines = [];

		/// <summary>
		/// Creates the fixture.
		/// </summary>
		/// <param name="host"></param>
		/// <param name="output"></param>
		public BuffPricingTests(BalanceHost host, ITestOutputHelper output)
			=> _output = output;

		/// <summary>
		/// Prices every buff in scope, and writes them when the apply variable
		/// is set.
		/// </summary>
		[Fact]
		public void PriceBuffs()
		{
			if (!BalanceSuites.BuffEnabled)
			{
				_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.BuffVariable));
				return;
			}

			var only = Environment.GetEnvironmentVariable(SkillVariable);
			var write = Environment.GetEnvironmentVariable(ApplyVariable) == "1";
			var started = DateTime.UtcNow;

			var result = BuffPricer.ApplyAll(write, only);

			this.Write("# Buff prices");
			this.Write();
			this.Write($"{result.Prices.Count} buff(s) priced, {result.NotPriced.Count} not priced, " +
				$"in {(DateTime.UtcNow - started).TotalMinutes:0.0} min.");
			this.Write();
			this.Write($"`{BuffDials.AnchorSkill}` is pinned at ratio {BuffDials.AnchorRatio:0.##} and measures " +
				$"{result.Anchor.Value:0.000} on offense alone, so the roster's budget is " +
				$"**{result.AnchorContribution:0.000}** per rotation" +
				(BuffDials.ApplyCirclePremium ? ", times each buff's circle premium." : "."));
			this.Write();
			this.Write("Scenarios: " + string.Join(", ", BuffScenarios.All.Select(sc => $"`{sc.Id}` {sc.Name} (x{sc.Weight:0.#})")) + ".");
			this.Write();
			this.Write("`M_buff = uptime * (value - 1)` is what every buff is solved onto, so a buff that is up half " +
				"the time is solved to twice the magnitude of a permanent one - the uptime premium is exactly " +
				"`1 / uptime` and needs no dial. The scale moves all of a " +
				"buff's declared slots together, so a percentage stays a percentage and only its magnitude moves. " +
				"Durations are read and never written - `captionTime` is authored, not priced.");
			this.Write();
			this.Write("| skill | class | circle | cap | uptime | premium | scale | value | by level | by scenario | target | M_buff | ratios |");
			this.Write("|---|---|---|---|---|---|---|---|---|---|---|---|---|");

			foreach (var price in result.Prices.OrderBy(p => p.ClassName).ThenBy(p => p.SkillClassName))
			{
				var byLevel = string.Join(" / ", price.LevelValues.OrderBy(l => l.Key).Select(l => $"{l.Key}:{l.Value:0.00}"));

				var byScenario = string.Join(" / ", (price.ScenarioValues ?? new Dictionary<string, float>())
					.OrderBy(v => v.Key).Select(v => $"{v.Key}:{v.Value:0.00}"));

				var ratios = string.Join(", ", price.Slots.OrderBy(s => s.Key)
					.Select(s => $"r{s.Key} {s.Value.Base:0.#} +{s.Value.ByLevel:0.#}/lv"));

				this.Write($"| `{price.SkillClassName}` | {price.ClassName} | {price.Circle} | {price.MaxLevel} | " +
					$"{price.Uptime:0.00} | x{price.CirclePremium * price.SkillPremium:0.00} | x{price.SlotScale:0.000} | " +
					$"{price.Value:0.000} | {byLevel} | {byScenario} | {price.TargetValue:0.000} | " +
					$"{price.Contribution:+0.000;-0.000} | {ratios} |");
			}

			var unauthored = result.Prices.Where(p => p.DurationSeconds <= 0).OrderBy(p => p.SkillClassName).ToArray();

			if (unauthored.Length > 0)
			{
				this.Write();
				this.Write("## Priced as permanent, duration not authored");
				this.Write();
				this.Write("These rows carry no `captionTime`, so uptime defaults to 1.00 and they take no uptime " +
					"premium. A buff here that really does expire is under-paid by its own `1 / uptime` - the fix " +
					"is authoring `captionTime` on the row, not a dial.");
				this.Write();
				this.Write(string.Join(", ", unauthored.Select(p => $"`{p.SkillClassName}`")));
			}

			if (result.NotPriced.Count > 0)
			{
				this.Write();
				this.Write("## Not priced");
				this.Write();
				this.Write("These keep whatever magnitudes the file already carries, except for any slot BuffDials.PinnedRatios holds, which is written from the dial.");
				this.Write();

				foreach (var (skill, reason) in result.NotPriced.OrderBy(n => n.Reason).ThenBy(n => n.Skill))
					this.Write($"- `{skill}` - {reason}");
			}

			this.Write();
			this.Write(result.Written ? "written to " + SfrData.OverridesPath : "dry run - nothing written");

			_output.WriteLine($"report -> {this.SaveReport()}");

			// A single-skill run is a diagnostic and may well name a buff the
			// pass refuses to price; only the roster run has to produce prices.
			if (string.IsNullOrWhiteSpace(only))
				Assert.NotEmpty(result.Prices);
		}

		/// <summary>
		/// The anchor holds the roster's level, so it moving means every other
		/// buff moved with it.
		/// </summary>
		/// <remarks>
		/// It is pinned arithmetically rather than solved, so this is a test of
		/// the write path: whatever the probe reads, the row the anchor gets
		/// carries the chosen ratio and the growth rule's per-level term.
		/// </remarks>
		[Fact]
		public void AnchorHoldsItsRatio()
		{
			if (!BalanceSuites.BuffEnabled)
			{
				_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.BuffVariable));
				return;
			}

			var anchor = BuffScope.Find(BuffDials.AnchorSkill);

			Assert.NotNull(anchor);

			var scale = BuffPricer.AnchorScale(anchor);
			var slot = anchor.Slots.OrderBy(s => s.Key).First().Key;
			var (baseValue, byLevel) = BuffPricer.SlotValues(anchor, slot, scale);

			Assert.Equal(0f, baseValue, 2);
			Assert.Equal(BuffDials.AnchorRatio, byLevel, 2);

			// Nothing is given away at level one, so the whole magnitude is
			// bought a point at a time: 3 a level to 15 at a cap of five.
			Assert.Equal(BuffDials.AnchorRatio * anchor.MaxLevel, baseValue + byLevel * anchor.MaxLevel, 2);
		}

		/// <summary>
		/// A row already sitting on its price has to solve back to the same
		/// numbers, or the pass reads its own output.
		/// </summary>
		/// <remarks>
		/// Asserted over the whole roster rather than the anchor alone, and at
		/// several scales, because what used to break it was the seed and the
		/// written value being two different quantities - which the anchor's own
		/// cap of five hid, and every cap of ten or fifteen did not.
		/// </remarks>
		[Fact]
		public void WritingIsIdempotent()
		{
			if (!BalanceSuites.BuffEnabled)
			{
				_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.BuffVariable));
				return;
			}

			var scales = new[] { 0.4f, 1f, 2.7f };

			foreach (var subject in BuffScope.Subjects)
			{
				foreach (var scale in scales)
				{
					foreach (var slot in subject.Slots.Keys)
					{
						var written = BuffPricer.SlotValues(subject, slot, scale);

						// What the row reads at its cap once written is what the
						// next pass seeds from, so putting it back through the
						// same arithmetic at a scale of one has to reproduce it.
						var rewritten = new BuffSubject
						{
							SkillClassName = subject.SkillClassName,
							SkillId = subject.SkillId,
							ClassName = subject.ClassName,
							Buffs = subject.Buffs,
							Slots = new Dictionary<int, float> { [slot] = written.Base + written.ByLevel * subject.MaxLevel },
							MaxLevel = subject.MaxLevel,
							DurationSeconds = subject.DurationSeconds,
							CycleSeconds = subject.CycleSeconds,
						};

						var again = BuffPricer.SlotValues(rewritten, slot, 1f);

						Assert.Equal(written.Base, again.Base, 3);
						Assert.Equal(written.ByLevel, again.ByLevel, 3);
					}
				}
			}

			var anchor = BuffScope.Find(BuffDials.AnchorSkill);

			Assert.NotNull(anchor);
			Assert.Equal(1f, BuffPricer.AnchorScale(anchor, anchor.WrittenMagnitudes), 2);
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
