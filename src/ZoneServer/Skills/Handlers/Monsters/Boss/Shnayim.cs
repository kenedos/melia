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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Shnayim_Skill_1)]
	public class Mon_boss_Shnayim_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(1800);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 50);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1600;
			var aniTime = 1800;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 6000f, 1, 10, -1, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits, 20);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Shnayim_Skill_2)]
	public class Mon_boss_Shnayim_Skill_2 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 45f, angle: 2f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3.5f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_burstup022_smoke", 0.6f),
				Range = 40f,
				KnockdownPower = 180f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 6000f, 1, 10, -1, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits, 20);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Shnayim_Skill_3)]
	public class Mon_boss_Shnayim_Skill_3 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			var startingPosition = originPos.GetRelative(farPos, distance: 32.419632f);
			var endingPosition = originPos.GetRelative(farPos, distance: 300f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_arrow_monster##0.2", 1f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.02f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 1000f,
				HitEffect = EffectConfig.None,
				Range = 15f,
				KnockdownPower = 100f,
				Delay = 1000f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.04f,
				HitCount = 1,
				HitDuration = 0f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 6000f, 1, 50, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Shnayim_Skill_4)]
	public class Mon_boss_Shnayim_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			var spawnPos = originPos.GetRelative(farPos, distance: 66, angle: -1f);
			MonsterSkillCreateMob(skill, caster, "shtayim_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			spawnPos = originPos.GetRelative(farPos, distance: 80, angle: 123f);
			MonsterSkillCreateMob(skill, caster, "shtayim_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			spawnPos = originPos.GetRelative(farPos, distance: 76, angle: -148f);
			MonsterSkillCreateMob(skill, caster, "shtayim_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			spawnPos = originPos.GetRelative(farPos, distance: 94, angle: 56f);
			MonsterSkillCreateMob(skill, caster, "shtayim_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			spawnPos = originPos.GetRelative(farPos, distance: 88, angle: -69f);
			MonsterSkillCreateMob(skill, caster, "shtayim_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
		}
	}

	[SkillHandler(SkillId.Mon_boss_Shnayim_Skill_5)]
	public class Mon_boss_Shnayim_Skill_5 : ITargetSkillHandler
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
			var baseDir = originPos.GetDirection(farPos);
			var slamPos = originPos.GetRelative(baseDir, 59f);

			var slamConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.3", 3.5f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_ground058_smoke", 1f),
				Range = 40f,
				KnockdownPower = 180f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};
			var slamConfig2 = slamConfig;
			slamConfig2.GroundEffect = new EffectConfig("F_sys_target_boss##0.3", 4f);
			slamConfig2.Effect = new EffectConfig("F_ground058_smoke", 1.6f);
			slamConfig2.Range = 60f;
			var slamConfig3 = slamConfig;
			slamConfig3.GroundEffect = new EffectConfig("F_sys_target_boss##0.3", 6f);
			slamConfig3.Effect = new EffectConfig("F_ground058_smoke", 2.5f);
			slamConfig3.Range = 80f;

			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.3", 2f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_rize001", 0.5f),
				Range = 20f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 2,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};
			var effectHitConfig2 = effectHitConfig;
			effectHitConfig2.Effect = new EffectConfig("F_rize001##0.3", 0.55f);
			effectHitConfig2.Range = 30f;
			var effectHitConfig3 = effectHitConfig;
			effectHitConfig3.GroundEffect = new EffectConfig("F_sys_target_boss##0.3", 3.5f);
			effectHitConfig3.Effect = new EffectConfig("F_rize001##0.3", 0.8f);
			effectHitConfig3.Range = 40f;
			effectHitConfig3.KnockdownPower = 100f;

			var schedule = new (int Time, EffectHitConfig Config, bool Slam)[]
			{
				(0, slamConfig, true),
				(1000, effectHitConfig, false), (1200, effectHitConfig, false), (1400, effectHitConfig, false), (1600, effectHitConfig, false), (1800, effectHitConfig, false),
				(2300, slamConfig2, true),
				(3500, effectHitConfig2, false), (3500, effectHitConfig2, false), (3700, effectHitConfig2, false), (3800, effectHitConfig2, false), (4000, effectHitConfig2, false),
				(5700, slamConfig3, true),
				(7000, effectHitConfig3, false), (7200, effectHitConfig3, false), (7400, effectHitConfig3, false), (7600, effectHitConfig3, false), (7800, effectHitConfig3, false),
			};

			var elapsed = 0;
			var tasks = new List<Task>();
			foreach (var entry in schedule)
			{
				if (entry.Time > elapsed)
				{
					await skill.Wait(TimeSpan.FromMilliseconds(entry.Time - elapsed));
					elapsed = entry.Time;
				}

				if (entry.Slam)
				{
					tasks.Add(this.Blast(caster, skill, slamPos, entry.Config));
					continue;
				}

				if (!caster.Position.InRange2D(target.Position, 300))
					continue;

				var position = GetLeadPositionScatter(target, 1200, 60, caster);
				tasks.Add(this.Blast(caster, skill, originPos.GetNearestPositionWithinDistance(position, 250f), entry.Config));
			}
			await Task.WhenAll(tasks);
		}

		private async Task Blast(ICombatEntity caster, Skill skill, Position position, EffectHitConfig config)
		{
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 6000f, 1, 20, -1, hits);
		}
	}
}
