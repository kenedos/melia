using System;
using Yggdrasil.Geometry;
using Yggdrasil.Geometry.Shapes;

namespace Melia.Zone.Skills.SplashAreas
{
	/// <summary>
	/// Extension methods for IShapeF to support radius-aware hit detection.
	/// </summary>
	public static class ShapeExtensions
	{
		/// <summary>
		/// Returns whether the given position is inside the shape, or within
		/// the given radius of the shape's boundary. This accounts for entity
		/// body size in hit detection rather than treating entities as points.
		/// </summary>
		/// <param name="shape"></param>
		/// <param name="pos"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public static bool IsInsideOrInRange(this IShapeF shape, Vector2F pos, float radius)
		{
			if (shape.IsInside(pos))
				return true;

			if (radius <= 0)
				return false;

			// Fast path for circles — simple center-to-center distance
			if (shape is Circle circle)
			{
				var dx = pos.X - circle.Center.X;
				var dz = pos.Y - circle.Center.Z;
				var effectiveRadius = circle.Radius + radius;
				return dx * dx + dz * dz <= effectiveRadius * effectiveRadius;
			}

			if (shape is CircleF circleF)
			{
				var dx = pos.X - circleF.Center.X;
				var dz = pos.Y - circleF.Center.Y;
				var effectiveRadius = circleF.Radius + radius;
				return dx * dx + dz * dz <= effectiveRadius * effectiveRadius;
			}

			// Polygon-based shapes (Square, Fan, Donut, BladedFan):
			// check distance from entity center to each edge segment.
			var edges = shape.GetEdgePoints();
			if (edges.Length < 2)
				return false;

			var radiusSq = radius * radius;

			for (var i = 0; i < edges.Length; i++)
			{
				var a = edges[i];
				var b = edges[(i + 1) % edges.Length];

				if (DistanceToSegmentSq(pos, a, b) <= radiusSq)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Returns the squared distance from a point to a line segment.
		/// </summary>
		/// <param name="p"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		private static float DistanceToSegmentSq(Vector2F p, Vector2F a, Vector2F b)
		{
			var dx = b.X - a.X;
			var dy = b.Y - a.Y;
			var lengthSq = dx * dx + dy * dy;

			if (lengthSq == 0)
			{
				var ex = p.X - a.X;
				var ey = p.Y - a.Y;
				return ex * ex + ey * ey;
			}

			var t = ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / lengthSq;
			t = Math.Max(0, Math.Min(1, t));

			var projX = a.X + t * dx;
			var projY = a.Y + t * dy;
			var fx = p.X - projX;
			var fy = p.Y - projY;

			return fx * fx + fy * fy;
		}
	}
}
