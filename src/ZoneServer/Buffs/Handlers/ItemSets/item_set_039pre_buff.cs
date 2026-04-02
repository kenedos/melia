using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.Handlers.ItemSets
{
	/// <summary>
	/// Handler for the Varpas/Verijo Set 3-piece bonus buff.
	/// After every 5 attacks, deals an explosion with random bonus damage.
	/// </summary>
	[BuffHandler(BuffId.item_set_039pre_buff)]
	public class item_set_039pre_buff : BuffHandler
	{
		private const string AttackCountVar = "Melia.Set039.AttackCount";
		private const int RequiredAttacks = 5;
		private const float BonusDamageRatio = 0.1f;
		private const int MinBaseDamage = 1000;
		private const int MaxBaseDamage = 3000;

		/// <summary>
		/// Called after attack damage is calculated.
		/// Counts attacks and deals explosion damage after 5 hits.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.item_set_039pre_buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.item_set_039pre_buff, out var buff))
				return;

			// Don't apply if no damage was dealt
			if (skillHitResult.Damage <= 0)
				return;

			// Increment attack counter
			var attackCount = buff.Vars.GetInt(AttackCountVar) + 1;
			buff.Vars.SetInt(AttackCountVar, attackCount);

			// Check if we've reached 5 attacks
			if (attackCount < RequiredAttacks)
				return;

			// Reset counter
			buff.Vars.SetInt(AttackCountVar, 0);

			// Calculate random explosion damage
			var baseDamage = RandomProvider.Get().Next(MinBaseDamage, MaxBaseDamage + 1);
			var explosionDamage = baseDamage * BonusDamageRatio;

			// Add explosion damage
			skillHitResult.Damage += explosionDamage;

			// Display as 2 hits for the explosion effect
			skillHitResult.HitCount += 1;

			// TODO: Play explosion effect
			// PlayEffect(target, "F_explosion065_violet", 1, 1, 'BOT')
		}
	}
}
