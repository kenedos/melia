using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Swordsmen.NakMuay
{
	/// <summary>
	/// Ability handler for Nak Muay: Additional Damage.
	/// Adds 10% of the character's physical attack as additional damage per ability level.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.NakMuay14)]
	public class NakMuay_AdditionalDamageAbility : IAbilityHandler
	{
		private const int MaxAbilityLevel = 5;
		private const float AdditionalDamageRatePerLevel = 0.10f;

		/// <summary>
		/// Applies the additional damage after the base damage calculation.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.NakMuay14)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker is not Character character)
				return;

			if (!this.IsNakMuayAttackSkill(skill))
				return;

			var abilityLevel = Math.Min(character.Abilities.GetLevel(AbilityId.NakMuay14), MaxAbilityLevel);

			if (abilityLevel <= 0)
				return;

			var minimumPhysicalAttack = character.Properties.GetFloat(PropertyName.MINPATK);
			var maximumPhysicalAttack = character.Properties.GetFloat(PropertyName.MAXPATK);
			var physicalAttack = (minimumPhysicalAttack + maximumPhysicalAttack) / 2f;
			var additionalDamage = physicalAttack * AdditionalDamageRatePerLevel * abilityLevel;

			skillHitResult.Damage += additionalDamage;
		}

		/// <summary>
		/// Returns whether the skill is a Nak Muay basic attack or damaging skill.
		/// </summary>
		private bool IsNakMuayAttackSkill(Skill skill)
		{
			return
				skill.Id == SkillId.NakMuay_Attack ||
				skill.Id == SkillId.NakMuay_Attack2 ||
				skill.Id == SkillId.NakMuay_TeKha ||
				skill.Id == SkillId.NakMuay_SokChiang ||
				skill.Id == SkillId.NakMuay_TeTrong ||
				skill.Id == SkillId.NakMuay_KhaoLoi;
		}
	}
}
