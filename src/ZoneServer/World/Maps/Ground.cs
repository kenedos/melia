using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using g4;
using Melia.Shared.Data.Database;
using Melia.Shared.Util;
using Melia.Shared.World;
using Yggdrasil.Geometry;

namespace Melia.Zone.World.Maps
{
	/// <summary>
	/// Represents a map's ground, providing collision, height, and pathing information.
	/// </summary>
	public class Ground
	{
		private const float RayOriginHeight = 30000;
		private const int MinGridCellSize = 8;
		private const int MaxGridDimension = 512;
		private const float BarycentricEpsilon = 0.0001f;

		private const float ClearanceQueryRange = 50f;
		private const float ClearanceQueryMaxRange = 4096f;

		private GroundData _data;
		private DMesh3 _mesh;
		private DMeshAABBTree3 _spatial;
		private Polygon2d[] _cells;
		private LineF[] _outlines;

		private QuadTree<int> _outlineQuadTree;

		private int _left, _right, _bottom, _top;

		private int _gridCellSize;
		private int _gridWidth;
		private int _gridHeight;
		private float _gridMinX;
		private float _gridMinZ;

		private int[] _triangleGridStarts;
		private int[] _triangleGridItems;
		private int[] _cellGridStarts;
		private int[] _cellGridItems;

		private SampleTriangle[] _sampleTriangles;
		private float[] _sampleAreas;
		private float _sampleTotalArea;

		[ThreadStatic]
		private static List<int> OutlineQueryBuffer;

		[ThreadStatic]
		private static List<int> ClearanceQueryBuffer;

		/// <summary>Returns the width of the ground in world units.</summary>
		public int SizeX => _right - _left;

		/// <summary>Returns the depth of the ground in world units.</summary>
		public int SizeZ => _top - _bottom;

		/// <summary>Returns the left (minimum X) boundary of the ground.</summary>
		public int Left => _left;

		/// <summary>Returns the right (maximum X) boundary of the ground.</summary>
		public int Right => _right;

		/// <summary>Returns the bottom (minimum Z) boundary of the ground.</summary>
		public int Bottom => _bottom;

		/// <summary>Returns the top (maximum Z) boundary of the ground.</summary>
		public int Top => _top;

		/// <summary>
		/// Returns the data that was used to create the ground.
		/// </summary>
		public GroundData Data => _data;

		/// <summary>
		/// Loads the ground data and builds internal spatial structures.
		/// </summary>
		/// <param name="data">The ground data to load.</param>
		public async Task LoadAsync(GroundData data)
		{
			_data = data;

			if (!this.HasData()) return;

			this.CacheBounds();

			// Load these in parallel since they're independent
			var meshTask = Task.Run(() => this.LoadGroundMesh());
			var cellsTask = Task.Run(() => this.LoadCells());
			var outlinesTask = meshTask.ContinueWith(_ => this.LoadOutlines());

			await Task.WhenAll(meshTask, cellsTask);
			await outlinesTask;

			this.BuildSpatialIndices();
		}

		/// <summary>
		/// Synchronous version for backward compatibility
		/// </summary>
		public void Load(GroundData data)
		{
			LoadAsync(data).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Caches the ground's extents, which the data itself recomputes by
		/// scanning every vertex on each access.
		/// </summary>
		private void CacheBounds()
		{
			var vertices = _data.Vertices;

			var minX = vertices[0].X;
			var maxX = vertices[0].X;
			var minZ = vertices[0].Y;
			var maxZ = vertices[0].Y;

			for (var i = 1; i < vertices.Length; i++)
			{
				var vertex = vertices[i];

				if (vertex.X < minX) minX = vertex.X;
				if (vertex.X > maxX) maxX = vertex.X;
				if (vertex.Y < minZ) minZ = vertex.Y;
				if (vertex.Y > maxZ) maxZ = vertex.Y;
			}

			_left = (int)minX;
			_right = (int)maxX;
			_bottom = (int)minZ;
			_top = (int)maxZ;
		}

		/// <summary>
		/// Builds the lookup grids and the outline index.
		/// </summary>
		private void BuildSpatialIndices()
		{
			this.SetUpGrid();
			this.BuildTriangleGrid();
			this.BuildCellGrid();
			this.BuildCellSampling();
			this.BuildOutlineIndex();
		}

		/// <summary>
		/// Determines the resolution and dimensions of the lookup grids.
		/// </summary>
		private void SetUpGrid()
		{
			_gridMinX = _left;
			_gridMinZ = _bottom;

			var sizeX = Math.Max(1, _right - _left);
			var sizeZ = Math.Max(1, _top - _bottom);
			var maxSize = Math.Max(sizeX, sizeZ);

			_gridCellSize = Math.Max(MinGridCellSize, (int)Math.Ceiling(maxSize / (float)MaxGridDimension));
			_gridWidth = sizeX / _gridCellSize + 1;
			_gridHeight = sizeZ / _gridCellSize + 1;
		}

		/// <summary>
		/// Buckets the mesh triangles into the lookup grid by their 2D bounds.
		/// </summary>
		private void BuildTriangleGrid()
		{
			var triangles = _data.Triangles;
			if (triangles == null || triangles.Length == 0)
				return;

			var bounds = new float[triangles.Length * 4];

			for (var i = 0; i < triangles.Length; i++)
			{
				var vertices = triangles[i].Vertices;
				var minX = vertices[0].X;
				var maxX = vertices[0].X;
				var minZ = vertices[0].Y;
				var maxZ = vertices[0].Y;

				for (var j = 1; j < 3; j++)
				{
					if (vertices[j].X < minX) minX = vertices[j].X;
					if (vertices[j].X > maxX) maxX = vertices[j].X;
					if (vertices[j].Y < minZ) minZ = vertices[j].Y;
					if (vertices[j].Y > maxZ) maxZ = vertices[j].Y;
				}

				bounds[i * 4 + 0] = minX;
				bounds[i * 4 + 1] = minZ;
				bounds[i * 4 + 2] = maxX;
				bounds[i * 4 + 3] = maxZ;
			}

			this.BuildIndexGrid(bounds, triangles.Length, out _triangleGridStarts, out _triangleGridItems);
		}

		/// <summary>
		/// Buckets the walkable cell polygons into the lookup grid by their
		/// 2D bounds.
		/// </summary>
		private void BuildCellGrid()
		{
			if (_cells == null || _cells.Length == 0)
				return;

			var bounds = new float[_cells.Length * 4];

			for (var i = 0; i < _cells.Length; i++)
			{
				if (_cells[i] == null)
				{
					bounds[i * 4] = float.NaN;
					continue;
				}

				var cellBounds = _cells[i].Bounds;

				bounds[i * 4 + 0] = (float)cellBounds.Min.x;
				bounds[i * 4 + 1] = (float)cellBounds.Min.y;
				bounds[i * 4 + 2] = (float)cellBounds.Max.x;
				bounds[i * 4 + 3] = (float)cellBounds.Max.y;
			}

			this.BuildIndexGrid(bounds, _cells.Length, out _cellGridStarts, out _cellGridItems);
		}

		/// <summary>
		/// Builds a grid index in compressed sparse row form, bucketing every
		/// item into each grid cell its 2D bounds overlap.
		/// </summary>
		/// <param name="bounds">Four floats per item, min X, min Z, max X, max Z. A NaN min X skips the item.</param>
		/// <param name="itemCount"></param>
		/// <param name="starts"></param>
		/// <param name="items"></param>
		private void BuildIndexGrid(float[] bounds, int itemCount, out int[] starts, out int[] items)
		{
			var gridCellCount = _gridWidth * _gridHeight;
			var offsets = new int[gridCellCount + 1];

			for (var i = 0; i < itemCount; i++)
			{
				if (!this.TryGetGridRange(bounds, i, out var minCellX, out var minCellZ, out var maxCellX, out var maxCellZ))
					continue;

				for (var gz = minCellZ; gz <= maxCellZ; gz++)
				{
					for (var gx = minCellX; gx <= maxCellX; gx++)
						offsets[gz * _gridWidth + gx + 1]++;
				}
			}

			for (var i = 0; i < gridCellCount; i++)
				offsets[i + 1] += offsets[i];

			var cursors = new int[gridCellCount];
			var entries = new int[offsets[gridCellCount]];

			for (var i = 0; i < itemCount; i++)
			{
				if (!this.TryGetGridRange(bounds, i, out var minCellX, out var minCellZ, out var maxCellX, out var maxCellZ))
					continue;

				for (var gz = minCellZ; gz <= maxCellZ; gz++)
				{
					for (var gx = minCellX; gx <= maxCellX; gx++)
					{
						var gridCell = gz * _gridWidth + gx;
						entries[offsets[gridCell] + cursors[gridCell]++] = i;
					}
				}
			}

			starts = offsets;
			items = entries;
		}

		/// <summary>
		/// Returns the inclusive range of grid cells the item's bounds cover
		/// via out. Returns false if the item is skipped.
		/// </summary>
		/// <param name="bounds"></param>
		/// <param name="index"></param>
		/// <param name="minCellX"></param>
		/// <param name="minCellZ"></param>
		/// <param name="maxCellX"></param>
		/// <param name="maxCellZ"></param>
		/// <returns></returns>
		private bool TryGetGridRange(float[] bounds, int index, out int minCellX, out int minCellZ, out int maxCellX, out int maxCellZ)
		{
			minCellX = minCellZ = maxCellX = maxCellZ = 0;

			var minX = bounds[index * 4 + 0];
			if (float.IsNaN(minX))
				return false;

			minCellX = Math.Clamp((int)((minX - _gridMinX) / _gridCellSize), 0, _gridWidth - 1);
			minCellZ = Math.Clamp((int)((bounds[index * 4 + 1] - _gridMinZ) / _gridCellSize), 0, _gridHeight - 1);
			maxCellX = Math.Clamp((int)((bounds[index * 4 + 2] - _gridMinX) / _gridCellSize), 0, _gridWidth - 1);
			maxCellZ = Math.Clamp((int)((bounds[index * 4 + 3] - _gridMinZ) / _gridCellSize), 0, _gridHeight - 1);

			return true;
		}

		/// <summary>
		/// Returns the index of the grid cell containing the given 2D
		/// position via out. Returns false if the position is off the grid.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <param name="gridCell"></param>
		/// <returns></returns>
		private bool TryGetGridCell(float x, float z, out int gridCell)
		{
			gridCell = -1;

			if (_gridWidth <= 0)
				return false;

			var offsetX = (x - _gridMinX) / _gridCellSize;
			var offsetZ = (z - _gridMinZ) / _gridCellSize;

			if (offsetX < 0 || offsetZ < 0)
				return false;

			var cellX = (int)offsetX;
			var cellZ = (int)offsetZ;

			if (cellX >= _gridWidth || cellZ >= _gridHeight)
				return false;

			gridCell = cellZ * _gridWidth + cellX;
			return true;
		}

		/// <summary>
		/// Triangulates the walkable cells and builds the cumulative area
		/// table used to sample random positions.
		/// </summary>
		private void BuildCellSampling()
		{
			if (_data.Cells == null)
				return;

			var triangles = new List<SampleTriangle>();
			var areas = new List<float>();
			var total = 0f;

			foreach (var cell in _data.Cells)
			{
				if (cell?.Indices == null || cell.Indices.Length < 3)
					continue;

				for (var i = 1; i < cell.Indices.Length - 1; i++)
				{
					var triangle = new SampleTriangle(cell.Indices[0], cell.Indices[i], cell.Indices[i + 1]);
					var area = this.GetTriangleArea(triangle);

					if (area <= 0)
						continue;

					total += area;

					triangles.Add(triangle);
					areas.Add(total);
				}
			}

			_sampleTriangles = triangles.ToArray();
			_sampleAreas = areas.ToArray();
			_sampleTotalArea = total;
		}

		/// <summary>
		/// Returns the 2D area of the given triangle.
		/// </summary>
		/// <param name="triangle"></param>
		/// <returns></returns>
		private float GetTriangleArea(SampleTriangle triangle)
		{
			var a = _data.Vertices[triangle.V0];
			var b = _data.Vertices[triangle.V1];
			var c = _data.Vertices[triangle.V2];

			return Math.Abs((b.X - a.X) * (c.Y - a.Y) - (c.X - a.X) * (b.Y - a.Y)) * 0.5f;
		}

		/// <summary>
		/// Builds the quad tree used to test lines against the ground's
		/// outer boundary.
		/// </summary>
		private void BuildOutlineIndex()
		{
			if (_outlines == null || _outlines.Length == 0)
				return;

			var first = _outlines[0];
			double minX = Math.Min(first.Point1.X, first.Point2.X);
			double minY = Math.Min(first.Point1.Y, first.Point2.Y);
			double maxX = Math.Max(first.Point1.X, first.Point2.X);
			double maxY = Math.Max(first.Point1.Y, first.Point2.Y);

			for (var i = 1; i < _outlines.Length; i++)
			{
				var outline = _outlines[i];

				minX = Math.Min(minX, Math.Min(outline.Point1.X, outline.Point2.X));
				minY = Math.Min(minY, Math.Min(outline.Point1.Y, outline.Point2.Y));
				maxX = Math.Max(maxX, Math.Max(outline.Point1.X, outline.Point2.X));
				maxY = Math.Max(maxY, Math.Max(outline.Point1.Y, outline.Point2.Y));
			}

			_outlineQuadTree = new QuadTree<int>(new AxisAlignedBox2d(minX, minY, maxX, maxY), maxDepth: 6, maxObjectsPerNode: 10);

			for (var i = 0; i < _outlines.Length; i++)
			{
				var lineBounds = new AxisAlignedBox2d(
					Math.Min(_outlines[i].Point1.X, _outlines[i].Point2.X),
					Math.Min(_outlines[i].Point1.Y, _outlines[i].Point2.Y),
					Math.Max(_outlines[i].Point1.X, _outlines[i].Point2.X),
					Math.Max(_outlines[i].Point1.Y, _outlines[i].Point2.Y));

				_outlineQuadTree.Insert(i, lineBounds);
			}
		}

		/// <summary>
		/// Generates the outline of the ground mesh, for simpler 2D collision checks.
		/// </summary>
		private void LoadOutlines()
		{
			var outlines = new List<LineF>();
			var boundaryFinder = new MeshBoundaryLoops(_mesh);

			foreach (var loop in boundaryFinder.Loops)
			{
				for (var i = 0; i < loop.VertexCount; i++)
				{
					var vert1 = _mesh.GetVertex(loop.Vertices[i]);
					var vert2 = _mesh.GetVertex(loop.Vertices[(i + 1) % loop.VertexCount]);

					var p1 = new Vector2F((float)vert1.x, (float)vert1.z);
					var p2 = new Vector2F((float)vert2.x, (float)vert2.z);

					outlines.Add(new LineF(p1, p2));
				}
			}

			_outlines = outlines.ToArray();
		}

		/// <summary>
		/// Generates a 3D mesh for the ground that can be used for raycasting.
		/// </summary>
		private void LoadGroundMesh()
		{
			var srcVerts = _data.Vertices;
			var srcTris = _data.Triangles;

			var vertices = new Vector3f[srcVerts.Length];
			for (var i = 0; i < srcVerts.Length; i++)
				vertices[i] = new Vector3f(srcVerts[i].X, srcVerts[i].Z, srcVerts[i].Y);

			var triangles = new Index3i[srcTris.Length];
			for (var i = 0; i < srcTris.Length; i++)
				triangles[i] = new Index3i(srcTris[i].Indices[0], srcTris[i].Indices[1], srcTris[i].Indices[2]);

			_mesh = DMesh3Builder.Build<Vector3f, Index3i, Vector3f>(vertices, triangles, null, null);
			_spatial = new DMeshAABBTree3(_mesh, autoBuild: true);
		}

		/// <summary>
		/// Generates 2D polygon representations of walkable cells.
		/// </summary>
		private void LoadCells()
		{
			_cells = new Polygon2d[_data.Cells.Length];

			for (var i = 0; i < _data.Cells.Length; ++i)
			{
				var cellData = _data.Cells[i];
				if (cellData == null) continue;

				var srcVerts = cellData.Vertices;
				var vertices = new Vector2d[srcVerts.Length];
				for (var j = 0; j < srcVerts.Length; j++)
					vertices[j] = new Vector2d(srcVerts[j].X, srcVerts[j].Y);

				_cells[i] = new Polygon2d(vertices);
			}
		}

		/// <summary>
		/// Returns whether the given 2D position is on a walkable cell.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public bool IsValidPosition(Position pos)
		{
			return this.TryGetCellIndex(pos, out _);
		}

		/// <summary>
		/// Returns the height of the ground at the given 2D position.
		/// If there's no ground at the position, float.NaN is returned.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public float GetHeightAt(Position pos)
		{
			return this.TryGetHeightAt(pos, out var height) ? height : float.NaN;
		}

		/// <summary>
		/// Returns the height of the ground at the given 2D position via out.
		/// Returns false if there is no ground at the position.
		/// </summary>
		/// <param name="pos">The position to check (X and Z are used).</param>
		/// <param name="height">When this method returns, contains the ground height, or NaN if no ground was found.</param>
		/// <returns>True if ground was found and height was set; otherwise, false.</returns>
		public bool TryGetHeightAt(Position pos, out float height)
		{
			height = float.NaN;

			if (_triangleGridStarts == null)
				return this.TryGetHeightFromMesh(pos, out height);

			if (!this.TryGetGridCell(pos.X, pos.Z, out var gridCell))
				return false;

			var found = false;
			var highest = 0f;

			for (var i = _triangleGridStarts[gridCell]; i < _triangleGridStarts[gridCell + 1]; i++)
			{
				if (!TryGetTriangleHeight(_data.Triangles[_triangleGridItems[i]], pos.X, pos.Z, out var triangleHeight))
					continue;

				if (!found || triangleHeight > highest)
				{
					highest = triangleHeight;
					found = true;
				}
			}

			if (!found)
				return false;

			height = highest;
			return true;
		}

		/// <summary>
		/// Returns the ground height at the given position by casting a ray
		/// down onto the mesh via out. Returns false if the ray missed.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		private bool TryGetHeightFromMesh(Position pos, out float height)
		{
			height = float.NaN;
			if (_spatial == null) return false;

			var origin = new Vector3d(pos.X, RayOriginHeight, pos.Z);
			var ray = new Ray3d(origin, Vector3d.AxisY * -1);

			var hitId = _spatial.FindNearestHitTriangle(ray);
			if (hitId == DMesh3.InvalidID) return false;

			var intersection = MeshQueries.TriangleIntersection(_mesh, hitId, ray);
			height = (float)(origin.y - intersection.RayParameter);
			return true;
		}

		/// <summary>
		/// Returns the interpolated height of the triangle at the given 2D
		/// position via out. Returns false if the position lies outside it.
		/// </summary>
		/// <param name="triangle"></param>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		private static bool TryGetTriangleHeight(VertexListData triangle, float x, float z, out float height)
		{
			height = float.NaN;

			var a = triangle.Vertices[0];
			var b = triangle.Vertices[1];
			var c = triangle.Vertices[2];

			var denominator = (b.Y - c.Y) * (a.X - c.X) + (c.X - b.X) * (a.Y - c.Y);
			if (Math.Abs(denominator) < 1e-6f)
				return false;

			var weightA = ((b.Y - c.Y) * (x - c.X) + (c.X - b.X) * (z - c.Y)) / denominator;
			var weightB = ((c.Y - a.Y) * (x - c.X) + (a.X - c.X) * (z - c.Y)) / denominator;
			var weightC = 1f - weightA - weightB;

			if (weightA < -BarycentricEpsilon || weightB < -BarycentricEpsilon || weightC < -BarycentricEpsilon)
				return false;

			height = weightA * a.Z + weightB * b.Z + weightC * c.Z;
			return true;
		}

		/// <summary>
		/// Returns a copy of position, where Y is replaced with the cell
		/// index. If no cell could be found, Y is -1.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public Position GetCellPosition(Position pos)
		{
			this.TryGetCellIndex(pos, out var cellIndex);
			pos.Y = cellIndex;
			return pos;
		}

		/// <summary>
		/// Returns the cell index for the given position via out. Returns
		/// false if no cell exists at the position.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="cellIndex"></param>
		/// <returns></returns>
		public bool TryGetCellIndex(Position pos, out int cellIndex)
		{
			cellIndex = -1;
			if (_cells == null) return false;

			var vecPos = new Vector2d(pos.X, pos.Z);

			if (_cellGridStarts != null)
			{
				if (!this.TryGetGridCell(pos.X, pos.Z, out var gridCell))
					return false;

				for (var i = _cellGridStarts[gridCell]; i < _cellGridStarts[gridCell + 1]; i++)
				{
					var candidateIndex = _cellGridItems[i];
					if (_cells[candidateIndex].Contains(vecPos))
					{
						cellIndex = candidateIndex;
						return true;
					}
				}

				return false;
			}

			// Fallback to linear scan
			for (var i = 0; i < _cells.Length; ++i)
			{
				var cell = _cells[i];
				if (cell != null && cell.Contains(vecPos))
				{
					cellIndex = i;
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Adds the indices of every cell whose bounds cover the given
		/// position to the given list, which is cleared first.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="results"></param>
		public void GetCellCandidates(Position pos, List<int> results)
		{
			results.Clear();

			if (_cells == null)
				return;

			if (_cellGridStarts == null)
			{
				for (var i = 0; i < _cells.Length; ++i)
				{
					if (_cells[i] != null)
						results.Add(i);
				}

				return;
			}

			if (!this.TryGetGridCell(pos.X, pos.Z, out var gridCell))
				return;

			for (var i = _cellGridStarts[gridCell]; i < _cellGridStarts[gridCell + 1]; i++)
				results.Add(_cellGridItems[i]);
		}

		/// <summary>
		/// Returns a random position on the walkable ground via out.
		/// Returns false if no valid position could be found.
		/// </summary>
		/// <returns></returns>
		public bool TryGetRandomPosition(out Position pos)
		{
			pos = Position.Zero;

			if (_sampleTriangles == null || _sampleTriangles.Length == 0)
				return false;

			var target = (float)(GameRandom.Get().NextDouble() * _sampleTotalArea);

			var index = Array.BinarySearch(_sampleAreas, target);
			if (index < 0) index = ~index;
			if (index >= _sampleTriangles.Length) index = _sampleTriangles.Length - 1;

			var triangle = _sampleTriangles[index];
			var a = _data.Vertices[triangle.V0];
			var b = _data.Vertices[triangle.V1];
			var c = _data.Vertices[triangle.V2];

			var weightB = (float)GameRandom.Get().NextDouble();
			var weightC = (float)GameRandom.Get().NextDouble();

			if (weightB + weightC > 1)
			{
				weightB = 1 - weightB;
				weightC = 1 - weightC;
			}

			var x = a.X + weightB * (b.X - a.X) + weightC * (c.X - a.X);
			var z = a.Y + weightB * (b.Y - a.Y) + weightC * (c.Y - a.Y);

			var candidatePos = new Position(x, 0, z);

			// The cells and the mesh disagree along the very edges, where the
			// cell's own plane is the better answer.
			if (!this.TryGetHeightAt(candidatePos, out var height))
				height = a.Z + weightB * (b.Z - a.Z) + weightC * (c.Z - a.Z);

			pos = candidatePos.WithHeight(height);
			return true;
		}

		/// <summary>
		/// Returns the last valid position on the path between origin and
		/// destination. If there are no obstacles, the destination is returned.
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="destination"></param>
		/// <returns></returns>
		public Position GetLastValidPosition(Position origin, Position destination)
		{
			var dir = origin.GetDirection(destination);
			var stepSize = 10;
			var currentPos = origin;
			var lastValidPos = currentPos;

			while (currentPos.Get2DDistance(destination) > stepSize)
			{
				currentPos = currentPos.GetRelative(dir, stepSize);

				if (!this.TryGetHeightAt(currentPos, out var height))
					return lastValidPos;

				lastValidPos = currentPos.WithHeight(height);
			}

			return destination;
		}

		/// <summary>
		/// Returns the last valid center position of a circle on the path between the origin and destination centers.
		/// </summary>
		/// <param name="originCenter"></param>
		/// <param name="radius"></param>
		/// <param name="destinationCenter"></param>
		/// <returns></returns>
		public Position GetLastValidCirclePosition(Position originCenter, float radius, Position destinationCenter)
		{
			var dir = originCenter.GetDirection(destinationCenter);
			var stepSize = Math.Max(2.5f, radius * 0.5f);
			var currentPos = originCenter;
			var lastValidPos = currentPos;

			while (currentPos.Get2DDistance(destinationCenter) > stepSize)
			{
				currentPos = currentPos.GetRelative(dir, stepSize);

				if (!this.IsValidCirclePosition(currentPos, radius))
					return lastValidPos;

				if (this.TryGetHeightAt(currentPos, out var height))
				{
					lastValidPos = currentPos.WithHeight(height);
				}
				else
				{
					// This should not happen if IsValidCirclePosition is true, but as a safeguard:
					return lastValidPos;
				}
			}

			// Final check at the destination itself.
			if (this.IsValidCirclePosition(destinationCenter, radius) && this.TryGetHeightAt(destinationCenter, out var destHeight))
			{
				return destinationCenter.WithHeight(destHeight);
			}

			return lastValidPos;
		}

		private static readonly (float dx, float dz)[] PerimeterUnitOffsets =
		[
			(1, 0), (-1, 0), (0, 1), (0, -1),
			(0.707f, 0.707f), (-0.707f, 0.707f),
			(0.707f, -0.707f), (-0.707f, -0.707f)
		];

		/// <summary>
		/// Checks if a circle is on valid ground (not on a hole or too-steep slope).
		/// </summary>
		/// <param name="center"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public bool IsValidCirclePosition(Position center, float radius)
		{
			const float maxTerrainVarianceMultiplier = 1.5f;
			var maxTerrainVariance = radius * maxTerrainVarianceMultiplier;

			if (!this.TryGetHeightAt(center, out var centerHeight)) return false;

			var minHeight = centerHeight;
			var maxHeight = centerHeight;

			foreach (var offset in PerimeterUnitOffsets)
			{
				var perimeterPos = new Position(center.X + offset.dx * radius, 0, center.Z + offset.dz * radius);
				if (!this.TryGetHeightAt(perimeterPos, out var height)) return false;

				minHeight = Math.Min(minHeight, height);
				maxHeight = Math.Max(maxHeight, height);
			}

			return (maxHeight - minHeight) <= maxTerrainVariance;
		}

		/// <summary>
		/// Returns true if a line between two positions intersects with the ground's outer boundary.
		/// Uses spatial indexing for better performance.
		/// </summary>
		/// <param name="pos1"></param>
		/// <param name="pos2"></param>
		/// <returns></returns>
		public bool AnyObstacles(Position pos1, Position pos2)
		{
			if (_outlines == null) return !this.IsValidPosition(pos2);
			if (!this.IsValidPosition(pos1) || !this.IsValidPosition(pos2)) return true;

			var pathLine = new LineF(new Vector2F(pos1.X, pos1.Z), new Vector2F(pos2.X, pos2.Z));

			if (_outlineQuadTree != null)
			{
				var candidates = OutlineQueryBuffer ??= new List<int>();
				var lineBounds = new AxisAlignedBox2d(
					Math.Min(pos1.X, pos2.X), Math.Min(pos1.Z, pos2.Z),
					Math.Max(pos1.X, pos2.X), Math.Max(pos1.Z, pos2.Z));

				_outlineQuadTree.Query(lineBounds, candidates);

				foreach (var candidateIdx in candidates)
				{
					if (pathLine.Intersects(_outlines[candidateIdx], out _))
						return true;
				}

				return false;
			}

			// Fallback to linear scan
			foreach (var outline in _outlines)
			{
				if (pathLine.Intersects(outline, out _))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Returns the distance from the given position to the nearest
		/// ground boundary via out. Returns false if the position isn't
		/// on valid ground or no boundary could be found.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="clearance"></param>
		/// <returns></returns>
		public bool TryGetClearance(Position pos, out float clearance)
		{
			clearance = 0;

			if (_outlines == null || _outlines.Length == 0)
				return false;

			if (!this.IsValidPosition(pos))
				return false;

			if (_outlineQuadTree == null)
			{
				var nearest = float.MaxValue;
				for (var i = 0; i < _outlines.Length; i++)
					nearest = Math.Min(nearest, DistanceToOutline(pos, _outlines[i]));

				clearance = nearest;
				return true;
			}

			var candidates = ClearanceQueryBuffer ??= new List<int>();

			for (var range = ClearanceQueryRange; range <= ClearanceQueryMaxRange; range *= 2)
			{
				candidates.Clear();
				_outlineQuadTree.Query(new AxisAlignedBox2d(pos.X - range, pos.Z - range, pos.X + range, pos.Z + range), candidates);

				var nearest = float.MaxValue;
				foreach (var candidateIdx in candidates)
					nearest = Math.Min(nearest, DistanceToOutline(pos, _outlines[candidateIdx]));

				// A boundary further away than the queried box can't be
				// trusted, a closer one might sit just outside of it.
				if (nearest <= range)
				{
					clearance = nearest;
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns the distance from the position to the outline segment.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="outline"></param>
		/// <returns></returns>
		private static float DistanceToOutline(Position pos, LineF outline)
		{
			var startX = outline.Point1.X;
			var startZ = outline.Point1.Y;
			var deltaX = outline.Point2.X - startX;
			var deltaZ = outline.Point2.Y - startZ;

			var lengthSquared = deltaX * deltaX + deltaZ * deltaZ;
			var offsetX = pos.X - startX;
			var offsetZ = pos.Z - startZ;

			if (lengthSquared <= float.Epsilon)
				return (float)Math.Sqrt(offsetX * offsetX + offsetZ * offsetZ);

			var projection = (offsetX * deltaX + offsetZ * deltaZ) / lengthSquared;
			projection = Math.Max(0, Math.Min(1, projection));

			var closestX = pos.X - (startX + deltaX * projection);
			var closestZ = pos.Z - (startZ + deltaZ * projection);

			return (float)Math.Sqrt(closestX * closestX + closestZ * closestZ);
		}

		/// <summary>
		/// Returns true if the ground has been loaded with data.
		/// </summary>
		public bool HasData()
		{
			return _data != null && _data.Vertices.Length > 0;
		}

		/// <summary>
		/// Attempts to find the nearest valid position to a given point.
		/// </summary>
		/// <param name="pos">Input position (X, Z used)</param>
		/// <param name="nearestPos">Nearest valid position with height.</param>
		/// <param name="maxDistance">Maximum search radius.</param>
		/// <returns>True if a valid position is found, false otherwise</returns>
		public bool TryGetNearestValidPosition(Position pos, out Position nearestPos, float maxDistance = 100f)
		{
			const float defaultRadius = 5f; // A small default radius suitable for NPCs.
			return this.TryGetNearestValidPosition(pos, defaultRadius, out nearestPos, maxDistance);
		}

		/// <summary>
		/// Attempts to find the nearest valid position that can accommodate an entity of a given radius.
		/// </summary>
		/// <param name="pos">Input position (X, Z used)</param>
		/// <param name="radius">The radius of the entity to check walkability for.</param>
		/// <param name="nearestPos">Nearest valid position with height.</param>
		/// <param name="maxDistance">Maximum search radius.</param>
		/// <returns>True if a valid position is found, false otherwise.</returns>
		public bool TryGetNearestValidPosition(Position pos, float radius, out Position nearestPos, float maxDistance = 100f)
		{
			nearestPos = pos;
			// Check original position first
			if (this.IsValidCirclePosition(pos, radius) && this.TryGetHeightAt(pos, out var height))
			{
				nearestPos = pos.WithHeight(height);
				return true;
			}

			// Spiral search for a nearby valid point
			const int pointsPerRing = 12;
			const float step = 10f;
			for (var r = step; r <= maxDistance; r += step)
			{
				for (var i = 0; i < pointsPerRing; i++)
				{
					var angle = 2 * Math.PI * i / pointsPerRing;
					var candidate = new Position(
						(float)(pos.X + r * Math.Cos(angle)), 0, (float)(pos.Z + r * Math.Sin(angle))
					);

					if (this.IsValidCirclePosition(candidate, radius) && this.TryGetHeightAt(candidate, out var candidateHeight))
					{
						nearestPos = candidate.WithHeight(candidateHeight);
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Returns if the Line of Sight is clear between two positions by performing a raycast against the ground mesh.
		/// </summary>
		/// <param name="pos1">The starting position.</param>
		/// <param name="pos2">The ending position.</param>
		/// <param name="eyeHeight">The vertical offset from the ground for the start and end points of the ray.</param>
		/// <returns>True if there is a clear line of sight, false otherwise.</returns>
		public bool LineOfSight(Position pos1, Position pos2, float eyeHeight = 10f)
		{
			if (_spatial == null)
			{
				// Fallback to 2D check if 3D spatial data isn't available.
				return !this.AnyObstacles(pos1, pos2);
			}

			if (!this.TryGetHeightAt(pos1, out var h1) || !this.TryGetHeightAt(pos2, out var h2))
			{
				// Cannot determine height at one of the positions, assume no line of sight.
				return false;
			}

			var startVec = new Vector3d(pos1.X, h1 + eyeHeight, pos1.Z);
			var endVec = new Vector3d(pos2.X, h2 + eyeHeight, pos2.Z);
			var direction = endVec - startVec;
			var totalDistance = direction.Length;

			if (totalDistance < 1.0f) return true; // Positions are too close to be obstructed.

			var ray = new Ray3d(startVec, direction.Normalized);

			var hitId = _spatial.FindNearestHitTriangle(ray);
			if (hitId == DMesh3.InvalidID)
			{
				// Ray didn't hit any part of the mesh, so sight is clear.
				return true;
			}

			var intersection = MeshQueries.TriangleIntersection(_mesh, hitId, ray);

			// If the first thing the ray hits is farther away than the target, the line of sight is clear.
			return intersection.RayParameter >= totalDistance;
		}

		/// <summary>
		/// A triangle of a walkable cell, used to sample random positions.
		/// </summary>
		/// <param name="V0"></param>
		/// <param name="V1"></param>
		/// <param name="V2"></param>
		private readonly record struct SampleTriangle(int V0, int V1, int V2);
	}

	/// <summary>
	/// Simple QuadTree implementation for spatial indexing
	/// </summary>
	public class QuadTree<T>
	{
		private readonly AxisAlignedBox2d _bounds;
		private readonly int _maxDepth;
		private readonly int _maxObjectsPerNode;
		private readonly List<(T item, AxisAlignedBox2d bounds)> _objects;
		private readonly QuadTree<T>[] _children;
		private readonly int _depth;

		public QuadTree(AxisAlignedBox2d bounds, int maxDepth = 5, int maxObjectsPerNode = 10, int depth = 0)
		{
			_bounds = bounds;
			_maxDepth = maxDepth;
			_maxObjectsPerNode = maxObjectsPerNode;
			_objects = new List<(T, AxisAlignedBox2d)>();
			_children = new QuadTree<T>[4];
			_depth = depth;
		}

		/// <summary>
		/// Inserts an item with its bounding box into the quad tree.
		/// </summary>
		public void Insert(T item, AxisAlignedBox2d itemBounds)
		{
			if (!_bounds.Intersects(itemBounds))
				return;

			if (_objects.Count < _maxObjectsPerNode || _depth >= _maxDepth)
			{
				_objects.Add((item, itemBounds));
				return;
			}

			if (_children[0] == null)
				this.Subdivide();

			foreach (var child in _children)
				child.Insert(item, itemBounds);
		}

		/// <summary>
		/// Returns all items whose bounding boxes intersect the given query bounds.
		/// </summary>
		public List<T> Query(AxisAlignedBox2d queryBounds)
		{
			var result = new List<T>();
			this.Query(queryBounds, result);
			return result;
		}

		/// <summary>
		/// Adds all items whose bounding boxes intersect the given query
		/// bounds to the given list, which is cleared first.
		/// </summary>
		public void Query(AxisAlignedBox2d queryBounds, List<T> result)
		{
			result.Clear();
			this.QueryInto(queryBounds, result);
		}

		private void QueryInto(AxisAlignedBox2d queryBounds, List<T> result)
		{
			if (!_bounds.Intersects(queryBounds))
				return;

			foreach (var (item, bounds) in _objects)
			{
				if (bounds.Intersects(queryBounds))
					result.Add(item);
			}

			if (_children[0] != null)
			{
				foreach (var child in _children)
					child.QueryInto(queryBounds, result);
			}
		}

		private void Subdivide()
		{
			var halfWidth = (_bounds.Max.x - _bounds.Min.x) / 2;
			var halfHeight = (_bounds.Max.y - _bounds.Min.y) / 2;
			var centerX = _bounds.Min.x + halfWidth;
			var centerY = _bounds.Min.y + halfHeight;

			_children[0] = new QuadTree<T>(new AxisAlignedBox2d(_bounds.Min.x, _bounds.Min.y, centerX, centerY), _maxDepth, _maxObjectsPerNode, _depth + 1);
			_children[1] = new QuadTree<T>(new AxisAlignedBox2d(centerX, _bounds.Min.y, _bounds.Max.x, centerY), _maxDepth, _maxObjectsPerNode, _depth + 1);
			_children[2] = new QuadTree<T>(new AxisAlignedBox2d(_bounds.Min.x, centerY, centerX, _bounds.Max.y), _maxDepth, _maxObjectsPerNode, _depth + 1);
			_children[3] = new QuadTree<T>(new AxisAlignedBox2d(centerX, centerY, _bounds.Max.x, _bounds.Max.y), _maxDepth, _maxObjectsPerNode, _depth + 1);
		}
	}
}
