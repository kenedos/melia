using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Shared.Data.Database;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Plate Mastery (Iron) ability.
	/// Reduces physical damage taken by 20% when wearing full plate armor.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Iron)]
	public class IronOverride : IAbilityHandler
	{
		/// <summary>
		/// Reduces physical damage taken if wearing full plate armor.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Iron)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target is not Character character)
				return;

			if (skill.Data.AttackType == SkillAttackType.Magic)
				return;

			var count = character.Inventory.CountEquipMaterial(ArmorMaterialType.Iron);
			if (count >= 4)
			{
				skillHitResult.Damage *= 0.7f;
			}
		}
	}
}
