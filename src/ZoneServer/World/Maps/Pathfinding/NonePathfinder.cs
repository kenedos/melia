using System.Collections.Generic;
using Melia.Shared.World;

namespace Melia.Zone.World.Maps.Pathfinding
{
	/// <summary>
	/// A pathfinder that does not perform pathfinding, and instead returns a direct, unchecked path
	/// to a destination.
	/// </summary>
	public class NonePathfinder : IPathfinder
	{
		/// <summary>
		/// Returns an unchecked path with only the start and destination positions.
		/// </summary>
		/// <param name="start">The starting position for the path.</param>
		/// <param name="destination">The desired destination for the path.</param>
		/// <param name="actorRadius">This parameter is ignored.</param>
		/// <param name="path">When this method returns, contains a list with the start and destination positions.</param>
		/// <returns>Always returns true.</returns>
		public bool TryFindPath(Position start, Position destination, float actorRadius, out List<Position> path)
		{
			path = [start, destination];
			return true;
		}
	}
}
