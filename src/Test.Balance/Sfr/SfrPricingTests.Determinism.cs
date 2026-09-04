using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Melia.Test.Balance.Sfr
{
	public partial class SfrPricingTests
	{
		/// <summary>
		/// Checks that a measurement repeats itself, one condition at a time.
		/// </summary>
		/// <remarks>
		/// PriceIsRepeatable answers whether the price moved, under the
		/// concurrency the roster pass runs at. This answers where a move comes
		/// from: the same window run twice on one thread has no scheduling in it
		/// at all, so a reading that already differs there is the window, and one
		/// that only differs pooled is the concurrency around it.
		/// </remarks>
		[Collection(BalanceCollection.Name)]
		public class Determinism
		{
			private readonly ITestOutputHelper _output;

			/// <summary>
			/// Creates the fixture.
			/// </summary>
			/// <param name="host"></param>
			/// <param name="output"></param>
			public Determinism(BalanceHost host, ITestOutputHelper output)
				=> _output = output;

			/// <summary>
			/// The defensive probe run twice in a row, alone, on one arena.
			/// </summary>
			[Fact]
			public void DefenseWindowRepeatsSerially()
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

				var skillName = SfrDials.AnchorSkill;

				Assert.True(Melia.Zone.ZoneServer.Instance.Data.SkillDb.TryFind(skillName, out var data));
				Assert.True(JobCatalog.TryGet(SfrData.ClassOf(skillName), out var job));

				var level = SfrData.SkillMaxLevel(skillName);
				var charLevel = ScenarioMatrix.CharacterLevelsFor(job, [50]).FirstOrDefault(50);

				var first = SfrDefenseProbe.Measure(job, data.Id, level, charLevel);
				var second = SfrDefenseProbe.Measure(job, data.Id, level, charLevel);

				_output.WriteLine("first  controls: " + string.Join(", ", first.Controls.Select(v => v.ToString("0.0"))));
				_output.WriteLine("second controls: " + string.Join(", ", second.Controls.Select(v => v.ToString("0.0"))));
				_output.WriteLine("first  treatments: " + string.Join(", ", first.Treatments.Select(v => v.ToString("0.0"))));
				_output.WriteLine("second treatments: " + string.Join(", ", second.Treatments.Select(v => v.ToString("0.0"))));

				Assert.Equal(first.Controls, second.Controls);
				Assert.Equal(first.Treatments, second.Treatments);
			}

			/// <summary>
			/// The same probe on a pooled arena, serially, and then pooled and
			/// concurrent, so the arena and the concurrency are separated.
			/// </summary>
			[Fact]
			public void DefenseWindowRepeatsPooled()
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

				var skillName = SfrDials.AnchorSkill;

				Assert.True(Melia.Zone.ZoneServer.Instance.Data.SkillDb.TryFind(skillName, out var data));
				Assert.True(JobCatalog.TryGet(SfrData.ClassOf(skillName), out var job));

				var level = SfrData.SkillMaxLevel(skillName);
				var charLevel = ScenarioMatrix.CharacterLevelsFor(job, [50]).FirstOrDefault(50);

				using var pool = new ArenaPool(SfrDials.ExplainPoolSize);

				var onPoolArena = pool.Use(m => SfrDefenseProbe.Measure(job, data.Id, level, charLevel, arena: m));

				var concurrent = new SfrDefenseResult[2];
				SkillPressProbe.RunAll(
					() => concurrent[0] = SfrDefenseProbe.Measure(job, data.Id, level, charLevel, pool: pool),
					() => concurrent[1] = SfrDefenseProbe.Measure(job, data.Id, level, charLevel, pool: pool));

				_output.WriteLine("serial on pool arena: " + string.Join(", ", onPoolArena.Controls.Select(v => v.ToString("0.0"))));
				_output.WriteLine("concurrent first:     " + string.Join(", ", concurrent[0].Controls.Select(v => v.ToString("0.0"))));
				_output.WriteLine("concurrent second:    " + string.Join(", ", concurrent[1].Controls.Select(v => v.ToString("0.0"))));

				Assert.Equal(concurrent[0].Controls, concurrent[1].Controls);
			}

			/// <summary>
			/// The defensive probe under the width the roster pass runs it at.
			/// </summary>
			[Fact]
			public void DefenseWindowRepeatsUnderLoad()
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

				var skillName = SfrDials.AnchorSkill;

				Assert.True(Melia.Zone.ZoneServer.Instance.Data.SkillDb.TryFind(skillName, out var data));
				Assert.True(JobCatalog.TryGet(SfrData.ClassOf(skillName), out var job));

				var level = SfrData.SkillMaxLevel(skillName);
				var charLevel = ScenarioMatrix.CharacterLevelsFor(job, [50]).FirstOrDefault(50);

				using var pool = new ArenaPool(SfrDials.ExplainPoolSize);

				var results = new SfrDefenseResult[8];

				SkillPressProbe.RunAll(Enumerable.Range(0, results.Length)
					.Select(i => (Action)(() => results[i] = SfrDefenseProbe.Measure(job, data.Id, level, charLevel, pool: pool)))
					.ToArray());

				foreach (var result in results)
					_output.WriteLine("controls: " + (result.Error ?? string.Join(", ", result.Controls.Select(v => v.ToString("0.0")))));

				foreach (var result in results)
					Assert.Equal(results[0].Controls, result.Controls);
			}
		}
	}
}
