using System;
using System.Collections.Generic;
using System.Diagnostics;
using Melia.Test.Balance.Buff;
using Melia.Test.Balance.Sfr;
using Xunit.Abstractions;

namespace Melia.Test.Balance
{
	/// <summary>
	/// Runs the full balance pass without a test runner, so it starts from
	/// Visual Studio's F5 and reports to a console instead of Test Explorer.
	/// </summary>
	/// <remarks>
	/// The gates are set here rather than in the environment, so this is
	/// always a writing run: the same one Test.Balance.runsettings describes.
	/// </remarks>
	public static class Program
	{
		private static readonly List<(string Name, TimeSpan Took, string Error)> _results = [];

		/// <summary>
		/// Boots the headless server and runs every pass in turn.
		/// </summary>
		/// <param name="args"></param>
		public static int Main(string[] args)
		{
			Environment.SetEnvironmentVariable(BalanceSuites.SfrVariable, "1");
			Environment.SetEnvironmentVariable(SfrPricingTests.ApplyVariable, "1");
			Environment.SetEnvironmentVariable(BalanceSuites.BuffVariable, "1");
			Environment.SetEnvironmentVariable(BuffPricingTests.ApplyVariable, "1");

			var started = DateTime.UtcNow;

			Console.WriteLine("Balance: full pass, writing to skills_overrides.txt.");
			Console.WriteLine("Booting headless ZoneServer...");

			var host = new BalanceHost();
			var output = new ConsoleOutput();

			var sfr = new SfrPricingTests(host, output);
			Run("Sfr.AnchorHoldsItsFactor", sfr.AnchorHoldsItsFactor);
			Run("Sfr.ScenarioWeightsMatchTheMatrix", sfr.ScenarioWeightsMatchTheMatrix);
			Run("Sfr.PriceRoster", sfr.PriceRoster);
			Run("Sfr.PriceIsRepeatable", sfr.PriceIsRepeatable);

			var scenarios = new BuffScenarioTests(host, output);
			Run("Buff.ScenariosProduceTheChancesTheyDeclare", scenarios.ScenariosProduceTheChancesTheyDeclare);
			Run("Buff.PlainScenariosTouchNothing", scenarios.PlainScenariosTouchNothing);
			Run("Buff.ReportsTheNaturalRolls", scenarios.ReportsTheNaturalRolls);

			var stacking = new BuffStackingTests(host, output);
			Run("Buff.StacksDoNotCompound", stacking.StacksDoNotCompound);

			var values = new BuffValueTests(host, output);
			Run("Buff.MeasureBuffs", values.MeasureBuffs);
			Run("Buff.NoiseFloorIsFlat", values.NoiseFloorIsFlat);
			Run("Buff.ScopeFollowsTheData", values.ScopeFollowsTheData);

			var prices = new BuffPricingTests(host, output);
			Run("Buff.PriceBuffs", prices.PriceBuffs);
			Run("Buff.AnchorHoldsItsRatio", prices.AnchorHoldsItsRatio);
			Run("Buff.WritingIsIdempotent", prices.WritingIsIdempotent);

			return Summarize(DateTime.UtcNow - started);
		}

		/// <summary>
		/// Runs one pass, timing it and holding onto its failure rather than
		/// letting it end the run.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="pass"></param>
		private static void Run(string name, Action pass)
		{
			Console.WriteLine();
			Console.WriteLine($"=== {name}");

			var watch = Stopwatch.StartNew();
			var error = (string)null;

			try
			{
				pass();
			}
			catch (Exception ex)
			{
				error = ex.Message;
				Console.WriteLine($"FAILED: {ex}");
			}

			watch.Stop();
			_results.Add((name, watch.Elapsed, error));

			Console.WriteLine($"--- {name}: {(error == null ? "ok" : "FAILED")} in {watch.Elapsed.TotalMinutes:0.0} min");
		}

		/// <summary>
		/// Prints what ran and returns the process's exit code.
		/// </summary>
		/// <param name="took"></param>
		private static int Summarize(TimeSpan took)
		{
			var failed = 0;

			Console.WriteLine();
			Console.WriteLine($"=== Done in {took.TotalMinutes:0.0} min");

			foreach (var result in _results)
			{
				if (result.Error != null)
					failed++;

				Console.WriteLine($"  {(result.Error == null ? "ok    " : "FAILED")} {result.Name} ({result.Took.TotalMinutes:0.0} min)" +
					(result.Error == null ? "" : $" - {result.Error}"));
			}

			Console.WriteLine();
			Console.WriteLine($"{_results.Count - failed}/{_results.Count} passed. Reports in logs/balance/.");

			if (!Console.IsInputRedirected)
			{
				Console.WriteLine("Press any key to close.");
				Console.ReadKey(true);
			}

			return failed == 0 ? 0 : 1;
		}
	}

	/// <summary>
	/// Sends the passes' report lines to the console, in place of the test
	/// runner's own output sink.
	/// </summary>
	public class ConsoleOutput : ITestOutputHelper
	{
		/// <summary>
		/// Writes one line.
		/// </summary>
		/// <param name="message"></param>
		public void WriteLine(string message)
			=> Console.WriteLine(message);

		/// <summary>
		/// Writes one formatted line.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void WriteLine(string format, params object[] args)
			=> Console.WriteLine(format, args);
	}
}
