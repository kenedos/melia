using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Zealot
{
	[Package("laima")]
	[BuffHandler(BuffId.BeadyEyed_Debuff)]
	public class BeadyEyed_DebuffOverride : BuffHandler
	{
		private const float MinimumCriticalChanceBonus = 0.15f;

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.BeadyEyed_Debuff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.BeadyEyed_Debuff, out _))
				return;

			modifier.MinCritChance += MinimumCriticalChanceBonus;
		}
	}
}
