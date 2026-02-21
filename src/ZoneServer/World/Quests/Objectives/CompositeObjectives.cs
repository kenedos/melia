using System.Collections.Generic;
using System.Linq;
using Melia.Zone.World.Quests;

namespace Melia.Zone.World.Dungeons.Stages
{
	/// <summary>
	/// Base class for objectives that contain other objectives.
	/// </summary>
	public abstract class CompositeObjective : QuestObjective
	{
		public List<QuestObjective> SubObjectives { get; } = new List<QuestObjective>();

		protected CompositeObjective(params QuestObjective[] objectives)
		{
			SubObjectives.AddRange(objectives);
		}

		public void AddObjective(QuestObjective objective)
		{
			SubObjectives.Add(objective);
		}
	}

	/// <summary>
	/// An objective that requires ALL sub-objectives to be completed (AND logic).
	/// </summary>
	public class AndObjective : CompositeObjective
	{
		public AndObjective(params QuestObjective[] objectives) : base(objectives)
		{
			this.Text = "Complete all objectives";
		}

		public override bool IsCompleted => this.SubObjectives.TrueForAll(obj => obj.IsCompleted);
	}

	/// <summary>
	/// An objective that requires ANY sub-objective to be completed (OR logic).
	/// </summary>
	public class OrObjective : CompositeObjective
	{
		public OrObjective(params QuestObjective[] objectives) : base(objectives)
		{
			this.Text = "Complete any objective";
		}

		public override bool IsCompleted => this.SubObjectives.Exists(obj => obj.IsCompleted);
	}

	/// <summary>
	/// An objective that requires a specific number of sub-objectives to be completed.
	/// </summary>
	public class CountObjective : CompositeObjective
	{
		public int RequiredCount { get; set; }

		public CountObjective(int requiredCount, params QuestObjective[] objectives) : base(objectives)
		{
			this.RequiredCount = requiredCount;
			this.Text = $"Complete {requiredCount} of {objectives.Length} objectives";
		}

		public override bool IsCompleted => this.SubObjectives.Count(obj => obj.IsCompleted) >= RequiredCount;
	}
}
