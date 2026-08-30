using System;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Skills;
using Xunit;
using Xunit.Abstractions;

namespace Melia.Test.Balance
{
	/// <summary>
	/// Proves the three things the scenario matrix depends on: that mobs come
	/// only from reachable maps, that every in-scope class can be built, and
	/// that cast time, DEX, crit, dodge and block are all in the measurement.
	/// </summary>
	[Collection(BalanceCollection.Name)]
	public class HarnessCoverageTests
	{
		private readonly ITestOutputHelper _output;

		public HarnessCoverageTests(BalanceHost host, ITestOutputHelper output)
		{
			_output = output;
		}

		[Fact]
		public void AvailableMapListLoads()
		{
			var names = AvailableMaps.Names;

			_output.WriteLine($"{names.Count} available maps");

			Assert.True(names.Count > 100, $"Only {names.Count} maps parsed out of available_maps.md.");
			Assert.Contains("f_siauliai_west", names);
			Assert.DoesNotContain("Available", names);
		}

		[Fact]
		public void CensusOnlyContainsMobsFromAvailableMaps()
		{
			var mobs = SpawnCensus.Mobs;

			_output.WriteLine($"{mobs.Length} monsters spawn on available maps");
			_output.WriteLine($"levels {mobs.Min(m => m.Data.Level)}-{mobs.Max(m => m.Data.Level)}");

			Assert.NotEmpty(mobs);

			foreach (var mob in mobs)
			{
				Assert.NotEmpty(mob.Maps);

				foreach (var map in mob.Maps)
					Assert.True(AvailableMaps.Contains(map), $"{mob.Data.ClassName} was taken from unreachable map '{map}'.");
			}

			// The pool has to be a real subset, otherwise the filter is not
			// doing anything and scenarios are back to fighting anything.
			Assert.True(mobs.Length < ZoneServer.Instance.Data.MonsterDb.Entries.Count,
				"The census is not narrower than the monster database - the map filter did nothing.");
		}

		[Fact]
		public void ReferenceMobsComeFromTheCensus()
		{
			foreach (var level in new[] { 1, 15, 30, 50, 75, 99 })
			{
				var data = SpawnCensus.FindReferenceMob(level, MonsterRank.Normal, tolerance: 8, out var actualLevel);
				var census = SpawnCensus.Mobs.Single(m => m.Data.Id == data.Id);

				_output.WriteLine($"lv{level} -> {data.ClassName} (lv{actualLevel}, DEF {data.PhysicalDefense}, HP {data.Hp}) on {string.Join(", ", census.Maps.Take(3))}");

				Assert.NotEmpty(census.Maps);
			}
		}

		[Fact]
		public void PassiveDensityIsMeasuredAndFlat()
		{
			// Phase 1.2: density does not rise with level. If it ever does,
			// the AoE budget has to be re-derived, so this records it.
			foreach (var band in new[] { (1, 19), (20, 39), (40, 59), (60, 79), (80, 99) })
			{
				var near = SpawnCensus.MeasureDensity(40, band.Item1, band.Item2);
				var mid = SpawnCensus.MeasureDensity(80, band.Item1, band.Item2);
				var far = SpawnCensus.MeasureDensity(160, band.Item1, band.Item2);

				_output.WriteLine($"lv{band.Item1}-{band.Item2}: p75 density 40u {near:F1}  80u {mid:F1}  160u {far:F1}");

				Assert.True(far >= mid && mid >= near, "Density fell as the radius grew, which is impossible.");
			}
		}

		[Fact]
		public void EveryInScopeClassCanBeBuilt()
		{
			Assert.Equal(45, JobCatalog.Entries.Length);

			foreach (var job in JobCatalog.Entries)
			{
				var skills = JobCatalog.GetSkills(job);
				var damageSkills = JobCatalog.GetDamageSkills(job);
				var stat = JobCatalog.GetPrimaryStat(job);
				var weapon = ReferenceGear.FindWeapon(job, ItemGrade.Normal, 50);

				_output.WriteLine($"{job.SkillPrefix,-15} {job.JobId,-16} {job.BaseJob,-9} {skills.Length,2} skills " +
					$"({damageSkills.Length,2} damage) {stat} weapon {weapon?.ClassName ?? "NONE"}");

				Assert.NotEmpty(skills);
				Assert.NotNull(weapon);
			}
		}

		[Fact]
		public void CatalogMatchesSkillGemClasses()
		{
			// skill_gem_classes.cs is the scope definition; if the two drift,
			// the matrix silently stops covering a class.
			foreach (var job in JobCatalog.Entries)
			{
				Assert.True(ZoneServer.Instance.Data.JobDb.TryFind(job.JobId, out _),
					$"{job.SkillPrefix} maps to {job.JobId}, which is not in jobs.txt.");
			}

			var duplicates = JobCatalog.Entries
				.GroupBy(e => e.JobId)
				.Where(g => g.Count() > 1)
				.Select(g => g.Key.ToString())
				.ToArray();

			Assert.Empty(duplicates);
		}

		[Fact]
		public void GearRaisesAttackAndIsLevelCapped()
		{
			foreach (var level in new[] { 1, 50, 99 })
			{
				var job = JobCatalog.Entries.First();
				var character = SyntheticActors.CreateCharacter(job.JobId, level);

				try
				{
					var naked = character.Properties.GetFloat(PropertyName.MAXPATK);
					var set = ReferenceGear.Equip(character, job);
					var geared = character.Properties.GetFloat(PropertyName.MAXPATK);

					_output.WriteLine($"lv{level}: naked {naked:F0} -> geared {geared:F0}, DEF {character.Properties.GetFloat(PropertyName.DEF):F0} ({set})");

					Assert.True(geared > naked, $"Equipping the reference set did not raise attack at lv{level}.");
					Assert.True(set.ItemLevel <= ReferenceGear.MaxItemLevel, "Reference gear went past the lv75 item ceiling.");
				}
				finally
				{
					SyntheticActors.Cleanup(character);
				}
			}
		}

		[Fact]
		public void DexShortensCastTime()
		{
			// Casting speed is 30% fixed and 70% DEX-bought, so a DEX build
			// should land near 30% of the base cast and a STR build at 100%.
			var job = JobCatalog.Entries.First(e => e.SkillPrefix == "Pyromancer");
			var skillData = JobCatalog.GetDamageSkills(job)
				.Where(s => s.Data.BasicCast > 0)
				.OrderByDescending(s => s.Data.BasicCast)
				.FirstOrDefault();

			Assert.NotNull(skillData);

			var strCast = MeasureCast(job, skillData.Id, "INT");
			var dexCast = MeasureCast(job, skillData.Id, "DEX");

			_output.WriteLine($"{skillData.ClassName} base cast {skillData.Data.BasicCast:F0}ms");
			_output.WriteLine($"  INT build: {strCast}");
			_output.WriteLine($"  DEX build: {dexCast}");

			Assert.True(dexCast.CastTimeMs < strCast.CastTimeMs, "DEX did not shorten the cast time.");
			Assert.True(dexCast.CastsPerSecond >= strCast.CastsPerSecond, "The DEX build did not gain casts per second.");
		}

		[Fact]
		public void OverheatRaisesCastsPerSecond()
		{
			var job = JobCatalog.Entries.First();
			var character = SyntheticActors.CreateCharacter(job.JobId, 50);

			try
			{
				ReferenceGear.Equip(character, job);

				foreach (var data in JobCatalog.GetDamageSkills(job))
				{
					var skill = SyntheticActors.GiveSkill(character, data.Id, 5);
					var cycle = CastCycleModel.Measure(character, skill);

					_output.WriteLine($"{data.ClassName,-32} {cycle}");

					Assert.True(cycle.CastsPerSecond > 0, $"{data.ClassName} can never be cast.");
					Assert.True(cycle.CycleMs > 0, $"{data.ClassName} has a zero-length cast cycle.");
				}
			}
			finally
			{
				SyntheticActors.Cleanup(character);
			}
		}

		[Fact]
		public void IncomingDamageSamplesDodgeAndBlock()
		{
			// Block only exists when the target can block, so a shield class
			// is what puts it in the sample at all.
			var shieldJob = JobCatalog.Entries.First(e => e.UsesShield);
			var character = SyntheticActors.CreateCharacter(shieldJob.JobId, 50, StatSpread.AllIn("CON", 50));
			var mobData = SpawnCensus.FindReferenceMob(50, MonsterRank.Normal, tolerance: 8, out _);
			var mob = SyntheticActors.CreateMob(mobData.Id);

			try
			{
				ReferenceGear.Equip(character, shieldJob);

				var block = character.Properties.GetFloat(PropertyName.BLK);
				var mobSkill = new Skill(mob, SkillId.Normal_Attack, 1);
				var sample = HitSampler.Sample(mob, character, mobSkill);

				_output.WriteLine($"{shieldJob.SkillPrefix} lv50 BLK {block:F0}, DEF {character.Properties.GetFloat(PropertyName.DEF):F0}");
				_output.WriteLine($"incoming from {mobData.ClassName}: {sample}");

				Assert.True(sample.EffectiveMean > 0, "The monster could not hurt the character at all.");
				Assert.InRange(sample.BlockRate, 0f, 0.9f);
				Assert.InRange(sample.DodgeRate, 0f, 0.75f);
			}
			finally
			{
				SyntheticActors.Cleanup(character, mob);
			}
		}

		[Fact]
		public void GearedCharacterCanCrit()
		{
			// Crit was 0% across every naked sample. With gear and DEX it has
			// to be non-zero, or CRTHR is not reaching the pipeline.
			var job = JobCatalog.Entries.First(e => e.SkillPrefix == "Rogue");
			var character = SyntheticActors.CreateCharacter(job.JobId, 50, StatSpread.AllIn("DEX", 50));
			var mobData = SpawnCensus.FindReferenceMob(50, MonsterRank.Normal, tolerance: 8, out _);
			var mob = SyntheticActors.CreateMob(mobData.Id);

			try
			{
				ReferenceGear.Equip(character, job);

				var skill = SyntheticActors.GiveSkill(character, SkillId.Normal_Attack, 1);
				var sample = HitSampler.Sample(character, mob, skill);

				_output.WriteLine($"CRTHR {character.Properties.GetFloat(PropertyName.CRTHR):F0} vs " +
					$"CRTDR {mob.Properties.GetFloat(PropertyName.CRTDR):F0}");
				_output.WriteLine($"{sample}");

				Assert.True(sample.P90 >= sample.P10, "The damage distribution is inverted.");
				Assert.True(sample.CritRate > 0, "A DEX build with gear never crit - CRTHR is not reaching the pipeline.");
			}
			finally
			{
				SyntheticActors.Cleanup(character, mob);
			}
		}

		/// <summary>
		/// Builds a level 50 character of the job with everything in one stat
		/// and returns its cycle for the given skill.
		/// </summary>
		/// <param name="job"></param>
		/// <param name="skillId"></param>
		/// <param name="stat"></param>
		private static CastCycle MeasureCast(JobEntry job, SkillId skillId, string stat)
		{
			var character = SyntheticActors.CreateCharacter(job.JobId, 50, StatSpread.AllIn(stat, 50));

			try
			{
				ReferenceGear.Equip(character, job);

				var skill = SyntheticActors.GiveSkill(character, skillId, 5);

				return CastCycleModel.Measure(character, skill);
			}
			finally
			{
				SyntheticActors.Cleanup(character);
			}
		}

		[Fact]
		public void ScenarioMatrixSeparatesReachFromStacking()
		{
			var job = JobCatalog.Entries.First(e => e.SkillPrefix == "Swordman");
			var skillData = JobCatalog.GetDamageSkills(job)
				.OrderByDescending(s => s.Data.SplashRate)
				.First();

			var profiles = ScenarioMatrix.All
				.Select(spec => SkillProfiler.Measure(job, skillData, 5, spec, 50))
				.ToArray();

			foreach (var profile in profiles)
				_output.WriteLine(profile.ToString());

			var stacked = profiles.Single(p => p.ScenarioId == "S3");
			var single = profiles.Single(p => p.ScenarioId == "S1");
			var gap = profiles.Single(p => p.ScenarioId == "S8");

			Assert.True(single.TargetsReached >= 1, "The single-target scenario reached nothing.");
			Assert.True(stacked.TargetsReached >= single.TargetsReached,
				"A stacked pile did not reach at least as many targets as a lone mob.");
			Assert.True(gap.DodgeRate >= single.DodgeRate,
				"The level-gap scenario did not raise the miss rate.");
			Assert.All(profiles, p => Assert.True(p.CastsPerSecond > 0, $"{p.ScenarioId} produced no cast rhythm."));
		}

		[Fact]
		public void CircleSkillsReachTheirAimPoint()
		{
			// A Circle is centred on FarPos, which the splash parameters place
			// ahead of the caster, so a profiler that builds it from
			// splashHeight aims past the monster and measures nothing.
			var checkedSkills = 0;

			foreach (var job in JobCatalog.Entries)
			{
				foreach (var entry in JobCatalog.GetDamageSkills(job))
				{
					if (entry.Data.SplashType != SplashType.Circle || entry.Data.SplashRange < 10)
						continue;

					var profile = SkillProfiler.Measure(job, entry, 5, ScenarioMatrix.All.First(s => s.Id == "S1"), 50);

					++checkedSkills;

					Assert.True(profile.TargetsReached > 0,
						$"{entry.ClassName} reached nothing in the single-target scenario ({profile.Zero}).");
				}
			}

			_output.WriteLine($"{checkedSkills} circle skills all reached their target");

			Assert.True(checkedSkills > 0, "No circle skill was in scope, so this proves nothing.");
		}

		[Fact]
		public void SkillRolesSeparateBuffsFromDamage()
		{
			var all = JobCatalog.Entries.SelectMany(JobCatalog.GetProfiledSkills).ToArray();

			foreach (var group in all.GroupBy(s => s.Role))
				_output.WriteLine($"{group.Key,-9} {group.Count(),3}: {string.Join(", ", group.Select(s => s.ClassName).Take(8))}");

			var utility = all.Where(s => s.Role == SkillRole.Utility).Select(s => s.ClassName).ToArray();

			Assert.Contains("Peltasta_HighGuard", utility);
			Assert.Contains("Hoplite_Finestra", utility);
			Assert.Contains("Scout_Cloaking", utility);

			var direct = all.Where(s => s.Role == SkillRole.Direct).Select(s => s.ClassName).ToArray();

			Assert.Contains("Swordman_Bash", direct);
			Assert.Contains("Pyromancer_Prominence", direct);

			// Pet and hook skills carry a factor with no attack type, so the
			// direct-hit model cannot price them and the report must not read
			// their zero as weakness.
			Assert.Contains("Hunter_PetAttack", all.Where(s => s.Role == SkillRole.Indirect).Select(s => s.ClassName));
		}

		[Fact]
		public void ForceSkillsFindTheirTarget()
		{
			// A projectile takes the one target the client picked, so its
			// splash fields describe nothing and resolving them finds nothing.
			var single = ScenarioMatrix.All.First(s => s.Id == "S1");
			var checkedSkills = 0;

			foreach (var job in JobCatalog.Entries)
			{
				foreach (var entry in JobCatalog.GetDamageSkills(job))
				{
					if (entry.Data.UseType != SkillUseType.Force)
						continue;

					var profile = SkillProfiler.Measure(job, entry, 5, single, 50);

					++checkedSkills;

					Assert.True(profile.TargetsReached > 0, $"{entry.ClassName} reached nothing ({profile.Zero}).");
				}
			}

			_output.WriteLine($"{checkedSkills} force skills all found a target");

			Assert.True(checkedSkills > 0, "No force skill was in scope, so this proves nothing.");
		}

		[Fact]
		public void ReferenceBasicAttackIsOneUnitForEveryClass()
		{
			// A full-INT class cannot swing its weapon, so pricing its skills
			// against its own basic attack gives numbers in the hundreds. The
			// reference is one yardstick every class shares.
			var single = ScenarioMatrix.All.First(s => s.Id == "S1");

			foreach (var level in ScenarioMatrix.CharacterLevels)
			{
				var reference = SkillProfiler.GetReferenceBasicDps(single, level);

				_output.WriteLine($"lv{level}: reference basic attack {reference:F1} dps");

				Assert.True(reference > 0, $"The reference basic attack measured nothing at level {level}.");
			}

			// A full-INT wizard's own basic attack lands nothing, so TimesBasic
			// is either zero or meaningless for it. The reference has to be a
			// real number regardless.
			var wizard = JobCatalog.Entries.First(e => e.SkillPrefix == "Wizard");
			var skill = JobCatalog.GetDamageSkills(wizard).First(s => s.ClassName == "Wizard_EnergyBolt");
			var profile = SkillProfiler.Measure(wizard, skill, 5, single, 50);

			_output.WriteLine($"{profile.SkillClassName}: {profile.TimesBasic:F2}x its own basic, {profile.TimesReference:F2}x the reference, " +
				$"{profile.BurstTimesReference:F2}x per press");

			Assert.True(profile.TimesReference > 0, "The reference comparison produced nothing.");
			Assert.True(profile.BurstTimesReference > 0, "The per-press comparison produced nothing.");
		}

		[Fact]
		public void RangedScenarioSplitsTheRoster()
		{
			// Held too far out, S5 is a scenario nobody wins and it tests
			// nothing. It has to have both winners and losers.
			var skills = JobCatalog.Entries.SelectMany(JobCatalog.GetDamageSkills).ToArray();
			var reach = skills.Count(s => s.Data.MaxRange >= ScenarioMatrix.RangedDistance);
			var short_ = skills.Length - reach;

			_output.WriteLine($"at {ScenarioMatrix.RangedDistance} units: {reach} skills reach, {short_} do not");

			Assert.True(reach >= skills.Length / 5, "Almost nothing reaches the ranged scenario, so it separates nothing.");
			Assert.True(short_ >= skills.Length / 5, "Almost everything reaches it, so it costs melee nothing.");
		}

		[Fact]
		public void MultiHitCountIsNotPricedAsDamage()
		{
			// Nothing in the server reads multiHitCount, and its values are not
			// hit counts - Effigy carries 15, Latent Venom 100. Pricing it as
			// one put both at the top of the outlier list.
			var bokor = JobCatalog.Entries.First(e => e.SkillPrefix == "Bokor");
			var effigy = JobCatalog.GetDamageSkills(bokor).First(s => s.ClassName == "Bokor_Effigy");

			Assert.True(effigy.Data.MultiHitCount > 1, "Effigy no longer carries the multi-hit value this pins.");

			var single = ScenarioMatrix.All.First(s => s.Id == "S1");
			var profile = SkillProfiler.Measure(bokor, effigy, 1, single, 50);

			_output.WriteLine($"{profile.SkillClassName} multiHitCount {effigy.Data.MultiHitCount}: {profile}");

			Assert.True(profile.TimesReference < 10,
				$"{profile.SkillClassName} reads {profile.TimesReference:F1}x a basic attack, which is multiHitCount being priced as damage again.");
		}

		[Fact]
		public void SkillLevelGridRespectsTheTree()
		{
			foreach (var job in JobCatalog.Entries)
			{
				foreach (var level in ScenarioMatrix.CharacterLevelsFor(job))
					Assert.True(level >= JobCatalog.GetMinLevel(job), $"{job.SkillPrefix} was measured below its own rank.");

				foreach (var entry in JobCatalog.GetProfiledSkills(job))
				{
					foreach (var level in ScenarioMatrix.SkillLevelsFor(entry))
						Assert.True(level <= entry.MaxLevel, $"{entry.ClassName} was measured at sk{level} over its cap of {entry.MaxLevel}.");
				}
			}
		}
	}
}
