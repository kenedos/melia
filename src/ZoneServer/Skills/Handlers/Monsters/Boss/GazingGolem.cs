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
	[SkillHandler(SkillId.Mon_boss_GazingGolem_Skill_1)]
	public class Mon_boss_GazingGolem_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1400);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 95, width: 25);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 1400;
			var damageDelay = 1400;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_GazingGolem_Skill_2)]
	public class Mon_boss_GazingGolem_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.Zero;
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
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.1f),
				PositionDelay = 2400,
				Effect = new EffectConfig("F_ground059_fire", 1f),
				Range = 35f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = originPos.GetRelative(farPos, distance: 50, angle: 44f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 50, angle: -44f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.1f),
				PositionDelay = 2400,
				Effect = new EffectConfig("F_ground059_fire", 1.5f),
				Range = 42f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_GazingGolem_Skill_3)]
	public class Mon_boss_GazingGolem_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2200);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 65);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2200;
			var damageDelay = 2200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 65);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 300;
			damageDelay = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 65);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 300;
			damageDelay = 2800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 65);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 300;
			damageDelay = 3100;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(1400));
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force023_fire#Dummy_wlk_L_effect", 1f),
				EndEffect = new EffectConfig("F_explosion050_fire", 0.5f),
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 0.35f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 0.4f),
			};

			var position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 120, angle: 300f, rand: 50, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			var missileConfig2 = new MissileConfig
			{
				Effect = new EffectConfig("I_force023_fire#Dummy_wlk_R_effect", 1f),
				EndEffect = new EffectConfig("F_explosion050_fire", 0.5f),
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 0.35f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 0.4f),
			};

			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 120, angle: 300f, rand: 50, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 120, angle: 239f, rand: 50, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 120, angle: 239f, rand: 50, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 120, angle: 180f, rand: 50, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 120, angle: 180f, rand: 50, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 120, angle: 119f, rand: 50, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 120, angle: 119f, rand: 50, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 120, angle: 60f, rand: 50, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 120, angle: 60f, rand: 50, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 120, rand: 50, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 120, rand: 50, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
		}
	}

	[SkillHandler(SkillId.Mon_boss_GazingGolem_Skill_4)]
	public class Mon_boss_GazingGolem_Skill_4 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2200);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 20);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 1900;
			var damageDelay = 2200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 20);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 1900;
			damageDelay = 2200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			var startingPosition = originPos.GetRelative(farPos, distance: 30f);
			var endingPosition = originPos.GetRelative(farPos, distance: 120f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_arrow_monster", 1f),
				ArrowSpacing = 15f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 2000f,
				HitEffect = new EffectConfig("None", 0.4f),
				Range = 0f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 120f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_arrow_monster", 1f),
				ArrowSpacing = 15f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 2000f,
				HitEffect = new EffectConfig("3D_F_mon_skl_gust_sand", 0.4f),
				Range = 0f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}
}
