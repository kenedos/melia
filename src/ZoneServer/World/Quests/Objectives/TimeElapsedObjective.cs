using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melia.Zone.World.Quests.Objectives
{
	public class TimeElapsedObjective : QuestObjective
	{
		public TimeSpan Duration { get; private set; }

		public TimeElapsedObjective(TimeSpan duration)
		{
			this.Duration = duration;
		}

		public void SetCompleted()
		{
			this.IncrementProgress();
		}
	}
}
