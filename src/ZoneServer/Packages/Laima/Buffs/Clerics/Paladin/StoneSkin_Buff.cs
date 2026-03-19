using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Shared.Data.Database;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.HandlersOverrides.Clerics.Paladin
{
	/// <summary>
	/// Handler for the Stone Skin buff.
	/// Reduces physical damage taken.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.StoneSkin_Buff)]
	public class StoneSkin_BuffOverride : BuffHandler
	{
		private const float DamageReductionBase = 0.10f;
		private const float DamageReductionPerLevel = 0.01f;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.StoneSkin_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.StoneSkin_Buff, out var buff))
				return;

			// Only reduce physical damage (not magic)
			if (skill.Data.AttackType == SkillAttackType.Magic)
				return;

			var skillLevel = buff.NumArg1;
			var damageReduction = DamageReductionBase + (skillLevel * DamageReductionPerLevel);

			var byAbility = 1f;
			if (buff.Caster is Character character)
				if (character.TryGetActiveAbilityLevel(AbilityId.Paladin24, out var abilityLevel))
				byAbility += abilityLevel * 0.005f;
			damageReduction *= byAbility;

			// Apply damage reduction (Cap at 90%)
			skillHitResult.Damage *= Math.Max(0.1f, (1f - damageReduction));
		}
	}
}
