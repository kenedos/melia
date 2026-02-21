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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_necrovanter_Skill_1)]
	public class Mon_boss_necrovanter_Skill_1 : ITargetSkillHandler
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
			var startingPosition = originPos;
			var endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.02f,
				ArrowLifeTime = 0.02f,
				PositionDelay = 1300f,
				HitEffect = new EffectConfig("F_burstup022_smoke", 0.5f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_blind, 1, 0f, 6000f, 1, 5, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_necrovanter_Skill_2)]
	public class Mon_boss_necrovanter_Skill_2 : ITargetSkillHandler
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

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			var targetHandle = target.Handle;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var hits = new List<SkillHitInfo>();
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.2f),
				PositionDelay = 1200,
				Effect = new EffectConfig("F_smoke007_dark", 1f),
				Range = 35f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = originPos.GetRelative(farPos, distance: 60, angle: 74f);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			position = originPos.GetRelative(farPos, distance: 60);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			position = originPos.GetRelative(farPos, distance: 60, angle: -74f);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(1100));
			var effectHitConfig2 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.2f),
				PositionDelay = 800,
				Effect = new EffectConfig("F_rize004_dark", 2f),
				Range = 35f,
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

			position = originPos.GetRelative(farPos, distance: 60, angle: 74f);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			position = originPos.GetRelative(farPos, distance: 60, angle: -74f);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			position = originPos.GetRelative(farPos, distance: 60);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_blind, 1, 0f, 6000f, 1, 20, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_necrovanter_Skill_3)]
	public class Mon_boss_necrovanter_Skill_3 : ITargetSkillHandler
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

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			var targetHandle = target.Handle;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var hits = new List<SkillHitInfo>();
			var startingPosition = originPos;
			var endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.1f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 500f,
				HitEffect = new EffectConfig("F_burstup004_dark", 0.8f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_blind, 1, 0f, 6000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_necrovanter_Skill_4)]
	public class Mon_boss_necrovanter_Skill_4 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 200f, 30));
			await skill.Wait(TimeSpan.FromMilliseconds(1400));
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("I_spread_out011_dark", 2f),
				PositionDelay = 1300,
				Effect = new EffectConfig("F_burstup004_dark", 1f),
				Range = 30f,
				KnockdownPower = 150f,
				Delay = 600f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(1600));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
		}
	}
}
