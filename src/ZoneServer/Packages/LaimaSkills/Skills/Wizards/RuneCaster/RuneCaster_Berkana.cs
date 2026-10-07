using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// Handler for the Rune Caster passive skill Rune of Beginning, which
	/// raises the final damage of Rune Caster skills.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.RuneCaster_Berkana)]
	public class RuneCaster_BerkanaOverride : ISkillHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, SkillId.RuneCaster_Berkana)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!skill.Data.ClassName.StartsWith("RuneCaster_") || !attacker.TryGetSkill(SkillId.RuneCaster_Berkana, out var berkana))
				return;

			modifier.FinalDamageMultiplier += berkana.Properties.GetFloat(PropertyName.CaptionRatio) / 100f;
		}
	}
}
