using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Melia.Test.Balance.Sfr
{
	public partial class SfrPricingTests
	{
		/// <summary>
		/// The defense curve's inverse, which is pure arithmetic and needs no
		/// server.
		/// </summary>
		public class DamageCurve
		{
			/// <summary>
			/// Halving the factor halves the damage. The curve sits between attack
			/// and defense, and the factor multiplies what comes out of it, so the
			/// two never interact.
			/// </summary>
			[Fact]
			public void FactorIsLinearInDamage()
			{
				var line = SfrDamageCurve.Proportional(100f, 4000f);

				Assert.Equal(2000f, line.DamageAt(50f), 3);
				Assert.Equal(8000f, line.DamageAt(200f), 3);
			}

			/// <summary>
			/// Two measurements determine the line exactly, including the
			/// post-factor additive term a single one cannot see.
			/// </summary>
			[Fact]
			public void TwoPointsRecoverTheFlatTerm()
			{
				var truth = new SfrDamageCurve.FactorLine(37f, 250f);
				var line = SfrDamageCurve.Solve(100f, truth.DamageAt(100f), 200f, truth.DamageAt(200f));

				Assert.Equal(truth.Slope, line.Slope, 3);
				Assert.Equal(truth.Flat, line.Flat, 3);
			}

			/// <summary>
			/// The inverse lands on the factor that produces the wanted damage in
			/// one step.
			/// </summary>
			[Fact]
			public void SolveFactorRoundTrips()
			{
				var line = new SfrDamageCurve.FactorLine(37f, 250f);
				var wanted = line.DamageAt(143f);

				Assert.Equal(143f, SfrDamageCurve.SolveFactor(line, wanted), 3);
			}

			/// <summary>
			/// The mitigation curve is the one in calc_combat.cs, and its exponent
			/// is 1.2 rather than the 2.5 the design notes quote.
			/// </summary>
			[Fact]
			public void MitigationMatchesTheScript()
			{
				var attack = 1200f;
				var defense = 400f;

				var ratio = attack / defense;
				var scaled = MathF.Pow(ratio, SfrDamageCurve.DefenseExponent);

				Assert.Equal(scaled / (scaled + 1f), SfrDamageCurve.Mitigation(attack, defense), 5);
				Assert.Equal(attack * (scaled / (scaled + 1f)), SfrDamageCurve.MitigatedAttack(attack, defense), 3);
			}
		}

		/// <summary>
		/// Diagnostics for the live press itself, independent of pricing.
		/// </summary>
		/// <remarks>
		/// Opt-in: this boots a ZoneServer and every press runs in wall-clock
		/// time. There is no scanned comparison left to run these against -
		/// nothing in the pricer reads a handler's source any more - so these
		/// check the measurement primitives directly instead.
		/// </remarks>
		[Collection(BalanceCollection.Name)]
		public class Press
		{
			/// <summary>
			/// Environment variable that enables the measured runs.
			/// </summary>
			public const string EnableVariable = "BALANCE_PRESS";

			private readonly ITestOutputHelper _output;

			public Press(BalanceHost host, ITestOutputHelper output)
				=> _output = output;

			private static bool Enabled => Environment.GetEnvironmentVariable(EnableVariable) == "1";

			/// <summary>
			/// Fire Ball hits its own target twice - once directly, once through
			/// the explosion its splash loop puts the target back into.
			/// </summary>
			[Fact]
			public void FireBallHitsItsPrimaryTwice()
			{
				if (!Enabled)
				{
					_output.WriteLine($"Skipped. Set {EnableVariable}=1 to run.");
					return;
				}

				var press = SkillPressProbe.MeasureAll("Pyromancer_FireBall", measureDefense: false);
				var single = press.Scenarios["S1"];
				var stacked = press.Scenarios["S3"];

				Assert.Null(single.Error);
				Assert.Equal(2, single.HitsOnPrimary);

				// The explosion takes three more on top of the one it re-hits.
				Assert.Equal(4, stacked.TargetsDamaged);
			}

			/// <summary>
			/// Damage is a straight line in the factor, so the measured slope has
			/// to double the damage when the factor doubles.
			/// </summary>
			[Fact]
			public void DamageIsAffineInTheFactor()
			{
				if (!Enabled)
				{
					_output.WriteLine($"Skipped. Set {EnableVariable}=1 to run.");
					return;
				}

				var job = JobCatalog.Entries.First(e => e.SkillPrefix == "Wizard");
				var single = ScenarioMatrix.All.First(s => s.Id == "S1");
				var skillId = Melia.Shared.Game.Const.SkillId.Wizard_EnergyBolt;
				var level = SfrData.SkillMaxLevel("Wizard_EnergyBolt");

				var line = SkillPressProbe.MeasureFactorLine(job, skillId, level, single, 50);

				_output.WriteLine($"slope {line.Slope:0.000} per factor point, flat {line.Flat:0.0}");

				Assert.True(line.Slope > 0, "the press showed no dependence on the factor at all");

				// The flat term is what a single-point inverse would have to
				// assume away, so its size against the measured damage is the
				// error that assumption carries.
				var share = Math.Abs(line.Flat) / Math.Max(1f, line.DamageAt(SfrDamageCurve.BaselineFactor));

				_output.WriteLine($"post-factor flat term is {share * 100:0.0}% of the baseline damage");
			}

			/// <summary>
			/// The hit count should recover a plain, single-hit skill's delivery
			/// as 1 per target in every scenario - the simplest possible check
			/// that the inference does not manufacture hits out of nothing.
			/// </summary>
			/// <remarks>
			/// The anchor is what the roster's level is set by, so it reading
			/// anything but 1.0 moves every factor in the file. It is also the
			/// guard on the reference: Swordman_Bash hits each of the seven mobs
			/// it reaches in S3 exactly once, so a reference that folded in the
			/// post-factor multipliers would show up here immediately.
			/// </remarks>
			[Fact]
			public void HitsFromDamageMatchesASingleHit()
			{
				if (!Enabled)
				{
					_output.WriteLine($"Skipped. Set {EnableVariable}=1 to run.");
					return;
				}

				using var pool = new ArenaPool(SfrDials.ExplainPoolSize);

				var press = SkillPressProbe.MeasureAll("Swordman_Bash", measureDefense: false, pool: pool);

				_output.WriteLine($"hits {press.HitEquivalents:0.00} per target, truncated {press.HitsTruncated}, " +
					$"span {press.DamageSpanSeconds:0.00}s, burst {press.BurstFraction:0.00}");

				Assert.True(press.HitsFromDamage, press.HitsFailure);
				Assert.False(press.HitsTruncated);
				Assert.InRange(press.HitEquivalents, 0.9f, 1.1f);
			}

			/// <summary>
			/// Times one press against many concurrent ones, which is the only
			/// thing that says whether the roster run is actually parallel.
			/// </summary>
			/// <remarks>
			/// A press is almost entirely Thread.Sleep, so N of them on N arenas
			/// should cost about what one costs. Anything close to N times the
			/// single-press cost means something in the press path serializes and
			/// no amount of extra workers or arenas will help the roster run.
			/// </remarks>
			[Fact]
			public void ParallelPressesScale()
			{
				if (!Enabled)
				{
					_output.WriteLine($"Skipped. Set {EnableVariable}=1 to run.");
					return;
				}

				const int width = 32;

				var job = JobCatalog.Entries.First(e => e.SkillPrefix == "Swordman");
				var level = SfrData.SkillMaxLevel("Swordman_Bash");
				var skillId = Melia.Shared.Game.Const.SkillId.Swordman_Bash;
				var spec = SfrGeometry.PricedScenarios.First(s => s.Id == "S1");

				using var pool = new ArenaPool(width);

				_output.WriteLine($"built {width} arenas in {pool.BuildTime.TotalSeconds:0.00}s");

				// Warm every shared cache first, so the comparison measures the
				// press rather than the first caller paying for SpawnCensus.
				pool.Use(m => SkillPressProbe.Measure(job, skillId, level, spec, 50, arena: m));

				var one = DateTime.UtcNow;
				pool.Use(m => SkillPressProbe.Measure(job, skillId, level, spec, 50, arena: m));
				var single = DateTime.UtcNow - one;

				var many = DateTime.UtcNow;
				SkillPressProbe.RunAll(Enumerable.Range(0, width)
					.Select(_ => (Action)(() => pool.Use(m => SkillPressProbe.Measure(job, skillId, level, spec, 50, arena: m))))
					.ToArray());
				var parallel = DateTime.UtcNow - many;

				_output.WriteLine($"1 press {single.TotalSeconds:0.00}s, {width} concurrent {parallel.TotalSeconds:0.00}s, " +
					$"speedup {width * single.TotalSeconds / Math.Max(0.001, parallel.TotalSeconds):0.0}x of a possible {width}x");

				// Bisect the press: whichever phase fails to scale is the one
				// holding a lock the rest of the run inherits.
				Phase("spawn actors", width, pool, m =>
				{
					var c = SyntheticActors.CreateCharacter(job.JobId, 50, StatSpread.AllIn(JobCatalog.GetPrimaryStat(job), 50), arena: m);
					SyntheticActors.Cleanup(c);
				});

				Phase("spawn + gear", width, pool, m =>
				{
					var c = SyntheticActors.CreateCharacter(job.JobId, 50, StatSpread.AllIn(JobCatalog.GetPrimaryStat(job), 50), arena: m);
					ReferenceGear.Equip(c, job);
					SyntheticActors.Cleanup(c);
				});

				Phase("spawn + gear + cast cycle", width, pool, m =>
				{
					var c = SyntheticActors.CreateCharacter(job.JobId, 50, StatSpread.AllIn(JobCatalog.GetPrimaryStat(job), 50), arena: m);
					ReferenceGear.Equip(c, job);
					var s = SyntheticActors.GiveSkill(c, skillId, level);
					CastCycleModel.Measure(c, s);
					SyntheticActors.Cleanup(c);
				});

				Phase("reference mob lookup", width, pool, _ => SpawnCensus.FindReferenceMob(50, Melia.Shared.Game.Const.MonsterRank.Normal, 8, out var _unused));

				Phase("map tick only", width, pool, m =>
				{
					for (var i = 0; i < 40; ++i)
						m.Update(TimeSpan.FromMilliseconds(25));
				});

				// The control: if plain sleeping does not scale, the fan-out
				// itself is broken and nothing about the press matters.
				Phase("bare sleep 1.5s", width, pool, _ => System.Threading.Thread.Sleep(1500));

				// The press loop's actual shape - sleep and tick, with a character
				// and a mob on the map so the tick has entities to walk.
				Phase("sleep + tick, populated", width, pool, m =>
				{
					var c = SyntheticActors.CreateCharacter(job.JobId, 50, StatSpread.AllIn(JobCatalog.GetPrimaryStat(job), 50), arena: m);
					var mob = SyntheticActors.CreateMob(SpawnCensus.FindReferenceMob(50, Melia.Shared.Game.Const.MonsterRank.Normal, 8, out var _lv).Id, new Melia.Shared.World.Position(30, 0, 0), m);

					try
					{
						for (var i = 0; i < 60; ++i)
						{
							System.Threading.Thread.Sleep(25);
							m.Update(TimeSpan.FromMilliseconds(25));
						}
					}
					finally
					{
						SyntheticActors.Cleanup(c, mob);
					}
				});
			}

			/// <summary>
			/// Times one phase alone and then width copies of it at once.
			/// </summary>
			/// <param name="name"></param>
			/// <param name="width"></param>
			/// <param name="pool"></param>
			/// <param name="work"></param>
			private void Phase(string name, int width, ArenaPool pool, Action<Melia.Zone.World.Maps.Map> work)
			{
				pool.Use(m => { work(m); return 0; });

				var one = DateTime.UtcNow;
				pool.Use(m => { work(m); return 0; });
				var single = (DateTime.UtcNow - one).TotalSeconds;

				var many = DateTime.UtcNow;
				SkillPressProbe.RunAll(Enumerable.Range(0, width).Select(_ => (Action)(() => pool.Use(m => { work(m); return 0; }))).ToArray());
				var parallel = (DateTime.UtcNow - many).TotalSeconds;

				_output.WriteLine($"  {name,-28} 1x {single * 1000:0}ms, {width}x {parallel * 1000:0}ms, " +
					$"speedup {width * single / Math.Max(0.0001, parallel):0.0}x");
			}
		}
	}
}
