using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using Yggdrasil.Util;

namespace Melia.Zone.Scripting
{
	public static partial class Shortcuts
	{
		private static readonly TimeSpan GuardRespawnDelay = TimeSpan.FromMinutes(5);

		private static readonly Dictionary<string, GuardAnchor> _guardAnchors = new();

		/// <summary>
		/// Owns the spawn/respawn loop for a single guard post. The anchor
		/// outlives the mob it spawns: when the mob dies, the anchor schedules
		/// a fresh spawn at the same spot. Disposing the anchor (e.g. on
		/// /reloadscripts) stops the loop and removes the live mob.
		/// </summary>
		private class GuardAnchor
		{
			public readonly int MonsterClassId;
			public readonly string Name;
			public readonly Map MapObj;
			public readonly Position Pos;
			public readonly Direction Dir;
			public readonly int Level;

			public Mob Current;
			public bool Disposed;

			/// <summary>
			/// Creates a new anchor with the spawn parameters. Does NOT spawn
			/// a mob; call SpawnNow() to do that.
			/// </summary>
			/// <param name="monsterClassId"></param>
			/// <param name="name"></param>
			/// <param name="mapObj"></param>
			/// <param name="pos"></param>
			/// <param name="dir"></param>
			/// <param name="level"></param>
			public GuardAnchor(int monsterClassId, string name, Map mapObj, Position pos, Direction dir, int level)
			{
				this.MonsterClassId = monsterClassId;
				this.Name = name;
				this.MapObj = mapObj;
				this.Pos = pos;
				this.Dir = dir;
				this.Level = level;
			}

			/// <summary>
			/// Spawns a fresh guard mob and wires its death back into this
			/// anchor so the respawn loop continues. No-op if the anchor was
			/// disposed.
			/// </summary>
			public void SpawnNow()
			{
				if (this.Disposed)
					return;

				this.Current = SpawnGuard(this.MonsterClassId, this.Name, this.MapObj, this.Pos, this.Dir, this.Level);
				this.Current.Died += this.OnDied;
			}

			/// <summary>
			/// Death handler that waits the respawn delay and then re-spawns
			/// via SpawnNow. The Disposed check inside SpawnNow drops the
			/// continuation if the anchor has been disposed in the meantime.
			/// </summary>
			/// <param name="mob"></param>
			/// <param name="killer"></param>
			private async void OnDied(Mob mob, ICombatEntity killer)
			{
				await Task.Delay(GuardRespawnDelay);
				this.SpawnNow();
			}

			/// <summary>
			/// Stops the respawn loop and removes the live mob from the map
			/// if there is one. Safe to call multiple times.
			/// </summary>
			public void Dispose()
			{
				this.Disposed = true;
				if (this.Current != null && !this.Current.IsDead)
					this.MapObj.RemoveMonster(this.Current);
				this.Current = null;
			}
		}

		private static readonly string[] GuardIdleLines =
		{
			"I'm garrisoned here to keep this area safe.",
			"Stay sharp out there. The roads aren't always quiet.",
			"Just on watch. Holler if anything's wrong.",
			"Trouble nearby? Let me know.",
			"Quiet shift so far. Long may it last.",
			"Safe travels. Don't stray too far from the road.",
			"If you see anything aggressive, give it a wide berth.",
			"Long shifts, but the village sleeps easier for it.",
			"Don't worry, I've got this stretch covered.",
			"Take care out there, traveler.",
		};

		private static readonly string[] GuardCombatLines =
		{
			"Get back, I'll cover you!",
			"Stay behind me!",
			"Careful — let me draw it off.",
			"I've got it, just keep your distance!",
			"Hold on, I'm right with you!",
			"Don't worry about me, just stay safe!",
			"Make some space, I need room to swing!",
			"Keep your head down, this won't take long!",
			"Watch yourself — I'll deal with this one!",
			"Step back if you can, I've got the angle!",
		};

		private static readonly TimeSpan GuardChatCooldown = TimeSpan.FromSeconds(2);

		/// <summary>
		/// Makes the given guard speak one of the canned guard lines as a
		/// chat bubble. Picks from the combat line pool when the guard is
		/// in attack state, otherwise from the idle pool. Rate-limited per
		/// guard via GuardChatCooldown so players can't spam clicks.
		/// </summary>
		/// <param name="guard"></param>
		public static void SayRandomGuardLine(Mob guard)
		{
			var now = DateTime.UtcNow;
			var nextSpeakAt = guard.Vars.Get<DateTime>("Laima.Guards.NextChatAt");
			if (now < nextSpeakAt)
				return;

			guard.Vars.Set("Laima.Guards.NextChatAt", now + GuardChatCooldown);

			var pool = (guard.CombatState?.AttackState ?? false) ? GuardCombatLines : GuardIdleLines;
			var line = pool[RandomProvider.Get().Next(pool.Length)];
			guard.Say(line);
		}

		/// <summary>
		/// Spawns a combat-capable guard NPC at the given location. Guards
		/// proactively attack any nearby monster with TendencyType.Aggressive,
		/// retaliate when attacked by hostile monsters, and respawn in place
		/// after they die.
		/// </summary>
		/// <remarks>
		/// Guards use the FactionType.Our_Forces faction so monsters treat
		/// them as enemies and players cannot attack them.
		///
		/// The monster id passed in supplies the visual model and base data;
		/// only models that have walk/run AND attack animations work
		/// correctly. Verified IDs:
		///
		///   20059  - orsha_soldier_f         (Orsha Soldier, female)
		///   20060  - orsha_soldier_m         (Orsha Soldier, male)
		///   147410 - npc_soldier_female_01   (Female Guard variant 1)
		///   147415 - npc_soldier_female_02   (Female Guard variant 2)
		///   147416 - npc_soldier_female_03   (Female Guard variant 3)
		///
		/// Models confirmed to have idle-only animations (do NOT use):
		/// soldier_axe, orsha_soldier_f (anim folder is empty in client),
		/// most monster_* soldiers (they're enemies, not guards).
		/// </remarks>
		/// <param name="monsterClassId">Monster id used for visuals/base data.</param>
		/// <param name="name">Display name. Pass null to keep the data name.</param>
		/// <param name="map">Map class name to spawn on.</param>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <param name="direction">Facing direction in degrees.</param>
		/// <param name="level">Guard level used for stat scaling.</param>
		/// <returns></returns>
		public static Mob AddCombatNpc(int monsterClassId, string name, string map, double x, double z, double direction, int level)
		{
			var mapObj = GetMapOrThrow(map);

			if (!ZoneServer.Instance.Data.MonsterDb.Entries.ContainsKey(monsterClassId))
				throw new ArgumentException($"Monster class id '{monsterClassId}' not found.");

			var pos = new Position((float)x, 0, (float)z);
			if (mapObj.Ground.TryGetHeightAt(pos, out var height))
				pos.Y = height;

			var dir = new Direction(direction);
			var key = $"{mapObj.ClassName}|{pos.X:F2}|{pos.Z:F2}";

			lock (_guardAnchors)
			{
				if (_guardAnchors.TryGetValue(key, out var existing))
					existing.Dispose();

				var anchor = new GuardAnchor(monsterClassId, name, mapObj, pos, dir, level);
				_guardAnchors[key] = anchor;
				anchor.SpawnNow();

				return anchor.Current;
			}
		}

		/// <summary>
		/// Builds and registers a single guard mob on the map. Used by
		/// GuardAnchor for both the initial spawn and every respawn — does
		/// not handle the respawn timer itself.
		/// </summary>
		/// <param name="monsterClassId"></param>
		/// <param name="name"></param>
		/// <param name="mapObj"></param>
		/// <param name="pos"></param>
		/// <param name="dir"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		private static Mob SpawnGuard(int monsterClassId, string name, Map mapObj, Position pos, Direction dir, int level)
		{
			var mob = new Mob(monsterClassId, RelationType.Friendly);

			mob.Position = pos;
			mob.SpawnPosition = pos;
			mob.Direction = dir;
			mob.Faction = FactionType.Our_Forces;
			mob.Tendency = TendencyType.Aggressive;

			if (!string.IsNullOrEmpty(name))
				mob.Name = name;

			mob.Vars.SetBool("Laima.Guards.IsGuard", true);

			mob.Components.Add(new MovementComponent(mob));
			mob.Components.Add(new AiComponent(mob, "Guard"));

			mob.ApplyOverrides(GenerateGuardStats(level));

			mapObj.AddMonster(mob);
			return mob;
		}

		/// <summary>
		/// Returns a stat override table scaled to the given guard level.
		/// Roughly 50-100% stronger than a normal mob of the same level so
		/// guards aren't pushovers; see formulas in body for the curve.
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		private static PropertyOverrides GenerateGuardStats(int level)
		{
			var overrides = new PropertyOverrides();
			var lv = Math.Max(1, level);

			var maxHp = (int)(1000 + (lv * lv) * 1.6 + lv * 60);
			var minPatk = (int)(240 + lv * 8.5);
			var maxPatk = (int)(minPatk * 1.15);
			var defStat = (int)(120 + lv * 14);

			var accuracy = SoftCapSubstat(lv * 0.9f);
			var critAttack = SoftCapSubstat(lv * 1.2f);
			var block = SoftCapSubstat(lv * 0.8f);

			overrides["Lv"] = lv;
			overrides["MHP"] = maxHp;
			overrides["MINPATK"] = minPatk;
			overrides["MAXPATK"] = maxPatk;
			overrides["MINMATK"] = minPatk;
			overrides["MAXMATK"] = maxPatk;
			overrides["DEF"] = defStat;
			overrides["MDEF"] = defStat;
			overrides["HR"] = accuracy;
			overrides["DR"] = accuracy;
			overrides["CRTHR"] = accuracy;
			overrides["CRTDR"] = accuracy;
			overrides["CRTATK"] = critAttack;
			overrides["BLK"] = block;
			overrides["BLK_BREAK"] = block;

			return overrides;
		}

		/// <summary>
		/// Applies a soft cap to accuracy/dodge-style substats so they don't
		/// scale linearly forever. Mirrors the diminishing-returns formula
		/// used by mob stat scripts (linear up to 60, then sqrt-tapered).
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static int SoftCapSubstat(float value)
		{
			if (value <= 60)
				return (int)Math.Round(value);

			return (int)Math.Round(60 + Math.Sqrt((value - 60) * 20));
		}

	}
}
