using Melia.Shared.World;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Maps;

/// <summary>
/// A stretch of the map a quest puts a wall across. The NPC standing in the
/// gap is only a picture, so the part that stops the player is a dynamic
/// obstacle, which is what the pathfinder walks around.
/// </summary>
/// <remarks>
/// The wall is a fact about the map rather than about one character, so it is
/// up for as long as anything asks for it. A quest that puts a wall up has to
/// take it down again; reconciling it against the quest state whenever it
/// handles the quest is enough, and both calls are idempotent.
/// </remarks>
public class MapBarrier
{
	private readonly Map _map;
	private readonly DynamicObstacle _obstacle;
	private bool _up;

	/// <summary>
	/// Creates a barrier, without putting it up.
	/// </summary>
	/// <param name="map">The map the barrier stands on.</param>
	/// <param name="name">Name the obstacle is known by.</param>
	/// <param name="position">Where the barrier stands.</param>
	/// <param name="radius">How far around that point is blocked, in units.</param>
	public MapBarrier(Map map, string name, Position position, float radius)
	{
		_map = map;
		_obstacle = new DynamicObstacle(position, new Circle(position, radius), name);
	}

	/// <summary>
	/// Returns whether the barrier is currently blocking the map.
	/// </summary>
	public bool IsUp => _up;

	/// <summary>
	/// Puts the barrier up or takes it down, doing nothing when it is already
	/// in the requested state.
	/// </summary>
	/// <param name="blocked"></param>
	public void Set(bool blocked)
	{
		if (blocked == _up)
			return;

		_up = blocked;

		if (blocked)
			_map.AddObstacle(_obstacle);
		else
			_map.RemoveObstacle(_obstacle);
	}
}
