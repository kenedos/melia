using System.Linq;
using g4;
using Melia.Shared.Data.Database;
using Yggdrasil.Geometry;

namespace Melia.Zone.World.Maps.NavMesh
{
	/// <summary>
	/// A utility class for converting navigation graph data representations.
	/// </summary>
	public static class NavGraphBuilder
	{
		/// <summary>
		/// Converts serialized navgraph data into an array of runtime Polygon2d objects.
		/// </summary>
		public static Polygon2d[] ConvertToPolygons(NavGraphData data)
		{
			return data.Nodes.Select(node =>
				new Polygon2d(node.Vertices.Select(v => new Vector2d(v.X, v.Y)).ToArray())
			).ToArray();
		}

		/// <summary>
		/// Converts an array of runtime Polygon2d objects into serializable navgraph data.
		/// </summary>
		public static NavGraphData ConvertFromPolygons(string mapName, Polygon2d[] polygons)
		{
			var nodes = polygons.Select((poly, idx) =>
			{
				var vertices = poly.Vertices.Select(v => new VertexData { X = (float)v.x, Y = (float)v.y, Z = 0 }).ToArray();
				return new VertexListData
				{
					Index = idx,
					Vertices = vertices,
					Indices = Enumerable.Range(0, vertices.Length).ToArray()
				};
			}).ToArray();

			return new NavGraphData
			{
				MapName = mapName,
				Nodes = nodes
			};
		}
	}
}
