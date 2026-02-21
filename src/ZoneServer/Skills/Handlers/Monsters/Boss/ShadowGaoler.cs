using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Shared.Data.Database;
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters
{
	[SkillHandler(SkillId.Mon_boss_ShadowGaoler_Skill_1)]
	public class Mon_boss_ShadowGaoler_Skill_1 : ITargetSkillHandler
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

			skill.Run(this.HandleSkill(skill, caster, originPos, farPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 70, angle: 130f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1300;
			var damageDelay = 100;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_bleed, 1, hits.Sum(h => h.HitInfo.Damage) * 0.3f, 8000f, 1, 40, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 70, angle: 130f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 300;
			damageDelay = 100;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_bleed, 1, hits.Sum(h => h.HitInfo.Damage) * 0.3f, 8000f, 1, 40, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_ShadowGaoler_Skill_2)]
	public class Mon_boss_ShadowGaoler_Skill_2 : ITargetSkillHandler
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

			skill.Run(this.HandleSkill(skill, caster, target, originPos, farPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, ICombatEntity target, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2800));
			var targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.ShadowGaoler_circlewave);
		}
	}

	[SkillHandler(SkillId.Mon_boss_ShadowGaoler_Skill_3)]
	public class Mon_boss_ShadowGaoler_Skill_3 : ITargetSkillHandler
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

			skill.Run(this.HandleSkill(skill, caster, target, originPos, farPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, ICombatEntity target, Position originPos, Position farPos)
		{
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 60);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, position, 200f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.4f),
				PositionDelay = 2300,
				Effect = new EffectConfig("F_ground031_green", 2f),
				Range = 70f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 3,
				HitDuration = 700f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 8000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_ShadowGaoler_Skill_4)]
	public class Mon_boss_ShadowGaoler_Skill_4 : ITargetSkillHandler
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

			skill.Run(this.HandleSkill(skill, caster, target, originPos, farPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, ICombatEntity target, Position originPos, Position farPos)
		{
			var hits = new List<SkillHitInfo>();
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 200f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force013_dark#Dummy_R_hand_effect", 1f),
				EndEffect = new EffectConfig("F_smoke021_green", 0.7f),
				Range = 20f,
				FlyTime = 2.5f,
				DelayTime = 0.1f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config, hits);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(2300));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config, hits);

		}
	}
}
