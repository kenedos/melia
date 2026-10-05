using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melia.Zone.World.Quests
{
	/// <summary>
	/// Specifies a quest's track.
	/// </summary>
	public class QuestTrackData
	{
		public string TrackName { get; set; }
		public int QuestId { get; set; }
		public QuestStatus OnTrackStart { get; set; } = QuestStatus.Possible;
		public QuestStatus OnTrackEnd { get; set; } = QuestStatus.Possible;
		public TimeSpan StartDelay { get; set; } = TimeSpan.Zero;
		public string EffectName { get; internal set; }
		public string Dialog { get; internal set; }
	}
}
