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
	/// Handler for the Devaluation: Damage Received debuff, which raises
	/// the damage the target takes by 10%.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Devaluation_Damage_Debuff)]
	public class Appraiser_Devaluation_Damage_DebuffOverride : BuffHandler
	{
		private const float DamageIncrease = 0.10f;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Devaluation_Damage_Debuff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.IsBuffActive(BuffId.Devaluation_Damage_Debuff))
				return;

			skillHitResult.Damage *= 1f + DamageIncrease;
		}
	}
}
