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
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_woodhoungan_Skill_1)]
	public class Mon_boss_woodhoungan_Skill_1 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 70, width: 50, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_woodhoungan_Skill_2)]
	public class Mon_boss_woodhoungan_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(3000);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 120);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2800;
			var damageDelay = 3000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1800,
				Effect = new EffectConfig("F_ground133_red", 1.5f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0,
			};

			var position = originPos.GetRelative(farPos, distance: 60, angle: 60f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = originPos.GetRelative(farPos, distance: 60, angle: 125f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = originPos.GetRelative(farPos, distance: 60, angle: -125f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = originPos.GetRelative(farPos, distance: 60, angle: -180f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = originPos.GetRelative(farPos, distance: 60);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = originPos.GetRelative(farPos, distance: 60, angle: -60f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			var effectHitConfig2 = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1800,
				Effect = new EffectConfig("F_ground133_red", 1.5f),
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0,
			};

			position = originPos.GetRelative(farPos, distance: 90, angle: 60f);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			position = originPos.GetRelative(farPos, distance: 90, angle: 125f);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			position = originPos.GetRelative(farPos, distance: 90, angle: -125f);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			position = originPos.GetRelative(farPos, distance: 90, angle: -180f);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			position = originPos.GetRelative(farPos, distance: 90);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			position = originPos.GetRelative(farPos, distance: 90, angle: -60f);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
		}
	}

	[SkillHandler(SkillId.Mon_boss_woodhoungan_Skill_3)]
	public class Mon_boss_woodhoungan_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 150f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_arrow_monster", 1f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.3f,
				PositionDelay = 1000f,
				HitEffect = EffectConfig.None,
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 35f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(900));
			MonsterSkillSetCollisionDamage(caster, skill, true, 1f);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			var position = originPos.GetRelative(farPos, distance: 180);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 70f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 10f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(700));
			MonsterSkillSetCollisionDamage(caster, skill, false, 1f);
		}
	}

	[SkillHandler(SkillId.Mon_boss_woodhoungan_Skill_4)]
	public class Mon_boss_woodhoungan_Skill_4 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 60);
		}
	}

	[SkillHandler(SkillId.Mon_boss_woodhoungan_Skill_5)]
	public class Mon_boss_woodhoungan_Skill_5 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			var position = originPos.GetRelative(farPos);
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.boss_woodhoungan, 0f, 100f, 3, 180f, 50f, 50f, 0f);
		}
	}
}
