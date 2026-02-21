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
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Armaox_Skill_1)]
	public class Mon_boss_Armaox_Skill_1 : ITargetSkillHandler
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
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, forceId, null);

			skill.Run(this.HandleSkill(caster, skill, originPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, originPos, length: 0, width: 100);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1800;
			var damageDelay = 100;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.CasterForward, 100, 30, 10, 1, 5, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Armaox_Skill_2)]
	public class Mon_boss_Armaox_Skill_2 : ITargetSkillHandler
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 400);
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, targetPos, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground016_light", 1.6f),
				Range = 60f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 150f,
				InnerRange = 0f,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(650));
			await EffectAndHit(skill, caster, targetPos, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup003", 2.4f),
				Range = 70f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 80f,
				InnerRange = 0f,
			}, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.CasterForward, 100, 87, 10, 1, 5, hits);

			caster.Position = caster.Map.Ground.GetLastValidPosition(caster.Position, caster.Position.GetRelative(caster.Direction, 120));
		}
	}

	[SkillHandler(SkillId.Mon_boss_Armaox_Skill_3)]
	public class Mon_boss_Armaox_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1800,
				Effect = new EffectConfig("F_spread_out023_circle_alpha##0.8", 1f),
				Range = 80f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 2000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(30));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 80f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 2000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			for (var i = 0; i < 7; i++)
			{
				position = originPos.GetRelative(farPos);
				await EffectAndHit(skill, caster, position, config);

				if (i < 6)
					await skill.Wait(TimeSpan.FromMilliseconds(30));
			}
			caster.StartBuff(BuffId.Mon_Shield, 1f, 0f, TimeSpan.FromMilliseconds(10000f), caster);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Armaox_Skill_4)]
	public class Mon_boss_Armaox_Skill_4 : ITargetSkillHandler
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 50);
			await skill.Wait(TimeSpan.FromMilliseconds(1900));
			await EffectAndHit(skill, caster, targetPos, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground046_smoke", 1.6f),
				Range = 60f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			await EffectAndHit(skill, caster, targetPos, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground046_smoke", 1.6f),
				Range = 60f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});

			caster.Position = caster.Map.Ground.GetLastValidPosition(caster.Position, caster.Position.GetRelative(caster.Direction, 50));
		}
	}

	[SkillHandler(SkillId.Mon_boss_Armaox_Skill_5)]
	public class Mon_boss_Armaox_Skill_5 : ITargetSkillHandler
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 5, width: 100);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
		
			var hits = new List<SkillHitInfo>();
			await skill.Wait(TimeSpan.FromMilliseconds(2800));
			var position = originPos.GetRelative(farPos, distance: 50f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 0.5f),
				PositionDelay = 800,
				Effect = new EffectConfig("None", 0.6f),
				Range = 60f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 80f,
				InnerRange = 0f,
			});
			//await skill.Wait(TimeSpan.FromMilliseconds(800));
			var startingPosition = originPos.GetRelative(farPos, distance: 30f);
			var endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 0.25f,
				ArrowSpacingTime = 0.25f,
				ArrowLifeTime = 0f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_burstup029_smoke_violet2", 2f),
				Range = 40f,
				KnockdownPower = 150f,
				Delay = 200f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.2f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Armaox_Skill_6)]
	public class Mon_boss_Armaox_Skill_6 : ITargetSkillHandler
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
			var targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 200);
			await skill.Wait(TimeSpan.FromMilliseconds(2100));

			var rnd = new Random();
			for (var i = 0; i < 9; i++)
			{
				var angle = rnd.NextDouble() * Math.PI * 2;
				var distance = rnd.NextDouble() * 20;
				var missilePos = new Position(
					target.Position.X + (float)(Math.Cos(angle) * distance),
					target.Position.Y,
					target.Position.Z + (float)(Math.Sin(angle) * distance)
				);
				await MissileThrow(skill, caster, missilePos, new MissileConfig
				{
					Effect = new EffectConfig("I_force051_dark#Bip001 Head", 1f),
					EndEffect = new EffectConfig("I_explosion002_violet", 1f),
					Range = 15f,
					FlyTime = 0.8f,
					DelayTime = 0f,
					Gravity = 800f,
					Speed = 1f,
					HitTime = 1000f,
					HitCount = 1,
					GroundEffect = EffectConfig.None,
					// TargetEffect.Name = "F_sys_target_boss##0.5",
					// TargetEffect.Scale = 1.5f,
				});
				if (i < 11)
					await skill.Wait(TimeSpan.FromMilliseconds(100));
			}
		}
	}
}
