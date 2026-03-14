using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.HandlersOverrides.Wizards.Chronomancer
{
	[Package("laima")]
	[BuffHandler(BuffId.QuickCast_After_Buff)]
	public class QuickCast_After_BuffOverride : BuffHandler, IBuffCombatAttackBeforeCalcHandler
	{
		public void OnAttackBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			var abilLevel = (int)buff.NumArg1;
			modifier.DamageMultiplier += 0.02f * abilLevel;
		}
	}
}
