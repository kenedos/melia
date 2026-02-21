using System;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_warpulis_Skill_1)]
	public class Mon_warpulis_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(700);
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
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 160, width: 20, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 500;
			var damageDelay = 700;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 160, width: 20, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 300;
			damageDelay = 1000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_warpulis_Skill_2)]
	public class Mon_warpulis_Skill_2 : ITargetSkillHandler
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
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 300f, 5));
			await skill.Wait(TimeSpan.FromMilliseconds(1600));
			caster.StartBuff(BuffId.UC_Cloaking_Buff, 1f, 0f, TimeSpan.FromMilliseconds(1050f), caster);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 280);
			await TeleportToPosition(caster, position, "F_sys_target_boss1", 1f);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetRandom, caster, target);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_explosion98_green", 2f),
				Range = 50f,
				KnockdownPower = 250f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 80f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_warpulis_Skill_3)]
	public class Mon_warpulis_Skill_3 : ITargetSkillHandler
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
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var position = originPos.GetRelative(farPos, distance: 20);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion123_3", 1f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion123_3", 4f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			position = originPos.GetRelative(farPos, distance: 20, angle: 180f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos, distance: 35, angle: 209f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 35, angle: 30f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos, distance: 50, angle: 60f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 50, angle: 240f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 65, angle: 90f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 65, angle: 270f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 80, angle: 299f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 80, angle: 120f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 95, angle: 149f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 95, angle: 329f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 110);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 110, angle: 180f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			position = originPos.GetRelative(farPos);
		}
	}

	[SkillHandler(SkillId.Mon_warpulis_Skill_4)]
	public class Mon_warpulis_Skill_4 : ITargetSkillHandler
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
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2300));
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss1##0.8", 1f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion123_3", 4f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			var position = originPos.GetRelative(farPos, distance: 30, angle: -90f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 60, angle: -60f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 90, angle: -30f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 120, angle: -4f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 140, angle: 15f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 150, angle: 30f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(750));
			position = originPos.GetRelative(farPos, distance: 30, angle: 90f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 60, angle: 60f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 90, angle: 30f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 120, angle: 4f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 140, angle: -15f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 150, angle: -30f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(550));
			position = originPos.GetRelative(farPos, distance: 30, angle: 90f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = originPos.GetRelative(farPos, distance: 30, angle: -90f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			var effectHitConfig2 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss1##0.8", 1f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion123_3", 4f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			position = originPos.GetRelative(farPos, distance: 60, angle: 60f);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			position = originPos.GetRelative(farPos, distance: 60, angle: -60f);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			var effectHitConfig3 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss1##0.8", 1f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion123_3", 4f),
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			position = originPos.GetRelative(farPos, distance: 90, angle: 30f);
			await EffectAndHit(skill, caster, position, effectHitConfig3);
			position = originPos.GetRelative(farPos, distance: 90, angle: -30f);
			await EffectAndHit(skill, caster, position, effectHitConfig3);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			var effectHitConfig4 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss1##0.8", 1f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion123_3", 4f),
				Range = 50f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			position = originPos.GetRelative(farPos, distance: 120, angle: 4f);
			await EffectAndHit(skill, caster, position, effectHitConfig4);
			position = originPos.GetRelative(farPos, distance: 120, angle: -4f);
			await EffectAndHit(skill, caster, position, effectHitConfig4);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			var effectHitConfig5 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss1##0.8", 1f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion123_3", 4f),
				Range = 60f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			position = originPos.GetRelative(farPos, distance: 140, angle: -15f);
			await EffectAndHit(skill, caster, position, effectHitConfig5);
			position = originPos.GetRelative(farPos, distance: 140, angle: -15f);
			await EffectAndHit(skill, caster, position, effectHitConfig5);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			var effectHitConfig6 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss1##0.8", 1f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion123_4", 7f),
				Range = 65f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			position = originPos.GetRelative(farPos, distance: 150, angle: -30f);
			await EffectAndHit(skill, caster, position, effectHitConfig6);
			position = originPos.GetRelative(farPos, distance: 150, angle: 30f);
			await EffectAndHit(skill, caster, position, effectHitConfig6);
		}
	}

	[SkillHandler(SkillId.Mon_warpulis_Skill_5)]
	public class Mon_warpulis_Skill_5 : ITargetSkillHandler
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
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(3100));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force018_trail_psychokinesis#Bip001 R Toe0", 2f),
				EndEffect = new EffectConfig("F_explosion120_blue", 2.5f),
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 300f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			};

			var position = originPos.GetRelative(farPos, distance: 100, rand: 100, height: 1);
			await MissilePadThrow(skill, caster, position, config, 0f, "boss_warpulis_windpad");
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 100, rand: 100, height: 1);
			await MissilePadThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force018_trail_psychokinesis#Bip001 L Hand", 2f),
				EndEffect = new EffectConfig("F_explosion120_blue", 2.5f),
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 300f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			}, 0f, "boss_warpulis_windpad");
			await skill.Wait(TimeSpan.FromMilliseconds(700));
			position = originPos.GetRelative(farPos, distance: 100, rand: 100, height: 1);
			await MissilePadThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force018_trail_psychokinesis#Bip001 R Hand", 2f),
				EndEffect = new EffectConfig("F_explosion120_blue", 2.5f),
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 300f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			}, 0f, "boss_warpulis_windpad");
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = originPos.GetRelative(farPos, distance: 100, rand: 100, height: 1);
			await MissilePadThrow(skill, caster, position, config, 0f, "boss_warpulis_windpad");
		}
	}
}
