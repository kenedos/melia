using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Stores the minimum Critical Chance granted by Attaque Composee.
	/// The next offensive Fencer skill consumes this buff once and applies
	/// the stored value to its complete hit sequence.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.AttaqueComposee_Critical_Buff)]
	public class AttaqueComposee_Critical_BuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.AttaqueComposee_Critical_Buff)]
		public static void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill == null || (skill.Id != SkillId.Fencer_SeptEtoiles && skill.Id != SkillId.Fencer_Fleche))
				return;

			if (!attacker.TryGetBuff(BuffId.AttaqueComposee_Critical_Buff, out var buff))
				return;

			modifier.MinCritChance = Math.Max(modifier.MinCritChance, buff.NumArg2);
		}

		public static float ConsumeMinimumCriticalChance(ICombatEntity caster)
		{
			if (caster == null || !caster.TryGetBuff(BuffId.AttaqueComposee_Critical_Buff, out var buff))
				return 0f;

			var minimumCriticalChance = buff.NumArg2;
			caster.StopBuff(BuffId.AttaqueComposee_Critical_Buff);

			return minimumCriticalChance;
		}
	}
}
