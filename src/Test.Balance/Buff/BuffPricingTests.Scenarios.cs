using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Melia.Shared.Game.Const;
using Melia.Test.Balance.Sfr;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Xunit;
using Xunit.Abstractions;

namespace Melia.Test.Balance.Buff
{
	public partial class BuffPricingTests
	{
		/// <summary>
		/// Whether a scenario really produces the fight it declares.
		/// </summary>
		/// <remarks>
		/// BuffScenarios aims each stat axis by inverting the combat script's own
		/// chance formula, which means it carries a copy of three slopes and three
		/// clamps that live in calc_combat.cs. A copy that drifts is silent: the
		/// scenario still runs, still reports a number, and the number is measured
		/// under conditions nobody chose. These press the real functions and check
		/// the chance that comes back.
		/// </remarks>
		[Collection(BalanceCollection.Name)]
		public class Scenarios
		{
			/// <summary>
			/// How far a realised chance may sit from the one the scenario asked
			/// for, in percentage points.
			/// </summary>
			/// <remarks>
			/// Small, because nothing here is sampled - the chance is read straight
			/// out of the formula, so the only slack needed is for the stat rounding
			/// the property table does on the way through.
			/// </remarks>
			private const float Tolerance = 1.5f;

			private readonly ITestOutputHelper _output;

			/// <summary>
			/// Creates the fixture.
			/// </summary>
			/// <param name="host"></param>
			/// <param name="output"></param>
			public Scenarios(BalanceHost host, ITestOutputHelper output)
				=> _output = output;

			/// <summary>
			/// Every stat axis lands the chance its scenario declares.
			/// </summary>
			[Fact]
			public void ScenariosProduceTheChancesTheyDeclare()
			{
				if (!BalanceSuites.BuffEnabled)
				{
					_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.BuffVariable));
					return;
				}

				foreach (var scenario in BuffScenarios.All)
				{
					var (character, mob) = this.Build(scenario);

					try
					{
						if (scenario.MobDodgeChance is { } mobDodge)
							this.Check(scenario, "mob dodge", mobDodge, Dodge(character, mob));

						if (scenario.MobBlockChance is { } mobBlock)
							this.Check(scenario, "mob block", mobBlock, Block(character, mob));

						if (scenario.MobCritChance is { } mobCrit)
							this.Check(scenario, "mob critical", mobCrit, Critical(mob, character));

						if (scenario.CharacterCritChance is { } charCrit)
							this.Check(scenario, "character critical", charCrit, Critical(character, mob));

						if (scenario.CharacterDodgeChance is { } charDodge)
							this.Check(scenario, "character dodge", charDodge, Dodge(mob, character));

						if (scenario.CharacterBlockChance is { } charBlock)
							this.Check(scenario, "character block", charBlock, Block(mob, character));
					}
					finally
					{
						SyntheticActors.Cleanup(character, mob);
					}
				}
			}

			/// <summary>
			/// A scenario that declares no stat axis leaves both sides alone, bar
			/// the critical chance every scenario is held at.
			/// </summary>
			/// <remarks>
			/// The baseline has to stay the baseline. If Load moved anything the
			/// grid had not declared, every buff's level-swept reading - the one B1
			/// carries alone - would be taken under undeclared conditions. The
			/// character's critical chance is the one axis the grid does declare
			/// for every scenario, so it is checked against BaselineCritChance
			/// rather than against the untouched pair.
			/// </remarks>
			[Fact]
			public void PlainScenariosTouchNothing()
			{
				if (!BalanceSuites.BuffEnabled)
				{
					_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.BuffVariable));
					return;
				}

				var plain = BuffScenarios.All.First();

				Assert.Null(plain.MobDodgeChance);
				Assert.Null(plain.MobBlockChance);
				Assert.Null(plain.MobCritChance);
				Assert.Null(plain.CharacterCritChance);
				Assert.Null(plain.CharacterDodgeChance);
				Assert.Null(plain.CharacterBlockChance);

				var (loadedCharacter, loadedMob) = this.Build(plain);
				var (bareCharacter, bareMob) = this.Build(null);

				try
				{
					foreach (var name in new[] { PropertyName.DR, PropertyName.BLK, PropertyName.HR })
					{
						Assert.Equal(bareCharacter.Properties.GetFloat(name), loadedCharacter.Properties.GetFloat(name), 3);
						Assert.Equal(bareMob.Properties.GetFloat(name), loadedMob.Properties.GetFloat(name), 3);
					}

					Assert.Equal(bareMob.Properties.GetFloat(PropertyName.CRTHR), loadedMob.Properties.GetFloat(PropertyName.CRTHR), 3);
					this.Check(plain, "character critical", BuffScenarios.BaselineCritChance, Critical(loadedCharacter, loadedMob));
				}
				finally
				{
					SyntheticActors.Cleanup(loadedCharacter, loadedMob);
					SyntheticActors.Cleanup(bareCharacter, bareMob);
				}
			}

			/// <summary>
			/// Reports the rolls the reference pair makes with no scenario loaded,
			/// at each level the pricer sweeps.
			/// </summary>
			/// <remarks>
			/// The natural rate is what an axis reads at in every scenario that
			/// does not pin it, so it decides what a buff acting on that axis can
			/// possibly be worth. It is reported rather than asserted: these are
			/// the combat scripts' own numbers, and the point is to have them
			/// written down next to the grid that has to account for them.
			/// </remarks>
			[Fact]
			public void ReportsTheNaturalRolls()
			{
				if (!BalanceSuites.BuffEnabled)
				{
					_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.BuffVariable));
					return;
				}

				foreach (var level in ScenarioMatrix.CharacterLevels)
				{
					var job = JobCatalog.Entries.First(e => e.SkillPrefix == BuffDials.AnchorSkill.Split('_')[0]);
					var stat = JobCatalog.GetPrimaryStat(job);

					var character = SyntheticActors.CreateCharacter(job.JobId, level, StatSpread.AllIn(stat, level));
					ReferenceGear.Equip(character, job, BuffDials.GearFor(level));

					var mob = SyntheticActors.CreateMob(SfrDefenseProbe.FindHostileReferenceMob(level).Id);

					try
					{
						_output.WriteLine($"level {level}: character criticals {Critical(character, mob):0.0}%, " +
							$"mob dodges {Dodge(character, mob):0.0}%, mob blocks {Block(character, mob):0.0}%");
					}
					finally
					{
						SyntheticActors.Cleanup(character, mob);
					}
				}
			}

			/// <summary>
			/// Reports what each gear tier is actually worth, and how much of an
			/// incoming swing is physical against magical, at every level the
			/// pricer sweeps.
			/// </summary>
			/// <remarks>
			/// Both halves of the model this exists to keep honest are otherwise
			/// invisible. A defensive buff's price depends on where the character
			/// sits on attack/defense, so the tiers have to really span a range
			/// rather than being three names for similar numbers; and the incoming
			/// split is even in samples but not in damage, because Magic_Attack
			/// carries the heavier factor and MDEF and DEF are not equal. Both are
			/// reported rather than asserted, for the reason ReportsTheNaturalRolls
			/// gives: the point is to have the grid's own conditions written down
			/// instead of reasoned about from outside.
			/// </remarks>
			[Fact]
			public void ReportsTheGearTiers()
			{
				if (!BalanceSuites.BuffEnabled)
				{
					_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.BuffVariable));
					return;
				}

				var job = JobCatalog.Entries.First(e => e.SkillPrefix == BuffDials.AnchorSkill.Split('_')[0]);
				var stat = JobCatalog.GetPrimaryStat(job);

				foreach (var level in ScenarioMatrix.CharacterLevels)
				{
					var tier = BuffDials.GearFor(level);

					var character = SyntheticActors.CreateCharacter(job.JobId, level, StatSpread.AllIn(stat, level));
					var set = ReferenceGear.Equip(character, job, tier);

					var mob = SyntheticActors.CreateMob(SfrDefenseProbe.FindHostileReferenceMob(level).Id);

					try
					{
						var def = character.Properties.GetFloat(PropertyName.DEF);
						var mdef = character.Properties.GetFloat(PropertyName.MDEF);
						var patk = character.Properties.GetFloat(PropertyName.MAXPATK);
						var matk = character.Properties.GetFloat(PropertyName.MAXMATK);

						var physical = HitSampler.Sample(mob, character, new Skill(mob, SkillId.Normal_Attack, 1), 2000);
						var magical = HitSampler.Sample(mob, character, new Skill(mob, SkillId.Magic_Attack, 1), 2000);
						var total = physical.EffectiveMean + magical.EffectiveMean;
						var share = total <= 0 ? 0 : physical.EffectiveMean / total * 100;

						_output.WriteLine($"level {level} {tier}: {set}");
						_output.WriteLine($"    DEF {def:N0}  MDEF {mdef:N0}  PATK {patk:N0}  MATK {matk:N0}");
						_output.WriteLine($"    mob pAtk {mob.Data.PhysicalAttackMax:N0}  mAtk {mob.Data.MagicalAttackMax:N0}");
						_output.WriteLine($"    incoming physical {physical.EffectiveMean:N1} per hit (landed {physical.Mean:N1}, dodge {physical.DodgeRate * 100:0.0}%, block {physical.BlockRate * 100:0.0}%)");
						_output.WriteLine($"    incoming magical  {magical.EffectiveMean:N1} per hit (landed {magical.Mean:N1}, dodge {magical.DodgeRate * 100:0.0}%, block {magical.BlockRate * 100:0.0}%)");
						_output.WriteLine($"    physical is {share:0.0}% of what the character takes");
					}
					finally
					{
						SyntheticActors.Cleanup(character, mob);
					}
				}
			}

			/// <summary>
			/// Reports what each layer of the endgame loadout is worth, one at a
			/// time.
			/// </summary>
			/// <remarks>
			/// The question this answers is whether gems and cards outweigh the
			/// equipment they sit in. If they do, the gear axis is really a gem
			/// axis wearing gear's name, and a defensive buff is being priced
			/// against a character whose defenses come from somewhere the item
			/// tiers do not describe. Reported rather than asserted, for the
			/// reason ReportsTheNaturalRolls gives.
			/// </remarks>
			[Fact]
			public void ReportsWhereEndgamePowerComesFrom()
			{
				if (!BalanceSuites.BuffEnabled)
				{
					_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.BuffVariable));
					return;
				}

				var job = JobCatalog.Entries.First(e => e.SkillPrefix == BuffDials.AnchorSkill.Split('_')[0]);
				var stat = JobCatalog.GetPrimaryStat(job);
				var level = ScenarioMatrix.CharacterLevels[^1];

				string[] sets = ["Raffye", "Blint"];

				var layers = new (string Name, GearLoadout Loadout)[]
				{
					("set only", new(ItemGrade.Normal, 0, false, 0, false, sets)),
					("+ Legend, identified", new(ItemGrade.Legend, 0, true, 0, false, sets)),
					("+ refine 15", new(ItemGrade.Legend, 15, true, 0, false, sets)),
					("+ gems", new(ItemGrade.Legend, 15, true, 10, false, sets)),
					("+ cards", new(ItemGrade.Legend, 15, true, 10, true, sets)),
				};

				var previous = (Def: 0f, MDef: 0f, Dealt: 0f, Taken: 0f);

				foreach (var (name, loadout) in layers)
				{
					var character = SyntheticActors.CreateCharacter(job.JobId, level, StatSpread.AllIn(stat, level));

					try
					{
						ReferenceGear.Equip(character, job, loadout);

						var mob = SyntheticActors.CreateMob(SfrDefenseProbe.FindHostileReferenceMob(level).Id);

						try
						{
							var def = character.Properties.GetFloat(PropertyName.DEF);
							var mdef = character.Properties.GetFloat(PropertyName.MDEF);

							// Damage rather than the raw stats alone, because
							// identification rolls critical rate, resistances and
							// ADD_ bonuses as readily as it rolls DEF - reading
							// three properties makes a layer that lands on any of
							// those look like it did nothing at all.
							var dealt = HitSampler.Sample(character, mob, new Skill(character, SkillId.Normal_Attack, 1), 2000).EffectiveMean;
							var takenPhysical = HitSampler.Sample(mob, character, new Skill(mob, SkillId.Normal_Attack, 1), 2000).EffectiveMean;
							var takenMagical = HitSampler.Sample(mob, character, new Skill(mob, SkillId.Magic_Attack, 1), 2000).EffectiveMean;
							var taken = (takenPhysical + takenMagical) / 2;

							_output.WriteLine($"{name,-22} DEF {def,7:N0}  MDEF {mdef,7:N0}  " +
								$"deals {dealt,8:N1} ({Growth(dealt, previous.Dealt)})  " +
								$"takes {taken,7:N1} ({Growth(taken, previous.Taken)})");

							previous = (def, mdef, dealt, taken);
						}
						finally
						{
							SyntheticActors.Cleanup(character, mob);
						}
					}
					catch
					{
						SyntheticActors.Cleanup(character);
						throw;
					}
				}
			}

			/// <summary>
			/// Reports what each item grade is worth, holding everything else
			/// at the endgame loadout.
			/// </summary>
			/// <remarks>
			/// Grade raises nothing on an item directly - the only thing it
			/// drives is how many random options identification rolls (Magic
			/// one or two, Rare two, Unique two or three, Legend three or four,
			/// Goddess three or four), so the ladder is really a ladder of
			/// rolled bonuses and its steps are not evenly spaced.
			/// </remarks>
			[Fact]
			public void ReportsGradesAgainstEachOther()
			{
				if (!BalanceSuites.BuffEnabled)
				{
					_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.BuffVariable));
					return;
				}

				var job = JobCatalog.Entries.First(e => e.SkillPrefix == BuffDials.AnchorSkill.Split('_')[0]);
				var stat = JobCatalog.GetPrimaryStat(job);
				var level = ScenarioMatrix.CharacterLevels[^1];

				string[] sets = ["Raffye", "Blint"];
				ItemGrade[] grades = [ItemGrade.Magic, ItemGrade.Rare, ItemGrade.Unique, ItemGrade.Legend, ItemGrade.Goddess];

				var first = 0f;

				foreach (var grade in grades)
				{
					var character = SyntheticActors.CreateCharacter(job.JobId, level, StatSpread.AllIn(stat, level));
					var mob = SyntheticActors.CreateMob(SfrDefenseProbe.FindHostileReferenceMob(level).Id);

					try
					{
						// The same seed for every grade, so what moves between
						// them is the number of options identification rolls
						// and not which options it happened to roll.
						DeterministicRandom.Seed(HitSampler.DefaultSeed);
						ReferenceGear.Equip(character, job, new GearLoadout(grade, 15, true, 10, true, sets));
						DeterministicRandom.Reset();

						var dealt = Dealt(character, mob, SyntheticActors.GiveSkill(character, SkillId.Normal_Attack, 1));
						var taken = Taken(character, mob);

						if (first <= 0)
							first = dealt;

						var weapon = character.Inventory.GetItem(EquipSlot.RightHand);
						var itemAtk = weapon?.Properties.GetFloat(PropertyName.MAXATK) ?? 0;

						_output.WriteLine($"{grade,-8} weapon MAXATK {itemAtk,7:N0}  deals {dealt,8:N1} ({Growth(dealt, first)} vs Magic)  takes {taken,7:N1}");
					}
					finally
					{
						SyntheticActors.Cleanup(character, mob);
					}
				}
			}

			/// <summary>
			/// Reports a colored gem against a skill gem, on a damage skill and
			/// on a buff.
			/// </summary>
			/// <remarks>
			/// The two are not interchangeable and the comparison is the point:
			/// a colored gem grants a stat and so raises everything the
			/// character does, and it stacks - every socket can hold another
			/// one. A skill gem raises one skill by one level and does not
			/// stack with itself, so a character holds at most one per skill.
			/// The design target is that a skill gem beats a colored one on the
			/// skill it touches, but not by so much that the choice stops being
			/// a choice.
			///
			/// The skill gem is applied as GemLevel_BM on the skill, which is
			/// exactly the end state Inventory.RefreshGemSkills produces from a
			/// socketed one, without needing the socket to be wired up here.
			/// </remarks>
			[Fact]
			public void ReportsGemsAgainstSkillGems()
			{
				if (!BalanceSuites.BuffEnabled)
				{
					_output.WriteLine(BalanceSuites.SkipMessage(BalanceSuites.BuffVariable));
					return;
				}

				var job = JobCatalog.Entries.First(e => e.SkillPrefix == BuffDials.AnchorSkill.Split('_')[0]);
				var stat = JobCatalog.GetPrimaryStat(job);
				var level = ScenarioMatrix.CharacterLevels[^1];
				var skillLevel = SfrData.SkillMaxLevel("Swordman_Bash");

				string[] sets = ["Raffye", "Blint"];
				var bare = new GearLoadout(ItemGrade.Legend, 15, true, 0, true, sets);

				(float Basic, float Bash) Read(Action<Character, GearSet, Skill> apply)
				{
					var character = SyntheticActors.CreateCharacter(job.JobId, level, StatSpread.AllIn(stat, level));
					var mob = SyntheticActors.CreateMob(SfrDefenseProbe.FindHostileReferenceMob(level).Id);

					try
					{
						DeterministicRandom.Seed(HitSampler.DefaultSeed);
						var set = ReferenceGear.Equip(character, job, bare);
						DeterministicRandom.Reset();

						var basic = SyntheticActors.GiveSkill(character, SkillId.Normal_Attack, 1);
						var bash = SyntheticActors.GiveSkill(character, SkillId.Swordman_Bash, skillLevel);

						// After the skills exist, because GiveSkill rebuilds one
						// and would drop a GemLevel_BM set before it.
						apply?.Invoke(character, set, bash);

						character.Properties.InvalidateAll();
						basic.Properties.InvalidateAll();
						bash.Properties.InvalidateAll();

						return (Dealt(character, mob, basic), Dealt(character, mob, bash));
					}
					finally
					{
						SyntheticActors.Cleanup(character, mob);
					}
				}

				var none = Read(null);

				var colored = Read((character, set, _) =>
				{
					var weapon = set.Weapon;

					if (weapon == null)
						return;

					var gem = new Item(643501);
					gem.SetLevel(10);

					weapon.CreateSocket(0);
					weapon.SocketGem(gem);
				});

				var skillGem = Read((_, _, bash) =>
				{
					bash.Properties.SetFloat(PropertyName.GemLevel_BM, 1);
					bash.Properties.InvalidateAll();
				});

				// The reinforce ability at its cap, which is what an enhance
				// attribute is: 0.5% of the skill's factor per level and a
				// further 10% at 100, so 60% maxed. Only the enhance ones are
				// read here - the rest change what a skill does rather than how
				// hard it hits, and no single number describes that.
				var reinforced = Read((character, _, bash) =>
				{
					if (bash.Data.ReinforceAbility != 0)
						character.Abilities.Learn(bash.Data.ReinforceAbility, 100);
				});

				_output.WriteLine($"{"no gem",-22} basic {none.Basic,8:N1}                 Bash {none.Bash,8:N1}");
				_output.WriteLine($"{"red gem lv10 (+10 STR)",-22} basic {colored.Basic,8:N1} ({Growth(colored.Basic, none.Basic)})  Bash {colored.Bash,8:N1} ({Growth(colored.Bash, none.Bash)})");
				_output.WriteLine($"{"Bash skill gem (+1 lv)",-22} basic {skillGem.Basic,8:N1} ({Growth(skillGem.Basic, none.Basic)})  Bash {skillGem.Bash,8:N1} ({Growth(skillGem.Bash, none.Bash)})");
				_output.WriteLine($"{"Bash enhance attr 100",-22} basic {reinforced.Basic,8:N1} ({Growth(reinforced.Basic, none.Basic)})  Bash {reinforced.Bash,8:N1} ({Growth(reinforced.Bash, none.Bash)})");

				// A buff gem raises the buff's own level, so what it buys is the
				// step between one caption magnitude and the next rather than
				// any damage of its own.
				var gungHo = ZoneServer.Instance.Data.SkillDb.Find(SkillId.Swordman_GungHo);

				if (gungHo != null)
				{
					float Ratio(int lv) => gungHo.CaptionRatio1 + gungHo.CaptionRatio1ByLevel * lv;

					var cap = SfrData.SkillMaxLevel("Swordman_GungHo");

					_output.WriteLine($"{"GungHo skill gem",-22} +1 level is {Ratio(cap):0.0}% -> {Ratio(cap + 1):0.0}% attack " +
						$"({Growth(Ratio(cap + 1), Ratio(cap))} on the buff)");

					// A buff's enhance attribute multiplies the caption ratio
					// through the same reinforce rate, and every buff the
					// character holds carries its own - which is why buff
					// attributes compound where a damage skill's does not.
					_output.WriteLine($"{"GungHo enhance attr 100",-22} the same 60% on the caption: {Ratio(cap):0.0}% -> {Ratio(cap) * 1.60f:0.0}% attack (x1.60 on the buff)");
				}
			}

			/// <summary>
			/// Returns what one hit of a skill lands for.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="mob"></param>
			/// <param name="skillId"></param>
			/// <param name="skillLevel"></param>
			private static float Dealt(Character character, Mob mob, Skill skill)
				=> HitSampler.Sample(character, mob, skill, 2000).EffectiveMean;

			/// <summary>
			/// Returns what one incoming hit lands for, averaged over the two
			/// attack types the ring swings.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="mob"></param>
			private static float Taken(Character character, Mob mob)
				=> (HitSampler.Sample(mob, character, new Skill(mob, SkillId.Normal_Attack, 1), 2000).EffectiveMean
					+ HitSampler.Sample(mob, character, new Skill(mob, SkillId.Magic_Attack, 1), 2000).EffectiveMean) / 2;

			/// <summary>
			/// Formats one layer's step as a multiple of the layer below it.
			/// </summary>
			/// <param name="value"></param>
			/// <param name="previous"></param>
			private static string Growth(float value, float previous)
				=> previous <= 0 ? "base" : $"x{value / previous:0.00}";

			/// <summary>
			/// Builds the pair a scenario describes and loads it, or an unloaded
			/// pair when given no scenario.
			/// </summary>
			/// <param name="scenario"></param>
			private (Character Character, Mob Mob) Build(BuffScenario scenario)
			{
				var job = JobCatalog.Entries.First(e => e.SkillPrefix == BuffDials.AnchorSkill.Split('_')[0]);
				var stat = JobCatalog.GetPrimaryStat(job);

				var spread = scenario == null
					? StatSpread.AllIn(stat, BuffDials.ProbeLevel)
					: BuffScenarios.Spread(scenario, stat, BuffDials.ProbeLevel);

				var character = SyntheticActors.CreateCharacter(job.JobId, BuffDials.ProbeLevel, spread);
				ReferenceGear.Equip(character, job);

				var mobData = SfrDefenseProbe.FindHostileReferenceMob(BuffDials.ProbeLevel);
				var mob = SyntheticActors.CreateMob(mobData.Id);

				if (scenario != null)
					BuffScenarios.Load(scenario, character, [mob]);

				return (character, mob);
			}

			/// <summary>
			/// Reports one axis and fails if it missed.
			/// </summary>
			/// <param name="scenario"></param>
			/// <param name="axis"></param>
			/// <param name="wanted"></param>
			/// <param name="got"></param>
			private void Check(BuffScenario scenario, string axis, float wanted, float got)
			{
				_output.WriteLine($"{scenario.Id} {axis}: asked {wanted:0.0}%, got {got:0.0}%");

				Assert.InRange(got, wanted - Tolerance, wanted + Tolerance);
			}

			/// <summary>
			/// Returns the chance the target evades the attacker's basic swing.
			/// </summary>
			/// <param name="attacker"></param>
			/// <param name="target"></param>
			private static float Dodge(ICombatEntity attacker, ICombatEntity target)
				=> Roll("SCR_GetDodgeChance", attacker, target);

			/// <summary>
			/// Returns the chance the target blocks the attacker's basic swing.
			/// </summary>
			/// <param name="attacker"></param>
			/// <param name="target"></param>
			private static float Block(ICombatEntity attacker, ICombatEntity target)
				=> Roll("SCR_GetBlockChance", attacker, target);

			/// <summary>
			/// Returns the chance the attacker's basic swing criticals the target.
			/// </summary>
			/// <param name="attacker"></param>
			/// <param name="target"></param>
			private static float Critical(ICombatEntity attacker, ICombatEntity target)
				=> Roll("SCR_GetCritChance", attacker, target);

			/// <summary>
			/// Presses one of the combat script's chance functions on a basic
			/// attack.
			/// </summary>
			/// <param name="function"></param>
			/// <param name="attacker"></param>
			/// <param name="target"></param>
			private static float Roll(string function, ICombatEntity attacker, ICombatEntity target)
			{
				var skill = new Skill(attacker, SkillId.Normal_Attack, 1);

				return ScriptableFunctions.Combat.Get(function)(attacker, target, skill, SkillModifier.Default, new SkillHitResult());
			}
		}
	}
}
