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
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Glass_mole_Skill_1)]
	public class Mon_boss_Glass_mole_Skill_1 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 62.570965f, angle: 0f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster", 1.2f),
				PositionDelay = 1500,
				Effect = EffectConfig.None,
				Range = 35f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Glass_mole_Skill_2)]
	public class Mon_boss_Glass_mole_Skill_2 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 50);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2700;
			var damageDelay = 2900;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			var startingPosition = originPos.GetRelative(farPos, distance: 70f);
			var endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_arrow_monster", 1f),
				ArrowSpacing = 20f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 2200f,
				HitEffect = EffectConfig.None,
				Range = 24f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.06f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Stun, 1, 0f, 2000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Glass_mole_Skill_3)]
	public class Mon_boss_Glass_mole_Skill_3 : ITargetSkillHandler
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
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(500));

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_glassmole_skl1_mash_down", 1f),
				EndEffect = new EffectConfig("E_glassmole_skl1_obj", 1f),
				DotEffect = EffectConfig.None,
				Range = 15f,
				DelayTime = 2.5f,
				FlyTime = 0.3f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = new EffectConfig("F_sys_target_monster", 0.40000001f),
			};

			var delays = new[] { 150, 150, 150, 150, 150, 150, 150, 150, 150 };
			var position = originPos.GetRelative(farPos, rand: 80, height: 1);
			await MissileFall(caster, skill, position, missileConfig);
			foreach (var delay in delays)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(delay));
				position = originPos.GetRelative(farPos, rand: 80, height: 1);
				await MissileFall(caster, skill, position, missileConfig);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(250));
			caster.StartBuff(BuffId.Mole_Buff, 1f, 0f, TimeSpan.FromMilliseconds(10000f), caster);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Glass_mole_Skill_4)]
	public class Mon_boss_Glass_mole_Skill_4 : ITargetSkillHandler
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
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(500));

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_glassmole_skl1_mash_down", 1f),
				EndEffect = new EffectConfig("E_glassmole_skl1_obj", 1f),
				DotEffect = EffectConfig.None,
				Range = 15f,
				DelayTime = 2.5f,
				FlyTime = 0.3f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = new EffectConfig("F_sys_target_monster", 0.40000001f),
			};

			var delays = new[] { 150, 150, 150, 150, 150, 150, 150, 150, 150 };
			var position = originPos.GetRelative(farPos, rand: 80, height: 1);
			await MissileFall(caster, skill, position, missileConfig);
			foreach (var delay in delays)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(delay));
				position = originPos.GetRelative(farPos, rand: 80, height: 1);
				await MissileFall(caster, skill, position, missileConfig);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1650));
			targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(500));

			var delays2 = new[] { 150, 150, 150, 150 };
			position = originPos.GetRelative(farPos, rand: 80, height: 1);
			await MissileFall(caster, skill, position, missileConfig);
			foreach (var delay in delays2)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(delay));
				position = originPos.GetRelative(farPos, rand: 80, height: 1);
				await MissileFall(caster, skill, position, missileConfig);
			}
		}
	}
}
