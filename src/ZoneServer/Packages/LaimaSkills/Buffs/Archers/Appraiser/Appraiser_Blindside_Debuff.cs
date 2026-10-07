using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Appraiser
{
	/// <summary>
	/// Handler for the Expose Weakness debuff, which gives attacks against
	/// the target a minimum critical chance.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Blindside_Debuff)]
	public class Appraiser_Blindside_DebuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Blindside_Debuff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Blindside_Debuff, out var buff))
				return;

			modifier.MinCritChance += GetCaptionRatio(buff, 2);
		}
	}
}
