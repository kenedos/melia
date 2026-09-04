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
	public partial class BuffPricingTests
	{
		/// <summary>
		/// What several buffs are worth held at once, against what they are worth
		/// one at a time.
		/// </summary>
		/// <remarks>
		/// The pricer values every buff alone, so nothing in it can see two of them
		/// compounding. A character in play holds four or five, and if each is
		/// priced at a fifth over neutral the stack should land near their product
		/// - not far above it. Far above means a magnitude is multiplying something
		/// another buff already multiplied, and the roster's whole level is wrong by
		/// however much that is.
		///
		/// This validates; it does not price. Feeding a stacked reading back into
		/// the solver would make every buff's magnitude depend on which others
		/// happened to be drawn with it.
		/// </remarks>
		[Collection(BalanceCollection.Name)]
		public class Stacking
		{
			/// <summary>
			/// Report the run leaves behind.
			/// </summary>
			public const string ReportName = "buff-stacking.md";

			/// <summary>
			/// Buffs drawn into one stack.
			/// </summary>
			public const int StackSize = 4;

			/// <summary>
			/// How far past the product of the parts a stack may land before it is
			/// a finding.
			/// </summary>
			/// <remarks>
			/// Generous, because the parts are measured on a live sample with
			/// dodge, block and crit rolling. What this is looking for is a stack
			/// worth half again what its parts are, not a stack three percent off.
			/// </remarks>
			public const float Tolerance = 0.25f;

			private readonly ITestOutputHelper _output;
			private readonly List<string> _lines = [];

			/// <summary>
			/// Creates the fixture.
			/// </summary>
			/// <param name="host"></param>
			/// <param name="output"></param>
			public Stacking(BalanceHost host, ITestOutputHelper output)
				=> _output = output;

			/// <summary>
			/// Draws a stack of buffs a single class can actually hold, and checks
			/// the whole against the product of its parts.
			/// </summary>
			[Fact]
			public void StacksDoNotCompound()
			{
				if (!BalanceSuites.BuffEnabled)
				{
					_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.BuffVariable));
					return;
				}

				if (!string.IsNullOrEmpty(SingleBuff))
				{
					_output.WriteLine($"Skipped. {SkillVariable} narrows this run to {SingleBuff}.");
					return;
				}

				using var pool = new ArenaPool(SfrDials.ExplainPoolSize);

				var stack = Draw();

				Assert.NotEmpty(stack);

				this.Write("# Buff stacking");
				this.Write();
				this.Write($"{stack.Length} buff(s) from `{stack[0].ClassName}`, measured one at a time and then together, " +
					$"at character level {BuffDials.ProbeLevel} and buff level {BuffDials.ProbeBuffLevel}.");
				this.Write();

				var parts = new List<(BuffSubject Subject, float Value)>();

				foreach (var subject in stack)
				{
					var reading = BuffValueProbe.Measure(subject, pool: pool, slotsOverride: subject.WrittenMagnitudes);

					if (reading.Error != null)
					{
						this.Write($"- `{subject.SkillClassName}` could not be measured: {reading.Error}");
						continue;
					}

					parts.Add((subject, reading.Value));
				}

				Assert.NotEmpty(parts);

				var combined = BuffValueProbe.MeasureStack(parts.Select(p => p.Subject).ToArray(), pool: pool);

				Assert.Null(combined.Error);

				var product = parts.Aggregate(1f, (acc, p) => acc * p.Value);

				this.Write("| buff | alone |");
				this.Write("|---|---|");

				foreach (var (subject, value) in parts)
					this.Write($"| `{subject.SkillClassName}` | {value:0.000} |");

				this.Write();
				this.Write($"- product of the parts: **{product:0.000}**");
				this.Write($"- measured together: **{combined.Value:0.000}**");
				this.Write($"- ratio: **{combined.Value / product:0.000}** (1.000 is exactly multiplicative)");
				this.Write();
				this.Write(combined.Value > product * (1f + Tolerance)
					? "The stack is worth more than its parts multiplied. Something is compounding."
					: "The stack lands within tolerance of its parts, so nothing is compounding.");

				_output.WriteLine($"report -> {this.SaveReport()}");

				Assert.InRange(combined.Value / product, 1f - Tolerance, 1f + Tolerance);
			}

			/// <summary>
			/// Picks the buffs to stack: the priced ones a single class can hold,
			/// which is what a character actually presses.
			/// </summary>
			/// <remarks>
			/// One class rather than a draw from the whole roster, because a stack
			/// nobody can assemble proves nothing. The order is the data's own and
			/// the draw takes the first few, so the same buffs come out every run
			/// without needing a seed.
			/// </remarks>
			private static BuffSubject[] Draw()
			{
				var byClass = BuffScope.Subjects
					.Where(s => s.Slots.Count > 0)
					.GroupBy(s => s.ClassName)
					.Where(g => g.Count() >= StackSize)
					.OrderByDescending(g => g.Count())
					.ThenBy(g => g.Key, StringComparer.Ordinal)
					.FirstOrDefault();

				return byClass == null
					? []
					: byClass.OrderBy(s => s.SkillClassName, StringComparer.Ordinal).Take(StackSize).ToArray();
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
			/// Saves everything written so far next to the pass's other reports.
			/// </summary>
			private string SaveReport()
			{
				var directory = Path.Combine(SfrData.Root, SweepReport.OutputDirectory);

				Directory.CreateDirectory(directory);

				var path = Path.Combine(directory, ReportName);

				File.WriteAllText(path, string.Join(Environment.NewLine, _lines), Encoding.UTF8);

				return Path.GetFullPath(path);
			}
		}
	}
}
