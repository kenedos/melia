using System.Collections.Generic;
using Melia.Shared.World;

namespace Melia.Zone.World.Maps.Pathfinding
{
	/// <summary>
	/// Describes a path finder.
	/// </summary>
	public interface IPathfinder
	{
		/// <summary>
		/// Finds a path that leads from the start position to the destination,
		/// returning it via out. Returns false if no path could be found.
		/// </summary>
		/// <remarks>
		/// The first element is always the start and the last one is the position
		/// closest to the destination.
		/// </remarks>
		/// <param name="start">The starting position for the path.</param>
		/// <param name="destination">The desired destination for the path.</param>
		/// <param name="actorRadius">The radius of the actor, used for clearance checks.</param>
		/// <param name="path">When this method returns, contains the sequence of waypoints for the path, or an empty list if no path was found.</param>
		/// <returns>True if a path was found; otherwise, false.</returns>
		bool TryFindPath(Position start, Position destination, float actorRadius, out List<Position> path);
	}
}
