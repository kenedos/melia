using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.ItemSets
{
	/// <summary>
	/// Handler for the Manahas Set 4-piece bonus buff.
	/// Adds an extra hit dealing 15% of original damage as Ice magic
	/// on normal attacks.
	/// </summary>
	[BuffHandler(BuffId.item_set_035_buff)]
	public class item_set_035_buff : BuffHandler, IBuffCombatAttackAfterCalcHandler
	{
		/// <summary>
		/// Bonus damage ratio for the extra hit.
		/// </summary>
		private const float BonusDamageRatio = 0.15f;

		/// <summary>
		/// Called after attack damage is calculated.
		/// Adds bonus Ice damage on normal attacks.
		/// </summary>
		public void OnAttackAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			// Only apply on normal attacks
			if (!skill.IsNormalAttack)
				return;

			// Don't apply if no damage was dealt
			if (skillHitResult.Damage <= 0)
				return;

			// Don't apply to item-type targets (destructible objects, etc.)
			if (target is Mob mob && mob.Data.Race == RaceType.Item)
				return;

			// Add 15% bonus damage as Ice magic true damage
			var bonusDamage = skillHitResult.Damage * BonusDamageRatio;
			skillHitResult.Damage += bonusDamage;

			// Display as 2 hits to match the original behavior
			skillHitResult.HitCount = 2;
		}
	}
}
