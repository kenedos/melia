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
		private const int MaxTargets = 8;
		private const int BaseHitCount = 6;
		private const float PenetrateHeight = 10f;
		private const float DebuffDurationSeconds = 7f;

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
				ctx => ExecuteManual(ctx, targetPos)));
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

			if (caster.IsAbilityActive(AbilityId.Falconer14))
				return;

			skill.IncreaseOverheat();

			FalconerHawkQueue.Enqueue(hawk, new HawkSkillRequest(
				skill, caster,
				ctx => ExecuteAuto(ctx, target)));
		}

		private static async Task ExecuteManual(HawkSkillContext ctx, Position targetPos)
		{
			FalconerHawkHelper.UnrestHawkIfNeeded(ctx.Hawk);
			await ctx.Delay(100);

			await Dive(ctx, targetPos);

			await ctx.Delay(500);
		}

		private static async Task ExecuteAuto(HawkSkillContext ctx, ICombatEntity target)
		{
			if (target.IsDead)
				return;

			await Dive(ctx, target.Position);
		}

		private static async Task Dive(HawkSkillContext ctx, Position targetPos)
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
				.Take(MaxTargets)
				.ToList();

			if (enemies.Count > 0)
			{
				var hits = new List<SkillHitInfo>();
				var damageDelay = TimeSpan.FromMilliseconds(50);
				var skillHitDelay = TimeSpan.Zero;

				foreach (var enemy in enemies)
				{
					if (enemy.IsDead)
						continue;

					enemy.StartBuff(BuffId.Blistering_Debuff, skill.Level, 0, TimeSpan.FromSeconds(DebuffDurationSeconds), caster);

					var skillHitResult = SCR_SkillHit(caster, enemy, skill, SkillModifier.MultiHit(BaseHitCount));
					enemy.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, enemy, skill, skillHitResult, damageDelay, skillHitDelay);
					skillHit.HitEffect = HitEffect.Impact;
					hits.Add(skillHit);
				}

				Send.ZC_SKILL_HIT_INFO(caster, hits);
			}

			hawk.BroadcastShockWave(2, 7, 0.5f, 50f, 0);
			hawk.SetPosition(targetPos);
		}
	}
}
