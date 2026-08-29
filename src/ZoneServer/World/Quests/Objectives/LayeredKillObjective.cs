using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;

namespace Melia.Zone.World.Quests.Objectives
{
	/// <summary>
	/// Single monster id + amount entry used by <see cref="LayeredKillObjective"/>.
	/// </summary>
	public sealed class KillSpec
	{
		/// <summary>
		/// Monster id to spawn and track.
		/// </summary>
		public int MonsterId { get; }

		/// <summary>
		/// How many of this monster to spawn and require kills for.
		/// </summary>
		public int Count { get; }

		/// <summary>
		/// Optional buff applied to each spawned monster (e.g. EliteMonsterBuff).
		/// </summary>
		public BuffId? BuffId { get; }

		/// <summary>
		/// Creates a kill specification.
		/// </summary>
		/// <param name="monsterId"></param>
		/// <param name="count"></param>
		/// <param name="buffId"></param>
		public KillSpec(int monsterId, int count = 1, BuffId? buffId = null)
		{
			if (count < 1)
				throw new ArgumentException("Count must be >= 1.", nameof(count));

			this.MonsterId = monsterId;
			this.Count = count;
			this.BuffId = buffId;
		}
	}

	/// <summary>
	/// Objective that warps the character to a private map layer and
	/// spawns a list of monsters there the moment the objective becomes
	/// unlocked (typically via <see cref="QuestUnlockType.Sequential"/>).
	/// All spawned monsters must be killed by the character to complete.
	/// If any spawned monster dies without the character getting credit
	/// (timer expires, killed by someone else, etc.), or the character
	/// leaves the map with the encounter still running, this objective
	/// resets and may also reset a named prerequisite objective so the
	/// player has to redo it before retriggering the spawn.
	/// </summary>
	public class LayeredKillObjective : QuestObjective
	{
		private readonly object _spawnSyncLock = new();
		private readonly Dictionary<long, List<Mob>> _spawnedMobs = new();

		/// <summary>
		/// Monsters spawned on the private layer.
		/// </summary>
		public IReadOnlyList<KillSpec> SpawnList { get; }

		/// <summary>
		/// Optional ident of an earlier objective whose progress is
		/// wiped when this objective fails. Pass null to leave prior
		/// objectives untouched on failure.
		/// </summary>
		public string ResetIdent { get; }

		/// <summary>
		/// Maximum spawn distance from the character.
		/// </summary>
		public int SpawnDistance { get; }

		/// <summary>
		/// Lifetime of each spawned monster before it despawns.
		/// </summary>
		public TimeSpan Lifetime { get; }

		/// <summary>
		/// Optional callback fired once when the spawn is triggered.
		/// </summary>
		public Action<Character> Triggered { get; set; }

		/// <summary>
		/// Creates a new layered-kill objective.
		/// </summary>
		/// <param name="spawnList"></param>
		/// <param name="resetIdent"></param>
		/// <param name="spawnDistance"></param>
		/// <param name="lifetime"></param>
		public LayeredKillObjective(KillSpec[] spawnList, string resetIdent = null, int spawnDistance = 100, TimeSpan? lifetime = null)
		{
			if (spawnList == null || spawnList.Length == 0)
				throw new ArgumentException("Spawn list must contain at least one entry.", nameof(spawnList));

			this.SpawnList = spawnList;
			this.ResetIdent = resetIdent;
			this.SpawnDistance = spawnDistance;
			this.Lifetime = lifetime ?? TimeSpan.FromMinutes(5);
			this.TargetCount = spawnList.Sum(s => s.Count);
		}

		/// <summary>
		/// Subscribes to the events that arm, disarm, and repair the spawn.
		/// </summary>
		public override void Load()
		{
			ZoneServer.Instance.ServerEvents.PlayerAbandonedQuest.Subscribe(this.OnPlayerAbandonedQuest);
			ZoneServer.Instance.ServerEvents.PlayerLeftMap.Subscribe(this.OnPlayerLeftMap);
			ZoneServer.Instance.ServerEvents.PlayerReady.Subscribe(this.OnPlayerReady);
		}

		/// <summary>
		/// Unsubscribes from the events subscribed to in Load.
		/// </summary>
		public override void Unload()
		{
			ZoneServer.Instance.ServerEvents.PlayerAbandonedQuest.Unsubscribe(this.OnPlayerAbandonedQuest);
			ZoneServer.Instance.ServerEvents.PlayerLeftMap.Unsubscribe(this.OnPlayerLeftMap);
			ZoneServer.Instance.ServerEvents.PlayerReady.Unsubscribe(this.OnPlayerReady);
		}

		/// <summary>
		/// Spawns the encounter as soon as the preceding objective
		/// completes, on the character's current map.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="quest"></param>
		public override void OnUnlocked(Character character, Quest quest)
		{
			if (character?.Map == null || character.Map == Map.Limbo)
				return;
			if (!character.IsOnline || !character.Map.TryGetCharacter(character.Handle, out _))
				return;
			if (!quest.TryGetProgress(this.Ident, out var progress) || progress.Done)
				return;
			if (this.IsSpawned(character, quest))
				return;
			if (!this.IsQuestLocation(character, quest))
				return;

			this.SpawnAll(character, quest);
		}

		/// <summary>
		/// Tears the encounter down when the quest is dropped.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnPlayerAbandonedQuest(object sender, PlayerAbandonedQuestEventArgs args)
		{
			var character = args.Character;
			if (character == null)
				return;

			var key = this.StateKeyById(args.QuestId);
			if (!character.Variables.Temp.GetBool(key, false))
				return;

			character.Variables.Temp.Remove(key);
			this.DespawnAll(character);
			this.ReturnFromLayer(character);
		}

		/// <summary>
		/// Fails the encounter when the character leaves the map it was
		/// spawned on, which also covers logging out.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnPlayerLeftMap(object sender, PlayerEventArgs args)
		{
			var character = args.Character;
			if (character == null)
				return;

			this.DespawnAll(character);

			character.Quests.UpdateObjectives<LayeredKillObjective>((quest, objective, progress) =>
			{
				if (objective != this)
					return;
				if (progress.Done)
					return;
				if (!objective.IsSpawned(character, quest))
					return;

				objective.ClearSpawned(character, quest);
				objective.Fail(character, quest, progress);
			});
		}

		/// <summary>
		/// Relocks any encounter that is armed but has no monsters behind
		/// it, which is the state a character loads in with after logging
		/// out mid-encounter.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnPlayerReady(object sender, PlayerEventArgs args)
		{
			var character = args.Character;
			if (character == null)
				return;

			character.Quests.UpdateObjectives<LayeredKillObjective>((quest, objective, progress) =>
			{
				if (objective != this)
					return;
				if (progress.Done)
					return;
				if (objective.IsSpawned(character, quest))
					return;

				objective.Fail(character, quest, progress);
			});
		}

		/// <summary>
		/// Warps the character to a private layer and spawns every
		/// monster in <see cref="SpawnList"/>.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="quest"></param>
		private void SpawnAll(Character character, Quest quest)
		{
			this.SetSpawned(character, quest, true);
			character.StartLayer();

			foreach (var spec in this.SpawnList)
			{
				for (var i = 0; i < spec.Count; i++)
					this.SpawnOne(character, spec.MonsterId, spec.BuffId);
			}

			this.Triggered?.Invoke(character);
		}

		/// <summary>
		/// Spawns a single monster on the character's current layer and
		/// hooks its death to resolve win or fail.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="monsterId"></param>
		/// <param name="buffId"></param>
		private void SpawnOne(Character character, int monsterId, BuffId? buffId = null)
		{
			if (character?.Map == null)
				return;

			var spawnMob = new Mob(monsterId, RelationType.Enemy);
			spawnMob.Layer = character.Layer;
			spawnMob.HasDrops = false;
			spawnMob.HasExp = false;

			var inner = Math.Max(0, this.SpawnDistance - 10);
			var randomOffset = character.Position.GetRandomInRange2D(inner, this.SpawnDistance);

			if (!character.Map.Ground.TryGetNearestValidPosition(randomOffset, out var spawnPos, this.SpawnDistance))
				spawnPos = character.Position;

			spawnMob.Position = spawnPos;
			spawnMob.SpawnPosition = spawnPos;
			spawnMob.Components.Add(new MovementComponent(spawnMob));
			spawnMob.Components.Add(new LifeTimeComponent(spawnMob, this.Lifetime));

			if (!string.IsNullOrEmpty(spawnMob.Data.AiName) && AiScript.Exists(spawnMob.Data.AiName))
				spawnMob.Components.Add(new AiComponent(spawnMob, spawnMob.Data.AiName));
			else
				spawnMob.Components.Add(new AiComponent(spawnMob, "BasicMonster"));

			spawnMob.InsertHate(character);
			spawnMob.Tendency = TendencyType.Aggressive;
			spawnMob.FromGround = true;

			if (character.Map.TryGetPropertyOverrides(monsterId, out var propertyOverrides))
				spawnMob.ApplyOverrides(propertyOverrides);

			var characterRef = character;
			spawnMob.Died += (deadMob, killer) => this.OnSpawnedDied(characterRef, deadMob, killer);

			this.Track(character, spawnMob);
			character.Map.AddMonster(spawnMob);

			if (buffId.HasValue)
				spawnMob.StartBuff(buffId.Value, 1, 0, TimeSpan.Zero, character);
		}

		/// <summary>
		/// Resolves the objective when a spawned monster dies. A clean
		/// kill by the character increments the kill counter; any other
		/// outcome resets this objective and optionally the prerequisite.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="mob"></param>
		/// <param name="killer"></param>
		private void OnSpawnedDied(Character character, Mob mob, ICombatEntity killer)
		{
			this.Untrack(character, mob);

			if (!character.IsOnline)
				return;

			var killedByCharacter = mob.GetKillBeneficiary(killer) == character;

			character.Quests.UpdateObjectives<LayeredKillObjective>((quest, objective, progress) =>
			{
				if (objective != this)
					return;
				if (progress.Done)
					return;
				if (!objective.IsSpawned(character, quest))
					return;

				if (killedByCharacter)
				{
					progress.Count = Math.Min(objective.TargetCount, progress.Count + 1);
					character.Quests.UpdateQuestProgress(quest.Data.Id.Value, objective.Id);

					if (progress.Count >= objective.TargetCount)
					{
						progress.Done = true;
						objective.Completed?.Invoke(character, this);
						character.Quests.CompleteObjective(quest.Data.Id.Value, objective.Ident);
						objective.ClearSpawned(character, quest);
						objective.DespawnAll(character);
						objective.ReturnFromLayer(character);
					}
				}
				else
				{
					objective.ClearSpawned(character, quest);
					objective.DespawnAll(character);
					objective.Fail(character, quest, progress);
					objective.ReturnFromLayer(character);
				}
			});
		}

		/// <summary>
		/// Relocks this objective and wipes the prerequisite so the
		/// encounter has to be earned again before it can respawn.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="quest"></param>
		/// <param name="progress"></param>
		private void Fail(Character character, Quest quest, QuestProgress progress)
		{
			progress.Count = 0;
			progress.Unlocked = false;

			if (!string.IsNullOrEmpty(this.ResetIdent) && quest.TryGetProgress(this.ResetIdent, out var prereqProgress))
			{
				prereqProgress.Count = 0;
				prereqProgress.Done = false;
				character.Quests.UpdateQuestProgress(quest.Data.Id.Value, prereqProgress.Objective.Id);
			}

			character.Quests.UpdateQuestProgress(quest.Data.Id.Value, this.Id);
		}

		/// <summary>
		/// Returns true if the character is standing on one of the maps
		/// the quest takes place on.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="quest"></param>
		private bool IsQuestLocation(Character character, Quest quest)
		{
			var location = quest.Data.Location;
			if (string.IsNullOrWhiteSpace(location))
				return true;

			var mapClassName = character.Map.ClassName;

			foreach (var mapName in location.Split(','))
			{
				if (mapName.Trim().Equals(mapClassName, StringComparison.InvariantCultureIgnoreCase))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Adds a spawned monster to the character's tracking list.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="mob"></param>
		private void Track(Character character, Mob mob)
		{
			lock (_spawnSyncLock)
			{
				if (!_spawnedMobs.TryGetValue(character.ObjectId, out var mobs))
					_spawnedMobs[character.ObjectId] = mobs = new List<Mob>();

				mobs.Add(mob);
			}
		}

		/// <summary>
		/// Removes a spawned monster from the character's tracking list.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="mob"></param>
		private void Untrack(Character character, Mob mob)
		{
			lock (_spawnSyncLock)
			{
				if (!_spawnedMobs.TryGetValue(character.ObjectId, out var mobs))
					return;

				mobs.Remove(mob);

				if (mobs.Count == 0)
					_spawnedMobs.Remove(character.ObjectId);
			}
		}

		/// <summary>
		/// Removes every monster this objective spawned for the character,
		/// so none are left behind on an abandoned layer.
		/// </summary>
		/// <param name="character"></param>
		private void DespawnAll(Character character)
		{
			List<Mob> mobs;

			lock (_spawnSyncLock)
			{
				if (!_spawnedMobs.TryGetValue(character.ObjectId, out mobs))
					return;

				_spawnedMobs.Remove(character.ObjectId);
			}

			foreach (var mob in mobs)
				mob.Map?.RemoveMonster(mob);
		}

		/// <summary>
		/// Returns the character to the default layer if they are
		/// still on a private quest layer.
		/// </summary>
		/// <param name="character"></param>
		private void ReturnFromLayer(Character character)
		{
			if (character?.Map == null)
				return;
			if (character.Layer != Map.DefaultLayer)
				character.StopLayer();
		}

		private string StateKey(Quest quest)
			=> this.StateKeyById(quest.Data.Id.Value);

		private string StateKeyById(long questId)
			=> "LayeredKill." + questId + "." + this.Ident + ".spawned";

		private bool IsSpawned(Character character, Quest quest)
			=> character.Variables.Temp.GetBool(this.StateKey(quest), false);

		private void SetSpawned(Character character, Quest quest, bool value)
			=> character.Variables.Temp.SetBool(this.StateKey(quest), value);

		private void ClearSpawned(Character character, Quest quest)
			=> character.Variables.Temp.Remove(this.StateKey(quest));
	}
}
