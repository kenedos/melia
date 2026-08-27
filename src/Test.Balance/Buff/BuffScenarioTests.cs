using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Test.Balance.Sfr;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Xunit;
using Xunit.Abstractions;

namespace Melia.Test.Balance.Buff
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
	public class BuffScenarioTests
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
		public BuffScenarioTests(BalanceHost host, ITestOutputHelper output)
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
				ReferenceGear.Equip(character, job);

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
