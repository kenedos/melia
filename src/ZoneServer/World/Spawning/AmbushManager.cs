using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World.Spawning
{
	/// <summary>
	/// Watches players walking into monster spawn areas and occasionally
	/// ambushes them with monsters that spawn right next to them.
	/// </summary>
	public class AmbushManager : IUpdateable
	{
		private static readonly TimeSpan CandidateCacheDuration = TimeSpan.FromSeconds(30);
		private static readonly TimeSpan PendingCallTimeout = TimeSpan.FromSeconds(5);

		// Caps how much time one roll can stand for, so a character
		// that was idle or warping doesn't roll for all of it at once.
		private static readonly TimeSpan MaxRollDelta = TimeSpan.FromSeconds(1);

		private static readonly TimeSpan StateCleanupInterval = TimeSpan.FromSeconds(30);
		private static readonly TimeSpan StateLifetime = TimeSpan.FromMinutes(2);

		// Distance a character has to cover before their areas are
		// tested again, so no one slips through a small spawn
		// rectangle untested.
		private const float MinTestDistance = 8f;

		private const float SpawnSearchRadius = 20f;
		private const float AmbushHate = 500f;

		private TimeSpan _cleanupDelay = StateCleanupInterval;

		private readonly Dictionary<int, CharacterAmbushState> _characterStates = new();
		private readonly Dictionary<string, AmbushCandidateCache> _candidateCaches = new();

		private readonly List<PendingAmbushCall> _pendingCalls = new();

		private readonly List<AmbushCandidate> _insideCandidates = new();
		private readonly List<int> _staleHandles = new();

		/// <summary>
		/// Checks players for spawn areas they walked into and rolls
		/// their ambushes.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			this.FlushPendingCalls();

			var conf = ZoneServer.Instance.Conf.World;
			if (conf.AmbushChance <= 0)
				return;

			var characters = ZoneServer.Instance.World.Maps.GetCharacters();
			foreach (var character in characters)
			{
				if (character.IsDead)
					continue;

				var map = character.Map;
				if (!IsAmbushMap(map))
					continue;

				this.UpdateCharacter(character, map, conf.AmbushChance);
			}

			this.RemoveStaleStates(elapsed);
		}

		/// <summary>
		/// Rolls an ambush for a character walking through the spawn
		/// areas they're inside of, once they covered enough ground.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="map"></param>
		/// <param name="chance"></param>
		private void UpdateCharacter(Character character, Map map, float chance)
		{
			var now = GameClock.Now;

			if (!_characterStates.TryGetValue(character.Handle, out var state))
				_characterStates[character.Handle] = state = new CharacterAmbushState(character.Position, now);

			state.LastSeenTime = now;

			// Ambushing a player who isn't going anywhere would just be a
			// spawn in their face, and nothing lies in wait for someone
			// it can't see.
			if (!IsWalking(character) || character.IsBuffActiveByKeyword(BuffTag.Cloaking) || now < state.CooldownEndTime)
			{
				state.LastTestPosition = character.Position;
				state.LastRollTime = now;
				return;
			}

			if (character.Position.InRange2D(state.LastTestPosition, MinTestDistance))
				return;

			state.LastTestPosition = character.Position;

			var candidates = this.GetCandidates(map);
			if (candidates.Count == 0)
			{
				state.LastRollTime = now;
				return;
			}

			_insideCandidates.Clear();

			foreach (var candidate in candidates)
			{
				foreach (var area in candidate.Areas)
				{
					if (!(area.Area?.IsInside(character.Position) ?? false))
						continue;

					_insideCandidates.Add(candidate);
					break;
				}
			}

			if (_insideCandidates.Count == 0)
			{
				state.LastRollTime = now;
				return;
			}

			// The chance stands for one full second of walking inside a
			// spawn area, spread over however much time this roll
			// covers, so the rate holds at any move speed or tick rate.
			var rollDelta = now - state.LastRollTime;
			if (rollDelta > MaxRollDelta)
				rollDelta = MaxRollDelta;

			state.LastRollTime = now;

			var rate = Math.Min(chance, 100f) / 100f;
			var probability = 1 - Math.Pow(1 - rate, rollDelta.TotalSeconds);

			var rnd = GameRandom.Get();
			if (rnd.NextDouble() >= probability)
				return;

			var candidateIndex = rnd.Next(_insideCandidates.Count);
			this.SpawnAmbush(character, map, _insideCandidates[candidateIndex].Spawner);

			var conf = ZoneServer.Instance.Conf.World;
			state.CooldownEndTime = now + TimeSpan.FromSeconds(conf.AmbushCooldownSeconds);
		}

		/// <summary>
		/// Returns true if the character is currently moving on their
		/// own feet.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		private static bool IsWalking(Character character)
		{
			if (!character.Components.TryGet<MovementComponent>(out var movement))
				return false;

			return movement.IsMoving;
		}

		/// <summary>
		/// Spawns the ambushing monsters around the character and makes
		/// them attack them right away.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="map"></param>
		/// <param name="spawner"></param>
		private void SpawnAmbush(Character character, Map map, MonsterSpawner spawner)
		{
			var conf = ZoneServer.Instance.Conf.World;
			var rnd = GameRandom.Get();

			var minAmount = Math.Max(1, conf.AmbushMinMonsters);
			var maxAmount = Math.Max(minAmount, conf.AmbushMaxMonsters);
			var amount = rnd.Next(minAmount, maxAmount + 1);

			Mob firstMonster = null;

			for (var i = 0; i < amount; ++i)
			{
				var angle = rnd.NextDouble() * Math.PI * 2;
				var idealPosition = new Position(
					character.Position.X + (float)(Math.Cos(angle) * conf.AmbushDistance),
					character.Position.Y,
					character.Position.Z + (float)(Math.Sin(angle) * conf.AmbushDistance));

				if (!map.Ground.TryGetNearestValidPosition(idealPosition, SpawnSearchRadius, out var spawnPosition))
					continue;

				var monster = CreateMonster(spawner, spawnPosition);

				if (!map.AddMonster(monster))
					continue;

				if (monster.Components.TryGet<AiComponent>(out var ai))
					ai.Script.QueueEventAlert(new HateIncreaseAlert(character, AmbushHate));

				firstMonster ??= monster;
			}

			// Monsters are queued into the map, so they can't speak
			// before their next update puts them in it.
			if (firstMonster != null)
				_pendingCalls.Add(new PendingAmbushCall(firstMonster, character, GameClock.Now + PendingCallTimeout));
		}

		/// <summary>
		/// Makes ambushing monsters call out once their victim's client
		/// knows about them.
		/// </summary>
		private void FlushPendingCalls()
		{
			for (var i = _pendingCalls.Count - 1; i >= 0; --i)
			{
				var pendingCall = _pendingCalls[i];
				var monster = pendingCall.Monster;

				// The client drops packets for handles it hasn't been
				// sent yet, so the monster has to be visible first.
				if (!monster.IsDead && pendingCall.Target.IsMonsterVisible(monster))
				{
					Send.ZC_CHAT(monster, "It's an ambush!");
					_pendingCalls.RemoveAt(i);
					continue;
				}

				if (GameClock.Now >= pendingCall.ExpirationTime)
					_pendingCalls.RemoveAt(i);
			}
		}

		/// <summary>
		/// Creates an ambushing monster for the given spawner at the
		/// given position.
		/// </summary>
		/// <param name="spawner"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		private static Mob CreateMonster(MonsterSpawner spawner, Position position)
		{
			var monster = new Mob(spawner.MonsterData.Id);
			monster.Position = position;
			monster.SpawnPosition = position;
			monster.FromGround = true;
			monster.Tendency = TendencyType.Aggressive;

			monster.Components.Add(new MovementComponent(monster));

			if (!string.IsNullOrWhiteSpace(monster.Data.AiName) && monster.Data.AiName != "None")
			{
				var aiName = monster.Data.AiName;
				if (!AiScript.Exists(aiName))
					aiName = "BasicMonster";

				monster.Components.Add(new AiComponent(monster, aiName));
			}

			return monster;
		}

		/// <summary>
		/// Returns the spawners on the map that can ambush players,
		/// along with their spawn areas.
		/// </summary>
		/// <param name="map"></param>
		/// <returns></returns>
		private List<AmbushCandidate> GetCandidates(Map map)
		{
			if (_candidateCaches.TryGetValue(map.ClassName, out var cache) && GameClock.Now < cache.ExpirationTime)
				return cache.Candidates;

			cache = new AmbushCandidateCache();
			cache.ExpirationTime = GameClock.Now + CandidateCacheDuration;

			var spawners = ZoneServer.Instance.World.GetSpawnersForMap(map.ClassName);
			foreach (var spawner in spawners)
			{
				if (spawner.Tendency != TendencyType.Aggressive)
					continue;

				if (spawner.MonsterData.Rank != MonsterRank.Normal)
					continue;

				if (!ZoneServer.Instance.World.TryGetSpawnAreas(spawner.SpawnPointsIdent, out var spawnAreas))
					continue;

				var areas = spawnAreas.GetAllOnMap(map);
				if (areas.Length == 0)
					continue;

				cache.Candidates.Add(new AmbushCandidate(spawner, areas));
			}

			_candidateCaches[map.ClassName] = cache;

			return cache.Candidates;
		}

		/// <summary>
		/// Drops the tracked state of characters that haven't been seen
		/// in a while.
		/// </summary>
		/// <param name="elapsed"></param>
		private void RemoveStaleStates(TimeSpan elapsed)
		{
			_cleanupDelay -= elapsed;
			if (_cleanupDelay > TimeSpan.Zero)
				return;

			_cleanupDelay = StateCleanupInterval;
			_staleHandles.Clear();

			foreach (var entry in _characterStates)
			{
				if (GameClock.Now - entry.Value.LastSeenTime >= StateLifetime)
					_staleHandles.Add(entry.Key);
			}

			foreach (var handle in _staleHandles)
				_characterStates.Remove(handle);
		}

		/// <summary>
		/// Returns true if ambushes can happen on the given map.
		/// </summary>
		/// <param name="map"></param>
		/// <returns></returns>
		private static bool IsAmbushMap(Map map)
		{
			if (map == null || map == Map.Limbo || map.IsDormant || map.IsCity)
				return false;

			return map.IsField;
		}

		/// <summary>
		/// The ambush state tracked for one character.
		/// </summary>
		private class CharacterAmbushState
		{
			/// <summary>
			/// Gets or sets the position the character's spawn areas
			/// were last tested at.
			/// </summary>
			public Position LastTestPosition { get; set; }

			/// <summary>
			/// Gets or sets the time of the character's last ambush roll.
			/// </summary>
			public DateTime LastRollTime { get; set; }

			/// <summary>
			/// Gets or sets the time until which the character can't be
			/// ambushed again.
			/// </summary>
			public DateTime CooldownEndTime { get; set; }

			/// <summary>
			/// Gets or sets the last time the character was updated.
			/// </summary>
			public DateTime LastSeenTime { get; set; }

			/// <summary>
			/// Creates a new ambush state.
			/// </summary>
			/// <param name="position"></param>
			/// <param name="now"></param>
			public CharacterAmbushState(Position position, DateTime now)
			{
				this.LastTestPosition = position;
				this.LastRollTime = now;
				this.LastSeenTime = now;
			}
		}

		/// <summary>
		/// An ambushing monster waiting to be added to its map so it
		/// can call out.
		/// </summary>
		private class PendingAmbushCall
		{
			/// <summary>
			/// Returns the monster that calls out.
			/// </summary>
			public Mob Monster { get; }

			/// <summary>
			/// Returns the character being ambushed.
			/// </summary>
			public Character Target { get; }

			/// <summary>
			/// Returns the time at which the call is given up on.
			/// </summary>
			public DateTime ExpirationTime { get; }

			/// <summary>
			/// Creates a new pending ambush call.
			/// </summary>
			/// <param name="monster"></param>
			/// <param name="target"></param>
			/// <param name="expirationTime"></param>
			public PendingAmbushCall(Mob monster, Character target, DateTime expirationTime)
			{
				this.Monster = monster;
				this.Target = target;
				this.ExpirationTime = expirationTime;
			}
		}

		/// <summary>
		/// A spawner that can ambush players, with the spawn areas it
		/// uses on one specific map.
		/// </summary>
		private class AmbushCandidate
		{
			/// <summary>
			/// Returns the spawner the ambushing monsters come from.
			/// </summary>
			public MonsterSpawner Spawner { get; }

			/// <summary>
			/// Returns the spawner's areas on the map.
			/// </summary>
			public SpawnArea[] Areas { get; }

			/// <summary>
			/// Creates a new ambush candidate.
			/// </summary>
			/// <param name="spawner"></param>
			/// <param name="areas"></param>
			public AmbushCandidate(MonsterSpawner spawner, SpawnArea[] areas)
			{
				this.Spawner = spawner;
				this.Areas = areas;
			}
		}

		/// <summary>
		/// The cached ambush candidates of one map.
		/// </summary>
		private class AmbushCandidateCache
		{
			/// <summary>
			/// Returns the time at which the cache needs to be rebuilt.
			/// </summary>
			public DateTime ExpirationTime { get; set; }

			/// <summary>
			/// Returns the cached candidates.
			/// </summary>
			public List<AmbushCandidate> Candidates { get; } = new();
		}
	}
}
