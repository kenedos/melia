using System;
using System.Collections.Generic;
using System.Reflection;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.ObjectProperties;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Database;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Melia.Zone.World.Items;
using Melia.Zone.World.Maps;

namespace Melia.Test.Balance
{
	/// <summary>
	/// Allocated stat points for a synthetic character. These map to the
	/// *_STAT properties, which are what stat point spending actually sets.
	/// </summary>
	public class StatSpread
	{
		public int Str { get; set; }
		public int Con { get; set; }
		public int Int { get; set; }
		public int Spr { get; set; }
		public int Dex { get; set; }

		/// <summary>
		/// Returns a spread with every point in one stat, which is the
		/// reference build sim.py calibrates against.
		/// </summary>
		/// <param name="stat"></param>
		/// <param name="points"></param>
		public static StatSpread AllIn(string stat, int points)
		{
			var spread = new StatSpread();

			switch (stat.ToUpperInvariant())
			{
				case "STR": spread.Str = points; break;
				case "CON": spread.Con = points; break;
				case "INT": spread.Int = points; break;
				case "SPR": spread.Spr = points; break;
				case "DEX": spread.Dex = points; break;
				default: throw new ArgumentException($"Unknown stat '{stat}'.", nameof(stat));
			}

			return spread;
		}
	}

	/// <summary>
	/// Builds characters and monsters in memory for scenario measurement,
	/// with no database or client involved.
	/// </summary>
	public static class SyntheticActors
	{
		/// <summary>
		/// Map every synthetic actor is placed on, chosen because it is a
		/// plain field with no scripted mechanics of its own.
		/// </summary>
		public const string ArenaMapName = "c_highlander";

		private static readonly object _mobCreateLock = new();

		/// <summary>
		/// Map ticks to allow a queued monster to register. The map adds at
		/// most five per tick, so this covers any scenario's group.
		/// </summary>
		private const int MaxSettleTicks = 20;

		private static int _teamNameCounter;
		private static readonly object _arenaCenterLock = new();
		private static readonly Dictionary<int, Position> _arenaCenters = [];

		/// <summary>
		/// Returns the map synthetic actors fight on by default.
		/// </summary>
		/// <remarks>
		/// This is the one map every serial test shares. A parallel run passes
		/// its own pool arena explicitly to every call below instead, so two
		/// workers never place actors on the same instance.
		/// </remarks>
		public static Map GetArena()
		{
			if (!ZoneServer.Instance.World.TryGetMap(ArenaMapName, out var map))
				throw new InvalidOperationException($"Arena map '{ArenaMapName}' is not loaded.");

			return Uncity(map);
		}

		/// <summary>
		/// Clears an arena's city flag and returns it.
		/// </summary>
		/// <remarks>
		/// A handler that refuses to build in town measures as a press that
		/// damaged nothing, and is then held back unpriced -
		/// Cryomancer_IceWall, QuarrelShooter_DeployPavise, the Sorcerer
		/// summons. The arena is a fighting ground that happens to be a city
		/// map, so the flag is what is wrong, not the skill. Dormancy is not a
		/// concern: the constructor has already run, so IsDormant stays false,
		/// and pool arenas are never registered with WorldManager to be ticked
		/// into it.
		/// </remarks>
		/// <param name="map"></param>
		public static Map Uncity(Map map)
		{
			map.IsCity = false;

			return map;
		}

		/// <summary>
		/// Creates a character at the given job and level, places it on the
		/// arena and returns it.
		/// </summary>
		/// <param name="jobId"></param>
		/// <param name="level"></param>
		/// <param name="stats"></param>
		/// <param name="position"></param>
		public static Character CreateCharacter(JobId jobId, int level, StatSpread stats = null, Position position = default, Map arena = null)
		{
			stats ??= StatSpread.AllIn("STR", level);
			arena ??= GetArena();

			var teamName = $"Synth{System.Threading.Interlocked.Increment(ref _teamNameCounter)}";

			var character = new Character
			{
				Name = teamName,
				TeamName = teamName,
				JobId = jobId,
			};

			// DummyConnection absorbs every Send.ZC_*; the account is real
			// but empty so Username and PermissionLevel resolve.
			character.Connection = new DummyConnection
			{
				SelectedCharacter = character,
				Account = new Account { Name = teamName },
			};

			character.Jobs.AddSilent(new Job(character, jobId));

			var properties = character.Properties;
			properties.SetFloat(PropertyName.Lv, level);
			properties.SetFloat(PropertyName.STR_STAT, stats.Str);
			properties.SetFloat(PropertyName.CON_STAT, stats.Con);
			properties.SetFloat(PropertyName.INT_STAT, stats.Int);
			properties.SetFloat(PropertyName.MNA_STAT, stats.Spr);
			properties.SetFloat(PropertyName.DEX_STAT, stats.Dex);
			properties.InvalidateAll();

			character.Position = ResolvePosition(arena, position);
			arena.AddCharacter(character);

			GiveReferenceSilver(character, level);

			properties.SetFloat(PropertyName.HP, properties.GetFloat(PropertyName.MHP));
			properties.SetFloat(PropertyName.SP, properties.GetFloat(PropertyName.MSP));

			return character;
		}

		/// <summary>
		/// Silver a character carries at the anchor levels.
		/// </summary>
		/// <remarks>
		/// Pardoner_Dekatos is the only press in the game that reads the
		/// player's purse, and it refuses to cast at all when the purse cannot
		/// pay its cost - so a silverless character would have it come back
		/// from the roster pass as a press that could not be dispatched, not
		/// as a weak one. What the purse is worth to it is swept separately by
		/// SfrPricingTests.Silver, which sets its own.
		/// </remarks>
		private static readonly (int Level, double Silver)[] SilverAnchors =
		[
			(1, 1_000),
			(30, 100_000),
			(60, 1_000_000),
			(90, 50_000_000),
		];

		/// <summary>
		/// Returns the silver a reference character of the given level
		/// carries.
		/// </summary>
		/// <remarks>
		/// Interpolated in the exponent and flat past the last anchor. What a
		/// player holds grows by orders of magnitude across the curve, so a
		/// linear interpolation between two anchors reads as the higher one
		/// for almost every level between them.
		/// </remarks>
		/// <param name="level"></param>
		public static int ReferenceSilver(int level)
		{
			if (level <= SilverAnchors[0].Level)
				return (int)SilverAnchors[0].Silver;

			for (var i = 1; i < SilverAnchors.Length; ++i)
			{
				var (highLevel, highSilver) = SilverAnchors[i];

				if (level > highLevel)
					continue;

				var (lowLevel, lowSilver) = SilverAnchors[i - 1];
				var share = (double)(level - lowLevel) / (highLevel - lowLevel);
				var log = Math.Log10(lowSilver) + share * (Math.Log10(highSilver) - Math.Log10(lowSilver));

				return (int)Math.Pow(10, log);
			}

			return (int)SilverAnchors[^1].Silver;
		}

		/// <summary>
		/// Puts the level's reference silver in the character's inventory.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="level"></param>
		private static void GiveReferenceSilver(Character character, int level)
		{
			var silver = ReferenceSilver(level);

			if (silver > 0)
				character.Inventory.AddSilent(new Item(ItemId.Silver, silver));
		}

		/// <summary>
		/// Gives the character a skill at the given level.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skillId"></param>
		/// <param name="level"></param>
		public static Skill GiveSkill(Character character, SkillId skillId, int level)
		{
			var skill = new Skill(character, skillId, level);

			PrivateSkillData(skill);
			character.Skills.Add(skill);

			return skill;
		}

		/// <summary>
		/// Backing field behind Skill.Data, which is get-only.
		/// </summary>
		private static readonly FieldInfo _skillDataField = typeof(Skill)
			.GetField($"<{nameof(Skill.Data)}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);

		private static readonly MethodInfo _memberwiseClone = typeof(object)
			.GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

		/// <summary>
		/// Replaces a synthetic skill's shared SkillData with a copy of its
		/// own, so pinning its factor cannot be seen by any other measurement.
		/// </summary>
		/// <remarks>
		/// Skill.Data is the single instance SkillDb hands to every Skill of
		/// that id, so SfrFactorScope's factor override had to hold a lock on
		/// it for the whole press to stay correct - seconds of sleeping, during
		/// which no other window of the same skill could run. Giving each
		/// measured skill a private copy removes the sharing instead of
		/// guarding it, which is what lets a skill's nine scenarios and two
		/// factor points run at once. The copy is shallow: only Factor and
		/// FactorByLevel are ever written, and both are value types.
		/// </remarks>
		/// <param name="skill"></param>
		private static void PrivateSkillData(Skill skill)
		{
			if (_skillDataField == null || _memberwiseClone == null)
				return;

			_skillDataField.SetValue(skill, (SkillData)_memberwiseClone.Invoke(skill.Data, null));
		}

		/// <summary>
		/// Creates a hostile monster of the given class name and places it
		/// on the arena.
		/// </summary>
		/// <param name="className"></param>
		/// <param name="position"></param>
		public static Mob CreateMob(string className, Position position = default)
		{
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(className, out var data))
				throw new ArgumentException($"Unknown monster '{className}'.", nameof(className));

			return CreateMob(data.Id, position);
		}

		/// <summary>
		/// Creates a hostile monster at the place a scenario put it, carrying
		/// the AoE defence ratio that scenario forces on it.
		/// </summary>
		/// <remarks>
		/// Only the penetration ranks set an SDR; every other placement leaves
		/// the monster's own, which its size already decides.
		/// </remarks>
		/// <param name="monsterId"></param>
		/// <param name="placement"></param>
		public static Mob CreateMob(int monsterId, ScenarioMob placement, Map arena = null)
		{
			var mob = CreateMob(monsterId, placement.Offset, arena);

			if (placement.Sdr > 0)
				mob.Properties.Modify(PropertyName.SDR_BM, placement.Sdr - mob.Properties.GetFloat(PropertyName.SDR));

			return mob;
		}

		/// <summary>
		/// Creates a hostile monster of the given id and places it on the
		/// arena.
		/// </summary>
		/// <param name="monsterId"></param>
		/// <param name="position"></param>
		/// <param name="arena"></param>
		public static Mob CreateMob(int monsterId, Position position = default, Map arena = null)
		{
			arena ??= GetArena();

			// Constructing a Mob first-touches that monster's shared data, and
			// a scenario now places several distinct monsters rather than N
			// copies of one, so a wide run has many threads first-touching
			// different ids at the same moment. Arenas are per-window and need
			// no guard; this does. It is microseconds against a 10 s window.
			Mob mob;

			lock (_mobCreateLock)
				mob = new Mob(monsterId, RelationType.Enemy);

			mob.Position = ResolvePosition(arena, position);
			mob.SpawnPosition = mob.Position;
			mob.Components.Add(new MovementComponent(mob));

			Normalize(mob);

			arena.AddMonster(mob);
			Settle(arena, mob);

			return mob;
		}

		/// <summary>
		/// Max HP given to every test monster, so no measurement window ends
		/// in a corpse.
		/// </summary>
		private const float SurvivalHp = 100_000_000f;

		/// <summary>
		/// Strips the traits that would make one monster a different yardstick
		/// than another.
		/// </summary>
		/// <remarks>
		/// Move type and element are measurement noise here, not the thing
		/// being measured: handlers that cannot hit a flying target read as
		/// dealing no damage at all against one, and an elemental matchup
		/// swings SCR_AttributeMultiplier by up to 1.5x on nothing but which
		/// monster the census happened to pick.
		/// </remarks>
		/// <param name="mob"></param>
		private static void Normalize(Mob mob)
		{
			mob.MoveType = MoveType.Normal;

			// Attribute is a reference property bound to the monster's data,
			// so it is replaced rather than set.
			mob.Properties.Create(new RFloatProperty(PropertyName.Attribute, () => (int)AttributeType.Melee));

			// RecoveryComponent heals while HP is below max, and the max below
			// guarantees it always is, which would undo the damage a window is
			// measuring.
			mob.Properties.Create(new RFloatProperty(PropertyName.RHP, () => 0));

			mob.Properties.SetFloat(PropertyName.MHP_BM, SurvivalHp);
			mob.Properties.Invalidate(PropertyName.MHP);
			mob.Properties.SetFloat(PropertyName.HP, mob.Properties.GetFloat(PropertyName.MHP));
		}

		/// <summary>
		/// Creates a hostile monster that actually fights back: it carries a
		/// real AI script, is immediately hostile to the given character
		/// rather than needing to notice it, and is aggressive by tendency so
		/// it presses the attack instead of waiting to be provoked.
		/// </summary>
		/// <remarks>
		/// Every other mob in this file is a passive dummy on purpose - the
		/// reach/hit measurements place mobs at fixed offsets and read what a
		/// press touches, which a mob wandering off to chase would corrupt.
		/// Only the defensive/CC probe wants a mob that actually swings back.
		/// </remarks>
		/// <param name="monsterId"></param>
		/// <param name="position"></param>
		/// <param name="hates"></param>
		/// <param name="arena"></param>
		public static Mob CreateHostileMob(int monsterId, Position position, ICombatEntity hates, Map arena = null)
		{
			var mob = CreateMob(monsterId, position, arena);
			var aiName = mob.Data?.AiName;

			mob.Components.Add(new Melia.Zone.World.Actors.CombatEntities.Components.AiComponent(mob,
				!string.IsNullOrEmpty(aiName) && Melia.Zone.Scripting.AI.AiScript.Exists(aiName) ? aiName : "BasicMonster"));
			mob.Tendency = TendencyType.Aggressive;
			mob.InsertHate(hates);

			return mob;
		}

		/// <summary>
		/// Runs the map until the monster is actually registered on it.
		/// </summary>
		/// <remarks>
		/// Map.AddMonster only queues the monster; the queue drains in
		/// UpdateEntities, which RunHeadless never starts. Without this the
		/// monster is invisible to every Map.GetAttackable* lookup, so splash
		/// geometry finds nothing even though direct damage sampling works.
		/// </remarks>
		/// <param name="map"></param>
		/// <param name="mob"></param>
		private static void Settle(Map map, Mob mob)
		{
			for (var i = 0; i < MaxSettleTicks && mob.Map != map; ++i)
				map.Update(TimeSpan.Zero);

			if (mob.Map != map)
				throw new InvalidOperationException($"'{mob.Data.ClassName}' never registered on '{ArenaMapName}' - the add queue is not draining.");
		}

		/// <summary>
		/// Returns the median-HP monster of the given rank at the given
		/// level, restricted to monsters that spawn on a map players can
		/// reach, so results are neither skewed by an outlier nor measured
		/// against content nobody fights.
		/// </summary>
		/// <param name="level"></param>
		/// <param name="rank"></param>
		public static MonsterData FindReferenceMob(int level, MonsterRank rank = MonsterRank.Normal)
			=> SpawnCensus.FindReferenceMob(level, rank);

		/// <summary>
		/// Radius the arena needs to be walkable out to, so a scenario can
		/// place monsters anywhere inside it without the ground moving them.
		/// </summary>
		public const float ArenaClearance = 300f;

		/// <summary>
		/// Random ground positions to try before settling for the roomiest
		/// one found.
		/// </summary>
		private const int ArenaSearchTries = 2000;

		/// <summary>
		/// Seed the clearance search runs under, so the ground every scenario
		/// is built on is the same ground on every run.
		/// </summary>
		private const int ArenaCenterSeed = 20260807;

		/// <summary>
		/// Returns a fixed point of walkable ground that every scenario is
		/// built around, so offsets between actors are meaningful.
		/// </summary>
		/// <remarks>
		/// The point must have open ground all around it. A merely walkable
		/// point is not enough: scenario offsets that land on a wall get
		/// snapped to the nearest valid ground, which silently moves a
		/// monster out of the cone the skill was aimed down.
		///
		/// Cached per map id rather than per Map instance, and searched under a
		/// seed of its own rather than the flow's. A pool arena is a separate
		/// Map built on the same id, so its ground is the same read-only
		/// geometry and one centre is correct for all of them - while a centre
		/// per instance made every arena a slightly different fighting ground,
		/// and which arena a press rents is decided by whichever is free. The
		/// private seed is what keeps the search off the flow's own RNG
		/// position: drawn from that, the centre depended on how many rolls the
		/// press that first touched the arena had already made, so the same
		/// arena came out different depending on who got there first.
		/// </remarks>
		public static Position GetArenaCenter(Map arena = null)
		{
			arena ??= GetArena();

			lock (_arenaCenterLock)
			{
				if (_arenaCenters.TryGetValue(arena.Id, out var cached))
					return cached;

				var best = SearchArenaCenter(arena);

				_arenaCenters[arena.Id] = best;

				return best;
			}
		}

		/// <summary>
		/// Runs the clearance search for the roomiest walkable point on a map.
		/// </summary>
		/// <param name="arena"></param>
		private static Position SearchArenaCenter(Map arena)
		{
			var ground = arena.Ground;
			var probes = GetClearanceProbes();

			var best = default(Position);
			var bestClear = -1;

			var previous = GameRandom.Current;
			GameRandom.Use(new Random(ArenaCenterSeed));

			try
			{
				for (var i = 0; i < ArenaSearchTries; ++i)
				{
					if (!ground.TryGetRandomPosition(out var candidate))
						continue;

					var clear = 0;

					foreach (var probe in probes)
					{
						if (ground.IsValidPosition(new Position(candidate.X + probe.X, candidate.Y, candidate.Z + probe.Z)))
							++clear;
					}

					if (clear > bestClear)
					{
						best = candidate;
						bestClear = clear;
					}

					if (clear == probes.Length)
						break;
				}
			}
			finally
			{
				GameRandom.Use(previous);
			}

			if (bestClear < 0)
				throw new InvalidOperationException($"Could not find walkable ground on '{ArenaMapName}'.");

			return best;
		}

		/// <summary>
		/// Returns the offsets the arena centre is tested for clearance at:
		/// rings at the distances scenarios actually place monsters.
		/// </summary>
		private static Position[] GetClearanceProbes()
		{
			var probes = new List<Position>();

			foreach (var radius in new[] { 40f, 120f, 200f, ArenaClearance })
			{
				for (var step = 0; step < 16; ++step)
				{
					var angle = step / 16f * MathF.PI * 2f;

					probes.Add(new Position(MathF.Cos(angle) * radius, 0, MathF.Sin(angle) * radius));
				}
			}

			return probes.ToArray();
		}

		/// <summary>
		/// Snaps a position onto valid ground, since skills and movement
		/// both depend on the actor standing somewhere walkable. A default
		/// position is treated as "the arena center".
		/// </summary>
		/// <remarks>
		/// Only the height is adjusted while the ground is walkable.
		/// Searching for the nearest valid position unconditionally moved
		/// actors tens of units sideways, which silently pushed them out of
		/// the cone a scenario had aimed at them.
		/// </remarks>
		/// <param name="arena"></param>
		/// <param name="position"></param>
		private static Position ResolvePosition(Map arena, Position position)
		{
			if (position == default)
				return GetArenaCenter(arena);

			var center = GetArenaCenter(arena);
			var absolute = new Position(center.X + position.X, center.Y + position.Y, center.Z + position.Z);
			var ground = arena.Ground;

			if (ground.IsValidPosition(absolute) && ground.TryGetHeightAt(absolute, out var height))
				return new Position(absolute.X, height, absolute.Z);

			if (ground.TryGetNearestValidPosition(absolute, out var validPos))
				return validPos;

			return absolute;
		}

		/// <summary>
		/// Removes actors from the arena so scenarios do not leak state into
		/// each other.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="mobs"></param>
		public static void Cleanup(Character character, params Mob[] mobs)
		{
			// Removed from whichever map each actor actually ended up on,
			// rather than the shared default - a pool arena's actors are
			// never on it.
			var map = character?.Map;

			foreach (var mob in mobs)
			{
				map ??= mob?.Map;
				mob?.Map?.RemoveMonster(mob);
			}

			// Anything a press leaves behind outlives it and lands on the next
			// measurement: a pad keeps ticking at the previous factor, a
			// summon keeps attacking, a drop keeps occupying the arena. The
			// arena is only ever ours, so it is emptied rather than tracked.
			if (map != null)
			{
				foreach (var pad in map.GetPads(_ => true))
					map.RemovePad(pad);

				foreach (var monster in map.GetMonsters())
					map.RemoveMonster(monster);
			}

			character?.Map?.RemoveCharacter(character);

			// The tables hand a removed entity's slot to the next one added, so
			// without this an arena enumerates its actors in an order set by
			// whatever ran on it before - and which arena a window rents is
			// decided by whichever is free. A handler keeping the first n of
			// several candidates at equal distance, which is every scenario that
			// stacks its pull on one point, then measured a different pull each
			// run.
			map?.CompactEntityTables();
		}
	}

	/// <summary>
	/// A fixed set of independent arena maps, so several presses can run at
	/// once without landing their actors on the same instance.
	/// </summary>
	/// <remarks>
	/// Each arena is a separate <see cref="Map"/> built with the *same id* as
	/// the primary arena, since Map resolves its ground/nav data by looking
	/// its id up in MapDb; a fresh id from
	/// <see cref="WorldManager.GenerateDynamicMapId"/> has no MapDb entry and
	/// loads no ground at all. Sharing the id is safe - MapDb data is
	/// read-only geometry, and each Map instance still keeps its own
	/// independent character/monster/pad tables. These are deliberately
	/// *not* registered through <see cref="WorldManager.AddMap"/>, since two
	/// maps can't share one id there.
	/// </remarks>
	public sealed class ArenaPool : IDisposable
	{
		private readonly System.Collections.Concurrent.BlockingCollection<Map> _free;

		/// <summary>
		/// How many arenas the pool hands out before a caller blocks waiting
		/// for one back.
		/// </summary>
		public int Size { get; }

		/// <summary>
		/// Builds a pool of independent arenas of the given map class.
		/// </summary>
		/// <param name="size"></param>
		/// <param name="mapClassName"></param>
		public ArenaPool(int size, string mapClassName = SyntheticActors.ArenaMapName)
		{
			this.Size = Math.Max(1, size);
			_free = new System.Collections.Concurrent.BlockingCollection<Map>(this.Size);

			var realId = SyntheticActors.GetArena().Id;
			var started = DateTime.UtcNow;

			// Every Map runs Ground.Load, which builds its own cells, spatial
			// index and pathfinder - real CPU per instance rather than a
			// shared reference. Built serially this was the single largest
			// cost of a roster run, and the one part of it that is not a sleep.
			System.Threading.Tasks.Parallel.For(0, this.Size, _ => _free.Add(SyntheticActors.Uncity(new Map(realId, mapClassName))));

			this.BuildTime = DateTime.UtcNow - started;
		}

		/// <summary>
		/// How long the pool's arenas took to build.
		/// </summary>
		public TimeSpan BuildTime { get; }

		/// <summary>
		/// Takes an arena out of the pool, blocking until one is free.
		/// </summary>
		public Map Rent()
			=> _free.Take();

		/// <summary>
		/// Returns an arena to the pool once its actors have been cleaned up.
		/// </summary>
		/// <remarks>
		/// Scrubbed on the way back in. SyntheticActors.Cleanup removes the
		/// character and the mobs a window placed, but not a pad the press
		/// left running, a summon it raised or an obstacle it built - those
		/// outlive the window and would still be on the arena when the next
		/// one rents it. Which arena a window gets is decided by whichever is
		/// free, so that residue lands on a different measurement every run,
		/// which is a correctness problem before it is a stability one.
		/// </remarks>
		/// <param name="arena"></param>
		public void Return(Map arena)
		{
			arena.ClearPendingEntities();
			arena.RemoveEntitiesOnLayer(0);
			arena.CompactEntityTables();
			_free.Add(arena);
		}

		/// <summary>
		/// Runs one unit of work against a pooled arena, returning it whether
		/// the work throws or not.
		/// </summary>
		/// <param name="work"></param>
		public T Use<T>(Func<Map, T> work)
		{
			var arena = this.Rent();

			try
			{
				return work(arena);
			}
			finally
			{
				this.Return(arena);
			}
		}

		/// <summary>
		/// Releases the pool's queue. The arenas were never registered with
		/// WorldManager, so there is nothing else to unwind.
		/// </summary>
		public void Dispose()
			=> _free.Dispose();
	}
}
