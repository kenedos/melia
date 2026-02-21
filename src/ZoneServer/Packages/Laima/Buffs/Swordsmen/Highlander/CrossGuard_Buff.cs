using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters.Components;

namespace Melia.Zone.Buffs.Handlers.Swordsman.Highlander
{
	/// <summary>
	/// Handle for the Cross Guard Buff, which increases the target's block rate,
	/// but prevents evasion.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: None
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.CrossGuard_Buff)]
	public class CrossGuard_BuffOverride : BuffHandler, IBuffCombatDefenseBeforeCalcHandler
	{
		private const float PercentageEvasionBlkBonusPerLevel = 0.05f;
		private const float AbilityBonus = 0.005f;
		private const float BlkBonus = 200f;
		private const float BlkBonusPerLevel = 50f;
		private const float DebuffDuration = 5f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var evasion = buff.Target.Properties.GetFloat(PropertyName.DR);

			var baseValue = BlkBonus;
			var byLevel = BlkBonusPerLevel * buff.NumArg1;
			var byEvasion = PercentageEvasionBlkBonusPerLevel * buff.NumArg1 * evasion;

			var value = baseValue + byLevel + byEvasion;

			var byAbility = 1f;
			if (buff.Target.TryGetActiveAbilityLevel(AbilityId.Highlander15, out var abilityLevel))
				byAbility += abilityLevel * AbilityBonus;

			value *= byAbility;
			value += 1f;

			AddPropertyModifier(buff, buff.Target, PropertyName.BLK_BM, value);
			AddPropertyModifier(buff, buff.Target, PropertyName.DR_BM, -evasion);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.BLK_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_BM);
		}

		/// <summary>
		/// Applies the buff's effect during the combat calculations.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		public void OnDefenseBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			target.StartBuff(BuffId.CrossGuard_Damage_Buff, buff.NumArg1, 0, TimeSpan.FromSeconds(DebuffDuration), target);
		}
	}
}
