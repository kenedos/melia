using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Handlers.Swordsman.Barbarian;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Barbarian
{
	/// <summary>
	/// Handler for the passive Barbarian skill Frenzy.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Barbarian_Frenzy)]
	public class Barbarian_FrenzyOverride : ISkillHandler, ISkillCombatDefenseBeforeCalcHandler, ISkillCombatAttackBeforeCalcHandler
	{
		/// <summary>
		/// Handles the offensive aspects of the Barbarian's passives.
		/// 1. Applies the Frenzy damage bonus (+5% per stack).
		/// 2. Applies the non-plate armor bonus (+50% damage) to specific skills.
		/// </summary>
		public void OnAttackBeforeCalc(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker is not Character character)
				return;

			// Apply the +5% damage bonus per Frenzy stack
			if (character.TryGetBuff(BuffId.Frenzy_Buff, out var frenzyBuff))
			{
				modifier.DamageMultiplier += frenzyBuff.OverbuffCounter * 0.05f;
			}
		}

		public void OnDefenseBeforeCalc(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target != skill.Owner)
				return;

			var frenzySkillLevel = skill.Level;


			// Calculate max stacks: 6 + (SkillLevel - 1) * (14.0 / 9.0)
			// Lv 1: 6 stacks, Lv 10: 20 stacks
			var maxStacks = 6;
			if (target.TryGetSkill(SkillId.Barbarian_Frenzy, out var frenzySkill))
			{
				maxStacks = (int)Math.Round(6 + (frenzySkill.Level - 1) * (14.0 / 9.0));
				maxStacks = Math.Max(6, Math.Min(20, maxStacks));
			}

			var buffInstanceDuration = TimeSpan.FromSeconds(10);

			// Gain a stack on every hit
			target.StartBuff(BuffId.Frenzy_Buff, maxStacks, 1, buffInstanceDuration, skill.Owner);
		}
	}
}
