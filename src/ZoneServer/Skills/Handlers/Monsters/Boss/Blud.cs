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
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Blud_Skill_1)]
	public class Mon_boss_Blud_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1200);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 45, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Blud_Skill_2)]
	public class Mon_boss_Blud_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2500);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 70, width: 40, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(2300));

			var arrowConfig = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_burstup003_1", 0.8f),
				Range = 30f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitEffectSpacing = 28f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			for (var i = 0; i < 3; i++)
			{
				var startingPosition = originPos.GetRelative(farPos, distance: 40f);
				var endingPosition = originPos.GetRelative(farPos, distance: 200f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Blud_Skill_3)]
	public class Mon_boss_Blud_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2100));

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force039_orange#Bip001 Ponytail2", 1f),
				EndEffect = new EffectConfig("I_explosion009_orange", 1f),
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			};

			var position = originPos.GetRelative(farPos, distance: 161, angle: -58f, rand: 90, height: 2);
			await MissilePadThrow(skill, caster, position, missileConfig, 0f, "Mon_PoisonPilla_orange");
			position = GetRelativePosition(PosType.TargetRandom, caster, target, distance: 155.05614, angle: 0f, rand: 130, height: 2);
			await MissilePadThrow(skill, caster, position, missileConfig, 0f, "Mon_PoisonPilla_orange");
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 117.14211, angle: 100f, rand: 130, height: 2);
			await MissilePadThrow(skill, caster, position, missileConfig, 0f, "Mon_PoisonPilla_orange");
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 157.14589, angle: 28f, rand: 120, height: 2);
			await MissilePadThrow(skill, caster, position, missileConfig, 0f, "Mon_PoisonPilla_orange");
			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 100, height: 2);
				await MissilePadThrow(skill, caster, position, missileConfig, 0f, "Mon_PoisonPilla_orange");
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Blud_Skill_4)]
	public class Mon_boss_Blud_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2100));

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force039_orange#Bip001 Ponytail2", 1f),
				EndEffect = new EffectConfig("I_explosion009_orange", 1f),
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			};

			var position = originPos.GetRelative(farPos, distance: 161, angle: -58f, rand: 90, height: 2);
			await MissilePadThrow(skill, caster, position, missileConfig, 0f, "Mon_PoisonPilla_orange");
			position = GetRelativePosition(PosType.TargetRandom, caster, target, distance: 155.05614, angle: 0f, rand: 130, height: 2);
			await MissilePadThrow(skill, caster, position, missileConfig, 0f, "Mon_PoisonPilla_orange");
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 117.14211, angle: 100f, rand: 130, height: 2);
			await MissilePadThrow(skill, caster, position, missileConfig, 0f, "Mon_PoisonPilla_orange");
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 157.14589, angle: 28f, rand: 120, height: 2);
			await MissilePadThrow(skill, caster, position, missileConfig, 0f, "Mon_PoisonPilla_orange");
			for (var i = 0; i < 4; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 100, height: 2);
				await MissilePadThrow(skill, caster, position, missileConfig, 0f, "Mon_PoisonPilla_orange");
			}
			await skill.Wait(TimeSpan.FromMilliseconds(1900));
			for (var i = 0; i < 8; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 100, height: 2);
				await MissilePadThrow(skill, caster, position, missileConfig, 0f, "Mon_PoisonPilla_orange");
			}
		}
	}
}
