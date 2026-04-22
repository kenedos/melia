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
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handler for the Falconer skill Pheasant.
	/// Creates a decoy pheasant that draws enemy attention and aggro.
	/// The hawk then attacks the pheasant, causing an explosion that damages
	/// and knocks down enemies.
	/// </summary>
	/// <remarks>
	/// 1. Pheasant is thrown to target location with ThrowActor
	/// 2. Hawk dives to attack the pheasant (CollisionAndBack)
	/// 3. Explosion damage occurs during hawk attack, not when pheasant dies
	/// 4. Pheasant dies AFTER hawk attack (Dead(pheasant))
	/// 5. Falconer12 ability: Chance to stun enemies
	/// 6. Falconer18 ability: Changes knockdown direction
	/// </remarks>
	[Package("laima")]
	[SkillHandler(SkillId.Falconer_Pheasant)]
	public class Falconer_PheasantOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float DamageRadius = 100f;
		private const int MaxTargets = 15;
		private const int PheasantDurationSeconds = 20;

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_archer_dummypheasant_shot", "voice_archer_m_dummypheasant_shot");
		}

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

			FalconerHawkHelper.EnqueueSkill(hawk, () => skill.Run(this.HandleSkill(caster, skill, hawk, originPos, targetPos)));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Companion hawk, Position originPos, Position targetPos)
		{
			// Remove previous pheasant if exists
			var previousPheasant = hawk.Vars.Get<Mob>("HAWK_PHEASANT");
			if (previousPheasant != null && !previousPheasant.IsDead)
				previousPheasant.Kill(null);

			await FalconerHawkHelper.PrepareForSkill(skill, caster, hawk, unrestHawk: false);

			await Task.Delay(TimeSpan.FromMilliseconds(700));

			// Spawn the pheasant decoy
			var pheasant = MonsterSkillCreateMob(skill, caster, "falconer_pheasantdol", originPos, 0f, "", "", 0, PheasantDurationSeconds, "None", "");
			if (pheasant == null)
				return;

			// Store pheasant reference
			hawk.Vars.Set("HAWK_PHEASANT", pheasant);

			// Throw pheasant to target location
			Send.ZC_NORMAL.ThrowActor(pheasant, "F_smoke109_2", 1.5f, targetPos, 0.5f, 0f, 900f, 1f, 0);
			pheasant.SetPosition(targetPos);

			// Take off from shoulder or leave roost
			if (hawk.IsLandedOnShoulder)
				hawk.TakeOff();
			else if (hawk.IsOnRoost)
				hawk.LeaveRoost();

			// Wait for pheasant to land before hawk dives
			await Task.Delay(TimeSpan.FromMilliseconds(1000));

			// Hawk dives to attack the pheasant
			var syncKey = hawk.GenerateSyncKey();
			Send.ZC_NORMAL.CollisionAndBack(hawk, pheasant, syncKey, "HOVERING_SHOT", 1f, 7f, 1f, 0.7f, 20f, true);

			// Wait for hawk to reach pheasant
			await Task.Delay(TimeSpan.FromMilliseconds(1000));

			// EXPLOSION DAMAGE - occurs during hawk attack, not after pheasant death
			await this.TriggerExplosion(caster, skill, hawk, targetPos);

			// Wait for hawk return animation
			await Task.Delay(TimeSpan.FromMilliseconds(1500));

			// Kill pheasant after hawk attack
			if (!pheasant.IsDead)
				pheasant.Kill(null);

			// Hawk flies away or processes next queued skill
			await FalconerHawkHelper.DequeueSkill(skill, caster, hawk);
		}

		private async Task TriggerExplosion(ICombatEntity caster, Skill skill, Companion hawk, Position explosionPos)
		{
			// Explosion effect and screen shake
			hawk.PlayGroundEffect(explosionPos, "F_archer_caltrop_hit_explosion", 1f);
			Send.ZC_CHANGE_CAMERA_ZOOM(hawk, 2, 300f, 7f, 0.5f, 50f, 0f, 0f);
			hawk.BroadcastShockWave(2, 7, 0.5f, 50f, 0);

			var enemies = caster.Map.GetAttackableEnemiesInPosition(caster, explosionPos, DamageRadius)
				.Take(MaxTargets)
				.ToList();

			if (enemies.Count == 0)
				return;

			var hits = new List<SkillHitInfo>();
			var damageDelay = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;

			// Check for Falconer12 (Pheasant: Stun) and Falconer18 abilities
			var hasFalconer12 = caster.TryGetActiveAbilityLevel(AbilityId.Falconer12, out var falconer12Level);
			var hasFalconer18 = caster.IsAbilityActive(AbilityId.Falconer18);

			foreach (var enemy in enemies)
			{
				if (enemy.IsDead)
					continue;

				var skillHitResult = SCR_SkillHit(caster, enemy, skill);
				enemy.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, enemy, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;

				// Knockdown logic
				// Falconer12 prevents knockdown when active
				// Falconer18 changes knockdown direction (towards explosion center)
				if (enemy.IsKnockdownable())
				{
					if (hasFalconer18)
					{
						// Knockdown towards explosion center
						skillHit.KnockBackInfo = new KnockBackInfo(explosionPos, enemy, KnockBackType.KnockDown, 150, 80);
					}
					else if (!hasFalconer12)
					{
						// Normal knockdown away from explosion
						skillHit.KnockBackInfo = new KnockBackInfo(hawk.Position, enemy, KnockBackType.KnockDown, 150, 80);
					}
					// If Falconer12 is active but not Falconer18, no knockdown

					if (skillHit.KnockBackInfo != null)
						enemy.ApplyKnockdown(caster, skill, skillHit);
				}

				// Falconer12: Pheasant: Stun - chance to stun
				if (hasFalconer12)
				{
					var stunChance = falconer12Level * 10;
					if (RandomProvider.Get().Next(100) < stunChance)
					{
						enemy.StartBuff(BuffId.Stun, skill.Level, 0, TimeSpan.FromSeconds(3), caster);
					}
				}

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);

			await Task.CompletedTask;
		}

		/// <summary>
		/// Attempts to auto-trigger Pheasant on a target.
		/// Called by the hawk AI during FirstStrike auto-attack.
		/// Performs a simplified version: hawk dives to target and
		/// triggers the explosion without spawning the pheasant decoy.
		/// </summary>
		public static void TryAutoActivate(ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TryGetSkill(SkillId.Falconer_Pheasant, out var skill))
				return;

			if (skill.IsOnCooldown || target.IsDead)
				return;

			if (!FalconerHawkHelper.TryGetHawk(caster, out var hawk))
				return;

			skill.IncreaseOverheat();

			FalconerHawkHelper.EnqueueSkill(hawk, () =>
			{
				FalconerHawkHelper.LockHawk(hawk);
				skill.Run(AutoActivate(skill, caster, hawk, target));
			});
		}

		private static async Task AutoActivate(Skill skill, ICombatEntity caster, Companion hawk, ICombatEntity target)
		{
			var targetPos = target.Position;
			var syncKey = hawk.GenerateSyncKey();

			Send.ZC_NORMAL.CollisionAndBack(hawk, target, syncKey, "HOVERING_SHOT", 0.7f, 7f, 0.5f, 0.7f, 30f, true);

			await skill.Wait(TimeSpan.FromMilliseconds(700));

			if (target.IsDead)
				return;

			hawk.PlayGroundEffect(targetPos, "F_archer_caltrop_hit_explosion", 1f);
			Send.ZC_CHANGE_CAMERA_ZOOM(hawk, 2, 300f, 7f, 0.5f, 50f, 0f, 0f);
			hawk.BroadcastShockWave(2, 7, 0.5f, 50f, 0);

			var enemies = caster.Map.GetAttackableEnemiesInPosition(caster, targetPos, DamageRadius)
				.Take(MaxTargets)
				.ToList();

			if (enemies.Count == 0)
				return;

			var hits = new List<SkillHitInfo>();
			var damageDelay = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;

			var hasFalconer12 = caster.TryGetActiveAbilityLevel(AbilityId.Falconer12, out var falconer12Level);
			var hasFalconer18 = caster.IsAbilityActive(AbilityId.Falconer18);

			foreach (var enemy in enemies)
			{
				if (enemy.IsDead)
					continue;

				var skillHitResult = SCR_SkillHit(caster, enemy, skill);
				enemy.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, enemy, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;

				if (enemy.IsKnockdownable())
				{
					if (hasFalconer18)
						skillHit.KnockBackInfo = new KnockBackInfo(targetPos, enemy, KnockBackType.KnockDown, 150, 80);
					else if (!hasFalconer12)
						skillHit.KnockBackInfo = new KnockBackInfo(hawk.Position, enemy, KnockBackType.KnockDown, 150, 80);

					if (skillHit.KnockBackInfo != null)
						enemy.ApplyKnockdown(caster, skill, skillHit);
				}

				if (hasFalconer12)
				{
					var stunChance = falconer12Level * 10;
					if (RandomProvider.Get().Next(100) < stunChance)
						enemy.StartBuff(BuffId.Stun, skill.Level, 0, TimeSpan.FromSeconds(3), caster);
				}

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);

			await FalconerHawkHelper.DequeueSkill(skill, caster, hawk);
		}
	}
}
