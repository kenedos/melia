using System;
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
	/// Schwarzer Reiter: Long-ranged Shot ability, which extends the basic
	/// attack's range, at a miss chance that grows with the distance.
	/// </summary>
	[Package("laima-skills")]
	[AbilityHandler(AbilityId.Schwarzereiter31)]
	public class Schwarzereiter31Override : IAbilityPropertyHandler
	{
		private const float BonusRange = 100f;
		private const float MaxMissRate = 0.50f;

		public void OnActivate(Ability ability, Character character)
		{
			character.StartBuff(BuffId.Schwarzereiter_MaxR_Buff, TimeSpan.Zero);
		}

		public void OnDeactivate(Ability ability, Character character)
		{
			character.StopBuff(BuffId.Schwarzereiter_MaxR_Buff);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, AbilityId.Schwarzereiter31)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!skill.IsNormalAttack || !attacker.IsAbilityActive(AbilityId.Schwarzereiter31))
				return;

			var extraDistance = (float)attacker.Position.Get2DDistance(target.Position) - skill.Data.MaxRange;
			if (extraDistance <= 0)
				return;

			var missRate = MaxMissRate * Math.Min(1f, extraDistance / BonusRange);
			modifier.HitRateMultiplier *= 1f - missRate;
		}
	}
}
