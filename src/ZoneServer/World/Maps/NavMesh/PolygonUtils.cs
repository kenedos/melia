using System;
using g4;
using Yggdrasil.Geometry;

namespace Melia.Zone.World.Maps.NavMesh
{
	/// <summary>
	/// Provides utility functions for 2D polygons.
	/// </summary>
	public static class PolygonUtils
	{
		/// <summary>
		/// Computes the geometric centroid of a 2D polygon.
		/// </summary>
		/// <param name="poly">The polygon.</param>
		/// <returns>The centroid of the polygon.</returns>
		public static Vector2d ComputeCentroid(Polygon2d poly)
		{
			double cx = 0;
			double cy = 0;
			double signedArea = 0;

			var count = poly.VertexCount;
			for (var i = 0; i < count; ++i)
			{
				var v0 = poly.Vertices[i];
				var v1 = poly.Vertices[(i + 1) % count];

				var a = v0.x * v1.y - v1.x * v0.y;
				signedArea += a;
				cx += (v0.x + v1.x) * a;
				cy += (v0.y + v1.y) * a;
			}

			signedArea *= 0.5;
			if (Math.Abs(signedArea) < 1e-6)
			{
				// Fallback for zero-area or invalid polygons.
				return poly.Vertices.Count > 0 ? poly.Vertices[0] : Vector2d.Zero;
			}

			cx /= (6.0 * signedArea);
			cy /= (6.0 * signedArea);

			return new Vector2d(cx, cy);
		}
	}
}
