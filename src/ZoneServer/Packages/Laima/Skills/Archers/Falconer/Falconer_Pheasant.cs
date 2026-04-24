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

			FalconerHawkQueue.Enqueue(hawk, new HawkSkillRequest(
				skill, caster,
				ctx => ExecuteManual(ctx, originPos, targetPos)));
		}

		/// <summary>
		/// Attempts to auto-trigger Pheasant on a target.
		/// Called by the hawk AI during FirstStrike auto-attack.
		/// Performs a simplified version: hawk dives to target and
		/// triggers the explosion without spawning the pheasant decoy.
		/// </summary>
		public static void TryActivate(ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TryGetSkill(SkillId.Falconer_Pheasant, out var skill))
				return;

			if (skill.IsOnCooldown || target.IsDead)
				return;

			if (!FalconerHawkHelper.TryGetHawk(caster, out var hawk))
				return;

			skill.IncreaseOverheat();

			FalconerHawkQueue.Enqueue(hawk, new HawkSkillRequest(
				skill, caster,
				ctx => ExecuteAuto(ctx, target)));
		}

		private static async Task ExecuteManual(HawkSkillContext ctx, Position originPos, Position targetPos)
		{
			var skill = ctx.Skill;
			var caster = ctx.Caster;
			var hawk = ctx.Hawk;

			var previousPheasant = hawk.Vars.Get<Mob>("HAWK_PHEASANT");
			if (previousPheasant != null && !previousPheasant.IsDead)
				previousPheasant.Kill(null);

			await ctx.Delay(700);

			var pheasant = MonsterSkillCreateMob(skill, caster, "falconer_pheasantdol", originPos, 0f, "", "", 0, PheasantDurationSeconds, "None", "");
			if (pheasant == null)
				return;

			hawk.Vars.Set("HAWK_PHEASANT", pheasant);
			Send.ZC_NORMAL.ThrowActor(pheasant, "F_smoke109_2", 1.5f, targetPos, 0.5f, 0f, 900f, 1f, 0);
			pheasant.SetPosition(targetPos);

			if (hawk.IsPerched)
			{
				FalconerHawkHelper.UnrestHawkIfNeeded(hawk);
				await ctx.Delay(800);
			}

			await ctx.Delay(1000);

			var syncKey = hawk.GenerateSyncKey();
			Send.ZC_NORMAL.CollisionAndBack(hawk, pheasant, syncKey, "HOVERING_SHOT", 1f, 7f, 1f, 0.7f, 20f, true);

			await ctx.Delay(900);

			TriggerExplosion(caster, skill, hawk, targetPos);

			await ctx.Delay(1500);

			if (pheasant != null && !pheasant.IsDead)
				pheasant.Kill(null);
		}

		private static async Task ExecuteAuto(HawkSkillContext ctx, ICombatEntity target)
		{
			if (target.IsDead)
				return;

			var skill = ctx.Skill;
			var caster = ctx.Caster;
			var hawk = ctx.Hawk;

			Send.ZC_SKILL_READY(caster, skill, 1, caster.Position, target.Position);

			if (hawk.IsPerched)
			{
				FalconerHawkHelper.UnrestHawkIfNeeded(hawk);
				await ctx.Delay(800);
			}

			var syncKey = hawk.GenerateSyncKey();
			Send.ZC_NORMAL.CollisionAndBack(hawk, target, syncKey, "HOVERING_SHOT", 0.7f, 7f, 0.5f, 0.7f, 30f, true);

			await ctx.Delay(700);

			if (target.IsDead)
				return;

			TriggerExplosion(caster, skill, hawk, target.Position);

			await ctx.Delay(1000);
		}

		private static void TriggerExplosion(ICombatEntity caster, Skill skill, Companion hawk, Position explosionPos)
		{
			hawk.PlayGroundEffect(explosionPos, "F_archer_caltrop_hit_explosion", 1f);
			Send.ZC_CHANGE_CAMERA_ZOOM(hawk, 2, 300f, 7f, 0.5f, 50f, 0f, 0f);
			hawk.BroadcastShockWave(2, 7, 0.5f, 50f, 0);

			var enemies = caster.Map.GetAttackableEnemiesInPosition(caster, explosionPos, DamageRadius)
				.LimitBySDR(caster, skill)
				.ToList();

			if (enemies.Count == 0)
				return;

			var falconer15BonusDamage = 0f;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Falconer15, out var falconer15Level))
				falconer15BonusDamage = hawk.Properties.GetFloat(PropertyName.PATK) * 0.0025f * falconer15Level;

			foreach (var enemy in enemies)
			{
				if (enemy.IsDead)
					continue;

				var modifier = new SkillModifier();
				if (enemy.Race == RaceType.Widling)
					modifier.DamageMultiplier += 0.5f;
				if (falconer15BonusDamage > 0f)
					modifier.BonusDamage += falconer15BonusDamage;

				var skillHitResult = SCR_SkillHit(caster, enemy, skill, modifier);
				enemy.TakeDamage(skillHitResult.Damage, caster);

				var hit = new HitInfo(caster, enemy, skill, skillHitResult, HitResultType.Hit);
				Send.ZC_HIT_INFO(caster, enemy, hit);

				if (RandomProvider.Get().Next(100) < 50)
					enemy.StartBuff(BuffId.Stun, skill.Level, 0, TimeSpan.FromSeconds(3), caster);
			}
		}
	}
}
