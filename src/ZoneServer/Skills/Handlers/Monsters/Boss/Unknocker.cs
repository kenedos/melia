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
	[SkillHandler(SkillId.Mon_boss_Unknocker_Skill_1)]
	public class Mon_boss_Unknocker_Skill_1 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 30, angle: 25f);
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 100);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 200;
			var damageDelay = 200;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 280, 10, 10, 1, 5, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Unknocker_Skill_2)]
	public class Mon_boss_Unknocker_Skill_2 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 80);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 120f, 20));
			var hits = new List<SkillHitInfo>();

			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force070_coin#Dummy_skl1_effect02", 1.2f),
				EndEffect = new EffectConfig("I_Unknocker_skl2_mash", 1f),
				Range = 15f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			};

			var position = GetRelativePosition(PosType.TargetRandom, caster, target, distance: 80, rand: 130);
			await MissileThrow(skill, caster, position, config, hits);

			var delays = new[] { 250, 250, 250, 250, 250, 250, 250, 150, 100 };
			foreach (var delay in delays)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(delay));
				position = GetRelativePosition(PosType.TargetRandom, caster, target, distance: 80, rand: 140);
				await MissileThrow(skill, caster, position, config, hits);
			}

			SkillResultTargetBuff(caster, skill, BuffId.UC_slowdown, 1, 0f, 4000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Unknocker_Skill_3)]
	public class Mon_boss_Unknocker_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var hits = new List<SkillHitInfo>();

			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 4f),
				PositionDelay = 2000,
				Effect = new EffectConfig("E_Unknocker_skl3", 0.8f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 1800f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 10f,
				InnerRange = 0,
			};

			var delays = new[] { 0, 250, 250, 250, 250, 3500, 0, 0, 0 };
			foreach (var delay in delays)
			{
				if (delay > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(delay));

				var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170);
				await EffectAndHit(skill, caster, position, config, hits);
			}

			SkillResultTargetBuff(caster, skill, BuffId.UC_slowdown, 1, 0f, 4000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Unknocker_Skill_4)]
	public class Mon_boss_Unknocker_Skill_4 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 10));
			await skill.Wait(TimeSpan.FromMilliseconds(4000));

			var smallConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.5f),
				PositionDelay = 1000,
				Effect = new EffectConfig("E_Unknocker_skl3", 0.5f),
				Range = 25f,
				KnockdownPower = 0f,
				Delay = 1800f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 10f,
				InnerRange = 0,
			};

			var largeConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.5f),
				PositionDelay = 1000,
				Effect = new EffectConfig("E_Unknocker_skl3", 0.8f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 1800f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 10f,
				InnerRange = 0,
			};

			for (var i = 0; i < 10; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 230);
				await EffectAndHit(skill, caster, position, smallConfig);
			}

			for (var i = 0; i < 8; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 140);
				await EffectAndHit(skill, caster, position, largeConfig);
			}
		}
	}
}
