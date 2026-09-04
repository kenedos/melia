using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Xunit;
using Xunit.Abstractions;

namespace Melia.Test.Balance.Sfr
{
	public partial class SfrPricingTests
	{
		/// <summary>
		/// What the purse is worth to a press that reads it, measured across
		/// the silver a player actually holds.
		/// </summary>
		/// <remarks>
		/// Opt-in: this boots a ZoneServer and presses in wall-clock time.
		///
		/// The roster pass cannot answer this on its own. Its factor probe
		/// presses at factor 100 and 200 and takes the slope, and the flat
		/// attack cancels out of HitEquivalents by construction, so a press
		/// that supplies attack of its own reads exactly like one that does
		/// not. Sweeping the purse instead of the factor is what makes the
		/// contribution visible, and the multiple this reports is what
		/// SfrDials.SkillSfrMultipliers has to invert.
		/// </remarks>
		[Collection(BalanceCollection.Name)]
		public class Silver
		{
			/// <summary>
			/// Environment variable that enables the measured runs.
			/// </summary>
			public const string EnableVariable = "BALANCE_SILVER";

			/// <summary>
			/// Character level the sweep is measured at, so only the purse
			/// varies between rows.
			/// </summary>
			private const int CharacterLevel = 50;

			/// <summary>
			/// Purses the sweep reads, spanning the anchors a player meets and
			/// two rows past the soft cap.
			/// </summary>
			private static readonly int[] Purses = [100_000, 1_000_000, 5_000_000, 50_000_000, 200_000_000];

			private readonly ITestOutputHelper _output;

			public Silver(BalanceHost host, ITestOutputHelper output)
				=> _output = output;

			private static bool Enabled => Environment.GetEnvironmentVariable(EnableVariable) == "1";

			/// <summary>
			/// Sweeps Dekatos across the purse and reports what each one is
			/// worth against the attack the pricer assumes it uses.
			/// </summary>
			[Fact]
			public void DekatosScalesWithThePurse()
			{
				if (!Enabled)
				{
					_output.WriteLine($"Skipped. Set {EnableVariable}=1 to run.");
					return;
				}

				var damage = new Dictionary<int, float>();
				var reference = 0f;

				foreach (var purse in Purses)
					damage[purse] = Press(purse, out reference);

				_output.WriteLine($"reference mitigated attack (caster's own MATK): {reference:0.0}");
				_output.WriteLine("purse            damage   x reference   x previous row");

				var previous = 0f;

				foreach (var purse in Purses)
				{
					var dealt = damage[purse];
					var step = previous > 0 ? dealt / previous : 0f;

					_output.WriteLine($"{purse,12:N0}   {dealt,8:0}   {dealt / Math.Max(1f, reference),9:0.00}x   {step,10:0.000}x");
					previous = dealt;
				}

				var full = damage[Purses[^2]];

				_output.WriteLine($"SkillSfrMultipliers value implied at the soft cap: {reference / Math.Max(1f, full):0.000}");

				// Every step up in the purse is worth strictly less than the
				// one before it, which is the whole point of the curve.
				Assert.True(damage[1_000_000] / damage[100_000] < 10f, "ten times the purse must not be worth ten times the damage");
				Assert.True(damage[50_000_000] / damage[5_000_000] < 1.1f, "past the soft cap the purse must stop mattering");
				Assert.True(damage[200_000_000] / damage[50_000_000] < 1.05f, "four times the purse past the cap must be worth almost nothing");
			}

			/// <summary>
			/// A caster who cannot pay the silver cost does not get the press
			/// at all, so the purse is a requirement and not only a scale.
			/// </summary>
			[Fact]
			public void DekatosRefusesAnEmptyPurse()
			{
				if (!Enabled)
				{
					_output.WriteLine($"Skipped. Set {EnableVariable}=1 to run.");
					return;
				}

				var dealt = Press(0, out _);

				_output.WriteLine($"damage with an empty purse: {dealt:0}");

				Assert.Equal(0f, dealt);
			}

			/// <summary>
			/// Presses Dekatos once at the given purse and returns the damage
			/// it dealt, with the attack the pricer assumes it used.
			/// </summary>
			/// <param name="silver"></param>
			/// <param name="reference"></param>
			private static float Press(int silver, out float reference)
			{
				var job = JobCatalog.Entries.First(e => e.SkillPrefix == "Pardoner");
				var arena = SyntheticActors.GetArena();
				var stat = JobCatalog.GetPrimaryStat(job);

				GameClock.Use(new VirtualClock());
				DeterministicRandom.Seed(SkillPressProbe.Seed);

				var character = SyntheticActors.CreateCharacter(job.JobId, CharacterLevel, StatSpread.AllIn(stat, CharacterLevel), arena: arena);
				ReferenceGear.Equip(character, job);
				SetPurse(character, silver);

				var level = SfrData.SkillMaxLevel("Pardoner_Dekatos");
				var skill = SyntheticActors.GiveSkill(character, SkillId.Pardoner_Dekatos, level);

				var mobData = SfrDefenseProbe.FindHostileReferenceMob(CharacterLevel);
				var mob = SyntheticActors.CreateMob(mobData.Id, new Position(30f, 0f, 0f), arena);

				SfrDefenseProbe.Fortify(mob);
				SfrDefenseProbe.Refill(character);

				character.Direction = character.Position.GetDirection(mob.Position);

				var matk = (character.Properties.GetFloat(PropertyName.MINMATK) + character.Properties.GetFloat(PropertyName.MAXMATK)) / 2f;
				reference = SfrDamageCurve.MitigatedAttack(matk, mob.Properties.GetFloat(PropertyName.MDEF));

				// Pinned so every row is read on the same factor and only the
				// purse separates them.
				using var factor = new SfrFactorScope(skill, SfrDamageCurve.BaselineFactor);
				using var recorder = new SfrPressRecorder(character);

				SfrDefenseProbe.Dispatch(skill, character, mob);

				var tick = TimeSpan.FromMilliseconds(TickMs);
				var clock = GameClock.Current;

				for (var elapsed = 0; elapsed < SettleMs; elapsed += TickMs)
				{
					SkillPressProbe.Step(clock, tick);
					arena.Update(tick);
				}

				return recorder.TotalDamage();
			}

			/// <summary>
			/// Milliseconds a press is given to land, which covers Dekatos's
			/// own wind-up several times over.
			/// </summary>
			private const int SettleMs = 3000;

			/// <summary>
			/// Clock step the press is driven at.
			/// </summary>
			private const int TickMs = 25;

			/// <summary>
			/// Replaces whatever the character is carrying with the given
			/// amount of silver.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="silver"></param>
			private static void SetPurse(Character character, int silver)
			{
				var held = character.Inventory.CountItem(ItemId.Silver);

				if (held > 0)
					character.Inventory.Remove(ItemId.Silver, held, InventoryItemRemoveMsg.Destroyed);

				if (silver > 0)
					character.Inventory.AddSilent(new Item(ItemId.Silver, silver));
			}
		}
	}
}
