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
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using Yggdrasil.Util;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Sparnanman_Skill_1)]
	public class Mon_boss_Sparnanman_Skill_1 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1300));
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 73.41925f, angle: -1f, height: -40);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 4f),
				PositionDelay = 1000,
				Effect = new EffectConfig("None", 3f),
				Range = 40f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 4000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Sparnanman_Skill_2)]
	public class Mon_boss_Sparnanman_Skill_2 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_smoke011_smoke#Bip001 L Finger0Nub", 0.5f),
				EndEffect = new EffectConfig("F_smoke025_blue##1", 0.4f),
				Range = 20f,
				FlyTime = (0.7f + (float)RandomProvider.Get().NextDouble() * 1.8f),
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			var missileConfig2 = new MissileConfig
			{
				Effect = new EffectConfig("I_smoke001_dark_3#Bip001 R Finger0Nub", 0.8f),
				EndEffect = new EffectConfig("F_ground092_dark", 2f),
				Range = 20f,
				FlyTime = (0.7f + (float)RandomProvider.Get().NextDouble() * 1.8f),
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			};

			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 4000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Sparnanman_Skill_3)]
	public class Mon_boss_Sparnanman_Skill_3 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			await skill.Wait(TimeSpan.FromMilliseconds(2100));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_cleric_hexing_cast_dark#Bip001 L Hand", 1.5f),
				EndEffect = new EffectConfig("F_burstup003_brown", 2.5f),
				DotEffect = EffectConfig.None,
				Range = 50f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var position = originPos.GetRelative(farPos, distance: 50);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(700));
			position = originPos.GetRelative(farPos, distance: 50);
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_Sparnanman, 0f, 150f, 6, 315f, 150f, 50f, 0f);
			await skill.Wait(TimeSpan.FromMilliseconds(1700));
			position = originPos.GetRelative(farPos, distance: 50);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			position = originPos.GetRelative(farPos, distance: 50);
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_Sparnanman, 2.0943952f, 150f, 6, 315f, 150f, 50f, 0f);
			await skill.Wait(TimeSpan.FromMilliseconds(1600));
			position = originPos.GetRelative(farPos, distance: 50);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_cleric_hexing_cast_dark#Bip001 L Hand", 1.5f),
				EndEffect = new EffectConfig("F_burstup003_brown", 2.5f),
				Range = 50f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(700));
			position = originPos.GetRelative(farPos, distance: 50);
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_Sparnanman, 4.1887903f, 150f, 6, 315f, 150f, 50f, 0f);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 6000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Sparnanman_Skill_4)]
	public class Mon_boss_Sparnanman_Skill_4 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(9499);
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
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			var hits = new List<SkillHitInfo>();
			var startingPosition = originPos.GetRelative(farPos, distance: 20f);
			var endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.02f,
				ArrowLifeTime = 0.03f,
				PositionDelay = 1800f,
				HitEffect = new EffectConfig("F_explosion065_green", 2.5f),
				Range = 40f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			startingPosition = originPos.GetRelative(farPos, distance: 20f);
			endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.02f,
				ArrowLifeTime = 0.03f,
				PositionDelay = 1800f,
				HitEffect = new EffectConfig("I_stone015_mash", 0.6f),
				Range = 40f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 0,
				HitDuration = 1000f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 4000f, 1, 10, -1, hits);
		}
	}
}
