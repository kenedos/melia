using System;
using System.Threading;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using Yggdrasil.Logging;

namespace Melia.Zone.Scripting
{
	/// <summary>
	/// The properties a cutscene's spawn keyframe can set on the actor it
	/// spawns.
	/// </summary>
	public class TrackActorSpec
	{
		/// <summary>
		/// Gets or sets the actor's display name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the faction the actor belongs to, which decides who
		/// it fights and who can target it.
		/// </summary>
		public FactionType Faction { get; set; } = FactionType.Monster;

		/// <summary>
		/// Gets or sets the name of the AI the actor runs.
		/// </summary>
		public string Ai { get; set; } = "BasicMonster";

		/// <summary>
		/// Gets or sets the actor's max HP, or 0 to keep the monster data's.
		/// </summary>
		public int MaxHp { get; set; }

		/// <summary>
		/// Gets or sets the actor's level, or 0 to keep the monster data's.
		/// </summary>
		public int Level { get; set; }

		/// <summary>
		/// Gets or sets the actor's run speed, or 0 to keep the monster
		/// data's.
		/// </summary>
		public float RunSpeed { get; set; }

		/// <summary>
		/// Gets or sets the actor's walk speed, or 0 to keep the monster
		/// data's.
		/// </summary>
		public float WalkSpeed { get; set; }

		/// <summary>
		/// Gets or sets how far the actor looks for targets, or 0 to keep
		/// the monster data's.
		/// </summary>
		public float SearchRange { get; set; }

		/// <summary>
		/// Gets or sets whether the actor attacks on its own.
		/// </summary>
		public bool Aggressive { get; set; }

		/// <summary>
		/// Gets or sets whether the actor fights alongside the player as a
		/// combat NPC, the same way the guards added by AddCombatNpc do.
		/// </summary>
		public bool CombatNpc { get; set; }

		/// <summary>
		/// Gets or sets where the cutscene leaves the actor standing, for the
		/// actors it walks somewhere before handing them over to the fight.
		/// </summary>
		/// <remarks>
		/// The client plays those walks itself and never reports them.
		/// </remarks>
		public Position? EndPosition { get; set; }
	}

	public static partial class Shortcuts
	{
		/// <summary>
		/// Spawns one of a cutscene's actors into the character's private
		/// layer, applying the properties the cutscene's spawn keyframe
		/// carries.
		/// </summary>
		/// <param name="character">The character whose layer the actor spawns into.</param>
		/// <param name="monsterId">The monster id to spawn.</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="spec">The properties to apply, or null for the defaults.</param>
		/// <returns></returns>
		public static Mob AddTrackActor(Character character, int monsterId, double x, double y, double z, double direction, TrackActorSpec spec = null)
		{
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(monsterId, out var monsterData))
			{
				Log.Warning("AddTrackActor: Monster '{0}' not found.", monsterId);
				return null;
			}

			spec ??= new TrackActorSpec();

			var friendly = spec.CombatNpc || spec.Faction == FactionType.Our_Forces || spec.Faction == FactionType.Mon_Our_Forces;
			var monster = new Mob(monsterData.Id, friendly ? RelationType.Friendly : RelationType.Enemy);

			if (!string.IsNullOrEmpty(spec.Name))
				monster.Name = spec.Name;

			monster.Position = new Position((float)x, (float)y, (float)z);
			monster.SpawnPosition = monster.Position;
			monster.Direction = new Direction(direction);
			monster.Layer = character.Layer;
			monster.Faction = spec.Faction;

			if (spec.Aggressive)
				monster.Tendency = TendencyType.Aggressive;

			monster.Visibility = ActorVisibility.Always;
			monster.AddEffect(new ScriptInvisibleEffect());

			if (spec.EndPosition.HasValue)
				monster.SpawnPosition = spec.EndPosition.Value;

			// The movement component is left to SetTrackTendency, which adds
			// it when the cutscene hands the actors over to the fight.
			if (spec.CombatNpc)
				MakeCombatNpc(monster, character);
			else
				monster.Components.Add(new AiComponent(monster, string.IsNullOrEmpty(spec.Ai) ? "BasicMonster" : spec.Ai));

			// Added at once, since the cutscene packet names it by handle right after.
			character.Map.AddMonster(monster, immediate: true);

			var overrides = new PropertyOverrides();

			if (spec.Level > 0)
				overrides["Lv"] = spec.Level;
			if (spec.MaxHp > 0)
				overrides["MHP"] = spec.MaxHp;
			if (spec.RunSpeed > 0)
				overrides["RunMSPD"] = spec.RunSpeed;
			if (spec.WalkSpeed > 0)
				overrides["WlkMSPD"] = spec.WalkSpeed;

			if (overrides.Count > 0)
				monster.ApplyOverrides(overrides);

			if (spec.SearchRange > 0)
				monster.Properties.SetFloat(PropertyName.SDR, spec.SearchRange);

			return monster;
		}

		/// <summary>
		/// Spawning a monster for a track.
		/// Used for monsters that spawn in tracks/cutscenes.
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
		public static Mob AddTrackMonster(Character character, int monsterId, string name, string mapName, double x, double y, double z, double direction, string faction = "Monster", string tendency = "")
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

			monster.Visibility = ActorVisibility.Always;
			monster.AddEffect(new ScriptInvisibleEffect());
			var ai = new AiComponent(monster, "BasicMonster");
			monster.Components.Add(ai);

			map.AddMonster(monster, immediate: true);

			return monster;
		}

		/// <summary>
		/// Adds new Track NPC to the world.
		/// Used for npcs that spawn in tracks/cutscenes.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="monsterId"></param>
		/// <param name="name"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="dialogFuncName"></param>
		/// <param name="enterFuncName"></param>
		/// <param name="leaveFuncName"></param>
		/// <param name="range"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static Npc AddTrackNpc(Character character, int monsterId, string name, string map, double x, double y, double z, double direction, string dialogFuncName = "", string enterFuncName = "", string leaveFuncName = "", int state = -2, double range = 100, double scale = 1)
		{
			var mapObj = GetMapOrThrow(map);

			var pos = new Position((float)x, (float)y, (float)z);

			// Wrap name in localization code if applicable
			if (Dialog.IsLocalizationKey(name))
			{
				name = Dialog.WrapLocalizationKey(name);
			}
			// Insert line breaks in tagged NPC names that don't have one
			else if (name.StartsWith('[') && !name.Contains("{nl}"))
			{
				var endIndex = name.LastIndexOf("] ");
				if (endIndex != -1)
				{
					// Remove space and insert new line instead.
					name = name.Remove(endIndex + 1, 1);
					name = name.Insert(endIndex + 1, "{nl}");
				}
			}

			var location = new Location(mapObj.Id, pos);
			var dir = new Direction(direction);

			ZoneServer.Instance.DialogFunctions.TryGet(dialogFuncName, out var dialog);
			ZoneServer.Instance.TriggerFunctions.TryGet(enterFuncName, out var enter);
			ZoneServer.Instance.TriggerFunctions.TryGet(leaveFuncName, out var leave);

			var uniqueId = Interlocked.Increment(ref UniqueNpcNameId);
			var uniqueName = $"__NPC{uniqueId}__";
			var monster = new Npc(monsterId, name, location, dir, 0);
			monster.UniqueName = uniqueName;
			if (dialog != null)
			{
				monster.SetClickTrigger(dialogFuncName, dialog);
				var uniqueDialogName = $"{dialogFuncName}_{mapObj.Data.ClassName}";
				// Account for multiple npcs using the same dialogue.
				ZoneServer.Instance.World.NPCs.TryAdd(uniqueDialogName, monster);
			}
			if (enter != null || leave != null)
				monster.SetTriggerArea(Spot(monster.Position.X, monster.Position.Z, range));
			if (enter != null)
				monster.SetEnterTrigger(enterFuncName, enter);
			if (leave != null)
				monster.SetLeaveTrigger(leaveFuncName, leave);

			if (state != -2)
				monster.State = (NpcState)state;
			if (range != 0)
				monster.Properties.SetFloat(PropertyName.Range, (float)range);
			if (scale != 1)
				monster.Properties.SetFloat(PropertyName.Scale, (float)scale);

			monster.Visibility = ActorVisibility.Always;
			monster.AddEffect(new ScriptInvisibleEffect());
			monster.Layer = character.Layer;

			mapObj.AddMonster(monster, immediate: true);

			return monster;
		}

		/// <summary>
		/// Adds a Track NPC.
		/// Used for elevators, cable cars, moving platforms, etc.
		/// They also work with the "Track" system of the
		/// client, although they are not necessarily in a cutscene.
		/// and traversing.
		/// </summary>
		/// <param name="monsterId"></param>
		/// <param name="name"></param>
		/// <param name="map"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="direction"></param>
		/// <param name="trackString"></param>
		/// <param name="i1"></param>
		/// <param name="i2"></param>
		/// <returns></returns>
		public static Npc AddTrackNPC(int monsterId, string name, string map, double x, double y, double z, double direction, string trackString, int i1 = 2, int i2 = 5)
		{
			if (string.IsNullOrEmpty(map) || map == "None")
			{
				Log.Debug($"Skipped adding Track NPC {monsterId} - {name} at {x},{y},{z} because of invalid map: {map}");
				return null;
			}
			var npc = AddNpc(0, monsterId, name, map, x, y, z, direction);
			npc.Visibility = ActorVisibility.Always;
			npc.AddEffect(new ReviveEffect());
			npc.AddEffect(new SetTrackPosition());
			npc.AddEffect(new DirectionAPC(trackString, i1, i2));
			//if (ZoneServer.Instance.Data.MapDb.TryFind(map, out var mapData))
			//Log.Debug($"Adding Track NPC {monsterId} - {name} at {x},{y},{z} on {mapData.Name}");
			return npc;
		}
	}
}
