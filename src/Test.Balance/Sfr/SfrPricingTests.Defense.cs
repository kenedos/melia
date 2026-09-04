using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Melia.Zone;
using Xunit;
using Xunit.Abstractions;

namespace Melia.Test.Balance.Sfr
{
	public partial class SfrPricingTests
	{
		/// <summary>
		/// Diagnostic runs for SfrDefenseProbe. Not wired into SfrPricer yet - this
		/// is the fast loop for checking the measurement itself reads sensibly
		/// before any pricing formula is built on top of it.
		/// </summary>
		/// <remarks>
		/// Opt-in: each named skill costs two full encounter windows
		/// (SfrDials.EncounterWindowMs apiece) in wall-clock time.
		/// </remarks>
		[Collection(BalanceCollection.Name)]
		public class Defense
		{
			/// <summary>
			/// Environment variable that enables the run.
			/// </summary>
			public const string EnableVariable = "BALANCE_DEFENSE";

			/// <summary>
			/// Environment variable naming the skills to measure.
			/// </summary>
			public const string SkillsVariable = "BALANCE_DEFENSE_SKILLS";

			/// <summary>
			/// Report the run leaves behind.
			/// </summary>
			public const string ReportName = "sfr-defense.md";

			/// <summary>
			/// A spread of skills with an obvious defensive or crowd-control
			/// payload, to sanity-check the measurement finds one where it is
			/// known to exist.
			/// </summary>
			private static readonly string[] DefaultSkills =
			[
				"Peltasta_Langort",
				"Swordman_Bash",
				"Highlander_CrossCut",
			];

			private readonly ITestOutputHelper _output;
			private readonly List<string> _lines = [];

			public Defense(BalanceHost host, ITestOutputHelper output)
				=> _output = output;

			private static bool Enabled => Environment.GetEnvironmentVariable(EnableVariable) == "1";

			/// <summary>
			/// Runs the control/treatment pair for each named skill and reports
			/// what it bought in avoided damage.
			/// </summary>
			[Fact]
			public void DefensiveValue()
			{
				if (!Enabled)
				{
					_output.WriteLine($"Skipped. Set {EnableVariable}=1 to run.");
					return;
				}

				var named = Environment.GetEnvironmentVariable(SkillsVariable);
				var skills = string.IsNullOrWhiteSpace(named)
					? DefaultSkills
					: named.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

				Write("# Defensive / crowd-control value");
				Write("");
				Write("Control is a mob swinging freely at an idle character for the window; treatment is the");
				Write("same window after one press of the named skill. The gap is what the skill bought.");
				Write("");

				var measuredAny = false;

				foreach (var skillName in skills)
				{
					if (!ZoneServer.Instance.Data.SkillDb.TryFind(skillName, out var data))
					{
						Write($"{skillName,-28}unknown skill");
						continue;
					}

					var prefix = SfrData.ClassOf(skillName);

					if (!JobCatalog.TryGet(prefix, out var job))
					{
						Write($"{skillName,-28}'{prefix}' is not in the job catalog");
						continue;
					}

					var level = SfrData.SkillMaxLevel(skillName);
					var charLevel = ScenarioMatrix.CharacterLevelsFor(job, [50]).FirstOrDefault(50);

					var result = SfrDefenseProbe.Measure(job, data.Id, level, charLevel);

					measuredAny |= result.Error == null;
					Write($"{skillName,-28}{result}");
				}

				_output.WriteLine("report saved to " + SaveReport());

				Assert.True(measuredAny, "no skill could be measured at all");
			}

			/// <summary>
			/// Report the repeatability run leaves behind.
			/// </summary>
			public const string RepeatReportName = "sfr-defense-repeat.md";

			/// <summary>
			/// Runs the whole probe twice for one skill and reports both sets of
			/// pairs side by side, so a reading that disagrees between runs can be
			/// read pair by pair instead of as one number.
			/// </summary>
			/// <remarks>
			/// This is the only reading in the model that still moves between two
			/// identically seeded runs, and one number cannot say why. The columns
			/// separate the two candidates: a control half that differs is the mob
			/// having behaved differently, which is the window itself leaking real
			/// time or shared state, while controls that match and differences that
			/// do not is the press.
			///
			/// Both runs go at once, on one pool, because a probe measured with the
			/// machine to itself is the one condition a scheduling-dependent
			/// reading looks stable in.
			/// </remarks>
			[Fact]
			public void DefenseProbeIsRepeatable()
			{
				if (!Enabled)
				{
					_output.WriteLine($"Skipped. Set {EnableVariable}=1 to run.");
					return;
				}

				var named = Environment.GetEnvironmentVariable(SkillsVariable);
				var skillName = string.IsNullOrWhiteSpace(named)
					? SfrDials.AnchorSkill
					: named.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[0];

				if (!ZoneServer.Instance.Data.SkillDb.TryFind(skillName, out var data))
					throw new ArgumentException($"Unknown skill '{skillName}'.");

				if (!JobCatalog.TryGet(SfrData.ClassOf(skillName), out var job))
					throw new ArgumentException($"'{skillName}' is not in the job catalog.");

				var level = SfrData.SkillMaxLevel(skillName);
				var charLevel = ScenarioMatrix.CharacterLevelsFor(job, [50]).FirstOrDefault(50);

				using var pool = new ArenaPool(SfrDials.ExplainPoolSize);

				SfrDefenseResult first = null;
				SfrDefenseResult second = null;

				SkillPressProbe.RunAll(
					() => first = SfrDefenseProbe.Measure(job, data.Id, level, charLevel, pool: pool),
					() => second = SfrDefenseProbe.Measure(job, data.Id, level, charLevel, pool: pool));

				Write($"# {skillName} defence probe, twice");
				Write("");
				Write($"per-tick realignment {(DeterministicRandom.RealignEnabled ? "on" : "off")}"
					+ $" ({DeterministicRandom.NoRealignVariable}=1 turns it off)");
				Write($"{SfrDials.DefenseProbeTrials} trials, {SfrDials.DefenseWindowMs} ms window,"
					+ $" {SfrDials.DefenseSettleMs} ms settle");
				Write("");
				Write($"basic swing {first.BasicSwing:F0} then {second.BasicSwing:F0}");
				Write($"swings prevented {first.SwingsPrevented:0.000} then {second.SwingsPrevented:0.000}");
				Write("");
				Write($"{"trial",6}{"controlA",12}{"controlB",12}{"treatA",12}{"treatB",12}{"diffA",12}{"diffB",12}");

				for (var trial = 0; trial < first.Controls.Length && trial < second.Controls.Length; ++trial)
				{
					var controlMoved = Math.Abs(first.Controls[trial] - second.Controls[trial]) > 0.5f;
					var treatmentMoved = Math.Abs(first.Treatments[trial] - second.Treatments[trial]) > 0.5f;

					Write($"{trial,6}{first.Controls[trial],12:F0}{second.Controls[trial],12:F0}"
						+ $"{first.Treatments[trial],12:F0}{second.Treatments[trial],12:F0}"
						+ $"{first.Controls[trial] - first.Treatments[trial],12:F0}{second.Controls[trial] - second.Treatments[trial],12:F0}"
						+ (controlMoved ? "   CONTROL MOVED" : "") + (treatmentMoved ? "   TREATMENT MOVED" : ""));
				}

				_output.WriteLine("report saved to " + SaveReport(RepeatReportName));

				Assert.Null(first.Error);
				Assert.Null(second.Error);
				Assert.Equal(first.SwingsPrevented, second.SwingsPrevented, 3);
			}

			private void Write(string line = "")
			{
				_output.WriteLine(line);
				_lines.Add(line);
			}

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
