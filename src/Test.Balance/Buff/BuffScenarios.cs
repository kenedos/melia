using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;

namespace Melia.Test.Balance.Buff
{
	/// <summary>
	/// One set of conditions a buff is measured under.
	/// </summary>
	/// <remarks>
	/// The damage pass has ScenarioMatrix, whose axes are geometry - how many
	/// targets a press reaches and how far away they are. None of that is what
	/// separates one buff from another. What separates them is whether the
	/// fight contains the thing the buff acts on: a hit-rate buff is worth
	/// nothing against something that never evades, a block-penetration buff
	/// nothing against something that never blocks, and a critical-rate buff
	/// reads differently on a character already near the top of that curve.
	/// So the axes here are the two sides' stats, and the party.
	///
	/// Every stat axis is declared as the chance it is meant to produce rather
	/// than as a flat bonus, because all three rolls are a clamped linear gap -
	/// dodge is (DR - HR) x 0.4 capped at 90, block (BLK - BLK_BREAK) x 0.5
	/// capped at 60, critical (CRTHR - CRTDR) x 0.5 capped at 100. A flat bonus
	/// large enough to make the axis exist at one character level saturates the
	/// clamp at another, and a saturated clamp is not a fight: at a block chance
	/// near its cap every swing blocks, and a blocked hit cannot critical at
	/// all, so a critical buff measured there reads exactly 1.000 and drags its
	/// price up by a scenario's whole weight.
	/// </remarks>
	public class BuffScenario
	{
		public string Id { get; init; }
		public string Name { get; init; }

		/// <summary>
		/// Share of the character's stat points going to its secondary stat,
		/// where 0 is the all-in build the damage pass measures.
		/// </summary>
		public float SecondaryShare { get; init; }

		/// <summary>
		/// Chance the monsters evade the character's swing, so a hit-rate buff
		/// has something to overcome.
		/// </summary>
		public float? MobDodgeChance { get; init; }

		/// <summary>
		/// Chance the monsters block the character's swing, so a
		/// block-penetration buff has something to break.
		/// </summary>
		/// <remarks>
		/// Kept well under the 60% clamp. A blocked hit cannot critical, so a
		/// scenario that blocks nearly everything silently zeroes every
		/// critical buff measured in it rather than discounting it.
		/// </remarks>
		public float? MobBlockChance { get; init; }

		/// <summary>
		/// Chance a monster's swing criticals the character, so a
		/// critical-resistance buff has something to resist.
		/// </summary>
		public float? MobCritChance { get; init; }

		/// <summary>
		/// Chance the character's own swing criticals, so a buff that acts on
		/// a critical is read at a known point on that curve rather than
		/// wherever the reference build happens to land.
		/// </summary>
		/// <remarks>
		/// The axis a critical-damage buff lives or dies on, and the one the
		/// grid used to have no control over at all. Its worth is very nearly
		/// linear in this number: at 10% it is a rounding error and at 90% it
		/// is most of the character's damage, so measuring it at one unknown
		/// point sets its price by an accident of the reference gear.
		/// </remarks>
		public float? CharacterCritChance { get; init; }

		/// <summary>
		/// Chance the character evades an incoming swing, so a dodge buff is
		/// read somewhere on its curve rather than only from zero.
		/// </summary>
		public float? CharacterDodgeChance { get; init; }

		/// <summary>
		/// Chance the character blocks an incoming swing, so a block buff is
		/// read somewhere on its curve rather than only from zero.
		/// </summary>
		public float? CharacterBlockChance { get; init; }

		/// <summary>
		/// Characters in the party, all of them attacking and all of them
		/// holding the buff.
		/// </summary>
		/// <remarks>
		/// The offensive reading is the party's total damage, so a buff that
		/// lands on four characters is worth four times what the same
		/// percentage is worth on one. That is the whole difference between
		/// Priest_Blessing and Swordman_GungHo, and it is why a party buff
		/// prices to a fraction of a self buff's magnitude.
		/// </remarks>
		public int PartySize { get; init; } = 1;

		/// <summary>
		/// Whether the character presses a damage skill instead of swinging its
		/// basic attack.
		/// </summary>
		/// <remarks>
		/// Every other scenario measures a basic-attack rotation, which reads a
		/// buff that only touches basic attacks - QuarrelShooter_RunningShot,
		/// Scout_DoubleAttack - as though it buffed everything the character
		/// does. A skill rotation is the other half of that: it is where those
		/// buffs are worth nothing and where a buff on skill damage is worth
		/// what it claims.
		/// </remarks>
		public bool UseSkill { get; init; }

		/// <summary>
		/// Weight this scenario carries in the blended value.
		/// </summary>
		/// <remarks>
		/// One each. A scenario is in the set because it is a fight the game
		/// really contains, and there is no measurement that says one of them
		/// happens more often than another - a weight would be a guess about
		/// how the game is played wearing a number's clothes.
		/// </remarks>
		public float Weight { get; init; } = 1f;

		public override string ToString()
			=> $"{this.Id} {this.Name}";
	}

	/// <summary>
	/// The conditions every buff is measured under, and the weights they carry.
	/// </summary>
	public static class BuffScenarios
	{
		/// <summary>
		/// Slopes the three rolls turn a stat gap into a chance on, matching
		/// SCR_GetDodgeChance, SCR_GetBlockChance and SCR_GetCritChance.
		/// </summary>
		/// <remarks>
		/// Duplicated from calc_combat.cs rather than called into it, because
		/// the scripts resolve a chance from two live entities and what is
		/// needed here is the inverse - the stat that produces a wanted chance.
		/// A change to either formula's slope has to be mirrored here, which is
		/// what SlopesMatchTheCombatScript asserts.
		/// </remarks>
		public const float DodgeSlope = 0.4f;
		public const float BlockSlope = 0.5f;
		public const float CritSlope = 0.5f;

		/// <summary>
		/// Chance a "the enemy does this" scenario aims for.
		/// </summary>
		/// <remarks>
		/// Deliberately well short of every clamp. The point of these scenarios
		/// is that the roll exists and sometimes goes the other way, not that it
		/// always lands - a saturated roll measures a buff against a wall rather
		/// than against a fight, and for block it removes criticals from the
		/// scenario entirely.
		/// </remarks>
		public const float LoadedChance = 40f;

		/// <summary>
		/// Chance a monster's swing criticals in the critical-enemy scenario.
		/// </summary>
		public const float LoadedCritChance = 60f;

		/// <summary>
		/// The two ends of the critical curve a critical buff is read across.
		/// </summary>
		/// <remarks>
		/// Under 20 and over 80, so a buff acting on a critical is priced on
		/// both the build that barely criticals and the build that nearly
		/// always does, rather than on whatever single point the reference gear
		/// produces.
		/// </remarks>
		public const float LowCritChance = 10f;
		public const float HighCritChance = 90f;

		/// <summary>
		/// Chance the enemy evades in the scenario accuracy is priced on.
		/// </summary>
		/// <remarks>
		/// Heavier than LoadedChance, and it has to be. An accuracy buff can
		/// be worth at most `1 / (1 - dodge)` in a scenario and exactly 1.000
		/// in one the enemy never evades, so against an unweighted mean its
		/// ceiling is set by how much dodge the whole grid contains. At B3's
		/// 40% alone that ceiling is 1.067 against a budget of 1.207 - not
		/// close, and no solve can reach it however wide the scale goes. B3
		/// stays the moderate case and this is the one that gives hit rate
		/// enough room to price against.
		/// </remarks>
		public const float EvasiveChance = 75f;

		/// <summary>
		/// Critical chance a scenario that does not pin one reads the
		/// character at.
		/// </summary>
		/// <remarks>
		/// An axis left undeclared is not neutral, it is pinned wherever the
		/// reference gear happens to land - which is 4% here, so eight of the
		/// ten scenarios priced every critical buff against a character that
		/// never criticals, and the magnitude that took to reach budget is
		/// then spent by a build that criticals most of its swings. Declared
		/// for the same reason the enemy's rolls are: the fight has to contain
		/// the thing the buff acts on.
		/// </remarks>
		public const float BaselineCritChance = 40f;

		/// <summary>
		/// Characters in the party scenario.
		/// </summary>
		public const int PartySize = 4;

		public static readonly BuffScenario[] All =
		[
			new()
			{
				Id = "B1",
				Name = "solo, all-in build, plain enemy",
			},
			new()
			{
				Id = "B2",
				Name = "solo, split build, plain enemy",
				SecondaryShare = 0.5f,
			},
			new()
			{
				Id = "B3",
				Name = "solo, evasive enemy",
				MobDodgeChance = LoadedChance,
			},
			new()
			{
				Id = "B4",
				Name = "solo, critical enemy",
				MobCritChance = LoadedCritChance,
			},
			new()
			{
				Id = "B5",
				Name = "party of four, plain enemy",
				PartySize = PartySize,
			},
			new()
			{
				Id = "B6",
				Name = "solo, skill rotation, plain enemy",
				UseSkill = true,
			},
			new()
			{
				Id = "B7",
				Name = "solo, low-critical character",
				CharacterCritChance = LowCritChance,
			},
			new()
			{
				Id = "B8",
				Name = "solo, high-critical character",
				CharacterCritChance = HighCritChance,
			},
			new()
			{
				Id = "B9",
				Name = "solo, blocking enemy",
				MobBlockChance = LoadedChance,
			},
			new()
			{
				Id = "B10",
				Name = "solo, evasive and blocking character",
				CharacterDodgeChance = LoadedChance,
				CharacterBlockChance = LoadedChance,
			},
			new()
			{
				Id = "B11",
				Name = "solo, highly evasive enemy",
				MobDodgeChance = EvasiveChance,
			},
		];

		public static BuffScenario Find(string id)
			=> All.FirstOrDefault(s => s.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

		/// <summary>
		/// Returns the stat spread a scenario asks for.
		/// </summary>
		/// <remarks>
		/// The secondary stat is DEX for everyone. It is what carries hit rate
		/// and critical rate, so splitting into it is what moves a character
		/// off the flat part of those curves - which is the reading a critical
		/// or accuracy buff has to be taken against.
		/// </remarks>
		/// <param name="scenario"></param>
		/// <param name="primaryStat"></param>
		/// <param name="points"></param>
		public static StatSpread Spread(BuffScenario scenario, string primaryStat, int points)
		{
			if (scenario.SecondaryShare <= 0 || primaryStat.Equals("DEX", StringComparison.OrdinalIgnoreCase))
				return StatSpread.AllIn(primaryStat, points);

			var secondary = (int)(points * scenario.SecondaryShare);
			var spread = StatSpread.AllIn(primaryStat, points - secondary);

			spread.Dex += secondary;

			return spread;
		}

		/// <summary>
		/// Gives both sides of the fight the rolls the scenario calls for.
		/// </summary>
		/// <remarks>
		/// Set on both halves of a pair, so what the buff has to overcome is
		/// identical with it and without it, and set before any buff is applied
		/// so a buff's own modifier composes on top rather than being erased.
		///
		/// Each axis is aimed against the opposing side's current stat, and the
		/// six axes read a set of properties disjoint from the six they write,
		/// so no knob can be aimed at a value another knob has already moved.
		/// </remarks>
		/// <param name="scenario"></param>
		/// <param name="character"></param>
		/// <param name="mobs"></param>
		public static void Load(BuffScenario scenario, ICombatEntity character, IReadOnlyList<ICombatEntity> mobs)
		{
			if (mobs == null || mobs.Count == 0)
				return;

			var reference = mobs[0];

			var characterHit = character.Properties.GetFloat(PropertyName.HR);
			var characterBlockBreak = character.Properties.GetFloat(PropertyName.BLK_BREAK);
			var characterCritDodge = character.Properties.GetFloat(PropertyName.CRTDR);

			foreach (var mob in mobs)
			{
				if (scenario.MobDodgeChance is { } dodge)
					Aim(mob, PropertyName.DR, PropertyName.DR_BM, characterHit + dodge / DodgeSlope);

				if (scenario.MobBlockChance is { } block)
					Aim(mob, PropertyName.BLK, PropertyName.BLK_BM, characterBlockBreak + block / BlockSlope);

				if (scenario.MobCritChance is { } critical)
					Aim(mob, PropertyName.CRTHR, PropertyName.CRTHR_BM, characterCritDodge + critical / CritSlope);
			}

			LoadCharacter(scenario, character, reference);
		}

		/// <summary>
		/// Gives one character the rolls the scenario calls for, against a
		/// monster whose own stats the scenario has already settled.
		/// </summary>
		/// <remarks>
		/// Split out for the party members, who need the same build as the
		/// caster but must not re-aim the shared monsters.
		/// </remarks>
		/// <param name="scenario"></param>
		/// <param name="character"></param>
		/// <param name="reference"></param>
		public static void LoadCharacter(BuffScenario scenario, ICombatEntity character, ICombatEntity reference)
		{
			var critical = scenario.CharacterCritChance ?? BaselineCritChance;
			Aim(character, PropertyName.CRTHR, PropertyName.CRTHR_BM, reference.Properties.GetFloat(PropertyName.CRTDR) + critical / CritSlope);

			if (scenario.CharacterDodgeChance is { } dodge)
				Aim(character, PropertyName.DR, PropertyName.DR_BM, reference.Properties.GetFloat(PropertyName.HR) + dodge / DodgeSlope);

			if (scenario.CharacterBlockChance is { } block)
				Aim(character, PropertyName.BLK, PropertyName.BLK_BM, reference.Properties.GetFloat(PropertyName.BLK_BREAK) + block / BlockSlope);
		}

		/// <summary>
		/// Moves an entity's bonus modifier until the stat it feeds reads the
		/// wanted value.
		/// </summary>
		/// <remarks>
		/// The existing modifier is read back rather than assumed to be zero, so
		/// aiming an axis twice lands on the target instead of accumulating.
		/// </remarks>
		/// <param name="entity"></param>
		/// <param name="stat"></param>
		/// <param name="bonus"></param>
		/// <param name="target"></param>
		private static void Aim(ICombatEntity entity, string stat, string bonus, float target)
		{
			var current = entity.Properties.GetFloat(stat);
			var existing = entity.Properties.GetFloat(bonus);

			entity.Properties.SetFloat(bonus, existing + (target - current));
			entity.Properties.Invalidate(stat);
		}

		/// <summary>
		/// Returns the weighted mean of a reading taken across the scenarios.
		/// </summary>
		/// <param name="readings"></param>
		public static float Blend(IReadOnlyDictionary<string, float> readings)
		{
			var total = 0f;
			var weight = 0f;

			foreach (var scenario in All)
			{
				if (!readings.TryGetValue(scenario.Id, out var value))
					continue;

				total += value * scenario.Weight;
				weight += scenario.Weight;
			}

			return weight <= 0 ? 0f : total / weight;
		}
	}
}
