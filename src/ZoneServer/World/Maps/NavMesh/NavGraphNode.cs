using System.Collections.Generic;
using System.Linq;
using g4;
using Yggdrasil.Geometry;

namespace Melia.Zone.World.Maps.NavMesh
{
	/// <summary>
	/// Represents a single node (a convex polygon) in a navigation graph.
	/// </summary>
	public class NavGraphNode
	{
		public int Index { get; }
		public Polygon2d Polygon { get; }
		public List<int> Neighbors { get; }
		public Vector2d Centroid { get; }

		public NavGraphNode(int index, Polygon2d polygon)
		{
			this.Index = index;
			this.Polygon = polygon;
			this.Neighbors = new List<int>();
			this.Centroid = PolygonUtils.ComputeCentroid(polygon);
		}
	}
}
