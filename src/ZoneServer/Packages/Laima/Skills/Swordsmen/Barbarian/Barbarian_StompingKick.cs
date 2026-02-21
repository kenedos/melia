using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Skills.Handlers.Barbarian
{
	/// <summary>
	/// Handler for the Barbarian skill Stomping Kick.
	/// Per the rework, this skill has the Barbarian stomp the ground upon landing,
	/// knocking back all surrounding enemies.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Barbarian_StompingKick)]
	public class Barbarian_StompingKickOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handles the initial casting of the skill.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			// Check for plate armor restriction
			if (caster is Character character && character.IsWearingArmorOfType(ArmorMaterialType.Iron))
			{
				caster.ServerMessage(Localization.Get("Can't use while wearing [Plate] armor."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			// Adjust the target position to ground level
			if (caster.Map.Ground.TryGetHeightAt(farPos, out var groundHeight))
				farPos.Y = groundHeight;

			// Define the circular area of effect based on skill data.
			var splashParam = skill.GetSplashParameters(caster, farPos, farPos, length: 0, width: 60, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);

			// Send client-side visuals.
			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			// Asynchronously call the Attack logic.
			skill.Run(this.Attack(skill, caster, splashArea));
		}

		/// <summary>
		/// Executes the actual attack after the caster has landed.
		/// </summary>
		private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			// --- Core Mechanic: Wait until the Barbarian lands from their jump ---
			await this.WaitUntilGrounded(skill, caster);

			var allTargetsInArea = caster.Map.GetAttackableEnemiesIn(caster, splashArea).ToList();

			if (!allTargetsInArea.Any())
				return;

			// Limit the number of targets by the skill's AoE Attack Ratio.
			var affectedTargets = allTargetsInArea.LimitBySDR(caster, skill);

			var hits = new List<SkillHitInfo>();
			var damageDelay = TimeSpan.FromMilliseconds(100);

			// Process all targets - damage, knockback, and stun chance for everyone
			foreach (var target in affectedTargets)
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill, SkillModifier.Default);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, TimeSpan.Zero);

				if (skillHitResult.Damage > 0 && target.IsKnockdownable())
				{
					skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, target, HitType.KnockBack, 120, 10);
					skillHit.HitInfo.Type = HitType.KnockBack;
					target.ApplyKnockback(caster, skill, skillHit);
				}

				hits.Add(skillHit);

				// Apply stun chance
				this.TryApplyStun(skill, caster, target, skillHitResult);
			}

			// Send all hit information to the client.
			Send.ZC_SKILL_HIT_INFO(caster, hits);

			// Optional: Keep the re-jump ability for added mobility.
			if (caster.IsAbilityActive(AbilityId.Barbarian31))
			{
				await skill.Wait(TimeSpan.FromMilliseconds(100));
				if (caster.Components.TryGet<MovementComponent>(out var movementComponent))
				{
					movementComponent.NotifyJump(caster.Position, caster.Direction, 0, 0);
				}
			}
		}

		/// <summary>
		/// Returns as soon as the caster is grounded.
		/// </summary>
		private async Task WaitUntilGrounded(Skill skill, ICombatEntity caster)
		{
			if (!caster.Components.TryGet<MovementComponent>(out var movementComponent))
				return;

			while (!movementComponent.IsGrounded)
				await skill.Wait(TimeSpan.FromMilliseconds(30));
		}

		/// <summary>
		/// Applies stun with (20 + SkillLv * 3)% probability
		/// </summary>
		private void TryApplyStun(Skill skill, ICombatEntity caster, ICombatEntity target, SkillHitResult skillHitResult)
		{
			// Only apply stun if damage was dealt
			if (skillHitResult.Damage <= 0)
				return;

			// Calculate stun chance: 20 + (SkillLv * 3)%
			var stunChance = 20 + (skill.Level * 3);

			// Roll for stun (0-99, check if less than stunChance)
			if (RandomProvider.Get().Next(100) < stunChance)
			{
				target.StartBuff(BuffId.Stun, skill.Level, 0, TimeSpan.FromSeconds(2), caster);
			}
		}
	}
}
