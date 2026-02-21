using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Zone.Scripting;

namespace Melia.Zone.World.Dungeons.Stages
{
	/// <summary>
	/// A specialized WaveSurvivalStage for the Uphill Defense mission that
	/// handles spawning rewards for the previously completed stage.
	/// </summary>
	public class UphillWaveSurvivalStage : WaveSurvivalStage
	{
		private readonly Action<InstanceDungeon> _onStageStartAction;

		public UphillWaveSurvivalStage(Action<InstanceDungeon> onStageStartAction, TimeSpan duration, List<Wave> waves, DungeonScript dungeonScript, string stageId, string message)
			: base(duration, waves, dungeonScript, stageId, message)
		{
			_onStageStartAction = onStageStartAction;
		}

		public override Task Initialize(InstanceDungeon instance)
		{
			// Execute the action provided, e.g., to spawn the previous stage's reward chest.
			_onStageStartAction?.Invoke(instance);

			// Proceed with the normal stage initialization.
			return base.Initialize(instance);
		}
	}
}
