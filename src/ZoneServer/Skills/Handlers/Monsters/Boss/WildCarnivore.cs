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
using Yggdrasil.Geometry.Shapes;
using Melia.Zone.World.Actors.Monsters;

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
			var splashArea = new CircleF(originPos.GetRelative(farPos, distance: 55f, angle: 2f), 20f);
			var hitDelay = 2200;
			var aniTime = 2400;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 7000f, 1, 40, -1, hits);
			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits, 20);
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), farPos);
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

			if (!caster.Position.InRange2D(target.Position, 300))
				return;

			foreach (var position in GetScatteredPositions(GetLeadPosition(target, 1000, caster), 8, 90, 40))
				_ = MissileThrow(skill, caster, originPos.GetNearestPositionWithinDistance(position, 250f), config);

			await skill.Wait(TimeSpan.FromMilliseconds(1000));
		}
	}

	[SkillHandler(SkillId.Mon_boss_Carnivore_Skill_3)]
	public class Mon_boss_Carnivore_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(200);
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(3400));

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

			for (var i = 0; i < 14; i++)
			{
				if (i > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(300));
				if (!caster.Position.InRange2D(target.Position, 300))
					break;

				var position = GetLeadPositionScatter(target, 1200, 40, caster);
				_ = this.Root(caster, skill, originPos.GetNearestPositionWithinDistance(position, 250f), config);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1200));
		}

		private async Task Root(ICombatEntity caster, Skill skill, Position position, EffectHitConfig config)
		{
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_fear, 1, 0f, 4000f, 1, 5, -1, hits);
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

			for (var i = 0; i < 10; i++)
			{
				if (i > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(300));
				if (!caster.Position.InRange2D(target.Position, 300))
					break;

				var position = GetLeadPositionScatter(target, 1200, 50, caster);
				_ = MissileThrow(skill, caster, originPos.GetNearestPositionWithinDistance(position, 250f), config);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1200));

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
			var spawnPos = originPos.GetRelative(farPos, distance: 76f, angle: 81f);
			MonsterSkillCreateMob(skill, caster, "Zignuts", spawnPos, 0f, "지그너츠", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			spawnPos = originPos.GetRelative(farPos, distance: 113f, angle: -85f);
			MonsterSkillCreateMob(skill, caster, "Zignuts", spawnPos, 0f, "지그너츠", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			spawnPos = originPos.GetRelative(farPos, distance: 81f, angle: -36f);
			MonsterSkillCreateMob(skill, caster, "Zignuts", spawnPos, 0f, "지그너츠", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			spawnPos = originPos.GetRelative(farPos, distance: 87f, angle: 36f);
			MonsterSkillCreateMob(skill, caster, "Zignuts", spawnPos, 0f, "지그너츠", "BasicMonster_ATK", 0, 0f, "None", "");
		}
	}

	[SkillHandler(SkillId.Mon_boss_Carnivore_Skill_6)]
	public class Mon_boss_Carnivore_Skill_6 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.Zero;
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
