using System;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World.Quests.Objectives
{
	/// <summary>
	/// Objective to check for a character variable reaching a certain value.
	/// </summary>
	/// <remarks>
	/// This objective tracks integer variables set by scripts (e.g., NPC interactions)
	/// and completes when the variable reaches or exceeds the target value.
	/// The variable is checked periodically during the quest update cycle.
	/// </remarks>
	public class VariableCheckObjective : QuestObjective
	{
		/// <summary>
		/// Returns the name of the variable to check.
		/// </summary>
		public string VariableName { get; }

		/// <summary>
		/// Returns whether the variable is permanent or temporary.
		/// </summary>
		public bool IsPermanent { get; }

		/// <summary>
		/// Creates an objective to check for a character variable reaching
		/// a specific value.
		/// </summary>
		/// <param name="variableName">The name of the variable to track.</param>
		/// <param name="targetValue">The target value the variable needs to reach.</param>
		/// <param name="isPermanent">Whether to check permanent (true) or temporary (false) variables.</param>
		public VariableCheckObjective(string variableName, int targetValue, bool isPermanent = true)
		{
			if (string.IsNullOrWhiteSpace(variableName))
				throw new ArgumentException("Variable name cannot be null or empty.", nameof(variableName));

			if (targetValue <= 0)
				throw new ArgumentException("Target value must be greater than zero.", nameof(targetValue));

			this.VariableName = variableName;
			this.TargetCount = targetValue;
			this.IsPermanent = isPermanent;
		}

		/// <summary>
		/// Called when a player gets and starts a quest with this objective.
		/// Checks the initial value of the variable.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="quest"></param>
		public override void OnStart(Character character, Quest quest)
		{
			quest.UpdateObjectives<VariableCheckObjective>((quest, objective, progress) =>
			{
				var currentValue = this.GetVariableValue(character);
				progress.Count = Math.Min(objective.TargetCount, currentValue);
				progress.Done = progress.Count >= objective.TargetCount;
			});
		}

		/// <summary>
		/// Gets the current value of the tracked variable for a character.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public int GetVariableValue(Character character)
		{
			if (this.IsPermanent)
				return character.Variables.Perm.GetInt(this.VariableName, 0);
			else
				return character.Variables.Temp.GetInt(this.VariableName, 0);
		}

		/// <summary>
		/// Checks if the objective is complete for the given character.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public bool CheckCompletion(Character character)
		{
			return this.GetVariableValue(character) >= this.TargetCount;
		}
	}
}
