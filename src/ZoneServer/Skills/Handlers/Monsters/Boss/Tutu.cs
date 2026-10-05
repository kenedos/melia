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
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_tutu_Skill_1)]
	public class Mon_boss_tutu_Skill_1 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 200f, 5));
			var position = originPos.GetRelative(farPos, distance: 69.565086f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster##0.3", 1.5f),
				PositionDelay = 1800,
				Effect = EffectConfig.None,
				Range = 40f,
				KnockdownPower = 180f,
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

	[SkillHandler(SkillId.Mon_boss_tutu_Skill_2)]
	public class Mon_boss_tutu_Skill_2 : ITargetSkillHandler
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
			caster.StartBuff(BuffId.Mon_Shield, 1f, 0f, TimeSpan.FromMilliseconds(8000f), caster);
		}
	}

	[SkillHandler(SkillId.Mon_boss_tutu_Skill_3)]
	public class Mon_boss_tutu_Skill_3 : ITargetSkillHandler
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
			var hitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 7.5f),
				PositionDelay = 3100,
				Effect = new EffectConfig("F_smoke014", 2f),
				Range = 90f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = originPos.GetRelative(farPos, distance: 75f);
			await EffectAndHit(skill, caster, position, hitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(4000));
			position = originPos.GetRelative(farPos, distance: 75f);
			await EffectAndHit(skill, caster, position, hitConfig);
		}
	}

	[SkillHandler(SkillId.Mon_boss_tutu_Skill_4)]
	public class Mon_boss_tutu_Skill_4 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 22.046225f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss", 7f),
				PositionDelay = 2100,
				Effect = EffectConfig.None,
				Range = 100f,
				KnockdownPower = 180f,
				Delay = 1500f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 60000f, 1, 5, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_tutu_Skill_5)]
	public class Mon_boss_tutu_Skill_5 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 120f);
			var hits = new List<SkillHitInfo>();

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force072_darkgreen#Bip001 HeadNub", 1f),
				EndEffect = new EffectConfig("I_explosion002_green", 1.2f),
				Range = 20f,
				FlyTime = 0.8f,
				DelayTime = 0f,
				Gravity = 1f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster##0.3", 0.5f),
			};

			var firstMissileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force072_darkgreen#Bip001 HeadNub", 1f),
				EndEffect = new EffectConfig("i_explosion002_green_L", 1.2f),
				Range = 20f,
				FlyTime = 0.8f,
				DelayTime = 0f,
				Gravity = 1f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster##0.3", 0.5f),
			};

			var noHashGroundConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force072_darkgreen#Bip001 HeadNub", 1f),
				EndEffect = new EffectConfig("I_explosion002_green", 1.2f),
				Range = 20f,
				FlyTime = 0.8f,
				DelayTime = 0f,
				Gravity = 1f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 0.5f),
			};

			var position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 120, rand: 70);
			await MissileThrow(skill, caster, position, firstMissileConfig, hits);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 120, rand: 70);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = originPos.GetRelative(farPos, distance: 144.88928f);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = originPos.GetRelative(farPos, distance: 145.24872f);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = originPos.GetRelative(farPos, distance: 176.36995f);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = originPos.GetRelative(farPos, distance: 183.81847f);
			await MissileThrow(skill, caster, position, missileConfig, hits);

			await skill.Wait(TimeSpan.FromMilliseconds(3000));

			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 120, rand: 70);
			await MissileThrow(skill, caster, position, noHashGroundConfig, hits);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 120, rand: 70);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 120, rand: 70);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = originPos.GetRelative(farPos, distance: 131.26506f);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = originPos.GetRelative(farPos, distance: 134.75401f);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = originPos.GetRelative(farPos, distance: 179.47977f);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 120, rand: 70);
			await MissileThrow(skill, caster, position, missileConfig, hits);

			await skill.Wait(TimeSpan.FromMilliseconds(3000));

			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 120, rand: 70);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 72.842422, angle: 0f, rand: 150);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 72.842422, angle: 0f, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = originPos.GetRelative(farPos, distance: 163.5905f);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = originPos.GetRelative(farPos, distance: 138.24423f);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = originPos.GetRelative(farPos, distance: 196.79988f);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 72.842422, angle: 0f, rand: 120);
			await MissileThrow(skill, caster, position, missileConfig, hits);

			await skill.Wait(TimeSpan.FromMilliseconds(3000));

			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 72.842422, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 72.842422, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 72.842422, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 72.842422, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 72.842422, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 72.842422, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, missileConfig, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_tutu_Skill_6)]
	public class Mon_boss_tutu_Skill_6 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 180f, 5));
			var hits = new List<SkillHitInfo>();

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force011_green#Bip001 HeadNub", 1.5f),
				EndEffect = new EffectConfig("F_ground116_green", 2.5f),
				Range = 35f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 500f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_boss##0.3", 2.5f),
			};

			var delays = new int[] { 0, 2000, 1500, 1700 };
			for (var i = 0; i < delays.Length; i++)
			{
				if (delays[i] > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));

				var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 1);
				await MissileThrow(skill, caster, position, missileConfig, hits);
			}
		}
	}
}
