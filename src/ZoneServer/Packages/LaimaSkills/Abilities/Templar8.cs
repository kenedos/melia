using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// [Arts] Plate Mastery: Divine ability, which reduces damage taken by
	/// 5%, and Holy damage taken by 30%, while wearing 4 plate pieces.
	/// </summary>
	[Package("laima-skills")]
	[AbilityHandler(AbilityId.Templar8)]
	public class Templar8Override : IAbilityHandler
	{
		private const int RequiredPlatePieces = 4;
		private const float DamageReduction = 0.05f;
		private const float HolyDamageReduction = 0.30f;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Templar8)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target is not Character character || !character.IsAbilityActive(AbilityId.Templar8))
				return;

			if (character.Inventory.CountEquipMaterial(ArmorMaterialType.Iron) < RequiredPlatePieces)
				return;

			skillHitResult.Damage *= 1f - DamageReduction;

			var attribute = modifier.AttackAttribute != AttributeType.None ? modifier.AttackAttribute : skill.Data.Attribute;
			if (attribute == AttributeType.Holy)
				skillHitResult.Damage *= 1f - HolyDamageReduction;
		}
	}
}
