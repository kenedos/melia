using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.World.Actors.Monsters
{
	/// <summary>
	/// Turns a monster a quest is pointed at into an ally that stays with the
	/// character it belongs to, so a conversation can hand a real map monster
	/// back to the player instead of standing a second copy of it next to it.
	/// </summary>
	public static class QuestAlly
	{
		/// <summary>
		/// The mob variable holding the handle of the character an ally follows.
		/// </summary>
		public const string OwnerVar = "Melia.QuestFollower.Owner";

		/// <summary>
		/// The AI an ally is given, which follows its owner rather than the
		/// spot it was standing on.
		/// </summary>
		public const string AllyAi = "BasicMonster";

		/// <summary>
		/// How long an ally stays on the map before it gives up and leaves, so a
		/// quest that ends or is abandoned cannot strand one behind.
		/// </summary>
		public static readonly TimeSpan DefaultLifeTime = TimeSpan.FromMinutes(10);

		/// <summary>
		/// How far from the character an ally is looked for when giving them up,
		/// which has to reach past the follower AI's own catch-up teleport.
		/// </summary>
		private const float ReleaseRange = 2000;

		/// <summary>
		/// Makes the given monster a peaceful ally of the character and sends it
		/// after them.
		/// </summary>
		/// <param name="mob"></param>
		/// <param name="owner"></param>
		/// <param name="lifeTime">How long the ally stays before it disappears.</param>
		/// <returns>The mob, or null when it cannot follow the character.</returns>
		public static Mob MakeAlly(Mob mob, Character owner, TimeSpan? lifeTime = null)
		{
			if (mob == null || owner == null || mob.Map == null || mob.Map != owner.Map)
				return null;

			mob.Components.Get<MovementComponent>()?.Stop();

			mob.Tendency = TendencyType.Peaceful;
			mob.Vars.SetInt(OwnerVar, owner.Handle);
			mob.OwnerHandle = owner.Handle;

			// The field AI starts by walking the mob back to its spawn point
			// before it falls through to following its master. Moving the spawn
			// point onto the owner skips that detour, so the ally heads
			// straight for them.
			mob.SpawnPosition = owner.Position;

			// Adding the component drops the field AI, and with it the hate list
			// it was carrying, so the ally starts out with nothing to chase.
			mob.Components.Add(new MovementComponent(mob));
			mob.Components.Add(new AiComponent(mob, AllyAi, owner));
			mob.Components.Add(new LifeTimeComponent(mob, lifeTime ?? DefaultLifeTime));

			owner.LookAround();
			return mob;
		}

		/// <summary>
		/// Gives up on all of the character's followers, handing each one back to
		/// the map as a normal monster. Used when whatever turned them into
		/// allies runs out, so a quest that is left or fails cannot leave a pack
		/// of peaceful demons following the character around.
		/// </summary>
		/// <param name="character"></param>
		public static void ReleaseAllies(Character character)
		{
			if (character?.Map == null)
				return;

			foreach (var ally in character.Map.GetActorsInRange<Mob>(character.Position, ReleaseRange, mob => !mob.IsDead
					&& mob.Vars.GetInt(OwnerVar, 0) == character.Handle)
				.ToList())
			{
				ally.Tendency = TendencyType.Aggressive;
				ally.Vars.Remove(OwnerVar);
				ally.OwnerHandle = 0;
				ally.Components.Remove<LifeTimeComponent>();

				// Dropped and replaced so the new script has no master to follow.
				ally.Components.Remove<AiComponent>();
				ally.Components.Add(new AiComponent(ally, AllyAi));
			}
		}

		/// <summary>
		/// Returns every one of the character's followers on the map, wherever
		/// they currently are.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public static IEnumerable<Mob> Allies(Character character)
		{
			if (character?.Map == null)
				return Enumerable.Empty<Mob>();

			return character.Map.GetMonsters(monster => monster is Mob mob
				&& !mob.IsDead
				&& mob.Vars.GetInt(OwnerVar, 0) == character.Handle)
				.OfType<Mob>();
		}

		/// <summary>
		/// Returns the character's own followers standing within the given range
		/// of a position, nearest first.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="center"></param>
		/// <param name="range"></param>
		/// <returns></returns>
		public static IEnumerable<Mob> AlliesInRange(Character character, Position center, float range)
		{
			return character.Map.GetActorsInRange<Mob>(center, range, mob => !mob.IsDead
					&& mob.Layer == character.Layer
					&& mob.Vars.GetInt(OwnerVar, 0) == character.Handle)
				.OrderBy(mob => mob.Position.Get2DDistance(center));
		}
	}
}