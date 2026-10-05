using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Two-handed Spear Mastery: Penetration ability.
	/// Increases block penetration by 5% per ability level while using a two-handed spear.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Cataphract31)]
	public class Cataphract31Override : IAbilityHandler
	{
		/// <summary>
		/// Increases block penetration when attacking with a two-handed spear equipped.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, AbilityId.Cataphract31)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker is not Character character)
				return;

			if (!attacker.TryGetActiveAbility(AbilityId.Cataphract31, out var ability))
				return;

			var weapon = character.Inventory.GetEquip(EquipSlot.RightHand);
			if (weapon == null || weapon.Data.EquipType1 != EquipType.THSpear)
				return;

			modifier.BlockPenetrationMultiplier += ability.Level * 0.05f;
		}
	}
}
