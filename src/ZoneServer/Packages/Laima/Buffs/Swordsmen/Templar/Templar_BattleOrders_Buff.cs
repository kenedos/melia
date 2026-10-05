using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Templar
{
	[Package("laima")]
	[BuffHandler(BuffId.BattleOrders_Buff)]
	public class Templar_BattleOrders_BuffOverride : BuffHandler
	{
		private const float MinimumFinalDamageBonus = 0.10f;
		private const float MaximumFinalDamageBonus = 0.20f;
		private const float EnhancePerLevel = 0.005f;
		private const float MaximumEnhanceLevelBonus = 0.10f;
		private const int MaximumSkillLevel = 10;
		private const int MaximumEnhanceLevel = 100;

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.BattleOrders_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.BattleOrders_Buff, out var buff))
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, 1, MaximumSkillLevel);
			var bonusPerLevel = (MaximumFinalDamageBonus - MinimumFinalDamageBonus) / (MaximumSkillLevel - 1);
			var finalDamageBonus = MinimumFinalDamageBonus + (skillLevel - 1) * bonusPerLevel;

			finalDamageBonus *= this.GetEnhanceMultiplier(buff);
			modifier.FinalDamageMultiplier *= 1f + finalDamageBonus;
		}

		private float GetEnhanceMultiplier(Buff buff)
		{
			if (buff.Caster is not Character caster)
				return 1f;

			if (!caster.Abilities.TryGet(AbilityId.Templar6, out var ability) || !ability.Active)
				return 1f;

			var abilityLevel = Math.Clamp(ability.Level, 0, MaximumEnhanceLevel);
			var enhanceRate = abilityLevel * EnhancePerLevel;

			if (abilityLevel >= MaximumEnhanceLevel)
				enhanceRate += MaximumEnhanceLevelBonus;

			return 1f + enhanceRate;
		}
	}
}
