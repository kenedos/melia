using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Divine Stigma, damage taken increased.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DivineStigma_Debuff)]
	public class DivineStigma_DebuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.DivineStigma_Debuff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.DivineStigma_Debuff, out var buff))
				return;

			var damageBonus = buff.NumArg2;

			modifier.DamageMultiplier += damageBonus;
		}
	}
}
