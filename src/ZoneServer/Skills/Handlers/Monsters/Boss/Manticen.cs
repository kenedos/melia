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
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Manticen_Skill_1)]
	public class Mon_boss_Manticen_Skill_1 : ITargetSkillHandler
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
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 80, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Manticen_Skill_2)]
	public class Mon_boss_Manticen_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var hits = new List<SkillHitInfo>();
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_light014_red#R_Antenna_00", 3f),
				EndEffect = new EffectConfig("F_explosion097", 0.3f),
				DotEffect = EffectConfig.None,
				Range = 20f,
				FlyTime = 0.3f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.4f),
			};

			var position = originPos.GetRelative(farPos, distance: 100, rand: 50, height: 1);
			await MissileThrow(skill, caster, position, config);
			SkillResultTargetBuff(caster, skill, BuffId.CriticalWound_Mon, 1, hits.Sum(h => h.HitInfo.Damage) * 0.6f, 5000f, 1, 20, -1, hits);
			position = originPos.GetRelative(farPos, distance: 100, rand: 50, height: 1);
			hits.Clear();
			await MissileThrow(skill, caster, position, config);
			SkillResultTargetBuff(caster, skill, BuffId.CriticalWound_Mon, 1, hits.Sum(h => h.HitInfo.Damage) * 0.6f, 5000f, 1, 20, -1, hits);
			position = originPos.GetRelative(farPos, distance: 100, rand: 50, height: 1);
			hits.Clear();
			await MissileThrow(skill, caster, position, config);
			SkillResultTargetBuff(caster, skill, BuffId.CriticalWound_Mon, 1, hits.Sum(h => h.HitInfo.Damage) * 0.6f, 5000f, 1, 20, -1, hits);
			position = originPos.GetRelative(farPos, distance: 100, rand: 50, height: 1);
			hits.Clear();
			await MissileThrow(skill, caster, position, config);
			SkillResultTargetBuff(caster, skill, BuffId.CriticalWound_Mon, 1, hits.Sum(h => h.HitInfo.Damage) * 0.6f, 5000f, 1, 20, -1, hits);
			position = originPos.GetRelative(farPos, distance: 100, rand: 50, height: 1);
			hits.Clear();
			await MissileThrow(skill, caster, position, config);
			SkillResultTargetBuff(caster, skill, BuffId.CriticalWound_Mon, 1, hits.Sum(h => h.HitInfo.Damage) * 0.6f, 5000f, 1, 20, -1, hits);
			position = originPos.GetRelative(farPos, distance: 100, rand: 50, height: 1);
			hits.Clear();
			await MissileThrow(skill, caster, position, config);
			SkillResultTargetBuff(caster, skill, BuffId.CriticalWound_Mon, 1, hits.Sum(h => h.HitInfo.Damage) * 0.6f, 5000f, 1, 20, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Manticen_Skill_3)]
	public class Mon_boss_Manticen_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2200));
			var position = originPos.GetRelative(farPos, distance: 25);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup045", 2f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_ground121", 1.5f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.08f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var startingPosition = originPos.GetRelative(farPos, distance: 25f);
			var endingPosition = originPos.GetRelative(farPos, distance: 110f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos);
			endingPosition = originPos.GetRelative(farPos);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 43.299999f);
			endingPosition = originPos.GetRelative(farPos, distance: 130f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 25f);
			endingPosition = originPos.GetRelative(farPos, distance: 110f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 50f);
			endingPosition = originPos.GetRelative(farPos, distance: 150f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 43.299999f);
			endingPosition = originPos.GetRelative(farPos, distance: 130f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Manticen_Skill_4)]
	public class Mon_boss_Manticen_Skill_4 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2200);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 20, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 2000;
			var damageDelay = 2200;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 20, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 200;
			damageDelay = 2400;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 20, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 600;
			damageDelay = 3000;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.CriticalWound_Mon, 1, hits.Sum(h => h.HitInfo.Damage) * 0.6f, 5000f, 1, 30, -1, hits);
		}
	}
}
