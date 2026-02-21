using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Shared.Versioning;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Yggdrasil.Geometry;

namespace Melia.Zone
{
	public static class Debug
	{
		/// <summary>
		/// Gets or sets whether performance profiling logs are enabled.
		/// Can be controlled via server configuration or console commands.
		/// </summary>
		public static bool IsProfilingEnabled { get; set; } = true;

		/// <summary>
		/// Starts a profiling block that will be timed and logged upon disposal.
		/// Returns a disposable object, designed to be used in a 'using' statement.
		/// If profiling is disabled, returns a dummy object that does nothing.
		/// </summary>
		/// <param name="actionName">The name of the action to profile.</param>
		/// <param name="warningThresholdMs">The threshold in milliseconds to log a WARNING for.</param>
		/// <returns>An IDisposable to wrap the code block.</returns>
		public static IDisposable Profile(string actionName, long warningThresholdMs = 1000)
		{
			if (IsProfilingEnabled)
			{
				return new ActionProfiler(actionName, warningThresholdMs);
			}
			else
			{
				return NullProfiler.Instance;
			}
		}

		/// <summary>
		/// Temporarily visualizes the shape on the map using friendly
		/// monsters and range previews.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="shape"></param>
		/// <param name="duration"></param>
		/// <param name="edgePoints"></param>
		/// <param name="rangePreview"></param>
		public static void ShowShape(Map map, IShapeF shape, TimeSpan? duration = null, bool edgePoints = false, bool rangePreview = true)
		{
			if (Versions.Client == KnownVersions.ClosedBeta1)
			{
				edgePoints = true;
				rangePreview = false;
			}
			if (duration == null)
				duration = TimeSpan.FromSeconds(5);

			if (edgePoints)
			{
				var monsterId = 41094;
				if (Versions.Client == KnownVersions.ClosedBeta1)
					monsterId = 10005;
				foreach (var point in shape.GetEdgePoints())
				{
					var pos = new Position(point.X, 0, point.Y);
					var height = map.Ground.GetHeightAt(pos);
					pos.Y = height;

					var monster = new Mob(monsterId, RelationType.Neutral);
					monster.Position = pos;
					monster.DisappearTime = DateTime.Now.Add((TimeSpan)duration);
					map.AddMonster(monster);
				}
			}

			if (rangePreview)
			{
				if (shape is ISplashArea splashArea)
				{
					Send.ZC_START_RANGE_PREVIEW(map, 0, null, (TimeSpan)duration, splashArea);
				}
				else
				{
					var center = shape.Center;
					var centerPos = new Position(center.X, 0, center.Y);
					centerPos.Y = map.Ground.GetHeightAt(centerPos);

					var edgePointsArray = shape.GetEdgePoints();
					if (edgePointsArray.Length > 0)
					{
						var maxDistance = 0f;
						foreach (var point in edgePointsArray)
						{
							var distance = (float)center.GetDistance(point);
							if (distance > maxDistance)
								maxDistance = distance;
						}

						var splashType = SplashType.Circle;
						var height = maxDistance;
						var width = 0f;

						Send.ZC_START_RANGE_PREVIEW(map, 0, null, (TimeSpan)duration, splashType, centerPos, Direction.Zero, height, width);
					}
				}
			}
		}

		/// <summary>
		/// Temporarily places a monster at the given position.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="pos"></param>
		/// <param name="duration"></param>
		public static void ShowPosition(Map map, Position pos, TimeSpan? duration = null)
		{
			if (duration == null)
				duration = TimeSpan.FromSeconds(5);

			var monster = new Mob(10005, RelationType.Friendly);
			monster.Position = pos;
			monster.DisappearTime = DateTime.Now.Add((TimeSpan)duration);
			map.AddMonster(monster);
		}

		/// <summary>
		/// Temporarily places monsters at list of positions.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="positionList"></param>
		/// <param name="duration"></param>
		public static void ShowPositions(Map map, List<Position> positionList, TimeSpan? duration = null)
		{
			foreach (var position in positionList)
			{
				Debug.ShowPosition(map, position, duration);
			}
		}

		/// <summary>
		/// Announces the skill being used by a mob via chat message.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		public static void MobSkillAnnounce(ICombatEntity caster, Skill skill)
		{
			Send.ZC_CHAT(caster, "Watch out! I'm using {0}!", skill.Data.ClassName);
		}

		/// <summary>
		/// Creates an image of the map's ground and saves it to the given
		/// file path to help visualize potential problems.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="map"></param>
		public static void DrawMap(string filePath, Map map, float scale = 1.0f, float rotateAngle = 0)
		{
			var drawBorderBoxes = false;
			var drawCellsColored = false;
			var drawTriangles = false;
			var drawSpawners = false;

			var borderBoxColorOffset = 0x30;
			var imageMarginSize = 100;

			var mapClassName = map.Data.ClassName;
			var groundData = ZoneServer.Instance.Data.GroundDb.Find(mapClassName);

			var vertices = groundData.Vertices;
			var triangles = groundData.Triangles;
			var polygons = groundData.Cells;

			var left = (int)vertices.Min(a => a.X);
			var right = (int)vertices.Max(a => a.X);
			var bottom = (int)vertices.Min(a => a.Y);
			var top = (int)vertices.Max(a => a.Y);

			var width = right - left + imageMarginSize * 2;
			var height = top - bottom + imageMarginSize * 2;
			var offsetX = -left + imageMarginSize;
			var offsetY = -bottom + imageMarginSize;

			using var image = new Image<Rgba32>(width, height);
			image.Mutate(ctx => ctx.Clear(Color.Transparent));

			foreach (var polygon in polygons)
			{
				var r = new Random().Next(0x77, 0xDD);
				var g = new Random().Next(0x77, 0xDD);
				var b = new Random().Next(0x77, 0xDD);
				var color = new Rgba32((byte)r, (byte)g, (byte)b);

				if (!drawCellsColored)
					color = new Rgba32(207, 181, 118);

				var points = polygon.Vertices.Select(a => new PointF(a.X + offsetX, height - (offsetY + a.Y))).ToArray();
				image.Mutate(ctx => ctx.FillPolygon(color, points));

				if (drawBorderBoxes)
				{
					var minX = points.Min(a => a.X);
					var maxX = points.Max(a => a.X);
					var minY = points.Min(a => a.Y);
					var maxY = points.Max(a => a.Y);
					var borderColor = new Rgba32((byte)(r - borderBoxColorOffset), (byte)(g - borderBoxColorOffset), (byte)(b - borderBoxColorOffset));

					image.Mutate(ctx => ctx.Draw(new SolidPen(borderColor), new RectangleF(minX, minY, maxX - minX, maxY - minY)));
				}
			}

			if (drawTriangles)
			{
				var pen = new SolidPen(Color.Black);

				foreach (var triangle in triangles)
				{
					var points = triangle.Vertices.Select(a => new PointF(a.X + offsetX, height - (offsetY + a.Y))).ToArray();
					image.Mutate(ctx => ctx.DrawPolygon(pen, points));
				}
			}

			if (drawSpawners)
			{
				var brush = new SolidBrush(Color.FromRgba(255, 0, 0, 128));
				var pen = new SolidPen(Color.FromRgba(0, 255, 255, 0));

				var spawnAreasList = ZoneServer.Instance.World.GetSpawnAreas();
				var collectionsInMap = spawnAreasList.Where(a => a.GetAllOnMap(map).Length != 0).ToList();

				foreach (var collection in collectionsInMap)
				{
					var spawnAreas = collection.GetAllOnMap(map);
					foreach (var spawnArea in spawnAreas)
					{
						var points = spawnArea.Area.GetEdgePoints().Select(a => new PointF(a.X + offsetX, height - (offsetY + a.Y))).ToArray();
						image.Mutate(ctx => ctx.FillPolygon(brush, points));
						image.Mutate(ctx => ctx.DrawPolygon(pen, points));
					}
				}
			}

			// Apply rotation if specified
			if (rotateAngle != 0)
			{
				// Calculate new dimensions after rotation
				var radians = rotateAngle * Math.PI / 180;
				var cos = Math.Abs(Math.Cos(radians));
				var sin = Math.Abs(Math.Sin(radians));
				var newWidth = (int)(width * cos + height * sin);
				var newHeight = (int)(width * sin + height * cos);

				// Apply rotation directly to the image
				image.Mutate(ctx => ctx
					.Pad(newWidth, newHeight, Color.Transparent)
					.Rotate(rotateAngle));
			}

			// Apply scaling if needed
			if (scale != 1.0f)
			{
				var newWidth = (int)(image.Width * scale);
				var newHeight = (int)(image.Height * scale);
				image.Mutate(ctx => ctx.Resize(newWidth, newHeight));
			}

			filePath = Path.GetFullPath(filePath);

			var folderPath = System.IO.Path.GetDirectoryName(filePath);
			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			image.Save(filePath, new PngEncoder());
		}
	}
}
