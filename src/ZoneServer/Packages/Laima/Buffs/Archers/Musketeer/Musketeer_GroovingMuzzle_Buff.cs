using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Musketeer
{
	/// <summary>
	/// Handler for the Enhance Rifle buff, which raises the final damage of
	/// the next Musketeer attack skill and is consumed by it.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.GroovingMuzzle_UseStack_Buff)]
	public class Musketeer_GroovingMuzzle_BuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.GroovingMuzzle_UseStack_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!IsMusketeerAttackSkill(skill))
				return;

			if (!attacker.TryGetBuff(BuffId.GroovingMuzzle_UseStack_Buff, out var buff))
				return;

			skillHitResult.Damage *= 1f + GetCaptionRatio(buff, 1) / 100f;

			attacker.StopBuff(BuffId.GroovingMuzzle_UseStack_Buff);
		}

		/// <summary>
		/// Returns whether the skill is one of the Musketeer's attack skills.
		/// </summary>
		/// <param name="skill"></param>
		private static bool IsMusketeerAttackSkill(Skill skill)
		{
			return skill.Data.ClassName.StartsWith("Musketeer_", StringComparison.Ordinal);
		}
	}
}
