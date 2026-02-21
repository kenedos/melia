using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using Yggdrasil.Extensions;
using Yggdrasil.Logging;

namespace Melia.Zone.Scripting
{
	public static partial class Shortcuts
	{
		/// <summary>
		/// Checks the distance and sends a system message
		/// </summary>
		/// <remarks>
		/// Used in certain items (Guild Tower, Summon Monster Albums)
		/// </remarks>
		/// <param name="character"></param>
		/// <param name="position"></param>
		/// <param name="distance"></param>
		/// <returns></returns>
		public static bool CheckIfNearNPC(this Character character, Position position, float distance)
		{
			if (character.Map.GetActorsIn<Npc>(new Circle(position, distance)).Count > 0)
				return true;
			character.SystemMessage("TooNearFromNPC");
			return false;
		}

		public static Location Location(string mapName, double x, double z)
		{
			if (!ZoneServer.Instance.Data.MapDb.TryFind(mapName, out var map))
			{
				Log.Debug("Location: unable to find map {0}", mapName);
			}
			return new Location(map?.Id ?? Map.Limbo.Id, new Position((float)x, 0, (float)z));
		}

		public static Location Location(string mapName, double x, double y, double z)
		{
			if (!ZoneServer.Instance.Data.MapDb.TryFind(mapName, out var map))
			{
				Log.Debug("Location: unable to find map {0}", mapName);
			}
			return new Location(map?.Id ?? Map.Limbo.Id, new Position((float)x, (float)y, (float)z));
		}

		/// <summary>
		/// Parse Items
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
		public static Dictionary<string, int> ParseItems(string items)
		{
			var itemDict = new Dictionary<string, int>();
			foreach (var itemString in items.Split(';'))
			{
				if (itemString.Length == 0)
				{
					continue;
				}
				var itemSplit = itemString.Split('/');
				var itemClassName = itemSplit[0];
				var itemAmount = 1;
				if (string.IsNullOrWhiteSpace(itemClassName))
					continue;
				if (itemSplit.Length > 1)
					int.TryParse(itemSplit[1], out itemAmount);
				itemDict.Add(itemClassName, itemAmount);
			}

			return itemDict;
		}

		public static string GetItemName(int itemId)
		{
			return ZoneServer.Instance.Data.ItemDb.Entries.TryGetValue(itemId)?.Name ?? "";
		}

		public static string GetMapName(int mapId)
		{
			return ZoneServer.Instance.Data.MapDb.Entries.TryGetValue(mapId)?.Name ?? "";
		}

		public static string GetMapName(string mapClassName)
		{
			return ZoneServer.Instance.Data.MapDb.Entries.Values.FirstOrDefault(map => map.ClassName == mapClassName)?.Name ?? "";
		}

		public static string GetMapClassName(int mapId)
		{
			return ZoneServer.Instance.Data.MapDb.Entries.TryGetValue(mapId)?.ClassName ?? "";
		}

		public static string GetMonsterName(int monsterId)
		{
			return ZoneServer.Instance.Data.MonsterDb.Entries.TryGetValue(monsterId)?.Name ?? "";
		}
	}
}
