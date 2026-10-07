using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Zealot
{
	/// <summary>
	/// Handler for the Fanaticism buff, which raises the Zealot's damage.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Fanaticism_Buff)]
	public class Zealot_Fanaticism_BuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Fanaticism_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.Fanaticism_Buff, out var buff))
				return;

			modifier.DamageMultiplier += GetCaptionRatio(buff, 1) / 100f;
		}
	}
}
