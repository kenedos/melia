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
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Throneweaver_Skill_1)]
	public class Mon_boss_Throneweaver_Skill_1 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 100);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, position, 100f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force003_green#Bone015_L", 4f),
				EndEffect = new EffectConfig("F_explosion073_ground", 4f),
				Range = 45f,
				FlyTime = 0.4f,
				DelayTime = 0f,
				Gravity = 1f,
				Speed = 1f,
				HitTime = 10f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 3.5f),
			});
			SkillCreatePad(caster, skill, position, 0f, PadName.Monster_Slow);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Throneweaver_Skill_2)]
	public class Mon_boss_Throneweaver_Skill_2 : ITargetSkillHandler
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
			var rnd = RandomProvider.Get();
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(1400));
			for (var i = 0; i < 9; i++)
			{
				var distance = 30 + rnd.Next(30);
				var position = target.Position.GetRandomInRange2D(distance, rnd);
				await MissileThrow(skill, caster, position, new MissileConfig
				{
					Effect = new EffectConfig("I_force011_green#Dummy_effect_tail", 1f),
					EndEffect = new EffectConfig("F_explosion052_green##0.8", 1f),
					Range = 10f,
					FlyTime = 1.2f,
					DelayTime = 0f,
					Gravity = 800f,
					Speed = 1f,
					HitTime = 1000f,
					HitCount = 1,
					GroundEffect = new EffectConfig("None", 2.5f),
				});
				await skill.Wait(TimeSpan.FromMilliseconds(100));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Throneweaver_Skill_3)]
	public class Mon_boss_Throneweaver_Skill_3 : ITargetSkillHandler
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
			var spawnPos = originPos.GetRelative(farPos, distance: 77.427238f);
			MonsterSkillCreateMobPC(skill, caster, "Weaver_summon", spawnPos, 0f, "Swift-footed Weaver", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 56.025826f);
			MonsterSkillCreateMobPC(skill, caster, "Weaver_summon", spawnPos, 0f, "Swift-footed Weaver", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 69.017403f);
			MonsterSkillCreateMobPC(skill, caster, "Weaver_summon", spawnPos, 0f, "Swift-footed Weaver", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 83.855644f);
			MonsterSkillCreateMobPC(skill, caster, "Weaver_summon", spawnPos, 0f, "Swift-footed Weaver", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 77.090309f);
			MonsterSkillCreateMobPC(skill, caster, "Weaver_summon", spawnPos, 0f, "Swift-footed Weaver", "BasicMonster_ATK", 0, 0f, "None", "");
		}
	}

	[SkillHandler(SkillId.Mon_boss_Throneweaver_Skill_4)]
	public class Mon_boss_Throneweaver_Skill_4 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2900);
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
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 40);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2700;
			var damageDelay = 2900;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 60000f, 1, 10, -1, hits);
			var position = originPos.GetRelative(farPos, distance: 30);
			var pad = SkillCreatePad(caster, skill, position, 0f, PadName.Monster_Slow);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(15000);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Throneweaver_Q1_Skill_1)]
	public class Mon_boss_Throneweaver_Q1_Skill_1 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 100);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, position, 100f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force003_green#Bone015_L", 4f),
				EndEffect = new EffectConfig("F_explosion073_ground", 4f),
				Range = 50f,
				FlyTime = 0.4f,
				DelayTime = 0f,
				Gravity = 1f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 3.5f),
			});
			SkillCreatePad(caster, skill, position, 0f, PadName.Monster_Slow);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Throneweaver_Q1_Skill_2)]
	public class Mon_boss_Throneweaver_Q1_Skill_2 : ITargetSkillHandler
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
			var rnd = RandomProvider.Get();
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(1400));
			for (var i = 0; i < 9; i++)
			{
				var distance = 30 + rnd.Next(30);
				var position = target.Position.GetRandomInRange2D(distance, rnd);
				await MissileThrow(skill, caster, position, new MissileConfig
				{
					Effect = new EffectConfig("I_force011_green#Dummy_effect_tail", 1f),
					EndEffect = new EffectConfig("F_explosion052_green##0.8", 1f),
					Range = 10f,
					FlyTime = 1.2f,
					DelayTime = 0f,
					Gravity = 800f,
					Speed = 1f,
					HitTime = 1000f,
					HitCount = 1,
					GroundEffect = new EffectConfig("None", 2.5f),
				});
				await skill.Wait(TimeSpan.FromMilliseconds(100));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Throneweaver_Q1_Skill_3)]
	public class Mon_boss_Throneweaver_Q1_Skill_3 : ITargetSkillHandler
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
			var spawnPos = originPos.GetRelative(farPos, distance: 69.850212f, angle: 90f, rand: 30);
			MonsterSkillCreateMob(skill, caster, "Weaver_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "Weaver_summon", "");
			spawnPos = originPos.GetRelative(farPos, distance: 66.928703f, angle: -88f, rand: 30);
			MonsterSkillCreateMob(skill, caster, "Weaver_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "Weaver_summon", "");
			spawnPos = originPos.GetRelative(farPos, distance: 72.390182f, angle: 0f, rand: 30);
			MonsterSkillCreateMob(skill, caster, "Weaver_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "Weaver_summon", "");
			spawnPos = originPos.GetRelative(farPos, distance: 88.2714f, angle: 182f, rand: 30, height: 1);
			MonsterSkillCreateMob(skill, caster, "Weaver_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "Weaver_summon", "");
			spawnPos = originPos.GetRelative(farPos, distance: 123.14012f, angle: -95f, rand: 30, height: 1);
			MonsterSkillCreateMob(skill, caster, "Weaver_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "Weaver_summon", "");
			spawnPos = originPos.GetRelative(farPos, distance: 120.15373f, angle: 89f, rand: 30, height: 1);
			MonsterSkillCreateMob(skill, caster, "Weaver_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "Weaver_summon", "");
		}
	}

	[SkillHandler(SkillId.Mon_boss_Throneweaver_Q1_Skill_4)]
	public class Mon_boss_Throneweaver_Q1_Skill_4 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2900);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 40);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2700;
			var damageDelay = 2900;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 60000f, 1, 10, -1, hits);
			var position = originPos.GetRelative(farPos, distance: 30);
			var pad = SkillCreatePad(caster, skill, position, 0f, PadName.Monster_Slow);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(15000);
		}
	}
}
