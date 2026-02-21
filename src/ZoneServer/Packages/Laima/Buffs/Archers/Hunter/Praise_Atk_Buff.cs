using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Hunter
{
	/// <summary>
	/// Handler for the Praise Attack Buff, which dramatically increases
	/// the companion's attack power and applies bleeding on attacks.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill level
	/// NumArg2: None
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Praise_Atk_Buff)]
	public class Praise_Atk_BuffOverride : BuffHandler, IBuffCombatAttackAfterCalcHandler
	{
		private const float FlatAtkBonus = 200f;
		private const float BaseAtkRate = 0.20f;
		private const float AtkRatePerLevel = 0.03f;
		private const float BaseSrBonus = 3f;
		private const float SrBonusPerLevel = 0.5f;
		private const int BleedingDurationSeconds = 8;
		private const int BleedingTickCount = 8;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var skillLevel = buff.NumArg1;

			// Hunter31: Increases Praise buff effectiveness by 0.5% per ability level
			var byAbility = 1f;
			if (buff.Caster is ICombatEntity caster && caster.TryGetActiveAbilityLevel(AbilityId.Hunter31, out var abilityLevel))
				byAbility += abilityLevel * 0.005f;

			// +200 ATK flat bonus
			AddPropertyModifier(buff, target, PropertyName.ATK_BM, FlatAtkBonus * byAbility);

			// +20% + 3% * SkillLv attack rate bonus
			var atkRate = BaseAtkRate + AtkRatePerLevel * skillLevel;
			var currentAtk = target.Properties.GetFloat(PropertyName.ATK);
			var atkRateBonus = currentAtk * atkRate * byAbility;
			AddPropertyModifier(buff, target, PropertyName.ATK_BM, atkRateBonus);

			// +3 + SkillLv/2 AoE Attack Ratio (Splash Rate)
			var srBonus = BaseSrBonus + skillLevel / 2f;
			AddPropertyModifier(buff, target, PropertyName.SR_BM, srBonus);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.ATK_BM);
			RemovePropertyModifier(buff, target, PropertyName.SR_BM);
		}

		/// <summary>
		/// Applies bleeding to targets when the companion attacks.
		/// Bleeding deals 100% of attack damage over 8 seconds.
		/// </summary>
		public void OnAttackAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skillHitResult.Damage <= 0)
				return;

			var skillLevel = buff.NumArg1;

			// Apply bleeding: 100% of attack damage spread over 8 ticks
			var bleedingDamagePerTick = skillHitResult.Damage / BleedingTickCount;
			target.StartBuff(BuffId.HeavyBleeding, skillLevel, bleedingDamagePerTick, TimeSpan.FromSeconds(BleedingDurationSeconds), attacker);
		}
	}
}
