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
	/// [Arts] Cloth Mastery: Enchant Rune of Destruction ability, which
	/// raises critical chance by 15% and damage by 10% while the Rune Caster
	/// wears 4 pieces of cloth armor.
	/// </summary>
	[Package("laima-skills")]
	[AbilityHandler(AbilityId.RuneCaster13)]
	public class RuneCaster13Override : IAbilityHandler
	{
		private const int RequiredPieces = 4;
		private const float CritRateBonus = 0.15f;
		private const float DamageBonus = 0.10f;

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, AbilityId.RuneCaster13)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker is not Character character || !character.IsAbilityActive(AbilityId.RuneCaster13))
				return;

			if (character.Inventory.CountEquipMaterial(ArmorMaterialType.Cloth) < RequiredPieces)
				return;

			modifier.CritRateMultiplier += CritRateBonus;
			modifier.DamageMultiplier += DamageBonus;
		}
	}
}
