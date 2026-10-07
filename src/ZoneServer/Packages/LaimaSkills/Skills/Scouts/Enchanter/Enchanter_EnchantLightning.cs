using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Scouts.Enchanter
{
	/// <summary>
	/// Handler for the Enchanter passive skill Enchant Weapon, which raises
	/// the Enchanter's critical rate when attacking.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Enchanter_EnchantLightning)]
	public class Enchanter_EnchantLightningOverride : ISkillHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, SkillId.Enchanter_EnchantLightning)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetSkill(SkillId.Enchanter_EnchantLightning, out var enchantWeapon))
				return;

			modifier.CritRateMultiplier += enchantWeapon.Properties.GetFloat(PropertyName.CaptionRatio) / 100f;
		}
	}
}
