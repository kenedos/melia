using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Dungeons;
using Melia.Zone.World.Quests;

namespace Melia.Zone.World.Dungeons.Stages
{
	/// <summary>
	/// A stage that "unlocks" persistent content (warp portals, reward chests, etc.)
	/// that remains until the dungeon ends. Unlike regular stages, content spawned
	/// by an UnlockStage is NOT cleaned up when the stage completes.
	/// 
	/// Key behaviors:
	/// - Executes an action (spawn NPCs, open doors, show messages)
	/// - Content persists until dungeon end (tracked separately from stage monsters)
	/// - Immediately transitions to next stage after action completes (non-blocking by default)
	/// - Can optionally have objectives to block progression
	/// </summary>
	public class UnlockStage : DungeonStage
	{
		private readonly Func<InstanceDungeon, DungeonScript, UnlockStage, Task> _action;
		private readonly DungeonScript _script;

		/// <summary>
		/// List of persistent monsters/NPCs spawned by this stage.
		/// These are NOT cleaned up when the stage completes.
		/// They are cleaned up when the dungeon ends.
		/// </summary>
		public List<IMonster> PersistentMonsters { get; } = new List<IMonster>();

		/// <summary>
		/// If true (default), this stage transitions immediately after the action completes.
		/// If false, waits for objectives to complete before transitioning.
		/// </summary>
		public bool AutoTransition { get; set; } = true;

		/// <summary>
		/// Creates a new unlock stage.
		/// </summary>
		/// <param name="action">Action to execute. Receives instance, script, and this stage for spawning persistent content.</param>
		/// <param name="objectives">Optional objectives. If provided and AutoTransition is false, stage blocks until complete.</param>
		/// <param name="script">The dungeon script.</param>
		/// <param name="stageId">Unique stage identifier.</param>
		/// <param name="message">Optional stage message.</param>
		public UnlockStage(
			Func<InstanceDungeon, DungeonScript, UnlockStage, Task> action,
			List<QuestObjective> objectives,
			DungeonScript script,
			string stageId,
			string message = null)
			: base(script, stageId, message)
		{
			_action = action ?? throw new ArgumentNullException(nameof(action));
			_script = script ?? throw new ArgumentNullException(nameof(script));

			if (objectives != null)
			{
				foreach (var obj in objectives)
					this.Objectives.Add(obj);
			}
		}

		/// <summary>
		/// Spawns a persistent NPC that will remain until the dungeon ends.
		/// Use this instead of script.SpawnNpc() for content that should persist.
		/// </summary>
		public Npc SpawnPersistentNpc(InstanceDungeon instance, int monsterId, string name, Position pos, Direction dir = default)
		{
			var npc = _script.SpawnNpc(instance, monsterId, name, pos, dir);
			PersistentMonsters.Add(npc);
			instance.RegisterPersistentMonster(npc);
			return npc;
		}

		/// <summary>
		/// Spawns a persistent NPC with properties that will remain until the dungeon ends.
		/// </summary>
		public Npc SpawnPersistentNpcWithProperties(InstanceDungeon instance, int monsterId, Position pos, string dialogName)
		{
			var npc = _script.SpawnNpcWithProperties(instance, monsterId, pos, dialogName);
			PersistentMonsters.Add(npc);
			instance.RegisterPersistentMonster(npc);
			return npc;
		}

		/// <summary>
		/// Spawns a persistent monster that will remain until the dungeon ends.
		/// </summary>
		public Mob SpawnPersistentMonster(InstanceDungeon instance, int monsterId, Position pos)
		{
			var mob = _script.SpawnMonster(instance, monsterId, pos);
			PersistentMonsters.Add(mob);
			instance.RegisterPersistentMonster(mob);
			return mob;
		}

		/// <summary>
		/// Initializes the unlock stage.
		/// </summary>
		public override async Task Initialize(InstanceDungeon instance)
		{
			// Execute the unlock action
			await _action(instance, _script, this);

			// If auto-transition is enabled and no blocking objectives, transition immediately
			if (AutoTransition && IsObjectiveComplete())
			{
				if (DungeonScript.TryGet(instance.Id, out var script))
				{
					await instance.MoveToNextStage(script);
				}
			}
		}

		/// <summary>
		/// Override Complete to NOT clean up persistent monsters.
		/// Only cleans up regular stage monsters.
		/// </summary>
		public override void Complete()
		{
			// Clean up only non-persistent monsters (regular stage monsters)
			foreach (var monster in Monsters)
			{
				if (!PersistentMonsters.Contains(monster))
				{
					monster.Map?.RemoveMonster(monster);
				}
			}

			IsCompleted = true;
		}

		/// <summary>
		/// Manually removes a persistent monster before dungeon end.
		/// </summary>
		public void RemovePersistentMonster(IMonster monster)
		{
			if (PersistentMonsters.Remove(monster))
			{
				monster.Map?.RemoveMonster(monster);
			}
		}

		/// <summary>
		/// Removes all persistent monsters spawned by this stage.
		/// Called automatically when dungeon ends.
		/// </summary>
		public void CleanupPersistentMonsters()
		{
			foreach (var monster in PersistentMonsters)
			{
				monster.Map?.RemoveMonster(monster);
			}
			PersistentMonsters.Clear();
		}
	}
}
