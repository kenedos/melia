using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using System;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Shield Mastery: Cryomancer ability.
	/// Reduces incoming magic damage while using a shield.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Cryomancer21)]
	public class Cryomancer21Override : IAbilityHandler
	{
		/// <summary>
		/// Reduces magic damage taken by 5% per level while using a shield, up to 50%.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Cryomancer21)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target is not Character character)
				return;

			if (skill.Data.AttackType != SkillAttackType.Magic)
				return;

			var lhItem = character.Inventory.GetEquip(EquipSlot.LeftHand);
			if (lhItem == null || lhItem.Data.EquipType1 != EquipType.Shield)
				return;

			if (!target.TryGetActiveAbility(AbilityId.Cryomancer21, out var ability))
				return;

			var bonus = Math.Min(ability.Level * 0.05f, 0.5f);
			skillHitResult.Damage *= 1 - bonus;
		}
	}
}
