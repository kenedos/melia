using System;
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
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_AWK_boss_PantoRex_Skill_1)]
	public class Mon_AWK_boss_PantoRex_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1500);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 40f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1300;
			var damageDelay = 1500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_AWK_boss_PantoRex_Skill_2)]
	public class Mon_AWK_boss_PantoRex_Skill_2 : ITargetSkillHandler
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
			var config = new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_ground074_dark##1", 0.6f),
				ArrowSpacing = 30f,
				ArrowSpacingTime = 0.05f,
				ArrowLifeTime = 1f,
				PositionDelay = 500f,
				HitEffect = new EffectConfig("F_burstup029_smoke_grey", 0.8f),
				Range = 25f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			for (var i = 0; i < 5; i++)
			{
				var startingPosition = originPos.GetRelative(farPos, distance: 20f);
				var endingPosition = originPos.GetRelative(farPos, distance: 250f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_AWK_boss_PantoRex_Skill_3)]
	public class Mon_AWK_boss_PantoRex_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2800));
			var position = originPos.GetRelative(farPos, distance: 10);
			MonsterSkillPadLookDirMissile(caster, skill, position, PadName.boss_pnatorex_missile, 200f, 3, 250f, 0f, 250f, 0);
		}
	}

	[SkillHandler(SkillId.Mon_AWK_boss_PantoRex_Skill_4)]
	public class Mon_AWK_boss_PantoRex_Skill_4 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, height: 1);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 200f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1600));
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.5f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup029_smoke_dark2", 1f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			var effectHitConfig2 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.5f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup029_smoke_dark2", 1f),
				Range = 10f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			};

			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
		}
	}
}
