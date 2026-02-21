using System;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.World.Dungeons;
using Melia.Zone.World.Dungeons.Stages;

namespace Melia.Zone.World.Dungeons.Stages
{
	/// <summary>
	/// A special stage that runs once at the beginning of a dungeon to perform initial setup.
	/// After setup, it immediately moves to the next stage.
	/// </summary>
	public class InitialSetupStage : DungeonStage
	{
		private readonly Action<InstanceDungeon> _setupAction;

		public InitialSetupStage(Action<InstanceDungeon> setupAction, DungeonScript dungeonScript, string stageId = null)
			: base(dungeonScript, stageId)
		{
			_setupAction = setupAction;
			// This stage is instant and has no objectives of its own.
			IsAutoStart = true;
		}

		public override async Task Initialize(InstanceDungeon instance)
		{
			await base.Initialize(instance);

			// Execute the provided setup logic (e.g., spawn NPCs, start timers).
			_setupAction?.Invoke(instance);

			// Immediately transition to the actual first stage.
			await instance.MoveToNextStage(this.DungeonScript);
		}
	}
}
