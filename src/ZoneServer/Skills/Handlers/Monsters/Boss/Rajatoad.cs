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
	[SkillHandler(SkillId.Mon_boss_Rajatoad_Skill_1)]
	public class Mon_boss_Rajatoad_Skill_1 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2700));
			var spawnPos = originPos.GetRelative(farPos, rand: 100, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "Rajatadpole_summon", spawnPos, 0f, "", "BasicMonster_ATK", -2, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, rand: 100, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "Rajatadpole_summon", spawnPos, 0f, "", "BasicMonster_ATK", -2, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, rand: 100, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "Rajatadpole_summon", spawnPos, 0f, "", "BasicMonster_ATK", -2, 0f, "None", "");
		}
	}

	[SkillHandler(SkillId.Mon_boss_Rajatoad_Skill_2)]
	public class Mon_boss_Rajatoad_Skill_2 : ITargetSkillHandler
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
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 165f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 30f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 1200f,
				HitEffect = new EffectConfig("F_smoke124_blue3", 1f),
				Range = 45f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 60f,
				HitTimeSpacing = 0.02f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Rajatoad_Skill_3)]
	public class Mon_boss_Rajatoad_Skill_3 : ITargetSkillHandler
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
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(1400));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force014_ice#Bone032", 3.5f),
				EndEffect = new EffectConfig("I_explosion006_ice", 3f),
				DotEffect = EffectConfig.None,
				Range = 40f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 700f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 3f),
				GroundDelay = 1000f,
				EffectMoveDelay = 0f,
			};

			var delays = new[] { 1550, 1200, 1250 };
			for (var i = 0; i < 4; i++)
			{
				var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 200);
				await MissilePadThrow(skill, caster, position, config, 0f, "Rajatoad_bubble");

				if (i < delays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Rajatoad_Skill_4)]
	public class Mon_boss_Rajatoad_Skill_4 : ITargetSkillHandler
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
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 130, width: 30, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Rajatoad_Skill_5)]
	public class Mon_boss_Rajatoad_Skill_5 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1400));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force014_ice#Bone032", 2.5f),
				EndEffect = new EffectConfig("I_explosion006_ice", 2f),
				DotEffect = EffectConfig.None,
				Range = 30f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 700f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 2f),
				GroundDelay = 1000f,
				EffectMoveDelay = 0f,
			};

			var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190);
			await MissilePadThrow(skill, caster, position, config, 0f, "Rajatoad_bubble");
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190);
			await MissilePadThrow(skill, caster, position, config, 0f, "Rajatoad_bubble");
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190);
			await MissilePadThrow(skill, caster, position, config, 0f, "Rajatoad_bubble");
			await skill.Wait(TimeSpan.FromMilliseconds(1550));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190);
			await MissilePadThrow(skill, caster, position, config, 0f, "Rajatoad_bubble");
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190);
			await MissilePadThrow(skill, caster, position, config, 0f, "Rajatoad_bubble");
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190);
			await MissilePadThrow(skill, caster, position, config, 0f, "Rajatoad_bubble");
			await skill.Wait(TimeSpan.FromMilliseconds(1200));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190);
			await MissilePadThrow(skill, caster, position, config, 0f, "Rajatoad_bubble");
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190);
			await MissilePadThrow(skill, caster, position, config, 0f, "Rajatoad_bubble");
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190);
			await MissilePadThrow(skill, caster, position, config, 0f, "Rajatoad_bubble");
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190);
			await MissilePadThrow(skill, caster, position, config, 0f, "Rajatoad_bubble");
			await skill.Wait(TimeSpan.FromMilliseconds(1250));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190);
			await MissilePadThrow(skill, caster, position, config, 0f, "Rajatoad_bubble");
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190);
			await MissilePadThrow(skill, caster, position, config, 0f, "Rajatoad_bubble");
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190);
			await MissilePadThrow(skill, caster, position, config, 0f, "Rajatoad_bubble");
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190);
			await MissilePadThrow(skill, caster, position, config, 0f, "Rajatoad_bubble");
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190);
			await MissilePadThrow(skill, caster, position, config, 0f, "Rajatoad_bubble");
		}
	}
}
