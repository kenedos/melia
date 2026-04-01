using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Laima.Monster
{
	/// <summary>
	/// Handler for Mythic_Boosting_Morale_Atk_Buff.
	/// Applied to nearby monsters by Mythic_Boosting_Morale_Buff leader.
	/// Increases damage dealt by 30% and reduces damage taken by 50%.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Mythic_Boosting_Morale_Atk_Buff)]
	public class Mythic_Boosting_Morale_Atk_BuffOverride : BuffHandler
	{
		private const float AtkBoostRate = 0.50f;
		private const float DefBoostRate = 0.50f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		/// <summary>
		/// Increases outgoing damage based on mythic_morale_atk_rate config.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Mythic_Boosting_Morale_Atk_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.Mythic_Boosting_Morale_Atk_Buff, out _))
				return;

			skillHitResult.Damage *= 1f + AtkBoostRate;
		}

		/// <summary>
		/// Reduces incoming damage based on mythic_morale_def_rate config.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Mythic_Boosting_Morale_Atk_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Mythic_Boosting_Morale_Atk_Buff, out _))
				return;

			skillHitResult.Damage *= 1f - DefBoostRate;
		}
	}
}
