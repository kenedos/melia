using System;
using Melia.Shared.World;

namespace Melia.Zone.World.Dungeons.Stages
{
	public class Wave
	{
		public TimeSpan Delay { get; set; }
		public int MonsterId { get; set; }
		public Position Position { get; set; }
		public int Count { get; set; }
	}
}
