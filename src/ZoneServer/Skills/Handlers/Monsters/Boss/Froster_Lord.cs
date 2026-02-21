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
	[SkillHandler(SkillId.Mon_boss_froster_lord_Skill_1)]
	public class Mon_boss_froster_lord_Skill_1 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1200));
			var startingPosition = originPos.GetRelative(farPos, distance: 20f);
			var endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_explosion080_ice", 0.4f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_froster_lord_Skill_2)]
	public class Mon_boss_froster_lord_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1000));

			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force014_ice2#Bip01 R Finger21", 1f),
				EndEffect = new EffectConfig("F_explosion092_ice", 0.5f),
				DotEffect = EffectConfig.None,
				Range = 20f,
				FlyTime = 1f,
				DelayTime = 0.25f,
				Gravity = 450f,
				Speed = 1f,
				HitTime = 0f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var delays = new[] { 50, 50, 50, 0, 50, 0 };
			for (var i = 0; i < 7; i++)
			{
				var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130, height: 1);
				await MissileThrow(skill, caster, position, config);
				if (i < delays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_froster_lord_Skill_3)]
	public class Mon_boss_froster_lord_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1900));

			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 20f,
				HitEffect = new EffectConfig("I_ice005_mash2##0.1", 1f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			for (var i = 0; i < 3; i++)
			{
				var startingPosition = originPos.GetRelative(farPos, distance: 20f);
				var endingPosition = originPos.GetRelative(farPos, distance: 200f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_froster_lord_Skill_4)]
	public class Mon_boss_froster_lord_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(3000));
			var hits = new List<SkillHitInfo>();
			var targetPos = originPos.GetRelative(farPos, distance: 149.90352f, angle: 33f, rand: 100);
			SkillCreatePad(caster, skill, targetPos, 0.58450222f, PadName.boss_nuodai_hail);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			targetPos = originPos.GetRelative(farPos, distance: 136.72461f, angle: -67f, rand: 120);
			SkillCreatePad(caster, skill, targetPos, -1.1848099f, PadName.boss_nuodai_hail);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			targetPos = originPos.GetRelative(farPos, distance: 95.725113f, angle: 133f, rand: 90, height: 2);
			SkillCreatePad(caster, skill, targetPos, 2.3267205f, PadName.boss_nuodai_hail);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			targetPos = originPos.GetRelative(farPos, distance: 163.2444f, angle: -135f, rand: 90, height: 2);
			SkillCreatePad(caster, skill, targetPos, -2.361376f, PadName.boss_nuodai_hail);
			if (!target.IsBuffActive(BuffId.UC_Ignore_Frostbite))
				SkillResultTargetBuff(caster, skill, BuffId.UC_frostbite, 1, 0f, 30000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_froster_lord_Skill_5)]
	public class Mon_boss_froster_lord_Skill_5 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1000);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 100);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 800;
			var damageDelay = 1000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_froster_lord);
		}
	}

	[SkillHandler(SkillId.Mon_boss_froster_lord_Skill_6)]
	public class Mon_boss_froster_lord_Skill_6 : ITargetSkillHandler
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
			caster.StartBuff(BuffId.Invincible, 1f, 0f, TimeSpan.FromMilliseconds(3000f), caster);
			await skill.Wait(TimeSpan.FromMilliseconds(1100));

			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_ground020", 1.5f),
				PositionDelay = 2000,
				Effect = new EffectConfig("F_explosion092_ice", 1f),
				Range = 70f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			var position = originPos.GetRelative(farPos, distance: 175, angle: -135f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 175, angle: 45f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 125, angle: 90f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 125, angle: -90f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = originPos.GetRelative(farPos, distance: 175, angle: 135f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 175, angle: -45f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = originPos.GetRelative(farPos, distance: 130);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 130, angle: 180f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = originPos.GetRelative(farPos, distance: 175, angle: -135f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 130, angle: 180f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 175, angle: 135f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 130, angle: 90f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 175, angle: 45f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 130);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 175, angle: -45f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 130, angle: -90f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 175, angle: -135f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 130, angle: -180f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 175, angle: 135f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 130, angle: 90f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 175, angle: 45f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 130);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 175, angle: 45f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 175, angle: -45f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 175, angle: -135f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 175, angle: 135f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = originPos.GetRelative(farPos, distance: 130, angle: -90f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 130, angle: 180f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 130, angle: 90f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 130);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_ground020", 1.5f),
				PositionDelay = 2000,
				Effect = new EffectConfig("F_explosion092_ice", 1.5f),
				Range = 150f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
		}
	}
}
