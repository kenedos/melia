using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Zealot
{
	[Package("laima")]
	[BuffHandler(BuffId.Fanaticism_Buff)]
	public class Fanaticism_BuffOverride : BuffHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const float MinimumDamageBonus = 0.10f;
		private const float MaximumDamageBonus = 0.15f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Fanaticism_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.Fanaticism_Buff, out var buff))
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var bonusPerLevel = (MaximumDamageBonus - MinimumDamageBonus) / (MaximumSkillLevel - MinimumSkillLevel);
			var damageBonus = MinimumDamageBonus + (skillLevel - MinimumSkillLevel) * bonusPerLevel;

			modifier.DamageMultiplier += damageBonus;
		}
	}
}
