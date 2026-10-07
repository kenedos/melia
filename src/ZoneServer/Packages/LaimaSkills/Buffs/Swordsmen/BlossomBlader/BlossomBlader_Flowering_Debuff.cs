using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Handler for Flowering, which lowers the target's evasion against the
	/// Blossom Blader by the skill's ratio in percent per stack.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Flowering_Debuff)]
	public class BlossomBlader_Flowering_DebuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Flowering_Debuff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Flowering_Debuff, out var buff) || buff.Caster != attacker)
				return;

			modifier.HitRateMultiplier += GetCaptionRatio(buff, 1) * buff.OverbuffCounter / 100f;
		}
	}
}
