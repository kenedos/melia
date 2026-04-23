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

			FalconerHawkQueue.Enqueue(hawk, new HawkSkillRequest(
				skill, caster,
				ctx => ExecuteManual(ctx, targetPos)));
		}

		/// <summary>
		/// Attempts to auto-trigger Tomahawk on a target.
		/// Called by the hawk AI during FirstStrike auto-attack.
		/// </summary>
		public static void TryActivate(ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TryGetSkill(SkillId.Falconer_Tomahawk, out var skill))
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
			Send.ZC_NORMAL.PenetratePosition(hawk, divePos, PenetrateHeight, syncKey, "TOMAHAWK_SHOT", 0.7f, 7f, 0.5f, 0.7f, 30f);

			await ctx.Delay(700);

			hawk.PlayGroundEffect(targetPos, "F_explosion131_fire", 3f);
			hawk.BroadcastShockWave(2, 7, 0.5f, 50f, 0);

			var enemies = caster.Map.GetAttackableEnemiesInPosition(caster, targetPos, AttackRadius)
				.Take(MaxTargets)
				.ToList();

			foreach (var enemy in enemies)
			{
				if (enemy.IsDead)
					continue;

				var skillHitResult = SCR_SkillHit(caster, enemy, skill, SkillModifier.MultiHit(BaseHitCount));
				enemy.TakeDamage(skillHitResult.Damage, caster);

				var hit = new HitInfo(caster, enemy, skill, skillHitResult, HitResultType.Hit);
				Send.ZC_HIT_INFO(caster, enemy, hit);

				if (caster.IsAbilityActive(AbilityId.Falconer27))
					enemy.StartBuff(BuffId.Burn, skill.Level, 0, TimeSpan.FromSeconds(5), caster);
			}

			hawk.SetPosition(targetPos);
		}
	}
}
