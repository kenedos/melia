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
using Yggdrasil.Util;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Carnivore_Skill_1)]
	public class Mon_boss_Carnivore_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 45, angle: 150f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 900;
			var damageDelay = 900;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 7000f, 1, 100, -1, hits);
			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Carnivore_Skill_2)]
	public class Mon_boss_Carnivore_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 100f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(2100));

			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_smoke048#Dummy_effect_skl1", 0.8f),
				EndEffect = new EffectConfig("F_smoke052_green", 0.6f),
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 1f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster##0.8", 0.5f),
			};

			for (var i = 0; i < 8; i++)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 70);
				await MissileThrow(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Carnivore_Skill_3)]
	public class Mon_boss_Carnivore_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(200);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(3400));
			var hits = new List<SkillHitInfo>();

			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster##0.7", 0.7f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup022_smoke", 0.7f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			for (var i = 0; i < 25; i++)
			{
				var position = originPos.GetNearestPositionWithinDistance(target.Position, 250f);
				await EffectAndHit(skill, caster, position, config, hits);
				if (i < 24)
					await skill.Wait(TimeSpan.FromMilliseconds(300));
			}

			SkillResultTargetBuff(caster, skill, BuffId.UC_fear, 1, 0f, 4000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Carnivore_Skill_4)]
	public class Mon_boss_Carnivore_Skill_4 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(500));

			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_smoke048#Dummy_effect_skl1", 0.7f),
				EndEffect = new EffectConfig("F_explosion001_green6##0.5", 1.5f),
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0.2f,
				Gravity = 500f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 0.5f),
			};

			for (var i = 0; i < 14; i++)
			{
				var position = originPos.GetNearestPositionWithinDistance(target.Position, 250f);
				await MissileThrow(skill, caster, position, config);
				if (i < 13)
					await skill.Wait(TimeSpan.FromMilliseconds(300));
			}

			//SkillResultTargetBuff(caster, skill, BuffId.UC_fear, 1, 0f, 4000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Carnivore_Skill_5)]
	public class Mon_boss_Carnivore_Skill_5 : ITargetSkillHandler
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
			var spawnPos = originPos.GetRelative(farPos, distance: 76.078445f);
			MonsterSkillCreateMob(skill, caster, "Zignuts", spawnPos, 0f, "지그너츠", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			spawnPos = originPos.GetRelative(farPos, distance: 113.01619f);
			MonsterSkillCreateMob(skill, caster, "Zignuts", spawnPos, 0f, "지그너츠", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			spawnPos = originPos.GetRelative(farPos, distance: 81.225784f);
			MonsterSkillCreateMob(skill, caster, "Zignuts", spawnPos, 0f, "지그너츠", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			spawnPos = originPos.GetRelative(farPos, distance: 86.722321f);
			MonsterSkillCreateMob(skill, caster, "Zignuts", spawnPos, 0f, "지그너츠", "BasicMonster_ATK", 0, 0f, "None", "");
		}
	}

	[SkillHandler(SkillId.Mon_boss_Carnivore_Skill_6)]
	public class Mon_boss_Carnivore_Skill_6 : ITargetSkillHandler
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
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 180f, 5));
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var position = GetRelativePosition(PosType.Target, caster, target);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force011_green#머리", 1.5f),
				EndEffect = new EffectConfig("F_explosion009_green", 2f),
				Range = 50f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 5f),
			});
		}
	}

}
