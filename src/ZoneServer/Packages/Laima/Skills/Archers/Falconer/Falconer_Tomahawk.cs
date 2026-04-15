using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handler for the Falconer skill Tomahawk.
	/// Commands the hawk to perform a high-altitude dive at a target
	/// location, dealing multi-hit damage to enemies in the area.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Falconer_Tomahawk)]
	public class Falconer_TomahawkOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float AttackRadius = 80f;
		private const int MaxTargets = 10;
		private const int BaseHitCount = 3;
		private const float PenetrateHeight = 50f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!FalconerHawkHelper.TryGetHawk(caster, out var hawk))
			{
				if (caster is Character character)
					character.SystemMessage("CompanionIsNotActive");
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(skill, caster, hawk, targetPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Companion hawk, Position targetPos)
		{
			// Interrupt hawk if busy
			if (FalconerHawkHelper.IsHawkBusy(hawk))
			{
				hawk.Vars.Set("Hawk.UsingSkill", false);
				await skill.Wait(TimeSpan.FromMilliseconds(100));
			}

			// Unhide hawk if it flew away from a previous skill
			if (FalconerHawkHelper.IsHawkFlyingAway(hawk))
				await FalconerHawkHelper.HawkUnhide(skill, caster, hawk);

			// Lock hawk action
			hawk.Vars.Set("Hawk.UsingSkill", true);
			hawk.Vars.Set("Hawk.SkillFunction", "Tomahawk");

			// Take off from shoulder or leave roost
			if (hawk.IsLandedOnShoulder)
				hawk.TakeOff();
			else if (hawk.IsOnRoost)
				hawk.LeaveRoost();

			await skill.Wait(TimeSpan.FromMilliseconds(100));

			// TODO: Find correct ground effect at target position
			// hawk.PlayGroundEffect(targetPos, "F_archer_caltrop_hit_explosion", 3f);

			// Screen shake
			Send.ZC_CHANGE_CAMERA_ZOOM(hawk, 2, 99999f, 7f, 0.5f, 50f, 0f, 0f);

			// Hawk dive to target position with high penetration
			var divePos = new Position(targetPos.X, targetPos.Y + FalconerHawkHelper.DefaultHawkHeight, targetPos.Z);
			var syncKey = hawk.GenerateSyncKey();
			Send.ZC_NORMAL.PenetratePosition(hawk, divePos, PenetrateHeight, syncKey, "TOMAHAWK_SHOT", 0.7f, 7f, 0.5f, 0.7f, 30f);

			// Wait for hawk to reach target
			await skill.Wait(TimeSpan.FromMilliseconds(700));

			// Apply damage to enemies in area
			var enemies = caster.Map.GetAttackableEnemiesInPosition(caster, targetPos, AttackRadius)
				.Take(MaxTargets)
				.ToList();

			if (enemies.Count > 0)
			{
				foreach (var enemy in enemies)
				{
					if (enemy.IsDead)
						continue;

					// Multi-hit damage (3 hits)
					var skillHitResult = SCR_SkillHit(caster, enemy, skill, SkillModifier.MultiHit(BaseHitCount));
					enemy.TakeDamage(skillHitResult.Damage, caster);

					var hit = new HitInfo(caster, enemy, skill, skillHitResult, HitResultType.Hit);
					Send.ZC_HIT_INFO(caster, enemy, hit);

					// Falconer27: [Arts] Tomahawk: Scorched - applies burn
					if (caster.IsAbilityActive(AbilityId.Falconer27))
					{
						enemy.StartBuff(BuffId.Burn, skill.Level, 0, TimeSpan.FromSeconds(5), caster);
					}
				}
			}

			// Wait for hawk return animation
			await skill.Wait(TimeSpan.FromMilliseconds(500));

			// Cleanup
			hawk.Vars.Set("Hawk.UsingSkill", false);
			hawk.Vars.Set("Hawk.SkillFunction", "None");

			// Hawk flies away after attack
			await FalconerHawkHelper.HawkFlyAway(skill, caster, hawk);
		}
	}
}
