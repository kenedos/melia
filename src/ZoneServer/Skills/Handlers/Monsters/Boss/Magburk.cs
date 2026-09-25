using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Shared.Util;
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
using static Melia.Zone.Skills.Helpers.SkillUseHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_MagBurk_Skill_1)]
	public class Mon_boss_MagBurk_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(2500);
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
			var aniTime = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
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

			var config = new MissileConfig
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
			};

			var waveCounts = new[] { 3, 3, 3 };
			for (var wave = 0; wave < waveCounts.Length; wave++)
			{
				if (wave > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(1500));
				if (!caster.Position.InRange2D(target.Position, 300))
					break;

				foreach (var position in GetScatteredPositions(GetLeadPosition(target, 1300, caster), waveCounts[wave], 110, 45))
					_ = this.ThrowWithPad(caster, skill, originPos.GetNearestPositionWithinDistance(position, 250f), config, PadName.Mon_firewall);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1300));
		}

		private async Task ThrowWithPad(ICombatEntity caster, Skill skill, Position position, MissileConfig config, string padName)
		{
			await MissileThrow(skill, caster, position, config);
			SkillCreatePad(caster, skill, position, 0f, padName);
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			_ = MonsterSkillFollowMovePath(caster, skill, (2000, 0f, 0f), (3100, 85f, 0f));

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

			var padCount = GameRandom.Get().Next(4, 7);
			var baseDirection = originPos.GetDirection(farPos);

			for (var i = 0; i < padCount; i++)
			{
				var angleOffset = (GameRandom.Get().NextDouble() - 0.5) * 70;
				var distance = 30 + GameRandom.Get().NextDouble() * 105;
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

			var config = new MissileConfig
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
			};

			var waveCounts = new[] { 2, 3, 3 };
			for (var wave = 0; wave < waveCounts.Length; wave++)
			{
				if (wave > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(1500));
				if (!caster.Position.InRange2D(target.Position, 300))
					break;

				foreach (var position in GetScatteredPositions(GetLeadPosition(target, 1300, caster), waveCounts[wave], 140, 50))
					_ = this.ThrowWithPad(caster, skill, originPos.GetNearestPositionWithinDistance(position, 250f), config, PadName.Mon_movetrap);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1300));
		}

		private async Task ThrowWithPad(ICombatEntity caster, Skill skill, Position position, MissileConfig config, string padName)
		{
			await MissileThrow(skill, caster, position, config);
			SkillCreatePad(caster, skill, position, 0f, padName);
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

			var config = new MissileConfig
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
			};

			var waveCounts = new[] { 3, 3, 3 };
			for (var wave = 0; wave < waveCounts.Length; wave++)
			{
				if (wave > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(1500));
				if (!caster.Position.InRange2D(target.Position, 300))
					break;

				foreach (var position in GetScatteredPositions(GetLeadPosition(target, 1300, caster), waveCounts[wave], 150, 70))
					_ = MissileThrow(skill, caster, originPos.GetNearestPositionWithinDistance(position, 250f), config);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1300));
		}
	}
}
