using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Grace: Additional Holy Hits buff sold by a Spell
	/// Shop. Turns the target's attacks into holy property attacks and adds
	/// an extra hit to their basic attacks.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.SpellShop_Sacrament_Buff)]
	public class Pardoner_SpellShop_Sacrament_BuffOverride : BuffHandler
	{
		/// <summary>
		/// Share of the hit's damage the extra basic attack line deals.
		/// </summary>
		private const float BonusDamageRatio = 0.15f;

		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, BuffId.SpellShop_Sacrament_Buff)]
		public void OnAttackBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.IsBuffActive(BuffId.SpellShop_Sacrament_Buff))
				return;

			if (skill.Data.Attribute is AttributeType.None or AttributeType.Melee or AttributeType.Magic)
				modifier.AttackAttribute = AttributeType.Holy;
		}

		/// <summary>
		/// Adds an extra hit to the target's basic attacks.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.SpellShop_Sacrament_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.IsBuffActive(BuffId.SpellShop_Sacrament_Buff))
				return;

			if (!skill.IsNormalAttack)
				return;

			if (skillHitResult.Damage <= 0)
				return;

			if (target is Mob mob && mob.Data.Race == RaceType.Item)
				return;

			skillHitResult.AddExtraLine(skillHitResult.Damage * BonusDamageRatio, SkillId.SpellShop_Sacrament_AddBlow);
		}
	}
}
