using System;
using System.Threading.Tasks;
using Melia.Zone.World.Dungeons;
using Melia.Zone.World.Dungeons.Stages;

namespace Melia.Zone.Scripting
{
	/// <summary>
	/// Helper class for building stages with branching logic.
	/// </summary>
	public static class StageBuilder
	{
		/// <summary>
		/// Creates a timed stage that fails if objectives aren't completed in time.
		/// The setupAction should add the necessary objectives to the created stage.
		/// </summary>
		public static DungeonStage CreateTimedStage(
			DungeonScript script,
			string stageId,
			string successStageId,
			string failStageId,
			TimeSpan timeLimit,
			Action<ActionStage> setupAction,
			string message = null)
		{
			ActionStage stage = null; // Declare here to be captured by the lambda

			stage = new ActionStage(
				async (instance, dungeonScript) =>
				{
					setupAction?.Invoke(stage);

					// Start a timer that will trigger a transition check if objectives aren't met in time.
					_ = Task.Run(async () =>
					{
						await Task.Delay(timeLimit);
						// If the stage is still active and objectives aren't done, force a transition evaluation.
						if (instance?.CurrentStage == stage && !stage.IsCompleted && !stage.IsObjectiveComplete())
						{
							await instance.MoveToNextStage(dungeonScript);
						}
					});
				},
				null, // Objectives are added via setupAction
				script,
				stageId,
				message
			);

			// Success transition: high priority, checks if objectives are complete.
			stage.TransitionIf(successStageId,
				instance => stage.IsObjectiveComplete(),
				100);

			// Failure transition: low priority, this is the path taken if the timer expires and objectives are not complete.
			stage.TransitionTo(failStageId); // Default transition if success isn't met

			return stage;
		}

		/// <summary>
		/// Creates a stage with simple success/fail branches based on objectives.
		/// The setupAction should configure the stage and its objectives.
		/// </summary>
		public static DungeonStage CreateBranchingStage(
			DungeonScript script,
			string stageId,
			string successStageId,
			string failStageId,
			Func<InstanceDungeon, bool> failCondition,
			Action<ActionStage> setupAction,
			string message = null)
		{
			ActionStage stage = null; // Declare for capture
			stage = new ActionStage(
				(instance, dungeonScript) =>
				{
					setupAction?.Invoke(stage);
					return Task.CompletedTask;
				},
				null, // Objectives are added via setupAction
				script,
				stageId,
				message
			);

			// A high priority transition for the failure condition.
			stage.OnFailure(failStageId, failCondition);

			// A normal priority transition for the success condition.
			stage.OnSuccess(successStageId);

			return stage;
		}
	}
}
