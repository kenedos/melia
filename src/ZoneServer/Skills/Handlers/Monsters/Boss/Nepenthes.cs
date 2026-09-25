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
using Yggdrasil.Geometry.Shapes;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Nepenthes_Skill_1)]
	public class Mon_boss_Nepenthes_Skill_1 : ITargetSkillHandler
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
			var splashArea = new CircleF(originPos.GetRelative(farPos, distance: 40f), 30f);
			var hitDelay = 2000;
			var aniTime = 2000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_sleep, 1, 0f, 5000f, 1, 15, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Nepenthes_Skill_2)]
	public class Mon_boss_Nepenthes_Skill_2 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 30);
			var targets = SkillSelectEnemiesInCircle(caster, targetPos, 25f, 5);
			if (targets.Count == 0)
				return;
			SkillTargetDamage(skill, caster, targets, 3f);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			SkillTargetBuff(skill, caster, targets, BuffId.Mon_Throw, 1f, 0f, TimeSpan.FromMilliseconds(1900f));
			await skill.Wait(TimeSpan.FromMilliseconds(1950));
			await skill.Wait(TimeSpan.FromMilliseconds(1200));
			SkillTargetKnockdown(caster, skill, targets, KnockType.KnockDown, KnockDirection.CasterForward, 150f, 55f, 0f, 3, 5);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Nepenthes_Skill_3)]
	public class Mon_boss_Nepenthes_Skill_3 : ITargetSkillHandler
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
			_ = MonsterSkillFollowMovePath(caster, skill, (2200, 6f, 151f), (2900, 86f, 0f), (3400, 172f, 0f));

			await skill.Wait(TimeSpan.FromMilliseconds(1000));

			var startingPosition = originPos.GetRelative(farPos, distance: 30f);
			var endingPosition = caster.Map.Ground.GetLastValidPosition(originPos, originPos.GetRelative(farPos, distance: 180f));
			var arrow = EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 20f,
				ArrowSpacingTime = 0.03f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 1500f,
				HitEffect = new EffectConfig("F_smoke129_spreadout", 0.4f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.2f,
				HitCount = 1,
				HitDuration = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			MonsterSkillSetCollisionDamage(caster, skill, true, 1f);
			await skill.Wait(TimeSpan.FromMilliseconds(1700));
			MonsterSkillSetCollisionDamage(caster, skill, false, 1f);
			await arrow;
		}
	}

	[SkillHandler(SkillId.Mon_boss_Nepenthes_Skill_4)]
	public class Mon_boss_Nepenthes_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			var spawnPos = originPos.GetRelative(farPos, distance: 127, angle: 89f);
			MonsterSkillCreateMob(skill, caster, "Mallardu_summon", spawnPos, 0f, "", "BasicMonster_ATK", -10, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 90, angle: -90f);
			MonsterSkillCreateMob(skill, caster, "seedmia_summon", spawnPos, 0f, "", "BasicMonster_ATK", -10, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 117);
			MonsterSkillCreateMob(skill, caster, "seedmia_summon", spawnPos, 0f, "", "BasicMonster_ATK", -10, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 100, angle: -180f);
			MonsterSkillCreateMob(skill, caster, "Mallardu_summon", spawnPos, 0f, "", "BasicMonster_ATK", -10, 0f, "None", "");
			var matk = caster.Properties.GetFloat(PropertyName.MINMATK) + caster.Properties.GetFloat(PropertyName.MAXMATK) / 2;
			caster.StartBuff(BuffId.Mon_Heal_Buff, skill.Level, matk, TimeSpan.FromMilliseconds(10000f), caster);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Nepenthes_Skill_5)]
	public class Mon_boss_Nepenthes_Skill_5 : ITargetSkillHandler
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
			if (!caster.Position.InRange2D(target.Position, 300))
				return;

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("F_smoke128_green1#Spine_02_Nub", 0.5f),
				EndEffect = new EffectConfig("F_explosion001_green7", 2f),
				Range = 20f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 300f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.8f),
			};

			foreach (var position in GetScatteredPositions(GetLeadPosition(target, 1000, caster), 4, 120, 55))
				_ = MissileThrow(skill, caster, originPos.GetNearestPositionWithinDistance(position, 250f), missileConfig);

			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			if (!caster.Position.InRange2D(target.Position, 300))
				return;

			var missileConfig2 = new MissileConfig
			{
				Effect = new EffectConfig("F_smoke128_green3#Spine_02_Nub", 0.5f),
				EndEffect = new EffectConfig("F_explosion001_green7", 1.3f),
				Range = 20f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 300f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.8f),
			};

			foreach (var position in GetScatteredPositions(GetLeadPosition(target, 1000, caster), 5, 120, 55))
				_ = MissileThrow(skill, caster, originPos.GetNearestPositionWithinDistance(position, 250f), missileConfig2);

			await skill.Wait(TimeSpan.FromMilliseconds(1000));
		}
	}

	[SkillHandler(SkillId.Mon_boss_Nepenthes_Skill_6)]
	public class Mon_boss_Nepenthes_Skill_6 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(1500);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 1, width: 150, angle: 50f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1300;
			var aniTime = 1500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_confuse, 1, 0f, 3000f, 1, 15, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 1, width: 150, angle: 50f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 1500;
			aniTime = 1500;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_confuse, 1, 0f, 3000f, 1, 15, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 1, width: 150, angle: 50f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 2000;
			aniTime = 2000;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_confuse, 1, 0f, 3000f, 1, 15, -1, hits);
			await EffectAndHit(skill, caster, originPos, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("I_explosion002_green", 1f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0,
			});
		}
	}
}
