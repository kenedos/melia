using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the CounterSpell buff, which reduces incoming magic
	/// damage while active.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.CounterSpell_Buff)]
	public class Oracle_CounterSpell_BuffOverride : BuffHandler
	{
		private const float ReductionBase = 0.05f;
		private const float ReductionPerLevel = 0.02f;
		private const float MaxReduction = 0.9f;

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc_Defense, BuffId.CounterSpell_Buff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.CounterSpell_Buff, out var buff))
				return;

			if (skill?.Data?.AttackType != SkillAttackType.Magic)
				return;

			var reduction = MathF.Min(MaxReduction, ReductionBase + ReductionPerLevel * buff.NumArg1);

			modifier.DamageMultiplier *= 1f - reduction;
		}
	}
}
