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
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_ginklas_Skill_1)]
	public class Mon_boss_ginklas_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(3700);
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
			var hitDelay = 2800;
			var damageDelay = 2800;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 80);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 250;
			damageDelay = 250;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			var targetPos = originPos.GetRelative(farPos);
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

			Position position;

			for (var i = 0; i < 6; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, angle: 45f, rand: 60);
				await MissileThrow(skill, caster, position, config);
			}
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, angle: 45f, rand: 60);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 80);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 250;
			damageDelay = 250;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 80);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 250;
			damageDelay = 250;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 80);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 250;
			damageDelay = 250;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 80);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 250;
			damageDelay = 250;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 80);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 250;
			damageDelay = 250;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
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
			var position = originPos.GetRelative(farPos, distance: 20);
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
			SkillResultTargetBuff(caster, skill, BuffId.UC_flame, 1, hits.Sum(h => h.HitInfo.Damage) * 0.5f, 12000f, 1, 100, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 1000f, 1, 100, -1, hits);
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
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var hits = new List<SkillHitInfo>();
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

			var position = originPos.GetRelative(farPos, distance: 10);
			await EffectAndHit(skill, caster, position, config, hits);
			position = originPos.GetRelative(farPos, distance: 10);
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_flame, 1, 2f, 12000f, 1, 100, -1, hits);
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

			for (var i = 0; i < 5; i++)
			{
				var position = originPos.GetNearestPositionWithinDistance(target.Position, 150f);
				await MissileThrow(skill, caster, position, config);
			}
		}
	}
}
