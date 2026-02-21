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
	[SkillHandler(SkillId.Mon_boss_Basilisk_Skill_1)]
	public class Mon_boss_Basilisk_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(600);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 70, width: 30, angle: 25f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 400;
			var damageDelay = 600;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			var targetPos = originPos.GetRelative(farPos, distance: 45);
			SkillResultTargetBuff(caster, skill, BuffId.UC_debrave, 1, 0f, 8000f, 1, 15, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Basilisk_Skill_2)]
	public class Mon_boss_Basilisk_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(700);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 40);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 500;
			var damageDelay = 700;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_debrave, 1, 0f, 8000f, 1, 30, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Basilisk_Skill_3)]
	public class Mon_boss_Basilisk_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var hits = new List<SkillHitInfo>();

			var smallHitConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 100,
				Effect = new EffectConfig("I_explosion002_green_L", 1.5f),
				Range = 45f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			var position = originPos.GetRelative(farPos, distance: 40);
			await EffectAndHit(skill, caster, position, smallHitConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(1250));
			position = originPos.GetRelative(farPos, distance: 40);
			await EffectAndHit(skill, caster, position, smallHitConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			position = originPos.GetRelative(farPos, distance: 50);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground116_green", 3.5f),
				Range = 60f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_debrave, 1, 0f, 15000f, 1, 15, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Basilisk_Skill_4)]
	public class Mon_boss_Basilisk_Skill_4 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(2300));

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force027_green", 1f),
				EndEffect = new EffectConfig("I_explosion013_green2", 1f),
				DotEffect = EffectConfig.None,
				Range = 15f,
				DelayTime = 0.1f,
				FlyTime = 0.5f,
				Height = 350f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = EffectConfig.None,
				KnockdownPower = 150f,
				KnockType = (KnockType)3,
				VerticalAngle = 80f,
			};

			var position = GetRelativePosition(PosType.TargetDistance, caster, target);
			await MissileFall(caster, skill, position, missileConfig);
			for (var i = 0; i < 4; i++)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(100));
				position = GetRelativePosition(PosType.TargetDistance, caster, target);
				await MissileFall(caster, skill, position, missileConfig);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup003", 1.5f),
				Range = 100f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 80f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Basilisk_Skill_5)]
	public class Mon_boss_Basilisk_Skill_5 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2300));

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force027_green", 1f),
				EndEffect = new EffectConfig("I_explosion013_green2", 1f),
				DotEffect = EffectConfig.None,
				Range = 15f,
				DelayTime = 0.1f,
				FlyTime = 0.5f,
				Height = 350f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = EffectConfig.None,
				KnockdownPower = 150f,
				KnockType = (KnockType)3,
				VerticalAngle = 80f,
			};

			var burstHitConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup003", 1.5f),
				Range = 100f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 80f,
				InnerRange = 0f,
			};

			var position = GetRelativePosition(PosType.TargetHeight, caster, target);
			await MissileFall(caster, skill, position, missileConfig);
			for (var i = 0; i < 4; i++)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(100));
				position = GetRelativePosition(PosType.TargetHeight, caster, target);
				await MissileFall(caster, skill, position, missileConfig);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 134.51558f, angle: 0f);
			await EffectAndHit(skill, caster, position, burstHitConfig);

			await skill.Wait(TimeSpan.FromMilliseconds(2700));
			position = GetRelativePosition(PosType.TargetHeight, caster, target);
			await MissileFall(caster, skill, position, missileConfig);
			for (var i = 0; i < 4; i++)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(100));
				position = GetRelativePosition(PosType.TargetHeight, caster, target);
				await MissileFall(caster, skill, position, missileConfig);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 181.63824f, angle: -94f);
			await EffectAndHit(skill, caster, position, burstHitConfig);

			await skill.Wait(TimeSpan.FromMilliseconds(2300));
			position = GetRelativePosition(PosType.TargetHeight, caster, target);
			await MissileFall(caster, skill, position, missileConfig);
			for (var i = 0; i < 4; i++)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(100));
				position = GetRelativePosition(PosType.TargetHeight, caster, target);
				await MissileFall(caster, skill, position, missileConfig);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(700));
			position = originPos.GetRelative(farPos, distance: 216.62715f, angle: 138f);
			await EffectAndHit(skill, caster, position, burstHitConfig);
		}
	}
}
