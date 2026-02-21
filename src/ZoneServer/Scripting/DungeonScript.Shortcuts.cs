using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Dungeons;
using Melia.Zone.World.Dungeons.Stages;

namespace Melia.Zone.Scripting
{
	public abstract partial class DungeonScript
	{
		// Static definitions of difficulty profiles for easy access and modification.
		protected static readonly Dictionary<int, DifficultyProfile> DifficultyProfiles = new Dictionary<int, DifficultyProfile>
		{
			{ 1, DifficultyProfile.Normal },
			{ 2, DifficultyProfile.Hard },
			{ 3, DifficultyProfile.VeryHard }
		};

		/// <summary>
		/// Spawns a monster with optional property overrides from XML.
		/// </summary>
		/// <param name="instance">The dungeon instance</param>
		/// <param name="monsterId">Monster ID to spawn</param>
		/// <param name="position">Spawn position</param>
		/// <param name="properties">Optional property list string (format: "Prop#Value#Prop2#Value2")</param>
		/// <param name="mapName">Optional map name (defaults to dungeon's main map)</param>
		/// <returns>The spawned mob</returns>
		public virtual Mob SpawnMonsterWithProperties(InstanceDungeon instance, int monsterId, Position position, string properties = null, string mapName = null)
		{
			var mob = this.SpawnMonster(instance, monsterId, position, mapName);

			if (!string.IsNullOrEmpty(properties))
			{
				mob.ApplyPropList(properties, this, null);
			}

			mob.HealToFull();

			return mob;
		}

		/// <summary>
		/// Spawns an NPC with XML properties applied.
		/// </summary>
		public virtual Npc SpawnNpcWithProperties(InstanceDungeon instance, int monsterId, Position position, string properties = null, string mapName = null)
		{
			var npc = this.SpawnNpc(instance, monsterId, "", position, Direction.South, mapName);

			if (!string.IsNullOrEmpty(properties))
			{
				npc.ApplyXmlProperties(PropListParser.Parse(properties));
			}

			return npc;
		}

		/// <summary>
		/// Spawns an NPC with XML properties applied.
		/// </summary>
		public virtual Npc SpawnNpcWithProperties(InstanceDungeon instance, int monsterId, Position position, Direction direction, string properties = null, string mapName = null)
		{
			var npc = this.SpawnNpc(instance, monsterId, "", position, direction, mapName);

			if (!string.IsNullOrEmpty(properties))
			{
				npc.ApplyXmlProperties(PropListParser.Parse(properties));
			}

			return npc;
		}

		/// <summary>
		/// Spawns an NPC with XML properties applied.
		/// </summary>
		public virtual Npc SpawnNpcWithProperties(InstanceDungeon instance, int monsterId, string name, Position position, Direction direction, string properties = null, string mapName = null)
		{
			var npc = this.SpawnNpc(instance, monsterId, name, position, direction, mapName);

			if (!string.IsNullOrEmpty(properties))
			{
				npc.ApplyXmlProperties(PropListParser.Parse(properties));
			}

			return npc;
		}

		/// <summary>
		/// Spawns a boss monster with enhanced properties.
		/// </summary>
		public virtual Mob SpawnBoss(InstanceDungeon instance, int monsterId, Position position, string properties = null, string mapName = null)
		{
			var boss = this.SpawnMonsterWithProperties(instance, monsterId, position, properties, mapName);

			// Ensure boss has proper settings
			if (boss.Rank != MonsterRank.Boss)
			{
				boss.Rank = MonsterRank.Boss;
			}

			// Add boss-specific logic here (shield, special AI, etc.)

			return boss;
		}

		/// <summary>
		/// Creates a defensive objective NPC (like the sacred fire in Uphill Defense).
		/// </summary>
		public virtual Mob CreateDefensiveObjective(InstanceDungeon instance, int monsterId, string name, Position position, int maxHp, string properties = null)
		{
			var objective = this.SpawnMonsterWithProperties(instance, monsterId, position, properties);
			objective.Name = name;
			objective.SpawnPosition = position;
			objective.Layer = instance.Layer;
			objective.Direction = Direction.South;

			// Set up as a defensive objective
			objective.Properties.Modify(PropertyName.MHP_BM, maxHp);
			objective.Properties.SetString(PropertyName.AlwaysShowHP, "YES");
			objective.Faction = FactionType.FreeForAll; // Can be attacked by monsters
			objective.HasExp = false;
			objective.HasDrops = false;
			objective.InvalidateProperties();
			objective.HealToFull();

			// Track objective death for fail condition
			objective.Died += (mob, killer) =>
			{
				// Dungeon fails if objective is destroyed
				this.OnDefensiveObjectiveDestroyed(instance);
			};

			return objective;
		}

		/// <summary>
		/// Called when a defensive objective is destroyed.
		/// </summary>
		protected virtual void OnDefensiveObjectiveDestroyed(InstanceDungeon instance)
		{
			foreach (var character in instance.Characters)
			{
				if (character == null) continue;
				Send.ZC_NORMAL.IndunAddonMsgParam(character.Connection, 2, "FAIL", 1);
			}

			this.DungeonEnded(instance, true);
		}

		/// <summary>
		/// Spawns a gimmick monster (special non-combat monster like Lapemiter).
		/// </summary>
		public virtual Mob SpawnGimmickMonster(InstanceDungeon instance, int monsterId, Position position, string aiScript = null)
		{
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(monsterId, out var monsterData))
			{
				throw new ArgumentException($"SpawnGimmickMonster: Monster '{monsterId}' not found.");
			}

			if (!ZoneServer.Instance.World.TryGetMap(this.MapName, out var map))
			{
				throw new ArgumentException($"SpawnGimmickMonster: MapName '{this.MapName}' not found.");
			}

			var gimmick = new Mob(monsterData.Id, RelationType.Neutral)
			{
				Position = position,
				SpawnPosition = position,
				Layer = instance.Layer,
				Tendency = TendencyType.Peaceful,
				HasExp = false,
				HasDrops = false
			};

			var movement = new MovementComponent(gimmick) { ShowMinimapMarker = false };
			gimmick.Components.Add(movement);

			if (!string.IsNullOrEmpty(aiScript) && AiScript.Exists(aiScript))
				gimmick.Components.Add(new AiComponent(gimmick, aiScript));

			map.AddMonster(gimmick);
			instance.CurrentStage?.AddMonster(instance, gimmick);

			return gimmick;
		}

		/// <summary>
		/// Spawns a wave of monsters with optional property overrides.
		/// </summary>
		public virtual List<Mob> SpawnWave(InstanceDungeon instance, int monsterId, Position centerPosition, int count, float spawnRadius = 5f, string properties = null)
		{
			var spawnedMobs = new List<Mob>();

			for (var i = 0; i < count; i++)
			{
				var spawnPos = centerPosition.GetRandomInRange2D(1, (int)spawnRadius);
				var mob = this.SpawnMonsterWithProperties(instance, monsterId, spawnPos, properties);
				spawnedMobs.Add(mob);
			}

			return spawnedMobs;
		}
	}

	public static class PropListParser
	{
		/// <summary>
		/// Parses a propList string from XML and returns a dictionary of property names to values.
		/// Format: 'PropertyName' 'Value' 'PropertyName2' 'Value2' ...
		/// </summary>
		public static Dictionary<string, string> Parse(string propList)
		{
			var result = new Dictionary<string, string>();
			if (string.IsNullOrWhiteSpace(propList))
				return result;

			// Split by single quotes, removing empty entries
			var parts = propList.Split(new[] { '\'', '#' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(p => p.Trim())
				.Where(p => !string.IsNullOrWhiteSpace(p))
				.ToList();

			// Process pairs of property name and value
			for (var i = 0; i < parts.Count - 1; i += 2)
			{
				var propName = parts[i];
				var propValue = parts[i + 1];
				result[propName] = propValue;
			}

			return result;
		}

		/// <summary>
		/// Formats a property dictionary into a C# string suitable for ApplyPropList.
		/// </summary>
		public static string FormatForCode(Dictionary<string, string> properties)
		{
			if (properties == null || properties.Count == 0)
				return string.Empty;

			var pairs = properties.Select(kvp => $"{kvp.Key}#{kvp.Value}");
			return string.Join("#", pairs);
		}
	}
}
