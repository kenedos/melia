using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Doppelsoeldner: Tough ability, which lowers max stamina by 5 and
	/// raises the critical chance of Doppelsoeldner skills by 3% per level.
	/// </summary>
	/// <remarks>
	/// The +1 AoE Attack Ratio per level is applied in SCR_Get_SR_LV.
	/// </remarks>
	[Package("laima-skills")]
	[AbilityHandler(AbilityId.Doppelsoeldner24)]
	public class Doppelsoeldner24Override : AbilityPropertyHandler
	{
		private const float StaminaPenalty = 5f;
		private const float CritChancePerLevel = 3f;

		/// <summary>
		/// Lowers max stamina when the ability is activated.
		/// </summary>
		public override void OnActivate(Ability ability, Character character)
		{
			AddPropertyModifier(ability, character, PropertyName.MaxSta_BM, -StaminaPenalty);
		}

		/// <summary>
		/// Restores max stamina when the ability is deactivated.
		/// </summary>
		public override void OnDeactivate(Ability ability, Character character)
		{
			RemovePropertyModifier(ability, character, PropertyName.MaxSta_BM);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, AbilityId.Doppelsoeldner24)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!skill.Data.ClassName.StartsWith("Doppelsoeldner_"))
				return;

			if (!attacker.TryGetActiveAbilityLevel(AbilityId.Doppelsoeldner24, out var level))
				return;

			modifier.BonusCritChance += CritChancePerLevel * level;
		}
	}
}
