using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Schwarzer Reiter skill Retreat Shot.
	/// SkillId: 51004
	/// ClassName: Schwarzereiter_RetreatShot
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Schwarzereiter_RetreatShot)]
	public class SchwarzerReiter_RetreatShotOverride : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character || !character.IsRiding)
			{
				caster.ServerMessage(Localization.Get("You must be mounted on a companion."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			// Apply the Retreat Shot buff for the skill duration.
			caster.StartBuff(
				BuffId.RetreatShot,
				skill.Level,
				0f,
				TimeSpan.FromMilliseconds(10000),
				caster,
				skill.Id);

			// Start the periodic damage loop.
			skill.Run(this.Attack(skill, caster, character));
		}

		private async Task Attack(Skill skill, ICombatEntity caster, Character character)
		{
			// Check if [Arts] Retreat Shot: Enhanced Upgrade is active.
			// The ability handler itself should only act as a marker.
			var hasEnhancedUpgrade = character.IsAbilityActive(AbilityId.Schwarzereiter23);

			// Retreat Shot performs repeated attacks during the full skill duration.
			// Enhanced Upgrade increases the number of processed hits.
			var hitCount = hasEnhancedUpgrade ? 40 : 33;

			var totalDuration = TimeSpan.FromMilliseconds(10000);
			var firstHitDelay = TimeSpan.FromMilliseconds(250);

			// Spread all hits evenly across the skill duration.
			var delayBetweenHits = TimeSpan.FromMilliseconds(
				(totalDuration.TotalMilliseconds - firstHitDelay.TotalMilliseconds) / hitCount
			);

			var aniTime = TimeSpan.FromMilliseconds(270);
			var skillHitDelay = TimeSpan.Zero;

			// Enhanced Upgrade increases the final damage of each hit.
			var artsDamageMultiplier = hasEnhancedUpgrade ? 1.3f : 1f;

			await skill.Wait(firstHitDelay);

			for (var i = 0; i < hitCount; i++)
			{
				// Stop the loop if the Retreat Shot buff was removed.
				if (!caster.TryGetBuff(BuffId.RetreatShot, out _))
					break;

				// Retreat Shot should stop if the character dismounts.
				if (!character.IsRiding)
				{
					caster.StopBuff(BuffId.RetreatShot);
					break;
				}

				// Find the closest valid target behind the caster.
				var target = this.GetTargetBehind(caster);

				if (target != null)
				{
					// Calculate damage using the default Melia combat formula.
					var skillHitResult = SCR_SkillHit(caster, target, skill);

					// Apply [Arts] Retreat Shot: Enhanced Upgrade damage bonus.
					skillHitResult.Damage *= artsDamageMultiplier;

					// Apply the calculated damage to the target.
					target.TakeDamage(skillHitResult.Damage, caster);

					// Send hit information to the client.
					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, aniTime, skillHitDelay);
					Send.ZC_SKILL_HIT_INFO(caster, skillHit);
				}

				if (i + 1 < hitCount)
					await skill.Wait(delayBetweenHits);
			}

			caster.SetAttackState(false);
			caster.StopBuff(BuffId.RetreatShot);
		}

		private ICombatEntity GetTargetBehind(ICombatEntity caster)
		{
			var splashRadius = 180;
			var distanceBehind = 180;

			// Create a point behind the caster.
			// This makes Retreat Shot behave like Assault Fire, but in the opposite direction.
			var behindPos = caster.Position.GetRelative(caster.Direction, -distanceBehind);

			// Search enemies around the point behind the caster.
			var splashArea = new Circle(behindPos, splashRadius);

			return caster.Map
				.GetAttackableEnemiesIn(caster, splashArea)
				.Where(target => target != null && !target.IsDead)
				.OrderBy(target => target.Position.Get2DDistance(behindPos))
				.FirstOrDefault();
		}
	}
}
