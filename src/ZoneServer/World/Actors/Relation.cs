using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.World.Actors
{
	/// <summary>
	/// Extension methods for checking relations between entities.
	/// </summary>
	public static class Relation
	{

		/// <summary>
		/// Returns true if actor and target are alive allies
		/// (friendly, in party or in guild).
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool IsAlly(this IActor actor, IActor target)
		{
			if (actor == null || target == null)
				return false;

			if (actor is ICombatEntity combatActor && combatActor.IsDead)
				return false;

			if (target is ICombatEntity combatTarget && combatTarget.IsDead)
				return false;

			if (CheckRelation(actor, target, RelationType.Friendly))
				return true;
			if (CheckRelation(actor, target, RelationType.Party))
				return true;
			if (CheckRelation(actor, target, RelationType.Guild))
				return true;

			return false;
		}

		/// <summary>
		/// Returns true if actor is dead and would be an ally of target
		/// (friendly, in party or in guild).
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool IsDeadAlly(this IActor actor, IActor target)
		{
			if (actor == null || target == null)
				return false;

			if (actor is not ICombatEntity combatActor || !combatActor.IsDead)
				return false;

			if (CheckRelation(actor, target, RelationType.Friendly))
				return true;
			if (CheckRelation(actor, target, RelationType.Party))
				return true;
			if (CheckRelation(actor, target, RelationType.Guild))
				return true;

			return false;
		}

		/// <summary>
		/// Returns true if actor and target are enemies.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool IsEnemy(this IActor actor, IActor target)
		{
			if (actor == null || target == null) 
				return false;

			if (actor is ICombatEntity combatActor && combatActor.IsDead)
				return false;

			if (target is ICombatEntity combatTarget && combatTarget.IsDead)
				return false;

			return CheckRelation(actor, target, RelationType.Enemy);
		}

		/// <summary>
		/// Returns true if target has the given relation towards actor.
		/// Note: RelationType.Friendly does NOT include Party or Guild members!
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="target"></param>
		/// <param name="targetRelation"></param>
		/// <returns></returns>
		public static bool CheckRelation(this IActor actor, IActor target, RelationType targetRelation)
		{
			if (targetRelation == RelationType.All)
				return true;
			if (actor == target && targetRelation == RelationType.Party)
				return true;

			// GetRelation now handles owner lookups for summons/companions internally
			return GetRelation(actor, target) == targetRelation;
		}

		/// <summary>
		/// Makes all the checks between actor and target, returning their
		/// relation to eachother. Considers duels, parties, guilds, pvp maps,
		/// summons, and factions.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static RelationType GetRelation(this IActor actor, IActor target)
		{
			// Entity is always friendly to itself
			if (actor == target)
				return RelationType.Friendly;

			if (actor == null || target == null)
				return RelationType.Neutral;

			// Get the effective "owner" for relation checks (for owned summons/companions/dummies)
			var effectiveActor = actor;
			var effectiveTarget = target;

			if (actor is IMonster actorMonster && actorMonster.OwnerHandle != 0
				&& actor.Map.TryGetCombatEntity(actorMonster.OwnerHandle, out var actorOwner))
			{
				effectiveActor = actorOwner;
			}
			else if (actor is DummyCharacter dummyActor && dummyActor.HasOwner)
			{
				effectiveActor = dummyActor.Owner;
			}

			if (target is IMonster targetMonster && targetMonster.OwnerHandle != 0
				&& target.Map.TryGetCombatEntity(targetMonster.OwnerHandle, out var targetOwner))
			{
				effectiveTarget = targetOwner;
			}
			else if (target is DummyCharacter dummyTarget && dummyTarget.HasOwner)
			{
				effectiveTarget = dummyTarget.Owner;
			}

			// If both effective actors are characters, check duel/PvP relations
			if (effectiveActor is Character character && effectiveTarget is Character targetCharacter)
			{
				// Don't attack your own summons/companions
				if (character == targetCharacter)
					return RelationType.Friendly;

				// Check duels
				var duel = character.Connection?.ActiveDuel;
				if (duel != null)
				{
					if (duel.AreDueling(character, targetCharacter))
						return RelationType.Enemy;
					if (duel.AreOnSameTeam(character, targetCharacter))
						return RelationType.Friendly;
				}

				if (character.HasParty && targetCharacter.HasParty
				&& character.Connection?.Party == targetCharacter.Connection?.Party)
					return RelationType.Party;

				// Removed: Guild type deleted during Laima merge
				// Guild relation check and GTW enemy check removed

				// Check PvP Arena
				// Note: Keep it after party check
				if (character.Map.IsPVP && targetCharacter.Map.IsPVP)
					return RelationType.Enemy;
			}

			// For non-owned monsters or when effective actors aren't both characters
			if (actor.Faction == target.Faction)
			{
				// Monsters of same faction are friendly (but only if not owned by enemy players - checked above)
				if (actor is IMonster && target is IMonster)
				{
					return RelationType.Friendly;
				}

				// Same faction, but not same actor type.
				// Likely character + NPC or monster + NPC.
				return RelationType.Neutral;
			}

			// Combat entities check for their factions
			if ((actor is ICombatEntity actorEntity) && (target is ICombatEntity targetEntity))
			{
				var isHostile = ZoneServer.Instance.Data.FactionDb.CheckHostility(actorEntity.Faction, targetEntity.Faction);

				return isHostile ? RelationType.Enemy : RelationType.Neutral;
			}

			// Other actors are always neutral to everything
			return RelationType.Neutral;
		}
	}
}
