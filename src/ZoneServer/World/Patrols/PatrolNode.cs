using System.Collections.Generic;
using Melia.Shared.World;

namespace Melia.Zone.World.Patrols
{
	/// <summary>
	/// A point at the center of a room or corridor that monsters can
	/// patrol to.
	/// </summary>
	public class PatrolNode
	{
		/// <summary>
		/// Returns the node's position on its map.
		/// </summary>
		public Position Position { get; }

		/// <summary>
		/// Returns the distance from the node to the nearest wall.
		/// </summary>
		public float Clearance { get; }

		/// <summary>
		/// Returns the indices of the nodes this one connects to.
		/// </summary>
		public List<int> Neighbors { get; } = new();

		/// <summary>
		/// Creates a new patrol node.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="clearance"></param>
		public PatrolNode(Position position, float clearance)
		{
			this.Position = position;
			this.Clearance = clearance;
		}
	}
}
