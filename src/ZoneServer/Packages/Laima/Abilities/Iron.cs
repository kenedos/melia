using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
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
	public class IronOverride : IAbilityHandler, IAbilityCombatDefenseAfterCalcHandler
	{
		/// <summary>
		/// Reduces physical damage taken if wearing full plate armor.
		/// </summary>
		public void OnDefenseAfterCalc(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target is not Character character)
				return;

			if (skill.Data.AttackType == SkillAttackType.Magic)
				return;

			if (character.IsWearingFullArmorSetOfType(ArmorMaterialType.Iron))
			{
				skillHitResult.Damage *= 0.7f;
			}
		}
	}
}
