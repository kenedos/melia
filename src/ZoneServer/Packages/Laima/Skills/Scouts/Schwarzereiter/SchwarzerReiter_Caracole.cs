using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Schwarzer Reiter skill Caracole.
	/// SkillId: 51002
	/// ClassName: Schwarzereiter_Caracole
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Schwarzereiter_Caracole)]
	public class SchwarzerReiter_CaracoleOverride : IGroundSkillHandler
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

			// Check if [Arts] Caracole: Enhanced Upgrade is active.
			// The ability handler itself should only act as a marker.
			var hasEnhancedUpgrade = character.IsAbilityActive(AbilityId.Schwarzereiter21);

			// Create the splash area using the skill's own configured splash data.
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos);
			var splashArea = skill.GetSplashArea(skill.Data.SplashType, splashParam);

			// Get every valid enemy inside Caracole's configured area.
			var targets = caster.Map
				.GetAttackableEnemiesIn(caster, splashArea)
				.Where(hitTarget => hitTarget != null && !hitTarget.IsDead)
				.ToList();

			// Enhanced Upgrade increases the number of affected enemies.
			var maxTargets = hasEnhancedUpgrade ? 8 : 5;

			targets = targets
				.OrderBy(hitTarget => hitTarget.Position.Get2DDistance(farPos))
				.Take(maxTargets)
				.ToList();

			// Enhanced Upgrade increases the final damage of each hit.
			var artsDamageMultiplier = hasEnhancedUpgrade ? 1.3f : 1f;

			var skillHits = new List<SkillHitInfo>();

			foreach (var hitTarget in targets)
			{
				// Calculate damage using the default Melia combat formula.
				var skillHitResult = SCR_SkillHit(caster, hitTarget, skill);

				// Apply [Arts] Caracole: Enhanced Upgrade damage bonus.
				skillHitResult.Damage *= artsDamageMultiplier;

				// Apply the calculated damage to the target.
				hitTarget.TakeDamage(skillHitResult.Damage, caster);

				// Store hit information so all hits can be sent together.
				var skillHit = new SkillHitInfo(
					caster,
					hitTarget,
					skill,
					skillHitResult,
					TimeSpan.FromMilliseconds(270),
					TimeSpan.Zero);

				skillHits.Add(skillHit);
			}

			// Send the ground skill packet with all affected targets.
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, skillHits);

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
