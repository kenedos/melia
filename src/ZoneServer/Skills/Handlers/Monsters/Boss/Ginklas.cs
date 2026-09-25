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
using Yggdrasil.Geometry;
using Yggdrasil.Geometry.Shapes;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_ginklas_Skill_1)]
	public class Mon_boss_ginklas_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(3700);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 80);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			_ = this.Slam(caster, skill, splashArea, 3500);
			_ = this.Slam(caster, skill, splashArea, 9000);

			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force054_fire#Dummy003", 1.3f),
				EndEffect = new EffectConfig("F_ground021_fire", 3.5f),
				Range = 15f,
				FlyTime = 0.7f,
				DelayTime = 0f,
				Gravity = 800f,
				Speed = 1f,
				HitTime = 1f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 1.5f),
			};

			var waits = new[] { 2300, 300, 300, 300, 300, 300, 300, 300, 300, 2100, 2250, 300, 300, 300, 300, 300, 300 };
			foreach (var wait in waits)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(wait));
				if (!caster.Position.InRange2D(target.Position, 300))
					continue;

				var position = GetLeadPositionScatter(target, 700, 50, caster);
				_ = MissileThrow(skill, caster, originPos.GetNearestPositionWithinDistance(position, 250f), config);
			}
		}

		private async Task Slam(ICombatEntity caster, Skill skill, IShapeF splashArea, int hitDelay)
		{
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, hitDelay + 200, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits, 20);
		}
	}

	[SkillHandler(SkillId.Mon_boss_ginklas_Skill_2)]
	public class Mon_boss_ginklas_Skill_2 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 49f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.2f),
				PositionDelay = 1350,
				Effect = new EffectConfig("F_explosion050_fire", 1f),
				Range = 50f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_flame, 1, hits.Sum(h => h.HitInfo.Damage) * 0.5f, 12000f, 1, 40, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 1000f, 1, 15, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_ginklas_Skill_3)]
	public class Mon_boss_ginklas_Skill_3 : ITargetSkillHandler
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
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 3f),
				PositionDelay = 1700,
				Effect = new EffectConfig("F_ground102_fire", 1f),
				Range = 45f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var baseDir = originPos.GetDirection(farPos);
			var blasts = new List<Task>();
			foreach (var angle in new[] { 35f, -35f })
				blasts.Add(this.Blast(caster, skill, originPos.GetRelative(baseDir.AddDegreeAngle(angle), 50f), config));
			await Task.WhenAll(blasts);
		}

		private async Task Blast(ICombatEntity caster, Skill skill, Position position, EffectHitConfig config)
		{
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_flame, 1, 2f, 12000f, 1, 30, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_ginklas_Skill_4)]
	public class Mon_boss_ginklas_Skill_4 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 81.315224f);
			await skill.Wait(TimeSpan.FromMilliseconds(1200));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force054_fire#Bone006(mirrored)", 1f),
				EndEffect = new EffectConfig("F_ground021_fire", 3f),
				Range = 15f,
				FlyTime = 0.8f,
				DelayTime = 0f,
				Gravity = 1f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 1),
			};

			for (var wave = 0; wave < 2; wave++)
			{
				if (wave > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(2100));
				if (!caster.Position.InRange2D(target.Position, 300))
					break;

				foreach (var position in GetScatteredPositions(GetLeadPosition(target, 800, caster), 5, 100, 40))
					_ = MissileThrow(skill, caster, originPos.GetNearestPositionWithinDistance(position, 200f), config);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1000));
		}
	}
}
