using System;
using System.Linq;
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
	/// Handler for the Schwarzer Reiter skill Concentrated Fire.
	/// SkillId: 51001
	/// ClassName: Schwarzereiter_ConcentratedFire
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Schwarzereiter_ConcentratedFire)]
	public class SchwarzerReiter_ConcentratedFireOverride : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character || !character.IsRiding)
			{
				caster.ServerMessage(Localization.Get("You must be mounted on a companion."));
				return;
			}

			if (!this.HasPistol(caster))
			{
				caster.ServerMessage(Localization.Get("A pistol is required."));
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

			// Create the splash area around the aimed position.
			// Concentrated Fire damages all enemies inside this area.
			var splashRadius = 100;
			var splashArea = new Circle(farPos, splashRadius);

			// Retrieve all valid enemies inside the aimed area.
			var targets = caster.Map
				.GetAttackableEnemiesIn(caster, splashArea)
				.Where(hitTarget => hitTarget != null && !hitTarget.IsDead)
				.ToList();

			// Check if [Arts] Concentrated Fire: Enhanced Upgrade is active.
			// The ability handler itself should only act as a marker.
			var hasEnhancedUpgrade = character.IsAbilityActive(AbilityId.Schwarzereiter20);

			// Concentrated Fire performs 10 rapid hits by default.
			// Enhanced Upgrade increases the number of hits.
			var hitCount = hasEnhancedUpgrade ? 15 : 10;

			// Enhanced Upgrade increases the final damage of each hit.
			var artsDamageMultiplier = hasEnhancedUpgrade ? 1.3f : 1f;

			// Keep the hit animation short because all hits are processed immediately.
			var aniTime = TimeSpan.FromMilliseconds(20);
			var skillHitDelay = TimeSpan.Zero;

			foreach (var hitTarget in targets)
			{
				for (var i = 0; i < hitCount; i++)
				{
					// Calculate one hit using the default Melia combat formula.
					var skillHitResult = SCR_SkillHit(caster, hitTarget, skill);

					// Apply [Arts] Concentrated Fire: Enhanced Upgrade damage bonus.
					skillHitResult.Damage *= artsDamageMultiplier;

					// Apply the calculated damage to the target.
					hitTarget.TakeDamage(skillHitResult.Damage, caster);

					// Build hit information for the client.
					var skillHit = new SkillHitInfo(
						caster,
						hitTarget,
						skill,
						skillHitResult,
						aniTime,
						skillHitDelay);

					// Send each hit separately to guarantee all hits are processed.
					Send.ZC_SKILL_FORCE_TARGET(caster, hitTarget, skill, skillHit);
				}
			}

			// End the attack state immediately after all hits are sent.
			caster.SetAttackState(false);
		}

		private bool HasPistol(ICombatEntity caster)
		{
			caster.TryGetEquipItem(EquipSlot.LeftHand, out var leftHandWeapon);
			caster.TryGetEquipItem(EquipSlot.RightHand, out var rightHandWeapon);

			return
				(leftHandWeapon != null && leftHandWeapon.Data.EquipType1 == EquipType.Pistol) ||
				(rightHandWeapon != null && rightHandWeapon.Data.EquipType1 == EquipType.Pistol);
		}
	}
}
