using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Companion Mastery: Tenacity ability.
	/// Reduces magic damage received by the companion by 25%.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.CompMastery7)]
	public class CompMastery7Override : IAbilityHandler
	{
		[CombatCalcModifier(CombatCalcPhase.AfterCalc_CompanionDefense, AbilityId.CompMastery7)]
		public static void OnAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill?.Data == null)
				return;

			if (skill.Data.ClassType != SkillClassType.Magic)
				return;

			skillHitResult.Damage *= 0.75f;
		}
	}
}
