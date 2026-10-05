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
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Schwarzereiter_Caracole)]
	public class SchwarzerReiter_CaracoleOverride : IGroundSkillHandler
	{
		private const int NormalMaximumTargets = 5;
		private const int EnhancedMaximumTargets = 8;
		private const float EnhancedDamageMultiplier = 1.30f;
		private const float AccuracyReductionRate = 0.25f;
		private static readonly TimeSpan DebuffDuration = TimeSpan.FromSeconds(10);

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

			var hasEnhancedUpgrade = character.IsAbilityActive(AbilityId.Schwarzereiter21);
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos);
			var splashArea = skill.GetSplashArea(skill.Data.SplashType, splashParam);

			var maxTargets = hasEnhancedUpgrade ? EnhancedMaximumTargets : NormalMaximumTargets;
			var damageMultiplier = hasEnhancedUpgrade ? EnhancedDamageMultiplier : 1f;

			var targets = caster.Map
				.GetAttackableEnemiesIn(caster, splashArea)
				.Where(hitTarget => hitTarget != null && !hitTarget.IsDead)
				.OrderBy(hitTarget => hitTarget.Position.Get2DDistance(farPos))
				.Take(maxTargets)
				.ToList();

			var skillHits = new List<SkillHitInfo>();

			foreach (var hitTarget in targets)
			{
				var skillHitResult = SCR_SkillHit(caster, hitTarget, skill);
				skillHitResult.Damage *= damageMultiplier;

				if (skillHitResult.Result != HitResultType.Dodge)
				{
					hitTarget.StartBuff(BuffId.Caracole_Silence_Debuff, skill.Level, 0f, DebuffDuration, caster, skill.Id);
					hitTarget.StartBuff(BuffId.Caracole_HR_Debuff, skill.Level, AccuracyReductionRate, DebuffDuration, caster, skill.Id);
				}

				if (skillHitResult.Damage > 0)
					hitTarget.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, hitTarget, skill, skillHitResult, TimeSpan.FromMilliseconds(270), TimeSpan.Zero);
				skillHits.Add(skillHit);
			}

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
