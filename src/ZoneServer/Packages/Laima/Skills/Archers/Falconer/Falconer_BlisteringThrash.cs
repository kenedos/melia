using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handler for the Falconer skill Sonic Strike (Blistering Thrash).
	/// Commands the hawk to dive at a target location, dealing multi-hit
	/// damage and applying Blistering_Debuff to enemies.
	/// Also provides the auto-trigger used by First Strike (Pre-Emptive Strike).
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Falconer_BlisteringThrash)]
	public class Falconer_BlisteringThrashOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float AttackRadius = 80f;
		private const int BaseHitCount = 6;
		private const float PenetrateHeight = 10f;
		private const float DebuffDurationSeconds = 7f;
		private const int DebuffChancePercent = 50;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_atk_long_cast_f", "voice_war_atk_long_cast");
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopSound("voice_atk_long_cast_f", "voice_war_atk_long_cast");
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

			if (FalconerHawkQueue.IsLanding(hawk))
			{
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

			if (caster.IsAbilityActive(AbilityId.Falconer10))
			{
				var extraSp = skill.Properties.GetFloat(PropertyName.SpendSP) * 0.5f;
				if (!caster.TrySpendSp(extraSp))
				{
					caster.ServerMessage(Localization.Get("Not enough SP."));
					return;
				}
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			FalconerHawkQueue.Enqueue(hawk, new HawkSkillRequest(
				skill, caster,
				ctx => ExecuteManual(ctx, targetPos, forceId)));
		}

		/// <summary>
		/// Attempts to auto-trigger Sonic Strike on a target.
		/// Called by FirstStrike buff when the caster attacks.
		/// </summary>
		public static void TryActivate(ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TryGetSkill(SkillId.Falconer_BlisteringThrash, out var skill))
				return;

			if (skill.IsOnCooldown || target.IsDead)
				return;

			if (!FalconerHawkHelper.TryGetHawk(caster, out var hawk))
				return;

			if (FalconerHawkQueue.IsLanding(hawk))
				return;

			if (caster.IsAbilityActive(AbilityId.Falconer14))
				return;

			skill.IncreaseOverheat();

			FalconerHawkQueue.Enqueue(hawk, new HawkSkillRequest(
				skill, caster,
				ctx => ExecuteAuto(ctx, target)));
		}

		private static async Task ExecuteManual(HawkSkillContext ctx, Position targetPos, int forceId)
		{
			if (ctx.Hawk.IsPerched)
			{
				FalconerHawkHelper.UnrestHawkIfNeeded(ctx.Hawk);
				await ctx.Delay(1200);
			}

			await Dive(ctx, targetPos, forceId);

			await ctx.Delay(500);
		}

		private static async Task ExecuteAuto(HawkSkillContext ctx, ICombatEntity target)
		{
			if (target.IsDead)
				return;

			var skill = ctx.Skill;
			var caster = ctx.Caster;
			var hawk = ctx.Hawk;

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_READY(caster, skill, 1, caster.Position, target.Position);

			if (ctx.Hawk.IsPerched)
			{
				FalconerHawkHelper.UnrestHawkIfNeeded(ctx.Hawk);
				await ctx.Delay(1200);
			}

			await Dive(ctx, target.Position, forceId);

			await ctx.Delay(500);
		}

		private static async Task Dive(HawkSkillContext ctx, Position targetPos, int forceId)
		{
			var skill = ctx.Skill;
			var caster = ctx.Caster;
			var hawk = ctx.Hawk;

			Send.ZC_CHANGE_CAMERA_ZOOM(hawk, 2, 99999f, 7f, 0.5f, 50f, 0f, 0f);
			
			var divePos = new Position(targetPos.X, targetPos.Y + FalconerHawkHelper.DefaultHawkHeight, targetPos.Z);
			var syncKey = hawk.GenerateSyncKey();
			Send.ZC_NORMAL.PenetratePosition(hawk, divePos, PenetrateHeight, syncKey, "HOVERING_SHOT", 0.7f, 7f, 0.5f, 0.7f, 30f);

			await ctx.Delay(700);

			var enemies = caster.Map.GetAttackableEnemiesInPosition(caster, targetPos, AttackRadius)
				.LimitBySDR(caster, skill)
				.ToList();

			var falconer15BonusDamage = 0f;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Falconer15, out var falconer15Level))
				falconer15BonusDamage = hawk.Properties.GetFloat(PropertyName.PATK) * 0.0025f * falconer15Level;

			var applyConfuse = caster.IsAbilityActive(AbilityId.Falconer10);

			foreach (var enemy in enemies)
			{
				if (enemy.IsDead)
					continue;

				var hasBlind = enemy.IsBuffActive(BuffId.Blistering_Debuff);

				if (RandomProvider.Get().Next(100) < DebuffChancePercent)
					enemy.StartBuff(BuffId.Blistering_Debuff, skill.Level, 0, TimeSpan.FromSeconds(DebuffDurationSeconds), caster);

				var modifier = SkillModifier.MultiHit(BaseHitCount);
				if (enemy.Race == RaceType.Widling)
					modifier.DamageMultiplier += 0.5f;
				if (falconer15BonusDamage > 0f)
					modifier.BonusDamage += falconer15BonusDamage;

				var skillHitResult = SCR_SkillHit(caster, enemy, skill, modifier);
				enemy.TakeDamage(skillHitResult.Damage, caster);

				var hit = new HitInfo(caster, enemy, skill, skillHitResult);
				hit.ForceId = forceId;
				Send.ZC_HIT_INFO(caster, enemy, hit);

				if (applyConfuse && hasBlind)
					enemy.StartBuff(BuffId.Confuse, skill.Level, 0, TimeSpan.FromSeconds(5), caster, skill.Id);
			}

			await ctx.Delay(700);

			hawk.BroadcastShockWave(2, 7, 0.5f, 50f, 0);
			hawk.SetPosition(targetPos);
		}
	}
}
