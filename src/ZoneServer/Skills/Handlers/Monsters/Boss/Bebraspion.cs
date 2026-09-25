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
using static Melia.Zone.Skills.Helpers.SkillUseHelper;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.SplashAreas;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_bebraspion_Skill_1)]
	public class Mon_boss_bebraspion_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(2700);
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
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), farPos);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			ISplashArea splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 76f, angle: -15f), 40f);
			var hitDelay = 2300;
			var aniTime = 2500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 3000f, 1, 15, -1, hits);
			splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 76f, angle: 9f), 40f);
			hitDelay = 100;
			aniTime = 100;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 3000f, 1, 15, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_bebraspion_Skill_2)]
	public class Mon_boss_bebraspion_Skill_2 : ITargetSkillHandler
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
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), farPos);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 60);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2300;
			var aniTime = 2500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_shock, 1, 0f, 6000f, 1, 10, -1, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits, 20);
		}
	}

	[SkillHandler(SkillId.Mon_boss_bebraspion_Skill_3)]
	public class Mon_boss_bebraspion_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(2650);
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
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), farPos);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			ISplashArea splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 61f, angle: -3f), 65f);
			var hitDelay = 2450;
			var aniTime = 2650;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 3000f, 1, 15, -1, hits);
			var startingPosition = originPos.GetRelative(farPos, distance: 80f);
			var endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0f,
				ArrowLifeTime = 0.02f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_explosion033_green2", 1f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 6000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_bebraspion_Skill_4)]
	public class Mon_boss_bebraspion_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 2f),
				PositionDelay = 1200,
				Effect = new EffectConfig("F_burstup013", 1.5f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};
			var finalConfig = config;
			finalConfig.KnockdownPower = 100f;

			var baseDir = originPos.GetDirection(farPos);
			var angles = new[] { -30f, 10f, -70f, 50f, 0f };
			var delays = new[] { 400, 300, 200, 200 };
			var blasts = new List<Task>();
			for (var i = 0; i < angles.Length; i++)
			{
				if (i > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i - 1]));
				var position = originPos.GetRelative(baseDir.AddDegreeAngle(angles[i]), 60f);
				blasts.Add(this.Blast(caster, skill, position, i == angles.Length - 1 ? finalConfig : config));
			}
			await Task.WhenAll(blasts);
		}

		private async Task Blast(ICombatEntity caster, Skill skill, Position position, EffectHitConfig config)
		{
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_shock, 1, 0f, 6000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_bebraspion_Skill_5)]
	public class Mon_boss_bebraspion_Skill_5 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(3000));
			var spawnPos = originPos.GetRelative(farPos, distance: 81, angle: -45f);
			MonsterSkillCreateMob(skill, caster, "Spion_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 81, angle: -45f);
			MonsterSkillCreateMob(skill, caster, "Spion_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 71, angle: -18f);
			MonsterSkillCreateMob(skill, caster, "Spion_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 98, angle: 20f);
			MonsterSkillCreateMob(skill, caster, "Spion_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
		}
	}

	[SkillHandler(SkillId.Mon_boss_bebraspion_Skill_6)]
	public class Mon_boss_bebraspion_Skill_6 : ITargetSkillHandler
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
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), farPos);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 2f),
				PositionDelay = 1200,
				Effect = new EffectConfig("F_burstup013", 1.5f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};
			var finalConfig = effectHitConfig;
			finalConfig.KnockdownPower = 100f;

			var baseDir = originPos.GetDirection(farPos);
			var angles = new[] { -30f, 10f, -70f, 50f, 0f };
			var delays = new[] { 400, 300, 200, 200 };
			for (var i = 0; i < angles.Length; i++)
			{
				if (i > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i - 1]));
				var position = originPos.GetRelative(baseDir.AddDegreeAngle(angles[i]), 60f);
				_ = EffectAndHit(skill, caster, position, i == angles.Length - 1 ? finalConfig : effectHitConfig);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1400));
			_ = EffectAndHit(skill, caster, originPos.GetRelative(baseDir, 50f), new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 5f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_explosion033_green2", 2f),
				Range = 50f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});

			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var arrowConfig = new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_arrow_monster", 1f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0f,
				ArrowLifeTime = 0.02f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_explosion033_green2", 0.7f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var lines = new List<Task>();
			foreach (var angle in new[] { 0f, -32f, 32f, 60f, -60f })
			{
				var lineDir = baseDir.AddDegreeAngle(angle);
				lines.Add(EffectHitArrow(skill, caster, originPos.GetRelative(lineDir, 80f), originPos.GetRelative(lineDir, 220f), arrowConfig));
			}
			await Task.WhenAll(lines);
		}
	}
}
