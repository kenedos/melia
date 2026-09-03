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
	/// Handle for the CounterSpell buff, which nullifies incoming magic
	/// damage while active.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.CounterSpell_Buff)]
	public class Oracle_CounterSpell_BuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.CounterSpell_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.IsBuffActive(BuffId.CounterSpell_Buff))
				return;

			if (skill?.Data?.AttackType != SkillAttackType.Magic && skill?.Data?.ClassType != SkillClassType.Magic)
				return;

			skillHitResult.Damage = 0;
			skillHitResult.Effect = HitEffect.SAFETY;
			skillHitResult.Result = HitResultType.Miss;
		}
	}
}
