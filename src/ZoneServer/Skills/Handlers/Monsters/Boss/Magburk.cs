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
using Yggdrasil.Extensions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_MagBurk_Skill_1)]
	public class Mon_boss_MagBurk_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2500);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 30, angle: 90);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_MagBurk_Skill_2)]
	public class Mon_boss_MagBurk_Skill_2 : ITargetSkillHandler
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
			var targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 150);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));

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
					Effect = new EffectConfig("I_force054_fire#topbody_01", 1.2f),
					EndEffect = new EffectConfig("F_ground021_fire", 2f),
					Range = 10f,
					FlyTime = 1.3f,
					DelayTime = 0f,
					Gravity = 600f,
					Speed = 1f,
					HitTime = 1000f,
					HitCount = 1,
					GroundEffect = new EffectConfig("None", 1.2f),
					// TargetEffect.Name = "F_sys_target_boss##0.5",
					// TargetEffect.Scale = 1.5f,
				});
				SkillCreatePad(caster, skill, missilePos, 0f, PadName.Mon_firewall);
				if (i < 8)
					await skill.Wait(TimeSpan.FromMilliseconds(200));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_MagBurk_Skill_3)]
	public class Mon_boss_MagBurk_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var position = originPos.GetRelative(farPos, distance: 85);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 4f),
				PositionDelay = 2000,
				Effect = new EffectConfig("F_buff_fire_spread", 1f),
				Range = 60f,
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

			var rnd = new Random();
			var padCount = rnd.Next(4, 7);
			var baseDirection = originPos.GetDirection(farPos);

			for (var i = 0; i < padCount; i++)
			{
				var angleOffset = (rnd.NextDouble() - 0.5) * 70;
				var distance = 30 + rnd.NextDouble() * 105;
				var direction = new Direction((float)(baseDirection.DegreeAngle + angleOffset));
				var padPos = originPos.GetRelative(direction, (float)distance);
				SkillCreatePad(caster, skill, padPos, 0f, PadName.Mon_firewall);
				await skill.Wait(TimeSpan.FromMilliseconds(100));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_MagBurk_Skill_4)]
	public class Mon_boss_MagBurk_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var spawnPos = originPos.GetRelative(farPos, distance: 89.817085f, angle: -49f);
			MonsterSkillCreateMob(skill, caster, "InfroBurk_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 103.231f, angle: -133f);
			MonsterSkillCreateMob(skill, caster, "InfroBurk_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 79.445053f, angle: 51f);
			MonsterSkillCreateMob(skill, caster, "InfroBurk_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 102.52924f, angle: 118f);
			MonsterSkillCreateMob(skill, caster, "InfroBurk_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
		}
	}

	[SkillHandler(SkillId.Mon_boss_MagBurk_Skill_5)]
	public class Mon_boss_MagBurk_Skill_5 : ITargetSkillHandler
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
			var targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 150);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));

			var rnd = new Random();
			for (var i = 0; i < 7; i++)
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
					Effect = new EffectConfig("I_force054_fire#topbody_01", 1.2f),
					EndEffect = new EffectConfig("F_ground021_fire", 2f),
					Range = 10f,
					FlyTime = 1.3f,
					DelayTime = 0f,
					Gravity = 600f,
					Speed = 1f,
					HitTime = 1000f,
					HitCount = 1,
					GroundEffect = new EffectConfig("None", 1.2f),
					// TargetEffect.Name = "F_sys_target_boss##0.5",
					// TargetEffect.Scale = 1.5f,
				});
				SkillCreatePad(caster, skill, missilePos, 0f, PadName.Mon_movetrap);
				if (i < 6)
					await skill.Wait(TimeSpan.FromMilliseconds(200));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_MagBurk_Skill_6)]
	public class Mon_boss_MagBurk_Skill_6 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1500));

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
					Effect = new EffectConfig("I_force023_fire", 2f),
					EndEffect = new EffectConfig("F_explosion89_fire", 1f),
					Range = 40f,
					FlyTime = 1f,
					DelayTime = 0f,
					Gravity = 600f,
					Speed = 1f,
					HitTime = 1000f,
					HitCount = 1,
					GroundEffect = new EffectConfig("None", 3.5f),
					// TargetEffect.Name = "F_sys_target_boss##0.5",
					// TargetEffect.Scale = 1.5f,
				});
				if (i < 8)
					await skill.Wait(TimeSpan.FromMilliseconds(300));
			}
		}
	}
}
