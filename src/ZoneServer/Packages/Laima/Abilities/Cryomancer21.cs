using System.Collections.Concurrent;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using System;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Shield Mastery: Cryomancer ability.
	/// Increases magic defense by 5% per ability level while using a shield.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Cryomancer21)]
	public class Cryomancer21Override : IAbilityHandler, IAbilityCombatDefenseAfterCalcHandler
	{
		/// <summary>
		/// Reduces physical damage taken if wearing full plate armor.
		/// </summary>
		public void OnDefenseAfterCalc(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target is not Character character)
				return;

			if (skill.Data.AttackType != SkillAttackType.Magic)
				return;

			var lhItem = character.Inventory.GetEquip(EquipSlot.LeftHand);
			if (lhItem == null || lhItem.Data.EquipType1 != EquipType.Shield)
				return;

			var bonus = Math.Min(ability.Level * 0.05f, 0.5f);
			skillHitResult.Damage *= 1 - bonus;
		}
	}
}
