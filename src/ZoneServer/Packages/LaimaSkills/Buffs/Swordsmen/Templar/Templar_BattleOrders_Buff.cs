using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Templar
{
	/// <summary>
	/// Handler for the Battle Orders buff on the Templar's party, which
	/// raises final damage.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.BattleOrders_Buff)]
	public class Templar_BattleOrders_BuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.BattleOrders_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.BattleOrders_Buff, out var buff))
				return;

			modifier.FinalDamageMultiplier += GetCaptionRatio(buff, 1) / 100f;
		}
	}
}
