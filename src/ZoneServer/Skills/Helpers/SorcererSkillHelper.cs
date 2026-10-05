using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Extension methods for Sorcerer skill functionality.
	/// </summary>
	public static class SorcererExtensions
	{
		/// <summary>
		/// Checks if the summon is currently using a skill.
		/// </summary>
		/// <param name="summon"></param>
		/// <returns></returns>
		public static bool IsUsingSkill(this Summon summon)
		{
			// Check if the summon has an active skill being used
			return summon.Vars.GetBool("IsUsingSkill", false);
		}

		/// <summary>
		/// Gets the summon's normal attack skill.
		/// </summary>
		/// <param name="summon"></param>
		/// <returns></returns>
		public static Skill GetNormalAttackSkill(this Summon summon)
		{
			// Try to get the monster's normal attack skill
			var skillComponent = summon.Components.Get<BaseSkillComponent>();
			if (skillComponent == null)
				return null;

			// Return the first available attack skill
			return skillComponent.GetList().FirstOrDefault();
		}

		/// <summary>
		/// Gets a specific skill from the summon.
		/// </summary>
		/// <param name="summon"></param>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public static Skill GetSkill(this Summon summon, SkillId skillId)
		{
			var skillComponent = summon.Components.Get<BaseSkillComponent>();
			return skillComponent?.Get(skillId);
		}

		/// <summary>
		/// Moves the summon towards a target position.
		/// </summary>
		/// <param name="summon"></param>
		/// <param name="targetPos"></param>
		public static void MoveTo(this Summon summon, Shared.World.Position targetPos)
		{
			var movementComponent = summon.Components.Get<MovementComponent>();
			if (movementComponent != null)
			{
				movementComponent.MoveTo(targetPos);
			}
		}

		/// <summary>
		/// Stops the summon's current movement.
		/// </summary>
		/// <param name="summon"></param>
		public static void StopMove(this Summon summon)
		{
			var movementComponent = summon.Components.Get<MovementComponent>();
			movementComponent?.Stop();
		}

		/// <summary>
		/// Checks if the summon is in an OBB (oriented bounding box) area.
		/// </summary>
		/// <param name="summon"></param>
		/// <returns></returns>
		public static bool IsInOBB(this Summon summon)
		{
			// Check if the summon is in a restricted area
			// This typically prevents certain actions in indoor areas
			return summon.Vars.GetBool("IsInOBB", false);
		}

		/// <summary>
		/// Kills the summon.
		/// </summary>
		/// <param name="summon"></param>
		/// <param name="killer"></param>
		public static void Kill(this Summon summon, ICombatEntity killer)
		{
			if (summon.IsDead)
				return;

			// Set HP to 0 and trigger death
			summon.TakeDamage(summon.Hp, killer);
		}

		/// <summary>
		/// Gets a skill from the character.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public static Skill GetSkill(this Character character, SkillId skillId)
		{
			return character.Skills.Get(skillId);
		}

		/// <summary>
		/// Gets the level of an ability if active.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="abilityId"></param>
		/// <returns>The ability level, or 0 if not active.</returns>
		public static int GetAbilityLevel(this Character character, AbilityId abilityId)
		{
			var ability = character.Abilities.Get(abilityId);
			if (ability != null && ability.Active)
				return ability.Level;

			return 0;
		}

		/// <summary>
		/// Gets the level of an ability if active for any combat entity.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="abilityId"></param>
		/// <returns>The ability level, or 0 if not active or not a character.</returns>
		public static int GetAbilityLevel(this ICombatEntity entity, AbilityId abilityId)
		{
			if (entity is Character character)
				return character.GetAbilityLevel(abilityId);

			return 0;
		}

		/// <summary>
		/// Checks if a combat entity can attack another entity.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool CanAttack(this ICombatEntity attacker, ICombatEntity target)
		{
			if (target == null || target.IsDead)
				return false;

			// Check faction relation
			if (attacker is Summon summon && summon.Owner is Character owner)
			{
				// Use owner's faction for attack determination
				return owner.CanDamage(target);
			}

			// Default faction check
			return !attacker.IsFriendlyTo(target);
		}

		/// <summary>
		/// Checks if a combat entity is friendly to another.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		public static bool IsFriendlyTo(this ICombatEntity entity, ICombatEntity other)
		{
			if (entity == null || other == null)
				return false;

			// Same entity is always friendly
			if (entity.Handle == other.Handle)
				return true;

			// Check factions
			if (entity.Faction == other.Faction)
				return true;

			// Check if both are owned by the same player
			var entityOwner = GetTopOwner(entity);
			var otherOwner = GetTopOwner(other);

			if (entityOwner != null && otherOwner != null && entityOwner.Handle == otherOwner.Handle)
				return true;

			return false;
		}

		/// <summary>
		/// Gets the top-level owner of an entity.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		private static ICombatEntity GetTopOwner(ICombatEntity entity)
		{
			if (entity is Summon summon)
				return summon.Owner;

			return entity;
		}

		/// <summary>
		/// Toggles a control state on the character, enabling/disabling
		/// client-side UI controls.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="ident"></param>
		/// <param name="enabled"></param>
		public static void ToggleControl(this Character character, string ident, bool enabled)
		{
			Send.ZC_ENABLE_CONTROL(character, ident, enabled);
			Send.ZC_LOCK_KEY(character, ident, !enabled);
		}
	}
}
