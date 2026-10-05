using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.Druid
{
	public static class Druid_TypeSpecialityHelper
	{
		private const int MaximumLevel = 10;
		private const float DamageBonusPerLevel = 0.01f;

		public static void ApplySizeBonus(ICombatEntity attacker, ICombatEntity target, AbilityId abilityId, SizeType requiredSize, SkillModifier modifier)
		{
			if (attacker == null || target is not Mob || !attacker.TryGetActiveAbility(abilityId, out var ability))
				return;

			if (!Enum.TryParse<SizeType>(target.Properties.GetString(PropertyName.Size), true, out var targetSize))
				return;

			if (targetSize != requiredSize)
				return;

			var abilityLevel = Math.Clamp(ability.Level, 1, MaximumLevel);
			modifier.DamageMultiplier *= 1f + abilityLevel * DamageBonusPerLevel;
		}

		public static void ApplyRaceBonus(ICombatEntity attacker, ICombatEntity target, AbilityId abilityId, RaceType requiredRace, SkillModifier modifier)
		{
			if (attacker == null || target is not Mob || !attacker.TryGetActiveAbility(abilityId, out var ability))
				return;

			if (!Enum.TryParse<RaceType>(target.Properties.GetString(PropertyName.RaceType), true, out var targetRace))
				return;

			if (targetRace != requiredRace)
				return;

			var abilityLevel = Math.Clamp(ability.Level, 1, MaximumLevel);
			modifier.DamageMultiplier *= 1f + abilityLevel * DamageBonusPerLevel;
		}
	}

	[Package("laima")]
	[AbilityHandler(AbilityId.Druid2)]
	public class Druid_SmallTypeSpecialityAbility : IAbilityHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, AbilityId.Druid2)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			Druid_TypeSpecialityHelper.ApplySizeBonus(attacker, target, AbilityId.Druid2, SizeType.S, modifier);
		}
	}

	[Package("laima")]
	[AbilityHandler(AbilityId.Druid3)]
	public class Druid_MediumTypeSpecialityAbility : IAbilityHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, AbilityId.Druid3)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			Druid_TypeSpecialityHelper.ApplySizeBonus(attacker, target, AbilityId.Druid3, SizeType.M, modifier);
		}
	}

	[Package("laima")]
	[AbilityHandler(AbilityId.Druid4)]
	public class Druid_LargeTypeSpecialityAbility : IAbilityHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, AbilityId.Druid4)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			Druid_TypeSpecialityHelper.ApplySizeBonus(attacker, target, AbilityId.Druid4, SizeType.L, modifier);
		}
	}

	[Package("laima")]
	[AbilityHandler(AbilityId.Druid5)]
	public class Druid_AnimalTypeSpecialityAbility : IAbilityHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, AbilityId.Druid5)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			Druid_TypeSpecialityHelper.ApplyRaceBonus(attacker, target, AbilityId.Druid5, RaceType.Widling, modifier);
		}
	}

	[Package("laima")]
	[AbilityHandler(AbilityId.Druid6)]
	public class Druid_PlantTypeSpecialityAbility : IAbilityHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, AbilityId.Druid6)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			Druid_TypeSpecialityHelper.ApplyRaceBonus(attacker, target, AbilityId.Druid6, RaceType.Forester, modifier);
		}
	}

	[Package("laima")]
	[AbilityHandler(AbilityId.Druid7)]
	public class Druid_InsectTypeSpecialityAbility : IAbilityHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, AbilityId.Druid7)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			Druid_TypeSpecialityHelper.ApplyRaceBonus(attacker, target, AbilityId.Druid7, RaceType.Klaida, modifier);
		}
	}
}
