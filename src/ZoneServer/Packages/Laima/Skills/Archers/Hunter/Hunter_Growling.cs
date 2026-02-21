using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Components;
using Yggdrasil.Util;

namespace Melia.Zone.Skills.Handlers.Hunter
{
	/// <summary>
	/// Handler for the Hunter skill Growling.
	/// Passive skill that makes companions periodically apply fear debuff to nearby enemies.
	/// Also increases companion's attack damage based on skill factor.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Hunter_Growling)]
	public class Hunter_GrowlingOverride : IPassiveSkillHandler, ISkillCombatCompanionAttackAfterBonusesHandler
	{
		private const string GrowlingEventId = "Hunter.Growling.Fear";
		private const float GrowlingRange = 100f;
		private const int BaseInterval = 10000;
		private const int IntervalReductionPerLevel = 500;
		private const int MinimumInterval = 3000;

		/// <summary>
		/// Called when the skill is learned or activated.
		/// Sets up the periodic growling behavior on the owner's companion.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster)
		{
			if (!caster.TryGetActiveCompanion(out var companion))
				return;

			this.SetupGrowlingEvent(companion, skill.Level);
		}

		/// <summary>
		/// Sets up the periodic growling event on a companion.
		/// </summary>
		private void SetupGrowlingEvent(Companion companion, int skillLevel)
		{
			if (!companion.Components.TryGet<TimedEventComponent>(out var timedEvents))
			{
				timedEvents = new TimedEventComponent(companion);
				companion.Components.Add(timedEvents);
			}

			var interval = this.GetGrowlingInterval(skillLevel);
			var intervalTimeSpan = TimeSpan.FromMilliseconds(interval);

			timedEvents.Remove(GrowlingEventId);
			timedEvents.Add(
				startDelay: intervalTimeSpan,
				repeatDelay: intervalTimeSpan,
				repeatCount: -1,
				eventId: GrowlingEventId,
				function: this.ApplyGrowlingFear
			);
		}

		/// <summary>
		/// Calculates the growling interval based on skill level.
		/// Formula: 10 - 0.5 * SkillLevel seconds (minimum 3 seconds)
		/// </summary>
		private int GetGrowlingInterval(int skillLevel)
		{
			var interval = BaseInterval - (skillLevel * IntervalReductionPerLevel);
			return Math.Max(MinimumInterval, interval);
		}

		/// <summary>
		/// Applies the fear debuff to 1-3 nearby enemies randomly.
		/// 20% chance for 3 targets, 30% for 2 targets, 50% for 1 target.
		/// Hunter24 ability disables this effect.
		/// </summary>
		private void ApplyGrowlingFear(ICombatEntity entity)
		{
			if (entity is not Companion companion)
				return;

			if (!companion.IsActivated || companion.Owner == null)
				return;

			var owner = companion.Owner;

			// Hunter24: Growling no longer applies fear
			if (owner.IsAbilityActive(AbilityId.Hunter24))
				return;

			if (!owner.TryGetSkill(SkillId.Hunter_Growling, out var growlingSkill))
				return;

			if (!companion.CombatState.AttackState)
				return;

			var allEnemies = companion.Map.GetAttackableEnemiesInPosition(companion, companion.Position, GrowlingRange);

			if (allEnemies.Count == 0)
				return;

			var rnd = RandomProvider.Get();
			var roll = rnd.NextDouble();
			int targetCount;

			if (roll < 0.20)
				targetCount = 3;
			else if (roll < 0.50)
				targetCount = 2;
			else
				targetCount = 1;

			targetCount = Math.Min(targetCount, allEnemies.Count);

			var selectedTargets = allEnemies
				.Take(targetCount)
				.ToList();

			foreach (var target in selectedTargets)
			{
				target.StartBuff(BuffId.Growling_fear_Debuff, growlingSkill.Level, 0, TimeSpan.FromSeconds(3), companion);
			}
		}

		/// <summary>
		/// Called after bonuses are calculated during combat when a companion attacks.
		/// Increases companion damage based on Growling skill factor.
		/// Hunter15/Hunter35 abilities are applied via reinforceAbility in skill data.
		/// Hunter24 ability increases the damage bonus by 50%.
		/// </summary>
		public void OnCompanionAttackAfterBonuses(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			// Use the calculated skill factor which includes reinforceAbility bonuses
			var skillFactor = skill.Properties.GetFloat(PropertyName.SkillFactor);
			var damageIncrease = skillHitResult.Damage * (skillFactor / 100f);

			// Hunter24: Increases companion damage from Growling by 50%
			if (attacker.TryGetOwner(out var owner) && owner.IsAbilityActive(AbilityId.Hunter24))
				damageIncrease *= 1.5f;

			skillHitResult.Damage += damageIncrease;
		}
	}
}
