using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Templeshooter_Skill_1)]
	public class Mon_boss_Templeshooter_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1500);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 35, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 1300;
			var damageDelay = 1500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_debrave, 1, 0f, 5000f, 1, 30, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Templeshooter_Skill_2)]
	public class Mon_boss_Templeshooter_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2400);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 200, width: 60, angle: 20f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2400;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Templeshooter_Skill_3)]
	public class Mon_boss_Templeshooter_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_arrow003_dark1", 1f),
				EndEffect = new EffectConfig("F_ground077_smoke", 0.2f),
				DotEffect = EffectConfig.None,
				Range = 18f,
				DelayTime = 0f,
				FlyTime = 0.5f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = EffectConfig.None,
			};

			await skill.Wait(TimeSpan.FromMilliseconds(3400));

			// Phase 1: volleys relative to origin/far positions
			var phase1Volleys = new[] { 2, 2, 2, 2, 2 };
			for (var v = 0; v < phase1Volleys.Length; v++)
			{
				for (var i = 0; i < phase1Volleys[v]; i++)
				{
					var position = originPos.GetRelative(farPos, rand: 110);
					await MissileFall(caster, skill, position, config);
				}
				if (v < phase1Volleys.Length - 1)
					await skill.Wait(TimeSpan.FromMilliseconds(100));
			}

			await skill.Wait(TimeSpan.FromMilliseconds(100));

			// Phase 2: volleys relative to target
			var phase2Volleys = new[] { 3, 1, 2, 2, 2 };
			var phase2Delays = new[] { 100, 100, 100, 100 };
			for (var v = 0; v < phase2Volleys.Length; v++)
			{
				for (var i = 0; i < phase2Volleys[v]; i++)
				{
					var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
					await MissileFall(caster, skill, position, config);
				}
				if (v < phase2Delays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(phase2Delays[v]));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Templeshooter_Skill_4)]
	public class Mon_boss_Templeshooter_Skill_4 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.1f,
				ArrowLifeTime = 0.5f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_burstup031_dark", 0.4f),
				Range = 15f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 35f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			await skill.Wait(TimeSpan.FromMilliseconds(1400));

			var volleys = new[] { 1, 3, 2, 2, 2, 2, 2, 2 };
			var delays = new[] { 100, 100, 150, 150, 150, 150, 150 };
			for (var v = 0; v < volleys.Length; v++)
			{
				for (var i = 0; i < volleys[v]; i++)
				{
					var startingPosition = originPos.GetRelative(farPos, distance: 30f);
					var endingPosition = originPos.GetRelative(farPos, distance: 180f);
					await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
				}
				if (v < delays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[v]));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Templeshooter_Skill_5)]
	public class Mon_boss_Templeshooter_Skill_5 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2400);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 250, width: 30, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 2200;
			var damageDelay = 2400;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(2400));

			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup008_smoke##0.1", 0.8f),
				Range = 0f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 0,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var volleys = new[] { new[] { -4f }, new[] { 4f }, new[] { -4f, 4f }, new[] { -4f, 4f }, new[] { -4f, 4f } };
			var delays = new[] { 100, 50, 150, 150 };
			for (var v = 0; v < volleys.Length; v++)
			{
				foreach (var angle in volleys[v])
				{
					var position = originPos.GetRelative(farPos, distance: 225, angle: angle);
					await EffectAndHit(skill, caster, position, config);
				}
				if (v < delays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[v]));
			}
		}
	}
}
