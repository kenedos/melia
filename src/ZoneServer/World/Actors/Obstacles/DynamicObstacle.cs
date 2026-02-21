using Melia.Shared.World;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry;

namespace Melia.Zone.World.Actors
{
	/// <summary>
	/// Represents a dynamic obstacle in the game world.
	/// </summary>
	public class DynamicObstacle : Actor
	{
		/// <summary>
		/// Gets the shape of the obstacle.
		/// </summary>
		public IShapeF Shape { get; }

		/// <summary>
		/// Gets or sets the name of the obstacle.
		/// </summary>
		public override string Name { get; set; }

		/// <summary>
		/// Creates a new DynamicObstacle instance.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="shape"></param>
		/// <param name="name"></param>
		public DynamicObstacle(Position position, IShapeF shape, string name = "Obstacle")
		{
			this.Position = position;
			this.Shape = shape;
			this.Name = name;
		}

		/// <summary>
		/// Returns true if a point intersects with this obstacle.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public bool Intersects(Vector2F point)
		{
			return this.Shape.IsInside(point);
		}
	}
}
