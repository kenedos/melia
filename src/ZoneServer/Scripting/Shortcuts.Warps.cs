using System;
using System.Threading;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Logging;

namespace Melia.Zone.Scripting
{
	public static partial class Shortcuts
	{
		/// <summary>
		/// Creates a warp.
		/// </summary>
		/// <param name="warpName"></param>
		/// <param name="direction"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static WarpMonster AddWarp(string warpName, double direction, Location from, Location to)
		{
			return AddWarp(0, warpName, direction, from, to);
		}

		/// <summary>
		/// Creates a warp.
		/// </summary>
		/// <param name="genType">Used to show icon on the map.</param>
		/// <param name="warpName"></param>
		/// <param name="direction"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static WarpMonster AddWarp(int genType, string warpName, double direction, Location from, Location to)
		{
			if (!ZoneServer.Instance.World.TryGetMap(from.MapId, out var fromMap))
				throw new ArgumentException($"Map '{from.MapId}' not found.");

			if (!ZoneServer.Instance.World.TryGetMap(to.MapId, out var toMap))
				throw new ArgumentException($"Map '{to.MapId}' not found.");

			var targetLocationName = Localization.Get(toMap.Data.Name);

			var monster = new WarpMonster(genType, warpName, targetLocationName, from, to, new Direction(direction));
			fromMap.AddMonster(monster);

			ZoneServer.Instance.World.Maps.RecordMapConnection(fromMap, toMap);

			return monster;
		}

		/// <summary>
		/// Creates a warp portal.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static WarpMonster AddWarpPortal(Location from, Location to)
		{
			if (!ZoneServer.Instance.World.TryGetMap(from.MapId, out var fromMap))
				throw new ArgumentException($"Map '{from.MapId}' not found.");

			if (!ZoneServer.Instance.World.TryGetMap(to.MapId, out var toMap))
				throw new ArgumentException($"Map '{to.MapId}' not found.");

			var targetLocationName = Localization.Get(toMap.Data.Name);
			var fromLocationName = Localization.Get(fromMap.Data.Name);

			//Log.Debug($"Creating a warp portal from {fromLocationName} to {targetLocationName}");
			return CreateWarpPortal(from, to, name: targetLocationName, direction: 0, durationSeconds: 0);
		}

		/// <summary>
		/// Creates a warp portal at given location
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="name"></param>
		/// <param name="direction"></param>
		/// <param name="durationSeconds"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static WarpMonster CreateWarpPortal(Location from, Location to, string name = "", double direction = 0, int durationSeconds = 0)
		{
			if (!ZoneServer.Instance.World.TryGetMap(from.MapId, out var fromMap))
				throw new ArgumentException($"Map '{from.MapId}' not found.");

			if (!ZoneServer.Instance.World.TryGetMap(to.MapId, out var toMap))
				throw new ArgumentException($"Map '{to.MapId}' not found.");

			var warp = new WarpMonster(MonsterId.MissionGate,
				from,
				to,
				new Direction(direction));
			warp.Name = name;
			warp.Properties[PropertyName.Scale] = 1;

			if (durationSeconds > 0)
				warp.DisappearTime = DateTime.Now.AddSeconds(durationSeconds);

			// This is what makes the invisible npc look like a portal.
			warp.AddEffect(new AttachEffect(AnimationName.Portal, 1, EffectLocation.Top));
			fromMap.AddMonster(warp);

			return warp;
		}

		/// <summary>
		/// Returns a full location object from the given map class name
		/// and coordinates.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		public static Location From(string map, double x, double z)
			=> GetLocation(map, x, z);

		/// <summary>
		/// Returns a full location object from the given map class name
		/// and coordinates.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		public static Location From(string map, double x, double y, double z)
			=> GetLocation(map, x, y, z);

		/// <summary>
		/// Returns a full location object from the given map class name
		/// and coordinates.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		public static Location To(string map, double x, double z)
			=> GetLocation(map, x, z);

		/// <summary>
		/// Returns a full location object from the given map class name
		/// and coordinates.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		public static Location To(string map, double x, double y, double z)
			=> GetLocation(map, x, y, z);

		/// <summary>
		/// Returns a full location object from the given map class name
		/// and coordinates.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		private static Location GetLocation(string map, double x, double z)
		{
			var mapObj = GetMapOrThrow(map);

			var pos = new Position((int)x, 0, (int)z);
			if (mapObj.Ground.TryGetHeightAt(pos, out var height))
				pos.Y = height;
			else
			{
				Log.Warning($"Failed TryGetHeightAt in {mapObj.Id} at {pos}");
				mapObj.Ground.TryGetNearestValidPosition(pos, out pos);
			}

			return new Location(mapObj.Id, pos);
		}

		/// <summary>
		/// Returns a full location object from the given map class name
		/// and coordinates.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		private static Location GetLocation(string map, double x, double y, double z)
		{
			var mapObj = GetMapOrThrow(map);

			var pos = new Position((float)x, (float)y, (float)z);

			return new Location(mapObj.Id, pos);
		}
	}
}
