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
	[BuffHandler(BuffId.BlindFaith_Debuff)]
	public class BlindFaith_DebuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.BlindFaith_Debuff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.BlindFaith_Debuff, out var buff))
				return;

			modifier.DamageMultiplier += buff.NumArg2;
		}
	}
}
