using System;
using System.Collections.Generic;

namespace Melia.Zone.World.Dungeons.Stages
{
	/// <summary>
	/// Represents a possible transition from one stage to another with an optional condition.
	/// </summary>
	public class StageTransition
	{
		/// <summary>
		/// The ID of the target stage to transition to.
		/// </summary>
		public string TargetStageId { get; set; }

		/// <summary>
		/// Optional condition that must be met for this transition to be taken.
		/// If null, this is the default transition.
		/// </summary>
		public Func<InstanceDungeon, bool> Condition { get; set; }

		/// <summary>
		/// Priority of this transition. Higher priority transitions are evaluated first.
		/// </summary>
		public int Priority { get; set; }

		/// <summary>
		/// Optional callback to execute when this transition is taken.
		/// </summary>
		public Action<InstanceDungeon> OnTransition { get; set; }

		public StageTransition(string targetStageId, Func<InstanceDungeon, bool> condition = null, int priority = 0, Action<InstanceDungeon> onTransition = null)
		{
			this.TargetStageId = targetStageId;
			this.Condition = condition;
			this.Priority = priority;
			this.OnTransition = onTransition;
		}

		/// <summary>
		/// Checks if this transition's condition is met.
		/// </summary>
		public bool IsConditionMet(InstanceDungeon instance)
		{
			return Condition == null || Condition(instance);
		}
	}
}
