using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Exorcist
{
	/// <summary>
	/// Handler for Gregorate: Magic, which raises the damage the marked
	/// enemy takes from the Exorcist's skills.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.GregorateATK_Buff)]
	public class Exorcist_GregorateATK_BuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.GregorateATK_Buff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.GregorateATK_Buff, out var buff) || buff.Caster != attacker)
				return;

			if (!skill.Data.ClassName.StartsWith("Exorcist_", StringComparison.Ordinal))
				return;

			modifier.DamageMultiplier += GetCaptionRatio(buff, 2) / 100f;
		}
	}
}
