using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.ItemSets
{
	/// <summary>
	/// Handler for the Earth Armor Set buff (item_set_013_buff).
	/// Adds Earth damage on normal attacks based on overbuff counter.
	/// </summary>
	[BuffHandler(BuffId.item_set_013_buff)]
	public class item_set_013_buff : BuffHandler, IBuffCombatAttackAfterCalcHandler
	{
		/// <summary>
		/// Bonus damage ratio for the extra hit.
		/// </summary>
		private const float BonusDamageRatio = 0.15f;

		/// <summary>
		/// Called after attack damage is calculated.
		/// Adds bonus Earth damage on normal attacks based on overbuff count.
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

			// The overbuff counter determines the damage multiplier
			var damageMultiplier = buff.OverbuffCounter;
			if (damageMultiplier <= 0)
				return;

			// Add bonus damage based on overbuff count
			var bonusDamage = skillHitResult.Damage * BonusDamageRatio * damageMultiplier;
			skillHitResult.Damage += bonusDamage;

			// Display as 2 hits
			skillHitResult.HitCount = 2;
		}
	}
}
