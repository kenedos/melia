using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Melia.Shared.Configuration.Files;
using Melia.Shared.L10N;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Shared.World;
using Melia.Zone.Commands;
using Melia.Zone.Network;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Maps;
using Yggdrasil.Geometry;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone.Scripting
{
	public static partial class Shortcuts
	{
		/// <summary>
		/// Returns a reference to the global variables container.
		/// </summary>
		public static VariablesContainer GlobalVariables => ZoneServer.Instance.World.GlobalVariables.Variables;

		/// <summary>
		/// Script Argument Message
		/// Wraps key value pairs with Game specific message codes.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static string ScpArgMsg(string msg, params object[] args)
		{
			var result = new StringBuilder();
			var prefix = "!@#$";
			var suffix = "#@!";

			result.Append(prefix);
			result.Append(msg);
			if (args != null && args.Length % 2 != 1)
			{
				for (var i = 0; i < args.Length; i += 2)
				{
					var key = args[i];
					var value = args[i + 1];

					result.Append("$*$" + key);
					result.Append("$*$" + value);
				}
			}
			result.Append(suffix);

			return result.ToString();
		}

		/// <summary>
		/// Returns a time span of the given amount of hours.
		/// </summary>
		/// <param name="hours"></param>
		/// <returns></returns>
		public static TimeSpan Hours(double hours)
			=> TimeSpan.FromHours(hours);

		/// <summary>
		/// Returns a time span of the given amount of minutes.
		/// </summary>
		/// <param name="minutes"></param>
		/// <returns></returns>
		public static TimeSpan Minutes(double minutes)
			=> TimeSpan.FromMinutes(minutes);

		/// <summary>
		/// Returns a time span of the given amount of seconds.
		/// </summary>
		/// <param name="seconds"></param>
		/// <returns></returns>
		public static TimeSpan Seconds(double seconds)
			=> TimeSpan.FromSeconds(seconds);

		/// <summary>
		/// Returns a time span of the given amount of milliseconds.
		/// </summary>
		/// <param name="milliseconds"></param>
		/// <returns></returns>
		public static TimeSpan Milliseconds(double milliseconds)
			=> TimeSpan.FromMilliseconds(milliseconds);

		/// <summary>
		/// Returns true if the given event is active.
		/// </summary>
		/// <param name="gameEventId"></param>
		/// <returns></returns>
		public static bool IsEventActive(string gameEventId)
		{
			return ZoneServer.Instance.GameEvents.IsActive(gameEventId);
		}

		/// <summary>
		/// Simplified monster spawning for mini games.
		/// </summary>
		/// <param name="monsterId"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="tendency"></param>
		/// <returns></returns>
		public static Mob CreateTrackMonster(int key, int monsterId, string map, double x, double y, double z, double direction = 0, int tendency = 0)
		{
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(monsterId, out var monsterData))
			{
				Log.Warning("AddMonster: Failed monster not found with id: {0}", monsterId);
				throw new ArgumentException($"AddMonster: Monster '{monsterId}'  not found.");
			}

			var mapObj = GetMapOrThrow(map);

			var type = RelationType.Enemy;

			switch (monsterId)
			{
				case 20044:
				case 20045:
				case 20046:
				case 20047:
				case 20048:
				case 20049:
				case 20053:
				case 20054:
				case 147455:
					type = RelationType.Neutral;
					break;
			}

			var monster = new Mob(monsterData.Id, type);
			monster.Name = "";
			monster.GenType = 0;
			monster.Position = new Position((float)x, (float)y, (float)z);
			monster.Direction = new Direction(direction);
			if (tendency != 0)
				monster.Tendency = TendencyType.Aggressive;
			monster.Vars.SetInt("key", key);

			return monster;
		}

		/// <summary>
		/// Spawning a monster for a track.
		/// </summary>
		/// <param name="monsterId"></param>
		/// <param name="mapName"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="faction"></param>
		/// <param name="tendency"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// Shortcuts.AddMonster(0, 400001, "", "f_siauliai_west", -1231.022, 260.8354, -547.764, 16.875, "");
		public static Mob AddMonster(Character character, int monsterId, string name, string mapName, double x, double y, double z, double direction, string faction = "Monster", string tendency = "")
		{
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(monsterId, out var monsterData))
			{
				Log.Warning("AddMonster: Failed monster not found with id: {0}", monsterId);
				throw new ArgumentException($"AddMonster: Monster '{monsterId}'  not found.");
			}

			Map map;
			if (mapName != "None")
				map = GetMapOrThrow(mapName);
			else
				map = character.Map;

			var monster = new Mob(monsterData.Id, faction == "Our_Forces" ? RelationType.Friendly : RelationType.Enemy);
			monster.Name = name;
			monster.Position = new Position((float)x, (float)y, (float)z);
			monster.Direction = new Direction(direction);
			monster.Layer = character.Layer;
			monster.SpawnPosition = monster.Position;
			if (!string.IsNullOrEmpty(faction) && Enum.TryParse(typeof(FactionType), faction, true, out var factionType))
				monster.Faction = (FactionType)factionType;

			monster.SetVisibilty(ActorVisibility.Track, character.ObjectId);
			monster.AddEffect(new ScriptInvisibleEffect());
			var ai = new AiComponent(monster, "BasicMonster");
			monster.Components.Add(ai);

			map.AddMonster(monster);

			return monster;
		}

		/// <summary>
		/// Spawning a monster for a track.
		/// </summary>
		/// <param name="genType"></param>
		/// <param name="monsterId"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="faction"></param>
		/// <param name="tendency"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// Shortcuts.AddMonster(0, 400001, "", "f_siauliai_west", -1231.022, 260.8354, -547.764, 16.875, "");
		public static Mob AddMonster(int genType, int monsterId, string name, string map, double x, double y, double z, double direction, string faction = "Monster", string tendency = "")
		{
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(monsterId, out var monsterData))
			{
				Log.Warning("AddMonster: Failed monster not found with id: {0}", monsterId);
				throw new ArgumentException($"AddMonster: Monster '{monsterId}'  not found.");
			}

			var mapObj = GetMapOrThrow(map);

			var monster = new Mob(monsterData.Id, faction == "Our_Forces" ? RelationType.Friendly : RelationType.Enemy);
			monster.Name = name;
			monster.GenType = genType;
			monster.Position = new Position((float)x, (float)y, (float)z);
			monster.Direction = new Direction(direction);
			if (!string.IsNullOrEmpty(faction) && Enum.TryParse(typeof(FactionType), faction, true, out var factionType))
				monster.Faction = (FactionType)factionType;

			mapObj.AddMonster(monster);

			return monster;
		}

		/// <summary>
		/// Spawns multiple aggressive quest monsters around a character.
		/// Monsters will target the character and have a limited lifetime.
		/// </summary>
		/// <param name="character">The character to spawn monsters around</param>
		/// <param name="monsterId">The monster ID to spawn</param>
		/// <param name="count">Number of monsters to spawn</param>
		/// <param name="maxDistance">Maximum distance from character to spawn (0 = at character position)</param>
		/// <param name="lifetime">How long monsters persist (default: 1 minute)</param>
		/// <param name="fromGround">Whether to spawn from ground animation (default: true)</param>
		/// <returns>True if monsters were spawned successfully</returns>
		public static bool SpawnTempMonsters(Character character, int monsterId, int count, int maxDistance = 70, TimeSpan? lifetime = null, bool fromGround = true)
		{
			if (character?.Map == null)
				return false;

			var monsterLifetime = lifetime ?? TimeSpan.FromMinutes(1);
			var spawned = 0;

			for (var i = 0; i < count; i++)
			{
				var spawnMob = new Mob(monsterId, RelationType.Enemy);

				// Get position to spawn
				Position spawnPos;
				if (maxDistance > 10)
				{
					var randomOffset = character.Position.GetRandomInRange2D(maxDistance - 10, maxDistance);

					if (!character.Map.Ground.TryGetNearestValidPosition(randomOffset, out spawnPos, maxDistance))
						spawnPos = character.Position; // Fall back to character position
				}
				else
				{
					spawnPos = character.Position;
				}

				spawnMob.Position = spawnPos;
				spawnMob.SpawnPosition = spawnMob.Position;
				spawnMob.Components.Add(new MovementComponent(spawnMob));
				spawnMob.Components.Add(new LifeTimeComponent(spawnMob, monsterLifetime));

				// Add AI component
				if (!string.IsNullOrEmpty(spawnMob.Data.AiName) && AI.AiScript.Exists(spawnMob.Data.AiName))
					spawnMob.Components.Add(new AiComponent(spawnMob, spawnMob.Data.AiName));
				else
					spawnMob.Components.Add(new AiComponent(spawnMob, "BasicMonster"));

				// Make monster aggressive and target character
				spawnMob.InsertHate(character);
				spawnMob.Tendency = TendencyType.Aggressive;
				spawnMob.FromGround = fromGround;

				// Apply property overrides if they exist
				if (character.Map.TryGetPropertyOverrides(monsterId, out var propertyOverrides))
					spawnMob.ApplyOverrides(propertyOverrides);

				character.Map.AddMonster(spawnMob);
				spawned++;
			}

			return spawned > 0;
		}

		/// <summary>
		/// Returns a random number between 0 and max - 1.
		/// </summary>
		/// <param name="max"></param>
		/// <returns></returns>
		public static int Random(int max)
		{
			return RandomProvider.Next(max);
		}

		/// <summary>
		/// Returns a random number between min and max - 1.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static int Random(int min, int max)
		{
			return RandomProvider.Next(min, max);
		}

		/// <summary>
		/// Plays chest opening animations and makes the chest disappear.
		/// Returns after the animation played and the chest's contents
		/// can be distributed.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="npc"></param>
		/// <returns></returns>
		public static async Task OpenChest(Character character, Npc npc, bool disappearOnOpen = false)
		{
			character.ShowHelp("MINI_E_BUFFBOX");

			var anim = (npc.Id == 147392) ? AnimationName.Opened : AnimationName.Open;

			// Play animations for character to kick open the chest
			Send.ZC_PLAY_ANI(character, AnimationName.KickBox);
			Send.ZC_PLAY_ANI(npc, anim, true);

			// Wait a second, so the animations can play
			await Task.Delay(TimeSpan.FromSeconds(3));

			// Make chest disappear
			Send.ZC_NORMAL.FadeOut(npc, TimeSpan.FromSeconds(3));
			if (disappearOnOpen)
				npc.DisappearTime = DateTime.Now.AddSeconds(3);

			character.SetMapNPCState(npc, NpcState.Invisible);

			// Make chest reappear after a certain amount of time
			// TODO: Add timer component, to set up and associate timers
			//   and intervals with entities.
			_ = Task.Delay(TimeSpan.FromMinutes(1)).ContinueWith(_ => npc.SetState(NpcState.Normal));
		}

		/// <summary>
		/// Adds or overrides a command, making it available to players
		/// who have the given authority levels.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="usage"></param>
		/// <param name="description"></param>
		/// <param name="auth"></param>
		/// <param name="targetAuth"></param>
		/// <param name="func"></param>
		public static void AddChatCommand(string command, string usage, string description, int auth, int targetAuth, ChatCommandFunc func)
		{
			ZoneServer.Instance.ChatCommands.Add(command, usage, description, func);
			ZoneServer.Instance.Conf.Commands.CommandLevels[command] = new CommandAuthLevels(auth, targetAuth);
		}

		/// <summary>
		/// Returns a random element from the array.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="values"></param>
		/// <returns></returns>
		public static T RandomElement<T>(params T[] values)
		{
			return values[RandomProvider.Next(values.Length)];
		}

		/// <summary>
		/// Lures nearby enemies of specific monster types to aggro onto the character.
		/// Adds threat/hate to each matching enemy within the specified radius.
		/// </summary>
		/// <param name="character">The character to lure enemies towards</param>
		/// <param name="monsterIds">Array of monster IDs to lure</param>
		/// <param name="radius">The radius to search for enemies</param>
		/// <param name="threatAmount">Amount of threat/hate to add (default: 150)</param>
		/// <returns>Number of enemies lured</returns>
		public static int LureNearbyEnemies(Character character, int[] monsterIds, float radius, int threatAmount = 150)
		{
			if (character?.Map == null || monsterIds == null || monsterIds.Length == 0)
				return 0;

			var monsterIdSet = new HashSet<int>(monsterIds);
			var luredCount = 0;

			var nearbyEnemies = character.Map.GetAttackableEnemiesInPosition(character, character.Position, radius);

			foreach (var enemy in nearbyEnemies)
			{
				if (enemy is Mob mob && monsterIdSet.Contains(mob.Id) && !mob.IsDead)
				{
					mob.InsertHate(character, threatAmount);
					luredCount++;
				}
			}

			return luredCount;
		}

		/// <summary>
		/// Attempts to get a map by class name. If the map doesn't exist
		/// in the current version's map database, throws MapNotLoadedException
		/// to signal a version mismatch. If it exists in the database but
		/// not in the world, throws ArgumentException for a real error.
		/// </summary>
		internal static Map GetMapOrThrow(string mapClassName)
		{
			if (!ZoneServer.Instance.World.TryGetMap(mapClassName, out var map))
			{
				if (!ZoneServer.Instance.Data.MapDb.TryFind(mapClassName, out _))
					throw new MapNotLoadedException(mapClassName);

				throw new ArgumentException($"Map '{mapClassName}' not found.");
			}

			return map;
		}
	}
}
