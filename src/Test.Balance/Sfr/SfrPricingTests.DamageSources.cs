using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Test.Balance.Buff;
using Xunit;
using Xunit.Abstractions;

namespace Melia.Test.Balance.Sfr
{
	public partial class SfrPricingTests
	{
		/// <summary>
		/// Where a character's damage actually comes from, layer by layer and
		/// level by level.
		/// </summary>
		/// <remarks>
		/// Lives with the damage pass rather than the buff one because it is
		/// the damage pass's own question - what a point of factor is worth
		/// against everything else a character carries - and because it runs in
		/// seconds, so it can be produced alongside sfr-prices.md rather than
		/// waiting on the buff roster.
		///
		/// The buff column is measured here directly, by putting the stack up,
		/// so it needs nothing from BuffPricingTests.
		/// </remarks>
		[Collection(BalanceCollection.Name)]
		public class DamageSources
		{
			/// <summary>
			/// Character levels the progression is reported at.
			/// </summary>
			private static readonly int[] Thresholds = [10, 20, 30, 40, 50, 60, 70, 80, 90, 99];

			/// <summary>
			/// Skill the report is measured on.
			/// </summary>
			/// <remarks>
			/// The anchor's own damage press, so the report and the pass that
			/// prices it are talking about the same skill. Measured on a skill
			/// rather than a basic attack because a skill gem and an enhance
			/// attribute only touch a skill and would read as worthless
			/// otherwise.
			/// </remarks>
			private const string SkillName = "Swordman_Bash";

			private const int Samples = 2000;

			private readonly ITestOutputHelper _output;
			private readonly List<string> _lines = [];

			/// <summary>
			/// Creates the fixture.
			/// </summary>
			/// <param name="host"></param>
			/// <param name="output"></param>
			public DamageSources(BalanceHost host, ITestOutputHelper output)
				=> _output = output;

			/// <summary>
			/// Everything a character's damage is built out of, so the layers
			/// can be added one at a time.
			/// </summary>
			/// <param name="Points">Stat points spent, which is what levelling buys.</param>
			/// <param name="Equipment">Whether the level's gear is worn at all.</param>
			/// <param name="Grade">Item grade, which decides how many options identification rolls.</param>
			/// <param name="Identified">Whether the pieces are identified.</param>
			/// <param name="Refine">Reinforce level.</param>
			/// <param name="GemLevel">Colored gem level in every socket.</param>
			/// <param name="SkillGem">Whether a skill gem adds a level to the measured skill.</param>
			/// <param name="Cards">Whether three cards of each colour are equipped.</param>
			/// <param name="AbilityLevel">Level the skill's enhance attribute is held at.</param>
			/// <param name="Buffs">Whether the offensive buff stack is up.</param>
			private readonly record struct DamageBuild(int Points, bool Equipment, ItemGrade Grade,
				bool Identified, int Refine, int GemLevel, bool SkillGem, bool Cards, int AbilityLevel, bool Buffs);

			/// <summary>
			/// How far along a track a character of the given level is, as a
			/// share of that track's cap.
			/// </summary>
			/// <remarks>
			/// Deliberately not linear. The first reinforce levels, the first
			/// gem levels and the first attribute levels are cheap and everyone
			/// has them; the last ones are the long tail of the game. An
			/// exponent below one is the simplest curve with that shape, and it
			/// is one dial per track rather than a hand-written table.
			///
			/// These are assumptions, not measurements. Nothing here knows what
			/// a real character at level 40 is wearing - they are written down
			/// so the report's shape can be argued about directly.
			/// </remarks>
			/// <param name="characterLevel"></param>
			/// <param name="exponent"></param>
			private static float Progress(int characterLevel, float exponent)
				=> (float)Math.Pow(Math.Min(1f, characterLevel / 99f), exponent);

			/// <summary>
			/// What a character of the given level is assumed to have.
			/// </summary>
			/// <param name="characterLevel"></param>
			private static DamageBuild BuildFor(int characterLevel)
			{
				// A step ladder rather than a curve - nobody holds a fractional
				// grade.
				var grade = characterLevel switch
				{
					< 20 => ItemGrade.Normal,
					< 40 => ItemGrade.Magic,
					< 60 => ItemGrade.Rare,
					< 80 => ItemGrade.Unique,
					< 99 => ItemGrade.Legend,
					_ => ItemGrade.Goddess,
				};

				return new DamageBuild(
					Points: characterLevel,
					Equipment: true,
					Grade: grade,
					Identified: grade > ItemGrade.Normal,
					Refine: (int)Math.Round(15 * Progress(characterLevel, 0.5f)),
					GemLevel: (int)Math.Round(10 * Progress(characterLevel, 0.6f)),
					SkillGem: characterLevel >= 40,
					Cards: characterLevel >= 30,
					AbilityLevel: (int)Math.Round(100 * Progress(characterLevel, 0.5f)),
					Buffs: true);
			}

			/// <summary>
			/// Reports what share of a character's damage each source is
			/// responsible for, at every level threshold.
			/// </summary>
			/// <remarks>
			/// A build-up rather than a leave-one-out, and that is a correction:
			/// the layers nest, so taking them away one at a time double-counts.
			/// Removing the armour also removed its rolled options and its
			/// socketed gems while a separate row was removing identification
			/// from every piece, so neither row measured what it was labelled.
			/// Added in order each step is disjoint and the shares sum to
			/// exactly 100%, at the cost of the order being a choice - it is the
			/// order a character really acquires them in.
			///
			/// Each level is read against a monster of that level, so a share is
			/// what the source is worth against the content of its own bracket.
			///
			/// Grade is not a row of its own, because it is not independent: it
			/// decides how many options identification rolls and it scales base
			/// attack and defense through basicRatio, so it sits inside those
			/// two steps. The grade ladder itself is reported separately by
			/// BuffPricingTests.Scenarios.ReportsGradesAgainstEachOther.
			/// </remarks>
			[Fact]
			public void ReportsWhereDamageComesFrom()
			{
				if (!BalanceSuites.SfrEnabled)
				{
					_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.SfrVariable));
					return;
				}

				var skillId = Enum.Parse<SkillId>(SkillName);
				var job = JobCatalog.Entries.First(e => e.SkillPrefix == SkillName.Split('_')[0]);
				var stat = JobCatalog.GetPrimaryStat(job);
				var skillLevel = SfrData.SkillMaxLevel(SkillName);

				string[] sets = ["Raffye", "Blint"];

				float Measure(DamageBuild build, int characterLevel)
				{
					var spread = build.Points > 0 ? StatSpread.AllIn(stat, build.Points) : new StatSpread();
					var character = SyntheticActors.CreateCharacter(job.JobId, characterLevel, spread);
					var mob = SyntheticActors.CreateMob(SfrDefenseProbe.FindHostileReferenceMob(characterLevel).Id);

					try
					{
						// One seed for every step, so what moves between two
						// builds is the layer and not which options were rolled.
						DeterministicRandom.Seed(HitSampler.DefaultSeed);

						if (build.Equipment)
							ReferenceGear.Equip(character, job, new GearLoadout(build.Grade, build.Refine, build.Identified, build.GemLevel, build.Cards, sets));

						DeterministicRandom.Reset();

						var skill = SyntheticActors.GiveSkill(character, skillId, skillLevel);

						if (build.SkillGem)
							skill.Properties.SetFloat(PropertyName.GemLevel_BM, 1);

						if (build.AbilityLevel > 0 && skill.Data.ReinforceAbility != 0)
							character.Abilities.Learn(skill.Data.ReinforceAbility, build.AbilityLevel);

						if (build.Buffs)
						{
							character.StartBuff(BuffId.GungHo, 5, 0, TimeSpan.FromHours(1), character, SkillId.Swordman_GungHo);
							character.StartBuff(BuffId.Blessing_Buff, 15, 0, TimeSpan.FromHours(1), character, SkillId.Priest_Blessing);
							character.StartBuff(BuffId.SwellHands_Buff, 10, character.Properties.GetFloat(PropertyName.INT), TimeSpan.FromHours(1), character, SkillId.Thaumaturge_SwellHands);
						}

						character.Properties.InvalidateAll();
						skill.Properties.InvalidateAll();

						return HitSampler.Sample(character, mob, skill, Samples).EffectiveMean;
					}
					finally
					{
						SyntheticActors.Cleanup(character, mob);
					}
				}

				var names = new[]
				{
					"levelling stats", "equipment base", "identification", "reinforce",
					"colored gems", "skill gem", "boss cards", "enhance attribute", "buffs",
				};

				_lines.Add("# Where a character's damage comes from, by level");
				_lines.Add("");
				_lines.Add($"Damage per hit of `{SkillName}` at skill level {skillLevel}, each level read against a monster of that level.");
				_lines.Add("");
				_lines.Add("Each cell is the multiplier that layer puts on the damage, and in brackets its share of the whole climb - `ln(step / previous)` over `ln(full / bare)`, which sums to 100%.");
				_lines.Add("");
				_lines.Add("**Read the multiplier, not the share.** The bare baseline is a character of that level with no stat points and no equipment, which floors at almost no damage, so the climb out of it is enormous and every later layer's share of it is small by construction. A buff stack worth a real multiplier still reads as a few percent of a span that starts at nothing. The share says how much of the distance from nothing to full a layer covers; the multiplier says what turning it off would cost.");
				_lines.Add("");
				_lines.Add("Gear is assumed, not measured. Reinforce and attributes follow `cap * (level/99)^0.5` and gems `^0.6`, so the early levels of each come quickly and the last ones do not. Grade steps Normal / Magic / Rare / Unique / Legend / Goddess at 20 / 40 / 60 / 80 / 99, cards arrive at 30 and a skill gem at 40.");
				_lines.Add("");
				_lines.Add("| level | loadout | damage, bare -> full | " + string.Join(" | ", names) + " |");
				_lines.Add("|---" + string.Concat(Enumerable.Repeat("|---", names.Length + 2)) + "|");

				foreach (var characterLevel in Thresholds)
				{
					var target = BuildFor(characterLevel);
					var bare = new DamageBuild(0, false, ItemGrade.Normal, false, 0, 0, false, false, 0, false);

					var chain = new List<DamageBuild> { bare };
					var at = bare;

					chain.Add(at = at with { Points = target.Points });
					chain.Add(at = at with { Equipment = true });
					chain.Add(at = at with { Grade = target.Grade, Identified = target.Identified });
					chain.Add(at = at with { Refine = target.Refine });
					chain.Add(at = at with { GemLevel = target.GemLevel });
					chain.Add(at = at with { SkillGem = target.SkillGem });
					chain.Add(at = at with { Cards = target.Cards });
					chain.Add(at = at with { AbilityLevel = target.AbilityLevel });
					chain.Add(at with { Buffs = target.Buffs });

					var readings = chain.Select(b => Measure(b, characterLevel)).ToArray();
					var total = readings[0] > 0 ? Math.Log(readings[^1] / readings[0]) : 0;

					var shares = Enumerable.Range(0, names.Length)
						.Select(i => readings[i] > 0 && total > 0 ? Math.Log(readings[i + 1] / readings[i]) / total * 100 : 0)
						.ToArray();

					var loadout = $"{target.Grade} +{target.Refine}, gem {target.GemLevel}, attr {target.AbilityLevel}";

					var cells = Enumerable.Range(0, names.Length).Select(i =>
					{
						var step = readings[i] > 0 ? readings[i + 1] / readings[i] : 1;

						return $"x{step:0.00} ({shares[i]:0.0}%)";
					});

					_lines.Add($"| {characterLevel} | {loadout} | {readings[0]:N0} -> {readings[^1]:N0} | " +
						string.Join(" | ", cells) + " |");
				}

				foreach (var line in _lines)
					_output.WriteLine(line);

				_output.WriteLine($"report -> {this.SaveReport("damage-sources.md")}");
			}

			/// <summary>
			/// Writes the collected lines to the balance log directory.
			/// </summary>
			/// <param name="name"></param>
			private string SaveReport(string name)
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
