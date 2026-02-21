using System;
using System.Collections.Generic;
using Melia.Shared.World;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry;
using Yggdrasil.Geometry.Shapes;

namespace Melia.Zone.World.Maps.Spatial
{
	/// <summary>
	/// A spatial index for combat entities using a simple grid.
	/// Provides O(1) cell lookup for range queries.
	/// </summary>
	public class EntitySpatialIndex : IDisposable
	{
		private const float CellSize = 200f;

		private readonly Dictionary<long, List<ICombatEntity>> _cells = new();
		private readonly Dictionary<int, long> _entityCells = new();
		private readonly object _lock = new();

		private readonly float _minX, _minZ;
		private readonly int _gridWidth, _gridHeight;

		/// <summary>
		/// Creates a new spatial index with the specified bounds.
		/// </summary>
		public EntitySpatialIndex(float minX, float minZ, float maxX, float maxZ)
		{
			_minX = minX - 500;
			_minZ = minZ - 500;
			var width = (maxX + 500) - _minX;
			var height = (maxZ + 500) - _minZ;
			_gridWidth = (int)Math.Ceiling(width / CellSize);
			_gridHeight = (int)Math.Ceiling(height / CellSize);
		}

		private long GetCellKey(float x, float z)
		{
			var cellX = (int)((x - _minX) / CellSize);
			var cellY = (int)((z - _minZ) / CellSize);
			cellX = Math.Clamp(cellX, 0, _gridWidth - 1);
			cellY = Math.Clamp(cellY, 0, _gridHeight - 1);
			return ((long)cellX << 32) | (uint)cellY;
		}

		/// <summary>
		/// Inserts an entity into the spatial index.
		/// </summary>
		public void Insert(ICombatEntity entity)
		{
			if (entity == null)
				return;

			var key = this.GetCellKey(entity.Position.X, entity.Position.Z);

			lock (_lock)
			{
				if (!_cells.TryGetValue(key, out var cell))
				{
					cell = new List<ICombatEntity>();
					_cells[key] = cell;
				}
				cell.Add(entity);
				_entityCells[entity.Handle] = key;
			}
		}

		/// <summary>
		/// Removes an entity from the spatial index.
		/// </summary>
		public void Remove(ICombatEntity entity)
		{
			if (entity == null)
				return;

			lock (_lock)
			{
				if (_entityCells.TryGetValue(entity.Handle, out var key))
				{
					if (_cells.TryGetValue(key, out var cell))
					{
						cell.Remove(entity);

						if (cell.Count == 0)
							_cells.Remove(key);
					}
					_entityCells.Remove(entity.Handle);
				}
			}
		}

		/// <summary>
		/// Removes an entity from the spatial index using its handle and position.
		/// </summary>
		public void Remove(int handle, Position position)
		{
			lock (_lock)
			{
				if (_entityCells.TryGetValue(handle, out var key))
				{
					if (_cells.TryGetValue(key, out var cell))
					{
						cell.RemoveAll(e => e.Handle == handle);

						if (cell.Count == 0)
							_cells.Remove(key);
					}
					_entityCells.Remove(handle);
				}
			}
		}

		/// <summary>
		/// Updates an entity's position in the spatial index.
		/// </summary>
		public void Update(ICombatEntity entity, Position oldPos, Position newPos)
		{
			if (entity == null)
				return;

			var newKey = this.GetCellKey(newPos.X, newPos.Z);

			lock (_lock)
			{
				if (!_entityCells.TryGetValue(entity.Handle, out var currentKey))
					return;

				if (currentKey == newKey)
					return;

				if (_cells.TryGetValue(currentKey, out var oldCell))
				{
					oldCell.Remove(entity);

					if (oldCell.Count == 0)
						_cells.Remove(currentKey);
				}

				if (!_cells.TryGetValue(newKey, out var newCell))
				{
					newCell = new List<ICombatEntity>();
					_cells[newKey] = newCell;
				}
				newCell.Add(entity);
				_entityCells[entity.Handle] = newKey;
			}
		}

		/// <summary>
		/// Queries all entities within a circular area.
		/// </summary>
		public List<ICombatEntity> QueryCircle(Position center, float radius)
		{
			var results = new List<ICombatEntity>();
			var radiusSq = radius * radius;

			var minCellX = (int)((center.X - radius - _minX) / CellSize);
			var maxCellX = (int)((center.X + radius - _minX) / CellSize);
			var minCellY = (int)((center.Z - radius - _minZ) / CellSize);
			var maxCellY = (int)((center.Z + radius - _minZ) / CellSize);

			minCellX = Math.Max(0, minCellX);
			maxCellX = Math.Min(_gridWidth - 1, maxCellX);
			minCellY = Math.Max(0, minCellY);
			maxCellY = Math.Min(_gridHeight - 1, maxCellY);

			lock (_lock)
			{
				for (var cx = minCellX; cx <= maxCellX; cx++)
				{
					for (var cy = minCellY; cy <= maxCellY; cy++)
					{
						var key = ((long)cx << 32) | (uint)cy;
						if (_cells.TryGetValue(key, out var cell))
						{
							foreach (var entity in cell)
							{
								var dx = entity.Position.X - center.X;
								var dz = entity.Position.Z - center.Z;
								if (dx * dx + dz * dz <= radiusSq)
									results.Add(entity);
							}
						}
					}
				}
			}

			return results;
		}

		/// <summary>
		/// Queries all entities within an arbitrary shape.
		/// </summary>
		public List<ICombatEntity> QueryShape(IShapeF shape)
		{
			// Fast path for circles - avoid expensive GetEdgePoints
			if (shape is CircleF circle)
				return this.QueryCircle(new Position(circle.Center.X, 0, circle.Center.Y), circle.Radius + MaxAgentRadius);

			var results = new List<ICombatEntity>();
			var center = shape.Center;

			// Estimate radius from shape bounds, expanded by max agent
			// radius so that large entities near the edge are included.
			var edgePoints = shape.GetEdgePoints();
			var maxDistSq = 0f;
			foreach (var point in edgePoints)
			{
				var dx = point.X - center.X;
				var dy = point.Y - center.Y;
				var distSq = dx * dx + dy * dy;
				if (distSq > maxDistSq)
					maxDistSq = distSq;
			}
			var radius = (float)Math.Sqrt(maxDistSq) + MaxAgentRadius;

			var minCellX = (int)((center.X - radius - _minX) / CellSize);
			var maxCellX = (int)((center.X + radius - _minX) / CellSize);
			var minCellY = (int)((center.Y - radius - _minZ) / CellSize);
			var maxCellY = (int)((center.Y + radius - _minZ) / CellSize);

			minCellX = Math.Max(0, minCellX);
			maxCellX = Math.Min(_gridWidth - 1, maxCellX);
			minCellY = Math.Max(0, minCellY);
			maxCellY = Math.Min(_gridHeight - 1, maxCellY);

			lock (_lock)
			{
				for (var cx = minCellX; cx <= maxCellX; cx++)
				{
					for (var cy = minCellY; cy <= maxCellY; cy++)
					{
						var key = ((long)cx << 32) | (uint)cy;
						if (_cells.TryGetValue(key, out var cell))
						{
							foreach (var entity in cell)
							{
								if (shape.IsInsideOrInRange(entity.Position, entity.AgentRadius))
									results.Add(entity);
							}
						}
					}
				}
			}

			return results;
		}

		/// <summary>
		/// Maximum agent radius (XL/XXL entities) used to expand spatial
		/// query bounds so that large entities near shape edges are not
		/// missed.
		/// </summary>
		private const float MaxAgentRadius = 40f;

		/// <summary>
		/// Queries all entities in the spatial index.
		/// </summary>
		public List<ICombatEntity> QueryAll()
		{
			var results = new List<ICombatEntity>();

			lock (_lock)
			{
				foreach (var cell in _cells.Values)
					results.AddRange(cell);
			}

			return results;
		}

		/// <summary>
		/// Disposes the spatial index.
		/// </summary>
		public void Dispose()
		{
		}
	}
}
