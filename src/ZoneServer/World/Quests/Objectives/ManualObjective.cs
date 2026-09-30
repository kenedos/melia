namespace Melia.Zone.World.Quests.Objectives
{
	/// <summary>
	/// Objective that is completed manually by a script.
	/// </summary>
	/// <remarks>
	/// Commonly used for objectives that require the player to talk to
	/// an NPC. With a target count above one, the script advances it
	/// step by step via QuestComponent.AddObjectiveProgress.
	/// </remarks>
	public class ManualObjective : QuestObjective
	{
		/// <summary>
		/// Creates an objective that is completed manually by a script.
		/// </summary>
		/// <param name="targetCount">Amount of steps the script has to report.</param>
		public ManualObjective(int targetCount = 1)
		{
			this.TargetCount = targetCount;
		}
	}
}
