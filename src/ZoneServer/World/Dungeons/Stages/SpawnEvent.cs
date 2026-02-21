using System;
using Melia.Shared.World;

namespace Melia.Zone.World.Dungeons.Stages
{
	/// <summary>
	/// A data structure that declaratively defines a single spawn action
	/// within a timeline-based dungeon stage.
	/// </summary>
	public class SpawnEvent
	{
		public TimeSpan Delay { get; }
		public int MonsterId { get; }
		public int Count { get; }
		public Position Position { get; }
		public float SpawnRadius { get; }
		public string Properties { get; }
		public string AiScript { get; }

		public SpawnEvent(TimeSpan delay, int monsterId, int count, Position position, float spawnRadius = 5f, string properties = null, string aiScript = null)
		{
			this.Delay = delay;
			this.MonsterId = monsterId;
			this.Count = count;
			this.Position = position;
			this.SpawnRadius = spawnRadius;
			this.Properties = properties;
			this.AiScript = aiScript;
		}
	}
}
