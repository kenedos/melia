using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Leather Mastery (Leather) ability.
	/// Increases critical chance and damage when wearing full leather armor.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Leather)]
	public class LeatherOverride : IAbilityHandler
	{
		/// <summary>
		/// Applies the Leather Mastery effects before damage calculation.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, AbilityId.Leather)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker is not Character character)
				return;

			var count = character.Inventory.CountEquipMaterial(ArmorMaterialType.Leather);
			if (count >= 4)
			{
				switch (character.JobClass)
				{
					default:
						modifier.MinCritChance += 20;
						modifier.DamageMultiplier *= 1.15f;
						break;
				}
			}
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, AbilityId.Leather)]
		public void OnDefenseBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target is not Character character)
				return;

			var count = character.Inventory.CountEquipMaterial(ArmorMaterialType.Leather);
			if (count >= 4)
			{
				switch (character.JobClass)
				{
					case JobClass.Archer:
						break;
					case JobClass.Scout:
						modifier.BonusDodgeChance += 10;
						break;
				}
			}
		}
	}
}
