using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
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

			monster.SetVisibilty(ActorVisibility.Track, character.ObjectId);
			monster.AddEffect(new ScriptInvisibleEffect());

			if (spec.EndPosition.HasValue)
				monster.SpawnPosition = spec.EndPosition.Value;

			// The movement component is left to SetTrackTendency, which adds
			// it when the cutscene hands the actors over to the fight.
			if (spec.CombatNpc)
				MakeCombatNpc(monster, character);
			else
				monster.Components.Add(new AiComponent(monster, string.IsNullOrEmpty(spec.Ai) ? "BasicMonster" : spec.Ai));

			character.Map.AddMonster(monster);

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
	}
}
