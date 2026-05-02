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
	/// spawns a list of monsters there once the objective becomes
	/// unlocked (typically via <see cref="QuestUnlockType.Sequential"/>).
	/// All spawned monsters must be killed by the character to complete.
	/// If any spawned monster dies without the character getting credit
	/// (timer expires, killed by someone else, etc.), this objective
	/// resets and may also reset a named prerequisite objective so the
	/// player has to redo it before retriggering the spawn.
	/// </summary>
	public class LayeredKillObjective : QuestObjective
	{
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
		/// Subscribes to kill events.
		/// </summary>
		public override void Load()
		{
			ZoneServer.Instance.ServerEvents.EntityKilled.Subscribe(this.OnEntityKilled);
		}

		/// <summary>
		/// Unsubscribes from kill events.
		/// </summary>
		public override void Unload()
		{
			ZoneServer.Instance.ServerEvents.EntityKilled.Unsubscribe(this.OnEntityKilled);
		}

		/// <summary>
		/// Polls on every kill so the spawn fires as soon as the
		/// objective is unlocked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnEntityKilled(object sender, CombatEventArgs args)
		{
			if (args.Target is not IMonster)
				return;

			Character character;
			if (args.Target is Mob mob)
			{
				character = mob.GetKillBeneficiary(args.Attacker);
				if (character == null)
					return;
			}
			else if (args.Attacker is Character attackerCharacter)
			{
				character = attackerCharacter;
			}
			else
			{
				return;
			}

			this.TrySpawn(character);
		}

		/// <summary>
		/// Spawns the monsters on a private layer if this objective is
		/// unlocked and the spawn has not yet happened.
		/// </summary>
		/// <param name="character"></param>
		private void TrySpawn(Character character)
		{
			character.Quests.UpdateObjectives<LayeredKillObjective>((quest, objective, progress) =>
			{
				if (progress.Done)
					return;
				if (!progress.Unlocked)
					return;
				if (objective.IsSpawned(character, quest))
					return;

				objective.SpawnAll(character, quest);
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
						objective.ReturnFromLayer(character);
					}
				}
				else
				{
					objective.ClearSpawned(character, quest);
					progress.Count = 0;
					progress.Unlocked = false;

					if (!string.IsNullOrEmpty(objective.ResetIdent) && quest.TryGetProgress(objective.ResetIdent, out var prereqProgress))
					{
						prereqProgress.Count = 0;
						prereqProgress.Done = false;
						character.Quests.UpdateQuestProgress(quest.Data.Id.Value, prereqProgress.Objective.Id);
					}

					character.Quests.UpdateQuestProgress(quest.Data.Id.Value, objective.Id);
					objective.ReturnFromLayer(character);
				}
			});
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
			=> "LayeredKill." + quest.Data.Id.Value + "." + this.Ident + ".spawned";

		private bool IsSpawned(Character character, Quest quest)
			=> character.Variables.Perm.GetBool(this.StateKey(quest), false);

		private void SetSpawned(Character character, Quest quest, bool value)
			=> character.Variables.Perm.SetBool(this.StateKey(quest), value);

		private void ClearSpawned(Character character, Quest quest)
			=> character.Variables.Perm.Remove(this.StateKey(quest));
	}
}
