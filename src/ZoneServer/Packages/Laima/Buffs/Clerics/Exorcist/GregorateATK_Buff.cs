using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Exorcist
{
	[Package("laima")]
	[BuffHandler(BuffId.GregorateATK_Buff)]
	public class GregorateATK_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.GregorateATK_Buff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill == null || skill.Id != SkillId.Exorcist_Gregorate)
				return;

			if (!target.TryGetBuff(BuffId.GregorateATK_Buff, out var buff))
				return;

			modifier.DamageMultiplier *= 1f + buff.NumArg2;
		}
	}
}
