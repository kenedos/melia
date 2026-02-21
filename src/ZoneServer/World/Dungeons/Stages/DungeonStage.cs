using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;

namespace Melia.Zone.World.Dungeons.Stages
{
	/// <summary>
	/// Special stage IDs for common terminal states.
	/// </summary>
	public static class StageId
	{
		public const string Complete = "COMPLETE";
		public const string Fail = "FAIL";
	}

	public abstract class DungeonStage : IDisposable
	{
		public DungeonScript DungeonScript { get; }
		public string StageId { get; set; }
		public int TotalMonstersSpawned { get; private set; } = 0;
		public List<Mob> Monsters { get; } = new();
		public List<Npc> NPCs { get; } = new();
		public bool IsAutoStart { get; set; }
		public bool IsCompleted { get; set; }
		public string StageMessage { get; set; }
		public List<QuestObjective> Objectives { get; } = new();

		/// <summary>
		/// List of possible transitions from this stage to other stages.
		/// </summary>
		public List<StageTransition> Transitions { get; } = new();

		protected InstanceDungeon Instance { get; private set; }

		protected DungeonStage(DungeonScript dungeonScript, string stageId = null, string message = null)
		{
			this.DungeonScript = dungeonScript;
			this.StageId = stageId ?? Guid.NewGuid().ToString();
			this.StageMessage = message;
		}

		public virtual async Task Initialize(InstanceDungeon instance)
		{
			this.Instance = instance;
			this.IsCompleted = false;

			if (!string.IsNullOrEmpty(this.StageMessage))
			{
				foreach (var character in instance.Characters)
				{
					if (character?.MapId == instance.MapId)
					{
						if (ZoneServer.Instance.Data.PacketStringDb.TryFind(this.StageMessage, out _))
							Send.ZC_NORMAL.IndunAddonMsgParam(character.Connection, 2, StageMessage, 1);
						else
							Send.ZC_TEXT(character, NoticeTextType.Gold, this.StageMessage);
					}
				}
			}

			// If the stage is set to auto-start and its objectives are already complete, move to the next stage.
			if (this.IsAutoStart && this.IsObjectiveComplete())
			{
				// Check that we are still the current stage to prevent issues from async operations.
				if (instance.CurrentStage == this)
				{
					await instance.MoveToNextStage(this.DungeonScript);
				}
			}
		}

		public virtual void Complete()
		{
			this.IsCompleted = true;

			// Note: Monsters are intentionally NOT removed when a stage completes.
			// This allows remaining monsters to persist into subsequent stages.
			// Monsters will be cleaned up when the dungeon instance ends.

			// Clean up NPCs associated with this stage (e.g., barriers, triggers)
			// Take a snapshot to avoid collection modification during iteration
			var npcsSnapshot = this.NPCs.ToList();
			foreach (var npc in npcsSnapshot)
				npc.Map?.RemoveMonster(npc);
			this.NPCs.Clear();
		}

		public virtual bool IsObjectiveComplete()
		{
			return this.Objectives.TrueForAll(o => o.IsCompleted);
		}

		/// <summary>
		/// Returns the expected number of monsters this stage will spawn.
		/// Override this in derived stage types to provide accurate counts for
		/// instance-wide kill percentage calculation.
		/// </summary>
		/// <returns>Expected monster count for this stage.</returns>
		public virtual int GetExpectedMonsterCount()
		{
			// Default implementation returns 0.
			// Derived classes should override this to provide accurate counts.
			return 0;
		}

		/// <summary>
		/// Determines the next stage to transition to based on current conditions.
		/// Returns the stage ID, or null if there are no valid transitions.
		/// </summary>
		public virtual string GetNextStageId(InstanceDungeon instance)
		{
			// Sort transitions by priority (highest first)
			var sortedTransitions = this.Transitions.OrderByDescending(t => t.Priority).ToList();

			foreach (var transition in sortedTransitions)
			{
				if (transition.IsConditionMet(instance))
				{
					transition.OnTransition?.Invoke(instance);
					return transition.TargetStageId;
				}
			}

			return null;
		}

		/// <summary>
		/// Adds a monster to this stage and sets up tracking for instance-wide stats.
		/// </summary>
		public void AddMonster(InstanceDungeon instance, Mob monster)
		{
			this.Monsters.Add(monster);
			this.TotalMonstersSpawned++;

			// Register spawn with instance-wide tracking
			instance.RegisterMonsterSpawn(1);

			// Capture reference to this stage for the death event
			var owningStage = this;

			// Hook up death event for both stage and instance tracking
			monster.Died += (source, args) =>
			{
				// Register kill with instance-wide tracking, including base exp values
				var baseExp = monster.Data?.Exp ?? 0;
				var jobExp = monster.Data?.JobExp ?? 0;
				instance.RegisterMonsterKill(1, baseExp, jobExp);

				// Only notify dungeon script if this monster belongs to the current stage.
				// This prevents kills from previous stages affecting current stage progression.
				if (instance.CurrentStage == owningStage)
				{
					if (DungeonScript.TryGet(instance.Id, out var dungeonScript))
						dungeonScript.MonsterKilled(instance, monster);
				}
			};
		}

		public void AddNpc(InstanceDungeon instance, Npc npc)
		{
			this.NPCs.Add(npc);
		}

		/// <summary>
		/// Gets the percentage of monsters killed in THIS STAGE.
		/// For instance-wide percentage, use instance.InstanceKillPercentage.
		/// </summary>
		public int GetDeadMonstersPercentage()
		{
			if (this.TotalMonstersSpawned == 0) return 0;
			var aliveCount = this.Monsters.Count(m => !m.IsDead);
			double killedCount = this.TotalMonstersSpawned - aliveCount;
			return (int)((killedCount / this.TotalMonstersSpawned) * 100.0);
		}

		/// <summary>
		/// Gets the number of alive monsters in this stage.
		/// </summary>
		public int GetAliveMonsterCount()
		{
			return this.Monsters.Count(m => !m.IsDead);
		}

		/// <summary>
		/// Gets the number of killed monsters in this stage.
		/// </summary>
		public int GetKilledMonsterCount()
		{
			return this.TotalMonstersSpawned - GetAliveMonsterCount();
		}

		public void Dispose()
		{
			// Resource cleanup if needed
		}

		// ===========================
		// TRANSITION HELPER METHODS
		// ===========================

		/// <summary>
		/// Adds a simple unconditional transition to another stage.
		/// </summary>
		public void TransitionTo(string targetStageId, Action<InstanceDungeon> onTransition = null)
		{
			this.Transitions.Add(new StageTransition(targetStageId, null, 0, onTransition));
		}

		/// <summary>
		/// Adds a conditional transition to another stage.
		/// </summary>
		public void TransitionIf(string targetStageId, Func<InstanceDungeon, bool> condition, int priority = 1, Action<InstanceDungeon> onTransition = null)
		{
			this.Transitions.Add(new StageTransition(targetStageId, condition, priority, onTransition));
		}

		/// <summary>
		/// Adds a success transition (when objectives are complete).
		/// </summary>
		public void OnSuccess(string targetStageId, Action<InstanceDungeon> onTransition = null)
		{
			this.TransitionIf(targetStageId, instance => this.IsObjectiveComplete(), 100, onTransition);
		}

		/// <summary>
		/// Adds a failure transition (when objectives fail or timeout).
		/// </summary>
		public void OnFailure(string targetStageId, Func<InstanceDungeon, bool> failCondition, Action<InstanceDungeon> onTransition = null)
		{
			this.TransitionIf(targetStageId, failCondition, 200, onTransition);
		}

		/// <summary>
		/// Adds a timeout-based failure transition. This relies on an external trigger to evaluate the condition.
		/// </summary>
		public void OnTimeout(TimeSpan timeout, string failStageId = Stages.StageId.Fail)
		{
			var startTime = DateTime.UtcNow; // This is captured when the stage is created.
											 // For this to work correctly, startTime should be reset during Initialize.
											 // A better pattern is to store the start time in instance.Vars during Initialize.
			TransitionIf(failStageId,
				instance => (DateTime.UtcNow - startTime) > timeout,
				150,
				instance =>
				{
					foreach (var character in instance.Characters)
					{
						character?.AddonMessage("NOTICE_Dm_scroll", "Time's up!", 5);
					}
				});
		}
	}
}
