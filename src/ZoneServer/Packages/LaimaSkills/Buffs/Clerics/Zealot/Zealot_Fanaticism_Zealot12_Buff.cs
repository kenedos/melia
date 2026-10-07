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
	/// Handler for Price of Sacrifice, which raises damage by 10% per stack.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Fanaticism_Zealot12_Buff)]
	public class Zealot_Fanaticism_Zealot12_BuffOverride : BuffHandler
	{
		private const float DamageRatePerStack = 0.10f;

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Fanaticism_Zealot12_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.Fanaticism_Zealot12_Buff, out var buff))
				return;

			modifier.DamageMultiplier += buff.OverbuffCounter * DamageRatePerStack;
		}
	}
}
