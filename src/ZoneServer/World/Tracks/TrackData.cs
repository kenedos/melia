using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Zone.World.Quests;

namespace Melia.Zone.World.Tracks
{
	public class TrackData
	{
		public string Id { get; set; }
		public string PropertyId { get; set; }
		public bool Cancelable { get; set; } = false;
		public TimeSpan StartDelay { get; set; } = TimeSpan.Zero;
		public int QuestId { get; set; }
		public QuestStatus OriginalQuestStatus { get; set; } = QuestStatus.Possible;
		public QuestStatus OnCompleteQuestStatus { get; set; }
		public QuestStatus OnStartQuestStatus { get; set; }
	}
}
