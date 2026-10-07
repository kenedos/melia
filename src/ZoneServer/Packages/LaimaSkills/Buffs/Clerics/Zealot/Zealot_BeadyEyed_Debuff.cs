using System;
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
	/// Handler for Beady Eyed: Sudden Attack, which gives the Zealot a 15%
	/// minimum critical chance after slipping behind an enemy.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.BeadyEyed_Debuff)]
	public class Zealot_BeadyEyed_DebuffOverride : BuffHandler
	{
		private const float MinCritChance = 15f;

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.BeadyEyed_Debuff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.IsBuffActive(BuffId.BeadyEyed_Debuff))
				return;

			modifier.MinCritChance = Math.Max(modifier.MinCritChance, MinCritChance);
		}
	}
}
